using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuHomThu : MonoBehaviour
{
    // Start is called before the first frame update
    public ScrollRect ScrollRectThu;
    byte thuchon;
    public Sprite spriteCoThu, spriteDaDoc, spriteCoQua;
    private void OnEnable()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "HomThu";
        datasend["method"] = "XemHomThu";
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                GameObject objThu = transform.GetChild(2).gameObject;
                for (int i = 0; i < json["hethong"].Count; i++)
                {
                    GameObject thu = Instantiate(objThu, transform.position, Quaternion.identity);
                    thu.transform.SetParent(ScrollRectThu.transform.GetChild(0).transform.GetChild(0).transform, false);
                    thu.transform.GetChild(0).GetComponent<Text>().text = json["hethong"][i]["namethu"].Value;
                    thu.transform.GetChild(1).GetComponent<Text>().text = json["hethong"][i]["timehethan"].Value;
                    if (json["hethong"][i]["dadoc"].Value == "DaDoc")
                    {
                        thu.GetComponent<Image>().sprite = spriteDaDoc;
                    }
                    if (json["hethong"][i]["qua"].Count > 0)
                    {
                        thu.GetComponent<Image>().sprite = spriteCoQua;
                    }
                    thu.SetActive(true);
                }
                for (int i = 0; i < json["banbe"].Count; i++)
                {
                    GameObject thu = Instantiate(objThu, transform.position, Quaternion.identity);
                    thu.transform.SetParent(ScrollRectThu.transform.GetChild(0).transform.GetChild(1).transform, false);
                    thu.transform.GetChild(0).GetComponent<Text>().text = json["banbe"][i]["namethu"].Value;
                    thu.transform.GetChild(1).GetComponent<Text>().text = json["banbe"][i]["timehethan"].Value;
                    if (json["banbe"][i]["dadoc"].Value == "DaDoc")
                    {
                        thu.GetComponent<Image>().sprite = spriteDaDoc;
                    }
                    if (json["banbe"][i]["qua"].Count > 0)
                    {
                        thu.GetComponent<Image>().sprite = spriteCoQua;

                    }
                    thu.SetActive(true);
                }
                GameObject.FindGameObjectWithTag("btnhomthu").transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }
    public void XemThu()
    {
        int vitrithuchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        thuchon = (byte)vitrithuchon;
      //  CrGame.ins.panelLoadDao.SetActive(true);
        string thu = "hethong";
        if (ScrollRectThu.transform.GetChild(0).transform.GetChild(1).gameObject.activeSelf) thu = "banbe";

        JSONClass datasend = new JSONClass();
        datasend["class"] = "HomThu";
        datasend["method"] = "XemThu";
        datasend["data"]["thu"] = thu;
        datasend["data"]["vitri"] = vitrithuchon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                GameObject showthu = transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;
                showthu.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = json["nguoigui"].Value;
                showthu.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = json["timegui"].Value;
                debug.Log("timegui" + json["timegui"].Value);
                if (json["nguoigui"].Value == "Admin") showthu.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
                else showthu.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                showthu.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = json["namethu"].Value;
                showthu.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = json["noidung"].Value;
                if (json["qua"].Count > 0)
                {
                    for (int i = 1; i < showthu.transform.GetChild(3).childCount; i++)
                    {
                        Destroy(showthu.transform.GetChild(3).GetChild(i).gameObject);
                    }
                    for (int i = 0; i < json["qua"].Count; i++)
                    {
                        GameObject qua = Instantiate(showthu.transform.GetChild(3).GetChild(0).gameObject, transform.position, Quaternion.identity);
                        if (json["qua"][i]["name"].Value != "")
                        {
                            qua.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(json["qua"][i]["name"].Value);
                            qua.transform.GetChild(1).GetComponent<Text>().text = json["qua"][i]["soluong"].Value;
                        }
                        else
                        {
                            qua.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["qua"][i]["namerong"].Value + json["qua"][i]["tienhoa"].Value);
                            qua.transform.GetChild(1).GetComponent<Text>().text = json["qua"][i]["sao"].Value + "sao";
                        }
                        qua.transform.SetParent(showthu.transform.GetChild(3), false);
                        qua.SetActive(true);
                    }
                    showthu.transform.GetChild(3).gameObject.SetActive(true);
                    showthu.transform.GetChild(4).gameObject.SetActive(true);
                }
                else
                {
                    showthu.transform.GetChild(3).gameObject.SetActive(false);
                    showthu.transform.GetChild(4).gameObject.SetActive(false);
                }

                if (thu == "hethong")
                {
                    ScrollRectThu.transform.GetChild(0).transform.GetChild(0).GetChild(vitrithuchon).GetComponent<Image>().sprite = spriteDaDoc;
                }
                else ScrollRectThu.transform.GetChild(0).transform.GetChild(1).GetChild(vitrithuchon).GetComponent<Image>().sprite = spriteDaDoc;
                showthu.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }
    public void GuiThu()
    {
        Button btngui = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btngui.interactable = false;
        //CrGame.ins.panelLoadDao.SetActive(true);
        string txtguitoi = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<InputField>().text ,txtchude = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<InputField>().text;
        string txtnoidung = transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<InputField>().text;
        txtguitoi = txtguitoi.Trim();// cat het khoang trang o dau va cuoi chuoi
        txtchude = txtchude.Trim();txtnoidung = txtnoidung.Trim();
        debug.Log(txtguitoi + " + " + txtguitoi.Length);
        if(checktext(txtguitoi) && checktext(txtchude) && checktext(txtnoidung))
        {

            JSONClass datasend = new JSONClass();
            datasend["class"] = "HomThu";
            datasend["method"] = "GuiThu";
            datasend["data"]["guitoi"] = txtguitoi;
            datasend["data"]["chude"] = txtchude;
            datasend["data"]["noidung"] = txtnoidung;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode jsonn)
            {
                if (jsonn["status"].AsString == "0")
                {
                    transform.GetChild(1).gameObject.SetActive(false);
                    CrGame.ins.OnThongBaoNhanh("Đã gửi!");
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
                }
                btngui.interactable = true;
            }
        }    
        else
        {
            CrGame.ins.OnThongBaoNhanh("Cần điền đủ thông tin!");
            btngui.interactable = true;
            CrGame.ins.panelLoadDao.SetActive(false);
        }
    }
    bool checktext(string s)
    {
        if (s.Length < 5) return false;
        //int i = 0;
        //while(i < s.Length)
        //{
        //    if ((s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z'))
        //    {
        //        return true;
        //    }
        //    i++;
        //}
        return true;
    }    
    public void XemThuHeThong()
    {
        transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color32(255,255,255,255);
        transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = new Color32(125,125,125,255);
        GameObject content = ScrollRectThu.transform.GetChild(0).transform.GetChild(0).gameObject;
        content.SetActive(true);
        ScrollRectThu.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        ScrollRectThu.content = content.GetComponent<RectTransform>();
    }   
    public void XemThuBanBe()
    {
        transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        GameObject content = ScrollRectThu.transform.GetChild(0).transform.GetChild(1).gameObject;
        ScrollRectThu.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        content.SetActive(true);
        ScrollRectThu.content = content.GetComponent<RectTransform>();
    }
    public void OpenGuiThu()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<InputField>().text = "";
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<InputField>().text = "";
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<InputField>().text = "";
    }   
    public void HuyThu()
    {
       // CrGame.ins.panelLoadDao.SetActive(true);
        string thu = "hethong";
        if (ScrollRectThu.transform.GetChild(0).transform.GetChild(1).gameObject.activeSelf) thu = "banbe";

        JSONClass datasend = new JSONClass();
        datasend["class"] = "HomThu";
        datasend["method"] = "HuyThu";
        datasend["data"]["thu"] = thu;
        datasend["data"]["vitri"] = thuchon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {

                if (thu == "hethong")
                {
                    Destroy(ScrollRectThu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(thuchon).gameObject);
                }
                else Destroy(ScrollRectThu.transform.GetChild(0).transform.GetChild(1).transform.GetChild(thuchon).gameObject);
                transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }
    public void NhanQuaThu()
    {
        //CrGame.ins.panelLoadDao.SetActive(true);
        string thu = "hethong";
        if (ScrollRectThu.transform.GetChild(0).transform.GetChild(1).gameObject.activeSelf) thu = "banbe";

        JSONClass datasend = new JSONClass();
        datasend["class"] = "HomThu";
        datasend["method"] = "NhanQuaThu";
        datasend["data"]["thu"] = thu;
        datasend["data"]["vitri"] = thuchon.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {

                if (thu == "hethong")
                {
                    Destroy(ScrollRectThu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(thuchon).gameObject);
                }
                else Destroy(ScrollRectThu.transform.GetChild(0).transform.GetChild(1).transform.GetChild(thuchon).gameObject);
                transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Đã nhận!");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }
    public void HuyTatCaThu()
    {
        if(transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject.activeSelf)
        {

            string thu = "hethong";
            if (ScrollRectThu.transform.GetChild(0).transform.GetChild(1).gameObject.activeSelf) thu = "banbe";
            JSONClass datasend = new JSONClass();
            datasend["class"] = "HomThu";
            datasend["method"] = "HuyTatCaThu";
            datasend["data"]["thu"] = thu;   
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {

                    if (json["status"].Value == "0")
                    {
                        if (thu == "hethong")
                        {
                            for (int i = 0; i < ScrollRectThu.transform.GetChild(0).transform.GetChild(0).transform.childCount; i++)
                            {
                                Destroy(ScrollRectThu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ScrollRectThu.transform.GetChild(0).transform.GetChild(1).transform.childCount; i++)
                            {
                                Destroy(ScrollRectThu.transform.GetChild(0).transform.GetChild(1).transform.GetChild(i).gameObject);
                            }
                        }
                        transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
                    }
                    CrGame.ins.OnThongBaoNhanh(json["message"].Value);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
          
        }
        else CrGame.ins.OnThongBaoNhanh("Không còn thư để xóa");
  
    }    
    public void TraLoiThu()
    {
        GameObject showthu = transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<InputField>().text = showthu.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<InputField>().text = "";
        transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<InputField>().text = "";
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuHomThu");
    }    
}
