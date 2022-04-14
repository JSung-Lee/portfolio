using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpener : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if (!player) return;
            if (UI_Toggle.self) UI_Toggle.self.OpenUI_Store();
        }
    }
}
