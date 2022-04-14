using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public GameObject book;
    
    public AudioSource audioSource;
    public AudioClip audioClip_Open;
    public AudioClip audioClip_Close;

    public virtual void SetActiveBook(bool active)
    {
        PlaySound(active);
        book.SetActive(active);
    }

    public void PlaySound(bool active)
    {
        if (audioSource)
        {
            if (SettingManager.self) audioSource.volume = SettingManager.self.CalcFxValue();
            audioSource.clip = active ? audioClip_Open : audioClip_Close;
            audioSource.Play();
        }
    }

    public void SetActiveBookWittoutSound(bool active)
    {
        book.SetActive(active);
    }

    public bool IsActiveBook()
    {
        return book.activeSelf;
    }
}
