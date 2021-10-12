using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelect : MonoBehaviour {

    public UserManager user;
    public Text bestScore;
    public Text icon_Sound_BGM;
    public Text icon_Sound_Effect;
    public GameObject icon_Main;
    public GameObject icon_Setting;
    public GameObject panel_Tutorial;
    public GameObject panel_Event;
    public GameObject panel_RankingData;
    public GameObject panel_Warning;
    public AudioSource sound;
    public AudioSource music;
    public AudioClip defaultClip;
    public GameObject soundOn;
    public GameObject soundOff;
    public EmailSender email;
    private void OnEnable()
    {
        if (user.getBGM() == 1)
        {
            icon_Sound_BGM.text = "배경음 켜짐";
            if(GameMng.self.sceneNumber == (int)GameMng.Scenes.Scene_Start)
                music.volume = user.getBGM();
        }
        else
        {
            icon_Sound_BGM.text = "배경음 꺼짐";
            music.volume = user.getBGM();
        }
        if (user.getEffect() == 1)
            icon_Sound_Effect.text = "효과음 켜짐";
        else
            icon_Sound_Effect.text = "효과음 꺼짐";
        if(user.getEffect() == 1 || user.getBGM() == 1)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        icon_Main.SetActive(true);
        icon_Setting.SetActive(false);
        panel_Tutorial.SetActive(false);
        panel_RankingData.SetActive(false);
        panel_Warning.SetActive(false);
        bestScore.text = "최고점수 " + ((int)user.getBestScore()).ToString();
        if (user.isBeginner < 5)
        {
            panel_Tutorial.SetActive(true);
            user.isBeginner += 1;
        }
        if (user.pigEvent > 1.0f)
        {
            panel_Event.SetActive(true);
        }
        else
        {
            panel_Event.SetActive(false);
        }
    }

    public void clickBgmIcon(AudioSource s)
    {
        if (user.getBGM() == 0)
        {
            icon_Sound_BGM.text = "배경음 켜짐";
            user.setBGM(1);
            if (GameMng.self.sceneNumber == (int)GameMng.Scenes.Scene_Start)
                music.volume = user.getBGM();   
        }
        else
        {
            icon_Sound_BGM.text = "배경음 꺼짐";
            user.setBGM(0);
            music.volume = user.getBGM();
        }
        if (user.getEffect() == 1 || user.getBGM() == 1)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickEffectIcon(AudioSource s)
    {
        if (user.getEffect() == 0)
        {
            icon_Sound_Effect.text = "효과음 켜짐";
            user.setEffect(1);
        }
        else
        {
            icon_Sound_Effect.text = "효과음 꺼짐";
            user.setEffect(0);
        }
        if (user.getEffect() == 1 || user.getBGM() == 1)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickMuteIcon(AudioSource s)
    {
        if(soundOn.activeSelf)
        {
            user.setEffect(0);
            user.setBGM(0);
            icon_Sound_BGM.text = "배경음 꺼짐";
            icon_Sound_Effect.text = "효과음 꺼짐";
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            user.setEffect(1);
            user.setBGM(1);
            icon_Sound_BGM.text = "배경음 켜짐";
            icon_Sound_Effect.text = "효과음 꺼짐";
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        music.volume = user.getBGM();
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickHelpIcon(AudioSource s)
    {
        panel_Tutorial.SetActive(true);
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickWarningIcon(AudioSource s)
    {
        panel_Warning.SetActive(true);
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickSettingBackIcon(AudioSource s)
    {
        icon_Main.SetActive(true);
        icon_Setting.SetActive(false);
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickSettingIcon(AudioSource s)
    {
        icon_Main.SetActive(false);
        icon_Setting.SetActive(true);
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickURLIcon(AudioSource s)
    {
        //Application.OpenURL("https://www.i-m-all.com");
        InAppBrowser.OpenURL("https://i-m-all.com");
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void clickMainRankingIcon(AudioSource s)
    {
        panel_RankingData.SetActive(true);
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();

    }

    public void clickButton(GameObject g)
    {
        if(!g.activeSelf)
            g.SetActive(true);
        else
            g.SetActive(false);
        sound.volume = user.getEffect();
        sound.clip = defaultClip;
        sound.Play();
    }

    public void onClickNoticePage(AudioSource s)
    {
        InAppBrowser.DisplayOptions displayOptions = new InAppBrowser.DisplayOptions();
        displayOptions.pageTitle = "공지사항";
        InAppBrowser.OpenURL(UserManager.hostUrl + "notice.php", displayOptions);
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }

    public void onClickBugReport(AudioSource s)
    {
        email.OnClickEvent();
        sound.volume = user.getEffect();
        sound.clip = s.clip;
        sound.Play();
    }
}
