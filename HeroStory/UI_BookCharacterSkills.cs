using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BookCharacterSkills : MonoBehaviour
{
    public Image[] slots;
    public UI_BookSkillTooltip tooltip;
    
    List<Skill> skills = new List<Skill>();
    public void SetSkillSlots(Entity entity)
    {
        for(int i =1;i< entity.skillTemplates.Length;i++)
            skills.Add(new Skill(entity.skillTemplates[i]));

        for (int i = 0; i < skills.Count; i++)
            slots[i].sprite = skills[i].image;

        SetSkillTooltip(0);
    }

    public void SetSkillTooltip(int index)
    {
        tooltip.SetSkillTooltip(skills[index]);
    }
}
