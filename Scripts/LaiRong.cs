using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class LaiRong : MonoBehaviour
{
    public GameObject Slot, MenuSlot, OcotheLaira;
    public GameObject[] txtDuarong;
    public bool[] Chonlai = new bool[] { false, false };
    public GameObject[] Rongchon = new GameObject[2];
    public string[] HeRongLai = new string[2];
    public string[] GenRong = new string[2]; 
    public GameObject hieuunglai, objectchualai, banRongCatRong, btnExit,ItemRongLai,ItemCoTheLaiRa; public Button btnlai;
    public GameObject SaoRong; public byte Saoronglai; public Text txtTenRong;
    public Button[] nutcatlai;public string[] idronglai = new string[2];
    public string[] SaoRongBoMe = new string[2];public Image imgitemGiaLai;public Text txtSoitemCo,txtChiSo; public string[] Nameobjectronglai = new string[2];
    public GameObject HieuUngChucPhuc,btnChucPhuc;
    public GameObject contentChiSo;public string idrongvualai;
    // Start is called before the first frame update
    void Awake()
    {
        txtSoitemCo = imgitemGiaLai.transform.GetChild(0).GetComponent<Text>();
        //JSONNode json = JSON.Parse(net.);
        // debug.Log("congtrinh ne :" + json["congtrinh"][0][0]["name"]);
    }
    public void OpenMenu()
    {
        // menuLai.SetActive(true);
        NetworkManager.ins.socket.Emit("GetItemRong", JSONObject.CreateStringObject("false"));

    }
    private void OnEnable()
    {
        NetworkManager.ins.socket.Emit("GetItemRong", JSONObject.CreateStringObject("false"));
        banRongCatRong.transform.GetChild(0).GetComponent<Button>().interactable = true;
        banRongCatRong.transform.GetChild(1).GetComponent<Button>().interactable = true;
        banRongCatRong.transform.GetChild(2).GetComponent<Button>().interactable = true;
    }
    public void XemRongBan()
    {
        for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount; i++)
        {
            if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
            {
                if(Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).name == idrongvualai)
                {
                    ItemDragon idra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    Inventory.ins.XemRongBan(idra.nameObjectDragon, idra.txtSao.text, idra.name, idra.transform.GetChild(0).GetComponent<Image>());
                    break;
                }
            }
        }
    }
    public void HienChiSo(string namecs,string chiso)
    {
        for (int i = 0; i < contentChiSo.transform.childCount; i++)
        {
            if(contentChiSo.transform.GetChild(i).name == namecs)
            {
                contentChiSo.transform.GetChild(i).gameObject.SetActive(true);
                contentChiSo.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = chiso;
            }
        }
    }
    public void CloseMenu()
    {
        // menuLai.SetActive(false);
        gameObject.SetActive(false);
        Catrongxuong(0);
        Catrongxuong(1);
        for (int i = 0; i < MenuSlot.transform.childCount; i++)
        {
            Destroy(MenuSlot.transform.GetChild(i).gameObject);
        }
        for (int i = 1; i < OcotheLaira.transform.childCount; i++)
        {
            Destroy(OcotheLaira.transform.GetChild(i).gameObject);
        }
        objectchualai.SetActive(true);
        btnlai.interactable = false;
        GameObject objlaira = transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).gameObject;
        for (int i = 0; i < objlaira.transform.GetChild(0).transform.childCount; i++)
        {
            Destroy(objlaira.transform.GetChild(0).transform.GetChild(i).gameObject);
        }
      //  spritelaira.enabled = false;
        banRongCatRong.SetActive(false);
      //  StopCoroutine(offhieuung());
        for (int i = 0; i < SaoRong.transform.childCount; i++)
        {
            SaoRong.transform.GetChild(i).gameObject.SetActive(false);
        }
        Text TxtHiem = objlaira.transform.GetChild(0).GetComponent<Text>();
        TxtHiem.gameObject.SetActive(false);
        imgitemGiaLai.gameObject.SetActive(false);
        txtSoitemCo.text = "";
        btnChucPhuc.SetActive(true);
    }
    public void LoadItem(string name,int sao,string HeRong,string gen,string id,string nameObject,int tienhoa,string Hiem)
    {
      //  debug.Log("Tét " + nameObject);
        GameObject g = Instantiate(Slot, transform.position, Quaternion.identity) as GameObject;
        g.transform.SetParent(MenuSlot.transform,false);
        ScaleObject(g, 1.28f, 1.28f);
        GameObject item = Instantiate(ItemRongLai, g.transform.position, Quaternion.identity) as GameObject;
        g.SetActive(true);
        Image imgRong = item.transform.GetChild(0).GetComponent<Image>();
        imgRong.sprite = Inventory.LoadSpriteRong(nameObject + tienhoa,(short)sao);
        if (Hiem != "")
        {
            Text txtHiem = imgRong.transform.GetChild(0).GetComponent<Text>();
            txtHiem.text = DoHiemCuaRong(Hiem);
        }
        imgRong.SetNativeSize();
        ItemLai itemlai = item.GetComponent<ItemLai>();
        itemlai.NameObjectRong = nameObject;
        item.transform.SetParent(g.transform,true);
        ScaleObject(item, 0.7154113f, 0.7154113f);
        itemlai.txtSao.text = sao + "";
        itemlai.HeRong = HeRong;
        itemlai.Gen = gen;
        itemlai.idronglai = id;
        itemlai.tienhoa = tienhoa;
        item.SetActive(true);
    }
    public string DoHiemCuaRong(string s)
    {
        string dohiem = "";
        if (s == "Hiem")
        {
            dohiem = "Hiếm";
        }
        else if (s == "CucHiem")
        {
            dohiem = "Cực Hiếm";
        }
        else dohiem = s;
        return dohiem;
    }
    public void Lairong()
    {
        if(Chonlai[0] == true && Chonlai[1] == true)
        {
            for (int i = 0; i < contentChiSo.transform.childCount; i++)
            {
                contentChiSo.transform.GetChild(i).gameObject.SetActive(false);
            }
            btnlai.interactable = false;
            NetworkManager.ins.socket.Emit("LaiRong", JSONObject.CreateStringObject(idronglai[0] + "-" + idronglai[1]));//HeRongLai[0] + "-" + GenRong[0] + "+" + HeRongLai[1] + "-" + GenRong[1]
        }
    }
    public void Lai()
    {
        objectchualai.SetActive(false);
        hieuunglai.SetActive(true);
        nutcatlai[0].interactable = false; nutcatlai[1].interactable = false;
        btnlai.interactable = true;
        StartCoroutine(offhieuung());
        btnExit.SetActive(false);
        //for (int i = 1; i < OcotheLaira.transform.childCount; i++)
        //{
        //    OcotheLaira.transform.GetChild(i).gameObject.SetActive(false)
        //}
        imgitemGiaLai.gameObject.SetActive(false);
    }
    public void Chucphuc()
    {
        banRongCatRong.transform.GetChild(0).GetComponent<Button>().interactable = false;
        banRongCatRong.transform.GetChild(1).GetComponent<Button>().interactable = false;
        banRongCatRong.transform.GetChild(2).GetComponent<Button>().interactable = false;
        NetworkManager.ins.socket.Emit("chucphuc");
    }
    public void CatRong()
    {
        GameObject objlaira = transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).gameObject;
        GameObject rongbay = Instantiate(objlaira.transform.GetChild(0).transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
        rongbay.transform.SetParent(AllMenu.ins.transform,false);
        rongbay.transform.position = objlaira.transform.position;
        Inventory.ins.ScaleObject(rongbay, 70.61429f, 70.61429f);
        rongbay.AddComponent<QuaBay>();
        Destroy(rongbay.GetComponent<Animator>());
       // Inventory.ins.ScaleObject();
        Catrongxuong(0);
        Catrongxuong(1);
        for (int i = 1; i < OcotheLaira.transform.childCount; i++)
        {
            Destroy(OcotheLaira.transform.GetChild(i).gameObject);
        }
        objectchualai.SetActive(true);
        btnlai.interactable = false;
       // spritelaira.enabled = false;
        banRongCatRong.SetActive(false);
        for (int i = 0; i < SaoRong.transform.childCount; i++)
        {
            SaoRong.transform.GetChild(i).gameObject.SetActive(false);
        }
        Text TxtHiem = objlaira.transform.GetChild(0).GetComponent<Text>();
        TxtHiem.gameObject.SetActive(false);
        imgitemGiaLai.gameObject.SetActive(false);
        txtSoitemCo.text = "";
        btnChucPhuc.SetActive(true);
    }
    IEnumerator offhieuung()
    {
        yield return new WaitForSeconds(13f);
        btnChucPhuc.SetActive(true);
       // spritelaira.enabled = true;
        // btnlai.gameObject.SetActive(true);
        hieuunglai.SetActive(false);
        float sec = 0.5f;
        GameObject objlaira = transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).gameObject;
        Text TxtHiem = objlaira.transform.GetChild(0).GetComponent<Text>();
        TxtHiem.gameObject.SetActive(true);
        for (int i = 0; i < Saoronglai; i++)
        {
            SaoRong.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(sec);
            if(sec > 0.1f)
            {
                sec -= 0.04f;
            }
        }
        banRongCatRong.SetActive(true);
        for (int j = 1; j < OcotheLaira.transform.childCount; j++)
        {
            Destroy(OcotheLaira.transform.GetChild(j).gameObject);
        }
        nutcatlai[0].interactable = true; nutcatlai[1].interactable = true;
        btnExit.SetActive(true);
        imgitemGiaLai.sprite = Inventory.LoadSprite("HuyenTinh"); //GameObject.Find("HuyenTinh").GetComponent<Image>().sprite;
        imgitemGiaLai.gameObject.SetActive(true);
    }
    public void Catrongxuong(int i)
    {
        GameObject olai = transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).transform.GetChild(i).gameObject;
        if (Chonlai[i] == true)
        {
            btnlai.interactable = false;
            Chonlai[i] = false;

            Destroy(olai.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject);
            //for (int j = 0; j < length; j++)
            //{

            //}
            txtDuarong[i].SetActive(true);
            Rongchon[i].SetActive(true);
            for (int j = 1; j < OcotheLaira.transform.childCount; j++)
            {
                Destroy(OcotheLaira.transform.GetChild(j).gameObject);
            }
            banRongCatRong.SetActive(false);
            objectchualai.SetActive(true);
            GameObject objlaira = transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).gameObject;
            //  spritelaira.enabled = false;
            for (int j = 0; j < objlaira.transform.GetChild(0).transform.childCount; j++)
            {
                Destroy(objlaira.transform.GetChild(0).transform.GetChild(j).gameObject);
            }
            Text TxtHiem = objlaira.transform.GetChild(0).GetComponent<Text>();
            TxtHiem.gameObject.SetActive(false);
            for (int J = 0; J < Saoronglai + 1; J++)
            {
                SaoRong.transform.GetChild(J).gameObject.SetActive(false);
            }
            imgitemGiaLai.gameObject.SetActive(false);
            txtSoitemCo.text = "";
            btnChucPhuc.SetActive(true);
        }
    }
    public void ScaleObject(GameObject g, float x, float y)
    {
        Vector3 Scale;
        g.transform.position = new Vector3();
        Scale = g.transform.localScale;
        Scale.x = x; Scale.y = y;
        g.transform.localScale = Scale;
    }
    public void XemRonglai(string herong)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "xemRonglai";
        datasend["data"]["herong"] = herong;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode jsondata = jsonn["data"];
                if (jsondata["ronglai"].Count > 0)
                {
                    for (int i = 0; i < jsondata["ronglai"].Count; i++)
                    {
                        if (jsondata["ronglai"][i].AsString != "null" && jsondata["ronglai"][i].AsString != "")
                        {
                            string rong = jsondata["ronglai"][i].AsString;
                            string[] rongthongtin = rong.Split('-');
                            for (int j = 0; j < OcotheLaira.transform.childCount; j++)
                            {
                                if (OcotheLaira.transform.GetChild(j).name == rongthongtin[0])
                                {
                                    break;
                                }
                                else
                                {
                                    if (j + 1 == OcotheLaira.transform.childCount)
                                    {
                                        if (rongthongtin.Length > 1)
                                        {
                                            CreateItem(rongthongtin[0], rongthongtin[1]);
                                        }
                                        else
                                        {
                                            CreateItem(rongthongtin[0], "");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    int soitem = 0;
                    txtSoitemCo.text = "0" + "/" + jsondata["canitem"]["gia"].AsString;
                    // imgitemGiaLai.sprite = GameObject.Find("Sprite"+ ItemGiaLai[1]).GetComponent<SpriteRenderer>().sprite;
                    imgitemGiaLai.sprite = Inventory.LoadSprite(jsondata["canitem"]["nameitemcan"].AsString);//GameObject.Find(ItemGiaLai[1]).GetComponent<Image>().sprite;

                    imgitemGiaLai.gameObject.SetActive(true);
                    if (Inventory.ins.ListItemThuong.ContainsKey("item" + jsondata["canitem"]["nameitemcan"].AsString))
                    {
                        GameObject item = Inventory.ins.ListItemThuong["item" + jsondata["canitem"]["nameitemcan"].AsString] as GameObject;
                        Text soitemco = item.transform.GetChild(0).GetComponent<Text>();
                        soitem = int.Parse(soitemco.text);
                        txtSoitemCo.text = soitemco.text + "/" + jsondata["canitem"]["gia"].AsString;
                    }

                    //for (int j = 0; j < Inventory.ins.ListItemThuong.Count; j++)
                    //{
                    //    if (Inventory.ins.ListItemThuong[j].gameObject.name == "item" + ItemGiaLai[1])
                    //    {
                    //        Text soitemco = Inventory.ins.ListItemThuong[j].transform.GetChild(0).GetComponent<Text>();
                    //        soitem = int.Parse(soitemco.text);
                    //        txtSoitemCo.text = soitemco.text + "/" + jsondata["canitem"]["gia"].AsString;
                    //        break;
                    //    }
                    //}
                    if (soitem >= int.Parse(jsondata["canitem"]["gia"].AsString) && OcotheLaira.transform.childCount > 0)
                    {
                        btnlai.interactable = true;
                    }
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
    }
    public void NangCap(string s)
    {
        if (CrGame.ins.DangODao == 0)
        {
            CrGame.ins.OnThongBao(true, "Đang yêu cầu đến máy chủ...");
            XacNhanMuaCongTrinh xnmua = new XacNhanMuaCongTrinh("NhaApTrung", NetworkManager.ins.LevelNhaAp, 0, s);
            string data = JsonUtility.ToJson(xnmua);
            NetworkManager.ins.socket.Emit("NangCapNhaApTrung", new JSONObject(data));
        }
        else
        {
            gameObject.SetActive(false);
            AllMenu.ins.DestroyMenu("MenuNangCapItem");
        }
    }
    public void XemNangCapNhaApTrung()
    {
        if(NetworkManager.ins.LevelNhaAp < 14 && hieuunglai.activeSelf == false)
        {
            XemGiaNangCapLai(NetworkManager.ins.LevelNhaAp);
        }
    }
    public void XemGiaNangCapLai(int cap)
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "xemGiaNangCapNhaApTrung";
        datasend["data"]["name"] = "";
        datasend["data"]["cap"] = cap.ToString();
        datasend["data"]["idcongtrinh"] = "0";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                AllMenu.ins.OpenMenu("MenuNangCapItem");

                //   SpriteRong spritect = GameObject.Find("SpriteNhaApTrung").GetComponent<SpriteRong>();
                CrGame.ins.UI = AllMenu.ins.GetCreateMenu("MenuNangCapItem", null, true, transform.GetSiblingIndex() + 1).GetComponent<Ui>();
                //  CrGame.ins.UI.SpriteItem[0].sprite = spritect.spriteTienHoa[net.LevelNhaAp / 5]; CrGame.ins.UI.SpriteItem[0].SetNativeSize();
                StartCoroutine(LoadSpriteWeb(CrGame.ins.UI.SpriteItem[0], NetworkManager.ins.LevelNhaAp / 5));
                if ((NetworkManager.ins.LevelNhaAp + 1) / 5 < 3)
                {
                    //  CrGame.ins.UI.SpriteItem[1].sprite = CrGame.ins//spritect.spriteTienHoa[(net.LevelNhaAp + 1) / 5];
                    StartCoroutine(LoadSpriteWeb(CrGame.ins.UI.SpriteItem[1], (NetworkManager.ins.LevelNhaAp + 1) / 5));
                }
                else
                {
                    //  CrGame.ins.UI.SpriteItem[1].sprite = spritect.spriteTienHoa[spritect.spriteTienHoa.Length - 1]; 
                    StartCoroutine(LoadSpriteWeb(CrGame.ins.UI.SpriteItem[1], 2));
                }
                CrGame.ins.UI.SpriteItem[1].SetNativeSize();
                CrGame.ins.UI.txtlevel[0].text = NetworkManager.ins.CatDauNgoacKep(json["levelhientai"].AsString);
                CrGame.ins.UI.txtlevel[1].text = NetworkManager.ins.CatDauNgoacKep(json["leveltieptheo"].AsString);
                string chiso1 = NetworkManager.ins.CatDauNgoacKep(json["chiso1"].AsString);
                string chiso2 = NetworkManager.ins.CatDauNgoacKep(json["chiso2"].AsString);
                CrGame.ins.UI.txtChiSo[0].text = chiso1;
                CrGame.ins.UI.txtChiSo[1].text = chiso2;
                CrGame.ins.UI.tilethanhcong.SetActive(true);
                CrGame.ins.UI.nangcapDao.SetActive(false);
                CrGame.ins.UI.txtTileThanhCong[0].text = NetworkManager.ins.CatDauNgoacKep(json["tilethanhcongvang"].AsString + "%");
                CrGame.ins.UI.txtTileThanhCong[1].text = NetworkManager.ins.CatDauNgoacKep(json["tilethanhcongkimcuong"].AsString + "%");
                CrGame.ins.UI.btnNangCap[0].gameObject.SetActive(false);
                CrGame.ins.UI.btnNangCap[1].gameObject.SetActive(false);
                CrGame.ins.UI.btnNangCapNhaApTrung[0].gameObject.SetActive(true);
                CrGame.ins.UI.btnNangCapNhaApTrung[1].gameObject.SetActive(true);
                CrGame.ins.UI.btnNangCapThucAn[0].gameObject.SetActive(false); CrGame.ins.UI.btnNangCapThucAn[1].gameObject.SetActive(false);
                CrGame.ins.UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                GameObject objThanLong = CrGame.ins.UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;
                objThanLong.SetActive(false);
                if (int.Parse(CrGame.ins.txtLevel.text) >= int.Parse(NetworkManager.ins.CatDauNgoacKep(json["levelnang"].AsString)))
                {
                    CrGame.ins.UI.btnNangCapNhaApTrung[0].interactable = true;
                    CrGame.ins.UI.btnNangCapNhaApTrung[1].interactable = true;
                    CrGame.ins.UI.txtGiaNhaAptrung[0].text = NetworkManager.ins.CatDauNgoacKep(json["giavang"].AsString);
                    CrGame.ins.UI.txtGiaNhaAptrung[1].text = NetworkManager.ins.CatDauNgoacKep(json["giakimcuong"].AsString);
                }
                else
                {
                    CrGame.ins.UI.btnNangCapNhaApTrung[0].interactable = false;
                    CrGame.ins.UI.btnNangCapNhaApTrung[1].interactable = false;
                    CrGame.ins.UI.txtGiaNhaAptrung[0].text = "<color=#ff0000ff>" + "Cấp " + NetworkManager.ins.CatDauNgoacKep(json["levelnang"].AsString) + "</color>";
                    CrGame.ins.UI.txtGiaNhaAptrung[1].text = "<color=#ff0000ff>" + "Cấp " + NetworkManager.ins.CatDauNgoacKep(json["levelnang"].AsString) + "</color>";
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
    }
    IEnumerator LoadSpriteWeb(Image img, int cap)
    {
        WWW www = new WWW(CrGame.ins.ServerName + "image/name/NhaApTrung/cap/" + cap);
        yield return www;
        debug.Log("load xong");
        img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        img.SetNativeSize();
    }
    void CreateItem(string rong,string hiem)
    {
        GameObject itemCoTheLai = Instantiate(ItemCoTheLaiRa, transform.position, Quaternion.identity) as GameObject;
        itemCoTheLai.transform.SetParent(OcotheLaira.transform, false);
        itemCoTheLai.name = rong;
        Image imgRong = itemCoTheLai.transform.GetChild(0).GetComponent<Image>();
        imgRong.sprite = Inventory.LoadSpriteRong(rong + 1);
        imgRong.SetNativeSize();
        Text TxtHiem = imgRong.transform.GetChild(0).GetComponent<Text>();
        TxtHiem.text = DoHiemCuaRong(hiem);
        RectTransform rect = imgRong.GetComponent<RectTransform>();
        rect.position = new Vector3();
        itemCoTheLai.SetActive(true);
        imgRong.transform.position = itemCoTheLai.transform.position;
    }
    public void OpenMenuLongAp()
    {
        if(hieuunglai.activeSelf == false)
        {
            CrGame.ins.OpenGiaoDienLongAp();
            CloseMenu();
        }    
  
    }
}
