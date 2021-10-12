using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    //float timer;
    private Transform tr;
    public float speed;
    public bool setSpeedFromTimeScale;
    //private float timeScale;            //게임속도
    void OnEnable() {
        //timer = 0;
        tr = gameObject.transform;
        if (setSpeedFromTimeScale)
        {
            if (speed < 0)
                speed = -1;
            else
                speed = 1;
        }
	}
	
	// Update is called once per frame
	void Update() {
        if (setSpeedFromTimeScale)
            tr.Rotate(new Vector3(0, 0, speed * UserManager.timeScale * Time.deltaTime));
        else
            tr.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
	}
}
