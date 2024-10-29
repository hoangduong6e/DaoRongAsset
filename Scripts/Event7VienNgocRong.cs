using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Event7VienNgocRong : MonoBehaviour
{
    // Start is called before the first frame update
    string namemanhchon;
    CrGame crgame;Inventory inventory;public GameObject YeuCauNgocRong, Allnhiemvu, ContentManh, ObjectManh, AllMay,MocQua,contentQua; GameObject btnHopQua;
    public Sprite spriteMayTrang, spriteMayVang,sprirehopquachuanhan, sprirehopquaduocnhan, sprirehopquadanhan,sprite1,sprite2;
    void Awake()
    {
        crgame = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CrGame>();
        inventory = crgame.GetComponent<Inventory>();
        btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
    }
    public void LoadSoLanQuay(float solanquay,JSONNode json)
    {
        debug.Log(json);
        float fillamount = solanquay / 300;
        contentQua.transform.GetChild(0).transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = fillamount;
        transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = solanquay + "";
        Image img = contentQua.transform.GetChild(0).transform.GetChild(4).GetComponent<Image>();
        for (int i = 0; i < json.Count; i++)
        {
            
            Image imghopqua = contentQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
           
            if (json[i].Value == "chuaduocnhan")
            {
                if(imghopqua.sprite.name == "chuaduocnhanimg" || imghopqua.sprite.name == "chuanhanimg" || imghopqua.sprite.name == "danhanimg") imghopqua.sprite = sprirehopquachuanhan;
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                img.sprite = sprite1;
            }
            else if (json[i].Value == "duocnhan")
            {
                if (imghopqua.sprite.name == "chuaduocnhanimg" || imghopqua.sprite.name == "chuanhanimg" || imghopqua.sprite.name == "danhanimg") imghopqua.sprite = sprirehopquaduocnhan;
                Button btnnhan = contentQua.transform.GetChild(i).transform.GetChild(1).GetComponent<Button>();
                btnnhan.gameObject.SetActive(true);
                btnnhan.onClick.AddListener(NhanQuaTichLuy);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                img.sprite = sprite2;
            }
            else if (json[i].Value == "danhan")
            {
                if (imghopqua.sprite.name == "chuaduocnhanimg" || imghopqua.sprite.name == "chuanhanimg" || imghopqua.sprite.name == "danhanimg") imghopqua.sprite = sprirehopquadanhan;
                contentQua.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                contentQua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                img.sprite = sprite2;
            }
            img.SetNativeSize();
            imghopqua.SetNativeSize();
            if(i < json.Count - 1) img = contentQua.transform.GetChild(i + 1).transform.GetChild(3).GetComponent<Image>();
            //   contentQua.transform.GetChild(i).transform.GetChild(3)
        }
    }
    private void OnEnable()
    {
        //crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "GetDataEventRongThan/taikhoan/" + LoginFacebook.ins.id);
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
                        else
                        {
                            imgmanh.sprite = Inventory.LoadSpriteRong(json["ManhDoi"][i]["nameitem"].Value + "2");
                        }
                        manh.name = json["ManhDoi"][i]["namekey"];
                        manh.SetActive(true);
                    }
                }
                transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = "Bạn còn " + json["soluotquaymay"].Value + " lượt miễn phí";
              //  if (int.Parse(json["soluotquaymay"].Value) > 0) transform.GetChild(0).transform.GetChild(2).GetComponent<Button>().interactable = true;
           //     else transform.GetChild(0).transform.GetChild(2).GetComponent<Button>().interactable = false;
                debug.Log(www.downloadHandler.text);
                LoadSoLanQuay(float.Parse(json["solanquaymay"].Value), json["quaTichLuy"]);
                transform.GetChild(0).gameObject.SetActive(true);
                //crgame.panelLoadDao.SetActive(false);
                crgame.menulogin.SetActive(false);
            }
        }
    }
    public void Quay()
    {
        Button btnquay = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btnquay.interactable = false;
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "QuayEvent/taikhoan/" + LoginFacebook.ins.id);
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
                    transform.GetChild(0).transform.GetChild(3).GetComponent<Button>().interactable = false;
                    crgame.panelLoadDao.SetActive(false);
                    transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = "Bạn còn " + json["soluotquaymay"].Value + " lượt miễn phí";
                    for (int i = 0; i < 60; i++)
                    {
                        Image oquay = AllMay.transform.GetChild(Random.Range(0, 11)).GetComponent<Image>();
                        oquay.sprite = spriteMayVang;
                        yield return new WaitForSeconds(0.05f);
                        oquay.sprite = spriteMayTrang;
                    }
                    for (int i = 0; i < AllMay.transform.childCount; i++)
                    {
                        if (AllMay.transform.GetChild(i).name == json["qua"].Value)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                AllMay.transform.GetChild(i).GetComponent<Image>().sprite = spriteMayVang;
                                yield return new WaitForSeconds(0.1f);
                                AllMay.transform.GetChild(i).GetComponent<Image>().sprite = spriteMayTrang;
                                yield return new WaitForSeconds(0.1f);
                            }
                            break;
                        }
                    }
                    transform.GetChild(0).transform.GetChild(3).GetComponent<Button>().interactable = true;
                    LoadSoLanQuay(float.Parse(json["solanquaymay"].Value),json["quaTichLuy"]);
                    //  if(int.Parse(json["soluotquaymay"].Value) > 0) btnquay.interactable = true;
                }
                else if (json["status"].Value == "hetluotquay")
                {
                    ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung")).GetComponent<ThongBaoChon>();
                    tbc.txtThongBao.text = "Bạn muốn mua thêm lượt bằng 30 kim cương?";
                    tbc.btnChon.onClick.AddListener(MuaLuotQuay);
                }
                else
                {
                    crgame.OnThongBaoNhanh(json["status"].Value);
                }
                crgame.panelLoadDao.SetActive(false);
                btnquay.interactable = true;
            }
          
        }
    }    
    void MuaLuotQuay()
    {
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "MuaLuot/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.panelLoadDao.SetActive(false);
                crgame.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
            else
            {
                // Show results as text
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = "Bạn còn " + json["soluotquaymay"].Value + " lượt";
                }
                else crgame.OnThongBaoNhanh(json["status"].Value);
                crgame.panelLoadDao.SetActive(false);
                AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
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
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "NhanQuaTichLuy/taikhoan/" + LoginFacebook.ins.id + "/qua/"+ sibling);
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
                // Show results as text
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    btnnhan.onClick.RemoveAllListeners();
                    btnnhan.gameObject.SetActive(false);
                    contentQua.transform.GetChild(sibling).transform.GetChild(1).gameObject.SetActive(false);
                    contentQua.transform.GetChild(sibling).transform.GetChild(2).gameObject.SetActive(true);
                }
                else crgame.OnThongBaoNhanh(json["status"].Value);
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
    public void XemItemRongThan()
    {
        string nameitem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XeminfoitemRongThan/taikhoan/" + LoginFacebook.ins.id + "/nameitem/" + nameitem);
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
                // Show results as text
                Text txt = transform.GetChild(4).transform.GetChild(1).GetComponent<Text>();txt.fontSize = 40;
                txt.text = www.downloadHandler.text;
                Button btnNhan = transform.GetChild(4).transform.GetChild(3).GetComponent<Button>();
                btnNhan.onClick.RemoveAllListeners();
                btnNhan.onClick.AddListener(() => NhanSkill(nameitem));
                btnNhan.gameObject.SetActive(true);
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
    public void NhanSkill(string skillnhan)
    {
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "NhanDieuUocRongThan/taikhoan/" + LoginFacebook.ins.id + "/nameitem/" + skillnhan);
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
                // Show results as text

                Button btnNhan = transform.GetChild(4).transform.GetChild(3).GetComponent<Button>();
                btnNhan.onClick.RemoveAllListeners();

                crgame.panelLoadDao.SetActive(false);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                if (json["status"].Value == "thanhcong")
                {
                    transform.GetChild(2).gameObject.SetActive(false);
                    transform.GetChild(4).gameObject.SetActive(false);
                    for (int i = 1; i <= 7; i++)
                    {
                        inventory.AddItem("NgocRong" + i + "Sao", -1);
                    }
                }    
            }
        }
    }    
    public void OpenMenuTrieuHoiRong()
    {
        Text txt = transform.GetChild(4).transform.GetChild(1).GetComponent<Text>(); txt.fontSize = 50;
        txt.text = "Hãy chọn điều ước ngươi muốn...";
        
        GameObject objngocrong = transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
        Button btnNhan = transform.GetChild(4).transform.GetChild(3).GetComponent<Button>();
        btnNhan.onClick.RemoveAllListeners();
        btnNhan.gameObject.SetActive(false);

        byte ok = 0;
        for (int i = 0; i < 7; i++)
        {
            if(inventory.ListItemThuong.ContainsKey("itemNgocRong"+(i+1)+"Sao"))
            {
                objngocrong.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255,255);
                ok += 1;
            }
            else objngocrong.transform.GetChild(i).GetComponent<Image>().color = new Color32(82, 82, 82,255);
        }
        
        if(ok ==7) transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().interactable = true;
        else transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().interactable = false;
        transform.GetChild(2).gameObject.SetActive(true);

    }    
    public void TrieuHoiRongThan()
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
            yield return new WaitForSeconds(3.5f);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void XemManhDoi()
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
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
                Button btndoi = YeuCauNgocRong.transform.GetChild(6).GetComponent<Button>();
                btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                string[] arlixi = new string[] { "NgocRong2Sao", "NgocRong3Sao", "NgocRong4Sao", "NgocRong5Sao", "NgocRong6Sao", "NgocRong7Sao" };
                bool[] dieukien = new bool[] { false, false, false, false, false, false };
                for (int i = 0; i < 6; i++)
                {
                    Text txtlixiyeucau = YeuCauNgocRong.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
                    if (inventory.ListItemThuong.ContainsKey("item" + arlixi[i]))
                    {
                        int solixico = int.Parse(inventory.ListItemThuong["item" + arlixi[i]].transform.GetChild(0).GetComponent<Text>().text);
                        if (solixico >= int.Parse(json[arlixi[i]].Value))
                        {
                            txtlixiyeucau.text = "<color=#00ff00ff>" + solixico + "/" + json[arlixi[i]].Value + "</color>";
                            dieukien[i] = true;
                        }
                        else
                        {
                            txtlixiyeucau.text = "<color=#ff0000ff>" + solixico + "/" + json[arlixi[i]].Value + "</color>";
                        }
                    }
                    else
                    {
                        if (json[arlixi[i]].Value == "0")
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
                if (dieukien[0] && dieukien[1] && dieukien[2] && dieukien[3] && dieukien[4] && dieukien[5])
                {
                    btndoi.interactable = true;
                }
                YeuCauNgocRong.SetActive(true);
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
                    YeuCauNgocRong.SetActive(false);
                }
                else crgame.OnThongBaoNhanh(www.downloadHandler.text, 2);
            }
        }
    }
    public void VeNha()
    {
       // transform.GetChild(0).gameObject.SetActive(false);
       // gameObject.SetActive(false);
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        AllMenu.ins.DestroyMenu("MenuEvent7VienNgocRong");
    }
    public void OpenMenuDoiManhRong(string namerong)
    {
        Button btnTrieuHoi = transform.GetChild(6).GetChild(0).transform.GetChild(3).GetComponent<Button>();
        GameObject AllmanhRong = transform.GetChild(6).GetChild(0).transform.GetChild(2).gameObject;
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
        transform.GetChild(6).gameObject.SetActive(true);
    }
    public void TrieuHoiRong()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        GameObject AllmanhRong = transform.GetChild(6).GetChild(0).transform.GetChild(2).gameObject;
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
                    transform.GetChild(6).GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
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
                    GameObject g = Instantiate(transform.GetChild(6).GetChild(1).gameObject, transform.position, Quaternion.identity);
                    g.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
                    g.gameObject.SetActive(true);
                    Image img = g.GetComponent<Image>();
                    img.sprite = Inventory.LoadSpriteRong(json["namerong"]);
                    img.SetNativeSize();
                    transform.GetChild(6).GetChild(0).transform.GetChild(4).gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.1f);
                    QuaBay quabay = img.GetComponent<QuaBay>();
                    quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                    quabay.enabled = true;
                    transform.GetChild(6).gameObject.SetActive(false);
                }
            }
        }
    }
}
