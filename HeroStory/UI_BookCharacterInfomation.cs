using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BookCharacterInfomation : MonoBehaviour
{
    public Entity owner;
    public UI_BookCharacterStatus status;
    public UI_BookCharacterSkills skill;
    void Start()
    {
        status.SetStatusAllText(owner);
        skill.SetSkillSlots(owner);
    }

}
