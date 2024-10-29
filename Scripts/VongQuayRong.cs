using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class VongQuayRong : MonoBehaviour
{
    // Start is called before the first frame update
    CrGame crgame;public string nameTrung;
    public GameObject Item;public GameObject[] ORong,ContentRong;
    NetworkManager net;public Button btnQuay, btnExit;public byte soquay = 0;Inventory inventory;
    void Awake()
    {
        net = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NetworkManager>();
        crgame = net.GetComponent<CrGame>();
        inventory = net.GetComponent<Inventory>();
    }
    public void BatDauQuay()
    {
        net.socket.Emit("QuayRong",JSONObject.CreateStringObject(nameTrung));
    }
    public void ExitMenu()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ContentRong[i].transform.childCount; j++)
            {
                Destroy(ContentRong[i].transform.GetChild(j).gameObject);
            }
            if (ORong[i].transform.GetChild(0).GetComponent<UI_InfiniteScroll>())
            {
                UI_InfiniteScroll ui = ORong[i].transform.GetChild(0).GetComponent<UI_InfiniteScroll>();
                Destroy(ui);
            }
            QuayRong quayrong = ORong[i].transform.GetChild(0).GetComponent<QuayRong>();
            GridLayoutGroup grid = ContentRong[i].GetComponent<GridLayoutGroup>();grid.enabled = true;
            ContentSizeFitter contentsize = ContentRong[i].GetComponent<ContentSizeFitter>();contentsize.enabled = true;
            quayrong.enabled = false;
            ORong[i].SetActive(false);
            RectTransform rect = ContentRong[i].GetComponent<RectTransform>();
            rect.transform.position = new Vector3(rect.transform.position.x, 2,0);
        }
        gameObject.SetActive(false);
    }
    public void XemTrungRong(string nametrung)
    {
        nameTrung = nametrung;
        btnQuay.interactable = false;
        StartCoroutine(InfoTrungRong());
         IEnumerator InfoTrungRong() // xem dấu chấm hỏi công trình
         {
            XemTrung xn = new XemTrung(nametrung);
            string data = JsonUtility.ToJson(xn);
            var request = new UnityWebRequest(crgame.ServerName + "xemTrungRong", "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                debug.Log(request.error);
                crgame.OnThongBao(true, "Lỗi", true);
            }
            else
            {
                debug.Log(request.downloadHandler.text);
                JSONNode json = JSON.Parse(request.downloadHandler.text);
                if(json["OQuay0"] != null)
                {
                    ORong[0].SetActive(true);
                    for (int i = 0; i < json["OQuay0"].Count; i++)
                    {
                        string path = json["OQuay0"][i]["name"].Value + (int.Parse(json["OQuay0"][i]["tienhoa"].AsString) + 1);
                        GameObject item = Instantiate(Item, transform.position, Quaternion.identity) as GameObject;
                        item.name = "Oitem0" + json["OQuay0"][i]["name"].Value;
                        item.transform.SetParent(ContentRong[0].transform, false);
                        Image imgItem = item.transform.GetChild(0).GetComponent<Image>();
                        //if(inventory.Cachesprite.ContainsKey(path))
                        //{
                        //    imgItem.sprite = inventory.Cachesprite[path];
                        //}
                        //else
                        //{
                        //    imgItem.sprite = SGResources.Load<Sprite>("GameData/Sprite/Rong/" + path);
                        //    inventory.Cachesprite.Add(path, imgItem.sprite);
                        //}
                        imgItem.sprite = Inventory.LoadSpriteRong(path);
                        Text txtsao = item.transform.GetChild(1).GetComponent<Text>();
                        txtsao.text = json["OQuay0"][i]["sao"].AsString;
                        imgItem.SetNativeSize();
                        item.SetActive(true);
                    }
                    GameObject scroll = ORong[0].transform.GetChild(0).gameObject;
                    scroll.AddComponent<UI_InfiniteScroll>();
                }
                if (json["OQuay1"] != null)
                {
                    ORong[1].SetActive(true);
                    for (int i = 0; i < json["OQuay1"].Count; i++)
                    {
                        string path = json["OQuay1"][i]["name"].Value + (int.Parse(json["OQuay1"][i]["tienhoa"].AsString) + 1);
                        GameObject item = Instantiate(Item, transform.position, Quaternion.identity) as GameObject;
                        item.name = "Oitem1" + json["OQuay1"][i]["name"].Value;
                        item.transform.SetParent(ContentRong[1].transform, false);
                        Image imgItem = item.transform.GetChild(0).GetComponent<Image>();
                        imgItem.sprite = Inventory.LoadSpriteRong(path);
                        Text txtsao = item.transform.GetChild(1).GetComponent<Text>();
                        txtsao.text = json["OQuay1"][i]["sao"].AsString;
                        imgItem.SetNativeSize();
                        item.SetActive(true);
                    }
                    GameObject scroll = ORong[1].transform.GetChild(0).gameObject;
                    scroll.AddComponent<UI_InfiniteScroll>();
                }
                if (json["OQuay2"] != null)
                {
                    ORong[2].SetActive(true);
                    for (int i = 0; i < json["OQuay2"].Count; i++)
                    {
                        string path = json["OQuay2"][i]["name"].Value + (int.Parse(json["OQuay2"][i]["tienhoa"].AsString) + 1);
                        GameObject item = Instantiate(Item, transform.position, Quaternion.identity) as GameObject;
                        item.name = "Oitem2" + json["OQuay2"][i]["name"].Value;
                        item.transform.SetParent(ContentRong[2].transform, false);
                        Image imgItem = item.transform.GetChild(0).GetComponent<Image>();
                        imgItem.sprite = Inventory.LoadSpriteRong(path);
                        Text txtsao = item.transform.GetChild(1).GetComponent<Text>();
                        txtsao.text = json["OQuay2"][i]["sao"].AsString;
                        imgItem.SetNativeSize();
                        item.SetActive(true);
                    }
                    GameObject scroll = ORong[2].transform.GetChild(0).gameObject;
                    scroll.AddComponent<UI_InfiniteScroll>();
                }
                //menuQuay.SetActive(true);
                btnQuay.interactable = true;
                // menuQuay.SetActive(true);
            }
        }
    }
}
[SerializeField]
public class XemTrung
{
    public string nametrung;
    public XemTrung(string Name)
    {
        nametrung = Name;
    }
}
