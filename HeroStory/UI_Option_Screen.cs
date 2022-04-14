using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option_Screen : MonoBehaviour
{
    public Dropdown screen_Resolution; //해상도
    public Dropdown screen_fullScreen;
    [HideInInspector] int defaultWidth = 1920;
    [HideInInspector] int defaultHeight = 1080;
    [HideInInspector] int defaultResolutionIndex = 0;

    [HideInInspector] int defaultFullScreen = 2;
    [HideInInspector] int defaultFullScreenIndex = 0;
    List<string> sR_types = new List<string>();

    public Image select_image;

    void Start()
    {   
        Set_DropDown();
    }

    public void Set_DropDown()
    {
        //드롭다운 초기화 및 값 설정
        Dropdown_ScreenResolution_Init();
    }

    public void Dropdown_ScreenResolution_Init()
    {
        //Screen의 해상도(resolutions)를 string List에 담은 뒤  Dropdown option data로 옮김
        screen_Resolution.onValueChanged.RemoveAllListeners();
        screen_Resolution.options.Clear();
        int resolution = 0;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate != 60) continue;
            if (Screen.resolutions[i].width == SettingManager.self.resolution_Width &&
                Screen.resolutions[i].height == SettingManager.self.resolution_Height)
                resolution = i;
            if (Screen.resolutions[i].width == defaultWidth &&
                Screen.resolutions[i].height == defaultHeight)
                defaultResolutionIndex = i;
            var screen_text = Screen.resolutions[i].ToString();
            screen_text = screen_text.Replace(" @ 144Hz", "");
            sR_types.Add(screen_text);
        }
        screen_Resolution.AddOptions(sR_types);
        screen_Resolution.onValueChanged.AddListener(delegate {
            ResolutionValueChanged(screen_Resolution);
        });
        screen_Resolution.SetValueWithoutNotify(resolution);

        screen_fullScreen.onValueChanged.RemoveAllListeners();
        screen_fullScreen.options.Clear();
        int fullScreen = 0;
        List<string> options = new List<string>();
        options.Add("전체 화면");
        options.Add("창 모드");
        for (int i = 2; i < 4; i++) {
            if (ResolutionManager.self.fullScreen == (FullScreenMode)i)
                fullScreen = i;
            if (ResolutionManager.self.fullScreen == (FullScreenMode)defaultFullScreen)
                defaultFullScreenIndex = i;
            //options.Add(((FullScreenMode)i).ToString());
        }
        screen_fullScreen.AddOptions(options);
        screen_fullScreen.onValueChanged.AddListener(delegate {
            ResolutionValueChanged(screen_fullScreen);
        });
        screen_fullScreen.SetValueWithoutNotify(fullScreen - 2);
    }

    public void ResolutionValueChanged(Dropdown change)
    {
        ResolutionManager.self.SetResolution(sR_types[screen_Resolution.value],
                                             (FullScreenMode)(screen_fullScreen.value + 2));
    }

    public void SetResolutionAtDefault()
    {
        screen_Resolution.SetValueWithoutNotify(defaultResolutionIndex);
        screen_Resolution.onValueChanged?.Invoke(defaultResolutionIndex);
        screen_fullScreen.SetValueWithoutNotify(defaultFullScreenIndex - 2);
        screen_fullScreen.onValueChanged?.Invoke(defaultFullScreenIndex - 2);
    }
}
