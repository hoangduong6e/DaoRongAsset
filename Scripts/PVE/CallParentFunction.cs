using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallParentFunction : MonoBehaviour
{
    public void CallOfName(string functionName)
    {
        Transform parent = transform.parent;
        if (parent != null)
        {
            parent.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
        }
    }
}
