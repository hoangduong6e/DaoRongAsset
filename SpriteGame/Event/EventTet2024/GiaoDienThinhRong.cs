using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiaoDienThinhRong : MonoBehaviour
{
    Transform g;
    public void ParseData(JSONNode json)
    {
        g = transform.GetChild(0);
        SetTxtDaDoiRong(json["RongDaDoi"].AsString);
        GameObject contentItem = g.transform.Find("ScrollView").transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 0; i < contentItem.transform.childCount; i++)
        {
            contentItem.transform.GetChild(i).transform.Find("txtGioiHan").GetComponent<Text>().text = json["allQua"][i]["txtdadoi"].AsString;
            if (json["allQua"][i]["btn"].AsBool) contentItem.transform.GetChild(i).transform.Find("btnDoi").GetComponent<Button>().interactable = true;
            else contentItem.transform.GetChild(i).transform.Find("btnDoi").GetComponent<Button>().interactable = false;
        }
        SetLenhBaiCo(json["LenhBai"].AsString);
    }    
    private void SetTxtDaDoiRong(string dadoi)
    {
        if(dadoi == "0") g.transform.Find("txtDaDoiRong").GetComponent<Text>().text = "Đã đổi\r\n<color=lime>0/1</color>";
        else g.transform.Find("txtDaDoiRong").GetComponent<Text>().text = "Đã đổi\r\n<color=red>"+ dadoi + "/1</color>";
    }
    private void SetLenhBaiCo(string solenhbai)
    {
        g.transform.Find("txtSoLenhBaiCo").GetComponent<Text>().text = "Đang có: " + solenhbai;
    }    
    public void DoiQua()
    {
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "DoiQuaGiaoDienThinhRong";
        datasend["data"]["keyqua"] = tf.gameObject.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "ok")
            {
                ParseData(json);
                CrGame.ins.OnThongBaoNhanh("Đã đổi!");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }    
    public void CloseMenu()
    {
        EventManager.ins.DestroyMenu("GiaoDienThinhRong");
        //gameObject.SetActive(false);
    }    
}