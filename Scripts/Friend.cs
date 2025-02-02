
using SimpleJSON;

using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    public CrGame crgame; public bool QuaNha = false;
     public NetworkManager net; public byte DangODaoFriend;
    public GameObject DaoFriend; public string nameFriend,idFriend; public GameObject GiaoDienFriend, GiaoDienMinh; 
    public Image imgAvatarFriend, ThanhExp, KhungAvatar; public Text txtTienVang, txtNameFriend, txtlevel;
    int soluong = 1; public int MaxSoluong; string nameitemtang; public string idrongtang;// Tang qua
    VienChinh vienchinh; public GameObject GiaoDienNgoai; Inventory inventory;
    public JSONNode DataDao = null;
    public static Friend ins;
    public float timeNuiThanBi;
    public static NuiLua nuiluaFriend;
    // Start is called before the first frame update
    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        ins = this;
        vienchinh = GameObject.FindGameObjectWithTag("vienchinh").GetComponent<VienChinh>();
        inventory = GetComponent<Inventory>();
        crgame = GetComponent<CrGame>();
    }

    private void Update()
    {
        if (timeNuiThanBi > 0)
        {
            timeNuiThanBi -= Time.deltaTime;
        }
    }
    public void SoloChienTuong()
    {
        if (Input.touchCount > 1) return;
        crgame.panelLoadDao.SetActive(true);
        crgame.giaodien.SetActive(false);
        vienchinh.enabled = true;
        GiaoDienNgoai.SetActive(false);
        net.socket.Emit("solochientuong", JSONObject.CreateStringObject(txtNameFriend.text));
    }
  
    #region TangQua
    public void XemTangQua(string name, string idrong = "")
    {
        nameitemtang = name;
        idrongtang = idrong;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "xemGiaoDich";
        datasend["data"]["name"] = name + "*" + txtNameFriend.text;

        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];

                string[] cat = json.AsString.Split('*');
                if (cat[0] == "true")
                {
                    GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua", null, false);
                    Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                    Text txtnameTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
                    InputField inputsoluong = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<InputField>();
                    imgItemTang.SetNativeSize();
                    txtnameTang.text = cat[1];
                    soluong = 1;
                    inputsoluong.text = "1";
                    Button btntangsoluong = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<Button>();
                    Button btngiamsoluong = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<Button>();
                    Button btntangqua = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(3).GetComponent<Button>();
                    Button btnexit = menutangqua.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
                    btntangsoluong.onClick.RemoveAllListeners();
                    btngiamsoluong.onClick.RemoveAllListeners();
                    btntangqua.onClick.RemoveAllListeners();
                    btnexit.onClick.RemoveAllListeners();
                    btntangsoluong.onClick.AddListener(delegate { TangSoLuong(1); });
                    btngiamsoluong.onClick.AddListener(delegate { TangSoLuong(-1); });
                    btntangqua.onClick.AddListener(TangQua);
                    btnexit.onClick.AddListener(delegate { AllMenu.ins.DestroyMenu("MenuTangQua"); });
                    inputsoluong.onValueChanged.RemoveAllListeners();
                    inputsoluong.onValueChanged.AddListener(delegate { ChangeSoluong(); });
                    menutangqua.SetActive(true);
                }
                else
                {
                    AllMenu.ins.DestroyMenu("MenuTangQua");
                    crgame.OnThongBao(true, "Không tặng được vật phẩm này.", true);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
    }
    public void ChangeSoluong()
    {
        InputField inputsoluong = AllMenu.ins.menu["MenuTangQua"].transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<InputField>();
        if (int.Parse(inputsoluong.text) > MaxSoluong)
        {
            inputsoluong.text = MaxSoluong.ToString();
        }
        else if (int.Parse(inputsoluong.text) >= 999)
        {
            inputsoluong.text = 999.ToString();
        }
        soluong = int.Parse(inputsoluong.text);
    }
    public void TangSoLuong(int i)
    {
        InputField inputsoluong = AllMenu.ins.menu["MenuTangQua"].transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<InputField>();
        if (i > 0)
        {
            if (soluong < MaxSoluong)
            {
                soluong += 1;
                inputsoluong.text = soluong.ToString();
            }
        }
        else
        {
            if (soluong > 1)
            {
                soluong -= 1;
                inputsoluong.text = soluong.ToString();
            }
        }
    }
    public void TangQua()
    {
       // crgame.panelLoadDao.SetActive(true);
        crgame.OnThongBao(true, "Đang tặng...", false);
        TangQua Infoqua = new TangQua(txtNameFriend.text, soluong, nameitemtang, idrongtang);
        net.socket.Emit("tangqua", new JSONObject(JsonUtility.ToJson(Infoqua)));
    }
    public int quaxem = 0;
    public sbyte quanhan = 0;
    public void NhanQua()
    {
        crgame.OnThongBao(true, "Đang nhận...", false);
        net.socket.Emit("nhanquafriend", JSONObject.CreateStringObject(quanhan.ToString()));
    }
    #endregion
    public void GoHome(bool b = false)
    {
        GameObject Dao = crgame.AllDao.transform.Find("BGDao" + crgame.DangODao).gameObject;//GameObject.Find("BGDao" + crgame.DangODao);
        Dao.SetActive(true);
        DataDao = null;
        
        transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
        crgame.ngaydem.transform.position = Dao.transform.position;
        crgame.ngaydem.transform.SetParent(Dao.transform);
        crgame.ngaydem.transform.SetSiblingIndex(4);
        crgame.Bg.transform.position = Dao.transform.position;
        if (b) nameFriend = "";
        // DaoFriend.transform.GetChild(DangODaoFriend).gameObject.SetActive(false);
        Dao.gameObject.SetActive(true);
        GiaoDienFriend.SetActive(false);
        GiaoDienMinh.SetActive(true);
        DonDepDao();
        crgame.txtVang.gameObject.SetActive(true);
        DaoFriend.SetActive(false);
        QuaNha = false;
        net.socket.Emit("quanhafriend", JSONObject.CreateStringObject("venha"));
        KeLangThangManager.ins.setBtnKLT = true;
        // AllMenu.ins.DestroyMenu("NhaFriend");
        //Clear();
    }
    public void DonDepDao()
    {
        GameObject Daofr = DaoFriend.transform.GetChild(0).Find("RongFriendDao").gameObject;
        for (int i = 3; i < Daofr.transform.childCount; i++)
        {
            Destroy(Daofr.transform.GetChild(i).gameObject);
        }
        for (int i = 1; i < DaoFriend.transform.childCount; i++)
        {
            Destroy(DaoFriend.transform.GetChild(i).gameObject);
        }
    }    
    //public void GetAvatarFriend(string id, Image imgavatar)
    //{
    //    StartCoroutine(DownloadImage());
    //    IEnumerator DownloadImage()
    //    {
    //        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https" + "://graph.facebook.com/" + id + "/picture?type=large");
    //        yield return request.SendWebRequest();
    //        if (request.isNetworkError || request.isHttpError)
    //            debug.Log(request.error);
    //        else
    //        {
    //            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
    //            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
    //            imgavatar.sprite = sprite;
    //        }
    //    }
    //}
    public void LoadImage(string namefolder, string namefile, Image img)
    {
        StartCoroutine(DownloadImage());
        IEnumerator DownloadImage()
        {
            debug.Log("name file animator la: " + namefile);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(LoginFacebook.http + "://" + LoginFacebook.ins.ServerChinh + "/LoadImage/namefolder/" + namefolder + "/name/" + namefile);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                debug.Log(request.error);
            else
            {
              //  debug.Log(request.downloadHandler.text + " " + namefile);
               
                if (request.downloadHandler.text == "animation")
                {
                     namefile = "animator" + namefile.ToLower();
                    StartCoroutine(AssetBundleManager.ins.LoadAssetBundle(namefile,
           onSuccess: (assetBundle) =>
           {
                 RuntimeAnimatorController tmpController = assetBundle.LoadAsset("Khung") as RuntimeAnimatorController;

                 StartCoroutine(addAnim(tmpController));
           },
           onError: (error) => debug.LogError($"Load error: {error}")
       ));

                      IEnumerator addAnim(RuntimeAnimatorController anim)
        {
            Animator animator = null;
            if (!img.GetComponent<Animator>())
            {
                animator = img.gameObject.AddComponent<Animator>();
            }
            else animator = img.GetComponent<Animator>();
            animator.runtimeAnimatorController = anim;
            yield return new WaitUntil(() => animator.runtimeAnimatorController != null);
         //   yield return new WaitForSeconds(0.6f);
            img.SetNativeSize();
        }
                  //  DownLoadAssetBundle.ins.DownAssetBundleAnimator(img, namefile);

                }
                else
                {
                    Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                    if (img == null) yield break;
                    img.sprite = sprite;
                    if (img.GetComponent<Animator>()) Destroy(img.GetComponent<Animator>());
                    if (namefolder == "khungavt")
                    {
                        img.SetNativeSize();
                    }
                }
            }
        }
    }
    void Clear()
    {
        GameObject Dao = DaoFriend.transform.GetChild(0).gameObject;
        DangODaoFriend = 0;
        transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
        crgame.ngaydem.transform.position = Dao.transform.position;
        crgame.ngaydem.transform.SetParent(Dao.transform);
        crgame.ngaydem.transform.SetSiblingIndex(4);
        crgame.Bg.transform.position = Dao.transform.position;

        int i = 0;
        DaoFriend.transform.GetChild(i).gameObject.SetActive(false);
        GameObject RongFriend = crgame.FindObject(DaoFriend.transform.GetChild(i).gameObject, "RongFriendDao");
        for (int j = 3; j < RongFriend.transform.childCount; j++) Destroy(RongFriend.transform.GetChild(j).gameObject);
        GameObject objecttrangtridao = crgame.FindObject(DaoFriend.transform.GetChild(i).gameObject, "TrangTriFriend");
        foreach (Transform child in objecttrangtridao.transform) Destroy(child.gameObject);
        for (int j = 0; j < 4; j++)
        {
            GameObject congtrinhdao = crgame.FindObject(DaoFriend.transform.GetChild(i).gameObject, "CongtrinhFriend");
            CongTrinh ct = congtrinhdao.transform.GetChild(j).GetComponent<CongTrinh>();
            ct.nameCongtrinh = "DatTrong"; ct.levelCongtrinh = 0; ct.LoadImg();
        }

        for (int j = 1; j < DaoFriend.transform.childCount; j++)
        {
            Destroy(DaoFriend.transform.GetChild(j).gameObject);
        }
    }

    //static string Decompress(string compressedString)
    //{
    //    byte[] compressedBytes = Convert.FromBase64String(compressedString);
    //    return System.Text.Encoding.UTF8.GetString(compressedBytes);
    //}
    public string MauNgocNuiThanBi;
    public void QuaNhaFriend()
    {
        //crgame.OnThongBao(true, "Đang tải", false);
        //  GoHome();
        CrGame.ins.panelLoadDao.SetActive(true);
        DataDao = null;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "GetFriend";
        datasend["data"]["idFriend"] = nameFriend;
        datasend["data"]["id"] = LoginFacebook.ins.id;
        datasend["data"]["idfr"] = idFriend;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].Value == "0")
            {
                KeLangThangManager.ins.setBtnKLT = false;
                JSONNode json = jsonn["data"];
                //  DonDepDao();
                net.socket.Emit("quanhafriend", JSONObject.CreateStringObject("quanha"));
                crgame.txtVang.gameObject.SetActive(false);
                QuaNha = true;
                DataDao = json["dao"];
                Clear();
                DaoFriend.SetActive(true);
                int i = 0;
                DaoFriend.transform.GetChild(i).gameObject.SetActive(true);
                //GamIns.ins.SetMinMax();
                Image imgCapDao = crgame.FindObject(DaoFriend.transform.GetChild(i).gameObject, "btnCapDaoFriend").GetComponent<Image>();

                imgCapDao.sprite = Inventory.LoadSprite("Dao" + int.Parse(json["dao"][i]["leveldao"].Value));
                imgCapDao.SetNativeSize();
                debug.Log("friend 0");
                for (int j = 0; j < json["dao"][i]["congtrinh"].Count; j++)
                {
                    GameObject congtrinhdao = crgame.FindObject(DaoFriend.transform.GetChild(i).gameObject, "CongtrinhFriend");
                    CongTrinh ct = congtrinhdao.transform.GetChild(j).GetComponent<CongTrinh>();
                    byte capct = byte.Parse(json["dao"][i]["congtrinh"][j]["cap"].Value);
                    ct.nameCongtrinh = json["dao"][i]["congtrinh"][j]["name"].Value; ct.levelCongtrinh = capct; ct.LoadImg();
                }


                for (int j = 0; j < json["dao"].Count; j++)
                {
                    if (json["dao"][j]["timeNuiThanBi"].ToString() != "")
                    {
                        timeNuiThanBi = json["dao"][j]["timeNuiThanBi"].AsFloat;
                        debug.Log("Time nui than bi dao " + j + ": " + timeNuiThanBi);

                        MauNgocNuiThanBi = json["dao"][j]["namengoc"].AsString;
                        break;

                    }
                }


                debug.Log("friend 1");
                GameObject RongFriend = crgame.FindObject(DaoFriend.transform.GetChild(i).gameObject, "RongFriendDao");
                //for (int j = 3; j < RongFriend.transform.childCount; j++)
                //{
                //    Destroy(RongFriend.transform.GetChild(j).gameObject);
                //}
                for (int j = 0; j < json["dao"][i]["rong"].Count; j++)
                {
                    DragonIslandManager.ParseDragonIsland(json["dao"][i]["rong"][j], (byte)i, Vector3.zero, RongFriend.transform);
                }
                GameObject objecttrangtridao = crgame.FindObject(DaoFriend.transform.GetChild(i).gameObject, "TrangTriFriend");
                //debug.Log("friend 2");
                //foreach (Transform child in objecttrangtridao.transform)
                //{
                //    Destroy(child.gameObject);
                //}
                Transform ObjItemEvent = DaoFriend.transform.GetChild(i).transform.Find("ObjItemEvent");
                if (ObjItemEvent != null) Destroy(ObjItemEvent.gameObject);
                DragonIslandManager.InsAllItemDao(json["dao"][i]["itemEvent"], (byte)i);
                //for (int j = 0; j < json["dao"][i]["itemEvent"].Count; j++)
                //{
                //    GameObject item = DragonIslandManager.InsItemDao(json["dao"][i]["itemEvent"][j], (byte)i);
                //    item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y - Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y));
                //}
                for (int j = 0; j < json["dao"][i]["trangtri"].Count; j++)
                {
                    //string nametrangtri = net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["name"].Value);

                    float x = 0, y = 0;
                    // float y = float.Parse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["y"].Value)) - (Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y) + Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y));

                    float.TryParse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["y"].Value), out y);

                    y -= Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y);

                    float.TryParse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["x"].Value), out x);

                    //  float y = float.Parse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["y"].Value));
                    //        float x = float.Parse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["x"].Value));
                    //  debug.Log("x " + x + " y " + y);
                    // GameObject trangtri = Instantiate(GameObject.Find(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["name"].Value)), new Vector3(float.Parse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["x"].Value)), y), Quaternion.identity) as GameObject;
                    //  System.Math.Round(y, 2);
                    GameObject trangtri = Instantiate(Inventory.LoadObjectResource("GameData/Item/" + net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["name"].Value)), transform.position, Quaternion.identity) as GameObject;
                    Destroy(trangtri.GetComponent<ItemTrangTri>());
                    Destroy(trangtri.GetComponent<EventTrigger>());
                    Destroy(trangtri.GetComponent<Button>());
                    trangtri.transform.SetParent(objecttrangtridao.transform);
                    trangtri.transform.position = new Vector3(x, y, 0);
                    trangtri.transform.GetChild(1).gameObject.SetActive(true);
                    trangtri.GetComponent<Image>().enabled = false;// new Vector3(float.Parse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["x"].Value)), y)
                    trangtri.transform.GetChild(1).transform.position = new Vector3(trangtri.transform.GetChild(1).transform.position.x, trangtri.transform.GetChild(1).transform.position.y, 0);
                    trangtri.transform.GetChild(0).gameObject.SetActive(false);
                    if (net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["name"].Value) == "TocLenh")
                    {
                        trangtri.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator("TocLenh" + net.CatDauNgoacKep(json["coban"]["toc"].Value));
                    }
                }

                debug.Log("friend 3");
                txtTienVang.text = json["coban"]["vang"].Value;
                debug.Log("friend 4");
                txtNameFriend.text = json["tenhienthi"].Value;

                LoadAvtFriend(json["taikhoan"].Value, imgAvatarFriend, imgAvatarFriend.transform.GetChild(1).GetComponent<Image>());
                imgAvatarFriend.name = json["taikhoan"].Value;
                //  imgAvatarFriend.sprite = imgavatar;
                debug.Log("friend 5");
                // GetAvatarFriend(json["taikhoan"].Value,imgAvatarFriend);
                KhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["coban"]["toc"].Value);
                txtlevel.text = json["coban"]["level"].Value;
                debug.Log("friend 6");
                float fillamount = float.Parse(json["coban"]["exp"].Value) / float.Parse(json["coban"]["maxexp"].Value);
                debug.Log("friend 7");
                ThanhExp.fillAmount = fillamount;


                //GameObject Dao = DaoFriend.transform.GetChild(0).gameObject;
                //DangODaoFriend = 0;
                //transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
                //crgame.ngaydem.transform.position = Dao.transform.position;
                //crgame.ngaydem.transform.SetParent(Dao.transform);
                //crgame.ngaydem.transform.SetSiblingIndex(4);
                //crgame.Bg.transform.position = Dao.transform.position;
                GiaoDienFriend.SetActive(true); GiaoDienMinh.SetActive(false);
                CrGame.ins.panelLoadDao.SetActive(false);
                //  crgame.OnThongBao(false);
            }
        }
    }
    public void QuaDaoFriend(int dao)
    {
        GameObject Dao = null;
        JSONNode json = DataDao;
        if (DaoFriend.transform.Find("DaoFriend" + dao))
        {
            Dao = DaoFriend.transform.Find("DaoFriend" + dao).gameObject;
        }
        else
        {
            Dao = Instantiate(Inventory.LoadObjectResource("GameData/Dao/DaoFriend" + dao), transform.position, Quaternion.identity);
            Dao.transform.SetParent(DaoFriend.transform, false);
        }

        Dao.name = "DaoFriend" + dao;
        Dao.transform.position = DaoFriend.transform.GetChild(0).transform.position;
        Dao.SetActive(true);
        transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
        crgame.ngaydem.transform.position = Dao.transform.position;
        crgame.ngaydem.transform.SetParent(Dao.transform);
        crgame.ngaydem.transform.SetSiblingIndex(4);
        crgame.Bg.transform.position = Dao.transform.position;


        Dao daoo = Dao.GetComponent<Dao>();

        if (!daoo.load)
        {
            int i = dao;
            Image imgCapDao = crgame.FindObject(Dao, "btnCapDaoFriend").GetComponent<Image>();
            imgCapDao.sprite = Inventory.LoadSprite("Dao" + int.Parse(json[i]["leveldao"].Value));
            imgCapDao.SetNativeSize();
            daoo.load = true;
            for (int j = 0; j < json[i]["congtrinh"].Count; j++)
            {
                GameObject congtrinhdao = crgame.FindObject(Dao, "CongtrinhFriend");
                CongTrinh ct = congtrinhdao.transform.GetChild(j).GetComponent<CongTrinh>();
                byte capct = byte.Parse(json[i]["congtrinh"][j]["cap"].Value);
                ct.nameCongtrinh = json[i]["congtrinh"][j]["name"].Value; ct.levelCongtrinh = capct; ct.LoadImg();
            }

            debug.Log("friend 1");
            GameObject RongFriend = crgame.FindObject(Dao, "RongFriendDao");
           
            //for (int j = 3; j < RongFriend.transform.childCount; j++)
            //{
            //    Destroy(RongFriend.transform.GetChild(j).gameObject);
            //}
            for (int j = 0; j < json[i]["rong"].Count; j++)
            {
                DragonIslandManager.ParseDragonIsland(json[i]["rong"][j], (byte)dao, Vector3.zero, RongFriend.transform);
            }
            DragonIslandManager.InsAllItemDao(json[i]["itemEvent"], (byte)i);
            GameObject objecttrangtridao = crgame.FindObject(Dao, "TrangTriFriend");
            //debug.Log("friend 2");
            //foreach (Transform child in objecttrangtridao.transform)
            //{
            //    Destroy(child.gameObject);
            //}
            for (int j = 0; j < json[i]["trangtri"].Count; j++)
            {
                string nametrangtri = net.CatDauNgoacKep(json[i]["trangtri"][j]["name"].Value);


                float x = 0, y = 0;
                // float y = float.Parse(net.CatDauNgoacKep(json["dao"][i]["trangtri"][j]["y"].Value)) - (Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y) + Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y));

                float.TryParse(GamIns.CatDauNgoacKep(json[i]["trangtri"][j]["y"].Value,true), out y);

                y -= Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y);

                float.TryParse(GamIns.CatDauNgoacKep(json[i]["trangtri"][j]["x"].Value,true), out x);



               
               // float y = float.Parse(net.CatDauNgoacKep(json[i]["trangtri"][j]["y"].Value)) - Mathf.Abs(DaoFriend.transform.GetChild(i).transform.position.y);
         
              //  float x = float.Parse(net.CatDauNgoacKep(json[i]["trangtri"][j]["x"].Value));




                //  debug.Log("x " + x + " y " + y);
                // GameObject trangtri = Instantiate(GameObject.Find(net.CatDauNgoacKep(json[i]["trangtri"][j]["name"].Value)), new Vector3(float.Parse(net.CatDauNgoacKep(json[i]["trangtri"][j]["x"].Value)), y), Quaternion.identity) as GameObject;
                //  System.Math.Round(y, 2);
                GameObject trangtri = Instantiate(Inventory.LoadObjectResource("GameData/Item/" + net.CatDauNgoacKep(json[i]["trangtri"][j]["name"].Value)), transform.position, Quaternion.identity) as GameObject;
                Destroy(trangtri.GetComponent<ItemTrangTri>());
                Destroy(trangtri.GetComponent<EventTrigger>());
                Destroy(trangtri.GetComponent<Button>());
                trangtri.transform.SetParent(objecttrangtridao.transform);
                trangtri.transform.position = new Vector3(x, y, 0);
                trangtri.transform.GetChild(1).gameObject.SetActive(true);
                trangtri.GetComponent<Image>().enabled = false;// new Vector3(float.Parse(net.CatDauNgoacKep(json[i]["trangtri"][j]["x"].Value)), y)
                trangtri.transform.GetChild(1).transform.position = new Vector3(trangtri.transform.GetChild(1).transform.position.x, trangtri.transform.GetChild(1).transform.position.y, 0);
                trangtri.transform.GetChild(0).gameObject.SetActive(false);
                if (net.CatDauNgoacKep(json[i]["trangtri"][j]["name"].Value) == "TocLenh")
                {
                    trangtri.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator("TocLenh" + net.CatDauNgoacKep(json["coban"]["toc"].Value));
                }
            }
        }
      
    }
   
    public void XemThongTin()
    {
        MenuTTNguoichoi menutt = AllMenu.ins.GetCreateMenu("GiaoDienThongTin",null,false,1).GetComponent<MenuTTNguoichoi>();
        menutt.ID = imgAvatarFriend.name;
        menutt.gameObject.SetActive(true);
    }
    public void LoadAvtFriend(string id, Image imgavt, Image imgkhung)
    {
        //  crgame.panelLoadDao.SetActive(true);
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "GetAvtFriend";
        datasend["data"]["id"] = id;
        NetworkManager.ins.SendServer(datasend, Ok,true);
        
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                LoadImage("khungavt", json["data"]["khungavtsudung"].AsString, imgkhung);
                LoadImage("avt", json["data"]["avtsudung"].AsString, imgavt);
            }
        }
    }
}
[SerializeField]
public class Search
{
    public string idFriend,id,idfr;
    public Search(string Name,string ID,string Idfr)
    {
        idFriend = Name;
        id = ID;
        idfr = Idfr;
    }
}
[SerializeField]
public class TangQua
{
    public string nameFriend,nameitem,idrong;
    public int soluong;
    public TangQua(string Idfriend,int Soluong,string Nameitem,string idRong)
    {
        nameFriend = Idfriend; nameitem = Nameitem;
        soluong = Soluong;
        idrong = idRong;
    }
}
