using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSkillEffect : SkillEffect
{
    public bool followEffectMount = false;
    public bool isAggroStay = false;
    public bool useLifeTime = true;
    public float lifeTime = 1;
    float timer = 0;
    [HideInInspector] public float attackDamage = 0;
    [HideInInspector] public float abilityPower = 0;
    [HideInInspector] public float healthMaxPerDamage = 0;
    [HideInInspector] public float currentHealthPerDamage = 0;
    [HideInInspector] public float lostHealthPerDamage = 0;
    [HideInInspector] public float trueDamage = 0;
    private const float DAMAGE_TIMER_MAX = 1f;
    float stayTimer = DAMAGE_TIMER_MAX;
    public float speed = 1;

    void Start() { Update(); }

    void Update()
    {
        if (caster != null)
        {
            if (useLifeTime)
            {
                if (timer >= lifeTime)
                    Destroy(gameObject);
                timer += Time.deltaTime;
            }

            if (followEffectMount)
                gameObject.transform.localPosition = caster.transform.localPosition;
            
        }
        else
            Destroy(gameObject);

    }

    public override void OnAggroEnter(Entity entity)
    {
        if (isAggroStay) return;
        if(caster)
        {
            if(entity.team != caster.team)
            {
                if (entity.health > 0)
                {
                    var ad = attackDamage;
                    ad += entity.health * currentHealthPerDamage;
                    ad += entity.healthMax * healthMaxPerDamage;
                    ad += (entity.healthMax - entity.health) * lostHealthPerDamage;
                    caster.DealDamageAt(entity, ad, abilityPower, trueDamage, true, name, true);
                    AddBuffToHittedTarget(entity);
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
                    stayTimer -= Time.deltaTime;
                    if (stayTimer <= 0)
                    {
                        //Debug.Log("Projectil Skill Effect : " + stayTimer);

                        stayTimer = DAMAGE_TIMER_MAX;

                        var ad = attackDamage;
                        ad += entity.health * currentHealthPerDamage;
                        ad += entity.healthMax * healthMaxPerDamage;
                        ad += (entity.healthMax - entity.health) * lostHealthPerDamage;
                        caster.DealDamageAt(entity, ad, abilityPower,trueDamage, true, name, true);

                        //caster.DealDamageAt(entity, ad / lifeTime * Time.deltaTime,
                        //                            abilityPower / lifeTime * Time.deltaTime,
                        //                            trueDamage / lifeTime * Time.deltaTime, true, name, false);
                        AddBuffToHittedTarget(entity);
                    }
                }
            }
        }
    }
}

