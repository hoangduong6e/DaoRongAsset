using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLuaTest;

public class LuaUpdate : LuaBehaviour
{

    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second 
    protected Action luaUpdate;
    protected override void Awake()
    {
        base.Awake();
        scriptEnv.Get("update", out luaUpdate);
    }
    // Update is called once per frame
    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
        if (Time.time - lastGCTime > GCInterval)
        {
            luaEnv.Tick();
            lastGCTime = Time.time;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        luaUpdate = null;
    }
}
