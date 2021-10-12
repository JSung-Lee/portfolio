using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour {
    public static GameMng self;
    public enum Scenes { Scene_Loading, Scene_Start, Scene_Play}
    public int sceneNumber = 0;
    public GameObject[] startSceneObject;
    public GameObject[] playSceneObject;
    // Use this for initialization
    void OnEnable () {
        self = this;
        changeScene(Scenes.Scene_Start);

    }
	
    public void changeScene(Scenes scenes)
    {
        //ebug.Log(scenes.ToString());
        sceneNumber = (int)scenes;
        if (sceneNumber == (int)Scenes.Scene_Start)
        {
            for (int i = 0; i < playSceneObject.Length; i++)
            {
                playSceneObject[i].SetActive(false);
            }
            for (int i = 0; i < startSceneObject.Length; i++)
            {
                startSceneObject[i].SetActive(true);
            }
        }
        else if(sceneNumber == (int)Scenes.Scene_Play)
        {
            for (int i = 0; i < playSceneObject.Length; i++)
            {
                playSceneObject[i].SetActive(true);
            }
            for (int i = 0; i < startSceneObject.Length; i++)
            {
                startSceneObject[i].SetActive(false);
            }
        }
    }
}
