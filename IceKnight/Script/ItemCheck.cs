using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCheck : MonoBehaviour
{

    void Awake()
    {
        if (this.name == "Potion")
        {
            if (PlayerPrefs.GetInt("nVision") != 0 && PlayerPrefs.GetInt("nCheckVision") == 1)
                this.GetComponentInChildren<Toggle>().isOn = true;
        }
        if (this.name == "Clock")
        {
            if (PlayerPrefs.GetInt("nClock") != 0 && PlayerPrefs.GetInt("nCheckClock") == 1)
                this.GetComponentInChildren<Toggle>().isOn = true;
        }
        if (this.name == "Revival")
        {
            if (PlayerPrefs.GetInt("nRevival") != 0 && PlayerPrefs.GetInt("nCheckRevival") == 1)
                this.GetComponentInChildren<Toggle>().isOn = true;
        }
    }
    public void CheckClock()
    {
        if (PlayerPrefs.GetInt("nClock") != 0)
        {
            this.GetComponentInChildren<Toggle>().isOn = !GetComponentInChildren<Toggle>().isOn;
            if (this.GetComponentInChildren<Toggle>().isOn)
                PlayerPrefs.SetInt("nCheckClock", 1);
            else
                PlayerPrefs.SetInt("nCheckClock", 0);
        }
        else
        {
            PopupOpener ClockOpener = new PopupOpener();
            ClockOpener.OpenClockShopPopup();
        }
    }

    public void CheckVision()
    {
        if (PlayerPrefs.GetInt("nVision") != 0)
        {
            this.GetComponentInChildren<Toggle>().isOn = !GetComponentInChildren<Toggle>().isOn;
            if (this.GetComponentInChildren<Toggle>().isOn)
                PlayerPrefs.SetInt("nCheckVision", 1);
            else
                PlayerPrefs.SetInt("nCheckVision", 0);
        }
        else
        {
            PopupOpener PotionOpener = new PopupOpener();
            PotionOpener.OpenPotionShopPopup();
        }
    }
    public void CheckRevival()
    {
        if (PlayerPrefs.GetInt("nRevival") != 0)
        {
            this.GetComponentInChildren<Toggle>().isOn = !GetComponentInChildren<Toggle>().isOn;
            if (this.GetComponentInChildren<Toggle>().isOn)
                PlayerPrefs.SetInt("nCheckRevival", 1);
            else
                PlayerPrefs.SetInt("nCheckRevival", 0);
        }
        else
        {
            PopupOpener RevivalOpener = new PopupOpener();
            RevivalOpener.OpenRevivalShopPopup();
        }
    }
    //public void InitiallizeAllCheck()
    //{
    //    PlayerPrefs.SetInt("nCheckClock", 0);
    //    PlayerPrefs.SetInt("nCheckVision", 0);
    //    PlayerPrefs.SetInt("nCheckRevival", 0);
    //}
}