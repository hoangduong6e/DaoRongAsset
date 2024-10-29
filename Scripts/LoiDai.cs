using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoiDai : MonoBehaviour
{
    public GameObject transformMay;
    public GameObject BtnRong,BongRong;
    public GameObject[] objGiaoDienOff;public GameObject GiaoDien,ChuaRongTop;string nameDau;
    public GameObject CanvasS;
    public Text txtSoluotFree;public bool load = false;
    // Start is called before the first frame update
    void Awake()
    {
        CanvasS = GameObject.FindGameObjectWithTag("CanvasS");
    }
    private void OnEnable()
    {
        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        if (Dao.activeSelf || load)
        {
            LoadLoiDai();
            AudioManager.SoundBg.Stop();
            if (Friend.ins.QuaNha)
            {
                Friend.ins.GoHome();
            }
            Dao.SetActive(false);
            NetworkManager.ins.socket.Emit("getUserLoiDai");
            //for (int i = 0; i < objGiaoDienOff.Length; i++)
            //{
            //    objGiaoDienOff[i].SetActive(false);
            //}
            GiaoDien.transform.SetParent(transform.parent.transform);
            GiaoDien.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            CrGame.ins.DonDepDao();
            load = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void LoadLoiDai()
    {
    //    CrGame.ins.menulogin.SetActive(true);
     //   Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
     //   Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
     //   Progress.fillAmount = (float)0 / (float)100;
      //  txtProgress.text = "0%";
        for (int i = 0; i < objGiaoDienOff.Length; i++)
        {
            objGiaoDienOff[i].SetActive(false);
        }
        CrGame.ins.giaodien.SetActive(true);
        CrGame.ins.txtDanhVong.gameObject.SetActive(true);
      
    }
    public void ChonDau()
    {
        if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.transform.GetSiblingIndex() > 0)
        {
            nameDau = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
            StartCoroutine(XemDau());
        }
        IEnumerator XemDau()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "xemdauloidai/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi!");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                ThongBaoChon tbc =  AllMenu.ins.GetCreateMenu("MenuXacNhan",GameObject.Find("CanvasTrenCung"),true, ChuaRongTop.transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                if (json["status"].AsString == "0")
                {
                    tbc.txtThongBao.text = "Bạn muốn thách đấu với người chơi " + "<color=#ffff00ff>" + nameDau + "</color> ?";
                    tbc.btnChon.onClick.AddListener(VaoDau);
                }
                else if (json["status"].AsString == "1")
                {
                    tbc.txtThongBao.text = json["message"].Value;
                    tbc.btnChon.onClick.AddListener(MoDauBangkimcuong);
                }
                else if (json["status"].AsString == "2")
                {
                    tbc.txtThongBao.text = json["message"].Value;
                    tbc.btnChon.onClick.AddListener(MoDauBangkimcuong);
                }
            }
        }
    }    
    void MoDauBangkimcuong()
    {
        StartCoroutine(Muabangkc());
        IEnumerator Muabangkc()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "MuaLuotDau2kc/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) debug.Log(www.error);
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].AsString == "0")
                {
                    VaoDau();
                }
            }
        }
    }
    public void VaoDau()
    {
        // LoadLoiDai();
        Clear();
        VienChinh.vienchinh.chedodau = CheDoDau.LoiDai;
        VienChinh.vienchinh.enabled = true;
        // GiaoDienPVP.ins.maxtime = 0;
        NetworkManager.ins.vienchinh.gameObject.SetActive(true);
        NetworkManager.ins.socket.Emit("DanhLoiDai",JSONObject.CreateStringObject(nameDau));
        //  ThongBaoChon tbc = AllMenu.ins.menu["MenuXacNhan"].GetComponent<ThongBaoChon>();
        if(AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        //tbc.btnChon.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
        VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanDem");
        //NetworkManager.ins.vienchinh.chedodau = "LoiDai";
      //  NetworkManager.ins.vienchinh.enabled = true;
    }    
    public void XemBXH()
    {
        GameObject trencung = GameObject.FindGameObjectWithTag("trencung");
        AllMenu.ins.GetCreateMenu("MenuTopLoiDai", trencung, true, ChuaRongTop.transform.GetSiblingIndex()+1);
    }
    public void QuayVe()
    {
        Clear();
        CrGame.ins.giaodien.SetActive(true);
        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao"+CrGame.ins.DangODao).gameObject;
        Dao.SetActive(true);
        Vector3 newvec = Dao.transform.position; newvec.z = -10;
        CrGame.ins.transform.position = newvec;
        GiaoDien.transform.SetParent(CanvasS.transform);
        GiaoDien.transform.SetSiblingIndex(2);
        for (int i = 0; i < objGiaoDienOff.Length; i++)
        {
            objGiaoDienOff[i].SetActive(true);
        }
        load = false;
        AudioManager.SetSoundBg("nhacnen0");
        CrGame.ins.txtDanhVong.gameObject.SetActive(false);
        gameObject.SetActive(false);
        
    }
    public void Clear()
    {
        for (int i = 0; i < ChuaRongTop.transform.childCount; i++)
        {
            for (int j = 0; j < ChuaRongTop.transform.GetChild(i).transform.childCount; j++)
            {
                Destroy(ChuaRongTop.transform.GetChild(i).transform.GetChild(j).gameObject);
            }
        }
        Resources.UnloadUnusedAssets();
    }    
    public void TrangBi()
    {
        GameObject tc = GameObject.FindGameObjectWithTag("trencung");
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", tc,true,ChuaRongTop.transform.GetSiblingIndex()+1);
    }
    public void CreateUserTop(int i, string tenhienthi, string top,string idfb,string namerong,byte sao)
    {
        //if (inevntory.Cacherong.ContainsKey(nameRong))
        //{
        //    Rong = inevntory.Cacherong[nameRong];
        //}
        //else
        //{
        //    Rong = SGResources.Load("GameData/Rong/" + nameRong) as GameObject;
        //    inevntory.Cacherong.Add(nameRong,Rong);
        //}
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject Bong = Instantiate(BongRong, new Vector3(ChuaRongTop.transform.GetChild(i).transform.position.x, ChuaRongTop.transform.GetChild(i).transform.position.y + 10), Quaternion.identity) as GameObject;
            Bong.transform.SetParent(ChuaRongTop.transform.GetChild(i).transform, false);
            Bong.SetActive(true);
            //GameObject hieuung = Instantiate(NetworkManager.ins.vienchinh.HieuUngTrieuHoi, new Vector3(ChuaRongTop.transform.GetChild(i).transform.position.x, ChuaRongTop.transform.GetChild(i).transform.position.y + 9), Quaternion.identity) as GameObject;
            //Destroy(hieuung, 0.5f);
            GameObject dra = null;
            if (i == 0) dra = AllMenu.ins.GetRongGiaoDien(namerong + 2, null, -1);
            else dra = AllMenu.ins.GetRongGiaoDien(namerong + 2, null);
            Vector3 tfinstantiate = new Vector3(ChuaRongTop.transform.GetChild(i).transform.position.x, GamIns.ins.MaxY,dra.transform.position.z);
            dra.transform.position = tfinstantiate;
            Animator anim = dra.GetComponent<Animator>();
            anim.Play("Spawn");
           // anim.SetFloat("speedRun",1.8f);
            Vector3 tfrong = new Vector3(ChuaRongTop.transform.GetChild(i).transform.position.x, ChuaRongTop.transform.GetChild(i).transform.position.y + 1);
            dra.transform.LeanMove(tfrong, 0.1f);
            //yield return new WaitForSeconds(1.f);
            //anim.SetFloat("speedRun", 1f);
            anim.Play("Idlle");

            dra.transform.SetParent(ChuaRongTop.transform.GetChild(i));

            Vector3 tfrongg = new Vector3(ChuaRongTop.transform.GetChild(i).transform.position.x, ChuaRongTop.transform.GetChild(i).transform.position.y + 90);
            yield return new WaitForSeconds(0.5f);
            GameObject Rongg = Instantiate(BtnRong, tfrongg, Quaternion.identity) as GameObject;
            PVEManager.SetScaleDragon(namerong,sao, dra.transform);
            //  Animator anim = Rongg.GetComponent<Animator>();idobject
            // anim.runtimeAnimatorController = animm;//inevntory.LoadAnimatorRongCoBan(nameRong);//SGResources.Load<RuntimeAnimatorController>("GameData/Animator/" + nameRong);
            Rongg.SetActive(true);
            Rongg.transform.SetParent(ChuaRongTop.transform.GetChild(i).transform, false);
            //  anim.SetInteger("TienHoa", tienhoa);


            Image imgAvatar = Rongg.transform.Find("avatar").GetComponent<Image>();
            //if(idfb != "bot")
            //{
            //   // friend.GetAvatarFriend(idfb, imgAvatar);
            //}
            Image imgToc = Rongg.transform.Find("Khung").GetComponent<Image>();
            Friend.ins.LoadAvtFriend(idfb, imgAvatar, imgToc);
            //   imgToc.sprite = Inventory.LoadSprite("Avatar" + toc);
            Text txtname = Rongg.transform.Find("txtname").GetComponent<Text>();
            txtname.text = tenhienthi;
            Text txttop = Rongg.transform.Find("txtTop").GetComponent<Text>();
            txttop.text = top;
            Rongg.name = tenhienthi;
            //  BtnRong.transform.SetParent()
            Destroy(Bong);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void GetNhatKiLoiDai()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "ReplayData";
        datasend["method"] = "GetNhatKiLoiDai";
        NetworkManager.ins.SendServer(datasend.ToString(), ok);
        void ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                MenuNhatKiLoiDai NhatKiLoiDai = AllMenu.ins.GetCreateMenu("MenuNhatKiLoiDai", GameObject.FindGameObjectWithTag("trencung"),true,2).GetComponent<MenuNhatKiLoiDai>(); //AllMenu.ins.transform.Find("MenuNhatKiLoiDai").GetComponent<MenuNhatKiLoiDai>();
                NhatKiLoiDai.gameObject.SetActive(true);
                NhatKiLoiDai.ParseData(json["data"]);
              
                NhatKiLoiDai.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform);
              //  debug.Log(json["data"].ToString());
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
}
