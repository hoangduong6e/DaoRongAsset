using SimpleJSON;
using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XLuaTest;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
       
            //NetworkManager.ins.socket.Emit("getUserLoiDai");
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
            XemDau();
        }
        void XemDau()
        {

            JSONClass datasend = new JSONClass();
            datasend["class"] = "Main";
            datasend["method"] = "xemdauloidai";

            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsInt <= 2)
                {
                    //ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true, ChuaRongTop.transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
                    ThongBaoChon tbc = ExThongBaoChon.OpenMenu(default, ChuaRongTop.transform.GetSiblingIndex() + 1);
                    tbc.btnChon.onClick.RemoveAllListeners();
                    if (json["status"].AsString == "0")
                    {
                        tbc.SetTxt("Bạn muốn thách đấu với người chơi " + " <color=#ffff00ff>" + nameDau + "</color> ?");
                        tbc.btnChon.onClick.AddListener(VaoDau);
                    }
                    else if (json["status"].AsString == "1")
                    {
                        tbc.SetTxt(json["message"].Value);
                        tbc.btnChon.onClick.AddListener(MoDauBangkimcuong);
                    }
                    else if (json["status"].AsString == "2")
                    {
                        tbc.SetTxt(json["message"].Value);
                        tbc.btnChon.onClick.AddListener(MoDauBangkimcuong);
                    }
                    boquatrandau = false;
                    tbc.OnToggle("<color=lime>Bỏ qua trận đấu</color>", TickBoQuaTranDau);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
                    // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                }
            }
        }
    }
    private bool boquatrandau = false;
    void TickBoQuaTranDau(bool b)
    {
        //debug.Log("tick bỏ qua trận đấu " + b);
        boquatrandau = b;
    }    
    void MoDauBangkimcuong()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "MuaLuotDau2kc";
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                VaoDau();
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
        }
    }

    void DanhLoiDai()
    {
        //debug.Log("send danh loi dai");
        CrGame.ins.panelLoadDao.SetActive(true);
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "DanhLoiDai";
        datasend["data"]["tenhienthi"] = nameDau;
        datasend["data"]["boquatrandau"] = boquatrandau.ToString();
        NetworkManager.ins.SendServer(datasend, Ok,true);

        void Ok(JSONNode data)
        {
            if (data["status"].AsString == "0")
            {
                VienChinh.vienchinh.chedodau = CheDoDau.LoiDai;
                VienChinh.vienchinh.enabled = true;
                // GiaoDienPVP.ins.maxtime = 0;
                NetworkManager.ins.vienchinh.gameObject.SetActive(true);
                gameObject.SetActive(false);
                ReplayData.Record = data["Record"].AsBool;
                ReplayData.ResetReplayData();
                CrGame.ins.giaodien.SetActive(false);
                Clear();
                VienChinh.vienchinh.chedodau = CheDoDau.LoiDai;
                Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
                Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
                GiaoDienPVP giaoDien = AllMenu.ins.GetCreateMenu("GiaoDienPVP").GetComponent<GiaoDienPVP>();
                giaoDien.btnSetting.SetActive(false);
                //   giaoDien.time = 0;
                // debug.Log("ok 1");
                for (int i = 0; i < data["danhloidai"]["doihinh"].Count; i++)
                {
                    string id = data["danhloidai"]["doihinh"][i]["id"].AsString;
                    string nameObject = data["danhloidai"]["doihinh"][i]["nameobject"].AsString;
                    giaoDien.AddItemRongDanh(nameObject, id, data["danhloidai"]["doihinh"][i]["sao"].AsInt, data["danhloidai"]["doihinh"][i]["tienhoa"].AsInt, i);
                    //  debug.Log("AddItemRongDanh " + i);
                }
                //   debug.Log("ok 1");
                GiaoDienPVP.ins.LoadSkill(data["danhloidai"]["skill"]);
                /// debug.Log("ok 2");
                if (data["danhloidai"]["icontoclenh"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Xanh", "iconTocLenhXanh");
                if (data["danhloidai"]["iconcovip"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Xanh", "iconCoVipXanh");
                if (data["danhloidai"]["iconhoathanlong"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Xanh", "iconHoaThanLongXanh");
                if (data["danhloidai"]["iconNuiThanBi"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Xanh", "iconNuiThanBiXanh");

                if (data["danhloidai"]["icontoclenhfriend"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Do", "iconTocLenhDo");
                if (data["danhloidai"]["iconcovipfriend"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Do", "iconCoVipDo");
                if (data["danhloidai"]["iconhoathanlongfriend"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Do", "iconHoaThanLongDo");
                if (data["danhloidai"]["iconNuiThanBifriend"].AsString == "true") VienChinh.vienchinh.HienIconSkill(999, "Do", "iconNuiThanBiDo");
                //  RuntimeAnimatorController[] AnimFriend = new RuntimeAnimatorController[e.data["danhloidai"]["doihinhfriend"].Count];
                // debug.Log("ok 3");
                VienChinh.vienchinh.StartCoroutine(delayfriend());
                IEnumerator delayfriend()
                {
                    VienChinh.vienchinh.DanhOnline = data["danhOnline"].AsBool;
                    int count = data["danhloidai"]["doihinhfriend"].Count;
                    CrGame.ins.menulogin.SetActive(false);
                    GiaoDienPVP.ins.transform.Find("btnTrieuHoiNhanh").gameObject.SetActive(true);
                    // AudioManager.SetSoundBg("");
                    //vienchinh.StartCoroutine(vienchinh.delayGame("nhacloidai",Team.TeamDo));
                    VienChinh.vienchinh.StartCoroutine(VienChinh.vienchinh.delayGame("nhacloidai", Team.TeamXanh));
                    if (!VienChinh.vienchinh.DanhOnline)
                    {
                        yield return new WaitForSeconds(3.5f);
                        for (int i = 0; i < count; i++)
                        {
                            // JSONObject json = JSONObject.Create(data["danhloidai"]["doihinhfriend"][i].ToString());
                           
                            PVEManager.TrieuHoiDra(ParseChiSoRongNode(data["danhloidai"]["doihinhfriend"][i]), VienChinh.vienchinh.Teamthis == Team.TeamXanh ? "TeamDo" : "TeamXanh");

                            yield return new WaitForSeconds(0.1f);

                        }
                    }

                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(data["message"].AsString);
            }
        }
    }
    public static JSONObject ParseChiSoRongNode(JSONNode data)
    {
        JSONObject json = new JSONObject();
        JSONNode datarong = data;
        json.AddField("sao", datarong["sao"].AsByte);
        json.AddField("id", datarong["id"].AsString);
        json.AddField("nameobject", datarong["nameobject"].AsString);
        json.AddField("namerong", datarong["namerong"].AsString);
        JSONObject chiso = new JSONObject();
        JSONObject chisoget = new JSONObject();
        chiso.AddField("hp", datarong["chiso"]["hp"].AsFloat);
        chiso.AddField("sucdanh", datarong["chiso"]["sucdanh"].AsFloat);
        chiso.AddField("hutmau", datarong["chiso"]["hutmau"].AsFloat);
        chiso.AddField("netranh", datarong["chiso"]["netranh"].AsFloat);
        chiso.AddField("tilechimang", datarong["chiso"]["tilechimang"].AsFloat);
        chiso.AddField("giapso", datarong["chiso"]["giapso"].AsFloat);
        chiso.AddField("giapphantram", datarong["chiso"]["giapphantram"].AsFloat);
        chiso.AddField("xuyengiap", datarong["chiso"]["xuyengiap"].AsFloat);
        chisoget.AddField("tocchay", datarong["chisoget"]["tocchay"].AsFloat);
        chisoget.AddField("tamdanhxa", datarong["chisoget"]["tamdanhxa"].AsFloat);

        if (datarong["Giap"].ToString() != "")
        {
            json.AddField("Giap", datarong["Giap"].AsString);
            chiso.AddField("hpgiap", datarong["chiso"]["hpgiap"].AsFloat);
            chiso.AddField("hpzin", datarong["chiso"]["hpzin"].AsFloat);
        }

        json.AddField("chiso", chiso);
        json.AddField("chisoget", chisoget);

        return json;
    }
    public void VaoDau()
    {
        // LoadLoiDai();
      

        DanhLoiDai();
        //NetworkManager.ins.socket.Emit("DanhLoiDai",JSONObject.CreateStringObject(nameDau));
        //  ThongBaoChon tbc = AllMenu.ins.menu["MenuXacNhan"].GetComponent<ThongBaoChon>();
        if(AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        //tbc.btnChon.onClick.RemoveAllListeners();
       
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
        NetworkManager.ins.SendServer(datasend, ok);
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
