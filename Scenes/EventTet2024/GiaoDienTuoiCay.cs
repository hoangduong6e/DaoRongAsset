using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GiaoDienTuoiCay : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite1, sprite2,CayMaiNho, CayMaiVua, CayMaiLon;
    public Sprite Top1, Top2, Top3, Top;
    bool sangtrang = false; int trang = 1, trangg = 1; float top, topcuoi;
    // private bool hienthilai = false;
    GameObject menuTuoiCay;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ParseData(JSONNode json)
    {
        duocquay = true;
        debug.Log(json.ToString());
        //   LoadMenuChonBanBe();
        Text txtdiemphao = transform.GetChild(0).transform.Find("btnTop").transform.GetChild(0).GetComponent<Text>();
        txtdiemphao.text = json["diemphaohoa"].Value;
      //  LoadMocQua(json);

        Image imgBg = transform.GetChild(0).GetComponent<Image>();
        if (CrGame.ins.NgayDem == "Ngay") imgBg.sprite = Resources.Load<Sprite>("GameData/EventTet2024/BGBanNgay");
        else imgBg.sprite = Resources.Load<Sprite>("GameData/EventTet2024/BGBanDem");
        // AudioManager.SetSoundBg("GameData/EventTet2023/nhacnentet", true);
        menuTuoiCay = transform.GetChild(0).gameObject;
        SetYeuCau(json["YeuCau"]);
        SetBinhNuoc(json["BinhThanhThuy"]);
        SetLiXi(json["BaoLiXi"]);
        SetCayMaiLon(json["soluottuoicay"].Value);
        LoadMocQuaGD1(json["quatichluygdTuoiCay"], json["allMocDiemGD2"], json["soluottuoicay"].AsInt);
    }
    private void SetBinhNuoc(string s)
    {
        Text txtSieuThanhThuy = menuTuoiCay.transform.Find("imgnuocthanh").transform.GetChild(1).GetComponent<Text>();
        txtSieuThanhThuy.text = s;
    }
    private void SetLiXi(string s)
    {
        Text txtBaoLiXi = menuTuoiCay.transform.Find("imgLiXi").transform.GetChild(1).GetComponent<Text>();
        txtBaoLiXi.text = s;
    }
    private void SetCayMaiLon(string s)
    {
        float soluottreo = float.Parse(s);
        //txtsoluottreo.text = soluottreo + "";

        Image imgcaymai = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        if (soluottreo < 100)
        {
            imgcaymai.sprite = CayMaiNho;
            if (transform.GetChild(0).transform.GetChild(0).childCount > 2) Destroy(transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject);
            //    transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        }
        if (soluottreo >= 100)
        {
            imgcaymai.sprite = CayMaiVua;
            if (transform.GetChild(0).transform.GetChild(0).childCount > 2) Destroy(transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject);
        }
        if (soluottreo >= 300)
        {
            imgcaymai.sprite = CayMaiLon;
            GameObject trangtri = Inventory.LoadObjectResource("GameData/EventTet2024/TrangTri");
            debug.Log("trang tri " + trangtri.name);
            GameObject objtrangtri = Instantiate(trangtri, transform.position, Quaternion.identity); //transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
            objtrangtri.transform.SetParent(transform.GetChild(0).transform.GetChild(0).transform, false);
            objtrangtri.transform.position = imgcaymai.transform.position;
            // transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
            //int[] arr = new int[] {1,3,5,7,9,2,4,6,8};
            //StartCoroutine(delay());
            //IEnumerator delay()
            //{
            //    for (int i = 0; i < arr.Length; i++)
            //    {
            //        objtrangtri.transform.GetChild(arr[i]).gameObject.SetActive(true);
            //        yield return new WaitForSeconds(0.2f);
            //    }
            //}
            objtrangtri.SetActive(true);
        }
    }
    void SetYeuCau(JSONNode yeucau)
    {
        GameObject btnX1 = transform.GetChild(0).transform.Find("btnquayX1").gameObject;
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

        GameObject btnX10 = transform.GetChild(0).transform.Find("btnquayX11").gameObject;
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
    private bool boquaxacnhan = false;
    bool duocquay = true;
    public void QuayMayMan(string solanquay)
    {
        AudioManager.PlaySound("soundClick");
        if (!duocquay) return;
        if (!boquaxacnhan)
        {
            GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            if (!btnchon.transform.Find("BinhThanhThuy").gameObject.activeSelf)
            {
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true,transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                tbc.txtThongBao.text = "Không đủ Bình Thánh Thủy, tưới cây sẽ tốn Kim Cương\n<size=45>(Chỉ nhắc lần đầu)</size>";
                tbc.btnChon.onClick.AddListener(delegate { BoQuaXacNhan(solanquay); });
                return;
            }
        }

        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "TuoiCay";
        datasend["data"]["solanquay"] = solanquay.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                duocquay = false;
                GameObject binhnuoc = transform.GetChild(0).transform.Find("BinhNuoc").gameObject;
             //   GameObject btncaymai = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;

               // hienthilai = menuxacnhan.transform.GetChild(5).GetComponent<Toggle>().isOn;

            //    LoadMocQua(json);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                  //  btncaymai.enabled = false;
                    binhnuoc.SetActive(true);
                    yield return new WaitForSeconds(2.3f);
                    binhnuoc.SetActive(false);
               //     btncaymai.enabled = true;

                    StartCoroutine(DelayHienQua(json["allqua"]));
                    SetYeuCau(json["YeuCau"]);
                    SetBinhNuoc(json["BinhThanhThuy"]);
                    SetLiXi(json["BaoLiXi"]);
                    SetCayMaiLon(json["soluottuoicay"].Value);
                    LoadMocQuaGD1(json["quatichluygdTuoiCay"], json["allMocDiemGD2"], json["soluottuoicay"].AsInt);
                    yield return new WaitForSeconds(0.2f);
                    duocquay = true;
                }

         
               // SetTraiChauVaHopQua(json["TraiChau"].AsString, json["HopQuaGiangSinh"].AsString);
                //LoadMocQuaGD1(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solantrangtricaythong"].AsInt);
                // menuevent["GiaoDien2"].transform.Find("itemLongDen").GetChild(1).GetComponent<Text>().text = json["LongDenGiay"].AsString;
                //LoadMocQuaGD2(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solanthaplongden"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void BoQuaXacNhan(string solanquay)
    {
        boquaxacnhan = true;
        if (AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        QuayMayMan(solanquay);
    }
    public IEnumerator DelayHienQua(JSONNode allqua)
    {
        GameObject panelnhanqua = EventManager.ins.GetCreateMenu("PanelNhanQua2", transform, true);
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
            for (int k = 0; k < MenuEventTet2024.inss.alltuchat.Length; k++)
            {
                if (MenuEventTet2024.inss.alltuchat[k].name == allqua[i]["tuchat"].Value)
                {
                    qua.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = MenuEventTet2024.inss.alltuchat[k];
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
                for (int j = 0; j < EventManager.ins.allitemEvent.Length; j++)
                {
                    if (EventManager.ins.allitemEvent[j].name == allqua[i]["nameitem"].Value)
                    {
                        Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                        img.sprite = EventManager.ins.allitemEvent[j];
                         img.SetNativeSize();
                        img.transform.localScale = new Vector3(img.transform.localScale.x / 2,img.transform.localScale.y/2) ;
                        objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                        objtrong.GetComponent<infoitem>().enabled = true;
                        objtrong.name = "item" + allqua[i]["nameitem"].Value;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.02f);
            qua.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            //   allquanhan = null;
        }
        btnnhan.interactable = true;
    }
    public void XacNhanQua()
    {
        AudioManager.PlaySound("soundClick");
        GameObject hopqua = EventManager.ins.btnHopQua;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject panelnhanqua = EventManager.ins.menuevent["PanelNhanQua2"];
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

    private void LoadMocQuaGD1(JSONNode allmocqua, JSONNode diemmocqua, int banhtrungthu)
    {
        GameObject ScrollViewallMocQua = menuTuoiCay.transform.Find("ScrollViewallMocQua").gameObject;
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
                qua.transform.GetChild(2).GetComponent<Image>().sprite = MenuEventTet2024.inss.chuaduocnhan;
                qua.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (trangthai == "duocnhan")
            {
                qua.transform.GetChild(4).gameObject.SetActive(true);
                qua.transform.GetChild(5).gameObject.SetActive(false);
                qua.transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                qua.transform.GetChild(2).GetComponent<Image>().sprite = MenuEventTet2024.inss.duocnhan;
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
                qua.transform.GetChild(2).GetComponent<Image>().sprite = MenuEventTet2024.inss.danhan;
            }
        }
    }
    private void NhanQuaTichLuyGd1()
    {
        AudioManager.PlaySound("soundClick");
        if (!duocquay) return;
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "NhanQuaTichLuyGdTuoiCay";
        datasend["data"]["mocqua"] = btnnhan.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadMocQuaGD1(json["quatichluygdTuoiCay"], json["allMocDiemGD2"], json["soluottuoicay"].AsInt);
                SetLiXi(json["BaoLiXi"].AsString);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void GetSoPhao()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "GetSoPhao";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject MenuChonPhao = transform.Find("MenuChonPhao").gameObject;
                GameObject Menu = MenuChonPhao.transform.GetChild(0).gameObject;

                Menu.transform.Find("Phao1Diem").transform.Find("txtSoPhao").GetComponent<Text>().text = json["phaohoanho"].AsString;
                Menu.transform.Find("Phao10Diem").transform.Find("txtSoPhao").GetComponent<Text>().text = json["phaohoalon"].AsString;

                MenuChonPhao.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    int soluongchon = 0;
    public void ThemSoLuongDotPhao(int i)
    {
        if (i == 0)
        {
            GameObject ObjPhaoo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
            if (ObjPhaoo.name == "Phao1Diem" || ObjPhaoo.name == "Phao10Diem")
            {
                ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text = ObjPhaoo.transform.GetChild(2).GetComponent<Text>().text;
                //if (ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text == "0") ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text = "1";
                //int Soluongdachonn = int.Parse(ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text);
                //if (int.Parse(ObjPhaoo.transform.GetChild(2).GetComponent<Text>().text) <= Soluongdachonn)
                //{
                //    ObjPhaoo.transform.GetChild(3).transform.GetComponent<InputField>().text = ObjPhaoo.transform.GetChild(2).GetComponent<Text>().text;
                //}
                //ObjPhaoo.transform.GetChild(3).transform.GetComponent<InputField>().text = int.Parse(ObjPhaoo.transform.GetChild(3).transform.GetComponent<InputField>().text).ToString();
            }
            return;
        }
        GameObject ObjPhao = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject;
        debug.Log(ObjPhao.name);
        int Soluongdachon = int.Parse(ObjPhao.transform.GetChild(3).GetComponent<InputField>().text);
        if (i > 0)
        {
            if (int.Parse(ObjPhao.transform.GetChild(2).GetComponent<Text>().text) > Soluongdachon)
            {
                debug.Log(ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text);
                Soluongdachon += 1;
                ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text = Soluongdachon.ToString();
            }
            // else ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text = ObjPhao.transform.GetChild(2).GetComponent<Text>().text;
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
    string PhaoChon = "Phao1Diem";
    bool loadfriend = false;
    public void ChonPhao()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        GameObject MenuTangPhao = transform.Find("MenuTangPhao").gameObject;
        GameObject MenuChonPhao = transform.Find("MenuChonPhao").gameObject;
        if (loadfriend)
        {
            PhaoChon = btnchon.transform.parent.name;
            soluongchon = int.Parse(btnchon.transform.parent.GetChild(3).GetComponent<InputField>().text);
            MenuChonPhao.gameObject.SetActive(false);
            MenuTangPhao.gameObject.SetActive(true);
            return;
        }
        CrGame.ins.panelLoadDao.SetActive(true);

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "GetListFriend";
        datasend["data"]["id"] = LoginFacebook.ins.id;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                PhaoChon = btnchon.transform.parent.name;
                soluongchon = int.Parse(btnchon.transform.parent.GetChild(3).GetComponent<InputField>().text);
                MenuChonPhao.SetActive(false);
                MenuTangPhao.gameObject.SetActive(true);

                JSONNode allfriend = json["data"]["allfriend"];
                GameObject menufriend = MenuTangPhao.gameObject;
                GameObject viewport = menufriend.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject;
                GameObject ObjFriend = viewport.transform.GetChild(0).gameObject;
                GameObject Content = viewport.transform.GetChild(1).gameObject;
                for (int i = 1; i < allfriend.Count; i++)
                {
                    GameObject Offriend = Instantiate(ObjFriend, Content.transform.position, Quaternion.identity) as GameObject;
                    Offriend.transform.SetParent(Content.transform, false);

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
                loadfriend = true;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
                AllMenu.ins.DestroyMenu("menuFriend");
            }
        }
    }
    public void TangPhao()
    {
        string taikhoanban = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "DotPhaoHoa";
        datasend["data"]["namephao"] = PhaoChon;
        datasend["data"]["soluong"] = soluongchon.ToString();
        datasend["data"]["taikhoanban"] = taikhoanban;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                GameObject MenuTangPhao = transform.Find("MenuTangPhao").gameObject;
                MenuTangPhao.SetActive(false);
               // Text soluongphaonho = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
              //  Text soluongphaoto = transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
            //    soluongphaonho.text = json["phaohoanho"].Value;
            //    soluongphaoto.text = json["phaohoalon"].Value;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
    public void OpenMenuTop()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "GetTopEvent";
        datasend["data"]["top"] = top.ToString();
        datasend["data"]["trang"] = trangg.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "0")
            {
                sangtrang = false;
                GameObject menu = transform.Find("MenuTop").gameObject;
                // GameObject menu = GetCreateMenu("MenuTop",gameObject,false,3);
                if (json["alltop"].Count > 0)
                {
                    GameObject menutop = menu.transform.GetChild(0).transform.Find("MenuTop").gameObject;

                    GameObject contentop = menutop.transform.GetChild(0).gameObject;
                    //  menu.transform.GetChild(0).GetChild(4).gameObject;

                    if (trangg % 2 == 1)
                    {
                        topcuoi = float.Parse(json["alltop"][0]["diemphaohoa"].Value);
                    }
                    if (json["alltop"].Count > 10)
                    {
                        top = float.Parse(json["alltop"][10]["diemphaohoa"].Value);
                        sangtrang = true;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        contentop.transform.GetChild(i).gameObject.SetActive(false);
                        if (i < json["alltop"].Count)
                        {
                            // debug.Log(json["alltop"][i]["Name"].Value);
                            Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].AsString;
                            //   debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                            //   debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = i + trang;
                            txtTop.text = sotop.ToString();
                            //   debug.Log("ok3");
                            NetworkManager.ins.friend.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar, imgKhungAvatar);
                            // imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                            contentop.transform.GetChild(i).gameObject.SetActive(true);
                            debug.Log("ok4");
                            if (sotop > 3) HuyHieu.sprite = Top;
                            else if (sotop == 1) HuyHieu.sprite = Top1;
                            else if (sotop == 2) HuyHieu.sprite = Top2;
                            else if (sotop == 3) HuyHieu.sprite = Top3;
                            HuyHieu.SetNativeSize();
                            //  debug.Log("ok5");
                            txtName.text = json["alltop"][i]["Name"].Value;
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["diemphaohoa"].AsString;
                            contentop.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
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
                    GameObject objquaphao = menu.transform.GetChild(0).transform.GetChild(6).transform.GetChild(1).gameObject;
                    objquaphao.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["duoctangphao"].Value;//json["quaphaohoa"]
                    Image fillaoumt = objquaphao.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
                    fillaoumt.fillAmount = float.Parse(json["duoctangphao"].Value) / 250;
                    GameObject allqua = objquaphao.transform.GetChild(2).gameObject;

                    for (int i = 0; i < json["quaphaohoa"].Count; i++)
                    {
                        if (json["quaphaohoa"][i].Value == "chuaduocnhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = MenuEventTet2024.inss.chuaduocnhan;
                            break;
                        }
                        if (json["quaphaohoa"][i].Value == "duocnhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = MenuEventTet2024.inss.duocnhan;
                            allqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                            allqua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                            allqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            // else allqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                        else if (json["quaphaohoa"][i].Value == "danhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = MenuEventTet2024.inss.danhan;
                            allqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                            allqua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                            allqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            //else allqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                    }
                }


                if (LoginFacebook.ins.NameServer != "daorongsv1.shop")
                {
                  menu.transform.GetChild(0).transform.Find("MenuPhanThuong").transform.GetChild(0).GetComponent<Image>().sprite = EventManager.ins.GetSprite("bxhtop20");
                }


                menu.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
            }
        }
    }
    public void NhanQuaPhao()
    {
        GameObject btntreo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "NhanQuaTichLuyPhao";
        datasend["data"]["qua"] = btntreo.transform.parent.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                btntreo.gameObject.SetActive(false);
                btntreo.transform.parent.transform.GetChild(2).gameObject.SetActive(true);
                btntreo.transform.parent.transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                btntreo.transform.parent.transform.GetChild(0).GetComponent<Image>().sprite = MenuEventTet2024.inss.danhan;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void sangtrangtop(int i)
    {
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
                OpenMenuTop();
            }
            //   debug.Log(i);
        }
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        AudioManager.PlaySound("soundClick");
        MenuTrieuHoiRongVangBac mn = AllMenu.ins.GetCreateMenu("MenuTrieuHoiRongVangBac", MenuEventTet2024.inss.trencung.gameObject, false).GetComponent<MenuTrieuHoiRongVangBac>();
        mn.Setnamerong = namerong;
        mn.gameObject.SetActive(true);
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
                GameObject menuDoiManh = EventManager.ins.GetCreateMenu("MenuDoiManh", transform, false);
                GameObject ContentManh = menuDoiManh.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject ObjectManh = ContentManh.transform.GetChild(0).gameObject;

                for (int i = 0; i < json["ManhDoi"].Count; i++)
                {
                    GameObject manh = Instantiate(ObjectManh, ContentManh.transform.position, Quaternion.identity);
                    manh.transform.SetParent(ContentManh.transform, false);
                    manh.GetComponent<Button>().onClick.AddListener(XemManhDoi);
                    Image imgmanh = manh.transform.GetChild(0).GetComponent<Image>();
                    if (json["ManhDoi"][i]["itemgi"].Value == "item" || json["ManhDoi"][i]["itemgi"].Value == "thuyenthucan")
                    {
                        imgmanh.sprite = Inventory.LoadSprite(json["ManhDoi"][i]["nameitem"].Value);
                    }
                    else if (json["ManhDoi"][i]["itemgi"].Value == "itemevent")
                    {
                        for (int j = 0; j < EventManager.ins.allitemEvent.Length; j++)
                        {
                            if (json["ManhDoi"][i]["nameitem"].Value == EventManager.ins.allitemEvent[j].name)
                            {
                                imgmanh.sprite = EventManager.ins.allitemEvent[j];
                            }
                        }
                        imgmanh.SetNativeSize();
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
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
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
                transform.GetChild(0).transform.Find("imgLiXi").transform.GetChild(1).GetComponent<Text>().text = json["BaoLiXi"].AsString;
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
    }

}
