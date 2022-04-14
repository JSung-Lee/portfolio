using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DragAndDropable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    // drag options
    public PointerEventData.InputButton button = PointerEventData.InputButton.Left;
    public GameObject drageePrefab;
    GameObject currentlyDragged;

    // status
    public bool dragable = true;
    public bool dropable = true;

    public void OnBeginDrag(PointerEventData d)
    {
        if (dragable && d.button == button)
        {
            currentlyDragged = Instantiate(drageePrefab, transform.position, Quaternion.identity);
            currentlyDragged.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
            currentlyDragged.transform.SetParent(transform.root, true);
            currentlyDragged.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData d)
    {
        if (dragable && d.button == button)
            currentlyDragged.transform.position = d.position;
    }

    public void OnEndDrag(PointerEventData d)
    {
        Destroy(currentlyDragged);
    }

    public void OnDrop(PointerEventData d)
    {
        if (dropable && d.button == button)
        {
            var dropDragable = d.pointerDrag.GetComponent<UI_DragAndDropable>();
            if (dropDragable)
            {
                var player = Player.player;
                if (!player) return;
                int from = int.Parse(name); 
                int to = int.Parse(dropDragable.name);
                player.SwapInventoryItem(from, to);
                //스토어 인벤토리 업데이트
                UI_Store.self.itemInfo.ItemUpdate();
            }
        }
    }

    void OnDisable()
    {
        Destroy(currentlyDragged);
    }

    void OnDestroy()
    {
        Destroy(currentlyDragged);
    }
}
