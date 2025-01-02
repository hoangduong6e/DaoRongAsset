using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class InfoLevelPhoBan : MonoBehaviour
{
    public Text txtNameLevel,txtManhVoThuNguyen,TxtGiaManhVoThuNguyen,txtsomanhvo,txtsohuyentinh;
    public GameObject SlotVatPham,menuInfo,SlotItem;
    public GameObject objectTang; 
    // Start is called before the first frame update
    public void CloseMenu()
    {
        for (int i = 1; i < SlotVatPham.transform.childCount; i++)
        {
            Destroy(SlotVatPham.transform.GetChild(i).gameObject);
        }
        menuInfo.SetActive(false);
    }
    public void XemLevel(string namemap)
    {
        XemPhoBan xemphoban = AllMenu.ins.menu["menuPhoban"].GetComponent<XemPhoBan>();


        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "xemLevelPhoBan";
        datasend["data"]["name"] = namemap;
        datasend["data"]["cap"] = "0";
        datasend["data"]["idcongtrinh"] = "0";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].Value == "0")
            {
                JSONNode Json = jsonn["data"];
                if (Json.AsString != "khoa")
                {
                    objectTang.SetActive(false);
                    // debug.Log(Json["vatphamcothethuduoc"][0].Value);
                    txtNameLevel.text = Json["namelevel"].Value;
                    txtManhVoThuNguyen.text = "Chiến đấu bằng <color=#00ffffff>Mảnh Vỡ Thứ Nguyên:</color><color=#00ff00ff>" + Json["somanhvothunguyen"].Value + "</color>";
                    if (Inventory.ins.ListItemThuong.ContainsKey("itemManhVoThuNguyen"))
                    {
                        if (int.Parse(Inventory.ins.ListItemThuong["itemManhVoThuNguyen"].transform.GetChild(0).GetComponent<Text>().text) >= int.Parse(Json["somanhvothunguyen"].Value))
                        {
                            TxtGiaManhVoThuNguyen.transform.parent.GetComponent<Button>().interactable = true;
                        }
                        else TxtGiaManhVoThuNguyen.transform.parent.GetComponent<Button>().interactable = false;
                    }
                    else TxtGiaManhVoThuNguyen.transform.parent.GetComponent<Button>().interactable = false;
                    TxtGiaManhVoThuNguyen.text = Json["somanhvothunguyen"].Value;
                    for (int i = 0; i < Json["vatphamcothethuduoc"].Count; i++)
                    {
                        GameObject item = Instantiate(SlotItem, transform.position, Quaternion.identity) as GameObject;
                        debug.Log(Json["vatphamcothethuduoc"][i]["name"].Value);
                        //SpriteRenderer spritevatpham = GameObject.Find("Sprite" + Json["vatphamcothethuduoc"][i]["name"].Value).GetComponent<SpriteRenderer>();
                        Sprite spritevatpham = Resources.Load<Sprite>("GameData/Sprite/" + Json["vatphamcothethuduoc"][i]["name"].Value);//GameObject.Find(Json["vatphamcothethuduoc"][i]["name"].Value).GetComponent<Image>();
                        item.transform.SetParent(SlotVatPham.transform, false);
                        Image img = item.transform.GetChild(0).GetComponent<Image>();
                        img.sprite = spritevatpham; img.SetNativeSize();
                        item.transform.GetChild(1).GetComponent<Text>().text = Json["vatphamcothethuduoc"][i]["hiem"].Value;
                        item.SetActive(true);
                    }
                    if (Json["tang"] != null)
                    {
                        objectTang.SetActive(true);
                        if (Json["tang"].Value == "1")
                        {
                            objectTang.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                            objectTang.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                        }
                        else
                        {
                            objectTang.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                            objectTang.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
                        }
                        GameObject prefab = VienChinh.vienchinh.transform.GetChild(1).gameObject;
                        for (int j = 0; j < prefab.transform.childCount; j++)
                        {
                            if (prefab.transform.GetChild(j).name == Json["nameboss1"].Value)
                            {
                                Image imgboss = objectTang.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                                imgboss.sprite = prefab.transform.GetChild(j).GetComponent<SpriteRenderer>().sprite;
                                imgboss.SetNativeSize();
                            }
                            if (prefab.transform.GetChild(j).name == Json["nameboss2"].Value)
                            {
                                Image imgboss = objectTang.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
                                imgboss.sprite = prefab.transform.GetChild(j).GetComponent<SpriteRenderer>().sprite;
                                imgboss.SetNativeSize();
                            }
                        }
                    }
                    //   GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = Json["randomtext"][Random.Range(0, Json["randomtext"].Count)].Value;
                    //  GiaoDienPVP.ins.maxtime = float.Parse(Json["time"].Value) * 60;
                    VienChinh.vienchinh.SetBGMap(Json["BackGround"].Value);
                    //  VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite();
                    menuInfo.SetActive(true);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh("Đang khóa", 1f);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2.5f);
            }
        }
    }
    public void TrangBi()
    {
        GameObject tc = GameObject.FindGameObjectWithTag("trencung");
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", tc, true, tc.transform.GetSiblingIndex() - 1);
    }
    public void VaoMap()
    {
        XemPhoBan xemphoban = AllMenu.ins.menu["menuPhoban"].GetComponent<XemPhoBan>();
        xemphoban.VaoMap();
    }    
    public void VeNha()
    {
        XemPhoBan xemphoban = AllMenu.ins.menu["menuPhoban"].GetComponent<XemPhoBan>();
        xemphoban.CloseMenu();
    }
}
