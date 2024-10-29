using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuQuayRuong : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject VienXanh,Oquay,btnexit,Qua;
    public Image imgThanhQua;public string nameRuong;
    public Sprite spchuadenmoc, spdenmoc,spchuanhan,spduocnhan,spdanhan;
    public Text txtsolanquay;
    public Button btnquay;
    public void NhanQuaRuong()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        Button btnnhan = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaRuong/taikhoan/" + LoginFacebook.ins.id + "/ruong/" + btnnhan.transform.parent.GetSiblingIndex());
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBao(true, "Lỗi", true);
            }
            else
            {
                // Show results as text

                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "0")
                {
                    Image imgQua = Qua.transform.GetChild(btnnhan.transform.parent.GetSiblingIndex()).GetChild(0).GetComponent<Image>();
                    imgQua.sprite = spdanhan;
                    btnnhan.gameObject.SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(www.downloadHandler.text,2);
                }    
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
       
       
    }    
    public void LoadSoLanQuay(float solanquay)
    {
        float fillamount = solanquay / 700;
        imgThanhQua.fillAmount = fillamount;
        txtsolanquay.text = solanquay + "";
    }
    public void QuayRuong()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "QuayRuong/taikhoan/" + LoginFacebook.ins.id + "/nameRuong/" + nameRuong);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBao(true, "Lỗi", true);
            }
            else
            {

                CrGame.ins.panelLoadDao.SetActive(false);
                btnquay.interactable = false;
                btnexit.SetActive(false);
               // debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["thongbao"].Value != "")
                {
                    CrGame.ins.OnThongBao(true, json["thongbao"].Value, true);
                    CrGame.ins.panelLoadDao.SetActive(false);
                }
                else
                {
                    VienXanh.SetActive(true);
                    for (int i = 0; i < 40; i++)
                    {
                        Transform oquay = Oquay.transform.GetChild(Random.Range(0, 8));
                        VienXanh.transform.SetParent(oquay);
                        VienXanh.transform.position = oquay.transform.position;
                        yield return new WaitForSeconds(0.05f);
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        if (Oquay.transform.GetChild(i).name == json["namee"].Value)
                        {
                            VienXanh.transform.SetParent(Oquay.transform.GetChild(i));
                            VienXanh.transform.position = Oquay.transform.GetChild(i).transform.position;
                            for (int j = 0; j < 6; j++)
                            {
                                VienXanh.SetActive(false);
                                yield return new WaitForSeconds(0.1f);
                                VienXanh.SetActive(true);
                                yield return new WaitForSeconds(0.1f);
                            }
                            GameObject hieuungbay = Instantiate(Oquay.transform.GetChild(i).transform.GetChild(0).gameObject, Oquay.transform.GetChild(i).transform.position, Quaternion.identity);
                            hieuungbay.SetActive(false);
                            hieuungbay.transform.SetParent(CrGame.ins.trencung, false);
                            hieuungbay.transform.position = Oquay.transform.GetChild(i).transform.position;
                            hieuungbay.AddComponent<QuaBay>();
                            if (json["namerong"].Value == "")
                            {

                            }
                            else
                            {
                                bool hoangkim = false;
                                if (json["hoangkim"].ToString() != "") hoangkim = true;
                                Inventory.ins.AddItemRong(json["id"].Value, json["nameitem"].Value, byte.Parse(json["sao"].Value), int.Parse(json["level"].Value), int.Parse(json["exp"].Value), int.Parse(json["maxexp"].Value),
                                    byte.Parse(json["tienhoa"].Value), 0, json["namerong"].Value, json["nameobject"].Value, hoangkim, json["ngoc"].Value,false);
                            }
                            Oquay.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                            hieuungbay.SetActive(true);
                            Inventory.ins.AddItem(nameRuong, -1);
                            btnquay.interactable = true;
                            btnexit.SetActive(true);
                            VienXanh.SetActive(false);
                            if (Inventory.ins.ListItemThuong.ContainsKey("item" + nameRuong))
                            {
                                Inventory.ins.ListItemThuong["item" + nameRuong].GetComponent<itemRuong>().XemRuong(false);
                                btnquay.transform.GetChild(0).GetComponent<Text>().text = "Bắt Đầu " + "(" + Inventory.ins.ListItemThuong["item" + nameRuong].transform.GetChild(0).GetComponent<Text>().text + ")";
                            }
                            else
                            {
                                gameObject.SetActive(false);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
    public void Exit()
    {
        for (int i = 0; i < Oquay.transform.childCount; i++)
        {
            Oquay.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            Oquay.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
