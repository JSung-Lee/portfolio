using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : MonoBehaviour
{
    GameObject currentFX;
    //public AudioClip Gotit;
	void OnTriggerStay(Collider col) {
		if ((col.gameObject.GetComponent<Transform> ().position - gameObject.GetComponent<Transform> ().position).magnitude <= 0.6f) {
	        
	        if (col.gameObject.layer == LayerMask.NameToLayer("Player") && Controller.ShoesCheck == 0)
	        {
                FindItem.instance.GetShoes().SetActive(true);
				this.gameObject.SetActive (false);
                //AudioSource.PlayClipAtPoint(Gotit, transform.position);
				Controller.ShoesCheck = 1;
				Controller.anim.Play ("Get");
				Controller.anim.SetBool ("Move", false);
                Controller.isGet = true;
                Controller.MoveOn = false;
				currentFX = Instantiate(Controller.Loot_FX, this.transform.position, Quaternion.Euler(0, 0, 0));
	            Destroy(currentFX, 0.6f);
                Camera.main.GetComponent<FollowCam>().enabled = false;
                iTween.MoveTo(Camera.main.gameObject, col.gameObject.transform.position + Vector3.up * 4.8f + Vector3.back * 1.6f, 1.0f);
                Invoke ("canDestroy", 0.8f);
	       	}
    	}
	}

    void canDestroy()
    {
        iTween.MoveTo(Camera.main.gameObject, StageManager.Player_Current.transform.position + StageManager.visionPos, 1.0f);
        Invoke("objectDestroy", 0.5f);
    }

    void objectDestroy()
    {
        Controller.anim.Play("Move");
        Controller.anim.SetBool("Move", true);
        Controller.MoveOn = true;
        Camera.main.GetComponent<FollowCam>().enabled = true;
        Controller.isGet = false;
        Destroy(this.gameObject);
    }
}
