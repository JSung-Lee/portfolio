using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRenderTexture : MonoBehaviour
{
    public static UIRenderTexture self;

    public Canvas canvas;
    public Camera cam;
    public Vector2 textureSize;
    private void Start()
    {
        if (self) Destroy(this);
        else
            self = this;
    }

    public Texture CopyTexture(Transform copy)
    {
        GameObject target = Instantiate(copy.gameObject, transform);
        if (target == null) return null;
        cam.gameObject.SetActive(true);
        RectTransform rect = target.GetComponent<RectTransform>();
        RenderTexture renderTexture = new RenderTexture((int)textureSize.x, (int)textureSize.y, (int)cam.depth);
        cam.targetTexture = renderTexture;
        rect.sizeDelta = textureSize;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector3.zero;
        cam.Render();
        renderTexture = cam.targetTexture;
        cam.gameObject.SetActive(false);
        cam.targetTexture = null;
        Destroy(target);
        return renderTexture;
    }
}
