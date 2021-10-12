using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour {

    public Animator anim;
    public Outline outline;

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            gameObject.SetActive(false);
    }

    public void setEffectColor(Color c)
    {
        outline.effectColor = c;
    }
}
