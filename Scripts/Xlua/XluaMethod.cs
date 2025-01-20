using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using XLua;
[LuaCallCSharp]
public class XluaMethod 
{
    public void SetColor(Image img,byte r, byte g, byte b, byte a) => img.color = new Color32(r, g, b, a);

}

[LuaCallCSharp]
public class XluaNetwork
{
    public void SendRequest(string method, string nameClass, List<string> keyData, List<string> valueData, LuaFunction callback)
    {
        // In các tham số nhận được từ Lua
        Debug.Log("Method: " + method);
        Debug.Log("Class: " + nameClass);

        // Tạo JSON gửi đi
        JSONClass datasend = new JSONClass();
        datasend["class"] = method;
        datasend["method"] = nameClass;

        for (int i = 0; i < keyData.Count; i++)
        {
            Debug.Log("Key: " + keyData[i] + ", Value: " + valueData[i]);
            datasend["data"][keyData[i]] = valueData[i];
        }

        // Gọi NetworkManager để gửi dữ liệu (giả sử SendServer có callback)
       
        NetworkManager.ins.SendServerLua(datasend, (response) =>
        {

            // Sau khi nhận được phản hồi từ server, gọi callback Lua
            callback?.Call(response) ;
        });

    }

    public void CallBack(string response)
    {
        // Debug.Log("Received callback with response: " + response);
    }
    
}
[LuaCallCSharp]
public class XluaJSONNode
{
    public string str(JSONNode json,string key)
    {
        Debug.Log("Get value by key " + json.ToString());
        Debug.Log("key: " + key);
        return json[key].AsString;
    }



    public string str(JSONNode json, int index)
    {
        Debug.Log("Get value by index " + json.ToString());
        Debug.Log("index: " + index);
        return json[index].AsString;
    }

    public string str(JSONNode json, List<string> key)
    {
        JSONNode json2 = null;
        for (int i = 0; i < key.Count; i++)
        {
             json2 = json[key[i]];
        }
        Debug.Log("Get value by list " + json.ToString());
        return json2.AsString;
    }
    public JSONNode json(JSONNode json, string key)
    {
        json = JSON.Parse(json.ToString());
        Debug.Log("Get value by key " + json.ToString());
        Debug.Log("key: " + key);
        return json[key];
    }

    public JSONNode json(JSONNode json, int index)
    {
        json = JSON.Parse(json.ToString());
        Debug.Log("Get value by index " + json.ToString());
        Debug.Log("index: " + index);
        return json[index];
    }
    public string tostring(JSONNode json)
    {
        return json.ToString();
    }
}