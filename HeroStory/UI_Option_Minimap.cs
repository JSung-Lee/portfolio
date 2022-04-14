using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option_Minimap : MonoBehaviour
{
    Player player;
    public static UI_Option_Minimap self;
    //public GameObject enemy_Count;

    //RectTransform obj;

    float default_scale = 500f;
    float default_position = -300f;


    void OnEnable()
    {
        if (self) Destroy(this);
        else self = this;

        player = Player.player;

        //obj = transform.GetComponent<RectTransform>();
    }

    public void Scale_MiniMap(float scale)
    {
        scale *= 0.01f;
        var new_pos = default_position + (1f - scale) * default_scale * 0.5f;

        transform.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 0);
        transform.GetComponent<RectTransform>().localPosition = new Vector3(new_pos, new_pos, 0);
        /*
        var enemyCount_rectTr = enemy_Count.GetComponent<RectTransform>();
        var enemy_width = obj.rect.x + (obj.rect.width * (-0.2f)) + (enemyCount_rectTr.rect.width * (-0.2f) - 50);
        enemyCount_rectTr.localPosition = new Vector3(enemy_width, -300);
        */
    }
    //  minimap  x +  minimap width/(-2)  ec width/(-2) -50
}
