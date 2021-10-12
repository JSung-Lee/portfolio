using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
public class StartManager : MonoBehaviour {

    int state;
    public UserManager userManager;
    // Use this for initialization
    void OnEnable () {
        if (GameMng.self.sceneNumber == (int)GameMng.Scenes.Scene_Start)
        {
            transform.GetComponent<Text>().text = "tap to start";
        }
        else if (GameMng.self.sceneNumber == (int)GameMng.Scenes.Scene_Play)
        {
            transform.GetComponent<Text>().text = "tap to restart";
        }

    }

    public void clickStartButton() { 
        if (GameMng.self.sceneNumber == (int)GameMng.Scenes.Scene_Start)
        {
            GameMng.self.changeScene(GameMng.Scenes.Scene_Play);
            userManager.reStart();
        }
        else if (GameMng.self.sceneNumber == (int)GameMng.Scenes.Scene_Play)
        {
            GameMng.self.changeScene(GameMng.Scenes.Scene_Start);
        }
    }



}
