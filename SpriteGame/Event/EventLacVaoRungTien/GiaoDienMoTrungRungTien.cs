using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GiaoDienMoTrungRungTien : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ObjTiaSet;
    public Sprite[] alltuchat;
    string nameEvent = "EvenLacVaoRungTien";
    float giaChuyenDoiTrung = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool HienXacnhanmotrung = true;
    public void ParseData(JSONNode json)
    {
        debug.Log(json.ToString());
        Transform allTrung = transform.Find("allTrung");
        string[] allitemtrung = new string[] { "TrungRongLua", "TrungRongBang", "TrungRongSam", "TrungRongDat", "TrungRongCay" };
        for (int i = 0; i < allTrung.transform.childCount; i++)
        {
            Text txttrung = allTrung.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
            int trung = json["allItem"][allitemtrung[i]].AsInt;
            txttrung.text = (trung > 0) ? "<color=lime>" + trung + "/1</color>" : "<color=red>" + trung + "/1</color>";
        }
        transform.Find("TrungVoSac").transform.GetChild(1).GetComponent<Text>().text = json["allItem"]["TrungVoSac"].AsString;
        transform.Find("TrungNguSac").transform.GetChild(1).GetComponent<Text>().text = json["allItem"]["TrungNguSac"].AsString;
        transform.Find("DaNguSac").transform.GetChild(1).GetComponent<Text>().text = json["allItem"]["DaNguSac"].AsString;
        SetYeuCau(json["YeuCau"]);
        giaChuyenDoiTrung = json["giaChuyenDoiTrung"].AsFloat;
        allmoc = json["allmoc"];
        LoadMocQua(json["solanquay"].AsInt);
        EventManager.ins.btnHopQua.transform.SetParent(transform, false);
        gameObject.SetActive(true);
        SetAllMoc();
    }

    public void VeNha()
    {
        EventManager.ins.btnHopQua.transform.SetParent(EventManager.ins.transform, false);
        EventManager.ins.DestroyMenu("GiaoDienMoTrung");
        //  gameObject.SetActive(false);
    }
    void SetYeuCau(JSONNode yeucau)
    {
        debug.Log(yeucau.ToString());
        GameObject btnX1 = transform.Find("btnquayX1").gameObject;
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

        GameObject btnX10 = transform.Find("btnquayX10").gameObject;
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
    private bool duocquay = true;
    public void MoTrung(string x)
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
         // debug.Log(btnchon.transform.GetChild(3).gameObject.name);
        if (HienXacnhanmotrung && btnchon.transform.GetChild(3).gameObject.activeSelf)
        {
            EventManager.OpenThongBaoChon("<color=red>Lượt quay này sẽ tốn</color> <color=magenta>Kim cương</color>\n<color=yellow>(chỉ nhắc lần đầu)</color>", delegate { Quay(); HienXacnhanmotrung = false; });
            
        }
        else Quay();
        void Quay()
        {
            if (!duocquay) return;
            duocdunghop = false;
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "MoTrung";
            datasend["data"]["solanquay"] = x;
            NetworkManager.ins.SendServer(datasend, Ok, true);
            void Ok(JSONNode json)
            {
                debug.Log(json.ToString());
                if (json["status"].AsString == "0")
                {
                    duocquay = false;

                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        ObjTiaSet.SetActive(true);
                        yield return new WaitForSeconds(0.5f);
                        ObjTiaSet.SetActive(false);
                        SetYeuCau(json["YeuCau"]);
                        duocquay = true;
                        transform.Find("TrungVoSac").transform.GetChild(1).GetComponent<Text>().text = json["TrungVoSac"].AsString;

                        StartCoroutine(DelayHienQua(json["allqua"]));

                        duocdunghop = true;
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                    duocdunghop = true;
                }
            }
        }
   
    }
    GameObject panelnhanqua;
    public IEnumerator DelayHienQua(JSONNode allqua,bool updatetrung = true)
    {
        //yield return new WaitForSeconds(0.4f);
        // JSONNode allqua = allquanhan["allqua"];
        if (allqua.Count == 0) yield break;
        panelnhanqua = transform.Find("PanelNhanQua").gameObject;
     //   panelnhanqua.transform.SetParent(CrGame.ins.trencung);
        Button btnnhan = panelnhanqua.transform.GetChild(1).transform.GetChild(1).GetComponent<Button>();
        btnnhan.onClick.RemoveAllListeners();
        // btnnhan.onClick.AddListener(XacNhanQua);
        panelnhanqua.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        //SetYeuCau(allquanhan["YeuCau"]);
        GameObject objallqua = panelnhanqua.transform.GetChild(1).gameObject;
        btnnhan.interactable = false;
        objallqua.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameObject allquaa = objallqua.transform.GetChild(0).gameObject;
        GameObject objqua = allquaa.transform.GetChild(0).gameObject;
        allquaa.GetComponent<GridLayoutGroup>().enabled = true;
        //bool themqua = false;
        for (int i = 0; i < allqua.Count; i++)
        {
            //if(allqua[i]["nameitem"].AsString != "luibuoc")
            //{

            //}
            if (i > 14) break;
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
            if (allqua[i]["loaiitem"].AsString == "item")
            {
                //if(!themqua)
                //{
                //    themqua = true;
                //    EventManager.ins.btnHopQua.GetComponent<HopQua>().Them1Qua();
                //}
                // if (objtrong.transform.Find("vien")) Destroy(objtrong.transform.Find("vien"));
                objtrong.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(allqua[i]["nameitem"].Value);
                objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                objtrong.name = "item" + allqua[i]["nameitem"].Value;
                objtrong.GetComponent<infoitem>().enabled = true;
            }
            else if (allqua[i]["loaiitem"].AsString == "itemevent")
            {
                Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                img.sprite = EventManager.ins.GetSprite(allqua[i]["nameitem"].AsString);
                img.SetNativeSize();
                // img.SetNativeSize();
                objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                objtrong.GetComponent<infoitem>().enabled = true;
                objtrong.name = "item" + allqua[i]["nameitem"].Value;
                if(updatetrung) UpdateTrung(allqua[i]["nameitem"].AsString, allqua[i]["soluonghientai"].AsInt);
       
            }
            qua.name = allqua[i]["nameitem"].Value;

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
    public void XacNhanQua()
    {
        AudioManager.PlaySound("soundClick");
        GameObject hopqua = EventManager.ins.btnHopQua;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject objallqua = panelnhanqua.transform.GetChild(1).transform.GetChild(0).gameObject;
            panelnhanqua.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;

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
        //    panelnhanqua.transform.SetParent(transform);
            panelnhanqua.transform.GetChild(1).gameObject.SetActive(false);
            panelnhanqua.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            panelnhanqua.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
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
            Transform vitribay = null;
            Transform allTrung = transform.Find("allTrung");


            for (int i = 0; i < allTrung.transform.childCount; i++)
            {
                Transform child = allTrung.transform.GetChild(i);
                if (child.name == qua.name)
                {
                    if (!dic.ContainsKey(qua.name))
                    {
                        vitribay = child;
                    }
                    else vitribay = child.transform.GetChild(4);
                    break;
                }
            }
            QuaBay quabay = quatrong.AddComponent<QuaBay>();
            if (vitribay == null) vitribay = transform.Find("TrungNguSac");
            quabay.vitribay = vitribay.gameObject;
            qua.SetActive(false);
            //  Destroy(qua, 5);
        }
    }
    bool duocdunghop = true;
    public void DungHopTrung()
    {
        EventManager.OpenThongBaoChon("Bạn có muốn dung hợp tất cả trứng để lấy trứng Ngũ Sắc không?", xacnhan);

        void xacnhan()
        {
            if (!duocdunghop) return;
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "DungHopTrung";
            NetworkManager.ins.SendServer(datasend, Ok, true);
            void Ok(JSONNode json)
            {
                debug.Log(json.ToString());
                if (json["status"].AsString == "0")
                {
                    duocdunghop = false;

                    Transform spriteTrungNguSac = transform.Find("imgTrungNguSac").transform.GetChild(0);
                    Transform vitribay = transform.Find("TrungNguSac");
                    Animator anim = spriteTrungNguSac.transform.parent.GetComponent<Animator>();
                    anim.SetFloat("speed", 2f);
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        ObjTiaSet.SetActive(true);
                        anim.transform.LeanScale(new Vector3(0.9f, 0.9f), 0.3f).setOnComplete(() =>
                        {
                            anim.transform.LeanScale(new Vector3(0.75f, 0.75f), 0.3f).setOnComplete(() =>
                            {

                            });
                        });
                        yield return new WaitForSeconds(0.5f);
                        ObjTiaSet.SetActive(false);

                        GameObject gspriteTrungNguSac = Instantiate(spriteTrungNguSac.gameObject, transform.position, Quaternion.identity);
                        gspriteTrungNguSac.transform.SetParent(CrGame.ins.trencung, false);
                        QuaBay quabay = gspriteTrungNguSac.AddComponent<QuaBay>();
                        quabay.vitribay = vitribay.gameObject;
                        gspriteTrungNguSac.SetActive(true);
                        EventManager.StartDelay2(() => { anim.SetFloat("speed", 1f); }, 1f);

                        duocdunghop = true;
                        transform.Find("TrungNguSac").transform.GetChild(1).GetComponent<Text>().text = json["TrungNguSac"].AsString;
                        LoadMocQua(json["solanquay"].AsInt);
                        Transform allTrung = transform.Find("allTrung");
                        string[] allitemtrung = new string[] { "TrungRongLua", "TrungRongBang", "TrungRongSam", "TrungRongDat", "TrungRongCay" };
                        for (int i = 0; i < allTrung.transform.childCount; i++)
                        {
                            Text txttrung = allTrung.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                            int trung = json[allitemtrung[i]].AsInt;
                            txttrung.text = (trung > 0) ? "<color=lime>" + trung + "/1</color>" : "<color=red>" + trung + "/1</color>";
                        }
                        StartCoroutine(DelayHienQua(json["allqua"],false));
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
    
    }
    int soluong = 1;
    int maxsoluong = 0;
    string trungChuyenDoi;
    public void OpenChuyenDoiTrungRong()
    {
        soluong = 1;
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        trungChuyenDoi = btnchon.transform.parent.name;
        maxsoluong = int.Parse(btnchon.transform.parent.transform.GetChild(1).GetComponent<Text>().text.Split("/")[0].Split(">")[1]);
        GameObject menuchuyendoi = transform.Find("PanelChuyenDoiTrungRong").gameObject;

        menuchuyendoi.transform.Find("imgtrung").GetComponent<Image>().sprite = btnchon.transform.parent.transform.GetChild(0).GetComponent<Image>().sprite;
        menuchuyendoi.transform.Find("txtnametrung").GetComponent<Text>().text = btnchon.transform.parent.transform.GetChild(2).GetComponent<Text>().text;
        menuchuyendoi.transform.Find("Vang").transform.GetChild(2).GetComponent<Text>().text = GamIns.FormatCash(giaChuyenDoiTrung);
        menuchuyendoi.transform.Find("txtsoluong").GetComponent<Text>().text = "1";
        menuchuyendoi.SetActive(true);
    }    
    public void TangSoLuong(int i)
    {
        soluong += i;
        if (soluong > maxsoluong) soluong = maxsoluong;
        else if (soluong <= 0) soluong = 1;
        GameObject menuchuyendoi = transform.Find("PanelChuyenDoiTrungRong").gameObject;
        menuchuyendoi.transform.Find("txtsoluong").GetComponent<Text>().text = soluong.ToString();
        menuchuyendoi.transform.Find("Vang").transform.GetChild(2).GetComponent<Text>().text = GamIns.FormatCash(giaChuyenDoiTrung * soluong);
    }

    public void ChuyenDoiTrung()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "ChuyenDoiTrung";
        datasend["data"]["nametrung"] = trungChuyenDoi;
        datasend["data"]["soluong"] = soluong.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                Transform PanelChuyenDoiTrungRong = transform.Find("PanelChuyenDoiTrungRong");
                Transform menudangchuyendoi = PanelChuyenDoiTrungRong.transform.Find("menudangchuyendoi");
                menudangchuyendoi.gameObject.SetActive(true);
                Image fill = menudangchuyendoi.transform.Find("fill").GetComponent<Image>();
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
                    StartCoroutine(DelayHienQua(json["allqua"]));
                    menudangchuyendoi.gameObject.SetActive(false);
                    PanelChuyenDoiTrungRong.gameObject.SetActive(false);
                    Transform allTrung = transform.Find("allTrung");
                    string[] allitemtrung = new string[] { "TrungRongLua", "TrungRongBang", "TrungRongSam", "TrungRongDat", "TrungRongCay" };
                    for (int i = 0; i < allTrung.transform.childCount; i++)
                    {
                        Text txttrung = allTrung.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                        int trung = json["allItem"][allitemtrung[i]].AsInt;
                        txttrung.text = (trung > 0) ? "<color=lime>" + trung + "/1</color>" : "<color=red>" + trung + "/1</color>";
                    }
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
     }    
    void UpdateTrung(string nametrung,int soluong)
    {
        if(nametrung == "TrungVoSac" || nametrung == "TrungNguSac") transform.Find(nametrung).transform.GetChild(1).GetComponent<Text>().text = soluong.ToString();
        else
        {
            Transform allTrung = transform.Find("allTrung");
            for (int i = 0; i < allTrung.transform.childCount; i++)
            {
                if(allTrung.transform.GetChild(i).name == nametrung)
                {
                    Text txttrung = allTrung.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    txttrung.text = (soluong > 0) ? "<color=lime>" + soluong + "/1</color>" : "<color=red>" + soluong + "/1</color>";
                    break;
                }
            }
        }
    }
    public Sprite danhansprite;
    JSONNode allmoc;
    private void SetAllMoc()
    {
        GameObject ScrollViewallMocQua = transform.Find("ScrollViewallMocQua").gameObject;
        GameObject content = ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 0; i < allmoc.Count - 1; i++)
        {
            if (i < allmoc.Count - 1)
            {
                Image img = content.transform.GetChild(i + 1).transform.Find("imgqua").GetComponent<Image>();
                img.transform.GetChild(0).GetComponent<Text>().text = allmoc[i].AsString;

            }
            else if (i == allmoc.Count - 1)
            {
                Image img2 = content.transform.GetChild(i).transform.Find("imgqua2").GetComponent<Image>();
                img2.transform.GetChild(0).GetComponent<Text>().text = allmoc[i].AsString;
            }
        }
    }
    private void LoadMocQua(int vong)
    {
        GameObject ScrollViewallMocQua = transform.Find("ScrollViewallMocQua").gameObject;
        GameObject content = ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(0).gameObject;
        int cong = 0;
        for (int i = 0; i < allmoc.Count; i++)
        {
            debug.Log("allmoc[i] " + allmoc[i]);
            Image fill = content.transform.GetChild(i).transform.Find("load").GetComponent<Image>();
            cong += allmoc[i].AsInt;
            if (vong >= allmoc[i].AsInt)
            {
                if (i < allmoc.Count - 1)
                {
                    Image img = content.transform.GetChild(i + 1).transform.Find("imgqua").GetComponent<Image>();
                    img.transform.GetChild(0).GetComponent<Text>().text = allmoc[i].AsString;
                    img.sprite = danhansprite;
                    img.SetNativeSize();

                }
                else if (i == allmoc.Count - 1)
                {
                    Image img2 = content.transform.GetChild(i).transform.Find("imgqua2").GetComponent<Image>();
                    img2.transform.GetChild(0).GetComponent<Text>().text = allmoc[i].AsString;
                    img2.sprite = danhansprite;
                    img2.SetNativeSize();
                }
                fill.fillAmount = 1;

                if (vong == allmoc[i].AsInt) break;
            }
            else
            {
                int max = allmoc[i].AsInt - allmoc[i - 1].AsInt;
                fill.fillAmount = (float)(vong - allmoc[i - 1].AsInt) / (float)max;
                debug.Log("chia " + (float)(vong - allmoc[i - 1].AsInt) / max);
                break;
            }

            //if (vong >= allmoc[i])
            //{
            //    fill.fillAmount = 1;
            //}
            //else
            //{
            //    fill.fillAmount = vong / allmoc[i];
            //    break;
            //}

        }

    }
    public void OpenShopTrung()
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
                ShopTrungNguSac GiaoDien = EventManager.ins.GetCreateMenu("ShopTrungNguSac",transform,false,EventManager.ins.btnHopQua.transform.GetSiblingIndex()).GetComponent<ShopTrungNguSac>();
             //       AllMenu.ins.transform.Find("ShopTrungNguSac").GetComponent<ShopTrungNguSac>();
             //   GiaoDien.gameObject.SetActive(true);
                GiaoDien.ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        EventManager.ins.OpenMenuDoiManhRong(namerong);
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
                SetYeuCau(json["YeuCau"]);
                transform.Find("TrungVoSac").transform.GetChild(1).GetComponent<Text>().text = json["TrungVoSac"].AsString;
                EventManager.ins.GetComponent<EventLacVaoRungTien>().SetCanhCamYeuCau(json["CanhCam"].AsInt, json["CanhCamYeuCau"].AsInt);
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
}
