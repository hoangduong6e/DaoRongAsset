using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using SimpleJSON;

public class GiaoDienRuongThanBi : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite imgbailat, imgbaichualat;
    private string nameRuong;
    private GameObject giaodien;
    private bool duocLatBai = true;
    public void ParseData(JSONNode json,string nameRuongg)
    {
       // debug.Log(json.ToString());
   
        gameObject.SetActive(true);
        giaodien = transform.GetChild(0).gameObject;
        Image imgquanhanduoc = giaodien.transform.Find("btnCoTheNhanDuoc").transform.GetChild(0).GetComponent<Image>();
        if (nameRuongg == "RuongThanBi") imgquanhanduoc.sprite = EventManager.ins.GetSprite("QuaRuongThanBi");
        SetSoRuongHoanThanh(json["soRuongHoanThanh"].AsString, json["soRuongDangCo"].AsString);

         GameObject ObjTheBai = giaodien.transform.Find("ObjTheBai").gameObject;
        byte soruongchuamo = 0;
        for(var i = 0; i < json["dataRuong"].Count;i++)
        {
            if (json["dataRuong"][i]["name"].AsString != "ChuaMo")
            {
                GameObject thebai = ObjTheBai.transform.GetChild(i).gameObject;
                thebai.GetComponent<Image>().sprite = imgbailat;
                //thebai.GetComponent<Button>().enabled = false;
                Image imgQua = thebai.transform.GetChild(0).GetComponent<Image>();
                Text txtqua = thebai.transform.GetChild(1).GetComponent<Text>();
                if (json["dataRuong"][i]["itemgi"].AsString == "item")
                {
                    imgQua.sprite = Inventory.LoadSprite(json["dataRuong"][i]["name"].AsString);
                    txtqua.text = json["dataRuong"][i]["soluong"].AsString;

                }
                else if (json["dataRuong"][i]["itemgi"].AsString == "itemrong")
                {
                    imgQua.sprite = Inventory.LoadSpriteRong(json["dataRuong"][i]["name"].AsString + "1");
                    txtqua.text = json["dataRuong"][i]["sao"].AsString + " Sao";
                }
                imgQua.SetNativeSize();
                imgQua.gameObject.SetActive(true);
                txtqua.gameObject.SetActive(true);
            }
            else soruongchuamo += 1;
        }
        nameRuong = json["nameRuong"].AsString;
        if (soruongchuamo == 9)
        {
            StartRollBai();
        }    
    }    
    private void SetSoRuongHoanThanh(string soRuongHoanThanh,string soRuongDangCo)
    {
        giaodien.transform.Find("txt").GetComponent<Text>().text = "Số rương đã hoàn thành: <color=yellow>" + soRuongHoanThanh + "</color>\r\n\r\nSố rương hiện có: <color=lime>" + soRuongDangCo + "</color>";
    }
    public void MoLaBai()
    {
        if (!duocLatBai) return;
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        if (btn.transform.GetChild(0).gameObject.activeSelf)
        {
            return;
        }
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "GetGiaKc";
        datasend["data"]["nameruong"] = nameRuong;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                EventManager.OpenThongBaoChon(json["message"].AsString,delegate { XacNhanMoLaBai(btn); });
            }
        }
    }
    private void XacNhanMoLaBai(GameObject btn)
    {
        if (!duocLatBai) return;
        duocLatBai = false;
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "MoLaBai";
        datasend["data"]["nameruong"] = nameRuong;
        datasend["data"]["index"] = btn.transform.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                // btn.GetComponent<Animator>().Play("LaBai");
                btn.transform.LeanScale(new Vector3(0, 1, 1), 0.3f);
                StartDelay(() => {
                    btn.GetComponent<Image>().sprite = imgbailat;
                    Image imgQua = btn.transform.GetChild(0).GetComponent<Image>();
                    Text txtqua = btn.transform.GetChild(1).GetComponent<Text>();
                    if (json["qua"]["itemgi"].AsString == "item")
                    {
                        imgQua.sprite = Inventory.LoadSprite(json["qua"]["name"].AsString);
                        txtqua.text = json["qua"]["soluong"].AsString;

                    }
                    else if (json["qua"]["itemgi"].AsString == "itemrong")
                    {
                        imgQua.sprite = Inventory.LoadSpriteRong(json["qua"]["name"].AsString + "1");
                        txtqua.text = json["qua"]["sao"].AsString + " Sao";
                    }
                    imgQua.SetNativeSize();
                    imgQua.gameObject.SetActive(true);
                    txtqua.gameObject.SetActive(true);
                    btn.transform.LeanScale(new Vector3(1, 1, 1), 0.3f);
                    //btn.GetComponent<Button>().enabled = false;
                    SetSoRuongHoanThanh(json["soRuongHoanThanh"].AsString, json["soRuongDangCo"].AsString);
                    duocLatBai = true;
                }, 0.3f);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                   duocLatBai = true;
            }

        }
    }    
    public void BtnBatDau()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "BatDau";
        datasend["data"]["nameruong"] = nameRuong;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                StartRollBai();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
     }    
    private void StartRollBai()
    {
        GameObject btnbatdau = giaodien.transform.Find("btnBatDau").gameObject;
        btnbatdau.SetActive(false);
        duocLatBai = false;
        Transform ObjTheBai = giaodien.transform.Find("ObjTheBai");
        Animator anim = ObjTheBai.GetComponent<Animator>();
        anim.enabled = true;
     //   anim.Play("Roll");
        for (int i = 0; i < ObjTheBai.transform.childCount; i++)
        {
            Transform thebai = ObjTheBai.transform.GetChild(i);
            Image imgthebai = thebai.GetComponent<Image>();
            imgthebai.sprite = imgbaichualat;
            for (int j = 0; j < thebai.transform.childCount; j++)
            {
                thebai.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
    public void DonePhatBai()
    {
        Transform ObjTheBai = giaodien.transform.Find("ObjTheBai");

        Animator anim = ObjTheBai.GetComponent<Animator>();
      //  anim.Play("idlle");
        anim.enabled = false;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitUntil(()=> anim.enabled == false);
            for (int i = 0; i < ObjTheBai.transform.childCount; i++)
            {
                Transform thebai = ObjTheBai.transform.GetChild(i);
                Image imgthebai = thebai.GetComponent<Image>();
                imgthebai.sprite = imgbaichualat;
              //  thebai.GetComponent<Button>().enabled = true;
            }
            duocLatBai = true;
            GameObject btnbatdau = giaodien.transform.Find("btnBatDau").gameObject;
            btnbatdau.SetActive(true);
        }
      
    }    
    public void ExitMenu()
    {
        //if(duocLatBai)
       // {
            // gameObject.SetActive(false);
            EventManager.ins.DestroyMenu("GiaoDienRuongThanBi");
        //}
    }
    public void StartDelay(Action actionDelay, float sec)
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(sec);
            actionDelay();
        }
    }
}
