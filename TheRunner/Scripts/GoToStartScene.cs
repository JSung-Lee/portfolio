using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToStartScene : MonoBehaviour {

    public LoadingSceneManager loadingSceneManager;
    float timer = 0;
	// Use this for initialization
	void Update () {
        timer += Time.deltaTime;
        if (timer >= 1.0f)
        {
            loadingSceneManager.LoadScene("Scene_Start");
            timer = -9999;
        }
	}
}
