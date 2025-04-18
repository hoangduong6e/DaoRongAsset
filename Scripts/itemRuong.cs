﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using static UnityEngine.Rendering.DebugUI;

public class itemRuong : MonoBehaviour
{
    public string nameruong;
    // Update is called once per frame
    public void XemRuong(bool autoquay)
    {
        if (Friend.ins.QuaNha == false)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "Main";
            datasend["method"] = "UseItem";
            datasend["data"]["nameitem"] = nameruong;
            NetworkManager.ins.SendServer(datasend, Ok,true);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    AllMenu.ins.OpenMenu("MenuGiaoDienRuong");
                    MenuQuayRuong quayruong = AllMenu.ins.menu["MenuGiaoDienRuong"].GetComponent<MenuQuayRuong>();
                    for (int i = 0; i < quayruong.Oquay.transform.childCount; i++)
                    {
                        if (json["qua"][i]["nameitem"].Value != "")
                        {
                            Image imgitem = quayruong.Oquay.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                            imgitem.sprite = Inventory.LoadSprite(json["qua"][i]["nameitem"]); imgitem.SetNativeSize();
                            quayruong.Oquay.transform.GetChild(i).gameObject.name = json["qua"][i]["nameitem"].Value;
                            imgitem.gameObject.SetActive(true);
                            Text txtsoluong = quayruong.Oquay.transform.GetChild(i).GetChild(1).GetComponent<Text>();
                            txtsoluong.text = json["qua"][i]["soluong"].Value; txtsoluong.gameObject.SetActive(true);
                        }
                        else if (json["qua"][i]["namerong"] != "")
                        {
                            Image imgitemrong = quayruong.Oquay.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                            imgitemrong.sprite = Inventory.LoadSpriteRong(json["qua"][i]["namerong"].Value + json["qua"][i]["tienhoa"].Value);
                            imgitemrong.gameObject.SetActive(true); imgitemrong.SetNativeSize();
                            quayruong.Oquay.transform.GetChild(i).gameObject.name = json["qua"][i]["namerong"].Value;
                            Text txtsoluong = quayruong.Oquay.transform.GetChild(i).GetChild(1).GetComponent<Text>();
                            txtsoluong.text = json["qua"][i]["sao"].Value + " sao";
                            txtsoluong.gameObject.SetActive(true);
                        }
                        //  debug.Log(json["qua"][i]["namerong"].Value);
                    }
                    quayruong.nameRuong = nameruong;
                    quayruong.LoadSoLanQuay(float.Parse(json["solanquayruong"].Value));
                    for (int i = 0; i < json["quaRuong"].Count; i++)
                    {
                        Image imgMoc = quayruong.Qua.transform.GetChild(i).GetComponent<Image>();
                        Image imgQua = quayruong.Qua.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                        GameObject btnnhan = quayruong.Qua.transform.GetChild(i).GetChild(1).gameObject;
                        if (json["quaRuong"][i].Value == "duocnhan")
                        {
                            imgMoc.sprite = quayruong.spdenmoc;
                            imgQua.sprite = quayruong.spduocnhan;
                            btnnhan.SetActive(true);
                        }
                        else if (json["quaRuong"][i].Value == "chuaduocnhan")
                        {
                            imgMoc.sprite = quayruong.spchuadenmoc;
                            imgQua.sprite = quayruong.spchuanhan;
                            btnnhan.SetActive(false);
                        }
                        else if (json["quaRuong"][i].Value == "danhan")
                        {
                            imgMoc.sprite = quayruong.spdenmoc;
                            imgQua.sprite = quayruong.spdanhan;
                            btnnhan.SetActive(false);
                        }
                    }
                    CrGame.ins.panelLoadDao.SetActive(false);
                    if (autoquay)
                    {
                      //  Debug.Log("auto quay");
                        quayruong.QuayRuong();
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString); AllMenu.ins.CloseMenu("MenuGiaoDienRuong");
                }
            }

        }
        else
        {

            GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua", null, false);
            Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            imgItemTang.sprite = GetComponent<Image>().sprite;
            Friend.ins.MaxSoluong = int.Parse(transform.GetChild(0).GetComponent<Text>().text);
            Friend.ins.XemTangQua("item*" + nameruong);
        }
    }
}
