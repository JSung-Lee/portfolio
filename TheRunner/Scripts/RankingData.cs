using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
public class RankingData : MonoBehaviour {

    public UserManager user;
    public Text bestScore;
    public Text nickName;
    public Text userName;
    public Text mileage;
    void OnEnable() {
        bestScore.text = ((int)user.getBestScore()).ToString();
        nickName.text = user.nickname;
        userName.text = user.imallid;
        mileage.text = "조회중";
        StartCoroutine(loadUsableMileage());
    }

    IEnumerator loadUsableMileage()
    {
        string file = "access.php";
        WWW accessTokenWeb = new WWW(UserManager.hostUrl + file);
        yield return accessTokenWeb;
        if (accessTokenWeb.isDone)
        {
            if (accessTokenWeb.text.Equals("x"))
            {
                mileage.text = "조회 실패";
            }
            else
            {
                string accessToken = accessTokenWeb.text;
                string[] tokens = accessToken.Split("\n".ToCharArray());
                //Debug.Log(accessTokenWeb.text);
                string client = UserManager.cafe24Url + user.phoneNumber;
                UnityWebRequest request = UnityWebRequest.Get(client);
                request.SetRequestHeader("Cache-Control", "no-cache");
                request.SetRequestHeader("content-type", "application/json");
                request.SetRequestHeader("authorization", "Bearer " + tokens[0]);
                yield return request.Send();
                if (request.isDone)
                {
                    if (request.isNetworkError)
                        mileage.text = "조회 실패";
                    else
                    {
                        //Debug.Log(request.downloadHandler.text);
                        JsonData data = JsonMapper.ToObject(request.downloadHandler.text);
                        if (data[0].Count < 1)
                        {
                            mileage.text = "조회 실패";
                        }
                        else if (data[0].Count == 1)
                        {
                            mileage.text = data[0][0]["available_mileage"].ToString().Split(".".ToCharArray())[0];
                        }
                        else
                        {
                            if (data[0][0].ToString().Equals("401"))
                            {
                                mileage.text = "조회 실패";
                            }
                            else
                            {
                                for (int i = 0; i < data[0].Count; i++)
                                {
                                    if (data[0][i]["member_id"].ToString().Equals(user.imallid))
                                    {
                                        mileage.text = data[0][i]["available_mileage"].ToString().Split(".".ToCharArray())[0];
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
    }

}
