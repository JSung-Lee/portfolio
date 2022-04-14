using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "Skill/Usable Passive Skill", order = 999)]
public class UsablePassiveSkillTemplate : PassiveSkillTemplate
{
    [Header("Template To be Used When Used Skill")]
    public SkillTemplate usableSkillTemplate; //패시브 스킬인데 스킬 쓸 수 있는거 기본 지속효과, 스킬 사용 가능한거.

    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        return usableSkillTemplate.CorrectedTarget(caster, target);
    }
    public override bool CheckTarget(Entity caster)
    {
        return usableSkillTemplate.CheckTarget(caster);
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    {
        usableSkillTemplate.CheckDistance(caster, skillLevel, out destination);
        return true;
    }

    public override void Apply(Entity caster, int skillLevel)
    {
        usableSkillTemplate.Apply(caster, skillLevel);
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
    }
    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}