using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;


public class Entity : MonoBehaviour
{
    public Entity() { }

    public Entity(Entity entity)
    {
        var fields = entity.GetType().GetFields();
        foreach (var field in fields)
            field.SetValue(this, field.GetValue(entity));
    }

    public Sprite sprite;
    public string entityName;
    public Team team = Team.Player;

    new public Collider collider;
    public NavMeshAgent agent;
    public Animator animator;
    [SerializeField] public State state;
    protected bool isRun = false;


    //스텟
    [Header("Properties")]
    //레벨
    public int level = 1;
    public int maxLevel = 18;
    [SerializeField] float _experience = 0;
    public float experience
    {
        get { return _experience; }
        set
        {
            if (value <= _experience)
            {
                _experience = Math.Max(value, 0);
            }
            else
            {
                _experience = value;

                while (_experience >= experienceMax && level < maxLevel)
                {
                    _experience -= experienceMax;
                    ++level;
                    UpdateSpecialConditionSkill(SkillCondition.LevelUp);
                    if(this is Player) {
                        Player player = (Player)this;
                        if (SettingManager.self)
                            player.audioSource.volume = SettingManager.self.CalcFxValue();
                        player.audioSource.clip = player.audioClip_LevelUp;
                        player.audioSource.Play();
                    }
                }
                if (_experience > experienceMax) _experience = experienceMax;
            }
        }
    }

    //경험치
    [SerializeField] protected LevelBasedMultiplierFloat _experienceMax = new LevelBasedMultiplierFloat { baseValue = 10, bonusPerLevel = 1.5f };
    public float experienceMax { get { return _experienceMax.Get(level); } }

    //최대 체력
    [SerializeField] protected LevelBasedInt _healthMax = new LevelBasedInt { baseValue = 100 };
    public virtual int healthMax
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsHealthMax);
            return _healthMax.Get(level) + buffbonus;
        }
    }
    //체력 재생
    [SerializeField] protected LevelBasedFloat _healthRegeneration = new LevelBasedFloat { baseValue = 1 };
    public virtual float healthRegeneration
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsHealthRegeneration);
            var buffMaxPerbonus = healthMax * buffs.Sum(buff => buff.buffsHealthMaxPerRegeneration);
            return _healthRegeneration.Get(level) + buffbonus + buffMaxPerbonus;
        }
    }
    //현재 체력
    [SerializeField] float _health = 1f;
    public float health
    {
        get { return Mathf.Min(_health * healthMax, healthMax); }
        set { _health = Mathf.Clamp(value / healthMax, 0f, 1f); }
    }

    //최대 마나
    [SerializeField] protected LevelBasedInt _manaMax = new LevelBasedInt { baseValue = 100 };
    public virtual float manaMax
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsManaMax);
            return _manaMax.Get(level) + buffbonus;
        }
    }
    //마나 재생
    [SerializeField] protected LevelBasedFloat _manaRegeneration = new LevelBasedFloat { baseValue = 1 };
    public virtual float manaRegeneration
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsManaRegeneration);
            var buffMaxPerbonus = manaMax * buffs.Sum(buff => buff.buffsManaMaxPerRegeneration);
            return _manaRegeneration.Get(level) + buffbonus + buffMaxPerbonus;
        }
    }
    //현재 마나
    [SerializeField] float _mana = 1f;
    public float mana
    {
        get { return Mathf.Min(_mana * manaMax, manaMax); }
        set { _mana = Mathf.Clamp(value / manaMax, 0f, 1f); }
    }
    // 스킬 사용 마나 소모량 감소(1 => 100%)
    [SerializeField] protected LevelBasedFloat _manaConsumptions = new LevelBasedFloat { baseValue = 0 };

    public virtual float manaConsumptions
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsManaPerConsumption);
            var manaConsumptionsValue = _manaConsumptions.Get(level) + buffbonus >= 0.5f ? .5f : _manaConsumptions.Get(level) + buffbonus;
            return manaConsumptionsValue;
        }
    }

    //공격력
    [SerializeField] protected LevelBasedInt _attackDamage = new LevelBasedInt { baseValue = 1 };
    public virtual int attackDamage
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsAttackDamage);
            return _attackDamage.Get(level) + buffbonus;
        }
    }

    //주문력
    [SerializeField] protected LevelBasedInt _abilityPower = new LevelBasedInt { baseValue = 0 };
    public virtual int abilityPower
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsAbilityPower);
            return _abilityPower.Get(level) + buffbonus;
        }
    }

    //방어력
    [SerializeField] protected LevelBasedInt _armor = new LevelBasedInt { baseValue = 1 };
    public virtual int armor
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsArmor);
            return _armor.Get(level) + buffbonus;
        }
    }

    //마법저항
    [SerializeField] protected LevelBasedInt _magicResist = new LevelBasedInt { baseValue = 1 };
    public virtual int magicResist
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsMagicResist);
            return _magicResist.Get(level) + buffbonus;
        }
    }

    //크리티컬 확률(1 => 100%)
    [SerializeField] protected LevelBasedFloat _criticalChance = new LevelBasedFloat { baseValue = 0 };
    public virtual float criticalChance
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsCriticalChance);
            return _criticalChance.Get(level) + buffbonus;
        }
    }

    //크리티컬 데미지(배율 1 => 100%)
    [SerializeField] protected LevelBasedFloat _criticalDamage = new LevelBasedFloat { baseValue = 2 };
    public virtual float criticalDamage
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsCriticalDamage);
            return _criticalDamage.Get(level) + buffbonus;
        }
    }

    //이동속도
    [SerializeField] protected LevelBasedFloat _moveSpeed = new LevelBasedFloat { baseValue = 10 };
    public virtual float moveSpeed
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsMoveSpeed);
            return (_moveSpeed.Get(level) + buffbonus) <= 0 ? 1 : (_moveSpeed.Get(level) + buffbonus);
        }
    }

    //공격속도(배율 1 => 초당 1대)
    [SerializeField] protected LevelBasedFloat _attackSpeed = new LevelBasedFloat { baseValue = 1 };
    public virtual float attackSpeed
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsAttackSpeed);
            return _attackSpeed.Get(level) * (1f + buffbonus);
        }
    }

    //쿨감(배율 1 => 100%)
    [SerializeField] protected LevelBasedFloat _cooldown = new LevelBasedFloat { baseValue = 0 };
    public virtual float cooldown
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsCooldown);
            return _cooldown.Get(level) + buffbonus;
        }
    }

    //피흡(배율 1 => 100%)
    [SerializeField] protected LevelBasedFloat _absorption = new LevelBasedFloat { baseValue = 0 };
    public virtual float absorption
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsAbsorption);
            return _absorption.Get(level) + buffbonus;
        }
    }

    //쉴드
    float _shield = 0;
    public float shield
    {
        get 
        {
            var buffbonus = buffs.Sum(buff => buff.buffsShield);
            return _shield + buffbonus;
        }
        set
        {
            float s = shield - value;
            for(int i =0;i<buffs.Count;i++) {
                if(buffs[i].buffsShield > 0) {
                    var buff = buffs[i];
                    if (s >= buff.buffsShield) {
                        s -= buff.buffsShield;
                        buff.buffsShield = 0;                        
                    }
                    else {
                        buff.buffsShield -= s;
                        s = 0;
                    }
                    buffs[i] = buff;
                }
            }
        }
    }
    // 사거리 증가
    [SerializeField] protected LevelBasedFloat _addCastRange = new LevelBasedFloat { baseValue = 0 };
    public virtual float addCastRange
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsCastRange);
            return _addCastRange.Get(level) + buffbonus;
        }
    }

    [Header("Respawn")]
    //부활
    [SerializeField] protected int _revive = 0;
    public int revive
    {
        get
        {
            var buffbonus = buffs.Sum(buff => buff.buffsReviveCount);
            return _revive + buffbonus;
        }
    }
    public float deathTime = 5f;
    double deathTimeEnd;
    public float respawnTime = 10;
    double respawnTimeEnd;

    [Header("Target")]
    public Entity target;
    public Vector3 targetPos;
    Vector3 lookPosition;
    Quaternion lookRotationOrigin;
    float lookAtSpeed;
    float lookAtTimer;

    [Header("Skills")]
    public SkillTemplate[] skillTemplates;
    public List<Skill> skills = new List<Skill>();
    public List<Buff> buffs = new List<Buff>();
    [SerializeField] public int currentSkill = -1;
    [SerializeField] protected int nextSkill = -1;
    [SerializeField] public int normalAttackIndex = 0;

    [Header("EffectMount")]
    public Transform leftHandMount;
    public Transform rightHandMount;
    public Transform weaponMount1;
    public Transform weaponMount2;
    public Transform weaponMount3;
    public Transform headMount;
    public Transform centerMount;
    public Transform bottomMount;
    public Transform moveMount; //이동스킬 사용시 사용
    public Transform leftFootMount;
    public Transform rightFootMount;

    [Header("UI")]
    public GameObject ui_StatusBar;
    public float ui_StatusBar_Gap = 1;
    UI_EntityStatusBar statusBar;
    CastingRange castingRange;

    public static Dictionary<Team, HashSet<Entity>> teams = new Dictionary<Team, HashSet<Entity>>() {
        {Team.Player, new HashSet<Entity>()},
        {Team.Enemy, new HashSet<Entity>()},
        {Team.Neutral, new HashSet<Entity>()}
    };

    protected float isBattle = 0f;

    public bool IsMoving()
    {
        if (!agent.enabled) return false;
        return agent.pathPending ||
               agent.remainingDistance > agent.stoppingDistance ||
               agent.velocity != Vector3.zero;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        teams[team].Add(this);

        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = moveSpeed;
        }
        if (!animator) animator = GetComponent<Animator>();
        if (!collider) collider = GetComponent<Collider>();

        state = State.Idle;

        foreach (var t in skillTemplates)
            skills.Add(new Skill(t));

        _health = 1f;
        _mana = 1f;
        isBattle = 0f;

        if(ui_StatusBar)
        {
            GameObject _statusBar = Instantiate(ui_StatusBar);
            _statusBar.transform.SetParent(gameObject.transform);
            _statusBar.transform.SetAsFirstSibling();
            _statusBar.name = "UI_StatusBar_" + transform.name;
            statusBar = _statusBar.GetComponent<UI_EntityStatusBar>();
            statusBar.player = this;
            statusBar.gap = ui_StatusBar_Gap;
        }
        castingRange = GetComponentInChildren<CastingRange>();

        //if (Preloader.self) Preloader.self.PreloadAsset_Entity(this);

        //NavMesh.pathfindingIterationsPerFrame = 2;
    }
    void OnDestroy()
    {
        teams[team].Remove(this);
    }

    protected virtual void Update()
    {
        UpdateState();
        UpdateRepeatSkills();
        UpdateSpecialConditionSkill();
        CleanupBuffs();
        if (agent.path.corners.Length > 1)
        {
            Vector3 lookrotation = agent.steeringTarget - transform.position;
            if (lookrotation != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), agent.angularSpeed * Time.deltaTime);
        }
        Debug.DrawLine(transform.position, agent.nextPosition, Color.yellow);
        //Debug.Log(experienceMax);
    }
    protected bool EventDied()
    {
        return health == 0;
    }
    protected bool EventReviveTimeEnd()
    {
        return state == State.Dead && Time.time >= respawnTimeEnd;
    }
    protected bool EventTargetDied()
    {
        return target != null && target.health == 0;
    }
    protected bool EventMoveEnd()
    {
        return state == State.Move && !IsMoving();
    }
    protected bool EventMoveSkill()
    {
        return state == State.Casting && IsCurrentMoveSKill();
    }
    protected bool IsCurrentMoveSKill()
    {
        return currentSkill >= 0 && currentSkill < skills.Count && skills[currentSkill].template is MoveSkillTemplate;
    }
    protected bool EventStunRequest()
    {
        int buffsStun = buffs.Where(buff => buff.buffsStun == true).Count();
        return (buffsStun > 0);
    }
    protected bool EventStunEnd()
    {
        int buffsStun = buffs.Where(buff => buff.buffsStun == true).Count();
        return (buffsStun == 0);
    }
    protected bool EventSilenceRequest()
    {
        int buffsStun = buffs.Where(buff => buff.buffsSilence == true).Count();
        return (buffsStun > 0);
    }
    protected bool EventSilenceEnd()
    {
        int buffsStun = buffs.Where(buff => buff.buffsSilence == true).Count();
        return (buffsStun == 0);
    }
    protected bool IsStunState()
    {
        return state == State.Stun;
    }
    public virtual void UpdateState()
    {
        if (state == State.Idle) UpdateState_Idle();
        else if (state == State.Move) UpdateState_Move();
        else if (state == State.Rotate) UpdateState_Rotate();
        else if (state == State.Casting) UpdateState_Casting();
        else if (state == State.Silence) UpdateState_Silence();
        else if (state == State.Stun) UpdateState_Stun();
        else if (state == State.Dead) UpdateState_Dead();
        if (state != State.Dead && health > 0) {
            RegenerationHealth((healthRegeneration * Time.deltaTime));
            RegenerationMana((manaRegeneration * Time.deltaTime));
        }
        agent.speed = moveSpeed;
        animator.SetFloat("NormalAttackSpeed", attackSpeed);
        animator.SetFloat("MoveAnimMultiplier", moveSpeed / _moveSpeed.baseValue);
    }

    public virtual void UpdateState_Idle()
    {
        if (EventDied())
        {
            OnDeath();
            currentSkill = nextSkill = -1;
            state = State.Dead;
            return;
        }
        if (EventStunRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Stun;
            return;
        }
        if (EventSilenceRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Silence;
            return;
        }
    }
    public virtual void UpdateState_Move()
    {
        if (EventDied())
        {
            OnDeath();
            currentSkill = nextSkill = -1;
            state = State.Dead;
            return;
        }
        if (EventStunRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Stun;
            return;
        }
        if (EventSilenceRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Silence;
            return;
        }
        else
        {
            if (agent.path.corners.Length == 2)
                agent.velocity = (agent.destination - transform.position).normalized * moveSpeed;
        }

    }

    public virtual void UpdateState_Rotate()
    {
        if (EventDied())
        {
            OnDeath();
            currentSkill = nextSkill = -1;
            state = State.Dead;
            return;
        }
        if (EventStunRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Stun;
            return;
        }
        if (EventSilenceRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Silence;
            return;
        }
        Debug.DrawLine(transform.position, transform.position + (transform.forward * 5f), Color.green);
        lookAtTimer -= (Time.deltaTime * lookAtSpeed);
        if (lookAtTimer < 0) {
            LookAtY(lookPosition);
            state = State.Idle;
            return;
        }
        Vector3 relativePos = lookPosition - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(toRotation, lookRotationOrigin, lookAtTimer);
    }

    public virtual void UpdateState_Casting()
    {
        if (EventDied())
        {
            OnDeath();
            currentSkill = nextSkill = -1;
            state = State.Dead;
            return;
        }
        if (EventStunRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Stun;
            return;
        }
        if (EventSilenceRequest())
        {
            currentSkill = nextSkill = -1;
            StopAction();
            state = State.Silence;
            return;
        }
    }
    public virtual void UpdateState_Stun()
    {
        if (EventStunEnd())
        {
            state = State.Idle;
        }
        if (EventDied())
        {
            OnDeath();
            currentSkill = nextSkill = -1;
            state = State.Dead;
        }
    }
    public virtual void UpdateState_Silence()
    {
        if (EventSilenceEnd())
        {
            state = State.Idle;
        }
        if (EventDied())
        {
            OnDeath();
            currentSkill = nextSkill = -1;
            state = State.Dead;
        }
    }
    public virtual void UpdateState_Dead()
    {
        for (int i = 0; i < buffs.Count; i++)
        {

            if (buffs[i].effect)
                Destroy(buffs[i].effect.gameObject);
            if (buffs[i].buffsReviveCount <= 0)
            {
                // 부활이 포함된 이펙트 외 전부 제거
                buffs.RemoveAt(i);
                i--;
            }
            else
            {
                //After Effect
            }
        }

        if (EventReviveTimeEnd())
        {
            if (revive > 0)
                Revive();
            else
                Remove();
        }
    }
    public virtual void OnDeath()
    {
        agent.ResetPath();
        target = null;

        deathTimeEnd = Time.time + deathTime;
        if (revive > 0)
            respawnTimeEnd = deathTimeEnd + respawnTime;
        else
        {
            respawnTimeEnd = deathTimeEnd + 0f;
            buffs.Clear();
        }

        collider.enabled = false;

        UpdateSpecialConditionSkill(SkillCondition.Death);
    }
    public virtual void Revive(float healthPercent = 1f)
    {
        
        if(_revive > 0)
            _revive--;
        buffs.Clear();
        
        collider.enabled = true;
        health = healthMax * healthPercent;
        state = State.Idle;
    }

    public virtual void Remove()
    {
        Destroy(gameObject);
    }

    public virtual bool CanAttack(Entity _target)
    {
        return true;
    }

    public virtual void DealDamageAt(Entity _target, float attackDamage, float abilityPower, float trueDamage, bool useCrit, string skillName, bool isOnce)
    {
        bool isCrit = UnityEngine.Random.Range(1, 100) <= (criticalChance * 100);
        float totalDamage = _target.BeDamagedBy(this,
                                              isCrit ? (attackDamage * criticalDamage) : attackDamage,
                                              isCrit ? (abilityPower * criticalDamage) : abilityPower,
                                              trueDamage,
                                              isCrit, skillName, isOnce);
        if (isCrit)
        {
            //Debug.Log("크리티컬 발동");
            UpdateSpecialConditionSkill(SkillCondition.Critical, this);
        }
        if (totalDamage > 0) {
            UpdateSpecialConditionSkill(SkillCondition.Attack, this);
            isBattle = 10f;
        }
        RegenerationHealth(totalDamage * absorption);   
        if(_target.health <= 0 && _target.revive <= 0)
            UpdateSpecialConditionSkill(SkillCondition.KillEnemy, _target);
    }

    public virtual float BeDamagedBy(Entity _attacker, float attackDamage, float abilityPower, float trueDamage, bool isCrit, string skillName, bool isOnce)
    {
        float damageDealt;
        int interval = 2;
        float totalDamage = 0;
        //물뎀
        float _armorResist = armor / (armor + 100);
        damageDealt = Mathf.Max(0f, (attackDamage - (attackDamage * _armorResist)));

        totalDamage += damageDealt;
        health -= DealDamageShield(damageDealt);
        
        if (damageDealt > 0)
            DamagePopupManager.self.ShowDamagePopup(isCrit ? PopupType.Critical : PopupType.Normal, DamageType.Physical, damageDealt, this, interval--, skillName, isOnce);
        
        //마뎀
        float _magicResist = magicResist / (magicResist + 100);
        damageDealt = Mathf.Max(0, (abilityPower - (abilityPower * _magicResist)));
        totalDamage += damageDealt;
        health -= DealDamageShield(damageDealt);
        
        if (damageDealt > 0)
            DamagePopupManager.self.ShowDamagePopup(isCrit ? PopupType.Critical : PopupType.Normal, DamageType.Magic, damageDealt, this, interval--, skillName, isOnce);

        //고뎀
        damageDealt = Mathf.Max(trueDamage, 0);
        totalDamage += damageDealt;
        health -= DealDamageShield(damageDealt);
        if (damageDealt > 0)
            DamagePopupManager.self.ShowDamagePopup(PopupType.Normal, DamageType.True, damageDealt, this, interval--, skillName, isOnce);

        if (totalDamage > 0) {
            UpdateSpecialConditionSkill(SkillCondition.ReceiveDeal, this);
            isBattle = 10f;
        }
        return totalDamage;
    }

    public virtual float DealDamageShield(float damageDealt)
    {
        if (shield >= damageDealt) {
            shield -= damageDealt;
            return 0;
        }
        float gap = damageDealt - shield;
        shield -= shield;
        return gap;
    }

    public virtual void RegenerationHealth(float regen)
    {
        health += regen;
    }

    public virtual void RegenerationMana(float regen)
    {
        mana += regen;
    }

    public virtual void MoveTo(Vector3 destination, float stopDistance = 0)
    {
        if (!agent.enabled) return;
        if (state == State.Dead || state == State.Stun) return;
        agent.destination = destination;
        //agent.SetDestination(destination);
        //agent.SetDestinationWithCalculatePath(destination);
        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.stoppingDistance = stopDistance;
        currentSkill = -1;
        state = State.Move;
    }

    public virtual void TeleportTo(Vector3 destination, float stopDistance = 0)
    {
        if (state == State.Dead || state == State.Stun) return;

        transform.position = destination;
        agent.Warp(destination);

        currentSkill = -1;
        state = State.Move;
    }

    public virtual void OnAggroEnter(Entity entity) { }
    public virtual void OnAggroStay(Entity entity) { }

    public virtual void StopAction()
    {
        if (currentSkill >= 0)
        {
            var skill = skills[currentSkill];
            skill.castTimeEnd = Time.time - 1f;
            skill.invokeTimeEnd = Time.time - 1f;
            skill.repeatEnd = Time.time - 1f;
            skills[currentSkill] = skill;
            currentSkill = -1;
        }
        state = State.Idle;
        agent.SetDestination(transform.position);        
    }

    public void LookAtY(Vector3 position)
    {
        lookPosition = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookPosition);
    }
    public void LookAtY(Vector3 position, float speed)
    {
        lookPosition = new Vector3(position.x, transform.position.y, position.z);
        lookAtSpeed = speed;
        lookAtTimer = 1f;
        lookRotationOrigin = transform.rotation;
        state = State.Rotate;
    }

    public virtual bool CastCheckSelf(Skill skill, bool setMessage = true)
    {
        //쿨돌앗나, 살아있나, 마나 남았나
        var manaCosts = (skill.onOff && skill.isOn ? 0 : skill.manaCosts);

        manaCosts -= (manaCosts * manaConsumptions);

        if (!skill.IsReady()) return false;
        if (health <= 0) return false;
        if (mana < manaCosts) return false;
        return skill.learned;
    }

    public bool RepeatCheckSelf(Skill skill)
    {
        var repatManaCosts = skill.repeatManaCosts;
        repatManaCosts -= (repatManaCosts * manaConsumptions);

        //쿨돌앗나, 살아있나, 마나 남았나
        return (skill.IsRepeatReady()) &&
               health > 0 &&
               mana >= repatManaCosts;
    }

    public bool CastCheckTarget(Skill skill)
    {
        //스킬쓸수있는 타겟인가
        return skill.CheckTarget(this);
    }

    public bool CastCheckDistance(Skill skill, out Vector3 destination)
    {
        //스킬쓸수있는거리인가
        return skill.CheckDistance(this, out destination);
    }

    public virtual void CastRepeatSkill(Skill skill, int skillIndex)
    {
        var repatManaCosts = skill.repeatManaCosts;
        repatManaCosts -= (repatManaCosts * manaConsumptions);

        if (RepeatCheckSelf(skill))
        {
            skill.RepeatApply(this);
            mana -= repatManaCosts;                
            skill.repeatEnd = Time.time + skill.repeatTime;
            skills[skillIndex] = skill;
        }
        else if(mana < repatManaCosts)
        {
            if (skill.onOff)
                skill.isOn = false;            
            if(skill.isNormalAttack)
                skill.cooldownEnd = Time.time + (skill.cooldown / attackSpeed);
            else
                skill.cooldownEnd = Time.time + (skill.cooldown * (1f - cooldown));
            skill.repeatEnd = Time.time + skill.repeatTime;
            skill.invoked = false;
            skills[skillIndex] = skill;
        }
    }

    public virtual void CastSkill(Skill skill)
    {
        skill.invoked = false;
        skills[currentSkill] = skill;
        if (skill.visualEffect_End != null)
        {
            GameObject go = Instantiate(skill.visualEffect_End.gameObject, GetEffectMount(skill.effectMount).position, GetEffectMount(skill.effectMount).rotation);
            var _effect = go.GetComponent<VisualSkillEffect>();
            if (_effect.followEffectMount) go.transform.parent = GetEffectMount(skill.effectMount);
            if (skill.effectMount == EffectMount.BothHands) {
                GameObject goRight = Instantiate(skill.visualEffect_End.gameObject, GetEffectMount(EffectMount.RightHand).position, GetEffectMount(EffectMount.RightHand).rotation);
                var _effectRight = goRight.GetComponent<VisualSkillEffect>();
                if (_effectRight.followEffectMount) goRight.transform.parent = GetEffectMount(EffectMount.RightHand);
            }
        }
        else
            Debug.LogWarning("visualEffect_Invoke is Null");
    }

    public void InvokeSkill(Skill skill)
    {
        if (CastCheckSelf(skill) && CastCheckTarget(skill))
        {
            var manaCosts = skill.manaCosts;
            manaCosts -= (manaCosts * manaConsumptions);

            if (skill.onOff) {
                if(!skill.isOn) //OnOff스킬일때는 스킬On할때만 마나소모
                    mana -= manaCosts;
                skill.isOn = !skill.isOn;
            }
            else
                mana -= manaCosts;

            skill.Apply(this);
            if (skill.isNormalAttack)
                skill.cooldownEnd = Time.time + (skill.cooldown / attackSpeed);
            else
                skill.cooldownEnd = Time.time + (skill.cooldown * (1f - cooldown));
            skill.repeatEnd = Time.time + skill.repeatTime;
            skill.invoked = true;
            if (currentSkill >= skills.Count) {
                if(this is Player) {
                    var player = (Player)this;
                    var item = player.inventory[currentSkill - skills.Count];
                    item.skill = skill;
                    player.inventory[currentSkill - skills.Count] = item;
                }
            }    
            else
                skills[currentSkill] = skill;
        }
        else
        {
            currentSkill = -1;
        }
    }

    public void AddOrRefreshBuff(Buff buff)
    {
        int index = buffs.FindIndex(b => b.name == buff.name);
        if (index != -1) buffs[index] += buff;
        else buffs.Add(buff);
    }

    public virtual void UpdateRepeatSkills()
    {
        for (int i = 0; i < skills.Count;i++)
        {
            Skill skill = skills[i];
            if (skills[i].onOff) {
                if(skills[i].isOn)
                    CastRepeatSkill(skill, i);
            }
            else if(skills[i].template is PassiveSkillTemplate &&
                    !skills[i].isSpecialCondition) {
                CastRepeatSkill(skill, i);
            }
        }        
    }

    public virtual void UpdateSpecialConditionSkill()
    {
        if(isBattle > 0f)
        {
            isBattle -= Time.deltaTime;
            UpdateSpecialConditionSkill(SkillCondition.Battle, null);
        }
    }

    public virtual void UpdateSpecialConditionSkill(SkillCondition condition, Entity _target = null)
    {
        
        for (int i = 0; i < skills.Count; i++) 
        {
            if (skills[i].isSpecialCondition && skills[i].skillCondition == condition 
                && skills[i].skillConditionType == SkillConditionType.Self)
            {
                CastSpecialConditionSkill(skills[i], i, _target, true);
            }
            else if (skills[i].isSpecialCondition && skills[i].skillCondition == condition 
                && skills[i].skillConditionType == SkillConditionType.ConditionSkill)
            {
                if(skills[i].template.buffSkillTemplateConditionSkill)
                    CastSpecialConditionSkill(skills[i], i, _target, false); 
            }
        }
    }

    public void CastSpecialConditionSkill(Skill skill, int skillIndex, Entity _target, bool skillSelf)
    {
        Skill targetSkill = skillSelf ? skill : new Skill(skill.template.buffSkillTemplateConditionSkill);

        if (!CastCheckSelf(targetSkill, false)) return;

        var targets = new List<Entity>();

        var skillCastRange = targetSkill.addUseCastRange ? targetSkill.castRange + addCastRange : targetSkill.castRange;

        if (_target) targets.Add(_target);  //attacker, receiver

        if (skill.skillTarget == SkillTarget.Self) targets.Add(this);
        else if (skill.skillTarget == SkillTarget.NearEnemy) {
            var entities = GetEntitiesInRange(skillCastRange, Team.Enemy);
            if (entities.Length > 0) targets.Add(entities[0]);
        }
        else if (skill.skillTarget == SkillTarget.AllEnemy) {
            var entities = GetEntitiesInRange(skillCastRange, Team.Enemy);
            if (entities.Length > 0)
                foreach (var entity in entities)
                    targets.Add(entity);
        }
        else if (skill.skillTarget == SkillTarget.NearTeam) {
            var entities = GetEntitiesInRange(skillCastRange, Team.Player);
            if (entities.Length > 0) targets.Add(entities[0]);
        }
        else if (skill.skillTarget == SkillTarget.AllTeam) {
            var entities = GetEntitiesInRange(skillCastRange, Team.Player);
            if (entities.Length > 0)
                foreach (var entity in entities)
                    targets.Add(entity);
        }
        else if (skill.skillTarget == SkillTarget.NearAll) {
            var entities = GetEntitiesInRange(skillCastRange, Team.All);
            if (entities.Length > 0) targets.Add(entities[0]);
        }
        else if (skill.skillTarget == SkillTarget.All) {
            var entities = GetEntitiesInRange(skillCastRange, Team.All);
            if (entities.Length > 0)
                foreach (var entity in entities)
                    targets.Add(entity);
        }
        if (skillSelf)
        {
            if (CastSkillAtOnceOnTarget(targetSkill, skillIndex, targets.ToArray()))
                Debug.Log("Cast Special Condition Skill(" + targetSkill.skillCondition.ToString() + ") " + targetSkill.name);
        }
        else
        {
            for (int i = 0; i < targets.Count; i++)
            {
                //Debug.Log("CondiTion Target : " + targets[i].name);
                targets[i].AddOrRefreshBuff(new Buff(skill.template.buffSkillTemplateConditionSkill, targetSkill.level, _target));
            }
        }
    }

    public virtual bool CastSkillAtOnceOnTarget(Skill skill, int index, Entity[] targets)
    {

        if (targets.Length <= 0 || !CastCheckSelf(skill, false)) return false;
        foreach (var _target in targets)
        {
            target = _target;
            skill.Apply(this);
        }
        var manaCosts = skill.manaCosts;
        manaCosts -= (manaCosts * manaConsumptions);

        if (skill.onOff) {
            skill.isOn = !skill.isOn;
            if (skill.isOn) //OnOff스킬일때는 스킬On할때만 마나소모
                mana -= manaCosts;
        }
        else
            mana -= manaCosts;
        skill.cooldownEnd = Time.time + (skill.cooldown * (1f - cooldown));
        skill.repeatEnd = Time.time + skill.repeatTime;
        skills[index] = skill;
        return true;
    }
    void CleanOnlyDeBuffs()
    {
        for (int i = 0; i < buffs.Count; ++i)
        {
            // 디버프를 골라내야함(Caster : 본인과 같지 않은 Entity, Target : )
            Debug.Log("Clean De Buff Caster Team : " + buffs[i].caster.team + ", this Team : " + team);
            if (buffs[i].caster.team != team)
            {
                buffs.RemoveAt(i);
                --i;
            }
        }
    }
    void CleanupBuffs()
    {
        for (int i = 0; i < buffs.Count; ++i)
        {
            bool DotDamageCool = false;
            if (buffs[i].BuffDotDamageTimeUpdate()){
                DotDamageCool = true;
            }

            if (buffs[i].BuffTimeRemaining() == 0)
            {
                if (buffs[i].decreaseBuffStack && buffs[i].buffStack > 1)
                {
                    var buff = buffs[i];
                    buff.buffStack--;
                    buff.buffTimeEnd = Time.time + buff.buffTime;
                    float shield = (buff.template.GetBuffsShield(buff.caster, buff.level) * buff.template.buffsStackValue.Get(buff.buffStack));
                    buff.buffsShield = Math.Min(shield, buff.buffsShield);
                    buffs[i] = buff;
                }
                else
                {
                    buffs.RemoveAt(i);
                    --i;
                }
            }
            else
            {
                if (DotDamageCool)
                {
                    // isOnce -> false에서 True로 처리
                    float dotDamage = buffs[i].buffsDamaged;
                    if (buffs[i].dotDamageType == DamageType.Physical)
                        buffs[i].caster.DealDamageAt(this, dotDamage, 0, 0, false, buffs[i].name, true);
                    else if (buffs[i].dotDamageType == DamageType.Magic)
                        buffs[i].caster.DealDamageAt(this, 0, dotDamage, 0, false, buffs[i].name, true);
                    else if (buffs[i].dotDamageType == DamageType.True)
                        buffs[i].caster.DealDamageAt(this, 0, 0, dotDamage, false, buffs[i].name, true);
                }
            }
        }
    }

    public void SetEntityStatusUI(bool active)
    {
        if (!statusBar) statusBar = GetComponentInChildren<UI_EntityStatusBar>();
        if (statusBar) statusBar.gameObject.SetActive(active);
    }

    public void SetAttackRangeUI(bool active)
    {
        if(!castingRange) castingRange = GetComponentInChildren<CastingRange>();
        if (castingRange) castingRange.gameObject.SetActive(active);
    }

    public Entity[] GetEntitiesInRange(float range, Team _team)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        List<Entity> entities = new List<Entity>();
        foreach (Collider co in colliders)
        {
            Entity entity = co.GetComponentInParent<Entity>();
            if (entity && entity != this && entity.health > 0) {
                if (_team == Team.All || entity.team == _team) {
                    entities.Add(entity);
                }
            }
        }
        entities.Sort(delegate (Entity a, Entity b)
        {
            if (Vector3.Distance(a.transform.position, transform.position) >
               Vector3.Distance(b.transform.position, transform.position))
                return 1;
            if (Vector3.Distance(a.transform.position, transform.position) <
               Vector3.Distance(b.transform.position, transform.position))
                return -1;
            return 0;
        });
        return entities.ToArray();
    }

    public Entity[] GetEntitiesInRangeFromPosition(Vector3 position, float range, Team _team)
    {
        Collider[] colliders = Physics.OverlapSphere(position, range);
        List<Entity> entities = new List<Entity>();
        foreach (Collider co in colliders)
        {
            Entity entity = co.GetComponentInParent<Entity>();
            if (entity && entity != this && entity.health > 0)
            {
                if (_team == Team.All || entity.team == _team)
                {
                    entities.Add(entity);
                }
            }
        }
        entities.Sort(delegate (Entity a, Entity b)
        {
            if (Vector3.Distance(a.transform.position, position) >
               Vector3.Distance(b.transform.position, position))
                return 1;
            if (Vector3.Distance(a.transform.position, position) <
               Vector3.Distance(b.transform.position, position))
                return -1;
            return 0;
        });
        return entities.ToArray();
    }

    public Transform GetEffectMount(EffectMount mount)
    {
        if (mount == EffectMount.LeftHand) return leftHandMount;
        else if (mount == EffectMount.RightHand) return rightHandMount;
        else if (mount == EffectMount.Head) return headMount;
        else if (mount == EffectMount.Center) return centerMount;
        else if (mount == EffectMount.Bottom) return bottomMount;
        else if (mount == EffectMount.Weapon1) return weaponMount1;
        else if (mount == EffectMount.Weapon2) return (weaponMount2 ? weaponMount2 : weaponMount1);
        else if (mount == EffectMount.Weapon3) return (weaponMount3 ? weaponMount3 : weaponMount1);
        else if (mount == EffectMount.LeftFoot) return leftFootMount;
        else if (mount == EffectMount.RightFoot) return rightFootMount;
        else return leftHandMount;
    }
    public Vector3 GetEffectMountPositionWhenSkillInvokeTime(EffectMount mount, int skillIndex)
    {
        float currentNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        int nameHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        var skill = skills[skillIndex];
        
        bool castingBool = animator.GetBool("Casting");
        bool skillBool = animator.GetBool(skill.name);
        
        animator.SetBool("Casting", true);
        animator.SetBool(skill.name, true);


        float normalizedTime = skill.invokeTime / skill.castTime;
        animator.Play(skill.name, 0, normalizedTime);
        animator.Update(0f);

        var result = GetEffectMount(mount).position;

        animator.SetBool("Casting", castingBool);
        animator.SetBool(skill.name, skillBool);
        animator.Play(nameHash, 0, currentNormalizedTime);
        animator.Update(Time.deltaTime);

        return result;
    }

    public void LevelUp()
    {
        experience += experienceMax;
    }
    public float healthMaxOrigin(int _level) { return _healthMax.Get(_level); }
    public float healthRegenerationOrigin(int _level) { return _healthRegeneration.Get(_level); }
    public float manaMaxOrigin(int _level) { return _manaMax.Get(_level); }
    public float manaRegenerationOrigin(int _level) { return _manaRegeneration.Get(_level); }
    public float attackDamageOrigin(int _level) { return _attackDamage.Get(_level); }
    public float abilityPowerOrigin(int _level) { return _abilityPower.Get(_level); }
    public float armorOrigin(int _level) { return _armor.Get(_level); }
    public float magicResistOrigin(int _level) { return _magicResist.Get(_level); }
    public float criticalChanceOrigin(int _level) { return _criticalChance.Get(_level); }
    public float criticalDamageOrigin(int _level) { return _criticalDamage.Get(_level); }
    public float moveSpeedOrigin(int _level) { return _moveSpeed.Get(_level); }
    public float attackSpeedOrigin(int _level) { return _attackSpeed.Get(_level); }
    public float cooldownOrigin(int _level) { return _cooldown.Get(_level); }
    public float absorptionOrigin(int _level) { return _absorption.Get(_level); }
    public float castRangeOrigin(int _level) { return skillTemplates[normalAttackIndex].castRange.Get(_level); }
}
