using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour {

    public AudioSource sound;
    public Animator anim;
    public UserManager userManager;
    bool isAnim = false;
    
    public void clickHelpPanel()
    {
        if (!isAnim)
        {
            isAnim = true;

            anim.SetTrigger("playBack");
            sound.volume = userManager.getEffect();
            sound.Play();
            Invoke("disablePanel", 1.0f);
        }
    }

    void disablePanel()
    {
        gameObject.SetActive(false);
        isAnim = false;
    }


}
