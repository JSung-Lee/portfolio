using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BookListHoverHighlight : MonoBehaviour
{
    public Text[] texts;
    public float animTime = 1;

    public int progress = 0;
    public int maxProgress = 20;

    Coroutine current;

    private void Start()
    {
        ResetHighlight();
    }

    public void StartHighlight()
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(ShowHighlight());
    }

    public void EndHighlight()
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(HideHighlight());
    }

    public void SetHighlight()
    {
        progress = maxProgress;
        string str = "";
        for (int i = 0; i < maxProgress; i++)
            str += "=";
        foreach (var text in texts)
            text.text = str;
    }

    public void SetHighlight(int _progress)
    {
        progress = _progress;
        string str = "";
        for (int i = 0; i < maxProgress; i++)
            str += "=";
        foreach (var text in texts)
            text.text = str;
    }

    public void ResetHighlight()
    {
        foreach (var text in texts)
            text.text = "";
    }

    IEnumerator ShowHighlight()
    {
        float waitTime = animTime / maxProgress;
        while (progress < maxProgress)
        {
            foreach (var text in texts)
                text.text += "==";
            progress+=2;
            yield return new WaitForSeconds(waitTime);
        }
        yield return null;
    }

    IEnumerator HideHighlight()
    {
        if(texts.Length > 0)
        {
            if (progress != texts[0].text.Length)
                SetHighlight(progress);
        }
        float waitTime = animTime / maxProgress;
        while (progress > 0)
        {
            progress-=2;
            foreach (var text in texts)
                text.text = text.text.Remove(progress);
            yield return new WaitForSeconds(waitTime);
        }
        yield return null;
    }

    private void OnDisable()
    {
        if (current != null) StopCoroutine(current);
        ResetHighlight();
    }
}
