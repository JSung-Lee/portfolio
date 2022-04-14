using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBook : MonoBehaviour
{
    public float speed = 1f;
    Animator animator;
    Vector3 startPosition;
    Vector3 startAngle;

    public Book turnOnUI;
    UI_Book ui_Book;
    public MeshRenderer leftPage;
    public MeshRenderer rightPage;
    public bool isAnimating = false;
    public bool isClosing = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        startAngle = transform.eulerAngles;
        CopyUI();
    }

    public void CopyUI()
    {
        if (turnOnUI)
        {
            ui_Book = turnOnUI.GetComponentInChildren<UI_Book>();
            if (ui_Book)
            {
                CopyPage(leftPage, ui_Book.LeftNextPage.transform);
                CopyPage(rightPage, ui_Book.RightNextPage.transform);
                turnOnUI.SetActiveBook(false);
            }
        }
    }

    public void CopyPage(MeshRenderer my, Transform copy)
    {
        if (copy.childCount == 0) return;
        Transform child = null;
        for (int i = copy.childCount - 1; i >= 0; i--) {
            if (copy.GetChild(i).gameObject.activeSelf) {
                child = copy.GetChild(i);
                break;
            }
        }
        if (!child) return;
        if (UIRenderTexture.self)
            my.material.mainTexture = UIRenderTexture.self.CopyTexture(child);
    }

    public void StartOpenBook(Vector3 center)
    {
        StopAllCoroutines();
        StartCoroutine(BookOpen(center));
    }

    public void StartCloseBook(Vector3 center)
    {
        StopAllCoroutines();
        StartCoroutine(BookClose(center));
    }

    IEnumerator BookOpen(Vector3 center)
    {
        if (turnOnUI) turnOnUI.PlaySound(true);
        isAnimating = true;
        isClosing = false;
        StartCoroutine(MoveTo(transform.position, new Vector3(startPosition.x, 0, center.z), 0.5f / speed));
        yield return new WaitForSeconds(0.5f / speed);
        StartCoroutine(MoveTo(transform.position, new Vector3(0, 0, center.z), 0.5f / speed));
        StartCoroutine(RotateTo(transform.eulerAngles, new Vector3(0, 180, 0), 0.5f / speed));
        yield return new WaitForSeconds(0.5f / speed);
        animator.SetFloat("Speed", speed);
        animator.SetBool("Open", true);
        StartCoroutine(MoveTo(transform.position, new Vector3(center.x, 0, center.z), 0.5f / speed));
        yield return new WaitForSeconds(0.5f / speed);
        if (turnOnUI) turnOnUI.SetActiveBookWittoutSound(true);
        yield return null;
    }

    IEnumerator BookClose(Vector3 center)
    {
        isClosing = true;
        if (turnOnUI) turnOnUI.SetActiveBook(false);
        animator.SetFloat("Speed", speed);
        animator.SetBool("Open", false);
        StartCoroutine(MoveTo(transform.position, new Vector3(0, 0, center.z), 0.5f / speed));
        yield return new WaitForSeconds(0.5f / speed);
        StartCoroutine(RotateTo(transform.eulerAngles, new Vector3(0, startAngle.y, 0), 0.5f / speed));
        StartCoroutine(MoveTo(transform.position, new Vector3(startPosition.x, 0, center.z), 0.5f / speed));
        yield return new WaitForSeconds(0.5f / speed);
        StartCoroutine(MoveTo(transform.position, new Vector3(startPosition.x, 0, startPosition.z), 0.5f / speed));
        yield return new WaitForSeconds(0.5f / speed);
        isAnimating = false;
        yield return null;
    }

    IEnumerator MoveTo(Vector3 start, Vector3 end, float timer)
    {
        float time = 0;
        while (time <= timer)
        {
            transform.position = Vector3.Lerp(start, end, time / timer);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = Vector3.Lerp(start, end, 1f);
    }

    IEnumerator RotateTo(Vector3 start, Vector3 end, float timer)
    {
        float time = 0;
        while (time <= timer)
        {
            transform.eulerAngles = Vector3.Lerp(start, end, time / timer);
            time += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = Vector3.Lerp(start, end, 1f);
    }
}
