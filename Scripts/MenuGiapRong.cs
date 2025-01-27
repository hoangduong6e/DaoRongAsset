using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuGiapRong : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] giaoDien;
    private string nameTab = "MatXanh";
    GameObject RongChon;
    public GameObject 
        GdRenGiap, 
        GdThongtinren,
        hieuungren,
        GdNangCapGiap;
    void Start()
    {
        GameObject trencung = GameObject.FindGameObjectWithTag("trencung").gameObject;
        GdRenGiap.transform.SetParent(trencung.transform);
        GdRenGiap.transform.SetAsFirstSibling();
        GdThongtinren.transform.SetParent(trencung.transform);
        GdThongtinren.transform.SetAsFirstSibling();
        hieuungren.transform.SetParent(trencung.transform);
        hieuungren.transform.SetAsFirstSibling();
        GdNangCapGiap.transform.SetParent(trencung.transform);
        GdNangCapGiap.transform.SetAsFirstSibling();
        LoadVatPhamYc();
        LoadItemGiap();
        LoadGd1();
        Invoke("LoadRongStart",0.2f);
    }
    void LoadRongStart()
    {
        SetRongGd(new string[] { "RongLuaMatXanh+GiapKimLoai", "RongLuaMatXanh+GiapDo", "RongLuaMatXanh+GiapNgocXanh" },"");
    }    
    public void Close()
    {
        Destroy(GdRenGiap);
        Destroy(GdThongtinren);
        Destroy(hieuungren);
        Destroy(GdNangCapGiap);
        AllMenu.ins.DestroyMenu("MenuGiapRong");
    }
    public void OpenTab(string tab)
    {
        //if(tab == "MaTroi")
        //{
        //    CrGame.ins.OnThongBaoNhanh("Chưa mở");
        //    return;
        //}
        if (tab == nameTab) return;
        nameTab = tab;

        GameObject giaodienren = transform.GetChild(0).transform.Find("giaodienRen").gameObject;
        GameObject objrong = giaodienren.transform.Find("objRong").gameObject;
        transform.GetChild(0).transform.Find("btnMatXanh").GetComponent<Image>().color = new Color32(255, 255, 255, 140);
        transform.GetChild(0).transform.Find("btnMatXanhSapphire").GetComponent<Image>().color = new Color32(255, 255, 255, 140);
        transform.GetChild(0).transform.Find("btnMaTroi").GetComponent<Image>().color = new Color32(255, 255, 255, 140);
        transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khunggiap").transform.GetChild(0).gameObject.SetActive(false);
        //    GameObject orong = transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0).gameObject;
        //orong.SetActive(false);
        ClearRongCHon();
        RongChon = null;
        if (transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite.name == "gd2")
        {
            switch (tab)
            {
                case "MatXanh":
                    transform.GetChild(0).transform.Find("btnMatXanh").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    break;
                case "Sapphire":
                    transform.GetChild(0).transform.Find("btnMatXanhSapphire").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    break;
                case "MaTroi":
                    transform.GetChild(0).transform.Find("btnMaTroi").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    break;
                case "KyLan":
                    transform.GetChild(0).transform.Find("btnKyLan").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    break;

            }
            LoadItemRong();
        }
        else
        {
            string[] allrong = null;
            switch (tab)
            {
                case "MatXanh":
                    transform.GetChild(0).transform.Find("btnMatXanh").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    allrong = new string[] { "RongLuaMatXanh+GiapKimLoai", "RongLuaMatXanh+GiapDo", "RongLuaMatXanh+GiapNgocXanh" };
                    break;
                case "Sapphire":
                    transform.GetChild(0).transform.Find("btnMatXanhSapphire").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    allrong = new string[] { "RongLuaMatXanhSapphire+GiapKimLoai", "RongLuaMatXanhSapphire+GiapDo", "RongLuaMatXanhSapphire+GiapNgocXanh" };
                    break;
                case "MaTroi":
                    transform.GetChild(0).transform.Find("btnMaTroi").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    allrong = new string[] { "RongMaTroi+GiapKimLoai", "RongMaTroi+GiapDo", "RongMaTroi+GiapNgocXanh" };
                    break;
                case "KyLan":
                    transform.GetChild(0).transform.Find("btnKyLan").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    allrong = new string[] { "", "", "RongKyLanTim+GiapKyLan" };
                    break;
            }
            //for (int i = 0; i < 3; i++)
            //{
            //    objrong.transform.GetChild(i).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(allrong[i]);
            //}
            SetRongGd(allrong, tab);
        }
     
    }    
    private void SetRongGd(string[] namerong,string nametab)
    {
        GameObject giaodienren = transform.GetChild(0).transform.Find("giaodienRen").gameObject;
        GameObject objrong = giaodienren.transform.Find("objRong").gameObject;

        for (int i = 0; i < 3; i++)
        {
            Transform G = objrong.transform.GetChild(i).transform.GetChild(1);
            if (namerong[i] != "")
            {
                AllMenu.ins.LoadRongGiaoDien(namerong[i] + "2",G);
            }
            else
            {
                for(int j = 0; j < G.transform.childCount;j++)
                {
                    Destroy(G.transform.GetChild(j).gameObject);
                }
            }
            if(nametab == "KyLan")
            {
                objrong.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            }
            else objrong.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
            // objrong.transform.GetChild(i).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(allrong[i]);
        }
    }    
    private void LoadVatPhamYc()
    {
        GameObject vatpham = transform.GetChild(0).Find("vatpham").gameObject;
        string[] allitem = {"DaThuoc","ManhThep", "Adamantium","Uru"};
        for (int i = 0; i < allitem.Length; i++)
        {
            if(Inventory.ins.ListItemThuong.ContainsKey("item" +allitem[i]))
            {
                vatpham.transform.GetChild(i).GetComponent<Text>().text = Inventory.ins.ListItemThuong["item" + allitem[i]].transform.GetChild(0).GetComponent<Text>().text;
            }
        }
    }    
    private void LoadItemGiap()
    {
        string[] allgiap = {"GiapDo-","GiapDo-CuongHoa","GiapKimLoai-","GiapKimLoai-CuongHoa","GiapNgocXanh-","GiapNgocXanh-CuongHoa", "GiapKyLan-", "GiapKyLan-CuongHoa" };
        GameObject content = transform.GetChild(0).transform.Find("giaodienRen").transform.Find("ScrollView").transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject item = content.transform.GetChild(0).gameObject;
    
        GameObject giaodien2 = transform.GetChild(0).transform.Find("giaodien2").gameObject;
        GameObject contentgiap = giaodien2.transform.Find("ScrollViewGiap").transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject itemgiap = contentgiap.transform.GetChild(0).gameObject;

        for (int i = 0; i < allgiap.Length; i++)
        {
            if (Inventory.ins.ListItemThuong.ContainsKey("item" + allgiap[i]))
            {
                int soluong = int.Parse(Inventory.ins.ListItemThuong["item" + allgiap[i]].transform.GetChild(0).GetComponent<Text>().text);
                for (int j = 0; j < soluong; j++)
                {
                    Sprite sp = Inventory.LoadSprite(allgiap[i]); 
                    GameObject tem = Instantiate(item, transform.position, Quaternion.identity);
                    tem.transform.GetChild(0).GetComponent<Image>().sprite = sp;
                    tem.transform.SetParent(content.transform, false);
                    tem.SetActive(true);
                    tem.name = allgiap[i];

                    GameObject tem2 = Instantiate(itemgiap, transform.position, Quaternion.identity);
                    tem2.transform.GetChild(0).GetComponent<Image>().sprite = sp;
                    tem2.transform.SetParent(contentgiap.transform, false);
                    tem2.SetActive(true);
                    tem2.name = allgiap[i];
                }
            }
        }
    }    
    public void LoadGd2()
    {
        transform.GetChild(0).transform.Find("btngd1").GetComponent<Image>().color = new Color32(255, 255, 255, 140);
        transform.GetChild(0).transform.Find("btngd2").GetComponent<Image>().color = new Color32(255,255,255,255);
        transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = giaoDien[1];
        transform.GetChild(0).transform.Find("giaodienRen").gameObject.SetActive(false);
        GameObject giaodien2 = transform.GetChild(0).transform.Find("giaodien2").gameObject;
        giaodien2.SetActive(true);
        LoadItemRong();
    }
    public void LoadGd1()
    {
        transform.GetChild(0).transform.Find("btngd1").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        transform.GetChild(0).transform.Find("btngd2").GetComponent<Image>().color = new Color32(255, 255, 255, 140);
        transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = giaoDien[0];
        transform.GetChild(0).transform.Find("giaodien2").gameObject.SetActive(false);
        GameObject giaodien1 = transform.GetChild(0).transform.Find("giaodienRen").gameObject;
        giaodien1.SetActive(true);
    }
  
    public void ChonRong()
    {
        if(NetworkManager.isSend)
        {
            RongChon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            JSONClass datasend = new JSONClass();
            datasend["class"] = "GiapRong";
            datasend["method"] = "ChonRong";
            datasend["data"]["idrong"] = RongChon.transform.parent.name;
            NetworkManager.ins.SendServer(datasend, ChonRongOk);
        }
        void ChonRongOk(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                string nameobjrong = RongChon.name;
               // Animator animrongchon = transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0).GetComponent<Animator>();
                AllMenu.ins.LoadRongGiaoDien(nameobjrong + "2", transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0));
                // animrongchon.runtimeAnimatorController = Inventory.LoadAnimator(nameobjrong);
                // animrongchon.gameObject.name = RongChon.transform.parent.name;
                // animrongchon.gameObject.SetActive(true);
                //  animrongchon.SetInteger("TienHoa", 2);
                if (json["giaprong"].AsString != "")
                {
                    Image imggiap = transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khunggiap").transform.GetChild(0).GetComponent<Image>();
                    imggiap.sprite = Inventory.LoadSprite(json["giaprong"].AsString);
                    imggiap.gameObject.SetActive(true);
                    transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("imgthaogiap").gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khunggiap").transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("imgthaogiap").gameObject.SetActive(false);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }
    public void ChonGiap()
    {
        if(RongChon == null)
        {
            CrGame.ins.OnThongBaoNhanh("Bạn chưa chọn rồng!");
            return;
        }    
        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "MacGiap";
        datasend["data"]["idrong"] = RongChon.transform.parent.name;
        datasend["data"]["giap"] = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        NetworkManager.ins.SendServer(datasend, ChonGiapok);
        void ChonGiapok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                hieuungren.SetActive(true);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(1.7f);
                    hieuungren.SetActive(false);
                    for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                        {
                            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            if (itemdra.name == json["id"].AsString)
                            {
                                Image imgdra = itemdra.transform.GetChild(0).GetComponent<Image>();
                                imgdra.sprite = Inventory.LoadSpriteRong(json["nameobject"].AsString + "2");
                                itemdra.nameObjectDragon = json["nameobject"].AsString;
                                imgdra.SetNativeSize();
                                break;
                            }
                        }
                    }
                    GameObject giaodien2 = transform.GetChild(0).transform.Find("giaodien2").gameObject;
                    GameObject contentgiap = giaodien2.transform.Find("ScrollViewGiap").transform.GetChild(0).transform.GetChild(0).gameObject;
                    Destroy(contentgiap.transform.Find(json["giaprong"].AsString).gameObject);
                    Inventory.ins.AddItem(json["giaprong"].AsString, -1);
                    Image imggiap = transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khunggiap").transform.GetChild(0).GetComponent<Image>();
                    imggiap.sprite = Inventory.LoadSprite(json["giaprong"].AsString);
                    imggiap.gameObject.SetActive(true);

                    RongChon.name = json["nameobject"].AsString;
                    AllMenu.ins.LoadRongGiaoDien(RongChon.name + "2", transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0));
                  //  transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(json["nameobject"].AsString);
                    RongChon.GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["nameobject"].AsString + "2");
                    transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("imgthaogiap").gameObject.SetActive(true);
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }   
    public void ThaoGiap()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "ThaoGiap";
        datasend["data"]["idrong"] = RongChon.transform.parent.name;
        NetworkManager.ins.SendServer(datasend, ThaoGiapOk);
        void ThaoGiapOk(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {

                hieuungren.SetActive(true);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(1.7f);
                    hieuungren.SetActive(false);
                    for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                        {
                            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            if (itemdra.name == json["id"].AsString)
                            {
                                Image imgdra = itemdra.transform.GetChild(0).GetComponent<Image>();
                                imgdra.sprite = Inventory.LoadSpriteRong(json["nameobject"].AsString + "2");
                                itemdra.nameObjectDragon = json["nameobject"].AsString;
                                imgdra.SetNativeSize();
                                break;
                            }
                        }
                    }
                    GameObject content = transform.GetChild(0).transform.Find("giaodienRen").transform.Find("ScrollView").transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject item = content.transform.GetChild(0).gameObject;

                    GameObject tem = Instantiate(item, transform.position, Quaternion.identity);
                    tem.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(json["giaprong"].AsString);
                    tem.transform.SetParent(content.transform, false);
                    tem.SetActive(true);
                    tem.name = json["giaprong"].AsString;

                    GameObject giaodien2 = transform.GetChild(0).transform.Find("giaodien2").gameObject;
                    GameObject contentgiap = giaodien2.transform.Find("ScrollViewGiap").transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject itemgiap = contentgiap.transform.GetChild(0).gameObject;
                    GameObject tem2 = Instantiate(itemgiap, transform.position, Quaternion.identity);
                    tem2.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(json["giaprong"].AsString);
                    tem2.transform.SetParent(contentgiap.transform, false);
                    tem2.SetActive(true);
                    tem2.name = json["giaprong"].AsString;
                    Inventory.ins.AddItem(json["giaprong"].AsString, 1);

                    transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khunggiap").transform.GetChild(0).gameObject.SetActive(false);

                    RongChon.name = json["nameobject"].AsString;
                    //  Animator anim = transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0).GetComponent<Animator>();
                    // anim.runtimeAnimatorController = Inventory.LoadAnimator(json["nameobject"].AsString);
                    //anim.SetInteger("TienHoa", 2);
                    AllMenu.ins.LoadRongGiaoDien(RongChon.name + "2", transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0));
                    RongChon.GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["nameobject"].AsString + "2");
                    transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("imgthaogiap").gameObject.SetActive(false);


                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }
    public void XemCs()
    {
        CrGame.ins.ChiSoRong(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name);
    }
   
    public void XnRen(string namegiap)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "RenGiap";
        datasend["data"]["namegiap"] = namegiap;
        NetworkManager.ins.SendServer(datasend, Renok);
        void Renok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GdRenGiap.SetActive(false);
                hieuungren.SetActive(true);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(1.7f);
                    hieuungren.SetActive(false);

                    GameObject content = transform.GetChild(0).transform.Find("giaodienRen").transform.Find("ScrollView").transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject item = content.transform.GetChild(0).gameObject;

                    GameObject giaodien2 = transform.GetChild(0).transform.Find("giaodien2").gameObject;
                    GameObject contentgiap = giaodien2.transform.Find("ScrollViewGiap").transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject itemgiap = contentgiap.transform.GetChild(0).gameObject;

                    for (int i = 0; i < json["allitem"].Count; i++)
                    {
                        Inventory.ins.AddItem(json["allitem"][i]["name"].AsString, -json["allitem"][i]["soluong"].AsInt);
                        if (json["allitem"][i]["name"].AsString.Contains("Giap"))
                        {
                            for (int j = 0; j < json["allitem"][i]["soluong"].AsInt; j++)
                            {
                                if (content.transform.Find(json["allitem"][i]["name"].AsString))
                                {
                                    Destroy(content.transform.Find(json["allitem"][i]["name"].AsString).gameObject);
                                }
                                if (contentgiap.transform.Find(json["allitem"][i]["name"].AsString))
                                {
                                    Destroy(contentgiap.transform.Find(json["allitem"][i]["name"].AsString).gameObject);
                                }
                            }
                        }
                    }
                    Inventory.ins.AddItem(json["namegiap"].AsString, 1);

                    Sprite sp = Inventory.LoadSprite(json["namegiap"].AsString);
                    GameObject tem = Instantiate(item, transform.position, Quaternion.identity);
                    tem.transform.GetChild(0).GetComponent<Image>().sprite = sp;
                    tem.transform.SetParent(content.transform, false);
                    tem.SetActive(true);
                    tem.name = json["namegiap"].AsString;

                    GameObject tem2 = Instantiate(itemgiap, transform.position, Quaternion.identity);
                    tem2.transform.GetChild(0).GetComponent<Image>().sprite = sp;
                    tem2.transform.SetParent(contentgiap.transform, false);
                    tem2.SetActive(true);
                    tem2.name = json["namegiap"].AsString;
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }
 
    public void RenGiap()
    {
        string namegiap = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "GetInfoRenGiap";
        datasend["data"]["namegiap"] = namegiap;
        Button btnxn = GdRenGiap.transform.Find("btnxacnhan").GetComponent<Button>();
        btnxn.onClick.RemoveAllListeners();
        btnxn.onClick.AddListener(delegate { XnRen(namegiap); });
        NetworkManager.ins.SendServer(datasend, InfoRenGiap);
        void InfoRenGiap(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                Button btnxn = GdRenGiap.transform.Find("btnxacnhan").GetComponent<Button>();
                btnxn.interactable = true;
                GameObject allitem = GdRenGiap.transform.Find("allitem").gameObject;
                for (int i = 0; i < allitem.transform.childCount; i++)
                {
                    allitem.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < json["data"]["allitem"].Count; i++)
                {
                    if (allitem.transform.Find(json["data"]["allitem"][i]["name"].AsString))
                    {
                        string s = "";
                        if (Inventory.ins.ListItemThuong.ContainsKey("item" + json["data"]["allitem"][i]["name"].AsString) && json["data"]["allitem"][i]["name"].AsString.Contains("ngoc") == false)
                        {
                            if (int.Parse(Inventory.ins.ListItemThuong["item" + json["data"]["allitem"][i]["name"].AsString].transform.GetChild(0).GetComponent<Text>().text) >= json["data"]["allitem"][i]["soluong"].AsInt)
                            {
                                s = "<color=lime>" + Inventory.ins.ListItemThuong["item" + json["data"]["allitem"][i]["name"].AsString].transform.GetChild(0).GetComponent<Text>().text + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                            }
                            else
                            {
                                s = "<color=red>" + Inventory.ins.ListItemThuong["item" + json["data"]["allitem"][i]["name"].AsString].transform.GetChild(0).GetComponent<Text>().text + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                                btnxn.interactable = false;
                            }
                            // Inventory.ins.contentNgoc
                        }
                        else if (json["data"]["allitem"][i]["name"].AsString.Contains("ngoc"))
                        {
                            debug.Log("item ngoc");
                            for (int j = 0; j < Inventory.ins.contentNgoc.transform.childCount; j++)
                            {
                                if (Inventory.ins.contentNgoc.transform.GetChild(j).gameObject.name == json["data"]["allitem"][i]["name"].AsString)
                                {
                                    string txt = Inventory.ins.contentNgoc.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                                    if (int.Parse(txt) >= json["data"]["allitem"][i]["soluong"].AsInt)
                                    {
                                        s = "<color=lime>" + txt + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                                    }
                                    else
                                    {
                                        s = "<color=red>" + txt + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                                        btnxn.interactable = false;
                                    }
                                    break;
                                }
                                else if (j == Inventory.ins.contentNgoc.transform.childCount - 1)
                                {
                                    s = "<color=red>0/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                                    btnxn.interactable = false;
                                }
                            }
                        }
                        else
                        {
                            s = "<color=red>0/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                            btnxn.interactable = false;
                        }
                        GameObject item = allitem.transform.Find(json["data"]["allitem"][i]["name"].AsString).gameObject;
                        item.transform.GetChild(1).GetComponent<Text>().text = s;
                        item.SetActive(true);
                    }
                }
                Text txtkc = GdRenGiap.transform.Find("btnxacnhan").transform.GetChild(1).GetComponent<Text>();
                Text txtcomuon = GdRenGiap.transform.Find("txtbancomuon").GetComponent<Text>();
                txtkc.text = json["data"]["kimcuong"].AsString;
                txtcomuon.text = "Bạn có muốn rèn <color=yellow>" + json["data"]["namegiap"].AsString + "</color> hay không?";

                GdRenGiap.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }    
  
    public void InfoRenGiap(string s)
    {
        if (s.Split('+')[0] == "false")
        {
            GdThongtinren.SetActive(false);
            return;
        }
        debug.Log("ok1");
        string namegiap = s.Split('+')[1];
        debug.Log("ok2");

        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "GetInfoRenGiap";
        datasend["data"]["namegiap"] = namegiap;
        debug.Log("ok3");
        GameObject giaodien1 = transform.GetChild(0).transform.Find("giaodienRen").gameObject;
        GameObject bt = giaodien1.transform.Find("objRong").transform.Find(namegiap).gameObject;
        GdThongtinren.transform.position = new Vector3(bt.transform.position.x - 2f, GdThongtinren.transform.position.y, GdThongtinren.transform.position.z);
        NetworkManager.ins.SendServer(datasend, InfoRenGiapok);
        void InfoRenGiapok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject allitem = GdThongtinren.transform.Find("allitem").gameObject;
                for (int i = 0; i < allitem.transform.childCount; i++)
                {
                    allitem.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < json["data"]["allitem"].Count; i++)
                {
                    if (allitem.transform.Find(json["data"]["allitem"][i]["name"].AsString))
                    {
                        string s = "";
                        if (Inventory.ins.ListItemThuong.ContainsKey("item" + json["data"]["allitem"][i]["name"].AsString) && json["data"]["allitem"][i]["name"].AsString.Contains("ngoc") == false)
                        {
                            if (int.Parse(Inventory.ins.ListItemThuong["item" + json["data"]["allitem"][i]["name"].AsString].transform.GetChild(0).GetComponent<Text>().text) >= json["data"]["allitem"][i]["soluong"].AsInt)
                            {
                                s = "<color=lime>" + Inventory.ins.ListItemThuong["item" + json["data"]["allitem"][i]["name"].AsString].transform.GetChild(0).GetComponent<Text>().text + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                            }
                            else
                            {
                                s = "<color=red>" + Inventory.ins.ListItemThuong["item" + json["data"]["allitem"][i]["name"].AsString].transform.GetChild(0).GetComponent<Text>().text + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";

                            }
                            // Inventory.ins.contentNgoc
                        }
                        else if (json["data"]["allitem"][i]["name"].AsString.Contains("ngoc"))
                        {
                            debug.Log("item ngoc");
                            for (int j = 0; j < Inventory.ins.contentNgoc.transform.childCount; j++)
                            {
                                if (Inventory.ins.contentNgoc.transform.GetChild(j).gameObject.name == json["data"]["allitem"][i]["name"].AsString)
                                {
                                    string txt = Inventory.ins.contentNgoc.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                                    if (int.Parse(txt) >= json["data"]["allitem"][i]["soluong"].AsInt)
                                    {
                                        s = "<color=lime>" + txt + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                                    }
                                    else
                                    {
                                        s = "<color=red>" + txt + "/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                                    }
                                    break;
                                }
                                else if (j == Inventory.ins.contentNgoc.transform.childCount - 1)
                                {
                                    s = "<color=red>0/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                                }
                            }
                        }
                        else
                        {
                            s = "<color=red>0/" + json["data"]["allitem"][i]["soluong"].AsString + "</color>";
                        }
                        GameObject item = allitem.transform.Find(json["data"]["allitem"][i]["name"].AsString).gameObject;
                        item.transform.GetChild(1).GetComponent<Text>().text = s;
                        item.SetActive(true);
                    }
                }
                GdThongtinren.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }    
    private void ClearRongCHon()
    {
        GameObject orong = transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0).gameObject;
        for (int i = 0; i < orong.transform.childCount; i++)
        {
            Destroy(orong.transform.GetChild(i).gameObject);
        }
    }    
    public void CatRong()
    {
        // GameObject orong = transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khungrong").transform.GetChild(0).gameObject;
        // orong.SetActive(false);
        ClearRongCHon();
        transform.GetChild(0).transform.transform.Find("giaodien2").transform.Find("khunggiap").transform.GetChild(0).gameObject.SetActive(false);
        RongChon = null;
    }
    void LoadItemRong()
    {
        string[] allrong = { };
        switch(nameTab)
        {
            case "MatXanh":
                allrong = new string[] { "RongLuaMatXanh", "RongLuaMatXanh+GiapKimLoai", "RongLuaMatXanh+GiapDo", "RongLuaMatXanh+GiapNgocXanh", 
                    "RongLuaMatXanhGiapThin", "RongLuaMatXanhGiapThin+GiapKimLoai", "RongLuaMatXanhGiapThin+GiapDo", "RongLuaMatXanhGiapThin+GiapNgocXanh" };
                break;
            case "Sapphire":
                transform.GetChild(0).transform.Find("btnMatXanhSapphire").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                allrong = new string[] { "RongLuaMatXanhSapphire", "RongLuaMatXanhSapphire+GiapKimLoai", "RongLuaMatXanhSapphire+GiapDo", "RongLuaMatXanhSapphire+GiapNgocXanh" ,
                     "RongLuaMatXanhSapphireGiapThin", "RongLuaMatXanhSapphireGiapThin+GiapKimLoai", "RongLuaMatXanhSapphireGiapThin+GiapDo", "RongLuaMatXanhSapphireGiapThin+GiapNgocXanh"
                };
                break;
            case "MaTroi":
                allrong = new string[] { "RongMaTroi", "RongMaTroi+GiapKimLoai", "RongMaTroi+GiapDo", "RongMaTroi+GiapNgocXanh",
                     "RongMaTroiGiapThin", "RongMaTroiGiapThin+GiapKimLoai", "RongMaTroiGiapThin+GiapDo", "RongMaTroiGiapThin+GiapNgocXanh"
                };
                break;
            case "KyLan":
                allrong = new string[] { "RongKyLanTim","RongKyLanTim+GiapKyLan"
                };
                break;
        }
        GameObject giaodien2 = transform.GetChild(0).transform.Find("giaodien2").gameObject;
        GameObject content = giaodien2.transform.Find("ScrollView").transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 1; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        GameObject item = content.transform.GetChild(0).gameObject;
        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                    for (int j = 0; j < allrong.Length; j++)
                    {
                        if (itemdra.nameObjectDragon == allrong[j] && nameimg == allrong[j].Replace("GiapThin","") + "2")
                        {
                            GameObject tem = Instantiate(item, transform.position, Quaternion.identity);
                            tem.transform.SetParent(content.transform, false);
                            Image img = tem.transform.GetChild(1).GetComponent<Image>();
                            img.sprite = itemdra.transform.GetChild(0).GetComponent<Image>().sprite;
                            tem.transform.GetChild(3).GetComponent<Text>().text = itemdra.txtSao.text;
                            tem.SetActive(true);
                            tem.name = itemdra.name;
                            img.gameObject.name = itemdra.nameObjectDragon;
                            break;
                        }
                    }
                }
            }
        }
    }
    public void ChonGiapgd1()
    {
        string namegiap = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        GameObject giaodien1 = transform.GetChild(0).transform.Find("giaodienRen").gameObject;
        Image imggiap = giaodien1.transform.Find("imggiap").GetComponent<Image>();
        imggiap.sprite = Inventory.LoadSprite(namegiap);
        if(namegiap.Split('-')[1] != "CuongHoa")
        {
            imggiap.transform.GetChild(0).gameObject.SetActive(true);
            Button btn = imggiap.transform.GetChild(0).GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { XemNangCapGiap(namegiap); });
        }
        else imggiap.transform.GetChild(0).gameObject.SetActive(false);
        imggiap.gameObject.SetActive(true);
    }    
    public void XemNangCapGiap(string namegiap)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "XemGiaGiap";
        datasend["data"]["namegiap"] = namegiap;
        NetworkManager.ins.SendServer(datasend, XemNangCapGiapOk);
        void XemNangCapGiapOk(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GdNangCapGiap.SetActive(true);
                GdNangCapGiap.transform.GetChild(1).gameObject.SetActive(true);
                Image imggiap = GdNangCapGiap.transform.GetChild(3).GetComponent<Image>();
                imggiap.sprite = Inventory.LoadSprite(json["namegiap"].AsString);
                Text txtkc = GdNangCapGiap.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
                txtkc.text = json["giakc"].AsString;
                Button btn = GdNangCapGiap.transform.GetChild(1).GetComponent<Button>();
                btn.onClick.AddListener(delegate { NangCapGiap(json["namegiap"].AsString); });
                GdNangCapGiap.transform.GetChild(0).GetComponent<Text>().text = "Bạn có muốn nâng cấp <color=yellow>" + json["data"]["name"].AsString + "</color> hay không?"; ;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }
    void NangCapGiap(string namegiap)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "NangCapGiap";
        datasend["data"]["namegiap"] = namegiap;
        NetworkManager.ins.SendServer(datasend, NangCapGiapok);
        void NangCapGiapok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GdNangCapGiap.SetActive(false);
                hieuungren.SetActive(true);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(1.7f);
                    hieuungren.SetActive(false);
                    Inventory.ins.AddItem(json["namegiap"].AsString, -1);
                    Inventory.ins.AddItem(json["namegiap"].AsString + "CuongHoa", 1);
                    GameObject content = transform.GetChild(0).transform.Find("giaodienRen").transform.Find("ScrollView").transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject item = content.transform.GetChild(0).gameObject;

                    GameObject giaodien2 = transform.GetChild(0).transform.Find("giaodien2").gameObject;
                    GameObject contentgiap = giaodien2.transform.Find("ScrollViewGiap").transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject itemgiap = contentgiap.transform.GetChild(0).gameObject;

                    Destroy(content.transform.Find(json["namegiap"].AsString).gameObject);
                    Destroy(contentgiap.transform.Find(json["namegiap"].AsString).gameObject);

                    Sprite sp = Inventory.LoadSprite(json["namegiap"].AsString + "CuongHoa");
                    GameObject tem = Instantiate(item, transform.position, Quaternion.identity);
                    tem.transform.GetChild(0).GetComponent<Image>().sprite = sp;
                    tem.transform.SetParent(content.transform, false);
                    tem.SetActive(true);
                    tem.name = json["namegiap"].AsString + "CuongHoa";

                    GameObject tem2 = Instantiate(itemgiap, transform.position, Quaternion.identity);
                    tem2.transform.GetChild(0).GetComponent<Image>().sprite = sp;
                    tem2.transform.SetParent(contentgiap.transform, false);
                    tem2.SetActive(true);
                    tem2.name = json["namegiap"].AsString + "CuongHoa";
                    GameObject giaodien1 = transform.GetChild(0).transform.Find("giaodienRen").gameObject;
                    giaodien1.transform.Find("imggiap").gameObject.SetActive(false);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            }
        }
    }
  
    bool xemgiap = false;
    short length = 0;
    public void XemInfoGiap(bool enab)
    {
        xemgiap = enab;
        if (!enab)
        {
            // AllMenu.ins.menu["infoitem"].SetActive(false);
            CrGame.ins.OffThongBaoNhanh(length);
            return;
        } 
            
        string nameitem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "XemShop";
        datasend["data"]["nameitem"] = nameitem;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            CrGame.ins.OnThongBaoNhanh(json["thongtin"].AsString);
            length = (short)json["thongtin"].AsString.Length;
        }
    }
}
