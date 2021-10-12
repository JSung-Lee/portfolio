using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public AudioClip TriggerSound;
    public AudioClip SwipeSound;
    public static Transform tr;
    public static bool MoveOn = false;
    public static Vector3 playerPosition;
    public static Vector3 lastPlayerPosition;
    private Vector3 screenPos;
    private float[] playerRotationY;

    public static int pState = 0; // up = 1, right = 2, down = 3, left = 4, 
    public static bool inTrigger = false;
    public float moveSpeed = 10.0f;
    public float turnSpeed = 540.0f;
    public static float step;
    public static int layerMask;
    public static int PlayerCheck;
    public static int CrushCheck;
    public static int GlovesCheck;
    public static int ShoesCheck;
    public static bool KeyOn;
    public static bool StageOn;
    public static bool PushOn;
    private RaycastHit hit;

    public static Vector3 startPos, endPos;
    public static float minSwipeDistance;
    public static float startTime, endTime, maxTime;
    public static GameObject smokeFX;
    public static GameObject ElplodeFX;
    public static GameObject PortalI_FX;
    public static GameObject PortalO_FX;
    public static GameObject PortalT_FX;
    public static GameObject Loot_FX;
    private GameObject currentFX;

    public static Animator anim;
    // Use this for initialization
    public static GameObject hammer;
    public static GameObject Shield;
    public static bool isCrush;
    public static bool isGet;
    public static bool isPush;
    public static bool isDie;
    public static int nRevival;
    void Awake()
    {
        isGet = false;
        isCrush = false;
        isPush = false;
        isDie = false;
        nRevival = 0;
        anim = gameObject.GetComponent<Animator>();

        playerRotationY = new float[2];

        // 레이어마스크에 출구 레이어 추가해주면 가끔 클리어 안되는 문제 해결됨.(추가함)
        layerMask = (-1) - ((1 << LayerMask.NameToLayer("Portal_O")) | (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Crush")) 
            | (1 << LayerMask.NameToLayer("Shoes")) | (1 << LayerMask.NameToLayer("Gloves")) 
            | (1 << LayerMask.NameToLayer("Star")) | (1 << LayerMask.NameToLayer("Key")) 
            | (1 << LayerMask.NameToLayer("Player_P"))
            | (1 << LayerMask.NameToLayer("Dir")) | (1 << LayerMask.NameToLayer("Stop")));

        smokeFX = Resources.Load<GameObject>("Prefab/FX/SmokeExplosionWhite");
        ElplodeFX = Resources.Load<GameObject>("Prefab/FX/ConfettiBlast");

        PortalI_FX = Resources.Load<GameObject>("Prefab/FX/SwordSpinRed");
        PortalO_FX = Resources.Load<GameObject>("Prefab/FX/SwordSpinGreen");
        PortalT_FX = Resources.Load<GameObject>("Prefab/FX/SwordSpinPurple");
        Loot_FX = Resources.Load<GameObject>("Prefab/FX/StarBurst");

        tr = GetComponent<Transform>();
        hammer = gameObject.transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/Dummy Prop Right/Hammer").gameObject;
        hammer.SetActive(false);
        Shield = gameObject.transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigLArm1/RigLArm2/RigLArmPalm/Dummy Prop Left/Shield").gameObject;
        Shield.SetActive(false);

        PlayerCheck = 0;
        GlovesCheck = 0;
        ShoesCheck = 0;
        KeyOn = false;
        StageOn = false;
        PushOn = false;
        MoveOn = false;
        minSwipeDistance = 200.0f;
        maxTime = 1.0f;

        lastPlayerPosition = tr.position;
    }

    // Update is called once per frame
    void Update()
    {
        step = moveSpeed * Time.deltaTime*Time.timeScale;

        if (!MoveOn)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //Touch
                if (Input.touchCount > 0)
                {
                    UnityEngine.Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        startPos = touch.position;
                        startTime = Time.time;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        endPos = touch.position;
                        endTime = Time.time;

                        playerRotationY[0] = StageManager.Player_Current.transform.eulerAngles.y;
                        if (StageManager.List_Crush.Count > 0)
                        {
                            for (int i = 0; i < StageManager.List_Crush.Count; i++) {
                                if(StageManager.List_Crush[i] != null)
                                    StageManager.List_Crush[i].SendMessage("swipe", playerRotationY[0]);
                            }
                        }

                        swipe();

                        playerRotationY[1] = StageManager.Player_Current.transform.eulerAngles.y;
                        if (StageManager.List_Push.Count > 0)
                        {
                            for (int i = 0; i < StageManager.List_Push.Count; i++)
                            {
                                if (StageManager.List_Push[i] != null)
                                    StageManager.List_Push[i].SendMessage("swipe", playerRotationY);
                            }
                        }
                    }
                }
            }
            else
            {
                //Click
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = Input.mousePosition;
                    startTime = Time.time;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    endPos = Input.mousePosition;
                    endTime = Time.time;

                    playerRotationY[0] = StageManager.Player_Current.transform.eulerAngles.y;
                    if (StageManager.List_Crush.Count > 0)
                    {
                        for (int i = 0; i < StageManager.List_Crush.Count; i++) {
                            if (StageManager.List_Crush[i] != null)
                                StageManager.List_Crush[i].SendMessage("swipe", playerRotationY[0]);
                        }
                    }

                    swipe();

                    playerRotationY[1] = StageManager.Player_Current.transform.eulerAngles.y;
                    if (StageManager.List_Push.Count > 0)
                    {
                        for (int i = 0; i < StageManager.List_Push.Count; i++)
                        {
                            if (StageManager.List_Push[i] != null)
                                StageManager.List_Push[i].SendMessage("swipe", playerRotationY);
                        }
                    }
                }
            }
            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    pState = 1;
            //    MoveOn = true;
            //    tr.rotation = Quaternion.Euler(0, 0, 0);
            //}
            //else if (Input.GetKeyDown(KeyCode.RightArrow))
            //{
            //    pState = 2;
            //    MoveOn = true;
            //    tr.rotation = Quaternion.Euler(0, 90, 0);
            //}
            //else if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    pState = 3;
            //    MoveOn = true;
            //    tr.rotation = Quaternion.Euler(0, 180, 0);
            //}
            //else if (Input.GetKeyDown(KeyCode.LeftArrow))
            //{
            //    pState = 4;
            //    MoveOn = true;
            //    tr.rotation = Quaternion.Euler(0, -90, 0);
            //}
        }
        // 움직이는 중일때. 이후 pState에 따라 이동 방향 결정
        else if (MoveOn)
        {

            //레이캐스트를 쏘아서 직진방향으로 0.6만큼의 거리에 어떤 오브젝트가 있는지 검사
            if (Physics.Raycast(tr.position, tr.forward, out hit, 0.6f, layerMask))
            {

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Push"))
                {
                    if (PlayerCheck == 0)
                    { PlayerCheck = 1; }
                    MoveOn = false;
                    //움직임을 0으로 만듦
                    tr.Translate(Vector3.forward * 0);
                    playerPosition.Set(Mathf.Round(tr.position.x), Mathf.Round(tr.position.y), Mathf.Round(tr.position.z));
                    tr.position = playerPosition;
                    lastPlayerPosition = playerPosition;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("IceBall"))
                {
                    MoveOn = false;
                    if (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Die"))
                    {
                        anim.Play("Die");
                        anim.SetBool("Move", false);
                    }
                    Time.timeScale = 0;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    MoveOn = false;
                    //움직임을 0으로 만듦
                    tr.Translate(Vector3.forward * 0);
                    playerPosition.Set(Mathf.Round(tr.position.x), Mathf.Round(tr.position.y), Mathf.Round(tr.position.z));
                    tr.position = playerPosition;
                    lastPlayerPosition = playerPosition;
                    currentFX = Instantiate(smokeFX, tr.position, tr.rotation);
                    Destroy(currentFX, 0.5f);
                }
                else
                {
                    MoveOn = false;
                    //움직임을 0으로 만듦
                    tr.Translate(Vector3.forward * 0);
                    playerPosition.Set(Mathf.Round(tr.position.x), Mathf.Round(tr.position.y), Mathf.Round(tr.position.z));
                    tr.position = playerPosition;
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Crush"))
                        lastPlayerPosition = playerPosition;
                }
                if (!isCrush && !anim.GetCurrentAnimatorStateInfo(0).IsName("Push"))
                {
                    anim.Play("Damage");
                    anim.SetBool("Move", false);
                }

                AudioSource.PlayClipAtPoint(TriggerSound, transform.position);
            }
            //레이캐스트를 쏘았을때 아무것도 없으면
            else
            {
                //1이면 z값 증가 (앞으로 이동)
                if (pState == 1)
                { tr.position += (Vector3.forward * step); }
                //2이면 x값 증가 (오른쪽으로 이동)
                else if (pState == 2)
                { tr.position += (Vector3.right * step); }
                //3이면 z값 감소 (뒤로 이동)
                else if (pState == 3)
                { tr.position += (Vector3.back * step); }
                //4이면 x값 감소 (왼쪽으로 이동)
                else if (pState == 4)
                { tr.position += (Vector3.left * step); }
            }
            //MoveOn이 True일때
        }
    }


    //스와이프시 동작
    void swipe()
    {
        float swipeDistance = (endPos - startPos).magnitude;
        float swipeTime = endTime - startTime;
        screenPos = startPos;
        screenPos.y = 3f;
        if (isGet || isDie || isCrush || isPush || !Camera.main.GetComponent<FollowCam>().enabled || this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return;
        if (swipeTime < maxTime && swipeDistance >= minSwipeDistance)
        {
            Vector2 distance = endPos - startPos;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                if (distance.x > 0)
                {
                    AudioSource.PlayClipAtPoint(SwipeSound, transform.position);

                    anim.Play("Move");
                    anim.SetBool("Move", true);
                    pState = 2;
                    MoveOn = true;
                    tr.rotation = Quaternion.Euler(0, 90, 0);
                }
                if (distance.x < 0)
                {
                    AudioSource.PlayClipAtPoint(SwipeSound, transform.position);
                    anim.Play("Move");
                    anim.SetBool("Move", true);
                    pState = 4;
                    MoveOn = true;
                    tr.rotation = Quaternion.Euler(0, -90, 0);
                }
            }
            else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
            {
                if (distance.y > 0)
                {
                    AudioSource.PlayClipAtPoint(SwipeSound, transform.position);
                    anim.Play("Move");
                    anim.SetBool("Move", true);
                    pState = 1;
                    MoveOn = true;
                    tr.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (distance.y < 0)
                {
                    AudioSource.PlayClipAtPoint(SwipeSound, transform.position);
                    anim.Play("Move");
                    anim.SetBool("Move", true);
                    pState = 3;
                    MoveOn = true;
                    tr.rotation = Quaternion.Euler(0, 180, 0);
                }

            }

        }
    }

}

