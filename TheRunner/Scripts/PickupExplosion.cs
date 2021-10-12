using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupExplosion : MonoBehaviour {
    float timer;
    public AudioSource sound;
    public UserManager user;
	// Use this for initialization
	void OnEnable() {
        sound.volume = user.getEffect();
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update() {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            transform.parent.gameObject.SetActive(false);
            timer = 0.0f;
        }
	}
}
