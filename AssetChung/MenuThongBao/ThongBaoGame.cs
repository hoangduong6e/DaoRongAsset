using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ThongBaoGame : MonoBehaviour
{
    // Start is called before the first frame update
    string Trang = "ThongBao";
    string muc = "TatCa";
    public int sotrang = 1;
    public Sprite[] spriteMuc;
    GameObject panelLoadDao;
    public string ServerName;
    public string id; CrGame crgame;
    public void Close()
    {
        if (crgame == null)
        {
            Destroy(gameObject);
        }
        else
        {
            AllMenu.ins.DestroyMenu("MenuThongBaoGame");
        }
    }
    public void OnThongBaoNhanh(string s, float time = 1f)
    {
        if(crgame == null)
        {
            LoginFacebook fb = GameObject.Find("facebookManager").GetComponent<LoginFacebook>();
            fb.OnThongBaoNhanh(s,time);
        }
        else
        {
            crgame.OnThongBaoNhanh(s,time);
        }
    }
    private void OnEnable()
    {
        //TextMeshPro txt = transform.GetChild(0).transform.GetChild(9).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();

        //TMP_TextInfo textInfo = txt.textInfo;
        //// The informations of the character at given index inside the text.
        //TMP_CharacterInfo charInfo = textInfo.characterInfo[15];
        //// The bottom right position of the character.
        //Vector3 bottomRight = charInfo.bottomRight;
        //debug.LogError("vi tri " + bottomRight);
        LoginFacebook fb = GameObject.Find("facebookManager").GetComponent<LoginFacebook>();
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CrGame>())
        {
            crgame = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CrGame>();
            panelLoadDao = crgame.panelLoadDao.gameObject;
        }
        else
        {
            panelLoadDao = GameObject.Find("facebookManager").GetComponent<LoginFacebook>().panelloaddao;
        }
        id = fb.id;
        ServerName = "http://" + fb.ServerChinh + "/"; ;
        LoadThongBao();

    }
    public void TestUpAnh(string link,Image newImage)
    {
        debug.LogError(link);
        StartCoroutine(DownloadImage());
        IEnumerator DownloadImage()
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                debug.Log(request.error);
            else
            {

                Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));

                //cập nhật thông tin tmp text lúc cần tới
                newImage.sprite = sprite;
                //TextMeshProUGUI txt = transform.GetChild(0).transform.GetChild(9).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
               // txt.ForceMeshUpdate();

                //lấy vị trí chữ cuối cùng của text
            //    int lastCharacterIndex = txt.textInfo.lineInfo[txt.textInfo.lineCount - 1].lastCharacterIndex;
               // Vector3 lastCharacterPosition = txt.textInfo.characterInfo[lastCharacterIndex].bottomRight;

                //Chuyển đổi sang vị trí world Vector3
                


                // đặt vị trí ảnh RectTransform 
             //   debug.LogError("Vị trí cuối cùng của chữ " + lastCharacterPositionWorld);

                //  GameObject newImageRectTransform = Instantiate(GameObject, transform.position, Quaternion.identity);

                // TextMeshProUGUI txtt = g.transform.GetChild(9).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                //GameObject objnew = new GameObject();
                //objnew.transform.SetParent(transform.GetChild(0).transform.GetChild(9).transform.GetChild(0).transform.GetChild(0).transform, false);
                //Image newImage = objnew.AddComponent<Image>();

               
               // newImage.SetNativeSize();

              

              //  objnew.transform.SetParent(txt.transform.parent);
            }
        }

     
    }
    void LoadThongBao()
    {
        panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(ServerName + "GetDataThongBao/taikhoan/" + id + "/trang/" + Trang + "/muc/" + muc + "/sotrang/" + sotrang);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    Quaylai();
                    GameObject objthongbao = transform.GetChild(0).transform.GetChild(8).gameObject;
                    for (int i = 0; i < 4; i++)
                    {
                        objthongbao.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    for (int i = 0; i < json["data"][i].Count; i++)
                    {
                        if (i >= 4) break;
                        objthongbao.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = json["data"][i]["time"].Value;
                        string muc = "Khác";
                        objthongbao.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().sprite = spriteMuc[1];
                        if (json["data"][i]["muc"].Value == "SuKien")
                        {
                            muc = "Sự kiện";
                            objthongbao.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().sprite = spriteMuc[2];
                        }
                        else if (json["data"][i]["muc"].Value == "BaoTri")
                        {
                            objthongbao.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().sprite = spriteMuc[0];
                            muc = "Bảo trì";
                        }

                        objthongbao.transform.GetChild(i).transform.GetChild(3).GetComponent<Text>().text = muc;
                        objthongbao.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>().text = json["data"][i]["tieude"].Value;
                        objthongbao.transform.GetChild(i).name = json["data"][i]["id"].Value;
                        objthongbao.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    GameObject alltrang = transform.GetChild(0).transform.GetChild(10).gameObject;
                    for (int i = 1; i < alltrang.transform.childCount - 1; i++)
                    {
                        Destroy(alltrang.transform.GetChild(i).gameObject);
                    }
                    GameObject btntrang = alltrang.transform.GetChild(0).gameObject;
                    if(sotrang == 1) btntrang.transform.GetChild(0).GetComponent<Image>().sprite = spriteMuc[7];
                    else btntrang.transform.GetChild(0).GetComponent<Image>().sprite = spriteMuc[8];
                    for (int j = 1; j < int.Parse(json["trang"].Value); j++)
                    {
                        GameObject obj = Instantiate(btntrang, transform.position, Quaternion.identity);
                        if(sotrang - 1 == j) obj.transform.GetChild(0).GetComponent<Image>().sprite = spriteMuc[7];
                        else obj.transform.GetChild(0).GetComponent<Image>().sprite = spriteMuc[8];
                        obj.transform.SetParent(alltrang.transform, false);
                        obj.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = (j + 1).ToString();
                        obj.transform.SetSiblingIndex(alltrang.transform.childCount-2);
                    }
                    //for (int i = 0; i < alltrang.transform.childCount - 2; i++)
                    //{
                       
                    //    if(i != sotrang - 1) alltrang.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = spriteMuc[8];
                    //    else alltrang.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = spriteMuc[7];
                    //    alltrang.transform.GetChild(i).name = "trang" + i;
                    //}
                 //   debug.LogError("nameobject " + alltrang.transform.GetChild(sotrang - 1).name);
                   // alltrang.transform.GetChild(sotrang - 1).transform.GetChild(0).GetComponent<Image>().sprite = spriteMuc[7]; 
                }
                else
                {
                    OnThongBaoNhanh("Lỗi");
                }
                panelLoadDao.SetActive(false);
            }
        }
    }
    public void XemBai()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;

        panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(ServerName + "Xembai/taikhoan/" + id + "/trang/" + Trang + "/muc/" + muc + "/id/" + btnchon.transform.parent.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject g = transform.GetChild(0).gameObject;
                    g.transform.GetChild(4).gameObject.SetActive(false);
                    g.transform.GetChild(5).gameObject.SetActive(false);
                    g.transform.GetChild(6).gameObject.SetActive(false);
                    g.transform.GetChild(7).gameObject.SetActive(false);
                    g.transform.GetChild(8).gameObject.SetActive(false);
                    g.transform.GetChild(9).gameObject.SetActive(true);
                    g.transform.GetChild(10).gameObject.SetActive(false);


                    string[] arrListStr = json["data"]["noidung"].Value.Split('\n');

                    GameObject Viewport = g.transform.GetChild(9).transform.GetChild(0).gameObject;

                    Image imgClone = Viewport.transform.GetChild(1).GetComponent<Image>();
                    Text TextClone = Viewport.transform.GetChild(2).GetComponent<Text>();
                    GameObject DeMucClone = Viewport.transform.GetChild(3).gameObject;

                    Text txtt = null;

                    if (arrListStr[0].StartsWith("link") == false && arrListStr[0].StartsWith("demuc") == false)
                    {
                        //  debug.LogError("Text");
                        GameObject txt = Instantiate(TextClone.gameObject, transform.position, Quaternion.identity);
                        txtt = txt.GetComponent<Text>();
                        txt.transform.SetParent(Viewport.transform.GetChild(0), false);
                        txt.SetActive(true);
                    }

                    for (int i = 0; i < arrListStr.Length; i++)
                    {
                        debug.Log(arrListStr[i]);
                        if (arrListStr[i].StartsWith("link"))
                        {
                            txtt = null;
                            GameObject img = Instantiate(imgClone.gameObject, transform.position, Quaternion.identity);
                            img.transform.SetParent(Viewport.transform.GetChild(0), false);
                            Image imgload = img.GetComponent<Image>();
                            RectTransform rt = imgload.rectTransform;
                            string[] width_length = arrListStr[i].Split('#')[2].Split('x');
                            rt.sizeDelta = new Vector2(float.Parse(width_length[0]), float.Parse(width_length[1]));
                            string linkimg = arrListStr[i].Split('#')[1];
                            TestUpAnh(linkimg, imgload);
                            img.SetActive(true);
                        }
                        else if (arrListStr[i].StartsWith("demuc"))
                        {
                            GameObject demuc = Instantiate(DeMucClone, transform.position, Quaternion.identity);
                            demuc.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = arrListStr[i].Split('#')[1];
                            demuc.transform.SetParent(Viewport.transform.GetChild(0), false);
                            demuc.SetActive(true);

                            //  txtt.text = arrListStr[i].Split('#')[1];
                            txtt = null;
                        }
                        else
                        {
                            if (txtt == null)
                            {
                                GameObject txt = Instantiate(TextClone.gameObject, transform.position, Quaternion.identity);
                                txtt = txt.GetComponent<Text>();
                                txt.transform.SetParent(Viewport.transform.GetChild(0), false);
                                txt.SetActive(true);
                            }
                            if (i + 1 < arrListStr.Length)
                            {
                                if (arrListStr[i + 1].StartsWith("link") == false)
                                {
                                    txtt.text += arrListStr[i] + "\n";
                                }
                                else
                                {
                                    txtt.text += arrListStr[i];
                                }
                            }
                            else
                            {
                                txtt.text += arrListStr[i];
                            }
                        }
                    }

                    //  TextMeshProUGUI txt = g.transform.GetChild(9).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                    //string[] arrListStr = json["data"]["noidung"].Value.Split('\n');

                    //List<GameObject> allimage =  new List<GameObject>();
                    //for (int i = 0; i < arrListStr.Length; i++)
                    //{
                    //    if (arrListStr[i].StartsWith("link"))
                    //    {
                    //        txt.ForceMeshUpdate();
                    //        // trước khi lấy thông tin text phải gọi hàm này để cập nhật
                    //        debug.LogError("Dòng " + i + " là link "+ arrListStr[i]);
                    //        // lấy dòng cuối cùng của text
                    //        int lastCharacterIndex = txt.textInfo.lineInfo[txt.textInfo.lineCount - 1].lastCharacterIndex; 
                    //        // lấy vị trí của dòng 
                    //        Vector3 lastCharacterPosition = txt.textInfo.characterInfo[lastCharacterIndex].bottomRight;
                    //        debug.LogError("vitri: " + lastCharacterPosition);
                    //       // Vector3 vec = txt.textInfo.lineInfo[0].
                    //          //  tạo object mới
                    //          GameObject objnew = new GameObject();
                    //        // nhét vào text
                    //        objnew.transform.SetParent(txt.transform, false);
                    //        // add image cho object
                    //        Image newImage = objnew.AddComponent<Image>();

                    //        // cắt chuỗi để lấy thông tin
                    //        string linkimg = arrListStr[i].Split('#')[1];
                    //        RectTransform rt = newImage.rectTransform;
                    //        string[] width_length = arrListStr[i].Split('#')[2].Split('x');
                    //        rt.sizeDelta = new Vector2(float.Parse(width_length[0]), float.Parse(width_length[1]));

                    //        Vector3 lastCharacterPositionWorld = txt.transform.TransformPoint(lastCharacterPosition);
                    //        RectTransform newImageRectTransform = newImage.GetComponent<RectTransform>();
                    //        newImageRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    //        newImageRectTransform.anchorMin = new Vector2(0.5f, 0f);
                    //        newImageRectTransform.anchorMax = new Vector2(0.5f, 0f);
                    //        newImageRectTransform.anchoredPosition = new Vector2(lastCharacterPositionWorld.x, lastCharacterPositionWorld.y + txt.preferredHeight); //-txt.preferredHeight

                    //        //  yield return new WaitForSeconds(0.1f);
                    //        //objnew.transform.SetParent(txt.transform.parent.transform,false); // đưa image ra chỗ khác để thêm khoảng trống không bị kéo trôi theo
                    //        //RectTransform vitringoai = objnew.GetComponent<RectTransform>();
                    //        //    yield return new WaitForSeconds(0.1f); 

                    //        for (int j = 0; j < float.Parse(width_length[0]) * 2.5f / 100; j++) // tạo khoảng trống cho hình ảnh
                    //        {
                    //            txt.text += "\n"; // thêm khoảng trống cho vừa với hình ảnh
                    //        }

                    //        //  yield return new WaitForSeconds(0.1f);
                    //        allimage.Add(objnew);
                    //        //objnew.transform.SetParent(txt.transform,false); // đưa image quay lại
                    //       // newImageRectTransform.anchoredPosition = new Vector2(lastCharacterPositionWorld.x, lastCharacterPositionWorld.y);
                    //        //  newImageRectTransform = vitringoai;
                    //        TestUpAnh(linkimg, newImage);
                    //       // yield return new WaitForSeconds(0.1f);
                    //    }
                    //    else txt.text += arrListStr[i] + "\n";

                    //}
                    //yield return new WaitForSeconds(0.1f);
                    //for (int j = 0; j < allimage.Count; j++)
                    //{
                    //    allimage[j].transform.SetParent(txt.transform); // đưa image quay lại
                    //}
                }
                else
                {
                    OnThongBaoNhanh(json["status"].Value);
                }
                panelLoadDao.SetActive(false);
            }
        }



    }
    public void SangTrang()
    {
        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int trangsang = int.Parse(g.transform.GetChild(0).GetComponent<Text>().text);
        if (trangsang != sotrang)
        {
            sotrang = int.Parse(g.transform.GetChild(0).GetComponent<Text>().text);
            LoadThongBao();
        }
    }
    public void Sangtrangnhanh(int i)
    {
        GameObject objthongbao = transform.GetChild(0).transform.GetChild(10).gameObject;
        if (sotrang + i < objthongbao.transform.childCount)
        {
            sotrang += i;
            LoadThongBao();
        }
        else if(sotrang + 1 < objthongbao.transform.childCount)
        {
            sotrang += 1;
            LoadThongBao();
        }
    }    
    public void LoadThongBaoMuc(string Muc)
    {
        Image imgchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<Image>();
        if(muc != Muc)
        {
            sotrang = 1;
            muc = Muc;
            GameObject g = transform.GetChild(0).gameObject;
            g.transform.GetChild(4).GetComponent<Image>().sprite = spriteMuc[4];
            g.transform.GetChild(5).GetComponent<Image>().sprite = spriteMuc[4];
            g.transform.GetChild(6).GetComponent<Image>().sprite = spriteMuc[4];
            g.transform.GetChild(7).GetComponent<Image>().sprite = spriteMuc[4];
            imgchon.sprite = spriteMuc[3];

            LoadThongBao();
        }    
       
    }
    public void LoadThongBaotrang(string trang)
    {
        Image imgchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<Image>();
        if(Trang != trang)
        {
            sotrang = 1;
            Trang = trang;
            muc = "TatCa";
            GameObject g = transform.GetChild(0).gameObject;

            g.transform.GetChild(4).GetComponent<Image>().sprite = spriteMuc[3];
            g.transform.GetChild(5).GetComponent<Image>().sprite = spriteMuc[4];
            g.transform.GetChild(6).GetComponent<Image>().sprite = spriteMuc[4];
            g.transform.GetChild(7).GetComponent<Image>().sprite = spriteMuc[4];

            g.transform.GetChild(1).GetComponent<Image>().sprite = spriteMuc[6];
            g.transform.GetChild(2).GetComponent<Image>().sprite = spriteMuc[6];
            g.transform.GetChild(3).GetComponent<Image>().sprite = spriteMuc[6];
            imgchon.sprite = spriteMuc[5];
            LoadThongBao();
        }
     
    }
  
    public void Quaylai()
    {
        GameObject g = transform.GetChild(0).gameObject;
        g.transform.GetChild(4).gameObject.SetActive(true);
        g.transform.GetChild(5).gameObject.SetActive(true);
        g.transform.GetChild(6).gameObject.SetActive(true);
        g.transform.GetChild(7).gameObject.SetActive(true);
        g.transform.GetChild(8).gameObject.SetActive(true);
        g.transform.GetChild(9).gameObject.SetActive(false);
        g.transform.GetChild(10).gameObject.SetActive(true);

        //   TextMeshProUGUI txt = g.transform.GetChild(9).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //   txt.text = "";
        GameObject content = g.transform.GetChild(9).transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }    
}
