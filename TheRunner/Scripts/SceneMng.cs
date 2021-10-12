using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMng : MonoBehaviour {
    public AudioSource sound;
    public UserManager user;
    public AudioClip[] clips;
    IEnumerator fadeSound;

    private void OnEnable()
    {
        if (clips.Length > 0)
            sound.clip = clips[Random.Range(0, clips.Length)];
        sound.volume = user.getBGM();
        sound.Play();
    }

    public void onBackgroundMusicFadeOut()
    {
        fadeSound = AudioFadeOut(sound, 2.0f);
        if (fadeSound == null)
            return;
        StartCoroutine(fadeSound);
    }

    public void offBackgroundMusicFadeOut()
    {
        if (fadeSound == null)
            return;
        StopCoroutine(fadeSound);
    }

    IEnumerator AudioFadeOut(AudioSource source, float fadeTime)
    {
        float startVolume = source.volume;

        while(source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        source.Stop();
        source.volume = startVolume;
    }
    public void changeScene1()
    {
        SceneManager.LoadScene("Scene_Start");
    }
}
