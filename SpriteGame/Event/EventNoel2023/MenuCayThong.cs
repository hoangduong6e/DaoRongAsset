using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCayThong : MonoBehaviour
{
    public GameObject objtreotat, manden;
    public Sprite[] allTraiChau;
    // Start is called before the first frame update
    private MenuEventNoel2023 ev;
    public Sprite[] alltuchat;
    public Sprite sprite1, sprite2, chuaduocnhan,duocnhan,danhan;

    void Start()
    {
        SetMauRandom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ParseData(JSONNode json)
    {
        ev = EventManager.ins.GetComponent<MenuEventNoel2023>();
        SetTraiChauVaHopQua(json["TraiChau"].AsString, json["HopQuaGiangSinh"].AsString);
        LoadMocQuaGD1(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solantrangtricaythong"].AsInt);
        SetYeuCau(json["YeuCau"]);
    }
    private void SetMauRandom()
    {
        for (int i = 0; i < objtreotat.transform.childCount; i++)
        {
            objtreotat.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = allTraiChau[Random.Range(0, allTraiChau.Length - 1)];
        }
    }
    public void SetMauNhapNhay()
    {
        manden.SetActive(true);
        for (byte i = 0; i < objtreotat.transform.childCount - 1; i++)
        {
            StartCoroutine(delay2(i));
        }
        IEnumerator delay2(byte a)
        {
            for (int i = 0; i < 20; i++)
            {
                objtreotat.transform.GetChild(a).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = allTraiChau[Random.Range(0, allTraiChau.Length - 1)];
                yield return new WaitForSeconds(0.1f);
            }
            manden.SetActive(false);
        }
    }
    private bool boquaxacnhan = false;
    public void QuayMayMan(string solanquay)
    {
        AudioManager.PlaySound("soundClick");
        if (!boquaxacnhan)
        {
            GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        
            if (!btnchon.transform.Find("TraiChau").gameObject.activeSelf)
            {
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true, ev.giaodiennut1.transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                tbc.txtThongBao.text = "Không đủ trái châu, trang trí sẽ tốn Kim Cương\n<size=45>(Chỉ nhắc lần đầu)</size>";
                tbc.btnChon.onClick.AddListener(delegate { BoQuaXacNhan(solanquay); });
                return;
            }
        }    
       
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "TrangTriCayThong";
        datasend["data"]["solanquay"] = solanquay.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                StartCoroutine(DelayHienQua(json["allqua"]));
                SetYeuCau(json["YeuCau"]);
                SetTraiChauVaHopQua(json["TraiChau"].AsString, json["HopQuaGiangSinh"].AsString);
                LoadMocQuaGD1(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solantrangtricaythong"].AsInt);
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
        AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        QuayMayMan(solanquay);
    }
    void SetTraiChauVaHopQua(string traichau,string hopqua)
    {
        transform.Find("itemTraiChau").transform.GetChild(1).GetComponent<Text>().text = traichau;
        transform.Find("itemHopQua").transform.GetChild(1).GetComponent<Text>().text = hopqua;
    }
    void SetYeuCau(JSONNode yeucau)
    {
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

        GameObject btnX10 = transform.Find("btnquayX11").gameObject;
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
    public IEnumerator DelayHienQua(JSONNode allqua)
    {
        SetMauNhapNhay();
        yield return new WaitForSeconds(1.5f);
        //yield return new WaitForSeconds(0.4f);
        // JSONNode allqua = allquanhan["allqua"];
        GameObject panelnhanqua = ev.GetCreateMenu("PanelNhanQua", transform, true);
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
                for (int j = 0; j < ev.allitemEvent.Length; j++)
                {
                    if (ev.allitemEvent[j].name == allqua[i]["nameitem"].Value)
                    {
                        Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                        img.sprite = ev.allitemEvent[j];
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
        GameObject hopqua = ev.btnHopQua;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject panelnhanqua = ev.menuevent["PanelNhanQua"];
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
        GameObject ScrollViewallMocQua = transform.Find("ScrollViewallMocQua").gameObject;
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
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "NhanQuaTichLuyGd2";
        datasend["data"]["mocqua"] = btnnhan.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                LoadMocQuaGD1(json["QuaTichLuyGD2"], json["allMocDiemGD2"], json["solantrangtricaythong"].AsInt);
                transform.Find("itemHopQua").transform.GetChild(1).GetComponent<Text>().text = json["HopQuaGiangSinh"].AsString;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenMenuDoiManh()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "MenuDoiManh";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                GameObject menuDoiManh = ev.GetCreateMenu("MenuDoiManh", transform, false);
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
                        for (int j = 0; j < ev.allitemEvent.Length; j++)
                        {
                            if (json["ManhDoi"][i]["nameitem"].Value == ev.allitemEvent[j].name)
                            {
                                imgmanh.sprite = ev.allitemEvent[j];
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
        GameObject yeucau = ev.menuevent["MenuDoiManh"].transform.GetChild(1).gameObject;
        namemanhchon = btnchon.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "XemManhDoi";
        datasend["data"]["namemanh"] = namemanhchon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
           //ebug.Log(json.ToString());
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
                        if(json["hienthisoitemco"][json["manhdoi"]["itemcan"][i]["nameitem"].AsString].AsBool)
                        {
                            sonvok += 1;
                        }
                        if (json["duocdoi"].AsBool)
                        {
                            ev.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(true);
                            ev.menuevent["MenuDoiManh"].transform.GetChild(2).GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            ev.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(false);
                        }
                        //int itemcan = int.Parse(json["manhdoi"]["itemcan"][i]["soluong"].Value);

                        //int itemco = json["hienthisoitemco"][json["manhdoi"]["itemcan"][i]["nameitem"].AsString].AsInt; // int.Parse(json["allitemevent"][json["manhdoi"]["itemcan"][i]["nameitem"].Value].Value);
                        //if (itemco >= itemcan)
                        //{
                        //    sonvok += 1;
                        //    txtyeucau.text = "<color=#00ff00ff>" + itemco + "/" + itemcan + "</color>";
                        //}
                        //else txtyeucau.text = "<color=#ff0000ff>" + itemco + "/" + itemcan + "</color>";


                        //if (json["manhdoi"]["itemcan"][i]["loaiitem"].Value == "ItemEvent")
                        //{
                        //    int itemco = int.Parse(json[json["manhdoi"]["itemcan"][i]["nameitem"].Value].Value); // int.Parse(json["allitemevent"][json["manhdoi"]["itemcan"][i]["nameitem"].Value].Value);
                        //    if (itemco >= itemcan)
                        //    {
                        //        sonvok += 1;
                        //        txtyeucau.text = "<color=#00ff00ff>" + itemco + "/" + itemcan + "</color>";
                        //    }
                        //    else txtyeucau.text = "<color=#ff0000ff>" + itemco + "/" + itemcan + "</color>";
                        //}
                        //else if (json["manhdoi"]["itemcan"][i]["loaiitem"].Value == "ItemNgoc")
                        //{
                        //    for (var k = 0; k < NetworkManager.ins.inventory.contentNgoc.transform.childCount; k++)
                        //    {
                        //        if (NetworkManager.ins.inventory.contentNgoc.transform.GetChild(k).name == json["manhdoi"]["itemcan"][i]["nameitem"].Value)
                        //        {
                        //            int solixico = int.Parse(NetworkManager.ins.inventory.contentNgoc.transform.GetChild(k).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text);
                        //            if (solixico >= itemcan)
                        //            {
                        //                sonvok += 1;
                        //                txtyeucau.text = "<color=#00ff00ff>" + solixico + "/" + itemcan + "</color>";
                        //            }
                        //            else txtyeucau.text = "<color=#ff0000ff>" + solixico + "/" + itemcan + "</color>";
                        //            break;
                        //        }
                        //        else if (k == NetworkManager.ins.inventory.contentNgoc.transform.childCount - 1)
                        //        {
                        //            txtyeucau.text = "<color=#ff0000ff>0/" + itemcan + "</color>";
                        //        }
                        //    }
                        //}
                        //else
                        //{

                        //    if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + json["manhdoi"]["itemcan"][i]["nameitem"].Value))
                        //    {
                        //        int solixico = int.Parse(NetworkManager.ins.inventory.ListItemThuong["item" + json["manhdoi"]["itemcan"][i]["nameitem"].Value].transform.GetChild(0).GetComponent<Text>().text);
                        //        if (solixico >= itemcan)
                        //        {
                        //            sonvok += 1;
                        //            txtyeucau.text = "<color=#00ff00ff>" + solixico + "/" + itemcan + "</color>";
                        //        }
                        //        else txtyeucau.text = "<color=#ff0000ff>" + solixico + "/" + itemcan + "</color>";
                        //    }
                        //    else if(json["manhdoi"]["itemcan"][i]["nameitem"].Value == "KimCuong")
                        //    {
                        //        int solixico = int.Parse(CrGame.ins.txtKimCuong.text);
                        //        if (solixico >= itemcan)
                        //        {
                        //            sonvok += 1;
                        //            txtyeucau.text = "<color=#00ff00ff>" + solixico + "/" + itemcan + "</color>";
                        //        }
                        //        else txtyeucau.text = "<color=#ff0000ff>" + solixico + "/" + itemcan + "</color>";
                        //    }   
                        //    else txtyeucau.text = "<color=#ff0000ff>0/" + itemcan + "</color>";
                        //}
                    }
                }
            }
            //if (sonvok >= json["manhdoi"]["itemcan"].Count)
            //{
            //    ev.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(true);
            //    ev.menuevent["MenuDoiManh"].transform.GetChild(2).GetComponent<Button>().interactable = true;
            //}
            //else
            //{
            //    ev.menuevent["MenuDoiManh"].transform.GetChild(2).gameObject.SetActive(false);
            //}
            ev.menuevent["MenuDoiManh"].transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = json["manhdoi"]["thongtin"].Value;
        }
    }
    void DoiManh()
    {
        AudioManager.PlaySound("soundClick");
        //Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "DoiQua";
        datasend["data"]["namemanh"] = namemanhchon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //foreach (string key in json["allItem"].AsObject.Keys)
                //{
                //    SetItemEvent(key, json["allItem"][key].AsString);
                //}

                GameObject menudoimanh = ev.menuevent["MenuDoiManh"];
                menudoimanh.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = "";
                //   menudoimanh.transform.GetChild(1).gameObject.SetActive(false);
                menudoimanh.transform.GetChild(2).gameObject.SetActive(false);
                GameObject yeucau = menudoimanh.transform.GetChild(1).gameObject;
                for (int i = 0; i < yeucau.transform.childCount; i++)
                {
                    yeucau.transform.GetChild(i).gameObject.SetActive(false);
                }
                transform.Find("itemHopQua").transform.GetChild(1).GetComponent<Text>().text = json["HopQuaGiangSinh"].AsString;
                //  ev.menuevent["GiaoDien2"].transform.Find("itemDaMatTrang").GetChild(1).GetComponent<Text>().text = json["DaMatTrang"].AsString;
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString,2.5f);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void ExitDoiManh()
    {
        ev.DestroyMenu("MenuDoiManh");
    }
    public void OpenMenuQuaThangSaoTuanLong()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "GetGiaoDienThangSaoTuanLong";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                GameObject menuQuaThangSao = ev.GetCreateMenu("MenuNhanQuaThangSao", transform, false);
                menuQuaThangSao.transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(ExitMenuQuaNangSao);
                GameObject ContentManh = menuQuaThangSao.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;

                for (int i = 0; i < json["quathangsaotuanlong"].Count; i++)
                {
                    Button btn = ContentManh.transform.GetChild(i).transform.Find("btnNhanQua").GetComponent<Button>();
                    if (json["quathangsaotuanlong"][i].AsString == "chuaduocnhan")
                    {
                        btn.interactable = false;
                    }
                    else if (json["quathangsaotuanlong"][i].AsString == "duocnhan")
                    {
                        btn.interactable = true;
                        btn.onClick.AddListener(NhanQuaThangSaoTuanLong);
                    }
                    else if (json["quathangsaotuanlong"][i].AsString == "danhan")
                    {
                        btn.interactable = false;
                        btn.transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
                    }
                }
               // menuQuaThangSao.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DoiManh);
                menuQuaThangSao.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void NhanQuaThangSaoTuanLong()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "NhanQuaNhanTuanLong";
        datasend["data"]["vitri"] = objchon.transform.parent.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
               // GameObject menuQuaThangSao = ev.menuevent["MenuNhanQuaThangSao"];
              //  GameObject ContentManh = menuQuaThangSao.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;

                Button btn = objchon.GetComponent<Button>();
                btn.interactable = false;
                btn.transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
                CrGame.ins.OnThongBaoNhanh("Đã nhận!");
                // menuQuaThangSao.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DoiManh);
                //menuQuaThangSao.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

    }
    private void ExitMenuQuaNangSao()
    {
        ev.DestroyMenu("MenuNhanQuaThangSao");
    }    
    public void QuayVe()
    {
        ev.menuevent.Remove("PanelNhanQua");
        ev.DestroyMenu("MenuCayThong");
    }    
}
