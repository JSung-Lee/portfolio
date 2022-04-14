using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option_Sound : MonoBehaviour
{
    public Slider sound_Master;
    public Toggle sound_Master_Use;
    public Text sound_Master_Text;
    [HideInInspector] public int default_Sound_Master = 75;
    [HideInInspector] public bool default_Sound_Master_Use = true;

    public Slider sound_Music;
    public Toggle sound_Music_Use;
    public Text sound_Music_Text;
    [HideInInspector] public int default_Sound_Music = 75;
    [HideInInspector] public bool default_Sound_Music_Use = true;

    public Slider sound_Voice;
    public Toggle sound_Voice_Use;
    public Text sound_Voice_Text;
    [HideInInspector] public int default_Sound_Voice = 75;
    [HideInInspector] public bool default_Sound_Voice_Use = true;

    public Slider sound_FX;
    public Toggle sound_FX_Use;
    public Text sound_FX_Text;
    [HideInInspector] public int default_Sound_FX = 75;
    [HideInInspector] public bool default_Sound_FX_Use = true;

    public Slider sound_Enviroment;
    public Toggle sound_Enviroment_Use;
    public Text sound_Enviroment_Text;
    [HideInInspector] public int default_Sound_Enviroment = 75;
    [HideInInspector] public bool default_Sound_Enviroment_Use = true;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSoundOptions();
    }

    public void InitializeSoundOptions()
    {
        sound_Master.value = SettingManager.self.sound_Master;
        sound_Music.value = SettingManager.self.sound_Music;
        sound_Voice.value = SettingManager.self.sound_Voice;
        sound_FX.value = SettingManager.self.sound_FX;
        sound_Enviroment.value = SettingManager.self.sound_Enviroment;

        sound_Master_Use.isOn = SettingManager.self.sound_Master_Use;
        sound_Music_Use.isOn = SettingManager.self.sound_Music_Use;
        sound_Voice_Use.isOn = SettingManager.self.sound_Voice_Use;
        sound_FX_Use.isOn = SettingManager.self.sound_FX_Use;
        sound_Enviroment_Use.isOn = SettingManager.self.sound_Enviroment_Use;

        sound_Master.onValueChanged.SetListener(OnValueChangedSoundMaster);
        sound_Music.onValueChanged.SetListener(OnValueChangedSoundMusic);
        sound_Voice.onValueChanged.SetListener(OnValueChangedSoundVoice);
        sound_FX.onValueChanged.SetListener(OnValueChangedSoundFX);
        sound_Enviroment.onValueChanged.SetListener(OnValueChangedSoundEnviroment);

        sound_Master_Use.onValueChanged.SetListener(OnValueChangedSoundMasterUse);
        sound_Music_Use.onValueChanged.SetListener(OnValueChangedSoundMusicUse);
        sound_Voice_Use.onValueChanged.SetListener(OnValueChangedSoundVoiceUse);
        sound_FX_Use.onValueChanged.SetListener(OnValueChangedSoundFXUse);
        sound_Enviroment_Use.onValueChanged.SetListener(OnValueChangedSoundEnviromentUse);

        sound_Master.onValueChanged?.Invoke(sound_Master.value);
        sound_Music.onValueChanged?.Invoke(sound_Music.value);
        sound_Voice.onValueChanged?.Invoke(sound_Voice.value);
        sound_FX.onValueChanged?.Invoke(sound_FX.value);
        sound_Enviroment.onValueChanged?.Invoke(sound_Enviroment.value);

        sound_Master_Use.onValueChanged?.Invoke(sound_Master_Use.isOn);
        sound_Music_Use.onValueChanged?.Invoke(sound_Music_Use.isOn);
        sound_Voice_Use.onValueChanged?.Invoke(sound_Voice_Use.isOn);
        sound_FX_Use.onValueChanged?.Invoke(sound_FX_Use.isOn);
        sound_Enviroment_Use.onValueChanged?.Invoke(sound_Enviroment_Use.isOn);
    }
    public void SetAllSoundDefault()
    {
        sound_Master.value = default_Sound_Master;
        sound_Music.value = default_Sound_Music;
        sound_Voice.value = default_Sound_Voice;
        sound_FX.value = default_Sound_FX;
        sound_Enviroment.value = default_Sound_Enviroment;

        sound_Master_Use.isOn = default_Sound_Master_Use;
        sound_Music_Use.isOn = default_Sound_Music_Use;
        sound_Voice_Use.isOn = default_Sound_Voice_Use;
        sound_FX_Use.isOn = default_Sound_FX_Use;
        sound_Enviroment_Use.isOn = default_Sound_Enviroment_Use;

        sound_Master.onValueChanged?.Invoke(sound_Master.value);
        sound_Music.onValueChanged?.Invoke(sound_Music.value);
        sound_Voice.onValueChanged?.Invoke(sound_Voice.value);
        sound_FX.onValueChanged?.Invoke(sound_FX.value);
        sound_Enviroment.onValueChanged?.Invoke(sound_Enviroment.value);

        sound_Master_Use.onValueChanged?.Invoke(sound_Master_Use.isOn);
        sound_Music_Use.onValueChanged?.Invoke(sound_Music_Use.isOn);
        sound_Voice_Use.onValueChanged?.Invoke(sound_Voice_Use.isOn);
        sound_FX_Use.onValueChanged?.Invoke(sound_FX_Use.isOn);
        sound_Enviroment_Use.onValueChanged?.Invoke(sound_Enviroment_Use.isOn);
    }

    public void OnValueChangedSoundMaster(float value)
    {
        SettingManager.self.sound_Master = (int)(value);
        sound_Master_Text.text = ((int)value).ToString();
    }
    public void OnValueChangedSoundMusic(float value)
    {
        SettingManager.self.sound_Music = (int)(value);
        sound_Music_Text.text = ((int)value).ToString();
    }
    public void OnValueChangedSoundVoice(float value)
    {
        SettingManager.self.sound_Voice = (int)(value);
        sound_Voice_Text.text = ((int)value).ToString();
    }
    public void OnValueChangedSoundFX(float value)
    {
        SettingManager.self.sound_FX = (int)(value);
        sound_FX_Text.text = ((int)value).ToString();
    }
    public void OnValueChangedSoundEnviroment(float value)
    {
        SettingManager.self.sound_Enviroment = (int)(value);
        sound_Enviroment_Text.text = ((int)value).ToString();
    }

    public void OnValueChangedSoundMasterUse(bool value)
    {
        SettingManager.self.sound_Master_Use = value;
    }
    public void OnValueChangedSoundMusicUse(bool value)
    {
        SettingManager.self.sound_Music_Use = value;
    }
    public void OnValueChangedSoundVoiceUse(bool value)
    {
        SettingManager.self.sound_Voice_Use = value;
    }
    public void OnValueChangedSoundFXUse(bool value)
    {
        SettingManager.self.sound_FX_Use = value;
    }
    public void OnValueChangedSoundEnviromentUse(bool value)
    {
        SettingManager.self.sound_Enviroment_Use = value;
    }
}
