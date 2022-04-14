using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Skills : MonoBehaviour
{
    public static UI_Skills self;
    public Transform contents;
    public List<UI_SkillSlot> slots = new List<UI_SkillSlot>();
    public int maxSlot = 4;
    public GameObject slotPrefab;
    public RectTransform tooltipPosition;
    public Player player;

    public Color notLearnColorl = new Color(0.3f, 0.3f, 0.3f);

    public bool isLearn = false;
    // Start is called before the first frame update
    void Start()
    {
        if (self) Destroy(this);
        else self = this;

        if (!player) player = Player.player;
        foreach (var slot in contents.GetComponentsInChildren<UI_SkillSlot>())
        {
            if (slot) slots.Add(slot);
        }
    }

    void Update()
    {
        if (!player) player = Player.player;
        if (!player) return;

        BalancePrefabs();

        for (int i = 0; i < slots.Count; i++)
        {
            int skillIndex = i + 1;
            var skill = player.skills[skillIndex];
            slots[i].skillImage.sprite = skill.image;
            slots[i].skillImage.color = skill.learned ? Color.white : notLearnColorl;
            slots[i].skillImage_cool.sprite = skill.image;
            if (player.skills[skillIndex].template is PassiveSkillTemplate &&
              !(player.skills[skillIndex].template is UsablePassiveSkillTemplate))
            {
                slots[i].cooldownProgressBar.value = 0;
                slots[i].coolTime.text = "";
            }
            else
            {
                slots[i].cooldownProgressBar.value = skill.CooldownRemaining() / (skill.cooldown * (1f - player.cooldown));
                slots[i].coolTime.text = !skill.IsReady() ? skill.CooldownRemaining().ToString("0") : "";
            }
            slots[i].manaCost.text = (skill.manaCosts - (skill.manaCosts * player.manaConsumptions)).ToString();
            if (SettingManager.self) slots[i].manaCost.gameObject.SetActive(SettingManager.self.interface_SkillCost);
            slots[i].hotKey.text = UI_Toggle.self.skillsHotkey[skillIndex].ToString();
            slots[i].tooltip.hotKey = slots[i].hotKey.text;
            slots[i].tooltip.enabled = true;
            slots[i].tooltip.skill = player.skills[skillIndex];
            slots[i].tooltip.text = skill.ToolTip(player);
            slots[i].tooltip.createPos = tooltipPosition;
            slots[i].tooltip.tooltipWidth = tooltipPosition.sizeDelta.x;
            slots[i].tooltip.owner = player;

            int icopy = skillIndex;
            slots[i].button.onClick.SetListener(() =>
            {
                if (player.skills[icopy].isSpecialCondition) return;
                if (player.skills[icopy].learned)
                    player.UseSkill(icopy);
            });

            if (player.CanLearnSkill(skill))
            {
                slots[i].learnButton.gameObject.SetActive(true);
                slots[i].learnButton.onClick.SetListener(() => { player.LearnSkill(icopy); });
            }
            else if (player.CanUpgradeSkill(skill))
            {
                slots[i].learnButton.gameObject.SetActive(true);
                slots[i].learnButton.onClick.SetListener(() => { player.UpgradeSkill(icopy); });
            }
            else slots[i].learnButton.gameObject.SetActive(false);

            if(slots[0].learnButton.gameObject.activeSelf == true || slots[1].learnButton.gameObject.activeSelf == true || 
                slots[2].learnButton.gameObject.activeSelf == true || slots[3].learnButton.gameObject.activeSelf == true) 
            {
                isLearn = true;
            }
            else
                isLearn = false;


            //스킬 최고 레벨만큼 스킬 스택 생성
            if (slots[i].skill_levelPos.childCount < skill.maxLevel)
            {
                for (int a = 0; a < skill.maxLevel; a++)
                {
                    var _stack = Instantiate(slots[i].skill_levelstack);
                    _stack.transform.SetParent(slots[i].skill_levelPos);

                    slots[i].slot_Stack.Add(_stack.GetComponent<UI_Skill_LevelStack>());
                }

            }

            for (int a = 0; a < slots[i].slot_Stack.Count; a++)
            {
                if (skill.learned)
                {
                    Color color;

                    if (a < skill.level)
                    {
                        color = slots[i].slot_Stack[a].stack.color;
                        color.a = 255 / 255f;

                        slots[i].slot_Stack[a].stack.color = color;
                    }
                }
            }

        }
    }


    public void BalancePrefabs()
    {
        var player = Player.player;
        if (!player) return;

        int amount = Mathf.Min(player.skills.Count - 1, maxSlot);

        if (slots.Count < amount)
        {
            for (int i = slots.Count; i < amount; i++)
            {
                var slot = Instantiate(slotPrefab);
                slot.transform.SetParent(contents);
                slots.Add(slot.GetComponent<UI_SkillSlot>());
            }
        }
        else if (slots.Count > amount)
        {
            for (int i = amount; i < slots.Count; i++)
            {
                var _slot = slots[i].gameObject;
                slots.RemoveAt(i);
                Destroy(_slot);
            }
        }
    }
}
