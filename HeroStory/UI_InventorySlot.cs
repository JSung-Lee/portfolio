using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour {
    public UI_ShowToolTip tooltip;
    public Button button;
    public UI_DragAndDropable dragAndDropable;
    public Image backImage;
    public Image image;
    public UI_CircularProgressBar cooldownProgressBar;
    public GameObject amountOverlay;
    public Text amountText;
    public Text hotKeyText;
}
