using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene = "Scene_Start";
    public Sprite tabtostart;
    public UserManager userManager;
    public PopUpManager popUpManager;
    AsyncOperation op;
    public Image progressBar;
    public GameObject tapToStart;
    IEnumerator loadScene;

    string nextSceneName;
    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        loadScene = LoadScene();
        StartCoroutine(loadScene);
    }

    IEnumerator LoadScene()
    {
        yield return null;

        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress * timer * 0.5f >= 0.9f)
            {
                //if (progressBar)
                //{
                    progressBar.fillAmount = op.progress * timer * 0.5f;

                if (progressBar.fillAmount == 1.0f)
                {
                    if (userManager.firstPlay == 0)
                        popUpManager.startInitionalize();
                    else
                        popUpManager.startLogIn();
                    tapToStart.SetActive(true);
                    progressBar.gameObject.SetActive(false);
                    stopCoroutine();
                }
                //}
            }
            else
            {
                //if(progressBar)
                    progressBar.fillAmount = op.progress * timer * 0.5f;
            }
        }
    }

    public void stopCoroutine()
    {
        StopCoroutine(loadScene);
    }
    public void disablePanelAndStartGame()
    {
        op.allowSceneActivation = true;
        
    }
}
