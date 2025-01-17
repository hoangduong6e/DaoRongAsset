
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public partial class MenuEventHalloween2024 : EventManager
{
    [Header("----Giao diện yểm bùa-----")]
    [SerializeField] private Transform[] allTxtAnim;

    private bool DuocYemBua = true;
    [Header("----Boss Halloween-----")]

    [SerializeField] private Transform PanelXemDanhBoss;

    public bool isKichHoatGiamSucManh;
    public void ExitYemBua()
    {
        AudioManager.PlaySound("soundClick");
        if (!DuocYemBua) return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        btnchon.gameObject.SetActive(false);
    }
    public void YemBua()
    {
        AudioManager.PlaySound("soundClick");
        if (!DuocYemBua) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] =nameEvent;
        datasend["method"] = "YemBua";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {

        
                DuocYemBua = false;
                debug.Log(json.ToString());

                Transform PanelYemBua = transform.Find("PanelYemBua");
                Animator anim = PanelYemBua.transform.GetChild(0).transform.Find("animBua").GetComponent<Animator>();
                anim.Play("anim");
                GameObject alleff = PanelYemBua.transform.GetChild(0).transform.Find("alleff").gameObject;
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.4f);
                    alleff.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.6f);
                    alleff.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.3f);
                    for (int i = 0; i < 3; i++)
                    {
                        allTxtAnim[i].gameObject.SetActive(true);
                        allTxtAnim[i].transform.GetChild(0).GetComponent<Text>().text = "+" + json["RandomCongBua"][i].AsString;
                    }

                    foreach (KeyValuePair<string, JSONNode> key in json["allitemUpdate"].AsObject)
                    {
                        SetItem(key.Key, key.Value.AsInt);
                    }

                    yield return new WaitForSeconds(0.3f);

                    DuocYemBua = true;

                    yield return new WaitForSeconds(1.3f);

                    for (int i = 0; i < 3; i++)
                    {
                        allTxtAnim[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
      
    }

    public void XemAiBoss()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemAiBoss";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                PanelXemDanhBoss.gameObject.SetActive(true);
                Transform g = PanelXemDanhBoss.transform.GetChild(0);
                g.transform.GetChild(1).GetComponent<Text>().text = "<color=orange>Boss Ma Đạo ải "+ (aiDangChon + 1) +"</color>";
                Transform all = g.transform.Find("all");
                g.transform.Find("txtluotdanh").GetComponent<Text>().text = json["txtluotdanh"].AsString;
                for (int i = 0; i < 3; i++)
                {
                    Image imgsao = all.transform.GetChild(i).GetComponent<Image>();
                    all.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["allDieuKienSao"][i].AsString;
                    Image imgQua = imgsao.transform.Find("qua").GetComponent<Image>();
                    Text txtsoluong = imgQua.transform.GetChild(0).GetComponent<Text>();
                    txtsoluong.text = (json["allQua"][i]["loaiitem"].AsString != "rong") ? json["allQua"][i]["soluong"].AsString: json["allQua"][i]["sao"].AsString + " sao";
                    imgQua.sprite = GetSpriteAll(json["allQua"][i]["name"].AsString, (LoaiItem)Enum.Parse(typeof(LoaiItem), json["allQua"][i]["loaiitem"].AsString, true));
                    imgQua.SetNativeSize();
                    GamIns.ResizeItem(imgQua,100);
                    Button btnNhan = imgsao.transform.GetChild(3).GetComponent<Button>();


                    btnNhan.interactable = (json["allQuaAi"][i].AsString == "2")?true:false;
                    Text txt = btnNhan.transform.GetChild(0).GetComponent<Text>();
                    if (json["allQuaAi"][i].AsString == "1")
                    {
                        btnNhan.interactable = false;
                        txt.text = "Nhận";
                    }
                    else if (json["allQuaAi"][i].AsString == "2")
                    {
                        btnNhan.interactable = true;
                        txt.text = "Nhận";
                    }
                    else if (json["allQuaAi"][i].AsString == "3")
                    {
                        btnNhan.interactable = false;
                        txt.text = "<color=cyan>Đã nhận</color>";
                    }
                    if (json["allQuaAi"][i].AsInt >= 2)
                    {
                        imgsao.sprite = MenuEventHalloween2024.inss.GetSprite("ngoisao2");
                    }
                    else
                    {
                        imgsao.sprite = MenuEventHalloween2024.inss.GetSprite("ngoisao1");
                    }
                }
                g.transform.Find("panelBonus").transform.GetChild(0).GetComponent<Text>().text = json["BonusKhiChienDau"].AsString;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    public void ThamChien()
    {
        VienChinh.vienchinh.nameMapvao = "BossHalloween";
        LoadMap();
    }

    void LoadMap()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "DanhBossHalloween";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                PanelXemDanhBoss.gameObject.SetActive(false);
                debug.Log(json.ToString());
                VienChinh.vienchinh.chedodau = CheDoDau.Halloween;
                NetworkManager.ins.socket.Emit("DoiHinhDanh", JSONObject.CreateStringObject(VienChinh.vienchinh.nameMapvao + "/" + VienChinh.vienchinh.chedodau.ToString()));
              
                VienChinh.vienchinh.enabled = true;
                //   AllMenu.ins.DestroyMenu("MenuXacNhan");
                VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGTinhVan");
                gameObject.SetActive(false);

                isKichHoatGiamSucManh = json["isKichHoatGiamSucManh"].AsBool;

                PanelXemDanhBoss.gameObject.SetActive(false);
            }
            else if(json["status"].AsString == "2")
            {
                EventManager.OpenThongBaoChon(json["message"].AsString, MuaLuotDanh, "Mua");
            }    
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    void MuaLuotDanh()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "MuaLuotDanh";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                Transform g = PanelXemDanhBoss.transform.GetChild(0);
                g.transform.Find("txtluotdanh").GetComponent<Text>().text = json["txtluotdanh"].AsString;
                debug.Log(json.ToString());
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

    }
    public void NhanQuaAi()
    {
        AudioManager.PlaySound("soundClick");
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        int quachon = btnchon.transform.parent.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanQuaAiBoss";
        datasend["data"]["quachon"] = quachon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                Image imgQuaNhan = btnchon.transform.parent.transform.Find("qua").GetComponent<Image>();
                Button btnnhan = btnchon.transform.parent.transform.Find("btnnhan").GetComponent<Button>();
                btnnhan.interactable = false;
                btnnhan.transform.GetChild(0).GetComponent<Text>().text = "<color=cyan>Đã nhận</color>";
                GameObject quabay = Instantiate(imgQuaNhan.gameObject,transform.position,Quaternion.identity);
                quabay.transform.GetChild(0).gameObject.SetActive(false);
                quabay.transform.SetParent(CrGame.ins.trencung,false);
                quabay.transform.position = imgQuaNhan.transform.position;
                QuaBay bay = quabay.AddComponent<QuaBay>();
                bay.vitribay = btnHopQua;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

    }
    public void SuaDoiHinh()
    {
        AudioManager.PlaySound("soundClick");
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", CrGame.ins.trencung.gameObject, true);
    }

    private GameObject menumuaitem;

    string txtTv(string s)
    {
        switch (s)
        {
            case "BuaTrang": return "Bùa Trắng";
        }
        return "";
    }
    string nameitemmua;
    Text txtupdate;
    public void OpenMenuMuaXeng()
    {
        AudioManager.SoundClick();
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        txtupdate = btnchon.transform.parent.GetComponentsInChildren<Text>()[0];
        nameitemmua = btnchon.transform.parent.name;
        debug.Log("Xem itemmua " + nameitemmua);
        GameObject menu = Instantiate(Inventory.LoadObjectResource("GameData/" + nameEvent + "/MenuMuaItem"), transform.position, Quaternion.identity);
        menumuaitem = menu;
        menu.transform.SetParent(CrGame.ins.trencung.transform, false);
        menu.transform.position = CrGame.ins.trencung.transform.position;
        menu.SetActive(true);

        //    GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaItem", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        g.transform.Find("txttanggia").GetComponent<Text>().text = "Giá tăng khi mua nhiều trong ngày, sẽ được reset khi qua ngày mới.";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = txtTv(nameitemmua);// tên giao diện
        Image imgitem = g.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        imgitem.sprite = Resources.Load<Sprite>("GameData/" + nameEvent + "/" + nameitemmua);
        imgitem.SetNativeSize();
        Button btnsangtrai = btn.transform.GetChild(0).GetComponent<Button>();
        btnsangtrai.onClick.AddListener(delegate { CongThemSoLuongMua(-1); });
        Button btnsangPhai = btn.transform.GetChild(1).GetComponent<Button>();
        btnsangPhai.onClick.AddListener(delegate { CongThemSoLuongMua(1); });
        Button btnExit = g.transform.Find("btnExit").GetComponent<Button>();
        btnExit.onClick.AddListener(ExitMenuQueThu);
        Button btnXacNhan = g.transform.Find("btnXacNhan").GetComponent<Button>();
        btnXacNhan.onClick.AddListener(MuaQueThu);
        InputField input = g.transform.Find("InputField").GetComponent<InputField>();
        input.onEndEdit.AddListener(onEndEdit);
    }
    short soluongMuaQueThu = 1;
    private void CongThemSoLuongMua(int i)
    {
        AudioManager.SoundClick();
        debug.Log("Tang so luong " + i);
        if (soluongMuaQueThu + i >= 1)
        {
            GameObject menu = menumuaitem;
            Transform g = menu.transform.GetChild(0);
            InputField input = g.transform.Find("InputField").GetComponent<InputField>();
            soluongMuaQueThu += (short)i;
            XemGiaMuaQueThu();
            input.text = soluongMuaQueThu.ToString();
        }
    }
    private void onEndEdit(string s)
    {
        if (s == "" || s == "0") s = "1";
        if (s.Length > 4) s = "500";
        if (int.Parse(s) >= 500) s = "500";
        debug.Log("onEndEdit " + s);
        GameObject menu = menumuaitem;
        Transform g = menu.transform.GetChild(0);
        InputField input = g.transform.Find("InputField").GetComponent<InputField>();
        soluongMuaQueThu = short.Parse(s);
        XemGiaMuaQueThu();
        input.text = soluongMuaQueThu.ToString();
    }

    public void XemGiaMuaQueThu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemGiaMua";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = menumuaitem;
                if (menu != null)
                {

                    Transform g = menu.transform.GetChild(0);
                    Text txtgia = g.transform.Find("txtGia").GetComponent<Text>();
                    txtgia.text = json["gia"].AsString;



                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void MuaQueThu()
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "MuaXeng";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;

        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                if (menumuaitem != null)
                {
                    ExitMenuQueThu();
                    //  SetXeng(json["soxeng"].AsString);
                    CrGame.ins.OnThongBaoNhanh("Mua thành công!");
                    SetItem(nameitemmua, json["soitem"].AsInt);
                    //   if (txtupdate.transform.parent.transform.parent.name == "Item")
                    // {
                    //   string s = json["soitem"].AsString;
                    //   txtupdate.text = s;
                    //  }
                    //SetsucXac(json["sucxac"].AsString);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void ExitMenuQueThu()
    {
        Destroy(menumuaitem); soluongMuaQueThu = 1;
    }
}
