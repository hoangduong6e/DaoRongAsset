using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EventTrungThu2023 : MonoBehaviour
{
    // Start is called before the first frame update

    public static EventTrungThu2023 ins;
    private GameObject btnHopQua;
    public GameObject BtnHopQua { get { return btnHopQua; } set { btnHopQua = value; } }
    public Sprite sprite1, sprite2, chuaduocnhan, danhan, duocnhan;
    public Sprite Top1, Top2, Top3, Top;
    public Sprite[] allItemEvent, alltuchat;
    public Dictionary<string, GameObject> menuevent = new Dictionary<string, GameObject>();
    public void DestroyMenu(string namemenu)
    {
        if (menuevent.ContainsKey(namemenu))
        {
            Destroy(menuevent[namemenu]);
            menuevent.Remove(namemenu);

            Resources.UnloadUnusedAssets();
        }
    }
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
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/EventTrungThu2023/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
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
    void Start()
    {
        ins = this;
    }
    public void ParseData(JSONNode json)
    {
        debug.Log("parsedata " + json);
        btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
        CrGame.ins.AllDao.transform.GetChild(0).gameObject.SetActive(false);
        btnHopQua.transform.SetParent(transform.Find("GiaoDien1"));
        GameObject giaodien1 = transform.Find("GiaoDien1").gameObject;
        GameObject ObjChoi = giaodien1.transform.Find("ObjChoi").gameObject;

        // Text txtDiem = ObjChoi.transform.Find("txtDiem").GetComponent<Text>();
        Text txtKyLuc = ObjChoi.transform.Find("txtKyLuc").GetComponent<Text>();
        txtKyLuc.text = "Kỷ lục: <color=yellow>" + json["data"]["KyLuc"].AsString + "</color>";
        CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(false);
        //Text txtbanhtrungthu = ObjChoi.transform.Find("txtBanhTrungThu").GetComponent<Text>();
        //txtbanhtrungthu.text = "Bánh trung thu: <color=red>" + json["data"]["BanhTrungThu"].AsString + "</color>";
        LoadMocQuaGD1(json["data"]["QuaTichLuyGD1"], json["allMocDiemGD1"], json["data"]["BanhTrungThu"].AsInt);
        if (json["data"]["nhannguyetlong"].AsBool)
        {
            giaodien1.transform.Find("objchuachoi").transform.Find("NguyetLong9S").gameObject.SetActive(false);
        }
        AudioManager.SetSoundBg("nhacnen1");
    }
    private void LoadMocQuaGD1(JSONNode allmocqua, JSONNode diemmocqua, int banhtrungthu)
    {
        GameObject gd1 = transform.GetChild(0).gameObject;
        GameObject ScrollViewallMocQua = gd1.transform.Find("objchuachoi").transform.Find("ScrollViewallMocQua").gameObject;
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
    private void NhanQuaTichLuyGd1()
    {
        AudioManager.PlaySound("soundClick");
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "NhanQuaTichLuyGd1";
        datasend["data"]["mocqua"] = btnnhan.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["BanhTrungThu"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void NhanQuaTichLuyGd2()
    {
        AudioManager.PlaySound("soundClick");
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "NhanQuaTichLuyGd2";
        datasend["data"]["mocqua"] = btnnhan.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadMocQuaGD2(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solanthaplongden"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void BatDauSolo()
    {
        AudioManager.PlaySound("soundClick");
        GameObject gd1 = transform.GetChild(0).gameObject;
        MiniGameTrungThu mini = gd1.transform.Find("ObjChoi").GetComponent<MiniGameTrungThu>();
        Text txtBatDau = gd1.transform.Find("TextBatDau").GetComponent<Text>();
        txtBatDau.gameObject.SetActive(true);
        gd1.transform.Find("objchuachoi").gameObject.SetActive(false);
        mini.gameObject.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            txtBatDau.text = "3";
            yield return new WaitForSeconds(0.7f);
            txtBatDau.text = "2";
            yield return new WaitForSeconds(0.9f);
            txtBatDau.text = "1";
            yield return new WaitForSeconds(0.80f);
            txtBatDau.text = "Bắt đầu";
            yield return new WaitForSeconds(0.80f);
            txtBatDau.gameObject.SetActive(false);
            mini.SetBatDau = true;
        }
    }
    public void SendKQ()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        GameObject gd1 = transform.GetChild(0).gameObject;
        gd1.transform.Find("objchuachoi").gameObject.SetActive(true);
        gd1.transform.Find("ObjChoi").gameObject.SetActive(false);

        MiniGameTrungThu.ins.Clear();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "SendKQ";
        datasend["data"]["Diem"] = MiniGameTrungThu.ins.GetSetDiem.ToString();

        foreach (KeyValuePair<string, int> allitemnhann in MiniGameTrungThu.ins.allitemNhan)
        {
            debug.Log(allitemnhann.Key + " " + allitemnhann.Value);
            datasend["data"]["allitemnhan"][allitemnhann.Key] = allitemnhann.Value.ToString();
        }
        MiniGameTrungThu.ins.allitemNhan = null;
        MiniGameTrungThu.ins.GetSetDiem = 0;

        for (int i = 0; i < MiniGameTrungThu.ins.allRongg.transform.childCount; i++)
        {
            Destroy(MiniGameTrungThu.ins.allRongg.transform.GetChild(i).gameObject);
        }
        MiniGameTrungThu.ins.GSNgayDem = "Ngay";
        gd1.GetComponent<Image>().sprite = MiniGameTrungThu.LoadSpriteResource("BGNGAY");

        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            CrGame.ins.panelLoadDao.SetActive(false);
            if (json["status"].AsString == "0")
            {
                GameObject BangKq = GetCreateMenu("BangKetQua", transform);
                BangKq.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = json["info"].Value;
                Button btnquaylai = BangKq.transform.GetChild(0).transform.Find("btnQuayLai").GetComponent<Button>();
                btnquaylai.onClick.RemoveAllListeners();
                btnquaylai.onClick.AddListener(QuayLai);
                Button btnChoiNhanh = BangKq.transform.GetChild(0).transform.Find("btnChoiNhanh").GetComponent<Button>();
                btnChoiNhanh.onClick.RemoveAllListeners();
                btnChoiNhanh.onClick.AddListener(ChoiNhanh);
                LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["BanhTrungThu"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void ChoiNhanh()
    {
        AudioManager.PlaySound("soundClick");
        //ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        //tbc.btnChon.onClick.RemoveAllListeners();
        //tbc.txtThongBao.text = "Bạn muốn thách đấu với người chơi " + "<color=#ffff00ff>" + nameDau + "</color> ?";
        //   tbc.btnChon.onClick.AddListener(VaoDau);
        QuayLai();
        GameObject KhungVeChoiNhanh = GetCreateMenu("KhungVeChoiNhanh", transform);
        GameObject khung = KhungVeChoiNhanh.transform.GetChild(0).transform.GetChild(0).gameObject;
        Button btnVe = khung.transform.GetChild(0).GetComponent<Button>();
        Button btnKC = khung.transform.GetChild(1).GetComponent<Button>();
        Button btnExit = khung.transform.GetChild(3).GetComponent<Button>();
        btnVe.onClick.RemoveAllListeners();
        btnVe.onClick.AddListener(delegate { XnChoiNhanh("Ve"); });
        btnKC.onClick.RemoveAllListeners();
        btnKC.onClick.AddListener(delegate { XnChoiNhanh("Kc"); });
        btnExit.onClick.RemoveAllListeners();
        btnExit.onClick.AddListener(delegate { DestroyMenu("KhungVeChoiNhanh"); });
    }
    void XnChoiNhanh(string itemdung)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "XnChoiNhanh";
        datasend["data"]["itemdung"] = itemdung;
        CrGame.ins.panelLoadDao.SetActive(true);
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            CrGame.ins.panelLoadDao.SetActive(false);
            if (json["status"].AsString == "0")
            {
                GameObject BangKq = GetCreateMenu("BangKetQua", transform);
                BangKq.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = json["info"].Value;
                Button btnquaylai = BangKq.transform.GetChild(0).transform.Find("btnQuayLai").GetComponent<Button>();
                btnquaylai.onClick.RemoveAllListeners();
                btnquaylai.onClick.AddListener(QuayLai);
                Button btnChoiNhanh = BangKq.transform.GetChild(0).transform.Find("btnChoiNhanh").GetComponent<Button>();
                btnChoiNhanh.onClick.RemoveAllListeners();
                btnChoiNhanh.onClick.AddListener(ChoiNhanh);
                LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["BanhTrungThu"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
            DestroyMenu("KhungVeChoiNhanh");
        }
    }
    public void QuayLai()
    {
        AudioManager.PlaySound("soundClick");
        DestroyMenu("BangKetQua");
    }
    public void VeNha()
    {
        AudioManager.PlaySound("soundClick");
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        AllMenu.ins.DestroyMenu("MenuEventTrungThu2023");
    }
    public void OpenGD2()
    {
        AudioManager.PlaySound("soundClick");
        GameObject gd2 = GetCreateMenu("GiaoDien2", transform);//transform.GetChild(1).gameObject;
                                                               //    menuevent["GiaoDien2"] = gd2;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
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
                btnDoiQua.onClick.AddListener(OpenMenuDoiManh);

                SetYeuCau(json["YeuCau"]);
                LoadMocQuaGD2(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solanthaplongden"].AsInt);
                gd2.SetActive(true);
                btnHopQua.transform.SetParent(gd2.transform);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
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
            datasend["class"] = "EventTrungThu2023";
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
    bool sangtrang = false; int trang = 1, trangg = 1; float top, topcuoi;
    private void XemBXH()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "XemBXH";
        datasend["data"]["top"] = top.ToString();
        datasend["data"]["trang"] = trangg.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = GetCreateMenu("MenuTop", transform, false);
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

                GameObject mntop = menu.transform.GetChild(0).transform.GetChild(4).gameObject;
                menu.transform.GetChild(0).transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuTop"); });
                mntop.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { sangtrangtop(1); });
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

    public void ThapLongDen(byte solanquay)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
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
    private int soluongchon = 0;
    string ThiepChon = "BanhTrungThuThuongHang";
    public void OpenMenuChonBanh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "Getitem";
        datasend["data"]["item"][0] = "HopBanhTrungThuThuongHang";
        datasend["data"]["item"][1] = "BanhTrungThuThuongHang";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject MenuChonBanh = GetCreateMenu("MenuChonBanh", transform, true);
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
            CrGame.ins.panelLoadDao.SetActive(true);
            StartCoroutine(Load());
            IEnumerator Load()
            {
                UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetListFriend/id/" + LoginFacebook.ins.id);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    debug.Log(www.error);
                    CrGame.ins.panelLoadDao.SetActive(false);
                    CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                    AllMenu.ins.DestroyMenu("menuFriend");
                }
                else
                {
                    // Show results as text
                    debug.Log(www.downloadHandler.text);

                    JSONNode json = JSON.Parse(www.downloadHandler.text);
                    if (json["status"].Value == "ok")
                    {
                        ThiepChon = btnchon.transform.parent.name;
                        soluongchon = int.Parse(btnchon.transform.parent.GetChild(3).GetComponent<InputField>().text);
                        GameObject menuchonthiep = menuevent["MenuChonBanh"];
                        menuchonthiep.SetActive(false);
                        GameObject menutangbanh = GetCreateMenu("MenuTangBanh", transform);

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
                        CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                        AllMenu.ins.DestroyMenu("menuFriend");
                    }
                    CrGame.ins.panelLoadDao.SetActive(false);
                    //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
                }
            }
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
            datasend["class"] = "EventTrungThu2023";
            datasend["method"] = "TangBanh";
            datasend["data"]["taikhoanban"] = taikhoanban;
            datasend["data"]["tenban"] = btnchon.transform.parent.transform.GetChild(3).GetComponent<Text>().text;
            datasend["data"]["soluong"] = soluongchon.ToString();
            datasend["data"]["namephao"] = ThiepChon;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "ok")
                {
                    DestroyMenu("MenuTangBanh");
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
            }

        }
    }
    public IEnumerator DelayHienQua(JSONNode allqua)
    {
        //yield return new WaitForSeconds(0.4f);
        // JSONNode allqua = allquanhan["allqua"];
        GameObject panelnhanqua = GetCreateMenu("PanelNhanQua", transform, true);
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
                for (int j = 0; j < allItemEvent.Length; j++)
                {
                    if (allItemEvent[j].name == allqua[i]["nameitem"].Value)
                    {
                        Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                        img.sprite = allItemEvent[j];
                        // img.SetNativeSize();
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
            //   allquanhan = null;
        }
        btnnhan.interactable = true;
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
    public void CloseGD2()
    {
        //GameObject gd2 = transform.GetChild(1).gameObject;
        //gd2.SetActive(false);
        btnHopQua.transform.SetParent(transform.GetChild(0).transform);
        //foreach (KeyValuePair<string, GameObject> allitemnhann in menuevent)
        //{
        //    debug.Log(allitemnhann.Key + " " + allitemnhann.Value.gameObject.name);
        //    // datasend["data"]["allitemnhan"][allitemnhann.Key] = allitemnhann.Value.ToString();
        //    string destroy = allitemnhann.Key.ToString();
        //    DestroyMenu(destroy);
        //}
        string[] array = menuevent.Keys.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            debug.Log(array[i]);
            DestroyMenu(array[i]);
        }


    }
    public void OpenSkGioiHan()
    {
        AudioManager.PlaySound("soundClick");
        AllMenu.ins.OpenMenuTrenCung("MenuSuKienGioiHan");
    }
    public void GetDiemDanh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "GetDiemDanh";
        NetworkManager.ins.SendServer(datasend, Ok);
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
        GameObject DiemDanh = GetCreateMenu("MenuDiemDanh", transform).transform.GetChild(0).gameObject;
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
                DiemDanh.transform.GetChild(3).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
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
                DiemDanh.transform.GetChild(4).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
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
                DiemDanh.transform.GetChild(5).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
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
            DiemDanh.transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
        }
        menuevent["MenuDiemDanh"].transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuDiemDanh"); });
        //     DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1);
    }

    private void DiemDanhh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "DiemDanhEvent";
        NetworkManager.ins.SendServer(datasend, DiemDanhok);
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


    public void NhanQuaDiemDanh()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        AudioManager.PlaySound("soundClick");
        int vitri = btn.transform.parent.GetSiblingIndex() - 3;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "NhanQuaDiemDanh";
        datasend["data"]["vitri"] = vitri.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //foreach (string key in json["allitemUpdate"].AsObject.Keys)
                //{
                //    debug.Log(key);
                //    if (key != "Capsule")
                //    {
                //        SetItemEvent(key, json["allitemUpdate"][key].AsString);
                //    }
                //    else
                //    {
                //        GameObject giaodien2 = transform.Find("GiaoDien2").gameObject;

                //        GameObject contentCapsule = giaodien2.transform.Find("GiaoDienChonCapsule").transform.Find("ScrollViewallcapsule").transform.GetChild(0).transform.GetChild(0).gameObject;
                //        GameObject objitem = contentCapsule.transform.GetChild(0).gameObject;

                //        GameObject item = Instantiate(objitem, transform.position, Quaternion.identity);
                //        item.transform.SetParent(contentCapsule.transform, false);
                //        for (int j = 0; j < allCapsuleSprite.Length; j++)
                //        {
                //            if (allCapsuleSprite[j].name == json["allitemUpdate"][key]["mau"].AsString)
                //            {
                //                item.transform.GetChild(1).GetComponent<Image>().sprite = allCapsuleSprite[j];
                //                break;
                //            }
                //        }
                //        string nameitem = json["allitemUpdate"][key]["nameitem"].AsString;
                //        if (nameitem.Contains("Rong") && nameitem != "BuiRong")
                //        {
                //            item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(nameitem + "1");
                //        }
                //        else
                //        {
                //            item.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite(nameitem);
                //        }
                //        item.name = json["allitemUpdate"][key]["namecapsule"].AsString;
                //        item.SetActive(true);
                //    }
                //}
                LoadDiemDanh(json["nhandiemdanh"], json["landiemdanh"].AsInt, json["diemdanh"].AsBool);
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        AudioManager.PlaySound("soundClick");
        MenuTrieuHoiRongVangBac mn = AllMenu.ins.GetCreateMenu("MenuTrieuHoiRongVangBac", gameObject, false, 1).GetComponent<MenuTrieuHoiRongVangBac>();
        mn.Setnamerong = namerong;
        mn.gameObject.SetActive(true);
    }
    public void OpenMenuNhiemvu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "GetNhiemVu";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", transform);
                GameObject AllNhiemVu = MenuNhiemVu.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                for (int i = 0; i < json["allNhiemvu"].Count; i++)
                {
                    Text txttiendo = AllNhiemVu.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    if (int.Parse(json["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["allNhiemvu"][i]["maxnhiemvu"].Value))
                    {
                        txttiendo.text = "<color=#00ff00ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                }
            }
        }
    }
    public void OpenMenuDoiManh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "MenuDoiManh";
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                GameObject menuDoiManh = GetCreateMenu("MenuDoiManh", transform, false);
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
                        for (int j = 0; j < allItemEvent.Length; j++)
                        {
                            if (json["ManhDoi"][i]["nameitem"].Value == allItemEvent[j].name)
                            {
                                imgmanh.sprite = allItemEvent[j];
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
        GameObject yeucau = menuevent["MenuDoiManh"].transform.GetChild(1).gameObject;
        namemanhchon = btnchon.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
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
                        int itemcan = int.Parse(json["manhdoi"]["itemcan"][i]["soluong"].Value);

                        if (json["manhdoi"]["itemcan"][i]["loaiitem"].Value == "ItemEvent")
                        {
                            int itemco = int.Parse(json[json["manhdoi"]["itemcan"][i]["nameitem"].Value].Value); // int.Parse(json["allitemevent"][json["manhdoi"]["itemcan"][i]["nameitem"].Value].Value);
                            if (itemco >= itemcan)
                            {
                                sonvok += 1;
                                txtyeucau.text = "<color=#00ff00ff>" + itemco + "/" + itemcan + "</color>";
                            }
                            else txtyeucau.text = "<color=#ff0000ff>" + itemco + "/" + itemcan + "</color>";
                        }
                        else if (json["manhdoi"]["itemcan"][i]["loaiitem"].Value == "ItemNgoc")
                        {
                            for (var k = 0; k < NetworkManager.ins.inventory.contentNgoc.transform.childCount; k++)
                            {
                                if (NetworkManager.ins.inventory.contentNgoc.transform.GetChild(k).name == json["manhdoi"]["itemcan"][i]["nameitem"].Value)
                                {
                                    int solixico = int.Parse(NetworkManager.ins.inventory.contentNgoc.transform.GetChild(k).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text);
                                    if (solixico >= itemcan)
                                    {
                                        sonvok += 1;
                                        txtyeucau.text = "<color=#00ff00ff>" + solixico + "/" + itemcan + "</color>";
                                    }
                                    else txtyeucau.text = "<color=#ff0000ff>" + solixico + "/" + itemcan + "</color>";
                                    break;
                                }
                                else if (k == NetworkManager.ins.inventory.contentNgoc.transform.childCount - 1)
                                {
                                    txtyeucau.text = "<color=#ff0000ff>0/" + itemcan + "</color>";
                                }
                            }
                        }
                        else
                        {
                            if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + json["manhdoi"]["itemcan"][i]["nameitem"].Value))
                            {
                                int solixico = int.Parse(NetworkManager.ins.inventory.ListItemThuong["item" + json["manhdoi"]["itemcan"][i]["nameitem"].Value].transform.GetChild(0).GetComponent<Text>().text);
                                if (solixico >= itemcan)
                                {
                                    sonvok += 1;
                                    txtyeucau.text = "<color=#00ff00ff>" + solixico + "/" + itemcan + "</color>";
                                }
                                else txtyeucau.text = "<color=#ff0000ff>" + solixico + "/" + itemcan + "</color>";
                            }
                            else txtyeucau.text = "<color=#ff0000ff>0/" + itemcan + "</color>";
                        }

                    }
                }
            }
            if (sonvok >= json["manhdoi"]["itemcan"].Count)
            {
                menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(true);
                menuevent["MenuDoiManh"].transform.GetChild(2).GetComponent<Button>().interactable = true;
            }
            else
            {
                menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(false);
            }
            menuevent["MenuDoiManh"].transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = json["manhdoi"]["thongtin"].Value;
        }
    }
    void DoiManh()
    {
        AudioManager.PlaySound("soundClick");
        //Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "DoiQua";
        datasend["data"]["namemanh"] = namemanhchon;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //foreach (string key in json["allItem"].AsObject.Keys)
                //{
                //    SetItemEvent(key, json["allItem"][key].AsString);
                //}

                GameObject menudoimanh = menuevent["MenuDoiManh"];
                menudoimanh.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = "";
                //   menudoimanh.transform.GetChild(1).gameObject.SetActive(false);
                menudoimanh.transform.GetChild(2).gameObject.SetActive(false);
                GameObject yeucau = menudoimanh.transform.GetChild(1).gameObject;
                for (int i = 0; i < yeucau.transform.childCount; i++)
                {
                    yeucau.transform.GetChild(i).gameObject.SetActive(false);
                }
                menuevent["GiaoDien2"].transform.Find("itemDaMatTrang").GetChild(1).GetComponent<Text>().text = json["DaMatTrang"].AsString;
                CrGame.ins.OnThongBaoNhanh("Đã đổi!");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
}
