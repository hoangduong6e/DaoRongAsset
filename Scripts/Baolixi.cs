using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Baolixi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delay(Random.Range(0.1f, 1)));
        IEnumerator delay(float s)
        {
            yield return new WaitForSeconds(s);
            GetComponent<Animator>().enabled = true;
        }
    }
}
