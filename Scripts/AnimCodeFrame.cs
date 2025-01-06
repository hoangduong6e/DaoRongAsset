using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCodeFrame : MonoBehaviour
{
    public Action<string> action;

    public void CallAction(string str)
    {
        if (action != null) action(str);
    }    
}
