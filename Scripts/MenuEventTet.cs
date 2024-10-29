using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEventTet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CayLiXi,baolixi,Allnhiemvu,ContentManh,ObjectManh,YeuCauLiXi;
    CrGame crgame;GameObject btnHopQua;Inventory inventory;string namemanhchon;public bool LoadLixi = true;

    public GameObject AllmanhRong,menudoimanhrong,hieuungtrieuhoi;
    public Button btnTrieuHoi;public Image imgRong;

    public GameObject contentRong, itemrong;
    void Awake()
    {
        crgame = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CrGame>();
        inventory = crgame.GetComponent<Inventory>();
        btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
    }
    private void OnEnable()
    {
        if(LoadLixi)
        {
            crgame.panelLoadDao.SetActive(true);
            StartCoroutine(Load());
            IEnumerator Load()
            {
                UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "GetDataEventTet/taikhoan/" + LoginFacebook.ins.id);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    debug.Log(www.error);
                    crgame.panelLoadDao.SetActive(false);
                    crgame.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                    gameObject.SetActive(false);
                }
                else
                {
                    // Show results as text
                    btnHopQua.transform.SetParent(transform.GetChild(0).transform);
                    JSONNode json = JSON.Parse(www.downloadHandler.text);
                    int Sobaolixi = int.Parse(json["baolixi"].Value);
                    if (CayLiXi.transform.childCount - 1 < Sobaolixi)
                    {
                        int solixi = Sobaolixi - (CayLiXi.transform.childCount - 1);
                        //  debug.Log("solixi " + solixi);
                        for (int i = 0; i < solixi; i++)
                        {
                            MocLiXi();
                        }
                    }
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
                                debug.Log(json["ManhDoi"][i]["itemgi"].Value);
                                imgmanh.sprite = Inventory.LoadSprite(json["ManhDoi"][i]["nameitem"].Value);
                            }
                            else
                            {
                                imgmanh.sprite = Inventory.LoadSpriteRong(json["ManhDoi"][i]["nameitem"].Value + "2");
                            }
                            manh.name = json["ManhDoi"][i]["namekey"];
                            manh.SetActive(true);
                        }
                    }
                    debug.Log(www.downloadHandler.text);
                    transform.GetChild(0).gameObject.SetActive(true);
                    crgame.panelLoadDao.SetActive(false);
                    YeuCauLiXi.SetActive(false);
                }
            }
        }
    }
    public void MocLiXi()
    {
        GameObject lixi = Instantiate(baolixi,CayLiXi.transform.position,Quaternion.identity);
        lixi.transform.SetParent(CayLiXi.transform,false);
        lixi.transform.position = new Vector3(CayLiXi.transform.position.x + Random.Range(-2.5f,2.5f), CayLiXi.transform.position.y + Random.Range(-2, 3));
        lixi.SetActive(true);
    }
    public void VeNha()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
    }
    public void NhanLixi()
    {
        Button btnlixichon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btnlixichon.enabled = false;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "ThuHoachLiXi/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi thu hoạch");
                btnlixichon.enabled = true;
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                if(www.downloadHandler.text == "thuhoachok")
                {
                    QuaBay quabay = btnlixichon.GetComponent<QuaBay>();
                    quabay.vitribay = btnHopQua;
                    quabay.enabled = true;
                }   
                else
                {
                    crgame.OnThongBaoNhanh("Lỗi thu hoạch");
                    btnlixichon.enabled = true;
                }
            }
        }
    }
    public void XemManhDoi()
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        namemanhchon = btnchon.name;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XemManhDoi/taikhoan/" + LoginFacebook.ins.id + "/namemanh/"+ btnchon.name);
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
                Button btndoi = YeuCauLiXi.transform.GetChild(3).GetComponent<Button>();
                btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                string[] arlixi = new string[] { "BaoLiXiDo", "BaoLiXiVang", "BaoLiXiXanh"};
                bool[] dieukien = new bool[] {false,false,false};
                for (int i = 0; i < 3; i++)
                {
                    Text txtlixiyeucau = YeuCauLiXi.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
                    if (inventory.ListItemThuong.ContainsKey("item"+arlixi[i]))
                    {
                        int solixico = int.Parse(inventory.ListItemThuong["item" + arlixi[i]].transform.GetChild(0).GetComponent<Text>().text);
                        if (solixico >= int.Parse(json[arlixi[i]].Value))
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>" + solixico  + "/" + json[arlixi[i]].Value + "</color>";
                            dieukien[i] = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>" + solixico + "/" + json[arlixi[i]].Value + "</color>";
                        }
                    }
                    else
                    {
                        if(json[arlixi[i]].Value == "0")
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>0/" + json[arlixi[i]].Value + "</color>";
                            dieukien[i] = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>0/" + json[arlixi[i]].Value + "</color>";
                        }
                    }
                }
                if(dieukien[0] == true && dieukien[1] == true && dieukien[2] == true)
                {
                    btndoi.interactable = true;
                }
                YeuCauLiXi.SetActive(true);
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
                if(www.downloadHandler.text == "0")
                {
                    YeuCauLiXi.SetActive(false);
                }
                else crgame.OnThongBaoNhanh(www.downloadHandler.text,2);
            }
        }
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        if (namerong == "vang")
        {
            string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
            int soluongmanhco = 0;
            for (int i = 0; i < AllmanhRong.transform.childCount; i++)
            {
                Image img = AllmanhRong.transform.GetChild(i).GetComponent<Image>();
                img.sprite = Inventory.LoadSprite(allnamemanh[i]);
                if (inventory.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
                {
                    img.color = new Color32(255, 255, 255, 255);
                    soluongmanhco += 1;
                }
                else img.color = new Color32(125, 125, 125, 181);
                if(soluongmanhco == 5) btnTrieuHoi.interactable = true;
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
                if (inventory.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
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
        menudoimanhrong.SetActive(true);
    }    
    public void TrieuHoiRong()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
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
                    hieuungtrieuhoi.SetActive(true);
                    StartCoroutine(HieuUngTrieuHoi());
                    if(AllmanhRong.name == "vang")
                    {
                        string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang"};
                        for (int i = 0; i < allnamemanh.Length; i++)
                        {
                            inventory.AddItem(allnamemanh[i],-1);
                        }
                    }
                    else if(AllmanhRong.name == "bac")
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
                    GameObject g = Instantiate(imgRong.gameObject,transform.position,Quaternion.identity);
                    g.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform,false);
                    g.gameObject.SetActive(true);
                    Image img = g.GetComponent<Image>();
                    img.sprite = Inventory.LoadSpriteRong(json["namerong"]);
                    img.SetNativeSize();
                    hieuungtrieuhoi.SetActive(false);
                    yield return new WaitForSeconds(0.1f);
                    QuaBay quabay = img.GetComponent<QuaBay>();
                    quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                    quabay.enabled = true;
                    menudoimanhrong.SetActive(false);
                }
            }
        }
    }
    public void XemThongTinRong()
    {
        crgame.ChiSoRong(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name);
    }
   
}
