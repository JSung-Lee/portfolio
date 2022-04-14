using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor self;

    bool isCursorTarget = false;
    public Texture2D cursorBase_Texture;
    public Texture2D cursorAttack_Texture;
    public Texture2D cursorTarget_Texture;
    public Texture2D cursorTargetTeam_Texture;
    public Texture2D cursorTargetEnemy_Texture;

    public Vector2 hotspot = Vector2.zero;

    [Header("Indicator")]
    public GameObject indicatorPrefab;
    // Start is called before the first frame update

    public float mousesensitivity = 1f;
    void Start()
    {
        if (self)
            Destroy(this);
        self = this;
        hotspot = Vector2.zero;
        Cursor.SetCursor(cursorBase_Texture, hotspot, CursorMode.Auto);
        isCursorTarget = false;
    }

    private void Update()
    {
        //var mou


        if (!isCursorTarget) return;
        int layerMask = 1 << LayerMask.NameToLayer("Raycast");
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            var entity = hit.transform.GetComponent<Entity>();
            if (entity)
            {
                if (entity.team == Team.Player)
                    SetCursorTargetTeam();
                else
                    SetCursorTargetEnemy();
                return;
            }
        }
        SetCursorTarget();
    }

    public void SetCursorBase()
    {
        hotspot = Vector2.zero;        
        Cursor.SetCursor(cursorBase_Texture, hotspot, CursorMode.Auto);
        isCursorTarget = false;
    }
    public void SetCursorAttack()
    {
        hotspot = Vector2.zero;
        Cursor.SetCursor(cursorAttack_Texture, hotspot, CursorMode.Auto);
        isCursorTarget = false;
    }

    public void SetCursorTarget()
    {
        hotspot = new Vector2(cursorTarget_Texture.width / 2, cursorTarget_Texture.height / 2);
        Cursor.SetCursor(cursorTarget_Texture, hotspot, CursorMode.Auto);
        isCursorTarget = true;
    }
    public void SetCursorTargetTeam()
    {
        hotspot = new Vector2(cursorTarget_Texture.width / 2, cursorTarget_Texture.height / 2);        
        Cursor.SetCursor(cursorTargetTeam_Texture, hotspot, CursorMode.Auto);
        isCursorTarget = true;
    }
    public void SetCursorTargetEnemy()
    {
        hotspot = new Vector2(cursorTarget_Texture.width / 2, cursorTarget_Texture.height / 2);
        Cursor.SetCursor(cursorTargetEnemy_Texture, hotspot, CursorMode.Auto);
        isCursorTarget = true;
    }

    public void CreateIndicator(Vector3 pos, bool isAttack = false)
    {
        GameObject indicator = Instantiate(indicatorPrefab);
        indicator.transform.parent = null;
        indicator.transform.position = pos + Vector3.up * 0.01f;
        var renderers = indicator.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
            renderer.color = isAttack ? Color.red : Color.white;
        indicator.SetActive(true);
    }
}
