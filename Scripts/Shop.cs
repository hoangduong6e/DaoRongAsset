using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
public class Shop : MonoBehaviour
{
    public GameObject MenuMua;
    //UI trong menuMUA
    public InputField txtSoLuong ;public Text txtGia,txtGia2, TxtNameVatPham;
    public Image imgVatPham,imgTien,ImgTien2;
    long gia;
    string nameVatPham;
    public GameObject[] AllShop;public int soluong  = 1;
    Admobb admob;
    private void Awake()
    {
        MenuMua.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform,false);
        MenuMua.transform.SetSiblingIndex(0);
        admob = CrGame.ins.GetComponent<Admobb>();
    }
    void OnEnable()
    {
        LoadShop();
        for (int i = 0; i < AllShop[4].transform.childCount; i++)
        {
            Image img = AllShop[4].transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
            Friend.ins.LoadImage("avt", AllShop[4].transform.GetChild(i).name,img);
            img.name = "avatar*" + img.transform.parent.name;
        }
        if(NetworkManager.ins.vienchinh.dangdau == false) transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ClickVatPham()
    {
        AudioManager.SoundClick();
        // Image img = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        nameVatPham = EventSystem.current.currentSelectedGameObject.name;
        LoadInfoitem(nameVatPham);
    }
    public void LoadInfoitem(string nameVatpham)
    {
        nameVatPham = nameVatpham;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "XemShop";
        datasend["data"]["nameitem"] = nameVatpham;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["gia"].Value != "")
            {
                imgVatPham.sprite = Inventory.LoadSprite(nameVatpham);//img.sprite;
                imgVatPham.SetNativeSize();
                soluong = 1;
                txtSoLuong.text = "1";
                TxtNameVatPham.text = json["name"].Value;
                txtGia.text = json["gia"].Value;
                gia = long.Parse(json["gia"].Value);
                imgTien.sprite = Inventory.LoadSprite(json["tienmua"].Value);//img.transform.GetChild(3).GetComponent<Image>().sprite; 
                imgTien.SetNativeSize();
                txtGia2.gameObject.SetActive(false);
                ImgTien2.gameObject.SetActive(false);
            }
            else
            {
                nameVatpham = nameVatPham.Split('*')[1];
                NetworkManager.ins.friend.LoadImage("avt", nameVatpham, imgVatPham);
                soluong = 1;
                txtSoLuong.text = "1";
                TxtNameVatPham.text = json["name"].Value;
                txtGia.text = json["tienmua"][0]["gia"].Value;
                gia = long.Parse(json["tienmua"][0]["gia"].Value);
                imgTien.sprite = Inventory.LoadSprite(json["tienmua"][0]["name"].Value);//img.transform.GetChild(3).GetComponent<Image>().sprite; 
                imgTien.SetNativeSize();

                txtGia2.gameObject.SetActive(true);
                ImgTien2.gameObject.SetActive(true);
                txtGia2.text = json["tienmua"][1]["gia"].Value;
                gia = long.Parse(json["tienmua"][1]["gia"].Value);
                ImgTien2.sprite = Inventory.LoadSprite(json["tienmua"][1]["name"].Value);//img.transform.GetChild(3).GetComponent<Image>().sprite; 
                ImgTien2.SetNativeSize();
            }
            MenuMua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = json["thongtin"].Value;
            MenuMua.SetActive(true);
        }
    }    
    public void TangSoLuong(int add)
    {
        if (ImgTien2.gameObject.activeSelf) return;
        if (add > 0)
        {
            soluong += add;
        }
        else
        {
            if(soluong > 1)
            {
                soluong += add;
            }
        }
        txtGia.text = (gia * soluong) + "";
        txtSoLuong.text = soluong + "";
    }
    public void EndEditInput()
    {
        //if (txtSoLuong.text == "") txtSoLuong.text = "1";
        if(txtSoLuong.text != "")
        {
            soluong = int.Parse(txtSoLuong.text);
            if (soluong <= 0) soluong = 1;
            if (soluong > 9999) soluong = 9999;
            txtSoLuong.text = soluong + "";
            txtGia.text = (gia * soluong) + "";
        }    
    }    
    public void ClickMua()
    {
        AudioManager.SoundClick();
        if (txtSoLuong.text == "")
        {
            txtSoLuong.text = "1"; soluong = 1;
        }
      
        NetworkManager.ins.socket.Emit("MuaVatPham",JSONObject.CreateStringObject(nameVatPham + "+" + soluong + "+"));
    }
    public void LoadShop()
    {
    //    if(loadshop) StartCoroutine(Load());
    //    IEnumerator Load()
    //    {
    //        crgame.OnThongBao(true,"Đang tải...",false);
    //        UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XemShop/taikhoan/" + crgame.fb.id);
    //        www.downloadHandler = new DownloadHandlerBuffer();
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            debug.Log(www.error);
    //        }
    //        else
    //        {
    //            // Show results as text
    //            debug.Log(www.downloadHandler.text);
    //            JSONNode json = JSON.Parse(www.downloadHandler.text);
    //            loadshop = false;
    //            for (int i = 0; i < json.Count; i++)
    //            {
    //                GameObject shop = AllShop[i];
    //                for (int j = 0; j < json[i].Count; j++)
    //                {
    //                    for (int k = 0; k < shop.transform.childCount; k++)
    //                    {
    //                        if(shop.transform.GetChild(k).name == json[i][j]["name"].Value)
    //                        {
    //                            shop.transform.GetChild(k).gameObject.SetActive(true);
    //                        }
    //                    }
    //                }
    //            }
    //            crgame.OnThongBao(false);
    //        }
    //    }
    }

    public void XemQuangCaoShop()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        admob.namequaxem = btnchon.transform.parent.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "CheckXemQc";
        datasend["data"]["namequaxem"] = admob.namequaxem;
        NetworkManager.ins.SendServer(datasend, oK);
        void oK(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                admob.RequestRewardedVideo();
                EventManager.OpenThongBaoChon("Bạn có muốn xem quảng cáo để nhận gói quà này?", Ok);
                void Ok()
                {
                    admob.ShowRewardedVideo();
                    admob.autoXemQc = true;
                }
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }   
}
