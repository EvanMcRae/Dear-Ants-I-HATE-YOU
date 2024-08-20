using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeDelete : MonoBehaviour
{
    public float Lifetime;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > Lifetime + startTime)
            Destroy(gameObject);
    }
}
