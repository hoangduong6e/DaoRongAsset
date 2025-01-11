using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuTrungSinhRong : MonoBehaviour
{
    // Start is called before the first frame update
    string idrong;
    private void OnEnable()
    {
        OpenNangSaoRong();
    }
    private void ClearRongGd()
    {
        GameObject g = transform.GetChild(0).gameObject;
        Button btnTrungSinh = g.transform.Find("btnTrungSinh").GetComponent<Button>();
        btnTrungSinh.interactable = false;
        Transform RongChon = g.transform.Find("RongChon");
        Transform RongNangCap = g.transform.Find("RongNangCap");
        for (int i = 0; i < RongChon.transform.childCount; i++) Destroy(RongChon.transform.GetChild(i).gameObject);
        for (int i = 0; i < RongNangCap.transform.childCount; i++) Destroy(RongNangCap.transform.GetChild(i).gameObject);

    }
    private void SetRongGd(string namerongchon,string namerongnangcap)
    {
        GameObject g = transform.GetChild(0).gameObject;
        Transform RongChon = g.transform.Find("RongChon");
        Transform RongNangCap = g.transform.Find("RongNangCap");
        AllMenu.ins.LoadRongGiaoDien(namerongchon, RongChon);
        AllMenu.ins.LoadRongGiaoDien(namerongnangcap, RongNangCap);
    }    
    public void OpenNangSaoRong()
    {
        GameObject contentRong = transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject allitemcan = transform.GetChild(0).transform.Find("allitemcan").gameObject;
        Image item1 = allitemcan.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        item1.sprite = Inventory.LoadSprite("QuaThong");
        transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Trùng sinh Rồng Event";
        transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Hạt Thông</color> để <color=#ffa500ff>Trùng sinh</color> <color=#ffff00ff>Rồng Event</color> <color=#ff0000ff>trường thành</color> 2-21 sao. Sau khi <color=#ffa500ff>Trùng sinh</color> sẽ trờ thành rồng <color=#ff0000ff>baby</color> ";
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            Destroy(contentRong.transform.GetChild(i).gameObject);
        }
        GameObject g = transform.GetChild(0).gameObject;

        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                    if (itemdra.nameObjectDragon == "RongVang" && nameimg == "RongVang2" || itemdra.nameObjectDragon == "RongBac" && nameimg == "RongBac2" ||
                        itemdra.nameObjectDragon == "RongLuaMatXanh" && nameimg == "RongLuaMatXanh2" || itemdra.nameObjectDragon == "RongLuaMatXanhGiapThin" && nameimg == "RongLuaMatXanh2" ||
                             itemdra.nameObjectDragon == "RongLuaMatXanhSapphire" && nameimg == "RongLuaMatXanhSapphire2" || itemdra.nameObjectDragon == "RongLuaMatXanhSapphireGiapThin" && nameimg == "RongLuaMatXanhSapphire2" ||
                             itemdra.nameObjectDragon == "RongXuong" && nameimg == "RongXuong2" ||
                             itemdra.nameObjectDragon == "RongTuanLong" && nameimg == "RongTuanLong2" || itemdra.nameObjectDragon == "RongPhuongHoangBang" && nameimg == "RongPhuongHoangBang2" || itemdra.nameObjectDragon == "RongPhuongHoangLua" && nameimg == "RongPhuongHoangLua2" 
                             
                             || itemdra.nameObjectDragon == "RongKhongTuoc" && nameimg == "RongKhongTuoc2" || itemdra.nameObjectDragon == "RongKhongTuocValentine" && nameimg == "RongKhongTuoc2" ||
                             itemdra.nameObjectDragon == "RongNguyetLong" && nameimg == "RongNguyetLong2" || itemdra.nameObjectDragon == "RongNguyetLongGiapThin" && nameimg == "RongNguyetLong2" ||
                              itemdra.nameObjectDragon == "RongKyLanDo" && nameimg == "RongKyLanDo2" || itemdra.nameObjectDragon == "RongMaTroiGiapThin" && nameimg == "RongMaTroi2"
                              || itemdra.nameObjectDragon == "RongRua" && nameimg == "RongRua2" || itemdra.nameObjectDragon == "RongNguSac" && nameimg == "RongNguSac2"
                              || itemdra.nameObjectDragon == "RongPhuongHoangDungNham" && nameimg == "RongPhuongHoangDungNham2"
                              || itemdra.nameObjectDragon == "RongMatXe" && nameimg == "RongMatXe2"
                              || itemdra.nameObjectDragon == "RongHuyetNguyetLong" && nameimg == "RongHuyetNguyetLong2"
                              || itemdra.nameObjectDragon == "RongMaThach1" && nameimg == "RongMaThach12"
                              || itemdra.nameObjectDragon == "RongMaThach2" && nameimg == "RongMaThach22"
                              || itemdra.nameObjectDragon == "RongMaThach3" && nameimg == "RongMaThach32"
                              || itemdra.nameObjectDragon == "RongHacLong" && nameimg == "RongHacLong2"
                             ) 
                    {

                        if (itemdra.nameObjectDragon == "RongXuong" && nameimg == "RongXuong2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 16 && int.Parse(itemdra.txtSao.text) < 20)
                            {
                                SetRong();
                            }

                        }
                        else if (itemdra.nameObjectDragon == "RongKhongTuoc" && nameimg == "RongKhongTuoc2" || itemdra.nameObjectDragon == "RongKhongTuocValentine" && nameimg == "RongKhongTuoc2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 10 && int.Parse(itemdra.txtSao.text) <= 29)
                            {
                                SetRong();
                            }

                        }
                        else if (itemdra.nameObjectDragon == "RongNguyetLong" && nameimg == "RongNguyetLong2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 10 && int.Parse(itemdra.txtSao.text) <= 22)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongPhuongHoangBang" && nameimg == "RongPhuongHoangBang2"
                          || itemdra.nameObjectDragon == "RongPhuongHoangLua" && nameimg == "RongPhuongHoangLua2" || itemdra.nameObjectDragon == "RongKyLanDo" && nameimg == "RongKyLanDo2"
                          )
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 10 && int.Parse(itemdra.txtSao.text) <= 20)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongPhuongHoangDungNham" && nameimg == "RongPhuongHoangDungNham2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 16 && int.Parse(itemdra.txtSao.text) <= 19)
                            {
                                SetRong();
                            }
                        }
                        else if(itemdra.nameObjectDragon == "RongTuanLong" && nameimg == "RongTuanLong2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 10 && int.Parse(itemdra.txtSao.text) <= 29)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongMaTroiGiapThin" && nameimg == "RongMaTroi2" || itemdra.nameObjectDragon == "RongNguyetLongGiapThin" && nameimg == "RongNguyetLong2" || itemdra.nameObjectDragon == "RongLuaMatXanhSapphireGiapThin" && nameimg == "RongLuaMatXanhSapphire2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 16 && int.Parse(itemdra.txtSao.text) <= 22)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongRua" && nameimg == "RongRua2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 10 && int.Parse(itemdra.txtSao.text) <= 22)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongNguSac" && nameimg == "RongNguSac2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 10 && int.Parse(itemdra.txtSao.text) <= 25)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongMatXe" && nameimg == "RongMatXe2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 16 && int.Parse(itemdra.txtSao.text) <= 20)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongHuyetNguyetLong" && nameimg == "RongHuyetNguyetLong2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 23 && int.Parse(itemdra.txtSao.text) <= 24)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongMaThach1" && nameimg == "RongMaThach12"
                              || itemdra.nameObjectDragon == "RongMaThach2" && nameimg == "RongMaThach22"
                              || itemdra.nameObjectDragon == "RongMaThach3" && nameimg == "RongMaThach32")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 10 && int.Parse(itemdra.txtSao.text) <= 22)
                            {
                                SetRong();
                            }
                        }
                        else if (itemdra.nameObjectDragon == "RongHacLong" && nameimg == "RongHacLong2")
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 16 && int.Parse(itemdra.txtSao.text) <= 29)
                            {
                                SetRong();
                            }
                        }
                        else
                        {
                            if (int.Parse(itemdra.txtSao.text) >= 2 && int.Parse(itemdra.txtSao.text) < 22)
                            {
                                SetRong();
                            }
                        }
                  
                        void SetRong()
                        {
                            GameObject rong = Instantiate(contentRong.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                            rong.transform.SetParent(contentRong.transform, false);
                            // ite
                            rong.name = itemdra.name;
                            Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                            imgRong.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "2"); imgRong.SetNativeSize();
                            rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                            rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                            // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                            rong.SetActive(true);
                        }
                    }
                }
            }
        }
    }    
    public void OpenChuyenHoaRong()
    {
        GameObject contentRong = transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject allitemcan = transform.GetChild(0).transform.Find("allitemcan").gameObject;
        for (int i = 0; i < allitemcan.transform.childCount; i++)
        {
            allitemcan.transform.GetChild(i).gameObject.SetActive(false);
        }
        Image item1 = allitemcan.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        item1.sprite = Inventory.LoadSprite("NgocEmerald");
        transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Chuyển hóa Rồng Lửa Mắt Xanh";
        transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Ngọc Emerald</color> để <color=#ffa500ff>Chuyển hóa</color> <color=#ffff00ff>Rồng Lửa Mắt Xanh</color> <color=#ff0000ff>trường thành</color> trên 15 sao thành <color=#ffff00ff>Rồng Lửa Mắt Xanh Sapphire</color> ";
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            Destroy(contentRong.transform.GetChild(i).gameObject);
        }
        GameObject g = transform.GetChild(0).gameObject;
        // SpriteRenderer spriteRongChon = g.transform.Find("SpriteRongChon").GetComponent<SpriteRenderer>();
        // Text txtQuaThong = transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
        // SpriteRenderer SpriteRongNang = g.transform.Find("SpriteRongNangCap").GetComponent<SpriteRenderer>();
        //spriteRongChon.enabled = false; SpriteRongNang.enabled = false;
        ClearRongGd();
    //    txtQuaThong.gameObject.SetActive(false);

        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                    if (int.Parse(itemdra.txtSao.text) > 15)
                    {
                        if (itemdra.nameObjectDragon == "RongLuaMatXanh" && nameimg == "RongLuaMatXanh2" || itemdra.nameObjectDragon == "RongLuaMatXanhGiapThin" && nameimg == "RongLuaMatXanh2")
                        {
                            GameObject rong = Instantiate(contentRong.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                            rong.transform.SetParent(contentRong.transform, false);
                            // ite
                            rong.name = itemdra.name;
                            Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                            imgRong.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "2"); imgRong.SetNativeSize();
                            rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                            rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                            // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                            rong.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void OpenChuyenHoaRongXuong()
    {
        GameObject contentRong = transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject allitemcan = transform.GetChild(0).transform.Find("allitemcan").gameObject;
        Image item1 = allitemcan.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        item1.sprite = Inventory.LoadSprite("SungMaTroi");
        transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Chuyển hóa Rồng Ma Trơi";
        transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Sừng Ma Trơi</color> để <color=#ffa500ff>Chuyển hóa</color> <color=#ffff00ff>Rồng Xương</color> <color=#ff0000ff>trường thành</color> 20 sao thành <color=#ffff00ff>Rồng Ma Trơi</color> ";
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            Destroy(contentRong.transform.GetChild(i).gameObject);
        }
        GameObject g = transform.GetChild(0).gameObject;
        //SpriteRenderer spriteRongChon = g.transform.Find("SpriteRongChon").GetComponent<SpriteRenderer>();
        // Text txtQuaThong = transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
        //SpriteRenderer SpriteRongNang = g.transform.Find("SpriteRongNangCap").GetComponent<SpriteRenderer>();
        // txtQuaThong.gameObject.SetActive(false);
        ClearRongGd();
        for (int i = 0; i < allitemcan.transform.childCount; i++)
        {
            allitemcan.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                    if (int.Parse(itemdra.txtSao.text) == 20)
                    {
                        if (itemdra.nameObjectDragon == "RongXuong" && nameimg == "RongXuong2")
                        {
                            GameObject rong = Instantiate(contentRong.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                            rong.transform.SetParent(contentRong.transform, false);
                            // ite
                            rong.name = itemdra.name;
                            Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                            imgRong.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "2"); imgRong.SetNativeSize();
                            rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                            rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                            // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                            rong.SetActive(true);
                        }
                    }
                }
            }
        }
    }
    public void OpenChuyenHoaRongNguyetLong()
    {
        GameObject contentRong = transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject allitemcan = transform.GetChild(0).transform.Find("allitemcan").gameObject;
        Image item1 = allitemcan.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        item1.sprite = Inventory.LoadSprite("SungMaTroi");
        transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Chuyển hóa Rồng Huyết Nguyệt Long";
        transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Trăng Tròn</color> để <color=#ffa500ff>Chuyển hóa</color> <color=#ffff00ff>Rồng Nguyệt Long</color> <color=#ff0000ff>trường thành</color> 23 sao thành <color=#ffff00ff>Rồng Huyết Nguyệt Long</color> ";
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            Destroy(contentRong.transform.GetChild(i).gameObject);
        }
        GameObject g = transform.GetChild(0).gameObject;
        //SpriteRenderer spriteRongChon = g.transform.Find("SpriteRongChon").GetComponent<SpriteRenderer>();
        // Text txtQuaThong = transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
        //SpriteRenderer SpriteRongNang = g.transform.Find("SpriteRongNangCap").GetComponent<SpriteRenderer>();
        // txtQuaThong.gameObject.SetActive(false);
        ClearRongGd();
        for (int i = 0; i < allitemcan.transform.childCount; i++)
        {
            allitemcan.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                    if (int.Parse(itemdra.txtSao.text) == 23)
                    {
                        if (itemdra.nameObjectDragon == "RongNguyetLong" && nameimg == "RongNguyetLong2")
                        {
                            GameObject rong = Instantiate(contentRong.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                            rong.transform.SetParent(contentRong.transform, false);
                            // ite
                            rong.name = itemdra.name;
                            Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                            imgRong.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "2"); imgRong.SetNativeSize();
                            rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                            rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                            // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                            rong.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void OpenChuyenHoaRongMaThach()
    {
        GameObject contentRong = transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject allitemcan = transform.GetChild(0).transform.Find("allitemcan").gameObject;
        Image item1 = allitemcan.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        item1.sprite = Inventory.LoadSprite("SungMaTroi");
        transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Chuyển hóa Rồng Ma Thạch";
        transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Nọc nhện đen</color> để <color=#ffa500ff>Chuyển hóa</color> <color=#ffff00ff>Rồng Ma Thạch</color> <color=#ff0000ff>trường thành</color> Cấp 1 thành <color=#ffff00ff>Rồng Ma Thạch trưởng thành </color> Cấp 2";
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            Destroy(contentRong.transform.GetChild(i).gameObject);
        }
        GameObject g = transform.GetChild(0).gameObject;
        //SpriteRenderer spriteRongChon = g.transform.Find("SpriteRongChon").GetComponent<SpriteRenderer>();
        // Text txtQuaThong = transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
        //SpriteRenderer SpriteRongNang = g.transform.Find("SpriteRongNangCap").GetComponent<SpriteRenderer>();
        // txtQuaThong.gameObject.SetActive(false);
        ClearRongGd();
        for (int i = 0; i < allitemcan.transform.childCount; i++)
        {
            allitemcan.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                    if (itemdra.nameObjectDragon == "RongMaThach1" && nameimg == "RongMaThach12" || itemdra.nameObjectDragon == "RongMaThach2" && nameimg == "RongMaThach22")
                    {
                        GameObject rong = Instantiate(contentRong.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                        rong.transform.SetParent(contentRong.transform, false);
                        // ite
                        rong.name = itemdra.name;
                        Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                        imgRong.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "2"); imgRong.SetNativeSize();
                        rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                        rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                        // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                        rong.SetActive(true);
                    }
                }
            }
        }
    }
    public void ChonRongNangSao()
    {
        CrGame.ins.panelLoadDao.SetActive(true) ;
        AudioManager.PlaySound("soundClick");
        // Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        // int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        GameObject chon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        idrong = chon.transform.parent.name;
        string load = "TrungSinh", namerong = "", saorong = "";
        for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
        {
            if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).name == idrong)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    Image img = itemdra.transform.GetChild(0).GetComponent<Image>();
                    namerong = itemdra.nameObjectDragon;
                    saorong = itemdra.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text;
                    if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Trùng sinh Rồng Event")
                    {
                        
                    }
                    else
                    {
                        load = "ChuyenHoa";
                    }
                    img.SetNativeSize();
                    break;
                }
            }
        }

        JSONClass datasend = new JSONClass();
        datasend["class"] = "TrungSinh";
        datasend["method"] = "GetItemTrungSinh";
        datasend["data"]["idrong"] = idrong;
        datasend["data"]["load"] = load;
        datasend["data"]["namerong"] = namerong;
        datasend["data"]["saorong"] = saorong;
        GameObject g = transform.GetChild(0).gameObject;
        Button btnTrungSinh = g.transform.Find("btnTrungSinh").GetComponent<Button>();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "ok")
            {
               
                string nameobjectrong = chon.GetComponent<Image>().sprite.name;
              

            
             //   SpriteRenderer spriteRongChon = g.transform.Find("SpriteRongChon").GetComponent<SpriteRenderer>();
                // Text txtQuaThong = transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
               // SpriteRenderer SpriteRongNang = g.transform.Find("SpriteRongNangCap").GetComponent<SpriteRenderer>();

                Text txtsaotruoc = g.transform.Find("txtsaotruoc").GetComponent<Text>();
                Text txtsaosau = g.transform.Find("txtsaosau").GetComponent<Text>();
              //  spriteRongChon.enabled = true; SpriteRongNang.enabled = true;
                //  txtQuaThong.gameObject.SetActive(true);
                GameObject allitemcan = g.transform.Find("allitemcan").gameObject;
                for (int i = 0; i < allitemcan.transform.childCount; i++)
                {
                    allitemcan.transform.GetChild(i).gameObject.SetActive(false);
                }
                
               // Image item2 = allitemcan.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();

                //Animator animrongchon = spriteRongChon.GetComponent<Animator>();
                //Animator animrongnang = SpriteRongNang.GetComponent<Animator>();
                //animrongnang.runtimeAnimatorController = animrongchon.runtimeAnimatorController;

                if (load == "TrungSinh")
                {
                    txtsaotruoc.text = saorong.ToString() + " sao";
                    txtsaosau.text = (int.Parse(saorong) + 1) + " sao";
                    SetRongGd(namerong + "2",namerong + "1");
                }    
                else
                {
                    
                    txtsaotruoc.text = saorong.ToString() + " sao";
                    txtsaosau.text = saorong.ToString() + " sao";
                    if(namerong == "RongNguyetLong")
                    {
                        txtsaosau.text = (int.Parse(saorong.ToString())-1) + " sao";
                    }
                    //animrongnang.runtimeAnimatorController = Inventory.LoadAnimator(json["namechuyenhoa"].AsString);
                    //animrongnang.SetInteger("TienHoa", 2);
                    if(namerong == "RongMaThach1" || namerong == "RongMaThach2")
                    {
                        SetRongGd(namerong + "2", json["namechuyenhoa"].AsString + "2");
                    }    
                    else SetRongGd(namerong + "2", json["namechuyenhoa"].AsString + "1");

                }
                byte ok = 0;
                for (int i = 0; i < json["allitem"].Count; i++)
                {
                    Text txtyeucau = allitemcan.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    Image imgitem = allitemcan.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                    int itemco = Inventory.ins.GetItem(json["allitem"][i]["nameitem"].AsString);
                    int itemcan = json["allitem"][i]["soluong"].AsInt;
                    imgitem.sprite = Inventory.LoadSprite(json["allitem"][i]["nameitem"].AsString);
                    txtyeucau.text = json["allitem"][i]["txtyeucau"].AsString;
                    imgitem.SetNativeSize();
                    GamIns.ResizeItem(imgitem);
                    //if (itemco >= itemcan)
                    //{
                    //    txtyeucau.text = "<color=#00ff00ff>" + itemco + "/" + itemcan + "</color>";
                    //    ok += 1;
                    //}
                    //else
                    //{
                    //    txtyeucau.text = "<color=#ff0000ff>" + itemco + "/" + itemcan + "</color>";
                    //}
                    allitemcan.transform.GetChild(i).gameObject.SetActive(true);
                }
                if(json["ok"].AsBool)
                {
                    btnTrungSinh.interactable = true;
                }
                else btnTrungSinh.interactable = false;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
            }
            CrGame.ins.panelLoadDao.SetActive(false);
        }

       

        //int yeucau = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.GetChild(2).GetComponent<Text>().text);
        //int yeucau2 = 0;



        //animrongchon.runtimeAnimatorController = Inventory.LoadAnimator(nameobjectrong.Substring(0, nameobjectrong.Length - 1));
        //animrongchon.SetInteger("TienHoa", 2);

        //string nameitemyeucau = "itemQuaThong";
        //string nameitem2 = "";

        //  if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Trùng sinh Rồng Event")
        //   {

        //if (animrongchon.runtimeAnimatorController.name == "RongTuanLong")
        //{
        //    nameitemyeucau = "itemNgocAquamarine";
        //    item1.sprite = Inventory.LoadSprite("NgocAquamarine");
        //    transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Ngọc Aquamarine</color> để <color=#ffa500ff>Trùng sinh</color> <color=#ffff00ff>Tuần Long</color> <color=#ff0000ff>trường thành</color> 10-20 sao. Sau khi <color=#ffa500ff>Trùng sinh</color> sẽ trờ thành rồng <color=#ff0000ff>baby</color> ";
        //}
        //else if(animrongchon.runtimeAnimatorController.name == "RongKhongTuoc")
        //{
        //    nameitemyeucau = "itemChocolate";
        //    if(yeucau < 20) yeucau = 20; // nếu dưới 20 sao thì cần 20 socola
        //    else yeucau = 30;
        //    item1.sprite = Inventory.LoadSprite("Chocolate");
        //    transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#800000ff>Chocolate</color> để <color=#ffa500ff>Trùng sinh</color> <color=#ffff00ff>Khổng Tước</color> <color=#ff0000ff>trường thành</color> 10-19 sao. Sau khi <color=#ffa500ff>Trùng sinh</color> sẽ trờ thành rồng <color=#ff0000ff>baby</color> ";
        //}
        //else if (animrongchon.runtimeAnimatorController.name == "RongNguyetLong")
        //{
        //    nameitemyeucau = "itemTrangKhuyet";
        //    nameitem2 = "itemThucAnKeoSuaBoKobe";
        //    item1.sprite = Inventory.LoadSprite("TrangKhuyet");

        //    item2.sprite = Inventory.LoadSprite("ThucAnKeoSuaBoKobe");
        //    transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Trăng khuyết</color> và <color=#00ff00ff>Kẹo sữa bò kobe</color> để <color=#ffa500ff>Trùng sinh</color> <color=#ffff00ff>Nguyệt Long</color> <color=#ff0000ff>trường thành</color> 10-17 sao. Sau khi <color=#ffa500ff>Trùng sinh</color> sẽ trờ thành rồng <color=#ff0000ff>baby</color> ";
        //    for (int i = 11; i <=yeucau; i++)
        //    {
        //        if(i % 2 == 0)
        //        {
        //            yeucau2 += 1;
        //        }    
        //    }
        //}
        //else
        //{
        //    if (animrongchon.runtimeAnimatorController.name == "RongPhuongHoangBang" || animrongchon.runtimeAnimatorController.name == "RongPhuongHoangLua") yeucau *= 2;
        //    item1.sprite = Inventory.LoadSprite("QuaThong");
        //    transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Yêu cầu vật phẩm <color=#00ff00ff>Hạt Thông</color> để <color=#ffa500ff>Trùng sinh</color> <color=#ffff00ff>Rồng Event</color> <color=#ff0000ff>trường thành</color> 2-21 sao. Sau khi <color=#ffa500ff>Trùng sinh</color> sẽ trờ thành rồng <color=#ff0000ff>baby</color> ";
        //}
        //      }
        //else if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Lửa Mắt Xanh")
        //{
        //    nameitemyeucau = "itemNgocEmerald";
        //    txtsaotruoc.text = yeucau.ToString() + " sao";
        //    txtsaosau.text = yeucau.ToString() + " sao";
        //    yeucau = 2;
        //    animrongnang.runtimeAnimatorController = Inventory.LoadAnimator("RongLuaMatXanhSapphire");
        //    animrongnang.SetInteger("TienHoa", 2);
        //}
        //else if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Ma Trơi")
        //{
        //    nameitemyeucau = "itemSungMaTroi";
        //    txtsaotruoc.text = yeucau.ToString() + " sao";
        //    txtsaosau.text = yeucau.ToString() + " sao";
        //    yeucau = 2;
        //    animrongnang.runtimeAnimatorController = Inventory.LoadAnimator("RongMaTroi");
        //    animrongnang.SetInteger("TienHoa", 2);
        //}
        //Text txtyeucau1 = allitemcan.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        //Button btnTrungSinh = g.transform.Find("btnTrungSinh").GetComponent<Button>();
        //allitemcan.transform.GetChild(0).gameObject.SetActive(true);
        //if (Inventory.ins.ListItemThuong.ContainsKey(nameitemyeucau))
        //{
        //    int soquathongco = int.Parse(Inventory.ins.ListItemThuong[nameitemyeucau].transform.GetChild(0).GetComponent<Text>().text);
        //    if (soquathongco >= yeucau)
        //    {
        //        txtyeucau1.text = "<color=#00ff00ff>" + soquathongco + "/" + yeucau + "</color>";
        //        btnTrungSinh.interactable = true;
        //    }
        //    else
        //    {
        //        txtyeucau1.text = "<color=#ff0000ff>" + soquathongco + "/" + yeucau + "</color>";
        //        btnTrungSinh.interactable = false;
        //    }
        //}
        //else
        //{
        //    txtyeucau1.text = "<color=#ff0000ff>0/" + yeucau + "</color>";
        //    btnTrungSinh.interactable = false;
        //}
        //Text txtyeucau2 = allitemcan.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        //if (nameitem2 != "" && yeucau2 > 0)
        //{
        //    allitemcan.transform.GetChild(1).gameObject.SetActive(true);
        //    if (Inventory.ins.ListItemThuong.ContainsKey(nameitem2))
        //    {
        //        int soquathongco = int.Parse(Inventory.ins.ListItemThuong[nameitem2].transform.GetChild(0).GetComponent<Text>().text);
        //        if (soquathongco >= yeucau2 && btnTrungSinh.interactable)
        //        {
        //            txtyeucau2.text = "<color=#00ff00ff>" + soquathongco + "/" + yeucau2 + "</color>";
        //            btnTrungSinh.interactable = true;
        //        }
        //        else
        //        {
        //            txtyeucau2.text = "<color=#ff0000ff>" + soquathongco + "/" + yeucau2 + "</color>";
        //            btnTrungSinh.interactable = false;
        //        }
        //    }
        //    else
        //    {
        //        txtyeucau2.text = "<color=#ff0000ff>0/" + yeucau2 + "</color>";
        //        btnTrungSinh.interactable = false;
        //    }
        //}
        //else allitemcan.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void CloseMenuNangSao()
    {
        AllMenu.ins.DestroyMenu("menuNangSaoRong");
        //gameObject.SetActive(false);
    }
    public void TrungSinh()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        CrGame.ins.panelLoadDao.SetActive(true);
        string load = "TrungSinh"; 
        if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Lửa Mắt Xanh" 
            || transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Ma Trơi"
            || transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Huyết Nguyệt Long"
            || transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Ma Thạch"
            ) load = "ChuyenHoa";

        AudioManager.PlaySound("soundClick");
       // Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
       // int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TrungSinh";
        datasend["method"] = load;
        datasend["data"]["idrong"] = idrong;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                for (int i = 0; i < json["allitem"].Count; i++)
                {
                    Inventory.ins.AddItem(json["allitem"][i]["nameitem"].AsString,-json["allitem"][i]["soluong"].AsInt);
                }
                for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                {
                    if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).name == idrong)
                        {
                            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            Image img = itemdra.transform.GetChild(0).GetComponent<Image>();

                            if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Trùng sinh Rồng Event")
                            {
                                img.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "1");
                                CrGame.ins.OnThongBaoNhanh("Trùng sinh thành công");
                            }
                            else
                            {
                                if(json["namerongchuyenhoa"].AsString == "RongHuyetNguyetLong")
                                {
                                    img.sprite = Inventory.LoadSpriteRong(json["namerongchuyenhoa"].AsString + "1");
                                    itemdra.txtSao.text = "22";
                                }
                                else img.sprite = Inventory.LoadSpriteRong(json["namerongchuyenhoa"].AsString + "2");

                                CrGame.ins.OnThongBaoNhanh("Chuyển hóa thành công");
                            }
                            img.SetNativeSize();
                            CloseMenuNangSao();
                            break;
                        }
                    }
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
            }
            CrGame.ins.panelLoadDao.SetActive(false);
        }



        //StartCoroutine(Load());
        //IEnumerator Load()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + load + "/taikhoan/" + LoginFacebook.ins.id + "/idrong/" + idrong);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.OnThongBaoNhanh("Lỗi");
        //        btndoi.interactable = true;
        //    }
        //    else
        //    {
        //        // Show results as text
        //        //   btndoi.interactable = false;
        //        debug.Log(www.downloadHandler.text);
        //        if (www.downloadHandler.text == "0")
        //        {
        //            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
        //            {
        //                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
        //                {
        //                    if (Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).name == idrong)
        //                    {
        //                        ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
        //                        Image img = itemdra.transform.GetChild(0).GetComponent<Image>();
                                
        //                        if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Trùng sinh Rồng Event")
        //                        {
        //                            img.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "1");
        //                            int itemquathongcan = int.Parse(itemdra.txtSao.text);
        //                            if (itemdra.nameObjectDragon == "RongPhuongHoangBang" || itemdra.nameObjectDragon == "RongPhuongHoangLua")
        //                            {
        //                                itemquathongcan *= 2;
        //                            } 
                                        
        //                            else if (itemdra.nameObjectDragon == "RongTuanLong") Inventory.ins.AddItem("NgocAquamarine", -int.Parse(itemdra.txtSao.text));
        //                            else if (itemdra.nameObjectDragon == "RongKhongTuoc")
        //                            {
        //                                //ko
        //                            }
        //                            else if (itemdra.nameObjectDragon == "RongNguyetLong")
        //                            {
        //                                int gia2 = 0;
        //                                for (int j = 11; j < int.Parse(itemdra.txtSao.text); j++)
        //                                {
        //                                    if(j % 2 == 0)
        //                                    {
        //                                        gia2 += 1;
        //                                    }
        //                                }
        //                                Inventory.ins.AddItem("TrangKhuyet", -int.Parse(itemdra.txtSao.text));
        //                                Inventory.ins.AddItem("ThucAnKeoSuaBoKobe", -gia2);
        //                            }
        //                            else
        //                            {
        //                                Inventory.ins.AddItem("QuaThong", -itemquathongcan);
        //                            }
        //                            CrGame.ins.OnThongBaoNhanh("Trùng sinh thành công");
        //                        }
        //                        else if(transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Lửa Mắt Xanh")
        //                        {
        //                            img.sprite = Inventory.LoadSpriteRong("RongLuaMatXanhSapphire2");
        //                            Inventory.ins.AddItem("NgocEmerald", -2);
        //                            CrGame.ins.OnThongBaoNhanh("Chuyển hóa thành công");
        //                        }
        //                        else if (transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text == "Chuyển hóa Rồng Ma Trơi")
        //                        {
        //                            img.sprite = Inventory.LoadSpriteRong("RongMaTroi2");
        //                            Inventory.ins.AddItem("SungMaTroi", -2);
        //                            CrGame.ins.OnThongBaoNhanh("Chuyển hóa thành công");
        //                        }
        //                        img.SetNativeSize();
        //                        CloseMenuNangSao();
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        else CrGame.ins.OnThongBaoNhanh(www.downloadHandler.text, 2);
        //    }
        //}
    }
}
