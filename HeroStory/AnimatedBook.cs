using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBook : MonoBehaviour
{

    public Transform center;
    public SelectBook currentBook;

    void Update()
    {
        if (currentBook && !currentBook.isAnimating) {
            currentBook = null;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && currentBook && !currentBook.isClosing)
        {
            if (UI_Option_Button.self.currentActiveButton) return;
            currentBook.CopyUI();
            currentBook.StartCloseBook(center.position);
            return;
        }    
        if (!Input.GetMouseButtonDown(0) || currentBook) return;
        int layerMask = 1 << LayerMask.NameToLayer("Book");
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask))
        {
            var book = hit.transform.GetComponent<SelectBook>();
            if (book && !book.isAnimating)
            {
                currentBook = book;
                currentBook.CopyUI();
                book.StartOpenBook(center.position);
            }
        }
    }
}
