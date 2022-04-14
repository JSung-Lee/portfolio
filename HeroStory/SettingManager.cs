using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager self;

    [Header("Key Setting")]
    public KeyCode keycode_Skill1 = KeyCode.Q;
    public KeyCode keycode_Skill2 = KeyCode.W;
    public KeyCode keycode_Skill3 = KeyCode.E;
    public KeyCode keycode_Skill4 = KeyCode.R;
    public bool smartCasting_Skill1 = false;
    public bool smartCasting_Skill2 = false;
    public bool smartCasting_Skill3 = false;
    public bool smartCasting_Skill4 = false;
    public KeyCode keycode_Item1 = KeyCode.Alpha1;
    public KeyCode keycode_Item2 = KeyCode.Alpha2;
    public KeyCode keycode_Item3 = KeyCode.Alpha3;
    public KeyCode keycode_Item4 = KeyCode.Alpha4;
    public KeyCode keycode_Item5 = KeyCode.Alpha5;
    public KeyCode keycode_Item6 = KeyCode.Alpha6;
    public bool smartCasting_Item1 = false;
    public bool smartCasting_Item2 = false;
    public bool smartCasting_Item3 = false;
    public bool smartCasting_Item4 = false;
    public bool smartCasting_Item5 = false;
    public bool smartCasting_Item6 = false;
    public KeyCode keycode_Attack = KeyCode.A;
    public KeyCode keycode_Move = KeyCode.Mouse1;
    public KeyCode keycode_LearnSkill = KeyCode.LeftControl;
    public KeyCode keycode_StopAction = KeyCode.S;
    public KeyCode keycode_DetailStatus = KeyCode.C;
    public KeyCode keycode_Store = KeyCode.I;
    public KeyCode keycode_Information = KeyCode.Tab;
    public KeyCode keycode_Setting = KeyCode.Escape;
    public KeyCode keycode_FixedCamera = KeyCode.U;
    public KeyCode keycode_FollowCamera = KeyCode.Space;
    
    [Header("Screen Setting")]
    public int resolution_Width = 1920;
    public int resolution_Height = 1080;
    public int resolution_fullScreen = 2;

    [Header("Sound Setting")]
    public int sound_Master = 75;
    public int sound_Music = 75;
    public int sound_Voice = 75;
    public int sound_FX = 75;
    public int sound_Enviroment = 75;
    public bool sound_Master_Use = true;
    public bool sound_Music_Use = true;
    public bool sound_Voice_Use = true;
    public bool sound_FX_Use = true;
    public bool sound_Enviroment_Use = true;

    [Header("Interface Setting")]
    public int interface_InterfaceSize = 100;
    public int interface_MinimapSize = 100;
    public bool interface_HealthBar = true;
    public bool interface_CrowdControlStateUI = true;
    public bool interface_DamagedFlicker = true;
    public bool interface_CrowdControlFlicker = true;
    public bool interface_CenterMark = true;
    public bool interface_TargetOutline = true;
    public bool interface_ProjectileOrbit = true;
    public bool interface_AttackRange = true;
    public bool interface_SkillCost = true;

    [Header("Game Setting")]
    public int game_MouseSpeed = 100;
    public int game_CameraSpeed_Mouse = 100;
    public int game_CameraSpeed_Keyboard = 100;
    public bool game_AutoAttack = true;

    bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!self)
            self = this;
        else
            Destroy(this);

        LoadData();
    }

    private void OnDestroy()
    {
        //한번도 로드된적이 없으면 저장 안함
        if(isLoaded) SaveData();
    }

    public void SaveData()
    {
        //Key Setting
        PlayerPrefs.SetInt("keycode_Skill1", (int)keycode_Skill1);
        PlayerPrefs.SetInt("keycode_Skill2", (int)keycode_Skill2);
        PlayerPrefs.SetInt("keycode_Skill3", (int)keycode_Skill3);
        PlayerPrefs.SetInt("keycode_Skill4", (int)keycode_Skill4);
        PlayerPrefs.SetInt("smartCasting_Skill1", smartCasting_Skill1.ToInt());
        PlayerPrefs.SetInt("smartCasting_Skill2", smartCasting_Skill2.ToInt());
        PlayerPrefs.SetInt("smartCasting_Skill3", smartCasting_Skill3.ToInt());
        PlayerPrefs.SetInt("smartCasting_Skill4", smartCasting_Skill4.ToInt());
        PlayerPrefs.SetInt("keycode_Item1", (int)keycode_Item1);
        PlayerPrefs.SetInt("keycode_Item2", (int)keycode_Item2);
        PlayerPrefs.SetInt("keycode_Item3", (int)keycode_Item3);
        PlayerPrefs.SetInt("keycode_Item4", (int)keycode_Item4);
        PlayerPrefs.SetInt("keycode_Item5", (int)keycode_Item5);
        PlayerPrefs.SetInt("keycode_Item6", (int)keycode_Item6);
        PlayerPrefs.SetInt("smartCasting_Item1", smartCasting_Item1.ToInt());
        PlayerPrefs.SetInt("smartCasting_Item2", smartCasting_Item2.ToInt());
        PlayerPrefs.SetInt("smartCasting_Item3", smartCasting_Item3.ToInt());
        PlayerPrefs.SetInt("smartCasting_Item4", smartCasting_Item4.ToInt());
        PlayerPrefs.SetInt("smartCasting_Item5", smartCasting_Item5.ToInt());
        PlayerPrefs.SetInt("smartCasting_Item6", smartCasting_Item6.ToInt());
        PlayerPrefs.SetInt("keycode_Attack", (int)keycode_Attack);
        PlayerPrefs.SetInt("keycode_Move", (int)keycode_Move);
        PlayerPrefs.SetInt("keycode_LearnSkill", (int)keycode_LearnSkill);
        PlayerPrefs.SetInt("keycode_StopAction", (int)keycode_StopAction);
        PlayerPrefs.SetInt("keycode_DetailStatus", (int)keycode_DetailStatus);
        PlayerPrefs.SetInt("keycode_Store", (int)keycode_Store);
        PlayerPrefs.SetInt("keycode_Information", (int)keycode_Information);
        PlayerPrefs.SetInt("keycode_Setting", (int)keycode_Setting);
        PlayerPrefs.SetInt("keycode_FixedCamera", (int)keycode_FixedCamera);
        PlayerPrefs.SetInt("keycode_FollowCamera", (int)keycode_FollowCamera);

        //Screen Setting
        PlayerPrefs.SetInt("resolution_Width", resolution_Width);
        PlayerPrefs.SetInt("resolution_Height", resolution_Height);
        PlayerPrefs.SetInt("resolution_fullScreen", resolution_fullScreen);

        //Sound Setting
        PlayerPrefs.SetInt("sound_Master", sound_Master);
        PlayerPrefs.SetInt("sound_Music", sound_Music);
        PlayerPrefs.SetInt("sound_Voice", sound_Voice);
        PlayerPrefs.SetInt("sound_FX", sound_FX);
        PlayerPrefs.SetInt("sound_Enviroment", sound_Enviroment);
        PlayerPrefs.SetInt("sound_Master_Use", sound_Master_Use.ToInt());
        PlayerPrefs.SetInt("sound_Music_Use", sound_Music_Use.ToInt());
        PlayerPrefs.SetInt("sound_Voice_Use", sound_Voice_Use.ToInt());
        PlayerPrefs.SetInt("sound_FX_Use", sound_FX_Use.ToInt());
        PlayerPrefs.SetInt("sound_Enviroment_Use", sound_Enviroment_Use.ToInt());

        //Interface Setting
        PlayerPrefs.SetInt("interface_InterfaceSize", interface_InterfaceSize);
        PlayerPrefs.SetInt("interface_MinimapSize", interface_MinimapSize);
        PlayerPrefs.SetInt("interface_HealthBar", interface_HealthBar.ToInt());
        PlayerPrefs.SetInt("interface_CrowdControlStateUI", interface_CrowdControlStateUI.ToInt());
        PlayerPrefs.SetInt("interface_DamagedFlicker", interface_DamagedFlicker.ToInt());
        PlayerPrefs.SetInt("interface_CrowdControlFlicker", interface_CrowdControlFlicker.ToInt());
        PlayerPrefs.SetInt("interface_CenterMark", interface_CenterMark.ToInt());
        PlayerPrefs.SetInt("interface_TargetOutline", interface_TargetOutline.ToInt());
        PlayerPrefs.SetInt("interface_ProjectileOrbit", interface_ProjectileOrbit.ToInt());
        PlayerPrefs.SetInt("interface_AttackRange", interface_AttackRange.ToInt());
        PlayerPrefs.SetInt("interface_SkillCost", interface_SkillCost.ToInt());

        //Game Setting
        PlayerPrefs.SetInt("game_MouseSpeed", game_MouseSpeed);
        PlayerPrefs.SetInt("game_CameraSpeed_Mouse", game_CameraSpeed_Mouse);
        PlayerPrefs.SetInt("game_CameraSpeed_Keyboard", game_CameraSpeed_Keyboard);
        PlayerPrefs.SetInt("game_AutoAttack", game_AutoAttack.ToInt());

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        //Key Setting
        keycode_Skill1 = (KeyCode)PlayerPrefs.GetInt("keycode_Skill1", (int)keycode_Skill1);
        keycode_Skill2 = (KeyCode)PlayerPrefs.GetInt("keycode_Skill2", (int)keycode_Skill2);
        keycode_Skill3 = (KeyCode)PlayerPrefs.GetInt("keycode_Skill3", (int)keycode_Skill3);
        keycode_Skill4 = (KeyCode)PlayerPrefs.GetInt("keycode_Skill4", (int)keycode_Skill4);
        smartCasting_Skill1 = PlayerPrefs.GetInt("smartCasting_Skill1", smartCasting_Skill1.ToInt()).ToBool();
        smartCasting_Skill2 = PlayerPrefs.GetInt("smartCasting_Skill2", smartCasting_Skill2.ToInt()).ToBool();
        smartCasting_Skill3 = PlayerPrefs.GetInt("smartCasting_Skill3", smartCasting_Skill3.ToInt()).ToBool();
        smartCasting_Skill4 = PlayerPrefs.GetInt("smartCasting_Skill4", smartCasting_Skill4.ToInt()).ToBool();
        keycode_Item1 = (KeyCode)PlayerPrefs.GetInt("keycode_Item1", (int)keycode_Item1);
        keycode_Item2 = (KeyCode)PlayerPrefs.GetInt("keycode_Item2", (int)keycode_Item2);
        keycode_Item3 = (KeyCode)PlayerPrefs.GetInt("keycode_Item3", (int)keycode_Item3);
        keycode_Item4 = (KeyCode)PlayerPrefs.GetInt("keycode_Item4", (int)keycode_Item4);
        keycode_Item5 = (KeyCode)PlayerPrefs.GetInt("keycode_Item5", (int)keycode_Item5);
        keycode_Item6 = (KeyCode)PlayerPrefs.GetInt("keycode_Item6", (int)keycode_Item6);
        smartCasting_Item1 = PlayerPrefs.GetInt("smartCasting_Item1", smartCasting_Item1.ToInt()).ToBool();
        smartCasting_Item2 = PlayerPrefs.GetInt("smartCasting_Item2", smartCasting_Item2.ToInt()).ToBool();
        smartCasting_Item3 = PlayerPrefs.GetInt("smartCasting_Item3", smartCasting_Item3.ToInt()).ToBool();
        smartCasting_Item4 = PlayerPrefs.GetInt("smartCasting_Item4", smartCasting_Item4.ToInt()).ToBool();
        smartCasting_Item5 = PlayerPrefs.GetInt("smartCasting_Item5", smartCasting_Item5.ToInt()).ToBool();
        smartCasting_Item6 = PlayerPrefs.GetInt("smartCasting_Item6", smartCasting_Item6.ToInt()).ToBool();
        keycode_Attack = (KeyCode)PlayerPrefs.GetInt("keycode_Attack", (int)keycode_Attack);
        keycode_Move = (KeyCode)PlayerPrefs.GetInt("keycode_Move", (int)keycode_Move);
        keycode_LearnSkill = (KeyCode)PlayerPrefs.GetInt("keycode_LearnSkill", (int)keycode_LearnSkill);
        keycode_StopAction = (KeyCode)PlayerPrefs.GetInt("keycode_StopAction", (int)keycode_StopAction);
        keycode_DetailStatus = (KeyCode)PlayerPrefs.GetInt("keycode_DetailStatus", (int)keycode_DetailStatus);
        keycode_Store = (KeyCode)PlayerPrefs.GetInt("keycode_Store", (int)keycode_Store);
        keycode_Information = (KeyCode)PlayerPrefs.GetInt("keycode_Information", (int)keycode_Information);
        keycode_Setting = (KeyCode)PlayerPrefs.GetInt("keycode_Setting", (int)keycode_Setting);
        keycode_FixedCamera = (KeyCode)PlayerPrefs.GetInt("keycode_FixedCamera", (int)keycode_FixedCamera);
        keycode_FollowCamera = (KeyCode)PlayerPrefs.GetInt("keycode_FollowCamera", (int)keycode_FollowCamera);

        //Screen Setting
        resolution_Width = PlayerPrefs.GetInt("resolution_Width", resolution_Width);
        resolution_Height = PlayerPrefs.GetInt("resolution_Height", resolution_Height);
        resolution_fullScreen = PlayerPrefs.GetInt("resolution_fullScreen", resolution_fullScreen);

        //Sound Setting
        sound_Master = PlayerPrefs.GetInt("sound_Master", sound_Master);
        sound_Music = PlayerPrefs.GetInt("sound_Music", sound_Music);
        sound_Voice = PlayerPrefs.GetInt("sound_Voice", sound_Voice);
        sound_FX = PlayerPrefs.GetInt("sound_FX", sound_FX);
        sound_Enviroment = PlayerPrefs.GetInt("sound_Enviroment", sound_Enviroment);
        sound_Master_Use = PlayerPrefs.GetInt("sound_Master_Use", sound_Master_Use.ToInt()).ToBool();
        sound_Music_Use = PlayerPrefs.GetInt("sound_Music_Use", sound_Music_Use.ToInt()).ToBool();
        sound_Voice_Use = PlayerPrefs.GetInt("sound_Voice_Use", sound_Voice_Use.ToInt()).ToBool();
        sound_FX_Use = PlayerPrefs.GetInt("sound_FX_Use", sound_FX_Use.ToInt()).ToBool();
        sound_Enviroment_Use = PlayerPrefs.GetInt("sound_Enviroment_Use", sound_Enviroment_Use.ToInt()).ToBool();

        //Interface Setting
        interface_InterfaceSize = PlayerPrefs.GetInt("interface_InterfaceSize", interface_InterfaceSize);
        interface_MinimapSize = PlayerPrefs.GetInt("interface_MinimapSize", interface_MinimapSize);
        interface_HealthBar = PlayerPrefs.GetInt("interface_HealthBar", interface_HealthBar.ToInt()).ToBool();
        interface_CrowdControlStateUI = PlayerPrefs.GetInt("interface_CrowdControlStateUI", interface_CrowdControlStateUI.ToInt()).ToBool();
        interface_DamagedFlicker = PlayerPrefs.GetInt("interface_DamagedFlicker", interface_DamagedFlicker.ToInt()).ToBool();
        interface_CrowdControlFlicker = PlayerPrefs.GetInt("interface_CrowdControlFlicker", interface_CrowdControlFlicker.ToInt()).ToBool();
        interface_CenterMark = PlayerPrefs.GetInt("interface_CenterMark", interface_CenterMark.ToInt()).ToBool();
        interface_TargetOutline = PlayerPrefs.GetInt("interface_TargetOutline", interface_TargetOutline.ToInt()).ToBool();
        interface_ProjectileOrbit = PlayerPrefs.GetInt("interface_ProjectileOrbit", interface_ProjectileOrbit.ToInt()).ToBool();
        interface_AttackRange = PlayerPrefs.GetInt("interface_AttackRange", interface_AttackRange.ToInt()).ToBool();
        interface_SkillCost = PlayerPrefs.GetInt("interface_SkillCost", interface_SkillCost.ToInt()).ToBool();

        //Game Setting
        game_MouseSpeed = PlayerPrefs.GetInt("game_MouseSpeed", game_MouseSpeed);
        game_CameraSpeed_Mouse = PlayerPrefs.GetInt("game_CameraSpeed_Mouse", game_CameraSpeed_Mouse);
        game_CameraSpeed_Keyboard = PlayerPrefs.GetInt("game_CameraSpeed_Keyboard", game_CameraSpeed_Keyboard);
        game_AutoAttack = PlayerPrefs.GetInt("game_AutoAttack", game_AutoAttack.ToInt()).ToBool();

        isLoaded = true;
    }

    public float CalcFxValue()
    {
        if (!sound_FX_Use || !sound_Master_Use) return 0f;
        return (sound_FX * 0.01f) * (sound_Master * 0.01f);
    }
}
