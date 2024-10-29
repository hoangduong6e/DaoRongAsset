using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConMa : MonoBehaviour
{
    // Update is called once per frame
    float time = 0, maxtime = 3f;
    void Update()
    {
        transform.position += Vector3.up * 2 * Time.deltaTime;
        time += Time.deltaTime; 
        if(time >= maxtime)
        {
            Destroy(gameObject);
        }
    }
}
