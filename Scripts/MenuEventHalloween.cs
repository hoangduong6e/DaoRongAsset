using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEventHalloween : MonoBehaviour
{
    // Start is called before the first frame update
    Inventory inventory; CrGame crgame;public Sprite[] spriteXucXac;
    GameObject btnHopQua; public GameObject Allnhiemvu, ContentManh, ObjectManh; int quayden = 1;
    public Button btnquay;public Sprite[] spriteItemEvent;string namemanhchon;
    public Sprite SaoSang, SaoToi, sprite1, sprite2, sprirehopquaduocnhan, sprirehopquachuanhan, sprirehopquadanhan;
    int trang = 1;bool gettop = true;int trangg = 1;
    public Sprite Top1, Top2, Top3,Top;int top,topcuoi;public Text txttimedemnguoc;public float timedemnguoc = 0;
    VienChinh vienchinh;NetworkManager net; public bool timerIsRunning = false;
    void Awake()
    {
        crgame = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CrGame>();
        inventory = crgame.GetComponent<Inventory>();
        net = crgame.GetComponent<NetworkManager>();
        btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
        vienchinh = GameObject.FindGameObjectWithTag("vienchinh").GetComponent<VienChinh>();
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timedemnguoc > 0)
            {
                timedemnguoc -= Time.deltaTime;
                DisplayTime(timedemnguoc);
            }
            else
            {
                debug.Log("Time has run out!");
                timedemnguoc = 0;
                txttimedemnguoc.transform.parent.transform.GetChild(2).GetComponent<Button>().interactable = true ;
                timerIsRunning = false;
               // gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
            //    gameObject.SetActive(false);
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        int gio = 0;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        while(minutes >= 60)
        {
            minutes -= 60;
            gio += 1;
        }    
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        txttimedemnguoc.text = gio + ":" + minutes + ":" + seconds;
       // txttimedemnguoc.text = string.Format("{0:00}:{0:00}:{1:00}",gio, minutes, seconds);
    }
    private void OnEnable()
    {
        //crgame.panelLoadDao.SetActive(true);
        StartCoroutine(GetTop());
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "GetDataEvent/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                //crgame.panelLoadDao.SetActive(false);
                crgame.menulogin.SetActive(false);
                crgame.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                gameObject.SetActive(false);
            }
            else
            {
                // Show results as text
                btnHopQua.transform.SetParent(transform.GetChild(0).transform);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                for (int i = 0; i < json["allNhiemvu"].Count; i++)
                {
                    Text txttiendo = Allnhiemvu.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    if (int.Parse(json["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["allNhiemvu"][i]["maxnhiemvu"].Value))
                    {
                        txttiendo.text = "<color=#00ff00ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                }
                if (ContentManh.transform.childCount == 1)
                {
                    for (int i = 0; i < json["ManhDoi"].Count; i++)
                    {
                        GameObject manh = Instantiate(ObjectManh, ContentManh.transform.position, Quaternion.identity);
                        manh.transform.SetParent(ContentManh.transform, false);
                        Image imgmanh = manh.transform.GetChild(0).GetComponent<Image>();
                        if (json["ManhDoi"][i]["itemgi"].Value == "item" || json["ManhDoi"][i]["itemgi"].Value == "thuyenthucan")
                        {
                            imgmanh.sprite = Inventory.LoadSprite(json["ManhDoi"][i]["nameitem"].Value);
                        }
                        else if(json["ManhDoi"][i]["itemgi"].Value == "itemevent")
                        {
                            for (int j = 0; j < spriteItemEvent.Length; j++)
                            {
                                if (spriteItemEvent[j].name == json["ManhDoi"][i]["nameitem"].Value)
                                {
                                    imgmanh.sprite = spriteItemEvent[j];
                                }
                            }
                        }    
                        else
                        {
                            imgmanh.sprite = Inventory.LoadSpriteRong(json["ManhDoi"][i]["nameitem"].Value + "2");
                        }
                        manh.name = json["ManhDoi"][i]["namekey"];
                        manh.SetActive(true);
                    }
                }

                Text bonuskhichiendau = transform.GetChild(5).transform.GetChild(10).transform.GetChild(1).GetComponent<Text>();

                bonuskhichiendau.text = "Xuyên giáp: +" + "<color=#00ffffff>" + json["boss"]["Buffxuyengiap"] +"</color> \n " +
                    "Tấn công: +" + "<color=#ff0000ff>" + json["boss"]["Buffdame"] + "</color> \n " +
                    "Chí mạng: +" + "<color=#ff00ffff>" + json["boss"]["Buffchimang"] + "%</color> \n " +
                    "Thời gian: +" + "<color=#ffff00ff>" + json["boss"]["Bufftime"] + " giây</color>";

                //   Text txttimedemnguoc = transform.GetChild(5).transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
                timedemnguoc = float.Parse(json["timemoboss"].Value) * 60;
                if(timedemnguoc > 0)
                {
                    txttimedemnguoc.transform.parent.transform.GetChild(2).GetComponent<Button>().interactable = false;
                }
                //for (int i = 0; i < json["boss"]["Map"].Count; i++)
                //{
                //    debug.Log(json["boss"]["Map"][i]);
                //}
                transform.GetChild(5).transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = json["timetop"].Value;

                LoadSoSao(json["boss"]);

                transform.GetChild(0).transform.GetChild(11).transform.GetChild(1).GetComponent<Text>().text = json["thongbao"].Value;
                transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).GetComponent<Text>().text = json["bingo"].Value;
                quayden = int.Parse(json["diemquayden"].Value) + 1;
                transform.GetChild(0).transform.GetChild(3).transform.position = new Vector2(transform.GetChild(0).transform.GetChild(0).transform.GetChild(quayden - 1).transform.position.x, transform.GetChild(0).transform.GetChild(0).transform.GetChild(quayden - 1).transform.position.y + 0.09f);
                //  if (int.Parse(json["soluotquaymay"].Value) > 0) transform.GetChild(0).transform.GetChild(2).GetComponent<Button>().interactable = true;
                //     else transform.GetChild(0).transform.GetChild(2).GetComponent<Button>().interactable = false;
                debug.Log(www.downloadHandler.text);
                transform.GetChild(0).gameObject.SetActive(true);
                //crgame.panelLoadDao.SetActive(false);
                crgame.menulogin.SetActive(false);
                AudioManager.SetSoundBg("nhacnen1");
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }
    public void LoadSoSao(JSONNode json)
    {

        GameObject Map = transform.GetChild(4).gameObject;
        float sosaoo = 0;
        int ok = 0;
        for (int i = 0; i < 5; i++)
        {
            int sao = int.Parse(json["Map"][i]["sao"].Value);
            if (sao > 0)
            {
                for (int j = 0; j < sao; j++)
                {
                    Map.transform.GetChild(0).transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().sprite = SaoSang;
                }
                sosaoo += sao;
                ok += 1;
                if(ok >= 5)
                {
                    Map.transform.GetChild(0).transform.GetChild(5).GetComponent<Button>().enabled = true;
                    Map.transform.GetChild(0).transform.GetChild(5).transform.GetChild(1).gameObject.SetActive(false);
                    timerIsRunning = true;
                }    
            }
        }
       // debug.Log(sosaoo / 15);
        transform.GetChild(4).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = sosaoo + "";
        transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = sosaoo / 15;
        debug.Log(transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).name);
        GameObject contentQua = transform.GetChild(4).transform.GetChild(2).transform.GetChild(2).gameObject;

        Image img = contentQua.transform.GetChild(0).transform.GetChild(3).GetComponent<Image>();
        for (int i = 0; i < json["QuaTichLuy"].Count; i++)
        {
            
            Image imghopqua = contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();

            if (json["QuaTichLuy"][i].Value == "chuaduocnhan")
            {
                imghopqua.sprite = sprirehopquachuanhan;
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                img.sprite = sprite1;
            }
            else if (json["QuaTichLuy"][i].Value == "duocnhan")
            {
                imghopqua.sprite = sprirehopquaduocnhan;
                Button btnnhan = contentQua.transform.GetChild(i).transform.GetChild(1).GetComponent<Button>();
                btnnhan.gameObject.SetActive(true);
             //   btnnhan.onClick.AddListener(NhanQuaTichLuy);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                img.sprite = sprite2;
            }
            else if (json["QuaTichLuy"][i].Value == "danhan")
            {
                imghopqua.sprite = sprirehopquadanhan;
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                contentQua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                img.sprite = sprite2;
            }
            img.SetNativeSize();
            imghopqua.SetNativeSize();
            if (i < json["QuaTichLuy"].Count - 1) img = contentQua.transform.GetChild(i + 1).transform.GetChild(3).GetComponent<Image>();
            //   contentQua.transform.GetChild(i).transform.GetChild(3)
        }
       
    }
    public void NhanQuaTichLuy()
    {
        Button btnnhan = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int sibling = btnnhan.transform.parent.GetSiblingIndex();
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "NhanQuaTichLuy/taikhoan/" + LoginFacebook.ins.id + "/qua/" + sibling);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.panelLoadDao.SetActive(false);
                crgame.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
            }
            else
            {
                GameObject contentQua = transform.GetChild(4).transform.GetChild(2).transform.GetChild(2).gameObject;
                // Show results as text
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    btnnhan.onClick.RemoveAllListeners();
                    btnnhan.gameObject.SetActive(false);
                    contentQua.transform.GetChild(sibling).transform.GetChild(1).gameObject.SetActive(false);
                    contentQua.transform.GetChild(sibling).transform.GetChild(2).gameObject.SetActive(true);
                    contentQua.transform.GetChild(sibling).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                }
                else crgame.OnThongBaoNhanh(json["status"].Value);
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
    public void Quay()
    {
        btnquay.interactable = false;
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "TungXucXac/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
                crgame.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    QuayXucXac(int.Parse(json["xucxac"].Value) - 1,json);
                    transform.GetChild(0).transform.GetChild(11).transform.GetChild(1).GetComponent<Text>().text = json["thongbao"].Value;
                    transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).GetComponent<Text>().text = json["bingo"].Value;
                    CongDiem("1");
                }
                else
                {
                    crgame.OnThongBaoNhanh(json["status"].Value);
                }
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
    public void QuayXucXac(int xucxac,JSONNode json)
    {
        Image imgXucXac = transform.GetChild(0).transform.GetChild(2).GetComponent<Image>();
        GameObject vienodangdung = transform.GetChild(0).transform.GetChild(3).gameObject;
        GameObject oquuay = transform.GetChild(0).transform.GetChild(0).gameObject;
        imgXucXac.gameObject.SetActive(false);
     
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            //for (int i = 0; i < 15; i++)
            //{
            //    imgXucXac.sprite = spriteXucXac[Random.Range(0,6)];
            //    yield return new WaitForSeconds(0.2f);
            //}
            transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
            imgXucXac.SetNativeSize();
            yield return new WaitForSeconds(1.05f);
       
            imgXucXac.sprite = spriteXucXac[xucxac];
            imgXucXac.gameObject.SetActive(true);
            transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            //Image imghientai = transform.GetChild(0).transform.GetChild(0).transform.GetChild(quayden).GetComponent<Image>();
            //if (quayden == 7 || quayden == 11 || quayden == 18 || quayden == 22) imghientai.sprite = obingo;
            //else imghientai.sprite = oxanh;

            int SoXucXac = int.Parse(imgXucXac.sprite.name);
           
            for (int i = 0; i < SoXucXac + 1; i++)
            {
                //Image imgo = transform.GetChild(0).transform.GetChild(0).transform.GetChild(quayden).GetComponent<Image>();
                //if (imgo.sprite.name == "obingo") imgo.sprite = obingodo;
                //else imgo.sprite = odo;
                yield return new WaitForSeconds(0.2f);
                if(i < SoXucXac)
                {
                    //if (quayden == 7 || quayden == 11 || quayden == 18 || quayden == 22) imgo.sprite = obingo;
                    //else imgo.sprite = oxanh;
                    vienodangdung.transform.position = new Vector2(oquuay.transform.GetChild(quayden).transform.position.x, oquuay.transform.GetChild(quayden).transform.position.y + 0.09f);
                    quayden += 1;
                    if (quayden > 22)
                    {
                        //Image img = transform.GetChild(0).transform.GetChild(0).transform.GetChild(quayden).GetComponent<Image>();
                        //if (quayden == 7 || quayden == 11 || quayden == 18 || quayden == 22) img.sprite = obingo;
                        //else img.sprite = oxanh;
                        quayden = 1;
                    }
                }
            }
            if (json["randombingo"].Value != "")
            {
                int randombingo = 23 + int.Parse(json["randombingo"].Value);
                vienodangdung.transform.position = new Vector2(oquuay.transform.GetChild(randombingo).transform.position.x, oquuay.transform.GetChild(randombingo).transform.position.y + 0.09f);
                CongDiem(oquuay.transform.GetChild(randombingo).transform.GetChild(1).GetComponent<Text>().text);
            }
            yield return new WaitForSeconds(0.3f);
            btnquay.interactable = true;
        }
    }
    public void CongDiem(string diem)
    {
        GameObject hieuung = transform.GetChild(0).transform.GetChild(4).transform.GetChild(3).gameObject;
        hieuung.SetActive(true);
        hieuung.transform.GetChild(0).GetComponent<Text>().text = "+"+diem;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.7f);
            hieuung.SetActive(false);
        }
    }    
    public void OpenMenuDoiManhRong(string namerong)
    {
        crgame.MenuDoiRongVangBac(namerong,transform.GetChild(1).gameObject);
    }
    public void TrieuHoiRong()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        GameObject AllmanhRong = transform.GetChild(1).GetChild(0).transform.GetChild(2).gameObject;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "TrieuHoiRongEventTet/taikhoan/" + LoginFacebook.ins.id + "/namerong/" + AllmanhRong.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
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
                    transform.GetChild(1).GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                    StartCoroutine(HieuUngTrieuHoi());
                    if (AllmanhRong.name == "vang")
                    {
                        string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            inventory.AddItem(allnamemanh[i], -1);
                        }
                    }
                    else if (AllmanhRong.name == "bac")
                    {
                        string[] allnamemanh = new string[] { "DauRongBac", "CanhRongBac", "ChanRongBac", "ThanRongBac", "DuoiRongBac" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            inventory.AddItem(allnamemanh[i], -1);
                        }
                    }
                }
                else crgame.OnThongBaoNhanh(json["thongbao"], 2);
                IEnumerator HieuUngTrieuHoi()
                {
                    yield return new WaitForSeconds(0.5f);
                    GameObject g = Instantiate(transform.GetChild(1).GetChild(1).gameObject, transform.position, Quaternion.identity);
                    g.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
                    g.gameObject.SetActive(true);
                    Image img = g.GetComponent<Image>();
                    img.sprite = Inventory.LoadSpriteRong(json["namerong"]);
                    img.SetNativeSize();
                    transform.GetChild(1).GetChild(0).transform.GetChild(4).gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.1f);
                    QuaBay quabay = img.GetComponent<QuaBay>();
                    quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                    quabay.enabled = true;
                    transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
    public void XemManhDoi()
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        GameObject yeucau = transform.GetChild(3).transform.GetChild(1).gameObject;
        namemanhchon = btnchon.name;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XemManhDoi/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + btnchon.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
            }
            else
            {
                // Show results as text
                Button btndoi = yeucau.transform.GetChild(3).GetComponent<Button>();
                btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                yeucau.transform.GetChild(0).gameObject.SetActive(false);
                yeucau.transform.GetChild(1).gameObject.SetActive(false);
                yeucau.transform.GetChild(2).gameObject.SetActive(false);

                Text txtlixiyeucau = null;
                bool ok = true;
                if (json["BiNgo"].Value != "")
                {
                    debug.LogError("BiNgo ne " + json["BiNgo"].Value);
                    yeucau.transform.GetChild(0).gameObject.SetActive(true);

                    int solixico = int.Parse(transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).GetComponent<Text>().text);

                    debug.LogError(solixico);
                    if (solixico >= int.Parse(json["BiNgo"].Value))
                    {
                        yeucau.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>" + solixico + "/" + json["BiNgo"].Value + "</color>";
                        if(ok) ok = true;
                        //dieukien[i] = true;
                    }
                    else
                    {
                        yeucau.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "<color=#ff0000ff>" + solixico + "/" + json["BiNgo"].Value + "</color>";
                        ok = false;
                    }
                }
                if (json["ngoc"]["namengoc"].Value != "")
                {
                    yeucau.transform.GetChild(1).gameObject.SetActive(true);
                    txtlixiyeucau = yeucau.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
                    yeucau.transform.GetChild(1).GetComponent<Image>().sprite = Inventory.LoadSprite(json["ngoc"]["namengoc"].Value);
                    for (int i = 0; i < inventory.contentNgoc.transform.childCount; i++)
                    {
                        if (json["ngoc"]["namengoc"].Value == inventory.contentNgoc.transform.GetChild(i).name)
                        {

                            Text txtsoluong = inventory.contentNgoc.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                            debug.Log("soluong  " + txtsoluong.text);
                            int solixico = int.Parse(txtsoluong.text);
                            if (solixico >= int.Parse(json[json["ngoc"]["namengoc"].Value].Value))
                            {
                                txtlixiyeucau.text = "<color=#00ff00ff>" + solixico + "/" + json[json["ngoc"]["namengoc"].Value].Value + "</color>";
                                if (ok) ok = true;
                            }
                            else
                            {
                                txtlixiyeucau.text = "<color=#ff0000ff>" + solixico + "/" + json[json["ngoc"]["namengoc"].Value].Value + "</color>";
                                ok = false;
                            }
                            break;
                        }
                        else if (i == inventory.contentNgoc.transform.childCount - 1)
                        {
                            if (json[json["ngoc"]["namengoc"].Value].Value == "0")
                            {
                                txtlixiyeucau.text = "<color=#00ff00ff>0/" + json[json["ngoc"]["namengoc"].Value].Value + "</color>";
                                if (ok) ok = true;
                            }
                            else
                            {
                                txtlixiyeucau.text = "<color=#ff0000ff>0/" + json[json["ngoc"]["namengoc"].Value].Value + "</color>";
                                ok = false;
                            }
                        }
                    }
                }

                if (json["HoaTuyet"].Value != "")
                {
                    yeucau.transform.GetChild(2).gameObject.SetActive(true);
                    txtlixiyeucau = yeucau.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();

                    if (inventory.ListItemThuong.ContainsKey("itemHoaTuyet"))
                    {
                        int solixico = int.Parse(inventory.ListItemThuong["itemHoaTuyet"].transform.GetChild(0).GetComponent<Text>().text);
                        if (solixico >= int.Parse(json["HoaTuyet"]))
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>" + solixico + "/" + json["HoaTuyet"].Value + "</color>";
                            if (ok) ok = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>" + solixico + "/" + json["HoaTuyet"].Value + "</color>";
                            ok = false;
                        }
                    }
                    else
                    {
                        if (json["HoaTuyet"].Value == "0")
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>0/" + json["HoaTuyet"].Value + "</color>";
                            if (ok) ok = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>0/" + json["HoaTuyet"].Value + "</color>";
                            ok = false;
                        }
                    }
                }

                Text txtthongtin = transform.GetChild(3).transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
                txtthongtin.text = json["thongtin"].Value;

                btndoi.interactable = ok;
                yeucau.SetActive(true);
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
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "DoiManh/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + namemanhchon);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
                btndoi.interactable = true;
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "0")
                {
                    transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
                }
                else crgame.OnThongBaoNhanh(www.downloadHandler.text, 2);
            }
        }
    }
    public void ChonMap()
    {
        Button btnmap = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        int ok = 0;
        for (int j = 0; j < 3; j++)
        {
            if (btnmap.transform.GetChild(j).GetComponent<Image>().sprite.name == "SaoSang") ok += 1;
        }
        if (ok < 3)
        {
            vienchinh.nameMapvao = btnmap.name;
            ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung"), true, transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
            tbc.txtThongBao.text = "Bạn muốn tấn công " + btnmap.transform.GetChild(3).GetComponent<Text>().text + "?";
            tbc.btnChon.onClick.AddListener(LoadMap);
        }
        else crgame.OnThongBaoNhanh("Bạn đã chinh phục ải này!");
    }
    void LoadMap()
    {
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "LoadMap/taikhoan/" + LoginFacebook.ins.id + "/namemap/" + vienchinh.nameMapvao);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.panelLoadDao.SetActive(false);
                crgame.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
            }
            else
            {
                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "ok")
                {
                    net.socket.Emit("DoiHinhDanh", JSONObject.CreateStringObject(vienchinh.nameMapvao));
                    vienchinh.chedodau = CheDoDau.Halloween;
                    vienchinh.enabled = true;
                 //   AllMenu.ins.DestroyMenu("MenuXacNhan");
                    vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGTinhVan");
                    gameObject.SetActive(false);
                }
                else
                {
                    crgame.OnThongBaoNhanh(www.downloadHandler.text);
                }
                AllMenu.ins.DestroyMenu("MenuXacNhan");
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
    public void ThamChien()
    {
        vienchinh.nameMapvao = "BossHalloween";
        LoadMap();
        //net.socket.Emit("DoiHinhDanh", JSONObject.CreateStringObject("BossHalloween"));
        //vienchinh.chedodau = "Halloween";
        //vienchinh.enabled = true;
        //vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGTinhVan");
        //gameObject.SetActive(false);
    }
    public void VeNha()
    {
     //   gameObject.SetActive(false);
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        AudioManager.SetSoundBg("nhacnen0");
        crgame.giaodien.SetActive(true);
        crgame.AllDao.transform.GetChild(crgame.DangODao).gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(crgame.AllDao.transform.GetChild(crgame.DangODao).transform.position.x, crgame.AllDao.transform.GetChild(crgame.DangODao).transform.position.y,-10);
        if (Friend.ins.QuaNha) Friend.ins.GoHome();
        AllMenu.ins.DestroyMenu("MenuEventHalloween");
        Resources.UnloadUnusedAssets();
        // AllMenu.ins.DestroyMenu("MenuEvent7VienNgocRong");
    }
    IEnumerator GetTop()
    {
        if(gettop)
        {
            //  crgame.OnThongBao(true, "Đang tải...", false);
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "GetTop/top/" + (trang - 1));
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                // Show results as text 
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                GameObject objecttop = transform.GetChild(5).transform.GetChild(6).gameObject;
                GameObject imgtop = transform.GetChild(5).transform.GetChild(7).gameObject;
                for (int i = 0; i < json["alltop"].Count; i++)
                {
                    GameObject top = Instantiate(objecttop, transform.position, Quaternion.identity);

                    Image imgAvatar = top.transform.GetChild(0).GetComponent<Image>();
                    Image imgKhungAvatar = top.transform.GetChild(1).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].Value;
                    //  Image HuyHieu = top.transform.GetChild(3).GetComponent<Image>();
                    Text txtName = top.transform.GetChild(2).GetComponent<Text>();
                    //  Text txtTop = top.transform.GetChild(5).GetComponent<Text>();
                    //    txtTop.text = json[i]["Top"].Value;
                 //   net.friend.GetAvatarFriend(json["alltop"][i]["idfb"].Value, imgAvatar);
                    imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                    //int sotop = int.Parse(json[i]["Top"].Value);
                    //if (sotop > 3) HuyHieu.sprite = Top;
                    //else if (sotop == 1) HuyHieu.sprite = Top1;
                    //else if (sotop == 2) HuyHieu.sprite = Top2;
                    //else if (sotop == 3) HuyHieu.sprite = Top3;
                    //HuyHieu.SetNativeSize();
                    txtName.text = json["alltop"][i]["Name"].Value;
                    top.transform.SetParent(imgtop.transform, false);
                    // crgame.OnThongBao(false);
                    //AllTop.SetActive(true);
                    // txtTrang.text = trang + "/100";
                    gettop = false;
                    top.SetActive(true);
                    if (i == 9) break;
                }
                //GameObject allchientuong = transform.GetChild(5).transform.GetChild(11).gameObject;
                //for (int i = 0; i < json["chientuong"].Count; i++)
                //{
                //    GameObject Rongg = Instantiate(net.loidai.BtnRong, transform.position, Quaternion.identity) as GameObject;
                //    Animator anim = Rongg.GetComponent<Animator>();
                //    anim.runtimeAnimatorController = Inventory.LoadAnimator(json["chientuong"][i]["nameobject"].Value);//SGResources.Load<RuntimeAnimatorController>("GameData/Animator/" + nameRong);
                //    Rongg.SetActive(true);
                //    Rongg.transform.SetParent(allchientuong.transform, false);
                //    Rongg.transform.localScale = new Vector3(-80, 80);
                //    anim.SetInteger("TienHoa", int.Parse(json["chientuong"][i]["tienhoa"].Value));
                //    Image imgAvatar = Rongg.transform.Find("avatar").GetComponent<Image>();
                //    string idfb = json["chientuong"][i]["id"].Value;
                //    if (idfb != "bot")
                //    {
                //        crgame.friend.GetAvatarFriend(idfb, imgAvatar);
                //    }
                //    Image imgToc = Rongg.transform.Find("Khung").GetComponent<Image>();
                //    imgToc.sprite = Inventory.LoadSprite("Avatar" + json["chientuong"][i]["toc"].Value);
                //    Text txtname = Rongg.transform.Find("txtname").GetComponent<Text>();
                //    txtname.text = json["chientuong"][i]["name"].Value;
                //    Rongg.transform.Find("txtTop").gameObject.SetActive(false);
                //    for (int j = 0; j < Rongg.transform.childCount; j++)
                //    {
                //        Rongg.transform.GetChild(i).transform.localScale = new Vector3(-Rongg.transform.GetChild(i).transform.localScale.x, Rongg.transform.GetChild(i).transform.localScale.y);
                //    }
                //    //   txttop.text = top;
                //    //Rongg.name = tenhienthi;
                //}
            }
        }
    }
    public void OpenMenuTop()
    {
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            //  crgame.OnThongBao(true, "Đang tải...", false);
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "GetTop/top/" + top);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.panelLoadDao.SetActive(false);
                crgame.OnThongBaoNhanh("Lỗi");
            }
            else
            {
                // Show results as text 
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                //GameObject objecttop = transform.GetChild(5).transform.GetChild(6).gameObject;

                if (json["alltop"].Count > 0)
                {
                    GameObject menutop = transform.GetChild(5).transform.GetChild(9).gameObject;

                    GameObject contentop = menutop.transform.GetChild(0).transform.GetChild(4).transform.GetChild(1).gameObject;



                    if (trangg % 2 == 1) topcuoi = int.Parse(json["alltop"][0]["timetop"].Value);
                    for (int i = 1; i <= 6; i++)
                    {
                        contentop.transform.GetChild(i).gameObject.SetActive(false);
                        if (i <= json["alltop"].Count)
                        {
                            debug.Log(json["alltop"][i - 1]["Name"].Value);
                            Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i - 1]["idobject"].Value;
                            debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                            debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = i + trang - 1;
                            txtTop.text = sotop.ToString();
                            debug.Log("ok3");
                           // net.friend.GetAvatarFriend(json["alltop"][i - 1]["idfb"].Value, imgAvatar);
                            imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i - 1]["Toc"].Value);
                            contentop.transform.GetChild(i).gameObject.SetActive(true);
                            debug.Log("ok4");
                            if (sotop > 3) HuyHieu.sprite = Top;
                            else if (sotop == 1) HuyHieu.sprite = Top1;
                            else if (sotop == 2) HuyHieu.sprite = Top2;
                            else if (sotop == 3) HuyHieu.sprite = Top3;
                            HuyHieu.SetNativeSize();
                            debug.Log("ok5");
                            txtName.text = json["alltop"][i - 1]["Name"].Value;
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i - 1]["time"].Value;
                            debug.Log("ok5.1");
                            if (i == 6) top = int.Parse(json["alltop"][i-1]["timetop"].Value);
                            else if(i == json["alltop"].Count) topcuoi = int.Parse(json["alltop"][i-1]["timetop"].Value);
                            debug.Log("ok5.2");
                        }
                

                        //  contentop.transform.GetChild(i).transform.SetParent(imgtop.transform, false);
                        // crgame.OnThongBao(false);
                        //AllTop.SetActive(true);S
                        // txtTrang.text = trang + "/100";
                    }
                    debug.Log("ok6");
                    debug.Log("Dung si tien phong" + json["dungsitienphong"]["idfb"].Value);
                    if (json["dungsitienphong"]["idfb"].Value != "")
                    {
                        Image imgAvatar = contentop.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
                        Image imgKhungAvatar = contentop.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["dungsitienphong"]["idobject"].Value;
                        debug.Log("ok7");
                        Text txtName = contentop.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
                        Text txttime = contentop.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>(); txttime.text = json["dungsitienphong"]["time"].Value;
                        debug.Log("ok8");
                   //     net.friend.GetAvatarFriend(json["dungsitienphong"]["idfb"].Value, imgAvatar);
                        imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["dungsitienphong"]["Toc"].Value);
                        debug.Log("ok9");
                        txtName.text = json["dungsitienphong"]["Name"].Value;
                        contentop.transform.GetChild(0).gameObject.SetActive(true);
                        debug.Log("ok10");
                    }
                    menutop.SetActive(true);
                    //   trang += 6;
                }
                else crgame.OnThongBaoNhanh("Chưa có xếp hạng");
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
   public void exitMenuTop()
   {
        top = 0;
        trang = 1;trangg = 1;
        transform.GetChild(5).transform.GetChild(9).gameObject.SetActive(false);
   }    
    public void sangtrangtop(int i)
    {
        if(top > topcuoi)
        {
            if (i > 0) trangg += 1;
            else return;
            //   else trangg -= 1;
            if (trang + i >= 0)
            {
                if (i < 0)
                {
                    trang -= 12;
                    top = topcuoi;
                }
                else trang += 6;
                OpenMenuTop();
            }
         //   debug.Log(i);
        }    
       
    }    
    public void TrangBi()
    {
        GameObject tc = GameObject.FindGameObjectWithTag("trencung");
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", tc, true, tc.transform.GetSiblingIndex() - 1);
    }
    public void VaoBoss()
    {
        transform.GetChild(5).gameObject.SetActive(true);
    }
}
