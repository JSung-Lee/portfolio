using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public float lifeTime = 0.25f;
    public float timer = 0;

    // Start is called before the first frame update
    private void OnEnable()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= lifeTime)
            Destroy(gameObject);
        timer += Time.deltaTime;
    }


}
