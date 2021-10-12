using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreBoard : MonoBehaviour {

    public Text bestScore;
    public Text bestDistance;
    public Text bestCombo;
    public Text bestScore2;
    public UserManager userManager;
    public RectTransform tr;
    public RectTransform parentTr;
    int size = 3;   //text갯수
    int index = 0;
    float timer = 0.0f;
    float interval = 2.0f;
    float speed = 1.0f;
    float textBoxSize;
	// Use this for initialization
	void OnEnable () {
        parentTr.anchorMin = new Vector2(0, 0);
        parentTr.anchorMax = new Vector2(1, 1);
        textBoxSize = 100.0f * UIScaler.screenRatio;
        bestScore.text = "BEST SCORE " + ((int)userManager.getBestScore()).ToString();
        bestDistance.text = "BEST DISTANCE " + ((int)userManager.getBestDistance()).ToString();
        bestCombo.text = "BEST COMBO " + userManager.getBestCombo();
        bestScore2.text = "BEST SCORE " + ((int)userManager.getBestScore()).ToString();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= interval)
        {
            if (timer >= interval + speed)
            {
                timer = 0.0f;
                index++;
                if (index >= size)
                {
                    index = 0;
                    tr.position = new Vector3(tr.position.x, 0.0f, tr.position.z);
                }
                tr.position = new Vector3(tr.position.x, Mathf.Round(tr.position.y), tr.position.z);
                
            }
            else
            {
                tr.position += new Vector3(0.0f, textBoxSize * (speed * Time.deltaTime), 0.0f);
            }
        }
	}
}
