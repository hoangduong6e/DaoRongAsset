using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XLua;
[LuaCallCSharp]
public class Inventory : MonoBehaviour
{
    //public Hashtable ListItemThuong = new Hashtable();
    public Dictionary<string, GameObject> ListItemThuong = new Dictionary<string, GameObject>();
    //public Hashtable CacheRong = new Hashtable();
   // public Dictionary<string, Sprite> Cachesprite = new Dictionary<string, Sprite>();
    //public Hashtable CacheSprite = new Hashtable();
    //public List<GameObject> ListItemThuong;
    public GameObject tuido, tuirong,tuingoc,contentNgoc,SlotNgoc; GameObject Tuidangmo;
    public GameObject TuiRong, OSlot,TuiGiap,contentGiap,slotgiap;
  //  public Vector3 targetTharong;
    public GameObject ItemRong;public CrGame crGame; NetworkManager net;
    public Text txtTrang;
    [HideInInspector] public int MaxTrangRong, trangtuirong = 1,MaxTrangNgoc,trangtuingoc = 1; public GameObject btnsangtrai, btnsangphai, btnsangphaicuoi, btnsangtraicuoi;
    //tui do thuong
    int MaxTrangtuithuong, trangtuithuong = 1; public Text txtTenTui;
    int trangttuigiap = 1, maxtrangtuigiap;
    public GameObject Otuithuong; public GameObject menuTuiDo, HuyThaoTac;ThuyenThucAn thuyen;
    public static Inventory ins;
    // public Dictionary<string, RuntimeAnimatorController> animrong = new Dictionary<string, RuntimeAnimatorController>();// RuntimeAnimatorController[] AnimRong; // giam lag
    //public GameObject[] RongCoBan;// giam lag

    // Start is called before the first frame update

    public void SapXepTuiRong()
    {
        Transform tuiRong = Inventory.ins.TuiRong.transform;
        int childCount = tuiRong.childCount;

        // Tạo một danh sách để lưu các rồng
        List<Transform> dragons = new List<Transform>();

        // Duyệt qua các ô trong túi
        for (int i = 0; i < childCount - 1; i++)
        {
            Transform slot = tuiRong.GetChild(i);

            if (slot.childCount > 0)
            {
                // Lấy rồng trong ô và lưu vào danh sách
                dragons.Add(slot.GetChild(0));
            }
        }

        // Xóa toàn bộ rồng hiện tại trong túi
        //for (int i = 0; i < childCount - 1; i++)
        //{
        //    Transform slot = tuiRong.GetChild(i);

        //    if (slot.childCount > 0)
        //    {
        //        Destroy(slot.GetChild(0).gameObject); // Xóa rồng trong ô
        //    }
        //}

        // Sắp xếp lại các rồng vào các ô từ trên xuống dưới
        for (int i = 0; i < dragons.Count; i++)
        {
            Transform slot = tuiRong.GetChild(i);
            dragons[i].SetParent(slot,false); // Đặt rồng vào ô trống
            dragons[i].transform.position = slot.transform.position; // Đặt lại vị trí
        }
    }

    bool sapXepGiamDan = false;
    public void SapXepTuiRongTheoSao()
    {
        Transform tuiRong = Inventory.ins.TuiRong.transform;
        int childCount = tuiRong.childCount;

        // Tạo một danh sách để lưu các rồng
        List<ItemDragon> dragons = new List<ItemDragon>();

        // Duyệt qua các ô trong túi
        for (int i = 0; i < childCount - 1; i++)
        {
            Transform slot = tuiRong.GetChild(i);

            if (slot.childCount > 0)
            {
                // Lấy rồng trong ô và lưu vào danh sách
                ItemDragon itemDragon = slot.GetChild(0).GetComponent<ItemDragon>();
                dragons.Add(itemDragon);
            }
        }

        // Sắp xếp danh sách rồng theo số sao
        if (sapXepGiamDan)
        {
            dragons.Sort((x, y) => int.Parse(y.txtSao.text).CompareTo(int.Parse(x.txtSao.text))); // Sắp xếp giảm dần
        }
        else
        {
            dragons.Sort((x, y) => int.Parse(x.txtSao.text).CompareTo(int.Parse(y.txtSao.text))); // Sắp xếp tăng dần
        }

        // Xóa toàn bộ rồng hiện tại trong túi
        //for (int i = 0; i < childCount - 1; i++)
        //{
        //    Transform slot = tuiRong.GetChild(i);

        //    if (slot.childCount > 0)
        //    {
        //        Destroy(slot.GetChild(0).gameObject); // Xóa rồng trong ô
        //    }
        //}

        // Sắp xếp lại các rồng vào các ô từ trên xuống dưới
        for (int i = 0; i < dragons.Count; i++)
        {
            Transform slot = tuiRong.GetChild(i);
            dragons[i].transform.SetParent(slot,false); // Đặt rồng vào ô trống
            dragons[i].transform.position = slot.transform.position; // Đặt lại vị trí
        }
        sapXepGiamDan = !sapXepGiamDan;
    }


    public static GameObject GetObjRong(string s)
    {
      //  debug.Log("sao rong " + sao) ;
        // return Resources.Load("GameData/Dragon/" + s) as GameObject;
        GameObject obj = DownLoadAssetBundle.bundleDragon.LoadAsset("Assets/ChuaSuDung/Dragon/"+s+ ".prefab") as GameObject;//Resources.Load("GameData/Dragon/"+s) as GameObject;
       // debug.Log(obj.name);
        if (obj != null)
        {
            return obj;
        }
        return Resources.Load("GameData/DragonChuaBuild/"+s) as GameObject;
    }
    public GameObject GetEffect(string name)
    {
        return Resources.Load("GameData/Effect/"+ name) as GameObject;
    }
    public GameObject GetObj(string name)
    {
        return Resources.Load("GameData/Object/" + name) as GameObject;
    }
    public static Sprite LoadSpriteRong(string s, short sao = -1)
    {
        if (sao != -1)
        {
            if (s == "RongRua2" && sao >= 30)
            {
                s = "RongRua3";
            }
        }

        // Tải texture từ asset bundle
        Texture2D tex = DownLoadAssetBundle.bundleDragon.LoadAsset<Texture2D>(s);
        Sprite sprite = null;

        if (tex != null)
        {
            // Tạo sprite từ texture nếu tồn tại
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprite.name = tex.name;
        }
        else
        {
            // Xử lý khi không tìm thấy texture
            if (s.Contains("GiapThin"))
            {
                string newStr = s.Replace("GiapThin", "");
                tex = DownLoadAssetBundle.bundleDragon.LoadAsset<Texture2D>(newStr);
            }
            else if (s.Contains("Valentine"))
            {
                string newStr = s.Replace("Valentine", "");
                tex = DownLoadAssetBundle.bundleDragon.LoadAsset<Texture2D>(newStr);
            }
            if (tex == null)
            {
                tex = DownLoadAssetBundle.bundleDragon.LoadAsset<Texture2D>(s + "1"); //cho thêm 1 để thử load ra baby
            }
            if (tex != null)
            {
                // Tạo sprite từ texture sau khi xử lý
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                sprite.name = tex.name;
            }
            else
            {
                // Trả về sprite mặc định nếu không tìm thấy texture
                sprite = Resources.Load<Sprite>("GameData/Sprite/Default");

                if (sprite == null)
                {
                    Debug.LogError("Sprite mặc định không tồn tại ở đường dẫn GameData/Sprite/Default");
                }
            }
        }

        // Trả về sprite (đã tìm thấy hoặc mặc định)
        return sprite;
    }


    public static Sprite LoadSprite(string s)
    {
        // Tải sprite từ Resources
        Sprite sprite = Resources.Load<Sprite>("GameData/Sprite/" + s);

        // Nếu sprite không tồn tại, trả về sprite mặc định
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>("GameData/Sprite/Default");
        }

        return sprite;
    }
    public static RuntimeAnimatorController LoadAnimator(string name)
    {
        RuntimeAnimatorController anim = Resources.Load<RuntimeAnimatorController>("GameData/Animator/" + name);
        return anim;
    }
    public static GameObject LoadObjectResource(string s)
    {
        return Resources.Load(s) as GameObject;
    }
    public GameObject ObjectRong(string s)
    {
        GameObject load = null;
        if(s.Contains("RongDat"))
        {
            load = LoadRongCoBan("RongDat");
        }
        else if(s.Contains("RongCay"))
        {
            load = LoadRongCoBan("RongCay");
        }
        else if (s.Contains("RongSam"))
        {
            load = LoadRongCoBan("RongSam");
        }
        else if (s.Contains("RongLua") && s.Contains("RongLuaMatXanh") == false) 
        {
            load = LoadRongCoBan("RongLua");
        }
        else if (s.Contains("RongBang"))
        {
            load = LoadRongCoBan("RongBang");
        }
        else
        {
            load = Resources.Load("GameData/Rong/" + s) as GameObject;
        }
        return load;
    }
    public GameObject LoadRongCoBan(string name)
    {
        GameObject g = Resources.Load("GameData/Rong/" + name) as GameObject;
        return g;
    }
    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
  
        crGame = GetComponent<CrGame>();
        net = GetComponent<NetworkManager>();
        Tuidangmo = Otuithuong;
        thuyen = crGame.GetComponent<ThuyenThucAn>();
    }
    public void DoiRong(string rongdoi)// vitriidrongdoi + idrongdoi
    {
        net.socket.Emit("DoiRong", JSONObject.CreateStringObject(rongdoi));
    }
    //public void AddItemThucAn(string nameitem, int soluong)
    //{
    //    for (int j = 0; j < crGame.tuithucAn.transform.childCount; j++)
    //    {
    //        if (nameitem == crGame.tuithucAn.transform.GetChild(j).name)
    //        {
    //            crGame.tuithucAn.transform.GetChild(j).gameObject.SetActive(true);
    //            ThucAn thucAn = crGame.tuithucAn.transform.GetChild(j).transform.GetChild(0).gameObject.GetComponent<ThucAn>();
    //        }
    //    }
    //}
    public void AddThuyen(string nameThuyen)
    {
        
        for (int j = 0; j < thuyen.contentTau.transform.childCount; j++)
        {
            if (nameThuyen == thuyen.contentTau.transform.GetChild(j).name)
            {
                thuyen.contentTau.transform.GetChild(j).gameObject.SetActive(true);
                break;
            }
        }
    }
    public void AddItemGiap(string namegiap,int soluongadd = 1)
    {
        if (soluongadd > 0)
        {
            GameObject Slot = Instantiate(slotgiap, transform.position, Quaternion.identity);
            Slot.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = soluongadd.ToString();
            Image imgNgoc = Slot.transform.GetChild(0).GetComponent<Image>();
            imgNgoc.sprite = LoadSprite(namegiap);
            Slot.name = namegiap;
       //     imgNgoc.SetNativeSize();
            Slot.transform.SetParent(contentGiap.transform, false);
            Slot.SetActive(true);
            maxtrangtuigiap = contentGiap.transform.childCount / 12 + 1;
        }
        else
        {
            for (int i = 0; i < Mathf.Abs(soluongadd); i++)
            {
                if(contentGiap.transform.Find(namegiap)) Destroy(contentGiap.transform.Find(namegiap).gameObject);
            }
        }
        //if (contentGiap.transform.childCount == 0)
        //{
          
        //    return;
        //}
        //else
        //{
        //    //GameObject Slot = Instantiate(SlotNgoc, transform.position, Quaternion.identity);
        //    //Slot.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = soluongadd.ToString();
        //    //Image imgNgoc = Slot.transform.GetChild(0).GetComponent<Image>();
        //    //imgNgoc.sprite = LoadSprite(namegiap);
        //    //Slot.name = namegiap;
        //    //imgNgoc.SetNativeSize();
        //    //Slot.transform.SetParent(contentGiap.transform, false);
        //    //Slot.SetActive(true);
        //    //maxtrangtuigiap = contentGiap.transform.childCount / 12 + 1;
        //}
    }
    public void AddItem(string nameitem, int soluong)
    {
        if (soluong != 0 && nameitem != "Exp")
        {
            if(nameitem.Contains("Giap"))
            {
                AddItemGiap(nameitem,soluong);
               // return;
            }    
            else if(nameitem.Contains("ngoc"))
            {
                AddNgoc(nameitem,soluong);
                return;
            }
            //if (nameitem == "Exp")
            //{
            //    //crGame.StartCoroutine(crGame.LoadExp(soluong,crGame.MaxExp));
            //}
            if(nameitem != "Vang" && nameitem != "KimCuong" && nameitem != "DanhVong")
            {
                if (ListItemThuong.ContainsKey("item" + nameitem))
                {
                    GameObject itemm = ListItemThuong["item" + nameitem];
                    Text txtsoluong = itemm.transform.GetChild(0).GetComponent<Text>();
                    int newsoluong = int.Parse(txtsoluong.text) + soluong;
                    txtsoluong.text = newsoluong + "";
                    if (nameitem.Contains("ThucAn")) AddThucAn(nameitem, true);
                    if (newsoluong <= 0)
                    {
                        ListItemThuong.Remove("item" + nameitem);
                        Destroy(itemm);
                        if (nameitem.Contains("ThucAn"))
                        {
                            thuyen.ImageThucAn.enabled = false;
                            AddThucAn(nameitem, false);
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < Otuithuong.transform.childCount; i++)
                    {
                        if (Otuithuong.transform.GetChild(i).transform.childCount == 0)
                        {
                            GameObject item = null;
                            if (nameitem.Contains("BinhExpLevel"))
                            {
                             
                                if(nameitem.Length > 12)
                                {
                                    item = Instantiate(LoadObjectResource("GameData/Item/BinhExpLevel"), Otuithuong.transform.GetChild(i).transform.position, Quaternion.identity);
                                    string[] cat = nameitem.Split('-');
                                    item.transform.Find("txttag").GetComponent<TextMeshProUGUI>().text = cat[1] + "\n-\n" + cat[2];
                                    item.GetComponent<ItemExp>().Value = nameitem;
                                }    
                                else
                                {
                                    int number = int.Parse(crGame.txtLevel.text);
                                    int roundedNumber = number - (number % 10);
                                    nameitem = "BinhExpLevel-" + roundedNumber + "-" + (roundedNumber + 10);
                                    if (ListItemThuong.ContainsKey(nameitem))
                                    {
                                        item = Instantiate(LoadObjectResource("GameData/Item/BinhExpLevel"), Otuithuong.transform.GetChild(i).transform.position, Quaternion.identity);
                                        debug.Log("inventory add item binhexp name: " + nameitem);
                                        item.transform.Find("txttag").GetComponent<TextMeshProUGUI>().text = roundedNumber + "\n-\n" + (roundedNumber + 10);
                                        item.GetComponent<ItemExp>().Value = nameitem;
                                    }
                                    else AddItem(nameitem, soluong);
                                }
                            } 
                            else item = Instantiate(LoadObjectResource("GameData/Item/" + nameitem), Otuithuong.transform.GetChild(i).transform.position, Quaternion.identity);
                            item.transform.SetParent(Otuithuong.transform.GetChild(i).transform);
                            ScaleObject(item, 0.5f, 0.5f);
                            item.name = "item" + nameitem;
                            if (item.transform.GetChild(0).GetComponent<Text>())
                            {
                                Text txtsoluong = item.transform.GetChild(0).GetComponent<Text>();
                                txtsoluong.text = soluong + "";
                            }
                            ListItemThuong.Add("item" + nameitem, item);
                            if (nameitem.Contains("ThucAn"))
                            {
                                AddThucAn(nameitem);
                            }
                            break;
                        }
                        else if (i == Otuithuong.transform.childCount - 1)
                        {
                            addMaxSlot("tuithuong"); break;
                        }
                    }
                    if (nameitem == "BinhTieuHoa30p" || nameitem == "BinhTieuHoa1h" || nameitem == "BinhTieuHoa3h")
                    {
                        GameObject objtuibinhtieuhoa = crGame.tuithucAn.transform.parent.transform.parent.transform.parent.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).gameObject;
                        objtuibinhtieuhoa.transform.Find(nameitem).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public void AddNgoc(string namengoc,int soluongadd)
    {
        if(contentNgoc.transform.childCount == 0)
        {
            if (soluongadd <= 0) return;
            GameObject Slot = Instantiate(SlotNgoc, transform.position, Quaternion.identity);
            Slot.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = soluongadd.ToString();
            Image imgNgoc = Slot.transform.GetChild(0).GetComponent<Image>();
            imgNgoc.sprite = LoadSprite(namengoc);
            Slot.name = namengoc;
            imgNgoc.SetNativeSize();
            Slot.transform.SetParent(contentNgoc.transform, false);
            Slot.SetActive(true);
            MaxTrangNgoc = contentNgoc.transform.childCount / 12 + 1;
            return;
        }
        else
        {
            for (int i = 0; i < contentNgoc.transform.childCount; i++)
            {
                if (namengoc == contentNgoc.transform.GetChild(i).name)
                {
                    Text txtsoluong = contentNgoc.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    int soluongngoc = int.Parse(txtsoluong.text);
               
                    soluongngoc += soluongadd;
                    debug.Log("Soluong ngoc " + soluongngoc);
                    txtsoluong.text = soluongngoc.ToString();
                    if (soluongngoc <= 0)
                    {
                        Destroy(contentNgoc.transform.GetChild(i).gameObject);
                    }
                    break;
                }
                else if (i == contentNgoc.transform.childCount - 1)
                {
                    GameObject Slot = Instantiate(SlotNgoc, transform.position, Quaternion.identity);
                    Slot.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = soluongadd.ToString();
                    Image imgNgoc = Slot.transform.GetChild(0).GetComponent<Image>();
                    imgNgoc.sprite = LoadSprite(namengoc);
                    Slot.name = namengoc;
                    imgNgoc.SetNativeSize();
                    Slot.transform.SetParent(contentNgoc.transform, false);
                    Slot.SetActive(true);
                    MaxTrangNgoc = contentNgoc.transform.childCount / 12 + 1;
                    break;
                }
            }
        }    
      
    }
    void AddThucAn(string nameitem,bool a = true)
    {
        for (int j = 0; j < crGame.tuithucAn.transform.childCount; j++)
        {
            if (nameitem == crGame.tuithucAn.transform.GetChild(j).name)
            {
                crGame.tuithucAn.transform.GetChild(j).gameObject.SetActive(a);
                break;
            }
        }
    }
    public void XemRongBan(string nameRong,string sao,string id,Image img)
    {
        //  net.socket.Emit("xemGiaRongBan", JSONObject.CreateStringObject(nameRong + "-" + sao));
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "xemGiaBanRong";
        datasend["data"]["sao"] = sao;
        datasend["data"]["name"] = nameRong;

        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];

                BanRong banrong = AllMenu.ins.GetCreateMenu("MenuBanRong", GameObject.Find("CanvasTrenCung")).GetComponent<BanRong>();
                string[] cat = json.AsString.Split('-');
                banrong.txtGiaBan.text = cat[1];
                banrong.TxtTenRong.text = cat[0];
                banrong.idrongban = id;
                banrong.imgRongBan.sprite = img.sprite;//LoadSprite(cat[2] + tienhoa);
                banrong.imgRongBan.SetNativeSize();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
    }
    public void BanRong(string id)
    {
        net.socket.Emit("BanRong",JSONObject.CreateStringObject(id));
    }
    public void RemoveItem(string nameitem,int soluongremove)
    {
        for (int i = 0; i < Otuithuong.transform.childCount; i++)
        {
            if (Otuithuong.transform.GetChild(i).transform.childCount > 0)
            {
                if (Otuithuong.transform.GetChild(i).transform.GetChild(0).name == "item" + nameitem)
                {
                    if (Otuithuong.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0))
                    {
                        Text txtsoluong = Otuithuong.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                        float newsoluong = float.Parse(txtsoluong.text) - soluongremove;
                        txtsoluong.text = newsoluong + "";
                        if (newsoluong <= 0)
                        {
                            ListItemThuong.Remove("item" + nameitem);
                            Destroy(Otuithuong.transform.GetChild(i).transform.GetChild(0).gameObject);
                        }
                    }
                    break;
                }
            }
        }
    }
    public int GetItem(string nameitem)
    {
        string s = "0";
        if(ListItemThuong.ContainsKey("item" + nameitem))
        {
            GameObject item = ListItemThuong["item" + nameitem] as GameObject;
            Text txt = item.transform.GetChild(0).GetComponent<Text>();
            s = txt.text;
        }
        return int.Parse(s);
    }
    #region SlotTui
    public void LoadSlotRong(int SlotL)
    {
        int n = 0;
        for (int i = 0; i < SlotL; i++)
        {
            n++;
            GameObject ORong = Instantiate(OSlot,TuiRong.transform.position, Quaternion.identity) as GameObject;
            ORong.transform.SetParent(TuiRong.transform);
            ORong.name = "ORong" + i;
            if (TuiRong.transform.childCount == 1)
            {
                ORong.transform.SetSiblingIndex(0);
            }
            else
            {
                ORong.transform.SetSiblingIndex(TuiRong.transform.childCount - 2);
            }
            ScaleObject(ORong,1.04f,1.02f);
            if(n <= 12)
            {
                ORong.SetActive(true);
            }
        }
        MaxTrangRong = SlotL / 12 + 1;
     //   txtTrang.text = trang+ "/" + SoTrangRong;
    }

    public void addMaxSlot(string s)
    {
        if (s == "tuirong")
        {
            GameObject ORong = Instantiate(OSlot,TuiRong.transform.position, Quaternion.identity) as GameObject;
            ORong.transform.SetParent(TuiRong.transform);
            ScaleObject(ORong, 1.04f, 1.02f);
            ORong.transform.SetSiblingIndex(TuiRong.transform.childCount - 2);
          //  ORong.name = "hahaha";
            ORong.SetActive(true);
            MaxTrangRong = TuiRong.transform.childCount / 12 + 1;
            txtTrang.text = trangtuirong + "/" + MaxTrangRong;
            loadbuttonsang(trangtuirong, MaxTrangRong);
        }
        else
        {
            GameObject ORong = Instantiate(OSlot, Otuithuong.transform.position, Quaternion.identity) as GameObject;
            ORong.transform.SetParent(Otuithuong.transform);
            ScaleObject(ORong, 1.04f, 1.02f);
            ORong.transform.SetSiblingIndex(Otuithuong.transform.childCount - 2);
            ORong.SetActive(true);
            MaxTrangtuithuong = Otuithuong.transform.childCount / 12 + 1;
            txtTrang.text = trangtuithuong + "/" + MaxTrangtuithuong;
            loadbuttonsang(trangtuithuong, MaxTrangtuithuong);
            //    ORong.name = "hahaha";
        }

        if(AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
    }
    string oslotnao;
    public void OpenMenuAddSlot(string s)
    {
        oslotnao = s;
        XemGiaMaxSlot();
    }
    public void XemGiaMaxSlot()
    {
        int soslothientai = 0;
        if(oslotnao == "tuirong")
        {
            soslothientai = TuiRong.transform.childCount;
        }
        else
        {
            soslothientai = Otuithuong.transform.childCount;
        }

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "xemGiaAddSlot";
        datasend["data"]["cap"] = soslothientai.ToString();
        datasend["data"]["tui"] = oslotnao;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();

                tbc.txtThongBao.text = json["message"].AsString;
                tbc.btnChon.onClick.AddListener(addMaxSlot);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
            }
       
        }
    }
    public void addMaxSlot()
    {
        net.socket.Emit("AddSlot",JSONObject.CreateStringObject(oslotnao));
    }
    public void LoadSlotTuiThuong(int slot)
    {
        int n = 0;
        for (int i = 0; i < slot; i++)
        {
            n++;
            GameObject ORong = Instantiate(OSlot, Otuithuong.transform.position, Quaternion.identity) as GameObject;

            ORong.transform.SetParent(Otuithuong.transform);
            if (Otuithuong.transform.childCount == 1)
            {
                ORong.transform.SetSiblingIndex(0);
            }
            else
            {
                ORong.transform.SetSiblingIndex(Otuithuong.transform.childCount - 2);
            }
            ScaleObject(ORong, 1.04f, 1.02f);
            if (n <= 12)
            {
                ORong.SetActive(true);
            }
        }
        MaxTrangtuithuong = slot / 12 + 1;

        txtTrang.text = trangtuithuong + "/" + MaxTrangtuithuong;
    }
    public void Sangtui(string s)
    {
        if(s == "tuido")
        {
            TatTui(true, false, false,false);
            txtTrang.text = trangtuithuong + "/" + MaxTrangtuithuong;
            Tuidangmo = Otuithuong;
            loadbuttonsang(trangtuithuong, MaxTrangtuithuong);
            txtTenTui.text = "Túi chứa vật phẩm";
        }
        else if(s == "tuirong")
        {
            TatTui(false, true, false,false);
            txtTrang.text = trangtuirong + "/" + MaxTrangRong;
            Tuidangmo = TuiRong;
            loadbuttonsang(trangtuirong, MaxTrangRong);
            txtTenTui.text = "Túi chứa rồng";
        }
        else if (s == "tuingoc")
        {
            TatTui(false,false,true,false);
            txtTrang.text = trangtuingoc + "/" + MaxTrangNgoc;
            Tuidangmo = contentNgoc;
            loadbuttonsang(trangtuingoc, MaxTrangNgoc);
            txtTenTui.text = "Túi chứa ngọc";
        }
        else if(s == "tuigiap")
        {
            TatTui(false, false, false, true);
            txtTrang.text = trangttuigiap + "/" + maxtrangtuigiap;
            Tuidangmo = contentGiap;
            loadbuttonsang(trangttuigiap, maxtrangtuigiap);
            txtTenTui.text = "Túi chứa giáp";
        }    
    }
    void TatTui(bool a,bool b,bool c,bool d)
    {
        tuido.SetActive(a);
        tuirong.SetActive(b);
        tuingoc.SetActive(c);
        TuiGiap.SetActive(d);
    }    
    public void Sangtrang(bool phai)
    {
        for (int i = 0; i < Tuidangmo.transform.childCount; i++)
        {
            if (Tuidangmo.transform.GetChild(i).gameObject.activeSelf)
            {
                Tuidangmo.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        int childCount = Tuidangmo.transform.childCount;
        if (phai)
        {
            if(Tuidangmo.name == "ContentTui")
            {
                for (int i = trangtuithuong * 12; i < trangtuithuong * 12 + 12; i++)
                {
                    if (i < childCount)
                    {
                        Tuidangmo.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                trangtuithuong++;
                loadbuttonsang(trangtuithuong, MaxTrangtuithuong);
                txtTrang.text = trangtuithuong + "/" + MaxTrangtuithuong;
            }
            else if (Tuidangmo.name == "ContentRong")
            {
                for (int i = trangtuirong * 12; i < trangtuirong * 12 + 12; i++)
                {
                    if (i < childCount)
                    {
                        Tuidangmo.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                trangtuirong++;
                loadbuttonsang(trangtuirong, MaxTrangRong);
                txtTrang.text = trangtuirong + "/" + MaxTrangRong;
            }
            else if (Tuidangmo.name == "ContentNgoc")
            {
                for (int i = trangtuingoc * 12; i < trangtuingoc * 12 + 12; i++)
                {
                    if (i < childCount)
                    {
                        Tuidangmo.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                trangtuingoc++;
                loadbuttonsang(trangtuingoc, MaxTrangNgoc);
                txtTrang.text = trangtuingoc + "/" + MaxTrangNgoc;
            }
            else if(Tuidangmo.name == "ContentGiap")
            {
                for (int i = trangttuigiap * 12; i < trangttuigiap * 12 + 12; i++)
                {
                    if (i < childCount)
                    {
                        Tuidangmo.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                trangttuigiap++;
                loadbuttonsang(trangttuigiap, maxtrangtuigiap);
                txtTrang.text = trangttuigiap + "/" + maxtrangtuigiap;
            }
        }
        else
        {
            int trang = 0;
            if (Tuidangmo.name == "ContentTui")
            {
                trangtuithuong--;
                loadbuttonsang(trangtuithuong, MaxTrangtuithuong);
                txtTrang.text = trangtuithuong + "/" + MaxTrangtuithuong;
                trang = trangtuithuong;
            }
            else if(Tuidangmo.name == "ContentRong")
            {
                trangtuirong--;
                loadbuttonsang(trangtuirong, MaxTrangRong);
                txtTrang.text = trangtuirong + "/" + MaxTrangRong;
                trang = trangtuirong;
            }
            else if(Tuidangmo.name == "ContentNgoc")
            {
                trangtuingoc--;
                loadbuttonsang(trangtuingoc, MaxTrangNgoc);
                txtTrang.text = trangtuingoc + "/" + MaxTrangNgoc;
                trang = trangtuingoc;
            }
            else if(Tuidangmo.name == "ContentGiap")
            {
                trangttuigiap--;
                loadbuttonsang(trangttuigiap, maxtrangtuigiap);
                txtTrang.text = trangttuigiap + "/" + maxtrangtuigiap;
                trang = trangttuigiap;
            }
            for (int i = trang * 12 - 1; i >= trang * 12 - 12; i--)
            {
                if(i < childCount)
                {
                    Tuidangmo.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
   
    }
    void loadbuttonsang(int trangtui,int maxtrang)
    {
        if (trangtui == maxtrang)
        {
            btnsangphai.SetActive(false);
            btnsangphaicuoi.SetActive(false);
        }
        else
        {
            btnsangphai.SetActive(true);
            btnsangphaicuoi.SetActive(true);
        }
        if (trangtui == 1)
        {
            btnsangtrai.SetActive(false);
            btnsangtraicuoi.SetActive(false);
        }
        else
        {
            btnsangtrai.SetActive(true);
            btnsangtraicuoi.SetActive(true);
        }
    }
    public void SangCuoiTrang(bool phai)
    {
        for (int i = 0; i < Tuidangmo.transform.childCount; i++)
        {
            if (Tuidangmo.transform.GetChild(i).gameObject.activeSelf)
            {
                Tuidangmo.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (phai)
        {
            if (Tuidangmo.name == "ContentTui")
            {
                trangtuithuong = MaxTrangtuithuong;
                txtTrang.text = trangtuithuong + "/" + MaxTrangtuithuong;
            }
            if (Tuidangmo.name == "ContentRong")
            {
                trangtuirong = MaxTrangRong;
                txtTrang.text = trangtuirong + "/" + MaxTrangRong;
            }
            if (Tuidangmo.name == "ContentNgoc")
            {
                trangtuingoc = MaxTrangNgoc;
                txtTrang.text = trangtuingoc + "/" + MaxTrangNgoc;
            }
            for (int i = Tuidangmo.transform.childCount - 1; i >= Tuidangmo.transform.childCount - 12; i--)
            {
                Tuidangmo.transform.GetChild(i).gameObject.SetActive(true);
            }
            btnsangphai.SetActive(false);
            btnsangphaicuoi.SetActive(false);
            btnsangtrai.SetActive(true);
            btnsangtraicuoi.SetActive(true);
        }
        else
        {
            if (Tuidangmo.name == "ContentTui")
            {
                trangtuithuong = 1;
                txtTrang.text = trangtuithuong + "/" + MaxTrangtuithuong;
            }
            if (Tuidangmo.name == "ContentRong")
            {
                trangtuirong = 1;
                txtTrang.text = trangtuirong + "/" + MaxTrangRong;
            }
            if(Tuidangmo.name == "ContentNgoc")
            {
                trangtuingoc = 1;
                txtTrang.text = trangtuingoc + "/" + MaxTrangNgoc;
            }
            for (int i = 0; i < 12; i++)
            {
                Tuidangmo.transform.GetChild(i).gameObject.SetActive(true);
            }
            btnsangtrai.SetActive(false);
            btnsangtraicuoi.SetActive(false);
            btnsangphai.SetActive(true);
            btnsangphaicuoi.SetActive(true);
        }
    }
    #endregion

    private void SeticonNgoc(GameObject item, JSONObject allngoc,bool hoangkim)
    {
        debug.Log(allngoc.ToString()) ;
        GameObject objngoc = item.transform.Find("allngoc").gameObject;
        byte ngoc = 0;
        for (int i = 0; i < allngoc.Count; i++)
        {
            string namengoc = GamIns.CatDauNgoacKep(allngoc[i]["name"].ToString());
            if (namengoc!= "lock" && namengoc != "")
            {
                Image imgngoc = objngoc.transform.GetChild(ngoc).GetComponent<Image>();
                imgngoc.gameObject.SetActive(true);
                imgngoc.sprite = Inventory.LoadSprite("icon" + namengoc.Split("-")[0]);
                ngoc += 1;
            }
        }
        if(hoangkim)
        {
            item.transform.Find("hoangkim").gameObject.SetActive(true);
        }    
    }
    private void SeticonNgoc(GameObject item, JSONNode allngoc, bool hoangkim)
    {
        debug.Log(allngoc.ToString());
        GameObject objngoc = item.transform.Find("allngoc").gameObject;
        byte ngoc = 0;
        for (int i = 0; i < allngoc.Count; i++)
        {
            string namengoc = allngoc[i]["name"].AsString;
            if (namengoc != "lock" && namengoc != "")
            {
                Image imgngoc = objngoc.transform.GetChild(ngoc).GetComponent<Image>();
                imgngoc.gameObject.SetActive(true);
                imgngoc.sprite = Inventory.LoadSprite("icon" + namengoc.Split("-")[0]);
                ngoc += 1;
            }
        }
        if (hoangkim)
        {
            item.transform.Find("hoangkim").gameObject.SetActive(true);
        }
    }
    public void AddItemRong(string iditem, string nameItem, byte sao, int level, int exp, int maxexp, byte tienhoa, float sothucan, string tenrong, string nameObject, bool hoangkim, JSONNode allngoc,bool lockk)
    {
        for (int i = 0; i < TuiRong.transform.childCount; i++)
        {
            if (TuiRong.transform.GetChild(i).transform.childCount == 0)
            {
                GameObject itemRong = Instantiate(ItemRong, TuiRong.transform.GetChild(i).transform.position, Quaternion.identity);
                itemRong.transform.SetParent(TuiRong.transform.GetChild(i).transform);

                itemRong.name = iditem;
                Image imgRong = itemRong.transform.GetChild(0).GetComponent<Image>();
                string[] Dohiemcuarong = nameItem.Split('-');
                imgRong.sprite = LoadSpriteRong(nameObject + tienhoa,sao);
                if (Dohiemcuarong[1] != "")
                {
                    Text txtHiem = imgRong.transform.GetChild(0).GetComponent<Text>();
                    txtHiem.text = DoHiemCuaRong(Dohiemcuarong[1]);
                }
                imgRong.SetNativeSize();
                ItemDragon idra = itemRong.GetComponent<ItemDragon>();
                idra.nameObjectDragon = nameObject;
                idra.txtSao.text = sao.ToString();
                //idra.LoadRong();
                ScaleObject(itemRong, 1, 1);
                itemRong.SetActive(true);
                SeticonNgoc(itemRong,allngoc,hoangkim);
                if (lockk) itemRong.transform.Find("lock").gameObject.SetActive(true);
                break;
            }
        }
    }
    public void AddItemRong(string iditem,string nameItem,byte sao,int level,int exp,int maxexp,byte tienhoa,float sothucan,string tenrong,string nameObject, bool hoangkim, JSONObject allngoc,bool lockk)
    {
        for (int i = 0; i < TuiRong.transform.childCount; i++)
        {
            if (TuiRong.transform.GetChild(i).transform.childCount == 0)
            {
                GameObject itemRong = Instantiate(ItemRong, TuiRong.transform.GetChild(i).transform.position, Quaternion.identity);
                itemRong.transform.SetParent(TuiRong.transform.GetChild(i).transform);

                itemRong.name = iditem;
                Image imgRong = itemRong.transform.GetChild(0).GetComponent<Image>();
                string[] Dohiemcuarong = nameItem.Split('-');
                imgRong.sprite = LoadSpriteRong(nameObject + tienhoa,sao);
                if (Dohiemcuarong[1] != "")
                {
                    Text txtHiem = imgRong.transform.GetChild(0).GetComponent<Text>();
                    txtHiem.text = DoHiemCuaRong(Dohiemcuarong[1]);
                }
                imgRong.SetNativeSize();
                ItemDragon idra = itemRong.GetComponent<ItemDragon>();
                idra.nameObjectDragon = nameObject;
                idra.txtSao.text = sao.ToString();
                //idra.LoadRong();
                ScaleObject(itemRong,1,1);
                itemRong.SetActive(true);
                SeticonNgoc(itemRong,allngoc,hoangkim);
                if (lockk) itemRong.transform.Find("lock").gameObject.SetActive(true);
                break;
            }
        }
    }
    public void AddItemRongByindex(int indexchild, string iditem, string nameItem, byte sao, int level, double exp, double maxexp, byte tienhoa, float sothucan, string tenrong, string nameObject,bool hoangkim, JSONObject allngoc, bool lockk)
    {
        GameObject itemRong = Instantiate(ItemRong, TuiRong.transform.GetChild(indexchild).transform.position, Quaternion.identity) as GameObject;
        itemRong.transform.SetParent(TuiRong.transform.GetChild(indexchild).transform);
        itemRong.name = iditem;
        Image imgRong = itemRong.transform.GetChild(0).GetComponent<Image>();
        string[] Dohiemcuarong = nameItem.Split('-');
        if (Dohiemcuarong[1] != "")
        {
            Text txtHiem = imgRong.transform.GetChild(0).GetComponent<Text>();
            txtHiem.text = DoHiemCuaRong(Dohiemcuarong[1]);
        }
        imgRong.sprite = LoadSpriteRong(nameObject + tienhoa,sao);
        imgRong.SetNativeSize();
        ItemDragon idra = itemRong.GetComponent<ItemDragon>();
        idra.nameObjectDragon = nameObject;
        idra.txtSao.text = sao.ToString();
        // idra.LoadRong();
        ScaleObject(itemRong, 1, 1);
        itemRong.SetActive(true);
        SeticonNgoc(itemRong, allngoc, hoangkim);
        if (lockk) itemRong.transform.Find("lock").gameObject.SetActive(true);
    }
    public static string DoHiemCuaRong(string s)
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
    public void ScaleObject(GameObject g,float x,float y)
    {
        Vector3 Scale;
        Scale = g.transform.localScale;
        Scale.x = x; Scale.y = y;
        g.transform.localScale = Scale;
    }


    public void DoiTenRong()
    {
        InputField inputname = AllMenu.ins.menu["PanelDoiTenRong"].transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponent<InputField>();
        if (inputname.text != "")
        {
            //DragonController dra = crGame.TfrongInfo.gameObject.GetComponent<DragonController>();
            //net.socket.Emit("doitenrong", JSONObject.CreateStringObject(dra.name + "-" + inputname.text));

            AudioManager.PlaySound("soundClick");
            // Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
            // int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
            JSONClass datasend = new JSONClass();
            datasend["class"] = "DragonIsland";
            datasend["method"] = "DoiTenRong";
            datasend["data"]["idrong"] = CrGame.ins.TfrongInfo.gameObject.name;
            datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
            datasend["data"]["namenew"] = inputname.text;
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    // inventory.MenuDoiTenRong.SetActive(false);
                    AllMenu.ins.DestroyMenu("PanelDoiTenRong");
                  //  DragonController dra = CrGame.ins.TfrongInfo.gameObject.GetComponent<DragonController>();
                    DragonIslandController dra = CrGame.ins.TfrongInfo.gameObject.GetComponent<DragonIslandController>();
                  //  if (json["hoangkim"].AsString != "undefined") hoangkim = true;
                    dra.SetNameRong = DragonIslandManager.ParseName(json["namenew"].AsString, json["hoangkim"].AsBool, json["sao"].AsString);
                    // dra.tenrong = json["namenew"].AsString;
                    //dra.txtnamerong.text = dra.tenrong + " (" + dra.sao + " sao)";
                    //dra.txtnamerong.gameObject.SetActive(true);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }

    }
}
