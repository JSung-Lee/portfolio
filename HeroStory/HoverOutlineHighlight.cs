using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOutlineHighlight : MonoBehaviour
{
    public Outline[] outlines;
    void Start()
    {
        outlines = GetComponents<Outline>();
        HideHighlight();
    }

    public void ShowHighlight()
    {
        foreach (var outline in outlines)
            outline.enabled = true;
    }

    public void HideHighlight()
    {
        foreach (var outline in outlines)
            outline.enabled = false;
    }
}

