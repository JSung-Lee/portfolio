using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
public class UserManager : MonoBehaviour {
    //private UserManager current;
    public static string hostUrl = "https://****";
    public static string cafe24Url = "https://****";
    public static float timeScale;
    private float score;        //인게임 중 사용될 점수
    private float bestScore;    //최고 점수

    private int combo;          //인게임 콤보
    private int bestCombo;      //최고 콤보

    private float distance;     //인게임 거리 점수
    private float bestDistance; //최고 거리 점수

    private int BGM;            //브금 소리 On/Off
    private int effect;         //효과음 On/Off
    public int isBeginner;
    public int firstPlay;      //최초 실행 여부
    public float version;
    public string nickname;
    public string imallid;
    public string phoneNumber;
    public GameObject reTryBoard;
    public GameObject reTryBoard_Button;
    public Text reTryBoard_text;

    public float pigEvent;
    // Use this for initialization
    void OnEnable () {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        version = 1.84f;
        //resetData();
        //saveData();
        getData();
    }

    public void saveData()
    {
        PlayerPrefs.SetFloat("bestScore", bestScore);
        PlayerPrefs.SetInt("bestCombo", bestCombo);
        PlayerPrefs.SetFloat("bestDistance", bestDistance);
        PlayerPrefs.SetFloat("pigEvent", pigEvent);
        PlayerPrefs.SetInt("BGM", BGM);
        PlayerPrefs.SetInt("effect", effect);
        PlayerPrefs.SetInt("firstPlay", firstPlay);
        PlayerPrefs.SetInt("isBeginner", isBeginner);
        PlayerPrefs.SetString("nickname", nickname);
        PlayerPrefs.SetString("imallid", imallid);
        PlayerPrefs.SetString("phoneNumber", phoneNumber);
    }

    public void createAccount(string nick, string id, string phone)
    {
        firstPlay = 1;
        nickname = nick;
        imallid = id;
        phoneNumber = phone;
        saveData();
    }

    void getData()
    {
        firstPlay = PlayerPrefs.GetInt("firstPlay");
        if (firstPlay == 0)
        {
            initData();
            saveData();
        }
        else
        {
            bestScore = PlayerPrefs.GetFloat("bestScore");
            bestCombo = PlayerPrefs.GetInt("bestCombo");
            bestDistance= PlayerPrefs.GetFloat("bestDistance");
            pigEvent = PlayerPrefs.GetFloat("pigEvent");
            BGM = PlayerPrefs.GetInt("BGM");
            effect = PlayerPrefs.GetInt("effect");
            isBeginner = PlayerPrefs.GetInt("isBeginner");
            nickname = PlayerPrefs.GetString("nickname");
            imallid = PlayerPrefs.GetString("imallid");
            phoneNumber = PlayerPrefs.GetString("phoneNumber");
        }

    }

    public void resetData()
    {
        initData();
        PlayerPrefs.SetFloat("bestScore", bestScore);
        PlayerPrefs.SetInt("bestCombo", bestCombo);
        PlayerPrefs.SetFloat("bestDistance", bestDistance);
        PlayerPrefs.SetFloat("pigEvent", pigEvent);
        PlayerPrefs.SetInt("BGM", BGM);
        PlayerPrefs.SetInt("effect", effect);
        PlayerPrefs.SetInt("firstPlay", firstPlay);
        PlayerPrefs.SetString("nickname", nickname);
        PlayerPrefs.SetString("imallid", imallid);
        PlayerPrefs.SetString("phoneNumber", phoneNumber);
        PlayerPrefs.SetInt("isBeginner", isBeginner);
    }
 
    private void OnDestroy()
    {
        saveData();
    }

    public void initData()
    {
        bestScore = score = 0;
        bestDistance = distance = 0;
        bestCombo = combo = 0;
        firstPlay = 0;
        isBeginner = 0;
        BGM = effect = 1;
        pigEvent = 0.0f;
        nickname = "";
        imallid = "";
        phoneNumber = "";
    }

    public void reStart()
    {
        score = distance = 0;
        combo = 0;
    }
    public void setScore(float s) { score = s; }
    public void plusScore(float s) { score += s; }
    public float getScore() { return score; }
    public void setBestScore(float bs)
    {
        bestScore = bs;
        if (bestScore == 0)
        {
            bestCombo = 0;
            bestDistance = 0;
        }
    }
    public float getBestScore() { return bestScore; }

    public void setCombo(int c) { combo = c; }
    public void plusCombo(int c) { combo += c; }
    public int getCombo() { return combo; }
    public void setBestCombo(int bc) { bestCombo = bc; }
    public int getBestCombo() { return bestCombo; }

    public void setDistance(float d) { distance = d; }
    public void plusDistance(float d) { distance += d; }
    public float getDistance() { return distance; }
    public void setBestDistance(float bd) { bestDistance = bd; }
    public float getBestDistance() { return bestDistance; }

    public void setBGM(int b) { BGM = b; PlayerPrefs.SetInt("BGM", BGM); }
    public int getBGM() { return BGM; }
    public void setEffect(int e) { effect = e; PlayerPrefs.SetInt("effect", effect); }
    public int getEffect() { return effect; }

    public void scoreUpdate()
    {
        if (score < bestScore)
            return;
        reTryBoard_text.text = "\n랭킹등록 중입니다.\n" + 
            "잠시만 기다려 주십시오.";
        reTryBoard_Button.SetActive(false);
        reTryBoard.SetActive(true);
        StartCoroutine(SendScoreUpdate(score));
    }

    public void retryScoreUpdate()
    {
        reTryBoard.SetActive(false);
        StartCoroutine(SendScoreUpdate(score));
    }

    public void closeRetryBoard()
    {
        reTryBoard.SetActive(false);
    }

    IEnumerator SendScoreUpdate(float s)
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", phoneNumber);
        form.AddField("score", (int)s);
        string file = "scoreupdate.php";
        WWW webRequest = new WWW(UserManager.hostUrl + file, form);
        yield return webRequest;
        if (webRequest.isDone)
        {
            //Debug.Log(((int)s).ToString() + ", " + webRequest.text);
            if (webRequest.error == null)
            {
                //Debug.Log(webRequest.text);
                if (webRequest.text.Equals("o"))
                {
                    reTryBoard.SetActive(false);
                    bestScore = s;
                    saveData();
                }
                else if (webRequest.text.Equals("x"))
                {
                    reTryBoard_text.text = "랭킹등록에 실패 하였습니다.";
                    reTryBoard_Button.SetActive(true);
                    //reTryBoard.SetActive(true);
                }

            }
            else
            {
                reTryBoard_text.text = "랭킹등록에 실패 하였습니다.";
                reTryBoard_Button.SetActive(true);
                reTryBoard.SetActive(true);
            }
        }
    }

    public bool checkVersion(string str)
    {
        float ver = (float)System.Convert.ToDouble(str);
        if (version >= ver)
            return true;
        else
            return false;
    }
}
