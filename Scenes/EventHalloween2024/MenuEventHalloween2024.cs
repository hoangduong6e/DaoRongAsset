using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;
using Unity.VisualScripting;

public class MenuEventHalloween2024 : EventManager
{
    [SerializeField]
    private GameObject PanelItemYeuCau, PanelQua, Eff2, quabay;

    private Transform KhungBua;

    [SerializeField]
    private Transform[] allDa;

    [SerializeField]
    private Sprite Sprite1, Sprite2;
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
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        btnHopQua.transform.SetParent(transform);

        debug.Log("data event: " + json.ToString());

        foreach (KeyValuePair<string, JSONNode> key in json["data"]["allItem"].AsObject)
        {
            SetItem(key.Key,key.Value.AsInt);
        }

        for (int i = 0; i < json["data"]["Da"].Count; i++)
        {
            if (json["data"]["Da"][i]["phongan"].AsBool) // nếu đang phong ấn
            {
                if(json["data"]["Da"][i]["name"].AsString != "cot")
                {
                    Transform Da = transform.Find(i.ToString());
                    Transform da = allDa[json["data"]["Da"][i]["name"].AsInt];

                    da.gameObject.SetActive(true);
                    da.transform.SetParent(Da);
                    da.transform.position = new Vector3(Da.transform.position.x, da.transform.position.y, da.transform.position.z);
                    if(i == 0)
                    {
                        transform.Find("QuaThuong").GetComponent<Image>().sprite = GetSprite("QuaThuongToi");
                    }
                    else transform.Find("QuaHiem").GetComponent<Image>().sprite = GetSprite("QuaHiemToi");
                }
                else // nếu đây là cột
                {
                    Transform cot = transform.Find("2").transform.GetChild(0);
                    cot.transform.GetChild(2).gameObject.SetActive(false);// tắt lửa đi
                    cot.transform.GetChild(3).gameObject.SetActive(true);// bật nút giải phong ấn lên
                    transform.Find("QuaCucHiem").GetComponent<Image>().sprite = GetSprite("QuaCucHiemToi");
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
    private string DaChon;
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
                DaChon = btnchon.transform.parent.transform.parent.name;
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
    public void MoPhongAn()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "MoPhongAn";
        datasend["data"]["DaChon"] = DaChon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                MenuMoPhongAn.gameObject.SetActive(false);
                foreach (KeyValuePair<string, JSONNode> key in json["updateitem"].AsObject)
                {
                    SetItem(key.Key, key.Value.AsInt);
                }
                if(json["namedachon"].AsString != "cot")
                {
                    Transform dachon = allDa[json["namedachon"].AsInt];

                    transform.Find(json["nameQua"].AsString).GetComponent<Image>().sprite = GetSprite(json["nameQua"].AsString + "Sang");
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        Animator anim = dachon.transform.GetChild(0).GetComponent<Animator>();
                        Eff2.transform.position = new Vector3(dachon.transform.position.x, Eff2.transform.position.y, Eff2.transform.position.z);
                        Eff2.SetActive(true);
                        yield return new WaitForSeconds(0.5f);
                        anim.enabled = true;
                        yield return new WaitForSeconds(0.1f);
                        Eff2.SetActive(false);
                        yield return new WaitForSeconds(0.2f);
                    
                        anim.enabled = false;
                        dachon.gameObject.SetActive(false);
                        Transform tfqua = transform.Find(json["nameQua"].AsString);
                        for (int i = 0; i < json["quaAi"].Count; i++)
                        {
                            GameObject inss = Instantiate(quabay, transform.position, Quaternion.identity);
                            inss.transform.SetParent(transform, false);
                            Image img = inss.GetComponent<Image>();
                            PanelQua.transform.GetChild(i).gameObject.SetActive(true);
                            LoaiItem loai = (LoaiItem)Enum.Parse(typeof(LoaiItem), json["quaAi"][i]["loaiitem"].AsString, true);
                            img.sprite = GetSpriteAll(json["quaAi"][i]["name"].AsString, loai);
                            img.SetNativeSize();
                            GamIns.ResizeItem(img, 120);
                            inss.transform.position = tfqua.transform.position;
                            inss.gameObject.SetActive(true);
                            yield return new WaitForSeconds(0.3f);
                            QuaBay qbay = inss.AddComponent<QuaBay>();
                            qbay.enabled = false;
                            qbay.vitribay = btnHopQua;
                            qbay.enabled = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                   // dachon.gameObject.SetActive(false);
                }    
                else // nếu đây là cột
                {
                    Transform cot = transform.Find("2").transform.GetChild(0);
                    cot.transform.GetChild(2).gameObject.SetActive(true);// bật lửa lên
                    cot.transform.GetChild(3).gameObject.SetActive(false);// tắt nút giải phong ấn

                    transform.Find("2").transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
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
