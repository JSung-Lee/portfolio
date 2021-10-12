using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageManager : MonoBehaviour
{

    public static bool isMapClick;
    // 포탈정보 담을 클래스 선언
    public class Portal
    {
        private string Typeof;
        private Vector3 posPortal;

        public Portal() { Typeof = "NULL"; posPortal.Set(0f, 0f, 0f); }
        public Portal(string kind, Vector3 pos)
        {
            Typeof = kind;
            posPortal = pos;
        }
        public void setPortal(string kind, Vector3 pos)
        {
            Typeof = kind;
            posPortal = pos;
        }
        public Vector3 getPosition() { return posPortal; }
        public string getType() { return Typeof; }
    }

    // 포탈 정보를 담는 리스트
    public static List<Portal> List_Portal = new List<Portal>();
    // 푸시큐브 정보를 담는 리스트
    public static List<GameObject> List_Push = new List<GameObject>();
    // 크러쉬큐브 정보를 담는 리스트
    public static List<GameObject> List_Crush = new List<GameObject>();
    // 플레이어 각도 저장변수
    public static Quaternion p_Angle;
    // 메모장의 줄을 세는 변수
    public static int CountZ = 0;
    public static GameObject Player_Current;

    // 메모장 데이터를 담는 변수
    private string[][] Map;
    private string[] textValue;

    // 오브젝트 위치설정 변수
    private Vector3 m_vPos;
    // 오브젝트 각도 설정 변수
    private Quaternion m_vAngle;
    // 플레이어 위치 저장변수 
    //private Vector3 p_Pos;

    public Text _guiTime;
    public static float done;
    //// Update is called once per frame



    // 프리팹 정보 저장하는 변수
    private GameObject Enter;
    private GameObject Exit;
    private GameObject Wall;
    private GameObject Floor;

    private GameObject Portal_I;
    private GameObject Portal_O;
    private GameObject Portal_T;
    private GameObject IceBallUp;
    private GameObject IceBallRight;
    private GameObject Crush;
    private GameObject Push;
    private GameObject Trap_H;

    private GameObject Player;

    private GameObject Gloves;
    private GameObject Shoes;
    private GameObject Key;
    private GameObject Mang;
    private GameObject Star;

    public static int[] chch = new int[5];
    public static float Clock;

    private GameObject Trap_D;
    private GameObject Trap_S;

    // UI 프리팹
    public static Canvas Canvas_Stop;
    public static Canvas Canvas_Resume;
    public static Canvas Canvas_Option;
    public static Button Resume;
    // 파일경로(TextAsset으로 읽을 것이기 때문에 확장자는 쓰지 않음)
    private string fileName = "Stage/stage";
    // 스테이지 정보를 담는 변수 
    public static int stage;
    // 튜토리얼 오브젝트 배열
    public static GameObject[] zoomObj;

    public static Vector3 visionPos;
    void Awake()
    {

        // 0611 하트 추가
        //Debug.Log("하트 차감 전 : " + UserManager.nHeart);
        //하트 차감
        //UserManager.nHeart -= 1;
        //Debug.Log("게임 시작. 하트 차감 후 : " + UserManager.nHeart);



        // 이부분 추가함(0529)
        switch (stage)
        {
            case 1:
                zoomObj = new GameObject[6];
                break;
            case 6:
                zoomObj = new GameObject[4];
                break;
            case 11:
                zoomObj = new GameObject[4];
                break;
            case 16:
                zoomObj = new GameObject[3];
                break;
            case 21:
                zoomObj = new GameObject[3];
                break;
            case 26:
                zoomObj = new GameObject[3];
                break;
            case 31:
                zoomObj = new GameObject[4];
                break;
            case 36:
                zoomObj = new GameObject[4];
                break;
        }
        // 여기까지 (0529)

        UserManager.nStar = 0;
        for (int i = 0; i < 5; i++)
        { chch[i] = 0; }
        Clock = 0;
        Screen.SetResolution(1080, 1920, true);
        Controller.MoveOn = false;
        isMapClick = false;
        //done = 95f + Clock;
        Time.timeScale = 1;
        //stage = 1;
        Player = Resources.Load<GameObject>("Prefab/Character/" + UserManager.characters[PlayerPrefs.GetInt("lastCharacter")]);

        Enter = Resources.Load<GameObject>("Prefab/Obstacle/Enter");
        Exit = Resources.Load<GameObject>("Prefab/Obstacle/Exit");
        Wall = Resources.Load<GameObject>("Prefab/Obstacle/Cube_Default");
        Floor = Resources.Load<GameObject>("Prefab/Obstacle/Cube_Ice");
        // 함정형(D는 방향, S는 정지 H는 균열)
        Trap_D = Resources.Load<GameObject>("Prefab/Obstacle/Trap_D");
        Trap_S = Resources.Load<GameObject>("Prefab/Obstacle/Trap_S");
        Trap_H = Resources.Load<GameObject>("Prefab/Obstacle/Trap_H");

        Portal_I = Resources.Load<GameObject>("Prefab/Obstacle/Portal_I");
        Portal_O = Resources.Load<GameObject>("Prefab/Obstacle/Portal_O");
        Portal_T = Resources.Load<GameObject>("Prefab/Obstacle/Portal_T");
        // 이동형(H는 좌우,V는 상하)
        IceBallUp = Resources.Load<GameObject>("Prefab/Obstacle/Iceball_V");
        IceBallRight = Resources.Load<GameObject>("Prefab/Obstacle/Iceball_H");

        // 조작형
        Crush = Resources.Load<GameObject>("Prefab/Obstacle/Control_B");
        Push = Resources.Load<GameObject>("Prefab/Obstacle/Control_P");
        // 습득형
        Key = Resources.Load<GameObject>("Prefab/Item_Ingame/Key");
        Gloves = Resources.Load<GameObject>("Prefab/Item_Ingame/Shield");
        Shoes = Resources.Load<GameObject>("Prefab/Item_Ingame/Shoes");
        Mang = Resources.Load<GameObject>("Prefab/Item_Ingame/Hammer");
        Star = Resources.Load<GameObject>("Prefab/Item_Ingame/Star");

        // UI 생성 및 트리거 등록
        Canvas_Stop = Instantiate(Resources.Load<Canvas>("Prefab/UI/Canvas_Stop"));
        _guiTime = Canvas_Stop.transform.Find("Time").GetComponent<Text>();

        Canvas_Stop.transform.Find("Stage_Text").transform.Find("Text Moves Amount").GetComponent<Text>().text = stage.ToString();

        Canvas_Stop.transform.Find("Btn_Map/Button").gameObject.SetActive(false);
        Canvas_Stop.transform.Find("Btn_Resume/Button").gameObject.SetActive(false);

        Canvas_Stop.transform.Find("Btn_Map/Button").GetComponent<Button>().enabled = true;
        Canvas_Stop.transform.Find("Btn_Resume/Button").GetComponent<Button>().enabled = true;
        Canvas_Stop.transform.Find("Btn_Stop").GetComponent<Button>().onClick.AddListener(StageSelect.Stop);

        Canvas_Stop.transform.Find("Btn_Map/Button").GetComponent<Button>().onClick.AddListener(StageSelect.Map);
        Resume = Canvas_Stop.transform.Find("Btn_Resume/Button").GetComponent<Button>();
        Resume.onClick.AddListener(StageSelect.MapResume);

        Canvas_Option = Instantiate(Resources.Load<Canvas>("Prefab/UI/Canvas_Option"));
        Canvas_Option.transform.Find("Button/Btn_Menu").GetComponent<Button>().onClick.AddListener(StageSelect.Menu);
        //Canvas_Option.transform.Find("Button/Btn_Retry").GetComponent<Button>().onClick.AddListener(StageSelect.Retry);
        Canvas_Option.transform.Find("Button/Btn_Resume").GetComponent<Button>().onClick.AddListener(StageSelect.Resume);
        Canvas_Option.gameObject.SetActive(false);

        List_Push = new List<GameObject>();
        List_Portal = new List<Portal>();
        List_Crush = new List<GameObject>();
        List_Push.Clear();
        List_Crush.Clear();
        List_Portal.Clear();

        visionPos = Vector3.up * 8.5f + Vector3.back * 2.1f;
        if (PlayerPrefs.GetInt("nCheckVision") == 1)
        { 
            switch (PlayerPrefs.GetInt("nVision"))
            {
                case 0:
                    visionPos *= 1.0f;
                    break;
                case 1:
                    visionPos *= 1.2f;
                    break;
                case 2:
                    visionPos *= 1.4f;
                    break;
                case 3:
                    visionPos *= 1.6f;
                    break;
                default:
                    //Debug.Log("정상적이지 않은 Vision");
                    break;
            }
        }
        else
            visionPos *= 1.0f;

        TextLoad(fileName + stage);

        m_vPos = new Vector3(0f, 0f, 0f);
        m_vAngle = new Quaternion(0f, 0f, 0f, 0f);
        //p_Pos = new Vector3(0f, 0f, 0f);
        p_Angle = new Quaternion(0f, 0f, 0f, 0f);

        MapSetting();
        StageSelect.HT1 = new Hashtable();
        StageSelect.HT2 = new Hashtable();

        StageSelect.HT1.Add("position", Player_Current.transform.position + Vector3.up * 4.5f + Vector3.back * 1.5f);
        StageSelect.HT1.Add("time", 1f);
        StageSelect.HT1.Add("oncomplete", "iTweenEnd");
        StageSelect.HT1.Add("islocal", true);
        StageSelect.HT1.Add("movetopath", false);
        StageSelect.HT1.Add("ignoretimescale", true);
        StageSelect.HT1.Add("easetype", iTween.EaseType.easeInOutSine);

        StageSelect.HT2.Add("x", 70f);
        StageSelect.HT2.Add("time", 1f);
        StageSelect.HT2.Add("islocal", true);
        StageSelect.HT2.Add("movetopath", false);
        StageSelect.HT2.Add("ignoretimescale", true);
        StageSelect.HT2.Add("easetype", iTween.EaseType.easeInOutSine);

        // 아래 로그가 시간 데이터 받아오는 코드
        //Debug.Log(textValue[0].ToString());


        // 상점에서 어떤 시계아이템을 샀는지 확인하여 시간을 추가로 줌. (중첩불가)
        //Debug.Log(PlayerPrefs.GetInt("nClock"));
        if (PlayerPrefs.GetInt("nCheckClock") == 1) { 
            switch (PlayerPrefs.GetInt("nClock")) {
                case 0:
                    Clock = int.Parse(textValue[0]) * 0.0f;
                    break;
                case 1:
                    Clock = int.Parse(textValue[0]) * 0.2f;
                    break;
                case 2:
                    Clock = int.Parse(textValue[0]) * 0.4f;
                    break;
                case 3:
                    Clock = int.Parse(textValue[0]) * 0.6f;
                    break;
                default:
                    //Debug.Log("정상적이지 않은 Clock");
                    break;
            }
        }
        else
            Clock = int.Parse(textValue[0]) * 0.0f;
        done = int.Parse(textValue[0]) + Clock;
    }
    void Update()
    {
        if (done > 0F )
        {
            if(!Controller.isDie && Camera.main.GetComponent<FollowCam>().enabled)
                done -= Time.deltaTime;
            //_guiTime.text = "Time : " + (int)done;
        }
        else
        {
            if (!Controller.anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                Controller.MoveOn = false;
                Controller.anim.SetTrigger("Die");
                Invoke("visibleGameoverCanvas", 1.5f);
            }
        }
        if (Player_Current == null)
            Canvas_Stop.gameObject.SetActive(false);

        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        if (Canvas_Option.gameObject.activeSelf == true)
        //        {
        //            Canvas_Option.gameObject.SetActive(false);
        //        }
        //        else if (Canvas_Option.gameObject.activeSelf == false)
        //        {
        //            Canvas_Option.gameObject.SetActive(true);
        //        }
        //    }
        //}
        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        if (Canvas_Option.gameObject.activeSelf == true)
        //        {
        //            StageSelect.Resume();
        //            //Canvas_Option.gameObject.SetActive(false);
        //        }
        //        else if (Canvas_Option.gameObject.activeSelf == false)
        //        {
        //            StageSelect.Stop();
        //            //Canvas_Option.gameObject.SetActive(true);
        //        }
        //        Debug.Log("캔버스 옵션은 : " + Canvas_Option.gameObject.activeSelf);
        //    }
        //}
    }
    // Update is called once per frame
    void MapSetting()
    {
        //GameObject Parent = null;
        int temp = 0;

        if (stage <= 50)
            temp = 11;
        else if (stage > 50 && stage <= 100)
            temp = 16;
        else if (stage > 100 && stage <= 150)
            temp = 21;

        List_Portal.Clear();

        if (textValue.Length > 0)
        {
            for (int x = 0; x < CountZ - 1; x++)
            {
                for (int y = 0; y < CountZ - 1; y++)
                {
                    m_vPos = new Vector3((float)x, 0f, (float)y);
                    Instantiate(Floor, m_vPos, m_vAngle);
                }
            }
            //Parent = null;
            Map = new string[CountZ - 1][];

            for (int j = 1; j < CountZ; j++)
            {
                Map[j - 1] = textValue[j].Split(' ');
            }
            for (int k = 0; k < CountZ - 1; k++)
            {
                for (int l = 0; l < Map[k].Length; l++)
                {
                    m_vPos = new Vector3((float)l, 0f, (float)temp - k);
                    m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                    // 고정벽
                    if (Map[k][l] == "1")
                    {
                        m_vPos = new Vector3((float)l, 1f, (float)temp - k);
                        if (stage == 1 && stage > PlayerPrefs.GetInt("nStage") && k == 2 && l == 6)
                            zoomObj[2] = Instantiate(Wall, m_vPos, m_vAngle);
                        else
                            Instantiate(Wall, m_vPos, m_vAngle);
                    }
                    // 입구
                    if (Map[k][l] == "E")
                    {
                        if (m_vPos.x >= temp)
                        {
                            //p_Pos = m_vPos + Vector3.left;
                            p_Angle = Quaternion.Euler(0f, -90f, 0f);
                            Player_Current = Instantiate(Player, new Vector3(l - 1, 1f, temp - k), p_Angle);
                            Controller.pState = 4;
                            m_vPos = new Vector3((float)l - 0.5f, 2.6f, (float)temp - k + 0.1f);
                            m_vAngle = Quaternion.Euler(0f, 90f, 0f);
                            Instantiate(Enter, m_vPos, m_vAngle);
                        }
                        if (m_vPos.x == 0)
                        {
                            //p_Pos = m_vPos + Vector3.right;
                            p_Angle = Quaternion.Euler(0f, 90f, 0f);
                            Player_Current = Instantiate(Player, new Vector3(l + 1, 1f, temp - k), p_Angle);
                            Controller.pState = 2;
                            m_vPos = new Vector3((float)l + 0.5f, 2.6f, (float)temp - k - 0.1f);
                            m_vAngle = Quaternion.Euler(0f, -90f, 0f);
                            Instantiate(Enter, m_vPos, m_vAngle);
                        }
                        if (m_vPos.z >= temp)
                        {
                            //p_Pos = m_vPos + Vector3.back;
                            p_Angle = Quaternion.Euler(0f, 180f, 0f);
                            Player_Current = Instantiate(Player, new Vector3(l, 1f, temp - k - 1), p_Angle);
                            Controller.pState = 3;
                            m_vPos = new Vector3((float)l - 0.1f, 2.6f, (float)temp - k - 0.5f);
                            m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                            Instantiate(Enter, m_vPos, m_vAngle);
                        }
                        if (m_vPos.z == 0)
                        {
                            //p_Pos = m_vPos + Vector3.forward;
                            p_Angle = Quaternion.Euler(0f, 0f, 0f);
                            Player_Current = Instantiate(Player, new Vector3(l, 1f, temp - k + 1), p_Angle);
                            Controller.pState = 1;
                            m_vPos = new Vector3((float)l + 0.1f, 2.6f, (float)temp - k + 0.5f);
                            m_vAngle = Quaternion.Euler(0f, 180f, 0f);
                            Instantiate(Enter, m_vPos, m_vAngle);
                        }
                        if (stage > PlayerPrefs.GetInt("nStage"))
                        {
                            if (stage == 1)
                            {
                                zoomObj[0] = Player_Current;
                                zoomObj[1] = zoomObj[0];
                                zoomObj[5] = Player_Current;
                            }
                            else if (stage == 16 || stage == 21 || stage == 26)
                            {
                                zoomObj[0] = Player_Current;
                                zoomObj[2] = Player_Current;
                            }
                            else if (stage == 6 || stage == 11 || stage == 31 || stage == 36)
                            {
                                zoomObj[0] = Player_Current;
                                zoomObj[3] = Player_Current;
                            }

                        }
                    }
                    // 출구
                    if (Map[k][l] == "X")
                    {
                        if (m_vPos.x >= temp)
                        {
                            m_vPos = new Vector3((float)l - 0.5f, 2.6f, (float)temp - k + 0.1f);
                            m_vAngle = Quaternion.Euler(0f, 90f, 0f);
                        }
                        if (m_vPos.x == 0)
                        {
                            m_vPos = new Vector3((float)l + 0.5f, 2.6f, (float)temp - k - 0.1f);
                            m_vAngle = Quaternion.Euler(0f, -90f, 0f);
                        }
                        if (m_vPos.z >= temp)
                        {
                            m_vPos = new Vector3((float)l - 0.1f, 2.6f, (float)temp - k - 0.5f);
                            m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                        }
                        if (m_vPos.z == 0)
                        {
                            m_vPos = new Vector3((float)l + 0.1f, 2.6f, (float)temp - k + 0.5f);
                            m_vAngle = Quaternion.Euler(0f, 180f, 0f);
                        }
                        if (stage == 1 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[4] = Instantiate(Exit, m_vPos, m_vAngle);
                        else
                            Instantiate(Exit, m_vPos, m_vAngle);
                    }
                    // 포탈형(단방향 입)
                    if (Map[k][l] == "I")
                    {
                        m_vPos = new Vector3((float)l, 1.1f, (float)temp - k);
                        if (stage == 31 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[1] = Instantiate(Portal_I, m_vPos, m_vAngle);
                        else
                            Instantiate(Portal_I, m_vPos, m_vAngle);
                    }
                    // 포탈형(단방향 출)
                    if (Map[k][l] == "O")
                    {
                        m_vPos = new Vector3((float)l, 1.1f, (float)temp - k);
                        if (stage == 31 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[2] = Instantiate(Portal_O, m_vPos, m_vAngle);
                        else
                            Instantiate(Portal_O, m_vPos, m_vAngle);
                        List_Portal.Add(new Portal("OUTPUT", m_vPos));
                    }
                    // 포탈형(양방향)
                    if (Map[k][l] == "T")
                    {
                        m_vPos = new Vector3((float)l, 1.1f, (float)temp - k);
                        if (stage == 36 && stage > PlayerPrefs.GetInt("nStage"))
                        {
                            if (zoomObj[1])
                                zoomObj[2] = Instantiate(Portal_T, m_vPos, m_vAngle);
                            else
                                zoomObj[1] = Instantiate(Portal_T, m_vPos, m_vAngle);
                        }
                        else
                            Instantiate(Portal_T, m_vPos, m_vAngle);
                        List_Portal.Add(new Portal("TELEPORT", m_vPos));
                    }
                    // 얼음구슬(좌우)
                    if (Map[k][l] == "C")
                    {
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        if (stage == 16 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[1] = Instantiate(IceBallRight, m_vPos, m_vAngle);
                        else
                        {
                            IceBall.iceTime = 1;
                            Instantiate(IceBallRight, m_vPos, m_vAngle);
                        }
                    }
                    // 얼음구슬(상하)
                    if (Map[k][l] == "F")
                    {
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        Instantiate(IceBallUp, m_vPos, m_vAngle);
                    }
                    // 열쇠
                    if (Map[k][l] == "K")
                    {
                        chch[0] = 1;
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        if (stage == 1 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[3] = Instantiate(Key, m_vPos, m_vAngle);
                        else
                            Instantiate(Key, m_vPos, m_vAngle);
                    }
                    // 별
                    if (Map[k][l] == "A")
                    {
                        chch[1] = 1;
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        Instantiate(Star, m_vPos, m_vAngle);
                    }
                    // 망치
                    if (Map[k][l] == "M")
                    {
                        chch[3] = 1;
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        if (stage == 11 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[1] = Instantiate(Mang, m_vPos, m_vAngle);
                        else
                            Instantiate(Mang, m_vPos, m_vAngle);
                    }
                    // 부시기
                    if (Map[k][l] == "B")
                    {
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        if (stage == 11 && stage > PlayerPrefs.GetInt("nStage"))
                        {
                            zoomObj[2] = Instantiate(Crush, m_vPos, m_vAngle);
                            List_Crush.Add(zoomObj[2]);
                        }
                        else
                            List_Crush.Add(Instantiate(Crush, m_vPos, m_vAngle));
                    }
                    // 장갑
                    if (Map[k][l] == "G")
                    {
                        chch[2] = 1;
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        if (stage == 6 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[1] = Instantiate(Gloves, m_vPos, m_vAngle);
                        else
                            Instantiate(Gloves, m_vPos, m_vAngle);
                    }
                    // 신발
                    if (Map[k][l] == "N")
                    {
                        chch[4] = 1;
                        m_vPos = new Vector3((float)l, 1.3f, (float)temp - k);
                        Instantiate(Shoes, m_vPos, m_vAngle);
                    }
                    // 밀기
                    if (Map[k][l] == "P")
                    {
                        m_vPos = new Vector3((float)l, 1.5f, (float)temp - k);
                        if (stage == 6 && stage > PlayerPrefs.GetInt("nStage"))
                        {
                            zoomObj[2] = Instantiate(Push, m_vPos, m_vAngle);
                            List_Push.Add(zoomObj[2]);
                        }
                        else
                            List_Push.Add(Instantiate(Push, m_vPos, m_vAngle));
                    }
                    // 이동형(상)
                    if (Map[k][l] == "U")
                    {
                        m_vAngle = Quaternion.Euler(0f, 90f, 0f);
                        m_vPos = new Vector3((float)l, 1.01f, (float)temp - k);
                        if (stage == 26 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[1] = Instantiate(Trap_D, m_vPos, m_vAngle);
                        else
                            Instantiate(Trap_D, m_vPos, m_vAngle);
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                    }
                    // 이동형(하)
                    if (Map[k][l] == "D")
                    {
                        m_vAngle = Quaternion.Euler(0f, 270f, 0f);
                        m_vPos = new Vector3((float)l, 1.01f, (float)temp - k);
                        Instantiate(Trap_D, m_vPos, m_vAngle);
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                    }
                    // 이동형(좌)
                    if (Map[k][l] == "L")
                    {
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                        m_vPos = new Vector3((float)l, 1.01f, (float)temp - k);
                        Instantiate(Trap_D, m_vPos, m_vAngle);
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                    }
                    // 이동형(우)
                    if (Map[k][l] == "R")
                    {
                        m_vAngle = Quaternion.Euler(0f, 180f, 0f);
                        m_vPos = new Vector3((float)l, 1.01f, (float)temp - k);
                        Instantiate(Trap_D, m_vPos, m_vAngle);
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                    }
                    // 이동형(정지)
                    if (Map[k][l] == "S")
                    {
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                        m_vPos = new Vector3((float)l, 1.01f, (float)temp - k);
                        Instantiate(Trap_S, m_vPos, m_vAngle);
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                    }
                    // 균열
                    if (Map[k][l] == "H")
                    {
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                        m_vPos = new Vector3((float)l, 1.01f, (float)temp - k);
                        if (stage == 21 && stage > PlayerPrefs.GetInt("nStage"))
                            zoomObj[1] = Instantiate(Trap_H, m_vPos, m_vAngle);
                        else
                            Instantiate(Trap_H, m_vPos, m_vAngle);
                        m_vAngle = Quaternion.Euler(0f, 0f, 0f);
                    }

                }
            }
        }
    }
    void visibleGameoverCanvas()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("GameOverScenePortrait");
        Time.timeScale = 1;
    }
    void TextLoad(string fileName)
    {
        int count = 0;
        TextAsset _txtFile = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        TextReader reader = new StringReader(_txtFile.text);
        while (reader.ReadLine() != null)
        {
            count++;
        }

        reader = new StringReader(_txtFile.text);

        textValue = new string[count];
        CountZ = count;
        for (int j = 0; j < count; j++)
        { textValue[j] = reader.ReadLine(); }

        reader.Close();
    }
}