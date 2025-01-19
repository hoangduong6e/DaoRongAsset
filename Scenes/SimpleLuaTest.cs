using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

public class SimpleLuaTest : MonoBehaviour
{
    private LuaEnv luaEnv; // Môi trường Lua
 //      private string url = "https://daorongmobile.online/test/testlua.txt";
    // Phương thức nhận tham số từ Lua
    public void SendRequestLua(string method, string nameClass, List<string> keyData, List<string> valueData, LuaFunction callback)
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
        NetworkManager.ins.SendServer(datasend, (response) => 
        {
            // Sau khi nhận được phản hồi từ server, gọi callback Lua
            callback?.Call(response.ToString());
        });
     
    }

    // Hàm callback này sẽ được gọi từ Lua sau khi xử lý
    public void CallBack(string response)
    {
       // Debug.Log("Received callback with response: " + response);
    }

    private void Start()
    {
        EventManager.StartDelay2(()=>{

            StartCoroutine(LoadText());
            },5f);
        
    }

    private void OnDestroy()
    {
        // Dọn dẹp LuaEnv
        luaEnv.Dispose();
    }


     // StartCoroutine cho Lua gọi
    public void StartLuaCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
    
   IEnumerator LoadText()
    {
        // Gửi yêu cầu GET tới URL
        UnityWebRequest www = UnityWebRequest.Get(LoginFacebook.http + "://" + LoginFacebook.ins.ServerChinh + "/Lua/testDelay.lua");

        // Đợi yêu cầu hoàn thành
        yield return www.SendWebRequest();

        // Kiểm tra lỗi
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Lỗi khi tải tệp: " + www.error);
        }
        else
        {
            // Lấy nội dung của tệp văn bản
            string text = www.downloadHandler.text;
            Debug.Log("Nội dung tệp tải về: " + text);
 
                          luaEnv = new LuaEnv(); // Khởi tạo môi trường Lua

        // Đăng ký phương thức SendRequestLua và CallBack để Lua có thể gọi
        luaEnv.Global.Set("SendRequestLua", (System.Action<string, string, List<string>, List<string>, LuaFunction>)SendRequestLua);
        luaEnv.Global.Set("CallBack", (System.Action<string>)CallBack);
         luaEnv.Global.Set("StartDeLay2", (Action<Action,float>)EventManager.StartDelay2);
     //   luaEnv.Global.Set("OnThongBaoNhanh", (System.Action<string,float,bool>)CrGame.ins.OnThongBaoNhanh);

        // Đoạn script Lua gọi phương thức C#



        //luaEnv.DoString(@"
        //    print('Calling C# method from Lua...')

        //    -- Dữ liệu mẫu để truyền vào C#
        //    local method = 'Main'
        //    local nameClass = 'XemItemTrangTri'
        //    local keyData = {'nametrangtri', 'key2'}
        //    local valueData = {'DoiNguSac', 'value2'}

        //    -- Định nghĩa callback Lua
        //    local function myCallback(response)
        //        print('Lua received callback response: ' .. response)
        //    end

        //    -- Gọi phương thức SendRequestLua trong C#, và truyền callback Lua vào
        //    SendRequestLua(method, nameClass, keyData, valueData, myCallback)
        //");

                 luaEnv.DoString(text);
            // Làm gì đó với nội dung, ví dụ in ra Console
        }
    }

}