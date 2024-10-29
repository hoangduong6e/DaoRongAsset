using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SachKiNang : MonoBehaviour
{
    public GameObject TextItem,ObjSkill,SkillChon;
    CrGame crgame;Inventory inventory;GameObject nameSkill;
    // Start is called before the first frame update
    void Awake()
    {
        crgame = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CrGame>();
        inventory = crgame.GetComponent<Inventory>();
    }
    private void OnEnable()
    {
        LoadSkill();
        LoadItem();
    }

    public void XemNangCap()
    {
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XemSkill/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
                crgame.panelLoadDao.SetActive(false);
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
                    crgame.OnThongBaoNhanh("Đã đạt cấp tối đa");
                }
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }

    public void LoadSkill()
    {
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XemSkill/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
                crgame.panelLoadDao.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                // Show results as text
                if(www.downloadHandler.text == "chuacoskill")
                {

                }   
                else
                {
                    debug.Log(www.downloadHandler.text);
                    JSONNode json = JSON.Parse(www.downloadHandler.text);
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
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
    public void XeminfoSkill(bool nangcap)
    {
        crgame.panelLoadDao.SetActive(true);
        if(!nangcap) nameSkill = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XeminfoSkill/nameSkill/" + nameSkill.name + "/taikhoan/" + LoginFacebook.ins.id+"/nangcap/"+ nangcap.ToString());
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
                crgame.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                //  debug.Log(json["infoMoSkill"]);
                Image imgSkill = SkillChon.transform.GetChild(0).GetComponent<Image>();
                if (json == "" || json == null)
                {
                    crgame.OnThongBaoNhanh(www.downloadHandler.text);
                    crgame.panelLoadDao.SetActive(false);
                }    
              
                else if (json["infoMoSkill"] == "" || json["infoMoSkill"] == null)
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
                else
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

                    if (inventory.ListItemThuong.ContainsKey("itemBuiRong"))
                    {
                        if (int.Parse(inventory.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text) >= int.Parse(json["infoMoSkill"]["yeucau"]["BuiRong"].Value))
                        {
                            textyeucau.transform.GetChild(5).GetComponent<Text>().text = "<color=#008000ff>" + inventory.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiRong"].Value;
                            duyeucau += 1;
                        }
                        else textyeucau.transform.GetChild(5).GetComponent<Text>().text = "<color=#ff0000ff>" + inventory.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text + "</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiRong"].Value;
                    }
                    else textyeucau.transform.GetChild(5).GetComponent<Text>().text = "<color=#ff0000ff>0</color>" + "/" + json["infoMoSkill"]["yeucau"]["BuiRong"].Value;
                    if (duyeucau == 6) btnNangCap.interactable = true;
                    else btnNangCap.interactable = false;
                    imgSkill.gameObject.SetActive(false);
                    menunangcap.SetActive(true);
                }
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }    
    public void LoadItem()
    {
        if (inventory.ListItemThuong.ContainsKey("itemNgocPhep"))
        {
            TextItem.transform.GetChild(0).GetComponent<Text>().text = inventory.ListItemThuong["itemNgocPhep"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(0).GetComponent<Text>().text = "0";
        if(inventory.ListItemThuong.ContainsKey("itemBuiPhep"))
        {
            TextItem.transform.GetChild(1).GetComponent<Text>().text = inventory.ListItemThuong["itemBuiPhep"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(1).GetComponent<Text>().text = "0";
        if (inventory.ListItemThuong.ContainsKey("itemLongChimPhep"))
        {
            TextItem.transform.GetChild(2).GetComponent<Text>().text = inventory.ListItemThuong["itemLongChimPhep"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(2).GetComponent<Text>().text = "0";
        if (inventory.ListItemThuong.ContainsKey("itemNuocThanh"))
        {
            TextItem.transform.GetChild(3).GetComponent<Text>().text = inventory.ListItemThuong["itemNuocThanh"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else TextItem.transform.GetChild(3).GetComponent<Text>().text = "0";
        if (inventory.ListItemThuong.ContainsKey("itemCoTien"))
        {
            TextItem.transform.GetChild(4).GetComponent<Text>().text = inventory.ListItemThuong["itemCoTien"].transform.GetChild(0).GetComponent<Text>().text;
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
        crgame.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "NangCapSkill/nameSkill/" + nameSkill.name + "/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                crgame.OnThongBaoNhanh("Lỗi");
                crgame.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
               JSONNode json = JSON.Parse(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                if(json["status"].Value == "thanhcong")
                {
                    GameObject menunangcap = transform.GetChild(1).gameObject;
                    nameSkill.transform.GetChild(0).gameObject.SetActive(true);
                    nameSkill.transform.GetChild(3).gameObject.SetActive(false);
                    nameSkill.transform.GetChild(0).GetComponent<Text>().text = json["level"].Value;
                    SkillChon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = json["level"].Value; 
                    LoadItem();
                    menunangcap.SetActive(false);
                   // crgame.OnThongBaoNhanh("Mở khóa Skill thành công");
                }
                else
                {
                    crgame.OnThongBaoNhanh(json["status"].Value);
                }
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
}
