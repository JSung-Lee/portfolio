using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public static ResolutionManager self;
    public int width, height;
    public FullScreenMode fullScreen;
    private void Start()
    {
        if (!self)
            self = this;
        else
            Destroy(this);
        width = SettingManager.self.resolution_Width;
        height = SettingManager.self.resolution_Height;
        fullScreen = (FullScreenMode)SettingManager.self.resolution_fullScreen;
        SetResolution(width, height, fullScreen);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;

    }
    public void SetResolution(string size, FullScreenMode full = FullScreenMode.ExclusiveFullScreen)
    {
        if (size.Contains("@"))
            size = size.Split('@')[0].Trim();
        var parse = size.Split('x');
        int w = System.Int32.Parse(parse[0].Trim());
        int h = System.Int32.Parse(parse[1].Trim());
        Debug.Log(w + ", " + h + " " + full.ToString());
        SetResolution(w, h, full);
    }

    public void SetResolution(Vector2 size, FullScreenMode full = FullScreenMode.ExclusiveFullScreen)
    {
        SetResolution((int)size.x, (int)size.y, full);
    }

    public void SetResolution(int w, int h, FullScreenMode full = FullScreenMode.ExclusiveFullScreen)
    {
        Screen.SetResolution(w, h, full);
        width = w;
        height = h;
        fullScreen = full;
        SettingManager.self.resolution_Width = width;
        SettingManager.self.resolution_Height = height;
        SettingManager.self.resolution_fullScreen = (int)fullScreen;
    }

    public void SetResolution(int w, int h)
    {
        SetResolution(w, h, fullScreen);
    }
}
