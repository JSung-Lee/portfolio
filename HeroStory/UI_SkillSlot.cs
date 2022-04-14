using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    public Skill skill;
    public UI_ShowToolTip tooltip;
    public Image skillImage;
    public Image skillImage_cool;
    public Button button;
    public UI_CircularProgressBar cooldownProgressBar;
    public Text coolTime;
    public Text manaCost;
    public Text hotKey;
    public Button learnButton;
    public Transform skill_levelPos;
    public GameObject skill_levelstack;
    public List<UI_Skill_LevelStack> slot_Stack = new List<UI_Skill_LevelStack>();
}
