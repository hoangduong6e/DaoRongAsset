using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.EventSystems;
//using static UnityEditor.Progress;

public class Event7VienNgocRong2023 : EventManager
{
    // Start is called before the first frame update
    public Text[] txtallItem;
    public Sprite[] alltuchat;
    public static Event7VienNgocRong2023 inss;
    public Sprite[] allItemEvent;
    public Sprite[] allCapsuleSprite;
    public Sprite sprite1, sprite2, chuaduocnhan, danhan, duocnhan;
    public RuntimeAnimatorController VienDo, VienVang;
    string nameitemmua;
    void Start()
    {
        inss = this;
    }
    protected override void ABSAwake()
    {

    }
    public void HuyCapsule()
    {
        EventManager.OpenThongBaoChon("Hủy chọn không mất Capsule nhưng mảnh ghép sẽ bị reset", Huy);
        void Huy()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = EventManager.ins.nameEvent;
            datasend["method"] = "HuyCapsule";
            NetworkManager.ins.SendServer(datasend, Ok, true);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
                    GameObject KhungCapsuleChon = giaodien2.transform.Find("KhungCapsuleChon").gameObject;
                    KhungCapsuleChon.transform.GetChild(0).gameObject.SetActive(true);
                    KhungCapsuleChon.transform.GetChild(1).gameObject.SetActive(false);
                  


                    GameObject gdchoncapsule = giaodien2.transform.Find("GiaoDienChonCapsule").gameObject;

                    GameObject KhungCapsule = gdchoncapsule.transform.Find("KhungCapsule").gameObject;

                    Text txtnamecsl = gdchoncapsule.transform.Find("ttxnamecapsule").GetComponent<Text>();
                    txtnamecsl.gameObject.SetActive(false);
                    KhungCapsule.gameObject.SetActive(false);

                    GameObject gdchoncs = gdchoncapsule.transform.Find("gdchoncs").gameObject;
                    GameObject gdphanracs = gdchoncapsule.transform.Find("gdphanracs").gameObject;
                    Text txtgioihan = gdchoncs.transform.Find("txtgioihan").GetComponent<Text>();
                    txtgioihan.gameObject.SetActive(false);
                    gdchoncs.transform.Find("btnBatDau").gameObject.SetActive(false);
                    gdphanracs.transform.Find("btnPhanRa").gameObject.SetActive(false);
                    LoadOgd2(json["OGiaoDien2"]);

                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
    }    

    private GameObject menumuaitem;
    public void OpenMenuMuaXeng()
    {
        AudioManager.SoundClick();
      //  GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        //// txtupdate = btnchon.transform.parent.GetComponentsInChildren<Text>()[0];
        //  nameitemmua = btnchon.transform.parent.name;
          nameitemmua = "ManhCongThuc";

        GameObject menu = Instantiate(Inventory.LoadObjectResource("GameData/"+nameEvent+"/MenuMuaItem"), transform.position, Quaternion.identity);
        menumuaitem = menu;
        menu.transform.SetParent(transform, false);
    //    menu.transform.position = CrGame.ins.trencung.transform.position;
        menu.SetActive(true);

        //    GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaItem", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        g.transform.Find("txttanggia").GetComponent<Text>().text = "";//"Giá tăng khi mua nhiều trong ngày, sẽ được reset khi qua ngày mới.";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = "Mua Mảnh Công Thức";
        Image imgitem = g.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
       // imgitem.sprite = Resources.Load<Sprite>("GameData/EventSinhNhat2024/" + nameitemmua);
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
        if (s.Length > 4) s = "1000";
        if (int.Parse(s) > 1000) s = "1000";
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
        datasend["class"] = EventManager.ins.nameEvent;
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
        datasend["class"] = EventManager.ins.nameEvent;
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
                    //txtupdate.text = json["soitem"].AsString;
                    //if (txtupdate.transform.parent.transform.parent.name == "Item")
                    //{
                    //    string s = json["soitem"].AsString;
                    //    txtupdate.text = s;
                    //}
                    //SetsucXac(json["sucxac"].AsString);

                    int HoaTuyet = 0;
                    if (Inventory.ins.ListItemThuong.ContainsKey("itemHoaTuyet")) HoaTuyet = int.Parse(Inventory.ins.ListItemThuong["itemHoaTuyet"].transform.GetChild(0).GetComponent<Text>().text);
                    LoadManhCongThuc(int.Parse(json["soitem"].AsString), HoaTuyet);
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
    protected override void DiemDanhOk(JSONNode json)
    {
        debug.Log(json.ToString());

        foreach (string key in json["allitemUpdate"].AsObject.Keys)
        {
            debug.Log(key);
            if (key != "Capsule")
            {
                SetItemEvent(key, json["allitemUpdate"][key].AsString);
            }
            else
            {
                GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;

                GameObject contentCapsule = giaodien2.transform.Find("GiaoDienChonCapsule").transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject objitem = contentCapsule.transform.GetChild(0).gameObject;

                GameObject item = Instantiate(objitem, transform.position, Quaternion.identity);
                item.transform.SetParent(contentCapsule.transform, false);
                for (int j = 0; j < allCapsuleSprite.Length; j++)
                {
                    if (allCapsuleSprite[j].name == json["allitemUpdate"][key]["mau"].AsString)
                    {
                        item.transform.GetChild(1).GetComponent<Image>().sprite = allCapsuleSprite[j];
                        break;
                    }
                }
                string nameitem = json["allitemUpdate"][key]["nameitem"].AsString;
                if (nameitem.Contains("Rong") && nameitem != "BuiRong")
                {
                    item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(nameitem + "1");
                }
                else
                {
                    item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite(nameitem);
                }
                item.name = json["allitemUpdate"][key]["namecapsule"].AsString;
                item.SetActive(true);
            }
        }
    }
    public void TatPanelQua()
    {
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        GameObject panelqua = giaodien2.transform.Find("PanelNhanQua").gameObject;
        GameObject quabay = Instantiate(panelqua.transform.GetChild(1).gameObject, transform.position, Quaternion.identity);
        quabay.SetActive(false);
        quabay.transform.SetParent(transform, false);
        quabay.AddComponent<QuaBay>().vitribay = btnHopQua;
        panelqua.SetActive(false);
        quabay.SetActive(true);
        // nhanqua = false;
    }

    public void OpenMenuNhiemvu()
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetNhiemVu";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            //debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", CrGame.ins.trencung, true, transform.GetSiblingIndex() + 1);
                GameObject AllNhiemVu = MenuNhiemVu.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject obj = AllNhiemVu.transform.GetChild(0).gameObject;
                for (int i = 0; i < json["allNhiemvu"].Count; i++)
                {
                    //debug.Log(json["allNhiemvu"][i].ToString());
                    GameObject instan = Instantiate(obj, transform.position, Quaternion.identity);
                    instan.transform.SetParent(AllNhiemVu.transform, false);
                    Text txtnv = instan.transform.GetChild(0).GetComponent<Text>();
                    txtnv.text = json["allNhiemvu"][i]["namenhiemvu"].Value;

                    Text txttiendo = instan.transform.GetChild(1).GetComponent<Text>();
                    Text txtphanthuong = instan.transform.GetChild(2).GetComponent<Text>();
                    if (int.Parse(json["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["allNhiemvu"][i]["maxnhiemvu"].Value))
                    {
                        txttiendo.text = "<color=#00ff00ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    txtphanthuong.text = json["allNhiemvu"][i]["qua"]["soluong"].AsString;
                    Image img = instan.transform.GetChild(3).GetComponent<Image>();
                    img.sprite = GetSprite(json["allNhiemvu"][i]["qua"]["name"].AsString);

                    img.SetNativeSize();
                    instan.SetActive(true);
                }
                MenuNhiemVu.transform.GetChild(0).transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuNhiemVu"); });

            }
        }
    }

    public void ParseData(JSONNode json)
    {
        CrGame.ins.DonDepDao();
        btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
        GameObject giaodien1 = transform.GetChild(0).gameObject;

        //  GameObject trencung = GameObject.FindGameObjectWithTag("trencung").gameObject;
        btnHopQua.transform.SetParent(transform.GetChild(0).transform);
        if (Friend.ins.QuaNha) Friend.ins.GoHome();
        foreach (string key in json["data"]["allItem"].AsObject.Keys)
        {
            SetItemEvent(key, json["data"]["allItem"][key].AsString);
        }
        SetYeuCau(json["YeuCau"]);

        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        GameObject contentCapsule = giaodien2.transform.Find("GiaoDienChonCapsule").transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject objitem = contentCapsule.transform.GetChild(0).gameObject;

        for (int i = 0; i < json["data"]["allCapsule"].Count; i++)
        {
            for (int a = 0; a < json["data"]["allCapsule"][i]["soitem"].AsInt; a++)
            {
                GameObject item = Instantiate(objitem, transform.position, Quaternion.identity);
                item.transform.SetParent(contentCapsule.transform, false);
                for (int j = 0; j < allCapsuleSprite.Length; j++)
                {
                    if (allCapsuleSprite[j].name == json["data"]["allCapsule"][i]["mau"].AsString)
                    {
                        item.transform.GetChild(1).GetComponent<Image>().sprite = allCapsuleSprite[j];
                        break;
                    }
                }
                TaoVien(json["data"]["allCapsule"][i]["mau"].AsString, item.transform);
                string nameitem = json["data"]["allCapsule"][i]["nameitem"].AsString;
                if (nameitem.Contains("Rong") && nameitem != "BuiRong")
                {
                    item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(nameitem + "1");
                }
                else
                {
                    item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite(nameitem);
                }
                item.name = json["data"]["allCapsule"][i]["namecapsule"].AsString;
                item.SetActive(true);
            }

            //  AddCapsule(json["data"]["allCapsule"][i]);
        }
        LoadMocQuaTichLuy(json["data"]["quatichluy"], json["data"]["solanquay"].AsString);
       // LoadDiemDanh(json["data"]["nhandiemdanh"], json["data"]["landiemdanh"].AsInt, json["data"]["diemdanh"].AsBool);
        //debug.Log("test " + json["test"]["abc"].AsString);
        if (json["data"]["CapsuleChon"]["namecapsule"].AsString != "")
        {
            GameObject KhungCapsuleChon = giaodien2.transform.Find("KhungCapsuleChon").gameObject;
            KhungCapsuleChon.transform.GetChild(0).gameObject.SetActive(false);
            KhungCapsuleChon.transform.GetChild(1).gameObject.SetActive(true);

            GameObject KhungCapsuleChontrong = KhungCapsuleChon.transform.GetChild(1).gameObject;
            for (int j = 0; j < allCapsuleSprite.Length; j++)
            {
                if (allCapsuleSprite[j].name == json["data"]["CapsuleChon"]["mau"].AsString)
                {
                    KhungCapsuleChontrong.transform.GetChild(0).GetComponent<Image>().sprite = allCapsuleSprite[j];
                    break;
                }
            }
            string nameitem = json["data"]["CapsuleChon"]["nameitem"].AsString;
            Image imgcapsule = KhungCapsuleChontrong.transform.GetChild(1).GetComponent<Image>();
            if (nameitem.Contains("Rong") && nameitem != "BuiRong")
            {
                imgcapsule.sprite = Inventory.LoadSpriteRong(nameitem + "1");
            }
            else
            {
                imgcapsule.sprite = Inventory.LoadSprite(nameitem);
            }
            imgcapsule.name = json["data"]["CapsuleChon"]["namecapsule"].AsString;
            TaoVien(json["data"]["CapsuleChon"]["mau"].AsString, KhungCapsuleChontrong.transform);
        }
        LoadMocQuaTichLuyGd2(json["data"]["quatichluyGd2"], json["data"]["solanQuaAi"].AsString);
        LoadOgd2(json["data"]["OGiaoDien2"]);
        if (json["data"]["nhannro4sao"].AsString != "")
        {
            giaodien1.transform.Find("nro4sao").gameObject.SetActive(false);
        }
        CrGame.ins.giaodien.SetActive(false);
        CrGame.ins.menulogin.SetActive(false);
    }
    void TaoVien(string mauu, Transform tf)
    {
        if (tf.transform.Find("Vien"))
        {
            GameObject vien = tf.transform.Find("Vien").gameObject;
            Animator anim = vien.GetComponent<Animator>();
            if (mauu == "Tim")
            {
                anim.runtimeAnimatorController = VienDo;
                vien.gameObject.SetActive(true);
            }
            else if (mauu == "Vang")
            {
                anim.runtimeAnimatorController = VienVang;
                vien.gameObject.SetActive(true);
            }
            else
            {
                anim.runtimeAnimatorController = null;
                vien.gameObject.SetActive(false);
            }
        }
        string mau = mauu.ToLower();
        if (mau == "lam") mau = "xanhduong";
        if (mau == "luc") mau = "xanhla";
        // if (tf.gameObject.name == "KhungCapsuleChon") return;
        if (tf.GetComponent<Image>())
        {
            Image img = tf.GetComponent<Image>();

            for (int j = 0; j < alltuchat.Length; j++)
            {
                if (alltuchat[j].name == mau)
                {
                    img.sprite = alltuchat[j];
                    break;
                }
            }

        }
        else if (tf.GetChild(0).GetComponent<Image>())
        {
            Image img = tf.GetChild(0).GetComponent<Image>();
            for (int j = 0; j < alltuchat.Length; j++)
            {
                if (alltuchat[j].name == mau)
                {
                    img.sprite = alltuchat[j];
                    break;
                }
            }
        }
    }
    void LoadMocQuaTichLuy(JSONNode quatichluy, string diem)
    {
        GameObject allmocqua = transform.GetChild(0).transform.Find("allmocqua").gameObject;
        Text txtdiem = allmocqua.transform.GetChild(1).GetComponent<Text>();

        GameObject contentQua = allmocqua.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
        Image imgfill = contentQua.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>();
        imgfill.fillAmount = float.Parse(diem) / 2000;

        txtdiem.text = diem;
        for (int i = 0; i < quatichluy.Count; i++)
        {
            debug.Log(quatichluy[i].Value);
            if (quatichluy[i].Value == "chuaduocnhan")
            {
                string name = contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = chuaduocnhan;
                contentQua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = ((i + 1) * 50) + "";
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                if (i > 0) contentQua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite1;
                else contentQua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite1;
            }
            if (quatichluy[i].Value == "duocnhan")
            {
                string name = contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = duocnhan;
                Button btn = null;
                if (!contentQua.transform.GetChild(i).transform.GetChild(1).GetComponent<Button>())
                {
                    btn = contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.AddComponent<Button>();
                }
                else btn = contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Button>();
                btn.gameObject.SetActive(true);
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(NhanQuaTichLuy);

                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                if (i > 0) contentQua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                else contentQua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
            }
            else if (quatichluy[i].Value == "danhan")
            {
                string name = contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = danhan;
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                contentQua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                if (i > 0) contentQua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                else contentQua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
            }
        }
    }
    void LoadMocQuaTichLuyGd2(JSONNode quatichluy, string diem)
    {
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        GameObject allMocQua = giaodien2.transform.Find("allMocQua").gameObject;
        Text txt = allMocQua.transform.Find("txt").GetComponent<Text>();
        txt.text = diem;
        Image fill = allMocQua.transform.Find("thanhluot").transform.GetChild(0).GetComponent<Image>();
        GameObject content = allMocQua.transform.Find("Content").gameObject;
        fill.fillAmount = float.Parse(diem) / 21;
        for (int i = 0; i < quatichluy.Count; i++)
        {
            GameObject g = content.transform.GetChild(i).gameObject;
            if (quatichluy[i].AsString == "danhan")
            {
                g.transform.GetChild(2).transform.GetComponent<Text>().text = "Đã nhận";
            }
            else if (quatichluy[i].AsString == "duocnhan")
            {
                g.transform.GetChild(1).gameObject.SetActive(true);
                g.transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (quatichluy[i].AsString == "chuaduocnhan")
            {
                g.transform.GetChild(1).gameObject.SetActive(false);
                Text txtt = g.transform.GetChild(2).transform.GetComponent<Text>();
                g.transform.GetChild(2).gameObject.SetActive(true);
                txtt.text = ((i + 1) * 3).ToString();
            }
        }
    }
    //void LoadDiemDanh(JSONNode nhandiemdanh, int landiemdanh, bool diemdanh)
    //{
    //    GameObject DiemDanh = transform.Find("MenuDiemDanh").transform.GetChild(0).gameObject;
    //    int solandiemdanh = landiemdanh;
    //    DiemDanh.transform.GetChild(1).GetComponent<Text>().text = "Bạn đã điểm danh <color=lime>" + solandiemdanh + " ngày</color>.";
    //    if (diemdanh)
    //    {
    //        DiemDanh.transform.GetChild(2).GetComponent<Button>().interactable = true;
    //        DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + 0 + "/1</color>";
    //    }
    //    else
    //    {
    //        DiemDanh.transform.GetChild(2).GetComponent<Button>().interactable = false;
    //        DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + 1 + "/1</color>";
    //        if (nhandiemdanh[0].Value == "duocnhan") DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>().interactable = true;
    //        else if (nhandiemdanh[0].Value == "danhan")
    //        {
    //            DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>().interactable = false;
    //            DiemDanh.transform.GetChild(3).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
    //        }
    //    }
    //    if (solandiemdanh < 7) DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/7</color>";
    //    else
    //    {
    //        DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/7</color>";
    //        if (nhandiemdanh[1].Value == "duocnhan") DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>().interactable = true;
    //        else if (nhandiemdanh[1].Value == "danhan")
    //        {
    //            DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>().interactable = false;
    //            DiemDanh.transform.GetChild(4).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
    //        }
    //    }
    //    if (solandiemdanh < 14) DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/14</color>";
    //    else
    //    {
    //        DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/14</color>";
    //        if (nhandiemdanh[2].Value == "duocnhan") DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>().interactable = true;
    //        else if (nhandiemdanh[2].Value == "danhan")
    //        {
    //            DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>().interactable = false;
    //            DiemDanh.transform.GetChild(5).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
    //        }
    //    }
    //    if (nhandiemdanh[3].Value == "duocnhan") DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>().interactable = true;
    //    else if (nhandiemdanh[3].Value == "danhan")
    //    {
    //        DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>().interactable = false;
    //        DiemDanh.transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
    //    }
    //    //     DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1);
    //}
    public void NhanQuaTichLuy()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "NhanQuaTichLuy";
        datasend["data"]["vitri"] = btnchon.transform.parent.GetSiblingIndex().ToString();
        // NetworkManager.ins.SendServer(datasend, nhanquaok => { });

        NetworkManager.ins.SendServer(datasend, NhanQuaTichLuyOK);
        void NhanQuaTichLuyOK(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if (json["nameitem"].AsString != "")
                {
                    SetItemEvent(json["nameitem"].AsString, json["soluong"].AsString);
                }


                if (json["solanquay"].AsString != "")
                {
                    LoadMocQuaTichLuy(json["quatichluy"], json["solanquay"].AsString);
                }
                else
                {
                    GameObject allmocqua = transform.GetChild(0).transform.Find("allmocqua").gameObject;
                    GameObject contentQua = allmocqua.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;

                    GameObject g = contentQua.transform.GetChild(json["vitri"].AsInt).gameObject;

                    g.transform.GetChild(1).gameObject.SetActive(false);
                    g.transform.GetChild(2).gameObject.SetActive(true);
                    g.transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                    g.transform.GetChild(0).GetComponent<Image>().sprite = danhan;

                    CrGame.ins.OnThongBaoNhanh(json["info"].AsString);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    void GetGiaMoOgd2()
    {
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        GameObject KhungCapsuleChon = giaodien2.transform.Find("KhungCapsuleChon").gameObject;
        string mau = KhungCapsuleChon.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite.name;
        debug.Log("mau " + mau);
        int giamaydo = 0;
        if (mau == "Lam") giamaydo = 10;
        else if (mau == "Luc") giamaydo = 20;
        else if (mau == "Tim") giamaydo = 30;
        else if (mau == "Vang") giamaydo = 40;
        Text txtgiamo = giaodien2.transform.Find("txtgiamo").GetComponent<Text>();
        string maydo = GetItemEvent("MayDoCapsule");

        txtgiamo.transform.GetChild(1).gameObject.SetActive(false);
        txtgiamo.text = "Giá mở mỗi mảnh ghép của Capsule này là <color=orange>" + giamaydo + "</color>";

        if (int.Parse(maydo) >= giamaydo)
        {
            //txtgiamo.transform.GetChild(1).gameObject.SetActive(false);
            //txtgiamo.text = "Giá mở mỗi mảnh ghép là <color=orange>10</color>";
        }
        else
        {
            //txtgiamo.text = "Giá mở mỗi mảnh ghép là <color=orange>" + maydo+"</color>";
            Text txtkc = txtgiamo.transform.GetChild(1).GetComponent<Text>();
            txtkc.text = "Hiện tại sở hữu <color=orange>" + maydo + "</color> máy dò, Mở mảnh ghép tiếp theo yêu cầu thêm <color=lime>" + ((giamaydo - int.Parse(maydo)) * 10).ToString() + " Kim cương</color>";
            txtkc.gameObject.SetActive(true);
        }
    }

    public void NhanQuaTichLuyGD2()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "NhanQuaTichLuyGD2";
        datasend["data"]["vitri"] = btnchon.transform.parent.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend, NhanQuaTichLuyGD2Ok);
        void NhanQuaTichLuyGD2Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
                if (json["nameitem"].AsString != "")
                {
                    SetItemEvent(json["nameitem"].AsString, json["soluong"].AsString);
                }
                else
                {
                    GameObject contentCapsule = giaodien2.transform.Find("GiaoDienChonCapsule").transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;
               

                    for (int i = 0; i < json["Capsule"].Count; i++)
                    {


                        GameObject objitem = contentCapsule.transform.GetChild(0).gameObject;

                        GameObject item = Instantiate(objitem, transform.position, Quaternion.identity);
                        item.transform.SetParent(contentCapsule.transform, false);
                        for (int j = 0; j < allCapsuleSprite.Length; j++)
                        {
                            if (allCapsuleSprite[j].name == json["Capsule"][i]["mau"].AsString)
                            {
                                item.transform.GetChild(1).GetComponent<Image>().sprite = allCapsuleSprite[j];
                                break;
                            }
                        }

                        string nameitem = json["Capsule"][i]["nameitem"].AsString;
                        if (nameitem.Contains("Rong") && nameitem != "BuiRong")
                        {
                            item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(nameitem + "1");
                        }
                        else
                        {
                            item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite(nameitem);
                        }
                        item.name = json["Capsule"][i]["namecapsule"].AsString;
                        item.SetActive(true);
                        TaoVien(json["Capsule"][i]["mau"].AsString, item.transform);
                    }
                
                }

                GameObject allMocQua = giaodien2.transform.Find("allMocQua").gameObject;
                GameObject content = allMocQua.transform.Find("Content").gameObject;
                GameObject g = content.transform.GetChild(json["vitri"].AsInt).gameObject;

                g.transform.GetChild(1).gameObject.SetActive(false);
                g.transform.GetChild(2).gameObject.SetActive(true);
                g.transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";


                CrGame.ins.OnThongBaoNhanh(json["info"].AsString);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    void LoadOgd2(JSONNode array)
    {
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        GameObject allO = giaodien2.transform.Find("allO").gameObject;
        for (int i = 0; i < allO.transform.childCount; i++)
        {
            for (int j = 0; j < allO.transform.GetChild(i).transform.childCount; j++)
            {
                if (array[i][j].AsString == "damo")
                {
                    allO.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(false);
                }
                else allO.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(true);
            }
        }
    }
    public void OpenGd2()
    {
        AudioManager.PlaySound("soundClick");
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        btnHopQua.transform.SetParent(giaodien2.transform);
        // GameObject KhungCapsuleChon = giaodien2.transform.Find("KhungCapsuleChon").gameObject;
        GetGiaMoOgd2();
        giaodien2.SetActive(true);
    }
    public void ThoatGd2()
    {
        AudioManager.PlaySound("soundClick");
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        btnHopQua.transform.SetParent(transform.Find("GiaoDien1").transform);
        giaodien2.SetActive(false);
    }
    public void MoOGD2()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int mang = btnchon.transform.parent.transform.GetSiblingIndex();
        int mangtrong = btnchon.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "MoOGiaoDien2";
        datasend["data"]["mang"] = mang.ToString();
        datasend["data"]["mangtrong"] = mangtrong.ToString();
        NetworkManager.ins.SendServer(datasend, MoOOk);
        void MoOOk(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                AudioManager.PlaySound("soundClick2");
                LoadOgd2(json["OGiaoDien2"]);
                CrGame.ins.OnThongBaoNhanh(json["info"].AsString);
                if (json["reset"].AsBool)
                {
                    GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
                    GameObject KhungCapsuleChon = giaodien2.transform.Find("KhungCapsuleChon").gameObject;
                    KhungCapsuleChon.transform.GetChild(0).gameObject.SetActive(true);
                    KhungCapsuleChon.transform.GetChild(1).gameObject.SetActive(false);
                    LoadMocQuaTichLuyGd2(json["quatichluyGd2"], json["solanQuaAi"].AsString);

                    GameObject panelqua = giaodien2.transform.Find("PanelNhanQua").gameObject;

                    panelqua.transform.GetChild(1).GetComponent<Image>().sprite = KhungCapsuleChon.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite;
                    panelqua.transform.GetChild(2).GetComponent<Text>().text = "";

                    //if (json["QuaAi"]["loaiitem"].Value == "Item")
                    //{
                    //    panelqua.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSprite(json["QuaAi"]["name"].Value);
                    //    panelqua.transform.GetChild(2).GetComponent<Text>().text = "x" + json["QuaAi"]["soluong"].Value;
                    //}
                    //else if (json["QuaAi"]["loaiitem"].Value == "ItemRong")
                    //{
                    //    panelqua.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["QuaAi"]["name"].Value + 1);
                    //    panelqua.transform.GetChild(2).GetComponent<Text>().text = json["QuaAi"]["sao"].Value + " sao";
                    //}
                    //else if (json["QuaAi"]["loaiitem"].Value == "ItemEvent")
                    //{
                    //    for (int i = 0; i < allItemEvent.Length; i++)
                    //    {
                    //        if(json["QuaAi"]["name"].Value == allItemEvent[i].name)
                    //        {
                    //            panelqua.transform.GetChild(1).GetComponent<Image>().sprite = allItemEvent[i];
                    //            panelqua.transform.GetChild(2).GetComponent<Text>().text = "x" + json["QuaAi"]["soluong"].Value;
                    //            break;
                    //        }
                    //    }
                    //}
                    panelqua.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                    panelqua.SetActive(true);
                    GameObject contentCapsule = giaodien2.transform.Find("GiaoDienChonCapsule").transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;
                    Destroy(contentCapsule.transform.Find(json["namecapsule"].AsString).gameObject);

                    GameObject gdchoncapsule = giaodien2.transform.Find("GiaoDienChonCapsule").gameObject;

                    GameObject KhungCapsule = gdchoncapsule.transform.Find("KhungCapsule").gameObject;

                    Text txtnamecsl = gdchoncapsule.transform.Find("ttxnamecapsule").GetComponent<Text>();
                    txtnamecsl.gameObject.SetActive(false);
                    KhungCapsule.gameObject.SetActive(false);

                    GameObject gdchoncs = gdchoncapsule.transform.Find("gdchoncs").gameObject;
                    GameObject gdphanracs = gdchoncapsule.transform.Find("gdphanracs").gameObject;
                    Text txtgioihan = gdchoncs.transform.Find("txtgioihan").GetComponent<Text>();
                    txtgioihan.gameObject.SetActive(false);
                    gdchoncs.transform.Find("btnBatDau").gameObject.SetActive(false);
                    gdphanracs.transform.Find("btnPhanRa").gameObject.SetActive(false);
                }
                SetItemEvent("MayDoCapsule", json["MayDoCapsule"].AsString);
                GetGiaMoOgd2();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }


    //public void DiemDanh()
    //{
    //    AudioManager.PlaySound("soundClick");
    //    JSONClass datasend = new JSONClass();
    //    datasend["class"] = "EventRongThan2023";
    //    datasend["method"] = "DiemDanhEvent";
    //    NetworkManager.ins.SendServer(datasend, DiemDanhok);
    //    void DiemDanhok(JSONNode json)
    //    {
    //        if (json["status"].AsString == "0")
    //        {
    //            LoadDiemDanh(json["nhandiemdanh"], json["landiemdanh"].AsInt, json["diemdanh"].AsBool);
    //            CrGame.ins.OnThongBaoNhanh("Đã điểm danh!");
    //        }
    //        else
    //        {
    //            CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
    //        }
    //    }
    //}


    //public void NhanQuaDiemDanh()
    //{
    //    Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
    //    AudioManager.PlaySound("soundClick");
    //    int vitri = btn.transform.parent.GetSiblingIndex() - 3;
    //    JSONClass datasend = new JSONClass();
    //    datasend["class"] = "EventRongThan2023";
    //    datasend["method"] = "NhanQuaDiemDanh";
    //    datasend["data"]["vitri"] = vitri.ToString();
    //    NetworkManager.ins.SendServer(datasend, Ok);
    //    void Ok(JSONNode json)
    //    {
    //        if (json["status"].AsString == "0")
    //        {
    //            foreach (string key in json["allitemUpdate"].AsObject.Keys)
    //            {
    //                debug.Log(key);
    //                if (key != "Capsule")
    //                {
    //                    SetItemEvent(key, json["allitemUpdate"][key].AsString);
    //                }
    //                else
    //                {
    //                    GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;

    //                    GameObject contentCapsule = giaodien2.transform.Find("GiaoDienChonCapsule").transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;
    //                    GameObject objitem = contentCapsule.transform.GetChild(0).gameObject;

    //                    GameObject item = Instantiate(objitem, transform.position, Quaternion.identity);
    //                    item.transform.SetParent(contentCapsule.transform, false);
    //                    for (int j = 0; j < allCapsuleSprite.Length; j++)
    //                    {
    //                        if (allCapsuleSprite[j].name == json["allitemUpdate"][key]["mau"].AsString)
    //                        {
    //                            item.transform.GetChild(1).GetComponent<Image>().sprite = allCapsuleSprite[j];
    //                            break;
    //                        }
    //                    }
    //                    string nameitem = json["allitemUpdate"][key]["nameitem"].AsString;
    //                    if (nameitem.Contains("Rong") && nameitem != "BuiRong")
    //                    {
    //                        item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(nameitem + "1");
    //                    }
    //                    else
    //                    {
    //                        item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite(nameitem);
    //                    }
    //                    item.name = json["allitemUpdate"][key]["namecapsule"].AsString;
    //                    item.SetActive(true);
    //                }
    //            }
    //            LoadDiemDanh(json["nhandiemdanh"], json["landiemdanh"].AsInt, json["diemdanh"].AsBool);
    //            CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
    //        }
    //        else
    //        {
    //            CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
    //        }
    //    }
    //}
    int ManhCongThuc;
    public void OpenMenuCheCapsule()
    {
        AudioManager.PlaySound("soundClick");
        GameObject g = transform.Find("GiaoDien2").transform.Find("MenuCheTaoCapsule").transform.GetChild(0).gameObject;
        GameObject tren = g.transform.GetChild(4).gameObject;
        GameObject duoi = g.transform.GetChild(5).gameObject;

        Text txtmanhcongthuc = tren.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
        int ManhCongThuc = int.Parse(txtmanhcongthuc.text);

        int HoaTuyet = 0;

        if (Inventory.ins.ListItemThuong.ContainsKey("itemHoaTuyet")) HoaTuyet = int.Parse(Inventory.ins.ListItemThuong["itemHoaTuyet"].transform.GetChild(0).GetComponent<Text>().text);
        LoadManhCongThuc(ManhCongThuc, HoaTuyet);
        Button btnexit = g.transform.Find("btnExit").GetComponent<Button>();
        btnexit.onClick.AddListener(delegate { ExitCheCapsule(ManhCongThuc); });
        g.transform.parent.gameObject.SetActive(true);
    }
    void LoadManhCongThuc(int ManhCongThuc, int HoaTuyet)
    {
        GameObject g = transform.Find("GiaoDien2").transform.Find("MenuCheTaoCapsule").transform.GetChild(0).gameObject;
        GameObject tren = g.transform.GetChild(4).gameObject;
        GameObject duoi = g.transform.GetChild(5).gameObject;
        Text txtmanhcongthuc = tren.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();

        txtmanhcongthuc.text = "<color=red>" + ManhCongThuc + "/100</color>";
        if (ManhCongThuc >= 100) txtmanhcongthuc.text = "<color=lime>" + ManhCongThuc + "/100</color>";
        Text txtmanhcongthucduoi = duoi.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
        if (ManhCongThuc >= 250) txtmanhcongthucduoi.text = "<color=lime>" + ManhCongThuc + "/250</color>";
        else txtmanhcongthucduoi.text = "<color=red>" + ManhCongThuc + "/250</color>";
        Text txthoatuyet = tren.transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();

        txthoatuyet.text = "<color=red>" + HoaTuyet + "/1000</color>";
        if (Inventory.ins.ListItemThuong.ContainsKey("itemHoaTuyet")) HoaTuyet = int.Parse(Inventory.ins.ListItemThuong["itemHoaTuyet"].transform.GetChild(0).GetComponent<Text>().text);
        if (HoaTuyet >= 1000) txthoatuyet.text = "<color=lime>" + HoaTuyet + "/1000</color>";
        Text txthoatuyetduoi = duoi.transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
        if (HoaTuyet >= 3000) txthoatuyetduoi.text = "<color=lime>" + HoaTuyet + "/3000</color>";
        else txthoatuyetduoi.text = "<color=red>" + HoaTuyet + "/3000</color>";
        Button btntren = tren.transform.GetChild(4).transform.GetChild(1).GetComponent<Button>();
        if (ManhCongThuc >= 100 && HoaTuyet >= 1000) btntren.interactable = true;
        else btntren.interactable = false;
        Button btnduoi = duoi.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>();
        if (ManhCongThuc >= 250 && HoaTuyet >= 3000) btnduoi.interactable = true;
        else btnduoi.interactable = false;


      
        Button btnexit = g.transform.Find("btnExit").GetComponent<Button>();
        btnexit.onClick.RemoveAllListeners();
        btnexit.onClick.AddListener(delegate { ExitCheCapsule(ManhCongThuc); });

    }
    void ExitCheCapsule(int manhcongthuc)
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        AudioManager.PlaySound("soundClick");
        btn.onClick.RemoveAllListeners();

        GameObject g = transform.Find("GiaoDien2").transform.Find("MenuCheTaoCapsule").transform.GetChild(0).gameObject;
        GameObject tren = g.transform.GetChild(4).gameObject;
   //     GameObject duoi = g.transform.GetChild(5).gameObject;
        Text txtmanhcongthuc = tren.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
        txtmanhcongthuc.text = manhcongthuc.ToString();
        transform.Find("GiaoDien2").transform.Find("MenuCheTaoCapsule").gameObject.SetActive(false);
    }
    public void CheTaoCapsule(string s)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "CheTaoCapsule";
        datasend["data"]["chetao"] = s;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                Inventory.ins.AddItem("HoaTuyet", -json["giaHoatuyet"].AsInt);
                int HoaTuyet = 0;
                if (Inventory.ins.ListItemThuong.ContainsKey("itemHoaTuyet")) HoaTuyet = int.Parse(Inventory.ins.ListItemThuong["itemHoaTuyet"].transform.GetChild(0).GetComponent<Text>().text);
                LoadManhCongThuc(json["ManhCongThuc"].AsInt, HoaTuyet);


                GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;

                GameObject menudangchetao = giaodien2.transform.Find("MenuCheTaoCapsule").transform.GetChild(1).gameObject;
                Image fill = menudangchetao.transform.Find("fill").GetComponent<Image>();
                Text txtchetao = menudangchetao.transform.Find("txtchetao").GetComponent<Text>();
                menudangchetao.SetActive(true);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    int Randomfull = Random.Range(30, 100);
                    for (int i = 0; i < Randomfull; i++)
                    {
                        fill.fillAmount = (float)i / (float)100;
                        yield return new WaitForSeconds(Random.Range(0.0001f, 0.05f));
                    }
                    yield return new WaitForSeconds(0.02f);

                    if (json["chetao"].AsString == "ok")
                    {
                        GameObject contentCapsule = giaodien2.transform.Find("GiaoDienChonCapsule").transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;
                        GameObject objitem = contentCapsule.transform.GetChild(0).gameObject;

                        GameObject item = Instantiate(objitem, transform.position, Quaternion.identity);
                        item.transform.SetParent(contentCapsule.transform, false);
                        for (int j = 0; j < allCapsuleSprite.Length; j++)
                        {
                            if (allCapsuleSprite[j].name == json["Capsule"]["mau"].AsString)
                            {
                                item.transform.GetChild(1).GetComponent<Image>().sprite = allCapsuleSprite[j];
                                break;
                            }
                        }
                        string nameitem = json["Capsule"]["nameitem"].AsString;
                        if (nameitem.Contains("Rong") && nameitem != "BuiRong")
                        {
                            item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(nameitem + "1");
                        }
                        else
                        {
                            item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite(nameitem);
                        }
                        item.name = json["Capsule"]["namecapsule"].AsString;
                        item.SetActive(true);
                        TaoVien(json["Capsule"]["mau"].AsString, item.transform);
                    }
                    else
                    {

                    }
                    menudangchetao.SetActive(false);
                    CrGame.ins.OnThongBaoNhanh(json["info"].AsString);
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    public Dictionary<string, GameObject> menuevent = new Dictionary<string, GameObject>();
    public GameObject GetCreateMenu(string namemenu, Transform parnet = null, bool active = true)
    {
        GameObject g = null;
        if (menuevent.ContainsKey(namemenu))
        {
            menuevent[namemenu].SetActive(active);
            g = menuevent[namemenu];
        }
        else
        {
            if (parnet == null)
            {
                parnet = gameObject.transform;
            }
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/EventRongThan2023/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
            menuevent.Add(namemenu, mn);
            mn.transform.SetParent(parnet.transform, false);
            mn.transform.position = parnet.transform.position;
            mn.SetActive(active);
            g = mn;
            //  Resources.UnloadUnusedAssets();
        }
        //  g.transform.SetSiblingIndex(index);
        return g;
    }
    public void DestroyMenu(string namemenu)
    {
        if (menuevent.ContainsKey(namemenu))
        {
            Destroy(menuevent[namemenu]);
            menuevent.Remove(namemenu);

            Resources.UnloadUnusedAssets();
        }
    }
    public void OpenMenuDoiManh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "MenuDoiManh";
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                GameObject menuDoiManh = EventManager.ins.GetCreateMenu("MenuDoiManh", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
                GameObject ContentManh = menuDoiManh.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject ObjectManh = ContentManh.transform.GetChild(0).gameObject;

                for (int i = 0; i < json["ManhDoi"].Count; i++)
                {
                    //debug.Log(json["ManhDoi"][i].ToString());
                    GameObject manh = Instantiate(ObjectManh, ContentManh.transform.position, Quaternion.identity);
                    manh.transform.SetParent(ContentManh.transform, false);
                    manh.GetComponent<Button>().onClick.AddListener(XemManhDoi);
                    Image imgmanh = manh.transform.GetChild(0).GetComponent<Image>();
                    if (json["ManhDoi"][i]["itemgi"].AsString == "item" || json["ManhDoi"][i]["itemgi"].AsString == "thuyenthucan")
                    {
                        imgmanh.sprite = Inventory.LoadSprite(json["ManhDoi"][i]["nameitem"].AsString);
                    }
                    else if (json["ManhDoi"][i]["itemgi"].AsString == "itemevent")
                    {
                        //for (int j = 0; j < EventManager.ins.allitemEvent.Length; j++)
                        //{
                        //    if (json["ManhDoi"][i]["nameitem"].Value == EventManager.ins.allitemEvent[j].name)
                        //    {
                        //        imgmanh.sprite = EventManager.ins.allitemEvent[j];
                        //    }
                        //}
                        imgmanh.sprite = EventManager.ins.GetSprite(json["ManhDoi"][i]["nameitem"].AsString);
                        imgmanh.SetNativeSize();
                    }
                    else if (json["ManhDoi"][i]["itemgi"].AsString == "avatar")
                    {
                        Friend.ins.LoadImage("avt", json["ManhDoi"][i]["nameitem"].AsString, imgmanh);
                    }
                    else
                    {
                        imgmanh.sprite = Inventory.LoadSpriteRong(json["ManhDoi"][i]["nameitem"].Value + "2");
                    }
                    manh.name = json["ManhDoi"][i]["namekey"];
                    manh.SetActive(true);
                }
                menuDoiManh.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DoiManh);
                menuDoiManh.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ExitDoiManh);
                menuDoiManh.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    string namemanhchon;
    
    void XemManhDoi()
    {
        AudioManager.PlaySound("soundClick");
        Button btnchon = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        GameObject yeucau = EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(1).gameObject;
        namemanhchon = btnchon.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "XemManhDoi";
        datasend["data"]["namemanh"] = namemanhchon;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            for (int i = 0; i < yeucau.transform.childCount; i++)
            {
                yeucau.transform.GetChild(i).gameObject.SetActive(false);
            }
            int sonvok = 0;
            for (int i = 0; i < json["manhdoi"]["itemcan"].Count; i++)
            {
                for (int j = 0; j < yeucau.transform.childCount; j++)
                {
                    if (yeucau.transform.GetChild(j).name == json["manhdoi"]["itemcan"][i]["nameitem"].Value)
                    {
                        Text txtyeucau = yeucau.transform.GetChild(j).transform.GetChild(1).GetComponent<Text>();
                        yeucau.transform.GetChild(j).gameObject.SetActive(true);

                        txtyeucau.text = json["hienthisoitemco"][json["manhdoi"]["itemcan"][i]["nameitem"].AsString].AsString;
                        if (json["hienthisoitemco"][json["manhdoi"]["itemcan"][i]["nameitem"].AsString].AsBool)
                        {
                            sonvok += 1;
                        }
                        if (json["duocdoi"].AsBool)
                        {
                            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(true);
                            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(2).GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(false);
                        }
                    }
                }
            }
            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = json["manhdoi"]["thongtin"].Value;
        }
    }

    void DoiManh()
    {
        AudioManager.PlaySound("soundClick");

        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "DoiQua";
        datasend["data"]["namemanh"] = namemanhchon;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {

                GameObject menudoimanh = EventManager.ins.menuevent["MenuDoiManh"];
                menudoimanh.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = "";

                menudoimanh.transform.GetChild(2).gameObject.SetActive(false);
                GameObject yeucau = menudoimanh.transform.GetChild(1).gameObject;
                for (int i = 0; i < yeucau.transform.childCount; i++)
                {
                    yeucau.transform.GetChild(i).gameObject.SetActive(false);
                }
                //   transform.GetChild(0).transform.Find("imgLiXi").transform.GetChild(1).GetComponent<Text>().text = json["BaoLiXi"].AsString;
                //  ev.menuevent["GiaoDien2"].transform.Find("itemDaMatTrang").GetChild(1).GetComponent<Text>().text = json["DaMatTrang"].AsString;
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2.5f);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    private void ExitDoiManh()
    {
        EventManager.ins.DestroyMenu("MenuDoiManh");
        AudioManager.SoundClick();
    }
    public void ChonCapsule()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        GameObject gdchoncapsule = giaodien2.transform.Find("GiaoDienChonCapsule").gameObject;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "ChonCapsule";


        if (gdchoncapsule.transform.Find("gdchoncs").gameObject.activeSelf)
        {
            datasend["data"]["giaodien"] = "ChonCapsule";
        }
        else
        {
            datasend["data"]["giaodien"] = "PhanRaCapsule";
        }
        datasend["data"]["nameCapsule"] = btnchon.name;
        NetworkManager.ins.SendServer(datasend, ChonCapsuleok);
        void ChonCapsuleok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
                GameObject gdchoncapsule = giaodien2.transform.Find("GiaoDienChonCapsule").gameObject;

                GameObject KhungCapsule = gdchoncapsule.transform.Find("KhungCapsule").gameObject;

                Text txtnamecsl = gdchoncapsule.transform.Find("ttxnamecapsule").GetComponent<Text>();
                txtnamecsl.text = json["Capsule"]["namehienthi"].AsString;
                txtnamecsl.gameObject.SetActive(true);
                KhungCapsule.gameObject.SetActive(true);


                for (int j = 0; j < allCapsuleSprite.Length; j++)
                {
                    if (allCapsuleSprite[j].name == json["Capsule"]["mau"].AsString)
                    {
                        KhungCapsule.transform.GetChild(0).GetComponent<Image>().sprite = allCapsuleSprite[j];
                        break;
                    }
                }
                string nameitem = json["Capsule"]["nameitem"].AsString;
                Image imgcapsule = KhungCapsule.transform.GetChild(1).GetComponent<Image>();
                if (nameitem.Contains("Rong") && nameitem != "BuiRong")
                {
                    imgcapsule.sprite = Inventory.LoadSpriteRong(nameitem + "1");
                }
                else
                {
                    imgcapsule.sprite = Inventory.LoadSprite(nameitem);
                }
                imgcapsule.name = json["Capsule"]["namecapsule"].AsString;
                GameObject gdchoncs = gdchoncapsule.transform.Find("gdchoncs").gameObject;
                GameObject gdphanracs = gdchoncapsule.transform.Find("gdphanracs").gameObject;
                Text txtgioihan = gdchoncs.transform.Find("txtgioihan").GetComponent<Text>();
                txtgioihan.text = json["txtcapsulesudung"].AsString;
                txtgioihan.gameObject.SetActive(true);
                gdphanracs.transform.Find("txtgiaphanra").GetComponent<Text>().text = json["giaphanra"].AsString;
                gdchoncs.transform.Find("btnBatDau").gameObject.SetActive(true);
                gdphanracs.transform.Find("btnPhanRa").gameObject.SetActive(true);
                TaoVien(json["Capsule"]["mau"].AsString, KhungCapsule.transform);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void XacNhanDungCapsule()
    {
        AudioManager.PlaySound("soundClick");
        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
        GameObject gdchoncapsule = giaodien2.transform.Find("GiaoDienChonCapsule").gameObject;
        GameObject KhungCapsule = gdchoncapsule.transform.Find("KhungCapsule").gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "XacNhanCapsule";

        if (gdchoncapsule.transform.Find("gdchoncs").gameObject.activeSelf)
        {
            datasend["data"]["giaodien"] = "ChonCapsule";
        }
        else
        {
            datasend["data"]["giaodien"] = "PhanRaCapsule";
        }
        datasend["data"]["nameCapsule"] = KhungCapsule.transform.GetChild(1).name;
        NetworkManager.ins.SendServer(datasend, XacNhanCapsuleOk);
        void XacNhanCapsuleOk(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;
                GameObject gdchoncapsule = giaodien2.transform.Find("GiaoDienChonCapsule").gameObject;

                GameObject gdchoncs = gdchoncapsule.transform.Find("gdchoncs").gameObject;
                GameObject gdphanracs = gdchoncapsule.transform.Find("gdphanracs").gameObject;

                GameObject KhungCapsule = gdchoncapsule.transform.Find("KhungCapsule").gameObject;
                GameObject contentCapsule = gdchoncapsule.transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;

                gdchoncs.transform.Find("txtgioihan").GetComponent<Text>().text = "";
                gdphanracs.transform.Find("txtgiaphanra").GetComponent<Text>().text = "0";
                gdchoncs.transform.Find("btnBatDau").gameObject.SetActive(false);
                gdphanracs.transform.Find("btnPhanRa").gameObject.SetActive(false);
                KhungCapsule.SetActive(false);
                Text txtnamecsl = gdchoncapsule.transform.Find("ttxnamecapsule").GetComponent<Text>();
                txtnamecsl.text = "";

                if (json["giaodien"].AsString == "PhanRaCapsule")
                {
                    Destroy(contentCapsule.transform.Find(json["nameCapsule"].AsString).gameObject);
                    SetItemEvent("ManhCongThuc", json["ManhCongThuc"].AsString);
                    CrGame.ins.OnThongBaoNhanh("Phân rã thành công!");
                }
                else
                {
                    GameObject KhungCapsuleChon = giaodien2.transform.Find("KhungCapsuleChon").gameObject;
                    KhungCapsuleChon.transform.GetChild(0).gameObject.SetActive(false);
                    KhungCapsuleChon.transform.GetChild(1).gameObject.SetActive(true);

                    GameObject KhungCapsuleChontrong = KhungCapsuleChon.transform.GetChild(1).gameObject;
                    for (int j = 0; j < allCapsuleSprite.Length; j++)
                    {
                        if (allCapsuleSprite[j].name == json["CapsuleChon"]["mau"].AsString)
                        {
                            KhungCapsuleChontrong.transform.GetChild(0).GetComponent<Image>().sprite = allCapsuleSprite[j];
                            break;
                        }
                    }
                    string nameitem = json["CapsuleChon"]["nameitem"].AsString;
                    Image imgcapsule = KhungCapsuleChontrong.transform.GetChild(1).GetComponent<Image>();
                    if (nameitem.Contains("Rong") && nameitem != "BuiRong")
                    {
                        imgcapsule.sprite = Inventory.LoadSpriteRong(nameitem + "1");
                    }
                    else
                    {
                        imgcapsule.sprite = Inventory.LoadSprite(nameitem);
                    }
                    imgcapsule.name = json["CapsuleChon"]["namecapsule"].AsString;
                    
                    TaoVien(json["CapsuleChon"]["mau"].AsString, KhungCapsuleChontrong.transform);
                    gdchoncapsule.SetActive(false);
                    GetGiaMoOgd2();
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    //public void XemGiaManhCT()
    //{
    //    AudioManager.PlaySound("soundClick");
    //    JSONClass datasend = new JSONClass();
    //    datasend["class"] = "EventRongThan2023";
    //    datasend["method"] = "XemGiaMuaManhCongThuc";
    //    NetworkManager.ins.SendServer(datasend, XemGiaManhCTOk);
    //    void XemGiaManhCTOk(JSONNode json)
    //    {
    //        if (json["status"].AsString == "0")
    //        {
    //            ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true).GetComponent<ThongBaoChon>();
    //            tbc.btnChon.onClick.RemoveAllListeners();

    //            tbc.txtThongBao.text = json["info"].AsString;
    //            tbc.btnChon.onClick.AddListener(XacNhanMuaManhCt);
    //        }
    //        else
    //        {
    //            CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
    //        }
    //    }
    //}

    //void XacNhanMuaManhCt()
    //{
    //    AudioManager.PlaySound("soundClick");
    //    JSONClass datasend = new JSONClass();
    //    datasend["class"] = "EventRongThan2023";
    //    datasend["method"] = "MuaManhCt";
    //    NetworkManager.ins.SendServer(datasend, MuaManhCtOk);
    //    void MuaManhCtOk(JSONNode json)
    //    {
    //        if (json["status"].AsString == "0")
    //        {
    //            //  SetItemEvent("ManhCongThuc", json["ManhCongThuc"].AsString);
    //            CrGame.ins.OnThongBaoNhanh(json["info"].AsString);
    //            AllMenu.ins.CloseMenu("MenuXacNhan");

    //            //  GameObject g = transform.Find("GiaoDien2").transform.Find("MenuCheTaoCapsule").transform.GetChild(0).gameObject;
    //            //  GameObject tren = g.transform.Find("tren").gameObject;

    //            // Text txtmanhcongthuc = tren.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();

    //            //  txtmanhcongthuc.text = "<color=lime>" + json["ManhCongThuc"].AsString + "/100</color>";
    //            int HoaTuyet = 0;
    //            if (Inventory.ins.ListItemThuong.ContainsKey("itemHoaTuyet")) HoaTuyet = int.Parse(Inventory.ins.ListItemThuong["itemHoaTuyet"].transform.GetChild(0).GetComponent<Text>().text);
    //            LoadManhCongThuc(int.Parse(json["ManhCongThuc"].AsString), HoaTuyet);
    //        }
    //        else
    //        {
    //            CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
    //        }
    //    }
    //}

    public void QuayMayMan(string lanquay)
    {
        AudioManager.PlaySound("soundClick");
        if (KimQuay.ins.quay) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "QuayMayMan";
        datasend["data"]["solanquay"] = lanquay;
        NetworkManager.ins.SendServer(datasend, ResultQuay);
        void ResultQuay(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                KimQuay.ins.nameQua = json["allqua"][0]["nameobj"].AsString;
                KimQuay.ins.quay = true;
                allquanhan = json;
                foreach (string key in allquanhan["allItem"].AsObject.Keys)
                {
                    SetItemEvent(key, allquanhan["allItem"][key].AsString);
                }
                LoadMocQuaTichLuy(json["quatichluy"], json["solanquay"].AsString);
                //   StartCoroutine(Delay());
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private JSONNode allquanhan;

    public IEnumerator DelayHienQua()
    {
        yield return new WaitForSeconds(0.4f);
        GameObject giaodien1 = transform.Find("GiaoDien1").gameObject;
        JSONNode allqua = allquanhan["allqua"];
        GameObject panelnhanqua = transform.Find("PanelNhanQua").gameObject;

        panelnhanqua.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        SetYeuCau(allquanhan["YeuCau"]);
        GameObject objallqua = panelnhanqua.transform.GetChild(1).gameObject;
        objallqua.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        objallqua.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameObject allquaa = objallqua.transform.GetChild(1).gameObject;
        GameObject objqua = allquaa.transform.GetChild(0).gameObject;
        allquaa.GetComponent<GridLayoutGroup>().enabled = true;
        for (int i = 0; i < allqua.Count; i++)
        {
            GameObject qua = Instantiate(objqua, transform.position, Quaternion.identity);
            qua.transform.SetParent(allquaa.transform, false);
            qua.SetActive(true);
            if (allqua.Count == 1)
            {
                allquaa.GetComponent<GridLayoutGroup>().enabled = false;
                qua.transform.position = new Vector3(allquaa.transform.position.x - 0.5f, allquaa.transform.position.y + 0.6f);
            }
            for (int k = 0; k < alltuchat.Length; k++)
            {
                if (alltuchat[k].name == allqua[i]["tuchat"].Value)
                {
                    qua.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = alltuchat[k];
                    break;
                }
            }

            GameObject objtrong = qua.transform.GetChild(0).transform.GetChild(0).gameObject;
            if (allqua[i]["loaiitem"].Value == "Item")
            {
                // if (objtrong.transform.Find("vien")) Destroy(objtrong.transform.Find("vien"));
                objtrong.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(allqua[i]["nameitem"].Value);
                objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                objtrong.name = "item" + allqua[i]["nameitem"].Value;
                objtrong.GetComponent<infoitem>().enabled = true;
            }
            else if (allqua[i]["loaiitem"].Value == "ItemEvent")
            {
                for (int j = 0; j < allItemEvent.Length; j++)
                {
                    if (allItemEvent[j].name == allqua[i]["nameitem"].Value)
                    {
                        Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                        img.sprite = allItemEvent[j];
                        img.SetNativeSize();
                        objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                        objtrong.GetComponent<infoitem>().enabled = true;
                        objtrong.name = "item" + allqua[i]["nameitem"].Value;
                        break;
                    }
                }
            }


            //if (allqua[i]["tuchat"].Value == "do")
            //{
            //    GameObject vien = Instantiate(VienDo, transform.position, Quaternion.identity);
            //    vien.transform.SetParent(objtrong.transform, false);
            //    vien.transform.position = objtrong.transform.position;
            //    vien.transform.localScale = new Vector3(1.5f, 1.5f);
            //    vien.name = "vien";
            //    vien.SetActive(true);
            //}
            //else if (allqua[i]["tuchat"].Value == "vang")
            //{
            //    GameObject vien = Instantiate(VienVang, transform.position, Quaternion.identity);
            //    vien.transform.SetParent(objtrong.transform, false);
            //    vien.transform.position = objtrong.transform.position;
            //    vien.transform.localScale = new Vector3(1.5f, 1.5f);
            //    vien.name = "vien";
            //    vien.SetActive(true);
            //}

            //for (int j = 0; j < spritetuchat.Length; j++)
            //{
            //    if (spritetuchat[j].name == allqua[i]["tuchat"].Value)
            //    {
            //        objtrong.GetComponent<Image>().sprite = spritetuchat[j];
            //        break;
            //    }
            //}

            yield return new WaitForSeconds(0.02f);
            qua.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            allquanhan = null;
        }
        objallqua.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
    }
    public void OpenMenuTrieuHoi()
    {
        AudioManager.PlaySound("soundClick");
        GameObject menutrieuhoi = GetCreateMenu("MenuTrieuHoi", transform, false);//transform.Find("MenuTrieuHoi").gameObject;
        GameObject NroObj = menutrieuhoi.transform.GetChild(0).transform.Find("NgocRongobj").gameObject;
        byte ok = 0;
        for (int i = 0; i < 7; i++)
        {
            if (int.Parse(txtallItem[i].text) > 0)
            {
                ok += 1;
                NroObj.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            else NroObj.transform.GetChild(i).GetComponent<Image>().color = new Color32(82, 82, 82, 255);
        }
        Button btntrieuhoi = menutrieuhoi.transform.GetChild(0).transform.Find("btnTrieuHoi").GetComponent<Button>();
        if (ok == 7)
        {
            btntrieuhoi.onClick.AddListener(TrieuHoiRongThan);
            btntrieuhoi.interactable = true;
        }
        else menutrieuhoi.transform.GetChild(0).transform.Find("btnTrieuHoi").GetComponent<Button>().interactable = false;
        menutrieuhoi.transform.GetChild(0).transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuTrieuHoi"); });
        menutrieuhoi.SetActive(true);
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        AudioManager.PlaySound("soundClick");
        MenuTrieuHoiRongVangBac mn = AllMenu.ins.GetCreateMenu("MenuTrieuHoiRongVangBac", gameObject, false, 1).GetComponent<MenuTrieuHoiRongVangBac>();
        mn.Setnamerong = namerong;
        mn.gameObject.SetActive(true);
    }
    public void TrieuHoiRongThan()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "TrieuHoiRongThan";
        NetworkManager.ins.SendServer(datasend, TrieuHoiOk);
        void TrieuHoiOk(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                StartCoroutine(Delay());
                IEnumerator Delay()
                {
                    DestroyMenu("MenuTrieuHoi");
                    GameObject hieuungnro = GetCreateMenu("HieuUngTrieuHoiRongThan", transform, false);
                    //transform.Find("MenuTrieuHoi").gameObject.SetActive(false);

                    GameObject rongthan = GetCreateMenu("MenuRongThan", transform, false);
                    GameObject alldieuuoc = rongthan.transform.Find("DieuUoc").gameObject;
                    for (int i = 0; i < alldieuuoc.transform.childCount; i++)
                    {
                        alldieuuoc.transform.GetChild(i).transform.GetChild(0).GetComponent<Button>().onClick.AddListener(GetInfoSkill);
                    }
                    Text txt = rongthan.transform.GetChild(1).GetComponent<Text>(); txt.fontSize = 50;
                    txt.text = "Hãy chọn điều ước ngươi muốn...";
                    hieuungnro.gameObject.SetActive(true);
                    yield return new WaitForSeconds(3.5f);
                    DestroyMenu("HieuUngTrieuHoiRongThan");
                    rongthan.transform.GetChild(0).gameObject.SetActive(true);
                    rongthan.gameObject.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    //   rongthan.transform.GetChild(0).gameObject.SetActive(false);
                    // DestroyMenu("MenuRongThan");
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }

        }
    }

    void SetYeuCau(JSONNode yeucau)
    {
        GameObject giaodien1 = transform.Find("GiaoDien1").gameObject;
        GameObject btnX1 = giaodien1.transform.Find("btnquayX1").gameObject;
        if (yeucau["yeucauX1"]["name"].Value == "KimCuong")
        {
            btnX1.transform.GetChild(2).gameObject.SetActive(false);
            btnX1.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            btnX1.transform.GetChild(2).gameObject.SetActive(true);
            btnX1.transform.GetChild(3).gameObject.SetActive(false);
        }
        btnX1.transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu: " + yeucau["yeucauX1"]["soluong"].Value;

        GameObject btnX10 = giaodien1.transform.Find("btnquayX10").gameObject;
        if (yeucau["yeucauX10"]["name"].Value == "KimCuong")
        {
            btnX10.transform.GetChild(2).gameObject.SetActive(false);
            btnX10.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            btnX10.transform.GetChild(2).gameObject.SetActive(true);
            btnX10.transform.GetChild(3).gameObject.SetActive(false);
        }
        btnX10.transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu: " + yeucau["yeucauX10"]["soluong"].Value;
    }
    public void XacNhanQua()
    {
        AudioManager.PlaySound("soundClick");
        GameObject hopqua = btnHopQua;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject panelnhanqua = transform.Find("PanelNhanQua").gameObject;
            GameObject objallqua = panelnhanqua.transform.GetChild(1).transform.GetChild(1).gameObject;
            panelnhanqua.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;

            for (int i = 1; i < objallqua.transform.childCount; i++)
            {
                GameObject qua = objallqua.transform.GetChild(i).gameObject;
                StartCoroutine(delaydestroy(qua));
                yield return new WaitForSeconds(0.1f);
            }
            panelnhanqua.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            panelnhanqua.transform.GetChild(0).transform.LeanScale(new Vector3(0, 0.75f), 0.5f);
            yield return new WaitForSeconds(0.7f);
            panelnhanqua.SetActive(false);
            panelnhanqua.transform.GetChild(1).gameObject.SetActive(false);
            panelnhanqua.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            panelnhanqua.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
            for (int i = 1; i < objallqua.transform.childCount; i++)
            {
                Destroy(objallqua.transform.GetChild(i).gameObject);
            }
        }
        IEnumerator delaydestroy(GameObject qua)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject quatrong = qua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            //      GameObject namequa = qua.transform.GetChild(0).transform.GetChild(0).gameObject;

            quatrong.transform.SetParent(CrGame.ins.panelLoadDao.transform.parent.transform, false);
            QuaBay quabay = quatrong.AddComponent<QuaBay>();
            quabay.vitribay = hopqua;
            qua.SetActive(false);
            //  Destroy(qua, 5);
        }
    }
    void SetItemEvent(string name, string soluong)
    {
        for (int i = 0; i < txtallItem.Length; i++)
        {
            //debug.Log(txtallItem[i].transform.parent.name);
            if (txtallItem[i].transform.parent.name == "item" + name)
            {
                txtallItem[i].text = soluong;
                break;
            }
        }
    }
    string GetItemEvent(string name)
    {
        for (int i = 0; i < txtallItem.Length; i++)
        {
            //debug.Log(txtallItem[i].transform.parent.name);
            if (txtallItem[i].transform.parent.name == "item" + name)
            {
                return txtallItem[i].text;
            }
        }
        return "0";
    }
    void AddItemEvent(string name, int soluong)
    {
        for (int i = 0; i < txtallItem.Length; i++)
        {
            if (txtallItem[i].transform.parent.name == "item" + name)
            {
                txtallItem[i].text = (int.Parse(txtallItem[i].text) + soluong).ToString();
                break;
            }
        }
    }
    bool uoc = false;

    public void GetInfoSkill()
    {
        AudioManager.PlaySound("soundClick");
        if (uoc) return;
        string nameskill = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "GetInfoSkill";
        datasend["data"]["nameDieuUoc"] = nameskill;
        NetworkManager.ins.SendServer(datasend, InfoSkill);
        void InfoSkill(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menurongthan = menuevent["MenuRongThan"];
                Text txt = menurongthan.transform.Find("txtdieuuoc").GetComponent<Text>();
                txt.text = json["info"].AsString;
                txt.fontSize = 35;
                Button btnuoc = menurongthan.transform.Find("btnNhan").GetComponent<Button>();
                btnuoc.onClick.RemoveAllListeners();
                btnuoc.onClick.AddListener(delegate { UocRong(json["namedieuuoc"].AsString); });
                btnuoc.gameObject.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }

        }
    }

    void UocRong(string nameDieuUoc)
    {
        AudioManager.PlaySound("soundClick");
        if (uoc) return;
        uoc = true;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventRongThan2023";
        datasend["method"] = "UocRong";
        datasend["data"]["nameDieuUoc"] = nameDieuUoc;
        NetworkManager.ins.SendServer(datasend, uocok);
        void uocok(JSONNode json)
        {
            GameObject menurongthan = menuevent["MenuRongThan"];
            if (json["status"].AsString == "0")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    Text txt = menurongthan.transform.Find("txtdieuuoc").GetComponent<Text>();
                    txt.text = "Điều ước của ngươi đã thành sự thật, Hẹn gặp lại ngươi vào sự kiện năm sau!";
                    txt.fontSize = 40;
                    menurongthan.transform.Find("btnNhan").gameObject.SetActive(false);
                    yield return new WaitForSeconds(3f);
                    //transform.Find("MenuRongThan").gameObject.SetActive(false);
                    DestroyMenu("MenuRongThan");
                    foreach (string key in json["allItem"].AsObject.Keys)
                    {
                        SetItemEvent(key, json["allItem"][key].AsString);
                    }
                    uoc = false;
                    //transform.LeanColor();
                }
            }
            else
            {
                Text txt = menurongthan.transform.Find("txtdieuuoc").GetComponent<Text>();
                txt.text = json["message"].AsString;
                txt.fontSize = 40;
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(3f);
                    //menurongthan.SetActive(false);
                    DestroyMenu("MenuRongThan");
                    uoc = false;
                }
            }

        }
    }

    public void OpenSkGioiHan()
    {
        AudioManager.PlaySound("soundClick");
        AllMenu.ins.OpenMenuTrenCung("MenuSuKienGioiHan");
    }
    public void VeNha()
    {

        AudioManager.PlaySound("soundClick");
        CrGame.ins.giaodien.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        // gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuEvent7VienNgocRong2023");
    }
}
