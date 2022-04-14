using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "Skill/Passive Skill", order = 999)]
public class PassiveSkillTemplate : BuffSkillTemplate
{
    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        return true;
    }
    public override bool CheckTarget(Entity caster)
    {
        return true;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    {
        destination = caster.transform.position;
        return true;
    }

    public override void Apply(Entity caster, int skillLevel)
    {
        SpawnVisualEffect(caster, skillLevel);
        if (caster != null && caster.health > 0)
        {
            caster.AddOrRefreshBuff(new Buff(this, skillLevel, caster));
            SpawnEffect(caster, caster);
        }
    }
    public override void RepeatApply(Entity caster, int skillLevel)
    {
        if (caster != null && caster.health > 0)
        {
            caster.AddOrRefreshBuff(new Buff(this, skillLevel, caster));
            SpawnEffect(caster, caster);
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
        return caster.team;
    }
}
