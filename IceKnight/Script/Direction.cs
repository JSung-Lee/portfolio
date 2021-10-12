using UnityEngine;

public class Direction : MonoBehaviour {
    private Transform tr;
    private GameObject Dir_FX;
    private GameObject currentFX;
    public AudioClip DirectionSound;
    // Use this for initialization
    void Start()
    {
        Dir_FX = Resources.Load<GameObject>("Prefab/FX/Move");
        tr = GetComponent<Transform>();
    }
    void OnTriggerEnter(Collider col)
    {
        //신발 아이템을 먹지 않았다면 이동형 함정이 적용 됨.
        if(Controller.ShoesCheck == 0)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Controller.MoveOn = false;
                AudioSource.PlayClipAtPoint(DirectionSound, transform.position);

                if (tr.gameObject.layer == LayerMask.NameToLayer("Stop"))
                {
                    //Vector3.Lerp( 출발 좌표, 도착 좌표, float(1에 가까울수록 도착점에 닿음) ) 함수를 이용하여 Player를 해당 오브젝트 좌표로 이동시킨다.
                    col.transform.position = Vector3.Lerp(col.transform.position, this.gameObject.transform.position, 1.0f);

                }
                //오브젝트의 레이어 이름이 stop이 아니면
                //이동형 함정이며 각도에 따라 진행방향이 다름
                else
                {
                    //y좌표가 90은 Forward, 180은 Right, 270은 Back, 0은 Left 함정
                    if (tr.transform.eulerAngles.y == 90)
                    {
                        //Player의 좌표를 해당 함정 좌표로 도달하게 함
                        col.transform.position = Vector3.Lerp(col.transform.position, this.gameObject.transform.position, 1.0f);

                        //Forward방향으로 이동할때 설정되는 값으로 선언해주고 해당 방향으로 이동
                        Controller.pState = 1;
                        Controller.MoveOn = true;

                        Controller.tr.rotation = Quaternion.Euler(0, 0, 0);

                        currentFX = Instantiate(Dir_FX, tr.transform.position+Vector3.up, tr.transform.rotation);
                        Destroy(currentFX, 0.3f);
                        
                        col.transform.position += (Vector3.forward * Controller.step);
                    }
                    // Right 방향 이동함정
                    else if (tr.transform.eulerAngles.y == 180)
                    {
                        col.transform.position = Vector3.Lerp(col.transform.position, this.gameObject.transform.position, 1.0f);

                        Controller.pState = 2;
                        Controller.MoveOn = true;

                        Controller.tr.rotation = Quaternion.Euler(0, 90, 0);

                        currentFX = Instantiate(Dir_FX, tr.transform.position + Vector3.up, tr.transform.rotation);
                        Destroy(currentFX, 0.3f);
                        
                        col.transform.position += (Vector3.right * Controller.step);
                    }
                    // Down 방향 이동함정
                    else if (tr.transform.eulerAngles.y == 270)
                    {
                        col.transform.position = Vector3.Lerp(col.transform.position, this.gameObject.transform.position, 1.0f);

                        Controller.pState = 3;
                        Controller.MoveOn = true;

                        Controller.tr.rotation = Quaternion.Euler(0, 180, 0);
                        currentFX = Instantiate(Dir_FX, tr.transform.position + Vector3.up, tr.transform.rotation);
                        Destroy(currentFX, 0.3f);
                        
                        col.transform.position += (Vector3.back * Controller.step);
                    }
                    // Left 방향 이동함정
                    else if (tr.transform.eulerAngles.y == 0)
                    {
                        col.transform.position = Vector3.Lerp(col.transform.position, this.gameObject.transform.position, 1.0f);

                        Controller.pState = 4;
                        Controller.MoveOn = true;


                        Controller.tr.rotation = Quaternion.Euler(0, -90, 0);
                        currentFX = Instantiate(Dir_FX, tr.transform.position + Vector3.up, tr.transform.rotation);
                        Destroy(currentFX, 0.3f);
                        
                        col.transform.position += (Vector3.left * Controller.step);
                    }

                }

            }
        }
        
    }

}
