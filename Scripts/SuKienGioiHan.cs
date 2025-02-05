using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SuKienGioiHan : MonoBehaviour
{
    public GameObject contentbtn, contentnhiemvu,contentqua;
    public Sprite btndachon, btnchuachon;
    byte tab = 0;
    public Sprite odeitemxanh, odeitemhong;
    public Sprite[] alltuchat;
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetDataEventGioiHan/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
               // gameObject.SetActive(false);
                AllMenu.ins.DestroyMenu("MenuSuKienGioiHan");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject objbtn = contentbtn.transform.parent.transform.GetChild(0).gameObject;
                    for (int i = 0; i < json["tab"].Count; i++)
                    {
                        GameObject g = Instantiate(objbtn, transform.position, Quaternion.identity);
                        g.transform.SetParent(contentbtn.transform,false);
                        g.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["tab"][i]["nametab"].Value;
                        if (i == 0) g.transform.GetChild(0).GetComponent<Image>().sprite = btndachon;
                        g.name = json["tab"][i]["key"].Value;
                        g.SetActive(true);
                    }
                    LoadTab(json);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    AllMenu.ins.DestroyMenu("MenuSuKienGioiHan");
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
   
    public void DoiTab()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (objchon.transform.parent.transform.GetSiblingIndex() == tab) return;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "OpenTabSkGioiHan/taikhoan/" + LoginFacebook.ins.id + "/tab/"+ objchon.transform.parent.transform.GetSiblingIndex());
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
                    if (tab != objchon.transform.parent.transform.GetSiblingIndex())
                    {
                        contentbtn.transform.GetChild(tab).transform.GetChild(0).GetComponent<Image>().sprite = btnchuachon;

                        objchon.GetComponent<Image>().sprite = btndachon;
                        tab = (byte)objchon.transform.parent.transform.GetSiblingIndex();
                    }
                    LoadTab(json);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void DoiQua()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "DoiQuaGioiHan/taikhoan/" + LoginFacebook.ins.id + "/tab/" + tab +"/namequa/" + objchon.transform.parent.name);
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
                    Text txtitemcan = transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
                    txtitemcan.text = json["soitemcan"].Value;
                    objchon.transform.parent.transform.GetChild(2).GetComponent<Text>().text = json["txtdadoi"].Value;
                    if (json["btn"].Value == "true")
                    {
                        objchon.transform.parent.transform.GetChild(5).GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        objchon.transform.parent.transform.GetChild(5).GetComponent<Button>().interactable = false;

                    }
                    CrGame.ins.OnThongBaoNhanh("Đổi thành công");
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void NhanQua()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaGioiHan/taikhoan/" + LoginFacebook.ins.id + "/tab/" + tab + "/vitri/" + objchon.transform.parent.name);
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
                    objchon.GetComponent<Button>().interactable = false;
                    objchon.transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
                    objchon.transform.parent.gameObject.transform.SetAsLastSibling();
                    CrGame.ins.OnThongBaoNhanh("Đã nhận!");
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuSuKienGioiHan");
    }
    void LoadTab(JSONNode json)
    {
        List<GameObject> allodanhan = new List<GameObject>();
        for (int i = 0; i < contentnhiemvu.transform.childCount; i++)
        {
            Destroy(contentnhiemvu.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < contentqua.transform.childCount; i++)
        {
            Destroy(contentqua.transform.GetChild(i).gameObject);
        }
        if (json["data"]["loai"].Value == "NhiemVu")
        {
            transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
            Image imgbanner = transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
            Text txttimeketthuc = imgbanner.transform.GetChild(0).GetComponent<Text>();
            txttimeketthuc.text = json["txttime"].Value;
            imgbanner.gameObject.SetActive(false);
            LoadImage(json["data"]["banner"].Value, imgbanner);
            GameObject objnhiemvu = contentnhiemvu.transform.parent.transform.GetChild(0).gameObject;
            for (int i = 0; i < json["data"]["allnhiemvu"].Count; i++)
            {
                GameObject g = Instantiate(objnhiemvu, transform.position, Quaternion.identity);
                g.transform.SetParent(contentnhiemvu.transform, false);
                g.transform.GetChild(3).GetComponent<Text>().text = json["data"]["allnhiemvu"][i]["info"].Value;
                g.transform.GetChild(5).GetComponent<Text>().text = json["data"]["allnhiemvu"][i]["txtdalam"].Value;
                if (json["data"]["allnhiemvu"][i]["btn"].Value == "true")
                {
                    g.transform.GetChild(4).GetComponent<Button>().interactable = true;
                    g.transform.SetAsFirstSibling();
                }
                else if (json["data"]["allnhiemvu"][i]["btn"].Value == "danhan")
                {
                    g.transform.GetChild(4).GetComponent<Button>().interactable = false;
                    g.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "<color=red>Đã nhận</color>";
                    allodanhan.Add(g);
                   // g.transform.SetAsLastSibling();
                }
                else g.transform.GetChild(4).GetComponent<Button>().interactable = false;
                GameObject allqua = g.transform.GetChild(1).gameObject;
               
                for (int j = 0; j < json["data"]["allitem"][i].Count; j++)
                {
                    Text txtitem = allqua.transform.GetChild(j).transform.GetChild(2).GetComponent<Text>();
                    txtitem.text = json["data"]["allitem"][i][j]["soluong"];
                    allqua.transform.GetChild(j).gameObject.SetActive(true);
                    Image imgitem = allqua.transform.GetChild(j).transform.GetChild(1).GetComponent<Image>();
                    imgitem.name = "item" + json["data"]["allitem"][i][j]["name"].Value;
                    if (json["data"]["allitem"][i][j]["loaiitem"].AsString == "Item")
                    {
                        imgitem.sprite = Inventory.LoadSprite(json["data"]["allitem"][i][j]["name"].Value);
                        imgitem.SetNativeSize();
                    }
                    else if (json["data"]["allitem"][i][j]["loaiitem"].AsString == "ItemRong")
                    {
                        imgitem.sprite = Inventory.LoadSpriteRong(json["data"]["allitem"][i][j]["name"].Value + "1");
                        imgitem.SetNativeSize();
                        txtitem.text = json["data"]["allitem"][i][j]["sao"].AsString + " sao";
                    }
                    else
                    {
                        LoadImage("item", json["data"]["allitem"][i][j]["name"].Value, imgitem);
                    }
                    for (int k = 0; k < alltuchat.Length; k++)
                    {
                        if (alltuchat[k].name == json["data"]["allitem"][i][j]["tuchat"].Value)
                        {
                            Image imgtuchat = allqua.transform.GetChild(j).transform.GetChild(0).GetComponent<Image>();
                            imgtuchat.sprite = alltuchat[k];
                            break;
                        }
                    }
                    
                }
                g.name = i + "";
                g.SetActive(true);
            }
            for (int i = 0; i < allodanhan.Count; i++)
            {
                allodanhan[i].transform.SetAsLastSibling();
            }
        }
        else if (json["data"]["loai"].Value == "DoiQua")
        {
            transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);

            Image imgbanner = transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>();
            imgbanner.gameObject.SetActive(false);
            Text txttimeketthuc = imgbanner.transform.GetChild(0).GetComponent<Text>();
            txttimeketthuc.text = json["txttime"].Value;
            LoadImage(json["data"]["banner"].Value, imgbanner);

            GameObject objdoiqua = contentqua.transform.parent.transform.GetChild(0).gameObject;
            for (int i = 0; i < json["data"]["allitemdoi"].Count; i++)
            {
                GameObject g = Instantiate(objdoiqua, transform.position, Quaternion.identity);
                g.transform.SetParent(contentqua.transform, false);
                g.transform.GetChild(1).GetComponent<Text>().text = json["data"]["allitemdoi"][i]["nametv"].Value;
                g.transform.GetChild(2).GetComponent<Text>().text = json["data"]["allitemdoi"][i]["txtdadoi"].Value;
                Image imgitem = g.transform.GetChild(3).GetComponent<Image>();
                imgitem.name = "item" + json["data"]["allitemdoi"][i]["name"].Value;
                if (json["data"]["allitemdoi"][i]["loai"].Value == "Item")
                {
                    g.transform.GetChild(4).GetComponent<Text>().text = json["data"]["allitemdoi"][i]["soluong"].Value;
                    imgitem.sprite = Inventory.LoadSprite(json["data"]["allitemdoi"][i]["name"].Value);
                }
                else if (json["data"]["allitemdoi"][i]["loai"].Value == "ItemRong")
                {
                    g.transform.GetChild(4).GetComponent<Text>().text = json["data"]["allitemdoi"][i]["sao"].Value + " sao";
                    imgitem.sprite = Inventory.LoadSpriteRong(json["data"]["allitemdoi"][i]["name"].Value + "1");
                }
                else
                {
                    LoadImage("item", json["data"]["allitemdoi"][i]["name"].Value, imgitem);
                    g.transform.GetChild(4).GetComponent<Text>().text = json["data"]["allitemdoi"][i]["soluong"].Value;
                }
                g.transform.GetChild(5).transform.GetChild(1).GetComponent<Text>().text = json["data"]["allitemdoi"][i]["ItemYeuCau"]["soluong"].Value;
                g.name = json["data"]["allitemdoi"][i]["name"].Value;
                if(json["data"]["allitemdoi"][i]["btn"].Value == "true")
                {
                    g.transform.GetChild(5).GetComponent<Button>().interactable = true;
                }
                if (json["data"]["allitemdoi"][i]["tuchat"].Value == "hong")
                {
                    g.transform.GetChild(0).GetComponent<Image>().sprite = odeitemhong;
                }
                g.SetActive(true);
            }
           
            Text txtitemcan = transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
            txtitemcan.text = json["data"]["itemcan"]["soluong"].Value;
            if (json["data"]["itemcan"]["loai"].Value != "comayman")
            {
                Image imgitemcan = transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
            }
        
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public static void LoadImage(string link, Image img)
    {
        CrGame.ins.StartCoroutine(DownloadImage());
        IEnumerator DownloadImage()
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                debug.Log(request.error);
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                img.sprite = sprite;
                //if (namefolder == "khungavt")
                //{
                //    img.SetNativeSize();
                //}
                img.gameObject.SetActive(true);

            }
        }
    }
    public static void LoadImage(string namefolder, string namefile, Image img)
    {
       CrGame.ins. StartCoroutine(DownloadImage());
        IEnumerator DownloadImage()
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://" + LoginFacebook.ins.ServerChinh + "/LoadImage/namefolder/" + namefolder + "/name/" + namefile);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                debug.Log(request.error);
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                img.sprite = sprite;
                //if (namefolder == "khungavt")
                //{
                //    img.SetNativeSize();
                //}
                img.SetNativeSize();

            }
        }
    }
}
