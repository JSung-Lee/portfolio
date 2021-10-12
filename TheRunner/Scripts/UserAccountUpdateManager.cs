using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
public class UserAccountUpdateManager : MonoBehaviour {
    public GameObject nickNameChangeBoard;
    public InputField nickNameInputField;
    public Text nickNameInfoText;
    public Text scoreText;
    public bool isCheckedNickName = false;
    public string usableNickName;

    [Header("SelectAccount")]
    public GameObject selectAccountPanel;
    public RectTransform selectAccountContents;
    public SelectAccountText[] selectAccountTexts;

    public PopUpManager popUpManager;
    public LogInManager logInManager;
    public UserManager userManager;
    Color colorBasic = new Color(1.0f, 1.0f, 1.0f, 0.785f);
    Color colorWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color colorAccess = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    Color colorFail = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    public void openNickNameChangeBoard()
    {
        popUpManager.OpenBoard(nickNameChangeBoard);
    }
    public void closeNickNameChangeBoard()
    {
        popUpManager.closeBoard(nickNameChangeBoard);
    }
    public void okNickNameChangeBoard()
    {
        if(isCheckedNickName)
        {
            StartCoroutine(SendChangedNickName());
        }
        else
        {
            popUpManager.openNonButtonPopUp("중복확인이 필요합니다.");
            StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
        }
    }
    IEnumerator SendChangedNickName()
    {
        WWWForm form = new WWWForm();
        Debug.Log(userManager.phoneNumber.ToString());
        //Debug.Log(usableNickName);
        form.AddField("phone", userManager.phoneNumber);
        form.AddField("nickname", usableNickName);
        string file = "nickupdate.php";
        WWW webRequest = new WWW(UserManager.hostUrl + file, form);
        yield return webRequest;
        if (webRequest.isDone)
        {
            if (webRequest.error == null)
            {
                //Debug.Log(webRequest.text);
                if (webRequest.text.Equals("o"))
                {
                    logInManager.nickName.text = usableNickName;
                    userManager.nickname = usableNickName;
                    userManager.saveData();
                    popUpManager.closeBoard(nickNameChangeBoard);
                    popUpManager.openNonButtonPopUp("변경 완료!");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
                else
                {
                    isCheckedNickName = false;
                    nickNameInfoText.color = colorFail;
                    nickNameInfoText.text = "사용이 불가능한 닉네임입니다.";
                    popUpManager.openNonButtonPopUp("사용이 불가능한 닉네임입니다.");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
            }
            else
            {
                popUpManager.openNonButtonPopUp(webRequest.error);
                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
            }
        }

    }
    public void checkNickNameDuplication()
    {
        nickNameInfoText.color = colorBasic;
        nickNameInfoText.text = "사용 가능 여부를 확인 중 입니다.";
        StartCoroutine(NickNameDuplicationCheck());
    }
    IEnumerator NickNameDuplicationCheck()
    {
        bool isSpecial = System.Text.RegularExpressions.Regex.IsMatch(nickNameInputField.text, @"[^a-zA-Z0-9가-힣]");

        if (isSpecial || nickNameInputField.text.Equals(""))
        {
            nickNameInfoText.color = colorFail;
            nickNameInfoText.text = "특수문자나 공백은 사용할 수 없습니다.";
            isCheckedNickName = false;
            usableNickName = nickNameInputField.text;
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("nickname", nickNameInputField.text);
            string file = "nickcheck.php";
            WWW webRequest = new WWW(UserManager.hostUrl + file, form);
            yield return webRequest;
            if (webRequest.isDone)
            {
                if (webRequest.error == null)
                {
                    Debug.Log(webRequest.text);
                    if (webRequest.text.Equals("o"))
                    {
                        nickNameInfoText.color = colorAccess;
                        nickNameInfoText.text = "사용 가능한 닉네임 입니다.";
                        isCheckedNickName = true;
                        usableNickName = nickNameInputField.text;
                    }
                    else if (webRequest.text.Equals("x"))
                    {
                        nickNameInfoText.color = colorFail;
                        nickNameInfoText.text = "이미 사용중인 닉네임 입니다.";
                        isCheckedNickName = false;
                        usableNickName = nickNameInputField.text;
                    }
                    else
                    {
                        nickNameInfoText.color = colorFail;
                        nickNameInfoText.text = "연결오류";
                        isCheckedNickName = false;
                        usableNickName = nickNameInputField.text;
                    }
                }
                else
                {
                    nickNameInfoText.color = colorFail;
                    nickNameInfoText.text = webRequest.error;
                }
            }
        }
    }

    public void openiMallIDChangeBoard()
    {
        popUpManager.openNonButtonPopUp("아이디를 확인하는 중 입니다.");
        StartCoroutine(CheckPhoneNumber());
    }
    public void closeiMallIDChangeBoard()
    {
        popUpManager.closeBoard(selectAccountPanel);
    }
    public void okiMallIDChangeBoard(Text iMallid)
    {
        popUpManager.openNonButtonPopUp("아이디를 변경하는 중 입니다.");
        StartCoroutine(SendChangediMallID(iMallid.text));
    }

    IEnumerator CheckPhoneNumber()
    {

        string file = "access.php";
        WWW accessTokenWeb = new WWW(UserManager.hostUrl + file);
        yield return accessTokenWeb;
        if (accessTokenWeb.isDone)
        {
            if (accessTokenWeb.text.Equals("x"))
            {
                popUpManager.openNonButtonPopUp("서버 점검 중 입니다.");
                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
            }
            else
            {
                string accessToken = accessTokenWeb.text;
                string[] tokens = accessToken.Split("\n".ToCharArray());
                //Debug.Log(accessTokenWeb.text);
                string client = UserManager.cafe24Url + userManager.phoneNumber;
                UnityWebRequest request = UnityWebRequest.Get(client);
                request.SetRequestHeader("Cache-Control", "no-cache");
                request.SetRequestHeader("content-type", "application/json");
                request.SetRequestHeader("authorization", "Bearer " + tokens[0]);
                yield return request.Send();
                if (request.isDone)
                {
                    if (request.isNetworkError)
                    {
                        popUpManager.openNonButtonPopUp(request.error);
                        StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                    }
                    else
                    {
                        //Debug.Log(request.downloadHandler.text);
                        JsonData data = JsonMapper.ToObject(request.downloadHandler.text);
                        if (data[0].Count == 1)
                        {
                            popUpManager.openNonButtonPopUp("변경 가능한 ID가 없습니다.");
                            StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                        }
                        else
                        {
                            if (data[0][0].ToString().Equals("401"))
                            {
                                popUpManager.openNonButtonPopUp("연결 오류");
                                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                            }
                            else
                            {
                                int count = data[0].Count;
                                if (count > selectAccountTexts.Length)
                                {
                                    count = selectAccountTexts.Length;
                                }
                                for (int i = 0; i < selectAccountTexts.Length; i++)
                                    selectAccountTexts[i].gameObject.SetActive(false);
                                for (int i = 0; i < count; i++)
                                {
                                    selectAccountTexts[i].GetComponent<RectTransform>().anchoredPosition =
                                        new Vector3(0, ((float)i - ((float)(count - 1) * 0.5f)) * 180.0f, 0.0f);
                                    selectAccountTexts[i].setOnAccount(data[0][i]["member_id"].ToString());
                                }
                                selectAccountContents.anchoredPosition = new Vector2(0, count * -180.0f);
                                selectAccountContents.sizeDelta = new Vector2(0, count * -180.0f);
                                
                                popUpManager.OpenBoard(selectAccountPanel);
                                if (data[0].Count > selectAccountTexts.Length)
                                {
                                    popUpManager.openNonButtonPopUp("10개까지만 표시됩니다.");
                                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                                }

                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator SendChangediMallID(string iMallid)
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", userManager.phoneNumber);
        form.AddField("imallid", iMallid);
        string file = "idupdate.php";
        WWW webRequest = new WWW(UserManager.hostUrl + file, form);
        yield return webRequest;
        if (webRequest.isDone)
        {
            if (webRequest.error == null)
            {
                //Debug.Log(webRequest.text);
                if (webRequest.text.Equals("o"))
                {
                    logInManager.iMallId.text = iMallid;
                    userManager.imallid = iMallid;
                    userManager.saveData();
                    popUpManager.closeBoard(selectAccountPanel);
                    popUpManager.openNonButtonPopUp("변경 완료!");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
                else
                {
                    //popUpManager.closeNonButtonPopUp();
                    isCheckedNickName = false;
                    nickNameInfoText.color = colorFail;
                    nickNameInfoText.text = "사용이 불가능한 아이디입니다.";
                    popUpManager.openNonButtonPopUp("사용이 불가능한 아이디입니다.");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
            }
            else
            {
                popUpManager.openNonButtonPopUp(webRequest.error);
                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
            }
        }

    }
    private void Update()
    {
        if(nickNameChangeBoard.activeSelf)
        {
            if (nickNameInputField.text != usableNickName)
            {
                nickNameInfoText.color = colorFail;
                nickNameInfoText.text = "중복확인이 필요합니다.";
                isCheckedNickName = false;
                usableNickName = nickNameInputField.text;
            }
        }
    }

    public void onClickResetScore()
    {
        popUpManager.openYesNoButtonPopUp("최고 점수를 삭제하시겠습니까?");
        popUpManager.setCallBackToYesNoButtonPopUp(popUpManager.callBack_ResetScore(SendResetScore()));
    }

    public IEnumerator SendResetScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", userManager.phoneNumber);
        string file = "delete.php";
        WWW webRequest = new WWW(UserManager.hostUrl + file, form);
        yield return webRequest;
        if (webRequest.isDone)
        {
            if (webRequest.error == null)
            {
                //Debug.Log(webRequest.text);
                if (webRequest.text.Equals("o"))
                {
                    userManager.setBestScore(0.0f);
                    scoreText.text = "0";
                    userManager.saveData();
                    popUpManager.openNonButtonPopUp("삭제 완료!");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
                else
                {
                    nickNameInfoText.text = "연결 삭제 실패!";
                    popUpManager.openNonButtonPopUp("점수 삭제 실패!");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
            }
            else
            {
                popUpManager.openNonButtonPopUp(webRequest.error);
                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
            }
        }
    }
}
