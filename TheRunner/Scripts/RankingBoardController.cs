using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingBoardController : MonoBehaviour {
    
    
    public void onClickNextPage(RectTransform target)
    {
        transform.position = target.position;
    }

    public void onClickBackPage(RectTransform target)
    {
        transform.position = target.position;
    }

    public void onClickDisablePage(GameObject page)
    {
        page.SetActive(false);
    }
}
