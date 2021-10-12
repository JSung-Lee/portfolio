using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageSelect : MonoBehaviour
{
    private Canvas can;
    private InputField[] input;
    public static GameObject Popup;
    int[] num_stage;

    public static Hashtable HT1;
    public static Hashtable HT2;
    private int inStageText;
    

    public void LoadPlayScene()
    {
        StageManager.Clock = 100;
        StageManager.stage = 40;
        SceneManager.LoadScene("LoadingSceneToGame");
    }

    // Use this for initialization

    public static void NextStage()
    {
        Controller.MoveOn = false;
        //StageManager.stage += 1;
        SceneManager.LoadScene("LoadingSceneToGame");
    }

    //public static void Retry()
    //{
    //    Controller.MoveOn = false;
    //    SceneManager.LoadScene("LoadingSceneToGame");
    //}

    public static void Menu()
    {
        PlayerPrefs.SetInt("nCheckClock", 0);
        PlayerPrefs.SetInt("nCheckVision", 0);
        PlayerPrefs.SetInt("nCheckRevival", 0);
        Time.timeScale = 1;
        SceneManager.LoadScene("LoadingSceneToLevel");

    }

    public static void Resume()
    {
        StageManager.Canvas_Option.gameObject.SetActive(false);
        StageManager.Canvas_Option.transform.Find("Button/Btn_Resume").GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        //if(Camera.main.GetComponent<FollowCam>().enabled || DirectionManager.Tutorial_Progress)
        Time.timeScale = 1;
    }
    public static void Stop()
    {
        Time.timeScale = 0;
        StageManager.Canvas_Option.gameObject.SetActive(true);
        
    }
    public static void Map()
    {
        if (!Camera.main.GetComponent<FollowCam>().enabled)
            return;
        StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").gameObject.SetActive(false);
        StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").gameObject.SetActive(true);
        Camera.main.GetComponent<FollowCam>().enabled = false;
        //Time.timeScale = 0;
        //Controller.MoveOn = false;
        StageManager.isMapClick = true;
        //StageManager.Player_Current.SetActive(false);
        if (StageManager.stage > 0 && StageManager.stage <= 50)
        {
            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("x", 5.5f, "y", 25f, "z", 7.5f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
            iTween.RotateTo(Camera.main.gameObject, iTween.Hash("x", 90f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
        }
        if (StageManager.stage > 50 && StageManager.stage <= 100)
        {
            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("x", 8f, "y", 35f, "z", 10.5f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
            iTween.RotateTo(Camera.main.gameObject, iTween.Hash("x", 90f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
        }
        if (StageManager.stage > 100 && StageManager.stage <= 120)
        {
            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("x", 10.5f, "y", 44.5f, "z", 13.5f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
            iTween.RotateTo(Camera.main.gameObject, iTween.Hash("x", 90f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
        }
    }
    public static void MapResume()
    {
        HT1["position"] = StageManager.Player_Current.transform.position + StageManager.visionPos;
        StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").gameObject.SetActive(true);
        StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").gameObject.SetActive(false);
        iTween.MoveTo(Camera.main.gameObject, HT1);
        iTween.RotateTo(Camera.main.gameObject, HT2);
        //StageManager.Player_Current.SetActive(true);
    }

}
