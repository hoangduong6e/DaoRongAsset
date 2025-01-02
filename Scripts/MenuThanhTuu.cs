using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuThanhTuu : MonoBehaviour
{
    public ScrollRect scrollRectChon;
    public Sprite imgchon, imgkhongchon,ngoisaoSang;
    public GameObject prefabThanh, prefabThanhHoanThanh,contentHoanThanh;
    private bool getHoanthanh = false;
    public Sprite[] allIcon;
    //public ScrollRect scrollRectChon;
    public string nameThanhTuuGet = "thanhtuuEvent";
    public static List<string> thanhtuuEmitHoanThanh = new List<string>();
    private void OnEnable()
    {
        // GetList(nameThanhTuuGet);
        OpenTab(nameThanhTuuGet);
    }
    private Sprite GetIcon(string nameicon)
    {
        for (int i = 0; i < allIcon.Length; i++)
        {
            if (allIcon[i].name == nameicon) return allIcon[i];
        }
        return allIcon[0];
    }
    void ResizeItem(Image image)
    {
        // Lấy kích thước gốc của image
        float originalWidth = image.rectTransform.rect.width;
        float originalHeight = image.rectTransform.rect.height;

        // Kiểm tra nếu kích thước lớn hơn 150
        if (originalWidth > 150f || originalHeight > 150f)
        {
            // Tính toán tỉ lệ thu nhỏ
            float widthRatio = 120f / originalWidth;
            float heightRatio = 120f / originalHeight;
            float minRatio = Mathf.Min(widthRatio, heightRatio);

            // Điều chỉnh kích thước
            image.rectTransform.sizeDelta = new Vector2(originalWidth * minRatio, originalHeight * minRatio);
        }
    }
    private void GetList(string tab,string next = "")
    {
        //debug.Log("GetList " + tab);
        if (tab == "thanhtuuHoanThanh" && getHoanthanh && next == "") return;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "ThanhTuu";
        datasend["method"] = "GetDataThanhTuu";
        datasend["data"]["tab"] = tab;
        datasend["data"]["next"] = next;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject g = transform.GetChild(0).gameObject;

                GameObject tabchon = g.transform.Find(tab).gameObject;
                scrollRectChon = tabchon.GetComponent<ScrollRect>();
                GameObject content = tabchon.transform.GetChild(0).transform.GetChild(0).gameObject;
                if(tab != "thanhtuuHoanThanh")
                {
                    for (int i = 0; i < json["listThanhTuu"].Count; i++)
                    {
                        GameObject instan = Instantiate(prefabThanh, transform.position, Quaternion.identity);
                        instan.transform.SetParent(content.transform, false);
                        instan.transform.Find("txtnamenhiemvu").GetComponent<Text>().text = json["listThanhTuu"][i]["name"].AsString;
                        instan.name = json["listThanhTuu"][i]["key"].AsString + "*" + json["listThanhTuu"][i]["muc"].AsString;
                        instan.transform.Find("thanhthanhtuu").GetComponent<Image>().fillAmount = json["listThanhTuu"][i]["dalam"].AsFloat / json["listThanhTuu"][i]["maxnhiemvu"].AsFloat;
                        instan.transform.Find("txttiendo").GetComponent<Text>().text = json["listThanhTuu"][i]["dalam"].AsString + "/" + json["listThanhTuu"][i]["maxnhiemvu"].AsString;
                        instan.gameObject.SetActive(true);
                        GameObject allQua = instan.transform.Find("allQua").gameObject;
                        for (int j = 0; j < json["listThanhTuu"][i]["qua"].Count; j++)
                        {
                            allQua.transform.GetChild(j).gameObject.SetActive(true);
                            Image img = allQua.transform.GetChild(j).transform.GetChild(0).GetComponent<Image>();
                            Text txtsoluong = allQua.transform.GetChild(j).transform.GetChild(1).GetComponent<Text>();
                            if (json["listThanhTuu"][i]["qua"][j]["loaiitem"].AsString == "item")
                            {
                                img.sprite = Inventory.LoadSprite(json["listThanhTuu"][i]["qua"][j]["name"].AsString);
                            
                                txtsoluong.text = json["listThanhTuu"][i]["qua"][j]["soluong"].AsString;
                            }
                            else if (json["listThanhTuu"][i]["qua"][j]["loaiitem"].AsString == "itemrong")
                            {
                                img.sprite = Inventory.LoadSpriteRong(json["listThanhTuu"][i]["qua"][j]["name"].AsString + 1);
                                Vector3 localscale = img.transform.localScale;
                                img.transform.localScale = new Vector3(localscale.x / 1.5f, localscale.y / 1.5f, localscale.z);
                                txtsoluong.text = json["listThanhTuu"][i]["qua"][j]["sao"].AsString + " sao";
                            }
                            else
                            {
                                SuKienGioiHan.LoadImage("item", json["listThanhTuu"][i]["qua"][j]["name"].AsString, img);
                                txtsoluong.text = json["listThanhTuu"][i]["qua"][j]["soluong"].AsString;
                            }    
                            img.name = json["listThanhTuu"][i]["qua"][j]["name"].AsString;
                            img.SetNativeSize();
                            ResizeItem(img);

                        }
                        if (json["listThanhTuu"][i]["duocnhan"].AsBool)
                        {
                            GameObject btnnhanqua = instan.transform.Find("btnNhanQua").gameObject;
                            btnnhanqua.SetActive(true);
                            Image imgngoisao = instan.transform.Find("imgNgoiSao").GetComponent<Image>();
                            imgngoisao.sprite = ngoisaoSang;
                            imgngoisao.SetNativeSize();
                            btnnhanqua.GetComponent<Button>().onClick.AddListener(NhanQuaThanhTuu);
                        }
                        Image imgg = instan.transform.Find("iconnhiemvu").GetComponent<Image>();
                        imgg.gameObject.name = json["listThanhTuu"][i]["icon"].AsString;


                        imgg.sprite = GetIcon(json["listThanhTuu"][i]["icon"].AsString);
                        imgg.SetNativeSize();

                        ResizeItem(imgg);
                    }
                }    
                else
                {
                    // load thanh tuu hoan thanh
                    if (tab == "thanhtuuHoanThanh") getHoanthanh = true;
                    for (int i = 0; i < json["listThanhTuu"].Count; i++)
                    {
                        GameObject instan = Instantiate(prefabThanhHoanThanh, transform.position, Quaternion.identity);
                        instan.transform.SetParent(contentHoanThanh.transform, false);
                        instan.transform.Find("txtnamenhiemvu").GetComponent<Text>().text = json["listThanhTuu"][i]["name"].AsString;
                        instan.gameObject.SetActive(true);
                        instan.name = json["listThanhTuu"][i]["key"].AsString;
                        instan.transform.GetChild(0).name = json["listThanhTuu"][i]["keychinh"].AsString;
                        instan.transform.GetChild(1).name = json["listThanhTuu"][i]["namett"].AsString;
                        Image img = instan.transform.Find("iconnhiemvu").GetComponent<Image>();
                        img.sprite = GetIcon(json["listThanhTuu"][i]["icon"].AsString);
                        img.SetNativeSize();
                        ResizeItem(img);
                    }    

                }
            
           
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void NhanQuaThanhTuu()
    {
        Button btnnhan = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        string namethanhtuu = btnnhan.transform.parent.gameObject.name;
        string nametab = scrollRectChon.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "ThanhTuu";
        datasend["method"] = "NhanQuaThanhTuu";
        datasend["data"]["tab"] = nametab;
        datasend["data"]["namethanhtuu"] = namethanhtuu;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject g = transform.GetChild(0).gameObject;
                GameObject content = g.transform.Find(nametab).gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject thanhtuuNhan = content.transform.Find(namethanhtuu).gameObject;
                GameObject allQua = thanhtuuNhan.transform.Find("allQua").gameObject;

                Transform trencung = GameObject.FindGameObjectWithTag("trencung").transform;
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    for (int i = 0; i < allQua.transform.childCount; i++)
                    {
                        if (allQua.transform.GetChild(i).gameObject.activeSelf)
                        {
                            QuaBay quabay = allQua.transform.GetChild(i).GetChild(0).gameObject.AddComponent<QuaBay>();
                            quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                            quabay.transform.SetParent(trencung);
                            yield return new WaitForSeconds(0.2f);
                        }
                        else break;
                    }
                    if(getHoanthanh) AddHoanThanh(thanhtuuNhan.transform.GetChild(2).name, thanhtuuNhan.transform.Find("txtnamenhiemvu").GetComponent<Text>().text);
                    Destroy(thanhtuuNhan);
               
                }
              
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OnScroll()
    {
        debug.Log(scrollRectChon.verticalNormalizedPosition);
        float get = 0.5f;
        Transform content = scrollRectChon.transform.GetChild(0).transform.GetChild(0);
        if (content.transform.childCount < 10) get = 0.5f;
        else if (content.transform.childCount >= 10 && content.transform.childCount < 20) get = 0.1f;
        else get = 0f;
        if (scrollRectChon.verticalNormalizedPosition <= get)
        {
            LoadMoreContent();
        }
    }
    private void LoadMoreContent()
    {
        //GameObject content = scrollRectChon.transform.GetChild(0).transform.GetChild(0).gameObject;
        //GameObject item = content.transform.GetChild(0).gameObject;

        //for (int i = 0; i < 4; i++)
        //{
        //    GameObject instan = Instantiate(item, transform.position, Quaternion.identity);
        //    instan.transform.SetParent(content.transform, false);
        //}
        GameObject content = scrollRectChon.transform.GetChild(0).transform.GetChild(0).gameObject;
        //if(scrollRectChon.name == "thanhtuuHoanThanh")
        //{

        //    return;
        //}    
        if(content.transform.childCount > 0 && scrollRectChon.name != "thanhtuuHoanThanh")
        {
            GetList(scrollRectChon.name, content.transform.GetChild(content.transform.childCount - 1).name);
        }
        else if (content.transform.childCount > 0 && scrollRectChon.name == "thanhtuuHoanThanh")
        {

            Transform child = content.transform.GetChild(content.transform.childCount - 1);
            if(child.GetChild(0).name != "") GetList(scrollRectChon.name, child.GetChild(0).name + "*" + child.name + "*" + child.GetChild(1).name);

        }

        debug.Log("Thêm nội dung!!!");
    }
    public void OpenTab()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        GameObject g = transform.GetChild(0).gameObject;
        GameObject tabchon = g.transform.GetChild(btnchon.transform.GetSiblingIndex() + 4).gameObject;
        if(tabchon.transform.GetChild(0).transform.GetChild(0).transform.childCount == 0 || tabchon.name == "thanhtuuHoanThanh")
        {
            // debug.Log("GetList " + tabchon.name);
           
            GetList(tabchon.name);
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject btn = g.transform.Find("btn" + i).gameObject;
            btn.GetComponent<Image>().sprite = imgkhongchon;
            g.transform.GetChild(btn.transform.GetSiblingIndex() + 4).gameObject.SetActive(false);

        }
        scrollRectChon = tabchon.GetComponent<ScrollRect>();

        btnchon.GetComponent<Image>().sprite = imgchon;
        tabchon.SetActive(true);
    }    
    public void OpenTab(string s)
    {
        GameObject g = transform.GetChild(0).gameObject;
        GameObject objthanhtuu = g.transform.Find(s).gameObject;
        GetList(s);
        for (int i = 0; i < 4; i++)
        {
            GameObject btn = g.transform.Find("btn" + i).gameObject;
            btn.GetComponent<Image>().sprite = imgkhongchon;
            g.transform.GetChild(btn.transform.GetSiblingIndex() + 4).gameObject.SetActive(false);
        }
        objthanhtuu.gameObject.SetActive(true);
        scrollRectChon = objthanhtuu.GetComponent<ScrollRect>();
        g.transform.GetChild(objthanhtuu.transform.GetSiblingIndex() - 4).GetComponent<Image>().sprite = imgchon;
    }
    public void AddHoanThanh(string nameIcon, string name)
    {
        GameObject instan = Instantiate(prefabThanhHoanThanh, transform.position, Quaternion.identity);
        instan.transform.SetParent(contentHoanThanh.transform, false);
        instan.transform.Find("txtnamenhiemvu").GetComponent<Text>().text = name;
        instan.gameObject.SetActive(true);

        Image img = instan.transform.Find("iconnhiemvu").GetComponent<Image>();
        img.sprite = GetIcon(nameIcon);
        img.SetNativeSize();
        instan.transform.GetChild(0).name = "";
    }    
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuThanhTuu");
    }    
}
