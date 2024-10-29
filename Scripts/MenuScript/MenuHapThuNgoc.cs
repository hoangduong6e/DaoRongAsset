using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuHapThuNgoc : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform contentNgoc;
    public GameObject SlotNgoc;
    public Image imgNgoc;
    public Text txtTime, txtSoLuong, txtchiso,txthientai;
    JSONNode chisohapthu, thoigianhapthu;
    public Animator animChonNgoc;
    //float time = 0;
    bool settime = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (settime)
        {
            if (CrGame.ins.timeNuiThanBi > 1)
            {
            //    time -= Time.deltaTime;
                txtTime.text = CrGame.ParseTime(CrGame.ins.timeNuiThanBi);
            }
            else
            {
                settime = false;
                imgNgoc.gameObject.SetActive(false);
                txtTime.text = CrGame.ParseTime(0);
                transform.GetChild(0).transform.Find("objhapthu").gameObject.SetActive(true);
                txthientai.text = "";
                namengoc = "";
                animChonNgoc.gameObject.SetActive(false);
            }
        }
    
    }

    string GetName(string s)
    {
        if (s == "ngocdo")
        {
            return "<color=#ff0000ff>Sức Đánh</color>";
        }
        else if (s == "ngoctim")
        {
            return "<color=#ff00ffff>Chí Mạng</color>";
        }
        else if (s == "ngocvang")
        {
            return "<color=#ffff00ff>Máu</color>";
        }
        else if (s == "ngocluc")
        {
            return "<color=#00ffffff>Né</color>";
        }
        else if (s == "ngoclam")
        {
            return "<color=#00ff00ff>Hút Máu</color>";
        }
        else return "";
    }
    int soluong = 0;
    int maxsoluong = 0;
    public void ParseData(JSONNode json)
    {
        for (int i = 0; i < json["dataa"]["allngoc"].Count; i++)
        {
            string strnamengoc = json["dataa"]["allngoc"][i]["namengoc"].AsString;
            if (strnamengoc.Split('+').Length == 1) AddTuiNgoc(json["dataa"]["allngoc"][i]["namengoc"].AsString, json["dataa"]["allngoc"][i]["soluong"].AsInt);

        }
        chisohapthu = json["dataa"]["chisohapthu"];
        thoigianhapthu = json["dataa"]["thoigianhapthu"];

        if (json["HapThuNgoc"]["ngoc"].AsString != "")
        {
            imgNgoc.sprite = Inventory.LoadSprite(json["HapThuNgoc"]["ngoc"].AsString);
            imgNgoc.SetNativeSize();
            //    txtTime.text = CrGame.ParseTime(json["time"].AsFloat);
            CrGame.ins.timeNuiThanBi = json["time"].AsFloat;
            transform.GetChild(0).transform.Find("objhapthu").gameObject.SetActive(false);
            animChonNgoc.gameObject.SetActive(true);
            settime = true;
            imgNgoc.gameObject.SetActive(true);
            txthientai.text = LoadTxtHienTai(json["HapThuNgoc"]["ngoc"].AsString, json["timehientai"].AsFloat);
    
        }
        gameObject.SetActive(true);
    }
    string namengoc;
    public void ChonNgocKham()
    {
        //    if (!transform.GetChild(0).transform.Find("objhapthu").gameObject.activeSelf) return;
        if (txthientai.text != "") return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;

        imgNgoc.sprite = btnchon.GetComponent<Image>().sprite;
        imgNgoc.SetNativeSize();
        imgNgoc.gameObject.SetActive(true);

        namengoc = imgNgoc.sprite.name;
     
        maxsoluong = int.Parse(btnchon.transform.GetChild(0).GetComponent<Text>().text);
        txtSoLuong.text = "1";
         soluong = 1;
        txtchiso.text = LoadTxtChiSo(namengoc);
        transform.GetChild(0).transform.Find("objhapthu").gameObject.SetActive(true);

    }
    public void HapThu()
    {
        if (namengoc == "")
        {
            CrGame.ins.OnThongBaoNhanh("Hãy chọn ngọc để Hấp Thụ!");
            return;
        }    
        JSONClass datasend = new JSONClass();
        datasend["class"] = "HapThuNgoc";
        datasend["method"] = "HapThu";
        datasend["data"]["namengoc"] = namengoc;
        datasend["data"]["soluong"] = soluong.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString()) ;
               // txtTime.text = CrGame.ParseTime(json["time"].AsFloat);

                AddTuiNgoc(namengoc,-soluong);
                Inventory.ins.AddItem(namengoc,-soluong);

                transform.GetChild(0).transform.Find("objhapthu").gameObject.SetActive(false);
                CrGame.ins.timeNuiThanBi = json["time"].AsFloat;
                txthientai.text = LoadTxtHienTai(namengoc, CrGame.ins.timeNuiThanBi);
                settime = true;

                NuiLua.Instance.MauNgoc = (_mauNgoc)Enum.Parse(typeof(_mauNgoc), namengoc.Split('-')[0]);
                txtchiso.text = "";
                animChonNgoc.gameObject.SetActive(true);
                animChonNgoc.Play("effhapthu1");
           //     animChonNgoc.GetComponent<RectTransform>().sizeDelta = new Vector2(350,350);
         //       EventManager.StartDelay2(() => { animChonNgoc.SetActive(false); },1.1f);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    private string LoadTxtHienTai(string namengoc,float timee)
    {
        string[] cat = namengoc.Split('-');
        string namengocat = cat[0];
        float chiso = chisohapthu[namengoc].AsFloat;
        string strtangthem = chiso.ToString();
        if (namengocat == "ngoclam" || namengocat == "ngoctim" || namengocat == "ngocluc") strtangthem += "%";

        return "<color=lime>Hiện tại:</color> Tăng thêm <color=yellow>" + strtangthem + "</color><color=lime> " + GetName(namengocat) + "</color> trong <color=red>" + CrGame.ParseTime(timee) + "</color>";

    }
    private string LoadTxtChiSo(string namengoc)//"<color=lime>Hiện tại:</color>"
    {
        string[] cat = namengoc.Split('-');
        string namengocat = cat[0];
        float chiso = chisohapthu[namengoc].AsFloat;
        string strtangthem = chiso.ToString();
        if (namengocat == "ngoclam" || namengocat == "ngoctim" || namengocat == "ngocluc") strtangthem += "%";
        float time = thoigianhapthu[int.Parse(cat[1])].AsFloat * soluong;

        return "<color=red>Hấp Thụ:</color> Tăng thêm <color=yellow>" + strtangthem + "</color><color=lime> " + GetName(namengocat) + "</color> trong <color=red>" + CrGame.ParseTime(time) + "</color>";

    }
    public void TangSoLuong(int soluongg)
    {
        soluong += soluongg;
        if (soluong < 1) soluong = 1;
        if (soluong > maxsoluong) soluong = maxsoluong;
        txtSoLuong.text = soluong.ToString();
        txtchiso.text = LoadTxtChiSo(namengoc);
    }    
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuHapThuNgoc");
 
      //  gameObject.SetActive(false);
    }
    public void AddTuiNgoc(string namengoc, int soluongadd)
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

}
