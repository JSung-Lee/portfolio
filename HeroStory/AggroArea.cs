using UnityEngine;

// 몬스터/플레이어의 캐릭터가 현재 범위에 들어왔는지에 체크 후 어그로 관련 Trigger 처리
[RequireComponent(typeof(SphereCollider))]
public class AggroArea : MonoBehaviour
{
    public Entity owner;

    // Entity(owner)가 Co(Collider)에 충돌을 했을때
    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.layer == LayerMask.NameToLayer("Player") ||
            co.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            var entity = co.GetComponentInParent<Entity>();

            if (entity && entity.team != owner.team)
                owner.OnAggroEnter(entity);
        }
    }

    // Entity(owner)가 Co(Collider)에 충돌을 해 있을때를 의미
    void OnTriggerStay(Collider co)
    { 
        if (co.gameObject.layer == LayerMask.NameToLayer("Player") ||
            co.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            var entity = co.GetComponentInParent<Entity>();

            if (entity && entity.team != owner.team)
                owner.OnAggroStay(entity);
        }
    }
}