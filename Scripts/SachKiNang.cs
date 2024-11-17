using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SachKiNang : MonoBehaviour
{
    public GameObject TextItem,ObjSkill,SkillChon;
    GameObject nameSkill;
    // Start is called before the first frame update
    private void OnEnable()
    {
        LoadSkill();
        LoadItem();
    }

    public void XemNangCap()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemSkill/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi");
                CrGame.ins.panelLoadDao.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if(int.Parse(nameSkill.transform.GetChild(0).GetComponent<Text>().text) < 30)
                {

                }    
                else
                {
                    CrGame.ins.OnThongBaoNhanh("Đã đạt cấp tối đa");
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }

    public void LoadSkill()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "XemSkill";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "chuacoskill")
            {

            }
            else if (json["status"].AsString == "ok")
            {

                for (int i = 0; i < json["allskill"].Count; i++)
                {
                    for (int j = 0; j < ObjSkill.transform.childCount; j++)
                    {
                        if (json["allskill"][i]["nameskill"].Value == ObjSkill.transform.GetChild(j).name)
                        {
                            GameObject objskill = ObjSkill.transform.GetChild(j).transform.GetChild(0).gameObject;
                            GameObject objtext = objskill.transform.GetChild(0).gameObject;
                            objtext.SetActive(true);
                            objtext.GetComponent<Text>().text = json["allskill"][i]["level"].Value;
                            objskill.transform.GetChild(3).gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void XeminfoSkill(bool nangcap)
    {
        
        if(!nangcap) nameSkill = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "XeminfoSkill";
        datasend["data"]["nameSkill"] = nameSkill.name;
        datasend["data"]["nangcap"] = nangcap.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            Image imgSkill = SkillChon.transform.GetChild(0).GetComponent<Image>();


            if (json["status"].AsString == "0")
            {
                SkillChon.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = json["name"].AsString;
                SkillChon.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = json["satthuong"].AsString;
                SkillChon.transform.GetChild(0).GetChild(5).GetComponent<Text>().text = json["nokhi"].AsString;
                SkillChon.transform.GetChild(0).GetChild(6).GetComponent<Text>().text = json["txtinfo"].AsString;
                SkillChon.transform.GetChild(0).GetChild(7).GetComponent<Text>().text = json["txtnangcap"].AsString;
                imgSkill.sprite = nameSkill.transform.GetChild(2).GetComponent<Image>().sprite;
                imgSkill.SetNativeSize();
                SkillChon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = nameSkill.transform.GetChild(0).GetComponent<Text>().text;
                imgSkill.gameObject.SetActive(true);
            }
            else if (json["status"].AsString == "2")
            {
                int duyeucau = 0;
                GameObject menunangcap = transform.GetChild(1).gameObject;
                GameObject textyeucau = menunangcap.transform.GetChild(4).gameObject;
                Button btnNangCap = menunangcap.transform.GetChild(6).GetComponent<Button>();
                menunangcap.transform.GetChild(2).GetComponent<Text>().text = json["infoMoSkill"]["textmo"].Value;
                if (int.Parse(TextItem.transform.GetChild(0).GetComponent<Text>().text) >= int.Parse(json["infoMoSkill"]["yeucau"]["NgocPhep"].Value))
                {
                    textyeucau.transform.GetChild(0).GetComponent<Text>().text = "<color=#008000ff>" + TextItem.transform.GetChild(0).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["NgocPhep"].Value;
                    duyeucau += 1;
                }
                else textyeucau.transform.GetChild(0).GetComponent<Text>().text = "<color=#ff0000ff>" + TextItem.transform.GetChild(0).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["NgocPhep"].Value;

                if (int.Parse(TextItem.transform.GetChild(1).GetComponent<Text>().text) >= int.Parse(json["infoMoSkill"]["yeucau"]["BuiPhep"].Value))
                {
                    textyeucau.transform.GetChild(1).GetComponent<Text>().text = "<color=#008000ff>" + TextItem.transform.GetChild(1).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiPhep"].Value;
                    duyeucau += 1;
                }
                else textyeucau.transform.GetChild(1).GetComponent<Text>().text = "<color=#ff0000ff>" + TextItem.transform.GetChild(1).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiPhep"].Value;

                if (int.Parse(TextItem.transform.GetChild(2).GetComponent<Text>().text) >= int.Parse(json["infoMoSkill"]["yeucau"]["LongChimPhep"].Value))
                {
                    textyeucau.transform.GetChild(2).GetComponent<Text>().text = "<color=#008000ff>" + TextItem.transform.GetChild(2).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["LongChimPhep"].Value;
                    duyeucau += 1;
                }
                else textyeucau.transform.GetChild(2).GetComponent<Text>().text = "<color=#ff0000ff>" + TextItem.transform.GetChild(2).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["LongChimPhep"].Value;

                if (int.Parse(TextItem.transform.GetChild(3).GetComponent<Text>().text) >= int.Parse(json["infoMoSkill"]["yeucau"]["NuocThanh"].Value))
                {
                    textyeucau.transform.GetChild(3).GetComponent<Text>().text = "<color=#008000ff>" + TextItem.transform.GetChild(3).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["NuocThanh"].Value;
                    duyeucau += 1;
                }
                else textyeucau.transform.GetChild(3).GetComponent<Text>().text = "<color=#ff0000ff>" + TextItem.transform.GetChild(3).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["NuocThanh"].Value;

                if (int.Parse(TextItem.transform.GetChild(4).GetComponent<Text>().text) >= int.Parse(json["infoMoSkill"]["yeucau"]["CoTien"].Value))
                {
                    textyeucau.transform.GetChild(4).GetComponent<Text>().text = "<color=#008000ff>" + TextItem.transform.GetChild(4).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["CoTien"].Value;
                    duyeucau += 1;
                }
                else textyeucau.transform.GetChild(4).GetComponent<Text>().text = "<color=#ff0000ff>" + TextItem.transform.GetChild(4).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["CoTien"].Value;

                if (Inventory.ins.ListItemThuong.ContainsKey("itemBuiRong"))
                {
                    if (int.Parse(Inventory.ins.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text) >= int.Parse(json["infoMoSkill"]["yeucau"]["BuiRong"].Value))
                    {
                        textyeucau.transform.GetChild(5).GetComponent<Text>().text = "<color=#008000ff>" + Inventory.ins.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiRong"].Value;
                        duyeucau += 1;
                    }
                    else textyeucau.transform.GetChild(5).GetComponent<Text>().text = "<color=#ff0000ff>" + Inventory.ins.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiRong"].Value;
                }
                else textyeucau.transform.GetChild(5).GetComponent<Text>().text = "<color=#ff0000ff>0</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiRong"].Value;
                if (duyeucau == 6) btnNangCap.interactable = true;
                else btnNangCap.interactable = false;
                imgSkill.gameObject.SetActive(false);
                menunangcap.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
            }
        }
    }    
    public void LoadItem()
    {
        if (Inventory.ins.ListItemThuong.ContainsKey("itemNgocPhep"))
        {
            TextItem.transform.GetChild(0).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemNgocPhep"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(0).GetComponent<Text>().text = "0";
        if(Inventory.ins.ListItemThuong.ContainsKey("itemBuiPhep"))
        {
            TextItem.transform.GetChild(1).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemBuiPhep"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(1).GetComponent<Text>().text = "0";
        if (Inventory.ins.ListItemThuong.ContainsKey("itemLongChimPhep"))
        {
            TextItem.transform.GetChild(2).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemLongChimPhep"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(2).GetComponent<Text>().text = "0";
        if (Inventory.ins.ListItemThuong.ContainsKey("itemNuocThanh"))
        {
            TextItem.transform.GetChild(3).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemNuocThanh"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(3).GetComponent<Text>().text = "0";
        if (Inventory.ins.ListItemThuong.ContainsKey("itemCoTien"))
        {
            TextItem.transform.GetChild(4).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemCoTien"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(4).GetComponent<Text>().text = "0";
    }    
    public void Exit()
    {
        //SkillChon.transform.GetChild(0).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuSachKiNang");
    }
    public void NangCapSkill()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Skill";
        datasend["method"] = "NangCapSkill";
        datasend["data"]["nameSkill"] = nameSkill.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "thanhcong")
            {
                GameObject menunangcap = transform.GetChild(1).gameObject;
                nameSkill.transform.GetChild(0).gameObject.SetActive(true);
                nameSkill.transform.GetChild(3).gameObject.SetActive(false);
                nameSkill.transform.GetChild(0).GetComponent<Text>().text = json["level"].Value;
                SkillChon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = json["level"].Value;
                LoadItem();
                menunangcap.SetActive(false);
                // CrGame.ins.OnThongBaoNhanh("Mở khóa Skill thành công");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
}
