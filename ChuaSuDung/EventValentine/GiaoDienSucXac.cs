using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GiaoDienSucXac : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] spriteXucXac;
    public Sprite bongbongSang, bongbongToi,Odentoi, Odensang,Ovangtoi,Ovangsang;
    private bool duocquay = true;
    private byte odangdung = 0;
    public Sprite[] alltuchat;
    public Sprite danhansprite;
    byte[] allmoc = new byte[] {1, 3, 6, 10, 15, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200};
    public void ParseData(JSONNode json)
    {
        debug.Log(json.ToString());
        SetsucXac(json["sucxac"].AsString);
        //Text txtsucXac = transform.Find("txtSucXac").GetComponent<Text>();
        //txtsucXac.text = "Bạn đang có: <color=yellow>" + json["sucxac"].AsString + "</color>";
        Text txtVongHienTai = transform.Find("txtVongHienTai").GetComponent<Text>();
        txtVongHienTai.text = "Vòng hiện tại: <color=yellow>" + json["GiaoDienSucXac"]["Vong"].AsString + "</color>";
        Transform ObjOquay = transform.Find("OQuay");
        int vitri = json["GiaoDienSucXac"]["vitri"].AsInt;
        odangdung = (byte)vitri;
        LoadMocQua( json["GiaoDienSucXac"]["Vong"].AsInt);
        if(vitri > 0)
        {
            SetImgSang(ObjOquay.transform.GetChild(vitri-1).GetComponent<Image>());
        }
        gameObject.SetActive(true);
    }
    private void SetsucXac(string sucxac)
    {
        Text txtsucXac = transform.Find("txtSucXac").GetComponent<Text>();
        txtsucXac.text = "Bạn đang có: <color=yellow>" + sucxac + "</color>";
    }    
    private void LoadMocQua(int vong)
    {
        GameObject ScrollViewallMocQua = transform.Find("ScrollViewallMocQua").gameObject;
        GameObject content = ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(0).gameObject;
        int cong = 0;
        for (int i = 0; i < allmoc.Length; i++)
        {
            debug.Log("allmoc[i] " + allmoc[i]);
            Image fill = content.transform.GetChild(i).transform.Find("load").GetComponent<Image>();
            cong += allmoc[i];
            if (vong >= allmoc[i])
            {
                if (i < allmoc.Length - 1)
                {
                    Image img = content.transform.GetChild(i + 1).transform.Find("imgqua").GetComponent<Image>();
                    img.sprite = danhansprite;
                    img.SetNativeSize();
                 
                }
                else if (i == allmoc.Length - 1)
                {
                    Image img2 = content.transform.GetChild(i).transform.Find("imgqua2").GetComponent<Image>();
                    img2.sprite = danhansprite;
                    img2.SetNativeSize();
                }
                fill.fillAmount = 1;
               
                if (vong == allmoc[i]) break;
            }
            else
            {
                if (i > 0)
                {
                    int max = allmoc[i] - allmoc[i - 1];
                    fill.fillAmount = (float)(vong - allmoc[i - 1]) / (float)max;
                    debug.Log("chia " + (float)(vong - allmoc[i - 1]) / max);
                }

                else fill.fillAmount = 0;
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
    public void Quay(string x)
    {
        if (!duocquay) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "TungSucXac";
        datasend["data"]["solanquay"] = x;
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                duocquay = false;
                int sucXactungra = json["congsucxac"].AsInt;
               // int sucXactungra = 9;
                GameObject xucxacanim = transform.Find("xucxacanim").gameObject;
                xucxacanim.SetActive(true);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(1.05f);
                    xucxacanim.SetActive(false);
                    Image imgsucXac = transform.Find("imgSucXac").GetComponent<Image>();
                  
                    Transform ObjOquay = transform.Find("OQuay");
                    if(json["allqua"].Count <= 1)
                    {
                        if (sucXactungra <= 6) imgsucXac.sprite = spriteXucXac[sucXactungra - 1];
                        for (int i = 0; i < sucXactungra; i++)
                        {
                            if (odangdung - 1 >= 0 && odangdung - 1 < ObjOquay.transform.childCount)
                            {
                                SetImgToi(ObjOquay.transform.GetChild(odangdung - 1).GetComponent<Image>());
                            }
                            else SetImgToi(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 1).GetComponent<Image>());
                            //if (odangdung >= 0 && odangdung < ObjOquay.transform.childCount)
                            //{

                            //}
                            if (odangdung >= 0 && odangdung < ObjOquay.transform.childCount)  SetImgSang(ObjOquay.transform.GetChild(odangdung).GetComponent<Image>());
                            odangdung += 1;
                            yield return new WaitForSeconds(0.3f);
                            if (odangdung >= ObjOquay.transform.childCount)
                            {
                                odangdung = 0;
                                SetImgToi(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 2).GetComponent<Image>());
                                // SetImgToi(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 1).GetComponent<Image>());
                                SetImgSang(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 1).GetComponent<Image>());
                                yield return new WaitForSeconds(0.3f);
                            }
                       
                        }
                    }    
                    else
                    {
                        int count = json["allqua"].Count - 1;
                        imgsucXac.sprite = spriteXucXac[json["allqua"][count]["sucxac"].AsInt - 1];
                        int vitri = json["vitri"].AsInt;
                        int i = odangdung;
                        if (i == vitri) i += 1;
                        SetImgToi(ObjOquay.transform.GetChild(odangdung).GetComponent<Image>());
                        while (i != vitri)
                        {
                            if (i - 1 >= 0 && i - 1 < ObjOquay.transform.childCount)
                            {
                                SetImgToi(ObjOquay.transform.GetChild(i - 1).GetComponent<Image>());
                            }
                            else SetImgToi(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 1).GetComponent<Image>());
                            //if (odangdung >= 0 && odangdung < ObjOquay.transform.childCount)
                            //{

                            //}
                            if (i >= 0 && i < ObjOquay.transform.childCount)  SetImgSang(ObjOquay.transform.GetChild(i).GetComponent<Image>());
                            i += 1;
                            yield return new WaitForSeconds(0.3f);
                            if (i >= ObjOquay.transform.childCount)
                            {
                               i = 0;
                               SetImgToi(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 2).GetComponent<Image>());
                               // SetImgToi(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 1).GetComponent<Image>());
                               SetImgSang(ObjOquay.transform.GetChild(ObjOquay.transform.childCount - 1).GetComponent<Image>());
                                // break;
                            }    
                         
                        }
                        odangdung = (byte)i;
                    }
                    
                    if (odangdung != 10)
                    {
                        duocquay = true; 
                        StartCoroutine(DelayHienQua(json["allqua"]));
                    }
                    else
                    {
                        //    string[] ketqua = { "traitimphale", "sucxac", "luibuoc" };
                        // string ketquarandom = ketqua[Random.Range(0, ketqua.Length)];
                        OpenMenuRandom(json["namethebairandom"].AsString, json["allqua"]);
                    }
             
                    yield return new WaitUntil(()=>duocquay);
                    Text txtsucXac = transform.Find("txtSucXac").GetComponent<Text>();
                    txtsucXac.text = "Bạn đang có: <color=yellow>" + json["sucxac"].AsString + "</color>";
                    Text txtVongHienTai = transform.Find("txtVongHienTai").GetComponent<Text>();
                    txtVongHienTai.text = "Vòng hiện tại: <color=yellow>" + json["Vong"].AsString + "</color>";
                }
                LoadMocQua(json["Vong"].AsInt);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
       
    }    
    private void OpenMenuRandom(string kq,JSONNode json)
    {
        int status = 0;
        Animator anim = transform.Find("random gd2").GetComponent<Animator>();
        if (kq == "sucxac") status = 1;
        else if (kq == "luibuoc")
        {
            status = 2;
        }
        anim.gameObject.SetActive(true);
        anim.SetInteger("trangthai", status);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(7);
            anim.gameObject.SetActive(false);
            if(kq != "luibuoc")
            {
                StartCoroutine(DelayHienQua(json));
                duocquay = true;
            }
            else
            {
                Transform ObjOquay = transform.Find("OQuay");
                for (int i = odangdung; i > 3; i--)
                {
                    SetImgSang(ObjOquay.transform.GetChild(odangdung - 1).GetComponent<Image>());

                    SetImgToi(ObjOquay.transform.GetChild(odangdung).GetComponent<Image>());
                    odangdung -= 1;
                    yield return new WaitForSeconds(0.3f);
                }
                StartCoroutine(DelayHienQua(json));
                duocquay = true;
            }
        }
    }    
    private void SetImgSang(Image img)
    {
        if(img.sprite.name == "bongbongtoi")
        {
            img.sprite = bongbongSang;
        }
        else if (img.sprite.name == "odentoi")
        {
            img.sprite = Odensang;
        }
        else if (img.sprite.name == "ovangtoi")
        {
            img.sprite = Ovangsang;
        }
        RectTransform rect = img.GetComponent<RectTransform>();
   

        img.SetNativeSize();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x / 1.3f, rect.sizeDelta.y / 1.3f);
    }
    private void SetImgToi(Image img)
    {
        if (img.sprite.name == "bongbongsang")
        {
            img.sprite = bongbongToi;
        }
        else if (img.sprite.name == "odensang")
        {
            img.sprite = Odentoi;
        }
        else if (img.sprite.name == "ovangsang")
        {
            img.sprite = Ovangtoi;
        }
        RectTransform rect = img.GetComponent<RectTransform>();
        img.SetNativeSize();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x / 1.3f, rect.sizeDelta.y / 1.3f);
    }

    public IEnumerator DelayHienQua(JSONNode allqua)
    {
        //yield return new WaitForSeconds(0.4f);
        // JSONNode allqua = allquanhan["allqua"];
        if (allqua.Count == 0) yield break;
        GameObject panelnhanqua = transform.Find("PanelNhanQua").gameObject;
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
            if (allqua[i]["itemgi"].AsString == "item")
            {
                //if(!themqua)
                //{
                //    themqua = true;
                //    EventManager.ins.btnHopQua.GetComponent<HopQua>().Them1Qua();
                //}
                // if (objtrong.transform.Find("vien")) Destroy(objtrong.transform.Find("vien"));
                objtrong.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(allqua[i]["nameitem"].Value);
                objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                objtrong.transform.GetChild(2).GetComponent<Image>().sprite = spriteXucXac[allqua[i]["sucxac"].AsInt - 1];
                objtrong.name = "item" + allqua[i]["nameitem"].Value;
                objtrong.GetComponent<infoitem>().enabled = true;
            }
            else if (allqua[i]["itemgi"].AsString == "itemevent")
            {
                Image img = objtrong.transform.GetChild(0).GetComponent<Image>();
                img.sprite = EventManager.ins.GetSprite(allqua[i]["nameitem"].AsString);
                // img.SetNativeSize();
                objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                objtrong.GetComponent<infoitem>().enabled = true;
                objtrong.transform.GetChild(2).GetComponent<Image>().sprite = spriteXucXac[allqua[i]["sucxac"].AsInt - 1];
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
    public void XacNhanQua()
    {
        AudioManager.PlaySound("soundClick");
        GameObject hopqua = EventManager.ins.btnHopQua;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject panelnhanqua = transform.Find("PanelNhanQua").gameObject;
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
            QuaBay quabay = quatrong.AddComponent<QuaBay>();
            quabay.vitribay = hopqua;
            qua.SetActive(false);
            //  Destroy(qua, 5);
        }
    }

    public void OpenGiaoDienChucPhuc()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "GetGiaoDienChucPhuc";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
               // GameObject menu = CrGame.ins.trencung.transform.Find("GiaoDienChucPhuc").gameObject;
                 GameObject menu = EventManager.ins. GetCreateMenu("GiaoDienChucPhuc", CrGame.ins.trencung.transform,true,transform.GetSiblingIndex() + 1);
                menu.GetComponent<GiaoDienChucPhuc>().ParseData(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }



    short soluongMuaQueThu = 1;
    public void OpenMenuMuaQueThu()
    {
        GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaQueThuTinhYeu", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        g.transform.Find("txttanggia").GetComponent<Text>().text = "";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = "Xúc Xắc";
        Image imgitem = g.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        imgitem.sprite = EventManager.ins.GetSprite("sucxac");
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
    private void ExitMenuQueThu()
    {
        EventManager.ins.DestroyMenu("MenuMuaQueThuTinhYeu"); soluongMuaQueThu = 1;
    }
    private void CongThemSoLuongMua(int i)
    {
        debug.Log("Tang so luong " + i);
        if (soluongMuaQueThu + i >= 1)
        {
            GameObject menu = EventManager.ins.menuevent["MenuMuaQueThuTinhYeu"];
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
        GameObject menu = EventManager.ins.menuevent["MenuMuaQueThuTinhYeu"];
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
        datasend["method"] = "XemGiaMuaSucXac";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaQueThuTinhYeu"))
                {
                    GameObject menu = EventManager.ins.menuevent["MenuMuaQueThuTinhYeu"];
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
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "MuaSucXac";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaQueThuTinhYeu"))
                {
                    ExitMenuQueThu();
                    SetsucXac(json["sucxac"].AsString);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void ExitMenu()
    {
        EventManager.ins.DestroyMenu("GiaoDienXucSac");
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
    }
}
