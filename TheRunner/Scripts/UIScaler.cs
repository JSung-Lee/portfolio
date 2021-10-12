using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour {
    public RectTransform uiScaler;
    public CanvasScaler canvasScaler;
    public static float screenRatio;
    public static float screenSizeWidth;
    public static float screenSizeHeight;
    // Use this for initialization
    void OnEnable() {
        float xRatio = Screen.width / 1080.0f;
        float yRatio = Screen.height / 1920.0f;
        if (xRatio > yRatio)
            xRatio = yRatio;
        uiScaler.localScale = new Vector3(xRatio, xRatio, 1.0f);
        //Debug.Log("Screen - width : " + Screen.width.ToString()
        //             + " , height : " + Screen.height.ToString());
        //Debug.Log("Ratio : " + xRatio.ToString() + " , " + yRatio.ToString());
        screenRatio = xRatio;
        screenSizeWidth = Screen.width * screenRatio;
        screenSizeHeight = Screen.height * screenRatio;
    }

}
