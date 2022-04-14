using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [Header("Reward")]
    [SerializeField] public LevelBasedInt _goldMin;
    [SerializeField] public LevelBasedInt _goldMax;
    public int gold {
        get {
            int min = _goldMin.Get(level);
            return UnityEngine.Random.Range(min, Mathf.Max(min, _goldMax.Get(level)) + 1);
        }
    }

    [SerializeField] public LevelBasedInt _expMin;
    [SerializeField] public LevelBasedInt _expMax;
    public int exp { 
        get {
            int min = _expMin.Get(level);
            Debug.Log(min);
            return UnityEngine.Random.Range(min, Mathf.Max(min, _expMax.Get(level)) + 1);
        }
    }

    [Header("AI")]
    public EntityAI ai;

    protected override void Start()
    {
        base.Start();
        if (!ai) ai = GetComponent<EntityAI>();
    }

    protected override void Update()
    {
        base.Update();
        Debug.DrawLine(transform.position, targetPos);
        Debug.DrawLine(transform.position, agent.destination, Color.red);
    }

    bool EventSkillRequest()
    {
        // 최근 스킬을 사용했다면 True, 스킬이 없다면 False
        return 0 <= currentSkill && currentSkill < skills.Count;
    }
    bool EventSkillInvokeFinished()
    {
        return 0 <= currentSkill && currentSkill < skills.Count &&
               skills[currentSkill].InvokeTimeRemaining() == 0 &&
               !skills[currentSkill].invoked;
    }
    bool EventSkillCastFinished()
    {
        return 0 <= currentSkill && currentSkill < skills.Count &&
               skills[currentSkill].CastTimeRemaining() == 0;
    }
    bool EventTargetTooFarToAttack()
    {
        if (currentSkill < 0 || currentSkill >= skills.Count) return false;
        if (skills[currentSkill].CooldownRemaining() > 0) return false;
        if (ai && ai.isKittingNow) return false;
        Vector3 destination;
        return target != null &&
               0 <= currentSkill && currentSkill < skills.Count &&
               !CastCheckDistance(skills[currentSkill], out destination);
    }
    bool EventPatrol()
    {
        if (ai) {
            return ai.usePatrol && ai.patrolRadius > 0 && ai.waitTimer <= 0 && target == null;
        }
        return false;
    }
    bool EventKitting()
    {
        if (ai)
        {
            if(EventHaveCanUseSkill() == -1)
                return target != null && ai.useKitting && ai.kittingWaitTimer <= 0;
        }
        return false;
    }
    int EventHaveCanUseSkill()
    {
        if (ai)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                var skill = skills[i];
                if (CastCheckSelf(skill) && CastCheckTarget(skill))
                    return i;
            }
        }
        return -1;
    }
    bool EventSearching()
    {
        if(ai)
        {
            return target == null && ai.useSearching;
        }
        return false;
    }
    public float CurrentCastRange()
    {
        return 0 <= currentSkill && currentSkill < skills.Count ? skills[currentSkill].castRange : skills[normalAttackIndex].castRange;
    }
    public override void OnAggroEnter(Entity entity)
    {
        if (entity && entity.health <= 0) return;
        if ((state == State.Idle || state == State.Move) && currentSkill == -1 && target == null)
        {
            int skillIndex = normalAttackIndex;
            if (ai) skillIndex = ai.GetNextSkillSelect();
            skillIndex = skillIndex == -1 ? normalAttackIndex : skillIndex;
            //Debug.Log(skillIndex);
            if (CastCheckSelf(skills[skillIndex])) 
                UseSkill(skillIndex);
            target = entity;
        }
    }

    public override void OnAggroStay(Entity entity)
    {
        OnAggroEnter(entity);
    }

    public override void UpdateState_Idle()
    {
        base.UpdateState_Idle();

        if (EventSkillRequest())                // 스킬을 사용하기전 스킬이 있는지 체크?
        {
            // 현재 스킬을 변수에 저장
            var skill = skills[currentSkill];
            //Debug.Log("SkillRequest : " + skill.name);
            // 현재 스킬을 사용할 수 있는지 확인
            // CastCheckSelf : 현재 스킬 사용 여부 판단(마나, 쿨타임, 생존 여부)
            // CastCheckTarget : 현재 스킬 사용을 사용할 수 있는 대상 여부 판단
            if (CastCheckSelf(skill) && CastCheckTarget(skill))
            {
                if (ai && ai.useSlowRotate) {
                    if (!ai.finishedRotate) {
                        if (target) targetPos = target.transform.position;
                        LookAtY(targetPos, ai.slowRotateSpeed);
                        ai.finishedRotate = true;
                        return;
                    }
                }
                else if (target) targetPos = target.transform.position;
                Vector3 destination;
                if (CastCheckDistance(skill, out destination))
                {
                    agent.ResetPath();
                    if (ai && ai.useSlowRotate) {
                        if (!ai.finishedRotate) {
                            LookAtY(targetPos, ai.slowRotateSpeed);
                            ai.finishedRotate = true;
                            return;
                        }
                    }
                    else 
                        LookAtY(target.transform.position);
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
                    skills[currentSkill] = skill;
                    if (skill.visualEffect_Begin != null)
                    {
                        GameObject go = Instantiate(skill.visualEffect_Begin.gameObject, GetEffectMount(skill.effectMount).position, GetEffectMount(skill.effectMount).rotation);
                        var _effect = go.GetComponent<VisualSkillEffect>();
                        if (_effect.followEffectMount)
                        {
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
                    agent.stoppingDistance = skill.castRange;
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
                            if (Vector3.Distance(destination, transform.position) <= skill.castRange) return;
                            agent.stoppingDistance = skill.castRange;
                            agent.destination = agent.NearestValidDestination(destination);
                            //agent.SetDestination(agent.NearestValidDestination( destination));
                            state = State.Move;
                        }
                    }
                }
                else
                {
                    nextSkill = currentSkill;
                    state = State.Idle;
                    return;
                }
            }
        }
        if(EventPatrol())
        {
            if (EventSearching())
            {
                target = ai.GetEntitySuitableTarget();
                if (target) {
                    int skillIndex = normalAttackIndex;
                    if (ai) skillIndex = ai.GetNextSkillSelect();
                    skillIndex = skillIndex == -1 ? normalAttackIndex : skillIndex;
                    if (CastCheckSelf(skills[skillIndex]))
                        UseSkill(skillIndex);
                    return;
                }
            }
            MoveTo(ai.GetNextPatrolPosition());
            ai.waitTimer = ai.patrolWaitTime;
            state = State.Move;
            return;
        }
        if (EventKitting())
        {
            MoveTo(ai.GetNextKittingPosition(target.transform.position));
            ai.isKittingNow = true;
            ai.kittingWaitTimer = ai.kittingWaitTime;
            state = State.Move;
            return;
        }
        if(ai && target)
        {
            int skillIndex = ai.GetNextSkillSelect();
            if(skillIndex >= 0)
            UseSkill(skillIndex);
        }
    }
     
    public override void UpdateState_Move()
    {
        base.UpdateState_Move();
        if (EventMoveEnd())
        {
            currentSkill = -1;
            state = State.Idle;
            if (ai) ai.isKittingNow = false;
            return;
        }
        if (EventTargetTooFarToAttack())
        {
            agent.stoppingDistance = CurrentCastRange() * 0.8f;
            agent.destination = target.collider.ClosestPointOnBounds(transform.position);
            state = State.Move;
            return;
        }
        if (EventSkillRequest())
        {
            var skill = skills[currentSkill];
            if (CastCheckSelf(skill) && CastCheckTarget(skill))
            {
                if (ai && ai.useSlowRotate) {
                    if (!ai.finishedRotate) {
                        if (target && skill.haveToTarget) targetPos = target.transform.position;
                        LookAtY(targetPos, ai.slowRotateSpeed);
                        ai.finishedRotate = true;
                        return;
                    }
                }
                else if (target && skill.haveToTarget) targetPos = target.transform.position;
                Vector3 destination;
                if (CastCheckDistance(skill, out destination))
                {
                    agent.destination = transform.position;
                    agent.ResetPath();
                    if (ai && ai.useSlowRotate) {
                        if (!ai.finishedRotate) {
                            LookAtY(targetPos, ai.slowRotateSpeed);
                            ai.finishedRotate = true;
                            return;
                        }
                    }
                    else
                        LookAtY(target.transform.position);
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
                    skills[currentSkill] = skill;
                    if (skill.visualEffect_Begin != null)
                    {
                        GameObject go = Instantiate(skill.visualEffect_Begin.gameObject, GetEffectMount(skill.effectMount).position, GetEffectMount(skill.effectMount).rotation);
                        var _effect = go.GetComponent<VisualSkillEffect>();
                        if (_effect.followEffectMount)
                        {
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
                    agent.stoppingDistance = skill.castRange;
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
                            if (Vector3.Distance(destination, transform.position) <= skill.castRange) return;
                            agent.stoppingDistance = skill.castRange;
                            agent.destination = agent.NearestValidDestination(destination);
                            //agent.SetDestination(agent.NearestValidDestination( destination));
                            state = State.Move;
                        }
                    }
                }
                else
                {
                    currentSkill = -1;
                    state = State.Idle;
                    return;
                }
            }
        }
    }

    // 스킬 캐스팅 여부 상태 
    public override void UpdateState_Casting()
    {
        Debug.DrawLine(transform.position + Vector3.up, transform.position + (transform.forward * 10) + Vector3.up, Color.red);
        base.UpdateState_Casting();
        if (EventTargetDied())
        {
            Skill skill = skills[currentSkill];
            if (skill.cancelCastIfTargetDied)
            {
                currentSkill = nextSkill = -1;
                target = null;
                state = State.Idle;
            }
        }
        if (EventMoveSkill())
        {
            Skill skill = skills[currentSkill];
            if (skill.invoked && !skill.finish)
            {
                var moveEffect = GetComponentInChildren<MoveSkillEffect>();
                if (moveEffect) skill = moveEffect.MoveEntity(skill);
                skills[currentSkill] = skill;
            }
        }
        else {
            agent.SetDestination(transform.position);
        }
        if (EventSkillCastFinished())
        {
            Skill skill = skills[currentSkill];
            if (!skill.invoked)
                InvokeSkill(skill);
            CastSkill(skill);
            currentSkill = skill.followupDefaultAttack ? 0 : -1;
            state = State.Idle;
            if (ai && ai.useSlowRotate) ai.finishedRotate = false;
            return;
        }

        if (EventSkillInvokeFinished())
        {
            Skill skill = skills[currentSkill];
            InvokeSkill(skill);
            state = State.Casting;
            if (ai && ai.useSlowRotate) ai.finishedRotate = false;
            return;
        }

    }

    public override void UpdateState_Dead()
    {
        base.UpdateState_Dead();
    }

    private void LateUpdate()
    {
        // 애니메이터 파라미터 전달
        animator.SetBool("Walk", state == State.Move);
        animator.SetBool("Casting", state == State.Casting);
        animator.SetBool("Stun", state == State.Stun);
        animator.SetBool("Silence", state == State.Silence);
        // 스킬 목록을 가져와 해당 스킬 Entity와 이름을 비교하여 캐스팅 시간에 따라 시전
        //foreach (Skill skill in skills)
        //    if (skill.learned)
        //        animator.SetBool(skill.name, skill.CastTimeRemaining() > 0);
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].learned)
                animator.SetBool(skills[i].name, skills[i].CastTimeRemaining() > 0 && i == currentSkill);
        }
        animator.SetBool("Dead", state == State.Dead);
    }

    public override void MoveTo(Vector3 destination, float stopDistance = 0)
    {
        if (state == State.Dead || state == State.Stun) return;
        agent.NearestValidDestination(destination);
        agent.SetDestination(destination);
        agent.stoppingDistance = stopDistance;
        currentSkill = -1;
        state = State.Move;
    }
    public void TeleportTo(Vector3 destination, float stopDistance = 0)
    {
        if (state == State.Dead || state == State.Stun) return;

        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.isStopped = true;

        transform.position = destination;
        agent.Warp(destination);
        agent.nextPosition = destination;
        //agent.ResetPath();

        // 에이전트 목적지 설정
        agent.SetDestination(destination);

        agent.enabled = true;

        agent.updatePosition = true;
        agent.updateRotation = true;


        currentSkill = -1;
        state = State.Move;
    }

    public void UseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skills.Count) return;
        if (currentSkill == -1)
        {
            currentSkill = skillIndex;
            var skill = skills[currentSkill];
            skill.invoked = false;
            skill.finish = false;
            skills[currentSkill] = skill;
        }
        else
        {
            nextSkill = currentSkill;
        }
    }

}
