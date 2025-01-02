using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuDoiHinh : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SlotDoiHinh, SlotItem, ContentDoiHinh, ContentItem;
    Image RongChon;public GameObject odotrong, oxanhtrong,menuDH,menuskill,ObjectSkill,ObjectSkillChon;
    public Sprite spriteGdDoiHinh, spriteGdSkill;
    public void LoadSkill()
    {
      
    }
    public void XemChiSoRong()
    {
        GameObject objitem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string idrong = objitem.transform.parent.transform.GetChild(0).name.Split('-')[1];
        CrGame.ins.ChiSoRong(idrong);
    }
    public void LoadDoihinh(string id,Sprite sprite,string sao,int i)
    {
        GameObject itemdoihinh = Instantiate(SlotDoiHinh, transform.position,Quaternion.identity) as GameObject;
      //  itemdoihinh.transform.SetParent(ContentDoiHinh.transform.GetChild(i));
        itemdoihinh.transform.SetParent(ContentDoiHinh.transform.GetChild(i).transform,false);
        itemdoihinh.transform.position = ContentDoiHinh.transform.GetChild(i).transform.position;
        Image imgrong = itemdoihinh.transform.GetChild(0).GetComponent<Image>();
        imgrong.sprite = sprite;
        imgrong.SetNativeSize();
        itemdoihinh.transform.GetChild(1).GetComponent<Text>().text = sao;
        itemdoihinh.transform.GetChild(0).gameObject.name = "doihinh-" + id;
        itemdoihinh.SetActive(true);
    }
    public void LoadRong(string id,Sprite sprite, string sao)
    {
        GameObject itemdoihinh = Instantiate(SlotItem, transform.position, Quaternion.identity) as GameObject;
        itemdoihinh.transform.SetParent(ContentItem.transform,false);
        Image imgrong = itemdoihinh.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        imgrong.sprite = sprite;
        imgrong.SetNativeSize();
        itemdoihinh.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = sao;
        itemdoihinh.transform.SetSiblingIndex(0);
        itemdoihinh.transform.GetChild(0).transform.GetChild(0).gameObject.name = "doihinh-" + id;
        itemdoihinh.SetActive(true);
    }
    private void OnEnable()
    {
        if (menuDH.activeSelf) NetworkManager.ins.socket.Emit("getDoiHinh");
        else if (menuskill.activeSelf) OpenSkill();
    }
    public void CloseMenu()
    {
        //Xoahet();
        //gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuDoiHinh");
    }
    public void Xoahet()
    {
        for (int i = 0; i < ContentItem.transform.childCount; i++)
        {
            Destroy(ContentItem.transform.GetChild(i).gameObject);
        }
        //for (int i = 0; i < ContentItem.transform.childCount; i++)
        //{
        //    if (ContentItem.transform.GetChild(i).childCount > 0)
        //    {
        //        Destroy(ContentItem.transform.GetChild(i).transform.GetChild(0).gameObject);
        //    }
        //}
        //for (int i = 1; i < ContentDoiHinh.transform.childCount; i++)
        //{
        //    if (ContentDoiHinh.transform.GetChild(i).childCount > 0)
        //    {
        //        Destroy(ContentDoiHinh.transform.GetChild(i).transform.GetChild(0).gameObject);
        //    }
        //}]
        for (int i = 0; i < ContentDoiHinh.transform.childCount; i++)
        {
            Destroy(ContentDoiHinh.transform.GetChild(i).gameObject);
        }
    }
    public void TaoSlotTrong(int slot)
    {
        for (int i = 0; i < slot; i++)
        {
            GameObject otrong = Instantiate(odotrong, ContentDoiHinh.transform.position, Quaternion.identity) as GameObject;
            otrong.transform.SetParent(ContentDoiHinh.transform.transform,false);
            otrong.name = i.ToString();
            otrong.SetActive(true);
        }
        for (int i = 0; i < slot; i++)
        {
            GameObject otrong = Instantiate(oxanhtrong, ContentItem.transform.position, Quaternion.identity) as GameObject;
            otrong.transform.SetParent(ContentItem.transform.transform,false);
            otrong.SetActive(true);
        }
    }
    public void SaveDoiHinh()
    {
        NetworkManager.ins.socket.Emit("xepdoihinh", JSONObject.CreateStringObject("xacnhan"));
    }    
    public void MacDinh()
    {
        NetworkManager.ins.socket.Emit("xepdoihinh", JSONObject.CreateStringObject("macdinh"));
    }
    public void XoaAll()
    {
        NetworkManager.ins.socket.Emit("xepdoihinh", JSONObject.CreateStringObject("xoahet"));
    }
    public void Chon(string s)
    {
        GameObject objitem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        RongChon = objitem.GetComponent<Image>();
        string[] id = objitem.name.Split('-');
        if(s == "them")
        {
            for (int i = 0; i < ContentDoiHinh.transform.childCount; i++)
            {
                if (ContentDoiHinh.transform.GetChild(i).transform.childCount == 0)
                {
                    NetworkManager.ins.socket.Emit("xepdoihinh", JSONObject.CreateStringObject(s + "-" + id[1] + "-" + i));
                    break;
                }
            }
        }
        else
        {
            int i = objitem.transform.parent.transform.parent.gameObject.transform.GetSiblingIndex();
            NetworkManager.ins.socket.Emit("xepdoihinh", JSONObject.CreateStringObject(s + "-" + id[1] + "-" + i));
        }
    }
    public void AddDoiHinh()
    {
        Image img = RongChon;//UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        for (int i = 0; i < ContentDoiHinh.transform.childCount; i++)
        {
            if(ContentDoiHinh.transform.GetChild(i).transform.childCount == 0)
            {
                GameObject itemdoihinh = Instantiate(SlotDoiHinh,transform.position, Quaternion.identity) as GameObject;
                itemdoihinh.transform.SetParent(ContentDoiHinh.transform.GetChild(i),false);
                Image imgrong = itemdoihinh.transform.GetChild(0).GetComponent<Image>();
                imgrong.sprite = img.sprite;
                imgrong.SetNativeSize();
                itemdoihinh.transform.GetChild(1).GetComponent<Text>().text = img.transform.parent.gameObject.transform.GetChild(1).GetComponent<Text>().text;
                itemdoihinh.transform.position = ContentDoiHinh.transform.GetChild(i).transform.position;
                itemdoihinh.transform.GetChild(0).gameObject.name = RongChon.name;
                //img.transform.parent.gameObject.transform.SetParent(ContentDoiHinh.transform.GetChild(i), false);
                itemdoihinh.SetActive(true);
                Destroy(img.transform.parent.gameObject);
                break;
            }
        }
    }    
    public void Xoa1Rong()
    {
        Image img = RongChon;//UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        for (int i = 0; i < ContentItem.transform.childCount; i++)
        {
            if (ContentItem.transform.GetChild(i).transform.childCount == 0)
            {
                GameObject itemdoihinh = Instantiate(SlotItem.transform.GetChild(0).gameObject, ContentItem.transform.GetChild(i).transform.position, Quaternion.identity) as GameObject;
                itemdoihinh.transform.SetParent(ContentItem.transform.GetChild(i), false);
                Image imgrong = itemdoihinh.transform.GetChild(0).GetComponent<Image>();
                imgrong.sprite = img.sprite;
                imgrong.SetNativeSize();
                itemdoihinh.transform.GetChild(0).gameObject.name = RongChon.name;
                itemdoihinh.transform.position = ContentItem.transform.GetChild(i).transform.position;
                itemdoihinh.transform.GetChild(1).GetComponent<Text>().text = img.transform.parent.gameObject.transform.GetChild(1).GetComponent<Text>().text;
                //img.transform.parent.gameObject.transform.SetParent(ContentDoiHinh.transform.GetChild(i), false);
                itemdoihinh.SetActive(true);
                Destroy(img.transform.parent.gameObject);
                break;
            }
        }
    }
    public void OpenDoiHinh()
    {
        if (menuDH.activeSelf ==false) NetworkManager.ins.socket.Emit("getDoiHinh");
        transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = spriteGdDoiHinh;
        menuDH.SetActive(true);
        menuskill.SetActive(false);
    }
    public void OpenSkill()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "XemSkill";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "chuacoskill")
            {

            }
            else if (json["status"].AsString == "ok")
            {
               
                for (int i = 0; i < json["allskill"].Count; i++)
                {
                    for (int j = 0; j < ObjectSkill.transform.childCount; j++)
                    {
                        if (json["allskill"][i]["nameskill"].Value == ObjectSkill.transform.GetChild(j).name)
                        {
                            GameObject objtext = ObjectSkill.transform.GetChild(j).transform.GetChild(2).gameObject;
                            objtext.SetActive(true);
                            objtext.GetComponent<Text>().text = json["allskill"][i]["level"].Value;
                            ObjectSkill.transform.GetChild(j).transform.GetChild(1).gameObject.SetActive(false);
                            break;
                        }
                    }
                }
                LoadSkillChon(json["skillchon"]["skill1"].Value, json["skillchon"]["skill2"].Value, json["skillchon"]["skill3"].Value);
            }
            else
            {
                gameObject.SetActive(false);
            }
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            CrGame.ins.panelLoadDao.SetActive(false);
            transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = spriteGdSkill;
            menuskill.SetActive(true);
            menuDH.SetActive(false);
        }
    }
    public void ChonSkill()
    {
        GameObject Skillchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "AddSkill";
        datasend["data"]["nameSkill"] = Skillchon.transform.parent.name;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                LoadSkillChon(json["skill"]["skill1"].Value, json["skill"]["skill2"].Value, json["skill"]["skill3"].Value);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void XoaSkill()
    {
        GameObject Skillchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "XoaSkill";
        datasend["data"]["oskill"] = (Skillchon.transform.parent.parent.transform.GetSiblingIndex() + 1).ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                ObjectSkillChon.transform.GetChild(Skillchon.transform.parent.parent.transform.GetSiblingIndex()).transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                gameObject.SetActive(false);
            }
        }
    }
    public void XoaHetskill()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "XacNhanSkill";
        datasend["data"]["Key"] = "XoaHet";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                ObjectSkillChon.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                ObjectSkillChon.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
                ObjectSkillChon.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Đã xóa hết kỹ năng");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void MacDinhskill()
    {

    }
    public void XacNhanskill()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "XacNhanSkill";
        datasend["data"]["Key"] = "XacNhan";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }
    public void MoKhoaOKyNang()
    {
        GameObject trencung = GameObject.FindGameObjectWithTag("trencung");
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", trencung,true, trencung.transform.childCount - 1).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Mở khóa kĩ năng với 500.000 Bụi Rồng?";
        tbc.btnChon.onClick.AddListener(LoadMoKhoa);
    }
    void LoadMoKhoa()
    {
        GameObject Skillchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "MoKhoaKyNang";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                ObjectSkillChon.transform.GetChild(1).GetComponent<Image>().enabled = false;
                CrGame.ins.OnThongBaoNhanh("Mở khóa ô kỹ năng thành công");

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    void LoadSkillChon(string name1,string name2,string name3)
    {
        ObjectSkillChon.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        ObjectSkillChon.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
        ObjectSkillChon.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
        if (name1 != "")
        {
            for (int j = 0; j < ObjectSkill.transform.childCount; j++)
            {
                if (name1 == ObjectSkill.transform.GetChild(j).name)
                {
                    ObjectSkillChon.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = ObjectSkill.transform.GetChild(j).transform.GetChild(0).GetComponent<Image>().sprite;
                    ObjectSkillChon.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = ObjectSkill.transform.GetChild(j).transform.GetChild(2).GetComponent<Text>().text;
                    ObjectSkillChon.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    break;
                }
            }
        }
        if (name2 == "") ObjectSkillChon.transform.GetChild(1).GetComponent<Image>().enabled = false;
        if (name2 != "" && name2 != "Khoa")
        {
            for (int j = 0; j < ObjectSkill.transform.childCount; j++)
            {
                if (name2 == ObjectSkill.transform.GetChild(j).name)
                {
                    ObjectSkillChon.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = ObjectSkill.transform.GetChild(j).transform.GetChild(0).GetComponent<Image>().sprite;
                    ObjectSkillChon.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = ObjectSkill.transform.GetChild(j).transform.GetChild(2).GetComponent<Text>().text;
                    ObjectSkillChon.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                    ObjectSkillChon.transform.GetChild(1).GetComponent<Image>().enabled = false;

                    break;
                }
            }
        }
        if(name3 == "") ObjectSkillChon.transform.GetChild(2).GetComponent<Image>().enabled = false;
        if (name3 != "" && name3 != "Khoa")
        {
            for (int j = 0; j < ObjectSkill.transform.childCount; j++)
            {
                if (name3 == ObjectSkill.transform.GetChild(j).name)
                {
                    ObjectSkillChon.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = ObjectSkill.transform.GetChild(j).transform.GetChild(0).GetComponent<Image>().sprite;
                    ObjectSkillChon.transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = ObjectSkill.transform.GetChild(j).transform.GetChild(2).GetComponent<Text>().text;
                    ObjectSkillChon.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                    ObjectSkillChon.transform.GetChild(2).GetComponent<Image>().enabled = false;
                    break;
                }
            }
        }
    }
}
