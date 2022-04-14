using UnityEngine;
using UnityEditor;
using System.Text;

[CreateAssetMenu(menuName = "Skill/Summon Skill", order = 999)]
public class SummonSkillTemplate : SkillTemplate
{
    [Header("Summon Entity")]
    public Entity[] summonEntitys;
    public MinMaxInt[] summonCounts;
    public SkillEffect[] summonEffects;
    public LevelBasedInt[] summonLevels;

    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        if (target == null) return !haveToTarget;
        return true;
    }

    public override bool CheckTarget(Entity caster)
    {
        return true;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    {
        if (haveToTarget) {
            if (!caster.target) {
                destination = caster.targetPos;
                return false;
            }
            caster.targetPos = caster.target.transform.position;
        }
        if (canUseSkillFarAway)
        {
            if (Vector3.Distance(caster.transform.position, caster.targetPos) > castRange.Get(skillLevel))
            {
                Vector3 vec = (caster.targetPos - caster.transform.position).normalized;
                caster.targetPos = caster.transform.position + (vec * castRange.Get(skillLevel));
            }
            destination = caster.targetPos;
            return true;
        }
        destination = caster.targetPos;
        return Vector3.Distance(caster.transform.position, destination) <= castRange.Get(skillLevel);
    }

    public override void Apply(Entity caster, int skillLevel)
    {
        SpawnVisualEffect(caster, skillLevel);
        int ignore = 0;
        for (int i = 0; i < summonEntitys.Length; i++)
        {
            int count = summonCounts[i].Get();
            for (int c = 0; c < count; c++)
            {
                Vector3 summonPos = Extensions.FindEmptySpace(caster.targetPos, summonEntitys[i].agent.radius * 3f, ignore);
                if (summonPos == Vector3.zero) return;
                GameObject go = Instantiate(summonEntitys[i].gameObject);
                Extensions.SetTeamLayerInAllChildren(go.transform, caster.gameObject.layer);
                Entity entity = go.GetComponent<Entity>();
                entity.transform.position = summonPos;
                entity.level = summonLevels[i].Get(skillLevel);
                entity.team = caster.team;
                ignore++;
                if(i < summonEffects.Length)
                    Instantiate(summonEffects[i].gameObject, summonPos, summonEffects[i].transform.rotation);
            }
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

    public override string ToolTip(int skillLevel, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(base.ToolTip(skillLevel, showRequirements));

        return tip.ToString();
    }

    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}