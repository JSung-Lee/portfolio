using UnityEngine;
public class FollowCam : MonoBehaviour
{
    public GameObject Player;
    public static Camera mainCamera;
    public float rotX = 70;

    void Start()
    {
        Screen.SetResolution(1080, 1920, false);
        Player = StageManager.Player_Current;
        mainCamera = GetComponent<Camera>();
        this.transform.Rotate(rotX, 0, transform.rotation.z);
        this.transform.position = Player.transform.position + StageManager.visionPos;
        DirectionManager.directionManager.GetComponent<DirectionManager>().startDirection();
    }

    void LateUpdate()
    {
        if (!StageManager.isMapClick && Player)
        {
            this.transform.position = Player.transform.position + StageManager.visionPos;
        }
    }
    void iTweenEnd()
    {
        StageManager.isMapClick = false;
            mainCamera.GetComponent<FollowCam>().enabled = true;
        Time.timeScale = 1;
    }
}