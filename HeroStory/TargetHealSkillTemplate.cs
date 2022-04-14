using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Target Heal", order = 999)]
public class TargetHealSkillTemplate : HealSkillTemplate {

    [Header("Can Heal Target")]
    public bool canHealSelf = true;

    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        if (target == null) return !haveToTarget;
        if (canHealSelf && target == caster) return true;
        if (target.team != caster.team) return true;
        return false;
    }

    public override bool CheckTarget(Entity caster) {
        return caster.target != null && caster.target.health > 0;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination) {
        if (caster.target != null) {
            var skillCastRange = addUseCastRange ? castRange.Get(skillLevel) + caster.addCastRange : castRange.Get(skillLevel);

            destination = caster.target.collider.ClosestPointOnBounds(caster.transform.position);
            return Utils.ClosestDistance(caster.collider, caster.target.collider) <= skillCastRange;
        }
        destination = caster.transform.position;
        return false;
    }

    public override void Apply(Entity caster, int skillLevel) {
        SpawnVisualEffect(caster, skillLevel);
        if (caster.target != null && caster.target.health > 0) {
            caster.target.health += healsHealth.Get(skillLevel);
            caster.target.mana += healsMana.Get(skillLevel);

            SpawnEffect(caster, caster.target);
        }
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

    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        if (canHealSelf)
            return caster.team;
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}
