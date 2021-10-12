using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionColor : MonoBehaviour {
    public PickupExplosion white;
    public PickupExplosion red;
    public PickupExplosion purple;
    public PickupExplosion green;
    public PickupExplosion blue;
    public void setExplosionScale(int color, float scale)
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * scale;
    }

    public void setAudioClip(int color, AudioClip clip)
    {
        switch (color)
        {
            case (int)OrbManager.orbColor.white:
                white.sound.clip = clip;
                break;
            case (int)OrbManager.orbColor.red:
                red.sound.clip = clip;
                break;
            case (int)OrbManager.orbColor.purple:
                purple.sound.clip = clip;
                break;
            case (int)OrbManager.orbColor.green:
                green.sound.clip = clip;
                break;
            case (int)OrbManager.orbColor.blue:
                blue.sound.clip = clip;
                break;
        }
    }

    public void setExplosionColor(int color)
    {
        white.gameObject.SetActive(false);
        red.gameObject.SetActive(false);
        purple.gameObject.SetActive(false);
        green.gameObject.SetActive(false);
        blue.gameObject.SetActive(false);
        switch (color)
        {
            case (int)OrbManager.orbColor.white:
                white.gameObject.SetActive(true);
                break;
            case (int)OrbManager.orbColor.red:
                red.gameObject.SetActive(true);
                break;
            case (int)OrbManager.orbColor.purple:
                purple.gameObject.SetActive(true);
                break;
            case (int)OrbManager.orbColor.green:
                green.gameObject.SetActive(true);
                break;
            case (int)OrbManager.orbColor.blue:
                blue.gameObject.SetActive(true);
                break;
        }
    }

    public void setExplosionPosition(Vector3 pos) { transform.position = pos; }
}
