using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct Skill
{
    public List<SkillTemplate> templates;

    public bool learned;
    public int level;
    public double castTimeEnd;
    public double invokeTimeEnd;
    public double cooldownEnd;
    public double repeatEnd;
    public bool invoked;
    public bool finish;
    public bool isOn;
    public int index;

    public Skill(SkillTemplate _template)
    {
        templates = new List<SkillTemplate>();
        if (_template)
        {
            templates.Add(_template);
            for (int i = 0; i < _template.connectedSkills.Length; i++)
                templates.Add(_template.connectedSkills[i]);
            learned = _template.learnDefault;
        }
        else
            learned = false;
        level = 1;
        index = 0;
        invoked = finish = false;
        isOn = false;
        invokeTimeEnd = castTimeEnd = cooldownEnd = repeatEnd = Time.time;        
    }
    public string name { get { return templates[index].name; } }
    public SkillTemplate template { get { return templates.Count > 0 ? templates[index] : null; } }
    public float castTime { get { return templates[index].castTime.Get(level); } }
    public float invokeTime { get { return templates[index].invokeTime.Get(level); } }
    public float cooldown { get { return templates[index].cooldown.Get(level); } }
    public float castRange { get { return templates[index].castRange.Get(level); } }
    public bool addUseCastRange { get { return templates[index].addUseCastRange; } }

    public float manaCosts { get { return templates[index].manaCosts.Get(level); } }

    public float repeatManaCosts { get { return templates[index].RepeatManaCosts.Get(level); } }
    public bool followupDefaultAttack { get { return templates[index].followupDefaultAttack; } }
    public Sprite image { get { return templates[index].image; } }
    public string skillname { get { return template.skillname; } }
    public bool learnDefault { get { return templates[index].learnDefault; } }
    public bool cancelCastIfTargetDied { get { return templates[index].cancelCastIfTargetDied; } }
    public bool canCancelDuringCasting { get { return templates[index].canCancelDuringCasting; } }
    public bool isNormalAttack { get { return templates[index].isNormalAttack; } }
    public bool immediatelyCasting { get { return templates[index].immediatelyCasting; } }
    public bool haveToTarget { get { return templates[index].haveToTarget; } }
    public bool canUseSkillFarAway { get { return templates[index].canUseSkillFarAway; } }
    public bool onOff { get { return templates[index].onOff; } }
    public bool showSelector { get { return templates[index].showSelector; } }
    public int maxLevel { get { return templates[index].maxLevel; } }
    public int requiredLevel { get { return templates[index].requiredLevel.Get(1); } }
    public int upgradeRequiredLevel { get { return templates[index].requiredLevel.Get(level + 1); } }
    public float repeatTime { get { return templates[index].repeatTime.Get(level); } }
    public SkillEffect visualEffect_Begin { get { return templates[index].visualEffect_Begin; } }
    public SkillEffect visualEffect_Invoke { get { return templates[index].visualEffect_Invoke; } }
    public SkillEffect visualEffect_End { get { return templates[index].visualEffect_End; } }
    public EffectMount effectMount { get { return templates[index].effectMount; } }
    public bool CorrectedTarget(Entity caster, Entity target) { return templates[index].CorrectedTarget(caster, target); }
    public bool CheckTarget(Entity caster) { return templates[index].CheckTarget(caster); }
    public bool CheckDistance(Entity caster, out Vector3 destination) { return templates[index].CheckDistance(caster, level, out destination); }
    public void Apply(Entity caster) { templates[index].Apply(caster, level); }
    public void RepeatApply(Entity caster) { templates[index].RepeatApply(caster, level); }
    public bool isSpecialCondition { get { return templates[index].isSpecialCondition; } }
    public SkillConditionType skillConditionType { get { return templates[index].skillConditionType; } }
    public SkillCondition skillCondition { get { return templates[index].skillCondition; } }
    public SkillTarget skillTarget { get { return templates[index].skillTarget; } }

    public string ToolTip()
    {
        StringBuilder tip = new StringBuilder(templates[index].ToolTip(level, !learned));
        
        if (learned && level < maxLevel)
            tip.Replace("{UPGRADELEVEL}", upgradeRequiredLevel.ToString());
        //tip.Append("\n<b><i>Upgrade Required Level: " + upgradeRequiredLevel + "</i></b>\n");

        return tip.ToString();
    }

    public string ToolTip(Entity caster)
    {
        StringBuilder tip = new StringBuilder(templates[index].ToolTip(caster, level, !learned));

        if (learned && level < maxLevel)
            tip.Replace("{UPGRADELEVEL}", upgradeRequiredLevel.ToString());
        //tip.Append("\n<b><i>Upgrade Required Level: " + upgradeRequiredLevel + "</i></b>\n");

        return tip.ToString();
    }


    public float CastTimeRemaining()
    {
        return Time.time >= castTimeEnd ? 0 : (float)(castTimeEnd - Time.time);
    }

    public float InvokeTimeRemaining()
    {
        return Time.time >= invokeTimeEnd ? 0 : (float)(invokeTimeEnd - Time.time);
    }

    public bool IsCasting()
    {
        return CastTimeRemaining() > 0;
    }

    public float CooldownRemaining()
    {
        return Time.time >= cooldownEnd ? 0 : (float)(cooldownEnd - Time.time);
    }

    public bool IsReady()
    {
        return CooldownRemaining() == 0;
    }

    public bool IsRepeatReady()
    {
        float repeatRemaining = Mathf.Max(0,(float)(repeatEnd - Time.time));
        return repeatRemaining == 0;
    }

    public int NextIndex()
    {
        index += 1;
        if (templates.Count <= index)
            index = 0;
        return index;
    }
    public int NextIndex(int _index)
    {
        index = _index;
        if (templates.Count <= index)
            index = 0;
        return index;
    }
    public Team GetTeamThisSkillTarget(Entity caster)
    {
        return template.GetTeamThisSkillTarget(caster);
    }
}