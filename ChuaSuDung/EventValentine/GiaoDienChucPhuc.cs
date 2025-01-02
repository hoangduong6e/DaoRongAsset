using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiaoDienChucPhuc : MonoBehaviour
{
    public GameObject oItem, ContentDanhSachKhoBau, ObjKhoBau;
    public void ParseData(JSONNode json)
    {
      //  debug.Log(json.ToString());
        gameObject.SetActive(true);
        Transform g = transform.GetChild(0);
        for (int i = 0; i < json["GiaoDienChucPhuc"]["DanhSachKhoBau"].Count; i++)
        {
            GameObject newObjKhoBau = Instantiate(ObjKhoBau, transform.position, Quaternion.identity) as GameObject;
            newObjKhoBau.transform.SetParent(ContentDanhSachKhoBau.transform, false);
            //   newObjKhoBau.transform.position = ContentDanhSachKhoBau.transform.position;
            byte soluongdadoi = (byte)json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["soluongdadoi"].AsInt;
            byte maxsoluongdoi = (byte)json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["maxsoluongdoi"].AsInt;

            string soluonghienthi = "";
            if (soluongdadoi >= maxsoluongdoi)
            {
                soluonghienthi = "<color=red>" + soluongdadoi + "/" + maxsoluongdoi + "</color>";
            }
            else soluonghienthi = "<color=lime>" + soluongdadoi + "/" + maxsoluongdoi + "</color>";
            Text txtnameitem = newObjKhoBau.transform.Find("txtnameitem").GetComponent<Text>();
            Text txtsoluongdadoi = newObjKhoBau.transform.Find("ttxyeucau").GetComponent<Text>();
            Text txtsoitem = newObjKhoBau.transform.Find("txtsoitem").GetComponent<Text>();
            txtnameitem.text = json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["nametv"].AsString;
            txtsoluongdadoi.text = soluonghienthi;
            Image imgItem = newObjKhoBau.transform.Find("imgitem").GetComponent<Image>();
            if (json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["itemgi"].AsString == "item")
            {
                imgItem.sprite = Inventory.LoadSprite(json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["nameitem"].AsString);
                txtsoitem.text = json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["soluong"].AsString;
            }
            else if (json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["itemgi"].AsString == "rong")
            {
                Vector3 scale = imgItem.transform.localScale;
                scale.x /= 2;
                scale.y /= 2;
                imgItem.transform.localScale = scale;
                imgItem.sprite = Inventory.LoadSpriteRong(json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["nameitem"].AsString + "1");
                txtsoitem.text = json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["sao"].AsString + " sao";
            }
            else if (json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["itemgi"].AsString == "itemevent")
            {
                imgItem.sprite = EventManager.ins.GetSprite(json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["nameitem"].AsString);
                txtsoitem.text = json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["soluong"].AsString;
            }
            imgItem.SetNativeSize();
            newObjKhoBau.gameObject.SetActive(true);
            newObjKhoBau.name = json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["namekey"].AsString;
            imgItem.name = json["yeucauChucPhuc"][json["GiaoDienChucPhuc"]["DanhSachKhoBau"][i]["namekey"].AsString].AsString;
        }
        if (json["GiaoDienChucPhuc"]["itemkhoa"].AsString != "")
        {
            GameObject objdskhobau = ContentDanhSachKhoBau.transform.Find(json["GiaoDienChucPhuc"]["itemkhoa"].AsString).gameObject;
            oItem.transform.GetChild(0).gameObject.SetActive(false);
            oItem.transform.GetChild(1).gameObject.SetActive(true);
            GameObject obj = oItem.transform.GetChild(1).gameObject;
            Image img = obj.GetComponent<Image>();
            img.sprite = objdskhobau.transform.GetChild(1).GetComponent<Image>().sprite;
            img.transform.GetChild(0).GetComponent<Text>().text = objdskhobau.transform.Find("txtsoitem").GetComponent<Text>().text;
            img.SetNativeSize();
            g.Find("TienDoChucPhuc").gameObject.SetActive(true);
            g.Find("btnquayX1").GetComponent<Button>().interactable = true;
            g.Find("btnquayX5").GetComponent<Button>().interactable = true;
            Text txtYeuCau = g.transform.Find("txtYeuCau").GetComponent<Text>();txtYeuCau.gameObject.SetActive(true);
            txtYeuCau.text = json["GiaoDienChucPhuc"]["dachucphuc"].AsString + "/" + objdskhobau.transform.GetChild(1).gameObject.name;
            SetThanhProcessChucPhuc(json["GiaoDienChucPhuc"]["dachucphuc"].AsFloat, float.Parse(objdskhobau.transform.GetChild(1).gameObject.name));
            oItem.GetComponent<Image>().enabled = false;
        }
        SetTraiTimPhale(json["traitimphale"].AsString);
    }
    public void ClickDauCong()
    {
        AudioManager.SoundClick();
        transform.Find("DanhSachItem").gameObject.SetActive(true);
    }
    private void SetTraiTimPhale(string s)
    {
        transform.Find("itemTraitimphale").transform.GetChild(1).GetComponent<Text>().text = s;
    }
    public void ChonKhoBau()
    {
        AudioManager.SoundClick();
        GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject DanhSachItem = transform.Find("DanhSachItem").gameObject;
        GameObject oitem = DanhSachItem.transform.Find("Oitem").gameObject;

        Image img = oitem.transform.GetChild(0).GetComponent<Image>();
        img.gameObject.SetActive(true);
        img.sprite = objchon.GetComponent<Image>().sprite;
        img.SetNativeSize();
        img.transform.GetChild(0).GetComponent<Text>().text = "Điểm chúc phúc: <color=orange>" + objchon.name + "</color>";
        DanhSachItem.transform.Find("btnKhoa").GetComponent<Button>().interactable = true;
        img.gameObject.name = objchon.transform.parent.name;
        img.transform.GetChild(0).gameObject.name = objchon.transform.parent.Find("txtsoitem").GetComponent<Text>().text;
    }

    public void ExitDanhSachItem()
    {
        AudioManager.SoundClick();
        GameObject DanhSachItem = transform.Find("DanhSachItem").gameObject;
        GameObject oitem = DanhSachItem.transform.Find("Oitem").gameObject;
        oitem.transform.GetChild(0).gameObject.SetActive(false);
        DanhSachItem.SetActive(false);
        DanhSachItem.transform.Find("btnKhoa").GetComponent<Button>().interactable = false;
    }

    public void KhoaKhoBau()
    {
        AudioManager.SoundClick();
        EventManager.OpenThongBaoChon("Sau khi Khóa sẽ <color=red>không thể chọn vật phẩm khác</color> cho đến khi Chúc Phúc xong", delegate { Okk(); });

        void Okk()
        {
            AudioManager.SoundClick();
            GameObject DanhSachItem = transform.Find("DanhSachItem").gameObject;
            GameObject oitemdanhsach = DanhSachItem.transform.Find("Oitem").gameObject;
            // GameObject objchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            JSONClass datasend = new JSONClass();
            datasend["class"] = EventManager.ins.nameEvent;
            datasend["method"] = "KhoaItemPhongAn";
            datasend["data"]["nameitem"] = oitemdanhsach.transform.GetChild(0).name;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].Value == "0")
                {
                    CrGame.ins.OnThongBaoNhanh("Đã khóa!", 2);
                    oItem.transform.GetChild(0).gameObject.SetActive(false);
                    oItem.GetComponent<Image>().enabled = false;
                    GameObject obj = oItem.transform.GetChild(1).gameObject;
                    obj.gameObject.SetActive(true);
                    Image img = obj.GetComponent<Image>();
                    img.sprite = oitemdanhsach.transform.GetChild(0).GetComponent<Image>().sprite;
                    obj.transform.GetChild(0).GetComponent<Text>().text = oitemdanhsach.transform.GetChild(0).transform.GetChild(0).gameObject.name;
                    img.SetNativeSize();
                    DanhSachItem.SetActive(false);
                    //  transform.GetChild(0).Find("btnGiaiPhongAn").GetComponent<Button>().interactable = true;
                    transform.GetChild(0).Find("btnquayX1").GetComponent<Button>().interactable = true;
                    transform.GetChild(0).Find("btnquayX5").GetComponent<Button>().interactable = true;
                    transform.GetChild(0).Find("TienDoChucPhuc").gameObject.SetActive(true);
                    Text txtYeuCau = transform.GetChild(0).transform.Find("txtYeuCau").GetComponent<Text>(); txtYeuCau.gameObject.SetActive(true);
                    txtYeuCau.GetComponent<Text>().text = "0/" + ContentDanhSachKhoBau.transform.Find(json["itemkhoa"].AsString).transform.GetChild(1).gameObject.name;
                    SetThanhProcessChucPhuc(0, 100);
                    //   transform.GetChild(0).transform.Find("imgphantram").transform.GetChild(0).GetComponent<Text>().text = json["tylePhongAn"].AsString + "%";
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
                }
            }
        }
    }
    private bool duoctat = true;
    public void Quay(string x)
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "ChucPhuc";
        datasend["data"]["solanquay"] = x;
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject animChucPhuc = transform.Find("animChucPhuc").gameObject;
                if (!animChucPhuc.activeSelf) EventManager.ins.StartDelay(() => { animChucPhuc.SetActive(false); }, 1.2f);
                animChucPhuc.SetActive(true);
                transform.GetChild(0).transform.Find("txtYeuCau").GetComponent<Text>().text = json["dachucphuc"].AsString + "/" + json["maxchucphuc"].AsString;
                SetThanhProcessChucPhuc(json["dachucphuc"].AsFloat, json["maxchucphuc"].AsFloat);
               
                SetTraiTimPhale(json["traitimphale"].AsString);
                if (json["thanhcong"].AsBool)
                {
                    duoctat = false;
                    GameObject obj = oItem.transform.GetChild(1).gameObject;
                    obj.gameObject.SetActive(false);
                    Image img = obj.GetComponent<Image>();
                    oItem.transform.GetChild(0).gameObject.SetActive(true);
                    GameObject PanelNhanQua = transform.Find("PanelNhanQua").gameObject;
                    Image imgqua = PanelNhanQua.transform.GetChild(1).GetComponent<Image>();
                    imgqua.sprite = img.sprite;
                    imgqua.SetNativeSize();
                    PanelNhanQua.transform.GetChild(2).GetComponent<Text>().text = img.transform.GetChild(0).GetComponent<Text>().text;
                    PanelNhanQua.SetActive(true);
                    EventManager.ins.StartDelay(() => { duoctat = true; },1);
                    EventManager.ins.btnHopQua.GetComponent<HopQua>().Them1Qua();
                    GameObject g = transform.GetChild(0).gameObject;
                    g.transform.Find("txtYeuCau").gameObject.SetActive(false);
                 

                    g.transform.Find("TienDoChucPhuc").gameObject.SetActive(false);
                    g.transform.Find("btnquayX1").GetComponent<Button>().interactable = false;
                    g.transform.Find("btnquayX5").GetComponent<Button>().interactable = false;
                    oItem.GetComponent<Image>().enabled = true;
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }

        }
    }
    public void ClosePanelNhanQua()
    {
        if (duoctat) transform.Find("PanelNhanQua").gameObject.SetActive(false);
    }    
    private void SetThanhProcessChucPhuc(float dalam, float max)
    {
        Material material = transform.GetChild(0).Find("TienDoChucPhuc").transform.GetChild(1).GetComponent<Image>().material;
        float chia = dalam / max * 100;
        material.SetFloat("_TienDo", chia);
    }
    public void ExitMenu()
    {
        EventManager.ins.DestroyMenu("GiaoDienChucPhuc");
    }
}
