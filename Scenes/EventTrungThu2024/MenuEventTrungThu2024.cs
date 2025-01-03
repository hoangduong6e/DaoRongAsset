using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;
using static UnityEngine.PlayerLoop.EarlyUpdate;
using Random = UnityEngine.Random;

public class MenuEventTrungThu2024 : EventManager
{
    // Start is called before the first frame update
    public Transform allvitri, allvitriFriend, Khung;
    JSONNode NguyenLieuYeuCau;
    public Text txtLongDenThuThap;
    public GameObject BuomXanh, ObjFriend, allObjThis,animLamBanh;
    string[] MaubuomArray = Enum.GetNames(typeof(_mauBuom));
    public static MenuEventTrungThu2024 inss;
    public Transform trencay,duoicay,traicay,phaicay;
    void Start()
    {
        //GameObject g = transform.GetChild(1).gameObject;
        //for(int i = 0;i < allvitri.transform.childCount;i++)
        //{

        //}

    }
    protected override void DiemDanhOk(JSONNode json)
    {
        foreach (KeyValuePair<string, JSONNode> key in json["data"].AsObject)
        {
            debug.Log("key diem danh " + key.Key + " value " + json["data"][key.Key].ToString());
            SetTxtNguyenLieu(key.Key, json["data"][key.Key].AsInt);
        }
    }
    protected override void ABSAwake()
    {
        inss = this;
    }
    private void SetLongDenThuThap(string longden,string longdenfriend)
    {
        txtLongDenThuThap.text = "-Tổng số Lồng đèn đã thu thập từ đầu Event đến giờ: <color=lime>" + longden + "</color> <color=yellow>(+" +longdenfriend + ")</color>";
     }
     private void SetGiupBanBe(int solangiup)
    {
        Text txt = transform.Find("Khung").transform.Find("txtgiupbanbe").GetComponent<Text>();
        txt.text = "-Bạn sẽ nhận được <color=lime>1 bánh trung thu miẽn phí</color> khi giúp bạn bè lấy thêm lồng đèn <color=yellow>" +(5 - solangiup) + " lần nữa</color>";
     }
    public Transform allVitriBuom;
    private void TaoBuomBuom()
    {
        if (allVitriBuom.transform.childCount >= 5)
        {
            for (int i = 0; i < allVitriBuom.transform.childCount; i++)
            {
                Destroy(allVitriBuom.transform.GetChild(i).gameObject);
            }
        }
        GameObject ins = Instantiate(BuomXanh, transform.position, Quaternion.identity);
        ins.transform.SetParent(allVitriBuom, false);
        ins.SetActive(true);
        ins.GetComponent<Butterfly>().setMauBuom = (_mauBuom)Enum.Parse(typeof(_mauBuom), MaubuomArray[allVitriBuom.transform.childCount - 1]);
    }
    private void TaoBuomBuomFriend(int sobuom)
    {
        Transform allvitriBuomFriend = ObjFriend.transform.Find("allBuom");
        for (int i = 0; i < allvitriBuomFriend.transform.childCount; i++)
        {
            Destroy(allvitriBuomFriend.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < sobuom; i++)
        {
            GameObject ins = Instantiate(BuomXanh, transform.position, Quaternion.identity);
            ins.transform.SetParent(allvitriBuomFriend, false);
            ins.SetActive(true);
            ins.GetComponent<Butterfly>().setMauBuom = (_mauBuom)Enum.Parse(typeof(_mauBuom), MaubuomArray[allvitriBuomFriend.transform.childCount - 1]);
        }

    }
    private void KhoiTaoLongDen(JSONNode alllongden, bool delay = true, bool frien = false)
    {
        choan = false;
        for (int i = 0; i < alllongden.Count; i++)
        {
            if (alllongden[i]["dahai"].AsBool)
            {
                delay = false;
                break;
            }
        }

        Transform ALLVITRI = allvitri;
        if (frien) ALLVITRI = allvitriFriend;

        StartCoroutine(DelayyRoutine());

        IEnumerator DelayyRoutine()
        {
            for (int i = 0; i < alllongden.Count; i++)
            {
                if (i >= ALLVITRI.childCount)
                {
                    debug.LogError("Index out of range: " + i);
                    continue; // Bỏ qua phần tử không hợp lệ
                }

                Transform childi = ALLVITRI.transform.GetChild(i);
                childi.gameObject.SetActive(true);

                if (childi.childCount > 0)
                {
                    Destroy(childi.GetChild(0).gameObject);
                }

                GameObject g = LoadObjectResource(alllongden[i]["name"].AsString + "G");
                GameObject instan = Instantiate(g, transform.position, Quaternion.identity);
                instan.transform.SetParent(childi, false);

                Vector3 vector3 = childi.position;
                vector3.y -= 0.5f;
                instan.transform.position = vector3;

                if (!alllongden[i]["dahai"].AsBool)
                {
                    instan.SetActive(true);

                    if (delay)
                    {
                        Vector3 vecbandau = instan.transform.position;
                        instan.transform.position = new Vector3(vecbandau.x, vecbandau.y + 2);
                        instan.transform.LeanMove(vecbandau, 0.3f);
                    }

                    if (alllongden[i]["friend"]["id"].AsString != "")
                    {
                        Transform instan2 = instan.transform.GetChild(0);
                        instan2.gameObject.SetActive(true);

                        // Mở comment nếu cần cập nhật UI avatar và khung
                        Image imgavt = instan2.GetChild(0).GetComponent<Image>();
                        Image imgKhung = instan2.GetChild(1).GetComponent<Image>();
                        Text txtname = instan2.GetChild(2).GetComponent<Text>();
                        txtname.text = alllongden[i]["friend"]["name"].AsString;
                        Friend.ins.LoadAvtFriend(alllongden[i]["friend"]["id"].AsString, imgavt, imgKhung);
                        if (!frien)
                        {
                            Button btn = instan.AddComponent<Button>();
                            btn.onClick.AddListener(NhanLongDenFriend);
                        }
                    }
                }
                else
                {
                    instan.SetActive(false);
                }

                if (delay)
                {
                    yield return new WaitForSeconds(0.11f);
                }
            }

            choan = true;
        }
    }

    public void ParseData(JSONNode json)
    {
        CrGame.ins.GetComponent<ZoomCamera>().enabled = false; Camera.main.orthographicSize = 5;
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        btnHopQua.transform.SetParent(transform);
        debug.Log(json["data"].ToString());
        KhoiTaoLongDen(json["data"]["alllongden"]);
        NguyenLieuYeuCau = json["NguyenLieuYeuCau"];
        foreach (KeyValuePair<string, JSONNode> key in NguyenLieuYeuCau.AsObject)
        {
            SetTxtNguyenLieu(key.Key, json["data"]["allItem"][key.Key].AsInt);
        }
        string[] allLongDen = new string[] { "LongDenKimCuong", "LongDenPhaQuan", "LongDenOval", "LongDenCaChep", "LongDenTron" };
        for (int i = 0; i < allLongDen.Length; i++)
        {
            SetTxtLongDen(allLongDen[i], json["data"]["allItem"][allLongDen[i]].AsString);
        }
        SetLongDenThuThap(json["data"]["TongSoLongDenThuThap"].AsString,json["data"]["TongSoLongDenBanGiup"].AsString);
        SetGiupBanBe(json["data"]["TongSoLanGiupBanBe"].AsInt);
        for (int i = 0; i < json["data"]["allItem"]["BuomXanh"].AsInt; i++)
        {
            TaoBuomBuom();
        }
        transform.Find("txtTime").GetComponent<Text>().text = json["timeketthuc"].AsString;

    }
    public void SetTxtNguyenLieu(string name, int soluong)
    {
        int yeucau = NguyenLieuYeuCau[name].AsInt;
        Khung.transform.Find(name).transform.GetChild(0).GetComponent<Text>().text = (soluong >= yeucau) ? "<color=lime>" + soluong + "/" + yeucau + "</color>" : "<color=red>" + soluong + "/" + yeucau + "</color>";
    }
    public void SetTxtLongDen(string name, string soluong)
    {
        Khung.transform.Find(name).transform.GetChild(0).GetComponent<Text>().text = soluong;
    }

    public void LamBanh()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "LamBanh";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json);
            if (json["status"].AsString == "0")
            {
                if(!animLamBanh.activeSelf)
                {
                    animLamBanh.SetActive(true);
                    StartDelay(() => { animLamBanh.SetActive(false); },0.6f);
                }    
               
                foreach (KeyValuePair<string, JSONNode> key in json["item"].AsObject)
                {
                    SetTxtNguyenLieu(key.Key, json["item"][key.Key].AsInt);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    bool choan = false;
    public void ChoPanDaAn()
    {
        if (!choan) return;
        bool quanhafrien = ObjFriend.activeSelf;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        if (!quanhafrien) datasend["method"] = "ChoPanDaAn";
        else
        {
            datasend["method"] = "ChoPanDaFriendAn";

            datasend["data"]["idFriend"] = Friend.ins.nameFriend;
            datasend["data"]["id"] = LoginFacebook.ins.id;
            datasend["data"]["idfr"] = Friend.ins.idFriend;
        }
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            choan = false;
            debug.Log(json);
            if (json["status"].AsString == "0")
            {
                Animator Panda = transform.Find("Panda").GetComponent<Animator>();
                Panda.SetBool("pandasleep", false);
                SetTxtNguyenLieu("BanhTrungThu", json["BanhTrungThu"].AsInt);

                StartDelay(() => {
                    Panda.SetBool("pandasleep", true);
                    StartDelay(() => {
                        choan = true;
                    }, 0.7f);
                    if (!quanhafrien)
                    {
                        GameObject longden = allvitri.transform.GetChild(json["phantuhai"].AsInt).gameObject;
                        GameObject ins = Instantiate(longden, transform.position, Quaternion.identity);

                        ins.transform.SetParent(Khung, false);
                        ins.transform.position = longden.transform.position;
                        ins.transform.localScale = new Vector3(0.5f, 0.5f);
                        longden.transform.GetChild(0).GetComponent<Animator>().Play("1");
                        longden.SetActive(false);
                        ins.transform.LeanMove(Khung.transform.Find(json["nameLongDen"].AsString).transform.position, 1f).setOnComplete(() => {
                            Destroy(ins.gameObject);
                            SetTxtLongDen(json["nameLongDen"].AsString, json["soluong"].AsString);
                            SetTxtNguyenLieu("BuomXanh", json["BuomXanh"].AsInt);
                        });

                        if (!json["alllongden"][0].ISNull)
                        {
                            KhoiTaoLongDen(json["alllongden"]);
                        }
                           SetLongDenThuThap(json["TongSoLongDenThuThap"].AsString,json["TongSoLongDenBanGiup"].AsString);
                        TaoBuomBuom();
                    }
                    else if (Friend.ins.nameFriend != "")
                    {
                        debug.Log("ChoPanDaFriendAn");

                        GameObject longden = allvitriFriend.transform.GetChild(json["phantuhai"].AsInt).transform.GetChild(0).gameObject;
                        longden.transform.LeanScale(Vector3.zero, 0.5f).setOnComplete(() => {
                            longden.SetActive(false);
                        });
                         SetGiupBanBe(json["TongSoLanGiupBanBe"].AsInt);
                    }
                    SetTxtNguyenLieu("BanhTrungThu", json["BanhTrungThu"].AsInt);
                }, 2.7f);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                choan = true;
            }
        }
    }

    public void OpenMenuFriend()
    {
        GameObject menu = AllMenu.ins.GetCreateMenu("menuFriend", transform.gameObject, true, transform.childCount);
        menu.name = "menuFriend";
    }
    public void QuaNhaFriend()
    {
        debug.Log("Qua nhà event " + Friend.ins.idFriend);
        Text txtname = ObjFriend.transform.GetChild(2).GetComponent<Text>();
        if (txtname.text == Friend.ins.nameFriend) return;


        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "QuaNhaFriend";
        datasend["data"]["idFriend"] = Friend.ins.nameFriend;
        datasend["data"]["id"] = LoginFacebook.ins.id;
        datasend["data"]["idfr"] = Friend.ins.idFriend;

        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {


                Image imgAvt = ObjFriend.transform.GetChild(0).GetComponent<Image>();
                Image imgKhungAvt = ObjFriend.transform.GetChild(1).GetComponent<Image>();

                Friend.ins.LoadAvtFriend(Friend.ins.idFriend, imgAvt, imgKhungAvt);
                txtname.text = Friend.ins.nameFriend;
                ObjFriend.SetActive(true);
                allObjThis.SetActive(false);
                KhoiTaoLongDen(json["data"]["alllongden"], false, true);
                TaoBuomBuomFriend(json["data"]["BuomXanh"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void NhanLongDenFriend()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanLongDenFriend";
        datasend["data"]["vitri"] = btnchon.transform.parent.transform.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json);
            if (json["status"].AsString == "0")
            {
                GameObject longden = btnchon.transform.parent.gameObject;
                GameObject ins = Instantiate(longden, transform.position, Quaternion.identity);

                ins.transform.SetParent(Khung, false);
                ins.transform.position = longden.transform.position;
                ins.transform.localScale = new Vector3(0.5f, 0.5f);
                longden.transform.GetChild(0).GetComponent<Animator>().Play("1");
                longden.SetActive(false);
                ins.transform.LeanMove(Khung.transform.Find(json["nameLongDen"].AsString).transform.position, 1f).setOnComplete(() => {
                    Destroy(ins.gameObject);
                    SetTxtLongDen(json["nameLongDen"].AsString, json["soluong"].AsString);
                });

                if (!json["alllongden"][0].ISNull)
                {
                    KhoiTaoLongDen(json["alllongden"]);
                }
                SetLongDenThuThap(json["TongSoLongDenThuThap"].AsString,json["TongSoLongDenBanGiup"].AsString);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void VeNha()
    {
        Friend.ins.nameFriend = "";
        if (ObjFriend.activeSelf)
        {
            Text txtname = ObjFriend.transform.GetChild(2).GetComponent<Text>();
            txtname.text = "";
            ObjFriend.SetActive(false);
            allObjThis.SetActive(true);
            return;
        }
        Transform menuFriend = transform.Find("menuFriend");
        if (menuFriend != null)
        {
            AllMenu.ins.DestroyMenu("menuFriend");
            //Destroy(menuFriend.gameObject);
        }
          btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
         AllMenu.ins.DestroyMenu("MenuEventTrungThu2024");
    }
    public void EmitEvent(JSONObject json)
    {
        if(json["emitevent"]["phantuhai"])
        {
           // debug.Log("emit event " + json);
            int asint = int.Parse(json["emitevent"]["phantuhai"].ToString());
           // debug.Log("phantuhai " + asint);
            Transform objavt = allvitri.transform.GetChild(asint).transform.GetChild(0).transform.GetChild(0);

            Image imgavt = objavt.GetChild(0).GetComponent<Image>();
            Image imgKhung = objavt.GetChild(1).GetComponent<Image>();
            Text txtname = objavt.GetChild(2).GetComponent<Text>();
            txtname.text = json["emitevent"]["tenhienthi"].str;
            Friend.ins.LoadAvtFriend(json["emitevent"]["taikhoan"].str, imgavt, imgKhung);
            Button btn = objavt.transform.parent.gameObject.AddComponent<Button>();
            btn.onClick.AddListener(NhanLongDenFriend);
            objavt.gameObject.SetActive(true);
        }
        else if (json["emitevent"]["phantuhaifriend"])
        {
            int asint = int.Parse(json["emitevent"]["phantuhaifriend"].ToString());

            Transform longden = allvitriFriend.transform.GetChild(asint).transform.GetChild(0);

            longden.transform.LeanScale(Vector3.zero,0.5f);
        }
        if (json["emitevent"]["TangThiep"])
        {
            debug.Log("tangthiep");
            GameObject trencung = GameObject.FindGameObjectWithTag("trencung");

            GameObject phao = Inventory.LoadObjectResource("GameData/EventTrungThu2024/Anim" + GamIns.CatDauNgoacKep(json["emitevent"]["TangThiep"].ToString()));
            GameObject phaoo = Instantiate(phao, transform.position, Quaternion.identity);
            phaoo.transform.position = new Vector3(phaoo.transform.position.x, transform.position.y, 10);
            phaoo.transform.SetParent(trencung.transform);
            phaoo.transform.SetSiblingIndex(0);
            //    phaoo.transform.GetChild(0).transform.position = new Vector3(phaoo.transform.GetChild(0).transform.position.x , phaoo.transform.GetChild(0).transform.position.y,10);
            phaoo.SetActive(true);
            //if (i % 2 == 0)
            //{
            //    AudioSource audio = phaoo.GetComponent<AudioSource>();
            //    audio.Play();
            //}
            //  phaoo.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            Destroy(phaoo, 2f);
        }
    }

    public void OpenGD2()
    {
        AudioManager.PlaySound("soundClick");
        GameObject gd2 = GetCreateMenu("GiaoDien2", AllMenu.ins.transform,true,transform.GetSiblingIndex()+1);
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDataGD2";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["BanhTrungThu"].AsInt);
                gd2.transform.Find("itemLongDen").GetChild(1).GetComponent<Text>().text = json["LongDenGiay"].AsString;
                gd2.transform.Find("itemDaMatTrang").GetChild(1).GetComponent<Text>().text = json["DaMatTrang"].AsString;
                Button btnquayX1 = gd2.transform.Find("btnquayX1").GetComponent<Button>();
                Button btnquayX10 = gd2.transform.Find("btnquayX10").GetComponent<Button>();
                Button btnVeNha = gd2.transform.Find("btnVeNha").GetComponent<Button>();
                Button btnRongAnBanh = gd2.transform.Find("btnRongAnBanh").GetComponent<Button>();
                Button btnBXH = gd2.transform.Find("btnBXH").GetComponent<Button>();
                Button btnDoiQua = gd2.transform.Find("btnDoiQua").GetComponent<Button>();
                btnquayX1.onClick.RemoveAllListeners();
                btnquayX1.onClick.AddListener(delegate { ThapLongDen(1); });
                btnquayX10.onClick.RemoveAllListeners();
                btnquayX10.onClick.AddListener(delegate { ThapLongDen(10); });
                btnVeNha.onClick.RemoveAllListeners();
                btnVeNha.onClick.AddListener(CloseGD2);
                btnRongAnBanh.onClick.RemoveAllListeners();
                btnRongAnBanh.onClick.AddListener(OpenMenuChonBanh);
                btnBXH.onClick.RemoveAllListeners();
                btnBXH.onClick.AddListener(XemBXH);
                btnDoiQua.onClick.RemoveAllListeners();
                btnDoiQua.onClick.AddListener(OpenMenuDoiManh2);

                SetYeuCau(json["YeuCau"]);
                LoadMocQuaGD2(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solanthaplongden"].AsInt);
                gd2.SetActive(true);
                btnHopQua.transform.SetParent(gd2.transform);
                Transform allvitriBuomFriend = ObjFriend.transform.Find("allBuom");
                allvitriBuomFriend.gameObject.SetActive(false);
                allVitriBuom.gameObject.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private bool hienxacnhan = true;
    public void ThapLongDen(byte solanquay)
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (hienxacnhan && btnchon.transform.GetChild(3).gameObject.activeSelf)
        {
            EventManager.OpenThongBaoChon("<color=red>Lượt thắp Lồng Đèn này sẽ tốn</color> <color=magenta>Kim cương</color>\n<color=yellow>(chỉ nhắc lần đầu)</color>", delegate { Thap(); hienxacnhan = false; });
        }
        else
        {
            Thap();
        }
        void Thap()
        {
            AudioManager.PlaySound("soundClick");
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "ThapLongDen";
            datasend["data"]["solanquay"] = solanquay.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    StartCoroutine(DelayHienQua(json["allqua"]));
                    SetYeuCau(json["YeuCau"]);
                    menuevent["GiaoDien2"].transform.Find("itemLongDen").GetChild(1).GetComponent<Text>().text = json["LongDenGiay"].AsString;
                    LoadMocQuaGD2(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solanthaplongden"].AsInt);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
     
    }
    public void CloseGD2()
    {
        //GameObject gd2 = transform.GetChild(1).gameObject;
        //gd2.SetActive(false);
        btnHopQua.transform.SetParent(transform);
        //foreach (KeyValuePair<string, GameObject> allitemnhann in menuevent)
        //{
        //    debug.Log(allitemnhann.Key + " " + allitemnhann.Value.gameObject.name);
        //    // datasend["data"]["allitemnhan"][allitemnhann.Key] = allitemnhann.Value.ToString();
        //    string destroy = allitemnhann.Key.ToString();
        //    DestroyMenu(destroy);
        //}
        List<string> array = menuevent.Keys.ToList();
        for (int i = 0; i < array.Count; i++)
        {
            debug.Log(array[i]);
            DestroyMenu(array[i]);
        }
          Transform allvitriBuomFriend = ObjFriend.transform.Find("allBuom");
                allvitriBuomFriend.gameObject.SetActive(true);
                allVitriBuom.gameObject.SetActive(true);
    }
    private int soluongchon = 0;
    string ThiepChon = "BanhTrungThuThuongHang";
    public void OpenMenuChonBanh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "Getitem";
        datasend["data"]["item"][0] = "HopBanhTrungThuThuongHang";
        datasend["data"]["item"][1] = "BanhTrungThuThuongHang";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject MenuChonBanh = GetCreateMenu("MenuChonBanh", menuevent["GiaoDien2"].transform, true, menuevent["GiaoDien2"].transform.childCount + 1);
                GameObject menu = MenuChonBanh.transform.GetChild(0).gameObject;
                MenuChonBanh.transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuChonBanh"); });
                GameObject BanhTrungThuThuongHang = menu.transform.Find("BanhTrungThuThuongHang").gameObject;
                Button btntangBanhTrungThuThuongHang = BanhTrungThuThuongHang.transform.Find("btnChon").GetComponent<Button>();
                Button btntangHopBanhTrungThuThuongHang = BanhTrungThuThuongHang.transform.Find("btnChon").GetComponent<Button>();
                if (json["allitem"]["BanhTrungThuThuongHang"].AsString != "0")
                {
                    btntangBanhTrungThuThuongHang.onClick.AddListener(ChonBanhTang);
                }
                else btntangBanhTrungThuThuongHang.interactable = false;
                if (json["allitem"]["HopBanhTrungThuThuongHang"].AsString != "0")
                {
                    btntangHopBanhTrungThuThuongHang.onClick.AddListener(ChonBanhTang);
                }
                else btntangHopBanhTrungThuThuongHang.interactable = false;
                GameObject HopBanhTrungThuThuongHang = menu.transform.Find("HopBanhTrungThuThuongHang").gameObject;
                HopBanhTrungThuThuongHang.transform.Find("btnChon").GetComponent<Button>().onClick.AddListener(ChonBanhTang);
                BanhTrungThuThuongHang.transform.Find("txtsoluong").GetComponent<Text>().text = json["allitem"]["BanhTrungThuThuongHang"].AsString;
                BanhTrungThuThuongHang.transform.Find("InputField").transform.Find("ImageTang").GetComponent<Button>().onClick.AddListener(delegate { ThemSoLuongTangBanh(1); });
                BanhTrungThuThuongHang.transform.Find("InputField").transform.Find("ImageGiam").GetComponent<Button>().onClick.AddListener(delegate { ThemSoLuongTangBanh(-1); });
                HopBanhTrungThuThuongHang.transform.Find("txtsoluong").GetComponent<Text>().text = json["allitem"]["HopBanhTrungThuThuongHang"].AsString;
                HopBanhTrungThuThuongHang.transform.Find("InputField").transform.Find("ImageTang").GetComponent<Button>().onClick.AddListener(delegate { ThemSoLuongTangBanh(1); });
                HopBanhTrungThuThuongHang.transform.Find("InputField").transform.Find("ImageGiam").GetComponent<Button>().onClick.AddListener(delegate { ThemSoLuongTangBanh(-1); });
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
        void ChonBanhTang()
        {
            AudioManager.PlaySound("soundClick");
            GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
            if (menuevent.ContainsKey("MenuTangBanh"))
            {
                ThiepChon = btnchon.transform.parent.name;
                soluongchon = int.Parse(btnchon.transform.parent.GetChild(3).GetComponent<InputField>().text);
                GameObject menuchonthiep = menuevent["MenuChonBanh"];
                menuchonthiep.SetActive(false);
                menuevent["MenuTangBanh"].SetActive(true);
                return;
            }
            JSONClass datasend = new JSONClass();
            datasend["class"] = "Main";
            datasend["method"] = "GetListFriend";
            datasend["data"]["id"] = LoginFacebook.ins.id;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].Value == "0")
                {
                    ThiepChon = btnchon.transform.parent.name;
                    soluongchon = int.Parse(btnchon.transform.parent.GetChild(3).GetComponent<InputField>().text);
                    GameObject menuchonthiep = menuevent["MenuChonBanh"];
                    menuchonthiep.SetActive(false);
                    GameObject menutangbanh = GetCreateMenu("MenuTangBanh", menuevent["GiaoDien2"].transform, true, menuevent["GiaoDien2"].transform.childCount + 1);

                    JSONNode allfriend = json["data"]["allfriend"];
                    GameObject menufriend = menutangbanh;
                    GameObject viewport = menufriend.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject;
                    GameObject ObjFriend = viewport.transform.GetChild(0).gameObject;

                    GameObject Content = viewport.transform.GetChild(1).gameObject;
                    for (int i = 1; i < allfriend.Count; i++)
                    {
                        GameObject Offriend = Instantiate(ObjFriend, Content.transform.position, Quaternion.identity) as GameObject;
                        Offriend.transform.SetParent(Content.transform, false);
                        Offriend.transform.Find("btnMoi").GetComponent<Button>().onClick.AddListener(TangBanh);
                        Image imgAvatar = Offriend.transform.GetChild(1).GetComponent<Image>();
                        Image imgKhung = Offriend.transform.GetChild(2).GetComponent<Image>();
                        Text txtname = Offriend.transform.GetChild(3).GetComponent<Text>();
                        Offriend.name = allfriend[i]["idfb"].Value;
                        txtname.text = allfriend[i]["name"].Value;
                        if (txtname.text.Length > 8)
                        {
                            string newname = txtname.text.Substring(0, 8) + "...";
                            txtname.text = newname;
                        }
                        // friend.GetAvatarFriend(CatDauNgoacKep(allfriend[i]["idfb"].ToString()), Avatar);
                        //  Khung.sprite = Inventory.LoadSprite("Avatar" + CatDauNgoacKep(allfriend[i]["toc"].ToString()));
                        NetworkManager.ins.friend.LoadAvtFriend(allfriend[i]["objectId"].Value, imgAvatar, imgKhung);
                        Offriend.SetActive(true);
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].Value);
                    AllMenu.ins.DestroyMenu("menuFriend");
                }
            }
            CrGame.ins.panelLoadDao.SetActive(true);
        }
        void ThemSoLuongTangBanh(int i)
        {
            AudioManager.PlaySound("soundClick");
            if (i == 0)
            {
                GameObject ObjPhaoo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
                if (ObjPhaoo.name == "BanhTrungThuThuongHang" || ObjPhaoo.name == "HopBanhTrungThuThuongHang")
                {
                    ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text = ObjPhaoo.transform.GetChild(2).GetComponent<Text>().text;
                }
                return;
            }
            GameObject ObjPhao = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject;
            int Soluongdachon = int.Parse(ObjPhao.transform.GetChild(3).GetComponent<InputField>().text);
            if (i > 0)
            {
                if (int.Parse(ObjPhao.transform.GetChild(2).GetComponent<Text>().text) > Soluongdachon)
                {
                    debug.Log(ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text);
                    Soluongdachon += 1;
                    ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text = Soluongdachon.ToString();
                }
            }
            else
            {
                if (Soluongdachon > 0)
                {
                    Soluongdachon -= 1;
                    ObjPhao.transform.GetChild(3).GetComponent<InputField>().text = Soluongdachon.ToString();
                }
            }
            soluongchon = Soluongdachon;
            if (Soluongdachon > 0)
            {
                ObjPhao.transform.GetChild(5).GetComponent<Button>().interactable = true;
            }
            else ObjPhao.transform.GetChild(5).GetComponent<Button>().interactable = false;
        }
        void TangBanh()/////////////////////////////////////////////////////////////////
        {
            AudioManager.PlaySound("soundClick");
            debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name);
            GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            string taikhoanban = btnchon.transform.parent.name;
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "TangBanh";
            datasend["data"]["taikhoanban"] = taikhoanban;
            datasend["data"]["tenban"] = btnchon.transform.parent.transform.GetChild(3).GetComponent<Text>().text;
            datasend["data"]["soluong"] = soluongchon.ToString();
            datasend["data"]["namephao"] = ThiepChon;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    DestroyMenu("MenuTangBanh");
                }
                else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }

        }
    }
    bool sangtrang = false; int trang = 1, trangg = 1; float top, topcuoi;
    private void XemBXH()
    {
       
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemBXH";
        datasend["data"]["top"] = top.ToString();
        datasend["data"]["trang"] = trangg.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject menu = GetCreateMenu("MenuTop", menuevent["GiaoDien2"].transform, true, menuevent["GiaoDien2"].transform.childCount + 1);
                //    menuevent["MenuTop"] = menu;
                // GameObject menu = GetCreateMenu("MenuTop",gameObject,false,3);
                if (json["alltop"].Count > 0)
                {
                    GameObject menutop = menu.transform.GetChild(0).GetChild(4).gameObject;

                    GameObject contentop = menutop.transform.GetChild(0).gameObject;
                    //  menu.transform.GetChild(0).GetChild(4).gameObject;

                    if (trangg % 2 == 1)
                    {
                        topcuoi = float.Parse(json["alltop"][0]["diemtangthiep"].Value);
                    }
                    if (json["alltop"].Count > 10)
                    {
                        top = float.Parse(json["alltop"][10]["diemtangthiep"].Value);
                        sangtrang = true;
                    }
                    //  else if (i == json["alltop"].Count) topcuoi = float.Parse(json["alltop"][i]["diemtangthiep"].Value);
                    for (int i = 0; i < 10; i++)
                    {
                        contentop.transform.GetChild(i).gameObject.SetActive(false);
                        if (i < json["alltop"].Count)
                        {
                            debug.Log(json["alltop"][i]["Name"].Value);
                            Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].AsString;
                            debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                            debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = i + trang;
                            txtTop.text = sotop.ToString();
                            debug.Log("ok3");
                            NetworkManager.ins.friend.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar, imgKhungAvatar);
                            // imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                            contentop.transform.GetChild(i).gameObject.SetActive(true);
                            debug.Log("ok4");
                            if (sotop > 3) HuyHieu.sprite = Top;
                            else if (sotop == 1) HuyHieu.sprite = Top1;
                            else if (sotop == 2) HuyHieu.sprite = Top2;
                            else if (sotop == 3) HuyHieu.sprite = Top3;
                            HuyHieu.SetNativeSize();
                            debug.Log("ok5");
                            txtName.text = json["alltop"][i]["Name"].Value;
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["diemtangthiep"].AsString;
                            debug.Log("ok5.1");

                            contentop.transform.GetChild(i).gameObject.SetActive(true);

                            debug.Log("ok5.2");
                        }


                        //  contentop.transform.GetChild(i).transform.SetParent(imgtop.transform, false);
                        // CrGame.ins.OnThongBao(false);
                        //AllTop.SetActive(true);S
                        // txtTrang.text = trang + "/100";
                    }
                    debug.Log("ok6");

                    //  trang += 10;
                }
                else CrGame.ins.OnThongBaoNhanh("Chưa có xếp hạng");
                if (trang == 1)
                {
                    GameObject nhatki = menu.transform.GetChild(0).transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).gameObject;
                    GameObject contentnhatki = nhatki.transform.GetChild(1).gameObject;
                    GameObject objnhatki = nhatki.transform.GetChild(0).gameObject;
                    for (int i = json["nhatki"].Count - 1; i >= 0; i--)
                    {
                        GameObject objnhatkii = Instantiate(objnhatki, transform.position, Quaternion.identity);
                        objnhatkii.transform.GetChild(0).GetComponent<Text>().text = json["nhatki"][i].Value;
                        objnhatkii.transform.SetParent(contentnhatki.transform, false);
                        objnhatkii.SetActive(true);
                    }
                    //GameObject objquaphao = menu.transform.GetChild(0).transform.GetChild(6).transform.GetChild(1).gameObject;
                    //objquaphao.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["duoctangthiep"].Value;//json["quatichluythiep"]
                    //Image fillaoumt = objquaphao.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
                    //fillaoumt.fillAmount = float.Parse(json["duoctangthiep"].Value) / 250;
                    //GameObject allqua = objquaphao.transform.GetChild(2).gameObject;

                    LoadMocQuaBXH(json["quatichluythiep"], json["allMocDiemquatichluythiep"], json["duoctangthiep"].AsInt);
                }

                debug.Log("this server là " + LoginFacebook.thisServer);
                if (LoginFacebook.thisServer == "Sa Mạc" || LoginFacebook.thisServer == "Rừng Rậm") //sv2 sv3
                {
                    menu.transform.GetChild(0).transform.Find("MenuPhanThuong").transform.GetChild(0).GetComponent<Image>().sprite = EventManager.ins.GetSprite("bxhtop20");
                }
                GameObject mntop = menu.transform.GetChild(0).transform.GetChild(4).gameObject;
                menu.transform.GetChild(0).transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuTop"); sangtrang = false; trang = 1; trangg = 1; top = 0; topcuoi = 0; });
                Button btn = mntop.transform.GetChild(1).GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(delegate { sangtrangtop(1); });
                menu.SetActive(true);
            }
        }
        void sangtrangtop(int i)
        {
            AudioManager.PlaySound("soundClick");
            if (top < topcuoi && sangtrang)
            {
                if (i > 0) trangg += 1;
                else return;
                //   else trangg -= 1;
                if (trang + i >= 0)
                {
                    if (i < 0)
                    {
                        trang -= 20;
                        top = topcuoi;
                    }
                    else trang += 10;
                    XemBXH();
                }
                //   debug.Log(i);
            }
        }

    }
    void SetYeuCau(JSONNode yeucau)
    {
        GameObject giaodien = menuevent["GiaoDien2"];
        GameObject btnX1 = giaodien.transform.Find("btnquayX1").gameObject;
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

        GameObject btnX10 = giaodien.transform.Find("btnquayX10").gameObject;
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
    public Sprite sprite1, sprite2, chuaduocnhan, duocnhan, danhan;
    private void LoadMocQuaGD2(JSONNode allmocqua, JSONNode diemmocqua, int diemmoc)
    {
        GameObject gd1 = transform.GetChild(0).gameObject;
        GameObject ScrollViewallMocQua = menuevent["GiaoDien2"].transform.Find("ScrollViewallMocQua").gameObject;
        ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = diemmoc.ToString();
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
            if (diemmoc >= mocqua)
            {
                fill.fillAmount = 1;
                tru = diemmoc - mocqua;
                //trumocqua -= mocqua;
            }
            else
            {
                debug.Log("tru " + tru);
                if (i < allmocqua.Count - 1)
                {
                    int trumocqua = mocqua - diemmocqua[allmocqua.Count - 1 - i - 1].AsInt;
                    debug.Log("trumocqua " + trumocqua);
                    fill.fillAmount = (float)tru / (float)trumocqua;
                    tru = 0;
                }
                else
                {
                    fill.fillAmount = (float)diemmoc / diemmocqua[0].AsFloat;
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
                qua.transform.GetChild(4).gameObject.SetActive(false);
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

                    btn.onClick.AddListener(NhanQuaTichLuyGd2);
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
    public Sprite[] alltuchat;
    public IEnumerator DelayHienQua(JSONNode allqua)
    {
        //yield return new WaitForSeconds(0.4f);
        // JSONNode allqua = allquanhan["allqua"];
        GameObject panelnhanqua = GetCreateMenu("PanelNhanQua", menuevent["GiaoDien2"].transform, true, menuevent["GiaoDien2"].transform.childCount);
        Button btnnhan = panelnhanqua.transform.GetChild(1).transform.GetChild(0).GetComponent<Button>();
        debug.Log("delay 1");
        btnnhan.onClick.RemoveAllListeners();
        btnnhan.onClick.AddListener(XacNhanQua);
        panelnhanqua.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        //SetYeuCau(allquanhan["YeuCau"]);
        GameObject objallqua = panelnhanqua.transform.GetChild(1).gameObject;
        btnnhan.interactable = false;
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
                Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                img.sprite = EventManager.ins.GetSprite(allqua[i]["nameitem"].AsString);
                // img.SetNativeSize();
                objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                objtrong.GetComponent<infoitem>().enabled = true;
                objtrong.name = "item" + allqua[i]["nameitem"].Value;
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
            //   allquanhan = null;
        }
        btnnhan.interactable = true;
    }
    private void LoadMocQuaBXH(JSONNode allmocqua, JSONNode diemmocqua, int banhtrungthu)
    {
        debug.Log("MenuTop " + menuevent["MenuTop"].transform.GetChild(0).gameObject.name);
        GameObject ScrollViewallMocQua = menuevent["MenuTop"].transform.GetChild(0).transform.Find("MenuNhatKi").transform.Find("ScrollViewallMocQua").gameObject;

        ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = banhtrungthu.ToString();
        GameObject content = ScrollViewallMocQua.transform.GetChild(1).transform.GetChild(0).gameObject;
        int tru = 0;

        //  debug.Log(content.gameObject.name);
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
                debug.Log("tru " + tru);
                if (i < allmocqua.Count - 1)
                {
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
                    btn.onClick.AddListener(NhanQua);
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
        void NhanQua()
        {
            AudioManager.PlaySound("soundClick");
            Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
            int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "NhanQuaTichLuyBXH";
            datasend["data"]["mocqua"] = btnnhan.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    LoadMocQuaBXH(json["quatichluythiep"], json["allMocDiemquatichluythiep"], json["duoctangthiep"].AsInt);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
    }
    public void XacNhanQua()
    {
        AudioManager.PlaySound("soundClick");
        GameObject hopqua = btnHopQua;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject panelnhanqua = menuevent["PanelNhanQua"];
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
    private void NhanQuaTichLuyGd2()
    {
        AudioManager.PlaySound("soundClick");
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanQuaTichLuyGd2";
        datasend["data"]["mocqua"] = btnnhan.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadMocQuaGD2(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solanthaplongden"].AsInt);
                menuevent["GiaoDien2"].transform.Find("itemDaMatTrang").GetChild(1).GetComponent<Text>().text = json["DaMatTrang"].AsString;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private GameObject menumuaitem;

    string txtTv(string s)
    {
        switch (s)
        {
            case "TrungMuoi": return "Trứng Muối";
            case "BotBanh": return "Bột Bánh";
            case "NhanThuongHang": return "Nhân Thượng Hạng";
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
                    
                     SetTxtNguyenLieu(nameitemmua, json["soitem"].AsInt);
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
    public void OpenMenuNhiemvu()
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetNhiemVu";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", CrGame.ins.trencung, true);
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
                        txttiendo.text = "<color=#00ff00ff>" + GamIns.FormatCash(json["allNhiemvu"][i]["dalam"].AsInt) + "/" + GamIns.FormatCash(json["allNhiemvu"][i]["maxnhiemvu"].AsInt) + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + GamIns.FormatCash(json["allNhiemvu"][i]["dalam"].AsInt) + "/" + GamIns.FormatCash(json["allNhiemvu"][i]["maxnhiemvu"].AsInt) + "</color>";
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

    public void OpenMenuDoiManh2()
    {
        //return;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "MenuDoiManh";
        if (menuevent.ContainsKey("GiaoDien2")) datasend["method"] = "MenuDoiManh2";
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
                    GameObject manh = Instantiate(ObjectManh, ContentManh.transform.position, Quaternion.identity);
                    manh.transform.SetParent(ContentManh.transform, false);
                    manh.GetComponent<Button>().onClick.AddListener(XemManhDoi2);
                    Image imgmanh = manh.transform.GetChild(0).GetComponent<Image>();
                    if (json["ManhDoi"][i]["itemgi"].Value == "item" || json["ManhDoi"][i]["itemgi"].Value == "thuyenthucan")
                    {
                        imgmanh.sprite = Inventory.LoadSprite(json["ManhDoi"][i]["nameitem"].Value);
                    }
                    else if (json["ManhDoi"][i]["itemgi"].Value == "itemevent")
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
                    else
                    {
                        imgmanh.sprite = Inventory.LoadSpriteRong(json["ManhDoi"][i]["nameitem"].Value + "2");
                    }
                    imgmanh.SetNativeSize();
                    manh.name = json["ManhDoi"][i]["namekey"];
                    manh.SetActive(true);
                    GamIns.ResizeItem(imgmanh,200);
                }
             
                menuDoiManh.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DoiManh2);
                menuDoiManh.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ExitDoiManh2);
                menuDoiManh.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    string namemanhchon;

    void XemManhDoi2()
    {
        AudioManager.PlaySound("soundClick");
        Button btnchon = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        GameObject yeucau = EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(1).gameObject;
        namemanhchon = btnchon.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "XemManhDoi";
        if (menuevent.ContainsKey("GiaoDien2")) datasend["method"] = "XemManhDoi2";
        datasend["data"]["namemanh"] = namemanhchon;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            Debug.Log(json.ToString());
            for (int i = 1; i < yeucau.transform.childCount; i++)
            {
                Destroy(yeucau.transform.GetChild(i).gameObject);
            }
            int sonvok = 0;
            for (int i = 0; i < json["manhdoi"]["itemcan"].Count; i++)
            {
          
                GameObject g = yeucau.transform.GetChild(0).gameObject;
                GameObject instan = Instantiate(g, transform.position, Quaternion.identity);
                instan.transform.SetParent(yeucau.transform, false);
                string nameitem = json["manhdoi"]["itemcan"][i]["nameitem"].AsString; ;
                instan.name = nameitem;
                Image img = instan.transform.GetChild(0).GetComponent<Image>();
                if (json["manhdoi"]["itemcan"][i]["loaiitem"].AsString == "ItemEvent")
                {
                    img.sprite = GetSprite(nameitem);
                }
                else if (json["manhdoi"]["itemcan"][i]["loaiitem"].AsString == "Item")
                {
                    img.sprite = Inventory.LoadSprite(nameitem);
                }
                img.SetNativeSize();
                instan.gameObject.SetActive(true);
                Text txtyeucau = instan.transform.GetChild(1).GetComponent<Text>();

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
                GamIns.ResizeItem(img);
                //for (int j = 0; j < yeucau.transform.childCount; j++)
                //{
                //    if (yeucau.transform.GetChild(j).name == json["manhdoi"]["itemcan"][i]["nameitem"].Value)
                //    {
                //        Text txtyeucau = yeucau.transform.GetChild(j).transform.GetChild(1).GetComponent<Text>();
                //        yeucau.transform.GetChild(j).gameObject.SetActive(true);

                //        txtyeucau.text = json["hienthisoitemco"][json["manhdoi"]["itemcan"][i]["nameitem"].AsString].AsString;
                //        if (json["hienthisoitemco"][json["manhdoi"]["itemcan"][i]["nameitem"].AsString].AsBool)
                //        {
                //            sonvok += 1;
                //        }
                //        if (json["duocdoi"].AsBool)
                //        {
                //            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(true);
                //            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(2).GetComponent<Button>().interactable = true;
                //        }
                //        else
                //        {
                //            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(false);
                //        }
                //    }
                //}
            }
            EventManager.ins.menuevent["MenuDoiManh"].transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = json["manhdoi"]["thongtin"].Value;
        }
    }

    void DoiManh2()
    {
        AudioManager.PlaySound("soundClick");

        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "DoiQua";
        if (menuevent.ContainsKey("GiaoDien2")) datasend["method"] = "DoiQua2";
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

    private void ExitDoiManh2()
    {
        EventManager.ins.DestroyMenu("MenuDoiManh");
    }
}
