using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IceBall : MonoBehaviour
{
    private Transform tr;
    private float speed = 1.0f;
    float iMove;
    private int iState = 2;
    //private Animation anim;
    int layerMask;

    RaycastHit hit;
    public AudioClip IceballSound;

    public static int iceTime;


    // Use this for initialization
    void Awake()
    {
        tr = GetComponent<Transform>();
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Floor"));
        //anim = this.GetComponentInChildren<Animation>();

        iceTime = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (tr.gameObject.name.Equals("Iceball_H(Clone)"))
        {
            iMove = speed * Time.deltaTime* iceTime;
            if (iState == 1)
            {
                speed = -1.0f;
            }
            else if (iState == 2)
            {
                speed = 1.0f;
            }

            tr.Translate(Vector3.right * iMove);



            if (Physics.Raycast(tr.position, tr.right * speed, 0.6f, layerMask))
            {
                if (iState == 2)
                {
                    tr.Translate(Vector3.right * 0);
                    iState = 1;
                    this.gameObject.GetComponentInChildren<Animator>().SetFloat("Direction", -1.0f);
                    
                }
                else if (iState == 1)
                {
                    tr.Translate(Vector3.right * 0);
                    iState = 2;
                    this.gameObject.GetComponentInChildren<Animator>().SetFloat("Direction", 1.0f);
                }
            }
        }
        if (tr.gameObject.name.Equals("Iceball_V(Clone)"))
        {
            iMove = speed * Time.deltaTime * iceTime;
            if (iState == 1)
            {
                speed = -1.0f;
            }
            else if (iState == 2)
            {
                speed = 1.0f;
            }

            tr.Translate(Vector3.forward * iMove);


            if (Physics.Raycast(tr.position, tr.forward * speed, 0.6f, layerMask))
            {
                if (iState == 2)
                {
                    tr.Translate(Vector3.forward * 0);
                    iState = 1;
                    this.gameObject.GetComponentInChildren<Animator>().SetFloat("Direction", -1.0f);
                }
                else if (iState == 1)
                {
                    tr.Translate(Vector3.forward * 0);
                    iState = 2;
                    this.gameObject.GetComponentInChildren<Animator>().SetFloat("Direction", 1.0f);
                }

            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AudioSource.PlayClipAtPoint(IceballSound, transform.position);
            Controller.MoveOn = false;
            if (!Controller.anim.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                Controller.anim.Play("Die");
                Controller.anim.SetBool("Move", false);
            }
            Invoke("testFunc", 1.5f);
        }
    }
    void testFunc() {
        if (PlayerPrefs.GetInt("nCheckRevival") == 1)
        {
            if (PlayerPrefs.GetInt("nRevival") > 0 && Controller.nRevival < PlayerPrefs.GetInt("nRevival"))
            {
                if (!Controller.isDie)
                {
                    DirectionManager.directionManager.GetComponent<DirectionManager>().OpenRevivlaPopup();
                    Time.timeScale = 0;
                }
            }
            else
            {
                Time.timeScale = 0;
                SceneManager.LoadScene("GameOverScenePortrait");
                Time.timeScale = 1;
            }
        }
        else
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("GameOverScenePortrait");
            Time.timeScale = 1;
        }
        Controller.isDie = true;
    }
}
