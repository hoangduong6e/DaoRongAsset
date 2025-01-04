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
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TruyenNhan";
        datasend["method"] = "XemTruyenNhan";
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
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
                txtSoTruyenNhan.text = "Số truyền nhân đã thu nạp : <color=#00ff00ff>" + json["sotruyennhan"].Value + " </color>";
                txtSoExp.text = "Số kinh nghiệm nhận được từ truyền nhân : <color=#00ff00ff>" + json["exp"].Value + "</color>";
                txtSoKimCuong.text = "Số <color=#ff00ffff>kim cương</color> nhận được từ truyền nhân : <color=#ff00ffff>" + json["kimcuong"].Value + "</color>";
                if (json["exp"].Value != "0") btnNhanExp.interactable = true;
                else btnNhanExp.interactable = false;
                if (json["kimcuong"].Value != "0") btnNhanKc.interactable = true;
                else btnNhanKc.interactable = false;
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }
    public void ChonTruyenNhan()
    {
        if(inputName.text != "")
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "TruyenNhan";
            datasend["method"] = "ChonTruyenNhan";
            datasend["data"]["taikhoanchon"] = inputName.text;
            NetworkManager.ins.SendServer(datasend, Ok);

            void Ok(JSONNode jsonn)
            {
                JSONNode json = jsonn["data"];
                if (jsonn["status"].AsString == "0")
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
                    CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
                }
            }
        }    
    }
    public void XoaTruyenNhan()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TruyenNhan";
        datasend["method"] = "XoaTruyenNhan";
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                ThietLap.SetActive(true);
                XoaSuPhu.SetActive(false);
                inputName.text = "";
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }
    public void NhanQua(string qua)
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btnchon.interactable = false;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TruyenNhan";
        datasend["method"] = "NhanQuaTruyenNhan";
        datasend["data"]["qua"] = qua;
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                btnchon.interactable = false;
                if (qua == "exp") txtSoExp.text = "Số kinh nghiệm nhận được từ truyền nhân : <color=#00ff00ff>" + 0 + "</color>";
                else txtSoKimCuong.text = "Số <color=#ff00ffff>kim cương</color> nhận được từ truyền nhân : <color=#ff00ffff>" + 0 + "</color>";
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
                btnchon.interactable = true;
            }
        }
    }
    public void Exit()
    {
       AllMenu.ins.DestroyMenu("MenuTruyenNhan");
    }    
}
