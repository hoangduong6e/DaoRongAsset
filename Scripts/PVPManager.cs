using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPManager : MonoBehaviour
{
     public static Dictionary<string, Dictionary<string, Transform>> DragonsTF = new()
     {
        { "b", new Dictionary<string, Transform>() },//teamxanh
        { "r", new Dictionary<string, Transform>() }//teamdo
     };


     public static void AddDragonTF(string team, string id, Transform tf)
     {
        DragonsTF[team].Add(id,tf);
     }

     
}
