using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log<T>(T message)
    {
        Debug.Log(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning<T>(T message)
    {
        Debug.LogWarning(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError<T>(T message)
    {
        Debug.LogError(message);
    }
}
