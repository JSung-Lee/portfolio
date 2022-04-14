using System.Text;
using UnityEngine;

public abstract class BuffSkillTemplate : SkillTemplate {
    
    [Header("Buffs Status")]
    public LevelBasedFloat buffTime = new LevelBasedFloat{baseValue=60};

    public LevelBasedInt buffsHealthMax;
    public LevelBasedFloat buffsHealthRegeneration;
    public LevelBasedFloat buffsHealthMaxPerRegeneration;
    public LevelBasedInt buffsManaMax;
    public LevelBasedFloat buffsManaRegeneration;
    public LevelBasedFloat buffsManaMaxPerRegeneration;
    public LevelBasedFloat buffsManaPerConsumption;
    public LevelBasedInt buffsAttackDamage;
    public LevelBasedInt buffsAbilityPower;
    public LevelBasedInt buffsArmor;
    public LevelBasedInt buffsMagicResist;
    public LevelBasedFloat buffsCriticalChance;
    public LevelBasedFloat buffsCriticalDamage;
    public LevelBasedFloat buffsAttackSpeed;
    public LevelBasedFloat buffsMoveSpeed;
    public LevelBasedFloat buffsCooldown;

    public LevelBasedFloat buffsCastRange;


    public LevelBasedFloat buffsAbsorption;
    public LevelBasedFloat buffsShield;
    public LevelBasedFloat buffsShieldCurrentHealth;
    public LevelBasedFloat buffsShieldHealthMax;
    public LevelBasedFloat buffsShieldLostHealth;

    public LevelBasedFloat buffsShieldCurrentMana;
    public LevelBasedFloat buffsShieldManaMax;
    public LevelBasedFloat buffsShieldLostMana;

    public LevelBasedFloat buffsItemCoolDown;

    public LevelBasedInt buffsReviveCount;


    [Header("Dot Damage")]
    public DamageType dotDamageType;
    public LevelBasedFloat buffsDamaged;
    public float dotDamageTime = 1f;

    public bool buffsStun;                                                          // buffTime이 5초면 그 bufftrue가  5초동안
    public bool buffsSilence;                                                       // 침묵
    public LevelBasedInt buffsMaxStack = new LevelBasedInt { baseValue = 1 };
    public LevelBasedFloat buffsStackValue = new LevelBasedFloat { baseValue = 1 }; // 스택당 버프값 증가
    public bool decreaseBuffStack = true;                                           // 이게 점점 줄어드는지
    public bool resetTimeAddBuffStack = false;                                      // 다시 썼을때 지속시간 초기화 하는지

    [Header("Buff Occupied Effect")]
    public BuffSkillEffect effect;

    public float GetBuffsShield(Entity caster, int skillLevel)
    {
        float maxHealthBonus = buffsShieldHealthMax.Get(skillLevel) * caster.healthMax;
        float lostHealthBonus = buffsShieldLostHealth.Get(skillLevel) * (caster.healthMax - caster.health);
        float currentHealthBonus = buffsShieldCurrentHealth.Get(skillLevel) * caster.health;

        float maxManaBonus = buffsShieldManaMax.Get(skillLevel) * caster.manaMax;
        float currentmanaBonus = buffsShieldCurrentMana.Get(skillLevel) * caster.mana;
        float lostManaBonus = buffsShieldLostMana.Get(skillLevel) * (caster.manaMax - caster.mana);

        return buffsShield.Get(skillLevel) + maxHealthBonus + lostHealthBonus   + currentHealthBonus
                                           + maxManaBonus   + currentmanaBonus  + lostManaBonus;
    }

    public void SpawnEffect(Entity caster, Entity spawnTarget) {
        if (effect != null) {
            // 이펙트 중복일 경우 이펙트 생성 X
            for(int i=0; i < spawnTarget.buffs.Count; i++)
            {
                if (spawnTarget.buffs[i].effect == null)
                {
                    if (spawnTarget.buffs[i].template.name == this.name)
                    {
                        GameObject go = Instantiate(effect.gameObject, spawnTarget.transform.position, Quaternion.identity);
                        var buffEffect = go.GetComponent<BuffSkillEffect>();

                        buffEffect.caster = caster;
                        buffEffect.target = spawnTarget;
                        buffEffect.buffName = name;

                        if (buffEffect.useLifeTime){
                            buffEffect.TimerClean();
                            buffEffect.lifeTime = buffTime.baseValue;
                        }

                        spawnTarget.buffs[i].setBuffEffect(buffEffect);
                    }
                }
                else
                {
                    if (spawnTarget.buffs[i].name == this.name)
                    {
                        var buffEffect = spawnTarget.buffs[i].effect;

                        if (buffEffect.useLifeTime)
                        {
                            buffEffect.lastRemainingTime = Mathf.Infinity;
                            buffEffect.TimerClean();
                            buffEffect.lifeTime = buffTime.baseValue;
                        }

                        Debug.Log("Buff Name Update: " + spawnTarget.buffs[i].name);
                    }
                }
            }
        }
    }

    public override string ToolTip(int skillLevel, bool showRequirements = false) {
        StringBuilder tip = new StringBuilder(base.ToolTip(skillLevel, showRequirements));
        tip.Replace("{BUFFTIME}", Utils.PrettyTime(buffTime.Get(skillLevel)));
        tip.Replace("{BUFFREPEATTIME}", Utils.PrettyTime(buffTime.Get(skillLevel)));
   
        tip.Replace("{BUFFSHEALTHREGENERATION}", buffsHealthRegeneration.Get(skillLevel).ToString());
        tip.Replace("{BUFFSHEALTHREGENERATION_TIME}", (buffsHealthRegeneration.Get(skillLevel) * buffTime.Get(skillLevel)).ToString());
        tip.Replace("{BUFFSHEALTHMAXPERREGENERATION}", Mathf.RoundToInt(buffsHealthMaxPerRegeneration.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSMANAMAX}", buffsManaMax.Get(skillLevel).ToString());
        tip.Replace("{BUFFSMANAREGENERATION}", buffsManaRegeneration.Get(skillLevel).ToString());
        tip.Replace("{BUFFSMANAREGENERATION_TIME}", (buffsManaRegeneration.Get(skillLevel) * buffTime.Get(skillLevel)).ToString());
        tip.Replace("{BUFFSMANAMAXPERREGENERATION}", Mathf.RoundToInt(buffsManaMaxPerRegeneration.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSMANAPERCONSUMPTION}", (buffsManaPerConsumption.Get(skillLevel) * 100).ToString());

        tip.Replace("{BUFFSATTACKDAMAGE}", buffsAttackDamage.Get(skillLevel).ToString());
        tip.Replace("{BUFFSABILITYPOWER}", buffsAbilityPower.Get(skillLevel).ToString());
        tip.Replace("{BUFFSARMOR}", buffsArmor.Get(skillLevel).ToString());
        tip.Replace("{BUFFSCRITICALCHANCE}", (buffsCriticalChance.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSCRITICALDAMAGE}", (buffsCriticalDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSATTACKSPEED}", (buffsAttackSpeed.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSMOVESPEED}", (buffsMoveSpeed.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSCOOLDOWN}", (buffsCooldown.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSABSORPTION}", (buffsAbsorption.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELD}", buffsShield.Get(skillLevel).ToString());

        tip.Replace("{BUFFSCASTRANGE}", buffsCastRange.Get(skillLevel).ToString());

        tip.Replace("{DOTDAMAGETYPE}", dotDamageType.ToString());
        tip.Replace("{BUFFSDAMAGED}", buffsDamaged.Get(skillLevel).ToString());

        tip.Replace("{BUFFSSHIELDCURRENTHEALTH_PER}", (buffsShieldCurrentHealth.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELDHEALTHMAX_PER}", (buffsShieldHealthMax.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELDLOSTHEALTH_PER}", (buffsShieldLostHealth.Get(skillLevel) * 100).ToString());

        tip.Replace("{BUFFSSHIELDCURRENTMANA_PER}", (buffsShieldCurrentMana.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELDMANAMAX_PER}", (buffsShieldManaMax.Get(skillLevel) * 100).ToString());
        tip.Replace("{BUFFSSHIELDLOSTMANA_PER}", (buffsShieldLostMana.Get(skillLevel) * 100).ToString());


        tip.Replace("{BUFFSCASTRANGE}", buffsCastRange.Get(skillLevel).ToString());

        return tip.ToString();
    }

    public override string ToolTip(Entity caster, int skillLevel, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(base.ToolTip(caster, skillLevel, showRequirements));
        
        tip.Replace("{BUFFSSHIELDCURRENTHEALTH}", (caster.health * buffsShieldCurrentHealth.Get(skillLevel)).ToString());        
        tip.Replace("{BUFFSSHIELDHEALTHMAX}", (caster.healthMax * buffsShieldHealthMax.Get(skillLevel)).ToString());
        tip.Replace("{BUFFSSHIELDLOSTHEALTH}", ((caster.healthMax - caster.health) * buffsShieldLostHealth.Get(skillLevel)).ToString());

        tip.Replace("{BUFFSSHIELDCURRENTMANA}", (caster.mana * buffsShieldCurrentMana.Get(skillLevel)).ToString());
        tip.Replace("{BUFFSSHIELDMANAMAX}", (caster.manaMax * buffsShieldManaMax.Get(skillLevel)).ToString());
        tip.Replace("{BUFFSSHIELDLOSTMANA}", ((caster.manaMax - caster.mana) * buffsShieldLostMana.Get(skillLevel)).ToString());

        return tip.ToString();
    }


    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team;
    }
}
