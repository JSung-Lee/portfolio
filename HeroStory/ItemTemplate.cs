using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 999)]
public class ItemTemplate : ScriptableObject
{
    [Header("Base Stats")]
    public ItemType itemType;
    public ItemGrade itemGrade;
    public int maxStack;
    public int buyPrice;
    public int sellPrice;
    public int mergePrice;
    public ItemTemplate[] mergeTemplate;
    [TextArea(1, 30)] public string toolTip;
    public string itemname;
    public Sprite image;    
    public Sprite backImage;
    public Sprite frame;
    public Sprite wrapper;

    [Header("Usage Boosts")]
    public bool usageDestroy;
    public SkillTemplate usageSkill;

    [Header("Equipment Boosts")]
    public int equipHealthMaxBonus;
    public float equipHealthRegenBonus;
    public int equipManaMaxBonus;
    public float equipManaRegenBonus;
    [Range(0, 1)] public float equipManaPerConsumptions;
    public int equipAttackDamageBonus;
    public int equipAbilityPowerBonus;
    public int equipArmorBonus;
    public int equipMagicResistBonus;
    [Range(0, 1)] public float equipCriticalChanceBonus;
    public float equipCriticalDamageBonus;
    public float equipMoveSpeedBonus;
    [Range(0, 10)] public float equipAttackSpeedBonus;
    [Range(0, 1)] public float equipCooldownBonus;
    public float equipAbsorptionBonus;
    public int equipShieldBonus;
    public float equipGainGoldBonus;
    public float equipGainExpBonus;
}
