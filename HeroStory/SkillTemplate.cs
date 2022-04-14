using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using UnityEditor;


public abstract partial class SkillTemplate : ScriptableObject {
    [Header("Info")]
    public bool followupDefaultAttack; //스킬 후 기본공격이 자동으로 나가는가
    [SerializeField, TextArea(1, 30)] protected string toolTip;
    public Sprite image;
    public string skillname;
    public bool learnDefault;            //이 스킬을 기본으로 배웠는가?
    public bool cancelCastIfTargetDied;  //타겟이 죽었을때 스킬 캐스팅을 취소할건가
    public bool canCancelDuringCasting;  //캐스팅 중에 취소가 가능한가
    public bool isNormalAttack;          //이 스킬은 기본공격인가
    public bool immediatelyCasting;      //즉시 시전
    public bool haveToTarget;            //타겟이 있어야만 사용이 가능하다
    public bool canUseSkillFarAway;      //멀어도 스킬을 쓸 수 있다
    public bool onOff = false;           //껏다켰다 하는건가
    public bool showSelector = true;     //스킬을 쓸때 타겟 선택이 떠야하는가 <-> 베인 구르기
    public bool addUseCastRange = false; // 사거리가 변경 될 수 있는가

    [Header("Special Condition")]
    public bool isSpecialCondition = false;
    public SkillConditionType skillConditionType = SkillConditionType.Self;           // 사용될 스킬(현재 스킬, 버프 타겟, 버프 셀트)
    public SkillCondition skillCondition = SkillCondition.None;
    public SkillTarget skillTarget = SkillTarget.None;

    [Header("Learn Requirements per Skill Level")]
    public LevelBasedInt requiredLevel;

    [Header("Properties per Skill Level")]
    public int maxLevel = 1;
    public LevelBasedFloat manaCosts;
    public LevelBasedFloat RepeatManaCosts; //마나를 지속적으로 쓰는거
    public LevelBasedFloat castTime;
    public LevelBasedFloat invokeTime; // 총 스킬이 사용하는 시간이 1초라 가정할 때 몇초에 나가게 할건가
    public LevelBasedFloat repeatTime; // on/off 스킬이 도는 시간 카서스 w 틱 데미지 간격
    public LevelBasedFloat cooldown;
    public LevelBasedFloat castRange;

    [Header("Connected Skill Template")]
    public SkillTemplate[] connectedSkills; // 제이스 엘리스 이런애들 폼 변환

    [Header("Buff Skill Template Use This Skill On Hitted Target")]
    public BuffSkillTemplate buffSkillTemplateHittedTarget;

    [Header("Buff Skill Template Use This Skill On Self")]
    public BuffSkillTemplate buffSkillTemplateSelf;

    [Header("Buff Skill Template Use This Special Condition Skill")]
    public BuffSkillTemplate buffSkillTemplateConditionSkill;

    [Header("Visual Effect")]
    public SkillEffect visualEffect_Begin;
    public SkillEffect visualEffect_Invoke;
    public SkillEffect visualEffect_End;
    public EffectMount effectMount;

    public abstract bool CorrectedTarget(Entity caster, Entity target);

    public abstract bool CheckTarget(Entity caster);

    public abstract bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination);
    
    public abstract void Apply(Entity caster, int skillLevel);
    public abstract void SpawnVisualEffect(Entity caster, int skillLevel);
    public abstract void RepeatApply(Entity caster, int skillLevel);
    public abstract Team GetTeamThisSkillTarget(Entity caster);

    
    public virtual string ToolTip(int level, bool showRequirements = false) {
        StringBuilder tip = new StringBuilder(toolTip);
        tip.Replace("{NAME}", name);
        tip.Replace("{SKILLNAME}", skillname);
        tip.Replace("{LEVEL}", level.ToString());
        tip.Replace("{CASTTIME}", Utils.PrettyTime(castTime.Get(level)));
        tip.Replace("{COOLDOWN}", Utils.PrettyTime(cooldown.Get(level)));
        tip.Replace("{CASTRANGE}", castRange.Get(level).ToString());
        tip.Replace("{MANACOSTS}", manaCosts.Get(level).ToString());
        tip.Replace("{REPEATMANACOST}", RepeatManaCosts.Get(level).ToString());
        tip.Replace("{REPEATTIME}", Utils.PrettyTime(repeatTime.Get(level)));

        return tip.ToString();
    }
    

    public virtual string ToolTip(Entity caster, int level, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(ToolTip(level, showRequirements));
        

        return tip.ToString();
    }
    
}