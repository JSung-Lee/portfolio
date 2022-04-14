using UnityEngine;

public class BuffSkillEffect : SkillEffect
{
    public float lastRemainingTime = Mathf.Infinity;
   
    [HideInInspector] public string buffName;

    public bool isOnlyHitMaintain = false;
    public bool useLifeTime = true;
    public float lifeTime = 1;
    public float timer = 0;

   public void TimerClean()   { timer = 0; }
    void Update()
    {

        if (target != null)
        {
            if (useLifeTime)
            {
                if (timer >= lifeTime && !isOnlyHitMaintain)
                    Destroy(gameObject);

                timer += Time.deltaTime;
            }

            // 아래 구문 들 caster -> target 바꿈(kdh)
            int index = target.buffs.FindIndex(s => s.name == buffName);
            
            if (index != -1)
            {
                Buff buff = target.buffs[index];

                if (lastRemainingTime >= buff.BuffTimeRemaining())
                {
                    transform.position = target.collider.bounds.center;
                    if (yisZero) transform.SetYPosition(0);
                    transform.rotation = target.transform.rotation;
                    lastRemainingTime = buff.BuffTimeRemaining();

                    return;
                }
            }
        }

        if(isOnlyHitMaintain) Destroy(gameObject);
    }
}