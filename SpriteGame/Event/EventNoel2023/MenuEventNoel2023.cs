using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEventNoel2023 : EventManager
{

    Vector3 dragOrigin; Camera cam; SpriteRenderer imgMap;
    float MapMinX, MapMaxX, mapMiny, MapMaxY;

    public GameObject btnHopQua { get; private set; } bool drag;
    public GameObject giaodiennut1;
    private float timeSec = 0;
    public Text txttime; 
    private bool dem = false;
    public byte vitrichon;
    public Sprite[] allitemEvent;
    public Transform BtnKeo;
    private int BinhNangLuongCap1Chon,
     BinhNangLuongCap2Chon, SoBinhSuDung;
    private int soBinhNangLuongCap1, soBinhNangLuongCap2;
    // Start is called before the first frame update
    protected override void ABSAwake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;

        imgMap = transform.GetChild(0).transform.GetChild(1).GetComponent<SpriteRenderer>();
        MapMinX = imgMap.transform.position.x - imgMap.bounds.size.x / 2f;
        MapMaxX = imgMap.transform.position.x + imgMap.bounds.size.x / 2f;
        mapMiny = imgMap.transform.position.y - imgMap.bounds.size.y / 2f;
        MapMaxY = imgMap.transform.position.y + imgMap.bounds.size.y / 2f;
      //  imgMap.sprite = GetSprite("BGDem");
        if (CrGame.ins.NgayDem != "Ngay")
        {
            imgMap.sprite = GetSprite("BGDem");
        }    
    }

    private void OnEnable()
    {
        if (Friend.ins.QuaNha) Friend.ins.GoHome();

        GameObject trencung = GameObject.FindGameObjectWithTag("trencung").gameObject;
        giaodiennut1.transform.SetParent(trencung.transform);
        giaodiennut1.transform.SetAsFirstSibling();
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("vienchinh").transform);
        //  giaodiennut1.transform.position = new Vector3(trencung.transform.position.x, trencung.transform.position.y, 0);
        btnHopQua.transform.SetParent(trencung.transform);

        CrGame.ins.giaodien.SetActive(false);

        Camera.main.orthographicSize = 5;
        cam.GetComponent<ZoomCamera>().enabled = false;
        GameObject img = gameObject.transform.GetChild(0).gameObject;
        cam.transform.position = new Vector3(img.transform.position.x, img.transform.position.y, -10);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        CrGame.ins.menulogin.SetActive(false);
        //BtnKeo.transform.position = giaodiennut1.transform.position;
    }

    public void ParseData(JSONNode json)
    {
        //debug.Log(json.ToString());
        Text txtThongTin = giaodiennut1.transform.Find("Info").transform.GetChild(0).GetComponent<Text>();
        Text txtXuThamhiem = giaodiennut1.transform.Find("itemXuThamHiem").transform.GetChild(1).GetComponent<Text>();
        txtThongTin.text = "                <color=yellow>Thông tin</color>\n-Long Hoa Viên: <color=lime> Vòng " + (json["data"]["luotquavong"].AsInt) + "</color>\n-Tổng số lần check-in:<color=lime> " + json["data"]["tongsolancheckin"].AsString + "</color>";
        txtXuThamhiem.text = json["data"]["XuThamHiem"].AsString;
        LoadNhiemVu(json["data"]["mocnhiemvu"]);
        LoadMocNv(json["data"]["infomocnv"]);

        GameObject alloQua = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
        GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
        int odanglam = int.Parse(json["data"]["odanglam"].Value) - 1;
        if (odanglam >= alloQua.transform.childCount) odanglam = alloQua.transform.childCount - 1;
        YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Điểm đến thứ " + alloQua.transform.GetChild(odanglam).name + " Long Hoa Viên";
        GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
        allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;

        if (json["data"]["batdaunhiemvu"].AsBool)
        {
            txttime.transform.GetChild(0).gameObject.SetActive(false);
            txttime.transform.GetChild(1).gameObject.SetActive(false);
            txttime.transform.GetChild(2).gameObject.SetActive(true);
            timeSec = float.Parse(json["data"]["sec"].Value);
            txttime.transform.GetChild(2).GetComponent<Button>().interactable = false;
            dem = true;
        }
        if (json["data"]["duocquavong"].AsBool)
        {
            giaodiennut1.transform.Find("Info").transform.GetChild(1).gameObject.SetActive(true);
        }
        giaodiennut1.transform.Find("txtDay").GetComponent<Text>().text = "Ngày " + json["data"]["ngaydangnhap"].AsString;
        AudioManager.SetSoundBg("nhacnoel");
    }
    private void LoadMocNv(JSONNode json)
    {
        GameObject alloQua = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
        for (int i = 0; i < json.Count; i++)
        {
            GameObject oqua = alloQua.transform.GetChild(i).gameObject;
            if (json[i].Value == "chuaduoclam")
            {
                Image imgqua = oqua.GetComponent<Image>();
                imgqua.color = new Color32(109, 105, 105, 255);
            }
            else if (json[i].Value == "chualam" || json[i].Value == "danglam")
            {
                Image imgqua = oqua.GetComponent<Image>();
                imgqua.color = new Color32(255, 255, 255, 255);
                //if (imgqua.name == "hopqua")
                //{
                //    if (imgqua.GetComponent<Animator>() == false)
                //    {
                //        Animator anim = imgqua.gameObject.AddComponent<Animator>();
                //        anim.runtimeAnimatorController = animquacam;
                //    }
                //}
            }
            else if (json[i].Value == "dalam")
            {
              //  debug.Log("ok1");
                if (oqua.GetComponent<Image>()) oqua.GetComponent<Image>().enabled = false;

                //  debug.Log("ok2");
                // alloQua.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                //    debug.Log("ok3");
                oqua.transform.GetChild(0).gameObject.SetActive(true);
                if(oqua.transform.Find("hopqua"))
                {
                    oqua.transform.Find("hopqua").gameObject.SetActive(false);
                }
            }
        }
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
    // Update is called once per frame
    void Update()
    {
        BtnKeo.transform.position = giaodiennut1.transform.position;
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
    public void ThucHienNV()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        AudioManager.PlaySound("soundClick");
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "ThucHienNv";
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

                GameObject menuchonrong = giaodiennut1.transform.GetChild(12).gameObject;
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

    public void BatDauNhiemVu()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "BatDauNhiemVu";
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
                Text txtXuThamhiem = giaodiennut1.transform.Find("itemXuThamHiem").transform.GetChild(1).GetComponent<Text>();

                txtXuThamhiem.text = json["XuThamHiem"].AsString;
                txttime.transform.GetChild(2).GetComponent<Button>().interactable = false;
                dem = true;

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
    private bool hoanthanh = true;
    public void MoQua()
    {
        if (!hoanthanh) return;
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;


        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "XemQua";
        datasend["data"]["vitri"] = objchon.transform.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                GameObject menuxacnhan = giaodiennut1.transform.Find("menuchonqua").gameObject;
                Text txtxacnhan = menuxacnhan.transform.GetChild(1).GetComponent<Text>();
                txtxacnhan.text = json["thongtin"].Value;
                menuxacnhan.SetActive(true);

                GameObject allqua = menuxacnhan.transform.GetChild(2).gameObject;
                // Image imgqua = menuxacnhan.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
                //  Text txtsoluong = imgqua.transform.GetChild(0).GetComponent<Text>();

                for (int i = 0; i < allqua.transform.childCount; i++)
                {
                    allqua.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < json["qua"].Count; i++)
                {
                    Image imgqua = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                    Text txtsoluong = allqua.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
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
                        for (int a = 0; a < allitemEvent.Length; a++)
                        {
                            if (allitemEvent[a].name == json["qua"][i]["nameitem"].Value)
                            {
                                imgqua.sprite = allitemEvent[a];
                                break;
                            }
                        }

                        txtsoluong.text = json["qua"][i]["soluong"].Value;
                        imgqua.SetNativeSize();
                    }
                    else if (json["qua"][i]["loaiitem"].Value == "KhungAvatar")
                    {
                        //  Friend.ins.LoadImage("khungavt", json["qua"][i]["nameitem"].Value, imgqua);
                        // txtsoluong.text = "1";
                        imgqua.sprite = Inventory.LoadSprite(json["qua"][i]["nameitem"].Value);
                        txtsoluong.text = json["qua"][i]["soluong"].Value;
                        imgqua.SetNativeSize();
                    }
                    allqua.transform.GetChild(i).gameObject.SetActive(true);
                }
           
                //   panelqua.transform.GetChild(i).gameObject.SetActive(true);


                if (json["duocnhan"].Value == "true")
                {
                    // panelqua.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                    // panelqua.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
                    menuxacnhan.transform.Find("btnqua2").gameObject.SetActive(true);
                }
                else
                {
                    // panelqua.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                    //panelqua.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                    menuxacnhan.transform.Find("btnqua2").gameObject.SetActive(false);
                }
                vitrichon = (byte)objchon.transform.GetSiblingIndex();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void HoanThanh()
    {
       // GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "HoanThanhNhiemVu";
       // datasend["data"]["vitri"] = objchon.transform.GetSiblingIndex().ToString();
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

                    GameObject alloQua = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;

                  
                    GameObject imgqua = transform.GetChild(0).transform.GetChild(2).gameObject;
                    int vitri = int.Parse(json["vitri"].Value);
                    //if (alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>())
                    //{
                    //    alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().Play("quamo");
                    //}
                    CrGame.ins.panelLoadDao.SetActive(false);
                    byte soitemthuong = 0;
                    for (int i = 0; i < json["quatang"].Count; i++)
                    {
                        soitemthuong = 0;
                        if (json["quatang"][i]["loaiitem"].Value == "Item")
                        {
                            yield return new WaitForSeconds(0.2f);
                            soitemthuong += 1;
                            GameObject item = Instantiate(imgqua, alloQua.transform.GetChild(vitri).transform.position, Quaternion.identity);
                            item.GetComponent<Image>().sprite = Inventory.LoadSprite(json["quatang"][i]["nameitem"].Value);
                            QuaBay quabay = item.AddComponent<QuaBay>();
                            quabay.vitribay = btnHopQua;
                            item.transform.SetParent(gameObject.transform,false);
                            item.transform.position = alloQua.transform.GetChild(vitri).transform.position;
                            item.SetActive(true);
                          //  yield return new WaitForSeconds(1.2f);
                        }
                        else if (json["quatang"][i]["loaiitem"].Value == "ItemRong")
                        {
                            yield return new WaitForSeconds(0.2f);
                            soitemthuong += 1;
                            GameObject item = Instantiate(imgqua, alloQua.transform.GetChild(vitri).transform.position, Quaternion.identity);
                            item.GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["quatang"][i]["nameitem"].Value + "1");
                            QuaBay quabay = item.AddComponent<QuaBay>();
                            quabay.vitribay = btnHopQua;
                            item.transform.SetParent(gameObject.transform,false);
                            item.transform.position = alloQua.transform.GetChild(vitri).transform.position;
                            item.SetActive(true);
                           // yield return new WaitForSeconds(1.2f);
                        }
                        if(soitemthuong > 0) yield return new WaitForSeconds(1.2f);

                    }
                    if (json["duocquavong"].AsBool)
                    {
                        giaodiennut1.transform.Find("Info").transform.GetChild(1).gameObject.SetActive(true);
                    }

                    Text txtThongTin = giaodiennut1.transform.Find("Info").transform.GetChild(0).GetComponent<Text>();
                    txtThongTin.text = "                <color=yellow>Thông tin</color>\n-Long Hoa Viên: <color=lime> Vòng " + (json["luotquavong"].AsInt) + "</color>\n-Tổng số lần check-in:<color=lime> " + json["tongsolancheckin"].AsString + "</color>";
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
    public void NhanNv()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "NhanNV";
        datasend["data"]["vitri"] = vitrichon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                GameObject menuxacnhan = giaodiennut1.transform.Find("menuchonqua").gameObject;
                menuxacnhan.SetActive(false);
                GameObject alloQua = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
                GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
                //    YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Yêu cầu mở đường mốc thứ " + (int.Parse(json["event"]["odanglam"]) + 1);
                LoadNhiemVu(json["mocnhiemvu"]);
                GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;
                YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Điểm đến thứ " + alloQua.transform.GetChild(vitrichon).name + " Long Hoa Viên";
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
    public void ChonRong()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        GameObject menuchonrong = giaodiennut1.transform.GetChild(12).gameObject;
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
    public void XacNhanRong()
    {
        GameObject menuchonrong = giaodiennut1.transform.GetChild(12).gameObject;
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
        datasend["class"] = "EventNoel2023";
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


    public void QuaVong()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "QuaVong";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                LoadNhiemVu(json["mocnhiemvu"]);
             //   LoadMocNv(json["infomocnv"]);
                giaodiennut1.transform.Find("Info").transform.GetChild(1).gameObject.SetActive(false);

                GameObject YeuCau = giaodiennut1.transform.Find("YeuCau").gameObject;
                GameObject alloQua = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
                YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Điểm đến thứ " + alloQua.transform.GetChild(0).name + " Long Hoa Viên";
                GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;

                Image imgo1 = alloQua.transform.GetChild(0).GetComponent<Image>();
                imgo1.color = new Color32(255, 255, 255, 255);
                imgo1.enabled = true;
                alloQua.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);

                for (int i = 1; i < alloQua.transform.childCount - 1; i++)
                {
                    GameObject oqua = alloQua.transform.GetChild(i).gameObject;
                    oqua.transform.GetChild(0).gameObject.SetActive(false);
                    Image imgqua = oqua.GetComponent<Image>();
                    imgqua.color = new Color32(109, 105, 105, 255);
                    imgqua.enabled = true;
                }

                Text txtThongTin = giaodiennut1.transform.Find("Info").transform.GetChild(0).GetComponent<Text>();
                txtThongTin.text = "                <color=yellow>Thông tin</color>\n-Long Hoa Viên: <color=lime> Vòng " + (json["luotquavong"].AsInt) + "</color>\n-Tổng số lần check-in:<color=lime> " + json["tongsolancheckin"].AsString + "</color>";
                CrGame.ins.OnThongBaoNhanh("<size=60><color=lime>Vòng " + json["luotquavong"].AsString + " Long Hoa Viên</color></size>", 2);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 3);
            }
        }
    }
    private float giamthoigian, maxgiam;
    public Sprite BinhNangLuongCap1, BinhNangLuongCap2;
    public void OpenMenuTangToc()
    {
        GameObject menubonphan = giaodiennut1.transform.Find("MenuTangToc").gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "GetBinhNangLuong";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].Value == "ok")
            {
                maxgiam = float.Parse(json["max"].Value);
             
                menubonphan.transform.GetChild(5).GetComponent<Text>().text = "0";
                BinhNangLuongCap1Chon = 0;
                BinhNangLuongCap2Chon = 0;

                soBinhNangLuongCap1 = json["BinhNangLuongCap1"].AsInt;

                soBinhNangLuongCap2 = json["BinhNangLuongCap2"].AsInt;
                menubonphan.transform.GetChild(3).GetComponent<Text>().text = soBinhNangLuongCap1.ToString();

                SoBinhSuDung = json["GiamThoiGian"].AsInt;
                giamthoigian = json["GiamThoiGian"].AsFloat;
                menubonphan.transform.GetChild(7).GetComponent<Image>().fillAmount = SoBinhSuDung / maxgiam;

                menubonphan.transform.GetChild(8).GetComponent<Text>().text = "Giảm thời gian:<color=lime> " + giamthoigian + " phút</color>";
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
        int sophanbonchon = int.Parse(menubonphan.transform.GetChild(5).GetComponent<Text>().text);
        
        Image imgbinhnangluong = menubonphan.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();

        int TongSoBinhNangLuong = BinhNangLuongCap1Chon + BinhNangLuongCap2Chon;
       
        if (imgbinhnangluong.sprite.name == "BinhNangLuongCap1")
        {
            if (soBinhNangLuongCap1 > 0 && TongSoBinhNangLuong < maxgiam)
            {
                BinhNangLuongCap1Chon += (short)i;
                if (BinhNangLuongCap1Chon <= 0)
                {
                    BinhNangLuongCap1Chon = 0;
                    menubonphan.transform.GetChild(5).GetComponent<Text>().text = BinhNangLuongCap1Chon.ToString();
                    return;
                }
                menubonphan.transform.GetChild(5).GetComponent<Text>().text = BinhNangLuongCap1Chon.ToString();
            
                giamthoigian += i;
            }
        }
        else if (imgbinhnangluong.sprite.name == "BinhNangLuongCap2")
        {
            if (soBinhNangLuongCap2 > 0 && TongSoBinhNangLuong < maxgiam)
            {
                BinhNangLuongCap2Chon += (short)i;
                if (BinhNangLuongCap2Chon <= 0)
                {
                    BinhNangLuongCap2Chon = 0;
                    menubonphan.transform.GetChild(5).GetComponent<Text>().text = BinhNangLuongCap2Chon.ToString();
                    return;
                }
                menubonphan.transform.GetChild(5).GetComponent<Text>().text = BinhNangLuongCap2Chon.ToString();
                giamthoigian += 2 * i;
            }
        }
        SoBinhSuDung += (short)i;
        menubonphan.transform.GetChild(7).GetComponent<Image>().fillAmount = (float)SoBinhSuDung / (float)maxgiam;
  

        menubonphan.transform.GetChild(8).GetComponent<Text>().text = "Giảm thời gian:<color=lime> " + giamthoigian + " phút</color>";

        //if (BinhNangLuongchon >= 1 && giamthoigian < maxgiam)
        //{
        //    BinhNangLuongchon += i;
        //    if (BinhNangLuongchon <= 0)
        //    {
        //        BinhNangLuongchon = 1;
        //        return;
        //    }
        //    giamthoigian += i;

        //}
    }
    public void DoiBinhNangLuong()
    {
        GameObject menubonphan = giaodiennut1.transform.Find("MenuTangToc").gameObject;
        Image imgbinhnangluong = menubonphan.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        Text txtdangco = menubonphan.transform.GetChild(3).GetComponent<Text>();
        if (imgbinhnangluong.sprite.name == "BinhNangLuongCap1")
        {
            imgbinhnangluong.sprite = BinhNangLuongCap2;
            txtdangco.text = soBinhNangLuongCap2.ToString();
            menubonphan.transform.GetChild(5).GetComponent<Text>().text = BinhNangLuongCap2Chon.ToString();
           // if (soBinhNangLuongCap2 > 0 && BinhNangLuongCap2Chon == 0) BinhNangLuongCap2Chon = 1;
        }
        else
        {
            imgbinhnangluong.sprite = BinhNangLuongCap1;
            txtdangco.text = soBinhNangLuongCap1.ToString();
            menubonphan.transform.GetChild(5).GetComponent<Text>().text = BinhNangLuongCap1Chon.ToString();

        }
      
        imgbinhnangluong.SetNativeSize();
    }    
    public void SuDungBinhNangLuong()
    {
        if(BinhNangLuongCap1Chon + BinhNangLuongCap2Chon <= 0)
        {
            CrGame.ins.OnThongBaoNhanh("Chọn số lượng phù hợp!");
            return;
        }
        GameObject menubonphan = giaodiennut1.transform.Find("MenuTangToc").gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventNoel2023";
        datasend["method"] = "SuDungBinhNangLuong";
        datasend["data"]["BinhNangLuongCap1"] = BinhNangLuongCap1Chon.ToString();
        datasend["data"]["BinhNangLuongCap2"] = BinhNangLuongCap2Chon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                CrGame.ins.OnThongBaoNhanh("Giảm thời gian <color=lime>+" + json["timeCong"].AsString + " phút</color>", 2);
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
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", giaodiennut1.transform);
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
    public void VeNha()
    {
       // giaodiennut1.transform.SetParent(gameObject.transform);
        CrGame.ins.AllDao.transform.Find("BGDao"+CrGame.ins.DangODao).gameObject.SetActive(true);
        Vector3 vec = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).transform.position;
        vec.z = -10;
        cam.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        gameObject.transform.SetParent(AllMenu.ins.transform);
        gameObject.SetActive(false);
        Destroy(giaodiennut1);
        AllMenu.ins.DestroyMenu("MenuEventNoel2023");
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        AudioManager.PlaySound("soundClick");
        MenuTrieuHoiRongVangBac mn = AllMenu.ins.GetCreateMenu("MenuTrieuHoiRongVangBac", giaodiennut1, false, giaodiennut1.transform.childCount - 1).GetComponent<MenuTrieuHoiRongVangBac>();
        mn.Setnamerong = namerong;
        mn.gameObject.SetActive(true);
    }
    bool sangtrang = false; int trang = 1, trangg = 1; float top, topcuoi;
    public void XemBXH()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemBXH";
        datasend["data"]["top"] = top.ToString();
        datasend["data"]["trang"] = trangg.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = GetCreateMenu("MenuTop", giaodiennut1.transform, false);
                //    menuevent["MenuTop"] = menu;
                // GameObject menu = GetCreateMenu("MenuTop",gameObject,false,3);
                if (json["alltop"].Count > 0)
                {
                    GameObject menutop = menu.transform.GetChild(0).GetChild(3).gameObject;

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
                           // debug.Log(json["alltop"][i]["Name"].Value);
                            Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].AsString;
                         //   debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                         //   debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = i + trang;
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
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["diemtangthiep"].AsString;
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

                GameObject mntop = menu.transform.GetChild(0).transform.GetChild(3).gameObject;
                menu.transform.GetChild(0).transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate { 
                    DestroyMenu("MenuTop");
                    trang = 1; trangg = 1; top = 0; topcuoi = 0;
                });
                Button btn = mntop.transform.GetChild(1).GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(delegate { sangtrangtop(1); });
                menu.SetActive(true);
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
        }
        void sangtrangtop(int i)
        {
            AudioManager.PlaySound("soundClick");
            if (top <= topcuoi && sangtrang)
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
    protected override void DiemDanhOk(JSONNode json)
    {
        
    }
    public void OpenSkGioiHan()
    {
        AudioManager.PlaySound("soundClick");
        AllMenu.ins.OpenMenuTrenCung("MenuSuKienGioiHan");
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

                MenuNgocTrai menuNgocTrai = GetCreateMenu("GiaoDienNgocTrai", CrGame.ins.trencung.transform,true,giaodiennut1.transform.GetSiblingIndex() + 1).GetComponent<MenuNgocTrai>();//giaodiennut1.transform.Find("GiaoDienNgocTrai").GetComponent<MenuNgocTrai>();
                menuNgocTrai.ParseData(json);
             //   menuNgocTrai.transform.GetChild(0).Find("btnExit").GetComponent<Button>().onClick.AddListener(ExitNgocTrai);
                menuNgocTrai.gameObject.SetActive(true);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
        }
    }
    public void OpenGiaoDienCayThong()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDataGiaoDienCayThong";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            //    debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {

                MenuCayThong menuCayThong = GetCreateMenu("MenuCayThong", giaodiennut1.transform).GetComponent<MenuCayThong>();
                menuCayThong.ParseData(json);
                //   menuNgocTrai.transform.GetChild(0).Find("btnExit").GetComponent<Button>().onClick.AddListener(ExitNgocTrai);
                menuCayThong.transform.SetAsLastSibling();
                menuCayThong.gameObject.SetActive(true);
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
                MenuPhongAn menuPhongAn = GetCreateMenu("GiaoDienPhongAn", giaodiennut1.transform).GetComponent<MenuPhongAn>();
                menuPhongAn.ParseData(json);
                menuPhongAn.transform.SetAsLastSibling();
                //   menuNgocTrai.transform.GetChild(0).Find("btnExit").GetComponent<Button>().onClick.AddListener(ExitNgocTrai);
                menuPhongAn.gameObject.SetActive(true);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
        }
    }
}
