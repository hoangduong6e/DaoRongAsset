using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class KhamNgoc : MonoBehaviour
{
    public Animator animRong;
    public GameObject SlotNgoc, contentNgoc;
    public GameObject oNgoc;
    public Image maskHp, maskSatthuong, MaskHutHp, maskChiMang, maskNeTranh;
    public Text txtHp, txtSatThuong, txthuthp, txtchimang, txtnetranh;
    // Start is called before the first frame update
    public void AddTuiNgoc(string namengoc,int soluongadd)
    {
        if (contentNgoc.transform.childCount == 0)
        {
            GameObject Slot = Instantiate(SlotNgoc, transform.position, Quaternion.identity);
            Slot.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = soluongadd.ToString();
            Image imgNgoc = Slot.transform.GetChild(0).GetComponent<Image>();
            Slot.name = namengoc;
            imgNgoc.sprite = Inventory.LoadSprite(namengoc);
            imgNgoc.SetNativeSize();
            Slot.transform.SetParent(contentNgoc.transform, false);
            Slot.SetActive(true);
            return;
        }
        for (int i = 0; i < contentNgoc.transform.childCount; i++)
        {
            if (namengoc == contentNgoc.transform.GetChild(i).name)
            {
                Text txtsoluong = contentNgoc.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                int soluongngoc = int.Parse(txtsoluong.text);
                soluongngoc += soluongadd;
                if (soluongngoc <= 0)
                {
                    Destroy(contentNgoc.transform.GetChild(i).gameObject);
                }
                txtsoluong.text = soluongngoc.ToString();
                break;
            }
            else if (i == contentNgoc.transform.childCount - 1)
            {
                GameObject Slot = Instantiate(SlotNgoc, transform.position, Quaternion.identity);
                Slot.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = soluongadd.ToString();
                Image imgNgoc = Slot.transform.GetChild(0).GetComponent<Image>();
                Slot.name = namengoc;
                imgNgoc.sprite = Inventory.LoadSprite(namengoc);
                imgNgoc.SetNativeSize();
                Slot.transform.SetParent(contentNgoc.transform, false);
                Slot.SetActive(true);
                break;
            }
        }
    }    
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuKhamNgoc");
        //if(contentNgoc.transform.childCount > 1)
        //{
        //    for (int i = 1; i < contentNgoc.transform.childCount; i++)
        //    {
        //        Destroy(contentNgoc.transform.GetChild(i).gameObject);
        //    }
        //}    
        //oNgoc.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
        //oNgoc.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(true);

        //for (int i = 0; i < 3; i++)
        //{
        //    oNgoc.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);// btn thao ngoc

        //    oNgoc.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);// img ngoc
        //    oNgoc.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        //    oNgoc.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
        //}
        //gameObject.SetActive(false);
    }
    public void ChonNgocKham()
    {
        string namengockham = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        string[] namengoc = namengockham.Split('-');
        tbc.txtThongBao.text = "Bạn muốn khảm ngọc " + GetName(namengoc[0]) + " cấp <color=#ff0000ff>" + namengoc[1] + "</color> vào rồng ?";
        tbc.btnChon.onClick.AddListener(delegate { Kham(namengockham, CrGame.ins.TfrongInfo.name); });
        //  tbc.btnChon.onClick.AddListener(VaoDau);
    }    
    string GetName(string s)
    {
        if (s == "ngocdo")
        {
            return "<color=#ff0000ff>Đỏ</color>";
        }
        else if (s == "ngoctim")
        {
            return "<color=#ff00ffff>Tím</color>";
        }
        else if (s == "ngocvang")
        {
            return "<color=#ffff00ff>Vàng</color>";
        }
        else if (s == "ngocluc")
        {
            return "<color=#00ffffff>Lục</color>";
        }
        else if (s == "ngoclam")
        {
            return "<color=#00ff00ff>Lam</color>";
        }
        else return "";
    }    
    public void Kham(string namengoc,string idrong)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "KhamNgoc";
        datasend["data"]["idrong"] = idrong;
        datasend["data"]["ngoc"] = namengoc;
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["tb"].Value == "thành công")
            {
                if (json["ngocrong"]["ngoc1"]["name"].Value != "lock" && json["ngocrong"]["ngoc1"]["name"].Value != "")
                {
                    oNgoc.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    Image imgngoc = oNgoc.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
                    imgngoc.gameObject.SetActive(true);
                    imgngoc.sprite = Inventory.LoadSprite(json["ngocrong"]["ngoc1"]["name"].Value);
                    imgngoc.name = json["ngocrong"]["ngoc1"]["name"].Value;
                }
                if (json["ngocrong"]["ngoc2"]["name"].Value != "lock" && json["ngocrong"]["ngoc2"]["name"].Value != "")
                {
                    oNgoc.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                    Image imgngoc = oNgoc.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
                    imgngoc.gameObject.SetActive(true);
                    imgngoc.sprite = Inventory.LoadSprite(json["ngocrong"]["ngoc2"]["name"].Value);
                    imgngoc.name = json["ngocrong"]["ngoc2"]["name"].Value;
                }
                if (json["ngocrong"]["ngoc3"]["name"].Value != "lock" && json["ngocrong"]["ngoc3"]["name"].Value != "")
                {
                    oNgoc.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                    Image imgngoc = oNgoc.transform.GetChild(2).transform.GetChild(1).GetComponent<Image>();
                    imgngoc.gameObject.SetActive(true);
                    imgngoc.sprite = Inventory.LoadSprite(json["ngocrong"]["ngoc3"]["name"].Value);
                    imgngoc.name = json["ngocrong"]["ngoc3"]["name"].Value;
                }
                maskChiMang.fillAmount = float.Parse(json["chisongoc"]["tilechimang"].Value) / 100;
                maskNeTranh.fillAmount = float.Parse(json["chisongoc"]["netranh"].Value) / 100;
                MaskHutHp.fillAmount = float.Parse(json["chisongoc"]["hutmau"].Value) / 100;
                txtHp.text = "+" + json["chisongoc"]["hp"].Value;
                txtSatThuong.text = "+" + json["chisongoc"]["sucdanh"].Value;
                txtchimang.text = "+" + json["chisongoc"]["tilechimang"].Value + "%";
                txtnetranh.text = "+" + json["chisongoc"]["netranh"].Value + "%";
                txthuthp.text = "+" + json["chisongoc"]["hutmau"].Value + "%";
                AddTuiNgoc(namengoc, -1);
                Inventory.ins.AddNgoc(namengoc, -1);
            }
            else
            {
                //CrGame.ins.OnThongBao(true, json["tb"].Value, true);
                CrGame.ins.OnThongBaoNhanh(json["tb"].Value);
            }
            if(AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            CrGame.ins.panelLoadDao.SetActive(false);
        }
    }
    public void ThaoNgoc()
    {
        int oRongThao = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.GetSiblingIndex() + 1;
        string namengockham = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.GetChild(1).name;
        Button btnThao = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btnThao.interactable = false;
        string idRongThao = CrGame.ins.TfrongInfo.name;

        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "ThaoNgoc";
        datasend["data"]["vitri"] = oRongThao.ToString();
        datasend["data"]["idrong"] = idRongThao;
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["tb"].Value == "thành công")
            {
                if (json["ngocrong"]["ngoc1"]["name"].Value == "")
                {
                    oNgoc.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                    oNgoc.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                    oNgoc.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().interactable = true;
                }
                if (json["ngocrong"]["ngoc2"]["name"].Value == "")
                {
                    oNgoc.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
                    oNgoc.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                    oNgoc.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().interactable = true;
                }
                if (json["ngocrong"]["ngoc3"]["name"].Value == "")
                {
                    oNgoc.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
                    oNgoc.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
                    oNgoc.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().interactable = true;
                }
                maskChiMang.fillAmount = float.Parse(json["chisongoc"]["tilechimang"].Value) / 100;
                maskNeTranh.fillAmount = float.Parse(json["chisongoc"]["netranh"].Value) / 100;
                MaskHutHp.fillAmount = float.Parse(json["chisongoc"]["hutmau"].Value) / 100;
                txtHp.text = "+" + json["chisongoc"]["hp"].Value;
                txtSatThuong.text = "+" + json["chisongoc"]["sucdanh"].Value;
                txtchimang.text = "+" + json["chisongoc"]["tilechimang"].Value + "%";
                txtnetranh.text = "+" + json["chisongoc"]["netranh"].Value + "%";
                txthuthp.text = "+" + json["chisongoc"]["hutmau"].Value + "%";
                AddTuiNgoc(namengockham, 1);
                Inventory.ins.AddNgoc(namengockham, 1);
            }
            else
            {
                //CrGame.ins.OnThongBao(true, json["tb"].Value, true);
                CrGame.ins.OnThongBaoNhanh(json["tb"].Value);
            }
            btnThao.interactable = true;
            CrGame.ins.panelLoadDao.SetActive(false);
        }


        //StartCoroutine(KhamNgoc());
        //IEnumerator KhamNgoc()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ThaoNgoc/vitri/" + oRongThao + "/idrong/" + idRongThao + "/taikhoan/" + LoginFacebook.ins.id);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();
        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        btnThao.interactable = true;
        //        debug.Log(www.error);
        //    } 
        //    else
        //    {
        //        JSONNode json = JSON.Parse(www.downloadHandler.text);
        //        if (json["tb"].Value == "thành công")
        //        {
        //            if (json["ngocrong"]["ngoc1"]["name"].Value == "")
        //            {
        //                oNgoc.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        //                oNgoc.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        //                oNgoc.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().interactable = true;
        //            }
        //            if (json["ngocrong"]["ngoc2"]["name"].Value == "")
        //            {
        //                oNgoc.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
        //                oNgoc.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        //                oNgoc.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().interactable = true;
        //            }
        //            if (json["ngocrong"]["ngoc3"]["name"].Value == "")
        //            {
        //                oNgoc.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
        //                oNgoc.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
        //                oNgoc.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().interactable = true;
        //            }
        //            maskChiMang.fillAmount = float.Parse(json["chisongoc"]["tilechimang"].Value) / 100;
        //            maskNeTranh.fillAmount = float.Parse(json["chisongoc"]["netranh"].Value) / 100;
        //            MaskHutHp.fillAmount = float.Parse(json["chisongoc"]["hutmau"].Value) / 100;
        //            txtHp.text = "+" + json["chisongoc"]["hp"].Value;
        //            txtSatThuong.text = "+" + json["chisongoc"]["sucdanh"].Value;
        //            txtchimang.text = "+" + json["chisongoc"]["tilechimang"].Value + "%";
        //            txtnetranh.text = "+" + json["chisongoc"]["netranh"].Value + "%";
        //            txthuthp.text = "+" + json["chisongoc"]["hutmau"].Value + "%";
        //            AddTuiNgoc(namengockham, 1);
        //            Inventory.ins.AddNgoc(namengockham, 1);
        //        }
        //        else
        //        {
        //            CrGame.ins.OnThongBao(true, json["tb"].Value, true);
        //        }
        //        btnThao.interactable = true;
        //        debug.Log(www.downloadHandler.text);
        //    }
        //}
    }
    public void MoKhoa()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Bạn muốn mở ô ngọc này với 500 hoa tuyết ?";
        tbc.btnChon.onClick.AddListener(Unlock);
    }
    public void Unlock()
    {
        string idRong = CrGame.ins.TfrongInfo.name;
        StartCoroutine(unlock());
        IEnumerator unlock()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "unlockNgoc/idrong/" + idRong + "/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) debug.Log(www.error);
            else
            {
                if (AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["tb"].Value == "thành công")
                {
                    oNgoc.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBao(true, json["tb"].Value, true);
                }
                debug.Log(www.downloadHandler.text);
            }
        }
    } 
}
