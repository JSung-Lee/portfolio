using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour {

    public GameObject[] points;        //길(점)
    public GameObject[] leftbeams;         //길(선)
    public GameObject[] rightbeams;         //길(선)
    private float pointVector;          //다음 생성될 길의 방향 벡터(좌상, 우상)
    private float minXDegree;           //생성되는 길의 최소 x벡터(nomalize안됨)
    private float maxXDegree;           //생성되는 길의 최대 x벡터
    private float minYDegree;           //생성되는 길의 최소 Y벡터(nomalize안됨)
    private float maxYDegree;           //생성되는 길의 최대 Y벡터
    public GameObject character;       //플레이어 캐릭터 게임 오브젝트
    public OrbManager orbManager;      //오브 매니저
    public UserManager userManager;
    public GameObject eventText;
    //private float timeScale;            //게임 속도
    // Use this for initialization
    void OnEnable() {
        Vector3 vec;                    //다음 길(점)의 위치
        minXDegree = 2.0f;              //길 최소 x설정(랜덤값의 최소)
        maxXDegree = 5.0f;              //길 최대 x설정(랜덤값의 최대)
        minYDegree = 10.0f;              //길 최소 x설정(랜덤값의 최소)
        maxYDegree = 30.0f;             //길 최대 x설정(랜덤값의 최대)
        bool isEventCheck = false;
        int eventCount = 0;
        if (userManager.pigEvent >= Random.Range(0.0f, 100.0f))
        {
            isEventCheck = true;
            eventCount = 4;
        }
        eventText.SetActive(isEventCheck);
        //Debug.Log(userManager.pigEvent);
        pointVector = 1;    //길이 생성될 방향 지정(음수 - 좌상, 양수 - 우상)
        for (int i = 0; i < points.Length; i++)  //시작 시 모든 포인트 좌표 지정
        {
            if (i == 0) vec = new Vector3(0, 1, -21);   //최초 포인트는 중앙 아랫부분에 생성
            else if (i == 1) vec = new Vector3(Random.Range(minXDegree * pointVector, maxXDegree * pointVector), 1,
                                   points[i - 1].transform.position.z + 26.0f); //각각 지정한 랜덤값의 범위 내에서 길(점)의 위치값 만들기
            else vec = new Vector3(Random.Range(minXDegree * pointVector, maxXDegree * pointVector), 1,
                                   points[i - 1].transform.position.z + Random.Range(minYDegree, maxYDegree)); //각각 지정한 랜덤값의 범위 내에서 길(점)의 위치값 만들기
            setWidth(i, 20.0f);                 //길의 양쪽 간격 조정
            points[i].transform.position = vec; //만들어둔 위치값으로 길(점)의 위치 설정
            pointVector *= -1.0f;               //다음 만들어질 방향 변경
            setBeamPositionStart(i, i);         //길(선)의 시작 포인트는 이번 생성된 길(점)의 위치
            setBeamPositionEnd(i - 1, i);       //길(선)의 끝 포인트는 다음 길(점)이 생성된 후 길(선)의 끝점을 다음 길(점)의 위치로 지정

        }
        for (int i = 2; i < points.Length; i++)  //시작 시 모든 포인트 좌표 지정
        {
            float sizeZ = points[i].transform.position.z - points[i - 1].transform.position.z;
            float ratio = Random.Range(0.0f, sizeZ) / sizeZ;
            float nZ = points[i - 1].transform.position.z + (sizeZ * ratio);
            float sizeX = leftbeams[i - 1].GetComponent<LineRenderer>().GetPosition(1).x -
                            leftbeams[i - 1].GetComponent<LineRenderer>().GetPosition(0).x;
            float nX = Random.Range(leftbeams[i - 1].GetComponent<LineRenderer>().GetPosition(0).x + (sizeX * ratio) + 2.5f,
                                    rightbeams[i - 1].GetComponent<LineRenderer>().GetPosition(0).x + (sizeX * ratio) - 2.5f);
            Vector3 orbPos = new Vector3(nX, 1, nZ);  //오브 위치 설정
            if (isEventCheck)
            {
                if (eventCount > 0)
                    eventCount--;
                else
                    isEventCheck = false;
                if (orbManager)
                    orbManager.createOrb(orbPos, isEventCheck);    //오브 생성
            }
            else if (Random.Range(0, 10) <= 9)  //처음만 90%확률로 오브 생성
            {
                if (orbManager)
                    orbManager.createOrb(orbPos, false);    //오브 생성
            }
            
        }
        //timeScale = 0.0f;
	}
	
	// Update is called once per frame
	void Update() {
        //if (timeScale < 1.0f)   //게임속도가 1보다 적으면 속도가 초당 0.2씩 상승
        //    timeScale += (Time.deltaTime * 0.2f);
        //else                    //1보다 크면 속도가 초당 0.02씩 상승
        //    timeScale += (Time.deltaTime * 0.045f);  
        //if (timeScale > 5.0f)   //최대 게임속도 지정
        //    timeScale = 5.0f;
        for (int i = 0;i<points.Length; i++)
        {
            int lastIndex = i - 1;  //이전 인덱스 지정
            if (lastIndex < 0) lastIndex = leftbeams.Length - 1;    //이전 인덱스가 음수면 마지막 인덱스가 이전인덱스
            if (character && character.transform.position.z - 40 > points[i].transform.position.z)   //길(점)이 화면 밖으로 벗어나면(화면이 캐릭터에 고정이므로 캐릭터 위치로 비교)
            {
                //points[i].SetActive(false);
                Vector3 vec = new Vector3(Random.Range(minXDegree * pointVector, (maxXDegree + (UserManager.timeScale * 0.8f)) * pointVector), 1,
                                          points[lastIndex].transform.position.z + Random.Range(minYDegree, maxYDegree));  //새로 생성될 위치값 만들기
                setWidth(i, 20.0f);                 //길의 양쪽 간격 조정
                points[i].transform.position = vec; //새로 생성한 위치로 바로 이동
                pointVector *= -1.0f;   //다음 길의 방향 변경
                setWidth(i, 20.0f - (UserManager.timeScale / 2.0f)); //게임속도가 증가함에 따라 길의 간격을 점점 좁게 함
                setBeamPositionEnd(lastIndex, i);   //끝점 지정(다음 길(점)이 올라오면 그전에 올라왔던 길(점)의 위치로 끝점 지정)
                if (Random.Range(0, 9) <= 4)  //50%확률로 오브 생성
                {
                    float sizeZ = points[i].transform.position.z - points[lastIndex].transform.position.z;
                    float ratio = Random.Range(0.0f, sizeZ) / sizeZ;
                    float nZ = points[lastIndex].transform.position.z + (sizeZ * ratio);
                    float sizeX = leftbeams[lastIndex].GetComponent<LineRenderer>().GetPosition(1).x -
                                    leftbeams[lastIndex].GetComponent<LineRenderer>().GetPosition(0).x;
                    float nX = Random.Range(leftbeams[lastIndex].GetComponent<LineRenderer>().GetPosition(0).x + (sizeX * ratio) + 1.0f,
                                        rightbeams[lastIndex].GetComponent<LineRenderer>().GetPosition(0).x + (sizeX * ratio) - 1.0f);
                    Vector3 orbPos = new Vector3(nX, 1, nZ);  //오브 위치 설정
                    orbManager.createOrb(orbPos);    //오브 생성
                }
            }
        }


	}

    void setBeamPositionStart(int beamIndex, int pointIndex)    //받은 인덱스의 길(선) 시작 포인트를 길(점)의 인덱스를 받아 그 위치로 지정
    {
        if (leftbeams.Length <= beamIndex || 0 > beamIndex)
            return; //길(선)의 배열 범위를 벗어나면 함수 종료
        LineRenderer beamLeft = leftbeams[beamIndex].GetComponent<LineRenderer>();  //길(선)의 왼쪽선
        LineRenderer beamRight = rightbeams[beamIndex].GetComponent<LineRenderer>();    //길(선)의 오른쪽 선
        beamLeft.SetPosition(0, points[pointIndex].GetComponent<LaserPoints>().left.transform.position);  //왼쪽 선의 시작점을 받은 인덱스의 길(점) 위치로 지정
        beamRight.SetPosition(0, points[pointIndex].GetComponent<LaserPoints>().right.transform.position);//오른쪽 선의 시작점을 받은 인덱스의 길(점) 위치로 지정
        beamLeft.SetPosition(1, points[pointIndex].GetComponent<LaserPoints>().left.transform.position);  //길(선)의 끝점도 시작점과 일치시켜 초기화
        beamRight.SetPosition(1, points[pointIndex].GetComponent<LaserPoints>().right.transform.position);//상동
        leftbeams[beamIndex].transform.parent.position = points[pointIndex].transform.position;    //길(선) 자체의 위치를 끝점과 동일 선상에 둠(실제론 이 위치에 생성되지 않음)
    }

    void setBeamPositionEnd(int beamIndex, int pointIndex)  //받은 인덱스의 길(선) 시작 포인트를 길(점)의 인덱스를 받아 그 위치로 지정
    {
        if (leftbeams.Length <= beamIndex || 0 > beamIndex)
            return; //길(선)의 배열 범위를 벗어나면 함수 종료
        LineRenderer beamLeft = leftbeams[beamIndex].GetComponent<LineRenderer>();  //길(선)의 왼쪽 선
        LineRenderer beamRight = rightbeams[beamIndex].GetComponent<LineRenderer>();//길(선)의 오른쪽 선
        beamLeft.SetPosition(1, points[pointIndex].GetComponent<LaserPoints>().left.transform.position);  //왼쪽 선의 끝점을 받은 인덱스의 길(점) 위치로 지정
        beamRight.SetPosition(1, points[pointIndex].GetComponent<LaserPoints>().right.transform.position);//오른쪽 선의 끝점을 받은 인덱스의 길(점) 위치로 지정
        leftbeams[beamIndex].transform.position = points[pointIndex].transform.position;    //길(선) 자체의 위치를 끝점과 동일 선상에 둠(실제론 이 위치에 생성되지 않음)
        beamLeft.transform.GetComponentInChildren<LineCollider>().updateCollider();
        beamRight.transform.GetComponentInChildren<LineCollider>().updateCollider();
    }

    void setWidth(int index, float width)
    {
        Vector3 vec = points[index].GetComponent<LaserPoints>().left.transform.position;  //왼쪽 길(점)의 좌표
        vec.x = points[index].transform.position.x - width / 2.0f;  //왼쪽 길(점)을 width의 절반만큼 왼쪽으로 이동
        points[index].GetComponent<LaserPoints>().left.transform.position = vec;  //이동한값 적용
        vec = points[index].GetComponent<LaserPoints>().right.transform.position; //오른쪽 길(점)의 좌표
        vec.x = points[index].transform.position.x + width / 2.0f;   //오른쪽 길(점)을 width의 절반만큼 오른쪽으로 이동
        points[index].GetComponent<LaserPoints>().right.transform.position = vec; //이동한 값 적용
        setBeamPositionStart(index, index); //이동한 길(점)의 위치로 연결된 길(선) 적용
    }

}
