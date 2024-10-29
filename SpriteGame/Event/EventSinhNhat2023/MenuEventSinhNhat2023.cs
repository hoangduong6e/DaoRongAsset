using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEventSinhNhat2023 : MonoBehaviour
{
    Vector3 dragOrigin; Camera cam; SpriteRenderer imgMap;
    float MapMinX, MapMaxX, mapMiny, MapMaxY;
    GameObject btnHopQua;bool drag;
    public GameObject giaodiennut1;
    public Sprite[] allitemEvent;public RuntimeAnimatorController animquacam;
    float timeSec = 0;public Text txttime;bool dem = false;
    public byte vitrichon;int soluongchon; string ThiepChon = "ThiepHong";
    public Sprite Top1, Top2, Top3, Top;
    public Sprite sprite1, sprite2, chuaduocnhan, duocnhan, danhan;
    public Dictionary<string, GameObject> menuevent = new Dictionary<string, GameObject>();
    public Sprite[] alltuchat;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;

        imgMap = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
        MapMinX = imgMap.transform.position.x - imgMap.bounds.size.x / 2f;
        MapMaxX = imgMap.transform.position.x + imgMap.bounds.size.x / 2f;
        mapMiny = imgMap.transform.position.y - imgMap.bounds.size.y / 2f;
        MapMaxY = imgMap.transform.position.y + imgMap.bounds.size.y / 2f;
    }
    private void OnEnable()
    {
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetDataEvent/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                //CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.menulogin.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                //gameObject.SetActive(false);
                // VeNha();
                AllMenu.ins.DestroyMenu("MenuEventSinhNhat2023");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);

                JSONNode json = JSON.Parse(www.downloadHandler.text);
               
                if (json["status"].Value == "ok")
                {
                    if (Friend.ins.QuaNha) Friend.ins.GoHome();
                    Text txtTheLuc = giaodiennut1.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    txtTheLuc.text = json["event"]["theluc"].Value;
                    GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    LoadMocNv(json["event"]["infomocnv"]);
                    LoadDiemDanh(json);
                    GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
                    Text soluongthiephong = menuchonthiep.transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
                    Text soluongthiepvang = menuchonthiep.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
                    soluongthiephong.text = json["event"]["ThiepHong"].Value;
                    soluongthiepvang.text = json["event"]["ThiepVang"].Value;

                    GameObject YeuCau = giaodiennut1.transform.GetChild(5).gameObject;
                    YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Yêu cầu mở đường mốc thứ " + alloQua.transform.GetChild(int.Parse(json["event"]["odanglam"].Value) - 1).name;
                    LoadNhiemVu(json["event"]["mocnhiemvu"]);
                    GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                    allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;

                    if(json["event"]["batdaunhiemvu"].Value == "True")
                    {
                        txttime.transform.GetChild(0).gameObject.SetActive(false);
                        txttime.transform.GetChild(1).gameObject.SetActive(false);
                        txttime.transform.GetChild(2).gameObject.SetActive(true);
                        timeSec = float.Parse(json["event"]["sec"].Value);
                        txttime.transform.GetChild(2).GetComponent<Button>().interactable = false;
                        dem = true;
                    }

                    GameObject AllNhiemVu = giaodiennut1.transform.GetChild(13).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                    for (int i = 0; i < json["event"]["allNhiemvu"].Count; i++)
                    {
                        Text txttiendo = AllNhiemVu.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                        if (int.Parse(json["event"]["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value))
                        {
                            txttiendo.text = "<color=#00ff00ff>" + json["event"]["allNhiemvu"][i]["dalam"].Value + "/" + json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                        }
                        else
                        {
                            txttiendo.text = "<color=#ff0000ff>" + json["event"]["allNhiemvu"][i]["dalam"].Value + "/" + json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                        }

                    }
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
                    CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(false);
                    transform.GetChild(0).gameObject.SetActive(true);
                    CrGame.ins.menulogin.SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    CrGame.ins.menulogin.SetActive(false);
                    VeNha();
                //     AllMenu.ins.DestroyMenu("MenuEventNoel");
                }
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }
    public void OpenSkGioiHan()
    {
        AllMenu.ins.OpenMenuTrenCung("MenuSuKienGioiHan");
        AllMenu.ins.menu["MenuSuKienGioiHan"].transform.position = new Vector3(AllMenu.ins.menu["MenuSuKienGioiHan"].transform.position.x+0.07f, AllMenu.ins.menu["MenuSuKienGioiHan"].transform.position.y-0.4f, AllMenu.ins.menu["MenuSuKienGioiHan"].transform.position.z) ;
      //  AllMenu.ins.menu["MenuSuKienGioiHan"].transform.position = cam.transform.position;
      // AllMenu.ins.OpenCreateMenu("MenuSuKienGioiHan",transform.parent.transform.gameObject,true);
      // AllMenu.ins.menu["MenuSuKienGioiHan"].transform.SetAsLastSibling();
      // AllMenu.ins.menu["MenuSuKienGioiHan"].transform.position = transform.GetChild(0).transform.position;
    }    
    void LoadNhiemVu(JSONNode json)
    {
        GameObject YeuCau = giaodiennut1.transform.GetChild(5).gameObject;
        if(json.Count == 0)
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
            txtnhiemvu.name = json[i]["key"].Value;
            txtnhiemvu.text = json[i]["tiendo"].Value;
            txtnhiemvu.gameObject.SetActive(true);
            if (json[i]["xong"].Value == "daxong")
            {
                txtnhiemvu.transform.GetChild(0).gameObject.SetActive(false);
                txtnhiemvu.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
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
                if(imgqua.name == "hopqua")
                {
                    if(imgqua.GetComponent<Animator>() == false)
                    {
                        Animator anim = imgqua.gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = animquacam;
                    }    
                }
            }
            else if (json[i].Value == "dalam")
            {
                debug.Log("ok1");
                if(alloQua.transform.GetChild(i).GetComponent<Image>()) alloQua.transform.GetChild(i).GetComponent<Image>().enabled = false;
                debug.Log("ok2");
                alloQua.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                debug.Log("ok3");
                alloQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    void LoadDiemDanh(JSONNode json)
    {
        GameObject DiemDanh = giaodiennut1.transform.GetChild(15).transform.GetChild(0).gameObject;
        int solandiemdanh = int.Parse(json["event"]["landiemdanh"].Value);
        DiemDanh.transform.GetChild(1).GetComponent<Text>().text = "Bạn đã điểm danh <color=lime>" + solandiemdanh + " ngày</color>.";
        if (json["event"]["diemdanh"].Value == "True")
        {
            DiemDanh.transform.GetChild(2).GetComponent<Button>().interactable = true;
            DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + 0 + "/1</color>";
        }
        else
        {
            DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + 1 + "/1</color>";
            if (json["event"]["nhandiemdanh"][0].Value == "duocnhan") DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>().interactable = true;
            else if (json["event"]["nhandiemdanh"][0].Value == "danhan")
            {
                DiemDanh.transform.GetChild(3).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(3).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
            }
        }
        if (solandiemdanh < 7) DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/7</color>";
        else
        {
            DiemDanh.transform.GetChild(4).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/7</color>";
            if (json["event"]["nhandiemdanh"][1].Value == "duocnhan") DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>().interactable = true;
            else if (json["event"]["nhandiemdanh"][1].Value == "danhan")
            {
                DiemDanh.transform.GetChild(4).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(4).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
            }
        }
        if (solandiemdanh < 14) DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=red>" + solandiemdanh + "/14</color>";
        else
        {
            DiemDanh.transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tiến độ: <color=lime>" + solandiemdanh + "/14</color>";
            if (json["event"]["nhandiemdanh"][2].Value == "duocnhan") DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>().interactable = true;
            else if (json["event"]["nhandiemdanh"][2].Value == "danhan")
            {
                DiemDanh.transform.GetChild(5).transform.GetChild(2).GetComponent<Button>().interactable = false;
                DiemDanh.transform.GetChild(5).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
            }
        }
        if (json["event"]["nhandiemdanh"][3].Value == "duocnhan") DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>().interactable = true;
        else if (json["event"]["nhandiemdanh"][3].Value == "danhan")
        {
            DiemDanh.transform.GetChild(6).transform.GetChild(2).GetComponent<Button>().interactable = false;
            DiemDanh.transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
        }
        //     DiemDanh.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1);
    }
    public void DiemDanh()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "DiemDanhEvent/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    LoadDiemDanh(json);
                    CrGame.ins.OnThongBaoNhanh("Đã điểm danh!");
                    btn.interactable = false;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void NhanQuaDiemDanh()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int vitri = btn.transform.parent.GetSiblingIndex() - 3;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaDiemDanh/taikhoan/" + LoginFacebook.ins.id + "/vitri/" + vitri);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    if (json["theluc"].Value != "") giaodiennut1.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["theluc"].Value;
                    CrGame.ins.OnThongBaoNhanh("Đã nhận quà");
                    btn.interactable = false;
                    btn.transform.GetChild(0).GetComponent<Text>().text = "Đã nhận";
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void OpenMenuDoiQua()
    {
        GameObject menuDoiManh = GetCreateMenu("MenuDoiManh",transform.GetChild(1).transform,false);
        GameObject ContentManh = menuDoiManh.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject ObjectManh = ContentManh.transform.GetChild(0).gameObject;
        if (ContentManh.transform.childCount == 1)
        {
            CrGame.ins.panelLoadDao.SetActive(true);
            StartCoroutine(Load());
            IEnumerator Load()
            {
                UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "MenuDoiManhh/taikhoan/" + LoginFacebook.ins.id);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    debug.Log(www.error);
                    CrGame.ins.OnThongBaoNhanh("Lỗi");
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
                else
                {
                    // Show results as text
                    debug.Log(www.downloadHandler.text);
                    JSONNode json = JSON.Parse(www.downloadHandler.text);
                    if (json["status"].Value == "ok")
                    {
                        //  Button btnexit = menuDoiManh.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
                        //  btnexit.onClick.AddListener(delegate { DestroyMenu(); });

                       // Button btnXemQua = ObjectManh.GetComponent<Button>();
                       // btnXemQua.onClick.AddListener(XemManhDoi);
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
                                for (int j = 0; j < allitemEvent.Length; j++)
                                {
                                    if(json["ManhDoi"][i]["nameitem"].Value == allitemEvent[j].name)
                                    {
                                        imgmanh.sprite = allitemEvent[j];
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
                        CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    }
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
            }
        }
        else menuDoiManh.SetActive(true);
    }
    string namemanhchon = "";
    public void XemManhDoi()
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        GameObject yeucau = menuevent["MenuDoiManh"].transform.GetChild(1).gameObject;
        namemanhchon = btnchon.name;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemManhDoii/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + btnchon.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
            }
            else
            {
                // Show results as text
           //     Button btndoi = yeucau.transform.GetChild(4).GetComponent<Button>();
             //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);

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
                                int itemco = int.Parse(json["allitemevent"][json["manhdoi"]["itemcan"][i]["nameitem"].Value].Value);
                                if (itemco >= itemcan)
                                {
                                    sonvok += 1;
                                    txtyeucau.text = "<color=#00ff00ff>" + itemco + "/" + itemcan + "</color>";
                                } 
                                else txtyeucau.text = "<color=#ff0000ff>" + itemco + "/" + itemcan + "</color>";
                            }
                            else if (json["manhdoi"]["itemcan"][i]["loaiitem"].Value == "ItemNgoc")
                            {
                                for(var k = 0; k < NetworkManager.ins.inventory.contentNgoc.transform.childCount;k++)
                                {
                                    if(NetworkManager.ins.inventory.contentNgoc.transform.GetChild(k).name == json["manhdoi"]["itemcan"][i]["nameitem"].Value)
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
                                    else if(k == NetworkManager.ins.inventory.contentNgoc.transform.childCount - 1)
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
    }
    public void DoiManh()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "DoiManhh/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + namemanhchon);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                btndoi.interactable = true;
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject menudoimanh = menuevent["MenuDoiManh"];
                    menudoimanh.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = "";
                 //   menudoimanh.transform.GetChild(1).gameObject.SetActive(false);
                    menudoimanh.transform.GetChild(2).gameObject.SetActive(false);
                    GameObject yeucau = menudoimanh.transform.GetChild(1).gameObject;
                    for (int i = 0; i < yeucau.transform.childCount;i++)
                    {
                        yeucau.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    btndoi.interactable = true;

                    GameObject menucaunguyen = transform.GetChild(1).gameObject;
                    Text txtngoisao = menucaunguyen.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
                    txtngoisao.text = json["NgoiSaoUocNguyen"].Value;

                    GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
                    Text soluongthiephong = menuchonthiep.transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
                    Text soluongthiepvang = menuchonthiep.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
                    soluongthiephong.text = json["ThiepHong"].Value;
                    soluongthiepvang.text = json["ThiepVang"].Value;
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
    public void ThucHienNV()
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ThucHienNV/taikhoan/" + LoginFacebook.ins.id+"/key/"+ btn.transform.parent.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "1")
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].Value,2);
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
                    
                    GameObject menuchonrong = giaodiennut1.transform.GetChild(8).gameObject;
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
                    for(var i = 0; i < ContentRongChon.transform.childCount - 1;i ++)
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
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void ChonRong()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        GameObject menuchonrong = giaodiennut1.transform.GetChild(8).gameObject;
        GameObject allobj = menuchonrong.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject ContentRongDaChon = allobj.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
        GameObject ContentRongChon = allobj.transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;

        if(objchon.transform.parent.transform.parent.name == "ContentRongDaChon")
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
    public void VeNha()
    {
        giaodiennut1.transform.SetParent(gameObject.transform);
        CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(true);
        Vector3 vec = CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).transform.position;
        vec.z = -10;
        cam.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        //gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuEventSinhNhat2023");
    }
    // Update is called once per frame
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
        if(dem)
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
    public void Nhannv()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanNV/taikhoan/" + LoginFacebook.ins.id + "/vitri/" + vitrichon);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject menuxacnhan = giaodiennut1.transform.GetChild(7).gameObject;
                    menuxacnhan.SetActive(false);
                    GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject YeuCau = giaodiennut1.transform.GetChild(5).gameObject;
                //    YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Yêu cầu mở đường mốc thứ " + (int.Parse(json["event"]["odanglam"]) + 1);
                    LoadNhiemVu(json["mocnhiemvu"]);
                    GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                    allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;
                    YeuCau.transform.GetChild(0).GetComponent<Text>().text = "Yêu cầu mở đường mốc thứ " + alloQua.transform.GetChild(vitrichon).name;
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
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void MoQua()
    {
        if (!hoanthanh) return;
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemQua/taikhoan/" + LoginFacebook.ins.id + "/vitri/" + objchon.transform.GetSiblingIndex());
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
           //     debug.Log(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject menuxacnhan = giaodiennut1.transform.GetChild(7).gameObject;
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
                        for (int a = 0; a < allitemEvent.Length; a++)
                        {
                            if (allitemEvent[a].name == json["qua"]["nameitem"].Value)
                            {
                                imgqua.sprite = allitemEvent[a];
                                break;
                            }
                         }
                     
                        txtsoluong.text = json["qua"]["soluong"].Value;
                        imgqua.SetNativeSize();
                    }
                    else if (json["qua"]["loaiitem"].Value == "KhungAvatar")
                    {
                        Friend.ins.LoadImage("khungavt", json["qua"]["nameitem"].Value,imgqua);
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
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void BatDau()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "BatDau/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                //    CrGame.ins.OnThongBaoNhanh(json["thongbao"].Value, 2);
                    objchon.gameObject.SetActive(false);
                    objchon.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
                    objchon.transform.parent.transform.GetChild(2).gameObject.SetActive(true);
                    timeSec = float.Parse(json["timeSec"].Value);
                    Text txtTheLuc = giaodiennut1.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    txtTheLuc.text = json["theluc"].Value;
                    txttime.transform.GetChild(2).GetComponent<Button>().interactable = false;
                    dem = true;
                  
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                }    
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void XacNhanRong()
    {
        GameObject menuchonrong = giaodiennut1.transform.GetChild(8).gameObject;
        GameObject allobj = menuchonrong.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject ContentRongDaChon = allobj.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
        //GameObject ContentRongChon = allobj.transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        string allrong = "";
        for (int i = 0; i < ContentRongDaChon.transform.childCount; i++)
        {
            if(ContentRongDaChon.transform.GetChild(i).transform.childCount > 0)
            {
                allrong += ContentRongDaChon.transform.GetChild(i).transform.GetChild(0).name + "*";
            }    
        }
       
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NVRONG/taikhoan/" + LoginFacebook.ins.id + "/allrong/" + allrong + "/nhiemvu/" + menuchonrong.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    string[] allrongxoa = allrong.Split('*');
                    for (int i = 0; i < allrongxoa.Length; i++)
                    {
                        if(allrongxoa[i] != "")
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
                    GameObject YeuCau = giaodiennut1.transform.GetChild(5).gameObject;
                    GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                    for (int i = 0; i < allnhiemvu.transform.childCount; i++)
                    {
                        if(allnhiemvu.transform.GetChild(i).gameObject.name == json["nhiemvu"].Value)
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
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    bool hoanthanh = true;
    public void HoanThanh()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "HoanThanh/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    hoanthanh = false;
                    txttime.transform.GetChild(0).gameObject.SetActive(true);
                    txttime.transform.GetChild(1).gameObject.SetActive(true);
                    txttime.transform.GetChild(2).gameObject.SetActive(false);
                    txttime.text = "Yêu cầu: 8 giờ";
                    dem = false;
              
                    GameObject YeuCau = giaodiennut1.transform.GetChild(5).gameObject;
                    YeuCau.SetActive(false);

                    GameObject alloQua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject imgqua = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
                    int vitri = int.Parse(json["vitri"].Value);
                    if(alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>())
                    {
                        alloQua.transform.GetChild(vitri).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().Play("quamo");
                    }
                    CrGame.ins.panelLoadDao.SetActive(false);
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

                     LoadMocNv(json["infomocnv"]);
                    hoanthanh = true;
                    // CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    float BinhNangLuongchon,giamthoigian,maxgiam;
    public void OpenMenuBonPhan()
    {
        GameObject menubonphan = giaodiennut1.transform.GetChild(9).gameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetBinhNangLuong/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
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
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void TangSoLuongBinhNangLuong(int i)
    {
        GameObject menubonphan = giaodiennut1.transform.GetChild(9).gameObject;
        //int sophanbonchon = int.Parse(menubonphan.transform.GetChild(5).GetComponent<Text>().text);

        if (BinhNangLuongchon >= 1 && giamthoigian < maxgiam)
        {
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
        GameObject menubonphan = giaodiennut1.transform.GetChild(9).gameObject;
        int sophanbonchon = int.Parse(menubonphan.transform.GetChild(5).GetComponent<Text>().text);
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "SuDungBinhNangLuong/taikhoan/" + LoginFacebook.ins.id + "/sobinh/" + sophanbonchon);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    CrGame.ins.OnThongBaoNhanh("Giảm thời gian <color=lime>+" + sophanbonchon + " phút</color>", 2);
                    GameObject YeuCau = giaodiennut1.transform.GetChild(5).gameObject;
                    GameObject allnhiemvu = YeuCau.transform.GetChild(2).gameObject;
                    allnhiemvu.transform.GetChild(allnhiemvu.transform.childCount - 1).GetComponent<Text>().text = json["time"].Value;
                    menubonphan.SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
   
    public void OpenMenuChonThiep()
    {
        GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
        menuchonthiep.gameObject.SetActive(true);
        menuchonthiep.transform.GetChild(0).transform.GetChild(1).transform.GetChild(3).GetComponent<InputField>().text = "0";
        menuchonthiep.transform.GetChild(0).transform.GetChild(1).transform.GetChild(5).GetComponent<Button>().interactable = false;
        menuchonthiep.transform.GetChild(0).transform.GetChild(2).transform.GetChild(5).GetComponent<Button>().interactable = false;
        menuchonthiep.transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).GetComponent<InputField>().text = "0";
        soluongchon = 0;
    }
    bool loadfriend = false;
    public void ChonThiep()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;

        if(loadfriend)
        {
            ThiepChon = btnchon.transform.parent.name;
            soluongchon = int.Parse(btnchon.transform.parent.GetChild(3).GetComponent<InputField>().text);
            GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
            menuchonthiep.SetActive(false);
            giaodiennut1.transform.GetChild(11).gameObject.SetActive(true);
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
                    GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
                    menuchonthiep.SetActive(false);
                    giaodiennut1.transform.GetChild(11).gameObject.SetActive(true);

                    JSONNode allfriend = json["data"]["allfriend"];
                    GameObject menufriend = giaodiennut1.transform.GetChild(11).gameObject;
                    GameObject viewport = menufriend.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject;
                    GameObject ObjFriend = viewport.transform.GetChild(0).gameObject;
                    GameObject Content = viewport.transform.GetChild(1).gameObject;
                    for (int i = 1; i < allfriend.Count; i++)
                    {
                        GameObject Offriend = Instantiate(ObjFriend, Content.transform.position, Quaternion.identity) as GameObject;
                        Offriend.transform.SetParent(Content.transform, false);

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
                    loadfriend = true;
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
    public void TangThiep()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        string taikhoanban = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "TangThiep/taikhoan/" + LoginFacebook.ins.id + "/namephao/" + ThiepChon + "/soluong/" + soluongchon + "/taikhoanban/" + taikhoanban);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                GameObject menubanbe = giaodiennut1.transform.GetChild(11).gameObject;
                menubanbe.SetActive(false);

                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
                    Text soluongthiephong = menuchonthiep.transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
                    Text soluongthiepvang = menuchonthiep.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
                    soluongthiephong.text = json["ThiepHong"].Value;
                    soluongthiepvang.text = json["ThiepVang"].Value;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    bool sangtrang = false; int trang = 1, trangg = 1; float top, topcuoi;
    public void ExitMenuTop()
    {
        GameObject menutop = giaodiennut1.transform.GetChild(12).gameObject;
        sangtrang = false; trang = 1; trangg = 1; top = 0; topcuoi = 0;
        GameObject contentnhatki = menutop.transform.GetChild(0).transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetChild(1).gameObject;
        for (int i = 0; i < contentnhatki.transform.childCount; i++)
        {
            Destroy(contentnhatki.transform.GetChild(i).gameObject);
        }
        menutop.SetActive(false);
    }
    public void OpenMenuTop()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            //  CrGame.ins.OnThongBao(true, "Đang tải...", false);
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetTopEvent/taikhoan/" + LoginFacebook.ins.id + "/top/" + top + "/trang/" + trangg);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
            }
            else
            {
                // Show results as text 
                sangtrang = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                //GameObject objecttop = transform.GetChild(5).transform.GetChild(6).gameObject;
                GameObject menu = giaodiennut1.transform.GetChild(12).gameObject;
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
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].Value;
                            debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                            debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = i + trang;
                            txtTop.text = sotop.ToString();
                            debug.Log("ok3");
                            NetworkManager.ins.friend.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar,imgKhungAvatar);
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
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["diemtangthiep"].Value;
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
                    GameObject objquaphao = menu.transform.GetChild(0).transform.GetChild(6).transform.GetChild(1).gameObject;
                    objquaphao.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["duoctangthiep"].Value;//json["quatichluythiep"]
                    Image fillaoumt = objquaphao.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
                    fillaoumt.fillAmount = float.Parse(json["duoctangthiep"].Value) / 250;
                    GameObject allqua = objquaphao.transform.GetChild(2).gameObject;

                    for (int i = 0; i < json["quatichluythiep"].Count; i++)
                    {
                        if (json["quatichluythiep"][i].Value == "chuaduocnhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = chuaduocnhan;
                            break;
                        }
                        if (json["quatichluythiep"][i].Value == "duocnhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = duocnhan;
                            allqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                            allqua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                            allqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            // else allqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                        else if (json["quatichluythiep"][i].Value == "danhan")
                        {
                            string name = allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name;
                            if (name == "chuaduocnhan" || name == "duocnhan" || name == "danhan") allqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = danhan;
                            allqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                            allqua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                            allqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            //else allqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                    }
                }



                menu.SetActive(true);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void NhanQuaThiep()
    {
        GameObject btntreo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaTichLuyThiep/taikhoan/" + LoginFacebook.ins.id + "/qua/" + btntreo.transform.parent.GetSiblingIndex());
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    btntreo.gameObject.SetActive(false);
                    btntreo.transform.parent.transform.GetChild(2).gameObject.SetActive(true);
                    btntreo.transform.parent.transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                    btntreo.transform.parent.transform.GetChild(0).GetComponent<Image>().sprite = danhan;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void ThemSoLuongDotPhao(int i)
    {
        if (i == 0)
        {
            GameObject ObjPhaoo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
            if (ObjPhaoo.name == "ThiepHong" || ObjPhaoo.name == "ThiepVang")
            {
                ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text = ObjPhaoo.transform.GetChild(2).GetComponent<Text>().text;
                //if (ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text == "0") ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text = "1";
                //int Soluongdachonn = int.Parse(ObjPhaoo.transform.GetChild(3).GetComponent<InputField>().text);
                //if (int.Parse(ObjPhaoo.transform.GetChild(2).GetComponent<Text>().text) <= Soluongdachonn)
                //{
                //    ObjPhaoo.transform.GetChild(3).transform.GetComponent<InputField>().text = ObjPhaoo.transform.GetChild(2).GetComponent<Text>().text;
                //}
                //ObjPhaoo.transform.GetChild(3).transform.GetComponent<InputField>().text = int.Parse(ObjPhaoo.transform.GetChild(3).transform.GetComponent<InputField>().text).ToString();
            }
            return;
        }
        GameObject ObjPhao = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject;
        debug.Log(ObjPhao.name);
        int Soluongdachon = int.Parse(ObjPhao.transform.GetChild(3).GetComponent<InputField>().text);
        if (i > 0)
        {
            if (int.Parse(ObjPhao.transform.GetChild(2).GetComponent<Text>().text) > Soluongdachon)
            {
                debug.Log(ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text);
                Soluongdachon += 1;
                ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text = Soluongdachon.ToString();
            }
            // else ObjPhao.transform.GetChild(3).transform.GetComponent<InputField>().text = ObjPhao.transform.GetChild(2).GetComponent<Text>().text;
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
    public void OpenMenuCauNguyen()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "MenuCauNguyen/id/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                   // giaodiennut1.transform.position = giaodiennut1.transform.parent.transform.position;
                    GameObject menucaunguyen = transform.GetChild(1).gameObject;
                    Text txtngoisao = menucaunguyen.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
                    Text txtquacau = menucaunguyen.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
                    txtngoisao.text = json["NgoiSaoUocNguyen"].Value;
                    txtquacau.text = json["QuaCauMongUoc"].Value;
                    giaodiennut1.SetActive(false);
                    transform.GetChild(0).transform.GetComponent<Image>().raycastTarget = false;
                    cam.transform.position = new Vector3(0, 0, -10);
                    //btnHopQua.transform.SetParent(menucaunguyen.transform);
                    LoadTichLuyUocNguyen(json["quatichluyuoc"], int.Parse(json["solanuoc"].Value));
                    menucaunguyen.SetActive(true);
                    if (json["yeucauX1"]["name"].Value == "KimCuong")
                    {
                        menucaunguyen.transform.GetChild(7).transform.GetChild(2).gameObject.SetActive(false);
                        menucaunguyen.transform.GetChild(7).transform.GetChild(3).gameObject.SetActive(true);
                    }
                    else
                    {
                        menucaunguyen.transform.GetChild(7).transform.GetChild(2).gameObject.SetActive(true);
                        menucaunguyen.transform.GetChild(7).transform.GetChild(3).gameObject.SetActive(false);
                    }
                    menucaunguyen.transform.GetChild(7).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu: " + json["yeucauX1"]["soluong"].Value;
                    if (json["yeucauX10"]["name"].Value == "KimCuong")
                    {
                        menucaunguyen.transform.GetChild(8).transform.GetChild(2).gameObject.SetActive(false);
                        menucaunguyen.transform.GetChild(8).transform.GetChild(3).gameObject.SetActive(true);
                    }
                    else
                    {
                        menucaunguyen.transform.GetChild(8).transform.GetChild(2).gameObject.SetActive(true);
                        menucaunguyen.transform.GetChild(8).transform.GetChild(3).gameObject.SetActive(false);
                    }
                    menucaunguyen.transform.GetChild(8).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu: " + json["yeucauX10"]["soluong"].Value;
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void CloseCauNguyen()
    {
      //  giaodiennut1.transform.position = transform.GetChild(0).position;
        giaodiennut1.SetActive(true);
        GameObject img = gameObject.transform.GetChild(0).gameObject;
        img.gameObject.SetActive(true);
        //btnHopQua.transform.SetParent(giaodiennut1.transform);
        transform.GetChild(1).gameObject.SetActive(false);
        cam.transform.position = new Vector3(img.transform.position.x, img.transform.position.y, -10);
        DestroyMenu("MenuDoiManh");
    }
    bool duocquay = true;
    public void UocNguyen(int solanquay)
    {
        if (!duocquay) return;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "CauNguyen/id/" + LoginFacebook.ins.id + "/quayx/X" + solanquay);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    CrGame.ins.panelLoadDao.SetActive(false);
                    duocquay = false;
                    GameObject menucaunguyen = transform.GetChild(1).gameObject;
                    Text txtquacau = menucaunguyen.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
                    LoadTichLuyUocNguyen(json["quatichluyuoc"],int.Parse(json["solanuoc"].Value));
                    txtquacau.text = json["QuaCauMongUoc"].Value;

                    JSONNode allqua = json["VatPhamQuayDuoc"];
                    GameObject panelnhanqua = transform.GetChild(2).gameObject;

                    panelnhanqua.SetActive(true);
                    yield return new WaitForSeconds(0.3f);
                    GameObject objallqua = panelnhanqua.transform.GetChild(1).gameObject;
                    objallqua.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
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
                        // if (objtrong.transform.Find("vien")) Destroy(objtrong.transform.Find("vien"));
                        if (allqua[i]["loai"].Value == "Item" || allqua[i]["loai"].Value == "ngoc")
                        {
                            objtrong.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(allqua[i]["name"].Value);
                            objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["soluong"].Value;
                            objtrong.name = "item" + allqua[i]["name"].Value;
                            objtrong.GetComponent<infoitem>().enabled = true;
                        }
                        else if (allqua[i]["loai"].Value == "ItemRong")
                        {
                            objtrong.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(allqua[i]["name"].Value + "1");
                            objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["sao"].Value + "sao";
                            objtrong.GetComponent<infoitem>().enabled = false;
                        }
                        else if (allqua[i]["loai"].Value == "ItemEvent")
                        {
                            for (int j = 0; j < allitemEvent.Length; j++)
                            {
                                if (allitemEvent[j].name == allqua[i]["name"].Value)
                                {
                                    objtrong.transform.GetChild(0).GetComponent<Image>().sprite = allitemEvent[j];
                                    objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["soluong"].Value;
                                    objtrong.GetComponent<infoitem>().enabled = false;
                                    objtrong.name = allqua[i]["name"].Value;
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
                        if (json["yeucauX1"]["name"].Value == "KimCuong")
                        {
                            menucaunguyen.transform.GetChild(7).transform.GetChild(2).gameObject.SetActive(false);
                            menucaunguyen.transform.GetChild(7).transform.GetChild(3).gameObject.SetActive(true);
                        }
                        else
                        {
                            menucaunguyen.transform.GetChild(7).transform.GetChild(2).gameObject.SetActive(true);
                            menucaunguyen.transform.GetChild(7).transform.GetChild(3).gameObject.SetActive(false);
                        }
                        menucaunguyen.transform.GetChild(7).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu: " + json["yeucauX1"]["soluong"].Value;
                        if (json["yeucauX10"]["name"].Value == "KimCuong")
                        {
                            menucaunguyen.transform.GetChild(8).transform.GetChild(2).gameObject.SetActive(false);
                            menucaunguyen.transform.GetChild(8).transform.GetChild(3).gameObject.SetActive(true);
                        }
                        else
                        {
                            menucaunguyen.transform.GetChild(8).transform.GetChild(2).gameObject.SetActive(true);
                            menucaunguyen.transform.GetChild(8).transform.GetChild(3).gameObject.SetActive(false);
                        }
                        menucaunguyen.transform.GetChild(8).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu: " + json["yeucauX10"]["soluong"].Value;
                        yield return new WaitForSeconds(0.02f);
                        qua.transform.GetChild(0).gameObject.SetActive(true);
                        yield return new WaitForSeconds(0.1f);
                    }
                    objallqua.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
            

            }
        }
    
    }
    public void sangtrangtop(int i)
    {
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
                OpenMenuTop();
            }
            //   debug.Log(i);
        }
    }
    void LoadTichLuyUocNguyen(JSONNode json, int soluotdaquay)
    {
        if (soluotdaquay >= 1000) soluotdaquay = 1000;
        GameObject menucaunguyen = transform.GetChild(1).gameObject;
        GameObject allqua = menucaunguyen.transform.GetChild(6).transform.GetChild(2).gameObject;
        GameObject alltext = menucaunguyen.transform.GetChild(6).transform.GetChild(3).gameObject;
        Image imgload = menucaunguyen.transform.GetChild(6).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();

        int quayden = 1;
        int truthem = 1;
        if (soluotdaquay >= 350)
        {
            soluotdaquay += 50;
            quayden = 7;
           
        }
        if (soluotdaquay >= 700)
        {
            quayden = 14;
          
        }
        for (int i = quayden; i < quayden + 7; i++)
        {
            if (json[i - truthem] == "chuaduocnhan")
            {
                allqua.transform.GetChild(i - quayden).GetComponent<Image>().sprite = chuaduocnhan;
            }
            else if (json[i - truthem] == "danhan")
            {
                allqua.transform.GetChild(i - quayden).GetComponent<Image>().sprite = danhan;
            }
            alltext.transform.GetChild(i - quayden).GetComponent<Text>().text = (i * 50) + "";
        }
        if(soluotdaquay < 1000)
        {
            while (soluotdaquay >= 350)
            {
                soluotdaquay -= 350;
            }
        }
       
        //  if (soluotdaquay >= 350) soluotdaquay -= 350;
        imgload.fillAmount = (float)soluotdaquay / 350;
    }
    public void XacNhanQua()
    {
        GameObject hopqua = btnHopQua;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject panelnhanqua = transform.GetChild(2).gameObject;
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
            duocquay = true;
            for (int i = 1; i < objallqua.transform.childCount; i++)
            {
                Destroy(objallqua.transform.GetChild(i).gameObject);
            }
        }
        IEnumerator delaydestroy(GameObject qua)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject quatrong = qua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            GameObject namequa = qua.transform.GetChild(0).transform.GetChild(0).gameObject;
            debug.Log(namequa.name);
            if(namequa.name == "ThiepVang")
            {
                GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
                Text soluongthiepvang = menuchonthiep.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
                soluongthiepvang.text = int.Parse(soluongthiepvang.text) + int.Parse(namequa.transform.GetChild(1).GetComponent<Text>().text) + "";
            }
            else if (namequa.name == "ThiepHong")
            {
                GameObject menuchonthiep = giaodiennut1.transform.GetChild(10).gameObject;
                Text soluongthiephong = menuchonthiep.transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
                soluongthiephong.text = int.Parse(soluongthiephong.text) + int.Parse(namequa.transform.GetChild(1).GetComponent<Text>().text) + "";
            }
            else if (namequa.name == "NgoiSaoUocNguyen")
            {
                GameObject menucaunguyen = transform.GetChild(1).gameObject;
                Text txtngoisao = menucaunguyen.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
                txtngoisao.text = int.Parse(txtngoisao.text) + int.Parse(namequa.transform.GetChild(1).GetComponent<Text>().text) + "";
            }
            quatrong.transform.SetParent(CrGame.ins.panelLoadDao.transform.parent.transform, false);
            QuaBay quabay = quatrong.AddComponent<QuaBay>();
            quabay.vitribay = hopqua;
            qua.SetActive(false);
            //  Destroy(qua, 5);
        }
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        Button btnTrieuHoi = giaodiennut1.transform.GetChild(14).GetChild(0).transform.GetChild(3).GetComponent<Button>();
        GameObject AllmanhRong = giaodiennut1.transform.GetChild(14).GetChild(0).transform.GetChild(2).gameObject;
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
        giaodiennut1.transform.GetChild(14).gameObject.SetActive(true);
    }
    public void TrieuHoiRong()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        GameObject AllmanhRong = giaodiennut1.transform.GetChild(14).GetChild(0).transform.GetChild(2).gameObject;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "TrieuHoiRongEventTet/taikhoan/" + LoginFacebook.ins.id + "/namerong/" + AllmanhRong.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                btndoi.interactable = true;
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"] == "0")
                {
                    giaodiennut1.transform.GetChild(14).GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                    StartCoroutine(HieuUngTrieuHoi());
                    if (AllmanhRong.name == "vang")
                    {
                        string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            NetworkManager.ins.inventory.AddItem(allnamemanh[i], -1);
                        }
                    }
                    else if (AllmanhRong.name == "bac")
                    {
                        string[] allnamemanh = new string[] { "DauRongBac", "CanhRongBac", "ChanRongBac", "ThanRongBac", "DuoiRongBac" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            NetworkManager.ins.inventory.AddItem(allnamemanh[i], -1);
                        }
                    }
                }
                else CrGame.ins.OnThongBaoNhanh(json["thongbao"], 2);
                IEnumerator HieuUngTrieuHoi()
                {
                    yield return new WaitForSeconds(0.5f);
                    GameObject g = Instantiate(giaodiennut1.transform.GetChild(14).GetChild(1).gameObject, transform.position, Quaternion.identity);//
                    g.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
                    g.gameObject.SetActive(true);
                    Image img = g.GetComponent<Image>();
                    img.sprite = Inventory.LoadSpriteRong(json["namerong"]);
                    img.SetNativeSize();
                    giaodiennut1.transform.GetChild(14).GetChild(0).transform.GetChild(4).gameObject.SetActive(false);//
                    yield return new WaitForSeconds(0.1f);
                    QuaBay quabay = img.GetComponent<QuaBay>();
                    quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                    quabay.enabled = true;
                    giaodiennut1.transform.GetChild(14).gameObject.SetActive(false);//
                }
            }
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
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/EventSinhNhat2023/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
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
    public void DestroyMenu(string namemenu)
    {
        if (menuevent.ContainsKey(namemenu))
        {
            Destroy(menuevent[namemenu]);
            menuevent.Remove(namemenu);

            Resources.UnloadUnusedAssets();
        }
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
}
