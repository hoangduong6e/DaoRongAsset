using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDuoi : MonoBehaviour
{
    public Transform parent;
    public ParticleSystem particleSystem;
    void Update()
    {

        var shape = particleSystem.shape;
        if (parent.transform.localScale.x < 0)
        {
            shape.alignToDirection = true;
        }
        else
        {

            shape.alignToDirection = false;
        }

        //if (parent.transform.localScale.x < 0)
        //{
        //    if(transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        //}
        //else if (transform.localScale.x < 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
