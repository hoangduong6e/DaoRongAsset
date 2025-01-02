using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEventTet2024 : EventManager
{
    // Start is called before the first frame update
    public Sprite[] alltuchat;
    private GameObject GiaoDienCayMai, CayMai;
    public Sprite sprite1, sprite2, chuaduocnhan, duocnhan, danhan;
    public Transform trencung;
    public static MenuEventTet2024 inss;
    protected override void ABSAwake()
    {
        inss = this;
    }
    protected override void DiemDanhOk(JSONNode json)
    {

    }
    public void ParseData(JSONNode json)
    {
        AudioManager.SetSoundBg(GetAudioEvent("nhacnentet"));
        trencung = GameObject.FindGameObjectWithTag("trencung").transform;
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        GiaoDienCayMai = transform.GetChild(0).gameObject;
        CayMai = GiaoDienCayMai.transform.Find("CayMai").gameObject;

        SetAllLiXiNhan(json["data"]["AllLiXiNhan"]);
        SetLiXi(json["data"]["luothailixi"].AsString);
        LoadMocQuaGD1(json["data"]["QuaTichLuyGD1"], json["allMocDiemGD1"], json["data"]["DongXu"].AsInt);

        if (json["data"]["nhankilan"].AsBool) GiaoDienCayMai.transform.Find("KyLan9Sao").gameObject.SetActive(false);
        btnHopQua.transform.SetParent(trencung.transform);
    }
    public void TriggerBaoLiXi(Sprite sprite)
    {
        Image img = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
    }    
    private void SetAllLiXiNhan(JSONNode json)
    {
        byte soLiXiDaHai = 0;
        for (int i = 0; i < json.Count; i++)
        {
            if (json[i].AsBool)
            {
                CayMai.transform.GetChild(i).gameObject.SetActive(false);
                soLiXiDaHai++;
            }
            else CayMai.transform.GetChild(i).gameObject.SetActive(true);
        }
        if(soLiXiDaHai >= json.Count)
        {
            GiaoDienCayMai.transform.Find("btnLamMoi").gameObject.SetActive(true);
        }
        else GiaoDienCayMai.transform.Find("btnLamMoi").gameObject.SetActive(false);
    }
    public void SetLiXi(string s)
    {
        GiaoDienCayMai.transform.Find("txtLuotHaiLiXi").GetComponent<Text>().text = "Bạn có <color=lime>" + s + "</color> lượt hái lì xì";
    }
    bool menuxacnhan = true;
    byte vitrichon;
    public void HaiLiXi()
    {
        AudioManager.PlaySound("soundClick");
        if(menuxacnhan) vitrichon = (byte)UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "HaiLiXi";
        datasend["data"]["vitrichon"] = vitrichon.ToString();
        datasend["data"]["menuxacnhan"] = menuxacnhan.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                menuxacnhan = true;
                SetLiXi(json["luothailixi"].AsString);
                StartCoroutine(DelayHienQua(json["allqua"]));
                SetAllLiXiNhan(json["AllLiXiNhan"]);
                LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["DongXu"].AsInt);
            }
            else if (json["status"].Value == "menuxacnhan")
            {
                menuxacnhan = false;
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                tbc.txtThongBao.text = json["message"].AsString;
                tbc.btnChon.onClick.AddListener(HaiLiXi);
                Button btnHuy = tbc.transform.Find("btnHuy").GetComponent<Button>();
                btnHuy.onClick.RemoveAllListeners();
                btnHuy.onClick.AddListener(OffMenuXacNhan);
            }    
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    private void OffMenuXacNhan()
    {
        menuxacnhan = true;
    }    
    private bool dotphao = true;
    public void DotPhaoGD1()
    {
        if (!dotphao) return;
        dotphao = false;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "DotPhaoGD1";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    GameObject PhaoTet2024 = GiaoDienCayMai.transform.Find("PhaoTet2024").gameObject;
                    PhaoTet2024.GetComponent<Animator>().Play("Explose");
                    yield return new WaitForSeconds(0.25f);
                    PhaoTet2024.GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(DelayHienQua(json["allqua"]));
                    LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["DongXu"].AsInt);
                    dotphao = true;
                    yield return new WaitForSeconds(5f);
                    PhaoTet2024.GetComponent<Animator>().Play("Idlle");
                    
                }
            
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                dotphao = true;
            }
        }
    }

    public IEnumerator DelayHienQua(JSONNode allqua)
    {
        //yield return new WaitForSeconds(0.4f);
        // JSONNode allqua = allquanhan["allqua"];
        GameObject panelnhanqua = GetCreateMenu("PanelNhanQua", transform, true);
        Button btnnhan = panelnhanqua.transform.GetChild(1).transform.GetChild(0).GetComponent<Button>();

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
                for (int j = 0; j < allitemEvent.Length; j++)
                {
                    if (allitemEvent[j].name == allqua[i]["nameitem"].Value)
                    {
                        Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                        img.sprite = allitemEvent[j];
                        // img.SetNativeSize();
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
    public void LamMoiCayMai()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "LamMoiCayMai";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                SetAllLiXiNhan(json["AllLiXiNhan"]);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }

    private void LoadMocQuaGD1(JSONNode allmocqua, JSONNode diemmocqua, int banhtrungthu)
    {
        GameObject ScrollViewallMocQua = GiaoDienCayMai.transform.Find("ScrollViewallMocQua").gameObject;
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
                LoadMocQuaGD1(json["QuaTichLuyGD1"], json["allMocDiemGD1"], json["DongXu"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenGiaoDienMiniGameTet()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetGiaoDienMiniGameTet";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                GiaoDienMiniGameTet2024 GiaoDienMiniGame = GetCreateMenu("GiaoDienMiniGame",trencung.transform,true,1).GetComponent<GiaoDienMiniGameTet2024>();
                GiaoDienMiniGame.gameObject.SetActive(true);
                GiaoDienMiniGame.ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void OpenGiaoDienThinhRong()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetGiaoDienThinhRong";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                GiaoDienThinhRong GiaoDien = GetCreateMenu("GiaoDienThinhRong",trencung.transform).GetComponent<GiaoDienThinhRong>();
                GiaoDien.gameObject.SetActive(true);
                GiaoDien.ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
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
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", trencung.transform);
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
    public void OpenGiaoDienTuoiCay()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetGiaoDienTuoiCay";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "0")
            {
                GiaoDienTuoiCay GiaoDien = GetCreateMenu("GiaoDienTuoiCay", trencung.transform).GetComponent<GiaoDienTuoiCay>();
                GiaoDien.gameObject.SetActive(true);
                GiaoDien.ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
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
        AllMenu.ins.DestroyMenu("MenuEventTet2024");
    }
}
