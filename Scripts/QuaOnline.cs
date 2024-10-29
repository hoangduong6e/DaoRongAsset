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
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanQuaOnline/taikhoan/" + LoginFacebook.ins.id + "/qua/" + index);
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
                CrGame.ins.panelLoadDao.SetActive(false);
                if(www.downloadHandler.text == "1")
                {
                    imgQua.transform.GetChild(index).transform.GetChild(2).gameObject.SetActive(true);
                    imgQua.transform.GetChild(index).transform.GetChild(1).gameObject.SetActive(false);

                    GameObject hieuungbay = Instantiate(imgQua.transform.GetChild(index).transform.GetChild(0).gameObject,transform.position, Quaternion.identity);
                    hieuungbay.SetActive(false);
                    hieuungbay.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
                    hieuungbay.transform.position = imgQua.transform.GetChild(index).transform.GetChild(0).transform.position;
                    hieuungbay.GetComponent<Image>().SetNativeSize();
                    hieuungbay.AddComponent<QuaBay>();
                    hieuungbay.SetActive(true);
                }
            }
        }
    }
    public void Close()
    {
        AllMenu.ins.DestroyMenu("MenuNhanQuaOnline");
    }
}
