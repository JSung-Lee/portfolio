using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkillEffect : SkillEffect
{
    public bool isAggroStay = false;
    public bool useLifeTime = true;
    public float lifeTime = 1;

    private const float DAMAGE_TIMER_MAX = 1f;
    float stayTimer = DAMAGE_TIMER_MAX;
    public float speed = 1;


    float timer = 0;

    [HideInInspector] public float attackDamage = 0;
    [HideInInspector] public float abilityPower = 0;
    [HideInInspector] public float healthMaxPerDamage = 0;
    [HideInInspector] public float currentHealthPerDamage = 0;
    [HideInInspector] public float lostHealthPerDamage = 0;

    [HideInInspector] public float manaMaxPerAbilityPower = 0;
    [HideInInspector] public float currentManaPerAbilityPower = 0;
    [HideInInspector] public float lostmanaPerAbilityPower = 0;

    [HideInInspector] public float trueDamage = 0;
    public bool keepAfterHit = false;
    public OrbitType orbitType = OrbitType.Straight;
    public bool useAfterEffectWhenHit = false;
    public bool isGoalEffectOn = false;

    public AreaSkillEffect afterEffect;

    Vector3 startPos;
    void Start() 
    {
        startPos = transform.position;
        Update(); 
    }

    void Update()
    {
        if (caster != null)
        {
            if (useLifeTime) {
                if (timer >= lifeTime)
                    Destroy(gameObject);
                timer += Time.deltaTime;
            }
            Vector3 goal;
            if (target == null)
                goal = new Vector3(targetPos.x, startPos.y, targetPos.z);
            else
            {
                goal = target.collider.bounds.center;
                goal.Set(goal.x, startPos.y, goal.z);
            }
            transform.position = Vector3.MoveTowards(transform.position, goal, speed);
            if (orbitType == OrbitType.Curve)
            {
                float totalDist = Vector3.Distance(startPos, goal);
                Vector3 prettyPos = new Vector3(transform.position.x, startPos.y, transform.position.z);
                float remaining = 1f - (Vector3.Distance(prettyPos, goal) / totalDist);
                float jumpHeight = 3f;
                float angle = Mathf.Max(remaining * 180f, 0);
                float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
                transform.position = prettyPos + new Vector3(0, sin * jumpHeight, 0);
            }
            transform.LookAt(goal);

            if (transform.position == goal) {
                if (target && target.health > 0) 
                {
                    var ad = attackDamage;
                    ad += target.health * currentHealthPerDamage;
                    ad += target.healthMax * healthMaxPerDamage;
                    ad += (target.healthMax - target.health) * lostHealthPerDamage;
                    // 데미지 적용
                    caster.DealDamageAt(target, ad, abilityPower, trueDamage, true, name, true);
                    AddBuffToHittedTarget(target);
                }
                if (keepAfterHit && afterEffect && useAfterEffectWhenHit) 
                {
                    Debug.Log("투사체 충돌 없음 Effect 생성");
                    CreateAfterEffect(true);
                }

                if(isGoalEffectOn)
                    CreateAfterEffect(true);

                Destroy(gameObject);
            }

            Debug.DrawLine(startPos, goal, Color.yellow);
        }
        else {
            if (afterEffect) {
                if(!useAfterEffectWhenHit)
                    CreateAfterEffect();
            }
            Destroy(gameObject);
        }
    }

    public override void OnAggroEnter(Entity entity)
    {
        if (isAggroStay) return;
        if (caster)
        {
            if (entity.team != caster.team)
            {
                if (entity.health > 0)
                {
                    var ad = attackDamage;
                    ad += entity.health * currentHealthPerDamage;
                    ad += entity.healthMax * healthMaxPerDamage;
                    ad += (entity.healthMax - entity.health) * lostHealthPerDamage;

                    // 데미지 적용
                    caster.DealDamageAt(entity, ad, abilityPower, trueDamage, true, name, true);
                    AddBuffToHittedTarget(entity);
                    if (!keepAfterHit && useAfterEffectWhenHit && afterEffect)
                    {
                        CreateAfterEffect();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public override void OnAggroStay(Entity entity)
    {
        if (!isAggroStay) return;
        if (caster)
        {
            if (entity.team != caster.team)
            {
                if (entity.health > 0)
                {
                    // 초당 데미지 적용
                    stayTimer -= Time.deltaTime;
                    if (stayTimer <= 0)
                    {

                        stayTimer = DAMAGE_TIMER_MAX;

                        var ad = attackDamage;
                        ad += entity.health * currentHealthPerDamage;
                        ad += entity.healthMax * healthMaxPerDamage;
                        ad += (entity.healthMax - entity.health) * lostHealthPerDamage;

                        // 데미지 적용
                        caster.DealDamageAt(entity, ad, abilityPower, trueDamage, true, name, true);

                        AddBuffToHittedTarget(entity);

                        if (!keepAfterHit && useAfterEffectWhenHit)
                        {
                            CreateAfterEffect();
                            Destroy(gameObject);
                        }

                        if (keepAfterHit && useAfterEffectWhenHit && afterEffect)
                        {
                            CreateAfterEffect();
                        }
                    }
                }
            }
        }
    }

    public override void OnAggroExit(Entity entity)
    {

    }

        //public void showEffect(Collision coll)
        //{
        //    //충돌지점
        //    //contacts[0] 은 첫번째 충돌지점
        //    ContactPoint contact = coll.contacts[0];
        //    //충돌지점의 법선벡터:contact.normal
        //    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        //    //첫 번째 벡터를 contact.normal까지 회전하는데 필요한 
        //    GameObject go = Instantiate(afterEffect.gameObject, contact.point, Quaternion.identity);
        //    AreaSkillEffect effect = go.GetComponent<AreaSkillEffect>();
        //}


        public void CreateAfterEffect(bool isGoal = false)
    {
        Vector3 createEffectPos = (isGoal ? transform.position : hitPos);
        GameObject go = Instantiate(afterEffect.gameObject, createEffectPos, Quaternion.identity);
        AreaSkillEffect effect = go.GetComponent<AreaSkillEffect>();
        effect.targetPos = transform.position;
        effect.caster = caster;
        effect.attackDamage = attackDamage;
        effect.abilityPower = abilityPower;
        effect.currentHealthPerDamage = currentHealthPerDamage;
        effect.healthMaxPerDamage = healthMaxPerDamage;
        effect.lostHealthPerDamage = lostHealthPerDamage;
        effect.trueDamage = trueDamage;
    }
}
