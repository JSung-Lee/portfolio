using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

[CreateAssetMenu(menuName = "Skill/Auto-Target Skill", order = 999)]
public class AutoTargetSkillTemplate : SkillTemplate
{
    [Header("Skill To be Used After Auto-Targeting")]
    public SkillTemplate template;

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
        template.Apply(caster, skillLevel);
    }
    public override void RepeatApply(Entity caster, int skillLevel)
    {
        Apply(caster, skillLevel);
    }
    public override void SpawnVisualEffect(Entity caster, int skillLevel)
    {
        
    }
    public override string ToolTip(int skillLevel, bool showRequirements = false)
    {
        return template.ToolTip(skillLevel, showRequirements);
    }

    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}
