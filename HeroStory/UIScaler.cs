using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    public static ScreenOrientation orientation;    //화면 방향
    public static float _scale;
    public RectTransform[] UIs;      //UI
    //제작에 기준이 되었던 해상도 사이즈
    public Vector2 baseScreenSize = new Vector2(1920, 1080);
    //가장 최근 설정된 해상도
    Vector3 currentResolution;
    public bool useMaxRatio = false;
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SetResolution(Screen.width, Screen.height);
    }

    //_w * _h 해상도로 변경
    void SetResolution(int _w, int _h)
    {
        if (_w > _h)
            orientation = ScreenOrientation.LandscapeLeft;
        else 
            orientation = ScreenOrientation.Portrait;

        //해상도 변경
        ResolutionManager.self.SetResolution(_w, _h);

        //가로 세로 두 비율 중 작은 최소한의 비율만 UI에 적용
        if(useMaxRatio)
            _scale = ((_w / baseScreenSize.x) > (_h / baseScreenSize.y)) ? (_w / baseScreenSize.x) : (_h / baseScreenSize.y);
        else
            _scale = ((_w / baseScreenSize.x) < (_h / baseScreenSize.y)) ? (_w / baseScreenSize.x) : (_h / baseScreenSize.y);
        for (int i = 0; i < UIs.Length; i++)
        {
            UIs[i].localScale = new Vector3(1, 1, 1) * _scale;
            UIs[i].sizeDelta = new Vector2(_w, _h) / _scale;
        }
        currentResolution = new Vector3(_w, _h);
    }

    private void Update()
    {
        //설정한 해상도와 현재 해상도가 달라지면 다시 해상도 설정
        if (Screen.width != currentResolution.x || Screen.height != currentResolution.y)
        {
            SetResolution(Screen.width, Screen.height);
        }
    }
    //세로 해상도로 설정 버튼 클릭시 호출 함수
    public void OnClickPortrait()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        int width = Screen.width < Screen.height ? Screen.width : Screen.height;
        int height = Screen.width > Screen.height ? Screen.width : Screen.height;
        ResolutionManager.self.SetResolution(height, width);
    }
    //가로 해상도로 설정 버튼 클릭시 호출 함수
    public void OnClickLandscape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        int width = Screen.width > Screen.height ? Screen.width : Screen.height;
        int height = Screen.width < Screen.height ? Screen.width : Screen.height;
        ResolutionManager.self.SetResolution(height, width);
    }

}
