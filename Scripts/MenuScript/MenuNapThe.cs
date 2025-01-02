using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuNapThe : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField inputMaThe, inputSeri;
    public string MenhGia;
    public string NhaMang;
    public GameObject DauVNhaMang, DauVMenhGia;
    public void SelectNhaMang()
    {
        GameObject obj= UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Vector3 vec = obj.transform.position;
        vec.x += 0.1f;
        DauVNhaMang.transform.position = vec;
        DauVNhaMang.SetActive(true);
        NhaMang = obj.name;
    }
    public void SelectMenhGia()
    {
        GameObject obj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Vector3 vec = obj.transform.position; ;
        vec.x += 0.1f;
        DauVMenhGia.transform.position = vec;
        DauVMenhGia.SetActive(true);
        MenhGia = obj.name;
    }
    public void Nap()
    {
        CrGame.ins.OnThongBao(true, "Đang gửi thẻ lên hệ thống");
        StartCoroutine(Napp());
    }
    public IEnumerator Napp()
    {
        InfoThe xn = new InfoThe(LoginFacebook.ins.id,inputMaThe.text,inputSeri.text,NhaMang,MenhGia);
        string data = JsonUtility.ToJson(xn);
        var request = new UnityWebRequest(CrGame.ins.ServerName + "Napthe", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            debug.Log(request.error);
            CrGame.ins.OnThongBao(true, "Lỗi kết nối", true);
        }
        else
        {
            debug.Log(request.downloadHandler.text);
            CrGame.ins.OnThongBao(true, request.downloadHandler.text, true);
        }
    }
    public void GuiThe()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "NapThe";
        datasend["method"] = "GuiThe";
        datasend["data"]["telco"] = NhaMang;
        datasend["data"]["code"] = inputMaThe.text;
        datasend["data"]["serial"] = inputSeri.text;
        datasend["data"]["amount"] = MenhGia;
        CrGame.ins.panelLoadDao.SetActive(true);
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            CrGame.ins.OnThongBao(true, json["message"].AsString, true);
            CrGame.ins.panelLoadDao.SetActive(false);
        }
    }
}
[SerializeField]
public class InfoThe
{
    public string TaiKhoan,Mathe, Seri,NhaMang,MenhGia;
    public InfoThe(string taikhoan,string mathe,string seri,string nhamang,string menhgia)
    {
        TaiKhoan = taikhoan;
        Mathe = mathe;
        Seri = seri;
        NhaMang = nhamang;
        MenhGia = menhgia;
    }
}
