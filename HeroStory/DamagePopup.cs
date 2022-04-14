using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMesh[] textMesh;

    [SerializeField] TextMeshPro textMeshPro;
    public SpriteRenderer sprite;
    public Entity receiver;
    public string skillName;
    public float damage;
    public Animator animator;
    public int interval;
    public AudioSource audioSource;
    public AudioClip audioClip_Coin;
    public AudioClip audioClip_Fail;

    public float lastCallTime = 0f;

    private const float DISAPPEAR_TIMER_MAX = 1f;
    [Header("LifeTime")]
    public float disappearTimer = DISAPPEAR_TIMER_MAX;
    public float moveYSpeed = 20f;
    public float moveXSpeed = 0.5f;
    public float moveXGap = 20f;
    public float disappearSpeed = 25f;
    public Color textColor;

    public void Update()
    {
        disappearTimer -= Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .3f)
        {
            float increaseScaleAmount = 1f;
            transform.position += new Vector3(moveXSpeed, moveYSpeed) * Time.deltaTime;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;

        }
        else
        {
            float increaseScaleAmount = 1f;
            transform.position += new Vector3(moveXSpeed, -moveYSpeed) * moveXGap * Time.deltaTime;
            transform.localScale -= Vector3.one * increaseScaleAmount * Time.deltaTime;
        }

        if (disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMeshPro.color = textColor;
            sprite.color = textColor;

            if (textColor.a < 0){
                transform.gameObject.SetActive(false);
                disappearTimer = DISAPPEAR_TIMER_MAX;
            }
        }
    }

    
    public void SetText(string str)
    {
        textMeshPro.text = str;
        //foreach (var mesh in textMesh)
        //    mesh.text = str;
    }
    public void SetTextSize(int size)
    {
        textMeshPro.fontSize = size;
        //Debug.Log("Popup size : " + size);
        //foreach (var mesh in textMesh)
        //    mesh.fontSize = size;
    }
    public int GetTextSize()
    {
        return textMesh[0].fontSize;
    }

    public void SetTextColor(Color color)
    {
        textColor = new Color(color.r, color.g, color.b, color.a ==0 ? 1 : color.a);
        textMeshPro.color = textColor;
        sprite.color = textColor;
    }
    public void ciriticalshowing(bool active)
    {
        sprite.gameObject.SetActive(active);
    }
    public void DisableDamagePopup()
    {
        if(Time.time - lastCallTime >= 0.5f)
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(false);
            transform.position = receiver.transform.position + (Vector3.up * interval);
            gameObject.SetActive(true);
        }
    }

    public void PlaySound(int amount)
    {
        if (!audioSource) return;
        if (SettingManager.self) audioSource.volume = SettingManager.self.CalcFxValue();

        audioSource.clip = amount > 0 ? audioClip_Coin : audioClip_Fail;
        audioSource.Play();
    }
}
