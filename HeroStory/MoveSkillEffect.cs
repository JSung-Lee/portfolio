using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkillEffect : SkillEffect
{
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
    [HideInInspector] public Vector3 startPosition;

    public bool keepAfterHit = false;
    public bool ignoreObject = false;
    void Start() 
    {
        startPosition = caster.transform.position;
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
        }
        else
            Destroy(gameObject);
    }

    public override void OnAggroEnter(Entity entity)
    {
        if (isAggroStay) return;
        Debug.Log(entity.name);
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
                    caster.DealDamageAt(entity, ad, abilityPower, trueDamage, true, name, true);
                    if (!keepAfterHit)
                        caster.StopAction();
                    AddBuffToHittedTarget(entity);
                }
            }
        }
    }

    public override void OnAggroStay(Entity entity)
    {
        if (!isAggroStay) return;
        Debug.Log(entity.name);
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
                    caster.DealDamageAt(entity, ad / lifeTime * Time.deltaTime,
                                                abilityPower / lifeTime * Time.deltaTime,
                                                trueDamage / lifeTime * Time.deltaTime, true, name, false);
                    if (!keepAfterHit)
                        caster.StopAction();
                    AddBuffToHittedTarget(entity);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (ignoreObject)
        {
            caster.agent.enabled = true;
            caster.collider.isTrigger = false;
        }
    }

    public Skill MoveEntity(Skill skill)
    {
        //Debug.Log("MoveEntity");
        caster.agent.enabled = !ignoreObject;
        caster.collider.isTrigger = ignoreObject;
        Vector3 moveTargetPos = skill.haveToTarget ? target.transform.position : caster.targetPos;
        moveTargetPos.Set(moveTargetPos.x, 0, moveTargetPos.z);
        Vector3 moveVector = (moveTargetPos - startPosition).normalized;
        Vector3 destination;
        float afterMotionTime = ((MoveSkillTemplate)skill.template).afterMotionTime;
        float moveTime = (skill.castTime - afterMotionTime - skill.invokeTime);
        float remaining = Mathf.Max(0, skill.CastTimeRemaining() - afterMotionTime);
        float speed = Mathf.Min(((MoveSkillTemplate)skill.template).distance.Get(skill.level), Vector3.Distance(moveTargetPos, startPosition)) / moveTime;
        if (((MoveSkillTemplate)skill.template).type.Equals(OrbitType.Straight))
            destination = moveVector * speed * Time.deltaTime;
        else if (((MoveSkillTemplate)skill.template).type.Equals(OrbitType.Curve))
        {
            float jumpHeight = 2.5f;
            float angle = Mathf.Min(180f - ((remaining / moveTime) * 180f), 180f);
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            caster.moveMount.position = new Vector3(caster.moveMount.position.x,
                                                    jumpHeight * sin,
                                                    caster.moveMount.position.z);
            destination = moveVector * speed * Time.deltaTime;
            if (angle >= 180) skill.finish = true;
        }
        else
        {
            destination = moveTargetPos - caster.transform.position;
            skill.finish = true;
        }
        caster.LookAtY(caster.transform.position + destination.normalized);
        if (skill.CastTimeRemaining() <= afterMotionTime)
        {
            caster.agent.enabled = true;
            return skill;
        }
        if (ignoreObject)
            caster.transform.position = caster.transform.position + destination;
        else {
            caster.agent.Warp(caster.transform.position + destination);
            //caster.agent.Move(destination);
            //caster.agent.destination = caster.agent.nextPosition;
        }
        if (skill.finish) caster.agent.enabled = true;
        return skill;
    }
}
