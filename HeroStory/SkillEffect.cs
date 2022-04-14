using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public Sprite image;
    public float size;
    public float height;
    public bool yisZero = false;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Entity target;
    [HideInInspector] public Entity caster;
    public SkillEffect afterVisualEffect;
   
    public BuffSkillTemplate buffSkillTemplate;
    public int buffSkillLevel;

    public AudioSource audioSource;
    public Vector3 hitPos;


    public void SethitPoint(Vector3 pos)
    {
        hitPos = pos;
    }
    private void Awake()
    {
        if (audioSource && SettingManager.self) {
            audioSource.volume = SettingManager.self.CalcFxValue();
        }
    }

    public virtual void OnAggroEnter(Entity entity)
    {

    }


    public virtual void OnAggroStay(Entity entity)
    {

    }
    public virtual void OnAggroExit(Entity entity)
    {

    }

    public virtual void CreateAfterVisualEffect()
    {
        Instantiate(afterVisualEffect.gameObject, transform.position, Quaternion.identity);
    }

    public virtual void AddBuffToHittedTarget(Entity _target)
    {
        if (buffSkillTemplate && caster && _target)
        {
            _target.AddOrRefreshBuff(new Buff(buffSkillTemplate, buffSkillLevel, caster));

            // 버프 스킬 이펙트 추가(kdh)
            buffSkillTemplate.SpawnEffect(caster, _target);

        }
    }
}
