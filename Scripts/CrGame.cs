using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;
using System;

public class CrGame : MonoBehaviour
{
    public GameObject[] GDien;
    public Image imgTimeDragon, imgThanhLevel;
    public GameObject menuload, btnOk, menulogin, PanelThangCap; public Text txtLoad, txtThangCap;
    public Text txtLevel; public float MaxExp;
    public string nameCongtrinh; public GameObject VungCongTrinh;
    public Text txtVang, txtKimCuong,txtDanhVong;
    public GameObject MenuMoDao, panelLoadDao, tuithucAn; public byte DangODao = 1;
    public string ServerName, NgayDem = "Ngay"; ThuyenThucAn thuyen; public Transform TfrongInfo;
    public Image ngaydem;
    public Image Bg;//public GameObject HieuungNgoiSao;
    public GameObject giaodien;
    public byte[] leveldao; public GameObject menuInfoDao; public Image khungAvatar;
    public Text FB_userName, txtnhac, txtamthanh, txtTimeQuaonl; public Image FB_useerDp; public GameObject AllDao; public Ui UI;
    public GameObject BtnXemQuangCao;public byte sodao = 7;
    public static CrGame ins;
    public Transform trencung;
    public Dictionary<string, bool> allHienQuaBay = new Dictionary<string, bool>();

    public float timeNuiThanBi = 0;

    // Start is called before the first frame update
    public void SetPanelThangCap(bool set)
    {
        if (!set)
        {
            Transform eff = PanelThangCap.transform.Find("EffLenCap");
            if(eff != null) eff.transform.SetParent(AllMenu.ins.transform);
        }
       // PanelThangCap.transform.Find("txtThangCap").GetComponent<Text>().text = txt;
        PanelThangCap.SetActive(set);
    }
    public IEnumerator offlencap(string offname)
    {
        yield return new WaitForSeconds(4);
        if (txtThangCap.text == offname)
        {
            SetPanelThangCap(false);
        }
    }
    private void Awake()
    {
        ins = this;
    }
    void Start()
    {

        //// Lấy thời gian hiện tại ở múi giờ của máy tính
        //DateTime currentTime = DateTime.UtcNow;

        //// Chuyển đổi thời gian sang múi giờ của Việt Nam
        //DateTime vietnamTime = ConvertToVietnamTime(currentTime);

        //// In ra thời gian hiện tại ở Việt Nam
        //debug.Log("Thời gian hiện tại ở Việt Nam: " + vietnamTime.ToString());

        string CultureName = Thread.CurrentThread.CurrentCulture.Name;
        CultureInfo ci = new CultureInfo(CultureName);

        if (ci.NumberFormat.NumberDecimalSeparator != ".")
        {
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
        }

        if (PlayerPrefs.GetString("nhac") == "false")
        {
            // AudioManager.SoundBg.Stop();
            Setting.amthanh = false;
            txtnhac.text = "Nhạc: Tắt";
        }
        if (PlayerPrefs.GetString("amthanh") == "false")
        {
            Setting.hieuungamthanh = false;
            txtamthanh.text = "Âm thanh: Tắt";
        }
        if (PlayerPrefs.HasKey("cauhinh"))
        {
            CauHinh cauhinh = (CauHinh)Enum.Parse(typeof(CauHinh), PlayerPrefs.GetString("cauhinh"));
            Text txt = txtamthanh.transform.parent.transform.parent.transform.Find("btnChatLuongGame").transform.GetChild(0).GetComponent<Text>();
            Setting.cauhinh = cauhinh;
            txt.text = Setting.GetStrCauHinh(Setting.cauhinh);
        }

        // zoomcam = GetComponent<ZoomCamera>();
        giaodien = GameObject.Find("Giaodien");
        //  UI = MenuNangCapItem.GetComponent<Ui>();
        // StartCoroutine(XemThucAn("ThucAnThit",1));
        thuyen = GetComponent<ThuyenThucAn>();
        GameObject CanvasGame = GameObject.Find("CanvasGame");
        for (int i = 0; i < GDien.Length; i++)
        {
            GDien[i].transform.SetParent(CanvasGame.transform);
            //string a = "<color=#00ff00ff>" + catchuoi[0] + "kinh nghiệm </color>\nRớt <color=#ffff00ff>" + catchuoi[1] + " tiền vàng </color>mỗi <color=#ffff00ff>" + catchuoi[2] + " giây</color>\ntổng thu hoạch: <color=#ffff00ff>" + catchuoi[3] + " tiền vàng </color>\nkinh nghiệm cho rồng con:<color=#ff00ffff>" + catchuoi[4] + "</color>\ntiêu hóa trong: <color=#ffff00ff>" + catchuoi[5] + " phút</color> \nsử dụng hiệu quả đến cấp độ:<color=#ffff00ff>" + catchuoi[6] + "</color>";
        }
        GDien = null;
        System.DateTime timeOfDayGreeting = System.DateTime.Now;
        if (timeOfDayGreeting.Hour >= 6 && timeOfDayGreeting.Hour < 16)
        {
            ngaydem.color = new Color(1, 1, 1, 0);
            NgayDem = "Ngay";
        }
        else if (timeOfDayGreeting.Hour >= 16 && timeOfDayGreeting.Hour < 18 || timeOfDayGreeting.Hour >= 5 && timeOfDayGreeting.Hour < 6)
        {
            // debug.Log("Buổi Trưa");
            ngaydem.color = new Color(1, 1, 1, 0.3745098f);
            Bg.sprite = Inventory.LoadSprite("BanNgayBG");
            NgayDem = "Ngay";
        }
        else
        {
            ngaydem.color = new Color(1, 1, 1, 1);
            Bg.sprite = Inventory.LoadSprite("BanDemBG");
            NgayDem = "Dem";
            GameObject.FindGameObjectWithTag("MainCamera").transform.Find("DomDom").gameObject.SetActive(true);
            //for (int i = 0; i < Random.Range(15, 30); i++)
            //{
            //    GameObject dom = Instantiate(Inventory.ins.GetEffect("domdom"), transform.position, Quaternion.identity) as GameObject;
            //    dom.name = "domdom";
            //    dom.transform.SetParent(GameObject.Find("BgAnhSang").transform);
            //    dom.SetActive(true);
            //}
        }
        //  Friend.ins.GetAvatarFriend(LoginFacebook.ins.id, FB_useerDp);
        //if (PlayerPrefs.HasKey("autologin"))
        // else FB_useerDp.sprite = LoginFacebook.ins.spriteAvatar;
        //FB_userName.text = LoginFacebook.ins.nameNv;
        //  net.socket.Emit("Login", JSONObject.CreateStringObject(LoginFacebook.ins.id));
        //  testdownassetsbundle();
#if UNITY_IOS
  if (LoginFacebook.ins.id == "testadmin")
        {
           // GameObject.Find("btnNap").GetComponent<Button>().onClick.AddListener(delegate { AllMenu.ins.OpenMenu("MenuNapinapp"); });
               giaodien.transform.Find("GameObjecttat").transform.Find("btnNap").GetComponent<Button>().onClick.AddListener(delegate { AllMenu.ins.OpenMenu("MenuNapinapp"); });
        }
        else
        {
             giaodien.transform.Find("GameObjecttat").transform.Find("btnNap").GetComponent<Button>().onClick.AddListener(delegate { AllMenu.ins.OpenMenu("MenuNapThe"); });
        }
#elif UNITY_ANDROID
        giaodien.transform.Find("GameObjecttat").transform.Find("btnNap").GetComponent<Button>().onClick.AddListener(delegate { AllMenu.ins.OpenMenu("MenuNapThe"); });
#endif
    }
    DateTime ConvertToVietnamTime(DateTime currentTime)
    {
        // Lấy thông tin về múi giờ của Việt Nam
        TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // Chuyển đổi thời gian sang múi giờ của Việt Nam
        DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, vietnamTimeZone);

        return vietnamTime;
    }
    public static string FormatCash(double n)
    {
        if (n < 1e3) return n.ToString(CultureInfo.InvariantCulture);
        if (n >= 1e3 && n < 1e6) return (n / 1e3).ToString("0.##") + "K";
        if (n >= 1e6 && n < 1e9) return (n / 1e6).ToString("0.##") + "M";
        if (n >= 1e9 && n < 1e12) return (n / 1e9).ToString("0.##") + "B";
        if (n >= 1e12) return (n / 1e12).ToString("0.##") + "T";

        return n.ToString(CultureInfo.InvariantCulture);
    }
    public void BatDauReplay()
    {
        //menulogin.SetActive(false);
        //VienChinh.vienchinh.chedodau = "Replay";
        //VienChinh.vienchinh.enabled = true;

        //ReplayData.StartReplay();
       // ReplayData.Record = false;
        //ReplayData.Replay = true;
    }
    //public void TangTocReplay()
    //{

    //}
    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    BatDauReplay();
        //    debug.Log("Bắt đầu replay");
        //}
        //if (Input.GetKeyUp(KeyCode.F1))
        //{
        //    debug.Log("Speed replay = 2");
        //    ReplayData.speedReplay = 2;
        //}
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    debug.Log("Speed replay = 3");
        //    ReplayData.speedReplay = 3;
        //}
        if(timeNuiThanBi > 0)
        {
            timeNuiThanBi -= Time.deltaTime;
        }
        #if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.L))
        {
            if (Time.timeScale == 2)
            {
                Time.timeScale = 3;
                return;
            }
            Time.timeScale = (Time.timeScale == 1)?Time.timeScale=2:Time.timeScale=1;
        }
        #endif
    }
    public void PointerDownNhatItemRoi(PointerEventData p)
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        AllMenu.ins.OpenMenu("menuNhatItem");
        GameObject menu = AllMenu.ins.menu["menuNhatItem"];
        Transform g = menu.transform.GetChild(0);
        g.transform.position = btnchon.transform.position;
        debug.Log("PointerDownNhatItemRoi " + btnchon.name);
        Image fill = g.transform.Find("fill").GetComponent<Image>();
        fill.fillAmount = 0;
        float process = 0;
        VienChinh.vienchinh.StartCoroutine(delay2());
        VienChinh.vienchinh.StartCoroutine(delay());
       
        IEnumerator delay()
        {
            
            for (byte i = 0; i < 90;i++)
            {
                process += 1;
                fill.fillAmount = process / 100;
                yield return new WaitForSeconds(0.005f);
                if (process >= 100) yield break;
            }
        }
        IEnumerator delay2()
        {
            yield return new WaitForSeconds(0.3f);
            if (!menu.activeSelf) yield break;
            JSONClass datasend = new JSONClass();
            datasend["class"] = "DragonIsland";
            datasend["method"] = "NhatItemRoi";
            datasend["data"]["nameitem"] = btnchon.name;
            if (Friend.ins.QuaNha) datasend["data"]["dao"] = Friend.ins.DangODaoFriend.ToString();
            else datasend["data"]["dao"] = DangODao.ToString();
            if(Friend.ins.QuaNha) datasend["data"]["chudao"] = "friend";
            else datasend["data"]["chudao"] = "this";
            datasend["data"]["trangthai"] = "nhat";
            datasend["data"]["nameitemm"] = btnchon.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;
            NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
            void Ok(JSONNode json)
            {
                debug.Log(json.ToString());
                if (json["status"].AsString == "0")
                {
                    VienChinh.vienchinh.StartCoroutine(delayy());
                    IEnumerator delayy()
                    {
                        for (byte i = 0; i < 100; i++)
                        {
                            process += 1;
                            fill.fillAmount = process / 100;
                            yield return new WaitForSeconds(0.03f);
                            if (process >= 100)
                            {
                                menu.SetActive(false);
                                yield break;
                            }
                        }
                    }
                }
                else if (json["status"].AsString == "1")
                {
                    menu.SetActive(false);
                    OnThongBaoNhanh(json["message"].AsString);
                }
                else if (json["status"].AsString == "3")
                {
                    Destroy(btnchon.gameObject);
                    menu.SetActive(false);
                    VienChinh.vienchinh.StopAllCoroutines();
                    OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
       
    }
    public void PointerUpNhatItemRoi(PointerEventData p)
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        debug.Log("PointerUpNhatItemRoi " + btnchon.name);

        GameObject menu = AllMenu.ins.menu["menuNhatItem"];
        Transform g = menu.transform.GetChild(0);
        Image fill = g.transform.Find("fill").GetComponent<Image>();
        if (fill.fillAmount < 0.1f)
        {
            VienChinh.vienchinh.StopAllCoroutines();
            menu.SetActive(false);
            return;
        }


        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "NhatItemRoi";
        datasend["data"]["nameitem"] = btnchon.name;
        datasend["data"]["trangthai"] = "tha";
        datasend["data"]["nameitemm"] = btnchon.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;

        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            VienChinh.vienchinh.StopAllCoroutines();
            menu.SetActive(false);
        }
    }
    public void DonDepDao()
    {
        for (int i = 1; i < AllDao.transform.childCount; i++)
        {
            if(AllDao.transform.GetChild(i).name != "BGDao" + DangODao)
            {
                Destroy(AllDao.transform.GetChild(i).gameObject);
            }
        }
    }
    public void XemThongTin()
    {
        if (AllMenu.ins.menu.ContainsKey(nameof(GiaoDienLanSu))) return;
        MenuTTNguoichoi menutt = AllMenu.ins.GetCreateMenu("GiaoDienThongTin", trencung.gameObject, false, 1).GetComponent<MenuTTNguoichoi>();
        menutt.ID = LoginFacebook.ins.id;
        menutt.gameObject.SetActive(true);
    }
    public void CheckBiaGame()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "CloseBiaGame";
        NetworkManager.ins.SendServer(datasend.ToString(), oK,true);
        void oK(JSONNode json)
        {

        }
    }
    public void OpenMenuEvent()
    {
      //  OnThongBaoNhanh("Sắp ra mắt!");
      //  return;
        menulogin.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.4f);

            JSONClass datasend = new JSONClass();
            datasend["class"] = "EventTrungThu2024";
            datasend["method"] = "GetData";
            NetworkManager.ins.SendServer(datasend.ToString(), GetEventok);
        }

        void GetEventok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {   
                // AllMenu.ins.OpenMenuTrenCung("MenuEvent7VienNgocRong2023");
                //    GameObject gdevent = AllMenu.ins.GetCreateMenu("MenuEvent7VienNgocRong2023",AllMenu.ins.gameObject,false);
                if (Friend.ins.QuaNha)
                {
                    Friend.ins.GoHome();
                }
               GameObject gdevent = AllMenu.ins.GetCreateMenu("MenuEventTrungThu2024", AllMenu.ins.gameObject);//AllMenu.ins.transform.GetChild(AllMenu.ins.transform.childCount - 1).gameObject;
               // GameObject gdevent = AllMenu.ins.transform.Find("MenuEventTrungThu2024").gameObject;

                gdevent.SetActive(true);
                gdevent.name = "MenuEventTrungThu2024";
                gdevent.GetComponent<MenuEventTrungThu2024>().ParseData(json);
                DonDepDao();
            
                menulogin.SetActive(false);

                //foreach (string key in json["dataEvent"]["allItem"].AsObject.Keys)
                //{
                //    DataManager.DataGame[key] = result[key];

                //    //   debug.Log("key" + key);
                //}

            }
            else
            {
                OnThongBaoNhanh(json["message"].AsString);
                menulogin.SetActive(false);
            }
        }

    }
    public void OpenMenuEvent2()
    {
        //  OnThongBaoNhanh("Sắp ra mắt!");
        //  return;
        menulogin.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.4f);

            JSONClass datasend = new JSONClass();
            datasend["class"] = "EventHalloween2024";
            datasend["method"] = "GetData";
            NetworkManager.ins.SendServer(datasend.ToString(), GetEventok);
        }

        void GetEventok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                // AllMenu.ins.OpenMenuTrenCung("MenuEvent7VienNgocRong2023");
                //    GameObject gdevent = AllMenu.ins.GetCreateMenu("MenuEvent7VienNgocRong2023",AllMenu.ins.gameObject,false);
                if (Friend.ins.QuaNha)
                {
                    Friend.ins.GoHome();
                }
              // GameObject gdevent = AllMenu.ins.GetCreateMenu("MenuEventHalloween", AllMenu.ins.gameObject);//AllMenu.ins.transform.GetChild(AllMenu.ins.transform.childCount - 1).gameObject;
                GameObject gdevent = AllMenu.ins.transform.Find("MenuEventHalloween2024").gameObject;

                gdevent.SetActive(true);
                gdevent.GetComponent<MenuEventHalloween2024>().ParseData(json);
                DonDepDao();

                menulogin.SetActive(false);

                //foreach (string key in json["dataEvent"]["allItem"].AsObject.Keys)
                //{
                //    DataManager.DataGame[key] = result[key];

                //    //   debug.Log("key" + key);
                //}

            }
            else
            {
                OnThongBaoNhanh(json["message"].AsString);
                menulogin.SetActive(false);
            }
        }

    }
    public void OpenMenuRaKhoi()
    {
        //  OnThongBaoNhanh("Sắp ra mắt!");
        //  return;


        Transform dangodao = AllDao.transform.Find("BGDao" + DangODao);
        Vector3 vec = dangodao.transform.position;
        vec.z = -10;
        transform.position = vec;

        menulogin.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.4f);

            JSONClass datasend = new JSONClass();
            datasend["class"] = MenuRaKhoi.nameEvent;
            datasend["method"] = "GetDataRaKhoi";
            NetworkManager.ins.SendServer(datasend.ToString(), GetEventok);
        }

        void GetEventok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                // AllMenu.ins.OpenMenuTrenCung("MenuEvent7VienNgocRong2023");
                //    GameObject gdevent = AllMenu.ins.GetCreateMenu("MenuEvent7VienNgocRong2023",AllMenu.ins.gameObject,false);
                if (Friend.ins.QuaNha)
                {
                    Friend.ins.GoHome();
                }
                GameObject gdevent = AllMenu.ins.GetCreateMenu("MenuRaKhoi", AllMenu.ins.gameObject);
                                                                                                                
             //    GameObject gdevent = AllMenu.ins.transform.Find("MenuRaKhoi").gameObject;

                gdevent.SetActive(true);
                gdevent.GetComponent<MenuRaKhoi>().ParseData(json);
                DonDepDao();

                menulogin.SetActive(false);

                //foreach (string key in json["dataEvent"]["allItem"].AsObject.Keys)
                //{
                //    DataManager.DataGame[key] = result[key];

                //    //   debug.Log("key" + key);
                //}

            }
            else
            {
                OnThongBaoNhanh(json["message"].AsString);
                menulogin.SetActive(false);
            }
        }

    }

    public void XemThanLong()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject.gameObject;
        AllMenu.ins.OpenMenu("VongTronThanLong");
        AllMenu.ins.menu["VongTronThanLong"].transform.GetChild(0).gameObject.transform.position = btn.transform.position;
        Button btnn = AllMenu.ins.menu["VongTronThanLong"].transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
        btnn.interactable = true;
        VongTronCongTrinh vongtron = AllMenu.ins.menu["VongTronThanLong"].GetComponent<VongTronCongTrinh>();
        if (btn.transform.GetChild(0).gameObject.activeSelf)
        {
            btnn.onClick.RemoveAllListeners();
            btnn.onClick.AddListener(delegate { vongtron.BuSua(btn.name); });
        }
        else btnn.interactable = false;
        Button btnnangcap = AllMenu.ins.menu["VongTronThanLong"].transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        btnnangcap.onClick.RemoveAllListeners();
        btnnangcap.onClick.AddListener(delegate { vongtron.XemNangCapThanLong(btn.name); });
        
        AllMenu.ins.menu["VongTronThanLong"].SetActive(true);
    }
    public void NangCapThanLong()
    {
        Button btnNangCap = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "NangCapThanLong";
        datasend["data"]["name"] = btnNangCap.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    GameObject BeThanLong = AllDao.transform.GetChild(2).transform.Find("Be" + btnNangCap.name).gameObject;
                    AllMenu.ins.DestroyMenu("MenuNangCapItem");
                    panelLoadDao.SetActive(false);
                    GameObject hieuung = Instantiate(Inventory.ins.GetObj("HieuUng1"), new Vector3(BeThanLong.transform.position.x, BeThanLong.transform.position.y + 1f), Quaternion.identity) as GameObject;
                    Vector3 Scale; Scale = hieuung.transform.localScale;
                    Scale.x = 1; Scale.y = 1.1f; hieuung.transform.localScale = Scale;

                    hieuung.SetActive(true);

                    yield return new WaitForSeconds(1.5f);
                    Destroy(hieuung);
                }
            

                //Animator anim = BeThanLong.transform.GetChild(0).GetComponent<Animator>();
                //  anim.SetBool("Doi", false);
                //BeThanLong.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                OnThongBaoNhanh(json["status"].Value, 2);
                panelLoadDao.SetActive(false);
            }
        }
    }
    public void XemthongtinThanLong(bool ok)
    {
        if(ok)
        {
            Button btnNangCap = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            JSONClass datasend = new JSONClass();
            datasend["class"] = "Main";
            datasend["method"] = "XemChiSoThanLong";
            datasend["data"]["name"] = btnNangCap.name;
            NetworkManager.ins.SendServer(datasend.ToString(), Ok,true);
            void Ok(JSONNode json)
            {
                OnThongBaoNhanh(json["thongtin"].Value, 3f);
            }
           
        }    
        else
        {
            //if (AllMenu.ins.menu.ContainsKey("infoitem")) AllMenu.ins.menu["infoitem"].SetActive(false);
        }
    }
    public void btnHoTro()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Xác nhận chuyển đến Fanpage Đảo Rồng Mobile";
        tbc.btnChon.onClick.AddListener(LinkFanpage);
    }
    public void MenuDoiRongVangBac(string namerong, GameObject g)
    {
        Button btnTrieuHoi = g.transform.GetChild(0).transform.GetChild(3).GetComponent<Button>();
        GameObject AllmanhRong = g.transform.GetChild(0).transform.GetChild(2).gameObject;
        if (namerong == "vang")
        {
            string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
            int soluongmanhco = 0;
            for (int i = 0; i < AllmanhRong.transform.childCount; i++)
            {
                Image img = AllmanhRong.transform.GetChild(i).GetComponent<Image>();
                img.sprite = Inventory.LoadSprite(allnamemanh[i]);
                if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
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
                if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
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
        g.gameObject.SetActive(true);
    }
    //public void OutGame()
    //{
    //    Application.Quit();
    //}    
    public void LinkFanpage()
    {
        panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(ServerName + "GetLinkFanpage/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                Application.OpenURL(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                panelLoadDao.SetActive(false);
            }
        }

    }
    public void OpenMenuNangSaoRongVangBac()
    {
        AllMenu.ins.OpenMenu("menuNangSaoRong");
    }

    public void XemQuaOnline()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "XemQuaOnline";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                AllMenu.ins.OpenCreateMenu("MenuNhanQuaOnline");
                QuaOnline qua = AllMenu.ins.menu["MenuNhanQuaOnline"].GetComponent<QuaOnline>();
                qua.txtsoluong.text = "";
                for (int i = 0; i < 12; i++)
                {
                    qua.txtsoluong.text = qua.txtsoluong.text + "\n" + json[i]["soluong"].Value;
                    Image imgitem = qua.imgQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                    imgitem.sprite = Inventory.LoadSprite(json[i]["name"].Value);
                    if (json[i]["trangthai"].Value == "duocnhan")
                    {
                        qua.imgQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if (json[i]["trangthai"].Value == "chuaduocnhan")
                    {
                        qua.imgQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                    }
                    else if (json[i]["trangthai"].Value == "danhan")
                    {
                        qua.imgQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                        qua.imgQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                    }
                    //  imgitem.SetNativeSize();
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
    }
    public void ChiSoRong(string id)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TayTuy";
        datasend["method"] = "XemHoaBuiRong";
        datasend["data"]["id"] = id;
        datasend["data"]["dao"] = "true";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject trncung = GameObject.FindGameObjectWithTag("trencung");
                GameObject menucs = AllMenu.ins.GetCreateMenu("MenuChiSoRong", trncung, true, trncung.transform.childCount - 1);
                menucs.transform.GetChild(1).GetComponent<Text>().text = json["rong"]["namerong"].Value + "(" + json["rong"]["sao"] + "sao)";
                GameObject thuoctinh = menucs.transform.GetChild(2).gameObject;
                for (int j = 0; j < thuoctinh.transform.childCount; j++) thuoctinh.transform.GetChild(j).gameObject.SetActive(false);
                for (int i = 0; i < json["chisogoc"].Count; i++)
                {
                    //debug.Log(json["chisogoc"][i]["name"].Value);
                    for (int j = 0; j < thuoctinh.transform.childCount; j++)
                    {
                        if (json["chisogoc"][i]["name"].Value == thuoctinh.transform.GetChild(j).name)
                        {
                            thuoctinh.transform.GetChild(j).gameObject.SetActive(true);
                            thuoctinh.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = json["chisogoc"][i]["sao"].Value;
                            thuoctinh.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = json["chisogoc"][i]["cong"].Value;
                            break;
                        }
                    }
                }
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }

    public void OpenMenuLoad(string namemenu)
    {
        //debug.Log(namemenu);
        //StartCoroutine(load());
        //menulogin.SetActive(true);
        //IEnumerator load()
        //{
        //    Application.backgroundLoadingPriority = ThreadPriority.High;
        //    // Application.backgroundLoadingPriority = ThreadPriority.Normal;
        //    //SGAsyncOperation operation = SGSceneManager.LoadSceneAsync("SampleScenetest");
        //    AsyncOperation operation = Resources.LoadAsync("GameData/Menu/" + namemenu);

        //    Image maskload = menulogin.transform.GetChild(0).GetComponent<Image>();
        //    Text txtload = menulogin.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        //    while (!operation.isDone)
        //    {
        //        maskload.fillAmount = (float)operation.progress * 100f / (float)100;
        //        txtLoad.text = Mathf.Floor(operation.progress * 100f) + "%";
        //        debug.Log(operation.progress * 100f);
        //        yield return null;
        //    }
        //    //UpdateProgressUI();
        //    maskload.fillAmount = (float)operation.progress * 100f / (float)100;
        //    txtLoad.text = Mathf.Floor(operation.progress * 100f) + "%";
        //}
        //Resources.LoadAsync("GameData/Menu/" + namemenu);
    }

    public void SettingAmThanh()
    {
        if (Setting.amthanh)
        {
            AudioManager.SoundBg.Stop();
            Setting.amthanh = false;
            txtnhac.text = "Nhạc: Tắt";
            PlayerPrefs.SetString("nhac", "false");
        }
        else
        {
            AudioManager.SoundBg.Play();
            Setting.amthanh = true;
            txtnhac.text = "Nhạc: Bật";
            PlayerPrefs.SetString("nhac", "true");
        }
    }
    public void SettingHieuUngAmThanh()
    {
        if (Setting.hieuungamthanh)
        {
            Setting.hieuungamthanh = false;
            txtamthanh.text = "Âm thanh: Tắt";
            PlayerPrefs.SetString("amthanh", "false");
        }
        else
        {
            Setting.hieuungamthanh = true;
            txtamthanh.text = "Âm thanh: Bật";
            PlayerPrefs.SetString("amthanh", "true");
        }
    }
    public void SettingCauHinh()
    {
        GameObject btnchatluonggame = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Text txt = btnchatluonggame.transform.GetChild(0).GetComponent<Text>();
        switch (Setting.cauhinh)
        {
            case CauHinh.CauHinhCao:
                Setting.cauhinh = CauHinh.CauHinhTrungBinh;
                break;
            case CauHinh.CauHinhTrungBinh:
                Setting.cauhinh = CauHinh.CauHinhThap;
                break;
            case CauHinh.CauHinhThap:
                Setting.cauhinh = CauHinh.CauHinhCao;
                break;
        }
        txt.text = Setting.GetStrCauHinh(Setting.cauhinh);
        PlayerPrefs.SetString("cauhinh", Setting.cauhinh.ToString());
    }
    public void DangXuat()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Bạn muốn đăng xuất ?";
        tbc.btnChon.onClick.AddListener(LoginFacebook.ins.DangXuatFb);
    }
    public GameObject FindRongDao(int dao, string id)
    {
        GameObject returnn = null;
        if (AllDao.transform.Find("BGDao" + dao))
        {
            GameObject daoo = AllDao.transform.Find("BGDao" + dao).gameObject;
            Transform tfRong = FindObject(daoo, "RongDao").transform;
          
            foreach (Transform child in tfRong.transform)
            {
                if (child.name == id)
                {
                    returnn = child.gameObject;
                    break;
                }
            }
        }
     
        return returnn;
    }

    public void OnThongBaoNhanh(string s, float time = 2f,bool disnable = true)
    {
        if (s.Length == 0) return;
        GameObject menu = AllMenu.ins.GetCreateMenu("infoitem", trencung.gameObject);

        // AllMenu.ins.menu["infoitem"].transform.SetSiblingIndex(menulogin.transform.GetSiblingIndex() - 1);
        //Transform find = menu.transform.Find(s.Length.ToString());
        //if (find != null)
        //{
        //    if (find.gameObject.activeSelf)
        //    {
        //        find.transform.GetChild(0).GetComponent<Text>().text = s;
        //        return;
        //    }
        //}
        for (int i = 0; i < menu.transform.childCount; i++)
        {
            if (!menu.transform.GetChild(i).gameObject.activeSelf)
            {
                menuinfoitem ifitem = menu.transform.GetChild(i).GetComponent<menuinfoitem>();

                menu.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = s;
                ifitem.gameObject.SetActive(true);
                if(disnable) ifitem.Disnable(time);
                // else Destroy(ifitem.gameObject, time);
                ifitem.name = s.Length.ToString();
                break;
            }
            else if (i == menu.transform.childCount - 1)
            {
                GameObject instan = Instantiate(menu.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                instan.transform.GetChild(0).GetComponent<Text>().text = s;
                instan.transform.SetParent(menu.transform, false);
                instan.gameObject.SetActive(true);
                menuinfoitem ifitem = instan.GetComponent<menuinfoitem>();
                //instan.name = menu.transform.childCount.ToString();
                if (menu.transform.childCount <= 3 && disnable) ifitem.Disnable(time);
                else
                {
                    Inventory.ins.StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        yield return new WaitForSeconds(time);
                        if (menu.transform.childCount >= 3) Destroy(ifitem.gameObject);
                        else ifitem.gameObject.SetActive(false);

                    }
                }
                ifitem.name = s.Length.ToString();
                break;
            }
        }
        if (menu.transform.childCount >= 6) Destroy(menu.transform.GetChild(1).gameObject);
        menu.transform.SetAsLastSibling();
    }
    public void OffThongBaoNhanh(short id)
    {
        GameObject menu = AllMenu.ins.GetCreateMenu("infoitem", trencung.gameObject);

        for (int i = 0; i < menu.transform.childCount; i++)
        {
            if (menu.transform.GetChild(i).name == id.ToString())
            {
                menu.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        //Transform find = menu.transform.Find(id.ToString());

        //if (find != null)
        //{
        //    find.gameObject.SetActive(false);
        //}
    }
    public GameObject FindObject(GameObject parnet, string namechild)
    {
        GameObject returnn = null;
        foreach (Transform child in parnet.transform)
        {
            if (child.name == namechild)
            {
                returnn = child.gameObject;
                break;
            }
        }
        return returnn;
    }
    public void Sapramat()
    {
        OnThongBaoNhanh("Sắp ra mắt!");
    }
    public void OpenMapPhoBan()
    {
        if (Input.touchCount > 1) return;
        //OnThongBao(true,"Sắp ra mắt",true);
        GameObject camera = gameObject;
        // camera.transform.position = MapPhoBan.transform.position;
        //   MapPhoBan.SetActive(true);

        GameObject Dao = AllDao.transform.Find("BGDao" + DangODao).gameObject;

        Dao.SetActive(false);
        Vector3 vec = Friend.ins.DaoFriend.transform.position;
        camera.transform.position = new Vector3(vec.x,vec.y,vec.z);
        AllMenu.ins.DestroyallMenu();
        GameObject GiaoDienMapVienChinh = AllMenu.ins.GetCreateMenu("menuPhoban",GameObject.Find("CanvasGame"),false);

        GiaoDienMapVienChinh.transform.position = Friend.ins.DaoFriend.transform.position;

        GiaoDienMapVienChinh.SetActive(true);
        GiaoDienMapVienChinh.transform.localScale = new Vector3(0.8f, 0.8f, GiaoDienMapVienChinh.transform.localScale.z);
        
        giaodien.SetActive(false);
    }
    public void DestroyObject(GameObject g)
    {
        Destroy(g);
    }
    public IEnumerator LoadExp(float exp, float maxexp)
    {
        float Exp = exp;
        MaxExp = exp;
        for (int i = 0; i < exp; i++)
        {
            yield return new WaitForSeconds(0.02f);
            Exp += 1;
            float fillamount = (float)Exp / (float)maxexp;
            imgThanhLevel.fillAmount = fillamount;
        }
    }
    public void NangCapDao(string s)
    {
        OnThongBao(true, "Đang yêu cầu đến máy chủ...");
        XacNhanMuaCongTrinh xnmua = new XacNhanMuaCongTrinh("NhaApTrung", leveldao[DangODao], 0, s);
        string data = JsonUtility.ToJson(xnmua);
        NetworkManager.ins.socket.Emit("NangCapDao", new JSONObject(data));
    }
    bool xeminfodao = false;
    public void InfoDao(bool b)
    {
        xeminfodao = b;
        if (b)
        {
            StartCoroutine(delaydao());
            NetworkManager.ins.socket.Emit("XemThongTinDao");
            IEnumerator delaydao()
            {
                yield return new WaitForSeconds(0.3f);
                if (xeminfodao)
                {
                    AllMenu.ins.OpenMenu("MenulInfoDao");
                    //menuInfoDao.SetActive(true);
                }
            }
        }
        else
        {
            if (AllMenu.ins.menu.ContainsKey("MenulInfoDao")) AllMenu.ins.menu["MenulInfoDao"].SetActive(false);

        }
    }
    public void XemNangCapDao()
    {
        if (leveldao[DangODao] < 3)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "Main";
            datasend["method"] = "xemGiaNangCapDao";
            datasend["data"]["name"] = "dao";
            datasend["data"]["cap"] = leveldao[DangODao].ToString();
            datasend["data"]["idcongtrinh"] = DangODao.ToString();
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode jsonn)
            {
                if (jsonn["status"].AsString == "0")
                {
                    JSONNode json = jsonn["data"];
                    UI = AllMenu.ins.GetCreateMenu("MenuNangCapItem", null, true).GetComponent<Ui>();
                    UI.SpriteItem[0].sprite = Inventory.LoadSprite("Dao" + leveldao[DangODao]);
                    UI.SpriteItem[0].SetNativeSize();
                    UI.SpriteItem[1].sprite = Inventory.LoadSprite("Dao" + (leveldao[DangODao] + 1));
                    UI.SpriteItem[1].SetNativeSize();
                    UI.txtlevel[0].text = "Cấp " + leveldao[DangODao];

                    UI.txtlevel[1].text = "Cấp " + (leveldao[DangODao] + 1);

                    UI.txtChiSo[0].text = json["sorongtoida"].Value;
                    UI.txtChiSo[1].text = json["sorongtoidanew"].Value;
                    UI.tilethanhcong.GetComponent<Text>().text = "Tỉ lệ thành công";
                    UI.tilethanhcong.transform.GetChild(0).GetComponent<Text>().text = "Tỉ lệ thành công";
                    UI.tilethanhcong.SetActive(false);
                    UI.nangcapDao.SetActive(true);
                    UI.btnNangCap[0].gameObject.SetActive(false);
                    UI.btnNangCap[1].gameObject.SetActive(false);
                    UI.btnNangCapThucAn[0].gameObject.SetActive(false);
                    UI.btnNangCapThucAn[1].gameObject.SetActive(false);
                    UI.btnNangCapNhaApTrung[0].gameObject.SetActive(false);
                    UI.btnNangCapNhaApTrung[1].gameObject.SetActive(false);

                    UI.btnNangCap[0].interactable = true;
                    UI.btnNangCap[1].interactable = true;
                    UI.txtGiaNangDao[0].text = NetworkManager.ins.CatDauNgoacKep(json["giavang"].ToString());
                    UI.txtGiaNangDao[1].text = NetworkManager.ins.CatDauNgoacKep(json["giakimcuong"].ToString());
                    UI.txtTileThanhCong[0].text = NetworkManager.ins.CatDauNgoacKep(json["huanchuongnew"].ToString());
                    UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                    GameObject objThanLong = UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;
                    objThanLong.SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                    // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                }
            }
        }
    }
    public void InfoCongTrinh(string name, int cap) // xem dấu chấm hỏi công trình
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "infocongtrinh";
        datasend["data"]["name"] = name;
        datasend["data"]["cap"] = cap.ToString();
        datasend["data"]["id"] = "0";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                GameObject menuinfoCongtrinh = AllMenu.ins.GetCreateMenu("MenuInfoCongtrinh");
                Image imagecongtrinh = menuinfoCongtrinh.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
                imagecongtrinh.sprite = VungCongTrinh.GetComponent<SpriteRenderer>().sprite; imagecongtrinh.SetNativeSize();
                Text txtname = menuinfoCongtrinh.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
                Text TextThongTin = menuinfoCongtrinh.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
                txtname.text = NetworkManager.ins.CatDauNgoacKep(json["ten"].ToString());
                TextThongTin.text = NetworkManager.ins.CatDauNgoacKep(json["infosanxuat"].ToString());
                //AllMenu.ins.menu["VongTronCongtrinh"].SetActive(false);
                AllMenu.ins.DestroyMenu("VongTronCongtrinh");
                menuinfoCongtrinh.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
    }
    public void OpenmenuUpcongtrinh()// ô vòng tròn 
    {
        //VongtronCongtrinh.SetActive(false);
        AllMenu.ins.DestroyMenu("VongTronCongtrinh");
        CongTrinh congtrinh = VungCongTrinh.GetComponent<CongTrinh>();
        XemNangCapCongTrinh(congtrinh.nameCongtrinh, congtrinh.levelCongtrinh);
    }
    public void XeminfoCongtrinh()// dấu chấm hỏi
    {
        CongTrinh congtrinh = VungCongTrinh.GetComponent<CongTrinh>();
        InfoCongTrinh(congtrinh.nameCongtrinh, congtrinh.levelCongtrinh);
    }
    byte choncongtrinh;
    public void ChonCongTrinhNang(string nameCt)
    {
        CongTrinh congTrinh = VungCongTrinh.GetComponent<CongTrinh>();
        debug.Log(nameCt + " " + congTrinh.levelCongtrinh);
        Xemct(nameCt, congTrinh.levelCongtrinh, congTrinh.idCongtrinh, false);
        congTrinh.nameCongtrinh = nameCt;
        //  SpriteRong spritect = GameObject.Find("SpriteCongTrinh" + nameCt).GetComponent<SpriteRong>();
        // imgMuaCongTrinh.sprite = spritect.spriteTienHoa[0];
        // imgMuaCongTrinh.SetNativeSize();
        AllMenu.ins.OpenMenu("menuMuaCongtrinh");
        AllMenu.ins.menu["menuMuaCongtrinh"].transform.SetAsLastSibling();
        //menuMuaCongtrinh.SetActive(true);
    }
    public void XemNangCapCongTrinh(string name, int cap,string nameThanLong = "")
    {
        if(nameThanLong == "")
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "Main";
            datasend["method"] = "xemnangcapcongtrinh";
            datasend["data"]["name"] = name;
            datasend["data"]["cap"] = cap.ToString();
            datasend["data"]["id"] = "0";
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode jsonn)
            {
                if (jsonn["status"].AsString == "0")
                {
                    JSONNode json = jsonn["data"];
                    UI = AllMenu.ins.GetCreateMenu("MenuNangCapItem", null, true).GetComponent<Ui>();
                    CongTrinh congtrinh = VungCongTrinh.GetComponent<CongTrinh>();
                    //   SpriteRong spritect = GameObject.Find("SpriteCongTrinh" + congtrinh.nameCongtrinh).GetComponent<SpriteRong>();
                    StartCoroutine(LoadSpriteWeb(UI.SpriteItem[0], congtrinh.nameCongtrinh, congtrinh.levelCongtrinh));// spritect.spriteTienHoa[congtrinh.levelCongtrinh];
                                                                                                                       //  UI.SpriteItem[0].SetNativeSize();
                    StartCoroutine(LoadSpriteWeb(UI.SpriteItem[1], congtrinh.nameCongtrinh, congtrinh.levelCongtrinh + 1));
                    // UI.SpriteItem[1].sprite = spritect.spriteTienHoa[congtrinh.levelCongtrinh + 1]; UI.SpriteItem[1].SetNativeSize();
                    UI.txtlevel[0].text = NetworkManager.ins.CatDauNgoacKep(json["levelhientai"].ToString());

                    UI.txtlevel[1].text = NetworkManager.ins.CatDauNgoacKep(json["leveltieptheo"].ToString());
                    string[] chiso1 = NetworkManager.ins.CatDauNgoacKep(json["chiso1"].ToString()).Split('|');
                    string[] chiso2 = NetworkManager.ins.CatDauNgoacKep(json["chiso2"].ToString()).Split('|');
                    UI.txtChiSo[0].text = chiso1[0] + "\n" + chiso1[1];
                    UI.txtChiSo[1].text = chiso2[0] + "\n" + chiso2[1];
                    UI.tilethanhcong.SetActive(true);
                    UI.nangcapDao.SetActive(false);
                    UI.txtTileThanhCong[0].text = NetworkManager.ins.CatDauNgoacKep(json["tilethanhcongvang"].ToString() + "%");
                    UI.txtTileThanhCong[1].text = NetworkManager.ins.CatDauNgoacKep(json["tilethanhcongkimcuong"].ToString() + "%");
                    UI.tilethanhcong.GetComponent<Text>().text = "Tỉ lệ thành công";
                    UI.tilethanhcong.transform.GetChild(0).GetComponent<Text>().text = "Tỉ lệ thành công";
                    UI.btnNangCap[0].gameObject.SetActive(true);
                    UI.btnNangCap[1].gameObject.SetActive(true);
                    UI.btnNangCapNhaApTrung[0].gameObject.SetActive(false);
                    UI.btnNangCapNhaApTrung[1].gameObject.SetActive(false);
                    UI.btnNangCapThucAn[0].gameObject.SetActive(false);
                    UI.btnNangCapThucAn[1].gameObject.SetActive(false);
                    UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                    UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                    GameObject objThanLong = UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;

                    objThanLong.SetActive(false);
                    if (int.Parse(txtLevel.text) >= int.Parse(NetworkManager.ins.CatDauNgoacKep(json["levelnang"].ToString())))
                    {
                        UI.btnNangCap[0].interactable = true;
                        UI.btnNangCap[1].interactable = true;
                        UI.txtGia[0].text = NetworkManager.ins.CatDauNgoacKep(json["giavang"].ToString());
                        UI.txtGia[1].text = NetworkManager.ins.CatDauNgoacKep(json["giakimcuong"].ToString());
                    }
                    else
                    {
                        UI.btnNangCap[0].interactable = false;
                        UI.btnNangCap[1].interactable = false;
                        UI.txtGia[0].text = "<color=#ff0000ff>" + "Cấp " + NetworkManager.ins.CatDauNgoacKep(json["levelnang"].ToString()) + "</color>";
                        UI.txtGia[1].text = "<color=#ff0000ff>" + "Cấp " + NetworkManager.ins.CatDauNgoacKep(json["levelnang"].ToString()) + "</color>";
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                    // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                }
            }
        }    
        else
        {
           
            JSONClass datasend = new JSONClass();
            datasend["class"] = "Main";
            datasend["method"] = "XemNangCapThanLong";
            datasend["data"]["name"] = nameThanLong;
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode json)
            {
                UI = AllMenu.ins.GetCreateMenu("MenuNangCapItem", null, true).GetComponent<Ui>();
                //   SpriteRong spritect = GameObject.Find("SpriteCongTrinh" + congtrinh.nameCongtrinh).GetComponent<SpriteRong>();
                StartCoroutine(LoadSpriteWeb(UI.SpriteItem[0], nameThanLong, 0));// spritect.spriteTienHoa[congtrinh.levelCongtrinh];
                                                                                 //  UI.SpriteItem[0].SetNativeSize();
                StartCoroutine(LoadSpriteWeb(UI.SpriteItem[1], nameThanLong, 0));
                //  UI.SpriteItem[1].sprite = spritect.spriteTienHoa[congtrinh.levelCongtrinh + 1]; UI.SpriteItem[1].SetNativeSize();
                UI.txtlevel[0].text = NetworkManager.ins.CatDauNgoacKep(json["levelhientai"].ToString());

                UI.txtlevel[1].text = NetworkManager.ins.CatDauNgoacKep(json["leveltieptheo"].ToString());
                UI.txtChiSo[0].text = json["chiso1"].Value;
                UI.txtChiSo[1].text = json["chiso2"].Value;
                UI.tilethanhcong.SetActive(false);
                UI.nangcapDao.SetActive(false);

                UI.btnNangCap[0].gameObject.SetActive(false);
                UI.btnNangCap[1].gameObject.SetActive(false);
                UI.btnNangCapNhaApTrung[0].gameObject.SetActive(false);
                UI.btnNangCapNhaApTrung[1].gameObject.SetActive(false);
                UI.btnNangCapThucAn[0].gameObject.SetActive(false);
                UI.btnNangCapThucAn[1].gameObject.SetActive(false);

                UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
                GameObject objThanLong = UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;
                objThanLong.SetActive(true);
                objThanLong.transform.GetChild(0).gameObject.name = nameThanLong;
                objThanLong.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = json["giakimcuong"].Value;
                objThanLong.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = json["tilethanhcong"].Value + "%";
                objThanLong.transform.GetChild(2).GetComponent<Text>().text = json["soitemyeucau"].Value;
                Image imgItemYeuCau = objThanLong.transform.GetChild(4).GetComponent<Image>();
                imgItemYeuCau.sprite = Inventory.LoadSprite(json["itemyeucau"].Value);
                imgItemYeuCau.name = json["itemyeucau"].AsString;
                if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + json["itemyeucau"].Value))
                {
                    int soyeucau = int.Parse(json["soitemyeucau"].Value);
                    int soitemco = int.Parse(NetworkManager.ins.inventory.ListItemThuong["item" + json["itemyeucau"].Value].transform.GetChild(0).GetComponent<Text>().text);
                    objThanLong.transform.GetChild(3).GetComponent<Text>().text = "(Đang có: " + NetworkManager.ins.inventory.ListItemThuong["item" + json["itemyeucau"].Value].transform.GetChild(0).GetComponent<Text>().text + ")";
                    if (soitemco >= soyeucau)
                    {
                        objThanLong.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    }
                    else objThanLong.transform.GetChild(0).GetComponent<Button>().interactable = false;
                }
                else
                {
                    objThanLong.transform.GetChild(3).GetComponent<Text>().text = "(Đang có: 0)";
                    objThanLong.transform.GetChild(0).GetComponent<Button>().interactable = false;
                }
                AllMenu.ins.DestroyMenu("VongTronThanLong");
            }
        }
    }
    public int GetAnimationCongTrinh(int levelCongtrinh)
    {
        int soanim = 1;
        if (levelCongtrinh >= 4 && levelCongtrinh <= 6)
        {
            soanim = 2;
        }
        if (levelCongtrinh >= 7 && levelCongtrinh <= 10)
        {
            soanim = 3;
        }
        if (levelCongtrinh >= 11 && levelCongtrinh <= 13)
        {
            soanim = 4;
        }
        if (levelCongtrinh >= 14 && levelCongtrinh <= 16)
        {
            soanim = 5;
        }
        if (levelCongtrinh >= 17 && levelCongtrinh <= 20)
        {
            soanim = 6;
        }
        if (levelCongtrinh >= 21 && levelCongtrinh <= 23)
        {
            soanim = 7;
        }
        if (levelCongtrinh >= 24 && levelCongtrinh <= 26)
        {
            soanim = 8;
        }
        if (levelCongtrinh >= 27 && levelCongtrinh <= 30)
        {
            soanim = 9;
        }
        return soanim;
    }
    public void InfoCongTrinh(string NameCongTrinh, string sxvang, string capmua, string giavang, bool xemanh = true)
    {
        CongTrinh ct = VungCongTrinh.GetComponent<CongTrinh>();
        //   SpriteRong spritect = GameObject.Find("SpriteCongTrinh" + ct.nameCongtrinh).GetComponent<SpriteRong>();
        nameCongtrinh = ct.nameCongtrinh;
        Image imgMuaCongTrinh = AllMenu.ins.menu["menuMuaCongtrinh"].transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        Text txtTenCongTrinh = AllMenu.ins.menu["menuMuaCongtrinh"].transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();

        txtTenCongTrinh.text = NameCongTrinh + " Level:" + (ct.levelCongtrinh + 1);
        if (xemanh)
        {
            if (GetAnimationCongTrinh(ct.levelCongtrinh - 1) < 29)
            {
                //imgMuaCongTrinh.sprite = spritect.spriteTienHoa[GetAnimationCongTrinh(ct.levelCongtrinh - 1)];
                StartCoroutine(LoadSpriteWeb(imgMuaCongTrinh, ct.nameCongtrinh, GetAnimationCongTrinh(ct.levelCongtrinh - 1)));
            }
            else
            {
                // imgMuaCongTrinh.sprite = spritect.spriteTienHoa[spritect.spriteTienHoa.Length - 1];
                StartCoroutine(LoadSpriteWeb(imgMuaCongTrinh, ct.nameCongtrinh, 29));
            }
        }
        else
        {
            // imgMuaCongTrinh.sprite = spritect.spriteTienHoa[0];
            StartCoroutine(LoadSpriteWeb(imgMuaCongTrinh, ct.nameCongtrinh, 0));
        }
        AllMenu.ins.menu["menuMuaCongtrinh"].SetActive(true);
        imgMuaCongTrinh.SetNativeSize();
        txtTenCongTrinh.text += " Level: " + ct.levelCongtrinh;
        AllMenu.ins.menu["menuMuaCongtrinh"].transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = sxvang;
        AllMenu.ins.menu["menuMuaCongtrinh"].transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>().text = capmua;
        Button btnmuacongtrinh = AllMenu.ins.menu["menuMuaCongtrinh"].transform.GetChild(0).transform.GetChild(2).transform.GetChild(4).GetComponent<Button>();
        btnmuacongtrinh.transform.GetChild(1).GetComponent<Text>().text = giavang;

        btnmuacongtrinh.onClick.RemoveAllListeners();
        btnmuacongtrinh.onClick.AddListener(() => XnMuaCongTrinh("vang"));
    }
    IEnumerator LoadSpriteWeb(Image img, string name, int cap)
    {
        WWW www = new WWW(ServerName + "image/name/" + name + "/cap/" + cap);
        yield return www;
        debug.Log("load xong");
        img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        img.SetNativeSize();
    }
    public void XnMuaCongTrinh(string s)
    {
      //  debug.LogError("mua ct");
        CongTrinh ct = VungCongTrinh.GetComponent<CongTrinh>();
        if (VungCongTrinh.transform.parent.transform.parent.transform.GetSiblingIndex() == DangODao)
        {
            nameCongtrinh = ct.nameCongtrinh;
            OnThongBao(true, "Đang yêu cầu đến máy chủ...");
            XacNhanMuaCongTrinh xnmua = new XacNhanMuaCongTrinh(ct.nameCongtrinh, ct.levelCongtrinh, ct.idCongtrinh, s);
            string data = JsonUtility.ToJson(xnmua);
            //debug.Log(data);
            NetworkManager.ins.socket.Emit("nangcapcongtrinh", new JSONObject(data));
        }
        else
        {
            AllMenu.ins.DestroyMenu("MenuNangCapItem");
        }
    }
    public void Xemct(string name, int cap, int id, bool b)
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "xemcongtrinh";
        datasend["data"]["name"] = name;
        datasend["data"]["cap"] = cap.ToString();
        datasend["data"]["id"] = id.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                string[] status = json.AsString.Split(',');
                InfoCongTrinh(status[3], status[0], status[1], status[2], b);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
    }
    public void QuaDao(int i)
    {
        //GameObject daoClone = Instantiate(AllDao.transform.GetChild(i).gameObject,transform.position,Quaternion.identity);
        //daoClone.transform.SetParent(AllDao.transform,false);
        //daoClone.transform.position = AllDao.transform.GetChild(0).transform.position;


        if (Friend.ins.QuaNha == false)
        {
            if (thuyen.ThaThucAn)
            {
                int a = DangODao;
                int tru = a + i;
                if (tru < sodao && tru > -1)//(GameObject.Find("BGDao" + (DangODao + i)))
                {
                    panelLoadDao.SetActive(true);
                    //  if (Random.Range(0, 50) == 25) ads.RequestInterstitial();
                    GameObject Dao = null;
                    string codao = "khongcodao";
                    if(AllDao.transform.Find("BGDao" + (DangODao + i)))
                    {
                        Dao = AllDao.transform.Find("BGDao" + (DangODao + i)).gameObject;
                        codao = "codao";
                    }    
                    else
                    {
                        Dao = Instantiate(Inventory.LoadObjectResource("GameData/Dao/BGDao" + (DangODao + i)), transform.position, Quaternion.identity);
                        Dao.transform.SetParent(AllDao.transform, false);
                    }
                    Dao.name = "BGDao" + (DangODao + i);
                
                    Dao.transform.position = AllDao.transform.GetChild(0).transform.position;
               
                    Dao.SetActive(true);
                    GameObject DaoTruoc = AllDao.transform.Find("BGDao" + DangODao).gameObject;
                    DaoTruoc.SetActive(false);
                    DangODao += (byte)i;
                    if(DangODao <= AllDao.transform.childCount) Dao.transform.SetSiblingIndex(DangODao);
                    else Dao.transform.SetSiblingIndex(AllDao.transform.childCount);
                    NetworkManager.ins.socket.Emit("QuaDao", JSONObject.CreateStringObject(DangODao.ToString() + "+" + codao));
                    transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
                    ngaydem.transform.position = Dao.transform.position;
                    ngaydem.transform.SetParent(Dao.transform);
                    ngaydem.transform.SetSiblingIndex(4);
                    Bg.transform.position = Dao.transform.position;
                    if (thuyen.ThaThucAn)
                    {
                        thuyen.ThuyenObject.transform.SetParent(Dao.transform);
                    }
                }
            }
        }
        else
        {
            int a = Friend.ins.DangODaoFriend;
            int tru = a + i;
            if (tru < Friend.ins.DataDao.Count && tru > -1)
            {
                // GameObject Dao = friend.DaoFriend.transform.GetChild(friend.DangODaoFriend + i).gameObject;
                //   Dao.SetActive(true);
                //   friend.DaoFriend.transform.GetChild(friend.DangODaoFriend).gameObject.SetActive(false);
                GameObject DaoTruoc = Friend.ins.DaoFriend.transform.Find("DaoFriend" + Friend.ins.DangODaoFriend).gameObject;
                Friend.ins.DangODaoFriend += (byte)i;
                DaoTruoc.SetActive(false);
                Friend.ins.QuaDaoFriend(Friend.ins.DangODaoFriend);
                //transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
                //ngaydem.transform.position = Dao.transform.position;
                //ngaydem.transform.SetParent(Dao.transform);
                //ngaydem.transform.SetSiblingIndex(4);
                //Bg.transform.position = Dao.transform.position;
            }
        }

        //if (friend.QuaNha == false)
        //{
        //    if (thuyen.ThaThucAn)
        //    {
        //        int a = DangODao;
        //        int tru = a + i;
        //        if (tru < AllDao.transform.childCount && tru > -1)//(GameObject.Find("BGDao" + (DangODao + i)))
        //        {
        //            panelLoadDao.SetActive(true);
        //            //  if (Random.Range(0, 50) == 25) ads.RequestInterstitial();
        //            GameObject Dao = AllDao.transform.GetChild(DangODao + i).gameObject;//GameObject.Find("BGDao" + (DangODao + i));
        //            Dao.SetActive(true);
        //            GameObject DaoTruoc = AllDao.transform.GetChild(DangODao).gameObject;
        //            DaoTruoc.SetActive(false);
        //            DangODao += (byte)i;
        //            net.socket.Emit("QuaDao", JSONObject.CreateStringObject(DangODao.ToString()));
        //            transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
        //            ngaydem.transform.position = Dao.transform.position;
        //            ngaydem.transform.SetParent(Dao.transform);
        //            ngaydem.transform.SetSiblingIndex(4);
        //            Bg.transform.position = Dao.transform.position;
        //            if (thuyen.ThaThucAn)
        //            {
        //                thuyen.ThuyenObject.transform.SetParent(Dao.transform);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    int a = friend.DangODaoFriend;
        //    int tru = a + i;
        //    if (tru < friend.DaoFriend.transform.childCount && tru > -1)
        //    {
        //        GameObject Dao = friend.DaoFriend.transform.GetChild(friend.DangODaoFriend + i).gameObject;
        //        Dao.SetActive(true);
        //        friend.DaoFriend.transform.GetChild(friend.DangODaoFriend).gameObject.SetActive(false);
        //        friend.DangODaoFriend += (byte)i;
        //        transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
        //        ngaydem.transform.position = Dao.transform.position;
        //        ngaydem.transform.SetParent(Dao.transform);
        //        ngaydem.transform.SetSiblingIndex(4);
        //        Bg.transform.position = Dao.transform.position;
        //    }
        //}
    }
    public void DropThucAn()
    {
        ThucAn thucAn = thuyen.ThucAnTha.GetComponent<ThucAn>();
        thucAn.Drop();
    }
    public void LoadExpRong(decimal exp, decimal maxexp)
    {
        float fillamount = (float)exp / (float)maxexp;
        AllMenu.ins.menu["PanelInfoRong"].transform.GetChild(5).transform.GetChild(0).GetComponent<Image>().fillAmount = fillamount;
   //     imgexpRong.fillAmount = fillamount;
    }
    public void OnThongBao(bool a, string tb = "", bool btok = false)
    {
        menuload.SetActive(a);
        txtLoad.text = tb;
        menuload.GetComponent<Button>().enabled = btok;
     //   btnOk.SetActive(btok);
    }
    //void LoadTimeDragon()
    //{
    //    if (timeDoi > 0)
    //    {
    //        timeDoi -= Time.deltaTime;
    //        float fillamount = (float)timeDoi / (float)maxTimeDoi;
    //        imgTimeDragon.fillAmount = fillamount;
    //    }
    //}
    public void OpenMenuTuiDo()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        if(btnchon.transform.parent.transform.parent.name == "Giaodien")
        {
            AllMenu.ins.transform.Find("menuTuiDo").gameObject.SetActive(true);
        }
        else
        {
            Destroy(btnchon.transform.parent.gameObject);
        }
    
    }    
    public void OpenMenuDau()
    {
        if (AllMenu.ins.transform.Find("PanelLoiDai").gameObject.activeSelf == false &&
            AllMenu.ins.menu.ContainsKey("MenuDauTruongOnIine") == false &&
            AllMenu.ins.menu.ContainsKey("MenuDauTruongThuThach") == false &&
            AllMenu.ins.menu.ContainsKey("GiaoDienLanSu") == false && NetworkManager.isSend)
        {
            AllMenu.ins.transform.Find("menuDau").gameObject.SetActive(true);
        }
    }
    public void OpenGiaoDienLanSu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LanSu";
        datasend["method"] = "GetData";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = AllMenu.ins.GetCreateMenu("GiaoDienLanSu", trencung.gameObject, true);
                // GameObject menu = trencung.transform.Find("GiaoDienLanSu").gameObject;
                menu.GetComponent<GiaoDienLanSu>().ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenGiaoDienLongAp()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LongAp";
        datasend["method"] = "GetData";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
               GameObject menu = AllMenu.ins.GetCreateMenu("MenuLongAp");
              //   GameObject menu = AllMenu.ins.transform.Find("MenuLongAp").gameObject;
                menu.GetComponent<MenuApRong>().ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenGiaoDienHapThuNgoc()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "HapThuNgoc";
        datasend["method"] = "GetData";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = AllMenu.ins.GetCreateMenu("MenuHapThuNgoc");
               //    GameObject menu = AllMenu.ins.transform.Find("MenuHapThuNgoc").gameObject;
                menu.GetComponent<MenuHapThuNgoc>().ParseData(json);
            }
            else
            {
                OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public static string ParseTime(float secc)
    {
        int sec = Mathf.FloorToInt(secc);
        int minutes = 0;
        int gio = 0;

        while (sec >= 60)
        {
            sec -= 60;
            minutes += 1;
        }
        while (minutes >= 60)
        {
            minutes -= 60;
            gio += 1;
        }
        if (gio > 0)
        {
           return gio.ToString("D2") + ":" + minutes.ToString("D2") + ":" + sec.ToString("D2");
        }
        else return minutes.ToString("D2") + ":" + sec.ToString("D2");
    }
    public void VaoBossTheGioi()
    {
        if (Input.touchCount > 1) return;
        //Button btnnhan = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        //int sibling = btnnhan.transform.parent.GetSiblingIndex();
        GameObject Dao = AllDao.transform.Find("BGDao" + DangODao).gameObject;
        if (Friend.ins.QuaNha)
        {
            Dao = Friend.ins.DaoFriend.transform.Find("DaoFriend" + Friend.ins.DangODaoFriend).gameObject;
        }
        else
        {
            Dao = AllDao.transform.Find("BGDao" + DangODao).gameObject;
        }

        JSONClass datasend = new JSONClass();
        datasend["class"] = "BossTheGioi";
        datasend["method"] = "XemBoss";
        NetworkManager.ins.SendServer(datasend.ToString(), ok);

        void ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    if (Friend.ins.QuaNha) Friend.ins.GoHome(true);
                    panelLoadDao.SetActive(false);
                    menulogin.SetActive(true);
                    NetworkManager.ins.vienchinh.chedodau = CheDoDau.BossTG;
                    yield return new WaitForSeconds(0.3f);
                    loadbosstg(json);
                }
              
            }
            else
            {
                OnThongBaoNhanh(json["status"].Value);
                Dao.SetActive(true);
                Vector3 vec = Dao.transform.position;
                vec.z = -10;
                gameObject.transform.position = vec;
                giaodien.SetActive(true);
                AudioManager.SetSoundBg("nhacnen0");
                NetworkManager.ins.vienchinh.TruDo.SetActive(true);
                NetworkManager.ins.vienchinh.TruXanh.SetActive(true);
                //  net.socket.Off("usersOut", UsersOut);;
                AllMenu.ins.DestroyMenu("MenuBossTG");
                menulogin.SetActive(false);
            }

            panelLoadDao.SetActive(false);
        }
    }
    public void LoadKQBoss()
    {

    }
    public void loadbosstg(JSONNode json)
    {
        menulogin.SetActive(true);

        for (int i = 1; i < VienChinh.vienchinh.TeamXanh.transform.childCount; i++)
        {
            Destroy(VienChinh.vienchinh.TeamXanh.transform.GetChild(i).gameObject);
        }
        for (int i = 1; i < VienChinh.vienchinh.TeamDo.transform.childCount; i++)
        {
            Destroy(VienChinh.vienchinh.TeamDo.transform.GetChild(i).gameObject);
        }
        GameObject menuboss = AllMenu.ins.GetCreateMenu("MenuBossTG");
        BossTG boss = menuboss.GetComponent<BossTG>();
        boss.GetTop();
        //float fillamount = (float)float.Parse(json["hp"].Value) / (float)float.Parse(json["maxhp"].Value);
        //boss.imgHpBoss.fillAmount = fillamount;
        if(json["nameboss"].Value != "TinhTinhLuaDo")
        {
            //menuboss.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(json["nameboss"].Value);]
            for (int i = 0; i < menuboss.transform.GetChild(2).transform.childCount; i++)
            {
                if(menuboss.transform.GetChild(2).transform.GetChild(i).gameObject.name == json["nameboss"].Value)
                {
                    menuboss.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                    menuboss.transform.GetChild(2).transform.GetChild(i).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(json["nameboss"].Value);
                    menuboss.transform.GetChild(2).transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            menuboss.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
        }
        boss.Updatehp(float.Parse(json["hp"].Value), float.Parse(json["maxhp"].Value));

        GameObject nguoichoi = menuboss.transform.GetChild(0).gameObject;
      //  GameObject contentChat = menuboss.GetComponent<BossTG>().menuChat.transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 1; i < nguoichoi.transform.childCount; i++)
        {
            Destroy(nguoichoi.transform.GetChild(i).gameObject);
        }
        //for (int i = 1; i < contentChat.transform.childCount; i++)
        //{
        //    Destroy(contentChat.transform.GetChild(i).gameObject);
        //}

        Camera.main.orthographicSize = 5;
        giaodien.SetActive(false);
        GameObject camera = gameObject;
        GameObject MapVienChinh = NetworkManager.ins.vienchinh.transform.GetChild(0).gameObject;
        camera.GetComponent<ZoomCamera>().enabled = false;
        GameObject Dao = AllDao.transform.Find("BGDao" + DangODao).gameObject;
        Dao.SetActive(false);

        // crgame.enabled = false;
        MapVienChinh.SetActive(true);
        camera.transform.position = new Vector3(MapVienChinh.transform.position.x, MapVienChinh.transform.position.y, -10);
        NetworkManager.ins.vienchinh.TruDo.SetActive(false);
        NetworkManager.ins.vienchinh.TruXanh.SetActive(false);
        // string[] arrayrong = new string[] { "RongXuong", "RongMaTroi", "Rong2DauBangDat", "RongLuaDat", "RongLuaMatXanh", "RongSamCay", "RongSamCay", "RongVang" };
        AudioManager.SetSoundBg("nhacnen1");
        // CreateUserTopBoss(2, json["users"][0]["tenhienthi"].Value, json["users"][0]["id"].Value, json["users"][0]["toc"].Value, Inventory.LoadAnimator(json["users"][0]["chientuong"]["name"].Value));

        for (int i = 0; i < json["users"]["all"].Count; i++)
        {
            //debug.Log(json["users"][i]);
            if (json["users"]["all"][i]["chientuong"]["name"].Value != "")
            {
                if (json["users"]["all"][i]["id"].Value != json["users"]["my"]["id"].Value)
                {
                    CreateUserTopBoss(2, json["users"]["all"][i]["tenhienthi"].Value, json["users"]["all"][i]["id"].Value, json["users"]["all"][i]["toc"].Value, json["users"]["all"][i]["chientuong"]["name"].Value, float.Parse(json["users"]["all"][i]["x"].AsString), float.Parse(json["users"]["all"][i]["y"].Value), false);
                }
            }
            // yield return new WaitForSeconds(0.3f);
        }
        menulogin.SetActive(false);
        CreateUserTopBoss(2, json["users"]["my"]["tenhienthi"].Value, json["users"]["my"]["id"].Value, json["users"]["my"]["toc"].Value, json["users"]["my"]["chientuong"]["name"].AsString, float.Parse(json["users"]["my"]["x"].Value), float.Parse(json["users"]["my"]["y"].Value), true);
        if (json["nguoitieudietboss"].Value != "")
        {
            boss.imgHpBoss.transform.GetChild(0).GetComponent<Text>().text = json["nguoitieudietboss"].Value;
        }
        else boss.imgHpBoss.transform.GetChild(0).GetComponent<Text>().text = "";
        menuboss.transform.GetChild(16).transform.GetChild(0).GetComponent<Text>().text = json["namekhuvuc"].Value;
        if(boss.gettop) boss.timedemnguoc = float.Parse(json["timemoboss"].Value) * 60;
        if (boss.timedemnguoc > 0)
        {
          //  boss.txttimedemnguoc.transform.parent.transform.GetChild(2).GetComponent<Button>().interactable = false;
            boss.timerIsRunning = true;
        }
        for (int i = 0; i < NetworkManager.ins.loidai.objGiaoDienOff.Length; i++)
        {
            NetworkManager.ins.loidai.objGiaoDienOff[i].SetActive(false);
        }
        if(NgayDem == "Ngay") NetworkManager.ins.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanNgay");
        else if(NgayDem == "Dem") NetworkManager.ins.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanDem");
        giaodien.SetActive(true);
        giaodien.transform.GetChild(5).gameObject.SetActive(false);
        txtDanhVong.gameObject.SetActive(true);
        menuboss.transform.GetChild(3).transform.GetChild(3).GetComponent<Text>().text = json["tongsatthuong"].Value;
        //net.loidai.GiaoDien.SetActive(true);
        //net.loidai.GiaoDien.transform.SetParent(transform.parent.transform);
        //net.loidai.GiaoDien.transform.SetSiblingIndex(menuboss.transform.GetSiblingIndex() + 1);
        //net.loidai.GiaoDien.SetActive(true);
        AllMenu.ins.DestroyallMenu("MenuBossTG");
        menuboss.SetActive(true);
   
    }
    public void CreateUserTopBoss(int tienhoa, string tenhienthi, string idfb, string toc, string nameobjrong,float x, float y,bool hieuung = true)
    {
        if(AllMenu.ins.menu.ContainsKey("MenuBossTG"))
        {
            GameObject menuboss = AllMenu.ins.menu["MenuBossTG"];//AllMenu.ins.GetCreateMenu("MenuBossTG");
            menuboss.SetActive(true);
            GameObject nguoichoi = menuboss.transform.GetChild(0).gameObject;
            BossTG bosstg = menuboss.GetComponent<BossTG>();
            //if (inevntory.Cacherong.ContainsKey(nameRong))
            //{
            //    Rong = inevntory.Cacherong[nameRong];
            //}
            //else
            //{
            //    Rong = SGResources.Load("GameData/Rong/" + nameRong) as GameObject;
            //    inevntory.Cacherong.Add(nameRong,Rong);
            //}
            StartCoroutine(delay());
            IEnumerator delay()
            {
                //Vector3 tf = new Vector3(-3,-3);
              //  GameObject vitri = NetworkManager.ins.vienchinh.transform.GetChild(0).gameObject;
                GameObject Bong = Instantiate(NetworkManager.ins.loidai.BongRong, transform.position, Quaternion.identity);
                Vector3 randomtf = new Vector3(NetworkManager.ins.vienchinh.TruXanh.transform.position.x + x, NetworkManager.ins.vienchinh.TruXanh.transform.position.y + y);
                
                Bong.transform.position = randomtf;
                Bong.SetActive(true);
                //if (hieuung)
                //{
                //    GameObject hieuung = Instantiate(NetworkManager.ins.vienchinh.HieuUngTrieuHoi, new Vector3(Bong.transform.position.x, Bong.transform.position.y + 9), Quaternion.identity) as GameObject;
                //    Destroy(hieuung, 0.5f);
                //    yield return new WaitForSeconds(0.3f);
                //}
                yield return new WaitForSeconds(0.01f);
                Vector3 tfrong = new Vector3(Bong.transform.position.x - 50, Bong.transform.position.y + 25);
              
                //   Animator anim = Rongg.GetComponent<Animator>();
                GameObject dra = AllMenu.ins.GetRongGiaoDien(nameobjrong + tienhoa, Bong.transform, -1,true,"Rong");
                if(Setting.cauhinh == CauHinh.CauHinhThap)
                {
                    Setting.CauHinhThap.OffspriteSkin(dra.transform);
                }
                if(hieuung)
                {
                    Vector3 tfinstantiate = new Vector3(Bong.transform.position.x, GamIns.ins.MaxY, dra.transform.position.z);
                    dra.transform.position = tfinstantiate;
                    Animator anim = dra.GetComponent<Animator>();
                    anim.Play("Spawn");
                 //   anim.SetFloat("speedRun", 1.8f);
                    //Vector3 tfrong = new Vector3(ChuaRongTop.transform.GetChild(i).transform.position.x, ChuaRongTop.transform.GetChild(i).transform.position.y + 1);
                    dra.transform.LeanMove(Bong.transform.position, 0.3f);
                    yield return new WaitForSeconds(0.3f);
                   // anim.SetFloat("speedRun", 1f);
                }
                Bong.transform.SetParent(nguoichoi.transform, false);
                dra.transform.SetParent(Bong.transform);
                dra.transform.localScale = new Vector3(-50, 50);

                GameObject Rongg = Instantiate(bosstg.RongUsers, tfrong, Quaternion.identity);
                //   anim.runtimeAnimatorController = animm;//inevntory.LoadAnimatorRongCoBan(nameRong);//SGResources.Load<RuntimeAnimatorController>("GameData/Animator/" + nameRong);
                Rongg.SetActive(true);
                Rongg.transform.SetParent(Bong.transform, false);
                Rongg.transform.localScale = new Vector3(-50, 50);
                Rongg.transform.position = new Vector3(dra.transform.position.x,Rongg.transform.position.y,Rongg.transform.position.z);
             //   anim.SetInteger("TienHoa", tienhoa);
                Image imgAvatar = Rongg.transform.Find("avatar").GetComponent<Image>();
                if (idfb != "bot")
                {
                   // friend.GetAvatarFriend(idfb, imgAvatar);
                 
                }
             
                Image imgToc = Rongg.transform.Find("Khung").GetComponent<Image>();
                Friend.ins.LoadAvtFriend(idfb, imgAvatar,imgToc);
                //imgToc.sprite = Inventory.LoadSprite("Avatar" + toc);
                Text txtname = Rongg.transform.Find("txtname").GetComponent<Text>();
                txtname.text = tenhienthi;
                Text txttop = Rongg.transform.Find("txtTop").GetComponent<Text>();
                txttop.text = "";
                Rongg.name = tenhienthi;

                Bong.name = idfb;
                Bong.transform.position = randomtf;
                Destroy(Bong.GetComponent<Image>());
            }
           // AudioManager.SetSoundBg("nhacnen1");
        }
    }

    public void MuaDao(string tienmua)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "MuaDao";
        datasend["data"]["dao"] = DangODao.ToString();
        datasend["data"]["tienmua"] = tienmua;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                if (AllMenu.ins.transform.Find("BtnSangDao"))
                {
                    AllMenu.ins.transform.Find("BtnSangDao").transform.SetParent(giaodien.transform);
                }
                MenuMoDao.SetActive(false);
            }
            else
            {
                OnThongBaoNhanh(json["status"].Value, 2.5f);
            }
        }
    }

    public static string EncryptString(string Message, string Passphrase) // Hamma khoa chuoi

    {

        byte[] Results;

        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Buoc 1: Bam chuoi su dung MD5

        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Tao doi tuong TripleDESCryptoServiceProvider moi

        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Cai dat bo ma hoa

        TDESAlgorithm.Key = TDESKey;

        TDESAlgorithm.Mode = CipherMode.ECB;

        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert chuoi (Message) thanh dang byte[]

        byte[] DataToEncrypt = UTF8.GetBytes(Message);

        // Step 5. Ma hoa chuoi

        try

        {

            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();

            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

        }

        finally

        {

            // Xoa moi thong tin ve Triple DES va HashProvider de dam bao an toan

            TDESAlgorithm.Clear();

            HashProvider.Clear();

        }

        // Step 6. Tra ve chuoi da ma hoa bang thuat toan Base64

        return System.Convert.ToBase64String(Results);

    }
    public static string DecryptString(string Message, string Passphrase)
    {

        byte[] Results;

        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Step 1. Bam chuoi su dung MD5

        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Tao doi tuong TripleDESCryptoServiceProvider moi

        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Cai dat bo giai ma

        TDESAlgorithm.Key = TDESKey;

        TDESAlgorithm.Mode = CipherMode.ECB;

        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert chuoi (Message) thanh dang byte[]

        byte[] DataToDecrypt = System.Convert.FromBase64String(Message);

        // Step 5. Bat dau giai ma chuoi

        try

        {

            ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();

            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);

        }

        finally

        {

            // Xoa moi thong tin ve Triple DES va HashProvider de dam bao an toan

            TDESAlgorithm.Clear();

            HashProvider.Clear();

        }

        // Step 6. Tra ve ket qua bang dinh dang UTF8

        return UTF8.GetString(Results);

    }
}
[SerializeField]
public class XacNhanMuaCongTrinh
{
    public string name;
    public int cap,idcongtrinh;
    public string tienmua;
    public XacNhanMuaCongTrinh(string Name, int capnang,int id,string tien = "Vang")
    {
        name = Name;
        cap = capnang;
        idcongtrinh = id;
        tienmua = tien;
    }
}