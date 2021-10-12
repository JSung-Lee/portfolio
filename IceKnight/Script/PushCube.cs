using UnityEngine;

public class PushCube : MonoBehaviour
{

    private Transform tr;
    int layerMask;
    int layerMask2;
    RaycastHit Player_hit, back_Player_hit;
    private GameObject currentFX;
    private GameObject pushFX;
    private GameObject Push_FX;
    public AudioClip PushSound;
    // Use this for initialization
    void Start()
    {
        Push_FX = Resources.Load<GameObject>("Prefab/FX/CartoonyPunchLight");
        tr = GetComponent<Transform>();

        layerMask = (-1) - (1 << LayerMask.NameToLayer("Floor"));
        layerMask2 = 1 << LayerMask.NameToLayer("Floor") | 1 << LayerMask.NameToLayer("Player");
        layerMask2 = ~layerMask2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.GlovesCheck == 1)
        {
            //////////////////// Push_Cube 뒤에 Player가 있는지 체크하고 있을때 Push_Cube 앞에 충돌체가 있으면 움직이면 안됨
            // 나머지 if는 player가 왼쪽에 있을 때 오른쪽 체크, 오른쪽에 있을때 왼쪽 체크 등등
            if (Physics.Raycast(tr.position, tr.forward * (-1), out back_Player_hit, 0.7f, layerMask))
            {
                if (back_Player_hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (Physics.Raycast(tr.position, tr.forward, 0.7f, layerMask))
                    {
                        Controller.PlayerCheck = 0;
                    }
                }
            }
            //Player가 왼쪽에 있을때 오른쪽에 어떤 오브젝트가 있는지 체크
            if (Physics.Raycast(tr.position, tr.right * (-1), out back_Player_hit, 0.7f, layerMask))
            {
                if (back_Player_hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (Physics.Raycast(tr.position, tr.right, 0.7f, layerMask))
                    {
                        Controller.PlayerCheck = 0;
                    }
                }
            }
            //Player가 앞쪽에 있을때 뒤쪽에 어떤 오브젝트가 있는지 체크
            if (Physics.Raycast(tr.position, tr.forward, out back_Player_hit, 0.7f, layerMask))
            {
                if (back_Player_hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (Physics.Raycast(tr.position, tr.forward * (-1), 0.7f, layerMask))
                    {
                        Controller.PlayerCheck = 0;
                    }

                }
            }
            //Player가 오른쪽에 있을때 왼쪽에 어떤 오브젝트가 있는지 체크
            if (Physics.Raycast(tr.position, tr.right, out back_Player_hit, 0.7f, layerMask))
            {
                if (back_Player_hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (Physics.Raycast(tr.position, tr.right * (-1), 0.7f, layerMask))
                    {
                        Controller.PlayerCheck = 0;
                    }
                }
            }
        }//end GlovesCheck
    }// end Update
    void swipe(float[] rotY)
    {
        if (Controller.isPush)
            return;


        if (Controller.GlovesCheck == 1)
        {
            //////////////////// 플레이어 진행 방향에 Push_Cube가 있을때 진행방향으로 push
            //                  Ctr.tr == 플레이어의 tr 
            if (Physics.Raycast(Controller.tr.position, Controller.tr.forward, out Player_hit, 0.7f, 1 << LayerMask.NameToLayer("Push")))
            {
                if (Player_hit.transform.gameObject.layer == LayerMask.NameToLayer("Push"))
                {
                    if (Controller.PlayerCheck == 1)
                    {
                        if ((Input.GetKeyDown(KeyCode.UpArrow) || (rotY[0] == 0f && rotY[1] == 0f)) && Controller.pState == 1)
                        {
                            AudioSource.PlayClipAtPoint(PushSound, transform.position);
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Push");
                            Controller.anim.SetBool("Push", true);
                            Controller.isPush = true;
                            Invoke("canPushCube", 0.4f);
                        }
                        else if ((Input.GetKeyDown(KeyCode.RightArrow) || (rotY[0] == 90f && rotY[1] == 90f)) && Controller.pState == 2)
                        {
                            AudioSource.PlayClipAtPoint(PushSound, transform.position);
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Push");
                            Controller.anim.SetBool("Push", true);
                            Controller.isPush = true;
                            Invoke("canPushCube", 0.4f);
                        }
                        else if ((Input.GetKeyDown(KeyCode.DownArrow) || (rotY[0] == 180f && rotY[1] == 180f)) && Controller.pState == 3)
                        {
                            AudioSource.PlayClipAtPoint(PushSound, transform.position);
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Push");
                            Controller.anim.SetBool("Push", true);
                            Controller.isPush = true;
                            Invoke("canPushCube", 0.4f);
                        }
                        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || (rotY[0] == 270f && rotY[1] == 270f)) && Controller.pState == 4)
                        {
                            AudioSource.PlayClipAtPoint(PushSound, transform.position);
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Push");
                            Controller.anim.SetBool("Push", true);
                            Controller.isPush = true;
                            Invoke("canPushCube", 0.4f);
                        }
                    }
                    else
                    {
                        currentFX = Instantiate(Controller.smokeFX, Controller.tr.position, Controller.tr.rotation);
                        Destroy(currentFX, 0.5f);
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(Controller.tr.position, Controller.tr.forward, out Player_hit, 0.7f, 1 << LayerMask.NameToLayer("Push")))
            {
                if (Player_hit.transform.gameObject.layer == LayerMask.NameToLayer("Push"))
                {
                    currentFX = Instantiate(Controller.smokeFX, Controller.tr.position, Controller.tr.rotation);
                    Destroy(currentFX, 0.5f);
                }
            }
        }

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player_P"))
        {
            currentFX = Instantiate(Controller.smokeFX, col.transform.position, col.transform.rotation);
            Destroy(currentFX, 0.5f);
        }
    }

    void canPushCube()
    {
        if (Physics.Raycast(Controller.tr.position, Controller.tr.forward, out Player_hit, 0.6f, 1 << LayerMask.NameToLayer("Push")))
        {
            if (Player_hit.transform.gameObject.layer == LayerMask.NameToLayer("Push"))
            {
                if (Controller.pState == 1)
                {
                    pushFX = Instantiate(Push_FX, Controller.tr.position + Vector3.up + Vector3.forward * 1.5f, Controller.tr.rotation);
                    Player_hit.transform.Translate(Vector3.forward);
                    Controller.tr.position += (Vector3.forward * Controller.step);
                }
                else if (Controller.pState == 2)
                {
                    pushFX = Instantiate(Push_FX, Controller.tr.position + Vector3.up + Vector3.right * 1.5f, Controller.tr.rotation);
                    Player_hit.transform.Translate(Vector3.right);
                    Controller.tr.position += (Vector3.right * Controller.step);
                }
                else if (Controller.pState == 3)
                {
                    pushFX = Instantiate(Push_FX, Controller.tr.position + Vector3.up + Vector3.back * 1.5f, Controller.tr.rotation);
                    Player_hit.transform.Translate(Vector3.back);
                    Controller.tr.position += (Vector3.back * Controller.step);
                }
                else if (Controller.pState == 4)
                {
                    pushFX = Instantiate(Push_FX, Controller.tr.position + Vector3.up + Vector3.left * 1.5f, Controller.tr.rotation);
                    Player_hit.transform.Translate(Vector3.left);
                    Controller.tr.position += (Vector3.left * Controller.step);
                }
                Destroy(pushFX, 0.5f);
                Controller.PlayerCheck = 0;
                Controller.anim.SetBool("Push", false);
                Controller.isPush = false;
            }
        }
    }
}