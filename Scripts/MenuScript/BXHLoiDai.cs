﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class BXHLoiDai : MonoBehaviour
{
    public Sprite Top1, Top2, Top3, Top;
    public int trang = 1;
    public GameObject AllTop;
    public Text txtTrang;
    // Start is called before the first frame update
    void Start()
    {
        GetTopp();
    }
    public void SangTrang(int i)
    {
        if(i > 0 && trang < 100)
        {
            trang += 1;
            GetTopp();
        }
        else if(i < 0 && trang > 1)
        {
            trang -= 1;
            GetTopp();
        }
    }
    void GetTopp()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "GetTopLoiDai";
        datasend["data"]["top"] = (trang - 1).ToString();
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                for (int i = 0; i < json.Count; i++)
                {
                    GameObject Topp = AllTop.transform.GetChild(i).gameObject;
                    Image imgAvatar = Topp.transform.GetChild(1).GetComponent<Image>();
                    Image imgKhungAvatar = Topp.transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json[i]["idfb"].Value;
                    Image HuyHieu = Topp.transform.GetChild(3).GetComponent<Image>();
                    Text txtName = Topp.transform.GetChild(4).GetComponent<Text>();
                    Text txtTop = Topp.transform.GetChild(5).GetComponent<Text>();
                    txtTop.text = json[i]["Top"].Value;
                    if (json[i]["idfb"].Value != "bot")
                    {
                        // net.friend.GetAvatarFriend(json[i]["idfb"].Value, imgAvatar);
                    }
                    Friend.ins.LoadAvtFriend(json[i]["idfb"].Value, imgAvatar, imgKhungAvatar);
                    //  imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json[i]["Toc"].Value);

                    int sotop = int.Parse(json[i]["Top"].Value);
                    if (sotop > 3) HuyHieu.sprite = Top;
                    else if (sotop == 1) HuyHieu.sprite = Top1;
                    else if (sotop == 2) HuyHieu.sprite = Top2;
                    else if (sotop == 3) HuyHieu.sprite = Top3;
                    HuyHieu.SetNativeSize();
                    txtName.text = json[i]["Name"].Value;
                    //   CrGame.ins.OnThongBao(false);
                    AllTop.SetActive(true);
                    txtTrang.text = trang + "/100";
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }
    public void XemThongTin()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        //net.friend.idObjectFriend = objitem.name;
        //net.friend.QuaNhaFriend(objitem.GetComponent<Image>().sprite);
        //net.menuLoiDai.SetActive(false);
        //gameObject.SetActive(false);
        MenuTTNguoichoi menutt = AllMenu.ins.GetCreateMenu("GiaoDienThongTin", gameObject.transform.parent.gameObject, false, transform.GetSiblingIndex() + 1).GetComponent<MenuTTNguoichoi>();
        menutt.transform.SetParent(gameObject.transform.parent,false);
        menutt.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        menutt.ID = btnchon.name;
        menutt.gameObject.SetActive(true);
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuTopLoiDai");
    }    
}
