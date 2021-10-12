using UnityEngine;

public class CrushBall : MonoBehaviour
{
    public Vector3 CurrentPosition;
    public  GameObject thisObject;
    private GameObject currentFX;
    private GameObject crushFX;
    private GameObject Crush_FX;

    public AudioClip CrushSound;

    void Start()
    {
        Crush_FX = Resources.Load<GameObject>("Prefab/FX/SparkExplosion");
        thisObject = this.gameObject;
        CurrentPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.5f, this.gameObject.transform.position.z);
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        currentFX = Instantiate(Controller.smokeFX, Controller.tr.position, Controller.tr.rotation);
        Destroy(currentFX, 0.5f);
        if (Controller.MoveOn)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {

                switch (Controller.pState)
                {
                    case 1:
                        Controller.MoveOn = false;
                        Controller.CrushCheck = 1;
                        col.gameObject.transform.position = CurrentPosition + Vector3.back;
                        Controller.anim.Play("Damage");
                        Controller.anim.SetBool("Move", false);
                        break;

                    case 2:
                        Controller.MoveOn = false;
                        Controller.CrushCheck = 2;
                        col.gameObject.transform.position = CurrentPosition + Vector3.left;
                        Controller.anim.Play("Damage");
                        Controller.anim.SetBool("Move", false);
                        break;

                    case 3:
                        Controller.MoveOn = false;
                        Controller.CrushCheck = 3;
                        col.gameObject.transform.position = CurrentPosition + Vector3.forward;
                        Controller.anim.Play("Damage");
                        Controller.anim.SetBool("Move", false);
                        break;

                    case 4:
                        Controller.MoveOn = false;
                        Controller.CrushCheck = 4;
                        col.gameObject.transform.position = CurrentPosition + Vector3.right;
                        Controller.anim.Play("Damage");
                        Controller.anim.SetBool("Move", false);
                        break;

                }
            }
        }
    }

    void swipe(float rotY)
    {
        float swipeDistance = (Controller.endPos - Controller.startPos).magnitude;
        float swipeTime = Controller.endTime - Controller.startTime;
        if (Controller.PushOn)
        {
            if (swipeTime < Controller.maxTime && swipeDistance >= Controller.minSwipeDistance)
            {
                Vector2 distance = Controller.endPos - Controller.startPos;
                if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
                {
                    if (distance.x > 0)
                    {
                        if (Controller.CrushCheck == 2 && rotY == 90f && thisObject.transform.position.x - Controller.tr.position.x == 1 && thisObject.transform.position.z == Controller.tr.position.z)
                        {
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Crush");
                            Controller.isCrush = true;
                            Invoke("canDestroy", 0.6f);
                        }
                    }
                    if (distance.x < 0)
                    {
                        if (Controller.CrushCheck == 4 && rotY == 270f && thisObject.transform.position.x - Controller.tr.position.x == -1 && thisObject.transform.position.z == Controller.tr.position.z)
                        {
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Crush");
                            Controller.isCrush = true;
                            Invoke("canDestroy", 0.6f);
                        }
                    }
                }
                else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
                {
                    if (distance.y > 0)
                    {
                        if (Controller.CrushCheck == 1 && rotY == 0f && thisObject.transform.position.z - Controller.tr.position.z == 1 && thisObject.transform.position.x == Controller.tr.position.x)
                        {
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Crush");
                            Controller.isCrush = true;
                            Invoke("canDestroy", 0.6f);
                        }
                    }
                    if (distance.y < 0)
                    {
                        if (Controller.CrushCheck == 3 && rotY == 180f && thisObject.transform.position.z - Controller.tr.position.z == -1 && thisObject.transform.position.x == Controller.tr.position.x)
                        {
                            Controller.anim.SetBool("Move", false);
                            Controller.anim.Play("Crush");
                            Controller.isCrush = true;
                            Invoke("canDestroy", 0.6f);
                        }
                    }
                }
            }
        }
    }
    void canDestroy()
    {
        AudioSource.PlayClipAtPoint(CrushSound, transform.position);
        crushFX = Instantiate(Crush_FX, this.transform.position + Vector3.up * 0.5f, Quaternion.Euler(0,0,0));
        Destroy(crushFX, 3f);
        StageManager.List_Crush.RemoveAt(StageManager.List_Crush.FindIndex(transform => transform.transform.position == thisObject.transform.position));
        Controller.MoveOn = true;
        Controller.isCrush = false;
        Destroy(thisObject);
    }
}