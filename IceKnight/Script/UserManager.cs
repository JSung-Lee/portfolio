using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static int nStage;  //최대 스테이지
    public static int nStar;   //각 스테이지의 클리어한 별 갯수 
    public static int nClock;  //상점에서 구매한 시계 레벨
    public static int nRevival; //상점에서 구매한 부활 아이템 레벨
    public static int nVision;  //상점에서 구매한 시야 아이템 레벨
    public static float _time; //각 스테이지의 클리어 타임
    public static UserManager current;
    public static int lastCharacter;
    public static string[] characters = { "Player", "Princess", "King", "Witch", "Angel", "Archer", "Barbarian", "Cyclops", "Devil", "Halloween", "Indian", "Medusa", "Santa", "Phantom", "FireMan" };

    public static int nHeart; //캔유필마핱빛
    
    //종료할때 시간을 저장
    public static int nBeforeMin;
    public static int nBeforeSec;
    //켰을때 시간을 저장
    public static int nAfterMin;
    public static int nAfterSec;

    private int Sec, Min, Hour;
    public static int CheckTime, CheckHeart;


    
    // Use this for initialization
    void Awake()
    {
        Screen.SetResolution(1080, 1920, false);
        if (current != null && current != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            current = this;
        }
    }

    void Start()
    {
        /////////////////////////////폰 끄고 있는 동안 흐른 시간 계산 코드///////////////////////////// 

        //켰을때 시간 저장
        nAfterMin = System.DateTime.Now.Minute;
        nAfterSec = System.DateTime.Now.Second;

        PlayerPrefs.SetInt("nAfterMin", nAfterMin);
        PlayerPrefs.SetInt("nAfterSec", nAfterSec);

        PlayerPrefs.SetInt("isWatched", 0);
        //Debug.Log("게임 시작 분 : " + PlayerPrefs.GetInt("nAfterMin"));
        //Debug.Log("게임 시작 초 : " + PlayerPrefs.GetInt("nAfterSec"));

        //흐른 분 계산
        if (PlayerPrefs.GetInt("nAfterMin") < PlayerPrefs.GetInt("nBeforeMin"))
        {
            Min = 60 - PlayerPrefs.GetInt("nBeforeMin") + nAfterMin;
        }
        else
        {
            Min = nAfterMin - PlayerPrefs.GetInt("nBeforeMin");
        }


        //흐른 초 계산
        if (nAfterSec < PlayerPrefs.GetInt("nBeforeSec"))
        {
            Sec = 60 - PlayerPrefs.GetInt("nBeforeSec") + nAfterSec;
            Min -= 1;
        }
        else
        {
            Sec = nAfterSec - PlayerPrefs.GetInt("nBeforeSec");
        }


        //Debug.Log("계산된 분 : " + Min);
        //Debug.Log("계산된 초 : " + Sec);
        //Debug.Log("총 몇초? " + (Min * 60 + Sec));

        //추가될 개수가 5개 이상이면.
        if ((Min * 60) + Sec > 1500)
        {
            CheckHeart = 5;
            //Debug.Log("하트 꽉 참");
        }
        else
        {
            //분 값을 초 단위로 바꿔서  초와 더해 계산 후 몫 만큼 하트에 추가. 5분단위 (300초)
            CheckHeart += (Min * 60 + Sec + PlayerPrefs.GetInt("CheckTime")) / 300;
            //Debug.Log("추가 될 하트 : " + CheckHeart);

            //몫 값은 nHeart에 넣고 남은 값은 게임 플레이할때 계산하는 값으로 넘김
            CheckTime += (Min * 60 + Sec) % 300;

            //PlayerPrefs.SetInt("CheckTime",CheckTime);
            //Debug.Log("이제부터 추가 될 시간 : " + CheckTime);
        }
        //Debug.Log("지금 하트 갯수 " + PlayerPrefs.GetInt("nHeart"));

        if (PlayerPrefs.GetInt("nHeart") + CheckHeart < 5)
        {
            PlayerPrefs.SetInt("nHeart", PlayerPrefs.GetInt("nHeart") + CheckHeart);
        }
        else
        {
            PlayerPrefs.SetInt("nHeart", 5);
        }
        


        //Debug.Log("getData 첫 합쳐진 하트 " + PlayerPrefs.GetInt("nHeart"));


        getData();
    }

    public void saveData()
    {
        // 최초로 클리어한 스테이지라면 총 별 개수에 클리어 별 개수 추가
        if (PlayerPrefs.GetInt("nStage") < (StageManager.stage - 1))
            PlayerPrefs.SetInt("nStarTotal", PlayerPrefs.GetInt("nStarTotal") + nStar);

        PlayerPrefs.SetInt("nStage", nStage);

        PlayerPrefs.SetInt("nClock", nClock);
        PlayerPrefs.SetInt("nRevival", nRevival);
        PlayerPrefs.SetInt("nVision", nVision);

        PlayerPrefs.SetInt("lastCharacter", lastCharacter);
        //Exit에서 끝날때 Stage 값을 미리 증가시키기 때문에 저장할때 스테이지 값을 1 낮춤.

        //해당 스테이지의 이전기록보다 많은 별을 먹었으면 데이터 갱신
        if (PlayerPrefs.HasKey("nStar"+(StageManager.stage-1)) && PlayerPrefs.GetInt("nStar" + (StageManager.stage - 1)) < nStar)
        {
            PlayerPrefs.SetInt("nStarTotal", PlayerPrefs.GetInt("nStarTotal") + (nStar - PlayerPrefs.GetInt("nStar" + (StageManager.stage - 1))));
            PlayerPrefs.SetInt("nStar" + (StageManager.stage - 1).ToString(), nStar);
        }
        if(!PlayerPrefs.HasKey("nStar" + (StageManager.stage - 1)))
            PlayerPrefs.SetInt("nStar" + (StageManager.stage - 1).ToString(), nStar);
        //해당 스테이지의 이전보다 빨리 클리어했거나 첫 플레이라면(기록이 0초) 데이터 갱신
        if (PlayerPrefs.GetFloat("_time" + (StageManager.stage - 1)) > _time || PlayerPrefs.GetFloat("_time" + (StageManager.stage - 1)) == 0)
        {
            PlayerPrefs.SetFloat("_time" + (StageManager.stage - 1).ToString(), _time);
        }



        if (nHeart > 5)
        {
            PlayerPrefs.SetInt("nHeart", 5);
        }
        else
        {
            PlayerPrefs.SetInt("nHeart", nHeart);
        }

    }

    public void getData()
    {
        //PlayerPrefs.DeleteAll();
        //if (!PlayerPrefs.HasKey("nStage"))
        //{
        //    Debug.Log("UserManager 데이터 삭제완료");
        //}

        //만약 nStage라는 저장 키 값이 있으면
        if (PlayerPrefs.HasKey("nStage"))
        {
            // nStage, nClock, nStar, _time을 호출.
            // nStar, _time은 스테이지 별로 있기 때문에 반복문을 통하여 nStar1, nStar2 방식으로 호출
            PlayerPrefs.GetInt("nStage");

            PlayerPrefs.GetInt("nClock");
            PlayerPrefs.GetInt("nRevival");
            PlayerPrefs.GetInt("nVision");
            PlayerPrefs.GetInt("lastCharacter");

            for (int i = 1; i <= PlayerPrefs.GetInt("nStage"); i++)
            {
                PlayerPrefs.GetInt("nStar" + i);
                PlayerPrefs.GetFloat("_time" + i);
            }


            PlayerPrefs.GetInt("nHeart");
        }
        //만약 첫 실행하여 nStage라는 HasKey가 없는 상태로 GetData를 호출하면
        else
        {
            //nStage라는 키에 0을 넣어 플레이 가능한 스테이지는 1이며
            //1라운드의 별, 시간 값에 0을 넣어줌
            //1 6 11 16 21 26 31 36
            PlayerPrefs.SetInt("nStage", 0);
            PlayerPrefs.SetInt("nClock", 0); //(0-3)
            PlayerPrefs.SetInt("nVision", 0); //(0-3)
            PlayerPrefs.SetInt("nRevival", 0); //(0-3)
            PlayerPrefs.SetInt("nCheckClock", 0);
            PlayerPrefs.SetInt("nCheckVision", 0);
            PlayerPrefs.SetInt("nCheckRevival", 0);
            PlayerPrefs.SetInt("lastCharacter", 0);
            for (int i = 1; i <= 120; i++)
            {
                PlayerPrefs.SetInt("nStar" + i, 0);
                PlayerPrefs.SetFloat("_time" + i, 0.0f);
            }

            //일단 임시로 1개만 넣어둠
            PlayerPrefs.SetInt("nHeart", 5);
        }
        //마지막으로 최종 저장된 스테이지 값을 nStage, nClock라는 변수에 넣음
        nStage = PlayerPrefs.GetInt("nStage");
        nClock = PlayerPrefs.GetInt("nClock");
        nRevival = PlayerPrefs.GetInt("nRevival");
        nVision = PlayerPrefs.GetInt("nVision");
        lastCharacter = PlayerPrefs.GetInt("lastCharacter");


        if (PlayerPrefs.GetInt("nHeart") > 5)
        {
            PlayerPrefs.SetInt("nHeart", 5);
        }

        nHeart = PlayerPrefs.GetInt("nHeart");



        //Debug.Log(nStage);
        //Debug.Log("하트비트 : " + nHeart);


        //Debug.Log(nStage);
    }

    private void OnDestroy()
    {
        nBeforeMin = System.DateTime.Now.Minute;
        nBeforeSec = System.DateTime.Now.Second;

        PlayerPrefs.SetInt("nBeforeMin", nBeforeMin);
        PlayerPrefs.SetInt("nBeforeSec", nBeforeSec);

        //Debug.Log("SaveData 분 : " + nBeforeMin);
        //Debug.Log("SaveData 초 : " + nBeforeSec);


        PlayerPrefs.SetInt("CheckTime", CheckTime);
        //Debug.Log("짜투리 몇초냐 " + PlayerPrefs.GetInt("CheckTime"));


        saveData();
    }
}