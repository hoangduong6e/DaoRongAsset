﻿using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ItemExp : MonoBehaviour
{
    public string Event, Value;
    // Start is called before the first frame update

    public void Use()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", CrGame.ins.trencung.gameObject, true, CrGame.ins.panelLoadDao.transform.GetSiblingIndex() - 1).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();
        string[] cat = Value.Split("-");
        if(int.Parse(CrGame.ins.txtLevel.text) >= int.Parse(cat[1]) && int.Parse(CrGame.ins.txtLevel.text) <= int.Parse(cat[2]))
        {
            tbc.txtThongBao.text = "Cấp độ của bạn trong khoảng cấp độ của bình. Bạn nhận được <color=yellow>5%</color> <color=lime>Kinh nghiệm</color> của cấp độ hiện tại";
        }
        else tbc.txtThongBao.text = "Cấp độ của bạn chưa đủ để sử dụng bình này. Bạn chỉ nhận được <color=yellow>1%</color> <color=lime>Kinh nghiệm</color> của cấp độ " + cat[1];
        tbc.btnChon.onClick.AddListener(delegate { NetworkManager.ins.socket.Emit(Event, JSONObject.CreateStringObject(Value)); ; });
    }
    public void UseHopQuaBiAn()
    {
        Button btnsd = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btnsd.enabled = false;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "UseItem";
        datasend["data"]["nameitem"] = Value;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                //clone.AddComponent<QuaBay>();
                if (gameObject.GetComponent<QuaBay>())
                {
                    GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity) as GameObject;
                    clone.transform.GetChild(0).gameObject.SetActive(false);
                    clone.transform.GetChild(1).gameObject.SetActive(false);
                    clone.transform.GetChild(2).gameObject.SetActive(false);
                    GameObject tfbay = GameObject.Find("btnQuaOnline");
                    clone.transform.SetParent(AllMenu.ins.transform, false);
                    clone.transform.position = gameObject.transform.position;
                    QuaBay quabay = clone.GetComponent<QuaBay>();
                    quabay.vitribay = tfbay;
                    quabay.enabled = true;
                }
                Inventory.ins.AddItem(Value, -1);
            }
            else CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            btnsd.enabled = true;
        }
    }
}
