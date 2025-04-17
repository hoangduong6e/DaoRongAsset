using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEventTet2023 : MonoBehaviour
{
    GameObject btnHopQua;bool hienthilai = false;
    string PhaoChon = "Phao1Diem";
    int soluongchon = 0;
    public GameObject contentQua, Allnhiemvu; 
    public Sprite sprite1, sprite2, chuaduocnhan, duocnhan, danhan,CayMaiNho, CayMaiVua, CayMaiLon;
    bool sangtrang = false; int trang = 1, trangg = 1;
    float top, topcuoi;
    public Sprite[] spriteItemEvent;
    public Sprite Top1, Top2, Top3, Top; 
    public GameObject ContentManh, ObjectManh;
    string namemanhchon;
    public Dictionary<string, GameObject> MenuEvent = new Dictionary<string, GameObject>();

    void Awake()
    {
        btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
    }
    private void OnEnable()
    {
        //CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetDataEvent/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                //CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.menulogin.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                gameObject.SetActive(false);
                VeNha();
                AllMenu.ins.DestroyMenu("MenuEventTet2023");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    if (NetworkManager.ins.friend.QuaNha) NetworkManager.ins.friend.GoHome();
                    btnHopQua.transform.SetParent(transform.GetChild(0).transform);

                   // AudioManager.SetSoundBg("nhacnoel");
                    CrGame.ins.giaodien.SetActive(true);
                    CrGame.ins.giaodien.transform.SetParent(transform);
              //     CrGame.ins.giaodien.transform.GetChild(5).gameObject.SetActive(false);
                    for (int i = 0; i < NetworkManager.ins.loidai.objGiaoDienOff.Length; i++)
                    {
                        NetworkManager.ins.loidai.objGiaoDienOff[i].SetActive(false);
                    }
                 //   LoadMenuChonBanBe();
                    Text txtdiemphao = transform.GetChild(0).transform.GetChild(7).transform.GetChild(0).GetComponent<Text>();
                    txtdiemphao.text = json["event"]["diemphaohoa"].Value;
                    LoadMocQua(json);

                    //GameObject menunhatki = transform.GetChild(3).transform.GetChild();
                    for (int i = 0; i < json["event"]["allNhiemvu"].Count; i++)
                    {
                        Text txttiendo = Allnhiemvu.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                        if (int.Parse(json["event"]["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value))
                        {
                            txttiendo.text = "<color=#00ff00ff>" + json["event"]["allNhiemvu"][i]["dalam"].Value + "/" + json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                        }
                        else
                        {
                            txttiendo.text = "<color=#ff0000ff>" + json["event"]["allNhiemvu"][i]["dalam"].Value + "/" + json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                        }
                    }
                    LoadDiemDanh(json);
                    Image imgBg = transform.GetChild(0).GetComponent<Image>();
                    if(CrGame.ins.NgayDem == ENgayDem.Ngay) imgBg.sprite = Resources.Load<Sprite>("GameData/EventTet2023/BGBanNgay");
                    else imgBg.sprite = Resources.Load<Sprite>("GameData/EventTet2023/BGBanDem");
                    AudioManager.SetSoundBg("GameData/EventTet2023/nhacnentet",true);
                    CrGame.ins.menulogin.SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    CrGame.ins.menulogin.SetActive(false);
                    VeNha();
                    AllMenu.ins.DestroyMenu("MenuEventNoel");
                }
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }
    public GameObject GetCreateMenu(string namemenu, GameObject parnet = null, bool active = true, int index = 1)
    {
        GameObject g = null;
        if (MenuEvent.ContainsKey(namemenu))
        {
            MenuEvent[namemenu].SetActive(active);
            g = MenuEvent[namemenu];
        }
        else
        {
            if (parnet == null)
            {
                parnet = gameObject;
            }
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/EventTet2023/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
            MenuEvent.Add(namemenu, mn);
            mn.transform.SetParent(parnet.transform, false);
            mn.transform.position = parnet.transform.position;
            mn.SetActive(active);
            g = mn;
            //  Resources.UnloadUnusedAssets();
        }
        g.transform.SetSiblingIndex(index);
        return g;
    }
    public void DestroyMenu(string namemenu)
    {
        if (MenuEvent.ContainsKey(namemenu))
        {
            Destroy(MenuEvent[namemenu]);
            MenuEvent.Remove(namemenu);

            Resources.UnloadUnusedAssets();
        }
    }
    void LoadDiemDanh(JSONNode json)
    {
        GameObject DiemDanh = transform.GetChild(7).transform.GetChild(0).gameObject;
        int solandiemdanh = int.Parse(json["event"]["landiemdanh"].Value);
        DiemDanh.transform.GetChild(1).GetComponent<Text>().text = "Bạn đã điểm danh <color=lime>" + solandiemdanh + " ngày</color>.";
        if (json["event"]["diemdanh"].Value == "True")
        {
            DiemDanh.transform.GetChild(2).GetComponent<Button>().interactable = true;
        }
        if (solandiemdanh < 1) DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/1</color>";
        else
        {
            DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/1</color>";
            if (json["event"]["nhandiemdanh"][0].Value == "duocnhan") DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>().interactable = true;
            else if (json["event"]["nhandiemdanh"][0].Value == "danhan")
            {
                DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(3).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
            }
        }
        if (solandiemdanh < 7) DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/7</color>";
        else
        {
            DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/7</color>";
            if (json["event"]["nhandiemdanh"][1].Value == "duocnhan") DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>().interactable = true;
            else if (json["event"]["nhandiemdanh"][1].Value == "danhan")
            {
                DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(4).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
            }
        }
        if (solandiemdanh < 14) DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/14</color>";
        else
        {
            DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/14</color>";
            if (json["event"]["nhandiemdanh"][2].Value == "duocnhan") DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>().interactable = true;
            else if (json["event"]["nhandiemdanh"][2].Value == "danhan")
            {
                DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(5).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
            }
        }
        if (json["event"]["nhandiemdanh"][3].Value == "duocnhan") DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>().interactable = true;
        else if (json["event"]["nhandiemdanh"][3].Value == "danhan")
        {
            DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>().interactable = false;
            DiemDanh.transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
        }
        //     DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1);
    }
    public void OpenMenuDoiQua()
    {
        if (ContentManh.transform.childCount == 1)
        {
            CrGame.ins.panelLoadDao.SetActive(true);
            StartCoroutine(Load());
            IEnumerator Load()
            {
                UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "MenuDoiManh/taikhoan/" + LoginFacebook.ins.id);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    debug.Log(www.error);
                    CrGame.ins.OnThongBaoNhanh("Lỗi");
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
                else
                {
                    // Show results as text
                    debug.Log(www.downloadHandler.text);
                    JSONNode json = JSON.Parse(www.downloadHandler.text);
                    if (json["status"].Value == "ok")
                    {

                        for (int i = 0; i < json["ManhDoi"].Count; i++)
                        {
                            GameObject manh = Instantiate(ObjectManh, ContentManh.transform.position, Quaternion.identity);
                            manh.transform.SetParent(ContentManh.transform, false);
                            Image imgmanh = manh.transform.GetChild(0).GetComponent<Image>();
                            if (json["ManhDoi"][i]["itemgi"].Value == "item" || json["ManhDoi"][i]["itemgi"].Value == "thuyenthucan")
                            {
                                imgmanh.sprite = Inventory.LoadSprite(json["ManhDoi"][i]["nameitem"].Value);
                            }
                            else if (json["ManhDoi"][i]["itemgi"].Value == "itemevent")
                            {
                                for (int j = 0; j < spriteItemEvent.Length; j++)
                                {
                                    if (spriteItemEvent[j].name == json["ManhDoi"][i]["nameitem"].Value)
                                    {
                                        imgmanh.sprite = spriteItemEvent[j];
                                        imgmanh.SetNativeSize();
                                    }
                                }
                            }
                            else
                            {
                                imgmanh.sprite = Inventory.LoadSpriteRong(json["ManhDoi"][i]["nameitem"].Value + "2");
                            }
                            manh.name = json["ManhDoi"][i]["namekey"];
                            manh.SetActive(true);
                        }
                        transform.GetChild(8).gameObject.SetActive(true);

                    }
                    else
                    {
                        CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    }
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
            }
        }
        else transform.GetChild(8).gameObject.SetActive(true);
    }
    public void XemManhDoi()
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        GameObject yeucau = transform.GetChild(8).transform.GetChild(1).gameObject;
        namemanhchon = btnchon.name;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemManhDoi/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + btnchon.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
            }
            else
            {
                // Show results as text
                Button btndoi = yeucau.transform.GetChild(2).GetComponent<Button>();
                btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                yeucau.transform.GetChild(0).gameObject.SetActive(false);
                yeucau.transform.GetChild(1).gameObject.SetActive(false);

                Text txtlixiyeucau = null;
                bool ok = true;
                
                if (json["BaoLiXi"].Value != "")
                {
                    //  debug.LogError("keogiangsinh ne " + json["BiNgo"].Value);

                    yeucau.transform.GetChild(0).gameObject.SetActive(true);

                    int sokeoco = int.Parse(transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text);

                    //debug.LogError(solixico);
                    if (sokeoco >= int.Parse(json["BaoLiXi"].Value))
                    {
                        yeucau.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>" + sokeoco + "/" + json["BaoLiXi"].Value + "</color>";
                        if (ok) ok = true;
                        //dieukien[i] = true;
                    }
                    else
                    {
                        yeucau.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "<color=#ff0000ff>" + sokeoco + "/" + json["BaoLiXi"].Value + "</color>";
                        ok = false;
                    }
                }

                if (json["HuyChuongDungKhi"].Value != "")
                {
                    yeucau.transform.GetChild(1).gameObject.SetActive(true);
                    txtlixiyeucau = yeucau.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

                    if (Inventory.ins.ListItemThuong.ContainsKey("itemHuyChuongDungKhi"))
                    {
                        int solixico = int.Parse(Inventory.ins.ListItemThuong["itemHuyChuongDungKhi"].transform.GetChild(0).GetComponent<Text>().text);
                        if (solixico >= int.Parse(json["HuyChuongDungKhi"]))
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>" + solixico + "/" + json["HuyChuongDungKhi"].Value + "</color>";
                            if (ok) ok = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>" + solixico + "/" + json["HuyChuongDungKhi"].Value + "</color>";
                            ok = false;
                        }
                    }
                    else
                    {
                        if (json["HuyChuongDungKhi"].Value == "0")
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>0/" + json["HuyChuongDungKhi"].Value + "</color>";
                            if (ok) ok = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>0/" + json["HuyChuongDungKhi"].Value + "</color>";
                            ok = false;
                        }
                    }
                }

                Text txtthongtin = transform.GetChild(8).transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
                txtthongtin.text = json["thongtin"].Value;

                btndoi.interactable = ok;
                yeucau.SetActive(true);
            }
        }
    }
    public void DoiManh()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "DoiManh/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + namemanhchon);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                btndoi.interactable = true;
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject menuTuoiCay = transform.GetChild(0).gameObject;
                    Text txtBaoLiXi = menuTuoiCay.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
                    Text soluongphaonho = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
                    Text soluongphaoto = transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
                    txtBaoLiXi.text = json["BaoLiXi"].Value;
                    soluongphaonho.text = json["phaohoanho"].Value;
                    soluongphaoto.text = json["phaohoalon"].Value;
                    transform.GetChild(8).transform.GetChild(1).gameObject.SetActive(false);
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
    public void DiemDanh()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "DiemDanhEvent/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    LoadDiemDanh(json);
                    CrGame.ins.OnThongBaoNhanh("Đã điểm danh!");
                    btn.interactable = false;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }   
    public void NhanQuaDiemDanh()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int vitri = btn.transform.parent.GetSiblingIndex()-3;

        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaDiemDanh/taikhoan/" + LoginFacebook.ins.id + "/vitri/" + vitri);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    if (json["phaohoalon"].Value != "") transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>().text = json["phaohoalon"].Value;
                    if (json["SieuThanhThuy"].Value != "") transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = json["SieuThanhThuy"].Value;
                    CrGame.ins.OnThongBaoNhanh("Đã nhận quà");
                    btn.interactable = false;
                    btn.transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    void LoadMocQua(JSONNode json)
    {
        GameObject menuTuoiCay = transform.GetChild(0).gameObject;
        Text txtSieuThanhThuy = menuTuoiCay.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
        Text txtBaoLiXi = menuTuoiCay.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
        Text soluongphaonho = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
        Text soluongphaoto = transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
        txtSieuThanhThuy.text = json["event"]["SieuThanhThuy"].Value;
        txtBaoLiXi.text = json["event"]["BaoLiXi"].Value;
        soluongphaonho.text = json["event"]["phaohoanho"].Value;
        soluongphaoto.text = json["event"]["phaohoalon"].Value;

        Text txtsoluottreo = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        float soluottreo = float.Parse(json["event"]["soluottuoicay"].Value);
        txtsoluottreo.text = soluottreo + "";
        Image img = contentQua.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>();
        float fillamount = soluottreo / 1000;
        img.fillAmount = fillamount;
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
            GameObject trangtri = Inventory.LoadObjectResource("GameData/EventTet2023/TrangTri");
            debug.Log("trang tri " + trangtri.name);
            GameObject objtrangtri = Instantiate(trangtri, transform.position,Quaternion.identity); //transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
            objtrangtri.transform.SetParent(transform.GetChild(0).transform.GetChild(0).transform,false);
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
        for (int i = 0; i < json["event"]["QuaTichLuy"].Count; i++)
        {
            if (json["event"]["QuaTichLuy"][i].Value == "chuaduocnhan")
            {
                string name = contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = chuaduocnhan;
                contentQua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = ((i + 1) * 50) + "";
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                if (i > 0) contentQua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite1;
                else contentQua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite1;
                //  break;
            }
            if (json["event"]["QuaTichLuy"][i].Value == "duocnhan")
            {
                string name = contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = duocnhan;
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                if (i > 0) contentQua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                else contentQua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
            }
            else if (json["event"]["QuaTichLuy"][i].Value == "danhan")
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
    public void ExitMenuTop()
    {
        GameObject menutop = transform.GetChild(3).gameObject;
        sangtrang = false; trang = 1; trangg = 1; top = 0; topcuoi = 0;
        GameObject contentnhatki = transform.GetChild(3).transform.GetChild(0).transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetChild(1).gameObject;
        for (int i = 0; i < contentnhatki.transform.childCount; i++)
        {
            Destroy(contentnhatki.transform.GetChild(i).gameObject);
        }
        menutop.SetActive(false);
    }
    public void OpenMenuTop()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            //  CrGame.ins.OnThongBao(true, "Đang tải...", false);
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetTopEvent/taikhoan/" + LoginFacebook.ins.id + "/top/" + top + "/trang/" + trangg);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
            }
            else
            {
                // Show results as text 
                sangtrang = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                //GameObject objecttop = transform.GetChild(5).transform.GetChild(6).gameObject;
                 GameObject menu = transform.GetChild(3).gameObject;
               // GameObject menu = GetCreateMenu("MenuTop",gameObject,false,3);
                if (json["alltop"].Count > 0)
                {
                    

                     GameObject menutop = menu.transform.GetChild(0).GetChild(4).gameObject;

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
                  //  else if (i == json["alltop"].Count) topcuoi = float.Parse(json["alltop"][i]["diemphaohoa"].Value);
                    for (int i = 0; i < 10; i++)
                    {
                        contentop.transform.GetChild(i).gameObject.SetActive(false);
                        if (i < json["alltop"].Count)
                        {
                            debug.Log(json["alltop"][i]["Name"].Value);
                            Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].Value;
                            debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                            debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = i + trang;
                            txtTop.text = sotop.ToString();
                            debug.Log("ok3");
                         //   NetworkManager.ins.friend.GetAvatarFriend(json["alltop"][i]["idfb"].Value, imgAvatar);
                            imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                            contentop.transform.GetChild(i).gameObject.SetActive(true);
                            debug.Log("ok4");
                            if (sotop > 3) HuyHieu.sprite = Top;
                            else if (sotop == 1) HuyHieu.sprite = Top1;
                            else if (sotop == 2) HuyHieu.sprite = Top2;
                            else if (sotop == 3) HuyHieu.sprite = Top3;
                            HuyHieu.SetNativeSize();
                            debug.Log("ok5");
                            txtName.text = json["alltop"][i]["Name"].Value;
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["diemphaohoa"].Value;
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
                if(trang == 1)
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
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = chuaduocnhan;
                            break;
                        }
                        if (json["quaphaohoa"][i].Value == "duocnhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = duocnhan;
                            allqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                            allqua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                            allqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            // else allqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                        else if (json["quaphaohoa"][i].Value == "danhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = danhan;
                            allqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                            allqua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                            allqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            //else allqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                    }
                }
             


                menu.SetActive(true);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void OpenMenuChonPhao()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(3).GetComponent<InputField>().text = "0";
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(5).GetComponent<Button>().interactable = false;
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(5).GetComponent<Button>().interactable = false;
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).GetComponent<InputField>().text = "0";
        soluongchon = 0;
    }
    public void NhanQuaPhao()
    {
        GameObject btntreo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaTichLuyPhao/taikhoan/" + LoginFacebook.ins.id + "/qua/" + btntreo.transform.parent.GetSiblingIndex());
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    btntreo.gameObject.SetActive(false);
                    btntreo.transform.parent.transform.GetChild(2).gameObject.SetActive(true);
                    btntreo.transform.parent.transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                    btntreo.transform.parent.transform.GetChild(0).GetComponent<Image>().sprite = danhan;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }    
    public void NhanQuaTichLuy()
    {
        GameObject btntreo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaTichLuyEvent/taikhoan/" + LoginFacebook.ins.id + "/qua/" + btntreo.transform.parent.GetSiblingIndex());
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    btntreo.gameObject.SetActive(false);
                    btntreo.transform.parent.transform.GetChild(2).gameObject.SetActive(true);
                    btntreo.transform.parent.transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                    btntreo.transform.parent.transform.GetChild(0).GetComponent<Image>().sprite = danhan;
                    if (json["BaoLiXi"].Value != "")
                    {
                        GameObject menuTuoiCay = transform.GetChild(0).gameObject;
                      
                        Text txtBaoLiXi = menuTuoiCay.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
                        txtBaoLiXi.text = json["BaoLiXi"].Value;
                    }
                    debug.Log(json["event"].Count);
                    if (json["event"].Count > 0)
                    {
                        LoadMocQua(json);
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    //public void LoadMenuChonBanBe()
    //{
    //    Friend friend = CrGame.ins.GetComponent<Friend>();
    //    GameObject viewport = transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject;
    //    GameObject ObjFriend = viewport.transform.GetChild(0).gameObject;
    //    GameObject Content = viewport.transform.GetChild(1).gameObject;
    //    for (int i = 0; i < friend.ContentFriend.transform.childCount; i++)
    //    {
    //        btnFriend btnfr = friend.ContentFriend.transform.GetChild(i).GetComponent<btnFriend>();
    //        GameObject ofr = Instantiate(ObjFriend, Content.transform.position, Quaternion.identity) as GameObject;
    //        Image imgAvatar = ofr.transform.GetChild(1).GetComponent<Image>();
    //        Image imgKhung = ofr.transform.GetChild(2).GetComponent<Image>();
    //        Text txtname = ofr.transform.GetChild(3).GetComponent<Text>();
    //        imgAvatar.sprite = friend.ContentFriend.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite;
    //        txtname.text = friend.ContentFriend.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text;
    //        imgKhung.sprite = friend.ContentFriend.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite;
    //        ofr.transform.SetParent(Content.transform, false);
    //        ofr.SetActive(true);
    //        ofr.name = btnfr.idfb;
    //        ofr.SetActive(true);
    //        //ofr.name = json[i]["idfb"].Value + "+" + json[i]["name"].Value;
    //    }
    //}
    public void ChonPhao()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        PhaoChon = btnchon.transform.parent.name;
        soluongchon = int.Parse(btnchon.transform.parent.GetChild(3).GetComponent<InputField>().text);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
    }    
    public void TangPhao()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        string taikhoanban = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "DotPhaoHoa/taikhoan/" + LoginFacebook.ins.id+"/namephao/"+PhaoChon+"/soluong/"+soluongchon+"/taikhoanban/"+taikhoanban);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                GameObject menubanbe = transform.GetChild(2).gameObject;
                menubanbe.SetActive(false);

                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    Text soluongphaonho = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
                    Text soluongphaoto = transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
                    soluongphaonho.text = json["phaohoanho"].Value;
                    soluongphaoto.text = json["phaohoalon"].Value;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value,2);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }    
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
        if(i > 0)
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
            if(Soluongdachon > 0)
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
    public void XemTuoiCayMai()
    {
        Text txtSieuThanhThuy = transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
        if (txtSieuThanhThuy.text != "0" || hienthilai)
        {
            TuoiNuoc();
        }    
        else
        {
            CrGame.ins.panelLoadDao.SetActive(true);
            StartCoroutine(Load());
            IEnumerator Load()
            {
                UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemGiaMuaBinh/taikhoan/" + LoginFacebook.ins.id);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    debug.Log(www.error);
                    CrGame.ins.OnThongBaoNhanh("Lỗi");
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
                else
                {
                    // Show results as text
                    debug.Log(www.downloadHandler.text);
                    GameObject menuxacnhan = transform.GetChild(4).gameObject;
                    Text txtxacnhan = menuxacnhan.transform.GetChild(2).GetComponent<Text>();
                    txtxacnhan.text = www.downloadHandler.text;
                    menuxacnhan.SetActive(true);
                    CrGame.ins.panelLoadDao.SetActive(false);
                    hienthilai = false;
                    menuxacnhan.transform.GetChild(5).GetComponent<Toggle>().isOn = false;
                }
            }
        }
      
        // TuoiNuoc();
    }    
     public void TuoiNuoc()
     {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "TuoiCay/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                GameObject menuxacnhan = transform.GetChild(4).gameObject;
                menuxacnhan.SetActive(false);

                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject binhnuoc = transform.GetChild(0).transform.GetChild(5).gameObject;
                    Button btncaymai = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();

                    //GameObject menuTuoiCay = transform.GetChild(0).gameObject;
                    //Text txtSieuThanhThuy = menuTuoiCay.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
                    //txtSieuThanhThuy.text = json["SieuThanhThuy"].Value;
                    hienthilai = menuxacnhan.transform.GetChild(5).GetComponent<Toggle>().isOn;

                    //Text txtsoluottreo = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    //float soluottreo = float.Parse(txtsoluottreo.text) + 1;
                    LoadMocQua(json);
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        btncaymai.enabled = false;
                        binhnuoc.SetActive(true);
                        yield return new WaitForSeconds(2.3f);
                        binhnuoc.SetActive(false);
                        btncaymai.enabled = true;
                        //if (soluottreo == 100)
                        //{
                        //    Image imgcaymai = menuTuoiCay.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
                        //    imgcaymai.sprite = CayMaiVua;
                        //}
                        //if (soluottreo == 300)
                        //{
                        //    Image imgcaymai = menuTuoiCay.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
                        //    imgcaymai.sprite = CayMaiLon;
                        //}
                        
                    }

                   
                    
                    //txtsoluottreo.text = soluottreo + "";
                    //Image img = contentQua.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>();
                    //float fillamount = soluottreo / 1000;
                    //img.fillAmount = fillamount;

                    //if (soluottreo % 50 == 0)
                    //{
                    //    if (soluottreo <= 1000)
                    //    {
                    //        debug.Log("ok1");
                    //        contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(1).gameObject.SetActive(true);
                    //        contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(2).gameObject.SetActive(false);
                    //        debug.Log("ok2");
                    //        if ((int)soluottreo / 50 - 1 > 0) contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                    //        else contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                    //        debug.Log("ok3");

                    //        string name = contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(0).GetComponent<Image>().sprite.name;
                    //        if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(0).GetComponent<Image>().sprite = duocnhan;
                    //        contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(1).gameObject.SetActive(true);
                    //        contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(2).gameObject.SetActive(false);
                    //        if ((int)soluottreo / 50 - 1 > 0) contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                    //        else contentQua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;

                    //    }
                      
                    //}
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }    
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }

       
    }
    public void VeNha()
    {
         //gameObject.SetActive(false);
        transform.GetChild(transform.childCount-1).SetParent(GameObject.Find("CanvasS").transform);
        CrGame.ins.giaodien.transform.GetChild(5).gameObject.SetActive(true);
        for (int i = 0; i < NetworkManager.ins.loidai.objGiaoDienOff.Length; i++)
        {
            NetworkManager.ins.loidai.objGiaoDienOff[i].SetActive(true);
        }
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        AudioManager.SetSoundBg("nhacnen0");
        CrGame.ins.giaodien.SetActive(true);
        CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(true);
        AllMenu.ins.DestroyMenu("MenuEventTet2023");
        Resources.UnloadUnusedAssets();
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        Button btnTrieuHoi = transform.GetChild(5).GetChild(0).transform.GetChild(3).GetComponent<Button>();
        GameObject AllmanhRong = transform.GetChild(5).GetChild(0).transform.GetChild(2).gameObject;
        if (namerong == "vang")
        {
            string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
            int soluongmanhco = 0;
            for (int i = 0; i < AllmanhRong.transform.childCount; i++)
            {
                Image img = AllmanhRong.transform.GetChild(i).GetComponent<Image>();
                img.sprite = Inventory.LoadSprite(allnamemanh[i]);
                if (Inventory.ins.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
                {
                    debug.Log(allnamemanh[i]);
                    img.color = new Color32(255, 255, 255, 255);
                    soluongmanhco += 1;
                }
                else img.color = new Color32(125, 125, 125, 181);
                if (soluongmanhco == 5) btnTrieuHoi.interactable = true;
                else btnTrieuHoi.interactable = false;
            }
        }
        else if (namerong == "bac")
        {
            string[] allnamemanh = new string[] { "DauRongBac", "CanhRongBac", "ChanRongBac", "ThanRongBac", "DuoiRongBac" };
            int soluongmanhco = 0;
            for (int i = 0; i < AllmanhRong.transform.childCount; i++)
            {
                Image img = AllmanhRong.transform.GetChild(i).GetComponent<Image>();
                img.sprite = Inventory.LoadSprite(allnamemanh[i]);
                if (Inventory.ins.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
                {
                    img.color = new Color32(255, 255, 255, 255);
                    soluongmanhco += 1;
                }
                else img.color = new Color32(125, 125, 125, 181);
                if (soluongmanhco == 5) btnTrieuHoi.interactable = true;
                else btnTrieuHoi.interactable = false;
            }
        }
        AllmanhRong.name = namerong;
        transform.GetChild(5).gameObject.SetActive(true);
    }

    public void TrieuHoiRong()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        GameObject AllmanhRong = transform.GetChild(5).GetChild(0).transform.GetChild(2).gameObject;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "TrieuHoiRongEventTet/taikhoan/" + LoginFacebook.ins.id + "/namerong/" + AllmanhRong.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                btndoi.interactable = true;
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"] == "0")
                {
                    transform.GetChild(5).GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                    StartCoroutine(HieuUngTrieuHoi());
                    if (AllmanhRong.name == "vang")
                    {
                        string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            Inventory.ins.AddItem(allnamemanh[i], -1);
                        }
                    }
                    else if (AllmanhRong.name == "bac")
                    {
                        string[] allnamemanh = new string[] { "DauRongBac", "CanhRongBac", "ChanRongBac", "ThanRongBac", "DuoiRongBac" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            Inventory.ins.AddItem(allnamemanh[i], -1);
                        }
                    }
                }
                else CrGame.ins.OnThongBaoNhanh(json["thongbao"], 2);
                IEnumerator HieuUngTrieuHoi()
                {
                    yield return new WaitForSeconds(0.5f);
                    GameObject g = Instantiate(transform.GetChild(5).GetChild(1).gameObject, transform.position, Quaternion.identity);
                    g.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
                    g.gameObject.SetActive(true);
                    Image img = g.GetComponent<Image>();
                    img.sprite = Inventory.LoadSpriteRong(json["namerong"]);
                    img.SetNativeSize();
                    transform.GetChild(5).GetChild(0).transform.GetChild(4).gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.1f);
                    QuaBay quabay = img.GetComponent<QuaBay>();
                    quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                    quabay.enabled = true;
                    transform.GetChild(5).gameObject.SetActive(false);
                }
            }
        }
    }
}
