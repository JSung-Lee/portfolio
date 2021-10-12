using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    private Canvas Clear;
    //private GameObject Function;
    private Hashtable HT;
    private Hashtable HT2;
    private int Mapsize;
    private GameObject currentFX;
    private Vector3 PortalPos;
    private Quaternion PortalRot;
    Ray CameraRay;
    RaycastHit rayHit;
    void Start()
    {
        //currentFX = new GameObject();
        PortalRot = new Quaternion();
        PortalPos = new Vector3();
        //Function = Resources.Load<GameObject>("Prefab/IngameUIFunction");
        HT = new Hashtable();
        HT.Add("position", new Vector3(0, 0, 0));
        HT.Add("time", 0.8f);
        HT.Add("oncomplete", "iTweenEnd");
        HT.Add("oncompletetarget", this.gameObject);
        HT.Add("islocal", true);
        HT.Add("movetopath", false);
        HT.Add("ignoretimescale", true);
        HT.Add("easetype", iTween.EaseType.easeInOutSine);

        HT2 = new Hashtable();
        HT2.Add("x", 0f);
        HT2.Add("y", 0f);
        HT2.Add("z", 0f);
        HT2.Add("time", 0.8f);
        HT2.Add("islocal", true);
        HT2.Add("movetopath", false);
        HT2.Add("ignoretimescale", true);
        HT2.Add("easetype", iTween.EaseType.easeInOutSine);

        if (StageManager.stage > 0 && StageManager.stage <= 50)
            Mapsize = 6;
        if (StageManager.stage > 50 && StageManager.stage <= 100)
            Mapsize = 8;
        if (StageManager.stage > 100 && StageManager.stage <= 120)
            Mapsize = 10;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Controller.MoveOn = false;

            //움직임을 0으로 만듦
            col.transform.Translate(Vector3.forward * 0);
            Controller.playerPosition.Set(Mathf.Round(col.transform.position.x), Mathf.Round(col.transform.position.y), Mathf.Round(col.transform.position.z));
            col.transform.position = Controller.playerPosition;
            if (Controller.KeyOn)
            {
                IceBall.iceTime = 0;
                // 출구 연출
                StageManager.Canvas_Stop.gameObject.SetActive(false);
                Camera.main.GetComponent<FollowCam>().enabled = false;
                //Time.timeScale = 0;

                if (col.transform.position.x >= Mapsize) // 오른쪽
                {
                    if (col.transform.position.z >= Mapsize) // 위
                    {
                        if (Controller.pState == 1) // 0
                        {
                            HT["position"] = new Vector3(col.transform.position.x - 3.5f, 1.95f, col.transform.position.z - 1f);
                            HT2["x"] = 0f;
                            HT2["y"] = 66f;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, 0f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.forward / 2;
                        }
                        if (Controller.pState == 2) // 90
                        {
                            HT["position"] = new Vector3(col.transform.position.x - 1f, 1.95f, col.transform.position.z - 4f);
                            HT2["x"] = 0f;
                            HT2["y"] = 18f;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, 90f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.right / 3;
                        }
                        //-2.5, 1.95, -0.5
                        //0 70 0
                    }
                    else // 오른쪽 아래
                    {
                        if (Controller.pState == 2) // 90
                        {
                            HT["position"] = new Vector3(col.transform.position.x - 1.2f, 1.95f, col.transform.position.z + 4f);
                            HT2["x"] = 0f;
                            HT2["y"] = 160f;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, 90f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.right / 3;
                        }
                        if (Controller.pState == 3) // 180
                        {
                            HT["position"] = new Vector3(col.transform.position.x - 3.3f, 1.95f, col.transform.position.z + 0.9f);
                            HT2["x"] = 0f;
                            HT2["y"] = 113;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, 180f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.back / 2;
                        }
                        //+2.5 1.95 - 0.5
                        //0 - 70 0
                    }
                }
                else // 왼쪽
                {
                    if (col.transform.position.z >= Mapsize) // 왼쪽 위
                    {
                        if (Controller.pState == 1) // 0
                        {
                            HT["position"] = new Vector3(col.transform.position.x + 3.5f, 1.95f, col.transform.position.z - 1f);
                            HT2["x"] = 0f;
                            HT2["y"] = -66f;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, 0f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.forward / 2;
                        }
                        if (Controller.pState == 4) // -90
                        {
                            HT["position"] = new Vector3(col.transform.position.x + 1f, 1.95f, col.transform.position.z - 3.5f);
                            HT2["x"] = 0f;
                            HT2["y"] = -23f;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, -90f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.left / 3;
                        }

                        //-2.5 1.95 + 0.5
                        //0 110 0
                    }
                    else // 왼쪽 아래
                    {
                        if (Controller.pState == 3) // 180
                        {
                            HT["position"] = new Vector3(col.transform.position.x + 3.3f, 1.95f, col.transform.position.z + 0.9f);
                            HT2["x"] = 0f;
                            HT2["y"] = -113;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, 180f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.back / 2;
                        }
                        if (Controller.pState == 4) // -90
                        {
                            HT["position"] = new Vector3(col.transform.position.x + 1f, 1.95f, col.transform.position.z + 3.5f);
                            HT2["x"] = 0f;
                            HT2["y"] = -157f;
                            HT2["z"] = 0f;
                            iTween.MoveTo(Camera.main.gameObject, HT);
                            iTween.RotateTo(Camera.main.gameObject, HT2);
                            PortalRot = Quaternion.Euler(0f, -90f, 0f);
                            PortalPos = col.transform.position + Vector3.up * 0.5f + Vector3.left / 3;
                        }
                        //+2.5 1.95 + 0.5
                        //0 - 110 0
                    }
                }

            }
            else
            {
                GameObject currentObject;
                currentObject = Instantiate(Controller.smokeFX, col.transform.position, col.transform.rotation);
                Destroy(currentObject, 0.5f);
                Controller.anim.SetTrigger("Damage");
                Controller.anim.SetBool("Move", false);
            }
        }
    }
    void OnTriggerEnd(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Controller.MoveOn = false;

            //움직임을 0으로 만듦
            col.transform.Translate(Vector3.forward * 0);
            Controller.playerPosition.Set(Mathf.Round(col.transform.position.x), Mathf.Round(col.transform.position.y), Mathf.Round(col.transform.position.z));
            col.transform.position = Controller.playerPosition;
        }
    }

    void iTweenEnd()
    {
        CameraRay = new Ray(Camera.main.transform.position, Controller.tr.position - Camera.main.transform.position);
        if (Physics.Raycast(CameraRay, out rayHit))
        {
            if (rayHit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                //Debug.DrawLine(CameraRay.origin, rayHit.point, Color.red);
                rayHit.transform.gameObject.SetActive(false);
            }
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Controller.anim.Play("Walk");
        Controller.anim.SetBool("Walk", true);
        Invoke("CreateFX", 1.2f);
    }
    void CreateFX()
    {
        Controller.tr.gameObject.SetActive(false);
        currentFX = Instantiate(Controller.PortalT_FX, PortalPos, PortalRot);
        Destroy(currentFX, 0.5f);
        Invoke("Loadscene", 0.5f);
    }
    void Loadscene()
    {
        //최신 스테이지인지 확인
        if (UserManager.nStage < StageManager.stage)
        {
            UserManager.nStage = StageManager.stage;
            //최신 스테이지 클리어해야만 하트 획득
            //if (UserManager.nHeart < 5)
            //{
            //    //Debug.Log("최신스테이지클리어");
            //    UserManager.nHeart += 1;
            //}

        }

        // 클리어 했으므로 스테이지 증가
        StageManager.stage += 1;
        // 클리어했으므로 별 증가
        UserManager.nStar += 1;

        // 제한시간 이내에 클리어 했는지 확인
        if (StageManager.done > ProgressBar.checkTime)
        {
            UserManager.nStar += 1; //제한시간 안에 클리어 지금은 70초 중에서 30초만에 깨면 ㅇㅈ
        }

        //시간을 UserManager의 _time에 저장
        UserManager._time = ProgressBar.totalTime - StageManager.done;
        UserManager.current.saveData();
        Time.timeScale = 1;
        SceneManager.LoadScene("WinScenePortrait");
    }
}