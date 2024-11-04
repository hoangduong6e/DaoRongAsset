using SimpleJSON;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DragonIslandManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject
        ChienTuong,
        DungThucAn;
    public static string ParseName(string name, bool hoangkim, string saorong)
    {
        string namedra = name + " <color=yellow>(" + saorong + " sao)</color>";
        if (hoangkim)
        {
            namedra = "<color=orange>[Hoàng Kim]</color> " + name + "<color=yellow>(" + saorong + " sao)</color>";
        }
        //debug.LogError(namedra + " " + hoangkim + " " + saorong);
        return namedra;
    }
    public static void SetChienTuong(Transform vitridra)
    {
        DragonIslandController dra = vitridra.GetComponent<DragonIslandController>();
        GameObject iconChienTuong = null;
        if (ChienTuong != null)
        {
            //Destroy(ChienTuong);
            iconChienTuong = ChienTuong;
        }    
        else
        {

            iconChienTuong = new GameObject();
            Inventory.ins.ScaleObject(iconChienTuong, 0.0035f, 0.0035f);
            Image SpriteChienTuong = iconChienTuong.AddComponent<Image>();
            SpriteChienTuong.sprite = VienChinh.vienchinh.ChienTuongVang; SpriteChienTuong.SetNativeSize();
            iconChienTuong.name = "iconchientuong";

        }
        iconChienTuong.transform.SetParent(dra.txtNameRong.transform.parent.transform);
        Vector3 vec3= dra.txtNameRong.transform.position;
        iconChienTuong.transform.position = new Vector3(vec3.x, vec3.y + 0.4f, vec3.z); 
        ChienTuong = iconChienTuong;
    }    

    public static void SetChienTuongDo(Transform vitridra)
    {
        DragonIslandController dra = vitridra.GetComponent<DragonIslandController>();
        GameObject iconChienTuong = new GameObject();
        Inventory.ins.ScaleObject(iconChienTuong, 0.0035f, 0.0035f);
        Image SpriteChienTuong = iconChienTuong.AddComponent<Image>();
        SpriteChienTuong.sprite = VienChinh.vienchinh.ChienTuongDo; SpriteChienTuong.SetNativeSize();
        iconChienTuong.name = "iconchientuongdo";
        iconChienTuong.transform.SetParent(dra.txtNameRong.transform.parent.transform);
        Vector3 vec3 = dra.txtNameRong.transform.position;
        iconChienTuong.transform.position = new Vector3(vec3.x, vec3.y + 0.4f, vec3.z);
    }
    public static void ParseDragonIsland(JSONNode data,byte dao,Vector3 vectarget = new Vector3(),Transform chuarongdao = null)
    {
        string nameobject = data["nameobject"].AsString;
        string tenrong = data["namerong"].AsString;
        Transform ChuaRongDao = chuarongdao;
        if(ChuaRongDao == null) ChuaRongDao = CrGame.ins.AllDao.transform.Find("BGDao" + dao).transform.Find("RongDao");//CrGame.ins.FindObject(CrGame.ins.AllDao.transform.GetChild(i).gameObject, "RongDao");                                                                                   //rong.transform.SetParent(ChuaRongDao.transform);
        string id = data["id"].AsString;
        string saorong = data["sao"].AsString;
        string namedra = "";
        bool hoangkim = false;
        if (data["hoangkim"].AsString != "") hoangkim = true;

        bool hienthiten = false;
        bool.TryParse(data["chiso"]["hienthiten"].AsString, out hienthiten);
        if (hienthiten || hoangkim)
        {
            namedra = ParseName(tenrong, hoangkim, saorong);
            // dra.txtnamerong.gameObject.SetActive(true);
        }
       // debug.Log("ParseDragonIsland 2");
        //rong.gameObject.SetActive(true);
        string tienhoa = data["tienhoa"].AsString;
        Vector3 vecrong = vectarget;
        if(vectarget == Vector3.zero)
        {
            Vector3 vec = ChuaRongDao.transform.position;
            vecrong = new Vector3(vec.x - Random.Range(-3, 3), vec.y + Random.Range(-2.5f, 2.5f), 0);
        }
      //  debug.Log("ParseDragonIsland 3");
        GameObject rong = Instantiate(Inventory.GetObjRong(nameobject + tienhoa), vecrong, Quaternion.identity);


        //if (nameobject == "RongRua")
        //{
        //    if (int.Parse(saorong) >= 30)
        //    {
        //        rong.transform.GetChild(0).transform.localScale = Vector3.one;
        //        rong.transform.GetChild(1).transform.localScale = Vector3.one;
        //    }
        //}

        PVEManager.SetScaleDragon(nameobject,byte.Parse(saorong),rong.transform);
        rong.transform.SetParent(ChuaRongDao.transform, true);
        DraInstantiate draInstantiate = rong.GetComponent<DraInstantiate>();
        DataDragonIsland dataDragonIsland = new DataDragonIsland(namedra, id);
        rong.transform.position = vecrong;
        draInstantiate.DraInsIsland(dataDragonIsland);
        if (bool.Parse(data["chiso"]["chientuong"].AsString))
        {
            if(!Friend.ins.QuaNha) SetChienTuong(rong.transform);
            else SetChienTuongDo(rong.transform);
        }
    }
    public static void InsAllItemDao(JSONNode data, byte dao)
    {
        for (int j = 0; j < data.Count; j++)
        {
            InsItemDao(JSON.Parse(data[j].ToString()), dao);
        }
    }
    public static GameObject InsItemDao(JSONNode data,byte dao)
    {
        debug.Log("InsItemDao " + data.ToString());
        Transform Dao = null;
        if(Friend.ins.QuaNha)
        {
            Dao = Friend.ins.DaoFriend.transform.Find("DaoFriend" + dao);
        }
        else Dao = CrGame.ins.AllDao.transform.Find("BGDao" + dao);
        Transform ObjItemEvent = Dao.transform.Find("ObjItemEvent");
        if(ObjItemEvent == null)
        {
            GameObject newObject = new GameObject("ObjItemEvent");
            newObject.transform.SetParent(Dao.transform,false);
            ObjItemEvent = newObject.transform;
        }
        GameObject ItemRoi = Instantiate(Inventory.ins.GetObj(data["nameitem"].AsString),Vector3.zero,Quaternion.identity);
        ItemRoi.transform.SetParent(ObjItemEvent.transform,false);
        float x = 0, y = 0;
        float.TryParse(data["x"].AsString,out x);
        float.TryParse(data["y"].AsString,out y);
        if(Friend.ins.QuaNha)
        {
            y -= Mathf.Abs(Dao.transform.position.y);
        }
        ItemRoi.transform.position = new Vector3(x,y,1);
        ItemRoi.name = data["namekey"].AsString;
        EventTrigger eventTrigger = ItemRoi.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { CrGame.ins.PointerDownNhatItemRoi((PointerEventData)eventData); });

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((eventData) => { CrGame.ins.PointerUpNhatItemRoi((PointerEventData)eventData); });

        eventTrigger.triggers.Add(entry);
        eventTrigger.triggers.Add(entry2);
        return ItemRoi;

    }
    
    public static void PhongChienTuong(string id)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "PhongChienTuong";
        datasend["data"]["idrong"] = id;
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                SetChienTuong(CrGame.ins.TfrongInfo);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }    
    public static void XemKhamNgoc(string id, string dao)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "XemKhamNgoc";
        datasend["data"]["idrong"] = id;
        datasend["data"]["dao"] = dao;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                AllMenu.ins.OpenMenu("MenuKhamNgoc");
                KhamNgoc khamngoc = AllMenu.ins.menu["MenuKhamNgoc"].GetComponent<KhamNgoc>();
                Image imgngoc = null;
                for (int i = 0; i < json["data"]["allngoc"].Count; i++)
                {
                    // debug.Log(json["data"]["allngoc"][i]["namengoc"].Value);
                    khamngoc.AddTuiNgoc(json["data"]["allngoc"][i]["namengoc"].Value, int.Parse(json["data"]["allngoc"][i]["soluong"].Value));
                }
                if (json["data"]["ngocrong"]["ngoc1"]["name"].Value != "")
                {
                    khamngoc.oNgoc.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                     imgngoc = khamngoc.oNgoc.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
                    imgngoc.gameObject.SetActive(true);
                    imgngoc.sprite = Inventory.LoadSprite(json["data"]["ngocrong"]["ngoc1"]["name"].Value);
                    imgngoc.name = json["data"]["ngocrong"]["ngoc1"]["name"].Value;
                }
                if (json["data"]["ngocrong"]["ngoc2"]["name"].Value != "lock")
                {
                    khamngoc.oNgoc.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
                    if (json["data"]["ngocrong"]["ngoc2"]["name"].Value != "")
                    {
                        khamngoc.oNgoc.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                         imgngoc = khamngoc.oNgoc.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
                        imgngoc.gameObject.SetActive(true);
                        imgngoc.sprite = Inventory.LoadSprite(json["data"]["ngocrong"]["ngoc2"]["name"].Value);
                        imgngoc.name = json["data"]["ngocrong"]["ngoc2"]["name"].Value;
                    }
                }
                if (json["data"]["ngocrong"]["ngoc3"]["name"].Value != "lock")
                {
                    khamngoc.oNgoc.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);
                    if (json["data"]["ngocrong"]["ngoc3"]["name"].Value != "")
                    {
                        khamngoc.oNgoc.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                         imgngoc = khamngoc.oNgoc.transform.GetChild(2).transform.GetChild(1).GetComponent<Image>();
                        imgngoc.gameObject.SetActive(true);
                        imgngoc.sprite = Inventory.LoadSprite(json["data"]["ngocrong"]["ngoc3"]["name"].Value);
                        imgngoc.name = json["data"]["ngocrong"]["ngoc3"]["name"].Value;
                    }
                }
                // chiso
                khamngoc.maskHp.fillAmount = float.Parse(json["data"]["thanhchiso"]["hp"].Value) / float.Parse(json["data"]["thanhchiso"]["maxhp"].Value);
                khamngoc.maskSatthuong.fillAmount = float.Parse(json["data"]["thanhchiso"]["dame"].Value) / float.Parse(json["data"]["thanhchiso"]["maxdame"].Value);
                khamngoc.maskChiMang.fillAmount = float.Parse(json["data"]["chisongoc"]["tilechimang"].Value) / 100;
                khamngoc.maskNeTranh.fillAmount = float.Parse(json["data"]["chisongoc"]["netranh"].Value) / 100;
                khamngoc.MaskHutHp.fillAmount = float.Parse(json["data"]["chisongoc"]["hutmau"].Value) / 100;
                khamngoc.txtHp.text = "+" + json["data"]["chisongoc"]["hp"].Value;
                khamngoc.txtSatThuong.text = "+" + json["data"]["chisongoc"]["sucdanh"].Value;
                khamngoc.txtchimang.text = "+" + json["data"]["chisongoc"]["tilechimang"].Value + "%";
                khamngoc.txtnetranh.text = "+" + json["data"]["chisongoc"]["netranh"].Value + "%";
                khamngoc.txthuthp.text = "+" + json["data"]["chisongoc"]["hutmau"].Value + "%";
                
                AllMenu.ins.LoadRongGiaoDien(json["data"]["nameobject"].AsString, khamngoc.animRong.transform,1,false);

                imgngoc.SetNativeSize();
                GamIns.ResizeItem(imgngoc, 120);
                //
                //DragonController dra = CrGame.ins.TfrongInfo.GetComponent<DragonController>();
                // khamngoc.animRong.runtimeAnimatorController = CrGame.ins.TfrongInfo.GetComponent<Animator>().runtimeAnimatorController;
                // khamngoc.animRong.SetInteger("TienHoa", dra.tienhoa);
                //DestroyGD();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
            CrGame.ins.panelLoadDao.SetActive(false);
        }
    }
    private static string GetMauSao(string sao)
    {
        if (sao == "5") return "<color=yellow>" + sao + "</color>";
        else if (sao == "6") return "<color=red>" + sao + "</color>";
        else return sao;
    }    
    public static void XemInfoRong(string id, string dao)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "XemInfoRong";
        datasend["data"]["idrong"] = id;
        datasend["data"]["dao"] = dao;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                AllMenu.ins.DestroyMenu("MenuVongtronXemRong");
                // AllMenu.ins.menu["MenuVongtronXemRong"].gameObject.SetActive(false);
                InfoRong info = AllMenu.ins.GetCreateMenu("MenuInfoRong").GetComponent<InfoRong>();
                info.txtNameRong.text = json["info"]["namerong"].AsString;
                info.txtDaTruongThanh.text = json["info"]["txttruongthanh"].AsString;
                info.imgRong.sprite = Inventory.LoadSpriteRong(json["info"]["nameobject"].AsString, short.Parse(json["saorong"].AsString));
                info.imgRong.SetNativeSize();
                info.txtHiem.text = json["info"]["hiem"].AsString;
                info.LoadSao(byte.Parse(json["saorong"].AsString));
                if (json["hoangkim"].AsBool)
                {
                    info.transform.GetChild(1).gameObject.SetActive(true);
                }
                else info.transform.GetChild(1).gameObject.SetActive(false);
                for (int i = 0; i < json["info"]["thongtin"].Count; i++)
                {
                   // debug.Log(json["info"]["thongtin"][i]["phantramvang"]["cong"].AsString);
                    if (json["info"]["thongtin"][i]["phantramvang"] != null)
                    {
                        info.panelinfo.transform.GetChild(0).gameObject.SetActive(true);
                        string cong = json["info"]["thongtin"][i]["phantramvang"]["cong"].AsString;
                        info.panelinfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>+" + cong + "%</color> tiền vàng";
                        info.panelinfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GetMauSao(json["info"]["thongtin"][i]["phantramvang"]["sao"].AsString);
                    }
                    else if (json["info"]["thongtin"][i]["phantramexp"] != null)
                    {
                        info.panelinfo.transform.GetChild(1).gameObject.SetActive(true);
                        string cong = json["info"]["thongtin"][i]["phantramexp"]["cong"].AsString;
                        info.panelinfo.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>+" + cong + "%</color> kinh nghiệm";
                        info.panelinfo.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = GetMauSao(json["info"]["thongtin"][i]["phantramexp"]["sao"].AsString);
                    }
                    else if (json["info"]["thongtin"][i]["satthuong"] != null)
                    {
                        info.panelinfo.transform.GetChild(2).gameObject.SetActive(true);
                        string cong = json["info"]["thongtin"][i]["satthuong"]["cong"].AsString;
                        info.panelinfo.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>+" + cong + "</color><color=#ff00ffff> sát thương chiến đấu</color>";
                        info.panelinfo.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = GetMauSao(json["info"]["thongtin"][i]["satthuong"]["sao"].AsString);
                    }
                    else if (json["info"]["thongtin"][i]["rotvatpham"] != null)
                    {
                        info.panelinfo.transform.GetChild(3).gameObject.SetActive(true);
                        string cong = json["info"]["thongtin"][i]["rotvatpham"]["cong"].AsString;
                        info.panelinfo.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff>+" + cong + "%</color><color=#ff00ffff> tỉ lệ rớt vật phẩm</color>";
                        info.panelinfo.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = GetMauSao(json["info"]["thongtin"][i]["rotvatpham"]["sao"].AsString);
                    }
                    else if (json["info"]["thongtin"][i]["giamthoigian"] != null)
                    {
                        info.panelinfo.transform.GetChild(4).gameObject.SetActive(true);
                        string cong = json["info"]["thongtin"][i]["giamthoigian"]["cong"].AsString;
                        info.panelinfo.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "<color=#00ff00ff> Giảm " + cong + "s</color> thời gian tiêu hóa";
                        info.panelinfo.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = GetMauSao(json["info"]["thongtin"][i]["giamthoigian"]["sao"].AsString);
                    }
                }
                if (json["info"]["infodacbiet"] != null)
                {
                    debug.Log(json["info"]["infodacbiet"]);
                    info.panelinfo.transform.GetChild(5).gameObject.SetActive(true);
                    info.panelinfo.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = json["info"]["infodacbiet"]["infodacbiet1"].AsString;

                    if (json["info"]["infodacbiet"]["infodacbiet2"] != null)
                    {
                        info.panelinfo.transform.GetChild(6).gameObject.SetActive(true);
                        info.panelinfo.transform.GetChild(6).transform.GetChild(0).GetComponent<Text>().text = json["info"]["infodacbiet"]["infodacbiet2"].AsString;
                    }
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
            CrGame.ins.panelLoadDao.SetActive(false);
        }
    }
    public static void CatRong(string id, string dao)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "CatRong";
        datasend["data"]["idrong"] = id;
        datasend["data"]["dao"] = dao;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                string nameitem = json["Dra"]["nameitem"].AsString;
                string namerong = json["Dra"]["namerong"].AsString;
                string nameObject = json["Dra"]["nameobject"].AsString;
                string id = json["Dra"]["id"].AsString;
                AllMenu.ins.menu["MenuVongtronXemRong"].SetActive(false);
                Destroy(GameObject.Find(id));
                bool hoangkim = false;
                if (json["Dra"]["hoangkim"].ToString() != "") hoangkim = true;

                Inventory.ins.AddItemRong(id, nameitem, byte.Parse(json["Dra"]["sao"].AsString),
                    json["Dra"]["level"].AsInt, json["Dra"]["exp"].AsInt, json["Dra"]["maxexp"].AsInt,
                    byte.Parse(json["Dra"]["tienhoa"].AsString), 0, namerong, nameObject, hoangkim, json["Dra"]["ngoc"],false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
            CrGame.ins.panelLoadDao.SetActive(false);
        }
    }
    public static void XemDoiTenRong(string id, string dao)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "GetinfoDraIsland";
        datasend["data"]["idrong"] = id;
        datasend["data"]["dao"] = dao;
        datasend["data"]["get"]["namerong"] = "";
        datasend["data"]["get"]["nameobject"] = "";
        datasend["data"]["get"]["tienhoa"] = "";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //  SpriteRenderer sprite = dra.GetComponent<SpriteRenderer>();
                GameObject PanelDoiTenRong = AllMenu.ins.GetCreateMenu("PanelDoiTenRong");
                Text TextName = PanelDoiTenRong.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
             //   SpriteRenderer spriteRong = PanelDoiTenRong.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<SpriteRenderer>();
              
               // spriteRong.sprite = Inventory.LoadSpriteRong(json["datadra"]["nameobject"].AsString + json["datadra"]["tienhoa"].AsString);
                TextName.text = json["datadra"]["namerong"].AsString;
                Button btndoiten = PanelDoiTenRong.transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).GetComponent<Button>();
                btndoiten.onClick.RemoveAllListeners();
                btndoiten.onClick.AddListener(Inventory.ins.DoiTenRong);
                CrGame.ins.StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.2f);
                    AllMenu.ins.LoadRongGiaoDien(json["datadra"]["nameobject"].AsString + json["datadra"]["tienhoa"].AsString, PanelDoiTenRong.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).transform, 1);
                }
       

                // transform.parent.gameObject.SetActive(false);
                //DestroyGD();
                //MenuDoiTenRong.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
            CrGame.ins.panelLoadDao.SetActive(false);
        }
    }
    public static void DropDragon(string id, Vector3 vecdrop)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "ThaRong";
        datasend["data"]["idrong"] = id;
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //string[] id =json["tharongok"]["id"].ToString().Split('"');
              
                ItemDragon itemdra = GameObject.Find(id).GetComponent<ItemDragon>();
                if (itemdra == null) return;
                Destroy(itemdra.gameObject);

                ParseDragonIsland(json["dragon"],CrGame.ins.DangODao, vecdrop);
            }
            else if (json["status"].AsString == "1")
            {
                GameObject menuDoiRong = Inventory.ins.menuTuiDo.transform.GetChild(2).gameObject;
                if (menuDoiRong.activeSelf == false)
                {
                    for (var i = 0; i < 9; i++)
                    {
                        if (menuDoiRong.transform.GetChild(i).transform.childCount > 0) Destroy(menuDoiRong.transform.GetChild(i).transform.GetChild(0).gameObject);
                        else break;
                    }

                    for (int i = 0; i < json["openmenudoirong"].Count; i++)
                    {
                        string nameitem = json["openmenudoirong"][i]["nameitem"].AsString;
                        string namerong = json["openmenudoirong"][i]["namerong"].AsString;
                        string nameObject = json["openmenudoirong"][i]["nameobject"].AsString;
                        string id = json["openmenudoirong"][i]["id"].AsString;

                        GameObject itemRong = Instantiate(Inventory.ins.ItemRong, menuDoiRong.transform.GetChild(i).transform.position, Quaternion.identity) as GameObject;
                        itemRong.transform.SetParent(menuDoiRong.transform.GetChild(i).transform);
                        itemRong.name = "rongdoi-" + id;
                        Image imgRong = itemRong.transform.GetChild(0).GetComponent<Image>();
                        string[] Dohiemcuarong = nameitem.Split('-');
                        imgRong.sprite = Inventory.LoadSpriteRong(nameObject + json["openmenudoirong"][i]["tienhoa"].AsString, short.Parse(json["openmenudoirong"][i]["saorong"].AsString));
                        if (Dohiemcuarong[1] != "")
                        {
                            Text txtHiem = imgRong.transform.GetChild(0).GetComponent<Text>();
                            txtHiem.text = Inventory.DoHiemCuaRong(Dohiemcuarong[1]);
                        }
                        imgRong.SetNativeSize();
                        ItemDragon idra = itemRong.GetComponent<ItemDragon>();
                        idra.txtSao.text = json["openmenudoirong"][i]["sao"].AsString;
                        Inventory.ins.ScaleObject(itemRong, 1, 1);
                        itemRong.SetActive(true);
                        Destroy(idra);
                    }
                    menuDoiRong.SetActive(true);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public static void RongDoi(JSONNode json)
    {
       // debug.Log("rong doi " + json.ToString());
        string status = json["status"].AsString;
        string idrong = json["idrong"].AsString;
        //DragonController dra = CrGame.ins.FindRongDao(int.Parse(CatDauNgoacKep(e.data["dao"].ToString())), idrong).GetComponent<DragonController>();//GameObject.Find(idrong[1]).GetComponent<DragonController>();
        DragonIslandController draa = CrGame.ins.FindRongDao((json["dao"].AsInt), idrong).GetComponent<DragonIslandController>();
      //  dra.thongbaodoi = dra.transform.GetChild(0).gameObject;
        if (status == "doi")
        {
            draa.txtNameRong.transform.parent.transform.Find("BieuCamRong").gameObject.SetActive(true);
            draa.doi = true;
            //dra.thongbaodoi.SetActive(true);
        }
      //  dra.timeconlai = float.Parse(e.data["timeconlai"].ToString()) + 0.3f;
       // dra.maxtimeconlai = float.Parse(e.data["maxtimedoi"].ToString());
    }    
    public static void RongAn(string id, string thucan,Transform tf)
    {
        byte dao = CrGame.ins.DangODao;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "RongAn";
        datasend["data"]["namerong"] = id;
        datasend["data"]["namethucan"] = thucan;
        datasend["data"]["dangodao"] = dao.ToString();
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok,true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //  string idrong = json["idrong"].AsString;
                // DragonController dracontroller = CrGame.ins.FindRongDao(CrGame.ins.DangODao, idrong[1]).GetComponent<DragonController>();//GameObject.Find(idrong[1]).GetComponent<DragonController>();
                //int exprongcong = int.Parse(e.data["exprongcong"].ToString());
                //int exprong = int.Parse(e.data["exprong"].ToString());
                //int levelrong = int.Parse(e.data["levelrong"].ToString());
                debug.Log(json.ToString());
                ThuongAn(json["qua"]["exp"].AsString, json["qua"]["exprongcong"].AsString,tf.transform);

                if (json["lencap"].AsBool)
                {
                    GameObject hieuung = Instantiate(Inventory.ins.GetObj("HieuUng1"), new Vector3(tf.position.x, tf.position.y + 0.5f), Quaternion.identity) as GameObject;
                    hieuung.transform.SetParent(GameObject.Find("CanvasGame").transform, true);
                    hieuung.SetActive(true);
                    Destroy(hieuung, 1.6f);
                    ParseDragonIsland(json["rong"],dao,tf.transform.position);
                    Destroy(tf.gameObject);
                    // tienhoa += tien_hoa;
                    // anim.SetInteger("TienHoa", tienhoa);
                }
                //dracontroller.LoadExpRong(exprong, int.Parse(e.data["maxexprong"].ToString()), levelrong, byte.Parse(e.data["tienhoa"].ToString()));
                //dracontroller.sao = byte.Parse(e.data["sao"].ToString());
            }    
        }
    }
    private static void ThuongAn(string exp, string exprong,Transform tf)
    {
        GameObject prefabthuong = Instantiate(Inventory.ins.GetObj("ThuongChoRongAnFrefab"), new Vector3(tf.transform.position.x, tf.transform.position.y), Quaternion.identity) as GameObject;
        //  prefabthuong.transform.SetParent(GameObject.Find("Canvas").transform);
        Text txtTangExp = prefabthuong.transform.GetChild(0).GetComponent<Text>();
        txtTangExp.text = exp;
        Text txtTangExpRong = prefabthuong.transform.GetChild(1).GetComponent<Text>();
        txtTangExpRong.text = exprong;
        prefabthuong.SetActive(true);
        Destroy(prefabthuong, 3);
    }
    public static void DropThucAn(string namethucan)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "ThaThucAn";
        datasend["data"]["namethucan"] = namethucan;
        datasend["data"]["dangodao"] = CrGame.ins.DangODao.ToString();
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                Inventory.ins.AddItem(namethucan, -int.Parse(json["dropthucan"]["soluong"].AsString));
                if (namethucan == "BinhTieuHoa30p" || namethucan == "BinhTieuHoa1h" || namethucan == "BinhTieuHoa3h")
                {
                    GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
                    GameObject RongDao = Dao.transform.Find("RongDao").gameObject;
                   // debug.Log("obj rong dao " + RongDao.name);
                    if (RongDao.transform.childCount > 2)
                    {
                        for (int i = 3; i < RongDao.transform.childCount; i++)
                        {
                            RongDao.transform.GetChild(i).GetComponent<DragonIslandController>().doi = true;
                        }
                    }
                    if (Inventory.ins.ListItemThuong.ContainsKey("item" + namethucan) == false)
                    {
                        GameObject objtuibinhtieuhoa = CrGame.ins.tuithucAn.transform.parent.transform.parent.transform.parent.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).gameObject;
                        objtuibinhtieuhoa.transform.Find(namethucan).gameObject.SetActive(false);
                    }

                }
                ThuyenThucAn thuyen = NetworkManager.ins.GetComponent<ThuyenThucAn>();  
                thuyen.StartCoroutine(thuyen.bay(Inventory.LoadSprite(namethucan), int.Parse(json["dropthucan"]["soluong"].AsString)));
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }    
        }

      //  string namethucan = CatDauNgoacKep(e.data["dropthucan"]["namethucan"].ToString());
     
    }
}
