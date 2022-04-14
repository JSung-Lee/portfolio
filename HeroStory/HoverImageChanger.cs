using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverImageChanger : MonoBehaviour
{
    bool isInit = false;
    public Image image;
    public Sprite sprite;

    Sprite origin;
    Vector3 originScale;
    void Start()
    {
        isInit = true;
        if (image) origin = image.sprite;
        originScale = transform.localScale;
    }
    public void ShowHoveHighlight()
    {
        image.sprite = sprite;
        transform.localScale = originScale * 1.1f;
    }
    public void HideHoveHighlight()
    {
        image.sprite = origin;
        transform.localScale = originScale;
    }

    private void OnDisable()
    {
        HideHoveHighlight();
    }

    private void OnEnable()
    {
        if (!isInit) Start();
        ShowHoveHighlight();
    }
}
