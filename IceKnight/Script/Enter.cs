using UnityEngine;

public class Enter : MonoBehaviour {
    private GameObject currentFX;
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Controller.MoveOn = false;
            //움직임을 0으로 만듦
            Controller.tr.Translate(Vector3.forward * 0);
            Controller.playerPosition.Set(Mathf.Round(Controller.tr.position.x), Mathf.Round(Controller.tr.position.y), Mathf.Round(Controller.tr.position.z));
            Controller.tr.position = Controller.playerPosition;
            currentFX = Instantiate(Controller.smokeFX, Controller.tr.position, Controller.tr.rotation);
            Destroy(currentFX, 0.5f);
            Controller.anim.SetTrigger("Damage");
            Controller.anim.SetBool("Move", false);
        }
    }
}
