using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton<CameraManager>
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public float angle = 65f;      //카메라와 타겟간의 각도
    public float distance = 20f;   //타겟과 카메라 간의 최대 거리
    public float scroll = 1f;      //최대거리 중 x%만큼의 거리로 스크롤링(0~1)

    public bool isPossibleMove = true;
    public float sensitiveArea = 0.1f;
    public float moveSpeed = 25f;

    public Vector3 lookPos;        //타겟위치

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined; //마우스가 화면밖으로 나가지 않음

        lookPos = Vector3.zero;
    }

    private void LateUpdate()
    {
        MoveCamera();
        LookPos();
    }

    public void FocusOn(Vector3 pos)
    {
        lookPos = pos;  //카메라가 보는 타겟   
    }

    //카메라가 lookPos를 각도와 거리에 맞추어 보게 하는 함수
    public void LookPos()   
    {
        //카메라의 각도가 적용될 방향 벡터
        Vector3 up = Vector3.up;
        Vector3 back = Vector3.back;

        //각도 함수 적용을 위해 degree -> radian
        var RadAngle = angle * Mathf.Deg2Rad;
        //각도를 적용시켰을때 카메라가 이동될 [벡터]값 계산
        Vector3 vec = new Vector3(0, up.y * Mathf.Sin(RadAngle), back.z * Mathf.Cos(RadAngle));
        //위에서 구한 벡터에 거리를 적용
        vec *= distance * scroll;
        //거리값을 카메라가 보게될 위치에 적용
        vec += lookPos;

        //계산된 값을 카메라에 적용
        transform.position = vec;
        transform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));
    }

    //마우스 위치에 따른 화면 이동 함수
    public void MoveCamera()
    {
        //마우스의 앵커 => 좌측하단 : 0,0
        if (Input.mousePresent && isPossibleMove)
        {
            var dir = Vector3.zero;
            var pos = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                                  Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));

            //top?
            if (pos.y > Screen.height * (1 - sensitiveArea) || Input.GetKey(KeyCode.DownArrow))
            {
                //끝에 얼마나 가까운가
                float ratio = pos.y / Screen.height;
                //가까운 정도를 0~1로 변환 및 대입(이에 따라 카메라 이동)
                dir.z = (ratio - (1 - sensitiveArea)) / sensitiveArea;
                if (SettingManager.self) {
                    var setting = SettingManager.self;
                    float speed = Input.GetKey(KeyCode.DownArrow) ? setting.game_CameraSpeed_Keyboard : setting.game_CameraSpeed_Mouse;
                    speed *= 0.05f;
                    dir.z = speed;
                }
            }

            //bottom?
            if (pos.y < Screen.height * sensitiveArea || Input.GetKey(KeyCode.UpArrow))
            {
                //끝에 얼마나 가까운가
                float ratio = pos.y / Screen.height;
                //가까운 정도를 0~1로 변환 및 대입(이에 따라 카메라 이동)
                dir.z = -(1 - (ratio / sensitiveArea));
                if (SettingManager.self) {
                    var setting = SettingManager.self;
                    float speed = Input.GetKey(KeyCode.UpArrow) ? setting.game_CameraSpeed_Keyboard : setting.game_CameraSpeed_Mouse;
                    speed *= 0.05f;
                    dir.z = speed;
                }
            }

            //right?
            if (pos.x > Screen.width * (1 - sensitiveArea) || Input.GetKey(KeyCode.LeftArrow))
            {
                //끝에 얼마나 가까운가
                float ratio = pos.x / Screen.width;
                //가까운 정도를 0~1로 변환 및 대입(이에 따라 카메라 이동)
                dir.x = (ratio - (1 - sensitiveArea)) / sensitiveArea;
                if (SettingManager.self) {
                    var setting = SettingManager.self;
                    float speed = Input.GetKey(KeyCode.LeftArrow) ? setting.game_CameraSpeed_Keyboard : setting.game_CameraSpeed_Mouse;
                    speed *= 0.05f;
                    dir.z = speed;
                }
            }

            //left?
            if (pos.x < Screen.width * sensitiveArea || Input.GetKey(KeyCode.RightArrow)) 
            {
                //끝에 얼마나 가까운가
                float ratio = pos.x / Screen.width;
                //가까운 정도를 0~1로 변환 및 대입(이에 따라 카메라 이동)
                dir.x = -(1 - (ratio / sensitiveArea));
                if (SettingManager.self) {
                    var setting = SettingManager.self;
                    float speed = Input.GetKey(KeyCode.RightArrow) ? setting.game_CameraSpeed_Keyboard : setting.game_CameraSpeed_Mouse;
                    speed *= 0.05f;
                    dir.z = speed;
                }
            }
            //이동 속도 고정을 위해 정규화
            dir.Normalize();
            //이동
            lookPos += dir * moveSpeed * Time.deltaTime;
        }

        
    }

    public Vector3 GetCameraViewVector()
    {
        var result = lookPos - transform.position;
        return result.normalized;
    }
}
