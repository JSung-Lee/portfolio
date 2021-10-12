using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour {
    public GameObject[] skins;
    public GameObject[] skin_Effects;
    public GameObject death;
    public AudioSource deathAudio;
    public GameObject score;
    public GameObject reStart;
    public UserManager userManager;     //게임 데이터 관리
    public Text comboText;
    public Text bestScore;
    public Text dataScore;
    public Text dataCombo;
    public Text dataDistance;
    //public GameObject gameData;
    public SceneMng sceneMng;
    private float scoreRatio;
    private int activatedSkinColor;
    private bool gameOver;
    public int bestCombo;
    private int totalCombo;
    public GameObject reTryBoard;
    private void OnEnable()
    {
        transform.position = new Vector3(0, 1, 5);
        activatedSkinColor = 0;
        scoreRatio = 1.0f;
        skins[0].SetActive(true);
        skins[1].SetActive(false);
        skins[2].SetActive(false);
        skins[3].SetActive(false);
        death.SetActive(false);
        reStart.SetActive(false);
        bestScore.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        //gameData.SetActive(false);
        gameOver = false;
        bestCombo = 0;
        totalCombo = 0;
        userManager.setBestCombo(0);
        userManager.setBestDistance(0);
        Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
        for (int i = 0; i < skin_Effects.Length; i++)
        {
            skin_Effects[i].transform.localScale = scale;
        }
    }
    
    void setSkinColor(int index)
    {
        if (skins[index].activeSelf)
            return;
        skins[index].SetActive(true);
        activatedSkinColor = index;
        Invoke("enabledSkinDeActivation", 0.2f);
    }

    void enabledSkinDeActivation()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            if (activatedSkinColor == i)
                continue;
            if (skins[i].activeSelf)
            {
                skins[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            int orbColor = other.transform.parent.GetComponent<Orb>().getOrbColor();
            switch (orbColor)
            {
                case (int)OrbManager.orbColor.white:
                    userManager.plusCombo(1);
                    userManager.plusScore(userManager.getCombo() * scoreRatio);
                    comboText.text = "Combo " + userManager.getCombo().ToString();
                    comboText.GetComponent<Combo>().setEffectColor(new Color(0.25f, 0.25f, 0.25f, 0.5f));
                    comboText.gameObject.SetActive(false);
                    comboText.gameObject.SetActive(true);
                    totalCombo++;
                    break;
                case (int)OrbManager.orbColor.red:
                    if (activatedSkinColor == orbColor)
                        userManager.plusCombo(1);
                    else
                    {
                        if(activatedSkinColor == (int)OrbManager.orbColor.white)
                            userManager.plusCombo(1);
                        else 
                            userManager.setCombo(1);
                        setSkinColor(orbColor);
                    }
                    activatedSkinColor = orbColor;
                    userManager.plusScore(userManager.getCombo() * scoreRatio);
                    comboText.text = "Combo " + userManager.getCombo().ToString();
                    comboText.GetComponent<Combo>().setEffectColor(new Color(1.0f, 0.25f, 0.25f, 0.5f));
                    comboText.gameObject.SetActive(false);
                    comboText.gameObject.SetActive(true);
                    totalCombo++;
                    break;
                case (int)OrbManager.orbColor.green:
                    if (activatedSkinColor == orbColor)
                        userManager.plusCombo(1);
                    else
                    {
                        if (activatedSkinColor == (int)OrbManager.orbColor.white)
                            userManager.plusCombo(1);
                        else
                            userManager.setCombo(1);
                        setSkinColor(orbColor);
                    }
                    activatedSkinColor = orbColor;
                    userManager.plusScore(userManager.getCombo() * scoreRatio);
                    comboText.text = "Combo " + userManager.getCombo().ToString();
                    comboText.GetComponent<Combo>().setEffectColor(new Color(0.25f, 1.0f, 0.25f, 0.5f));
                    comboText.gameObject.SetActive(false);
                    comboText.gameObject.SetActive(true);
                    totalCombo++;
                    break;
                case (int)OrbManager.orbColor.blue:
                    if (activatedSkinColor == orbColor)
                        userManager.plusCombo(1);
                    else
                    {
                        if (activatedSkinColor == (int)OrbManager.orbColor.white)
                            userManager.plusCombo(1);
                        else
                            userManager.setCombo(1);
                        setSkinColor(orbColor);
                    }
                    activatedSkinColor = orbColor;
                    userManager.plusScore(userManager.getCombo() * scoreRatio);
                    comboText.text = "Combo " + userManager.getCombo().ToString();
                    comboText.GetComponent<Combo>().setEffectColor(new Color(0.25f, 0.25f, 1.0f, 0.5f));
                    comboText.gameObject.SetActive(false);
                    comboText.gameObject.SetActive(true);
                    totalCombo++;
                    break;
                case (int)OrbManager.orbColor.purple:
                    userManager.setCombo(0);
                    setSkinColor((int)OrbManager.orbColor.white);
                    activatedSkinColor = (int)OrbManager.orbColor.white;
                    break;
                case (int)OrbManager.orbColor.pig:
                    //userManager.plusCombo(1);
                    userManager.plusScore(50);
                    break;
            }
            other.transform.parent.parent.GetComponent<OrbManager>().createFX(transform.position, orbColor);
            other.transform.parent.gameObject.SetActive(false);
            if (orbColor <= (int)OrbManager.orbColor.purple)
            {
                Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f) * (1.0f + (userManager.getCombo() * 0.2f));
                if (scale.x > 3.0f)
                    scale = new Vector3(3.0f, 3.0f, 3.0f);
                skin_Effects[orbColor].transform.localScale = scale;

                if (userManager.getCombo() >= bestCombo)
                    bestCombo = userManager.getCombo();
            }
        }
        else if (other.CompareTag("Line"))
        {
            int num = (int)userManager.getBestScore() * 10;
            bestScore.text = "최고점수 " + (num / 10).ToString();

            comboText.gameObject.SetActive(false);
            deathAudio.volume = userManager.getEffect();
            death.SetActive(true);
            dataScore.text = ((int)userManager.getScore()).ToString();
            dataCombo.text = bestCombo.ToString();
            dataDistance.text = ((int)userManager.getDistance()).ToString();
            sceneMng.onBackgroundMusicFadeOut();
            Invoke("stopGame", 2.0f);
            gameOver = true;
        }

    }

    void stopGame()
    {
        sceneMng.offBackgroundMusicFadeOut();
        reTryBoard.SetActive(false);
        reStart.SetActive(true);
        score.GetComponent<ScoreColor>().onUpScore(2.78f);
        score.transform.position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f * 1.10f, 0);
        score.GetComponent<Animator>().SetTrigger("play");
        bestScore.gameObject.SetActive(true);
        bestScore.transform.position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f * 0.95f, 0);
        //gameData.SetActive(true);
        if (userManager.getBestScore() < userManager.getScore())
        { 
            userManager.scoreUpdate();
        }
        userManager.setBestDistance(userManager.getDistance());
        userManager.saveData();
    }

    public bool isGameOver()
    { return gameOver; }
}
