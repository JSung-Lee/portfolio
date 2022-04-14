using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CircularProgressBar : MonoBehaviour
{
    public Image image;
    public float value;
    public bool clockwise = true;

    void Start()
    {
        if (!image) image = GetComponent<Image>();
        if (!image) return;
        image.fillAmount = value;
        image.fillClockwise = clockwise;
    }

    void Update()
    {
        if (!image) return;
        image.fillAmount = value;
    }
}
