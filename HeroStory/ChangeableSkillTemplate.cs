using UnityEngine;
using System.Text;
using UnityEditor;

[CreateAssetMenu(menuName = "Skill/Changeable Skill", order = 999)]
public class ChangeableSkillTemplate : SkillTemplate
{
    [Header("Template To be Changed After Using SKill")]
    public int[] changeSkillIndex; //변신
    public bool useFixedChangeIndex = false;
    public int[] fixedChangeIndex;
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
        for (int i = 0; i < changeSkillIndex.Length; i++)
        {
            int index = changeSkillIndex[i];
            var skill = caster.skills[index];
            if (useFixedChangeIndex)
                skill.NextIndex(fixedChangeIndex[i]);
            else
                skill.NextIndex();
            caster.skills[index] = skill;
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
        return caster.team;
    }
}
