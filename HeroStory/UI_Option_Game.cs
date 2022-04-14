using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option_Game : MonoBehaviour
{
    [Header("=== Slider ===")]
    public Slider mouse_Speed;
    public Slider screen_moveSpeed_Mouse;
    public Slider screen_moveSpeed_Keyboard;

    [HideInInspector] public int default_Mouse_Speed = 50;
    [HideInInspector] public int default_Screen_moveSpeed_Mouse = 50;
    [HideInInspector] public int default_Screen_moveSpeed_Keyboard = 50;

    public Text mouse_speed_Value;
    public Text screen_moveSpeed_Mouse_Value;
    public Text screen_moveSpeed_Keyboard_Value;

    [Header("=== Toggle ===")]
    public Toggle autoAttack_toggle;

    [HideInInspector] public bool default_AutoAttack = true;

    void Start()
    {
        InitializeGameOptions();
    }

    public void InitializeGameOptions()
    {
        mouse_Speed.value = SettingManager.self.game_MouseSpeed;
        screen_moveSpeed_Mouse.value = SettingManager.self.game_CameraSpeed_Mouse;
        screen_moveSpeed_Keyboard.value = SettingManager.self.game_CameraSpeed_Keyboard;
        
        autoAttack_toggle.isOn = SettingManager.self.game_AutoAttack;

        mouse_Speed.onValueChanged.SetListener(OnValueChangedMouseSpeed);
        screen_moveSpeed_Mouse.onValueChanged.SetListener(OnValueChangedCameraSpeed_Mouse);
        screen_moveSpeed_Keyboard.onValueChanged.SetListener(OnValueChangedCameraSpeed_Keyboard);

        autoAttack_toggle.onValueChanged.SetListener(OnValueChangedAutoAttack);

        mouse_Speed.onValueChanged?.Invoke(mouse_Speed.value);
        screen_moveSpeed_Mouse.onValueChanged?.Invoke(screen_moveSpeed_Mouse.value);
        screen_moveSpeed_Keyboard.onValueChanged?.Invoke(screen_moveSpeed_Keyboard.value);

        autoAttack_toggle.onValueChanged?.Invoke(autoAttack_toggle.isOn);
    }

    public void SetAllGameDefault()
    {
        mouse_Speed.value = default_Mouse_Speed;
        screen_moveSpeed_Mouse.value = default_Screen_moveSpeed_Keyboard;
        screen_moveSpeed_Keyboard.value = default_Screen_moveSpeed_Keyboard;

        autoAttack_toggle.isOn = default_AutoAttack;

        mouse_Speed.onValueChanged?.Invoke(mouse_Speed.value);
        screen_moveSpeed_Mouse.onValueChanged?.Invoke(screen_moveSpeed_Mouse.value);
        screen_moveSpeed_Keyboard.onValueChanged?.Invoke(screen_moveSpeed_Keyboard.value);

        autoAttack_toggle.onValueChanged?.Invoke(autoAttack_toggle.isOn);
    }

    public void OnValueChangedMouseSpeed(float value)
    {
        SettingManager.self.game_MouseSpeed = (int)value;
        mouse_speed_Value.text = ((int)value).ToString();
        //Input.GetAxis("Mouse X")
        
    }

    public void OnValueChangedCameraSpeed_Mouse(float value)
    {
        SettingManager.self.game_CameraSpeed_Mouse = (int)value;
        screen_moveSpeed_Mouse_Value.text = ((int)value).ToString();
    }

    public void OnValueChangedCameraSpeed_Keyboard(float value)
    {
        SettingManager.self.game_CameraSpeed_Keyboard = (int)value;
        screen_moveSpeed_Keyboard_Value.text = ((int)value).ToString();
    }

    public void OnValueChangedAutoAttack(bool value)
    {
        SettingManager.self.game_AutoAttack = value;
        if (Player.player) Player.player.autoAttack = value;
    }
}

