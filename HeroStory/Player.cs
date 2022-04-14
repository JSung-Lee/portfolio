using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class Player : Entity
{
    public static Player player;

    public Player() { }
    public Player(Player player)
    {
        var fields = GetType().GetFields();
        foreach (var field in fields)
            field.SetValue(this, field.GetValue(player));
    }

    //최대체력
    public override int healthMax
    {
        get
        {
            int itemBonus = (from item in inventory
                             where item.valid
                             select item.equipHealthMaxBonus).Sum();
            return base.healthMax + itemBonus;
        }
    }
    //체력재생
    public override float healthRegeneration
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipHealthRegenBonus).Sum();
            return base.healthRegeneration + itemBonus;
        }
    }
    //최대마나
    public override float manaMax
    {
        get
        {
            int itemBonus = (from item in inventory
                             where item.valid
                             select item.equipManaMaxBonus).Sum();
            return base.manaMax + itemBonus;
        }
    }
    //마나재생
    public override float manaRegeneration
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipManaRegenBonus).Sum();
            return base.manaRegeneration + itemBonus;
        }
    }
    // 스킬 사용 마나 소모량 감소
    public override float manaConsumptions
    {
        get
        {
            var itemBonus = (from item in inventory
                             where item.valid
                             select item.equipManaPerConsumptions).Sum();

            var manaConsumptionsValue = base.manaConsumptions + itemBonus >= 0.5f ? .5f : base.manaConsumptions + itemBonus;
            return base.manaConsumptions;
        }
    }

    //데미지
    public override int attackDamage
    {
        get
        {
            int itemBonus = (from item in inventory
                             where item.valid
                             select item.equipAttackDamageBonus).Sum();
            return base.attackDamage + itemBonus;
        }
    }
    public override int abilityPower
    {
        get
        {
            int itemBonus = (from item in inventory
                             where item.valid
                             select item.equipAbilityPowerBonus).Sum();
            return base.abilityPower + itemBonus;
        }
    }
    //방어력
    public override int armor
    {
        get
        {
            int itemBonus = (from item in inventory
                             where item.valid
                             select item.equipArmorBonus).Sum();
            return base.armor + itemBonus;
        }
    }
    // 마법저항력
    public override int magicResist
    {
        get
        {
            int itemBonus = (from item in inventory
                             where item.valid
                             select item.equipMagicResistBonus).Sum();
            return base.magicResist + itemBonus;
        }
    }
    //크리티컬 확률(1 => 100%)
    public override float criticalChance
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipCriticalChanceBonus).Sum();
            return base.criticalChance + itemBonus;
        }
    }
    //크리티컬 데미지(배율 1 => 100%)
    public override float criticalDamage
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipCriticalDamageBonus).Sum();
            return base.criticalDamage + itemBonus;
        }
    }
    //이동속도
    public override float moveSpeed
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipMoveSpeedBonus).Sum();
            return base.moveSpeed + itemBonus;
        }
    }
    //공격속도(배율 1 => 100%)
    public override float attackSpeed
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipAttackSpeedBonus).Sum();
            return base.attackSpeed * (1f + itemBonus);
        }
    }
    //쿨감(배율 1 => 100%)
    public override float cooldown
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipCooldownBonus).Sum();
            return base.cooldown + itemBonus;
        }
    }
    //피흡(배율 1 => 100%)
    public override float absorption
    {
        get
        {
            float itemBonus = (from item in inventory
                               where item.valid
                               select item.equipAbsorptionBonus).Sum();
            return base.absorption + itemBonus;
        }
    }
    [Header("Skills")]
    public KeyCode[] skillsHotkey;
    public bool autoAttack = true;
    public bool nearAttackFromPlayer = false;
    [HideInInspector] public bool isStopping = false;

    [Header("Inventory")]
    public int inventorySize = 6;
    public List<Item> inventory = new List<Item>();
    public ItemTemplate[] defaultItems;
    public KeyCode[] inventoryHotkeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };
    public int gold = 0;

    [Header("Camera")]
    public CameraManager cam;
    public KeyCode focusKey = KeyCode.Space;
    public bool isFixedFocus = false;

    [Header("Indicator")]
    public MouseCursor cursor;
    public UI_Message message;
    public CastingRange _skill_range;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip audioClip_SkillLevelUp;
    public AudioClip audioClip_LevelUp;

    [HideInInspector] public int wantSkill = -1;
    [HideInInspector] public float defaultMoveSpeed = 5;

    [HideInInspector] public bool isDamage = false;

    private void Awake()
    {

    }
    protected override void Start()
    {
        base.Start();
        
        if (!player)
            player = this;
        else
            Destroy(this);


        for (int i = 0; i < inventorySize; ++i)
        {
            if (i < defaultItems.Length)
                inventory.Add(new Item(defaultItems[i]));
            else
                inventory.Add(new Item());
        }

        cam = Camera.main.GetComponent<CameraManager>();
        if (cam) cam.isPossibleMove = !isFixedFocus;

        if (!cursor) cursor = MouseCursor.self;
        if (!message) message = UI_Message.self;

        
        for(int i=0; i<gameObject.transform.childCount; i++)
        {
            if(transform.GetChild(i).name == "AttackRange")
            {
                _skill_range = transform.GetChild(i).GetComponent<CastingRange>();
            }
        }
        if(SettingManager.self) autoAttack = SettingManager.self.game_AutoAttack;
        defaultMoveSpeed = moveSpeed;
        isStopping = false;
        DontDestroyOnLoad(this.gameObject);
    }

    protected override void Update()
    {
        //키보드 입력
        KeyInputHandling();

        if (!EventSystem.current.IsPointerOverGameObject())
            SelectionHandling(); //마우스 입력
        
        base.Update();
    }

    private void LateUpdate()
    {
        animator.SetBool("Walk", state == State.Move);
        animator.SetBool("Casting", state == State.Casting);
        animator.SetBool("Stun", state == State.Stun);
        animator.SetBool("Silence", state == State.Silence);
        animator.SetBool("Dead", state == State.Dead);
        //foreach (Skill skill in skills)
        //    if (skill.learned)
        //        animator.SetBool(skill.name, skill.CastTimeRemaining() > 0);
        //foreach (Item item in inventory)
        //    if (item.template && item.usageSkill)
        //        animator.SetBool(item.skill.name, item.skill.CastTimeRemaining() > 0);
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].learned)
                animator.SetBool(skills[i].name, skills[i].CastTimeRemaining() > 0 && i == currentSkill);
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            var item = inventory[i];
            if (item.template && item.usageSkill)
                animator.SetBool(item.skill.name, item.skill.CastTimeRemaining() > 0 && (i + skills.Count) == currentSkill);
        }
        animator.SetFloat("MoveAnimMultiplier", moveSpeed / defaultMoveSpeed);
    }
    bool EventSkillRequest()
    {
        return 0 <= currentSkill;
    }
    bool EventItemSkillRequest()
    {
        return skills.Count <= currentSkill && currentSkill < skills.Count + inventory.Count;
    }
    bool EventSkillInvokeFinished()
    {
        if (EventItemSkillRequest())
            return inventory[currentSkill - skills.Count].skill.InvokeTimeRemaining() == 0 &&
                   !inventory[currentSkill - skills.Count].skill.invoked;
        return 0 <= currentSkill && currentSkill < skills.Count &&
               skills[currentSkill].InvokeTimeRemaining() == 0 &&
               !skills[currentSkill].invoked;
    }
    bool EventSkillCastFinished()
    {
        if (EventItemSkillRequest())
            return inventory[currentSkill - skills.Count].skill.CastTimeRemaining() == 0;
        return 0 <= currentSkill && currentSkill < skills.Count &&
               skills[currentSkill].CastTimeRemaining() == 0;
    }
    public void SetIndicatorViaPosition(Vector3 pos, bool isAttack = false)
    {
        if (!cursor) cursor = MouseCursor.self;
        if (cursor) cursor.CreateIndicator(pos, isAttack);
    }

    public void PopupMessage(string msg)
    {
        if (!message) message = UI_Message.self;
        if (message) message.SetMessage(msg);
    }

    public override void OnAggroEnter(Entity entity)
    {
        if (isStopping) return;
        if (state == State.Idle && currentSkill == -1 && target == null)
        {
            if (autoAttack && entity && CastCheckSelf(skills[normalAttackIndex]))
            {
                SetTarget(entity);
                UseSkill(normalAttackIndex);
            }
        }
    }

    public override void OnAggroStay(Entity entity)
    {
        OnAggroEnter(entity);
    }

    public override void UpdateState_Idle()
    {
        base.UpdateState_Idle();
        if (EventSkillRequest())
        {
            Item item = inventory[0];
            Skill skill;
            if (EventItemSkillRequest())
            {
                item = inventory[currentSkill - skills.Count];
                skill = item.skill;
            }
            else
                skill = skills[currentSkill];

            var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;

            if (CastCheckSelf(skill, !skill.isNormalAttack) && CastCheckTarget(skill))
            {
                Vector3 destination;

                if (CastCheckDistance(skill, out destination))
                {
                    agent.destination = transform.position;
                    agent.ResetPath();
                    if (target) LookAtY(target.transform.position);
                    else LookAtY(targetPos);
                    if (skill.isNormalAttack)
                    {
                        skill.castTimeEnd = Time.time + (skill.castTime / attackSpeed);
                        skill.invokeTimeEnd = Time.time + (skill.invokeTime / attackSpeed);
                    }
                    else
                    {
                        skill.castTimeEnd = Time.time + skill.castTime;
                        skill.invokeTimeEnd = Time.time + skill.invokeTime;
                    }
                    skill.repeatEnd = Time.time + skill.repeatTime;
                    if (EventItemSkillRequest())
                    {
                        item.skill = skill;
                        inventory[currentSkill - skills.Count] = item;
                    }
                    else
                        skills[currentSkill] = skill;
                    if (skill.visualEffect_Begin != null)
                    {
                        GameObject go = Instantiate(skill.visualEffect_Begin.gameObject, GetEffectMount(skill.effectMount).position, GetEffectMount(skill.effectMount).rotation);
                        var _effect = go.GetComponent<VisualSkillEffect>();
                        if (_effect.followEffectMount)
                        {
                            Debug.Log(_effect.name);
                            go.transform.parent = GetEffectMount(skill.effectMount);
                        }
                        if (skill.effectMount == EffectMount.BothHands)
                        {
                            GameObject goRight = Instantiate(skill.visualEffect_Begin.gameObject, GetEffectMount(EffectMount.RightHand).position, GetEffectMount(EffectMount.RightHand).rotation);
                            var _effectRight = goRight.GetComponent<VisualSkillEffect>();
                            if (_effectRight.followEffectMount) goRight.transform.parent = GetEffectMount(EffectMount.RightHand);
                        }
                    }
                    state = State.Casting;
                    return;
                }
                else
                {
                    agent.stoppingDistance = skillCastRange;
                    agent.destination = agent.NearestValidDestination(destination);
                    //agent.SetDestination(agent.NearestValidDestination(destination));
                    state = State.Move;
                    return;
                }
            }
            else
            {
                if (skill.isNormalAttack)
                {
                    if (!skill.IsReady() && CastCheckTarget(skill))
                    {
                        Vector3 destination;
                        if (CastCheckDistance(skill, out destination))
                        {
                            if (Vector3.Distance(destination, transform.position) <= skillCastRange) return;
                            agent.stoppingDistance = skillCastRange;
                            agent.destination = agent.NearestValidDestination(destination);
                            //agent.SetDestination(agent.NearestValidDestination( destination));
                            state = State.Move;
                        }
                    }
                }
                else
                {
                    currentSkill = nextSkill = -1;
                    state = State.Idle;
                }
                return;
            }
        }

    }

    public override void UpdateState_Move()
    {
        for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
        }
        base.UpdateState_Move();
        if (EventMoveEnd())
        {
            //currentSkill = -1;
            state = State.Idle;
            return;
        }
        if (EventSkillRequest())
        {
            Item item = inventory[0];
            Skill skill;
            if (EventItemSkillRequest())
            {
                item = inventory[currentSkill - skills.Count];
                skill = item.skill;
            }
            else
                skill = skills[currentSkill];
            var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;

            if (CastCheckSelf(skill) && CastCheckTarget(skill))
            {
                Vector3 destination;
                if (CastCheckDistance(skill, out destination))
                {
                    agent.destination = transform.position;
                    agent.ResetPath();
                    if (target) LookAtY(target.transform.position);
                    else LookAtY(targetPos);
                    if (skill.isNormalAttack)
                    {
                        skill.castTimeEnd = Time.time + (skill.castTime / attackSpeed);
                        skill.invokeTimeEnd = Time.time + (skill.invokeTime / attackSpeed);
                    }
                    else
                    {
                        skill.castTimeEnd = Time.time + skill.castTime;
                        skill.invokeTimeEnd = Time.time + skill.invokeTime;
                    }
                    skill.repeatEnd = Time.time + skill.repeatTime;
                    if (EventItemSkillRequest())
                    {
                        item.skill = skill;
                        inventory[currentSkill - skills.Count] = item;
                    }
                    else
                        skills[currentSkill] = skill;
                    state = State.Casting;

                    if (skill.visualEffect_Begin != null)
                    {
                        GameObject go = Instantiate(skill.visualEffect_Begin.gameObject, GetEffectMount(skill.effectMount).position, GetEffectMount(skill.effectMount).rotation);
                        var _effect = go.GetComponent<VisualSkillEffect>();
                        if (_effect.followEffectMount) go.transform.parent = GetEffectMount(skill.effectMount);
                        if (skill.effectMount == EffectMount.BothHands)
                        {
                            GameObject goRight = Instantiate(skill.visualEffect_Begin.gameObject, GetEffectMount(EffectMount.RightHand).position, GetEffectMount(EffectMount.RightHand).rotation);
                            var _effectRight = goRight.GetComponent<VisualSkillEffect>();
                            if (_effectRight.followEffectMount) goRight.transform.parent = GetEffectMount(EffectMount.RightHand);
                        }
                    }
                    return;
                }
                else
                {
                    agent.stoppingDistance = skillCastRange;
                    agent.destination = agent.NearestValidDestination(destination);
                    //agent.SetDestination(agent.NearestValidDestination( destination));
                    state = State.Move;
                    return;
                }
            }
            else
            {
                if (skill.isNormalAttack)
                {
                    if (!skill.IsReady() && CastCheckTarget(skill))
                    {
                        Vector3 destination;
                        if (CastCheckDistance(skill, out destination))
                        {
                            if (Vector3.Distance(destination, transform.position) <= skillCastRange) return;
                            agent.stoppingDistance = skillCastRange;
                            agent.destination = agent.NearestValidDestination(destination);
                            //agent.SetDestination(agent.NearestValidDestination( destination));
                            state = State.Move;
                        }
                    }
                }
                else
                {
                    currentSkill = nextSkill = -1;
                    state = State.Idle;
                }
                return;
            }
        }
    }

    public override void UpdateState_Casting()
    {
        base.UpdateState_Casting();
        if (EventTargetDied())
        {
            Item item = inventory[0];
            Skill skill;
            if (EventItemSkillRequest())
            {
                item = inventory[currentSkill - skills.Count];
                skill = item.skill;
            }
            else
                skill = skills[currentSkill];
            if (skill.cancelCastIfTargetDied)
            {
                currentSkill = nextSkill = -1;
                target = null;
                state = State.Idle;
                return;
            }
        }
        if (EventMoveSkill())
        {
            Item item = inventory[0];
            Skill skill;
            if (EventItemSkillRequest())
            {
                item = inventory[currentSkill - skills.Count];
                skill = item.skill;
            }
            else
                skill = skills[currentSkill];


            if (skill.invoked && !skill.finish)
            {
                var moveEffect = GetComponentInChildren<MoveSkillEffect>();
                if (moveEffect) skill = moveEffect.MoveEntity(skill);
                if (EventItemSkillRequest())
                {
                    item.skill = skill;
                    inventory[currentSkill - skills.Count] = item;
                }
                else
                    skills[currentSkill] = skill;
            }
        }
        if (EventSkillCastFinished())
        {
            Item item = inventory[0];
            Skill skill;
            if (EventItemSkillRequest())
            {
                item = inventory[currentSkill - skills.Count];
                skill = item.skill;
            }
            else
                skill = skills[currentSkill];
            if (!skill.invoked)
                InvokeSkill(skill);
            CastSkill(skill);
            if (nextSkill != -1)
            {
                currentSkill = nextSkill;
                nextSkill = -1;
            }
            else currentSkill = skill.followupDefaultAttack ? normalAttackIndex : -1;
            if (currentSkill == normalAttackIndex)
            {
                if (target)
                {
                    var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;

                    agent.stoppingDistance = skillCastRange;
                    agent.SetDestination(agent.NearestValidDestination(target.transform.position));
                    state = State.Move;
                }
                else
                {
                    StopAction();
                    state = State.Idle;
                }
                return;
            }
            state = State.Idle;
            return;
        }
        if (EventSkillInvokeFinished())
        {
            Item item = inventory[0];
            Skill skill;
            if (EventItemSkillRequest())
            {
                item = inventory[currentSkill - skills.Count];
                skill = item.skill;
            }
            else
                skill = skills[currentSkill];

            InvokeSkill(skill);
            state = State.Casting;
            return;
        }
    }
    public override void UpdateState_Dead()
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
            else {
                // this point
                InGameManager.Instance.gameOverEvent();
            }
        }
    }
    
    public override void OnDeath()
    {
        base.OnDeath();

        long loseExperience = 0;
        experience -= loseExperience;

        int loseGold = 0;
        gold -= loseGold;
    }

    public Entity GetEntityOfTeamInRaycastHits(RaycastHit[] hits, Team _team = Team.All)
    {
        foreach(var hit in hits)
        {
            var entity = hit.transform.GetComponent<Entity>();
            if (entity && entity.health > 0 && (_team == Team.All || _team == entity.team))
                return entity;
        }
        return null;
    }

    void SelectionHandling()
    {
        bool left = Input.GetMouseButtonDown(0);
        bool right = Input.GetMouseButtonDown(1);

        if (wantSkill >= 0 && wantSkill < skills.Count && UI_Toggle.self.skillsSmartCasting[wantSkill])
            left = true;
        else if (wantSkill >= skills.Count && UI_Toggle.self.itemsSmartCasting[wantSkill - skills.Count])
            left = true;
        if (!left && !right)
            return;

        isStopping = false;
        int layerMask = (1 << LayerMask.NameToLayer("Raycast")) + (1 << LayerMask.NameToLayer("Floor"));
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        if (hits.Length > 0)
        {
            var entity = GetEntityOfTeamInRaycastHits(hits);
            if (entity)
            {
                int skillIndex = -1;
                if (left)
                {
                    if (0 <= wantSkill)
                    {
                        var skill = wantSkill >= skills.Count ? inventory[wantSkill - skills.Count].skill : skills[wantSkill];
                        entity = GetEntityOfTeamInRaycastHits(hits, skill.GetTeamThisSkillTarget(this));
                        //스킬 사용하려고 스킬 누르고(wantSkill) 대상 클릭(left) -> 타겟팅
                        if (!CorrectedTarget(skill, entity))
                        {
                            PopupMessage("잘못된 대상 선택입니다.");
                            return;
                        }
                        if (!entity || entity.health <= 0) return;
                        skillIndex = wantSkill;
                    }
                    else
                    {
                        //그냥 타겟 클릭
                        Debug.Log(entity.name + entity.health);
                    }
                }
                else if (right)
                {
                    //스킬사용 키 입력없이 타겟(적) 우클릭 -> 평타
                    if (entity.team != team)
                        skillIndex = normalAttackIndex;
                    else
                    {
                        //플레이어 영역 근처 우클릭(이동)
                        layerMask = (1 << LayerMask.NameToLayer("Floor"));
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit floorHit;
                        if (Physics.Raycast(ray, out floorHit, Mathf.Infinity, layerMask))
                        {
                            SetIndicatorViaPosition(floorHit.point);
                            var bestDestination = agent.NearestValidDestination(floorHit.point);
                            MoveTo(bestDestination, 0);
                        }
                    }
                }

                if (skillIndex >= 0)
                {
                    var skill = skillIndex >= skills.Count ? inventory[skillIndex - skills.Count].skill : skills[skillIndex];
                    if (currentSkill >= 0)
                    {
                        var _currentSkill = currentSkill >= skills.Count ? inventory[currentSkill - skills.Count].skill : skills[currentSkill];
                        if (!_currentSkill.canCancelDuringCasting || currentSkill == skillIndex)
                        {
                            nextSkill = skillIndex;
                            wantSkill = -1;
                            if (!cursor) cursor = MouseCursor.self;
                            if (cursor) cursor.SetCursorBase();

                            if(_skill_range) _skill_range.SkillCastingRange(false);
                            return;
                        }
                        else
                        {
                            StopAction();
                        }
                    }
                    if(entity != target)
                        SetTarget(entity);  //타겟 설정
                    //스킬 방향 지정 필요
                    if (!skill.IsReady())
                    {
                        var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;

                        agent.stoppingDistance = skillCastRange;
                        agent.SetDestination( agent.NearestValidDestination( target.transform.position));
                        state = State.Move;
                    }
                    if(skillIndex != currentSkill)
                        UseSkill(skillIndex);   //스킬 사용(대기)
                }
            }
            else
            {
                if (left)
                {
                    int skillIndex = -1;
                    if (left && 0 <= wantSkill)
                    {
                        var skill = wantSkill >= skills.Count ? inventory[wantSkill - skills.Count].skill : skills[wantSkill];
                        //스킬 사용하려고 스킬 누르고(wantSkill) 땅 클릭(left) -> 논타겟/어택땅
                        if (!CorrectedTarget(skill, entity))
                        {
                            PopupMessage("잘못된 대상 선택입니다.");
                            if ((wantSkill >= 0 && wantSkill < skills.Count && UI_Toggle.self.skillsSmartCasting[wantSkill]) ||
                                (wantSkill >= skills.Count && UI_Toggle.self.itemsSmartCasting[wantSkill - skills.Count]))
                            {
                                wantSkill = -1;
                                if (!cursor) cursor = MouseCursor.self;
                                if (cursor) cursor.SetCursorBase();

                                if(_skill_range) _skill_range.SkillCastingRange(false);
                            }
                            return;
                        }
                        if (wantSkill == normalAttackIndex)
                        {
                            Entity[] entities;
                            if (nearAttackFromPlayer)
                                entities = GetEntitiesInRangeFromPosition(transform.position, 150f, Team.Enemy);
                            else
                            {
                                var vec = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
                                var startPos = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
                                float r = startPos.y / vec.y;
                                var destination = startPos - (vec * r);
                                entities = GetEntitiesInRangeFromPosition(destination, 150f, Team.Enemy);
                            }
                            if (entities.Length > 0) {
                                SetTarget(entities[0]);
                                skillIndex = wantSkill;
                                SetIndicatorViaPosition(hits[0].point, true);
                            }
                            else
                            {
                                target = null;
                                layerMask = 1 << LayerMask.NameToLayer("Floor");
                                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                RaycastHit floorHit;
                                if (Physics.Raycast(ray, out floorHit, Mathf.Infinity, layerMask))
                                {
                                    SetIndicatorViaPosition(floorHit.point, true);
                                    var bestDestination = agent.NearestValidDestination(floorHit.point);
                                    MoveTo(bestDestination, 0);
                                }
                            }
                        }
                        else
                        {
                            targetPos = hits[0].point;
                            skillIndex = wantSkill;
                        }
                    }

                    if (skillIndex >= 0)
                    {
                        var skill = skillIndex >= skills.Count ? inventory[skillIndex - skills.Count].skill : skills[skillIndex];
                        var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;

                        if (currentSkill >= 0)
                        {
                            var _currentSkill = currentSkill >= skills.Count ? inventory[currentSkill - skills.Count].skill : skills[currentSkill];
                            if(!_currentSkill.canCancelDuringCasting || currentSkill == skillIndex)
                            {
                                nextSkill = skillIndex;
                                wantSkill = -1;
                                if (!cursor) cursor = MouseCursor.self;
                                if (cursor) cursor.SetCursorBase();

                                if(_skill_range) _skill_range.SkillCastingRange(false);
                                return;
                            }
                            else
                            {
                                StopAction();
                            }
                        }
                        //스킬 방향 지정 필요
                        if (!skill.IsReady())
                        {
                            agent.stoppingDistance = skillCastRange;
                            agent.SetDestination(agent.NearestValidDestination(target.transform.position));
                            state = State.Move;
                        }
                        UseSkill(skillIndex);   //스킬 사용(대기)
                    }
                }
                if (right)
                {
                    //스킬 사용중 취소
                    if (currentSkill >= 0)
                    {
                        var skill = currentSkill >= skills.Count ? inventory[currentSkill - skills.Count].skill : skills[currentSkill];
                        if (skill.template is MoveSkillTemplate)
                        {
                            if (skill.CastTimeRemaining() > ((MoveSkillTemplate)skill.template).afterMotionTime)
                                return;
                        }
                        else if (!skill.canCancelDuringCasting) return;
                        skill.invoked = false;
                        skill.finish = false;
                        if (currentSkill >= skills.Count)
                        {
                            var item = inventory[currentSkill - skills.Count];
                            item.skill = skill;
                            inventory[currentSkill - skills.Count] = item;
                        }
                        else
                            skills[currentSkill] = skill;
                    }
                    //우클릭했는데 스킬입력도 없었고, 클릭위치에 적도 없으면 걍 이동
                    target = null;
                    layerMask = 1 << LayerMask.NameToLayer("Floor");
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit floorHit;
                    if (Physics.Raycast(ray, out floorHit, Mathf.Infinity, layerMask))
                    {
                        SetIndicatorViaPosition(floorHit.point);
                        var bestDestination = agent.NearestValidDestination(floorHit.point);
                        MoveTo(bestDestination, 0);
                    }
                }
            }
            wantSkill = -1;
            if (!cursor) cursor = MouseCursor.self;
            if (cursor) cursor.SetCursorBase();

            if(_skill_range) _skill_range.SkillCastingRange(false);
        }
        else
        {
            var vec = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            var startPos = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            float r = startPos.y / vec.y;
            var destination = startPos - (vec * r);
            if (left)
            {
                int skillIndex = -1;
                if (left && 0 <= wantSkill)
                {
                    var skill = wantSkill >= skills.Count ? inventory[wantSkill - skills.Count].skill : skills[wantSkill];
                    //스킬 사용하려고 스킬 누르고(wantSkill) 땅 클릭(left) -> 논타겟/어택땅
                    if (!CorrectedTarget(skill, null))
                    {
                        PopupMessage("잘못된 대상 선택입니다.");
                        if ((wantSkill >= 0 && wantSkill < skills.Count && UI_Toggle.self.skillsSmartCasting[wantSkill]) ||
                            (wantSkill >= skills.Count && UI_Toggle.self.itemsSmartCasting[wantSkill - skills.Count]))
                        {
                            wantSkill = -1;
                            if (!cursor) cursor = MouseCursor.self;
                            if (cursor) cursor.SetCursorBase();

                            if (_skill_range) _skill_range.SkillCastingRange(false);
                        }
                        return;
                    }
                    if (wantSkill == normalAttackIndex)
                    {
                        Entity[] entities;
                        if (nearAttackFromPlayer)
                            entities = GetEntitiesInRangeFromPosition(transform.position, 150f, Team.Enemy);
                        else {
                            entities = GetEntitiesInRangeFromPosition(destination, 150f, Team.Enemy);
                        }
                        if (entities.Length > 0)
                        {
                            SetTarget(entities[0]);
                            skillIndex = wantSkill;
                            SetIndicatorViaPosition(destination, true);
                        }
                        else {
                            target = null;
                            SetIndicatorViaPosition(destination, true);
                            var bestDestination = agent.NearestValidDestination(destination);
                            MoveTo(bestDestination, 0);                            
                        }
                    }
                    else
                    {
                        targetPos = destination;
                        skillIndex = wantSkill;
                    }
                }

                if (skillIndex >= 0)
                {
                    var skill = skillIndex >= skills.Count ? inventory[skillIndex - skills.Count].skill : skills[skillIndex];
                    var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;

                    if (currentSkill >= 0)
                    {
                        var _currentSkill = currentSkill >= skills.Count ? inventory[currentSkill - skills.Count].skill : skills[currentSkill];
                        if (!_currentSkill.canCancelDuringCasting || currentSkill == skillIndex)
                        {
                            nextSkill = skillIndex;
                            wantSkill = -1;
                            if (!cursor) cursor = MouseCursor.self;
                            if (cursor) cursor.SetCursorBase();

                            if(_skill_range) _skill_range.SkillCastingRange(false);
                            return;
                        }
                        else
                        {
                            StopAction();
                        }
                    }
                    //스킬 방향 지정 필요
                    if (!skill.IsReady())
                    {
                        agent.stoppingDistance = skillCastRange;
                        agent.SetDestination(agent.NearestValidDestination(target.transform.position));
                        state = State.Move;
                    }
                    UseSkill(skillIndex);   //스킬 사용(대기)
                }
            }
            if (right)
            {
                SetIndicatorViaPosition(destination);
                var bestDestination = agent.NearestValidDestination(destination);
                MoveTo(bestDestination, 0);
            }
            wantSkill = -1;
            if (!cursor) cursor = MouseCursor.self;
            if (cursor) cursor.SetCursorBase();

            if(_skill_range) _skill_range.SkillCastingRange(false);
        }
    }

    void KeyInputHandling()
    {
        if (!UI_Toggle.self) return;
        if (UI_Option.self && UI_Option.self.IsActiveBook()) return;
        for (int i = 0; i < UI_Toggle.self.skillsHotkey.Length; i++)
        {
            if (Input.GetKeyDown(UI_Toggle.self.skillsHotkey[i]))
            {
                var skill = skills[i];
                var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;

                if (Input.GetKey(UI_Toggle.self.skillLearnKey))
                {
                    if (CanLearnSkill(skill)) { LearnSkill(i); }
                    else if (CanUpgradeSkill(skill)) { UpgradeSkill(i); }
                    continue;
                }
                //if (skill.isSpecialCondition) continue;
                if (skill.isNormalAttack)
                {
                    if (!cursor) cursor = MouseCursor.self;
                    if (cursor) cursor.SetCursorAttack();
                }
                else if (!CastCheckSelf(skill)) continue;
                wantSkill = i;
                if (skill.immediatelyCasting)
                {
                    if (skill.template is AutoTargetSkillTemplate)
                    {
                        var entities = GetEntitiesInRange(skillCastRange, Team.Enemy);
                        if (entities.Length > 0)
                        {
                            target = entities[0];
                            UseSkill(wantSkill);
                            wantSkill = -1;
                        }
                        else
                        {
                            PopupMessage("사용 가능한 대상이 없습니다.");
                            wantSkill = -1;
                            continue;
                        }
                    }
                    else
                    {
                        UseSkill(wantSkill);
                        wantSkill = -1;
                    }
                }
                else if (skill.showSelector && !UI_Toggle.self.skillsSmartCasting[wantSkill])
                {
                    if (_skill_range) _skill_range.xradius = skillCastRange;
                    if (_skill_range) _skill_range.yradius = skillCastRange;

                    //Debug.Log(skill.name);
                    if (_skill_range) _skill_range.SkillCastingRange(true, skill);
                    

                    if (!skill.isNormalAttack)
                    {
                        if (!cursor) cursor = MouseCursor.self;
                        if (cursor) cursor.SetCursorTarget();
                    }
                }
                isStopping = false;
            }
        }
        for (int i = 0; i < UI_Toggle.self.inventoryHotkeys.Length; i++)
        {
            if (Input.GetKeyDown(UI_Toggle.self.inventoryHotkeys[i]))
            {
                var item = inventory[i];
                if (!item.usageSkill) continue;
                var skill = item.skill;
                if (!CastCheckSelf(skill)) continue;
                //if (skill.isSpecialCondition) continue;
                wantSkill = skills.Count + i;
                if (skill.immediatelyCasting)
                {
                    if (skill.template is AutoTargetSkillTemplate)
                    {
                        var skillCastRange = skill.addUseCastRange ? skill.castRange + addCastRange : skill.castRange;
                        var entities = GetEntitiesInRange(skillCastRange, Team.Enemy);
                        if (entities.Length > 0)
                        {
                            target = entities[0];
                            UseItemSkill(i);
                            wantSkill = -1;
                        }
                        else
                        {
                            PopupMessage("사용 가능한 대상이 없습니다.");
                            wantSkill = -1;
                            continue;
                        }
                    }
                    else
                    {
                        UseItemSkill(i);
                        wantSkill = -1;
                    }
                }
                else if (skill.showSelector && !UI_Toggle.self.itemsSmartCasting[i])
                {
                    if (skill.isNormalAttack)
                    {
                        if (!cursor) cursor = MouseCursor.self;
                        if (cursor) cursor.SetCursorAttack();
                    }
                    else
                    {
                        if (!cursor) cursor = MouseCursor.self;
                        if (cursor) cursor.SetCursorTarget();
                    }
                }
                isStopping = false;
            }
        }
        if (Input.GetKeyDown(UI_Toggle.self.stopActionKey))
        {
            StopAction();
            isStopping = true;
        }
        if (Input.GetKeyDown(UI_Toggle.self.cancelKey))
        {
            if (wantSkill >= 0)
            {
                wantSkill = -1;
                if (!cursor) cursor = MouseCursor.self;
                if (cursor) cursor.SetCursorBase();
            }
        }
        //카메라가 캐릭터를 따라가게 함(누르고 있을때 혹은 캐릭터에 고정일때)
        if (Input.GetKeyDown(UI_Toggle.self.fixedFocusKey))
        {
            isFixedFocus = !isFixedFocus;
        }
        if (Input.GetKey(UI_Toggle.self.focusKey) || isFixedFocus)
        {
            if (!cam) cam = Camera.main.GetComponent<CameraManager>();
            cam.FocusOn(transform.position);
            cam.isPossibleMove = false;
        }
        else
        {
            cam.isPossibleMove = true;
        }
    }

    public bool CorrectedTarget(Skill skill, Entity entity = null)
    {
        return skill.CorrectedTarget(this, entity);
    }

    public override void MoveTo(Vector3 destination, float stopDistance = 0)
    {
        base.MoveTo(destination, stopDistance);
    }

    public override void TeleportTo(Vector3 destination, float stopDistance = 0)
    {
        base.TeleportTo(destination, stopDistance);
    }

    public override bool CastCheckSelf(Skill skill, bool setMessage = true)
    {

        //쿨돌앗나, 살아있나, 마나 남았나
        if (skill.isNormalAttack) setMessage = false;

        var manaCosts = skill.onOff && skill.isOn ? 0 : skill.manaCosts;
        manaCosts -= (manaCosts * manaConsumptions);

        if (mana < manaCosts)
        {
            if (setMessage) PopupMessage("마나가 부족합니다.");
            return false;
        }
        if (!skill.IsReady())
        {
            if (setMessage) PopupMessage("아직 사용할 수 없습니다.");
            return false;
        }
        if (health <= 0)
        {
            if (setMessage) PopupMessage("지금은 사용할 수 없습니다.");
            return false;
        }
        if (!skill.learned)
        {
            if (setMessage) PopupMessage("스킬을 아직 배우지 않았습니다.");
            return false;
        }
        return true;
    }

    void SetTarget(Entity entity)
    {
        if (entity != null)
        {
            target = entity;
            targetPos = entity.transform.position;
        }
    }

    public void UseSkill(int skillIndex)
    {
        Debug.Log("Use Skill " + skillIndex);

        if (skillIndex >= skills.Count)
        {
            UseItemSkill(skillIndex - skills.Count);
            return;
        }
        if (currentSkill == -1 && skills[skillIndex].learned)
        {
            currentSkill = skillIndex;
            var skill = skills[currentSkill];
            skill.invoked = false;
            skill.finish = false;
            skills[currentSkill] = skill;
        }
        else if (currentSkill != skillIndex && CanCancelDuringCasting())
        {
            StopAction();
            UseSkill(skillIndex);
            return;
        }
        else
        {
            nextSkill = skillIndex;
        }
    }

    public override void DealDamageAt(Entity _target, float attackDamage, float abilityPower, float trueDamage, bool useCrit, string skillName, bool isOnce)
    {
        base.DealDamageAt(_target, attackDamage, abilityPower, trueDamage, useCrit, skillName, isOnce);

        InGameManager.Instance.getPlayerDealDamageAt(attackDamage, abilityPower, trueDamage);

        if (_target.health <= 0 && _target.revive <= 0 && _target is Monster)
        {
            var reward = (Monster)_target;
            int rewardGold = (int)(reward.gold * (1f + (from item in inventory
                                                        where item.valid
                                                        select item.equipGainGoldBonus).Sum()));
            InGameManager.Instance.playerGetGold(rewardGold);

            gold += rewardGold;
            //Debug.Log("gold : " + gold + ", rewardGold : " + rewardGold);
            if (_target is Chest)
                DamagePopupManager.self.ShowGoldPopup(PopupType.BigGold, reward.gold, _target, 4, 1.0f);
            else
                DamagePopupManager.self.ShowGoldPopup(PopupType.Gold, reward.gold, _target, 3);
            int rewardExp = (int)(reward.exp * (1f + (from item in inventory
                                                       where item.valid
                                                       select item.equipGainGoldBonus).Sum()));
            experience += rewardExp;
            //Debug.Log("Experience : " + experience + ", rewardExp : " + rewardExp);
            if(!(_target is Chest))
                InGameManager.Instance.playerGetExperience(rewardExp);
        }
    }

    public void UseItemSkill(int index)
    {
        if (!inventory[index].usageSkill) return;
        if (currentSkill == -1 && inventory[index].usageSkill)
        {
            currentSkill = skills.Count + index;
            var item = inventory[index];
            var skill = item.skill;
            skill.invoked = false;
            skill.finish = false;
            item.skill = skill;
            inventory[index] = item;
            Debug.Log("Use Item Name : " + item.skill.name);
            UpdateSpecialConditionSkill(SkillCondition.cleanDebuffs);
        }
        else if (currentSkill != index && CanCancelDuringCasting())
        {
            StopAction();
            UseItemSkill(index);
        }
        else
        {
            nextSkill = skills.Count + index;
        }
        Debug.Log("Use Item : " + inventory[index].name);
    }

    public bool CanCancelDuringCasting()
    {
        if (currentSkill == -1) return false;
        if (currentSkill < skills.Count)
            return skills[currentSkill].canCancelDuringCasting;
        else
        {
            var item = inventory[currentSkill - skills.Count];
            if (!item.usageSkill) return false;
            return item.skill.canCancelDuringCasting;
        }
        return false;
    }

    public override void StopAction()
    {
        if (currentSkill == -1) { }
        else if (currentSkill < skills.Count)
        {
            var skill = skills[currentSkill];
            skill.castTimeEnd = Time.time - 1f;
            skill.invokeTimeEnd = Time.time - 1f;
            skill.repeatEnd = Time.time - 1f;
            skills[currentSkill] = skill;
            currentSkill = -1;
        }
        else
        {
            var item = inventory[currentSkill - skills.Count];
            var skill = item.skill;
            skill.castTimeEnd = Time.time - 1f;
            skill.invokeTimeEnd = Time.time - 1f;
            skill.repeatEnd = Time.time - 1f;
            Debug.Log("StopAction currentSkill : " + currentSkill + ", skill : " + skill.name);
            skills[currentSkill] = skill;
            inventory[currentSkill - skills.Count] = item;
            currentSkill = -1;
        }
        state = State.Idle;
        agent.SetDestination(transform.position);
        //agent.destination = transform.position;
    }

    public void SwapInventoryItem(int from, int to)
    {
        if (0 <= from && from < inventory.Count &&
            0 <= to && to < inventory.Count && from != to)
        {
            var temp = inventory[from];
            inventory[from] = inventory[to];
            inventory[to] = temp;
        }
    }

    public int GetInventoryValidCount()
    {
        return inventory.Where(item => item.valid == true).Count();
    }

    public int GetInventoryNotValidIndex()
    {
        for (int i = 0; i < inventory.Count; i++)
            if (!inventory[i].valid) return i;
        return -1;
    }

    public bool InventoryCanAddAmount(ItemTemplate template, int amount)
    {
        if (GetInventoryValidCount() < inventorySize) return true;
        for (int i = 0; i < inventory.Count; i++)
            if (inventory[i].name == template.name)
                return (inventory[i].amount + amount <= template.maxStack);
        return false;
    }
    public bool InventoryHaveItem(ItemTemplate template)
    {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i].name == template.name && inventory[i].valid)
                return true;
        }
        return false;
    }
    public bool InventoryAddAmount(ItemTemplate template, int amount, bool isPayPrice = false)
    {
        if (!InventoryCanAddAmount(template, amount)) return false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == template.name &&
                inventory[i].amount + amount <= inventory[i].maxStack)
            {
                if (isPayPrice)
                {
                    if (template.buyPrice > gold) return false;
                    int price = template.buyPrice;
                    if (template.mergeTemplate.Length > 0) 
                    {
                        foreach (var _temp in template.mergeTemplate) 
                        {
                            if (InventoryHaveItem(_temp))
                            {
                                InventoryDeleteAmount(_temp, 1);
                                price -= _temp.buyPrice;
                            }
                            else if(_temp.mergeTemplate.Length > 0)
                            {
                                foreach (var _temp2 in _temp.mergeTemplate)
                                {
                                    if (InventoryHaveItem(_temp2))
                                    {
                                        InventoryDeleteAmount(_temp2, 1);
                                        price -= _temp2.buyPrice;
                                    }
                                }
                            }
                        }
                    }
                    gold -= price;
                }
                var item = inventory[i];
                item.amount = Mathf.Min(item.amount + amount, item.maxStack);
                inventory[i] = item;
                return true;
            }
        }
        if (GetInventoryValidCount() < inventorySize)
        {
            int index = GetInventoryNotValidIndex();
            if (index == -1) return false;
            else
            {
                if (isPayPrice)
                {
                    if (template.buyPrice > gold) return false;
                    int price = template.buyPrice;
                    if (template.mergeTemplate.Length > 0)
                    {
                        foreach (var _temp in template.mergeTemplate)
                        {
                            if (InventoryHaveItem(_temp))
                            {
                                InventoryDeleteAmount(_temp, 1);
                                price -= _temp.buyPrice;
                            }
                            else if (_temp.mergeTemplate.Length > 0)
                            {
                                foreach (var _temp2 in _temp.mergeTemplate)
                                {
                                    if (InventoryHaveItem(_temp2))
                                    {
                                        InventoryDeleteAmount(_temp2, 1);
                                        price -= _temp2.buyPrice;
                                    }
                                }
                            }
                        }
                    }
                    gold -= price;
                }
                inventory[index] = new Item(template, amount);
            }
            return true;
        }
        return false;
    }

    public bool InventoryDeleteAmount(ItemTemplate template, int amount, bool isSellPrice = false)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (!inventory[i].valid) continue;
            if (inventory[i].name == template.name)
            {
                if (inventory[i].amount < amount) return false;
                if (isSellPrice) gold += template.sellPrice;
                var item = inventory[i];
                item.amount -= amount;
                inventory[i] = item;
                if (inventory[i].amount <= 0) inventory[i] = new Item();
                return true;
            }
        }
        return false;
    }

    public int GetInventoryCheckId()
    {
        int result = -1;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].checkId > result)
                result = inventory[i].checkId;
        }
        if (result > 100) {
            result = 0;
            ResetInventoryCheckId(result);
        }
        return result + 1;
    }

    public void ResetInventoryCheckId(int checkId = 0)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            var item = inventory[i];
            item.checkId = checkId;
            inventory[i] = item;
        }
    }

    public int GetInventoryItemIndex(ItemTemplate template, int checkId = -1)
    {
        //Debug.Log(template.name + " " + checkId);
        for (int i = 0; i < inventory.Count; i++)
        {
            if (!inventory[i].valid || inventory[i].checkId == checkId) continue;
            if (inventory[i].name == template.name) {
                var item = inventory[i];
                item.checkId = checkId;
                inventory[i] = item;
                return i;
            }
        }
        return -1;
    }

    public int GetPriceForBuyItem(ItemTemplate itemTemplate, int checkId = -1)
    {
        if(checkId == -1) checkId = GetInventoryCheckId();
        int result = itemTemplate.mergePrice == 0 ? itemTemplate.buyPrice : itemTemplate.mergePrice;
        foreach (var mergeTemplate in itemTemplate.mergeTemplate) {
            if (GetInventoryItemIndex(mergeTemplate, checkId) == -1)
                result += GetPriceForBuyItem(mergeTemplate, checkId);            
        }
        return result;
    }


    public override void CastSkill(Skill skill)
    {
        skill.invoked = false;
        if (currentSkill >= skills.Count)
        {
            var item = inventory[currentSkill - skills.Count];
            item.skill = skill;
            if (item.usageDestroy) item.amount--;
            inventory[currentSkill - skills.Count] = item;
            if (inventory[currentSkill - skills.Count].amount <= 0)
                inventory[currentSkill - skills.Count] = new Item();
        }
        else
            skills[currentSkill] = skill;
        if (skill.visualEffect_End != null)
        {
            GameObject go = Instantiate(skill.visualEffect_End.gameObject, GetEffectMount(skill.effectMount).position, Quaternion.identity);
            var _effect = go.GetComponent<VisualSkillEffect>();
            if (_effect.followEffectMount) go.transform.parent = GetEffectMount(skill.effectMount);
            if (skill.effectMount == EffectMount.BothHands)
            {
                GameObject goRight = Instantiate(skill.visualEffect_End.gameObject, GetEffectMount(EffectMount.RightHand).position, Quaternion.identity);
                var _effectRight = goRight.GetComponent<VisualSkillEffect>();
                if (_effectRight.followEffectMount) goRight.transform.parent = GetEffectMount(EffectMount.RightHand);
            }
        }
        else
            Debug.LogWarning("visualEffect_Invoke is Null");
    }

    public void RoomEnter()
    {
        if (!RoomController.Instance.currRoom.isRoomClear)
        {
            UpdateSpecialConditionSkill(SkillCondition.RoomEnter);
            RoomController.Instance.currRoom.TimerRoomStart(true);
        }
    }

    public void RoomClear()
    {
        UpdateSpecialConditionSkill(SkillCondition.RoomClear);
    }

    public void StageStart()
    {
        UpdateSpecialConditionSkill(SkillCondition.StageStart);
    }

    public void StageClear()
    {
        UpdateSpecialConditionSkill(SkillCondition.StageClear);
    }

    public bool CanLearnSkill(Skill skill)
    {
        return !skill.learned &&
               level >= skill.requiredLevel &&
               SkillpointsSpendable() > 0;
    }

    public bool CanUpgradeSkill(Skill skill)
    {
        return skill.learned &&
               skill.level < skill.maxLevel &&
               level >= skill.upgradeRequiredLevel &&
               SkillpointsSpendable() > 0;
    }

    public int SkillpointsSpendable()
    {
        int spent = skills.Where(s => s.learned).Sum(s => s.level);
        return level - spent + 1;
    }

    public void LearnSkill(int skillIndex)
    {
        if (0 <= skillIndex && skillIndex < skills.Count)
        {
            Skill skill = skills[skillIndex];
            if (CanLearnSkill(skill))
            {
                skill.learned = true;
                skills[skillIndex] = skill;
                if (SettingManager.self)
                    audioSource.volume = SettingManager.self.CalcFxValue();
                audioSource.clip = audioClip_SkillLevelUp;
                audioSource.Play();
            }
        }
    }

    public void UpgradeSkill(int skillIndex)
    {
        if (0 <= skillIndex && skillIndex < skills.Count)
        {
            Skill skill = skills[skillIndex];
            if (CanUpgradeSkill(skill))
            {
                skill.level++;
                skills[skillIndex] = skill;
                if(SettingManager.self)
                    audioSource.volume = SettingManager.self.CalcFxValue();
                audioSource.clip = audioClip_SkillLevelUp;
                audioSource.Play();
            }
        }
    }

    public override void UpdateRepeatSkills()
    {
        base.UpdateRepeatSkills();
        for (int i = 0; i < inventory.Count; i++)
        {
            if (!inventory[i].template || !inventory[i].usageSkill) continue;
            Skill skill = inventory[i].skill;
            if (skill.onOff)
            {
                if (skill.isOn)
                    CastRepeatSkill(skill, skills.Count + i);
            }
            else if (skill.template is PassiveSkillTemplate)
            {
                CastRepeatSkill(skill, skills.Count + i);
            }
        }
    }

    public override void CastRepeatSkill(Skill skill, int skillIndex)
    {
        var repatManaCosts = skill.repeatManaCosts;
        repatManaCosts -= (repatManaCosts * manaConsumptions);

        if (RepeatCheckSelf(skill))
        {
            skill.RepeatApply(this);
            mana -= repatManaCosts;
            skill.repeatEnd = Time.time + skill.repeatTime;
            if (skillIndex >= skills.Count)
            {
                var item = inventory[skillIndex - skills.Count];
                item.skill = skill;
                inventory[skillIndex - skills.Count] = item;
            }
            else
                skills[skillIndex] = skill;
        }
        else if (mana < repatManaCosts)
        {
            if (skill.onOff)
                skill.isOn = false;
            if (skill.isNormalAttack)
                skill.cooldownEnd = Time.time + (skill.cooldown / attackSpeed);
            else
                skill.cooldownEnd = Time.time + (skill.cooldown * (1f - cooldown));
            skill.repeatEnd = Time.time + skill.repeatTime;
            skill.invoked = false;
            if (skillIndex >= skills.Count)
            {
                var item = inventory[skillIndex - skills.Count];
                item.skill = skill;
                inventory[skillIndex - skills.Count] = item;
            }
            else
                skills[skillIndex] = skill;
        }

    }
    public override void UpdateSpecialConditionSkill(SkillCondition condition, Entity _target = null)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (!inventory[i].template || !inventory[i].usageSkill) 
                continue;
            
            Skill skill = inventory[i].skill;

            if (skill.isSpecialCondition && skill.skillCondition == condition)
            {
                if (skill.skillConditionType == SkillConditionType.Self)
                {
                    CastSpecialConditionSkill(skill, i, _target, true);
                }
                else if (skill.skillConditionType == SkillConditionType.ConditionSkill)
                {
                    if (skill.template.buffSkillTemplateConditionSkill)
                        CastSpecialConditionSkill(skill, i, _target, false);
                }
            }
        }
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].isSpecialCondition && skills[i].skillCondition == condition) {
                if (skills[i].skillConditionType == SkillConditionType.Self)
                {
                    CastSpecialConditionSkill(skills[i], i, _target, true);
                }
                else if (skills[i].skillConditionType == SkillConditionType.ConditionSkill)
                {
                    if (skills[i].template.buffSkillTemplateConditionSkill)
                        CastSpecialConditionSkill(skills[i], i, _target, false);
                }
            }
        }
    }
    public override float BeDamagedBy(Entity _attacker, float attackDamage, float abilityPower, float trueDamage, bool isCrit, string skillName, bool isOnce)
    {
        float damageDealt;
        int interval = 2;
        float totalDamage = 0;
        //물뎀
        float _armorResist = armor / (armor + 100);
        damageDealt = Mathf.Max(0f, (attackDamage - (attackDamage * _armorResist)));
        totalDamage += damageDealt;
        if (InGameManager.Instance) InGameManager.Instance.getPlayerBeDamagedBy(damageDealt, 0, 0);
        health -= DealDamageShield(damageDealt);
        if (damageDealt > 0)
            DamagePopupManager.self.ShowDamagePopup(isCrit ? PopupType.Critical : PopupType.Normal, DamageType.Physical, damageDealt, this, interval--, skillName, isOnce);
        //마뎀
        float _magicResist = magicResist / (magicResist + 100);
        damageDealt = Mathf.Max(0, (abilityPower - (abilityPower * _magicResist)));
        totalDamage += damageDealt;
        if (InGameManager.Instance) InGameManager.Instance.getPlayerBeDamagedBy(0, damageDealt, 0);
        health -= DealDamageShield(damageDealt);
        if (damageDealt > 0)
            DamagePopupManager.self.ShowDamagePopup(isCrit ? PopupType.Critical : PopupType.Normal, DamageType.Magic, damageDealt, this, interval--, skillName, isOnce);
        //고뎀
        damageDealt = Mathf.Max(trueDamage, 0);
        totalDamage += damageDealt;
        health -= DealDamageShield(damageDealt);
        if (damageDealt > 0)
            DamagePopupManager.self.ShowDamagePopup(PopupType.Normal, DamageType.True, damageDealt, this, interval--, skillName, isOnce);
        if (totalDamage > 0)
        {
            isDamage = true;
            UpdateSpecialConditionSkill(SkillCondition.ReceiveDeal, this);
            isBattle = 10f;
        }
        return totalDamage;
    }
}


