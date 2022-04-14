using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option_Interface : MonoBehaviour
{
    public static UI_Option_Interface self;

    [Header("=== Slider ===")]
    // Slider
    public Slider interface_Size;
    public Slider minimap_Size;
    // Text
    public Text interface_Value;
    public Text minimap_Value;

    [HideInInspector] public int default_Interface_Size = 50;
    [HideInInspector] public int default_Minimap_Size = 50;

    [Header("=== Toggle ===")]    
    public Toggle playerStatus_toggle; // 캐릭터 위 체력바 띄울지    
    public Toggle GetCrowdControlStateUI;   //CC기 UI

    public Toggle getDamageUI_toggle;  // 데미지 받으면 피해 UI 띄울지
    public Toggle getCrowdControlFlicker_toggle; // 군중제어기 화면깜빡임
    public Toggle centerMark_toggle; // 스페이스바 누르면 플레이어 밑에 표시 띄움

    public Toggle targetOutline_toggle; // 공격 사거리 표시
    public Toggle projectileOrbit_toggle; // 공격 사거리 표시
    public Toggle attackRange_toggle; // 공격 사거리 표시
    public Toggle manaCost_toggle;  //스킬 소모값 표시

    [HideInInspector] public bool default_PlayerStatus = true;
    [HideInInspector] public bool default_CrowdControlStateUI = true;

    [HideInInspector] public bool default_DamagedUI = true;
    [HideInInspector] public bool default_CrowdControlFlicker = true;
    [HideInInspector] public bool default_CenterMark = true;

    [HideInInspector] public bool default_TargetOutline = true;
    [HideInInspector] public bool default_ProjectileOrbit = true;
    [HideInInspector] public bool default_AttackRange = true;
    [HideInInspector] public bool default_ManaCost = true;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInterfaceOptions();
    }
    public void InitializeInterfaceOptions()
    {
        interface_Size.value = SettingManager.self.interface_InterfaceSize;
        minimap_Size.value = SettingManager.self.interface_MinimapSize;

        playerStatus_toggle.isOn = SettingManager.self.interface_HealthBar;
        GetCrowdControlStateUI.isOn = SettingManager.self.interface_CrowdControlStateUI;
        getDamageUI_toggle.isOn = SettingManager.self.interface_DamagedFlicker;
        getCrowdControlFlicker_toggle.isOn = SettingManager.self.interface_CrowdControlFlicker;
        centerMark_toggle.isOn = SettingManager.self.interface_CenterMark;
        targetOutline_toggle.isOn = SettingManager.self.interface_TargetOutline;
        projectileOrbit_toggle.isOn = SettingManager.self.interface_ProjectileOrbit;
        attackRange_toggle.isOn = SettingManager.self.interface_AttackRange;
        manaCost_toggle.isOn = SettingManager.self.interface_SkillCost;

        interface_Size.onValueChanged.SetListener(OnValueChangedInterfaceSize);
        minimap_Size.onValueChanged.SetListener(OnValueChangedMinimapSize);

        playerStatus_toggle.onValueChanged.SetListener(OnValueChangedPlayerStatus);
        GetCrowdControlStateUI.onValueChanged.SetListener(OnValueChangedCrowdControlStateUI);
        getDamageUI_toggle.onValueChanged.SetListener(OnValueChangedDamagedFlicker);
        getCrowdControlFlicker_toggle.onValueChanged.SetListener(OnValueChangedCrowdControlFlicker);
        centerMark_toggle.onValueChanged.SetListener(OnValueChangedCenterMarkUI);
        targetOutline_toggle.onValueChanged.SetListener(OnValueChangeTargetOutline);
        projectileOrbit_toggle.onValueChanged.SetListener(OnValueChangeProjectileOrbit);
        attackRange_toggle.onValueChanged.SetListener(OnValueChangeAttackRange);
        manaCost_toggle.onValueChanged.SetListener(OnValueChangeSkillCost);

        interface_Size.onValueChanged?.Invoke(interface_Size.value);
        minimap_Size.onValueChanged?.Invoke(minimap_Size.value);

        playerStatus_toggle.onValueChanged?.Invoke(playerStatus_toggle.isOn);
        GetCrowdControlStateUI.onValueChanged?.Invoke(GetCrowdControlStateUI.isOn);
        getDamageUI_toggle.onValueChanged?.Invoke(getDamageUI_toggle.isOn);
        getCrowdControlFlicker_toggle.onValueChanged?.Invoke(getCrowdControlFlicker_toggle.isOn);
        centerMark_toggle.onValueChanged?.Invoke(centerMark_toggle.isOn);
        targetOutline_toggle.onValueChanged?.Invoke(targetOutline_toggle.isOn);
        projectileOrbit_toggle.onValueChanged?.Invoke(projectileOrbit_toggle.isOn);
        attackRange_toggle.onValueChanged?.Invoke(attackRange_toggle.isOn);
        manaCost_toggle.onValueChanged?.Invoke(manaCost_toggle.isOn);
    }

    public void SetAllInterfaceDefault()
    {
        interface_Size.value = default_Interface_Size;
        minimap_Size.value = default_Minimap_Size;

        playerStatus_toggle.isOn = default_PlayerStatus;
        GetCrowdControlStateUI.isOn = default_CrowdControlStateUI;
        getDamageUI_toggle.isOn = default_DamagedUI;
        getCrowdControlFlicker_toggle.isOn = default_CrowdControlFlicker;
        centerMark_toggle.isOn = default_CenterMark;
        targetOutline_toggle.isOn = default_TargetOutline;
        projectileOrbit_toggle.isOn = default_ProjectileOrbit;
        attackRange_toggle.isOn = default_AttackRange;
        manaCost_toggle.isOn = default_ManaCost;

        interface_Size.onValueChanged?.Invoke(interface_Size.value);
        minimap_Size.onValueChanged?.Invoke(minimap_Size.value);

        playerStatus_toggle.onValueChanged?.Invoke(playerStatus_toggle.isOn);
        GetCrowdControlStateUI.onValueChanged?.Invoke(GetCrowdControlStateUI.isOn);
        getDamageUI_toggle.onValueChanged?.Invoke(getDamageUI_toggle.isOn);
        getCrowdControlFlicker_toggle.onValueChanged?.Invoke(getCrowdControlFlicker_toggle.isOn);
        centerMark_toggle.onValueChanged?.Invoke(centerMark_toggle.isOn);
        targetOutline_toggle.onValueChanged?.Invoke(targetOutline_toggle.isOn);
        projectileOrbit_toggle.onValueChanged?.Invoke(projectileOrbit_toggle.isOn);
        attackRange_toggle.onValueChanged?.Invoke(attackRange_toggle.isOn);
        manaCost_toggle.onValueChanged?.Invoke(manaCost_toggle.isOn);
    }

    public void OnValueChangedInterfaceSize(float value)
    {
        SettingManager.self.interface_InterfaceSize = (int)value;
        interface_Value.text = ((int)value).ToString();
    }
    public void OnValueChangedMinimapSize(float value)
    {
        SettingManager.self.interface_MinimapSize = (int)value;
        minimap_Value.text = ((int)value).ToString();

        var minimap = UI_Option_Minimap.self;
        if (minimap) minimap.Scale_MiniMap(value);
    }

    public void OnValueChangedPlayerStatus(bool value)
    {
        SettingManager.self.interface_HealthBar = value;

        var player = Player.player;
        if (player) player.SetEntityStatusUI(value);
    }

    public void OnValueChangedCrowdControlStateUI(bool value)
    {
        SettingManager.self.interface_CrowdControlStateUI = value;
        //미구현
    }

public void OnValueChangedDamagedFlicker(bool value)
    {
        SettingManager.self.interface_DamagedFlicker = value;
        if(UI_GetDamage.self) UI_GetDamage.self.isGetDamage = value;
    }

    public void OnValueChangedCrowdControlFlicker(bool value)
    {
        SettingManager.self.interface_CrowdControlFlicker = value;
        if (UI_GetDamage.self) UI_GetDamage.self.isGetStun = value;
    }

    public void OnValueChangedCenterMarkUI(bool value)
    {
        SettingManager.self.interface_CenterMark = value;
        if(UI_Toggle.self) UI_Toggle.self.isCenter = value;
    }

    public void OnValueChangeTargetOutline(bool value)
    {
        SettingManager.self.interface_TargetOutline = value;
    }

    public void OnValueChangeProjectileOrbit(bool value)
    {
        SettingManager.self.interface_ProjectileOrbit = value;
        //미구현
    }

    public void OnValueChangeAttackRange(bool value)
    {
        SettingManager.self.interface_AttackRange = value;

        var player = Player.player;
        if (player) player.SetAttackRangeUI(value);
    }

    public void OnValueChangeSkillCost(bool value)
    {
        SettingManager.self.interface_SkillCost = value;
    }
}
