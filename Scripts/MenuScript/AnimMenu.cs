using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        transform.localScale = new Vector2(0.3f, 0.3f);
        transform.LeanScale(Vector2.one, 0.2f);
    }
    // Update is called once per frame
}
