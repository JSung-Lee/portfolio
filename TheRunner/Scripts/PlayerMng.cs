using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMng : MonoBehaviour {

    //private float timeScale;            //게임속도
    private float vecticalMoveSpeed;    //캐릭터가 상하로 움직이는 속도
    public float HorizontalMoveSpeed;  //캐릭터가 좌우로 움직이는 속도
    //private float rotSpeed;             //화면이 회전하는 속도
    //private float rotStartTime;         //화면이 회전하기 시작하는 시간
    //private float rotEndAngle;          //화면의 회전이 끝나는 각도
    private Vector3 vecticalMoveVec;    //캐릭터가 상하로 움직이는 방향 벡터(normalize됨)
    //private float horizontalMoveVec;  //캐릭터가 좌우로 움직이는 방향 벡터(normalize됨)
    private Vector3 moveVec;
    //private Vector3 rotVec;             //화면이 회전하는 방향 벡터(normalize됨)
    public Transform cam;              //캐릭터 하위에 고정된 카메라
    public Transform skin;             //실제 캐릭터(혹은 스킨)
    public Rigidbody skinRigid;             //실제 캐릭터(혹은 스킨)
    public SkinManager skinManager;
    public UserManager userManager;    //스코어 관리
    public Text score;                 //스코어
    public PointManager points;
    public RectTransform pauseButton;
    public RectTransform pauseButton_play;
    public RectTransform pauseButton_pause;
    public Text pauseTimer;
    private float scoreRatio;
    private float inertia;
    private bool isPausedGame;
    private float timer;
    // Use this for initialization
    void OnEnable() {
        transform.position = new Vector3(0, 1, 0);
        vecticalMoveSpeed = 20.0f;//20.0f;       //캐릭터가 상하로 움직이는 속도 지정(최초값)
        HorizontalMoveSpeed = 40.0f;//40.0f;  //캐릭터가 좌우로 움직이는 속도는 상하와 동일
        //rotSpeed = 9.0f;     //화면이 회전하는 속도(9도 씩 10초동안 총 90도)
        //timeScale = 0.0f;    //게임 속도
        //rotStartTime = 5.0f;
        //rotEndAngle = 90.0f;
        scoreRatio = 1.0f;
        //horizontalMoveVec = 1.0f;  //캐릭터의 좌우 방향 벡터
        //rotVec = new Vector3(0.0f, 0.0f, -1.0f);            //화면의 회전 방향 벡터
        moveVec = new Vector3(1.0f, 0.0f, 1.0f);
        Input.multiTouchEnabled = false;
        inertia = 0.75f;
        UserManager.timeScale = 0.0f;
        isPausedGame = false;
        timer = 0;
        pauseTimer.gameObject.SetActive(false);
    }

    private void OnApplicationPause(bool pause)
    {
        //게임오버시 이벤트
        if (skinManager.isGameOver())
            return;
        Time.timeScale = 0;
        Camera.main.GetComponent<AudioSource>().Pause();
        pauseButton.position = pauseButton_pause.position;
        pauseButton.localScale = pauseButton_pause.localScale;
    }

    // Update is called once per frame
    void Update() {
        //게임오버시 이벤트
        if (skinManager.isGameOver())
        {
            skinRigid.velocity = Vector3.zero;
            return;
        }

        if(isPausedGame)
        {
            if (timer >= 3.0f)
            {
                isPausedGame = false;
                pauseTimer.gameObject.SetActive(false);
                timer = 0;
                Time.timeScale = 1;
                Camera.main.GetComponent<AudioSource>().Play();
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                if(((int)(4.0f - timer)) != 0 && ((int)(4.0f - timer)) != 4)
                    pauseTimer.text = ((int)(4.0f - timer)).ToString();
                Color color = pauseTimer.color;
                color.a = (3.0f - timer) - ((int)(3.0f - timer));
            }
            return;
        }

        //터치시 이벤트
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (rectContains(pauseButton, Input.mousePosition))
                {
                    if (Time.timeScale >= 1)
                    {
                        Time.timeScale = 0;
                        pauseButton.position = pauseButton_pause.position;
                        pauseButton.localScale = pauseButton_pause.localScale;
                        Camera.main.GetComponent<AudioSource>().Pause();
                    }
                    else
                    {
                        pauseButton.position = pauseButton_play.position;
                        pauseButton.localScale = pauseButton_play.localScale;
                        isPausedGame = true;
                        pauseTimer.gameObject.SetActive(true);
                        pauseTimer.text = "3";
                    }
                }
                if (Time.timeScale >= 1) 
                {
                    HorizontalMoveSpeed *= -1.0f;
                    skinRigid.velocity = new Vector3(skinRigid.velocity.x * inertia, 0, 0);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (rectContains(pauseButton, Input.GetTouch(0).position))
                    {
                        if (Time.timeScale >= 1)
                        {
                            Time.timeScale = 0;
                            pauseButton.position = pauseButton_pause.position;
                            pauseButton.localScale = pauseButton_pause.localScale;
                            Camera.main.GetComponent<AudioSource>().Pause();
                        }
                        else
                        {
                            pauseButton.position = pauseButton_play.position;
                            pauseButton.localScale = pauseButton_play.localScale;
                            isPausedGame = true;
                            pauseTimer.gameObject.SetActive(true);
                            pauseTimer.text = "3";
                        }
                    }
                    if (Time.timeScale >= 1)
                    {
                        HorizontalMoveSpeed *= -1.0f;
                        skinRigid.velocity = new Vector3(skinRigid.velocity.x * inertia, 0, 0);
                    }
                }
            }
        }

        if (Time.timeScale <= 0)
            return;

        //매 프레임마다 이벤트
        if (UserManager.timeScale < 1.0f)   //게임속도가 1보다 적으면 속도가 초당 0.2씩 상승
            UserManager.timeScale += (Time.deltaTime * 0.25f);
        else                    //1보다 크면 속도가 초당 0.02씩 상승
            UserManager.timeScale += (Time.deltaTime * 0.045f);
        if (UserManager.timeScale > 5.0f)   //최대 게임속도 지정
            UserManager.timeScale = 5.0f;

        transform.position += new Vector3(0.0f, 0.0f, moveVec.z) * Time.deltaTime * (UserManager.timeScale / 2.0f) * vecticalMoveSpeed;    //상하 움직임
        skinRigid.AddForce(new Vector3(moveVec.x, 0.0f, 0.0f) * UserManager.timeScale * 5.0f * HorizontalMoveSpeed);
        if (Mathf.Abs(skinRigid.velocity.x) > moveVec.z * UserManager.timeScale * 0.1f * Mathf.Abs(HorizontalMoveSpeed))
            if(skinRigid.velocity.x * HorizontalMoveSpeed > 0)
                skinRigid.velocity = new Vector3(moveVec.z * UserManager.timeScale * 0.1f * HorizontalMoveSpeed, 0.0f, 0.0f);
        //skin.transform.positskinRigid.velocityion += new Vector3(moveVec.x, 0.0f, 0.0f) * Time.deltaTime * (UserManager.timeScale / 2.0f) * HorizontalMoveSpeed;  //좌우 움직임

        //점수 처리
        userManager.plusScore(Time.deltaTime * UserManager.timeScale * 0.2f * scoreRatio);
        userManager.plusDistance(Time.deltaTime * UserManager.timeScale * 0.2f * scoreRatio);
        int num = (int)userManager.getScore() * 10;
        score.text = (num / 10).ToString();
    }

    bool rectContains(RectTransform rect, Vector3 pos)
    {
        //Debug.Log(rect.position.ToString());
        if (Mathf.Abs(rect.position.x - pos.x) < rect.rect.width)
            if (Mathf.Abs(rect.position.y - pos.y) < rect.rect.height)
                return true;
        return false;
    }

}
