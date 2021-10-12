using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
public class AccountManager : MonoBehaviour {
    [Header("Board")]
    public InputField nickNameInputField;
    public Text nickNameInfoText;
    public InputField phoneInputField;
    public Text idInfoText;

    [Header("SelectAccount")]
    public GameObject selectAccountPanel;
    public RectTransform selectAccountContents;
    public SelectAccountText[] selectAccountTexts;

    [Header("Managers")]
    public PopUpManager popUpManager;
    public UserManager userManager;

    bool isUsedAccount = false;
    bool isCheckedNickName = false;
    bool isCheckedPhoneNumber = false;
    string usableNickName = "";
    string usableID = "";
    string usablePhoneNumber = "";
    string usedAccountNickName = "";
    Color colorBasic = new Color(1.0f, 1.0f, 1.0f, 0.785f);
    Color colorAccess = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    Color colorFail = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    private void OnEnable()
    {
        phoneInputField.text = "";
        nickNameInputField.text = "";
        usablePhoneNumber = phoneInputField.text;
        usableNickName = nickNameInputField.text;
        idInfoText.color = colorBasic;
        idInfoText.text = "휴대폰번호 입력시 ID가 조회됩니다.";
        nickNameInfoText.text = "중복확인이 필요합니다.";
        nickNameInfoText.color = colorBasic;
        isUsedAccount = false;
        isCheckedNickName = false;
        isCheckedPhoneNumber = false;
        usedAccountNickName = "";
    }

    public void onClickNickNameDuplicationCheckButton()
    {
        if (isUsedAccount)
        {
            if (usedAccountNickName.Equals(nickNameInputField.text))
            {
                nickNameInfoText.color = colorAccess;
                nickNameInfoText.text = "사용 가능한 닉네임 입니다.";
                isCheckedNickName = true;
                usableNickName = nickNameInputField.text;
                return;
            }
        }
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
                    //Debug.Log(webRequest.text);
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

    public void onClickIMallIDCheckButton()
    {
        idInfoText.color = colorBasic;
        idInfoText.text = "가입 여부를 확인하는 중입니다.";
        StartCoroutine(CheckPhoneNumber());
    }

    IEnumerator CheckPhoneNumber()
    {
        isUsedAccount = false;
        if (!phoneInputField.text.Equals(""))
        {
            phoneInputField.text = phoneInputField.text.Replace("-", "");
            phoneInputField.text = phoneInputField.text.Insert(3, "-");
            phoneInputField.text = phoneInputField.text.Insert(phoneInputField.text.Length - 4, "-");
            usablePhoneNumber = phoneInputField.text;
            WWWForm form = new WWWForm();
            form.AddField("phone", phoneInputField.text);
            string file = "checkinfo.php";
            WWW webRequest = new WWW(UserManager.hostUrl + file, form);
            yield return webRequest;
            if (webRequest.isDone)
            {
                if (webRequest.error == null)
                {
                    string[] strs = webRequest.text.Split("\n".ToCharArray());
                    if (userManager.checkVersion(strs[0]))
                    {
                        if (strs[1].Equals("x"))
                        {
                            file = "access.php";
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
                                    string client = UserManager.cafe24Url + phoneInputField.text;
                                    UnityWebRequest request = UnityWebRequest.Get(client);
                                    request.SetRequestHeader("Cache-Control", "no-cache");
                                    request.SetRequestHeader("content-type", "application/json");
                                    request.SetRequestHeader("authorization", "Bearer " + tokens[0]);
                                    yield return request.Send();
                                    if (request.isDone)
                                    {
                                        if (request.isNetworkError)
                                            idInfoText.text = request.error;
                                        else
                                        {
                                            try
                                            {
                                                //Debug.Log(request.downloadHandler.text);
                                                JsonData data = JsonMapper.ToObject(request.downloadHandler.text);
                                                if (data[0].Count == 1)
                                                {
                                                    idInfoText.color = colorAccess;
                                                    idInfoText.text = data[0][0]["member_id"].ToString();
                                                    usableID = idInfoText.text;
                                                    usablePhoneNumber = phoneInputField.text;
                                                    isCheckedPhoneNumber = true;
                                                }
                                                else
                                                {
                                                    if (data[0][0].ToString().Equals("401"))
                                                    {
                                                        idInfoText.color = colorFail;
                                                        idInfoText.text = "연결 오류";
                                                        usableID = "";
                                                        usablePhoneNumber = phoneInputField.text;
                                                    }
                                                    else
                                                    {
                                                        int count = data[0].Count;
                                                        if (count > selectAccountTexts.Length)
                                                        {
                                                            count = selectAccountTexts.Length;
                                                        }
                                                        usablePhoneNumber = phoneInputField.text;
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
                                            catch (System.Exception e)
                                            {
                                                popUpManager.openYesNoButtonPopUp("가입된 아이몰ID가 없습니다.", "가입하기", "닫기");
                                                popUpManager.setCallBackToYesNoButtonPopUp(popUpManager.callBack_GoToCreateAccount());
                                                idInfoText.color = colorFail;
                                                idInfoText.text = "가입 되지 않은 정보입니다.";
                                                usableID = "";
                                                usablePhoneNumber = phoneInputField.text;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (strs[1].Equals("b"))
                        {
                            popUpManager.openNonButtonPopUp("정지된 계정입니다.");
                            StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                        }
                        else
                        {
                            //Debug.Log(webRequest.text);
                            string[] str = webRequest.text.Split("\n".ToCharArray());
                            usablePhoneNumber = phoneInputField.text;
                            isCheckedPhoneNumber = true;

                            nickNameInputField.text = str[1];
                            nickNameInfoText.color = colorAccess;
                            nickNameInfoText.text = "사용 가능한 닉네임 입니다.";
                            isCheckedNickName = true;
                            usableNickName = nickNameInputField.text;
                            usedAccountNickName = usableNickName;

                            idInfoText.text = str[2];
                            idInfoText.color = colorAccess;
                            usableID = idInfoText.text;

                            isUsedAccount = true;
                            //popUpManager.openYesNoButtonPopUp("이미 가입된 정보 입니다.", "로그인", "닫기");
                            //popUpManager.setCallBackToYesNoButtonPopUp(popUpManager.callBack_GoToLogIn(userManager, str, phoneInputField.text));
                        }
                    }
                    else
                    {
                        if (strs[0].Equals("v"))
                        {
                            popUpManager.openNonButtonPopUp("연결상태를 확인하여주십시오.");
                            idInfoText.text = "연결상태를 확인하여주십시오.";
                            idInfoText.color = colorFail;
                            nickNameInfoText.text = "연결상태를 확인하여주십시오.";
                            nickNameInfoText.color = colorFail;
                            StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                        }
                        else
                        {
                            popUpManager.openNonButtonPopUp("최신버전을 이용하여 주십시오." + "\n현재:" + userManager.version + "/최신:" + strs[0]);
                            idInfoText.text = "최신버전을 이용하여 주십시오.";
                            idInfoText.color = colorFail;
                            nickNameInfoText.text = "최신버전을 이용하여 주십시오.";
                            nickNameInfoText.color = colorFail;
                            StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                        }
                    }
                }
                else
                {
                    popUpManager.openNonButtonPopUp(webRequest.error + "!");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
            }
        }
        else
        {
            idInfoText.color = colorFail;
            idInfoText.text = "번호를 입력하세요.";
        }
    }

    public void onClickSelectID(Text idText)
    {
        idInfoText.color = colorAccess;
        idInfoText.text = idText.text;
        usableID = idText.text;
        usablePhoneNumber = phoneInputField.text;
        isCheckedPhoneNumber = true;
        popUpManager.closeBoard(selectAccountPanel);
    }


    public void onClickCreateIMallAccountButton()
    {
        InAppBrowser.OpenURL("https://i-m-all.com/member/login.html");
    }

    public void onClickOkButton()
    {
        if (!isUsedAccount)
        {
            //isCheckedNickName = isCheckedPhoneNumber = true;
            if (!isCheckedNickName || !isCheckedPhoneNumber)
            {
                popUpManager.openOKButtonPopUp("모든 정보를 입력하여 주십시오.");
                popUpManager.setCallBackToOkButtonPopUp(popUpManager.onClickOkButtonForClosePopUp());
            }
            else
            {
                popUpManager.openNonButtonPopUp("회원 정보를 등록하는 중입니다.");
                StartCoroutine(SendCreateAccount());
            }
        }
        else
        {
            if (!isCheckedNickName || !isCheckedPhoneNumber)
            {
                popUpManager.openOKButtonPopUp("모든 정보를 입력하여 주십시오.");
                popUpManager.setCallBackToOkButtonPopUp(popUpManager.onClickOkButtonForClosePopUp());
            }
            else
            {
                popUpManager.openNonButtonPopUp("로그인하는 중 입니다.");
                popUpManager.setCallBackToNonButtonPopUp(popUpManager.disableNonButtonPopUpAndOpen(0.5f, popUpManager.checkAccountBoard));

                userManager.createAccount(usableNickName, usableID, usablePhoneNumber);
            }

        }
    }

    IEnumerator SendCreateAccount()
    {

        WWWForm form = new WWWForm();
        form.AddField("nickname", usableNickName);
        form.AddField("imallid", usableID);
        form.AddField("phone", usablePhoneNumber);
        string file = "insert.php";
        WWW webRequest = new WWW(UserManager.hostUrl + file, form);
        yield return webRequest;
        if (webRequest.isDone)
        {
            if (webRequest.error == null)
            {
                //Debug.Log(webRequest.text);
                if (webRequest.text.Equals("o"))
                {
                    popUpManager.setStringToNonButtonPopUp("등록완료!");
                    popUpManager.setCallBackToNonButtonPopUp(popUpManager.disableNonButtonPopUpAndOpen(1.0f, popUpManager.checkAccountBoard));

                    userManager.createAccount(usableNickName, usableID, usablePhoneNumber);
                }
                else
                {
                    popUpManager.closeNonButtonPopUp();
                    popUpManager.openYesNoButtonPopUp("이미 존재하는 번호 입니다.","처음으로","닫기");
                    popUpManager.setCallBackToYesNoButtonPopUp(popUpManager.callBack_DuplicationPopUp());
                }
            }
            else
            {
                popUpManager.setStringToNonButtonPopUp(webRequest.error + "!");
                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
            }
        }

    }

    public void onClickCanselButton()
    {
        //gameObject.SetActive(false);
        popUpManager.startInitionalize();
    }

    private void Update()
    {
        if (nickNameInputField.text != usableNickName)
        {
            if (!isCheckedNickName)
            {
                nickNameInfoText.color = colorFail;
                nickNameInfoText.text = "중복확인이 필요합니다.";
                usableNickName = nickNameInputField.text;
            }
            isCheckedNickName = false;
        }
        if (phoneInputField.text != usablePhoneNumber)
        {
            if (!isCheckedPhoneNumber)
            {
                idInfoText.color = colorFail;
                idInfoText.text = "휴대폰번호를 입력하여 주십시오.";
                usablePhoneNumber = phoneInputField.text;
            }
            isCheckedPhoneNumber = false;
        }
    }
}
