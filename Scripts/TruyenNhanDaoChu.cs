using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TruyenNhanDaoChu : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txtSoTruyenNhan, txtSoExp, txtSoKimCuong;
    public GameObject ThietLap, XoaSuPhu;
    public Button btnNhanExp, btnNhanKc;
    public InputField inputName;
    void OnEnable()
    {
        StartCoroutine(Load());
        IEnumerator Load()
        {
            CrGame.ins.panelLoadDao.SetActive(true);
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemTruyenNhan/taikhoan/" +LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["namesuphu"].Value == "")
                {
                    ThietLap.SetActive(true);
                    XoaSuPhu.SetActive(false);
                }
                else
                {
                    ThietLap.SetActive(false);
                    XoaSuPhu.SetActive(true);

                    XoaSuPhu.transform.GetChild(0).GetComponent<Text>().text = json["namesuphu"].Value;
                    // CrGame.ins.friend.GetAvatarFriend(json["idsuphu"].Value,XoaSuPhu.transform.GetChild(1).GetComponent<Image>());
                    Friend.ins.LoadAvtFriend(json["idsuphu"].Value, XoaSuPhu.transform.GetChild(1).GetComponent<Image>(), XoaSuPhu.transform.GetChild(2).GetComponent<Image>());
                 //   XoaSuPhu.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite("Avatar" + json["toc"].Value);
                }
                txtSoTruyenNhan.text = "Số truyền nhân đã thu nạp : <color=#00ff00ff>"+ json["sotruyennhan"].Value + " </color>";
                txtSoExp.text = "Số kinh nghiệm nhận được từ truyền nhân : <color=#00ff00ff>"+ json["exp"].Value  + "</color>";
                txtSoKimCuong.text = "Số <color=#ff00ffff>kim cương</color> nhận được từ truyền nhân : <color=#ff00ffff>"+ json["kimcuong"].Value + "</color>";
                if (json["exp"].Value != "0") btnNhanExp.interactable = true;
                else btnNhanExp.interactable = false;
                if (json["kimcuong"].Value != "0") btnNhanKc.interactable = true;
                else btnNhanKc.interactable = false;
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void ChonTruyenNhan()
    {
        if(inputName.text != "")
        {
            StartCoroutine(Load());
            IEnumerator Load()
            {
                UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ChonTruyenNhan/taikhoanchon/" + inputName.text + "/taikhoan/" +LoginFacebook.ins.id);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    debug.Log(www.error);
                    CrGame.ins.OnThongBaoNhanh("Lỗi (0)", 1f);
                }
                else
                {
                    // Show results as text
                    debug.Log(www.downloadHandler.text);
                    JSONNode json = JSON.Parse(www.downloadHandler.text);
                    if (json != null)
                    {
                        ThietLap.SetActive(false);
                        XoaSuPhu.SetActive(true);
                        XoaSuPhu.transform.GetChild(0).GetComponent<Text>().text = json["namesuphu"].Value;
                       Friend.ins.LoadAvtFriend(json["idsuphu"].Value, XoaSuPhu.transform.GetChild(1).GetComponent<Image>(), XoaSuPhu.transform.GetChild(2).GetComponent<Image>());
                        //CrGame.ins.friend.GetAvatarFriend(json["idsuphu"].Value, XoaSuPhu.transform.GetChild(1).GetComponent<Image>());
                      //  XoaSuPhu.transform.GetChild(2).GetComponent<Image>().sprite = Inventory.LoadSprite("Avatar" + json["toc"].Value);
                        inputName.text = "";
                    }
                    else
                    {
                        CrGame.ins.OnThongBaoNhanh(www.downloadHandler.text, 1f);
                    }
                }
            }
        }    
    }
    public void XoaTruyenNhan()
    {
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XoaTruyenNhan/taikhoan/" +LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                if(www.downloadHandler.text == "0")
                {
                    ThietLap.SetActive(true);
                    XoaSuPhu.SetActive(false);
                    inputName.text = "";
                }
                else CrGame.ins.OnThongBaoNhanh(www.downloadHandler.text, 1f);
            }
        }
    }
    public void NhanQua(string qua)
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btnchon.interactable = false;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaTruyenNhan/qua/"+ qua +"/taikhoan/" +LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "0")
                {
                    btnchon.interactable = false;
                    if (qua == "exp") txtSoExp.text = "Số kinh nghiệm nhận được từ truyền nhân : <color=#00ff00ff>" + 0 + "</color>";
                    else txtSoKimCuong.text = "Số <color=#ff00ffff>kim cương</color> nhận được từ truyền nhân : <color=#ff00ffff>" + 0 + "</color>";
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(www.downloadHandler.text, 1f);
                    btnchon.interactable = true;
                }
            }
        }
    }
    public void Exit()
    {
       AllMenu.ins.DestroyMenu("MenuTruyenNhan");
    }    
}
