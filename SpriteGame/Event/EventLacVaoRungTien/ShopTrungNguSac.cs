using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTrungNguSac : MonoBehaviour
{
    Transform g;
    string nameEvent = "EventTet2024";
    public void ParseData(JSONNode json)
    {
        debug.Log(json.ToString());
        GameObject Content = transform.GetChild(0).transform.Find("ScrollView").transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject item = Content.transform.GetChild(0).gameObject;
        g = transform.GetChild(0);
        foreach (KeyValuePair<string, JSONNode> key in json["AllQuaThinhRong"].AsObject)
        {
            GameObject ins = Instantiate(item, transform.position, Quaternion.identity);
            ins.transform.GetChild(0).gameObject.name = key.Value["nametv"].AsString;
            ins.transform.SetParent(Content.transform, false);
            ins.name = key.Key;
            Image imgitem = ins.transform.GetChild(3).GetComponent<Image>();
            imgitem.name = key.Value["nameitem"].AsString;
            if (key.Value["loaiitem"].AsString == "Item")
            {
                ins.transform.GetChild(2).GetComponent<Text>().text = "x" + key.Value["soluong"].AsString;
                imgitem.sprite = Inventory.LoadSprite(key.Value["nameitem"].AsString);
                ins.transform.GetChild(4).GetComponent<Text>().text = key.Value["txtdadoi"].AsString;
              
                GamIns.ResizeItem(imgitem, 77);
                if(!key.Value["nameitem"].AsString.Contains("LongVan")) ins.transform.GetChild(1).gameObject.SetActive(false);//vienvang
            }
            else if (key.Value["loaiitem"].AsString == "ItemEvent")
            {
                ins.transform.GetChild(2).GetComponent<Text>().text = "x" + key.Value["soluong"].AsString;
                imgitem.sprite = EventManager.ins.GetSprite(key.Value["nameitem"].AsString);
                ins.transform.GetChild(4).GetComponent<Text>().text = key.Value["txtdadoi"].AsString;
                ins.transform.GetChild(1).gameObject.SetActive(false);//vienvang
                GamIns.ResizeItem(imgitem, 77);
            }
            else if (key.Value["loaiitem"].AsString == "ItemRong")
            {
                ins.transform.GetChild(2).GetComponent<Text>().text = key.Value["sao"].AsString + " sao";
                imgitem.sprite = Inventory.LoadSpriteRong(key.Value["nameitem"].AsString + 1);
                ins.transform.GetChild(4).GetComponent<Text>().text = key.Value["txtdadoi"].AsString;
                GamIns.ResizeItem(imgitem, 80);
            }
            else if (key.Value["loaiitem"].AsString == "Avatar")
            {

                Friend.ins.LoadImage("avt", key.Value["nameitem"].AsString, imgitem);

                ins.transform.GetChild(2).GetComponent<Text>().text = " ";
            
                ins.transform.GetChild(4).GetComponent<Text>().text = key.Value["txtdadoi"].AsString;
            }
            Button btndoi = ins.transform.Find("btnDoi").GetComponent<Button>();
            if (key.Value["btn"].AsBool) btndoi.interactable = true;
            else btndoi.interactable = false;
            btndoi.transform.GetChild(1).GetComponent<Text>().text = key.Value["giaLenhBai"].AsString;
            imgitem.SetNativeSize();
           
            ins.gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
        //g = transform.GetChild(0);
        SetTxtDaDoiRong(json["RongDaDoi"].AsString);
     
        SetLenhBaiCo(json["LenhBai"].AsString);
    }
    private void SetTxtDaDoiRong(string dadoi)
    {
        if (dadoi == "0") g.transform.Find("txtDaDoiRong").GetComponent<Text>().text = "Giới hạn quà Rồng đã đổi <color=lime>0/1</color>";
        else g.transform.Find("txtDaDoiRong").GetComponent<Text>().text = "Giới hạn quà Rồng đã đổi <color=red>" + dadoi + "/1</color>";
    }
    private void SetLenhBaiCo(string solenhbai)
    {
        g.transform.Find("txtSoLenhBaiCo").GetComponent<Text>().text = "Đang có: " + solenhbai;
    }
    public void DoiQua()
    {
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        EventManager.OpenThongBaoChon("Tiêu hao <color=yellow>" + tf.transform.Find("btnDoi").transform.GetChild(1).GetComponent<Text>().text + "</color> Lệnh bài để đổi <color=yellow>" + tf.transform.GetChild(0).name + "</color>?", XacNhan);
    void XacNhan()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "DoiQuaGiaoDienThinhRong";
            datasend["data"]["keyqua"] = tf.gameObject.name;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "ok")
                {
                    //  ParseData(json);
                    tf.transform.GetChild(4).GetComponent<Text>().text = json["txtdadoi"].AsString;

                    Button btndoi = tf.transform.Find("btnDoi").GetComponent<Button>();
                    if (json["btn"].AsBool) btndoi.interactable = true;
                    else btndoi.interactable = false;
                    CrGame.ins.OnThongBaoNhanh("Đã đổi!");

                    SetTxtDaDoiRong(json["RongDaDoi"].AsString);

                    SetLenhBaiCo(json["LenhBai"].AsString);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
            }
        }

    }
    public void CloseMenu()
    {
        EventManager.ins.DestroyMenu("ShopLenhBai");
        //gameObject.SetActive(false);
    }
}