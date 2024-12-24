using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPhongAn : MonoBehaviour
{
    public GameObject oItem, ContentDanhSachKhoBau, ObjKhoBau;
    private MenuEventNoel2023 ev;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ParseData(JSONNode json)
    {
        ev = EventManager.ins.GetComponent<MenuEventNoel2023>();
        debug.Log(json.ToString());
        for(int i = 0; i < json["GiaoDienPhongAn"]["DanhSachKhoBau"].Count;i++)
        {
            GameObject newObjKhoBau = Instantiate(ObjKhoBau, transform.position, Quaternion.identity) as GameObject;
            newObjKhoBau.transform.SetParent(ContentDanhSachKhoBau.transform,false);
         //   newObjKhoBau.transform.position = ContentDanhSachKhoBau.transform.position;
            byte soluongdadoi = (byte)json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["soluongdadoi"].AsInt;
            byte maxsoluongdoi = (byte)json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["maxsoluongdoi"].AsInt;

            string soluonghienthi = "";
            if(soluongdadoi >= maxsoluongdoi)
            {
                soluonghienthi = "<color=red>" + soluongdadoi + "/" + maxsoluongdoi + "</color>";
            }
            else soluonghienthi = "<color=lime>" + soluongdadoi + "/" + maxsoluongdoi + "</color>";
            Text txtnameitem = newObjKhoBau.transform.Find("txtnameitem").GetComponent<Text>();
            Text txtsoluongdadoi = newObjKhoBau.transform.Find("ttxyeucau").GetComponent<Text>();
            Text txtsoitem = newObjKhoBau.transform.Find("txtsoitem").GetComponent<Text>();
            txtnameitem.text = json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["nametv"].AsString;
            txtsoluongdadoi.text = soluonghienthi;
            Image imgItem = newObjKhoBau.transform.Find("imgitem").GetComponent<Image>();
            if (json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["itemgi"].AsString == "item")
            {
                imgItem.sprite = Inventory.LoadSprite(json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["nameitem"].AsString);
                txtsoitem.text = json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["soluong"].AsString;
            }
            else if (json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["itemgi"].AsString == "rong")
            {
                Vector3 scale = imgItem.transform.localScale;
                scale.x /= 2;
                scale.y /= 2;
                imgItem.transform.localScale = scale;
                imgItem.sprite = Inventory.LoadSpriteRong(json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["nameitem"].AsString + "1");
                txtsoitem.text = json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["sao"].AsString + " sao";
            }
            imgItem.SetNativeSize();
            newObjKhoBau.gameObject.SetActive(true);
            newObjKhoBau.name = json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["nameitem"].AsString;
            imgItem.name = json["yeucaungoctraiKhoBau"][json["GiaoDienPhongAn"]["DanhSachKhoBau"][i]["nameitem"].AsString].AsString;
        }
        if (json["GiaoDienPhongAn"]["itemkhoa"].AsString != "")
        {
            GameObject objdskhobau = ContentDanhSachKhoBau.transform.Find(json["GiaoDienPhongAn"]["itemkhoa"].AsString).gameObject;
            oItem.transform.GetChild(0).gameObject.SetActive(false);
            oItem.transform.GetChild(1).gameObject.SetActive(true);
            GameObject obj = oItem.transform.GetChild(1).gameObject;
            Image img = obj.GetComponent<Image>();
            img.sprite = objdskhobau.transform.GetChild(1).GetComponent<Image>().sprite;
            img.transform.GetChild(0).GetComponent<Text>().text = objdskhobau.transform.Find("txtsoitem").GetComponent<Text>().text;
            img.SetNativeSize();
            transform.GetChild(0).Find("btnCongHien").GetComponent<Button>().interactable = true;
            GameObject allitemconghien = transform.GetChild(0).Find("allitemCongHien").gameObject;
            for (int i = 0; i < json["GiaoDienPhongAn"]["allitemCongHien"].Count; i++)
            {
                if (json["GiaoDienPhongAn"]["allitemCongHien"][i].AsBool)
                {
                    allitemconghien.transform.GetChild(i).gameObject.SetActive(true);
                    allitemconghien.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = objdskhobau.transform.GetChild(1).gameObject.name;
                    transform.GetChild(0).Find("btnGiaiPhongAn").GetComponent<Button>().interactable = true;
                }
                if(i == 2 && json["GiaoDienPhongAn"]["allitemCongHien"][2].AsBool) transform.GetChild(0).Find("btnCongHien").GetComponent<Button>().interactable = false;
            }

            transform.GetChild(0).transform.Find("txtYeuCau").GetComponent<Text>().text = "Yêu cầu: " + objdskhobau.transform.GetChild(1).gameObject.name;
            transform.GetChild(0).transform.Find("imgphantram").transform.GetChild(0).GetComponent<Text>().text = json["tylePhongAn"].AsString + "%";
        }
        transform.GetChild(0).transform.Find("itemNgocTraiVang").transform.GetChild(1).GetComponent<Text>().text = json["NgocTraiVang"].AsString;
        transform.GetChild(0).transform.Find("itemManhNgocTrai").transform.GetChild(1).GetComponent<Text>().text = json["ManhNgocTrai"].AsString;

        LoadHienTiLePhongAn(json["GiaoDienPhongAn"]["lanGiaiPhongAn"].AsInt);
    }    
    private void LoadHienTiLePhongAn(int lanGiaiPhongAn)
    {
        GameObject imgTangTiLe = transform.GetChild(0).transform.Find("imgTangTiLe").gameObject;
        GameObject imgden = imgTangTiLe.transform.Find("imgden").gameObject;
        for (int i = 0; i < imgTangTiLe.transform.childCount - 2; i++)
        {
            imgTangTiLe.transform.Find(i.ToString()).transform.SetSiblingIndex(0);
        }
        imgTangTiLe.transform.Find((lanGiaiPhongAn - 1).ToString()).transform.SetSiblingIndex(imgden.transform.GetSiblingIndex() + 1);
    }    
    public void ChonKhoBau()
    {
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject DanhSachItem = transform.Find("DanhSachItem").gameObject;
        GameObject oitem = DanhSachItem.transform.Find("Oitem").gameObject;
    
        Image img = oitem.transform.GetChild(0).GetComponent<Image>();
        img.gameObject.SetActive(true);
        img.sprite = objchon.GetComponent<Image>().sprite;
        img.SetNativeSize();
        img.transform.GetChild(0).GetComponent<Text>().text = "Mỗi phong ấn yêu cầu: " + objchon.name;
        DanhSachItem.transform.Find("btnKhoa").GetComponent<Button>().interactable = true;
        img.gameObject.name = objchon.transform.parent.name;
        img.transform.GetChild(0).gameObject.name = objchon.transform.parent.Find("txtsoitem").GetComponent<Text>().text;
    }    
    public void KhoaKhoBau()
    {
        GameObject DanhSachItem = transform.Find("DanhSachItem").gameObject;
        GameObject oitemdanhsach = DanhSachItem.transform.Find("Oitem").gameObject;
        // GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "KhoaItemPhongAn";
        datasend["data"]["nameitem"] = oitemdanhsach.transform.GetChild(0).name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                CrGame.ins.OnThongBaoNhanh("Đã khóa!", 2);
                oItem.transform.GetChild(0).gameObject.SetActive(false);
                GameObject obj = oItem.transform.GetChild(1).gameObject;
                obj.gameObject.SetActive(true);
                Image img = obj.GetComponent<Image>();
                img.sprite = oitemdanhsach.transform.GetChild(0).GetComponent<Image>().sprite;
                obj.transform.GetChild(0).GetComponent<Text>().text = oitemdanhsach.transform.GetChild(0).transform.GetChild(0).gameObject.name;
                img.SetNativeSize();
                DanhSachItem.SetActive(false);
              //  transform.GetChild(0).Find("btnGiaiPhongAn").GetComponent<Button>().interactable = true;
                transform.GetChild(0).Find("btnCongHien").GetComponent<Button>().interactable = true;

                transform.GetChild(0).transform.Find("txtYeuCau").GetComponent<Text>().text = "Yêu cầu: " + ContentDanhSachKhoBau.transform.Find(json["itemkhoa"].AsString).transform.GetChild(1).gameObject.name;
                transform.GetChild(0).transform.Find("imgphantram").transform.GetChild(0).GetComponent<Text>().text = json["tylePhongAn"].AsString + "%";
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
            }
        }
    }    
    public void CongHien()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true, ev.giaodiennut1.transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
        tbc.transform.SetAsLastSibling();
        tbc.btnChon.onClick.RemoveAllListeners();
        tbc.txtThongBao.text = "Cống hiến " + transform.GetChild(0).transform.Find("txtYeuCau").GetComponent<Text>().text + " Ngọc Trai Vàng";
        tbc.btnChon.onClick.AddListener(XacNhanCongHien);
    }

    private void XacNhanCongHien()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "CongHienPhongAn";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                GameObject allitemconghien = transform.GetChild(0).Find("allitemCongHien").gameObject;
                for (int i = 0; i < allitemconghien.transform.childCount; i++)
                {
                    if (!allitemconghien.transform.GetChild(i).gameObject.activeSelf)
                    {
                        allitemconghien.transform.GetChild(i).gameObject.SetActive(true);
                        transform.GetChild(0).transform.Find("itemNgocTraiVang").transform.GetChild(1).GetComponent<Text>().text = json["NgocTraiVang"].AsString;
                        allitemconghien.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = json["giaCongHien"].AsString;
                        transform.GetChild(0).transform.Find("imgphantram").transform.GetChild(0).GetComponent<Text>().text = json["tylePhongAn"].AsString + "%";
                        transform.GetChild(0).Find("btnGiaiPhongAn").GetComponent<Button>().interactable = true;
                        break;
                    }
                    if (i == 2 && allitemconghien.transform.GetChild(2).gameObject.activeSelf) transform.GetChild(0).Find("btnCongHien").GetComponent<Button>().interactable = false;
                }
                CrGame.ins.OnThongBaoNhanh("Đã cống hiến", 2);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
        }
    }    
    public void ClickDauCong()
    {
        transform.Find("DanhSachItem").gameObject.SetActive(true);
    }
    public void ExitDanhSachItem()
    {
        GameObject DanhSachItem = transform.Find("DanhSachItem").gameObject;
        GameObject oitem = DanhSachItem.transform.Find("Oitem").gameObject;
        oitem.transform.GetChild(0).gameObject.SetActive(false);
        DanhSachItem.SetActive(false);
        DanhSachItem.transform.Find("btnKhoa").GetComponent<Button>().interactable = false;
    }
    public void GiaiPhongAn()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = ev.nameEvent;
        datasend["method"] = "GiaiPhongAn";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            GameObject allitemconghien = transform.GetChild(0).Find("allitemCongHien").gameObject;
            if (json["status"].Value == "0" || json["status"].Value == "2")
            {
                transform.GetChild(0).Find("btnGiaiPhongAn").GetComponent<Button>().interactable = false;
                transform.GetChild(0).Find("btnCongHien").GetComponent<Button>().interactable = false;
                transform.GetChild(0).Find("btnExit").GetComponent<Button>().interactable = false;
            }

            GameObject allTiaSet = transform.GetChild(0).transform.Find("allTiaSet").gameObject;

            for (int i = 0; i < allitemconghien.transform.childCount; i++)
            {
                if (allitemconghien.transform.GetChild(i).gameObject.activeSelf)
                {
                    allTiaSet.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        

            if (json["status"].Value == "0")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.3f);
                    transform.GetChild(0).transform.Find("MargicCircle").GetComponent<Animator>().Play("Success");
                
                    yield return new WaitForSeconds(8f);
                    GameObject objdskhobau = ContentDanhSachKhoBau.transform.Find(json["GiaoDienPhongAn"]["itemkhoa"].AsString).gameObject;
                    oItem.transform.GetChild(0).gameObject.SetActive(true);
                    oItem.transform.GetChild(1).gameObject.SetActive(false);

                    GameObject PanelNhanQua = transform.Find("PanelNhanQua").gameObject;
                    PanelNhanQua.SetActive(true);
                    Image img = PanelNhanQua.transform.GetChild(1).GetComponent<Image>();
                    img.sprite = oItem.transform.GetChild(1).GetComponent<Image>().sprite;
                    img.SetNativeSize();
                    PanelNhanQua.transform.GetChild(2).GetComponent<Text>().text = oItem.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text;

                    for (int i = 0; i < 3; i++)
                    {
                        allitemconghien.transform.GetChild(i).gameObject.SetActive(false);
                        allTiaSet.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    transform.GetChild(0).transform.Find("txtYeuCau").GetComponent<Text>().text = "";
                    transform.GetChild(0).transform.Find("imgphantram").transform.GetChild(0).GetComponent<Text>().text = "0%";
                    LoadHienTiLePhongAn(1);
                    CrGame.ins.OnThongBaoNhanh("Giải phong ấn thành công!", 2);
                    transform.GetChild(0).Find("btnGiaiPhongAn").GetComponent<Button>().interactable = false;
                    transform.GetChild(0).Find("btnCongHien").GetComponent<Button>().interactable = false;
                    transform.GetChild(0).Find("btnExit").GetComponent<Button>().interactable = true;

                }
            }
            else if (json["status"].Value == "2")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.3f);
                    transform.GetChild(0).transform.Find("MargicCircle").GetComponent<Animator>().Play("Failure");
                    yield return new WaitForSeconds(7f);
                    CrGame.ins.OnThongBaoNhanh("Giải phong ấn thất bại", 2);
                    LoadHienTiLePhongAn(json["lanGiaiPhongAn"].AsInt);
                    transform.GetChild(0).transform.Find("imgphantram").transform.GetChild(0).GetComponent<Text>().text = "0%";
                    for (int i = 0; i < 3; i++)
                    {
                        allitemconghien.transform.GetChild(i).gameObject.SetActive(false);
                        allTiaSet.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    transform.GetChild(0).Find("btnGiaiPhongAn").GetComponent<Button>().interactable = false;
                    transform.GetChild(0).Find("btnCongHien").GetComponent<Button>().interactable = true;
                    transform.GetChild(0).Find("btnExit").GetComponent<Button>().interactable = true;
                    transform.GetChild(0).transform.Find("itemNgocTraiVang").transform.GetChild(1).GetComponent<Text>().text = json["NgocTraiVang"].AsString;
                    transform.GetChild(0).transform.Find("itemManhNgocTrai").transform.GetChild(1).GetComponent<Text>().text = json["ManhNgocTrai"].AsString;
                }
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
        }
    }
    public void ExitMenu()
    {
        ev.DestroyMenu("GiaoDienPhongAn");
    }
}
