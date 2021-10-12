using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading_Game_Scene : MonoBehaviour
{
    float fTime = 0f;
    public Text animText;
    public GameObject animObj;
    AsyncOperation async_operation;

    void Start()
    {
        Time.timeScale = 1;
        async_operation = SceneManager.LoadSceneAsync("FileIOTest");
        async_operation.allowSceneActivation = false;
        animText.GetComponent<WritterAnimation>().startAnim();
        animObj = Instantiate(Resources.Load<GameObject>("Prefab/Character/Player"));
        animObj.GetComponent<Transform>().position = new Vector3(0, 0.213f, -5);
        animObj.GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
        animObj.transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/Dummy Prop Right/Hammer").gameObject.SetActive(true);
        animObj.transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigLArm1/RigLArm2/RigLArmPalm/Dummy Prop Left/Shield").gameObject.SetActive(true);
        animObj.GetComponent<Animator>().SetTrigger("Spin");
        animObj.GetComponent<Controller>().enabled = false;
    }

    void Update()
    {
        fTime += Time.deltaTime;

        if (async_operation.progress >= 0.9f)
        {
            if (fTime >= 2.0f)
                async_operation.allowSceneActivation = true;
            else
                Progress.value = (async_operation.progress / 0.9f) * (fTime / 2);
        }
        else
        {
            Progress.value = (async_operation.progress / 0.9f) * (fTime / 2);
        }
    }
}