using System.Text;
using UnityEngine;

[System.Serializable]
public struct Item
{
    public string name;

    public bool valid;
    public int amount;
    public int checkId;

    public Skill skill;
    public ItemTemplate template;


    public Item(ItemTemplate _template, int _amount = 1)
    {
        template = _template;
        name = _template.name;
        amount = _amount;
        checkId = 0;
        valid = true;
        skill = new Skill(_template.usageSkill);
    }

    public bool TemplateExists()
    {
        return name != null && template;
    }
    public ItemType itemType
    {
        get { return template.itemType; }
    }
    public ItemGrade itemGrade
    {
        get { return template.itemGrade; }
    }
    public int maxStack
    {
        get { return template.maxStack; }
    }
    public int buyPrice
    {
        get { return template.buyPrice; }
    }
    public int sellPrice
    {
        get { return template.sellPrice; }
    }
    public int mergePrice
    {
        get { return template.mergePrice; }
    }
    public int GetMergeItemCount()
    {
        return template.mergeTemplate.Length;
    }
    public ItemTemplate GetMergeTemplate(int index)
    {
        return template.mergeTemplate[index];
    }
    public bool IsNeedThisItemOnMerge(ItemTemplate _template)
    {
        for (int i = 0; i < GetMergeItemCount(); i++)
            if (GetMergeTemplate(i) == _template) return true;
        return false;
    }
    public string itemname
    {
        get { return template.itemname; }
    }
    public Sprite image
    {
        get { return template.image; }
    }
    public Sprite frame
    {
        get { return template.frame; }
    }
    public Sprite backImage
    {
        get { return template.backImage; }
    }
    public Sprite wrapper
    {
        get { return template.wrapper; }
    }
    public bool usageDestroy
    {
        get { return template.usageDestroy; }
    }
    public SkillTemplate usageSkill
    {
        get { return template.usageSkill; }
    }
    public int equipHealthMaxBonus
    {
        get { return template.equipHealthMaxBonus; }
    }
    public float equipHealthRegenBonus
    {
        get { return template.equipHealthRegenBonus; }
    }
    public int equipManaMaxBonus
    {
        get { return template.equipManaMaxBonus; }
    }
    public float equipManaRegenBonus
    {
        get { return template.equipManaRegenBonus; }
    }
    public float equipManaPerConsumptions
    {
        get { return template.equipManaPerConsumptions; }
    }
    public int equipAttackDamageBonus
    {
        get { return template.equipAttackDamageBonus; }
    }
    public int equipAbilityPowerBonus
    {
        get { return template.equipAbilityPowerBonus; }
    }
    public int equipArmorBonus
    {
        get { return template.equipArmorBonus; }
    }
    public int equipMagicResistBonus
    {
        get { return template.equipMagicResistBonus; }
    }
    public float equipCriticalChanceBonus
    {
        get { return template.equipCriticalChanceBonus; }
    }
    public float equipCriticalDamageBonus
    {
        get { return template.equipCriticalDamageBonus; }
    }
    public float equipMoveSpeedBonus
    {
        get { return template.equipMoveSpeedBonus; }
    }
    public float equipAttackSpeedBonus
    {
        get { return template.equipAttackSpeedBonus; }
    }
    public float equipCooldownBonus
    {
        get { return template.equipCooldownBonus; }
    }

    public float equipAbsorptionBonus
    {
        get { return template.equipAbsorptionBonus; }
    }
    public int equipShieldBonus
    {
        get { return template.equipShieldBonus; }
    }
    public float equipGainGoldBonus
    {
        get { return template.equipGainGoldBonus; }
    }

    public float equipGainExpBonus
    {
        get { return template.equipGainExpBonus; }
    }

    public string ToolTip()
    {
        var tip = new StringBuilder(template.toolTip);
        tip.Replace("{NAME}", name);
        tip.Replace("{ITEMTYPE}", itemType.ToString());
        tip.Replace("{ITEMGRADE}", itemGrade.ToString());
        tip.Replace("{EQUIPHEALTHMAXBONUS}", equipHealthMaxBonus.ToString());
        tip.Replace("{EQUIPHEALTHREGENBONUS}", Mathf.RoundToInt(equipHealthRegenBonus * 100).ToString());
        tip.Replace("{EQUIPMANAMAXBONUS}", equipManaMaxBonus.ToString());
        tip.Replace("{EQUIPMANAREGENBONUS}", Mathf.RoundToInt(equipManaRegenBonus * 100).ToString());
        tip.Replace("{EQUIPMANAPERCONSUMPTIONS}", Mathf.RoundToInt(equipManaPerConsumptions * 100).ToString());
        tip.Replace("{EQUIPATTACKDAMAGEBONUS}", equipAttackDamageBonus.ToString());
        tip.Replace("{EQUIPABILITYPOWERBONUS}", equipAbilityPowerBonus.ToString());
        tip.Replace("{EQUIPARMORBONUS}", equipArmorBonus.ToString());
        tip.Replace("{EQUIPMAGICRESISTBONUS}", equipMagicResistBonus.ToString());
        tip.Replace("{EQUIPCRITICALDAMAGEBONUS}", Mathf.RoundToInt(equipCriticalDamageBonus * 100).ToString());
        tip.Replace("{EQUIPCRITICALCHANCEBONUS}", Mathf.RoundToInt(equipCriticalChanceBonus * 100).ToString());
        tip.Replace("{EQUIPMOVESPEEDBONUS}", Mathf.RoundToInt(equipMoveSpeedBonus * 100).ToString());
        tip.Replace("{EQUIPATTACKSPEEDBONUS}", Mathf.RoundToInt(equipAttackSpeedBonus * 100).ToString());
        tip.Replace("{EQUIPCOOLDOWNBONUS}", Mathf.RoundToInt(equipCooldownBonus * 100).ToString());
        tip.Replace("{EQUIPABSORPTIONBONUS}", Mathf.RoundToInt(equipAbsorptionBonus * 100).ToString("f1"));
        tip.Replace("{EQUIPSHIELDBONUS}", equipShieldBonus.ToString());
        tip.Replace("{EQUIPGAINGOLDBONUS}", Mathf.RoundToInt(equipGainGoldBonus * 100).ToString());
        tip.Replace("{BUYPRICE}", buyPrice.ToString());
        tip.Replace("{SELLPRICE}", sellPrice.ToString());
        tip.Replace("{AMOUNT}", amount.ToString());

        var skillLevel = 1;
        if (template.usageSkill)
        {
            tip.Replace("{NAME}", name);
            tip.Replace("{SKILLNAME}", template.usageSkill.skillname);
            tip.Replace("{CASTTIME}", Utils.PrettyTime(template.usageSkill.castTime.Get(skillLevel)));
            tip.Replace("{COOLDOWN}", Utils.PrettyTime(template.usageSkill.cooldown.Get(skillLevel)));
            tip.Replace("{CASTRANGE}", template.usageSkill.castRange.Get(skillLevel).ToString());
            tip.Replace("{MANACOSTS}", template.usageSkill.manaCosts.Get(skillLevel).ToString());
            tip.Replace("{REPEATMANACOST}", template.usageSkill.RepeatManaCosts.Get(skillLevel).ToString());
            tip.Replace("{REPEATTIME}", Utils.PrettyTime(template.usageSkill.repeatTime.Get(skillLevel)));
        }
        
        
        if(template.usageSkill is TargetDamageSkillTemplate)
        {
            TargetDamageSkillTemplate damage = (TargetDamageSkillTemplate)template.usageSkill;
            tip.Replace("{ATTACKDAMAGE}", damage.AttackDamage.Get(skillLevel).ToString());
            tip.Replace("{ABILITYPOWER}", damage.AbilityPower.Get(skillLevel).ToString());
            tip.Replace("{TRUEDAMAGE}", damage.trueDamage.Get(skillLevel).ToString());
            tip.Replace("{TARGETCURRENTHEALTHPERDAMAGE}", (damage.targetCurrentHealthPerDamage.Get(skillLevel) * 100).ToString());
            tip.Replace("{TARGETHEALTHMAXPERDAMAGE}", (damage.targetHealthMaxPerDamage.Get(skillLevel) * 100).ToString());
            tip.Replace("{TARGETLOSTHEALTHPERDAMAGE}", (damage.targetLostHealthPerDamage.Get(skillLevel) * 100).ToString());
            tip.Replace("{CASTERHEALTHMAXPERDAMAGE_PER}", (damage.casterHealthMaxPerDamage.Get(skillLevel) * 100).ToString());
            tip.Replace("{CASTERCURRENTHEALTHPERDAMAGE_PER}", (damage.casterCurrentHealthPerDamage.Get(skillLevel) * 100).ToString());
            tip.Replace("{CASTERLOSTHEALTHPERDAMAGE_PER}", (damage.casterLostHealthPerDamage.Get(skillLevel) * 100).ToString());
        }
        else if (template.usageSkill is BuffSkillTemplate) 
        {
            BuffSkillTemplate buffs = (BuffSkillTemplate)template.usageSkill;

            TipReplace_Buff(tip, buffs, skillLevel);
        }
        else if (template.usageSkill is AreaSkillTemplate)
        {
            AreaSkillTemplate projectile = (AreaSkillTemplate)template.usageSkill;

            if (projectile.projectile.buffSkillTemplate)
            {
                var buff = projectile.projectile.buffSkillTemplate;
                TipReplace_Buff(tip, buff, skillLevel);
            }


        }

        return tip.ToString();
    }


    public void TipReplace_Buff(StringBuilder tip, BuffSkillTemplate buffs, int skillLevel)
    {
        tip.Replace("{BUFFTIME}", Utils.PrettyTime(buffs.buffTime.Get(skillLevel)).ToString());
        tip.Replace("{BUFFREPEATTIME}", Utils.PrettyTime(buffs.buffTime.Get(skillLevel)));
        tip.Replace("{BUFFSHEALTHMAX}", buffs.buffsHealthMax.Get(skillLevel).ToString());
        tip.Replace("{BUFFSHEALTHREGENERATION}", (buffs.buffsHealthRegeneration.Get(skillLevel) * buffs.buffTime.Get(skillLevel)).ToString());
        tip.Replace("{BUFFSHEALTHMAXPERREGENERATION}", Mathf.RoundToInt(buffs.buffsHealthMaxPerRegeneration.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSMANAMAX}", buffs.buffsManaMax.Get(skillLevel).ToString());
        tip.Replace("{BUFFSMANAREGENERATION}", (buffs.buffsManaRegeneration.Get(skillLevel) * buffs.buffTime.Get(skillLevel)).ToString());
        tip.Replace("{BUFFSMANAMAXPERREGENERATION}", Mathf.RoundToInt(buffs.buffsManaMaxPerRegeneration.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSMANAPERCONSUMPTION}", Mathf.RoundToInt(buffs.buffsManaPerConsumption.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSATTACKDAMAGE}", buffs.buffsAttackDamage.Get(skillLevel).ToString());
        tip.Replace("{BUFFSABILITYPOWER}", buffs.buffsAbilityPower.Get(skillLevel).ToString());
        tip.Replace("{BUFFSARMOR}", buffs.buffsArmor.Get(skillLevel).ToString());
        tip.Replace("{BUFFSCRITICALCHANCE}", (buffs.buffsCriticalChance.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSCRITICALDAMAGE}", (buffs.buffsCriticalDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSATTACKSPEED}", (buffs.buffsAttackSpeed.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSMOVESPEED}", (buffs.buffsMoveSpeed.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSCOOLDOWN}", (buffs.buffsCooldown.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSABSORPTION}", (buffs.buffsAbsorption.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELD}", buffs.buffsShield.Get(skillLevel).ToString());

        tip.Replace("{BUFFSSHIELDCURRENTHEALTH_PER}", (buffs.buffsShieldCurrentHealth.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELDHEALTHMAX_PER}", (buffs.buffsShieldHealthMax.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELDLOSTHEALTH_PER}", (buffs.buffsShieldLostHealth.Get(skillLevel) * 100).ToString());

        tip.Replace("{BUFFSCASTRANGE}", buffs.buffsCastRange.Get(skillLevel).ToString());

    }
}
