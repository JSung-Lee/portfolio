
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Buff
{ 
    public string name;

    public int level;
    public double buffTimeEnd;

    public int buffStack;

    public float buffsShield;

    public BuffSkillTemplate template;

    public Entity caster;

    public BuffSkillEffect effect;

    private const float DAMAGE_TIMER_MAX = 1f;
    float stayTimer = DAMAGE_TIMER_MAX;


    public Buff(BuffSkillTemplate _template, int level, Entity _caster, int buffStack = 1)
    {
        template = _template;
        name = _template.name;
        this.level = level;
        this.buffStack = buffStack;
        buffTimeEnd = Time.time + _template.buffTime.Get(level);
        buffsShield = template.GetBuffsShield(_caster, level) * buffStack;
        caster = _caster;
        this.effect = null;
    }
        
    public Sprite image { get { return template.image; } }
    public float buffTime { get { return template.buffTime.Get(level); } }
    public int buffsHealthMax { get { return (int)(template.buffsHealthMax.Get(level) * template.buffsStackValue.Get(buffStack)); } }
    public float buffsHealthRegeneration { get { return template.buffsHealthRegeneration.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsHealthMaxPerRegeneration { get { return template.buffsHealthMaxPerRegeneration.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public int buffsManaMax { get { return (int)(template.buffsManaMax.Get(level) * template.buffsStackValue.Get(buffStack)); } }
    public float buffsManaRegeneration { get { return template.buffsManaRegeneration.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsManaMaxPerRegeneration { get { return template.buffsManaMaxPerRegeneration.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public int buffsAttackDamage { get { return (int)(template.buffsAttackDamage.Get(level) * template.buffsStackValue.Get(buffStack)); } }
    public int buffsAbilityPower { get { return (int)(template.buffsAbilityPower.Get(level) * template.buffsStackValue.Get(buffStack)); } }
    public int buffsArmor { get { return (int)(template.buffsArmor.Get(level) * template.buffsStackValue.Get(buffStack)); } }
    public int buffsMagicResist { get { return (int)(template.buffsMagicResist.Get(level) * template.buffsStackValue.Get(buffStack)); } }
    public float buffsCriticalChance { get { return template.buffsCriticalChance.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsCriticalDamage { get { return template.buffsCriticalDamage.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsAttackSpeed { get { return template.buffsAttackSpeed.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsMoveSpeed { get { return template.buffsMoveSpeed.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsCooldown { get { return template.buffsCooldown.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public int buffsReviveCount { get { return (int)(template.buffsReviveCount.Get(level) * template.buffsStackValue.Get(buffStack)); } }

    public float buffsAbsorption { get { return template.buffsAbsorption.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsDamaged { get { return template.buffsDamaged.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsManaPerConsumption { get { return template.buffsManaPerConsumption.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public float buffsCastRange { get { return template.buffsCastRange.Get(level) * template.buffsStackValue.Get(buffStack); } }
    public DamageType dotDamageType { get { return template.dotDamageType; } }
    public bool buffsStun { get { return template.buffsStun; } }
    public bool buffsSilence { get { return template.buffsSilence; } }
    public int buffsMaxStack { get { return template.buffsMaxStack.Get(level); } }
    public bool decreaseBuffStack { get { return template.decreaseBuffStack; }}//버프 스택이 감소하는형식인가(아니면 스택이 몇개든 걍 꺼짐)
    public bool resetTimeAddBuffStack { get { return template.resetTimeAddBuffStack; } }

    public void setBuffEffect(BuffSkillEffect _effect) {
        this.effect = _effect;
    }
    public string ToolTip()
    {
        StringBuilder tip = new StringBuilder(template.ToolTip(level));

        return tip.ToString();
    }
    public string ToolTip(Entity caster)
    {
        StringBuilder tip = new StringBuilder(template.ToolTip(caster, level));

        return tip.ToString();
    }

    public bool BuffDotDamageTimeUpdate()
    {
        stayTimer -= Time.deltaTime;

        //Debug.Log("Buff Timer : " + stayTimer);
        if (stayTimer <= 0)
        {
            stayTimer = DAMAGE_TIMER_MAX;
            return true;
        }
        return false;
    }

    public float BuffTimeRemaining()
    {
        return Time.time >= buffTimeEnd ? 0 : (float)(buffTimeEnd - Time.time);
    }
    public static Buff operator+(Buff b1, Buff b2)
    {
        float shieldGap = (b1.template.GetBuffsShield(b1.caster, b1.level) * b1.template.buffsStackValue.Get(b1.buffStack)) - b1.buffsShield;
        b1.buffStack = Mathf.Min(b1.buffStack + b2.buffStack, b1.buffsMaxStack);
        if(b1.resetTimeAddBuffStack)
            b1.buffTimeEnd = b2.buffTimeEnd;
        float shield = (b1.template.GetBuffsShield(b1.caster, b1.level) * b1.template.buffsStackValue.Get(b1.buffStack));
        b1.buffsShield = shield - shieldGap;
        return b1;
    }
}