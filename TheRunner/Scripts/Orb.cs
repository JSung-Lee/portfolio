using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {

    public GameObject white;
    public GameObject red;
    public GameObject purple;
    public GameObject green;
    public GameObject blue;
    public GameObject pig;

    private int color;

    public int getOrbColor() { return color; }
    public void setOrbColor(int c)
    {
        color = c;
        white.SetActive(false);
        red.SetActive(false);
        purple.SetActive(false);
        green.SetActive(false);
        blue.SetActive(false);
        pig.SetActive(false);
        switch(color)
        {
            case (int)OrbManager.orbColor.white:
                white.SetActive(true);
                break;
            case (int)OrbManager.orbColor.red:
                red.SetActive(true);
                break;
            case (int)OrbManager.orbColor.purple:
                purple.SetActive(true);
                break;
            case (int)OrbManager.orbColor.green:
                green.SetActive(true);
                break;
            case (int)OrbManager.orbColor.blue:
                blue.SetActive(true);
                break;
            case (int)OrbManager.orbColor.pig:
                pig.SetActive(true);
                break;
        }
    }
    public void setOrbPosition(Vector3 pos) { transform.position = pos; }


}
