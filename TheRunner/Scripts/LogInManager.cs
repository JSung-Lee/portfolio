using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInManager : MonoBehaviour {
    public GameObject loginInfoBoard;
    public PopUpManager popUpManager;
    public UserManager userManager;
    public Text nickName;
    public Text iMallId;
    public Text bestScore;

    private void OnEnable()
    {
        StartCoroutine(requestLoginInfo());
    }

    IEnumerator requestLoginInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", userManager.phoneNumber);
        string file = "checkinfo.php"; 
        WWW webRequest = new WWW(UserManager.hostUrl + file, form);
        yield return webRequest;
        if (webRequest.isDone)
        {
            if (webRequest.error == null)
            {
                try
                {
                    
                    string[] str = webRequest.text.Split("\n".ToCharArray());
                    //Debug.Log(webRequest.text);
                    if (str[1].Equals("b"))
                    {
                        popUpManager.openNonButtonPopUp("정지된 계정입니다.");
                        StartCoroutine(popUpManager.disableNonButtonPopUpAndOpen(1.0f, popUpManager.initionalize()));
                    }
                    else if (userManager.checkVersion(str[0]))
                    {
                        nickName.text = str[1];
                        iMallId.text = str[2];
                        bestScore.text = str[3];
                        popUpManager.closeTopBoard();
                        popUpManager.OpenBoard(loginInfoBoard);
                        userManager.nickname = str[1];
                        userManager.imallid = str[2];
                        userManager.setBestScore(System.Convert.ToInt32(str[3]));
                        if (str.Length >= 5)
                        {
                            userManager.pigEvent = (float)System.Convert.ToDouble(str[4]);
                            
                        }
                        else
                            userManager.pigEvent = 0.0f;
                    }
                    else
                    {
                        if (str[0].Equals("v"))
                        {
                            popUpManager.openNonButtonPopUp("연결상태를 확인하여주십시오.");
                            StartCoroutine(popUpManager.disableNonButtonPopUpAndOpen(1.0f, popUpManager.initionalize()));
                        }
                        else
                        {
                            //Debug.Log(webRequest.text);
                            popUpManager.openNonButtonPopUp("최신버전을 이용하여 주십시오." + "\n현재:" + userManager.version + "/최신:" + str[0]);
                            StartCoroutine(popUpManager.disableNonButtonPopUpAndOpen(1.0f, popUpManager.initionalize()));
                        }
                    }
                }
                catch (System.Exception e)
                {
                    popUpManager.openNonButtonPopUp("등록된 회원정보가 없습니다.");
                    StartCoroutine(popUpManager.disableNonButtonPopUpAndOpen(1.0f, popUpManager.initionalize()));
                }
            }
            else
            {
                popUpManager.setStringToNonButtonPopUp(webRequest.error + "!");
                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
            }
        }
    }

}
