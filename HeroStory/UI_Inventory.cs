using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UI_Inventory : MonoBehaviour {
    public UI_InventorySlot slotPrefab;
    public Transform contents;
    public List<UI_InventorySlot> slots = new List<UI_InventorySlot>();
    public Text goldText;
    public Transform tooltipPosition;

    void Update() {
        var player = Player.player;;
        if (!player) return;

        BalancePrefabs();

        for (int i = 0; i < slots.Count; i++) {
            var slot = slots[i];
            slot.dragAndDropable.name = i.ToString(); 
            var item = player.inventory[i];
            
            slot.hotKeyText.text = UI_Toggle.self.inventoryHotkeys[i].ToString().Replace("Alpha", "");

            int icopy = i;
            if (item.valid) {
                slot.button.onClick.SetListener(() => {
                        player.UseItemSkill(icopy);
                });
                slot.tooltip.item = item;
                slot.tooltip.enabled = true;
                slot.tooltip.text = item.ToolTip();
                slot.tooltip.createPos = tooltipPosition;
                slot.dragAndDropable.dragable = true;
                slot.backImage.color = Color.white;
                slot.backImage.sprite = item.backImage;
                slot.image.color = Color.white;
                slot.image.sprite = item.image;
                slot.amountOverlay.SetActive(item.amount > 1);
                slot.amountText.text = item.amount.ToString();

                if (item.usageSkill)
                {
                    if (item.skill.template is PassiveSkillTemplate &&
                    !(item.skill.template is UsablePassiveSkillTemplate))
                    {
                        slot.cooldownProgressBar.value = 0;
                    }
                    else
                    {
                        slot.cooldownProgressBar.value = item.skill.CooldownRemaining() / item.skill.cooldown;
                    }
                }
                    
                


            } else {
                slot.button.onClick.RemoveAllListeners();
                slot.tooltip.enabled = false;
                slot.dragAndDropable.dragable = false;
                slot.backImage.color = Color.clear;
                slot.backImage.sprite = null;
                slot.image.color = Color.clear;
                slot.image.sprite = null;
                slot.amountOverlay.SetActive(false);
                slot.cooldownProgressBar.value = 0;
            }
        }

        // gold
        goldText.text = player.gold.ToString();
    }

    public void BalancePrefabs()
    {
        var player = Player.player;
        if (!player) return;

        int amount = player.inventory.Count;

        //Debug.Log("슬롯 카운트 : "+slots.Count+", amount : "+amount);
        if (slots.Count < amount)
        {
            for (int i = slots.Count; i < amount; i++)
            {
                var slot = Instantiate(slotPrefab);
                slot.transform.SetParent(contents);
                slots.Add(slot.GetComponent<UI_InventorySlot>());
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
