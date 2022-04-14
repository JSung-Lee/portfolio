using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BookSkillTooltip : MonoBehaviour
{
    public Image image;
    public Text nameText;
    public Text costText;
    public Text cooldownText;
    public Text tooltipText;
    public void SetSkillTooltip(Skill skill)
    {
        image.sprite = skill.image;
        nameText.text = skill.skillname;
        costText.text = skill.manaCosts.ToString();
        costText.text = "마나 (";
        for (int i = 1; i <= skill.maxLevel; i++)
        {
            skill.level = i;
            costText.text += (skill.manaCosts.ToString() + "/");
        }
        costText.text = costText.text.Remove(costText.text.Length - 1);
        costText.text += ")";
        cooldownText.text = "(";
        for (int i = 1; i <= skill.maxLevel; i++) {
            skill.level = i;
            cooldownText.text += (skill.cooldown.ToString() + "s/");
        }
        cooldownText.text = cooldownText.text.Remove(cooldownText.text.Length - 1);
        cooldownText.text += ")";

        tooltipText.text = skill.ToolTip();
    }
}
