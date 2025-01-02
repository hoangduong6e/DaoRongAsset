using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventDaiChienThuyQuai : EventManager
{
    // Start is called before the first frame update
    public GameObject SocCon, SocConThuong, PanelNui, MauBachTuoc, ScrollViewallMocQua;
    public Transform point0, pontnhatren, GAllItem, BachTuoc;
    public Image fillHp;
    public float HpBachTuoc, MaxHpBachTuoc;
    private bool duoctha = true;
    public static EventDaiChienThuyQuai ins;
    public SpriteRenderer materialXucTu;
    private bool isDamagedActive = false;
    //    public bool isDamaged = false;
    private Material material;
    private int XucTuHienTai;
    private JSONNode ArrHHp;
    public GameObject imgQuaDanhBaiBachTuoc, btnTieuDietXucTu, GiaoDienDoiHinhCHienDau;
    public Transform objtrencung;
    // public GameObject[] AllObjectSetParent;
    private string infoqua = "";
    protected override void ABSAwake()
    {
        ins = this;
        material = materialXucTu.material;

        //AllObjectSetParent = new GameObject[] { 
        //    PanelNui, GAllItem.gameObject,
        //    MauBachTuoc, ScrollViewallMocQua, 
        //    GiaoDienCheDoDanh, imgQuaDanhBaiBachTuoc
        //};

        //PanelNui.transform.SetParent(CrGame.ins.trencung);
        //GAllItem.transform.SetParent(CrGame.ins.trencung);
        //MauBachTuoc.transform.SetParent(CrGame.ins.trencung);
        //ScrollViewallMocQua.transform.SetParent(CrGame.ins.trencung);
        //GiaoDienCheDoDanh.transform.SetParent(CrGame.ins.trencung);
        //imgQuaDanhBaiBachTuoc.transform.SetParent(CrGame.ins.trencung);
    }
    public static string CheDoDanh;
    public static string methodd;
    public void XemGiaDanhXucTu()
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "GetGiaMuaLuot";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                if (json["message"].AsString != "") EventManager.OpenThongBaoChon(json["message"].AsString, delegate { VaoMapDanhXucTu("DanhXucTu"); });
                else VaoMapDanhXucTu("DanhXucTu");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void XemCanQuet()
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "GetGiaMuaLuot";
        datasend["data"]["canquet"] = "";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                if (json["message"].AsString != "") EventManager.OpenThongBaoChon(json["message"].AsString, delegate { CanQuet(); });
                else CanQuet();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void VaoMapDanhXucTu(string method)
    {
        methodd = method;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = method;
        if (methodd != "DanhXucTuKetLieu") datasend["data"]["chedodanh"] = CheDoDanh;
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                VienChinh.vienchinh.GetDoiHinh("BossXucTu", CheDoDau.XucTu);

                NetworkManager.ins.vienchinh.TruDo.SetActive(true);

                NetworkManager.ins.vienchinh.TruXanh.SetActive(true);

                TaoXucTu(json["NameBoss"].AsString, json["dame"].AsFloat);

                float chia = json["hp"].AsFloat / 3;

                float[] hplansu = new float[] { chia, chia, chia };

                SetHpLanSu(hplansu);
                // VienChinh.vienchinh.SetBGMap("BGLanSu");
                VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetSprite("BGBachTuoc");
                btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
                Destroy(objtrencung.gameObject);
                AllMenu.ins.DestroyMenu(nameof(EventDaiChienThuyQuai));
                //  gameObject.SetActive(false);
                //  VeNha();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void CanQuet()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "CanQuet";
        datasend["data"]["chedodanh"] = CheDoDanh;
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                GiaoDienDoiHinhCHienDau.transform.Find("VeChoiNhanh").transform.GetChild(1).GetComponent<Text>().text = json["VeChoiNhanh"].AsString;
                GiaoDienDoiHinhCHienDau.transform.Find("itemHuyHieuSocCon").transform.GetChild(1).GetComponent<Text>().text = json["HuyHieuSocCon"].AsString;
                GiaoDienDoiHinhCHienDau.transform.GetChild(0).transform.Find("txtDaDanh").GetComponent<Text>().text = "Hôm nay bạn đã đánh <color=lime>" + json["luotdanh"].AsString + "/" + json["maxluotdanh"].AsString + "</color> lượt";
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private bool enablepanelbonus = false;
    public void XemBonus(bool enable)
    {
        enablepanelbonus = enable;
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject panel = btnchon.transform.GetChild(0).gameObject;
        if(!enablepanelbonus)
        {
            panel.SetActive(false);
            return;
        }
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "XemBonus";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            //  debug.Log(json.ToString()) ;
            if (json["status"].AsString == "0")
            {
                if(enablepanelbonus)
                {
                    panel.transform.GetChild(0).GetComponent<Text>().text = json["str"].AsString;
                    panel.SetActive(true);
                }
             
            }
        }
}
    public static Action KetQua(KetQuaTranDau kq, bool quayve = false)
    {
        void kqq()
        {
            string mt = "KetQua";
            if (methodd == "DanhXucTuKetLieu") mt = "KetQuaKetLieu";
            JSONClass datasend = new JSONClass();
            datasend["class"] = EventManager.ins.nameEvent;
            datasend["method"] = mt;
            datasend["data"]["kq"] = kq.ToString();
            if (methodd != "DanhXucTuKetLieu") datasend["data"]["chedodanh"] = CheDoDanh;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
              //  debug.Log(json.ToString()) ;
                if (json["status"].AsString == "0")
                {
                    if (quayve)
                    {
                        CrGame.ins.OpenMenuEvent();
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
                        GiaoDienPVP.ins.thongtin.text = "Bạn đã đánh bại Boss Xúc Tu";
                    }
                    GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
                    GiaoDienPVP.ins.btnSetting.SetActive(true);
                    GiaoDienPVP.ins.spriteWin.SetNativeSize();
                    if (methodd == "DanhXucTuKetLieu") CrGame.ins.OnThongBaoNhanh(json["infoqua"].AsString);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
        return kqq;

    }
    private void TaoXucTu(string nameBoss, float dame)
    {
        GameObject LanSu = Instantiate(GetObjectLanSu(nameBoss), transform.position, Quaternion.identity);
        LanSu.transform.SetParent(VienChinh.vienchinh.TeamDo.transform, false);
        LanSu.transform.position = VienChinh.vienchinh.TeamDo.transform.position;
        DragonPVEController dragonPVEController = LanSu.transform.Find("SkillDra").GetComponent<DragonPVEController>();
        dragonPVEController.dame = dame;
     //   dragonPVEController.giapso = 200;
       // dragonPVEController.xuyengiap = 99999;
        VienChinh.vienchinh.TruDo.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void SetHpLanSu(float[] hp)
    {
        TruVienChinh tru = VienChinh.vienchinh.TruDo.GetComponent<TruVienChinh>();
        tru.allmau = 2;
        for (int i = 0; i < tru.Hp.Length; i++)
        {
            tru.MaxHp[i] = hp[i];
            tru.Hp[i] = tru.MaxHp[i];
        }
        tru.LoadImgHp();
        tru.actionwin = delegate {
            tru.actionwin = null;
            CrGame.ins.SetPanelThangCap(true);
            CrGame.ins.txtThangCap.text = "";
            VienChinh.vienchinh.OffThangCap();
            Transform xuctu = VienChinh.vienchinh.TeamDo.transform.GetChild(1).transform;
            VienChinh.vienchinh.StartCoroutine(MoveRight());
           
            IEnumerator MoveRight()
            {
                // Tốc độ di chuyển của đối tượng
                float moveSpeed = 2f;
                // Khoảng cách di chuyển
                float moveDistance = 5f;

                float startX = xuctu.transform.position.x;
                float targetX = startX + moveDistance;
                float elapsedTime = 0f;

                while (xuctu.transform.position.x < targetX)
                {
                    if (xuctu == null) break;
                    // Tính toán khoảng thời gian đã trôi qua
                    elapsedTime += Time.deltaTime;
                    // Tính toán vị trí mới của đối tượng
                    float newX = Mathf.Lerp(startX, targetX, elapsedTime * moveSpeed / moveDistance);
                    xuctu. transform.position = new Vector3(newX, xuctu.transform.position.y, xuctu.transform.position.z);

                    // Chờ một frame
                    yield return null;
                }

                // Đảm bảo đối tượng đã đến đúng vị trí đích
                xuctu.transform.position = new Vector3(targetX, xuctu.transform.position.y, xuctu.transform.position.z);
            
            }
        };
    }
    
    public GameObject GetObjectLanSu(string nameBoss)
    {
        return Inventory.LoadObjectResource("GameData/EventDaiChienThuyQuai/" + nameBoss);

    }
    protected override void DiemDanhOk(JSONNode json)
    {
        debug.Log(json.ToString());
        SetTextItem(json);
    }
    public void TrangBi()
    {
        AudioManager.SoundClick();
        GameObject tc = GameObject.FindGameObjectWithTag("trencung");
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", tc, true, CrGame.ins.panelLoadDao.transform.GetSiblingIndex());
    }
    public void GetDoiHinh()
    {
        AudioManager.SoundClick();
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CheDoDanh = btnchon.name;
        if (btnchon.transform.GetChild(0).gameObject.activeSelf) return;
        GameObject gDoiHinhChienDau = GiaoDienDoiHinhCHienDau.transform.GetChild(0).gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDoiHinhChienDau";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "0")
            {
                gDoiHinhChienDau.transform.Find("txtCheDo").transform.GetChild(0).GetComponent<Text>().text = "Chế độ:\r\n<size=35>"+GetTxtCheDo(btnchon.name)+"</size>";
                gDoiHinhChienDau.transform.Find("txtDaDanh").GetComponent<Text>().text = "Hôm nay bạn đã đánh <color=lime>" + json["luotdanh"].AsString + "/" + json["maxluotdanh"].AsString + "</color> lượt";
                gDoiHinhChienDau.transform.Find("txtBonus").GetComponent<Text>().text = "Tổng Bonus quà khi chiến thắng <color=lime>" + json["tongbonus"].AsString + "%</color>";

                GameObject ObjDoiHinhDaSanSang = gDoiHinhChienDau.transform.Find("ObjDoiHinhDaSanSang").transform.GetChild(0).gameObject;

                GiaoDienDoiHinhCHienDau.gameObject.SetActive(true);
                GiaoDienDoiHinhCHienDau.transform.Find("itemHuyHieuSocCon").transform.GetChild(1).GetComponent<Text>().text = json["HuyHieuSocCon"].AsString;
                GiaoDienDoiHinhCHienDau.transform.Find("VeChoiNhanh").transform.GetChild(1).GetComponent<Text>().text = json["VeChoiNhanh"].AsString;

                for (int i = 0; i < ObjDoiHinhDaSanSang.transform.childCount; i++)
                {
                    GameObject childi = ObjDoiHinhDaSanSang.transform.GetChild(i).gameObject;
                    if (!json["DoiHinh"][childi.name].AsBool)
                    {
                        childi.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 154);
                        childi.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 175);
                    }
                    else childi.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
            }
        }
    }
    private string GetTxtCheDo(string s)
    {
        switch(s)
        {
            case "de": return "Dễ";
            case "thuong":return "<color=cyan>Thường</color>";
            case "kho":return "<color=orange>Khó</color>";
            case "acmong":return "<color=red>Ác Mộng</color>";
        }
        return "Dễ";
    }    
    public void DanhBachTuoc(float dame)
    {
        HpBachTuoc -= dame;
        fillHp.fillAmount = HpBachTuoc / MaxHpBachTuoc;
        MauBachTuoc.transform.GetChild(2).GetComponent<Text>().text = (MaxHpBachTuoc > 0) ? Math.Round((HpBachTuoc / MaxHpBachTuoc) * 100, 1) + "%" : "0%";
        if (!isDamagedActive) StartCoroutine(DamageRoutine());
        if(HpBachTuoc <= 0)
        {
            btnTieuDietXucTu.SetActive(true);
            //if(XucTuHienTai < ArrHHp.Count)
            //{
            //    XucTuHienTai += 1;
            //    HpBachTuoc = ArrHHp[XucTuHienTai - 1].AsInt;
            //    MaxHpBachTuoc = HpBachTuoc;
            //    MauBachTuoc.transform.GetChild(1).GetComponent<Text>().text = "Xúc tu " + XucTuHienTai;
            //    fillHp.fillAmount = 1;
            //    if(XucTuHienTai == ArrHHp.Count)
            //    {
            //        MauBachTuoc.transform.GetChild(1).GetComponent<Text>().text = "Đầu Bạch Tuộc Ma";
            //        BachTuoc.gameObject.SetActive(false);
            //        transform.Find("BachTuocMa").gameObject.SetActive(true);
            //    }    
            //}    
               
            //CrGame.ins.OnThongBaoNhanh(infoqua, 3);
            //infoqua = "";
        }
    }
    private IEnumerator DamageRoutine()
    {
        material.SetFloat("_IsDamaged", 1.0f);
        isDamagedActive = true;
        yield return new WaitForSeconds(0.2f);
        material.SetFloat("_IsDamaged", 0.0f);
        isDamagedActive = false;
    }
    // Update is called once per frame
    public void ParseData(JSONNode json)
    {
        AudioManager.SetSoundBg("nhacnen1");
        //debug.Log(json.ToString());
        SetTextItem(json);

        //for (int i = 0; i < AllObjectSetParent.Length; i++)
        //{
        //    AllObjectSetParent[i].transform.SetParent(CrGame.ins.trencung);
        //}
        objtrencung.transform.SetParent(CrGame.ins.trencung);
        objtrencung.transform.SetSiblingIndex(1);
        //GAllItem.transform.SetParent(CrGame.ins.trencung);
        //MauBachTuoc.transform.SetParent(CrGame.ins.trencung);
        //ScrollViewallMocQua.transform.SetParent(CrGame.ins.trencung);
        //GiaoDienCheDoDanh.transform.SetParent(CrGame.ins.trencung);
        //imgQuaDanhBaiBachTuoc.transform.SetParent(CrGame.ins.trencung);
        //btnTieuDietXucTu.transform.SetParent(CrGame.ins.trencung);
        //GiaoDienDoiHinhCHienDau.transform.SetParent(CrGame.ins.trencung);
        MauBachTuoc.transform.GetChild(1).GetComponent<Text>().text = "Xúc tu " + json["data"]["XucTuHienTai"].AsString;
        ArrHHp = json["HpXucTu"];
        XucTuHienTai = json["data"]["XucTuHienTai"].AsInt;

        HpBachTuoc = json["data"]["HpXucTu"].AsFloat;
        MaxHpBachTuoc = json["MaxHpXucTu"].AsFloat;
        fillHp.fillAmount = HpBachTuoc / MaxHpBachTuoc;
        MauBachTuoc.transform.GetChild(2).GetComponent<Text>().text = (MaxHpBachTuoc > 0) ? Math.Round((HpBachTuoc / MaxHpBachTuoc) * 100, 1) + "%" : "0%";

        LoadMocQuaGD1(json["data"]["QuaTichLuyGD1"], json["allMocDiemGD1"], json["data"]["SoSocTha"].AsInt);
        SetGgCheDo(json["data"]["allchedo"]);
        if (HpBachTuoc <= 0)
        {
            btnTieuDietXucTu.SetActive(true);
        }
        else btnTieuDietXucTu.SetActive(false);
        if (XucTuHienTai >= ArrHHp.Count - 1)
        {
            MauBachTuoc.transform.GetChild(1).GetComponent<Text>().text = "Đầu Bạch Tuộc Ma";
            BachTuoc.gameObject.SetActive(false);
            transform.Find("BachTuocMa").gameObject.SetActive(true);
            btnTieuDietXucTu.SetActive(true);
        }
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        btnHopQua.transform.SetParent(objtrencung.transform);
    }
    public GameObject GiaoDienCheDoDanh;
 //   public Sprite[] spritesCheDo;
    private void SetGgCheDo(JSONNode json)
    {
        GameObject g = GiaoDienCheDoDanh.transform.GetChild(0).gameObject;
        string[] arr = new string[] {"thuong","kho","acmong" };
        for (int i = 0; i < arr.Length; i++)
        {
            if (json[arr[i]].AsBool)
            {
                Transform tf = g.transform.Find(arr[i]);
                Image img = tf.GetComponent<Image>();
              //  img.sprite = spritesCheDo[i];
              //  img.SetNativeSize();
                tf.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }    
    public Sprite sprite1, sprite2, chuaduocnhan, duocnhan, danhan;
    private void LoadMocQuaGD1(JSONNode allmocqua, JSONNode diemmocqua, int banhtrungthu)
    {
      //  GameObject ScrollViewallMocQua = transform.Find("ScrollViewallMocQua").gameObject;
        ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = banhtrungthu.ToString();
        GameObject content = ScrollViewallMocQua.transform.GetChild(1).transform.GetChild(0).gameObject;
        int tru = 0;
        // int trumocqua = diemmocqua[allmocqua.Count - 1].AsInt;
        for (int i = allmocqua.Count - 1; i >= 0; i--)
        {
            int mocqua = diemmocqua[allmocqua.Count - 1 - i].AsInt;
            //if(banhtrungthu >= mocqua)
            //{

            //}
            GameObject qua = content.transform.GetChild(i).gameObject;
            Image fill = qua.transform.GetChild(1).GetComponent<Image>();
            if (banhtrungthu >= mocqua)
            {
                fill.fillAmount = 1;
                tru = banhtrungthu - mocqua;
                //trumocqua -= mocqua;
            }
            else
            {
                if (i < allmocqua.Count - 1)
                {
                    debug.Log("tru " + tru);
                    int trumocqua = mocqua - diemmocqua[allmocqua.Count - 1 - i - 1].AsInt;
                    debug.Log("trumocqua " + trumocqua);
                    fill.fillAmount = (float)tru / (float)trumocqua;
                    tru = 0;
                }
                else
                {
                    fill.fillAmount = (float)banhtrungthu / diemmocqua[0].AsFloat;
                }


            }
            string trangthai = allmocqua[allmocqua.Count - 1 - i].AsString;
            if (trangthai == "chuaduocnhan")
            {
                Text txt = qua.transform.GetChild(5).GetComponent<Text>();
                txt.gameObject.SetActive(true);
                txt.text = mocqua.ToString();
                qua.transform.GetChild(3).GetComponent<Image>().sprite = sprite1;
                qua.transform.GetChild(2).GetComponent<Image>().sprite = chuaduocnhan;
            }
            else if (trangthai == "duocnhan")
            {
                qua.transform.GetChild(4).gameObject.SetActive(true);
                qua.transform.GetChild(5).gameObject.SetActive(false);
                qua.transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                qua.transform.GetChild(2).GetComponent<Image>().sprite = duocnhan;
                if (!qua.transform.GetChild(4).GetComponent<Button>())
                {
                    Button btn = qua.transform.GetChild(4).gameObject.AddComponent<Button>();
                    btn.onClick.AddListener(NhanQuaTichLuyGd1);
                }
            }
            else if (trangthai == "danhan")
            {
                qua.transform.GetChild(4).gameObject.SetActive(false);
                Text txt = qua.transform.GetChild(5).GetComponent<Text>();
                txt.gameObject.SetActive(true);
                txt.text = "<color=red>Đã nhận</color>";
                qua.transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                qua.transform.GetChild(2).GetComponent<Image>().sprite = danhan;
            }
        }
    }
    private void NhanQuaTichLuyGd1()
    {
        AudioManager.PlaySound("soundClick");
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanQuaTichLuyGd1";
        datasend["data"]["mocqua"] = btnnhan.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["SoSocTha"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void SetTextItem(JSONNode json)
    {
        string[] allitem = new string[] { "SocCon", "AoGiap", "MuGiap", "VuKhi" };
        for (int i = 0; i < allitem.Length; i++)
        {
            string key = allitem[i];
            string s = (int.Parse(json["data"][key].AsString) >= 3) ? json["data"][key].AsString + "/3" : "<color=red>" + json["data"][key].AsString + "</color>/3";
            GAllItem.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = s;
        }
    }    
    public void ThaSoc()
    {
        AudioManager.SoundClick();
        if (!duoctha) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "ThaSocCon";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                duoctha = false;
                SetTextItem(json);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject soc = Instantiate(SocConThuong, transform.position, Quaternion.identity);
                        soc.transform.SetParent(transform, false);
                        soc.transform.position = point0.transform.position;
                        soc.SetActive(true);
                        yield return new WaitForSeconds(0.1f);
                    }
                    duoctha = true;
                }
                LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["SoSocTha"].AsInt);
                if (json["infoqua"].AsString != "") infoqua = json["infoqua"].AsString;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
            }
        }
    }
    public void ThaSocGiap()
    {
        GameObject soc = Instantiate(SocCon, transform.position, Quaternion.identity);
        soc.transform.SetParent(transform, false);
        soc.transform.position = pontnhatren.transform.position;
        soc.SetActive(true);
    }
    public void ChangeSprite(Sprite sprite)
    {
        GameObject btnChon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Image img = btnChon.GetComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
    }
    public void VeNha()
    {
        //Destroy(PanelNui);
        //Destroy(GAllItem);
        //Destroy(MauBachTuoc);
        //PanelNui.transform.SetParent(transform);
        //GAllItem.transform.SetParent(transform);
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);

        AudioManager.PlaySound("soundClick");

  

        Transform dangodao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");

        Vector3 vec = dangodao.transform.position;
        vec.z = -10;
        CrGame.ins.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        Destroy(objtrencung.gameObject);
        AllMenu.ins.DestroyMenu(nameof(EventDaiChienThuyQuai));
        // AllMenu.ins.DestroyMenu("MenuEventValentine2024");
    }
    string nameitemmua;
    string txtTv(string s)
    {
        switch(s)
        {
            case "SocCon":return "Sóc Con";
            case "VuKhi":return "Vũ Khí";
            case "MuGiap":return "Mũ Giáp";
            case "AoGiap":return "Áo Giáp";
            case "VeChoiNhanh":return "Vé chơi nhanh";
        }
        return "";
    }
    Text txtupdate;
    public void OpenMenuMuaXeng()
    {
        AudioManager.SoundClick();
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        txtupdate = btnchon.transform.parent.GetComponentsInChildren<Text>()[0];
        nameitemmua = btnchon.transform.parent.name;
        GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaItem", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        if (nameitemmua == "VeChoiNhanh") g.transform.Find("txttanggia").GetComponent<Text>().text = "";
        else g.transform.Find("txttanggia").GetComponent<Text>().text = "Giá tăng khi mua nhiều trong ngày, sẽ được reset khi qua ngày mới.";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = txtTv(nameitemmua);// tên giao diện
        Image imgitem = g.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        imgitem.sprite = EventManager.ins.GetSprite(nameitemmua);
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
            GameObject menu = EventManager.ins.menuevent["MenuMuaItem"];
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
        GameObject menu = EventManager.ins.menuevent["MenuMuaItem"];
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
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaItem"))
                {
                    GameObject menu = EventManager.ins.menuevent["MenuMuaItem"];
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
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaItem"))
                {
                    ExitMenuQueThu();
                  //  SetXeng(json["soxeng"].AsString);
                    CrGame.ins.OnThongBaoNhanh("Mua thành công!");
                    txtupdate.text = json["soitem"].AsString;
                    if (txtupdate.transform.parent.transform.parent.name == "Item")
                    {
                        string s = (json["soitem"].AsInt >= 3) ? json["soitem"].AsString + "/3" : "<color=red>" + json["soitem"].AsString + "</color>/3";
                        txtupdate.text = s;
                    }
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
        EventManager.ins.DestroyMenu("MenuMuaItem"); soluongMuaQueThu = 1;
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
    public void OpenGiaoDienRuong()
    {
        AudioManager.SoundClick();
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDataRuong";
        datasend["data"]["nameruong"] = "RuongThanBi";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                // GameObject menu = CrGame.ins.trencung.transform.Find("GiaoDienRuongThanBi").gameObject;
                GameObject menu = GetCreateMenu("GiaoDienRuongThanBi", CrGame.ins.trencung.transform,true,objtrencung.transform.GetSiblingIndex()+1);
                menu.GetComponent<GiaoDienRuongThanBi>().ParseData(json, btnchon.transform.parent.name);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
}
