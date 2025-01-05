using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuTienHoaRong : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject menuChonRong, contentItemRong, ObjItemRong, ObjRongChon, contentRongChon, panetoidan, ContentItemYeuCau;
    private Transform vitriRong;
    private string[] allRong = new string[] { "ThanTuanLong" };

    private int indextabchon = -1;

    void Start()
    {
        Transform g = transform.GetChild(0);
        vitriRong = g.transform.Find("objRong");
        menuChonRong.transform.SetParent(CrGame.ins.trencung.transform);
        panetoidan.transform.SetParent(CrGame.ins.trencung.transform);
        menuChonRong.transform.SetAsFirstSibling();
        for (int i = 0; i < allRong.Length; i++)
        {
            GameObject instan = Instantiate(ObjRongChon, transform.position, Quaternion.identity);
            Image img = instan.transform.GetChild(1).GetComponent<Image>();
            img.sprite = Inventory.LoadSpriteRong(allRong[i] + "2");
            img.SetNativeSize();
            instan.transform.SetParent(contentRongChon.transform, false);
            instan.transform.SetSiblingIndex(i);
            instan.gameObject.SetActive(true);
            instan.name = allRong[i];
            GamIns.ResizeItem(img, 140);
        }
        LoadTab(0);

    }

    public void ParseData(JSONNode data)
    {
        debug.Log(data.ToString());
        gameObject.SetActive(true);
        LoadItemYeuCau(data["itemyeucau"]);
        GameObject g = transform.GetChild(0).gameObject;
        g.transform.Find("txtvangyeucau").GetComponent<Text>().text = "Yêu cầu: " + data["vangyeucau"].AsString;
        dataLongKhi = data["data"]["LongKhi"];
        gioiHanLongKhi = data["gioihanlongkhithuthap"];
    }
    private void LoadItemYeuCau(JSONNode data)
    {
        GameObject item = ContentItemYeuCau.transform.GetChild(0).gameObject;

        for (int i = 1; i < ContentItemYeuCau.transform.childCount; i++)
        {
            Destroy(ContentItemYeuCau.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < data.Count; i++)
        {
            GameObject g = Instantiate(item, transform.position, Quaternion.identity);
            g.transform.SetParent(ContentItemYeuCau.transform, false);
            Image img = g.transform.GetChild(1).GetComponent<Image>();
            img.sprite = Inventory.LoadSprite(data[i]["nameitem"].AsString);
            img.SetNativeSize();
            double soluongyeucau = data[i]["soluong"].AsDouble;
            double soluongco = (Inventory.ins.ListItemThuong.ContainsKey("item" + data[i]["nameitem"].AsString)) ? double.Parse(Inventory.ins.ListItemThuong["item" + data[i]["nameitem"].AsString].transform.GetChild(0).GetComponent<Text>().text) : 0;
            g.transform.GetChild(2).GetComponent<Text>().text = (soluongco >= soluongyeucau) ? "<color=lime>" + GamIns.FormatCash(soluongco) + "/" + GamIns.FormatCash(soluongyeucau) + "</color>" : "<color=red>" + GamIns.FormatCash(soluongco) + "/" + GamIns.FormatCash(soluongyeucau) + "</color>";
            g.gameObject.SetActive(true);
            GamIns.ResizeItem(img, 100);


        }
    }

    public void ChonRongTab()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        int index = btnchon.transform.parent.transform.GetSiblingIndex();
        LoadTab(index);
    }
    private void LoadTab(int index)
    {
        if (indextabchon == index) return;
        string rongchon = allRong[index];
        AllMenu.ins.LoadRongGiaoDien(rongchon + "2", vitriRong, 1, false, "RongGiaoDien", new Vector3(95, 95));
        indextabchon = index;

    }
    public void ChonRongTienHoa()
    {
        GameObject contentRong = contentItemRong;
        for (int i = 0; i < contentRong.transform.childCount; i++)
        {
            if (contentRong.transform.GetChild(i).gameObject.activeSelf && contentRong.transform.GetChild(i).transform.childCount > 0)
            {
                Destroy(contentRong.transform.GetChild(i).gameObject);
            }

        }
        GameObject slot = ObjItemRong;
        bool corong = false;
        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;


                    if (itemdra.nameObjectDragon == "RongTuanLong" && itemdra.txtSao.text == "30") SetRong();


                    void SetRong()
                    {
                        corong = true;
                        GameObject rong = Instantiate(slot, transform.position, Quaternion.identity);
                        rong.transform.SetParent(contentRong.transform, false);
                        // ite
                        rong.name = itemdra.name;
                        Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                        imgRong.name = itemdra.name;
                        imgRong.sprite = Inventory.LoadSpriteRong(nameimg); imgRong.SetNativeSize();
                        rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                        rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                        // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                        rong.SetActive(true);
                        rong.transform.SetAsFirstSibling();
                        //for (int i = 0; i < idronglongap.Length; i++)
                        //{
                        //    if (idronglongap[i] == itemdra.name)
                        //    {
                        //        rong.transform.GetChild(0).GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                        //        break;
                        //    }
                        //}
                    }
                }
            }
        }
        GameObject g = menuChonRong.transform.GetChild(0).gameObject;
        g.transform.Find("Scroll View").gameObject.SetActive(corong);
        g.transform.Find("objkhongdudieukien").gameObject.SetActive(!corong);
        menuChonRong.gameObject.SetActive(true);

    }
    public void CloseMenu()
    {
        if(EventSystem.current.currentSelectedGameObject == transform.gameObject)
        {
            Destroy(menuChonRong);
            Destroy(panetoidan);
            AllMenu.ins.DestroyMenu("MenuTienHoaRong");
        }
      
    }
    public void XemChiSoRong()
    {
        GameObject objitem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string idrong = objitem.transform.parent.name;
        CrGame.ins.ChiSoRong(idrong);
    }
    private int thuthapduocmax;
    public void ChonRong()
    {
        GameObject btnchonrong = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string idrong = btnchonrong.transform.parent.name;

        if(menulongkhi.activeSelf)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "TienHoaRong";
            datasend["method"] = "ChonRongLongKhi";
            datasend["data"]["idrong"] = idrong;
            datasend["data"]["nametab"] = alltab[tablongkhi];
            datasend["data"]["nameobject"] = btnchonrong.name;
            datasend["data"]["sao"] = btnchonrong.transform.parent.transform.GetChild(2).name;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    debug.Log(json.ToString());

                    menuChonRong.gameObject.SetActive(false);
                    vitriRong = menulongkhi.transform.Find("daucong").transform.GetChild(0);
                    vitriRong.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                    AllMenu.ins.LoadRongGiaoDien(btnchonrong.name + 2, vitriRong, 1, false, "RongGiaoDien", new Vector3(75, 75));


                    Text txtthuthapduoc = menulongkhi.transform.Find("txtthuthapduoc").GetComponent<Text>();

                    //   JSONNode cothenhanduoc = LongKhiCoTheThuThap[alltab[tablongkhi]];
                    string s = (json["thuthapduoc"]["min"].AsString == json["thuthapduoc"]["max"].AsString) ? json["thuthapduoc"]["min"].AsString : json["thuthapduoc"]["min"].AsString + "-" + json["thuthapduoc"]["max"].AsString;
                    txtthuthapduoc.text = "Số " + allnamecolor[tablongkhi] + " sẽ nhận được khi hấp thụ Rồng này: <color=lime>" + s + "</color>";
                    thuthapduocmax = json["thuthapduoc"]["max"].AsInt;
                    btnhapthu.interactable = true;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
        else
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "TienHoaRong";
            datasend["method"] = "ChonRong";
            datasend["data"]["idrong"] = idrong;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    debug.Log(json.ToString());

                    menuChonRong.gameObject.SetActive(false);
                    Transform g = transform.GetChild(0);
                    vitriRong = g.transform.Find("daucong").transform.GetChild(0);
                    vitriRong.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                    AllMenu.ins.LoadRongGiaoDien("RongTuanLong2", vitriRong, 1, false, "RongGiaoDien", new Vector3(100, 100));

                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
       

     

    }
    public void TienHoa()
    {
        if (DownLoadAssetBundle.MenuBundle.ContainsKey("animtienhoa"))
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "TienHoaRong";
            datasend["method"] = "TienHoa";
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    debug.Log(json.ToString());
                    gameObject.SetActive(false);
                    duocexitpaneltoi = false;
                    for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                        {
                            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            if (itemdra.name == json["idrongtienhoa"].AsString)
                            {
                                itemdra.txtSao.text = (int.Parse(itemdra.txtSao.text) + 1).ToString();
                                Image img = itemdra.transform.GetChild(0).GetComponent<Image>();
                                img.sprite = Inventory.LoadSpriteRong(json["tienhoathanh"].AsString + "2");
                                img.SetNativeSize();
                                itemdra.nameObjectDragon = json["tienhoathanh"].AsString;
                                break;
                            }
                        }
                    }
                    Transform g = transform.GetChild(0);
                    vitriRong = g.transform.Find("daucong").transform.GetChild(0);
                    //GameObject RongChon = vitriRong.transform.GetChild(0).gameObject;
                    //RongChon.transform.SetParent(panetoidan.transform);
                    //RongChon.transform.LeanMove(Vector3.zero,1f);
                    //RongChon.transform.LeanScale(new Vector3(95f,95f),1f);
                    panetoidan.gameObject.SetActive(true);
                    GameObject GetRonggdGiaoDien = AllMenu.ins.GetRonggdGiaoDien("RongTuanLong2", panetoidan.transform, 1, false, "trencung", new Vector3(95, 95));
                    // SortingGroup group = RongChon.GetComponent<SortingGroup>();
                    // group.sortingLayerName = "trencung";
                    TienHoa tienhoa = GetRonggdGiaoDien.GetComponent<TienHoa>();
                    tienhoa.DoneAnimTienHoa = () =>
                    {
                        tienhoa.DoneAnimTienHoa = null;
                        // Debug.Log("done tien hoa tuan long");
                        GameObject Rongtienhoa = AllMenu.ins.GetRonggdGiaoDien("ThanTuanLong2", panetoidan.transform, 1, false, "trencung", new Vector3(95, 95));
                        Rongtienhoa.GetComponent<Animator>().Play("Evolution");
                        TienHoa tienhoa2 = Rongtienhoa.GetComponent<TienHoa>();
                        tienhoa2.DoneAnimTienHoa = () =>
                        {
                            //done
                            duocexitpaneltoi = true;
                            for (int i = 0; i < json["setitem"].Count; i++)
                            {
                                Inventory.ins.AddItem(json["setitem"][i]["nameitem"].AsString, json["setitem"][i]["soluong"].AsInt);
                            }
                            LoadItemYeuCau(json["itemyeucau"]);

                            for (int i = 0; i < vitriRong.transform.childCount; i++)
                            {
                                Destroy(vitriRong.transform.GetChild(i).gameObject);
                            }

                            vitriRong.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

                        };

                    };
                    EventManager.StartDelay2(() => {


                        GetRonggdGiaoDien.GetComponent<Animator>().Play("Evolution");
                        GameObject g = Instantiate(DownLoadAssetBundle.MenuBundle["animtienhoa"], transform.position, Quaternion.identity);
                        Destroy(g, 4f);
                        //g.transform.position = CrGame.ins.transform.position;

                    }, 1f);

                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }

        }
        else
        {
            CrGame.ins.OnThongBaoNhanh("Đang xử lý!");
        }
    }
    bool duocexitpaneltoi = false;
    public void ExitPanelToi()
    {
        if (!duocexitpaneltoi) return;
        for (int i = 0; i < panetoidan.transform.childCount; i++)
        {
            Destroy(panetoidan.transform.GetChild(i).gameObject);
        }
        gameObject.SetActive(true);
        panetoidan.SetActive(false);
    }
    private byte tablongkhi = 0;
    private JSONNode dataLongKhi;
    private JSONNode gioiHanLongKhi;
  //  private JSONNode LongKhiCoTheThuThap;
    [SerializeField] GameObject menulongkhi;

    private string[] alltab = new string[] {"BangLongKhi","QuangLongKhi"};
    private string[] allnamecolor = new string[] { "<color=cyan>Băng Long Khí</color>", "<color=yellow>Quang Long Khí</color>" };
    public void OpenMenuLongKhi()
    {
        tablongkhi = 0;
        LoadTabLongKhi();
        menulongkhi.SetActive(true);
        gameObject.transform.GetChild(0).transform.Find("objRong").gameObject.SetActive(false);
        Transform g = transform.GetChild(0);
        g.transform.Find("daucong").gameObject.SetActive(false);
    }    
    public void CloseMenuLongKhi()
    {
        if (danghapthu) return;
        gameObject.transform.GetChild(0).transform.Find("objRong").gameObject.SetActive(true);
        menulongkhi.SetActive(false);

        Transform g = transform.GetChild(0);
        g.transform.Find("daucong").gameObject.SetActive(true);

    }
    [SerializeField] Sprite ochon, ochuachon;
    [SerializeField] Button btnhapthu;
    public void ChonTabLongKhi()
    {
        if (danghapthu) return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        int index = btnchon.transform.parent.transform.GetSiblingIndex();
      
        if (index < alltab.Length)
        {
            btnchon.transform.parent.transform.parent.transform.GetChild(tablongkhi).GetChild(0).GetComponent<Image>().sprite = ochuachon;
            tablongkhi = (byte)index;
            LoadTabLongKhi();
            btnchon.transform.parent.transform.GetChild(0).GetComponent<Image>().sprite = ochon;
          
        }
    }

    private void LoadTabLongKhi()
    {
        int solongkhidathuthap = dataLongKhi[alltab[tablongkhi] + "ThuThap"].AsInt;
        Text txtlongkhi = menulongkhi.transform.Find("txtlongkhithuthap").GetComponent<Text>();
        int gioihan = gioiHanLongKhi[alltab[tablongkhi]].AsInt;
        string thuthap = (solongkhidathuthap >= gioihan) ? "<color=red>" + solongkhidathuthap + "/" + gioihan + "</color>" : "<color=lime>" + solongkhidathuthap + "/" + gioihan + "</color>";
        txtlongkhi.text = "Số "+ allnamecolor[tablongkhi] + " đã thu thập hôm nay: " + thuthap;
        Image imglongkhi = menulongkhi.transform.Find("imglongkhi").GetComponent<Image>();
        imglongkhi.sprite = Inventory.LoadSprite(alltab[tablongkhi]);
        imglongkhi.SetNativeSize();

        vitriRong = menulongkhi.transform.Find("daucong").transform.GetChild(0);
        vitriRong.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        for (int i = 0; i < vitriRong.transform.childCount; i++)
        {
            Destroy(vitriRong.transform.GetChild(i).gameObject);

        }
       Text txtlongkhii = menulongkhi.transform.Find("txtlongkhi").GetComponent<Text>();
        txtlongkhii.text = allnamecolor[tablongkhi];
        btnhapthu.interactable = false;
        Text txtthuthapduoc = menulongkhi.transform.Find("txtthuthapduoc").GetComponent<Text>();
        txtthuthapduoc.text = "";
    }
    public void ChonRongLongKhi()
    {
        if (danghapthu) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TienHoaRong";
        datasend["method"] = "GetRongChonLongKhi";
        datasend["data"]["nametab"] = alltab[tablongkhi];
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());

                GameObject contentRong = contentItemRong;
                for (int i = 0; i < contentRong.transform.childCount; i++)
                {
                    if (contentRong.transform.GetChild(i).gameObject.activeSelf && contentRong.transform.GetChild(i).transform.childCount > 0)
                    {
                        Destroy(contentRong.transform.GetChild(i).gameObject);
                    }
                }
                GameObject slot = ObjItemRong;
                bool corong = false;
                if (Inventory.ins.TuiRong.transform.childCount > 1)
                {
                    for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                        {
                            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;

                            if (json["allrong"][itemdra.nameObjectDragon].ToString() != "" && nameimg == itemdra.nameObjectDragon + 2)
                            {
                                byte saomin = json["allrong"][itemdra.nameObjectDragon]["saomin"].AsByte;
                                byte saomax = json["allrong"][itemdra.nameObjectDragon]["saomax"].AsByte;
                                byte saorong = byte.Parse(itemdra.txtSao.text);
                                if(saorong >= saomin && saorong <= saomax && itemdra.transform.GetChild(0))
                                {
                                    SetRong();
                                }
                            }
                            void SetRong()
                            {
                                corong = true;
                                GameObject rong = Instantiate(slot, transform.position, Quaternion.identity);
                                rong.transform.SetParent(contentRong.transform, false);
                                // ite
                                rong.name = itemdra.name;
                                Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                                imgRong.name = itemdra.name;
                                imgRong.sprite = Inventory.LoadSpriteRong(nameimg); imgRong.SetNativeSize();
                                rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                                rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                                // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                                rong.SetActive(true);
                                rong.transform.SetAsFirstSibling();
                                imgRong.name = itemdra.nameObjectDragon;
                                rong.transform.GetChild(2).name = itemdra.txtSao.text;
                                //for (int i = 0; i < idronglongap.Length; i++)
                                //{
                                //    if (idronglongap[i] == itemdra.name)
                                //    {
                                //        rong.transform.GetChild(0).GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                                //        break;
                                //    }
                                //}
                            }
                        }
                    }
                }
                GameObject g = menuChonRong.transform.GetChild(0).gameObject;
                g.transform.Find("Scroll View").gameObject.SetActive(corong);
                g.transform.Find("objkhongdudieukien").gameObject.SetActive(!corong);
                menuChonRong.gameObject.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    [SerializeField] GameObject animxong1;
    bool danghapthu = false;
    public void HapThu()
    {
        if (danghapthu) return;
        // if(thuthapduocmax)
        int gioihan = gioiHanLongKhi[alltab[tablongkhi]].AsInt;
        int solongkhidathuthap = dataLongKhi[alltab[tablongkhi] + "ThuThap"].AsInt;
        if (gioihan - solongkhidathuthap < thuthapduocmax)
        {
           
            if(solongkhidathuthap >= gioihan)
            {
                CrGame.ins.OnThongBaoNhanh("Bạn đã đạt giới hạn trong ngày, quay lại vào ngày mai nhé!",3);
                return;
            }    
            EventManager.OpenThongBaoChon("Hôm nay bạn đã thu thập <color=lime>" + solongkhidathuthap + "/" + gioihan + "</color> "+ allnamecolor[tablongkhi] + ", lần hấp thụ này sẽ chỉ nhận được giới hạn "+(gioihan-solongkhidathuthap) +" long khí nữa", Okk);
           
        }    
       else
        {
            EventManager.OpenThongBaoChon("Bạn có chắc chắn Hấp Thụ Rồng này?", Okk);

        }
        void Okk()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "TienHoaRong";
            datasend["method"] = "HapThuRong";
            datasend["data"]["nametab"] = alltab[tablongkhi];
            // datasend["data"][""] = "";
            NetworkManager.ins.SendServer(datasend, Ok, true);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {

                    danghapthu = true;
                    debug.Log(json.ToString());
                    btnhapthu.interactable = false;

                    animxong1.transform.GetChild(0).gameObject.SetActive(false);
                    animxong1.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                    StartCoroutine(delayanimXong());
                    IEnumerator delayanimXong()
                    {
                        animxong1.gameObject.SetActive(true);
                        yield return new WaitForSeconds(3f);
                        animxong1.gameObject.SetActive(false);

                        //GameObject quabay = Instantiate(displayImage.gameObject, transform.position, Quaternion.identity);

                        vitriRong = menulongkhi.transform.Find("daucong").transform.GetChild(0);
                        vitriRong.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        for (int i = 0; i < vitriRong.transform.childCount; i++)
                        {
                            Destroy(vitriRong.transform.GetChild(i).gameObject);

                        }

                        Text txtthuthapduoc = menulongkhi.transform.Find("txtthuthapduoc").GetComponent<Text>();
                        txtthuthapduoc.text = "";

                        EventManager.OpenMenuNhanDuocItem(json["nameitem"].AsString, "x" + json["soluong"].AsString, LoaiItem.item);
                        danghapthu = false;

                        dataLongKhi[alltab[tablongkhi] + "ThuThap"] = json["soluongdathuthap"];
                        int solongkhidathuthap = dataLongKhi[alltab[tablongkhi] + "ThuThap"].AsInt;
                        Text txtlongkhi = menulongkhi.transform.Find("txtlongkhithuthap").GetComponent<Text>();
                        int gioihan = gioiHanLongKhi[alltab[tablongkhi]].AsInt;
                        string thuthap = (solongkhidathuthap >= gioihan) ? "<color=red>" + solongkhidathuthap + "/" + gioihan + "</color>" : "<color=lime>" + solongkhidathuthap + "/" + gioihan + "</color>";
                        txtlongkhi.text = "Số " + allnamecolor[tablongkhi] + " đã thu thập hôm nay: " + thuthap;
                        if (Inventory.ins.TuiRong.transform.childCount > 1)
                        {
                            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                            {
                                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                                {
                                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                                    if (itemdra.name == json["idrong"].AsString)
                                    {
                                        Destroy(itemdra.gameObject);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
    

    }
}

