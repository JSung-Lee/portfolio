using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHighlight : MonoBehaviour
{
    public Entity owner;
    public float hoverWidth = 3f;
    public float hoverAlphaValue = 0.2f;

    List<(Material, float)> renderers = new List<(Material, float)>();
    // Start is called before the first frame update
    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renders)
        {
            foreach (var material in renderer.materials)
            {                
                float edgeWidth = 0f;

                renderers.Add((material, edgeWidth));
                //if (material.shader.FindPropertyIndex("_OutlineWidth") != -1)
                //{
                //    edgeWidth = material.GetFloat("_OutlineWidth");
                //}
                //if (material.shader.FindPropertyIndex("_HoverWidth") != -1)
                //{
                //    renderers.Add((material, edgeWidth));
                //}
            }
        }
        if (!owner) owner = GetComponent<Entity>();
    }

    public void ShowHoveHighlight()
    {
        //Debug.Log("Hover");
        foreach (var renderer in renderers)
        {
            renderer.Item1.SetFloat("_HoverWidth", renderer.Item2 + hoverWidth);
            renderer.Item1.SetFloat("_HoverAlphaValue", hoverAlphaValue);
        }
    }
    public void HideHoveHighlight()
    {
        
        foreach (var renderer in renderers)
        {
            renderer.Item1.SetFloat("_HoverWidth", 0);
            renderer.Item1.SetFloat("_HoverAlphaValue", 0);
        }
    }

    private void OnMouseEnter()
    {
        var setting = SettingManager.self;
        if (setting && !setting.interface_TargetOutline) return;
        //Debug.Log("Hover Enter : " + gameObject.name);
        if (owner && owner.health <= 0) return;
        ShowHoveHighlight();
    }

    private void OnMouseExit()
    {
        var setting = SettingManager.self;
        if (setting && !setting.interface_TargetOutline) return;
        //Debug.Log("Hover Exit : " + gameObject.name);
        HideHoveHighlight();
    }
}
