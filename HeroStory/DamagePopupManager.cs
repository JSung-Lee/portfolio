using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    Normal, Critical, Block, Gold, BigGold,
}

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager self;
    public GameObject prefab;

    public List<DamagePopup> damagePopups;
    public int maxPopupCount;

    public float fontSize = 100;

    private void Start()
    {
        if (self)
            Destroy(this);
        else
            self = this;

        damagePopups = new List<DamagePopup>();
        for (int i = 0; i < maxPopupCount; i++)
        {
            var popup = Instantiate(prefab);
            popup.transform.parent = transform;
            damagePopups.Add(popup.GetComponent<DamagePopup>());
            popup.SetActive(false);
        }
    }

    public int GetAblePopupIndex()
    {
        for (int i = 0; i < damagePopups.Count; i++) {
            if (!damagePopups[i].gameObject.activeSelf)
                return i;
        }
        var popup = Instantiate(prefab);
        popup.transform.parent = transform;
        damagePopups.Add(popup.GetComponent<DamagePopup>());
        popup.SetActive(false);
        return damagePopups.Count - 1;
    }

    public int GetAblePopupIndex(string skillName, Entity receiver)
    {
        for (int i = 0; i < damagePopups.Count; i++) {
            if (damagePopups[i].gameObject.activeSelf && damagePopups[i].skillName == skillName &&
                damagePopups[i].receiver == receiver)
                return i;
        }
        return -1;
    }

    public void ShowDamagePopup(PopupType type, DamageType damageType, float amount, Entity receiver, int interval, string skillName, bool isOnce)
    {
        int index = -1;
        if (!isOnce) index = GetAblePopupIndex(skillName, receiver);
        if (index == -1) {
            index = GetAblePopupIndex();
            damagePopups[index].skillName = skillName;
            damagePopups[index].damage = 0;
            damagePopups[index].receiver = receiver;
        }

        if (damageType == DamageType.Physical)
            damagePopups[index].SetTextColor(Color.red);
        if (damageType == DamageType.Magic)
            damagePopups[index].SetTextColor(Color.blue);
        if (damageType == DamageType.True)
            damagePopups[index].SetTextColor(Color.white);

        if (type == PopupType.Block) {
            damagePopups[index].SetText("Block");
        }
        else {
            damagePopups[index].damage += amount;
            damagePopups[index].SetText(((int)damagePopups[index].damage).ToString());
            //damagePopups[index].SetText("-" + ((int)damagePopups[index].damage).ToString());

        }
        damagePopups[index].SetTextSize((int)fontSize);
        if (type == PopupType.Critical) {
            damagePopups[index].SetTextSize((int)(1.2f * fontSize));
            damagePopups[index].ciriticalshowing(true);
        }else
            damagePopups[index].ciriticalshowing(false);
        
        if (receiver)
        {
            if (!damagePopups[index].gameObject.activeSelf)
            {
                damagePopups[index].transform.position = receiver.transform.position + (Vector3.up * (Random.Range(2, 3) * 0.5f) * interval);
                //Debug.Log("Popup Position : " + damagePopups[index].transform.position);
                damagePopups[index].transform.localScale = Vector3.one;
                damagePopups[index].interval = interval;
                damagePopups[index].transform.LookAt(Camera.main.transform);
                damagePopups[index].gameObject.SetActive(true);
            }
            //damagePopups[index].animator.SetFloat("Speed", isOnce ? 1f : 2f);
            damagePopups[index].lastCallTime = Time.time;
        }
    }
    public void ShowGoldPopup(PopupType type, int amount, Entity receiver, int interval = 0)
    {
        int index = GetAblePopupIndex();
        if (index == -1) return;
        damagePopups[index].SetTextColor(Color.yellow);
        damagePopups[index].SetText("+ " + amount);
        if (type == PopupType.BigGold)
            damagePopups[index].SetTextSize((int)(2f * fontSize));
        else
            damagePopups[index].SetTextSize((int)(1.1f * fontSize));
        damagePopups[index].transform.position = receiver.transform.position + (Vector3.up * interval);
        damagePopups[index].transform.LookAt(Camera.main.transform);
        damagePopups[index].gameObject.SetActive(true);
        damagePopups[index].ciriticalshowing(false);
        damagePopups[index].PlaySound(amount);

    }

    public void ShowGoldPopup(PopupType type, int amount, Entity receiver, int interval, float invoke)
    {
        StartCoroutine(ShowGoldPopupInVoke(type, amount, receiver, interval, invoke));
    }

    IEnumerator ShowGoldPopupInVoke(PopupType type, int amount, Entity receiver, int interval, float invoke)
    {
        yield return new WaitForSeconds(invoke);
        ShowGoldPopup(type, amount, receiver, interval);

    }
}
