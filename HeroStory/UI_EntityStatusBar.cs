using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EntityStatusBar : MonoBehaviour
{
    public SpriteRenderer m_HealthBar;
    public SpriteRenderer m_ManaBar;
    public SpriteRenderer m_Shield;
    public TextMesh levelText;
    public Camera mainCamera;

    public Entity player;

    public float gap = 1f;
    public Vector3 revisionRatio = new Vector3(1f, 0f, 1f);
    Vector3 revisionPosition = new Vector3(0, 0, 0);

    void Start()
    {
        if (!mainCamera)
            mainCamera = CameraManager.Instance.transform.GetComponent<Camera>();

        Update();
    }
    void Update()
    {
        if (!player) 
            player = gameObject.transform.parent.GetComponent<UI_targetEnemyStatus>().owner; //Player.player;

        if (player)
        {
            if (player.team == Team.Enemy && gameObject.layer != 10)
                gameObject.layer = 10;
            
           // var screenPosition = Camera.main.WorldToScreenPoint(player.transform.position);
            var screenPosition = mainCamera.WorldToScreenPoint(player.transform.position);

            // 현재 카메라 내에 있는지 여부 판단
            if (isTargetCameraVisible(mainCamera))
            {
                revisionPosition.Set((screenPosition.x / (Screen.width * 0.5f) - 1f) * revisionRatio.x,
                     0f * revisionRatio.y,
                     (screenPosition.y / (Screen.height * 0.5f) - 1f) * revisionRatio.z);
                
                // 캐릭터의 센터 마운트를 Follow
                if(player.centerMount)
                    transform.position = player.centerMount.transform.position + (Vector3.up * gap) - revisionPosition;
                else
                    transform.position = player.transform.position + (Vector3.up * gap) - revisionPosition;
            }
            else
            {
                if (player.centerMount)
                    transform.position = player.centerMount.transform.position + (Vector3.up * gap);
                else
                    transform.position = player.transform.position + (Vector3.up * gap);
            }




            if (m_HealthBar)
                m_HealthBar.material.SetFloat("_Progress", (float)player.health / ((float)player.healthMax+(float)player.shield));
            if(m_ManaBar)
                m_ManaBar.material.SetFloat("_Progress", (float)player.mana / (float)player.manaMax);


            if(m_Shield)
                m_Shield.material.SetFloat("_Progress", ((float)player.health + (float)player.shield) / ((float)player.healthMax + (float)player.shield));

            if (levelText) levelText.text = player.level.ToString();
        }
    }

    // 카메라 내에 오브젝트가 존재하는지 여부 
    public bool isTargetCameraVisible(Camera _camera)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        var point = transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                return false;
        }
        return true;

        //Vector3 screenPoint = _camera.WorldToScreenPoint(transform.parent.gameObject.transform.position);
        //bool onScreen = 0 < screenPoint.x && screenPoint.x < 1 
        //                && 0 < screenPoint.y && screenPoint.y < 1;

        //return onScreen;
    }
}
