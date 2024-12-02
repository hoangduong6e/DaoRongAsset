using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class MenuEventHalloween2024 : EventManager
{
    [SerializeField]
    private GameObject PanelItemYeuCau, PanelQua;

    private Transform KhungBua;

    private bool SetPanelQua
    {
        set
        {
            if(!value)
            {
                for (int i = 0; i < PanelQua.transform.childCount; i++)
                {
                    PanelQua.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            PanelQua.SetActive(value);
        }
    }
    protected override void ABSAwake()
    {
        KhungBua = transform.Find("KhungBua");
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

        foreach (KeyValuePair<string, JSONNode> key in json["data"]["allItem"].AsObject)
        {
            SetItem(key.Key,key.Value.AsInt);
        }

        for (int i = 0; i < json["data"]["Da"].Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Transform Da = transform.Find(j.ToString());
                Transform da = Da.transform.Find("Da_" + json["data"]["Da"][i].AsString);
                if(da != null)
                {
                    da.gameObject.SetActive(true);
                    break;
                }
               
            }
         
        }
    }
    private bool xemItem = false;
    public void XemItemYeuCau(bool xem)
    {
        xemItem = xem;
        if(!xemItem)
        {
            PanelItemYeuCau.SetActive(xemItem);
            return;
        }    
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        debug.Log("Đá chọn là: " + btnchon.transform.parent.name);
        PanelItemYeuCau.transform.position = new Vector3(btnchon.transform.position.x, btnchon.transform.position.y, PanelItemYeuCau.transform.position.z);

        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemYeuCau";
        datasend["data"]["DaChon"] = btnchon.transform.parent.transform.parent.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                PanelItemYeuCau.SetActive(xemItem);

                for (int i = 0;i < 3; i++)
                {
                    PanelItemYeuCau.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["data"][i].AsString;
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    public void XemQua(bool xem)
    {
        xemItem = xem;
        if (!xemItem)
        {
            SetPanelQua = xemItem;
            return;
        }
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        debug.Log("quà chọn là: " + btnchon.name);
        PanelQua.transform.position = new Vector3(btnchon.transform.position.x, btnchon.transform.position.y, PanelQua.transform.position.z);


        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemQua";
        datasend["data"]["QuaChon"] = btnchon.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                SetPanelQua = xemItem;

                for (int i = 0; i < json["data"].Count; i++)
                {
                    PanelQua.transform.GetChild(i).gameObject.SetActive(true);
                    LoaiItem loai = (LoaiItem)Enum.Parse(typeof(LoaiItem), json["data"][i]["loaiitem"].AsString, true);
                    Image img = PanelQua.transform.GetChild(i).GetComponent<Image>();
                    img.sprite = GetSpriteAll(json["data"][i]["name"].AsString, loai);
                    img.SetNativeSize();
                    GamIns.ResizeItem(img,100);
                    img.transform.GetChild(0).GetComponent<Text>().text = (loai != LoaiItem.rong)? json["data"][i]["soluong"].AsString: json["data"][i]["sao"].AsString + " sao";
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    [SerializeField]
    private GameObject MenuMoPhongAn;
    public void XemMoPhongAn()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemMoPhongAn";
        datasend["data"]["DaChon"] = btnchon.transform.parent.transform.parent.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                MenuMoPhongAn.gameObject.SetActive(true);
                Transform g = MenuMoPhongAn.transform.GetChild(0);
                Transform allBua = g.transform.Find("allBua");
                for (int i = 0; i < 3; i++)
                {
                    allBua.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["yeucau"][i].AsString;
                }
                Transform allQua = g.transform.Find("allQua");
                for (int i = 0; i < allQua.transform.childCount; i++)
                {
                    allQua.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < json["quaai"].Count; i++)
                {
                    allQua.transform.GetChild(i).gameObject.SetActive(true);
                    LoaiItem loai = (LoaiItem)Enum.Parse(typeof(LoaiItem), json["quaai"][i]["loaiitem"].AsString, true);
                    Image img = allQua.transform.GetChild(i).GetComponent<Image>();
                    img.sprite = GetSpriteAll(json["quaai"][i]["name"].AsString, loai);
                    img.SetNativeSize();
                    GamIns.ResizeItem(img, 100);
                    img.transform.GetChild(0).GetComponent<Text>().text = (loai != LoaiItem.rong) ? json["quaai"][i]["soluong"].AsString : json["quaai"][i]["sao"].AsString + " sao";
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

    }    
    private void SetItem(string name, int soluong)
    {
        switch(name)
        {
            case "BuaTrang":
                KhungBua.transform.Find(name).transform.GetChild(0).GetComponent<Text>().text = (soluong < 1)?"<color=red>" + soluong +"/1</color>": "<color=lime>" + soluong + "/1</color>";
                break;
            case "BuaXanh":
            case "BuaVang":
            case "BuaDo":
                KhungBua.transform.Find(name).transform.GetChild(0).GetComponent<Text>().text = soluong.ToString();
                break;
            default: break;
        }
    }
}
