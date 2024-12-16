using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class EventManager : MonoBehaviour
{
    public string nameEvent;
    public Dictionary<string, GameObject> menuevent = new Dictionary<string, GameObject>();
    public Sprite Top1, Top2, Top3, Top;
    public static EventManager ins;
    public Sprite[] allitemEvent;
    public GameObject btnHopQua { get; protected set; }
    private void Awake()
    {
        ins = this;
        ABSAwake();
    }
    protected abstract void ABSAwake();
    public void DestroyMenu(string namemenu)
    {
        if (menuevent.ContainsKey(namemenu))
        {
            Destroy(menuevent[namemenu]);
            menuevent.Remove(namemenu);

            Resources.UnloadUnusedAssets();
        }
    }
    public GameObject LoadObjectResource(string name)
    {
        return Inventory.LoadObjectResource("GameData/" + nameEvent + "/" + name);
    }
    public GameObject GetCreateMenu(string namemenu, Transform parnet = null, bool active = true,int index = 1)
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
          //  debug.Log(namemenu + " menu");
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/"+ nameEvent + "/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
            menuevent.Add(namemenu, mn);
            mn.transform.SetParent(parnet.transform, false);
            mn.transform.position = parnet.transform.position;
            mn.SetActive(active);
            g = mn;
            //  Resources.UnloadUnusedAssets();
        }
        g.transform.SetSiblingIndex(index);
        //  g.transform.SetSiblingIndex(index);
        return g;
    }
    public AudioClip GetAudioEvent(string s)
    {
        return Resources.Load("GameData/" + nameEvent + "/" + s) as AudioClip;
    }    
    public Sprite GetSprite(string name)
    {

        Sprite sprite = Resources.Load<Sprite>("GameData/" + nameEvent + "/" + name);

        // Nếu sprite không tồn tại, trả về sprite mặc định
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>("GameData/Sprite/Default");
        }

        return sprite;
    }
    //void Start()
    //{
    //    ins = this;
    //}
    public void GetDiemDanh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDiemDanh";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadDiemDanh(json["nhandiemdanh"], json["landiemdanh"].AsInt, json["diemdanh"].AsBool);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    void LoadDiemDanh(JSONNode nhandiemdanh, int landiemdanh, bool diemdanh)
    {
        GameObject DiemDanh = GetCreateMenu("MenuDiemDanh", CrGame.ins.trencung.transform,true, transform.GetSiblingIndex() + 1).transform.GetChild(0).gameObject;
        int solandiemdanh = landiemdanh;
        DiemDanh.transform.GetChild(1).GetComponent<Text>().text = "Bạn đã điểm danh <color=lime>" + solandiemdanh + " ngày</color>.";
        if (diemdanh)
        {
            Button btn = DiemDanh.transform.GetChild(2).GetComponent<Button>();
            btn.interactable = true;
            btn.onClick.AddListener(DiemDanhh);
            DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + 0 + "/1</color>";
        }
        else
        {
            DiemDanh.transform.GetChild(2).GetComponent<Button>().interactable = false;
            DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + 1 + "/1</color>";
            if (nhandiemdanh[0].Value == "duocnhan")
            {
                Button btn = DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>();
                btn.interactable = true;
                btn.onClick.AddListener(NhanQuaDiemDanh);
            }
            else if (nhandiemdanh[0].Value == "danhan")
            {
                DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(3).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
            }
        }
        if (solandiemdanh < 7) DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/7</color>";
        else
        {
            DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/7</color>";
            if (nhandiemdanh[1].Value == "duocnhan")
            {
                Button btn = DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>();
                btn.interactable = true;
                btn.onClick.AddListener(NhanQuaDiemDanh);
            }
            else if (nhandiemdanh[1].Value == "danhan")
            {
                DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(4).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
            }
        }
        if (solandiemdanh < 14) DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/14</color>";
        else
        {
            DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/14</color>";
            if (nhandiemdanh[2].Value == "duocnhan")
            {
                Button btn = DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>();
                btn.interactable = true;
                btn.onClick.AddListener(NhanQuaDiemDanh);
            }
            else if (nhandiemdanh[2].Value == "danhan")
            {
                DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(5).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
            }
        }
        if (nhandiemdanh[3].Value == "duocnhan")
        {
            Button btn = DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>();
            btn.interactable = true;
            btn.onClick.AddListener(NhanQuaDiemDanh);
        }
        else if (nhandiemdanh[3].Value == "danhan")
        {
            DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>().interactable = false;
            DiemDanh.transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
        }
        menuevent["MenuDiemDanh"].transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuDiemDanh"); });
        //     DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1);
    }

    private void DiemDanhh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "DiemDanhEvent";
        NetworkManager.ins.SendServer(datasend.ToString(), DiemDanhok);
        void DiemDanhok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadDiemDanh(json["nhandiemdanh"], json["landiemdanh"].AsInt, json["diemdanh"].AsBool);
                CrGame.ins.OnThongBaoNhanh("Đã điểm danh!");
              
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    protected abstract void DiemDanhOk(JSONNode json);
    public void OpenMenuDoiManhRong(string namerong)
    {
        AudioManager.PlaySound("soundClick");
        MenuTrieuHoiRongVangBac mn = AllMenu.ins.GetCreateMenu("MenuTrieuHoiRongVangBac", CrGame.ins.trencung.gameObject, false,transform.GetSiblingIndex() + 1).GetComponent<MenuTrieuHoiRongVangBac>();
        mn.Setnamerong = namerong;
        mn.gameObject.SetActive(true);
    }
    public void NhanQuaDiemDanh()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        AudioManager.PlaySound("soundClick");
        int vitri = btn.transform.parent.GetSiblingIndex() - 3;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanQuaDiemDanh";
        datasend["data"]["vitri"] = vitri.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                LoadDiemDanh(json["nhandiemdanh"], json["landiemdanh"].AsInt, json["diemdanh"].AsBool);
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
                DiemDanhOk(json["dataupdate"]);
                //GetComponent<MenuEventTet2024>().SetLiXi(json["luothailixi"].AsString);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }

    public GameObject OpenMenuNhanDuoc(string namequa,string hienthi,LoaiItem itemgi)
    {
        GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Event/PanelHienQua"), transform.position, Quaternion.identity);
        mn.transform.SetParent(CrGame.ins.trencung.transform, false);
        //  mn.transform.position = parnet.transform.position;
        Image imgqua = mn.transform.Find("imgqua").GetComponent<Image>();
        Text txt = mn.transform.Find("txt").GetComponent<Text>();
        if (itemgi == LoaiItem.rong)
        {
            imgqua.sprite = Inventory.LoadSpriteRong(namequa);
        }
        else if(itemgi == LoaiItem.item)
        {
            imgqua.sprite = Inventory.LoadSprite(namequa);
        }
        else if (itemgi == LoaiItem.itemevent)
        {
            imgqua.sprite = GetSprite(namequa);
        }
        imgqua.SetNativeSize();
        txt.text = hienthi;
        StartDelay(() => {
            Button btn = mn.GetComponent<Button>();
            btn.onClick.AddListener(delegate {Destroy(mn);}) ;
        },0.5f);
        return mn;
    }

    public Sprite GetSpriteAll(string namequa, LoaiItem itemgi)
    {
        if (itemgi == LoaiItem.rong)
        {
            // Kiểm tra nếu chuỗi đã có "1" hoặc "2" ở cuối
            if (!namequa.EndsWith("1") && !namequa.EndsWith("2"))
            {
                // Thêm "1" vào cuối chuỗi
                namequa += "1";
            }
            return Inventory.LoadSpriteRong(namequa);
        }
        else if (itemgi == LoaiItem.item)
        {
            return Inventory.LoadSprite(namequa);
        }
        else if (itemgi == LoaiItem.itemevent)
        {
            return GetSprite(namequa);
        }
        return null;
    }
    public void StartDelay(Action actionDelay, float sec)
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(sec);
            actionDelay();
        }
    }
    public static void StartDelay2(Action actionDelay, float sec)
    {
        CrGame.ins.StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(sec);
            actionDelay();
        }
    }
    public static void OpenThongBaoChon(string tb, Action action,string strbtn = "",Action btnhuy = null,bool timetxt = false,string strbtnhuy = "")
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", CrGame.ins.trencung.gameObject, true, CrGame.ins.panelLoadDao.transform.GetSiblingIndex()).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();
        tbc.txtThongBao.text = tb;
        tbc.btnChon.onClick.AddListener(delegate { action(); });
        if(strbtn != "")
        {
            tbc.btnChon.transform.GetChild(0).GetComponent<Text>().text = strbtn;
        }    
        if(btnhuy != null)
        {
            tbc.transform.Find("btnHuy").GetComponent<Button>().onClick.AddListener(delegate {btnhuy(); });
        }
        if (strbtnhuy != "")
        {
            tbc.transform.Find("btnHuy").transform.GetChild(0).GetComponent<Text>().text = strbtnhuy;
        }
        if (timetxt)
        {
            tbc.txtThongBao.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            tbc.txtThongBao.transform.LeanScale(new Vector3(0.5f,0.5f,1),0.2f);
        }
    }
    public void OpenMenuDoiManh()
    {
        //return;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "MenuDoiManh";
      //  if (menuevent.ContainsKey("GiaoDien2")) datasend["method"] = "MenuDoiManh2";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);

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
                    manh.GetComponent<Button>().onClick.AddListener(XemManhDoi);
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
                    GamIns.ResizeItem(imgmanh, 200);
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
    //    if (menuevent.ContainsKey("GiaoDien2")) datasend["method"] = "XemManhDoi2";
        datasend["data"]["namemanh"] = namemanhchon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
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

    void DoiManh()
    {
        AudioManager.PlaySound("soundClick");

        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "DoiQua";
        if (menuevent.ContainsKey("GiaoDien2")) datasend["method"] = "DoiQua2";
        datasend["data"]["namemanh"] = namemanhchon;

        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
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
    }
}

public enum LoaiItem
{
    rong,
    item,
    itemevent
}
