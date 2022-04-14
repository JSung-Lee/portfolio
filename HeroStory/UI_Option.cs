using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Option : Book
{
    public static UI_Option self;

    [Header("=== Option Page ===")]
    public List<UI_OptionSlot> slots = new List<UI_OptionSlot>();
    public RectTransform optionSlot_Parent;

    public bool useOptionTab = true;
    public GameObject inputkeyPage;
    public GameObject viewPage;
    public GameObject soundPage;
    public GameObject interfacePage;
    public GameObject gamePage;

    void Awake()
    {
        if (self) Destroy(this);
        self = this;
    }

    void Start()
    {
        if (slots.Count < 1)
            Create_OptionSlot();

        inputkeyPage.SetActive(true);
        viewPage.SetActive(!useOptionTab);
        soundPage.SetActive(!useOptionTab);
        interfacePage.SetActive(!useOptionTab);
        gamePage.SetActive(!useOptionTab);  
    }

    public void Create_OptionSlot()
    {
        slots.Capacity = 5;
        for (int i=0; i<optionSlot_Parent.childCount; i++)
        {
            var optionSlot = optionSlot_Parent.GetChild(i).GetComponent<UI_OptionSlot>();
            slots.Add(optionSlot);
        }
    }


    
}
