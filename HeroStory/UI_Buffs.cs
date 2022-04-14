using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Buffs : MonoBehaviour
{
    public Entity owner;

    public GameObject buffPrefab;
    public List<UI_Buff> buffSlot = new List<UI_Buff>();

    public Transform tooltipPosition;

    Vector3 learnSkill_pos;
    float add_height;
    void Start()
    {
        owner = Player.player;

        learnSkill_pos = transform.localPosition;
        //RectTransform btn = (RectTransform)UI_Skills.self.
        //add_height = btn.rect.height;
    }

    void Update()
    {
        if (!owner) 
        {
            Start();
            return; 
        }

        BalancePrefabs();
        //Debug.Log(gameObject.transform.localPosition);
        
        if (UI_Skills.self.isLearn == false)
            transform.localPosition = learnSkill_pos;
        else
            transform.localPosition = new Vector3(learnSkill_pos.x, learnSkill_pos.y + add_height + 5, learnSkill_pos.z);

        for (int i = 0; i < owner.buffs.Count; i++)
        {
            var buff = owner.buffs[i];
            var slot = buffSlot[i];

            

            slot.buffImage.sprite = buff.image;
            slot.buffImageFill.sprite = buff.image;
            /*
            if (slot.buff.template is PassiveSkillTemplate &&
                !(slot.buff.template is UsablePassiveSkillTemplate))
            {
                Debug.Log("tpye : sss");
            }                
            else
            {
                Debug.Log("tpye : ");
            }*/
            
            slot.buffImageFill.fillAmount = buff.BuffTimeRemaining() / (float)buff.buffTime;

            slot.tooltip.buff = buff;
            slot.tooltip._targetEntity = owner;
        }
    }

    void BalancePrefabs()
    {
        if (!owner) return;

        int amount = owner.buffs.Count;

        if (buffSlot.Count < amount)
        {
            for (int i = buffSlot.Count; i < amount; i++)
            {
                var buff = Instantiate(buffPrefab);
                buff.transform.parent = transform;                
                buffSlot.Add(buff.GetComponent<UI_Buff>());
            }
            
            RectTransform btn = (RectTransform)UI_Skills.self.slots[0].learnButton.transform;
            add_height = btn.rect.height;
        }
        for (int i = 0; i < buffSlot.Count; i++)
        {
            buffSlot[i].gameObject.SetActive(i < amount);
        }
    }
}
