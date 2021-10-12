using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeServer : MonoBehaviour
{

    [SerializeField]
    public static DateTime serverTime;
    public static DateTime turnOnTime;
    public static bool isConnet = true;
    DateTime startTime;
    IEnumerator getDate;
    // Use this for initialization
    private void OnEnable()
    {
        //getServerTime();
    }

    public void getServerTime()
    {
        startTime = DateTime.Now;
        if (isConnet)
        {
            getDate = GetNISTDate();
            StartCoroutine(getDate);
        }
        else
        {
            serverTime = DateTime.Now;
        }
        
    }

    public static void updateServerTime()
    {
        if (isConnet)
        {
            serverTime += DateTime.Now - turnOnTime;
        }
        else
        {
            serverTime = DateTime.Now;
        }
    }

    #region NTPTIME

    //NTP time 을 NIST 에서 가져오기
    // 4초 이내에 한번 이상 요청 하면, ip가 차단됩니다.

    public static DateTime GetDummyDate()
    {
        return new DateTime(2017, 12, 24); //to check if we have an online date or not.
    }

    public IEnumerator GetNISTDate()
    {
        
        //System.Random ran = new System.Random(DateTime.Now.Millisecond);
        DateTime date = GetDummyDate();
        string serverResponse = string.Empty;

        // NIST 서버 목록
        string[] servers = new string[] {
            //"time.bora.net",
            //"time.nuri.net",
            //"ntp.kornet.net",
            //"time.kriss.re.kr",
            //"time.nist.gov",
            //"maths.kaist.ac.kr",
            //"nist1-ny.ustiming.org",
            "time-a.nist.gov",
            //"nist1-chi.ustiming.org",
            "time.nist.gov",
            //"ntp-nist.ldsbc.edu",
            //"nist1-la.ustiming.org",
        };

        // 너무 많은 요청으로 인한 차단을 피하기 위해 한 서버씩 순환한다. 5번만 시도한다.
        for (int i = 0; i < servers.Length; i++)
        {
            try
            {
                int ranServer = i;
                    //ran.Next(0, servers.Length);
                // 서버 리스폰스를 표시한다. (디버그 확인용)
                if (Debug.isDebugBuild)
                    Debug.Log(servers[ranServer]);
                
                // StreamReader(무작위 서버)
                StreamReader reader = new StreamReader(new System.Net.Sockets.TcpClient(servers[ranServer], 13).GetStream());
                serverResponse = reader.ReadToEnd();
                reader.Close();

                // 시그니처가 있는지 확인한다.
                if (serverResponse.Length > 47 && serverResponse.Substring(38, 9).Equals("UTC(NIST)"))
                {
                    // 날짜 파싱
                    int jd = int.Parse(serverResponse.Substring(1, 5));
                    int yr = int.Parse(serverResponse.Substring(7, 2));
                    int mo = int.Parse(serverResponse.Substring(10, 2));
                    int dy = int.Parse(serverResponse.Substring(13, 2));
                    int hr = int.Parse(serverResponse.Substring(16, 2));
                    int mm = int.Parse(serverResponse.Substring(19, 2));
                    int sc = int.Parse(serverResponse.Substring(22, 2));

                    if (jd > 51544)
                        yr += 2000;
                    else
                        yr += 1999;

                    date = new DateTime(yr, mo, dy, hr, mm, sc);

                    // Exit the loop
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                /* 아무것도 하지 않고 다음 서버를 시도한다. */
            }
            yield return null;
        }
        if (date != GetDummyDate())
        {
            //한국시간
            TimeSpan _duration = System.TimeSpan.FromHours(9);
            serverTime = date.Add(_duration);
            turnOnTime = DateTime.Now;
        }
        else
        {
            serverTime = DateTime.Now;
            isConnet = false;
            Debug.Log("서버 시간 불러오기 실패");
        }
        Debug.Log("Time : " + serverTime.ToString());
        Debug.Log("NIST 응답 시간 : " + (DateTime.Now - startTime).Milliseconds + "ms");
        StopCoroutine(getDate);
    }
    #endregion
}