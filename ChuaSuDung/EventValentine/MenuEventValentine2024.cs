using SimpleJSON;
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuEventValentine2024 : EventManager
{
    // Start is called before the first frame update
    public GameObject queTinhYeu;
    private Transform giaodien1;
    protected override void ABSAwake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseData(JSONNode json)
    {
        giaodien1 = transform.GetChild(0);
        debug.Log(json.ToString());
        SetQueTinhYeu(json["data"]["quetinhyeu"].AsString);
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        btnHopQua.transform.SetParent(CrGame.ins.trencung.transform);
        Transform alltrung = giaodien1.transform.Find("allTrung");
        bool nhankhongtuoc = json["data"]["nhankhongtuoc"].AsBool;
        for (int i = 1; i <= 8; i++)
        {
            int j = i - 1;
       
            if (json["data"]["AllTrung"]["trung"+i].AsInt >= 20)
            {
               // debug.Log(alltrung.transform.GetChild(j).name);
                Animator anim = alltrung.transform.GetChild(j).transform.GetChild(0).GetComponent<Animator>();
                anim.Play("trungdavo");
                if(!nhankhongtuoc) alltrung.transform.GetChild(j).transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        GameObject trungden = giaodien1.transform.Find("TrungDen").gameObject;
        if (nhankhongtuoc)
        {
            giaodien1.transform.Find("KhongTuoc").gameObject.SetActive(false);
            trungden.SetActive(true);
        }
        trungden.transform.GetChild(2).GetComponent<Text>().text = json["data"]["soVongTrungDen"].AsString;
        giaodien1.transform.Find("txtTimeEvent").GetComponent<Text>().text = json["TimeKetThucEvent"].AsString;
    }    
    private void SetQueTinhYeu(string s)
    {
        giaodien1.transform.Find("InfoQueTinhYeu").transform.GetChild(1).GetComponent<Text>().text = "Mỗi lần đập cần <color=yellow>10</color>\r\nBạn đang có: <color=lime>"+s+"</color>";
    }
    protected override void DiemDanhOk(JSONNode json)
    {
        SetQueTinhYeu(json["quetinhyeu"].AsString);
    }
    public void ClickTrung()
    {
        if (queTinhYeu.activeSelf || !duocdaptrung) return;

        GameObject btnchon = EventSystem.current.currentSelectedGameObject.gameObject;
        string namechon = btnchon.transform.parent.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "DapTrung";
        datasend["data"]["nametrung"] = namechon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                debug.Log("thanh congggg");
                SetQueTinhYeu(json["quetinhyeu"].AsString);
                queTinhYeu.transform.position = new Vector3(btnchon.transform.position.x, btnchon.transform.position.y);
                queTinhYeu.SetActive(true);
                GameObject animChucPhuc = giaodien1.transform.Find("animChucPhuc").gameObject;
                animChucPhuc.transform.position = queTinhYeu.transform.position;
                StartDelay(() => {
                    animChucPhuc.SetActive(true);
                }, 0.65f);
                StartDelay(() => { 
                    queTinhYeu.SetActive(false);
                }, 0.7f);
                StartDelay(() => {
                    animChucPhuc.SetActive(false);
                }, 1f);
                if (json["sotrungdadap"].AsInt >= 20 && namechon != "KhongTuoc" && namechon != "TrungDen")
                {
                    btnchon.GetComponent<Animator>().Play("trungvo");
                    StartDelay(() =>
                    {
                        GameObject longg = btnchon.transform.parent.transform.GetChild(1).gameObject;
                        longg.transform.localScale = Vector3.zero;


                        //  SpriteRenderer sprd = longg.GetComponent<SpriteRenderer>();
                        //   sprd.color = new Color(255, 255, 255, 0);
                        //   SpriteRenderer sprd1 = longg.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        //   sprd1.color = new Color(255, 255, 255, 0);

                        longg.SetActive(true);
                        longg.LeanScale(new Vector3(45.81818f, 45.81818f, 1), 0.3f);
                        // Color newColor = new Color(255, 255, 255, 255);
                        // LeftColor(sprd, newColor,5f);
                        //  LeftColor(sprd1, newColor,5f);
                        //  sprd.color = Color.Lerp(sprd.color, new Color(255, 255, 255, 255), 2f * Time.deltaTime);
                        // sprd1.color = Color.Lerp(sprd1.color, new Color(255, 255, 255, 255), 2f * Time.deltaTime);
                    }, 1.2f);

                }
                else if (namechon == "TrungDen")
                {
                    GameObject trungden = giaodien1.transform.Find("TrungDen").gameObject;
                    Text txt = trungden.transform.GetChild(2).GetComponent<Text>();
                    if (txt.text != json["soVongTrungDen"].AsString)
                    {
                        txt.text = json["soVongTrungDen"].AsString;
                        Vector3 vectorzin = txt.transform.localScale;
                        txt.transform.LeanScale(new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f), 0.3f);
                        StartDelay(() => { txt.transform.LeanScale(vectorzin, 0.3f); }, 0.5f);
                        OpenMenuNhanDuoc("Chocolate","X20 Chocolate",LoaiItem.item);
                    }
                }
            }
            else if (json["status"].Value == "2")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                   
                    btnchon.GetComponent<Button>().enabled = false;
                    GameObject LongBay = GetCreateMenu("LongBay", transform);
                    yield return new WaitForSeconds(0.2f);
                    Transform alltrung = giaodien1.transform.Find("allTrung");
                    for (int i = 0; i < alltrung.transform.childCount; i++)
                    {
                        alltrung.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                    }
                    yield return new WaitForSeconds(5.4f);
                    Destroy(LongBay);
                    Transform KhongTuoc = giaodien1.transform.Find("KhongTuoc");
                    KhongTuoc.transform.LeanScale(Vector3.zero, 0.3f);
                    OpenMenuNhanDuoc("HuyHieuKhongTuoc","<color=cyan>Huy Hiệu Khổng Tước</color>",LoaiItem.itemevent);
                    StartDelay(() => {
                        Transform trungden = giaodien1.transform.Find("TrungDen");
                        trungden.transform.localScale = Vector3.zero;

                        trungden.gameObject.SetActive(true);
                        trungden.transform.LeanScale(Vector3.one, 0.3f);


                    }, 0.3f);

                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
          
            }    
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }


    }
    private string nametrungchon;
    private short idtrung;
    private bool duocdaptrung = true;

    public void PointerDownXemTrung()
    {
        if (queTinhYeu.activeSelf) return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject.gameObject;
        string namechon = btnchon.transform.parent.name;
        nametrungchon = namechon;
        StartDelay(XemTrung, 0.2f);
        void XemTrung()
        {
            if (nametrungchon == namechon)
            {
               // debug.Log("Chon " + namechon);
                JSONClass datasend = new JSONClass();
                datasend["class"] = nameEvent;
                datasend["method"] = "GetThongTinTrung";
                datasend["data"]["nametrung"] = namechon;
                NetworkManager.ins.SendServer(datasend.ToString(), Ok,true);
                void Ok(JSONNode json)
                {
                    if(nametrungchon == namechon)
                    {
                        CrGame.ins.OnThongBaoNhanh(json["info"].AsString, 2, false);
                        idtrung = (short)json["info"].AsString.Length;
                        duocdaptrung = false;
                    }
               
                }
            }
        }
    }
    public void PointerUpXemTrung()
    {
        if (queTinhYeu.activeSelf) return;
        CrGame.ins.OffThongBaoNhanh(idtrung);
        nametrungchon = "";
        StartDelay(() => { duocdaptrung = true; },0.1f);
        
    }

    short soluongMuaQueThu = 1;
    public void OpenMenuMuaQueThu()
    {
        GameObject menu = GetCreateMenu("MenuMuaQueThuTinhYeu",CrGame.ins.trencung);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        Transform btn = g.transform.Find("btn");
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
    private void ExitMenuQueThu()
    {
        DestroyMenu("MenuMuaQueThuTinhYeu"); soluongMuaQueThu = 1;
    }
    private void CongThemSoLuongMua(int i)
    {
        debug.Log("Tang so luong " + i);
        if (soluongMuaQueThu + i >= 1)
        {
            GameObject menu = GetCreateMenu("MenuMuaQueThuTinhYeu");
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
        GameObject menu = GetCreateMenu("MenuMuaQueThuTinhYeu");
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
        datasend["method"] = "XemGiaMuaQueThu";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if(menuevent.ContainsKey("MenuMuaQueThuTinhYeu"))
                {
                    GameObject menu = menuevent["MenuMuaQueThuTinhYeu"];
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
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "MuaQueThu";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if (menuevent.ContainsKey("MenuMuaQueThuTinhYeu"))
                {
                    ExitMenuQueThu();
                    SetQueTinhYeu(json["quetinhyeu"].AsString);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenGiaoDienRuong()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDataRuong";
        datasend["data"]["nameruong"] = btnchon.transform.parent.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                // GameObject menu = CrGame.ins.trencung.transform.Find("GiaoDienRuongThanBi").gameObject;
                GameObject menu = GetCreateMenu("GiaoDienRuongThanBi", CrGame.ins.trencung.transform);
                menu.GetComponent<GiaoDienRuongThanBi>().ParseData(json, btnchon.transform.parent.name);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenMenuQuaThangSaoKhongTuoc()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetGiaoDienThangSaoKhongTuoc";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                GameObject menuQuaThangSao = GetCreateMenu("MenuNhanQuaThangSao", CrGame.ins.trencung, false);
                menuQuaThangSao.transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(ExitMenuQuaNangSao);
                GameObject ContentManh = menuQuaThangSao.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;

                for (int i = 0; i < json["quathangsaokhongtuoc"].Count; i++)
                {
                    Button btn = ContentManh.transform.GetChild(i).transform.Find("btnNhanQua").GetComponent<Button>();
                    if (json["quathangsaokhongtuoc"][i].AsString == "chuaduocnhan")
                    {
                        btn.interactable = false;
                    }
                    else if (json["quathangsaokhongtuoc"][i].AsString == "duocnhan")
                    {
                        btn.interactable = true;
                        btn.onClick.AddListener(Nhanquathangsaokhongtuoc);
                    }
                    else if (json["quathangsaokhongtuoc"][i].AsString == "danhan")
                    {
                        btn.interactable = false;
                        btn.transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
                    }
                }
                // menuQuaThangSao.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DoiManh);
                menuQuaThangSao.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void Nhanquathangsaokhongtuoc()
    {
        GameObject objchon = EventSystem.current.currentSelectedGameObject;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanQuaNhanKhongTuoc";
        datasend["data"]["vitri"] = objchon.transform.parent.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                // GameObject menuQuaThangSao = ev.menuevent["MenuNhanQuaThangSao"];
                //  GameObject ContentManh = menuQuaThangSao.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;

                Button btn = objchon.GetComponent<Button>();
                btn.interactable = false;
                btn.transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
                CrGame.ins.OnThongBaoNhanh("Đã nhận!");
                // menuQuaThangSao.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DoiManh);
                //menuQuaThangSao.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

    }
    private void ExitMenuQuaNangSao()
    {
        DestroyMenu("MenuNhanQuaThangSao");
    }
    public void OpenGiaoDienSucXac()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetGiaoDienSucXac";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                // GameObject menu = CrGame.ins.trencung.transform.Find("GiaoDienXucSac").gameObject;
                 GameObject menu = GetCreateMenu("GiaoDienXucSac", CrGame.ins.trencung.transform);
                menu.GetComponent<GiaoDienSucXac>().ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenMenuNhiemvu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetNhiemVu";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", CrGame.ins.trencung);
                GameObject AllNhiemVu = MenuNhiemVu.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                for (int i = 0; i < json["allNhiemvu"].Count; i++)
                {
                    Text txttiendo = AllNhiemVu.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    Text txtphanthuong = AllNhiemVu.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>();
                    if (int.Parse(json["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["allNhiemvu"][i]["maxnhiemvu"].Value))
                    {
                        txttiendo.text = "<color=#00ff00ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    txtphanthuong.text = json["allNhiemvu"][i]["qua"]["soluong"].AsString;
                }
            }
        }
    }


    public void VeNha()
    {
        AudioManager.PlaySound("soundClick");
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        // gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuEventValentine2024");
    }

}
