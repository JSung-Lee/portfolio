using UnityEngine;
using System.Text;
using UnityEditor;

[CreateAssetMenu(menuName = "Skill/Target Damage", order = 999)]
public class TargetDamageSkillTemplate : SkillTemplate
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

    [Header("Hit Effect")]
    public EffectMount afterEffectTargetMount;
    public SkillEffect afterEffect;

    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        if (target == null) return !haveToTarget;
        if (target.team != caster.team) return true;
        return false;
    }
    public override bool CheckTarget(Entity caster)
    {
        return (caster.target != null || !haveToTarget) && 
               caster.CanAttack(caster.target);
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    {
        if (caster.target != null)
        {
            var skillCastRange = addUseCastRange ? castRange.Get(skillLevel) + caster.addCastRange : castRange.Get(skillLevel);

            destination = caster.target.collider.ClosestPointOnBounds(caster.transform.position);
            return Utils.ClosestDistance(caster.collider, caster.target.collider) <= skillCastRange;
        }
        destination = caster.targetPos;
        return false;
    }

    public override void Apply(Entity caster, int skillLevel)
    {
        SpawnVisualEffect(caster, skillLevel);
        float ad = (AttackDamage.Get(skillLevel) + (caster.attackDamage * ADRatio.Get(skillLevel)));
        ad += caster.healthMax * casterHealthMaxPerDamage.Get(skillLevel);
        ad += caster.health * casterCurrentHealthPerDamage.Get(skillLevel);
        ad += (caster.healthMax - caster.health) * casterLostHealthPerDamage.Get(skillLevel);
        ad += caster.target.health * targetCurrentHealthPerDamage.Get(skillLevel);
        ad += caster.target.healthMax * targetHealthMaxPerDamage.Get(skillLevel);
        ad += (caster.target.healthMax - caster.target.health) * targetLostHealthPerDamage.Get(skillLevel);
        float ap = (AbilityPower.Get(skillLevel) + (caster.abilityPower * APRatio.Get(skillLevel)));
        float td = trueDamage.Get(skillLevel);
        caster.DealDamageAt(caster.target, ad, ap, td, true, name, true);


        if(afterEffect)
            CreateAfterEffect(caster);


        if (buffSkillTemplateHittedTarget)
        {
            if (caster && caster.target)
                caster.target.AddOrRefreshBuff(new Buff(buffSkillTemplateHittedTarget, skillLevel, caster));
        }
        if(buffSkillTemplateSelf)
        {
            if (caster)
                caster.AddOrRefreshBuff(new Buff(buffSkillTemplateSelf, skillLevel, caster));
        }
    }
    // 임시 메소드 생성(kdh)
    public void CreateAfterEffect(Entity caster)
    {
        Vector3 targetDistance = (caster.targetPos - caster.transform.position);
        Vector3 targetForDirection = targetDistance / targetDistance.magnitude;

        //Vector3 movePos = Random.Range();
        // moveEffect x : 가로 지름, y : 높이, z : 세로 지름
        Vector3 effectPos = caster.target.GetEffectMount(afterEffectTargetMount).position 
                                    - (targetForDirection * Random.Range(0, (caster.target.collider.bounds.size.x/2)));

        GameObject go = Instantiate(afterEffect.gameObject, effectPos, Quaternion.identity);
        var effect = go.GetComponent<VisualSkillEffect>();
        
        if (effect.followEffectMount) 
            go.transform.parent = caster.target.GetEffectMount(afterEffectTargetMount);

        effect.targetPos = caster.target.transform.position;
        effect.caster = caster;
       
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

        return tip.ToString();
    }

    public override string ToolTip(Entity caster, int skillLevel, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(base.ToolTip(caster, skillLevel, showRequirements));

        tip.Replace("{TOTAL_AD}", (AttackDamage.Get(skillLevel) + (caster.attackDamage * ADRatio.Get(skillLevel)) + trueDamage.Get(skillLevel)).ToString());
        tip.Replace("{ADRATIO}", (caster.attackDamage * ADRatio.Get(skillLevel)).ToString());

        tip.Replace("{TOTAL_AP}", (AbilityPower.Get(skillLevel) + (caster.abilityPower * APRatio.Get(skillLevel)) + trueDamage.Get(skillLevel)).ToString());
        tip.Replace("{APRATIO}", (caster.abilityPower * APRatio.Get(skillLevel)).ToString());

        tip.Replace("{CASTERHEALTHMAXPERDAMAGE}", (caster.healthMax * casterHealthMaxPerDamage.Get(skillLevel)).ToString());
        tip.Replace("{CASTERCURRENTHEALTHPERDAMAGE}", (caster.health * casterCurrentHealthPerDamage.Get(skillLevel)).ToString());
        tip.Replace("{CASTERLOSTHEALTHPERDAMAGE}", ((caster.healthMax - caster.health) * casterLostHealthPerDamage.Get(skillLevel)).ToString());

        return tip.ToString();
    }

    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}
