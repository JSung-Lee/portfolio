using UnityEngine;


public class VisualSkillEffect : SkillEffect
{
    public bool followEffectMount = true;
    public float lifeTime = 3;

    private void Start()
    {
        //Debug.Log("Start");
    }

    private void OnDestroy()
    {
        //Debug.Log("Destroy");
    }

    void Update()
    {
        if (lifeTime <= 0)
        {
            // 아래 조건문 추가(kdh)
            if (afterVisualEffect)
                CreateAfterVisualEffect();

            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;
    }
}
