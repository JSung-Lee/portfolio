using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "Skill/Target Buff", order = 999)]
public class TargetBuffSkillTemplate : BuffSkillTemplate {

    [Header("Can Guff Target")]
    public bool canBuffSelf = true;
    public bool canBuffEnemies = false;

    public override bool CorrectedTarget(Entity caster, Entity target) {
        if (immediatelyCasting) return canBuffSelf;
        if (target == null) return !haveToTarget;
        if (canBuffSelf && target == caster) return true;
        if (canBuffEnemies && target.team != caster.team) return true;
        return false;
    }

    public override bool CheckTarget(Entity caster) {
        if (immediatelyCasting) return canBuffSelf;
        return caster.target != null && caster.target.health > 0;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination) {
        if (immediatelyCasting && canBuffSelf) caster.target = caster;
        if (caster.target != null) {
            var skillCastRange = addUseCastRange ? castRange.Get(skillLevel) + caster.addCastRange : castRange.Get(skillLevel);

            destination = caster.target.collider.ClosestPointOnBounds(caster.transform.position);
            return Utils.ClosestDistance(caster.collider, caster.target.collider) <= skillCastRange;
        }
        destination = caster.targetPos;
        return false;
    }

    public override void Apply(Entity caster, int skillLevel) {
        SpawnVisualEffect(caster, skillLevel);
        if (caster.target != null && caster.target.health > 0) {
            caster.target.AddOrRefreshBuff(new Buff(this, skillLevel, caster));
            SpawnEffect(caster, caster.target);
        }
    }
    public override void RepeatApply(Entity caster, int skillLevel)
    {
        if (caster.target != null && caster.target.health > 0)
        {
            caster.target.AddOrRefreshBuff(new Buff(this, skillLevel, caster));
            SpawnEffect(caster, caster.target);
        }
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
        if (canBuffSelf)
            return caster.team;
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }

}
