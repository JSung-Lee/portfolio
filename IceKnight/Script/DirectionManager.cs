using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DirectionManager : MonoBehaviour
{
    public static GameObject directionManager;
    public static int tutorialState;
    public static bool Tutorial_Progress;
    private GameObject TutorialPrefab;
    private GameObject TutorialPopup;
    private GameObject RevivalPrefab;
    private GameObject RevivalPopup;
    private GameObject currentFX;

    string[] str;
    public void Awake()
    {
        TutorialPrefab = Resources.Load<GameObject>("Prefab/UI/TutorialPopup");
        RevivalPrefab = Resources.Load<GameObject>("Prefab/UI/RevivalPopUp");
        directionManager = gameObject;
        tutorialState = 0;
        Tutorial_Progress = false;
    }

    public void startDirection()
    {
        Camera.main.GetComponent<FollowCam>().enabled = false;
        //StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").GetComponent<Button>().enabled = false;
        Vector3 pos, rot;
        switch (Controller.pState)
        {
            case 1:
                pos = StageManager.Player_Current.transform.position + new Vector3(0.0f, 4.0f, 3.0f);
                rot = new Vector3(40, 180, 0);
                iTween.MoveTo(Camera.main.gameObject, pos, 1.5f);
                iTween.RotateTo(Camera.main.gameObject, rot, 1.5f);

                break;
            case 2:
                pos = StageManager.Player_Current.transform.position + new Vector3(3.0f, 4.0f, 0.0f);
                rot = new Vector3(40, 270, 0);
                iTween.MoveTo(Camera.main.gameObject, pos, 1.5f);
                iTween.RotateTo(Camera.main.gameObject, rot, 1.5f);

                break;
            case 3:
                pos = StageManager.Player_Current.transform.position + new Vector3(0.0f, 4.0f, -3.0f);
                rot = new Vector3(40, 0, 0);
                iTween.MoveTo(Camera.main.gameObject, pos, 1.5f);
                iTween.RotateTo(Camera.main.gameObject, rot, 1.5f);

                break;
            case 4:
                pos = StageManager.Player_Current.transform.position + new Vector3(-3.0f, 4.0f, 0.0f);
                rot = new Vector3(40, 90, 0);
                iTween.MoveTo(Camera.main.gameObject, pos, 1.5f);
                iTween.RotateTo(Camera.main.gameObject, rot, 1.5f);

                break;
        }
        Invoke("viewFullMap", 1.5f);
    }

    void viewFullMap()
    {
        if (StageManager.stage <= 50)
        {
            iTween.MoveTo(Camera.main.gameObject, new Vector3(5.5f, 25f, -1.5f), 1.5f);
        }
        else if (StageManager.stage <= 100)
        {
            iTween.MoveTo(Camera.main.gameObject, new Vector3(8.0f, 35f, -1.5f), 1.5f);
        }
        else
        {
            iTween.MoveTo(Camera.main.gameObject, new Vector3(10.5f, 44.5f, -1.5f), 1.5f);
        }
        iTween.RotateTo(Camera.main.gameObject, new Vector3(70f, 0f, 0f), 1.5f);
        Invoke("viewCharacter", 1.5f);
    }

    void viewCharacter()
    {
        iTween.MoveTo(Camera.main.gameObject, StageManager.Player_Current.transform.position + StageManager.visionPos, 1.5f);
        iTween.RotateTo(Camera.main.gameObject, new Vector3(70, 0, 0), 1.5f);
        Invoke("startGame", 1.5f);
    }

    void startGame()
    {
        //Debug.Log(PlayerPrefs.GetInt("nStage"));
        // 이부분 수정함
        if (StageManager.stage < 40 && PlayerPrefs.GetInt("nStage") < StageManager.stage)
        {
            if (StageManager.stage == 1 || StageManager.stage == 6 || StageManager.stage == 11 || StageManager.stage == 16 || StageManager.stage == 21 || StageManager.stage == 26 || StageManager.stage == 31 || StageManager.stage == 36)
            {
                if (tutorialState == 0)
                {
                    StartTutorial();
                    Tutorial_Progress = true;
                }
                else
                { 
                    Camera.main.GetComponent<FollowCam>().enabled = true;
                    Tutorial_Progress = false;
                    StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").GetComponent<Button>().onClick.AddListener(StageSelect.Map);

                    StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").GetComponent<Button>().onClick.AddListener(StageSelect.MapResume);

                    StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").gameObject.SetActive(true);
                    StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").gameObject.SetActive(false);
                }
            }
            else
            {
                Camera.main.GetComponent<FollowCam>().enabled = true;
                StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").GetComponent<Button>().onClick.AddListener(StageSelect.Map);

                StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").GetComponent<Button>().onClick.AddListener(StageSelect.MapResume);

                StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").gameObject.SetActive(true);
                StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").gameObject.SetActive(false);
            }
                
        }
        // 여기까지 수정함
        else
        {
            Camera.main.GetComponent<FollowCam>().enabled = true;
            
            StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").GetComponent<Button>().onClick.AddListener(StageSelect.Map);

            StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").GetComponent<Button>().onClick.AddListener(StageSelect.MapResume);

            StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").gameObject.SetActive(true);
            StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").gameObject.SetActive(false);

            //StageManager.Canvas_Stop.transform.Find("Btn_Map/Button").GetComponent<Button>().enabled = true;
            //StageManager.Canvas_Stop.transform.Find("Btn_Resume/Button").GetComponent<Button>().enabled = true;
        }
    }
    // 아래 함수들 전부 추가함(0529).
    void StartTutorial()
    {
        // 여기서부터
        switch (StageManager.stage)
        { // 한줄 16개
            case 1:
                str = new string[6];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "안녕하세요 용사님! 저는 용사님이 마녀의 성을 통과할 수 있게 도움을 주러 왔어요!";         //
                str[1] = "화면을 상, 하, 좌, 우로 밀면 이동 할 수 있어요.";
                str[2] = "캐릭터가 이동하면 물체에 충돌할 때 까지 계속 이동하게 되요.";
                str[3] = "이것은 열쇠에요! 이 열쇠가 있어야 출구로 나갈 수 있어요";
                str[4] = "여기가 출구에요! 열쇠를 가지고 도착하면 마녀의 성을 나갈 수 있어요";
                str[5] = "그럼 이제 마녀의 성을 통과하여 얼음마녀에게 납치된 공주님을 꼭 구해주세요!";
                zoomObject();
                break;
            case 6:
                str = new string[4];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "무사히 오셨네요! 새로운 장애물이 나타났어요";
                str[1] = "여기에 방패가 있네요! 이 방패가 있으면 바위를 밀 수 있어요!";
                str[2] = "이 바위가 방패로 밀 수 있는 바위에요. 바위를 잘 못 밀어내면 성을 나갈 수 없을지도 몰라요.";
                str[3] = "바위를 잘 밀어 마녀의 성을 통과하세요!";
                zoomObject();
                break;
            case 11:
                str = new string[4];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "무사히 오셨네요! 새로운 장애물이 나타났어요";
                str[1] = "여기에 망치가 있네요! 이 망치가 있으면 얼음을 깰 수 있어요!";
                str[2] = "이건 망치로 깰 수 있는 얼음이에요. 얼음을 잘 못 깨면 마녀의 성을 나갈 수 없을지도 몰라요.";
                str[3] = "깰 얼음을 잘 선택하여 마녀의 성을 통과하세요!";
                zoomObject();
                break;
            case 16:
                str = new string[3];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "무사히 오셨어요! 여기는 무서운 장애물이 있네요.";
                str[1] = "이 장애물에 닿으면 날카로운 가시가 용사님을 해칠 수 있어요. 상하좌우로 움직이니 조심하세요.";
                str[2] = "다치지 않고 이 곳을 통과할 수 있기를 빌게요.";
                zoomObject();
                break;
            case 21:
                str = new string[3];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "무사히 오셨어요! 여기는 또 다른 무서운 장애물이 있네요";
                str[1] = "이 장애물도 닿으면 용사님을 해칠 수 있어요. 하지만 움직이지는 않으니 앞만 조심하세요.";
                str[2] = "앞으로 갈 길이 멀어요! 계속 힘내주세요!";
                zoomObject();
                break;
            case 26:
                str = new string[3];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "여기는 특수한 마법이 걸린 곳이에요.";
                str[1] = "이 바닥을 밟으면 특정 방향으로 움직이게 되요. 특수한 신발을 얻는다면 저 마법을 풀 수 있어요.";
                str[2] = "마법을 잘 이용한다면 많은 도움이 될 수 있어요!";
                zoomObject();
                break;
            case 31:
                str = new string[4];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "여기도 특수한 마법이 걸린 곳이네요.";
                str[1] = "이 포탈에 들어가면 다른 포탈로 순간이동해요";
                str[2] = "이 포탈로 순간이동하게 될 거에요. 하지만 이 포탈에는 들어갈 수 없어요.";
                str[3] = "이 마법도 잘 이용한다면 많은 도움이 될 수 있어요!";
                zoomObject();
                break;
            case 36:
                str = new string[4];
                //줌 목표 obj 배열 생성
                //대사 배열 생성
                str[0] = "여기는 비슷하지만 조금 다른 마법이 걸린 곳이에요";
                str[1] = "이 포탈도 들어가면 다른 포탈로 순간이동해요";
                str[2] = "이 포탈로 순간이동하게 될 거에요. 그리고 이 포탈로도 들어갈 수 있어요.";
                str[3] = "저의 도움은 여기까지에요! 부디 무사히 공주님을 구출해주세요.";
                zoomObject();
                break;
        }
        // 여기까지 수정함
    }

    public void zoomObject()
    {
        if (tutorialState < StageManager.zoomObj.Length)
        {
            if (StageManager.stage == 16)
                IceBall.iceTime = 0;

            if (tutorialState != 0)
            {
                if (StageManager.zoomObj[tutorialState] != StageManager.zoomObj[tutorialState - 1])
                {
                    iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", StageManager.zoomObj[tutorialState].transform.position + Vector3.up * 5f + Vector3.back * 1.8f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
                    iTween.RotateTo(Camera.main.gameObject, iTween.Hash("x", 70f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
                    Invoke("popup", 1f);
                }
                else
                    popup();
            }
            else
            {
                iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", StageManager.zoomObj[tutorialState].transform.position + Vector3.up * 5f + Vector3.back * 1.8f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
                iTween.RotateTo(Camera.main.gameObject, iTween.Hash("x", 70f, "time", 1f, "islocal", true, "movetopath", false, "ignoretimescale", true, "easetype", iTween.EaseType.easeInOutSine));
                Invoke("popup", 1f);
            }

            //무브투(오브젝트[tutorialState])
            //무브투 시간 후 popup(str[tutorialState)
        }
        else
        {
            viewCharacter();
        }
    }

    void popup()
    {
        if (tutorialState < str.Length)
        {
            TutorialPopup = Instantiate(TutorialPrefab, StageManager.Canvas_Stop.transform);
            Text txt = TutorialPopup.transform.Find("Text").GetComponent<Text>();
            TutorialPopup.transform.Find("Btn_Next").gameObject.SetActive(false);
            txt.text = str[tutorialState];
            txt.GetComponent<WritterAnimation>().startAnim();
            StartCoroutine(checkDone());

        }
    }
    private IEnumerator checkDone()
    {
        while (!WritterAnimation.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        TutorialPopup.transform.Find("Btn_Next").gameObject.SetActive(true);
        IceBall.iceTime = 1;
    }
    // 여기까지 수정함

    public void OpenRevivlaPopup()
    {
        RevivalPopup = Instantiate(RevivalPrefab, StageManager.Canvas_Stop.transform);
        RevivalPopup.transform.Find("No Button").GetComponent<Button>().onClick.AddListener(dontRevival);
        RevivalPopup.transform.Find("Yes Button").GetComponent<Button>().onClick.AddListener(startRevival);
        RevivalPopup.gameObject.SetActive(true);
        StageManager.Player_Current.GetComponent<BoxCollider>().enabled = false;
    }

    public void startRevival() 
    {
        Controller.anim.speed = 0;
        RevivalPopup.gameObject.SetActive(false);
        Time.timeScale = 1;
        Controller.playerPosition = Controller.lastPlayerPosition;
        iTween.MoveTo(Controller.tr.gameObject,Controller.playerPosition, 2.0f);
        Invoke("upCharacter", 2.0f);
    }

    void upCharacter()
    {
        currentFX = Instantiate(Resources.Load<GameObject>("Prefab/FX/MagicGlowGroundPortalYellow"), Controller.playerPosition, Quaternion.Euler(0, 0, 0));
        Controller.anim.speed = 1.0f;
        Controller.anim.Play("Up");
        Invoke("revivalCharacter", 2.0f);
    }

    void revivalCharacter()
    {
        Controller.anim.speed = 0.5f;
        Controller.anim.Play("Revival");
        Invoke("revival", 2.8f);
    }

    void revival()
    {
        StageManager.Player_Current.GetComponent<BoxCollider>().enabled = true;
        Destroy(currentFX);
        Controller.anim.speed = 1.0f;
        Controller.isDie = false;
        Controller.nRevival += 1;
        Camera.main.GetComponent<FollowCam>().enabled = true;
    }


    public void dontRevival()
    {
        SceneManager.LoadScene("GameOverScenePortrait");
        Time.timeScale = 1;
    }
}
