using UnityEngine;
using UnityEngine.SceneManagement;
public class Trap_H : MonoBehaviour {
    private GameObject currentFX;
    public AudioClip Trap_HSound;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AudioSource.PlayClipAtPoint(Trap_HSound, transform.position);
            Controller.MoveOn = false;
            Controller.anim.Play("Die");
            Invoke("testFunc", 1.5f);
        }
    }
    void testFunc()
    {
        if (PlayerPrefs.GetInt("nCheckRevival") == 1)
        {
            if (PlayerPrefs.GetInt("nRevival") > 0 && Controller.nRevival < PlayerPrefs.GetInt("nRevival"))
            {
                if (!Controller.isDie)
                {
                    DirectionManager.directionManager.GetComponent<DirectionManager>().OpenRevivlaPopup();
                    Time.timeScale = 0;
                }
            }
            else
            {
                Time.timeScale = 0;
                SceneManager.LoadScene("GameOverScenePortrait");
                Time.timeScale = 1;
            }
        }
        else
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("GameOverScenePortrait");
            Time.timeScale = 1;
        }
        Controller.isDie = true;
    }
}
