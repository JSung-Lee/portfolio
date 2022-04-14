using UnityEngine;
using UnityEditor;
using System.Text;

public enum OrbitType
{
    Straight, Curve, Teleport
}

[CreateAssetMenu(menuName = "Skill/Move Skill", order = 999)]
public class MoveSkillTemplate : SkillTemplate
{
    [Header("Skill Damage")]
    public LevelBasedFloat AttackDamage = new LevelBasedFloat { baseValue = 1 };
    public LevelBasedFloat ADRatio = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat AbilityPower = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat APRatio = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterHealthMaxPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterCurrentHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterLostHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat targetHealthMaxPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat targetCurrentHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat targetLostHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat trueDamage = new LevelBasedFloat { baseValue = 0 };

    public LevelBasedFloat casterManaMaxPerAbilityPower = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterCurrentManaPerAbilityPower = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterLostManaPerAbilityPower = new LevelBasedFloat { baseValue = 0 };

    [Header("Move Skill Projectile")]
    public MoveSkillEffect projectile; //이동 스킬(투사체)의 콜라이더

    [Header("Move Skill Orbit")]
    public LevelBasedFloat distance = new LevelBasedFloat { baseValue = 20f };
    public OrbitType type; //궤도 타입 (직선 곡선 등등)

    [Header("Move Skill Detail Option")]
    public bool keepAfterHit = false; //맞고 난 후에도 유지가 되는가  관통 스킬
    public bool ignoreObject = false; //오브젝트 무시 (벽뚫기)
    public bool fixedDistance = false; //고정된 사거리 (짧게해도 지정거리 이동)
    public float afterMotionTime = 0f; //스킬이 캐스팅되고나서 움직이지 않는 모션의 시간


    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        if (target == null) return !haveToTarget;
        if (target.team != caster.team) return true;
        return false;
    }

    public override bool CheckTarget(Entity caster)
    {
        return true;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    {
        var skillCastRange = addUseCastRange ? castRange.Get(skillLevel) + caster.addCastRange : castRange.Get(skillLevel);


        if (canUseSkillFarAway)
        {
            if (fixedDistance || Vector3.Distance(caster.transform.position, caster.targetPos) > skillCastRange)
            {
                Vector3 vec = (caster.targetPos - caster.transform.position).normalized;
                caster.targetPos = caster.transform.position + (vec * distance.Get(skillLevel));
            }
            destination = caster.targetPos;
            destination = caster.targetPos = caster.agent.NearestValidDestination(destination);
            return true;
        }
        else if (fixedDistance)
        {
            if (Vector3.Distance(caster.transform.position, caster.targetPos) <= skillCastRange)
            {
                Vector3 vec = (caster.targetPos - caster.transform.position).normalized;
                caster.targetPos = caster.transform.position + (vec * distance.Get(skillLevel));
            }
        }
        destination = caster.targetPos;

        destination = caster.targetPos = caster.agent.NearestValidDestination(destination);
        return Vector3.Distance(caster.transform.position, destination) <= skillCastRange;
    }

    public override void Apply(Entity caster, int skillLevel)
    {
        if(projectile)
        {
            GameObject go = Instantiate(projectile.gameObject, caster.transform);
            MoveSkillEffect effect = go.GetComponent<MoveSkillEffect>();
            if (effect.yisZero) effect.transform.SetYPosition(0);
            effect.target = caster.target;
            effect.caster = caster;
            float ad = (AttackDamage.Get(skillLevel) + (caster.attackDamage * ADRatio.Get(skillLevel)));
            ad += caster.healthMax * casterHealthMaxPerDamage.Get(skillLevel);
            ad += caster.health * casterCurrentHealthPerDamage.Get(skillLevel);
            ad += (caster.healthMax - caster.health) * casterLostHealthPerDamage.Get(skillLevel);
            float ap = (AbilityPower.Get(skillLevel) + (caster.abilityPower * APRatio.Get(skillLevel)));
            ap += caster.manaMax * casterManaMaxPerAbilityPower.Get(skillLevel);
            ap += caster.mana * casterCurrentManaPerAbilityPower.Get(skillLevel);
            ap += (caster.manaMax - caster.mana) * casterLostManaPerAbilityPower.Get(skillLevel);

            float td = trueDamage.Get(skillLevel);
            effect.attackDamage = ad;
            effect.abilityPower = ap;
            effect.trueDamage = td;
            effect.currentHealthPerDamage = targetCurrentHealthPerDamage.Get(skillLevel);
            effect.healthMaxPerDamage = targetHealthMaxPerDamage.Get(skillLevel);
            effect.lostHealthPerDamage = targetLostHealthPerDamage.Get(skillLevel);
            effect.keepAfterHit = keepAfterHit;
            effect.ignoreObject = ignoreObject;
            if (buffSkillTemplateHittedTarget) {
                effect.buffSkillTemplate = buffSkillTemplateHittedTarget;
                effect.buffSkillLevel = skillLevel;
            }
            if (buffSkillTemplateSelf)
            {
                if (caster)
                    caster.AddOrRefreshBuff(new Buff(buffSkillTemplateSelf, skillLevel, caster));
            }
        }
        SpawnVisualEffect(caster, skillLevel);
        if (ignoreObject)
            caster.collider.isTrigger = true;
    }

    public override void RepeatApply(Entity caster, int skillLevel)
    {
        Apply(caster, skillLevel);
    }
    public override void SpawnVisualEffect(Entity caster, int skillLevel)
    {
        if (visualEffect_Invoke != null)
        {
            GameObject go = Instantiate(visualEffect_Invoke.gameObject, caster.GetEffectMount(effectMount).position, caster.GetEffectMount(effectMount).rotation);
            var _effect = go.GetComponent<VisualSkillEffect>();
            if (_effect.followEffectMount) go.transform.parent = caster.GetEffectMount(effectMount);
            if (effectMount == EffectMount.BothHands)
            {
                GameObject goRight = Instantiate(visualEffect_Invoke.gameObject, caster.GetEffectMount(EffectMount.RightHand).position, caster.GetEffectMount(EffectMount.RightHand).rotation);
                var _effectRight = goRight.GetComponent<VisualSkillEffect>();
                if (_effectRight.followEffectMount) goRight.transform.parent = caster.GetEffectMount(EffectMount.RightHand);
            }
        }
        else
            Debug.LogWarning("visualEffect_Invoke is Null");
    }

    public override string ToolTip(int skillLevel, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(base.ToolTip(skillLevel, showRequirements));

        tip.Replace("{ATTACKDAMAGE}", AttackDamage.Get(skillLevel).ToString());
        tip.Replace("{ADRATIO_PER}", (ADRatio.Get(skillLevel) * 100).ToString());

        tip.Replace("{ABILITYPOWER}", AbilityPower.Get(skillLevel).ToString());
        tip.Replace("{APRATIO_PER}", (APRatio.Get(skillLevel) * 100).ToString());

        tip.Replace("{TRUEDAMAGE}", trueDamage.Get(skillLevel).ToString());

        tip.Replace("{TARGETCURRENTHEALTHPERDAMAGE}", (targetCurrentHealthPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{TARGETHEALTHMAXPERDAMAGE}", (targetHealthMaxPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{TARGETLOSTHEALTHPERDAMAGE}", (targetLostHealthPerDamage.Get(skillLevel) * 100).ToString());

        tip.Replace("{CASTERHEALTHMAXPERDAMAGE_PER}", (casterHealthMaxPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERCURRENTHEALTHPERDAMAGE_PER}", (casterCurrentHealthPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERLOSTHEALTHPERDAMAGE_PER}", (casterLostHealthPerDamage.Get(skillLevel) * 100).ToString());

        tip.Replace("{CASTERMANAMAXPERABILITYPOWER_PER}", (casterManaMaxPerAbilityPower.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERCURRENTMANAPERABILITYPOWER_PER}", (casterCurrentManaPerAbilityPower.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERLOSTMANAPERABILITYPOWER_PER}", (casterLostManaPerAbilityPower.Get(skillLevel) * 100).ToString());

        tip.Replace("{BUFFSATTACKDAMAGE}", buffSkillTemplateSelf.buffsAttackDamage.Get(skillLevel).ToString());
        tip.Replace("{BUFFSKILLSELFBUFFTIME}", Utils.PrettyTime( buffSkillTemplateSelf.buffTime.Get(skillLevel)));
        return tip.ToString();
    }

    public override string ToolTip(Entity caster, int skillLevel, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(base.ToolTip(caster, skillLevel, showRequirements));

        tip.Replace("{TOTAL_AD}", (AttackDamage.Get(skillLevel) + (caster.attackDamage * ADRatio.Get(skillLevel)) + trueDamage.Get(skillLevel)).ToString());
        tip.Replace("{ADRATIO}", (caster.attackDamage * ADRatio.Get(skillLevel)).ToString());

        tip.Replace("{TOTAL_AP}", (AbilityPower.Get(skillLevel) + (caster.abilityPower * APRatio.Get(skillLevel)) + trueDamage.Get(skillLevel)).ToString());
        tip.Replace("{APRATIO}", (caster.abilityPower * APRatio.Get(skillLevel)).ToString());

        tip.Replace("{ADRATIO}", (caster.attackDamage * ADRatio.Get(skillLevel)).ToString());
        tip.Replace("{APRATIO}", (caster.abilityPower * APRatio.Get(skillLevel)).ToString());
        tip.Replace("{CASTERHEALTHMAXPERDAMAGE}", (caster.healthMax * casterHealthMaxPerDamage.Get(skillLevel)).ToString());
        tip.Replace("{CASTERCURRENTHEALTHPERDAMAGE}", (caster.health * casterCurrentHealthPerDamage.Get(skillLevel)).ToString());
        tip.Replace("{CASTERLOSTHEALTHPERDAMAGE}", ((caster.healthMax - caster.healthMax) * casterLostHealthPerDamage.Get(skillLevel)).ToString());

        tip.Replace("{CASTERMANAMAXPERABILITYPOWER_PER}", (casterManaMaxPerAbilityPower.Get(skillLevel)).ToString());
        tip.Replace("{CASTERCURRENTMANAPERABILITYPOWER_PER}", (casterCurrentManaPerAbilityPower.Get(skillLevel)).ToString());
        tip.Replace("{CASTERLOSTMANAPERABILITYPOWER_PER}", (casterLostManaPerAbilityPower.Get(skillLevel)).ToString());

        return tip.ToString();
    }


    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}