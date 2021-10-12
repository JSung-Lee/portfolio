using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectAccountText : MonoBehaviour {

    public Text titleText, idText;
    public void setOnAccount(string id)
    {
        gameObject.SetActive(true);
        idText.text = id;
        if (id.Substring(id.Length - 2).Equals("@n"))
            titleText.text = "네이버 아이디로 가입";
        else if (id.Substring(id.Length - 2).Equals("@f"))
            titleText.text = "페이스북 아이디로 가입";
        else if (id.Substring(id.Length - 2).Equals("@k"))
            titleText.text = "카카오톡 아이디로 가입";
        else
            titleText.text = "아이몰 페이지에서 가입";
    }
    public void setOnAccount(string title, string id)
    {
        gameObject.SetActive(true);
        titleText.text = title;
        idText.text = id;
    }
}
