using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BookCharacterStatus : MonoBehaviour
{ 
    public Text status_Health;
    public Text status_Mana;
    public Text status_Attack;
    public Text status_AbilityPower;
    public Text status_Armor;
    public Text status_Range;
    public Text status_AttackSpeed;
    public Text status_MoveSpeed;

    public void SetStatusAllText(Entity entity)
    {
        status_Health.text = "<b>" + entity.healthMaxOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.healthMaxOrigin(1), entity.healthMaxOrigin(entity.maxLevel), entity.maxLevel)
                             + ")</size> " + "/ <b>" + entity.healthMaxOrigin(entity.maxLevel) + "</b><size=15> (18레벨시)</size>";
        status_Mana.text = "<b>" + entity.manaMaxOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.manaMaxOrigin(1), entity.manaMaxOrigin(entity.maxLevel), entity.maxLevel)
                             + ")</size> " + "/ <b>" + entity.manaMaxOrigin(entity.maxLevel) + "</b><size=15> (18레벨시)</size>";
        status_Attack.text = "<b>" + entity.attackDamageOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.attackDamageOrigin(1), entity.attackDamageOrigin(entity.maxLevel), entity.maxLevel)
                             + ")</size> " + "/ <b>" + entity.attackDamageOrigin(entity.maxLevel) + "</b><size=15> (18레벨시)</size>";
        status_AbilityPower.text = "<b>" + entity.abilityPowerOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.abilityPowerOrigin(1), entity.abilityPowerOrigin(entity.maxLevel), entity.maxLevel)
                             + ")</size> " + "/ <b>" + entity.abilityPowerOrigin(entity.maxLevel) + "</b><size=15> (18레벨시)</size>";
        status_Armor.text = "<b>" + entity.armorOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.armorOrigin(1), entity.armorOrigin(entity.maxLevel), entity.maxLevel)
                             + ")</size> " + "/ <b>" + entity.armorOrigin(entity.maxLevel) + "</b><size=15> (18레벨시)</size>";
        status_Range.text = "<b>" + entity.castRangeOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.castRangeOrigin(1), entity.castRangeOrigin(entity.skillTemplates[entity.normalAttackIndex].maxLevel), entity.skillTemplates[entity.normalAttackIndex].maxLevel)
                             + ")</size> " + "/ <b>" + entity.castRangeOrigin(entity.skillTemplates[entity.normalAttackIndex].maxLevel) + "</b><size=15> (18레벨시)</size>";
        status_AttackSpeed.text = "<b>" + entity.attackSpeedOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.attackSpeedOrigin(1), entity.attackSpeedOrigin(entity.maxLevel), entity.maxLevel)
                             + ")</size> " + "/ <b>" + entity.attackSpeedOrigin(entity.maxLevel) + "</b><size=15> (18레벨시)</size>";
        status_MoveSpeed.text = "<b>" + entity.moveSpeedOrigin(1) + "</b><size=15> (+레벨당 " + GetLevelUpValue(entity.moveSpeedOrigin(1), entity.moveSpeedOrigin(entity.maxLevel), entity.maxLevel)
                             + ")</size> " + "/ <b>" + entity.moveSpeedOrigin(entity.maxLevel) + "</b><size=15> (18레벨시)</size>";
    }

    public float GetLevelUpValue(float lv1, float lvMax, int maxLevel)
    {
        return (lvMax - lv1) / ((float)maxLevel - 1f);
    }
}
