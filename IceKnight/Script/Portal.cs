using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Vector3 pos_extT;
    private Vector3 pos_extO;
    private GameObject currentFX;
    //public AudioClip PortalSound;

    //12 = I 13 = O 14 = T
    void Start()
    {
        if (this.gameObject.layer == 12)
        {
            for (int i = 0; i < StageManager.List_Portal.Count; i++)
            {
                if (StageManager.List_Portal[i].getType().Equals("OUTPUT"))
                {
                    pos_extO = StageManager.List_Portal[i].getPosition();
                    break;
                }
            }
        }

        if (this.gameObject.layer == 14)
        {
            for (int i = 0; i < StageManager.List_Portal.Count; i++)
            {
                if (StageManager.List_Portal[i].getType().Equals("TELEPORT") && StageManager.List_Portal[i].getPosition() != this.transform.position)
                {
                    pos_extT = StageManager.List_Portal[i].getPosition();
                    break;
                }
            }
        }
        // 현재 포탈이 Teleport이면

    }
    void OnTriggerEnter(Collider col)
    {
        // 충돌체가 플레이어면
        if (col.gameObject.layer == 8)
        {
            Controller.anim.SetTrigger("Damage");
            Controller.anim.SetBool("Move", false);
            switch (this.gameObject.layer)
            {
                // 인풋 포탈일때
                case 12:
                    Controller.MoveOn = false;
                    //AudioSource.PlayClipAtPoint(PortalSound, transform.position);
                    col.gameObject.SetActive(false);
                    col.gameObject.transform.position = this.transform.position;
                    currentFX = Instantiate(Controller.PortalI_FX, col.transform.position + Vector3.up * 0.5f, col.transform.rotation);
                    Destroy(currentFX, 0.5f);
                    Invoke("PortalI_Func", 0.5f);
                    break;
                // 텔레포트 포탈일때
                case 14:
                    if (Controller.MoveOn)
                    {
                        Controller.MoveOn = false;
                        //AudioSource.PlayClipAtPoint(PortalSound, transform.position);
                        col.gameObject.SetActive(false);
                        currentFX = Instantiate(Controller.PortalT_FX, col.transform.position + Vector3.up * 0.5f, col.transform.rotation);
                        col.gameObject.transform.position = this.transform.position;
                        Destroy(currentFX, 0.5f);
                        Invoke("PortalT_Func", 0.5f);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void PortalI_Func()
    {
        StageManager.Player_Current.transform.position = pos_extO;
        StageManager.Player_Current.SetActive(true);

        Quaternion PortalRot = new Quaternion();
        Vector3 PortalPos = new Vector3();
        if (StageManager.Player_Current.transform.eulerAngles.y == 0f)//up
        {
            PortalRot = Quaternion.Euler(0f, 0f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.back;
        }
        if (StageManager.Player_Current.transform.eulerAngles.y == 180f)//down
        {
            PortalRot = Quaternion.Euler(0f, 180f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.forward;
        }
        if (StageManager.Player_Current.transform.eulerAngles.y == 90f)//right
        {
            PortalRot = Quaternion.Euler(0f, 90f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.left;
        }
        if (StageManager.Player_Current.transform.eulerAngles.y == 270f)//left
        {
            PortalRot = Quaternion.Euler(0f, -90f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.right;
        }

        currentFX = Instantiate(Controller.PortalO_FX, PortalPos, PortalRot);
        Destroy(currentFX, 0.5f);
    }
    void PortalT_Func()
    {
        StageManager.Player_Current.transform.position = pos_extT;
        StageManager.Player_Current.SetActive(true);
        Quaternion PortalRot = new Quaternion();
        Vector3 PortalPos = new Vector3();
        if (StageManager.Player_Current.transform.eulerAngles.y == 0f)//up
        {
            PortalRot = Quaternion.Euler(0f, 0f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.back;
        }
        if (StageManager.Player_Current.transform.eulerAngles.y == 180f)//down
        {
            PortalRot = Quaternion.Euler(0f, 180f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.forward;
        }
        if (StageManager.Player_Current.transform.eulerAngles.y == 90f)//right
        {
            PortalRot = Quaternion.Euler(0f, 90f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.left;
        }
        if (StageManager.Player_Current.transform.eulerAngles.y == 270f)//left
        {
            PortalRot = Quaternion.Euler(0f, -90f, 0f);
            PortalPos = StageManager.Player_Current.transform.position + Vector3.up * 0.5f + Vector3.right;
        }

        currentFX = Instantiate(Controller.PortalT_FX, PortalPos, PortalRot);
        Destroy(currentFX, 0.5f);
    }
}
