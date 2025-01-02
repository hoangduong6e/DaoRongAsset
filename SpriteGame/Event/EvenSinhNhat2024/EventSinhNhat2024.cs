using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EventSinhNhat2024 : EventManager
{
    Vector3 dragOrigin; Camera cam; SpriteRenderer imgMap;
    float MapMinX, MapMaxX, mapMiny, MapMaxY;
    bool drag;
    public GameObject giaodiennut1;
    float timeSec = 0; public Text txttime; bool dem = false;
    public byte vitrichon; bool hoanthanh = true;
    public static EventSinhNhat2024 ins;
    public GameObject btnHuy;
    protected override void ABSAwake()
    {
        ins = this;
    }
    protected override void DiemDanhOk(JSONNode json)
    {
        debug.Log(json.ToString());
    }
    public void ParseData(JSONNode json)
    {
        debug.Log(json.ToString());

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;

        imgMap = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
        MapMinX = imgMap.transform.position.x - imgMap.bounds.size.x / 2f;
        MapMaxX = imgMap.transform.position.x + imgMap.bounds.size.x / 2f;
        mapMiny = imgMap.transform.position.y - imgMap.bounds.size.y / 2f;
        MapMaxY = imgMap.transform.position.y + imgMap.bounds.size.y / 2f;

        if (Friend.ins.QuaNha) Friend.ins.GoHome();
        Text txtTheLuc = giaodiennut1.transform.Find("TheLuc").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        txtTheLuc.text = json["data"]["theluc"].Value;
        GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        LoadMocNv(json["data"]["infomocnv"]);
     //   LoadDiemDanh(json);
        //GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
        //Text soluongthiephong = menuchonthiep.transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
        //Text soluongthiepvang = menuchonthiep.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
        //soluongthiephong.text = json["data"]["ThiepHong"].Value;
        //soluongthiepvang.text = json["data"]["ThiepVang"].Value;

        GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
        Transform child = alloQua.transform.GetChild(int.Parse(json["data"]["odanglam"].AsString) - 1);
        YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Yêu cầu mở đường mốc thứ " + child.name;
        if(json["data"]["mocnhiemvu"].Count > 0)
        {
            btnHuy.transform.SetParent(child.transform, false);
            btnHuy.transform.position = child.transform.position;
            btnHuy.SetActive(true);
        }    
    
        LoadNhiemVu(json["data"]["mocnhiemvu"]);
        GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
        allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;
  //      debug.Log("batdaunhiemvu " + json["data"]["batdaunhiemvu"].AsString);
        if (json["data"]["batdaunhiemvu"].AsString == "true")
        {
            txttime.transform.GetChild(0).gameObject.SetActive(false);
            txttime.transform.GetChild(1).gameObject.SetActive(false);
            txttime.transform.GetChild(2).gameObject.SetActive(true);
            timeSec = json["data"]["sec"].AsFloat;
            txttime.transform.GetChild(2).GetComponent<Button>().interactable = false;
            dem = true;
        }
     
        GameObject trencung = GameObject.FindGameObjectWithTag("trencung").gameObject;
        giaodiennut1.transform.SetParent(trencung.transform);
        giaodiennut1.transform.SetAsFirstSibling();
        giaodiennut1.transform.position = Vector3.zero;

        giaodiennut1.transform.localScale = Vector3.one;
        RectTransform rectTransform = giaodiennut1.transform.GetComponent<RectTransform>();
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Đặt giá trị Pos Z bằng 0
        Vector3 currentPos = rectTransform.localPosition;
        currentPos.z = 0;
        rectTransform.localPosition = currentPos;

        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("vienchinh").transform);
        //  giaodiennut1.transform.position = new Vector3(trencung.transform.position.x, trencung.transform.position.y, 0);
        btnHopQua.transform.SetParent(trencung.transform);

        CrGame.ins.giaodien.SetActive(false);

        Camera.main.orthographicSize = 5;
        cam.GetComponent<ZoomCamera>().enabled = false;
        GameObject img = gameObject.transform.GetChild(0).gameObject;
        cam.transform.position = new Vector3(img.transform.position.x, img.transform.position.y, -10);

        Transform dangodao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao);
        dangodao.gameObject.SetActive(false);
        //CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        CrGame.ins.menulogin.SetActive(false);
        AudioManager.SetSoundBg("nhacnen1");
    }
    void LoadNhiemVu(JSONNode json)
    {
        GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
        if (json.Count == 0)
        {
            YeuCau.SetActive(false);
            return;
        }
        YeuCau.SetActive(true);
        GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;

        for (int i = 0; i < allnhiemvu.transform.childCount - 1; i++)
        {
            allnhiemvu.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < json.Count; i++)
        {
            Text txtnhiemvu = allnhiemvu.transform.GetChild(i).GetComponent<Text>();
            txtnhiemvu.name = json[i]["key"].AsString;
            txtnhiemvu.text = json[i]["tiendo"].AsString;
            txtnhiemvu.gameObject.SetActive(true);
            if (json[i]["xong"].AsString == "daxong")
            {
                txtnhiemvu.transform.GetChild(0).gameObject.SetActive(false);
                txtnhiemvu.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    public void MoQua()
    {
        if (!hoanthanh) return;
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemQua";
        datasend["data"]["vitri"] = objchon.transform.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menuxacnhan = giaodiennut1.transform.Find("menuchonqua").gameObject;
                Text txtxacnhan = menuxacnhan.transform.GetChild(1).GetComponent<Text>();
                txtxacnhan.text = json["thongtin"].Value;
                menuxacnhan.SetActive(true);

                Image imgqua = menuxacnhan.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
                Text txtsoluong = imgqua.transform.GetChild(0).GetComponent<Text>();

                if (json["qua"]["loaiitem"].Value == "Item")
                {
                    imgqua.sprite = Inventory.LoadSprite(json["qua"]["nameitem"].Value);
                    txtsoluong.text = json["qua"]["soluong"].Value;
                    imgqua.SetNativeSize();
                }
                else if (json["qua"]["loaiitem"].Value == "ItemRong")
                {
                    imgqua.sprite = Inventory.LoadSpriteRong(json["qua"]["nameitem"].Value + 1);
                    txtsoluong.text = json["qua"]["sao"].Value + " sao";
                    imgqua.SetNativeSize();
                }
                else if (json["qua"]["loaiitem"].Value == "ItemEvent")
                {
                    //for (int a = 0; a < allitemEvent.Length; a++)
                    //{
                    //    if (allitemEvent[a].name == json["qua"]["nameitem"].Value)
                    //    {
                    //        imgqua.sprite = allitemEvent[a];
                    //        break;
                    //    }
                    //}
                    imgqua.sprite = GetSprite(json["qua"]["nameitem"].AsString);
                    txtsoluong.text = json["qua"]["soluong"].Value;
                    imgqua.SetNativeSize();
                }
                else if (json["qua"]["loaiitem"].Value == "KhungAvatar")
                {
                    Friend.ins.LoadImage("khungavt", json["qua"]["nameitem"].Value, imgqua);
                    txtsoluong.text = "1";
                }
                //   panelqua.transform.GetChild(i).gameObject.SetActive(true);


                if (json["duocnhan"].Value == "true")
                {
                    // panelqua.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                    // panelqua.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
                    menuxacnhan.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    // panelqua.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                    //panelqua.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                    menuxacnhan.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
                }
                vitrichon = (byte)objchon.transform.GetSiblingIndex();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void ThucHienNV()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "ThucHienNV";
        datasend["data"]["key"] = btn.transform.parent.name;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "1")
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
                //btn.interactable = false;
            }
            else if (json["status"].Value == "Item")
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
                btn.transform.parent.GetComponent<Text>().text = json["info"].Value;
                btn.gameObject.SetActive(false);
                btn.transform.parent.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (json["status"].Value == "Rong")
            {

                GameObject menuchonrong = giaodiennut1.transform.GetChild(11).gameObject;
                menuchonrong.name = btn.transform.parent.name;
                GameObject allobj = menuchonrong.transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject SlotDaChon = allobj.transform.GetChild(0).gameObject;
                GameObject SlotRong = allobj.transform.GetChild(1).gameObject;
                GameObject ImgDoiHinh = allobj.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject ContentRongDaChon = allobj.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
                GameObject ContentRongChon = allobj.transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;

                //for (int i = 0; i < ContentRongDaChon.transform.childCount; i++)
                //{
                //    Destroy(ContentRongDaChon.transform.GetChild(i).gameObject);
                //}
                //for (int i = 0; i < ContentRongChon.transform.childCount; i++)
                //{
                //    Destroy(ContentRongChon.transform.GetChild(i).gameObject);
                //}
                for (int i = 0; i < int.Parse(json["soluongyeucau"].Value); i++)
                {
                    GameObject otrong = Instantiate(SlotDaChon, ContentRongDaChon.transform.position, Quaternion.identity);
                    otrong.transform.SetParent(ContentRongDaChon.transform.transform, false);
                    otrong.name = i.ToString();
                    otrong.SetActive(true);
                }
                for (var i = 0; i < ContentRongChon.transform.childCount - 1; i++)
                {
                    Destroy(ContentRongChon.transform.GetChild(i).gameObject);
                }
                for (var i = 0; i < ContentRongDaChon.transform.childCount - 1; i++)
                {
                    Destroy(ContentRongDaChon.transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < NetworkManager.ins.inventory.TuiRong.transform.childCount - 1; i++)
                {
                    if (NetworkManager.ins.inventory.TuiRong.transform.GetChild(i).transform.childCount > 0)
                    {

                        ItemDragon itemdra = NetworkManager.ins.inventory.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                        debug.Log(itemdra.nameObjectDragon);
                        //   string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                        if (itemdra.nameObjectDragon == json["RongYeuCau"].Value)
                        {
                            GameObject otrong = Instantiate(SlotRong, ContentRongChon.transform.position, Quaternion.identity);
                            otrong.transform.SetParent(ContentRongChon.transform.transform, false);
                            otrong.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = itemdra.transform.GetChild(0).GetComponent<Image>().sprite;
                            otrong.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = itemdra.txtSao.text;
                            otrong.transform.GetChild(0).name = itemdra.gameObject.name;
                            otrong.SetActive(true);
                        }
                    }
                }
                menuchonrong.SetActive(true);
            }
        }
    }
    public void ChonRong()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        GameObject menuchonrong = giaodiennut1.transform.GetChild(11).gameObject;
        GameObject allobj = menuchonrong.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject ContentRongDaChon = allobj.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
        GameObject ContentRongChon = allobj.transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;

        if (objchon.transform.parent.transform.parent.name == "ContentRongDaChon")
        {
            for (int i = 0; i < ContentRongChon.transform.childCount; i++)
            {
                if (ContentRongChon.transform.GetChild(i).transform.childCount == 0)
                {
                    objchon.transform.SetParent(ContentRongChon.transform.GetChild(i).transform);
                    objchon.transform.position = ContentRongChon.transform.GetChild(i).transform.position;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < ContentRongDaChon.transform.childCount; i++)
            {
                if (ContentRongDaChon.transform.GetChild(i).transform.childCount == 0)
                {
                    objchon.transform.SetParent(ContentRongDaChon.transform.GetChild(i).transform);
                    objchon.transform.position = ContentRongDaChon.transform.GetChild(i).transform.position;
                    break;
                }
            }

        }
    }
    public void XemChiSoRong()
    {
        GameObject objitem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        CrGame.ins.ChiSoRong(objitem.name);
    }
    public void Nhannv()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanNV";
        datasend["data"]["vitri"] = vitrichon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                GameObject menuxacnhan = giaodiennut1.transform.Find("menuchonqua").gameObject;
                menuxacnhan.SetActive(false);
                GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
                //    YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Yêu cầu mở đường mốc thứ " + (int.Parse(json["event"]["odanglam"]) + 1);
                LoadNhiemVu(json["mocnhiemvu"]);
                GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;
                Transform child = alloQua.transform.GetChild(vitrichon);
                YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Yêu cầu mở đường mốc thứ " + child.name;

                btnHuy.transform.SetParent(child.transform, false);
                btnHuy.transform.position = child.transform.position;
                btnHuy.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    GameObject txtnhiemvu = allnhiemvu.transform.GetChild(i).gameObject;
                    txtnhiemvu.transform.GetChild(0).gameObject.SetActive(true);
                    txtnhiemvu.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
    public void BatDau()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "BatDau";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                //    CrGame.ins.OnThongBaoNhanh(json["thongbao"].Value, 2);
                objchon.gameObject.SetActive(false);
                objchon.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
                objchon.transform.parent.transform.GetChild(2).gameObject.SetActive(true);
                timeSec = float.Parse(json["timeSec"].Value);
                Text txtTheLuc = giaodiennut1.transform.Find("TheLuc").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                txtTheLuc.text = json["theluc"].Value;
                txttime.transform.GetChild(2).GetComponent<Button>().interactable = false;
                dem = true;

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
    public void XacNhanRong()
    {
        GameObject menuchonrong = giaodiennut1.transform.GetChild(11).gameObject;
        GameObject allobj = menuchonrong.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject ContentRongDaChon = allobj.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
        //GameObject ContentRongChon = allobj.transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        string allrong = "";
        for (int i = 0; i < ContentRongDaChon.transform.childCount; i++)
        {
            if (ContentRongDaChon.transform.GetChild(i).transform.childCount > 0)
            {
                allrong += ContentRongDaChon.transform.GetChild(i).transform.GetChild(0).name + "*";
            }
        }

        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "XacNhanRong";
        datasend["data"]["allrong"] = allrong;
        datasend["data"]["nhiemvu"] = menuchonrong.name;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                string[] allrongxoa = allrong.Split('*');
                for (int i = 0; i < allrongxoa.Length; i++)
                {
                    if (allrongxoa[i] != "")
                    {
                        for (int j = 0; j < NetworkManager.ins.inventory.TuiRong.transform.childCount - 1; j++)
                        {
                            if (NetworkManager.ins.inventory.TuiRong.transform.GetChild(j).transform.childCount > 0)
                            {
                                //  ItemDragon itemdra = net.inventory.TuiRong.transform.GetChild(j).transform.GetChild(0).GetComponent<ItemDragon>();
                                if (NetworkManager.ins.inventory.TuiRong.transform.GetChild(j).transform.GetChild(0).gameObject.name == allrongxoa[i])
                                {
                                    Destroy(NetworkManager.ins.inventory.TuiRong.transform.GetChild(j).transform.GetChild(0).gameObject);
                                    break;
                                }
                            }
                        }
                    }

                }
                GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
                GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                for (int i = 0; i < allnhiemvu.transform.childCount; i++)
                {
                    if (allnhiemvu.transform.GetChild(i).gameObject.name == json["nhiemvu"].Value)
                    {
                        allnhiemvu.transform.GetChild(i).GetComponent<Text>().text = json["info"].Value;
                        break;
                    }
                }
                menuchonrong.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
    public void HoanThanh()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "HoanThanh";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    hoanthanh = false;
                    txttime.transform.GetChild(0).gameObject.SetActive(true);
                    txttime.transform.GetChild(1).gameObject.SetActive(true);
                    txttime.transform.GetChild(2).gameObject.SetActive(false);
                    txttime.text = "Yêu cầu: 8 giờ";
                    dem = false;

                    GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;  
                    YeuCau.SetActive(false);

                    GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject imgqua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
                    int vitri = int.Parse(json["vitri"].Value);
                    if (alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>())
                    {
                        alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().Play("quamo");
                    }
                    if (json["quatang"]["loaiitem"].Value == "Item")
                    {
                        yield return new WaitForSeconds(0.2f);
                        GameObject item = Instantiate(imgqua, alloQua.transform.GetChild(vitri).transform.position, Quaternion.identity);
                        item.GetComponent<Image>().sprite = Inventory.LoadSprite(json["quatang"]["nameitem"].Value);
                        QuaBay quabay = item.AddComponent<QuaBay>();
                        quabay.vitribay = btnHopQua;
                        item.transform.SetParent(gameObject.transform);
                        item.transform.position = alloQua.transform.GetChild(vitri).transform.position;
                        item.SetActive(true);
                        yield return new WaitForSeconds(1.2f);
                    }
                    else if (json["quatang"]["loaiitem"].Value == "ItemRong")
                    {
                        yield return new WaitForSeconds(0.2f);
                        GameObject item = Instantiate(imgqua, alloQua.transform.GetChild(vitri).transform.position, Quaternion.identity);
                        item.GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["quatang"]["nameitem"].Value + "1");
                        QuaBay quabay = item.AddComponent<QuaBay>();
                        quabay.vitribay = btnHopQua;
                        item.transform.SetParent(gameObject.transform);
                        item.transform.position = alloQua.transform.GetChild(vitri).transform.position;
                        item.SetActive(true);
                        yield return new WaitForSeconds(1.2f);
                    }
                    btnHuy.SetActive(false);
                    LoadMocNv(json["infomocnv"]);
                    hoanthanh = true;
                }
               
                // CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }

        }
    }
    public RuntimeAnimatorController animquacam;
    void LoadMocNv(JSONNode json)
    {
        GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 0; i < json.Count; i++)
        {
            if (json[i].Value == "chuaduoclam")
            {
                Image imgqua = alloQua.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                imgqua.color = new Color32(109, 105, 105, 255);
            }
            else if (json[i].Value == "chualam" || json[i].Value == "danglam")
            {
                Image imgqua = alloQua.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                imgqua.color = new Color32(255, 255, 255, 255);
                if (imgqua.name == "hopqua")
                {
                    if (imgqua.GetComponent<Animator>() == false)
                    {
                        Animator anim = imgqua.gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = animquacam;
                    }
                }
            }
            else if (json[i].Value == "dalam")
            {
                if (alloQua.transform.GetChild(i).GetComponent<Image>()) alloQua.transform.GetChild(i).GetComponent<Image>().enabled = false;
                alloQua.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                alloQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    float BinhNangLuongchon, giamthoigian, maxgiam;
    public void OpenMenuBonPhan()
    {
        GameObject menubonphan = giaodiennut1.transform.Find("MenuTangToc").gameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetBinhNangLuong";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                maxgiam = float.Parse(json["max"].Value);
                menubonphan.transform.GetChild(3).GetComponent<Text>().text = json["BinhNangLuong"].Value;
                menubonphan.transform.GetChild(5).GetComponent<Text>().text = "1";
                BinhNangLuongchon = 1;
                float kphanbon = float.Parse(json["giamthoigian"].Value) + 1;
                menubonphan.transform.GetChild(7).GetComponent<Image>().fillAmount = kphanbon / (float)maxgiam;
                giamthoigian = kphanbon;
                menubonphan.transform.GetChild(8).GetComponent<Text>().text = "Giảm thời gian:<color=lime> " + kphanbon + " phút</color>";
                menubonphan.SetActive(true);


            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void TangSoLuongBinhNangLuong(int i)
    {
        GameObject menubonphan = giaodiennut1.transform.Find("MenuTangToc").gameObject;

      //  if (BinhNangLuongchon >= 1 && giamthoigian <= maxgiam)
        {
            Text txtphanbonDangCo = menubonphan.transform.Find("txtphanbonDangCo").GetComponent<Text>();
            int phanbondangco = int.Parse(txtphanbonDangCo.text);
            if (i > phanbondangco)
            {
                i = phanbondangco;
            
            }
            if (i > maxgiam) i = (int)maxgiam - (int)giamthoigian;
            BinhNangLuongchon += i;
            if (BinhNangLuongchon <= 0)
            {
                BinhNangLuongchon = 1;
                return;
            }
        
            giamthoigian += i;
            menubonphan.transform.GetChild(7).GetComponent<Image>().fillAmount = (float)giamthoigian / (float)maxgiam;
            menubonphan.transform.GetChild(5).GetComponent<Text>().text = BinhNangLuongchon.ToString();

            menubonphan.transform.GetChild(8).GetComponent<Text>().text = "Giảm thời gian:<color=lime> " + giamthoigian + " phút</color>";
        }
    }
    public void SuDungBinhNangLuong()
    {
        GameObject menubonphan = giaodiennut1.transform.Find("MenuTangToc").gameObject;
        int sophanbonchon = int.Parse(menubonphan.transform.GetChild(5).GetComponent<Text>().text);
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "SuDungBinhNangLuong";
        datasend["data"]["sobinh"] = sophanbonchon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                CrGame.ins.OnThongBaoNhanh("Giảm thời gian <color=lime>+" + sophanbonchon + " phút</color>", 2);
                GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
                GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;
                menubonphan.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void OpenGiaoDienNgocTrai()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetGiaoDienNgocTrai";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            //    debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {

                MenuNgocTrai menuNgocTrai = GetCreateMenu("GiaoDienNgocTrai", giaodiennut1.transform,true,giaodiennut1.transform.Find("btnVeNha").transform.GetSiblingIndex() + 1).GetComponent<MenuNgocTrai>();//giaodiennut1.transform.Find("GiaoDienNgocTrai").GetComponent<MenuNgocTrai>();
                menuNgocTrai.ParseData(json);
                //   menuNgocTrai.transform.GetChild(0).Find("btnExit").GetComponent<Button>().onClick.AddListener(ExitNgocTrai);
                menuNgocTrai.gameObject.SetActive(true);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
        }
    }
    public void GetGiaoDienPhongAn()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetGiaoDienPhongAn";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            //    debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {

                MenuPhongAn menuPhongAn = GetCreateMenu("GiaoDienPhongAn", giaodiennut1.transform,true, giaodiennut1.transform.Find("btnVeNha").transform.GetSiblingIndex() + 1).GetComponent<MenuPhongAn>();
                menuPhongAn.ParseData(json);
                //   menuNgocTrai.transform.GetChild(0).Find("btnExit").GetComponent<Button>().onClick.AddListener(ExitNgocTrai);
                menuPhongAn.gameObject.SetActive(true);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
        }
    }
    public void HuyNhiemVu()
    {
        AudioManager.PlaySound("soundClick");
        OpenThongBaoChon("Hủy nhiệm vụ sẽ không hoàn lại vật phẩm (nếu có)", send);
        void send()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "HuyNhiemVu";
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                //    debug.Log(json.ToString());
                if (json["status"].AsString == "0")
                {
                    txttime.transform.GetChild(0).gameObject.SetActive(true);
                    txttime.transform.GetChild(1).gameObject.SetActive(true);
                    txttime.transform.GetChild(2).gameObject.SetActive(false);
                    txttime.text = "Yêu cầu: 8 giờ";
                    dem = false;

                    GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
                    YeuCau.SetActive(false);

                    //GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    //GameObject imgqua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
                    //int vitri = int.Parse(json["vitri"].Value);
                    //if (alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>())
                    //{
                    //    alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().Play("quamo");
                    //}
                    btnHuy.SetActive(false);
                }
                else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
            }
        }
       
    }
    public void VeNha()
    {
        //  giaodiennut1.transform.SetParent(gameObject.transform);
        Destroy(giaodiennut1.gameObject);
        Transform dangodao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao);
        dangodao.gameObject.SetActive(true);
        // CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(true);
        Vector3 vec = dangodao.transform.position;
        vec.z = -10;
        cam.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
      //  gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuEventSinhNhat2024");
    }    
    void Update()
    {
        if (drag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
                //debug.Log(dragOrigin);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 difrence = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position = Clampcamera(cam.transform.position + difrence);
            }
        }
        if (dem)
        {
            if (timeSec > 0)
            {
                timeSec -= Time.deltaTime;
                DisplayTime();
            }
            else
            {
                dem = false;
                timeSec = 0;
                txttime.text = "Yêu cầu: Hoàn thành";
                txttime.transform.GetChild(2).GetComponent<Button>().interactable = true;
            }
        }

    }
    public void OpenMenuNhiemvu()
    {
        AudioManager.SoundClick();
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
    public void Drag(bool b)
    {
        drag = b;
    }
    void DisplayTime()
    {
        //  timeToDisplay += 1;
        float sec = Mathf.FloorToInt(timeSec);
        float minutes = 0;
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
        //float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        txttime.text = "Yêu cầu: " + gio + ":" + minutes + ":" + sec;
        //   txttime.text = string.Format("{0:00}:{0:00}:{1:00}",gio, minutes, seconds);
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
