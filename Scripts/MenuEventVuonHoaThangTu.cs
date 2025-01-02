using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEventVuonHoaThangTu : EventManager
{
    // Start is called before the first frame update
    Vector3 dragOrigin; Camera cam; SpriteRenderer imgMap;
    float MapMinX, MapMaxX, mapMiny, MapMaxY;CrGame crgame;
    public GameObject[] allodat, allonuoc, Xoaynuoc; public GameObject giaodiennut1,ngoisaoexp,dauchamthan;
    public RuntimeAnimatorController animHoaTrang, animHoaVang, animHoaChum;
    public Sprite imghoatrang, imghoavang, imghoachum; public GameObject MenuOHoa,ContentManh, ObjectManh;
    NetworkManager net;public Sprite spritephanbon,spriteXeng,spriteLongDenToi;float kinhnghiemphanbon;int sophanbonchon = 0;
    public bool drag = false;
    private byte current_garden = 0;// vườn hoa đang ở
    protected override void ABSAwake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        crgame = cam.GetComponent<CrGame>();
        net = crgame.GetComponent<NetworkManager>();
     
        btnHopQua = crgame.giaodien.transform.Find("btnQuaOnline").gameObject;
        SetMap(0);


       // AllMenu.ins.menu.Add("MenuEventVuonHoaThangTu",gameObject);
      //  Test();
    }
    private void SetMap(byte map)
    {
        GameObject img = transform.GetChild(0).transform.GetChild(0).gameObject;
        imgMap = img.transform.GetChild(map).GetComponent<SpriteRenderer>();
        MapMinX = imgMap.transform.position.x - imgMap.bounds.size.x / 2f;
        MapMaxX = imgMap.transform.position.x + imgMap.bounds.size.x / 2f;
        mapMiny = imgMap.transform.position.y - imgMap.bounds.size.y / 2f;
        MapMaxY = imgMap.transform.position.y + imgMap.bounds.size.y / 2f;
    }
    private void Test()
    {
        GameObject panel = transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject MapTrongCay2 = panel.transform.GetChild(1).gameObject;
        GameObject allONuoc2 = MapTrongCay2.transform.GetChild(0).gameObject;
        GameObject allodat = MapTrongCay2.transform.GetChild(1).gameObject;
        for (int i = 0; i < allONuoc2.transform.childCount;i++)
        {
            allONuoc2.transform.GetChild(i).gameObject.SetActive(false);
            allodat.transform.GetChild(i).gameObject.SetActive(false);
        }
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(4f);
            for (int i = 0; i < allONuoc2.transform.childCount; i++)
            {
                allodat.transform.GetChild(i).gameObject.SetActive(true);

                allONuoc2.transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                //yield return new WaitUntil(() => Input.GetKeyDown("space"));

                //yield return new WaitUntil(()=> Input.GetKeyUp("space"));
                
            }
        }
    }    
    protected override void DiemDanhOk(JSONNode json)
    {
        giaodiennut1.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = json["soxeng"].Value;
    }
    public void SetXeng(string s)
    {
        giaodiennut1.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = s;
    }
    public void SetLongDen(string s)
    {
        giaodiennut1.transform.GetChild(1).transform.GetChild(4).GetComponent<Text>().text = s;
    }
    public void ParseData(JSONNode json)
    {
        debug.Log(json.ToString());
        if (json["status"].AsString == "0")
        {
            GameObject trencung = GameObject.FindGameObjectWithTag("trencung").gameObject;
            giaodiennut1.transform.SetParent(trencung.transform);
            MenuOHoa.transform.SetParent(trencung.transform);
            giaodiennut1.transform.SetAsFirstSibling();
            MenuOHoa.transform.SetAsFirstSibling();
            gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("vienchinh").transform);
            giaodiennut1.transform.position = new Vector3(trencung.transform.position.x, trencung.transform.position.y, 0);
            MenuOHoa.transform.position = new Vector3(trencung.transform.position.x, trencung.transform.position.y, 0);
            if (net.friend.QuaNha) net.friend.GoHome();
            btnHopQua.transform.SetParent(giaodiennut1.transform);

            // AudioManager.SetSoundBg("nhacnoel");
            crgame.giaodien.SetActive(false);
            //  crgame.giaodien.transform.SetParent(transform);
            //for (int i = 0; i < net.loidai.objGiaoDienOff.Length; i++)
            //{
            //    net.loidai.objGiaoDienOff[i].SetActive(false);
            //}

            float exp = float.Parse(json["event"]["expvuonhoa"].Value);

            float maxexp = float.Parse(json["event"]["maxexpvuonhoa"].Value);

            giaodiennut1.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = json["event"]["levelvuonhoa"].AsString;

            giaodiennut1.transform.GetChild(0).transform.GetChild(5).GetComponent<Text>().text = exp + "/" + maxexp;
            Image imgmask = giaodiennut1.transform.GetChild(0).transform.GetChild(4).GetComponent<Image>();
            imgmask.fillAmount = exp / maxexp;

            SetXeng(json["event"]["soxeng"].Value);
            SetLongDen(json["event"]["longden"].Value);
            //   giaodiennut1.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = json["event"]["soxeng"].Value;
            //   giaodiennut1.transform.GetChild(1).transform.GetChild(4).GetComponent<Text>().text = json["event"]["longden"].Value;

            giaodiennut1.transform.GetChild(4).GetComponent<Text>().text = json["event"]["ngayexp"].Value;

            //Image imgBg = transform.GetChild(0).GetComponent<Image>();
            //if (crgame.NgayDem == "Ngay") imgBg.sprite = Resources.Load<Sprite>("GameData/EventTet2023/BGBanNgay");
            //else imgBg.sprite = Resources.Load<Sprite>("GameData/EventTet2023/BGBanDem");
            //AudioManager.SetSoundBg("GameData/EventTet2023/nhacnentet", true);
            luotfree = byte.Parse(json["event"]["luotmohoa"].AsString);

            crgame.giaodien.SetActive(false);
            Camera.main.orthographicSize = 5;
            cam.GetComponent<ZoomCamera>().enabled = false;
            GameObject img = transform.GetChild(0).transform.GetChild(0).gameObject;
            cam.transform.position = new Vector3(img.transform.position.x, img.transform.position.y, -10);
            crgame.AllDao.transform.Find("BGDao" + crgame.DangODao).gameObject.SetActive(false);
            for (int i = 0; i < json["event"]["allvuonhoa"].Count; i++)
            {
                for (int j = 0; j < json["event"]["allvuonhoa"][i].Count; j++)
                {
                    InfoCayTrong info = null;
                  
                  
                    if(i == 0)
                    {
                        info = new InfoCayTrong(j, json["event"]["allvuonhoa"][i][j]["name"].Value, json["event"]["allvuonhoa"][i][j]["exp"].Value, json["event"]["allvuonhoa"][0][j]["qua"]["nameitem"].Value, (byte)i,null);
                       
                    }
                    else
                    {
                        string truexp = "";
                        if (json["event"]["allvuonhoa"][i][j]["truexp"].ToString() != "") truexp = json["event"]["allvuonhoa"][i][j]["truexp"].AsString;
                        BoCanhCam boCanhCam = new BoCanhCam(json["event"]["allvuonhoa"][i][j]["namebo"].AsString, json["event"]["allvuonhoa"][i][j]["bocanhcam"].AsBool, json["event"]["allvuonhoa"][i][j]["txttime"].AsString, json["event"]["allvuonhoa"][i][j]["longden"].AsBool, truexp);
                        info = new InfoCayTrong(j, json["event"]["allvuonhoa"][i][j]["name"].Value, json["event"]["allvuonhoa"][i][j]["exp"].Value, json["event"]["allvuonhoa"][1][j]["qua"]["nameitem"].Value, (byte)i, boCanhCam);
                    }
                    Trong(info);

                }
            }

            LoadOHoa(json["event"]["allOhoa"]);
         
            if (json["event"]["QuaAi"]["name"].Value != "")
            {
                GameObject BongBong = MenuOHoa.transform.GetChild(6).transform.GetChild(0).gameObject;
                if (json["event"]["duocquaai"].AsBool) BongBong.GetComponent<Button>().interactable = true;
                BongBong.transform.GetChild(0).gameObject.SetActive(false);
                if (json["event"]["QuaAi"]["loaiitem"].Value == "Item")
                {
                    BongBong.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSprite(json["event"]["QuaAi"]["name"].Value);
                }
                else if (json["event"]["QuaAi"]["loaiitem"].Value == "ItemRong")
                {
                    BongBong.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["event"]["QuaAi"]["name"].Value + 1);
                }
                else if (json["event"]["QuaAi"]["loaiitem"].Value == "ItemEvent")
                {
                    if (json["event"]["QuaAi"]["name"].Value == "sophanbon") BongBong.transform.GetChild(1).GetComponent<Image>().sprite = spritephanbon;
                    if (json["event"]["QuaAi"]["name"].Value == "soxeng") BongBong.transform.GetChild(1).GetComponent<Image>().sprite = spriteXeng;
                }
                BongBong.transform.GetChild(1).gameObject.SetActive(true);
                BongBong.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                MenuOHoa.transform.GetChild(5).gameObject.SetActive(true);
            }
           // GameObject giaodien1 = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            //Animator animrongkhongtuoc = giaodien1.transform.GetChild(2).GetComponent<Animator>();
           // animrongkhongtuoc.SetInteger("TienHoa", 2);

            MenuOHoa.transform.GetChild(8).GetComponent<Text>().text = json["infomotuyet"].Value;

            MenuOHoa.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Vườn hoa:\n" + json["event"]["aihoa"].Value;
            MenuOHoa.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = json["event"]["mamcay"].Value;
            crgame.menulogin.SetActive(false);
        }
        else
        {
            crgame.OnThongBaoNhanh(json["message"].AsString);
            crgame.menulogin.SetActive(false);
            VeNha();
            //     AllMenu.ins.DestroyMenu("MenuEventNoel");
        }
    }
    int soO = 0;
    public void XemKhongTuoc()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung"),true,giaodiennut1.transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();
        tbc.txtThongBao.text = "<size=48>Bạn muốn thách đấu với " + "<color=#00ff00ff>Khổng Tước</color>?</size>\n<size=45>Mỗi ngày chỉ được tham gia 1 lần.</size>";
        tbc.btnChon.onClick.AddListener(DanhKhongTuoc);
    }    
    void DanhKhongTuoc()
    {
        crgame.panelLoadDao.SetActive(true);
        net.socket.Emit("soloKhongTuoc",JSONObject.CreateStringObject("danh+"));
        //gameObject.SetActive(false);
       // VeNha();
        //StartCoroutine(delay());
        //IEnumerator delay()
        //{
        //    yield return new WaitForSeconds(1);
           
        //}
       
    }
    public void OpenMenuOHoa()
    {
        Vector3 vec = crgame.AllDao.transform.Find("BGDao" + crgame.DangODao).transform.position;
        vec.z = -10;
        cam.transform.position = vec;
        MenuOHoa.SetActive(true);
        btnHopQua.transform.SetParent(MenuOHoa.transform.GetChild(0).transform);
        giaodiennut1.SetActive(false);
    }    
    public void ThoatOhoa()
    {
        cam.GetComponent<ZoomCamera>().enabled = false;
        GameObject img = transform.GetChild(0).transform.GetChild(0).gameObject;
        cam.transform.position = new Vector3(img.transform.position.x, img.transform.position.y, -10);
        giaodiennut1.SetActive(true);
        btnHopQua.transform.SetParent(giaodiennut1.transform);
        MenuOHoa.SetActive(false);
    }   
    void LoadOHoa(JSONNode json)
    {
        GameObject contentotuyet = MenuOHoa.transform.GetChild(1).gameObject;
        // var otuyet = 0;
        for (int j = 0; j < json.Count; j++)
        {
            GameObject ObjJ = contentotuyet.transform.GetChild(j).gameObject;
            for (int n = 0; n < json[j].Count; n++)
            {
              //  debug.Log(json[j][n].Value);
                if (json[j][n].Value == "chuamo")
                {
                    // contentotuyet.transform.GetChild(otuyet).GetComponent<Button>().enabled = false;
                    ObjJ.transform.GetChild(n).gameObject.SetActive(true);
                    if (ObjJ.transform.GetChild(n).transform.GetChild(0).GetComponent<Button>() == false)
                    {
                        Button btn = ObjJ.transform.GetChild(n).transform.GetChild(0).gameObject.AddComponent<Button>();
                        btn.onClick.AddListener(MoOHoa);
                    }
                   // Destroy(ObjJ.transform.GetChild(n).gameObject);
                   // ObjJ.transform.GetChild(n).GetComponent<Image>().sprite = spritetuyettoi;
                }
                else if (json[j][n].Value == "damo")
                {
                    ObjJ.transform.GetChild(n).gameObject.SetActive(false);
                }
            }
        }
    }
    bool nhanqua = true;
    byte luotfree = 0;
    private bool boquaxacnhan = false;
    public void MoOHoa()
    {
        GameObject tuyetchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (!boquaxacnhan)
        {
            if (luotfree <= 0)
            {
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", crgame.trencung.gameObject, true, transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                tbc.txtThongBao.text = "Không còn lượt miễn phí, tiếp tục sẽ tốn Kim Cương\n<size=45>(Chỉ nhắc lần đầu)</size>";
                tbc.btnChon.onClick.AddListener(delegate { 
                    BoQuaXacNhan(tuyetchon.transform.parent.transform.parent.GetSiblingIndex().ToString(), tuyetchon.transform.parent.GetSiblingIndex().ToString());
                    boquaxacnhan = true;
                    AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                });
                return;
            }
            else BoQuaXacNhan(tuyetchon.transform.parent.transform.parent.GetSiblingIndex().ToString(), tuyetchon.transform.parent.GetSiblingIndex().ToString());
        }
        else BoQuaXacNhan(tuyetchon.transform.parent.transform.parent.GetSiblingIndex().ToString(), tuyetchon.transform.parent.GetSiblingIndex().ToString());
        void BoQuaXacNhan(string hangtuyet, string otuyet)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "MoOHoa";
            datasend["data"]["hangtuyet"] = hangtuyet;
            datasend["data"]["otuyet"] = otuyet;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                debug.Log(json.ToString());
                if (json["status"].Value == "ok")
                {
                    // LoadOHoa(json["allOhoa"]);
                    tuyetchon.transform.parent.gameObject.SetActive(false);

                    MenuOHoa.transform.GetChild(8).GetComponent<Text>().text = json["infomotuyet"].Value;
                    //if (json["vitricanhcong"]["hangtuyet"].Value != "-1")
                    //{
                    //    GameObject congdichchuyen = menutuyet.transform.GetChild(5).gameObject;
                    //    if (congdichchuyen.gameObject.activeSelf == false)
                    //    {
                    //        congdichchuyen.transform.position = menutuyet.transform.GetChild(1).transform.GetChild(int.Parse(json["vitricanhcong"]["hangtuyet"].Value)).transform.GetChild(int.Parse(json["vitricanhcong"]["otuyet"].Value)).gameObject.transform.position;
                    //        congdichchuyen.SetActive(true);
                    //    }
                    //}
                    Text txthatgiong = MenuOHoa.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
                    txthatgiong.text = (int.Parse(txthatgiong.text) + 1) + "";
                    // debug.Log("check string " + json["QuaAi"]["name"].ToString());
                    luotfree = byte.Parse(json["luotmohoa"].AsString);
                    if (json["QuaAi"]["name"].Value != "")
                    {
                        GameObject panelqua = MenuOHoa.transform.GetChild(9).gameObject;
                        GameObject BongBong = MenuOHoa.transform.GetChild(6).transform.GetChild(0).gameObject;
                        if (nhanqua)
                        {
                            BongBong.GetComponent<Button>().interactable = true;
                            BongBong.transform.GetChild(0).gameObject.SetActive(false);
                            if (json["QuaAi"]["loaiitem"].Value == "Item")
                            {
                                panelqua.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSprite(json["QuaAi"]["name"].Value);
                                panelqua.transform.GetChild(2).GetComponent<Text>().text = "x" + json["QuaAi"]["soluong"].Value;
                                BongBong.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSprite(json["QuaAi"]["name"].Value);
                            }
                            else if (json["QuaAi"]["loaiitem"].Value == "ItemRong")
                            {
                                panelqua.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["QuaAi"]["name"].Value + 1);
                                panelqua.transform.GetChild(2).GetComponent<Text>().text = json["QuaAi"]["sao"].Value + " sao";
                                BongBong.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["QuaAi"]["name"].Value + 1);
                            }
                            else if (json["QuaAi"]["loaiitem"].Value == "ItemEvent")
                            {
                                if (json["QuaAi"]["name"].Value == "sophanbon") panelqua.transform.GetChild(1).GetComponent<Image>().sprite = spritephanbon;
                                if (json["QuaAi"]["name"].Value == "soxeng") panelqua.transform.GetChild(1).GetComponent<Image>().sprite = spriteXeng;
                                BongBong.transform.GetChild(1).GetComponent<Image>().sprite = panelqua.transform.GetChild(1).GetComponent<Image>().sprite;
                                panelqua.transform.GetChild(2).GetComponent<Text>().text = "x" + json["QuaAi"]["soluong"].Value;
                            }
                            panelqua.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                            panelqua.SetActive(true);


                            BongBong.transform.GetChild(1).gameObject.SetActive(true);
                            BongBong.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                            MenuOHoa.transform.GetChild(5).gameObject.SetActive(true);
                        }

                    }
                    //btnnhan.onClick.RemoveAllListeners();
                    //btnnhan.gameObject.SetActive(false);
                    //contentQua.transform.GetChild(sibling).transform.GetChild(1).gameObject.SetActive(false);
                    //contentQua.transform.GetChild(sibling).transform.GetChild(2).gameObject.SetActive(true);
                }
                else crgame.OnThongBaoNhanh(json["status"].Value);
            }
        }
    
    }
    public void QuaMan()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "QuaAi";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                GameObject BongBong = MenuOHoa.transform.GetChild(6).transform.GetChild(0).gameObject;
                GameObject panelqua = MenuOHoa.transform.GetChild(9).gameObject;
                BongBong.GetComponent<Button>().interactable = false;
                //  BongBong.transform.GetChild(0).gameObject.SetActive(true);
                // BongBong.transform.GetChild(1).gameObject.SetActive(false);
                //  MenuOHoa.transform.GetChild(5).gameObject.SetActive(false);

                MenuOHoa.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Vườn hoa:\n" + json["aihoa"].Value;
                LoadOHoa(json["allOhoa"]);
                nhanqua = true;
                crgame.OnThongBaoNhanh("Chúc mừng, bạn đã qua ải " + json["aihoa"].Value, 2);
                //btnnhan.onClick.RemoveAllListeners();
                //btnnhan.gameObject.SetActive(false);
                //contentQua.transform.GetChild(sibling).transform.GetChild(1).gameObject.SetActive(false);
                //contentQua.transform.GetChild(sibling).transform.GetChild(2).gameObject.SetActive(true);
            }
            else crgame.OnThongBaoNhanh(json["status"].Value);
        }
    }    
    public void TatPanelQua()
    {
        GameObject panelqua = MenuOHoa.transform.GetChild(9).gameObject;
        GameObject quabay = Instantiate(panelqua.transform.GetChild(1).gameObject,transform.position,Quaternion.identity);
        quabay.SetActive(false);
        quabay.transform.SetParent(giaodiennut1.transform.parent.transform,false);
        quabay.AddComponent<QuaBay>().vitribay = btnHopQua;
        panelqua.SetActive(false);
        quabay.SetActive(true);
        nhanqua = false;
    }
    public void OpenMenuBonPhan()
    {
        GameObject menubonphan = giaodiennut1.transform.GetChild(5).gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetPhanBon";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                menubonphan.transform.GetChild(3).GetComponent<Text>().text = json["sophanbon"].Value;
                menubonphan.transform.GetChild(5).GetComponent<Text>().text = "1";
                sophanbonchon = 1;
                float kphanbon = float.Parse(json["kinhnghiemphanbon"].Value) + 1;
                menubonphan.transform.GetChild(7).GetComponent<Image>().fillAmount = kphanbon / (float)300;
                kinhnghiemphanbon = kphanbon;
                menubonphan.transform.GetChild(8).GetComponent<Text>().text = "Bonus Exp:<color=lime> " + kphanbon + "%</color>";

                menubonphan.SetActive(true);
            }
            else
            {
                crgame.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void SuDungPhanBon()
    {
        GameObject menubonphan = giaodiennut1.transform.GetChild(5).gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "SuDungPhanBon";
        datasend["data"]["sophanbon"] = menubonphan.transform.GetChild(5).GetComponent<Text>().text;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                crgame.OnThongBaoNhanh("Bonus exp <color=lime>+" + sophanbonchon + "%</color>", 2);
                menubonphan.SetActive(false);
            }
            else
            {
                crgame.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void TangSoLuongPhanBon(int i)
    {
        GameObject menubonphan = giaodiennut1.transform.GetChild(5).gameObject;
       
        //int sophanbonchon = int.Parse(menubonphan.transform.GetChild(5).GetComponent<Text>().text);
        //if(sophanbonchon >= 1 && kinhnghiemphanbon < 300)
        //{

        //}
        if(i > 0)
        {
            Text txtphanbonDangCo = menubonphan.transform.Find("txtphanbonDangCo").GetComponent<Text>();
            int phanbondangco = int.Parse(txtphanbonDangCo.text);
            if (i > phanbondangco)
            {
                i = phanbondangco;
            }
            if (i > 300 - (int)kinhnghiemphanbon) i = 300 - (int)kinhnghiemphanbon;
        }




        sophanbonchon += i;
       // debug.Log("i " + i);
       
        if (sophanbonchon <= 0)
        {
            sophanbonchon = 1;
            return;
        }
        kinhnghiemphanbon += i;
        menubonphan.transform.GetChild(7).GetComponent<Image>().fillAmount = (float)kinhnghiemphanbon / (float)300;
        menubonphan.transform.GetChild(5).GetComponent<Text>().text = sophanbonchon.ToString();

        menubonphan.transform.GetChild(8).GetComponent<Text>().text = "Bonus Exp:<color=lime> " + kinhnghiemphanbon + "%</color>";
    }
    //public void XemMuaXeng()
    //{
    //    ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung"),true, giaodiennut1.transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
    //    tbc.btnChon.onClick.RemoveAllListeners();
    //    tbc.txtThongBao.text = "Mua thêm xẻng với giá " + "<color=#00ff00ff>199 kim cương</color> ?";
    //    tbc.btnChon.onClick.AddListener(MuaXeng);
    //}    
    //void MuaXeng()
    //{
    //    JSONClass datasend = new JSONClass();
    //    datasend["class"] = nameEvent;
    //    datasend["method"] = "MuaXeng";
    //    NetworkManager.ins.SendServer(datasend, Ok);
    //    void Ok(JSONNode json)
    //    {
    //        debug.Log(json.ToString());
    //        if (json["status"].Value == "ok")
    //        {
    //            giaodiennut1.transform.GetChild(2).gameObject.SetActive(false);
    //            giaodiennut1.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = json["soxeng"].Value;

    //        }
    //        else
    //        {
    //            crgame.OnThongBaoNhanh(json["status"].Value);
    //        }
    //        AllMenu.ins.CloseMenu("MenuXacNhan");
    //    }
    //}
    public void XemTrongCay()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        crgame.panelLoadDao.SetActive(true);
        if (allonuoc[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).GetComponent<Image>().enabled) // đã có cây
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "ThuHoach";
            datasend["data"]["vitri"] = objchon.transform.GetSiblingIndex().ToString();
            datasend["data"]["vuonhoa"] = current_garden.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].Value == "exp")
                {
                    crgame.OnThongBaoNhanh(json["thongbao"].Value, 2);
                    Image imgmask = giaodiennut1.transform.GetChild(0).transform.GetChild(4).GetComponent<Image>();
                    imgmask.fillAmount = float.Parse(json["exp"].Value) / float.Parse(json["maxexp"].Value);
                    giaodiennut1.transform.GetChild(0).transform.GetChild(5).GetComponent<Text>().text = json["exp"].Value + "/" + json["maxexp"].Value;
                    giaodiennut1.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = json["levelvuonhoa"].Value;
                    Destroy(allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("duocthuhoach").gameObject);
                    Transform Bo = allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("bocanhcam");
                    if (Bo != null)
                    {
                        Destroy(Bo.gameObject);
                    }
                    if (allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("truexp")) Destroy(allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("truexp").gameObject);

                    Transform longden = allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("imglongden");
                    if(longden != null)
                    {
                        if(longden.GetComponent<Animator>())
                        {
                            Destroy(longden.GetComponent<Animator>());
                            Image img = longden.GetComponent<Image>();
                            img.sprite = spriteLongDenToi;
                            img.SetNativeSize();

                            img.AddComponent<Button>().onClick.AddListener(ThapLongDen);
                        }
                     
                    }

                }
                else if (json["status"].Value == "Item")
                {
                    crgame.OnThongBaoNhanh(json["thongbao"].Value, 2);
                    Destroy(allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.GetChild(2).gameObject);
                }
                else if (json["status"].Value == "BatBo")
                {
                    crgame.OnThongBaoNhanh(json["thongbao"].Value, 2);
                    Destroy(allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("bocanhcam").gameObject);
                    if(json["truexp"].AsString != "0")
                    {
                        if (allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("truexp"))
                        {
                            allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("truexp").transform.GetChild(0).GetComponent<Text>().text = json["truexp"].AsString + " Exp";
                        }
                        else
                        {
                            GameObject truexp = Instantiate(LoadObjectResource("truexp"));
                            truexp.transform.SetParent(allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform, false);
                            truexp.transform.GetChild(0).GetComponent<Text>().text = json["truexp"].AsString + " Exp";
                            truexp.transform.position = new Vector3(allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.position.x + 0.5f, allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.position.y);
                            truexp.name = "truexp";
                        }
                    }
                    else if (allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("truexp")) Destroy( allodat[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.Find("truexp").gameObject);


                }
                else
                {
                    crgame.OnThongBaoNhanh(json["status"].Value);
                }
            }
        }
        else  // chưa trồng cây
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "XemGiaTrongCay";
            datasend["data"]["vitri"] = objchon.transform.GetSiblingIndex().ToString();
            datasend["data"]["vuonhoa"] = current_garden.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].Value == "ok")
                {
                    GameObject menuxacnhan = giaodiennut1.transform.GetChild(2).gameObject;
                    Text txtxacnhan = menuxacnhan.transform.GetChild(1).GetComponent<Text>();
                    txtxacnhan.text = json["thongtin"].Value;
                    menuxacnhan.SetActive(true);

                    GameObject panelqua = menuxacnhan.transform.GetChild(2).gameObject;

                    panelqua.transform.GetChild(1).gameObject.SetActive(false);
                    for (int i = 0; i < json["qua"].Count; i++)
                    {
                        Image imgqua = panelqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                        Text txtsoluong = panelqua.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

                        if (json["qua"][i]["loaiitem"].Value == "Item")
                        {
                            imgqua.sprite = Inventory.LoadSprite(json["qua"][i]["nameitem"].Value);
                            txtsoluong.text = json["qua"][i]["soluong"].Value;
                            imgqua.SetNativeSize();
                        }
                        else if (json["qua"][i]["loaiitem"].Value == "ItemRong")
                        {
                            imgqua.sprite = Inventory.LoadSpriteRong(json["qua"][i]["nameitem"].Value + 1);
                            txtsoluong.text = json["qua"][i]["sao"].Value + " sao";
                            imgqua.SetNativeSize();
                        }
                        else if (json["qua"][i]["loaiitem"].Value == "ItemEvent")
                        {
                            imgqua.sprite = GetSprite(json["qua"][i]["nameitem"].AsString);
                            txtsoluong.text = json["qua"][i]["soluong"].Value;
                            imgqua.SetNativeSize();
                        }
                        panelqua.transform.GetChild(i).gameObject.SetActive(true);
                    }

                    if (json["duocnhan"].Value == "true")
                    {
                        panelqua.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                        panelqua.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        panelqua.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                        panelqua.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else
                {
                    crgame.OnThongBaoNhanh(json["status"].Value);
                }
            }
        }
        
    }
    public void QuaMap2()
    {
        if (int.Parse(giaodiennut1.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text) >= 5)
        {
            SetMap(1);
            current_garden = 1;
            GameObject allvuonhoa = transform.GetChild(0).transform.GetChild(0).gameObject;
            allvuonhoa.transform.GetChild(0).gameObject.SetActive(false);
            allvuonhoa.transform.GetChild(1).gameObject.SetActive(true);
        }
        else crgame.OnThongBaoNhanh("Vườn hoa cần đạt cấp độ 5 để qua vườn hoa tiếp theo!");
    }    
    public void TrongCay()
    {
        byte vuonhoa = current_garden;
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "TrongCay";
        datasend["data"]["quachon"] = objchon.transform.parent.transform.GetSiblingIndex().ToString();
        datasend["data"]["vuonhoa"] = vuonhoa.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                // giaodiennut1.transform.GetChild(2).gameObject.SetActive(false);
                InfoCayTrong info = null;
                if (vuonhoa == 0)
                {
                    info = new InfoCayTrong(int.Parse(json["vitri"].Value), json["namehoa"].Value, "-1", "", vuonhoa, null);
                }   
                else
                {
                    BoCanhCam boCanhCam = new BoCanhCam(json["namebo"].AsString, json["bocanhcam"].AsBool, json["txttime"].AsString, json["longden"].AsBool,"");
                    info = new InfoCayTrong(int.Parse(json["vitri"].Value), json["namehoa"].Value, "-1", "", vuonhoa, boCanhCam);
                }
                 
                Trong(info);
                giaodiennut1.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = json["soxeng"].Value;
            }
            else
            {
                crgame.OnThongBaoNhanh(json["status"].Value, 2.5f);
            }
            // AllMenu.ins.CloseMenu("MenuXacNhan");
            GameObject menuxacnhan = giaodiennut1.transform.GetChild(2).gameObject;
            menuxacnhan.SetActive(false);
        }
        crgame.panelLoadDao.SetActive(true);
    }
    public class BoCanhCam
    {
        public string namebo;
        public bool bocanhcam;
        public string time;
        public bool longden;
        public string truexp;
        public BoCanhCam(string namebo, bool bocanhcam, string time, bool longden, string truexp)
        {
            this.namebo = namebo;
            this.bocanhcam = bocanhcam;
            this.time = time;
            this.longden = longden;
            this.truexp = truexp;
        }
    }
    public class InfoCayTrong
    {
        public int vitri;
        public string namehoa;
        public string exp;
        public string namequa;
        public byte vuonhoa;
        public BoCanhCam bocanhcam;
        public InfoCayTrong(int vitri, string namehoa, string exp, string namequa, byte vuonhoa, BoCanhCam bocanhcam)
        {
            this.vitri = vitri;
            this.namehoa = namehoa;
            this.exp = exp;
            this.namequa = namequa;
            this.vuonhoa = vuonhoa;
            this.bocanhcam = bocanhcam;
        }
    }
    void Trong(InfoCayTrong info)
    {
        GameObject onuoc = allonuoc[info.vuonhoa].transform.GetChild(info.vitri).gameObject;
        onuoc.GetComponent<Image>().enabled = true;
        if (onuoc.transform.childCount > 0)
        {
            for (int i = 0; i < onuoc.transform.childCount; i++)
            {
                onuoc.transform.GetChild(i).gameObject.SetActive(true);
                onuoc.transform.GetChild(i).GetComponent<Image>().enabled = true;
            }
       
        }
        GameObject odat = allodat[info.vuonhoa].transform.GetChild(info.vitri).gameObject;
        odat.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        string sodat = odat.GetComponent<Image>().sprite.name;
        if (sodat != "datvang")
        {
            Image img = odat.transform.GetChild(0).gameObject.GetComponent<Image>();
            if (info.namehoa == "HoaTrang")
            {
                img.sprite = imghoatrang;
                odat.transform.GetChild(0).gameObject.AddComponent<Animator>().runtimeAnimatorController = animHoaTrang;
            }
            else if (info.namehoa == "HoaVang")
            {
                img.sprite = imghoavang;
                odat.transform.GetChild(0).gameObject.AddComponent<Animator>().runtimeAnimatorController = animHoaVang;
            }
            else if (info.namehoa == "HoaChum")
            {
                img.sprite = imghoachum;
                odat.transform.GetChild(0).gameObject.AddComponent<Animator>().runtimeAnimatorController = animHoaChum;
            }
            img.color = new Color32(255, 255, 255, 255);
            img.SetNativeSize();
        }
        GameObject xoaynuoc = Instantiate(Xoaynuoc[info.vuonhoa], transform.position, Quaternion.identity);
        xoaynuoc.transform.SetParent(allonuoc[info.vuonhoa].transform);
        xoaynuoc.transform.position = new Vector3(odat.transform.position.x - 0.1f, odat.transform.position.y + 0.05f);
        xoaynuoc.SetActive(true);

        if(info.exp != "-1")
        {
            if (info.exp != "0")
            {
                GameObject expp = Instantiate(ngoisaoexp, transform.position, Quaternion.identity);
                expp.transform.SetParent(allodat[info.vuonhoa].transform.GetChild(info.vitri).transform);
                expp.transform.position = new Vector3(odat.transform.position.x, odat.transform.position.y + 1.2f);
                expp.SetActive(true);
                expp.name = "duocthuhoach";
            }
            else if(info.namequa != "")
            {
                GameObject chamthan = Instantiate(dauchamthan, transform.position, Quaternion.identity);
                chamthan.transform.SetParent(allodat[info.vuonhoa].transform.GetChild(info.vitri).transform);
                chamthan.transform.position = new Vector3(odat.transform.position.x - 0.4f, odat.transform.position.y);
                chamthan.SetActive(true);
                chamthan.name = "duocthuhoach";
            }
        }

        if(info.bocanhcam != null)
        {
            if(info.bocanhcam.bocanhcam && sodat != "datvang")
            {
                GameObject bocanhcam = Instantiate(LoadObjectResource("BoCanhCam"));
                bocanhcam.transform.SetParent(odat.transform);
                bocanhcam.GetComponent<Image>().sprite = GetSprite(info.bocanhcam.namebo);
                bocanhcam.transform.GetChild(0).GetComponent<Text>().text = info.bocanhcam.time;
                bocanhcam.transform.position = new Vector3(odat.transform.position.x,odat.transform.position.y+0.7f);
                bocanhcam.name = "bocanhcam";

            }
            if(info.bocanhcam.truexp != "")
            {
                debug.Log("tru expppp " + info.bocanhcam.truexp);
                if (int.Parse(info.bocanhcam.truexp) > 0)
                {
                    GameObject truexp = Instantiate(LoadObjectResource("truexp"));
                    truexp.transform.SetParent(odat.transform,false);
                    truexp.transform.GetChild(0).GetComponent<Text>().text = info.bocanhcam.truexp + " Exp";
                    truexp.transform.position = new Vector3(odat.transform.position.x+0.5f, odat.transform.position.y);
                    truexp.name = "truexp";
                }
            }    
            if(info.bocanhcam.longden)
            {
                if (sodat != "datvang")
                {
                    Image imglongden = odat.transform.Find("imglongden").GetComponent<Image>();
                    Animator anim = imglongden.AddComponent<Animator>();
                    anim.runtimeAnimatorController = animLongDen;
                    
                    RectTransform rectTransform = imglongden.rectTransform;

                    // Set the size of the RectTransform
                    rectTransform.sizeDelta = new Vector2(700, 528);
                    anim.Play("Idle");
                }
            
            }
            else if(sodat != "datvang")
            {
                Button btn = odat.transform.Find("imglongden").AddComponent<Button>();
                btn.onClick.AddListener(ThapLongDen);
            }    
        }
       // allodat[current_garden].transform.GetChild(vitri).gameObject.AddComponent<Button>();
      //  allodat[current_garden].transform.GetChild(vitri).GetComponent<Button>().onClick.AddListener(XemTrongCay);
        if (info.vitri < 41)
        {
            GameObject odatnew = allodat[info.vuonhoa].transform.GetChild(info.vitri + 1).gameObject;
            //odatnew.AddComponent<Button>();
            //odatnew.GetComponent<Button>().onClick.AddListener(XemTrongCay);
            odatnew.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if (odatnew.GetComponent<Image>().sprite.name != "datvang")
            {
                Image img = odatnew.transform.GetChild(0).gameObject.GetComponent<Image>();
                img.color = new Color32(255, 255, 255, 255);
                img.SetNativeSize();
            }
        }
    }
    public RuntimeAnimatorController animLongDen;
    private void ThapLongDen()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "ThapLongDen";
        datasend["data"]["vitri"] = btnchon.transform.parent.transform.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                Animator anim = btnchon.AddComponent<Animator>();
                anim.runtimeAnimatorController = animLongDen;
                anim.Play("animclick");
                RectTransform rectTransform = btnchon.GetComponent<Image>().rectTransform;

                // Set the size of the RectTransform
                rectTransform.sizeDelta = new Vector2(700, 528);
                Destroy(btnchon.GetComponent<Button>());
                SetLongDen(json["longden"].AsString);

            }
            else
            {
                crgame.OnThongBaoNhanh(json["status"].Value, 2.5f);
            }    
        }
    }    
    public void VeNha()
    {
        if(current_garden == 0)
        {
            giaodiennut1.transform.SetParent(gameObject.transform);
            Destroy(MenuOHoa);
            crgame.AllDao.transform.Find("BGDao"+crgame.DangODao).gameObject.SetActive(true);
            Vector3 vec = crgame.AllDao.transform.Find("BGDao" + crgame.DangODao).transform.position;
            vec.z = -10;
            cam.transform.position = vec;
            crgame.giaodien.SetActive(true);
            AudioManager.SetSoundBg("nhacnen0");
            btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
            //gameObject.SetActive(false);
            AllMenu.ins.DestroyMenu("MenuEventVuonHoaThangTu");
        }
        else
        {
            SetMap(0);
            Vector3 vec = transform.GetChild(0).transform.GetChild(0).transform.position;
            vec.z = -10;
            cam.transform.position = vec;
            current_garden = 0;
            GameObject allvuonhoa = transform.GetChild(0).transform.GetChild(0).gameObject;
            allvuonhoa.transform.GetChild(1).gameObject.SetActive(false);
            allvuonhoa.transform.GetChild(0).gameObject.SetActive(true);
        }
    }    
    // Update is called once per frame
    void Update()
    {
        if(drag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 difrence = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position = Clampcamera(cam.transform.position + difrence);
            }
        }    
    }
    public void Drag(bool b)
    {
        drag = b;
    }
    Vector3 Clampcamera(Vector3 targetPosition)
    {
        float CamHeight = cam.orthographicSize;
        float CamWidth = cam.orthographicSize * cam.aspect;
        float MinX = MapMinX + CamWidth;
        float MaxX = MapMaxX - CamWidth;
        float MinY = mapMiny + CamHeight;
        float MaxY = MapMaxY - CamHeight;
        float NewX = Mathf.Clamp(targetPosition.x, MinX, MaxX);
        float NewY = Mathf.Clamp(targetPosition.y, MinY, MaxY);
        return new Vector3(NewX, NewY, targetPosition.z);
    }
    public void ChonMam()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (soO == objchon.transform.GetSiblingIndex())
        {
            debug.Log("objchon" + objchon);
            allonuoc[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).GetComponent<Image>().enabled = true;
            if (allonuoc[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.childCount > 0)
            {
                allonuoc[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.GetChild(0).gameObject.SetActive(true);
                allonuoc[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
            if(objchon.GetComponent<Image>().sprite.name != "datvang")
            {
                Image img = objchon.transform.GetChild(0).gameObject.GetComponent<Image>();
                img.sprite = imghoatrang;
                img.SetNativeSize();
                objchon.transform.GetChild(0).gameObject.AddComponent<Animator>().runtimeAnimatorController = animHoaTrang;
            }

            GameObject xoaynuoc = Instantiate(Xoaynuoc[current_garden],transform.position,Quaternion.identity);
            xoaynuoc.transform.SetParent(allonuoc[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).transform);
            xoaynuoc.transform.position = new Vector3(objchon.transform.position.x - 0.1f, objchon.transform.position.y + 0.05f);
            xoaynuoc.SetActive(true);

            soO += 1;
        }    
      //  allonuoc[current_garden].transform.GetChild(objchon.transform.GetSiblingIndex()).gameObject.SetActive(true);
    }
    int trang = 1;
    public void XemBXH()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemBXH";
        datasend["data"]["trang"] = trang.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = GetCreateMenu("MenuTop", crgame.trencung.transform, false, transform.GetSiblingIndex() + 1);
                //GameObject menu = CrGame.ins.trencung.transform.Find("MenuTop").gameObject;
                //    menuevent["MenuTop"] = menu;
                // GameObject menu = GetCreateMenu("MenuTop",gameObject,false,3);
                GameObject menutop = menu.transform.GetChild(0).transform.Find("MenuTop").gameObject;
                if (json["alltop"].Count > 0)
                {
                  

                    GameObject contentop = menutop.transform.GetChild(0).gameObject;
            
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
                            int sotop = json["alltop"][i]["top"].AsInt;
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
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["levelvuonhoa"].AsString;
                            Text txtexp = contentop.transform.GetChild(i).transform.GetChild(8).GetComponent<Text>(); txtexp.text = json["alltop"][i]["expvuonhoa"].AsString;
                            // debug.Log("ok5.1");

                            contentop.transform.GetChild(i).gameObject.SetActive(true);

                            //   debug.Log("ok5.2");
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

             //   GameObject mntop = menu.transform.GetChild(0).transform.GetChild(3).gameObject;
                menu.transform.GetChild(0).transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate {
                    DestroyMenu("MenuTop");
                    trang = 1;
                });
                Button btn = menutop.transform.GetChild(1).GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(delegate { sangtrangtop(1); });

                Button btn2 = menutop.transform.GetChild(2).GetComponent<Button>();
                btn2.onClick.RemoveAllListeners();
                btn2.onClick.AddListener(delegate { sangtrangtop(-1); });

                menu.SetActive(true);
                if (LoginFacebook.ins.NameServer.Split(':')[1] == "4567") //sv2 sv3
                {
                    menu.transform.GetChild(0).transform.Find("MenuPhanThuong").transform.GetChild(0).GetComponent<Image>().sprite = EventManager.ins.GetSprite("bxhtop20");
                }

            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
        }
        void sangtrangtop(int i)
        {
            AudioManager.PlaySound("soundClick");
            if (trang + i >= 1) trang += i;
            else return;
            XemBXH();
        }
    }
    public void OpenMenuNhatKi()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetNhatKi";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                // GameObject menu = CrGame.ins.trencung.transform.Find("GiaoDienRuongThanBi").gameObject;
                GameObject menu = GetCreateMenu("MenuNhatKi", CrGame.ins.trencung.transform,false,transform.GetSiblingIndex()+1);
                menu.GetComponent<MenuNhatKyVuonHoa>().ParseData(json);

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenMenuNhiemvu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetNhiemVu";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            //debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", CrGame.ins.trencung, true, transform.GetSiblingIndex() + 1);
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
                        txttiendo.text = "<color=#00ff00ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
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
    public void OpenMenuCheTao()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDataGDCheTaoLongDen";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
               //  GameObject menu = CrGame.ins.trencung.transform.Find("MenuCheTaoLongDen").gameObject;
               GameObject menu = GetCreateMenu("MenuCheTaoLongDen", CrGame.ins.trencung.transform, false, transform.GetSiblingIndex() + 1);
                menu.GetComponent<GiaoDienCheTaoLongDen>().ParseData(json);

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
            }
        }
    }
    public void OpenMenuMuaXeng()
    {
        GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaItemCheTao", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        g.transform.Find("txttanggia").GetComponent<Text>().text = "";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = "Mua xẻng";// tên giao diện
        Image imgitem = g.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        imgitem.sprite = EventManager.ins.GetSprite("soxeng");
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
        debug.Log("Tang so luong " + i);
        if (soluongMuaQueThu + i >= 1)
        {
            GameObject menu = EventManager.ins.menuevent["MenuMuaItemCheTao"];
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
        GameObject menu = EventManager.ins.menuevent["MenuMuaItemCheTao"];
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
        datasend["method"] = "XemGiaMuaXeng";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaItemCheTao"))
                {
                    GameObject menu = EventManager.ins.menuevent["MenuMuaItemCheTao"];
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
        datasend["method"] = "MuaXeng";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaItemCheTao"))
                {
                    ExitMenuQueThu();
                    SetXeng(json["soxeng"].AsString);
                    crgame.OnThongBaoNhanh("Mua thành công!");
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
        EventManager.ins.DestroyMenu("MenuMuaItemCheTao"); soluongMuaQueThu = 1;
    }
}
