using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreColor : MonoBehaviour {

    private float interval;
    private float timer;
    private int index;
    private List<Color> colors = new List<Color>();
    private Color alpha = new Color(1.0f,1.0f,1.0f,0.0f);
    public Outline outline;
    public Outline outline_half;
    //public Animator anim;
    private bool isUpScore;
    private float upScoreTimer;
    private float upScoreInterval;
    private float upScoreNum;
    private float score;
    public AudioSource sound;
    public UserManager user;
    public GameObject gameData;
    public Animator gameData_Anim;
    public Text textComponent;
    public Image StartButtonImage;
	// Use this for initialization
	void OnEnable () {
        transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.89f, 0);
        colors.Add(new Color(1.0f, 0.25f, 0.25f, 0.5f));
        colors.Add(new Color(1.0f, 0.25f, 1.0f, 0.5f));
        colors.Add(new Color(0.25f, 0.25f, 1.0f, 0.5f));
        colors.Add(new Color(0.25f, 1.0f, 1.0f, 0.5f));
        colors.Add(new Color(0.25f, 1.0f, 0.25f, 0.5f));
        colors.Add(new Color(1.0f, 1.0f, 0.25f, 0.5f));
        colors.Add(new Color(1.0f, 1.0f, 1.0f, 0.5f));
        index = colors.Count - 1;
        outline.effectColor = colors[index];
        Color halfColor = colors[index];
        halfColor.a = 0.25f;
        outline_half.effectColor = halfColor;
        timer = 0.0f;
        interval = 2.0f;
        isUpScore = false;
        upScoreTimer = 0.0f;
        gameData.SetActive(false);
        upScoreInterval = 0.0f;
    }
	
	void Update() {
        if (textComponent.color.a < 1.0f)
        {
            Color effectColor = Color.white;
            effectColor.a = 0.5f * textComponent.color.a;
            outline.effectColor = effectColor;
            effectColor.a = 0.25f * textComponent.color.a;
            outline_half.effectColor = effectColor;
            
        }
        else
        {
            timer += Time.deltaTime;
            int nextIndex = index + 1;
            if (nextIndex >= colors.Count - 1)
                nextIndex = 0;
            Color c = colors[nextIndex] - colors[index];
            c /= interval;
            outline.effectColor += c * Time.deltaTime;
            outline_half.effectColor += c * Time.deltaTime;
            if (timer >= interval)
            {
                outline.effectColor = colors[nextIndex];
                Color halfColor = colors[nextIndex];
                halfColor.a = 0.25f;
                outline_half.effectColor = halfColor;
                index = nextIndex;
                timer = 0.0f;
            }
            
        }

        if(isUpScore)
        {
            //터치시 이벤트
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    upScoreTimer = upScoreInterval;
                    gameData.SetActive(true);
                }
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        upScoreTimer = upScoreInterval;
                        gameData.SetActive(true);
                    }
                }
            }

            if (upScoreTimer >= upScoreInterval)
            {
                if (!gameData.activeSelf)
                    gameData.SetActive(true);
                upScoreTimer = 0.0f;
                upScoreNum = 0.0f;
                isUpScore = false;
                upScoreNum = score;
                sound.Stop();
                StartButtonImage.raycastTarget = true;
            }
            else
            {
                if (upScoreTimer >= upScoreInterval * 0.66f)
                    if (!gameData.activeSelf)
                        gameData.SetActive(true);
                upScoreNum += score * (Time.deltaTime / upScoreInterval);
                upScoreTimer += Time.deltaTime;
            }
            transform.GetComponent<Text>().text = ((int)upScoreNum).ToString();
        }
	}

    public void onUpScore(float upInterval)
    {
        index = colors.Count - 1;
        timer = 0.0f;
        textComponent.color = alpha;
        outline.effectColor = alpha;
        outline_half.effectColor = alpha;
        StartButtonImage.raycastTarget = false;
        upScoreInterval = upInterval;
        upScoreTimer = 0.0f;
        upScoreNum = 0.0f;
        isUpScore = true;
        score = System.Convert.ToInt32(transform.GetComponent<Text>().text);
        sound.volume = user.getEffect();
        sound.Play();
    }

    private void OnDisable()
    {
        Color color = textComponent.color;
        color.a = 1.0f;
        textComponent.color = color;
    }

    public void clickStartButton()
    {
        if (GameMng.self.sceneNumber == (int)GameMng.Scenes.Scene_Play)
        {
            //Debug.Log(gameData_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (gameData_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                return;
            GameMng.self.changeScene(GameMng.Scenes.Scene_Start);
        }
    }
}
