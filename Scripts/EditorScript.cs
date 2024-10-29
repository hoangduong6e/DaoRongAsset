using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScript
{
#if UNITY_EDITOR
    public static bool editorGetData = true;
#endif
    #if UNITY_IOS
    public static bool ngrok = false;
#endif
}
