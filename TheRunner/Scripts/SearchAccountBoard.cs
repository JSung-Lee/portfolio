using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
public class SearchAccountBoard : MonoBehaviour {
    [Header("Board")]
    public InputField phoneInputField;
    public Text idInfoText;
    public Text nickNameInfoText;

    [Header("Managers")]
    public PopUpManager popUpManager;
    public UserManager userManager;

    bool isCheckedPhoneNumber = false;
    string usableNickName = "";
    string usableID = "";
    string usablePhoneNumber = "";
    Color colorBasic = new Color(1.0f, 1.0f, 1.0f, 0.785f);
    Color colorAccess = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    Color colorFail = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    private void OnEnable()
    {
        phoneInputField.text = "";
        usablePhoneNumber = phoneInputField.text;
        idInfoText.color = colorBasic;
        idInfoText.text = "휴대폰번호를 입력하여 주십시오.";
        nickNameInfoText.text = "휴대폰번호를 입력하여 주십시오.";
        nickNameInfoText.color = colorBasic;
    }

    public void onClickIMallIDCheckButton()
    {
        idInfoText.color = colorBasic;
        idInfoText.text = "가입 여부를 확인하는 중입니다.";
        nickNameInfoText.text = "가입 여부를 확인하는 중입니다.";
        nickNameInfoText.color = colorBasic;
        StartCoroutine(IDCheckPhoneNumber());
    }

    IEnumerator IDCheckPhoneNumber()
    {
        if (!phoneInputField.text.Equals(""))
        {
            phoneInputField.text = phoneInputField.text.Replace("-", "");
            phoneInputField.text = phoneInputField.text.Insert(3, "-");
            phoneInputField.text = phoneInputField.text.Insert(phoneInputField.text.Length - 4, "-");
        }
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
                if (strs[1].Equals("b"))
                {
                    popUpManager.openNonButtonPopUp("정지된 계정입니다.");
                    StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                }
                else if (userManager.checkVersion(strs[0]))
                { 
                    form = new WWWForm();
                    form.AddField("phone", phoneInputField.text);
                    file = "search.php";
                    WWW www = new WWW(UserManager.hostUrl + file, form);
                    yield return www;
                    if (www.isDone)
                    {
                        if (www.error == null)
                        {
                            //Debug.Log("access");
                            if (!www.text.Equals("x"))
                            {
                                string[] str = www.text.Split("\n".ToCharArray());
                                usableNickName = str[0];
                                usableID = str[1];
                                usablePhoneNumber = phoneInputField.text;
                                idInfoText.text = usableID;
                                idInfoText.color = colorAccess;
                                nickNameInfoText.text = usableNickName;
                                nickNameInfoText.color = colorAccess;
                                isCheckedPhoneNumber = true;
                            }
                            else
                            {
                                popUpManager.openNonButtonPopUp("등록된 회원정보가 없습니다.");
                                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                            }
                        }
                        else
                        {
                            popUpManager.setStringToNonButtonPopUp(www.error + "!");
                            StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
                        }
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
                Debug.Log(webRequest.error);
                popUpManager.openNonButtonPopUp(webRequest.error);
                StartCoroutine(popUpManager.disableNonButtonPopUp(1.0f));
            }
        }
        
    }

    public void onClickOkButton()
    {
        if (!isCheckedPhoneNumber)
        {
            popUpManager.openOKButtonPopUp("휴대폰 번호를 입력하여 주십시오.");
            popUpManager.setCallBackToOkButtonPopUp(popUpManager.onClickOkButtonForClosePopUp());
        }
        else
        {
            popUpManager.openNonButtonPopUp("로그인 하는 중입니다.");
            StartCoroutine(SendCreateAccount());
        }
    }

    
    IEnumerator SendCreateAccount()
    {
        popUpManager.setStringToNonButtonPopUp("로그인 완료!");
        popUpManager.setCallBackToNonButtonPopUp(popUpManager.disableNonButtonPopUpAndOpen(0.5f, popUpManager.checkAccountBoard));

        userManager.createAccount(usableNickName, usableID, usablePhoneNumber);
        yield return null;
        

    }

    private void Update()
    {
        if (phoneInputField.text != usablePhoneNumber)
        {
            if (!isCheckedPhoneNumber)
            {
                idInfoText.color = colorFail;
                idInfoText.text = "휴대폰번호를 입력하여 주십시오.";
                usablePhoneNumber = phoneInputField.text;
                nickNameInfoText.text = "휴대폰번호를 입력하여 주십시오.";
                nickNameInfoText.color = colorFail;
            }
            isCheckedPhoneNumber = false;
        }
    }
}
