using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MenuEventHalloween2024 : EventManager
{
    protected override void ABSAwake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void DiemDanhOk(JSONNode json)
    {

    }

    public void ParseData(JSONNode json)
    {
        debug.Log("data event: " + json.ToString());
    }
}
