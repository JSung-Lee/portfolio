using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ranking_UserData : MonoBehaviour {

    public string rank;
    public string nickName;
    public string score;
    public Text nameText;
    public Text scoreText;
    public bool isMe = false;
	// Use this for initialization
	void OnEnable() {
        //UpdateText();
    }

    public void UpdateText()
    {
        if (rank.Equals("0"))
        {
            nameText.text = "";
            scoreText.text = "";
            return;
        }
        if (nickName.Equals(""))
            return;
        nameText.text = rank.ToString() + "." + nickName;
        nameText.text = nameText.text;
        scoreText.text = score.ToString();
        if (isMe)
        {
            Color yellow = new Color(1.0f, 1.0f, 0.0f);
            nameText.color = yellow;
            scoreText.color = yellow;
        }
        else
        {
            Color white = new Color(1.0f, 1.0f, 1.0f);
            nameText.color = white;
            scoreText.color = white;
        }
    }

    public void UpdateText(string r, string n, string s)
    {
        rank = r;
        nickName = n;
        score = s;
        isMe = false;
        UpdateText();
    }

    public void UpdateText(string r, string n, string s, bool me)
    {
        rank = r;
        nickName = n;
        score = s;
        isMe = me;
        UpdateText();
    }

}
