using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HieuUngItem : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = new Vector2(0.1f, 0.1f);
        transform.eulerAngles = new Vector3(0,0,Random.Range(-180,180));
        transform.LeanScale(Vector2.one, 0.3f);
        transform.LeanRotate(new Vector3(0,0,0),0.3f);
        transform.LeanMove(transform.parent.transform.position,0.3f);
    }
}
