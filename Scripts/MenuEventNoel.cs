using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEventNoel : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject btnHopQua;
    public Sprite nutxanh, nutvang, nutdo,keovang,keoxanh,keodo,tatvang,tatxanh,tatdo, sprite1, sprite2,spritetuyettoi,spritetuyetsang;
    public GameObject objTat,mocqua; public GameObject Allnhiemvu,contentmocquatuyet, ContentManh, ObjectManh;string namemanhchon;
    void Awake()
    {
        btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
    }
    private void OnEnable()
    {
        //CrGame.ins.panelLoadDao.SetActive(true);
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
                gameObject.SetActive(false);
                VeNha();
                AllMenu.ins.DestroyMenu("MenuEventNoel");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if(json["status"].Value == "ok")
                {
                    GameObject menucaythong = transform.GetChild(0).gameObject;

                    menucaythong.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["event"]["tatxanh"].Value;
                    menucaythong.transform.GetChild(3).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = json["event"]["tatvang"].Value;
                    menucaythong.transform.GetChild(3).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = json["event"]["tatdo"].Value;
                    menucaythong.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["event"]["keogiangsinh"].Value;
                    LoadCayThong(json["event"]["caythong"]);

                    mocqua.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["event"]["solantreotat"].Value;
                    float fillamount = float.Parse(json["event"]["solantreotat"].Value) / 2000;
                    mocqua.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().fillAmount = fillamount;
                  //  transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = solanquay + "";

                    for (int i = 0; i < json["event"]["allNhiemvu"].Count; i++)
                    {
                        Text txttiendo = Allnhiemvu.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                        if (int.Parse(json["event"]["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value))
                        {
                            txttiendo.text = "<color=#00ff00ff>" + json["event"]["allNhiemvu"][i]["dalam"].Value + "/" + json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                        }
                        else
                        {
                            txttiendo.text = "<color=#ff0000ff>" + json["event"]["allNhiemvu"][i]["dalam"].Value + "/" + json["event"]["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                        }
                    }
                    GameObject contentmocqua = mocqua.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                    for (int i = 0; i < json["event"]["QuaTichLuy"].Count; i++)
                    {
                        if (json["event"]["QuaTichLuy"][i].Value == "chuaduocnhan") break;
                        if(json["event"]["QuaTichLuy"][i].Value == "duocnhan")
                        {
                            contentmocqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                            contentmocqua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                            if(i > 0) contentmocqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            else contentmocqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                        else if(json["event"]["QuaTichLuy"][i].Value == "danhan")
                        {
                            contentmocqua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                            contentmocqua.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                            if (i > 0) contentmocqua.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            else contentmocqua.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }    
                    }

                    for (int i = 0; i < json["event"]["QuaTichLuyTuyet"].Count; i++)
                    {
                        if (json["event"]["QuaTichLuyTuyet"][i].Value == "chuaduocnhan") break;
                        if (json["event"]["QuaTichLuyTuyet"][i].Value == "duocnhan")
                        {
                            contentmocquatuyet.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                            contentmocquatuyet.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                            if (i > 0) contentmocquatuyet.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            else contentmocquatuyet.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                        else if (json["event"]["QuaTichLuyTuyet"][i].Value == "danhan")
                        {
                            contentmocquatuyet.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                            contentmocquatuyet.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Đã nhận";
                            if (i > 0) contentmocquatuyet.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            else contentmocquatuyet.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                    }

                    if (ContentManh.transform.childCount == 1)
                    {
                        for (int i = 0; i < json["event"]["ManhDoi"].Count; i++)
                        {
                            GameObject manh = Instantiate(ObjectManh, ContentManh.transform.position, Quaternion.identity);
                            manh.transform.SetParent(ContentManh.transform, false);
                            Image imgmanh = manh.transform.GetChild(0).GetComponent<Image>();
                            if (json["event"]["ManhDoi"][i]["itemgi"].Value == "item" || json["event"]["ManhDoi"][i]["itemgi"].Value == "thuyenthucan")
                            {
                                imgmanh.sprite = Inventory.LoadSprite(json["event"]["ManhDoi"][i]["nameitem"].Value);
                            }
                            else
                            {
                                imgmanh.sprite = Inventory.LoadSpriteRong(json["event"]["ManhDoi"][i]["nameitem"].Value + "2");
                            }
                            manh.name = json["event"]["ManhDoi"][i]["namekey"];
                            manh.SetActive(true);
                        }
                    }

                    LoadOTuyet(json["event"]["allOTuyet"]);
                    GameObject menutuyet = transform.GetChild(5).gameObject;
                    menutuyet.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["event"]["phale"].Value;

                    menutuyet.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["event"]["aituyet"].Value;
                    contentmocquatuyet.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().fillAmount = float.Parse(json["event"]["aituyet"].Value) / 30;
                    menutuyet.transform.GetChild(6).GetComponent<Text>().text = json["infomotuyet"].Value;

                    if(json["event"]["vitricanhcong"]["hangtuyet"].Value != "-1")
                    {
                        GameObject congdichchuyen = menutuyet.transform.GetChild(5).gameObject;
                        congdichchuyen.transform.position = menutuyet.transform.GetChild(1).transform.GetChild(int.Parse(json["event"]["vitricanhcong"]["hangtuyet"].Value)).transform.GetChild(int.Parse(json["event"]["vitricanhcong"]["otuyet"].Value)).gameObject.transform.position;
                        congdichchuyen.SetActive(true);
                    }

                    btnHopQua.transform.SetParent(transform.GetChild(0).transform);
                    menucaythong.SetActive(true);
                    AudioManager.SetSoundBg("nhacnoel");
                    CrGame.ins.menulogin.SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    CrGame.ins.menulogin.SetActive(false);
                    VeNha();
                    AllMenu.ins.DestroyMenu("MenuEventNoel");
                }
               
              
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }
    void LoadOTuyet(JSONNode json)
    {
        GameObject menuotuyet = transform.GetChild(5).gameObject;
        GameObject contentotuyet = menuotuyet.transform.GetChild(1).gameObject;
       // var otuyet = 0;
        for (int j = 0; j < json.Count; j++)
        {
            for (int n = 0; n < json[j].Count; n++)
            {
                debug.Log(json[j][n].Value);
                if (json[j][n].Value == "chuamo")
                {
                    // contentotuyet.transform.GetChild(otuyet).GetComponent<Button>().enabled = false;
                    contentotuyet.transform.GetChild(j).transform.GetChild(n).gameObject.SetActive(true);
                    contentotuyet.transform.GetChild(j).transform.GetChild(n).GetComponent<Image>().sprite = spritetuyettoi;
                    if (contentotuyet.transform.GetChild(j).transform.GetChild(n).GetComponent<Button>()) Destroy(contentotuyet.transform.GetChild(j).transform.GetChild(n).GetComponent<Button>());
                }
                else if (json[j][n].Value == "duocmo")
                {
                    //contentotuyet.transform.GetChild(otuyet).GetComponent<Button>().interactable = true;
                    contentotuyet.transform.GetChild(j).transform.GetChild(n).gameObject.SetActive(true);
                    contentotuyet.transform.GetChild(j).transform.GetChild(n).GetComponent<Image>().sprite = spritetuyetsang;
                    if (!contentotuyet.transform.GetChild(j).transform.GetChild(n).GetComponent<Button>())
                    {
                        contentotuyet.transform.GetChild(j).transform.GetChild(n).gameObject.AddComponent<Button>().onClick.AddListener(MoOTuyet);
                    }
                   // contentotuyet.transform.GetChild(otuyet).getc
                }
                else if (json[j][n].Value == "damo")
                {
                    contentotuyet.transform.GetChild(j).transform.GetChild(n).gameObject.SetActive(false);
                }
            }
        }
    }    
    public void QuaMan()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "QuaMan/taikhoan/" + LoginFacebook.ins.id);
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
                    GameObject menutuyet = transform.GetChild(5).gameObject;
                    GameObject congdichchuyen = menutuyet.transform.GetChild(5).gameObject;

                    congdichchuyen.SetActive(false);

                    LoadOTuyet(json["allOTuyet"]);

                    menutuyet.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["aituyet"].Value;
                    // menutuyet.transform.GetChild(3).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = float.Parse(json["aituyet"].Value) / 30;
                    float aituyet = float.Parse(json["aituyet"].Value);
                    contentmocquatuyet.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().fillAmount = aituyet / 30;

                    if (aituyet % 3 == 0)
                    {
                        if (aituyet <= 30)
                        {
                            int chia = (int)aituyet / 3 - 1;
                            contentmocquatuyet.transform.GetChild(chia).transform.GetChild(1).gameObject.SetActive(true);
                            contentmocquatuyet.transform.GetChild(chia).transform.GetChild(2).gameObject.SetActive(false);
                            if (chia > 0) contentmocquatuyet.transform.GetChild(chia).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            else contentmocquatuyet.transform.GetChild(chia).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                        }
                    }

                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }    
    void LoadCayThong(JSONNode json)
    {
        GameObject menucaythong = transform.GetChild(0).gameObject;
        GameObject objtreotat = menucaythong.transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 0; i < objtreotat.transform.childCount; i++)
        {
            if(objtreotat.transform.GetChild(i).transform.childCount > 0) Destroy(objtreotat.transform.GetChild(i).transform.GetChild(0).gameObject);
            objtreotat.transform.GetChild(i).GetComponent<Button>().enabled = true;
        }
        bool duoctreo = false;
        if (json[0]["datreo"].Value == "False") duoctreo = true;
        for (int i = 0; i < json.Count; i++)
        {
            if (json[i]["nametat"].Value == "tatxanh")
            {
                objtreotat.transform.GetChild(i).GetComponent<Image>().sprite = nutxanh;
                if (json[i]["datreo"].Value == "True")
                {
                    TaoTat(i, "tatxanh");
                    duoctreo = true;
                }
                else
                {
                    if (duoctreo)
                    {
                        objtreotat.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        duoctreo = false;
                    }
                    else objtreotat.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
            else if (json[i]["nametat"].Value == "tatvang")
            {
                objtreotat.transform.GetChild(i).GetComponent<Image>().sprite = nutvang;
                if (json[i]["datreo"].Value == "True")
                {
                    TaoTat(i, "tatvang");
                    duoctreo = true;
                }
                else
                {
                    if (duoctreo)
                    {
                        objtreotat.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        duoctreo = false;
                    }
                    else objtreotat.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
            else if (json[i]["nametat"].Value == "tatdo")
            {
                objtreotat.transform.GetChild(i).GetComponent<Image>().sprite = nutdo;
                if (json[i]["datreo"].Value == "True")
                {
                    TaoTat(i, "tatdo");
                    duoctreo = true;
                }
                else
                {
                    if (duoctreo)
                    {
                        objtreotat.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        duoctreo = false;
                    }
                    else objtreotat.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
        }
    }    
    public void MoOTuyet()
    {
        debug.Log("mo o tuyet " + UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex());
        GameObject tuyetchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "MoOTuyet/taikhoan/" + LoginFacebook.ins.id + "/hangtuyet/" + tuyetchon.transform.parent.GetSiblingIndex() + "/otuyet/"+ tuyetchon.transform.GetSiblingIndex());
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
            }
            else
            {
                // Show results as text
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    LoadOTuyet(json["allOTuyet"]);
                    GameObject menutuyet = transform.GetChild(5).gameObject;
                    menutuyet.transform.GetChild(6).GetComponent<Text>().text = json["infomotuyet"].Value;
                    if (json["vitricanhcong"]["hangtuyet"].Value != "-1")
                    {
                        GameObject congdichchuyen = menutuyet.transform.GetChild(5).gameObject;
                        if (congdichchuyen.gameObject.activeSelf == false)
                        {
                            congdichchuyen.transform.position = menutuyet.transform.GetChild(1).transform.GetChild(int.Parse(json["vitricanhcong"]["hangtuyet"].Value)).transform.GetChild(int.Parse(json["vitricanhcong"]["otuyet"].Value)).gameObject.transform.position;
                            congdichchuyen.SetActive(true);
                        }
                    }
                    Text txtphale = menutuyet.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    txtphale.text = (int.Parse(txtphale.text) + 1) + "";
                    //btnnhan.onClick.RemoveAllListeners();
                    //btnnhan.gameObject.SetActive(false);
                    //contentQua.transform.GetChild(sibling).transform.GetChild(1).gameObject.SetActive(false);
                    //contentQua.transform.GetChild(sibling).transform.GetChild(2).gameObject.SetActive(true);
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }

    }    
    public void TreoTat()
    {
        GameObject btntreo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject menucaythong = transform.GetChild(0).gameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemTreoTat/taikhoan/" + LoginFacebook.ins.id + "/nuttreo/" + btntreo.GetComponent<Image>().sprite.name);
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

                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                tbc.txtThongBao.text = www.downloadHandler.text;
                debug.Log("tat" + btntreo.GetComponent<Image>().sprite.name.Substring(3));
                tbc.btnChon.onClick.AddListener(() => DongYTreoTat(btntreo.transform.GetSiblingIndex(),"tat"+ btntreo.GetComponent<Image>().sprite.name.Substring(3)));
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    void DongYTreoTat(int vitritreo,string mautat)
    {
        GameObject menucaythong = transform.GetChild(0).gameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "TreoTat/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                debug.Log(json["newcaythong"].Value);
                if (json["status"].Value == "ok")
                {
                    TaoTat(vitritreo,mautat);
                    for (int i = 0; i < menucaythong.transform.GetChild(3).transform.childCount; i++)
                    {
                        if(menucaythong.transform.GetChild(3).transform.GetChild(i).name == mautat)
                        {
                            Text txt = menucaythong.transform.GetChild(3).transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
                            if(txt.text != "0")txt.text = (int.Parse(txt.text) - 1) + "";
                            break;
                        }    
                    }
                    GameObject objtreotat = menucaythong.transform.GetChild(0).transform.GetChild(0).gameObject;
                    if (vitritreo < objtreotat.transform.childCount - 1) objtreotat.transform.GetChild(vitritreo + 1).GetComponent<Button>().interactable = true;
                    else
                    {
                        //reset cay thong
                        LoadCayThong(json["newcaythong"]);
                    }
                    Text txtsoluottreo = mocqua.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    float soluottreo = float.Parse(txtsoluottreo.text) + 1;
                    txtsoluottreo.text = soluottreo + "";
                    float fillamount = soluottreo / 2000;
 
                    GameObject contentmocqua = mocqua.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                    contentmocqua.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().fillAmount = fillamount;
                    if (soluottreo % 50 == 0)
                    {
                        if(soluottreo <= 2000)
                        {
                            debug.Log("ok1");
                            contentmocqua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(1).gameObject.SetActive(true);
                            contentmocqua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(2).gameObject.SetActive(false);
                            debug.Log("ok2");
                            if ((int)soluottreo / 50 - 1 > 0) contentmocqua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
                            else contentmocqua.transform.GetChild((int)soluottreo / 50 - 1).transform.GetChild(4).GetComponent<Image>().sprite = sprite2;
                            debug.Log("ok3");

                        }

                    }    
              
                    // CrGame.ins.OnThongBaoNhanh("Đã treo!", 1);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                }
                AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }    
    public void TaoTat(int vitri,string mautat)
    {
        GameObject tat = Instantiate(objTat, transform.position, Quaternion.identity);
        GameObject vitritat = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(vitri).gameObject;
        tat.transform.SetParent(vitritat.transform,false);
        Image imgkeo = tat.GetComponent<Image>();
        Image imgtat = tat.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        tat.transform.position = new Vector3( vitritat.transform.position.x, vitritat.transform.position.y - 0.2f);
        vitritat.GetComponent<Button>().enabled = false;
        if (mautat == "tatxanh")
        {
            imgkeo.sprite = keoxanh;
            imgtat.sprite = tatxanh;
        }
        else if (mautat == "tatvang")
        {
            imgkeo.sprite = keovang;
            imgtat.sprite = tatvang;
        }
        else if(mautat == "tatdo")
        {
            imgkeo.sprite = keodo;
            imgtat.sprite = tatdo;
        }
        tat.SetActive(true);
    }
    public void VeNha()
    {
        //   gameObject.SetActive(false);
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        AudioManager.SetSoundBg("nhacnen0");
        CrGame.ins.giaodien.SetActive(true);
        CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(true);
        AllMenu.ins.DestroyMenu("MenuEventNoel");
        Resources.UnloadUnusedAssets();
        // AllMenu.ins.DestroyMenu("MenuEvent7VienNgocRong");
    }

    public void OpenMenuDoiManhRong(string namerong)
    {
        Button btnTrieuHoi = transform.GetChild(1).GetChild(0).transform.GetChild(3).GetComponent<Button>();
        GameObject AllmanhRong = transform.GetChild(1).GetChild(0).transform.GetChild(2).gameObject;
        if (namerong == "vang")
        {
            string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
            int soluongmanhco = 0;
            for (int i = 0; i < AllmanhRong.transform.childCount; i++)
            {
                Image img = AllmanhRong.transform.GetChild(i).GetComponent<Image>();
                img.sprite = Inventory.LoadSprite(allnamemanh[i]);
                if (Inventory.ins.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
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
                if (Inventory.ins.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
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
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void TrieuHoiRong()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        GameObject AllmanhRong = transform.GetChild(1).GetChild(0).transform.GetChild(2).gameObject;
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
                    transform.GetChild(1).GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                    StartCoroutine(HieuUngTrieuHoi());
                    if (AllmanhRong.name == "vang")
                    {
                        string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            Inventory.ins.AddItem(allnamemanh[i], -1);
                        }
                    }
                    else if (AllmanhRong.name == "bac")
                    {
                        string[] allnamemanh = new string[] { "DauRongBac", "CanhRongBac", "ChanRongBac", "ThanRongBac", "DuoiRongBac" };
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            Inventory.ins.AddItem(allnamemanh[i], -1);
                        }
                    }
                }
                else CrGame.ins.OnThongBaoNhanh(json["thongbao"], 2);
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
    public void NhanQuaTichLuy()
    {
        GameObject btntreo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaTichLuy/taikhoan/" + LoginFacebook.ins.id + "/qua/" + btntreo.transform.parent.GetSiblingIndex());
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
                    if(json["keogiangsinh"].Value != "")
                    {
                        GameObject menucaythong = transform.GetChild(0).gameObject;
                        menucaythong.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["keogiangsinh"].Value;
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void NhanQuaTichLuyTuyet()
    {
        GameObject btntreo = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaTichLuyTuyet/taikhoan/" + LoginFacebook.ins.id + "/qua/" + btntreo.transform.parent.GetSiblingIndex());
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
                    //if (json["keogiangsinh"].Value != "")
                    //{
                    //    GameObject menucaythong = transform.GetChild(0).gameObject;
                    //    menucaythong.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["keogiangsinh"].Value;
                    //}
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
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
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemManhDoi/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + btnchon.name);
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
                Button btndoi = yeucau.transform.GetChild(3).GetComponent<Button>();
                btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                yeucau.transform.GetChild(0).gameObject.SetActive(false);
                yeucau.transform.GetChild(1).gameObject.SetActive(false);
                yeucau.transform.GetChild(2).gameObject.SetActive(false);

                Text txtlixiyeucau = null;
                bool ok = true;
                GameObject menucaythong = transform.GetChild(0).gameObject;
                if (json["keogiangsinh"].Value != "")
                {
                    //  debug.LogError("keogiangsinh ne " + json["BiNgo"].Value);
               
                    yeucau.transform.GetChild(0).gameObject.SetActive(true);

                    int sokeoco = int.Parse(menucaythong.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text);

                    //debug.LogError(solixico);
                    if (sokeoco >= int.Parse(json["keogiangsinh"].Value))
                    {
                        yeucau.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>" + sokeoco + "/" + json["keogiangsinh"].Value + "</color>";
                        if (ok) ok = true;
                        //dieukien[i] = true;
                    }
                    else
                    {
                        yeucau.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "<color=#ff0000ff>" + sokeoco + "/" + json["keogiangsinh"].Value + "</color>";
                        ok = false;
                    }
                }

                if (json["phale"].Value != "")
                {
                    //  debug.LogError("keogiangsinh ne " + json["BiNgo"].Value);
                    GameObject menutuyet = transform.GetChild(5).gameObject;

                    yeucau.transform.GetChild(1).gameObject.SetActive(true);

                    int sophaleco = int.Parse(menutuyet.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text);

                   // debug.LogError(sophaleco);
                    if (sophaleco >= int.Parse(json["phale"].Value))
                    {
                        yeucau.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>" + sophaleco + "/" + json["phale"].Value + "</color>";
                        if (ok) ok = true;
                        //dieukien[i] = true;
                    }
                    else
                    {
                        yeucau.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "<color=#ff0000ff>" + sophaleco + "/" + json["phale"].Value + "</color>";
                        ok = false;
                    }
                }

                if (json["HuyChuongDungKhi"].Value != "")
                {
                    yeucau.transform.GetChild(2).gameObject.SetActive(true);
                    txtlixiyeucau = yeucau.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();

                    if (Inventory.ins.ListItemThuong.ContainsKey("itemHuyChuongDungKhi"))
                    {
                        int solixico = int.Parse(Inventory.ins.ListItemThuong["itemHuyChuongDungKhi"].transform.GetChild(0).GetComponent<Text>().text);
                        if (solixico >= int.Parse(json["HuyChuongDungKhi"]))
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>" + solixico + "/" + json["HuyChuongDungKhi"].Value + "</color>";
                            if (ok) ok = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>" + solixico + "/" + json["HuyChuongDungKhi"].Value + "</color>";
                            ok = false;
                        }
                    }
                    else
                    {
                        if (json["HuyChuongDungKhi"].Value == "0")
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>0/" + json["HuyChuongDungKhi"].Value + "</color>";
                            if (ok) ok = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>0/" + json["HuyChuongDungKhi"].Value + "</color>";
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
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "DoiManh/taikhoan/" + LoginFacebook.ins.id + "/namemanh/" + namemanhchon);
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
                    GameObject menucaythong = transform.GetChild(0).gameObject;
                    GameObject menutuyet = transform.GetChild(5).gameObject;
                    transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
                    menucaythong.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["keogiangsinh"].Value;
                    menutuyet.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["phale"].Value;
                }    
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
        }
    }
}
