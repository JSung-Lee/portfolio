using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonPopUpSetting : MonoBehaviour {
    public enum PopUpType { NonButton, OKButton, YesNoButton};
    public PopUpManager popUpManager;
    public Text context;
    public Text[] buttonText;
    public string str;
    public PopUpType type;
    public float waitTime  = 2.0f;
    private void OnEnable()
    {
        context.text = str;
    }

    public void setContext(string con)
    {
        str = con;
        context.text = str;

    }

    public void setContext(string con, string yesButton, string noButton)
    {
        str = con;
        context.text = str;
        buttonText[0].text = yesButton;
        buttonText[1].text = noButton;
    }


}
