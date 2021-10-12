using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopUpManager : MonoBehaviour {

    public GameObject[] popUpParent;
    public Stack<GameObject> PopUpStack = new Stack<GameObject>();
    public GameObject backBoard;
    public GameObject yesNoPopUp;
    public GameObject nonButtonPopUp;
    public GameObject okButtonPopUp;
    public GameObject newAccountBoard;
    public GameObject searchAccountBoard;
    public GameObject checkAccountBoard;
    public GameObject loginInfoBoard;
    public GameObject startBoard;
    public int okButtonPushButtonCheck;
    public int yesNoButtonPushButtonCheck;

    private void OnEnable()
    {
        PopUpStack.Clear();
        PopUpStack.Push(backBoard);
    }
    public void OpenBoard(GameObject board)
    {
        if (board.activeSelf)
            closeBoard(board);
        if (!backBoard.activeSelf)
            backBoard.SetActive(true);
        board.SetActive(true);
        backBoard.transform.SetParent(popUpParent[PopUpStack.Count].transform); //보드 위 배경을 검게하는 빽보드 먼저 삽입
        board.transform.SetParent(popUpParent[PopUpStack.Count].transform);   //보드 삽입
        PopUpStack.Push(board); //스택에 쌓기
    }
    public void closeBoard(GameObject board)
    {
        if(PopUpStack.Contains(board)) //존재 유무
            PopUpStack.Pop();   //팝업에서 최상위 보드 삭제
        if (PopUpStack.Count <= 1)  //팝업이 존재하지 않으면 뺵보드 끄기
        {
            backBoard.transform.SetParent(popUpParent[0].transform);
            backBoard.SetActive(false);
        }
        else
            backBoard.transform.SetParent(popUpParent[PopUpStack.Count - 2].transform);
        board.transform.SetParent(gameObject.transform);  //삭제한 보드를 최상위로 변경
        board.SetActive(false); //보드 끄기
    }
    public void closeTopBoard()
    {
        if(PopUpStack.Count > 0)
            closeBoard(PopUpStack.Peek());
    }
    public void onClickYesButton()
    {
        yesNoButtonPushButtonCheck = 1;
    }
    public void onClickNoButton()
    {
        yesNoButtonPushButtonCheck = 2;
    }

    //YesNoButton PopUp
    public void openYesNoButtonPopUp()
    {
        OpenBoard(yesNoPopUp);
    }
    public void openYesNoButtonPopUp(string str)
    {
        OpenBoard(yesNoPopUp);
        setStringToYesNoButtonPopUp(str);
    }
    public void openYesNoButtonPopUp(string str, string button1, string button2)
    {
        OpenBoard(yesNoPopUp);
        setStringToYesNoButtonPopUp(str, button1, button2);
    }
    public void closeYesNoButtonPopUp()
    {
        closeBoard(yesNoPopUp);
    }
    public void setStringToYesNoButtonPopUp(string str)
    {
        yesNoPopUp.GetComponent<ButtonPopUpSetting>().setContext(str);
    }
    public void setStringToYesNoButtonPopUp(string str, string button1, string button2)
    {
        yesNoPopUp.GetComponent<ButtonPopUpSetting>().setContext(str, button1, button2);
    }
    public void setCallBackToYesNoButtonPopUp(IEnumerator enumerator)
    {
        yesNoButtonPushButtonCheck = 0;    //0-기본, 1-yes, 2-no
        
        StartCoroutine(enumerator);
    }
    public IEnumerator callBack_FirstPlay()
    {
        while(yesNoButtonPushButtonCheck == 0)
            yield return yesNoButtonPushButtonCheck;
        if(yesNoButtonPushButtonCheck == 1)    //yes
        {
            //Debug.Log("click Yes Button");
            closeYesNoButtonPopUp();
            OpenBoard(newAccountBoard);
        }
        else //no
        {
            //Debug.Log("click No Button");
            //closeYesNoButtonPopUp();
            //penBoard(searchAccountBoard);
            //Application.OpenURL("https://i-m-all.com/member/login.html");
            InAppBrowser.OpenURL("https://i-m-all.com/member/login.html");
            startInitionalize();
        }
    }
    public IEnumerator callBack_DuplicationPopUp()
    {
        while (yesNoButtonPushButtonCheck == 0)
            yield return yesNoButtonPushButtonCheck;
        if (yesNoButtonPushButtonCheck == 1)    //yes
        {
            //Debug.Log("click Yes Button");
            startInitionalize();
        }
        else //no
        {
            //Debug.Log("click No Button");
            closeYesNoButtonPopUp();
        }
    }
    public IEnumerator callBack_ResetScore(IEnumerator reset)
    {
        while (yesNoButtonPushButtonCheck == 0)
            yield return yesNoButtonPushButtonCheck;
        if (yesNoButtonPushButtonCheck == 1)    //yes
        {
            //Debug.Log("click Yes Button");
            closeYesNoButtonPopUp();
            StartCoroutine(reset);
        }
        else //no
        {
            //Debug.Log("click No Button");
            closeYesNoButtonPopUp();
        }
    }
    public IEnumerator callBack_GoToCreateAccount()
    {
        while (yesNoButtonPushButtonCheck == 0)
            yield return yesNoButtonPushButtonCheck;
        if (yesNoButtonPushButtonCheck == 1)    //yes
        {
            //Debug.Log("click Yes Button");
            closeYesNoButtonPopUp();
            InAppBrowser.OpenURL("https://i-m-all.com/member/login.html");
        }
        else //no
        {
            //Debug.Log("click No Button");
            closeYesNoButtonPopUp();
        }
    }
    public IEnumerator callBack_GoToLogIn(UserManager user, string[] userInfo, string phone)
    {
        while (yesNoButtonPushButtonCheck == 0)
            yield return yesNoButtonPushButtonCheck;
        if (yesNoButtonPushButtonCheck == 1)    //yes
        {
            //Debug.Log("click Yes Button");
            user.nickname = userInfo[1];
            user.imallid = userInfo[2];
            user.setBestScore(System.Convert.ToInt32(userInfo[3]));
            user.phoneNumber = phone;
            startLogIn();
        }
        else //no
        {
            //Debug.Log("click No Button");
            closeYesNoButtonPopUp();
            
        }
    }

    //NonButton PopUp
    public void openNonButtonPopUp()
    {
        OpenBoard(nonButtonPopUp);
    }
    public void openNonButtonPopUp(string str)
    {
        if(!nonButtonPopUp.activeSelf)
            OpenBoard(nonButtonPopUp);
        setStringToNonButtonPopUp(str);
    }
    public void closeNonButtonPopUp()
    {
        closeBoard(nonButtonPopUp);
    }
    public void setStringToNonButtonPopUp(string str)
    {
        nonButtonPopUp.GetComponent<ButtonPopUpSetting>().setContext(str);
    }
    public void setCallBackToNonButtonPopUp(IEnumerator enumerator)
    {
        okButtonPushButtonCheck = 0;    //0-기본, 1-yes, 2-no
        StartCoroutine(enumerator);
    }
    public IEnumerator disableNonButtonPopUp(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        closeNonButtonPopUp();
    }
    public IEnumerator disableNonButtonPopUpAndOpen(float w, GameObject nextStep)
    {
        yield return new WaitForSeconds(w);
        closeNonButtonPopUp();
        closeTopBoard();
        OpenBoard(nextStep);
    }
    public IEnumerator disableNonButtonPopUpAndOpen(float w, IEnumerator nextStep)
    {
        yield return new WaitForSeconds(w);
        closeNonButtonPopUp();
        closeTopBoard();
        StartCoroutine(nextStep);
    }
    public IEnumerator disableNonButtonPopUpAndOpen(float w, string str, ButtonPopUpSetting.PopUpType type)
    {
        yield return new WaitForSeconds(w);
        closeNonButtonPopUp();
        closeTopBoard();
        if(type == ButtonPopUpSetting.PopUpType.NonButton)
            openNonButtonPopUp(str);
        if (type == ButtonPopUpSetting.PopUpType.OKButton)
            openOKButtonPopUp(str);
        if (type == ButtonPopUpSetting.PopUpType.YesNoButton)
            openYesNoButtonPopUp(str);
    }

    //OKButton PopUp
    public void openOKButtonPopUp()
    {
        OpenBoard(okButtonPopUp);
    }
    public void openOKButtonPopUp(string str)
    {
        OpenBoard(okButtonPopUp);
        setStringToOKButtonPopUp(str);
    }
    public void closeOKButtonPopUp()
    {
        closeBoard(okButtonPopUp);
    }
    public void onClickOkButton()
    {
        okButtonPushButtonCheck = 1;
    }
    public void setStringToOKButtonPopUp(string str)
    {
        okButtonPopUp.GetComponent<ButtonPopUpSetting>().setContext(str);
    }
    public void setCallBackToOkButtonPopUp(IEnumerator enumerator)
    {
        okButtonPushButtonCheck = 0;    //0-기본, 1-yes, 2-no
        StartCoroutine(enumerator);
    }
    public IEnumerator onClickOkButtonForClosePopUp()
    {
        while (okButtonPushButtonCheck == 0)
            yield return okButtonPushButtonCheck;
        if (okButtonPushButtonCheck == 1)    //yes
        {
            //Debug.Log("click OK Button");
            closeOKButtonPopUp();
        }
    }

    //etc..
    public void disableBoardAndEnableBoard(GameObject disable, GameObject enable)
    {
        closeBoard(disable);
        OpenBoard(enable);
    }
    public void startInitionalize()
    {
        StartCoroutine(initionalize());
    }

    public IEnumerator initionalize()
    {
        while (PopUpStack.Count > 0)
            closeBoard(PopUpStack.Peek());
        PopUpStack.Clear();
        PopUpStack.Push(backBoard);
        openYesNoButtonPopUp("아이몰 아이디가 있습니까?", "네\n있습니다", "아니오\n없습니다");
        setCallBackToYesNoButtonPopUp(callBack_FirstPlay());
        yield return null;
    }

    public void startLogIn()
    {
        StartCoroutine(LogIn());
    }

    public IEnumerator LogIn()
    {
        while (PopUpStack.Count > 0)
            closeBoard(PopUpStack.Peek());
        PopUpStack.Clear();
        PopUpStack.Push(backBoard);
        OpenBoard(checkAccountBoard);      
        yield return null;
    }
}
