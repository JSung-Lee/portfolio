using UnityEngine;
using UnityEngine.UI;

public class InsertStage : MonoBehaviour {

    public string stageNum;
    private int inStageText;

    // Use this for initialization
    void Start () {
        stageNum = gameObject.transform.Find("LevelNumberText").GetComponent<Text>().text;
    }

    public void OnClick()
    {
        //Debug.Log(stageNum);

        if (int.TryParse(stageNum, out inStageText))
        {
            StageManager.stage = inStageText;
        }
    }

}
