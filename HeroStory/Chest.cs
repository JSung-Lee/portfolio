using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Monster
{
    public AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
    }

    private void LateUpdate()
    {
        animator.SetBool("Dead", state == State.Dead);
    }

    public void PlaySoundChestOpne()
    {
        if(audioSource)
        {
            if (SettingManager.self) audioSource.volume = SettingManager.self.CalcFxValue();
            audioSource.Play();
        }
    }
}
