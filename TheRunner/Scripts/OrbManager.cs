using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour {
    public enum orbColor { white, red, green, blue, purple, pig};
    private int color;
    public GameObject[] explosion;
    public GameObject[] orbs;
    public AudioClip[] FXSound;
    public AudioClip FXSound_Purple;
    public UserManager userManager;
    private void OnEnable()
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            if (orbs[i].activeSelf)
            {
                orbs[i].SetActive(false);
            }
        }
    }

    public void createOrb(Vector3 pos, bool isEventOrb = false)
    {
        if (!isEventOrb)
            color = Random.Range(0, 5);
        else
            color = (int)orbColor.pig;
        for(int i = 0;i<orbs.Length;i++)
        {
            if(!orbs[i].activeSelf)
            {
                orbs[i].SetActive(true);
                orbs[i].GetComponent<Orb>().setOrbColor(color);
                orbs[i].GetComponent<Orb>().setOrbPosition(pos);
                return;
            }
        }

    }

    public void createFX(Vector3 pos, int c)
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            if (!explosion[i].activeSelf)
            {
                
                ExplosionColor ec = explosion[i].GetComponent<ExplosionColor>();
                if (c != (int)orbColor.purple)
                {
                    int clipNum = userManager.getCombo();
                    if (clipNum >= FXSound.Length)
                        clipNum = FXSound.Length - 1;
                    ec.setAudioClip(c, FXSound[clipNum]);
                }
                else
                    ec.setAudioClip(c, FXSound_Purple);
                explosion[i].SetActive(true);
                if (c == (int)orbColor.pig)
                    ec.setExplosionColor((int)orbColor.white);
                else
                    ec.setExplosionColor(c);
                ec.setExplosionPosition(pos);
                return;
            }
        }
    }
}
