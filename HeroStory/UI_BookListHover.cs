using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BookListHover : MonoBehaviour
{
    public UI_BookListHoverHighlight currentHighlight;

    private void Start()
    {
        currentHighlight.SetHighlight();
    }

    public void StartListHighlight(UI_BookListHoverHighlight highlight)
    {
        currentHighlight.EndHighlight();
        currentHighlight = highlight;
        currentHighlight.StartHighlight();
    }

    public void EndListHighlight(UI_BookListHoverHighlight highlight)
    {
        //currentHighlight.EndHighlight();
    }

    private void OnEnable()
    {
        currentHighlight.SetHighlight();
    }
}
