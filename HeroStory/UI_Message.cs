using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Message : MonoBehaviour
{
    public static UI_Message self;
    public Text message;
    public float lifeTime = 2f;
    float timer = 0f;

    void Start()
    {
        if (self)
            Destroy(this);
        self = this;
        timer = 0f;
        message.text = "";
    }

    public void SetMessage(string contents, float _lifeTime = 2f)
    {
        lifeTime = _lifeTime;
        message.text = contents;
        timer = 0f;
    }    
    void Update()
    {
        if (timer >= lifeTime)
            message.text = "";
        timer += Time.deltaTime;
    }
}
