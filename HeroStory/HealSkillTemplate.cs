using System.Text;
using UnityEngine;

public abstract class HealSkillTemplate : SkillTemplate {
    [Header("Status To be Healed")]
    public LevelBasedInt healsHealth; //얘 쓸꺼면 버프나 쓰셈
    public LevelBasedInt healsMana;

    [Header("Effect To be Healed Target")]
    public OneTimeTargetSkillEffect effect;

    public void SpawnEffect(Entity caster, Entity spawnTarget) {
        if (effect != null) {
            GameObject go = Instantiate(effect.gameObject, spawnTarget.transform.position, Quaternion.identity);
            go.GetComponent<OneTimeTargetSkillEffect>().caster = caster;
            go.GetComponent<OneTimeTargetSkillEffect>().target = spawnTarget;
        }
    }

    public override string ToolTip(int skillLevel, bool showRequirements = false) {
        StringBuilder tip = new StringBuilder(base.ToolTip(skillLevel, showRequirements));
        tip.Replace("{HEALSHEALTH}", healsHealth.Get(skillLevel).ToString());
        tip.Replace("{HEALSMANA}", healsMana.Get(skillLevel).ToString());
        return tip.ToString();
    }
    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team;
    }
}
