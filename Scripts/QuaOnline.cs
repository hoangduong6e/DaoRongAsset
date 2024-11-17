using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuaOnline : MonoBehaviour
{
    public GameObject imgQua;
    public Text txtsoluong;
    // Start is called before the first frame update
    public void NhanQua()
    {
        Button btnnhan = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int index = btnnhan.transform.parent.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "NhanQuaOnline";
        datasend["data"]["qua"] = index.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                imgQua.transform.GetChild(index).transform.GetChild(2).gameObject.SetActive(true);
                imgQua.transform.GetChild(index).transform.GetChild(1).gameObject.SetActive(false);

                GameObject hieuungbay = Instantiate(imgQua.transform.GetChild(index).transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                hieuungbay.SetActive(false);
                hieuungbay.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
                hieuungbay.transform.position = imgQua.transform.GetChild(index).transform.GetChild(0).transform.position;
                hieuungbay.GetComponent<Image>().SetNativeSize();
                hieuungbay.AddComponent<QuaBay>();
                hieuungbay.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
            }
        }
    }
    public void Close()
    {
        AllMenu.ins.DestroyMenu("MenuNhanQuaOnline");
    }
}
