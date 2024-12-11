using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class MenuEventHalloween2024 : EventManager
{
    [SerializeField]
    private GameObject PanelItemYeuCau, PanelQua, Eff2, quabay, ThanhTienDo;

    private Transform KhungBua;

    [SerializeField]
    private Transform[] allDa;

    [SerializeField]
    private Sprite Sprite1, Sprite2;

    private byte AiDangChon;

    public byte aiDangChon {get{ return AiDangChon;} }
    public static MenuEventHalloween2024 inss;
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
        inss = this;
    }
    // Update is called once per frame
    void Update()
    {

    }
    protected override void DiemDanhOk(JSONNode json)
    {

          foreach (KeyValuePair<string, JSONNode> key in json["data"]["allitemUpdate"].AsObject)
        {
            SetItem(key.Key,key.Value.AsInt);
        }
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
        SetThanhTienDo(json["data"]["ThanhTienDo"]);
        Transform allAi = ThanhTienDo.transform.GetChild(2);

        for (int i = 0; i < allAi.transform.childCount; i++)
        {
            Transform child = allAi.transform.GetChild(i);
            child.transform.GetChild(2).GetComponent<Text>().text = (i + 1).ToString();
        }
         
        AiDangChon = json["data"]["AiDangChon"].AsByte;
        LoadAi(json["data"]["allAi"]);
    }
    private void SetThanhTienDo(JSONNode json)
    {
        Transform allAi = ThanhTienDo.transform.GetChild(2);
        Image fill = ThanhTienDo.transform.GetChild(1).GetComponent<Image>();

        for (int i = 0; i < json["Cong"].Count; i++)
        {
            Transform child = allAi.transform.GetChild(i);
            child.transform.GetChild(0).GetComponent<Image>().sprite = Sprite2;
            Image img = child.transform.GetChild(1).GetComponent<Image>();
            if(i == AiDangChon) img.sprite = GetSprite("aidango");
            else img.sprite = GetSprite(json["Cong"][i]["trangthai"].AsString);
            img.SetNativeSize();

            if (json["Cong"][i]["sao"].ToString() != "")
            {
                Transform allngoisao = child.transform.Find("allngoisao");
                for (int j = 0; j < json["Cong"][i]["sao"].AsInt; j++)
                {
                    allngoisao.transform.GetChild(j).GetComponent<Image>().sprite = GetSprite("ngoisao2");
                }
            }
        }

        fill.fillAmount = json["SoAiDaXong"].AsFloat / 20;
    }

    private void LoadAi(JSONNode json)
    {
        Transform allAi = ThanhTienDo.transform.GetChild(2);
        Transform child = allAi.transform.GetChild(AiDangChon);
        Image img = child.transform.GetChild(1).GetComponent<Image>();
        img.sprite = GetSprite("aidango");
        img.SetNativeSize();
        for (int i = 0; i < allDa.Length; i++)
        {
            allDa[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            if (json[i]["name"].AsString != "cot")
            {
                Transform Da = transform.Find(i.ToString());
                Transform da = allDa[json[i]["name"].AsInt];
                da.transform.GetChild(0).GetComponent<Image>().sprite = GetSprite("Da" + (json[i]["name"].AsInt + 1));
                if (json[i]["phongan"].AsBool) // nếu đang phong ấn
                {
                    da.gameObject.SetActive(true);
                    da.transform.SetParent(Da);
                    da.transform.position = new Vector3(Da.transform.position.x, da.transform.position.y, da.transform.position.z);
                    if (i == 0)
                    {
                        transform.Find("QuaThuong").GetComponent<Image>().sprite = GetSprite("QuaThuongToi");
                    }
                    else transform.Find("QuaHiem").GetComponent<Image>().sprite = GetSprite("QuaHiemToi");
                }
                else
                {
                    da.gameObject.SetActive(false);
                    if (i == 0)
                    {
                        transform.Find("QuaThuong").GetComponent<Image>().sprite = GetSprite("QuaThuongSang");
                    }
                    else transform.Find("QuaHiem").GetComponent<Image>().sprite = GetSprite("QuaHiemSang");
                }
            }
            else // nếu đây là cột
            {
                Transform cot = transform.Find("2").transform.GetChild(0);
                if (json[i]["phongan"].AsBool) // nếu đang phong ấn
                {
                    cot.transform.GetChild(2).gameObject.SetActive(false);// tắt lửa đi
                    cot.transform.GetChild(3).gameObject.SetActive(true);// bật nút giải phong ấn lên
                    transform.Find("QuaCucHiem").GetComponent<Image>().sprite = GetSprite("QuaCucHiemToi");
                }    
                else
                {
                    cot.transform.GetChild(2).gameObject.SetActive(true);
                    cot.transform.GetChild(3).gameObject.SetActive(false);
                    transform.Find("QuaCucHiem").GetComponent<Image>().sprite = GetSprite("QuaCucHiemSang");
                }    
            }
        }

        if (json["sao"].ToString() != "")
        {
            transform.Find("CongMaDao").gameObject.SetActive(true);
        }
        else transform.Find("CongMaDao").gameObject.SetActive(false);

        DuocQuaCong = true;
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
        //debug.Log("Đá chọn là: " + btnchon.transform.parent.name);
        PanelItemYeuCau.transform.position = new Vector3(btnchon.transform.position.x, btnchon.transform.position.y, PanelItemYeuCau.transform.position.z);
        string DaChon = btnchon.transform.parent.transform.parent.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemYeuCau";
        datasend["data"]["DaChon"] = DaChon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                PanelItemYeuCau.SetActive(xemItem);
                if (DaChon == "2") // nếu đây là cột
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Transform childi = PanelItemYeuCau.transform.GetChild(i);
                        childi.gameObject.SetActive(false);
                    }
                    for (int i = 0; i < json["data"].Count; i++)
                    {

                        Transform childi = PanelItemYeuCau.transform.GetChild(i);
                        LoaiItem loai = (LoaiItem)Enum.Parse(typeof(LoaiItem), json["data"][i]["loaiitem"].AsString, true);
                        Image img = childi.GetComponent<Image>();
                        img.sprite = GetSpriteAll(json["data"][i]["name"].AsString, loai);
                        img.SetNativeSize();
                        childi.transform.GetChild(0).GetComponent<Text>().text = json["data"][i]["soluong"].AsString;
                        childi.gameObject.SetActive(true);
                    }
                }   
                else // nếu đây là đá
                {
                    string[] bua = new string[] {"BuaXanh","BuaDo","BuaVang"};
                    for (int i = 0; i < 3; i++)
                    {
                        Transform childi = PanelItemYeuCau.transform.GetChild(i);
                        childi.GetComponent<Image>().sprite = GetSprite(bua[i]);
                        childi.transform.GetChild(0).GetComponent<Text>().text = json["data"][i].AsString;
                        childi.gameObject.SetActive(true);
                    }
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
                    string nameitem = json["data"][i]["name"].AsString;

                    Image img = PanelQua.transform.GetChild(i).GetComponent<Image>();
                    img.sprite = GetSpriteAll(nameitem, loai);
                    img.SetNativeSize();
                    GamIns.ResizeItem(img,100);
                    img.transform.GetChild(0).GetComponent<Text>().text = (loai != LoaiItem.rong)? json["data"][i]["nametv"].AsString: json["data"][i]["sao"].AsString + " sao";
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
                //for (int i = 0; i < 3; i++)
                //{
                //    allBua.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["yeucau"][i].AsString;
                //}

                if (DaChon == "2") // nếu đây là cột
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Transform childi = allBua.transform.GetChild(i);
                        childi.gameObject.SetActive(false);
                    }
                    for (int i = 0; i < json["yeucau"].Count; i++)
                    {

                        Transform childi = allBua.transform.GetChild(i);
                        LoaiItem loai = (LoaiItem)Enum.Parse(typeof(LoaiItem), json["yeucau"][i]["loaiitem"].AsString, true);
                        Image img = childi.GetComponent<Image>();
                        img.sprite = GetSpriteAll(json["yeucau"][i]["name"].AsString, loai);
                        img.SetNativeSize();
                        childi.transform.GetChild(0).GetComponent<Text>().text = json["yeucau"][i]["soluong"].AsString;
                        childi.gameObject.SetActive(true);
                    }
                }
                else // nếu đây là đá
                {
                    string[] bua = new string[] { "BuaXanh", "BuaDo", "BuaVang" };
                    for (int i = 0; i < 3; i++)
                    {
                        Transform childi = allBua.transform.GetChild(i);
                        childi.GetComponent<Image>().sprite = GetSprite(bua[i]);
                        childi.transform.GetChild(0).GetComponent<Text>().text = json["yeucau"][i].AsString;
                        childi.gameObject.SetActive(true);
                    }
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

                    string nameitem = json["quaai"][i]["name"].AsString;

          

                    img.sprite = GetSpriteAll(nameitem, loai);



                    img.SetNativeSize();
                    GamIns.ResizeItem(img, 100);
                    img.transform.GetChild(0).GetComponent<Text>().text = (loai != LoaiItem.rong) ? json["quaai"][i]["nametv"].AsString : json["quaai"][i]["sao"].AsString + " sao";
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
                DuocQuaCong = false;
                debug.Log(json.ToString());
                MenuMoPhongAn.gameObject.SetActive(false);
                foreach (KeyValuePair<string, JSONNode> key in json["updateitem"].AsObject)
                {
                    SetItem(key.Key, key.Value.AsInt);
                }
                transform.Find(json["nameQua"].AsString).GetComponent<Image>().sprite = GetSprite(json["nameQua"].AsString + "Sang");
                if (json["namedachon"].AsString != "cot")
                {
                    Transform dachon = allDa[json["namedachon"].AsInt];

                 
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
                            inss.transform.SetParent(CrGame.ins.trencung.transform, false);
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
                        DuocQuaCong = true;
                    }
                   // dachon.gameObject.SetActive(false);
                }    
                else // nếu đây là cột
                {
                    Transform cot = transform.Find("2").transform.GetChild(0);
           
                    cot.transform.GetChild(3).gameObject.SetActive(false);// tắt nút giải phong ấn

                    transform.Find("2").transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);

                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        Eff2.transform.position = new Vector3(cot.transform.position.x, Eff2.transform.position.y, Eff2.transform.position.z);
                        Eff2.SetActive(true);
                        yield return new WaitForSeconds(0.6f);
                        Eff2.SetActive(false);
                        cot.transform.GetChild(2).gameObject.SetActive(true);// bật lửa lên
                        Transform tfqua = transform.Find(json["nameQua"].AsString);

                        for (int i = 0; i < json["quaAi"].Count; i++)
                        {
                            GameObject inss = Instantiate(quabay, transform.position, Quaternion.identity);
                            inss.transform.SetParent(CrGame.ins.trencung.transform, false);
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
                        DuocQuaCong = true;
                    }
                }

                if (json["duocquaai"].AsBool)
                {
                    SetThanhTienDo(json["ThanhTienDo"]);
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

    private void TatSpriteDangMo()
    {
        Transform allAi = ThanhTienDo.transform.GetChild(2);
        Transform child = allAi.transform.GetChild(AiDangChon);
        Image img = child.transform.GetChild(1).GetComponent<Image>();
        img.sprite = GetSprite("aidamo");
        img.SetNativeSize();
    }
    private bool DuocQuaCong = true;
    public void ChonCong()
    {
        if (!DuocQuaCong) return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        if(btnchon.GetComponent<Image>().sprite.name == "aichuamo")
        {
            return;
        }
        int AiChon = btnchon.transform.parent.transform.GetSiblingIndex();
        if (AiDangChon == AiChon) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemAi";
        datasend["data"]["AiChon"] = AiChon.ToString();

        
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                DuocQuaCong = false;
                TatSpriteDangMo();

                debug.Log(json.ToString());
                AiDangChon = (byte)AiChon;
                LoadAi(json["dataAi"]);


            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }   
    public void QuaCong(int i)
    {
        if (!DuocQuaCong) return;
        int AiQua = AiDangChon + i;
        if (AiQua < 0) AiQua = 0;
        else if (AiQua > 19) AiQua = 19;

        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemAi";
        datasend["data"]["AiChon"] = AiQua.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                DuocQuaCong = false;
                debug.Log(json.ToString());
                TatSpriteDangMo();
                AiDangChon = json["Ai"].AsByte;
                LoadAi(json["dataAi"]);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    public static Action KetQua(KetQuaTranDau kq, bool quayve = false)
    {
        void kqq()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "EventHalloween2024";
            datasend["method"] = "KetQuaBoss";
            datasend["data"]["kq"] = kq.ToString();
            datasend["data"]["time"] = GiaoDienPVP.ins.TxtTime.text;



            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode json)
            {
                  debug.Log(json.ToString()) ;
                if (json["status"].AsString == "0")
                {

                    if (quayve)
                    {
                        CrGame.ins.OpenMenuRaKhoi();
                        return;
                    }
                    GiaoDienPVP.ins.menuWin.SetActive(true);
                    if (kq == KetQuaTranDau.Thua)
                    {
                        GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;
                        GiaoDienPVP.ins.thongtin.text = "Bạn đã bị đánh bại";
                    }
                    else
                    {
                        GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
                        GiaoDienPVP.ins.thongtin.text = "Bạn đã đánh bại Boss Halloween";
                        //CrGame.ins.OnThongBaoNhanh(json["infoqua"].AsString);
                    }
                    GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
                    GiaoDienPVP.ins.btnSetting.SetActive(true);
                    GiaoDienPVP.ins.spriteWin.SetNativeSize();

                    MenuEventHalloween2024.inss.PanelXemDanhBoss.gameObject.SetActive(true);
                    Transform g = MenuEventHalloween2024.inss.PanelXemDanhBoss.transform.GetChild(0);
                    Transform all = g.transform.Find("all");
                    for (int i = 0; i < 3; i++)
                    {
                        Image imgsao = all.transform.GetChild(i).GetComponent<Image>();
                     
                        Button btnNhan = imgsao.transform.GetChild(3).GetComponent<Button>();

                        btnNhan.interactable = (json["allQuaAi"][i].AsString == "2") ? true : false;

                        if(json["allQuaAi"][i].AsInt >= 2)
                        {
                            imgsao.sprite = MenuEventHalloween2024.inss.GetSprite("ngoisao2");
                        }
                        else
                        {
                            imgsao.sprite = MenuEventHalloween2024.inss.GetSprite("ngoisao1");
                        }
                    }
                    Transform allAi = MenuEventHalloween2024.inss.ThanhTienDo.transform.GetChild(2);
                    Transform child = allAi.transform.GetChild(MenuEventHalloween2024.inss.AiDangChon);


                    Transform allngoisao = child.transform.Find("allngoisao");
                    for (int j = 0; j < json["sao"].AsInt; j++)
                    {
                        allngoisao.transform.GetChild(j).GetComponent<Image>().sprite = MenuEventHalloween2024.inss.GetSprite("ngoisao2");
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
        return kqq;

    }
    public void OpenQuan3ConSoc()
    {
        Quan3ConSoc.OpenMenuQuan3ConSoc(1);
    }

    public void VeNha()
    {
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        AudioManager.PlaySound("soundClick");

        Transform dangodao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");

        Vector3 vec = dangodao.transform.position;
        vec.z = -10;
        CrGame.ins.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        //    gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuEventHalloween2024");
        //     Destroy(gameObject);
    }
}
