using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option_Button : MonoBehaviour
{
    public static UI_Option_Button self;

    [Header("=== Attack & Skill SmartKey ===")]
    public Button skill1_Button;
    public Toggle skill1_Toggle;
    [HideInInspector] KeyCode defaultSkill1Button = KeyCode.Q;
    [HideInInspector] bool defaultSkill1Toggle = false;

    public Button skill2_Button;
    public Toggle skill2_Toggle;
    [HideInInspector] KeyCode defaultSkill2Button = KeyCode.W;
    [HideInInspector] bool defaultSkill2Toggle = false;

    public Button skill3_Button;
    public Toggle skill3_Toggle;
    [HideInInspector] KeyCode defaultSkill3Button = KeyCode.E;
    [HideInInspector] bool defaultSkill3Toggle = false;

    public Button skill4_Button;
    public Toggle skill4_Toggle;
    [HideInInspector] KeyCode defaultSkill4Button = KeyCode.R;
    [HideInInspector] bool defaultSkill4Toggle = false;

    [Header("=== Item Skill Number Smart ===")]
    public Button item1_Button;
    public Toggle item1_Toggle;
    [HideInInspector] KeyCode defaultItem1Button = KeyCode.Alpha1;
    [HideInInspector] bool defaultItem1Toggle = false;

    public Button item2_Button;
    public Toggle item2_Toggle;
    [HideInInspector] KeyCode defaultItem2Button = KeyCode.Alpha2;
    [HideInInspector] bool defaultItem2Toggle = false;

    public Button item3_Button;
    public Toggle item3_Toggle;
    [HideInInspector] KeyCode defaultItem3Button = KeyCode.Alpha3;
    [HideInInspector] bool defaultItem3Toggle = false;

    public Button item4_Button;
    public Toggle item4_Toggle;
    [HideInInspector] KeyCode defaultItem4Button = KeyCode.Alpha4;
    [HideInInspector] bool defaultItem4Toggle = false;

    public Button item5_Button;
    public Toggle item5_Toggle;
    [HideInInspector] KeyCode defaultItem5Button = KeyCode.Alpha5;
    [HideInInspector] bool defaultItem5Toggle = false;

    public Button item6_Button;
    public Toggle item6_Toggle;
    [HideInInspector] KeyCode defaultItem6Button = KeyCode.Alpha6;
    [HideInInspector] bool defaultItem6Toggle = false;

    [Header("=== Other KeyCodes ===")]
    public Button button_Attack;
    [HideInInspector] KeyCode defaultAttackButton = KeyCode.A;
    public Button button_Move;
    [HideInInspector] KeyCode defaultMoveButton = KeyCode.Mouse1;
    public Button buttone_LearnSkill;
    [HideInInspector] KeyCode defaultLearnSkillButton = KeyCode.LeftControl;
    public Button button_StopAction;
    [HideInInspector] KeyCode defaultStopActionButton = KeyCode.S;
    public Button button_DetailStatus;
    [HideInInspector] KeyCode defaultDetailStatusButton = KeyCode.C;
    public Button button_Store;
    [HideInInspector] KeyCode defaultStoreButton = KeyCode.I;
    public Button button_Information;
    [HideInInspector] KeyCode defaultInformationButton = KeyCode.Tab;
    public Button button_Setting;
    [HideInInspector] KeyCode defaultSettingButton = KeyCode.Escape;
    public Button button_FixedCamera;
    [HideInInspector] KeyCode defaultFixedCameraButton = KeyCode.U;
    public Button button_FollowCamera;
    [HideInInspector] KeyCode defaultFollowCameraButton = KeyCode.Space;

    //[HideInInspector]
    public Button currentActiveButton;

    void Start()
    {
        if (!self)
            self = this;
        else
            Destroy(this);

        UpdateKeyValue();
        SetToggleListener();
        SetButtonListener();
        UpdateToggleGraphic();

        currentActiveButton = null;
    }

    private void Update()
    {
        if (!currentActiveButton) return;
        if(Input.anyKeyDown)
        {
            var setting = SettingManager.self;
            if (!setting) return;
            var key = Extensions.GetInputKey();
            if (key != KeyCode.None && key != KeyCode.Mouse0) {
                currentActiveButton.GetComponentInChildren<Text>().text = key.KeyCodeToString();
                SetKeyCodeFromCurrentActiveButton(key);
            }
            UpdateKeyValue();
            currentActiveButton = null;
        }
    }

    public void UpdateKeyValue()
    {
        var setting = SettingManager.self;
        if (!setting) return;
        skill1_Button.GetComponentInChildren<Text>().text = setting.keycode_Skill1.KeyCodeToString();
        skill2_Button.GetComponentInChildren<Text>().text = setting.keycode_Skill2.KeyCodeToString();
        skill3_Button.GetComponentInChildren<Text>().text = setting.keycode_Skill3.KeyCodeToString();
        skill4_Button.GetComponentInChildren<Text>().text = setting.keycode_Skill4.KeyCodeToString();
        skill1_Toggle.isOn = setting.smartCasting_Skill1;
        skill2_Toggle.isOn = setting.smartCasting_Skill2;
        skill3_Toggle.isOn = setting.smartCasting_Skill3;
        skill4_Toggle.isOn = setting.smartCasting_Skill4;

        item1_Button.GetComponentInChildren<Text>().text = setting.keycode_Item1.KeyCodeToString();
        item2_Button.GetComponentInChildren<Text>().text = setting.keycode_Item2.KeyCodeToString();
        item3_Button.GetComponentInChildren<Text>().text = setting.keycode_Item3.KeyCodeToString();
        item4_Button.GetComponentInChildren<Text>().text = setting.keycode_Item4.KeyCodeToString();
        item5_Button.GetComponentInChildren<Text>().text = setting.keycode_Item5.KeyCodeToString();
        item6_Button.GetComponentInChildren<Text>().text = setting.keycode_Item6.KeyCodeToString();
        item1_Toggle.isOn = setting.smartCasting_Item1;
        item2_Toggle.isOn = setting.smartCasting_Item2;
        item3_Toggle.isOn = setting.smartCasting_Item3;
        item4_Toggle.isOn = setting.smartCasting_Item4;
        item5_Toggle.isOn = setting.smartCasting_Item5;
        item6_Toggle.isOn = setting.smartCasting_Item6;

        button_Attack.GetComponentInChildren<Text>().text = setting.keycode_Attack.KeyCodeToString();
        button_Move.GetComponentInChildren<Text>().text = setting.keycode_Move.KeyCodeToString();
        buttone_LearnSkill.GetComponentInChildren<Text>().text = setting.keycode_LearnSkill.KeyCodeToString();
        button_StopAction.GetComponentInChildren<Text>().text = setting.keycode_StopAction.KeyCodeToString();
        button_DetailStatus.GetComponentInChildren<Text>().text = setting.keycode_DetailStatus.KeyCodeToString();
        button_Store.GetComponentInChildren<Text>().text = setting.keycode_Store.KeyCodeToString();
        button_Information.GetComponentInChildren<Text>().text = setting.keycode_Information.KeyCodeToString();
        button_Setting.GetComponentInChildren<Text>().text = setting.keycode_Setting.KeyCodeToString();
        button_FixedCamera.GetComponentInChildren<Text>().text = setting.keycode_FixedCamera.KeyCodeToString();
        button_FollowCamera.GetComponentInChildren<Text>().text = setting.keycode_FollowCamera.KeyCodeToString();
    }

    public void UpdateToggleGraphic()
    {
        skill1_Toggle.onValueChanged?.Invoke(skill1_Toggle.isOn);
        skill2_Toggle.onValueChanged?.Invoke(skill2_Toggle.isOn);
        skill3_Toggle.onValueChanged?.Invoke(skill3_Toggle.isOn);
        skill4_Toggle.onValueChanged?.Invoke(skill4_Toggle.isOn);
        item1_Toggle.onValueChanged?.Invoke(item1_Toggle.isOn);
        item2_Toggle.onValueChanged?.Invoke(item2_Toggle.isOn);
        item3_Toggle.onValueChanged?.Invoke(item3_Toggle.isOn);
        item4_Toggle.onValueChanged?.Invoke(item4_Toggle.isOn);
        item5_Toggle.onValueChanged?.Invoke(item5_Toggle.isOn);
        item6_Toggle.onValueChanged?.Invoke(item6_Toggle.isOn);
    }

    public void SetButtonListener()
    {
        skill1_Button.onClick.SetListener(() => { ButtonListener(skill1_Button); });
        skill2_Button.onClick.SetListener(() => { ButtonListener(skill2_Button); });
        skill3_Button.onClick.SetListener(() => { ButtonListener(skill3_Button); });
        skill4_Button.onClick.SetListener(() => { ButtonListener(skill4_Button); });

        item1_Button.onClick.SetListener(() => { ButtonListener(item1_Button); });
        item2_Button.onClick.SetListener(() => { ButtonListener(item2_Button); });
        item3_Button.onClick.SetListener(() => { ButtonListener(item3_Button); });
        item4_Button.onClick.SetListener(() => { ButtonListener(item4_Button); });
        item5_Button.onClick.SetListener(() => { ButtonListener(item5_Button); });
        item6_Button.onClick.SetListener(() => { ButtonListener(item6_Button); });

        button_Attack.onClick.SetListener(() => { ButtonListener(button_Attack); });
        button_Move.onClick.SetListener(() => { ButtonListener(button_Move); });
        buttone_LearnSkill.onClick.SetListener(() => { ButtonListener(buttone_LearnSkill); });
        button_StopAction.onClick.SetListener(() => { ButtonListener(button_StopAction); });
        button_DetailStatus.onClick.SetListener(() => { ButtonListener(button_DetailStatus); });
        button_Store.onClick.SetListener(() => { ButtonListener(button_Store); });
        button_Information.onClick.SetListener(() => { ButtonListener(button_Information); });
        button_Setting.onClick.SetListener(() => { ButtonListener(button_Setting); });
        button_FixedCamera.onClick.SetListener(() => { ButtonListener(button_FixedCamera); });
        button_FollowCamera.onClick.SetListener(() => { ButtonListener(button_FollowCamera); });
    }

    public void SetKeyCodeFromCurrentActiveButton(KeyCode keyCode)
    {
        var setting = SettingManager.self;
        if (!setting) return;

        if (currentActiveButton == skill1_Button) setting.keycode_Skill1 = keyCode;
        else if (currentActiveButton == skill2_Button) setting.keycode_Skill2 = keyCode;
        else if (currentActiveButton == skill3_Button) setting.keycode_Skill3 = keyCode;
        else if (currentActiveButton == skill4_Button) setting.keycode_Skill4 = keyCode;
        else if (currentActiveButton == item1_Button) setting.keycode_Item1 = keyCode;
        else if (currentActiveButton == item2_Button) setting.keycode_Item2 = keyCode;
        else if (currentActiveButton == item3_Button) setting.keycode_Item3 = keyCode;
        else if (currentActiveButton == item4_Button) setting.keycode_Item4 = keyCode;
        else if (currentActiveButton == item5_Button) setting.keycode_Item5 = keyCode;
        else if (currentActiveButton == item6_Button) setting.keycode_Item6 = keyCode;
        else if (currentActiveButton == button_Attack) setting.keycode_Attack = keyCode;
        else if (currentActiveButton == button_Move) setting.keycode_Move = keyCode;
        else if (currentActiveButton == buttone_LearnSkill) setting.keycode_LearnSkill = keyCode;
        else if (currentActiveButton == button_StopAction) setting.keycode_StopAction = keyCode;
        else if (currentActiveButton == button_DetailStatus) setting.keycode_DetailStatus = keyCode;
        else if (currentActiveButton == button_Store) setting.keycode_Store = keyCode;
        else if (currentActiveButton == button_Information) setting.keycode_Information = keyCode;
        else if (currentActiveButton == button_Setting) setting.keycode_Setting = keyCode;
        else if (currentActiveButton == button_FixedCamera) setting.keycode_FixedCamera = keyCode;
        else if (currentActiveButton == button_FollowCamera) setting.keycode_FollowCamera = keyCode;
    }

    public void SetToggleListener()
    {
        skill1_Toggle.onValueChanged.SetListener(SetSmartSkill1);
        skill2_Toggle.onValueChanged.SetListener(SetSmartSkill2);
        skill3_Toggle.onValueChanged.SetListener(SetSmartSkill3);
        skill4_Toggle.onValueChanged.SetListener(SetSmartSkill4);
        item1_Toggle.onValueChanged.SetListener(SetSmartItem1);
        item2_Toggle.onValueChanged.SetListener(SetSmartItem2);
        item3_Toggle.onValueChanged.SetListener(SetSmartItem3);
        item4_Toggle.onValueChanged.SetListener(SetSmartItem4);
        item5_Toggle.onValueChanged.SetListener(SetSmartItem5);
        item6_Toggle.onValueChanged.SetListener(SetSmartItem6);
    }

    public void ButtonListener(Button button)
    {
        if (currentActiveButton) {
            if (currentActiveButton == button)
                return;
            UpdateKeyValue();
        }
        button.GetComponentInChildren<Text>().text = "";
        currentActiveButton = button;
    }

    public void SetSmartSkill1(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Skill1 = isSmart;
    }
    public void SetSmartSkill2(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Skill2 = isSmart;
    }
    public void SetSmartSkill3(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Skill3 = isSmart;
    }
    public void SetSmartSkill4(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Skill4 = isSmart;
    }
    public void SetSmartItem1(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Item1 = isSmart;
    }
    public void SetSmartItem2(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Item2 = isSmart;
    }
    public void SetSmartItem3(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Item3 = isSmart;
    }
    public void SetSmartItem4(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Item4 = isSmart;
    }
    public void SetSmartItem5(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Item5 = isSmart;
    }
    public void SetSmartItem6(bool isSmart)
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.smartCasting_Item6 = isSmart;
    }

    public void SetAllSmartCasting(bool isSmart)
    {
        skill1_Toggle.isOn = isSmart;
        skill2_Toggle.isOn = isSmart;
        skill3_Toggle.isOn = isSmart;
        skill4_Toggle.isOn = isSmart;

        item1_Toggle.isOn = isSmart;
        item2_Toggle.isOn = isSmart;
        item3_Toggle.isOn = isSmart;
        item4_Toggle.isOn = isSmart;
        item5_Toggle.isOn = isSmart;
        item6_Toggle.isOn = isSmart;

        UpdateToggleGraphic();
    }

    public void SetAllKeyDefault()
    {
        var setting = SettingManager.self;
        if (!setting) return;
        setting.keycode_Skill1 = defaultSkill1Button;
        setting.keycode_Skill2 = defaultSkill2Button;
        setting.keycode_Skill3 = defaultSkill3Button;
        setting.keycode_Skill4 = defaultSkill4Button;
        setting.smartCasting_Skill1 = defaultSkill1Toggle;
        setting.smartCasting_Skill2 = defaultSkill2Toggle;
        setting.smartCasting_Skill3 = defaultSkill3Toggle;
        setting.smartCasting_Skill4 = defaultSkill4Toggle;

        setting.keycode_Item1 = defaultItem1Button;
        setting.keycode_Item2 = defaultItem2Button;
        setting.keycode_Item3 = defaultItem3Button;
        setting.keycode_Item4 = defaultItem4Button;
        setting.keycode_Item5 = defaultItem5Button;
        setting.keycode_Item6 = defaultItem6Button;
        setting.smartCasting_Item1 = defaultItem1Toggle;
        setting.smartCasting_Item2 = defaultItem2Toggle;
        setting.smartCasting_Item3 = defaultItem3Toggle;
        setting.smartCasting_Item4 = defaultItem4Toggle;
        setting.smartCasting_Item5 = defaultItem5Toggle;
        setting.smartCasting_Item6 = defaultItem6Toggle;

        setting.keycode_Attack = defaultAttackButton;
        setting.keycode_Move = defaultMoveButton;
        setting.keycode_LearnSkill = defaultLearnSkillButton;
        setting.keycode_StopAction = defaultStopActionButton;
        setting.keycode_DetailStatus = defaultDetailStatusButton;
        setting.keycode_Store = defaultStoreButton;
        setting.keycode_Information = defaultInformationButton;
        setting.keycode_Setting = defaultSettingButton;
        setting.keycode_FixedCamera = defaultFixedCameraButton;
        setting.keycode_FollowCamera = defaultFollowCameraButton;

        UpdateKeyValue();
    }

}
