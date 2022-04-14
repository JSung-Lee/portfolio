using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Skill/Area Buff", order = 999)]
public class AreaBuffSkillTemplate : BuffSkillTemplate {

    [Header("Can Guff Target")]
    public bool canBuffTeam = true;
    public bool canBuffEnemies = false;

    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        if (target == null) return !haveToTarget;
        if (canBuffTeam && target.team == caster.team) return true;
        if (canBuffEnemies && target.team != caster.team) return true;
        return false;
    }

    public override bool CheckTarget(Entity caster) {
        return true;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination) {
        destination = caster.transform.position;
        return true;
    }
    public override void Apply(Entity caster, int skillLevel) {
        SpawnVisualEffect(caster, skillLevel);
        int layerMask = (LayerMask.GetMask("Monster")) | (LayerMask.GetMask("Player"));

        var skillCastRange = addUseCastRange ? castRange.Get(skillLevel) + caster.addCastRange : castRange.Get(skillLevel);

        Collider[] colliders = Physics.OverlapSphere(caster.transform.position, skillCastRange, layerMask);

        foreach (Collider co in colliders) {

            Entity entity = co.GetComponent<Entity>();

            if (entity != null) {
                bool sameTeam = entity.team == caster.team;
                if ((canBuffTeam && sameTeam) || (canBuffEnemies && !sameTeam)) {
                    if (entity.health > 0) 
                    {
                        entity.AddOrRefreshBuff(new Buff(this, skillLevel, caster));
                        SpawnEffect(caster, entity);
                    }
                }
            }
        }
    }

    public override void SpawnVisualEffect(Entity caster, int skillLevel)
    {
        if (visualEffect_Invoke != null)
        {
            GameObject go = Instantiate(visualEffect_Invoke.gameObject, caster.GetEffectMount(effectMount).position, caster.GetEffectMount(effectMount).rotation);
            var _effect = go.GetComponent<VisualSkillEffect>();
            if (_effect.followEffectMount) go.transform.parent = caster.GetEffectMount(effectMount);
            if (effectMount == EffectMount.BothHands) {
                GameObject goRight = Instantiate(visualEffect_Invoke.gameObject, caster.GetEffectMount(EffectMount.RightHand).position, caster.GetEffectMount(EffectMount.RightHand).rotation);
                var _effectRight = goRight.GetComponent<VisualSkillEffect>();
                if (_effectRight.followEffectMount) goRight.transform.parent = caster.GetEffectMount(EffectMount.RightHand);
            }
        }
        else
            Debug.LogWarning("visualEffect_Invoke is Null");
    }

    public override void RepeatApply(Entity caster, int skillLevel)
    {
        Apply(caster, skillLevel);
    }

    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        if (canBuffTeam)
            return caster.team;
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}
