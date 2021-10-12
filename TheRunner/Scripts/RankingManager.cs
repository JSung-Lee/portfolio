using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour {
    public Ranking_UserData[] users;
    public Ranking_UserData playerData;
    public UserManager userManager;
    string dbURL = UserManager.hostUrl;
    
    private void OnEnable()
    {
        for(int i =0;i<users.Length;i++)
        {
            users[i].rank = "0";
        }
        StartCoroutine(DownLoadScore());
    }

    IEnumerator DownLoadScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", userManager.phoneNumber);
        string file = "download.php";
        WWW webRequest = new WWW(dbURL + file, form);
        yield return webRequest;
        if (webRequest.isDone)
        {
            if (webRequest.error == null)
            {
                //Debug.Log(webRequest.text);
                string[] userData = webRequest.text.Split("\n".ToCharArray());
                for (int i = 0; i < 10; i++)
                {
                    
                    //Debug.Log(userData[i]);
                    if (i >= userData.Length - 2)
                    {
                        users[i].rank = "0";
                        users[i].UpdateText();
                        continue;
                    }
                    string[] datas = userData[i].Split(" ".ToCharArray(), 3);
                    users[i].rank = datas[0];
                    users[i].nickName = datas[1].ToString();
                    users[i].score = datas[2];
                    users[i].isMe = false;
                    if (users[i].nickName.Equals(userManager.nickname))
                        users[i].isMe = true;
                    if (i > 0 && users[i - 1].score.Equals(datas[2]))
                        users[i].rank = users[i - 1].rank;
                    users[i].UpdateText();
                }
                string[] data = userData[userData.Length - 2].Split(" ".ToCharArray(), 3);
                playerData.isMe = true;
                playerData.rank = data[0];
                playerData.nickName = data[1];
                playerData.score = data[2];
                playerData.UpdateText();
            }
            else
            {

            }
        }
    }
}
