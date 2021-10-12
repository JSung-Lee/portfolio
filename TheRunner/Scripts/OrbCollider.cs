using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
