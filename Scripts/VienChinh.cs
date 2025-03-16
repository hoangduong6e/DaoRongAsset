using SimpleJSON;
using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum CheDoDau
{
    Null,
    VienChinh,
    Halloween,
    BossTG,
    SoloKhongTuoc,
    Replay,
    Online,
    LoiDai,
    ThuThach,
    solo,
    LanSu,
    XucTu
}
public enum KetQuaTranDau
{
    Thua,
    Thang
}
public enum Team
{
    TeamXanh,
    TeamDo,
    Null
}
public class VienChinh : MonoBehaviour
{
    GameObject camera;
    public GameObject vienchinhBg;
    NetworkManager net; public CrGame crgame;
    public GameObject TruXanh, TruDo, TeamXanh, TeamDo;
    public Camera cam { get; private set; }
    Vector3 dragOrigin;
    public SpriteRenderer imgMap;
    public float zoomstep, Maxcamsize, MinCamsize;
    float MapMinX, MapMaxX, mapMiny, MapMaxY;
    public GameObject LevelVienChinh; public Sprite ChienTuongVang, ChienTuongDo, thanhmaudo, thanhmauxanh; //public string chedodau;
    Friend friend; public GameObject MapVienChinh; Inventory inventory;

    public string nameMapvao;
    public bool dangdau = false;
    public float[] timeskill;
    public float buffgiapallxanh { get; private set; }
    public float buffgiapalldo { get; private set; }

    public float buffhuthpallxanh { get; private set; }
    public float buffhuthpalldo { get; private set; }

    public GameObject muctieuxanh, muctieudo, PoolEffect, ObjSkill;
    public static VienChinh vienchinh;
    public CheDoDau chedodau;

    public Team Teamthis;

    public Dictionary<Team, byte> tilehoisinh = new Dictionary<Team, byte>() {
        {Team.TeamXanh, 0 },
        {Team.TeamDo, 0 },
    };

    public Dictionary<Team, byte> maxHS = new Dictionary<Team, byte>() {
        {Team.TeamXanh, 0 },
        {Team.TeamDo, 0 },
    };

    public Dictionary<Team, byte> solanHSAll = new Dictionary<Team, byte>() {
        {Team.TeamXanh, 0 },
        {Team.TeamDo, 0 },
    };

    public Dictionary<Team, byte> maxHSAll = new Dictionary<Team, byte>() {
        {Team.TeamXanh, 0 },
        {Team.TeamDo, 0 },
    };

    public bool DanhOnline = false;
    //public JSONClass ThongKeDame = new JSONClass();

    //public void AddThongKeDame(string id, string nameobjrong,byte saorong,float damee)
    //{
    //   if()
    //   ThongKeDame[nameobjrong][id + "/" + saorong] = 0;
    // }    

    public void SetTyLeHoiSinh(byte set,byte maxhs, byte maxhsAll, Team team)
    {
        tilehoisinh[team] += set;
        if (tilehoisinh[team] < 0) tilehoisinh[team] = 0;
        maxHS[team] = maxhs;
        maxHSAll[team] = maxhsAll;
        solanHSAll[team] = 0;
    }
    public void SetBuffGiapall(float set, Team team)
    {
        if (team == Team.TeamDo)
        {
            buffgiapalldo += set;
            if (buffgiapalldo > 85) buffgiapalldo = 85;
            else if (buffgiapalldo < 0) buffgiapalldo = 0;
            debug.Log("buff giáp all đỏ" + buffgiapalldo);
        }
        else
        {
            buffgiapallxanh += set;
            if (buffgiapallxanh > 85) buffgiapallxanh = 85;
            else if(buffgiapallxanh < 0) buffgiapallxanh= 0;
            debug.Log("buff giáp all xanh" + buffgiapallxanh);
        }
    }

    public void SetBuffHutHpall(float set, Team team)
    {
        if (team == Team.TeamDo)
        {
            buffhuthpalldo += set;
            if (buffhuthpalldo < 0) buffhuthpalldo = 0;
            debug.Log("buff hút hp all đỏ" + buffhuthpalldo);
        }
        else
        {
            buffhuthpallxanh += set;
           if (buffhuthpallxanh < 0) buffhuthpallxanh = 0;
            debug.Log("buff hút hp all xanh" + buffhuthpallxanh);
        }
    }

    //  public GameObject[] rongdungdauxanh = new GameObject[10], rongdungdaudo = new GameObject[10];
    // public Vector3[] vitriteamxanh = new Vector3[50];
    // public Vector3[] vitriteamdo = new Vector3[50];
    // Start is called before the first frame update


    private void Awake()
    {
        //TruXanh = GameObject.Find("truxanh");
        //TruDo = GameObject.Find("trudo");
        //TeamXanh = GameObject.Find("TeamXanh");
        //TeamDo = GameObject.Find("TeamDo");
        vienchinh = this;
        MapVienChinh = gameObject.transform.GetChild(0).gameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        inventory = cam.GetComponent<Inventory>();
        friend = cam.GetComponent<Friend>();
        net = cam.GetComponent<NetworkManager>();
        crgame = net.GetComponent<CrGame>();
        vienchinhBg.SetActive(true);
        MapMinX = imgMap.transform.position.x - imgMap.bounds.size.x / 2f;
        MapMaxX = imgMap.transform.position.x + imgMap.bounds.size.x / 2f;
        camera = cam.gameObject;
        mapMiny = imgMap.transform.position.y - imgMap.bounds.size.y / 2f;
        MapMaxY = imgMap.transform.position.y + imgMap.bounds.size.y / 2f;

    }
    public float dameKhongTuoc = 0;
    public bool CheckTrigger(string name)
    {
        bool attack = false;
        if (name == "SKillRongBang")
        {
            attack = true;
        }
        return attack;
    }
    public bool checkIconSkill(string nameskill,string nameTeam)
    {
        GameObject objteam = null;
        if (nameTeam == "TeamXanh") objteam = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(1).gameObject;
        else objteam = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(2).gameObject;
        if (objteam.transform.childCount > 0)
        {
            for (int i = 0; i < objteam.transform.childCount; i++)
            {
                if (objteam.transform.GetChild(i).name.Contains(nameskill))
                {
                    if(objteam.transform.GetChild(i).gameObject.activeSelf)
                    {
                      //  debug.Log("name skill: " + nameskill + ", nameTeam: " + nameTeam + " đang có icon khổng tước, không thể spam skill thêm");
                        return true;
                    }    
                    else return false;
                }
            }
        }
      //  debug.Log("name skill: " + nameskill + ", nameTeam: " + nameTeam + " không có icon khổng tước");
        return false;
    }    
    public void HienIconSkill(float timehien, string team, string namee,bool setOnline = false)
    {
        //if(chedodau == "Online" && !setOnline)
        //{
        //    JSONObject newjsonn = new JSONObject();
        //    newjsonn.AddField("HienIconSkill", "");
        //    newjsonn.AddField("timehien", timehien.ToString());
        //    newjsonn.AddField("team", DauTruongOnline.namedoithu);
        //    newjsonn.AddField("namee", namee);
 
        //    DauTruongOnline.ins.AddUpdateData(newjsonn);

        //    return;
        //}
        GameObject iconSkill = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(0).gameObject;
        GameObject objteam = null;
        if (team == "Xanh") objteam = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(1).gameObject;
        else if (team == "Do") objteam = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(2).gameObject;
        if (objteam.transform.childCount > 0)
        {
            for (int i = 0; i < objteam.transform.childCount; i++)
            {
                if (objteam.transform.GetChild(i).name == namee)
                {
                    objteam.transform.GetChild(i).gameObject.SetActive(true);
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        yield return new WaitForSeconds(timehien);
                        objteam.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    break;
                }
                else if (i == objteam.transform.childCount - 1)
                {
                    StartCoroutine(taoiconskill());
                }
            }
        }
        else StartCoroutine(taoiconskill());
        IEnumerator taoiconskill()
        {
            GameObject iconHien = Instantiate(iconSkill, transform.position, Quaternion.identity);
            iconHien.transform.SetParent(objteam.transform, false);
            Image imgicon = iconHien.transform.GetChild(0).GetComponent<Image>();
            imgicon.sprite = Inventory.LoadSprite(namee);
            imgicon.SetNativeSize();
            iconHien.transform.GetChild(0).transform.localScale = new Vector2(1, 1);
            iconHien.name = namee;
            iconHien.transform.GetChild(0).name = ReplayData.time.ToString();
            iconHien.SetActive(true);

            yield return new WaitForSeconds(timehien);
            TaticonSkill(namee, team);
            // iconHien.SetActive(false);
        }

    }
    public void TaticonSkill(string nameskill, string team)
    {
        if (!AllMenu.ins.menu.ContainsKey("GiaoDienPVP")) return;
        GameObject objteam = null;
        if (team == "Xanh") objteam = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(1).gameObject;
        else if (team == "Do") objteam = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(2).gameObject;
        if (objteam.transform.childCount > 0)
        {
            for (int i = 0; i < objteam.transform.childCount; i++)
            {
                if (objteam.transform.GetChild(i).name == nameskill)
                {
                    GameObject objtat = objteam.transform.GetChild(i).gameObject;
                    objtat.SetActive(false);
                    ReplayData.AddIconSkill(objtat.transform.GetChild(0).name + "*" + team + "*" + nameskill + "*" + ReplayData.time);
                    break;
                }
            }
        }
    }
    private void OnEnable()
    {
        // xemphoban.NhacPvp.Play();
        // SoDoihinh = 0;
        CrGame.ins.giaodien.SetActive(false);

        // crgame.giaodien.SetActive(false);
        GameObject giaodienPvp = AllMenu.ins.GetCreateMenu("GiaoDienPVP");
        giaodienPvp.SetActive(true);
        Camera.main.orthographicSize = 5;
        cam.GetComponent<ZoomCamera>().enabled = false;
        GameObject Dao = crgame.AllDao.transform.Find("BGDao" + crgame.DangODao).gameObject;
        Dao.SetActive(false);
        // crgame.enabled = false;
        MapVienChinh.SetActive(true);
        camera.transform.position = new Vector3(MapVienChinh.transform.position.x, MapVienChinh.transform.position.y, -10);
        transform.position = new Vector3(transform.position.x, transform.position.y, 90);
        //   StartCoroutine(delay());
        transform.GetChild(0).gameObject.SetActive(true);
        GiaoDienPVP.ins.txtBatDau.text = "Đang kết nối tới máy chủ";
        GiaoDienPVP.ins.txtBatDau.fontSize = 100;
        GiaoDienPVP.ins.panelBatdau.SetActive(true);
        giaodienPvp.transform.GetChild(6).gameObject.SetActive(true);
        giaodienPvp.transform.GetChild(7).gameObject.SetActive(true);

        buffgiapallxanh = 0;
        buffgiapalldo = 0;
        buffhuthpallxanh = 0;
        buffhuthpalldo = 0;
    }
    //public GameObject TestGetGiaoDienPvp()
    //{
    //    GameObject giaodienPvp = AllMenu.ins.transform.Find("GiaoDienPVP").gameObject;//AllMenu.ins.GetCreateMenu("GiaoDienPVP");
    //    if (AllMenu.ins.menu.ContainsKey("GiaoDienPVP") == false)
    //    {
    //        GiaoDienPVP.ins = giaodienPvp.GetComponent<GiaoDienPVP>();
    //        AllMenu.ins.menu.Add("GiaoDienPVP", giaodienPvp);
    //    }
    //    return giaodienPvp;
    //}
    private void OnDisable()
    {
        //  xemphoban.NhacPvp.Stop();
        cam.GetComponent<ZoomCamera>().enabled = true;
        //AudioManager.SetSoundBg("nhacnen0");
        // camera.transform.position = new Vector3(0,0,-1);
        // crgame.enabled = true;
    }
    public void SetBGMap(string nameBG)
    {
        transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite(nameBG);
    }
    public IEnumerator delayGame(string sound = "nhacpvp",Team teamthis = Team.TeamXanh)
    {
        Transform Obj = transform.GetChild(0).transform.GetChild(1);
        Vector3 scale = Obj.transform.localScale;
        if (teamthis == Team.TeamDo) scale.x = -1;
        else scale.x = 1;
        Obj.transform.localScale = scale;
        Transform teamxanh = Obj.transform.GetChild(0);
        Transform teamDo = Obj.transform.GetChild(1);
        if (scale.x == 1)
        {
             teamxanh.name = "TeamXanh";
             teamDo.name = "TeamDo";
            TeamXanh = teamxanh.gameObject;
            TeamDo = teamDo.gameObject;

            PVPManager.XTeam = new()
            {
               { "b", PVEManager.XTruTeamXanh },
               { "r",  PVEManager.XTruTeamDo }
            };
        }
        else
        {
            PVPManager.XTeam = new()
            {
               { "b", PVEManager.XTruTeamDo },
               { "r",  PVEManager.XTruTeamXanh }
            };
            teamxanh.name = "TeamDo";
             teamDo.name = "TeamXanh";
            TeamXanh = teamDo.gameObject;
            TeamDo = teamxanh.gameObject;
        }


        Teamthis = teamthis;
        ThongKeDame.ResetThongKe();
        GameObject menutuido = AllMenu.ins.transform.Find("menuTuiDo").gameObject;
        if (menutuido.activeSelf) menutuido.SetActive(false);
        GiaoDienPVP.ins.ResetAllHienRONG();
        PoolEffect.SetActive(true);
        //txtBatDau.gameObject.SetActive(true);// tat roi bat de reset animation
        debug.Log("delay game");
        //  yield return task;
        GiaoDienPVP.ins.btnTrieuHoiSuPhu.interactable = false;
        if (chedodau == CheDoDau.Replay)
        {
            GiaoDienPVP.ins.btnTangToc.SetActive(true);
        }
        else
        {
            GiaoDienPVP.ins.btnTangToc.SetActive(false);
        }
        ReplayData.speedReplay = 1;
        ReplayData.time = 0;
        dangdau = true;
        GiaoDienPVP.ins.panelBatdau.gameObject.SetActive(true);
        // thahetrong = false;
        GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).gameObject.SetActive(true);
        net.loidai.Clear();
        AudioManager.SetSoundBg(sound);
        GiaoDienPVP.ins.txtBatDau.GetComponent<Animator>().Play("animStartVienChinh");
        GiaoDienPVP.ins.txtBatDau.fontSize = 250;
        GiaoDienPVP.ins.txtBatDau.text = "3";
        yield return new WaitForSeconds(0.7f);
        GiaoDienPVP.ins.txtBatDau.text = "2";
        yield return new WaitForSeconds(0.9f);
        GiaoDienPVP.ins.txtBatDau.text = "1";
        yield return new WaitForSeconds(0.80f);
        //GiaoDienPVP.ins.txtBatDau.text = "Bắt đầu";
        GiaoDienPVP.ins.txtBatDau.text = "";
        for (int i = 1; i < TeamXanh.transform.childCount; i++)
        {
            Destroy(TeamXanh.transform.GetChild(i).gameObject);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 2; i++)
        {
            timeskill[i] = 10;
        }
        GiaoDienPVP.ins.OSkill.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        GiaoDienPVP.ins.OSkill.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "150";

        GiaoDienPVP.ins.panelBatdau.transform.GetChild(0).gameObject.SetActive(false);
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -10);
        if(GiaoDienPVP.ins.SetPanelToi)
        {
            GiaoDienPVP.ins.SetPanelToi = false;
        }    
        debug.Log("timee " + GiaoDienPVP.ins.maxtime);
        if (GiaoDienPVP.ins.maxtime > 0)
        {

            GiaoDienPVP.ins.TxtTime.gameObject.SetActive(true);
            //GiaoDienPVP.ins.TxtTime.AddComponent<timePvp>();
            // GiaoDienPVP.ins.TxtTime.GetComponent<timePvp>().enabled = true;
        }
        //   else (GiaoDienPVP.ins.maxtime > 0) GiaoDienPVP.ins.TxtTime.gameObject.SetActive(true);
        if (chedodau == CheDoDau.VienChinh|| chedodau == CheDoDau.Halloween || chedodau == CheDoDau.BossTG || chedodau == CheDoDau.SoloKhongTuoc || chedodau == CheDoDau.LanSu || chedodau == CheDoDau.XucTu)
        {
            GiaoDienPVP.ins.btnSetting.SetActive(true);
            GiaoDienPVP.ins.btnTangToc.SetActive(false);
            if (chedodau == CheDoDau.LanSu) GiaoDienPVP.ins.btnTrieuHoiSuPhu.interactable = false;
            else GiaoDienPVP.ins.btnTrieuHoiSuPhu.interactable = true;
            GiaoDienPVP.ins.txtHuyenTinh.transform.parent.gameObject.SetActive(true);
            if (inventory.ListItemThuong.ContainsKey("itemHuyenTinh")) GiaoDienPVP.ins.txtHuyenTinh.text = inventory.ListItemThuong["itemHuyenTinh"].transform.GetChild(0).GetComponent<Text>().text;
            else GiaoDienPVP.ins.txtHuyenTinh.text = "0";
        }
        ThongKeDame.SetEnableThongKe();
        GiaoDienPVP.ins.panelBatdau.SetActive(false);

      
    }
    public void Thang()
    {
        debug.Log(ThongKeDame.Data.ToString());
        ReplayData.UpdateReplayData(ketquaWin(), "Thang");
    }
    public void Thua(string tb = "")
    {
        ReplayData.UpdateReplayData(ketquaThua(tb), "Thua");

        AudioManager.SoundBg.Stop();
        AudioManager.PlaySound("thuatran");
    }
    public void SetAnimWinAllDra()
    {
        for (int i = 1; i < VienChinh.vienchinh.TeamDo.transform.childCount; i++)
                {
                    Transform skilldra = VienChinh.vienchinh.TeamDo.transform.GetChild(i).transform.Find("SkillDra");
                    if(skilldra != null)
                    {
                        DragonPVEController dra = skilldra.GetComponent<DragonPVEController>();
                        dra.AnimWin();
                    }
               
                }
                for (int i = 1; i < VienChinh.vienchinh.TeamXanh.transform.childCount; i++)
                {
                    Transform skilldra = VienChinh.vienchinh.TeamXanh.transform.GetChild(i).transform.Find("SkillDra");
                    if (skilldra != null)
                    {
                        DragonPVEController dra = skilldra.GetComponent<DragonPVEController>();
                        dra.AnimWin();
                    }

                   // DragonPVEController dra = VienChinh.vienchinh.TeamXanh.transform.GetChild(i).GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
                   //if (dra != null) dra.AnimWin();
            
                    //Destroy(TeamXanh.transform.GetChild(i).gameObject);
                }
    }
    public Action ketquaWin()
    {
        ClearQuai();

        if (chedodau == CheDoDau.VienChinh)
        {
            return winVienchinh;
        }
        else if (chedodau == CheDoDau.LoiDai)
        {
            return winLoiDai;
        }
        else if (chedodau == CheDoDau.ThuThach)
        {
            return winDauTruongThuThach;
        }
        else if (chedodau == CheDoDau.Replay)
        {
            // debug.Log("done replayuyyyyyyy");
            return ReplayData.DoneReplay;
        }
        else if (chedodau == CheDoDau.LanSu)
        {
            return GiaoDienLanSu.KetQua(KetQuaTranDau.Thang);
        }
        else if (chedodau == CheDoDau.XucTu)
        {
       
            return MenuRaKhoi.KetQua(KetQuaTranDau.Thang);
        }
        else if (chedodau == CheDoDau.Halloween)
        {

            return MenuEventHalloween2024.KetQua(KetQuaTranDau.Thang);
        }
        else if (chedodau == CheDoDau.Online)
        {
            void ok()
            {
                reset();
                MenuTTNguoichoi.QuayVe();
                DauTruongOnline.OpenMenu();
            }
            return ok;
        }
        else if (chedodau == CheDoDau.SoloKhongTuoc)
        {
            void Ok()
            {
                net.socket.Emit("soloKhongTuoc", JSONObject.CreateStringObject("ketqua+" + dameKhongTuoc));
                crgame.OpenMenuEvent();
                GiaoDienPVP.ins.gameObject.SetActive(false);
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                reset();
            }
            return Ok;
        }
        else
        {
            reset();
            return MenuTTNguoichoi.QuayVe;
        }
        //  else return ReplayData.DoneReplay;
        void winVienchinh()
        {
            GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
            GiaoDienPVP.ins.spriteWin.SetNativeSize();
            GiaoDienPVP.ins.thongtin.text = "Chúc mừng bạn đã chinh phục ải này";
            crgame = net.GetComponent<CrGame>();
            CrGame.ins.SetPanelThangCap(true);
            crgame.txtThangCap.text = "";
            GiaoDienPVP.ins.menuWin.SetActive(true);
            OffThangCap();
            GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
            net.socket.Emit("WinVienChinh", JSONObject.CreateStringObject(nameMapvao));
        }
        void winLoiDai()
        {

            GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
            GiaoDienPVP.ins.spriteWin.SetNativeSize();
            GiaoDienPVP.ins.thongtin.text = "Bạn đã đánh bại đối thủ!";
            crgame = net.GetComponent<CrGame>();
            CrGame.ins.SetPanelThangCap(true);
            crgame.txtThangCap.text = "";
            GiaoDienPVP.ins.menuWin.SetActive(true);
            OffThangCap();
            GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
            net.socket.Emit("winloidai");

            //AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
            //CrGame.ins.SetPanelThangCap(true);
            //crgame.txtThangCap.text = "";

            //net.menuLoiDai.SetActive(true);
            //transform.GetChild(0).gameObject.SetActive(false);
            //OffThangCap();
            ////GetComponent<VienChinh>().enabled = false;
            //reset();
        }
        void winDauTruongThuThach()
        {
            //net.socket.Emit("WinEventHalloween", JSONObject.CreateStringObject(infophoban.nameMapvao));
            nameskillthuthach = "";
            nameskillthuthach = "";
            JSONClass datasend = new JSONClass();
            datasend["class"] = "DauTruongThuThach";
            datasend["method"] = "WinDauTruongThuThach";
            NetworkManager.ins.SendServer(datasend, Ok,true);
            void Ok(JSONNode json)
            {
                if (json["status"].Value == "ok")
                {
                    AllMenu.ins.OpenMenu("MenuDauTruongThuThach");
                    AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
                    CrGame.ins.SetPanelThangCap(true);
                    crgame.txtThangCap.text = "";
                    transform.GetChild(0).gameObject.SetActive(false);
                    OffThangCap();
                    reset();
                    // debug.LogError(json["thongtin"].Value);
                    if (json["thongtin"].Value != "")
                    {
                        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                        tbc.btnChon.onClick.RemoveAllListeners();
                        tbc.txtThongBao.text = json["thongtin"].Value;
                        tbc.btnChon.onClick.AddListener(ChapNhan);
                    }
                }
                else
                {
                    crgame.OnThongBaoNhanh(json["status"].Value);
                }
                crgame.panelLoadDao.SetActive(false);
            }
        }
    }
    public Action ketquaThua(string tb)
    {
        ClearQuai();
        if (chedodau == CheDoDau.VienChinh)
        {
            //chedodau = CheDoDau.Null;
            return ThuaVienChinh;
        }
        else if (chedodau == CheDoDau.LoiDai)
        {
            return ThuaLoiDai;
        }
        else if (chedodau == CheDoDau.BossTG)
        {
            return ThuaBossTG;
        }
        else if (chedodau == CheDoDau.ThuThach)
        {
            return ThuaDauTruongThuThach;
        }
        else if (chedodau == CheDoDau.Replay)
        {
            return ReplayData.DoneReplay;
        }
        else if (chedodau == CheDoDau.solo)
        {
            return ThuaSolo;
        }
        else if (chedodau == CheDoDau.LanSu)
        {
            Invoke("QuayVe", 10f);
            return GiaoDienLanSu.KetQua(KetQuaTranDau.Thua);
        }
        else if (chedodau == CheDoDau.XucTu)
        {
            Invoke("QuayVe", 10f);
            return MenuRaKhoi.KetQua(KetQuaTranDau.Thua);
        }
        else if (chedodau == CheDoDau.Halloween)
        {
            Invoke("QuayVe", 10f);
            return MenuEventHalloween2024.KetQua(KetQuaTranDau.Thua);
        }
        else if (chedodau == CheDoDau.Online)
        {
            reset();
            void ok()
            {
                MenuTTNguoichoi.QuayVe();
                DauTruongOnline.OpenMenu();
            }
            return ok;
        }
        else if (chedodau == CheDoDau.SoloKhongTuoc)
        {
            void ok()
            {
                net.socket.Emit("soloKhongTuoc", JSONObject.CreateStringObject("ketqua+" + dameKhongTuoc));
                crgame.OpenMenuEvent();
                GiaoDienPVP.ins.gameObject.SetActive(false);
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                reset();
            }
            return ok;
        }
        else
        {
            reset();
            return MenuTTNguoichoi.QuayVe;
        }
        void ThuaVienChinh()
        {
            GiaoDienPVP.ins.menuWin.SetActive(true);
            GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;
            if (tb != "") GiaoDienPVP.ins.thongtin.text = tb;
            else GiaoDienPVP.ins.thongtin.text = "Bạn đã bị đánh bại";
            GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
            GiaoDienPVP.ins.btnSetting.SetActive(true);
            GiaoDienPVP.ins.spriteWin.SetNativeSize();
            Invoke("QuayVe", 10f);
        }
        void ThuaLoiDai()
        {
            //AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
            //net.menuLoiDai.SetActive(true);
            //transform.GetChild(0).gameObject.SetActive(false);
           // reset();
            GiaoDienPVP.ins.menuWin.SetActive(true);
            GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;
            if (tb != "") GiaoDienPVP.ins.thongtin.text = tb;
            else GiaoDienPVP.ins.thongtin.text = "Bạn đã bị đánh bại";
            GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
            GiaoDienPVP.ins.btnSetting.SetActive(true);
            GiaoDienPVP.ins.spriteWin.SetNativeSize();
        }
        void ThuaBossTG()
        {
            crgame.panelLoadDao.SetActive(true);
            JSONClass datasend = new JSONClass();
            datasend["class"] = "BossTheGioi";
            datasend["method"] = "XemDameBossTG";
            NetworkManager.ins.SendServer(datasend, ok);
            void ok(JSONNode json)
            {
                if (json["status"].Value == "ok")
                {
                    //giaodienPvp.SetActive(false);
                    //transform.GetChild(0).gameObject.SetActive(false);
                    //reset();
                    GiaoDienPVP.ins.menuWin.SetActive(true);
                    if (json["nguoitieudietboss"].Value == crgame.FB_userName.text) GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
                    else GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;
                    GiaoDienPVP.ins.thongtin.text = json["thongtin"].Value;
                    GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
                    GiaoDienPVP.ins.btnSetting.SetActive(true);
                    GiaoDienPVP.ins.spriteWin.SetNativeSize();
                    Invoke("QuayVe", 10f);
                }
                else
                {
                    AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
                    transform.GetChild(0).gameObject.SetActive(false);
                    reset();
                    crgame.VaoBossTheGioi();
                }
                crgame.panelLoadDao.SetActive(false);
            }
        }
        void ThuaDauTruongThuThach()
        {
            nameskillthuthach = "";
            JSONClass datasend = new JSONClass();
            datasend["class"] = "DauTruongThuThach";
            datasend["method"] = "ThuaDauTruongThuThach";
            NetworkManager.ins.SendServer(datasend, Ok,true);
            void Ok(JSONNode json)
            {
                if (json["status"].Value == "ok")
                {
                    AllMenu.ins.OpenMenu("MenuDauTruongThuThach");

                    AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
                    //  crgame.panelThangCap.SetActive(true);
                    // crgame.txtThangCap.text = "";
                    transform.GetChild(0).gameObject.SetActive(false);
                    //OffThangCap();
                    reset();
                    debug.LogError(json["thongtin"].Value);
                    if (json["thongtin"].Value != "")
                    {
                        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                        tbc.btnChon.onClick.RemoveAllListeners();
                        tbc.txtThongBao.text = json["thongtin"].Value;
                    }
                }
                else
                {
                    crgame.OnThongBaoNhanh(json["status"].Value);
                }
                crgame.panelLoadDao.SetActive(false);
            }
        }
        void ThuaSolo()
        {
            soloWin("thua",true);
        }
    }
    public void QuayVe()
    {
        dangdau = false;
        AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
        if (chedodau == CheDoDau.VienChinh)
        {
            CancelInvoke("QuayVe");
            XemPhoBan xemphoban = AllMenu.ins.menu["menuPhoban"].GetComponent<XemPhoBan>();
            camera.transform.position = xemphoban.transform.position;
            xemphoban.gameObject.SetActive(true);
            if (AllMenu.ins.menu.ContainsKey("MenuShop")) AllMenu.ins.menu["MenuShop"].SetActive(false);
        }
        if (chedodau == CheDoDau.LoiDai)
        {
            AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
            net.menuLoiDai.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            reset();
        }
        else if (chedodau == CheDoDau.Halloween)
        {
            AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);

           // AllMenu.ins.transform.Find("MenuEventHalloween2024").gameObject.SetActive(true);
            AllMenu.ins.menu["MenuEventHalloween2024"].SetActive(true);
        }
        else if (chedodau == CheDoDau.BossTG)
        {
            CancelInvoke("QuayVe");
            AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
            crgame.menulogin.SetActive(true);
            crgame.VaoBossTheGioi();
        }
        else if (chedodau == CheDoDau.SoloKhongTuoc)
        {
            net.socket.Emit("soloKhongTuoc", JSONObject.CreateStringObject("ketqua+" + dameKhongTuoc));
            crgame.OpenMenuEvent();
            AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            reset();
        }
        else if (chedodau == CheDoDau.Replay)
        {
            ReplayData.QuayVe();
        }
        else if (chedodau == CheDoDau.solo)
        {
            soloWin("thua",true);
        }
        else if (chedodau == CheDoDau.Online)
        {
            chedodau = CheDoDau.Null;
            reset();
            MenuTTNguoichoi.QuayVe();
            DauTruongOnline.OpenMenu();
        }
        if (chedodau == CheDoDau.LanSu)
        {
            CancelInvoke("QuayVe");
            if (AllMenu.ins.menu.ContainsKey("MenuShop")) AllMenu.ins.menu["MenuShop"].SetActive(false);
            // CrGame.ins.trencung.transform.Find("GiaoDienLanSu").gameObject.SetActive(true);
            // crgame.OpenGiaoDienLanSu();
            GiaoDienLanSu.KetQua(KetQuaTranDau.Thua,true)();
            NetworkManager.ins.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanNgay");
        }
        if (chedodau == CheDoDau.XucTu)
        {
            CancelInvoke("QuayVe");
            if (AllMenu.ins.menu.ContainsKey("MenuShop")) AllMenu.ins.menu["MenuShop"].SetActive(false);
            // CrGame.ins.trencung.transform.Find("GiaoDienLanSu").gameObject.SetActive(true);
            // crgame.OpenGiaoDienLanSu();
            // EventDaiChienThuyQuai.KetQua(KetQuaTranDau.Thua, true)();
            CrGame.ins.OpenMenuRaKhoi();
            NetworkManager.ins.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanNgay");
        }
        //   else MenuTTNguoichoi.QuayVe();
        reset();
    }
    public void OpenPanelWinLose(string Win,string info)
    {
        GiaoDienPVP.ins.menuWin.SetActive(true);

        if (Win == "Win") GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
        else GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;

        GiaoDienPVP.ins.thongtin.text = info;
        GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
        GiaoDienPVP.ins.btnSetting.SetActive(true);
        GiaoDienPVP.ins.spriteWin.SetNativeSize();
        Invoke("QuayVe", 10f);
    }    

    void ChapNhan()
    {
        AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
    }
    public void ClearQuai()
    {
        for (int i = 1; i < TeamDo.transform.childCount; i++)
        {
            // TeamDo.transform.GetChild(i).transform.Find("skill").GetComponent<DragonPVEController>().AnimWin();
            Destroy(TeamDo.transform.GetChild(i).gameObject);
        }
        for (int i = 1; i < TeamXanh.transform.childCount; i++)
        {
            // TeamXanh.transform.GetChild(i).transform.Find("skill").GetComponent<DragonPVEController>().AnimWin();
            Destroy(TeamXanh.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 3; i++)
        {
            GiaoDienPVP.ins.OSkill.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < ObjSkill.transform.childCount; i++)
        {
            Destroy(ObjSkill.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 3; i++)
        {
            timeskill[i] = 0;
        }
        GiaoDienPVP.ins.maxtime = 0;
        GiaoDienPVP.ins.TxtTime.gameObject.SetActive(false);
        //    Destroy(GiaoDienPVP.ins.TxtTime.GetComponent<timePvp>());
        GiaoDienPVP.ins.TxtTime.GetComponent<timePvp>().enabled = false;
        GiaoDienPVP.ins.btnSetting.SetActive(false);
        Time.timeScale = 1;
        GiaoDienPVP.ins.menuSetting.SetActive(false);
        GiaoDienPVP.ins.txtHuyenTinh.transform.parent.gameObject.SetActive(false);
        dangdau = false;
        PoolEffect.SetActive(false);
        clearSkill();
    }
    void clearSkill()
    {
        if(AllMenu.ins.menu.ContainsKey("GiaoDienPVP"))
        {
            for (int j = 1; j <= 2; j++)
            {
                GameObject objteam = AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(j).gameObject;
                for (int i = 0; i < objteam.transform.childCount; i++)
                {
                    Destroy(objteam.transform.GetChild(i).gameObject);
                }
            }
        }

    }
    public void OffThangCap()
    {
        StartCoroutine(off());
        IEnumerator off()
        {
            yield return new WaitForSeconds(5);
            CrGame.ins.SetPanelThangCap(false);
        }
    }

    public void soloWin(string s,bool outlien = false)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DragonIsland";
        datasend["method"] = "KetQuaSolo";
        datasend["data"]["ketqua"] = s;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json) { }

        GiaoDienPVP.ins.btnSetting.SetActive(false);
        SetAnimWinAllDra();
        StartCoroutine(delay());
        IEnumerator delay()
        {
            if (s == "win")
            {
                //Invoke("invoke", 5);
                crgame.txtThangCap.text = "Win";
                CrGame.ins.SetPanelThangCap(true);
                GiaoDienPVP.ins.btnSetting.gameObject.SetActive(false);
                crgame.StartCoroutine(crgame.offlencap(crgame.txtThangCap.text));
            }
            if (outlien) yield return new WaitForSeconds(0f);
            else yield return new WaitForSeconds(3f);
             dangdau = false;
         AudioManager.SetSoundBg("nhacnen0");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        for (int i = 1; i < TeamDo.transform.childCount; i++)
        {
            Destroy(TeamDo.transform.GetChild(i).gameObject);
        }
        for (int i = 1; i < TeamXanh.transform.childCount; i++)
        {
            Destroy(TeamXanh.transform.GetChild(i).gameObject);
        }
        GiaoDienPVP.ins.DestroyAllItemRong();
 
        GiaoDienPVP.ins.panelBatdau.transform.GetChild(0).gameObject.SetActive(false);
        friend.GiaoDienNgoai.SetActive(true);
        Vector3 tf = friend.DaoFriend.transform.GetChild(friend.DangODaoFriend).transform.position;
        tf.z = -10;
        crgame.giaodien.SetActive(true);
        camera.transform.position = tf;
        friend.GiaoDienNgoai.SetActive(true);
        AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
        //    gameObject.SetActive(false);
        GetComponent<VienChinh>().enabled = false;
        clearSkill();
        }
       
    }

    public void reset()
    {
        ResetTru();
        chedodau = CheDoDau.Null;
        GiaoDienPVP.ins.DestroyAllItemRong();
        GiaoDienPVP.ins.menuWin.SetActive(false);
        GiaoDienPVP.ins.panelBatdau.transform.GetChild(0).gameObject.SetActive(false);
        ClearQuai();
        GetComponent<VienChinh>().enabled = false; transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ResetTru()
    {
        TruVienChinh trux = TruXanh.GetComponent<TruVienChinh>();
        trux.ReserHp();
        //for (int i = 0; i < trux.Hp.Length; i++)
        //{
        //    trux.Hp[i] = trux.MaxHp[i];
        //}
        //trux.LoadImgHp();
        //trux.GetComponent<Animator>().Play("truxanh");
        TruVienChinh trud = TruDo.GetComponent<TruVienChinh>();
        trud.ReserHp();
        //for (int i = 0; i < trux.Hp.Length; i++)
        //{
        //    trud.Hp[i] = trud.MaxHp[i];
        //}
        //trud.LoadImgHp();
        // trud.GetComponent<Animator>().Play("trudodungyen");
    }

    //IEnumerator delay()
    //{
    //    for (int i = 0; i < Random.Range(5,15); i++)
    //    {
    //        GameObject instance = Instantiate(tinhanhVienchinh,new Vector3(TruDo.transform.position.x + 2300, TruDo.transform.position.y + Random.Range(-2.5f, 0.5f)),Quaternion.identity)as GameObject;
    //        instance.transform.SetParent(GameObject.Find("TeamDo").transform,false);
    //        instance.SetActive(true);
    //        yield return new WaitForSeconds(Random.Range(1,3));
    //    }
    //}
    // Update is called once per frame
    // public List<Transform> testtttDraDoDungTruoc;
    // public List<Transform> testtttDraXanhDungTruoc;
    private void OnApplicationFocus(bool focus)
    {
        // debug.LogError("focus vien chinh" + focus);
        if (!this.enabled) return;

#if (!UNITY_EDITOR)
        if (chedodau == CheDoDau.Online)
        {
            net.socket.Emit("FocusGame",JSONObject.CreateStringObject(focus.ToString()));
        }
#endif
    }
    public float dragSpeed = 2.0f; // Tốc độ kéo
    public float smoothing = 0.1f; // Hệ số giảm chấn
    private Vector3 velocity = Vector3.zero;
    private bool isDragging = false;
    public IEnumerator Shake(float duration = 0.12f, float magnitude = 0.08f)
    {
        Vector3 originalPosition = CrGame.ins.transform.localPosition; // Lưu vị trí gốc của camera
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Tạo độ lệch ngẫu nhiên
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Tính toán vị trí rung
            Vector3 shakenPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            // Giới hạn vị trí rung bằng Clampcamera
            CrGame.ins.transform.localPosition = Clampcamera(shakenPosition);

            elapsed += Time.deltaTime;

            yield return null; // Đợi đến khung hình tiếp theo
        }

        // Trả camera về vị trí gốc
        CrGame.ins.transform.localPosition = originalPosition;
    }
    private object locktimmuctieudo = new object();  // Một đối tượng để khóa
    private object locktimmuctieuxanh = new object();  // Một đối tượng để khóa

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) // Nhấn phím Space để thử nghiệm
        //{
        //    StartCoroutine(Shake()); // Rung màn hình trong 0.2 giây với cường độ 0.1
        //}
        //if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        //    }
        //    if (Input.GetMouseButton(0))
        //    {
        //        Vector3 difrence = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
        //        cam.transform.position = Clampcamera(cam.transform.position + difrence);
        //    }
        //}    

        //if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    debug.Log("hoán đổi team");
            
        //}    
            if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isDragging)
                {
                    dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
                    isDragging = true;
                }
            }
            if (Input.GetMouseButton(0) && isDragging)
            {
                Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 targetPosition = cam.transform.position + difference;
                cam.transform.position = Vector3.SmoothDamp(cam.transform.position, Clampcamera(targetPosition), ref velocity, smoothing, dragSpeed);
                GiaoDienPVP.ins.minimap.UpdateMapIndicator();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        var oSkillTransform = GiaoDienPVP.ins.OSkill.transform.GetChild(1);

        for (int i = 0; i < 3; i++)
        {
            var skillButton = oSkillTransform.GetChild(i).GetComponent<Button>();
            var child1 = oSkillTransform.GetChild(i).transform.GetChild(1);
            var imgthanhcho = child1.GetComponent<Image>();
            var textComponent = child1.transform.GetChild(0).GetComponent<Text>();

            if (timeskill[i] > 0)
            {
                timeskill[i] -= Time.deltaTime;
                skillButton.interactable = false;
                imgthanhcho.gameObject.SetActive(true);
                textComponent.text = Math.Round(timeskill[i], 1).ToString();
                imgthanhcho.fillAmount = timeskill[i] / 15;
            }
            else
            {
                skillButton.interactable = true;
                imgthanhcho.gameObject.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        ReplayData.time += Time.fixedDeltaTime * ReplayData.speedReplay;
        SetMucTieuTeamDo();
        SetMucTieuTeamXanh();
    }
   
    //public void SetMucTieuTeamDo()
    //{
    //    if (TeamXanh.transform.childCount > 1)
    //    {
    //        float sotohon = 0;
    //        for (short i = 1; i < TeamXanh.transform.childCount; i++)
    //        {
    //            if (TeamXanh.transform.GetChild(i).transform.position.x > sotohon)
    //            {
    //                sotohon = TeamXanh.transform.GetChild(i).transform.position.x;
    //                muctieudo = TeamXanh.transform.GetChild(i).gameObject;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        muctieudo = TeamXanh.transform.GetChild(0).gameObject;
    //    }
    //    if (muctieudo == null)
    //    {
    //        if (TeamXanh.transform.childCount > 1) muctieudo = TeamXanh.transform.GetChild(1).gameObject;
    //        else muctieudo = TeamXanh.transform.GetChild(0).gameObject;
    //    }
    //}
    //public void SetMucTieuTeamXanh()
    //{
    //    if (TeamDo.transform.childCount > 1)
    //    {
    //        float sonhohon = TruDo.transform.position.x;
    //        for (short i = 1; i < TeamDo.transform.childCount; i++)
    //        {
    //            if (TeamDo.transform.GetChild(i).transform.position.x < sonhohon)
    //            {
    //                sonhohon = TeamDo.transform.GetChild(i).transform.position.x;
    //                muctieuxanh = TeamDo.transform.GetChild(i).gameObject;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        muctieuxanh = TeamDo.transform.GetChild(0).gameObject;
    //    }
    //    if (muctieuxanh == null)
    //    {
    //        if (TeamDo.transform.childCount > 1) muctieuxanh = TeamDo.transform.GetChild(1).gameObject;
    //        else muctieuxanh = TeamDo.transform.GetChild(0).gameObject;
    //    }
    //}
    public void SetMucTieuTeamDo()
    {
        lock(locktimmuctieudo)
        {
            int childCount = TeamXanh.transform.childCount;

            if (childCount > 1)
            {
                float maxXPosition = float.MinValue;
                Transform selectedChild = null;

                for (short i = 1; i < childCount; i++)
                {
                    Transform childTransform = TeamXanh.transform.GetChild(i);
                    float childXPosition = childTransform.position.x;

                    if (childXPosition > maxXPosition)
                    {
                        maxXPosition = childXPosition;
                        selectedChild = childTransform;
                    }
                }

                muctieudo = selectedChild != null ? selectedChild.gameObject : null;
            }
            else
            {
                muctieudo = TeamXanh.transform.GetChild(0).gameObject;
            }

            if (muctieudo == null)
            {
                muctieudo = childCount > 1 ? TeamXanh.transform.GetChild(1).gameObject : TeamXanh.transform.GetChild(0).gameObject;
            }
        }    
   
    }

    public void SetMucTieuTeamXanh()
    {
        lock (locktimmuctieuxanh)
        {
            int childCount = TeamDo.transform.childCount;

            if (childCount > 1)
            {
                float minXPosition = TruDo.transform.position.x;
                Transform selectedChild = null;

                for (short i = 1; i < childCount; i++)
                {
                    Transform childTransform = TeamDo.transform.GetChild(i);
                    float childXPosition = childTransform.position.x;

                    if (childXPosition < minXPosition)
                    {
                        minXPosition = childXPosition;
                        selectedChild = childTransform;
                    }
                }

                muctieuxanh = selectedChild != null ? selectedChild.gameObject : null;
            }
            else
            {
                muctieuxanh = TeamDo.transform.GetChild(0).gameObject;
            }

            if (muctieuxanh == null)
            {
                muctieuxanh = childCount > 1 ? TeamDo.transform.GetChild(1).gameObject : TeamDo.transform.GetChild(0).gameObject;
            }
        }
        
    }

    public void CloseVienchinh()
    {
        Vector3 tf = GameObject.Find("BGDao1").transform.position;
        tf.z = -10;
        camera.transform.position = tf;
        crgame.giaodien.SetActive(true);
        gameObject.SetActive(false);
        AllMenu.ins.menu["GiaoDienPVP"].SetActive(false);
    }


  public  IEnumerator CreateHieuUngSkillsBuff(global::Team team,string nameskill)
    {
        // Tải prefab hiệu ứng
        GameObject objskill = Resources.Load("GameData/Skill/" + nameskill) as GameObject;
        if (objskill == null)
        {
            Debug.LogError("Không tìm thấy objskill!");
            yield break;
        }

        Transform startPoint = null;

        // Xác định điểm bắt đầu dựa trên team
        if (team == global::Team.TeamXanh)
        {
            startPoint = VienChinh.vienchinh.TruXanh.transform;
        }
        else
        {
            startPoint = VienChinh.vienchinh.TruDo.transform;
        }

        // Kiểm tra startPoint
        if (startPoint == null)
        {
            Debug.LogError("StartPoint không hợp lệ!");
            yield break;
        }

        // Hiển thị panel
        GiaoDienPVP.ins.SetPanelToi = true;

        // Lấy vị trí động của muctieu
        Vector3 endPosition = (team == global::Team.TeamXanh)
            ? VienChinh.vienchinh.muctieuxanh.transform.position
            : VienChinh.vienchinh.muctieudo.transform.position;


        endPosition = new Vector3(endPosition.x, endPosition.y, startPoint.transform.position.z);

      //  Debug.Log($"StartPoint: {startPoint.position}, EndPosition: {endPosition}");

        // Tính khoảng cách và số hiệu ứng
        float distance = Vector3.Distance(startPoint.position, endPosition);

        // Xử lý ngoại lệ cho distance
        //if (distance > 30f)
        //{
        //    Debug.LogWarning("Khoảng cách quá lớn, giới hạn lại!");
        //    distance = 30f; // Giới hạn khoảng cách tối đa
        //}

        int skillCount = Mathf.Max(Mathf.FloorToInt(distance / 3f), 1); // Ít nhất 1 hiệu ứng

       // Debug.Log($"Distance: {distance}, SkillCount: {skillCount}");

        // Reset các hiệu ứng cũ (nếu cần)
        foreach (Transform child in startPoint)
        {
            if (child.name.Contains("HapHuyetHacLong"))
            {
                Destroy(child.gameObject);
            }
        }

        if (skillCount == 1)
        {
            // Nếu chỉ có 1 hiệu ứng, đặt nó ở giữa
            Vector3 middlePosition = (startPoint.position + endPosition) / 2;
            middlePosition.y = startPoint.position.y; // Giữ nguyên trục Y

            GameObject hieuung = Instantiate(objskill, middlePosition, Quaternion.identity);
            hieuung.name = "HapHuyetHacLong";
            hieuung.transform.SetParent(startPoint); // Gắn vào trụ để quản lý hierarchy
            hieuung.SetActive(true);

            Destroy(hieuung, 1f); // Tự động hủy sau 1 giây
             StartCoroutine(VienChinh.vienchinh.Shake());
        }
        else
        {
            // Tạo nhiều hiệu ứng
            for (int i = 0; i < skillCount; i++)
            {
                float t = i / (float)(skillCount - 1); // Giá trị từ 0 đến 1
                Vector3 spawnPosition = Vector3.Lerp(startPoint.position, endPosition, t);

                spawnPosition.y = startPoint.position.y + 2; // Giữ nguyên trục Y

                GameObject hieuung = Instantiate(objskill, spawnPosition, Quaternion.identity);
                hieuung.name = "HapHuyetHacLong";
                hieuung.transform.SetParent(startPoint); // Gắn vào trụ để quản lý hierarchy
                hieuung.SetActive(true);

                if (i % 2 == 0)
                {
                    StartCoroutine(VienChinh.vienchinh.Shake());
                }

                Destroy(hieuung, 1.5f);

                yield return new WaitForSeconds(0.2f); // Delay trước khi tạo hiệu ứng tiếp theo
            }
        }
        // Ẩn panel
        GiaoDienPVP.ins.SetPanelToi = false;
    }

    public GameObject InstantiateHieuUngSkill(string nameSkill,string team = "TeamDo")
    {
        GameObject objskill = Resources.Load("GameData/Skill/" + nameSkill) as GameObject;
        ReplayData.AddHieuUngSkill(nameSkill);
        if (nameSkill == "SkillSamNo" || nameSkill == "SkillCuongLoan")
        {
            string team_ = (team == "TeamDo") ? "TeamXanh" : "TeamDo";
          StartCoroutine(CreateHieuUngSkillsBuff((Team)Enum.Parse(typeof(Team), team_), nameSkill));

        }
        else if (nameSkill == "SkillDatBom")
        {
            GameObject Team = TeamDo;
            if (team != "TeamDo") Team = TeamXanh;

            int giua = Team.transform.childCount / 2;
            //int giua = UnityEngine.Random.Range(1, TeamDo.transform.childCount-1);
            GameObject hieuung = Instantiate(objskill, transform.position, Quaternion.identity);
            hieuung.GetComponent<DatBom>().Team = Team;
            hieuung.transform.SetParent(TruDo.transform);
            hieuung.transform.position = new Vector3(Team.transform.GetChild(giua).transform.position.x, Team.transform.GetChild(giua).transform.position.y);
            hieuung.SetActive(true);

        }
        else if(nameSkill == "SkillBienCuu")
        {

        }
        else if (nameSkill == "SkillDienKienTuThan")
        {

        }
        else
        {
                   StartCoroutine(CreateHieuUngSkillsBuff((Team)Enum.Parse(typeof(Team), team), nameSkill));

            return objskill;
            GameObject Team = TeamDo;
            if (team != "TeamDo") Team = TeamXanh;

            int chia = 1;
            if (Team.transform.childCount >= 6) chia = 2;
            else if (Team.transform.childCount >= 15) chia = 3;
            else if (Team.transform.childCount >= 30) chia = 4;
            StartCoroutine(delaySkill());

            IEnumerator delaySkill()
            {
                GiaoDienPVP.ins.SetPanelToi = true;
                for (int i = 0; i < Team.transform.childCount;)
                {
                    int tf = 0;
                    if (i + chia < Team.transform.childCount)
                    {
                        tf = i + chia;
                    }
                    else tf = i;
                    GameObject hieuung = Instantiate(objskill, transform.position, Quaternion.identity);
                    hieuung.transform.SetParent(TruDo.transform);
                    Vector3 vec = new Vector3();
                    if(i % 2 == 0) StartCoroutine(Shake());
                    if (nameSkill == "SkillDatBangDaySong" || nameSkill == "SkillMatTroiXuongNui") vec = new Vector3(Team.transform.GetChild(tf).transform.position.x, TruDo.transform.position.y + UnityEngine.Random.Range(1.5f, 2.5f));
                    else if (nameSkill == "SkillGioPhuongBac") vec = new Vector3(Team.transform.GetChild(tf).transform.position.x, TruDo.transform.position.y + UnityEngine.Random.Range(0.5f, 1f));
                    else vec = new Vector3(Team.transform.GetChild(tf).transform.position.x, TruDo.transform.position.y - UnityEngine.Random.Range(1f, 2f));
                    hieuung.transform.position = vec;
                    hieuung.SetActive(true);
                    Destroy(hieuung, 2f);
                    i += chia;
                    yield return new WaitForSeconds(0.2f);
                }
                GiaoDienPVP.ins.SetPanelToi = false;
                // giaodienPvp.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
        return objskill;
    }
    public void HieuUngSkill(string nameSkill,float dame,float level, bool setOnline = false,string team = "TeamDo")
    {
        //  giaodienPvp.transform.GetChild(3).gameObject.SetActive(true);
        if (vienchinh.chedodau == CheDoDau.Online)
        {
            if (!setOnline)
            {
                JSONObject newjsonn = new JSONObject();
                newjsonn.AddField("hieuungskill", nameSkill);
                newjsonn.AddField("dame", dame.ToString());
                newjsonn.AddField("level", level.ToString());
                newjsonn.AddField("team", DauTruongOnline.namedoithu);
                DauTruongOnline.ins.AddUpdateData(newjsonn, true);
              
            }
           // if (nameSkill == "SkillBienCuu" || nameSkill == "SkillCuongLoan") return;
        }
      //  debug.Log("xong1");

        GameObject hieuungg = InstantiateHieuUngSkill(nameSkill,team);
        // debug.Log("xong2");

        if (nameSkill == "SkillSamNo")
        {

        }
        else if (nameSkill == "SkillCuongLoan")
        {
            GameObject teammm = TeamXanh;
            if (team == "TeamXanh") teammm = TeamDo;
            float phantramdame = 20 + level * 2;
            float time = 3 + level * 0.2f;
            if (team == "TeamDo") vienchinh.HienIconSkill(time, "Xanh", "iconCuongLoanXanh");
            for (int i = 1; i < teammm.transform.childCount; i++)
            {
                DragonPVEController cs = teammm.transform.GetChild(i).GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
                //   cs.DayLui("TeamDo",0.5f);
                cs.newDame(cs.dame + cs.dame * phantramdame / 100, time, -200);
                cs.MatMau(cs.hp * 30 / 100, cs);
            }
        }
        else if (nameSkill == "SkillDatBom")
        {
            DatBom datbom = hieuungg.GetComponent<DatBom>();
            datbom.dame = dame;
            datbom.level = level;
        }
        else if (nameSkill == "SkillBienCuu")
        {
         
        }
        else if (nameSkill == "SkillDienKienTuThan" && !setOnline)
        {
            debug.Log("su dung DienKienTuThan");
            string[] randomthebai = new string [0];
            string nameskill = "";
            float[] probabilities = { 0.4f, 0.3f, 0.3f }; // Xác suất: 40%, 40%, 20%
            float randomPoint = Random.value; // Random từ 0.0 đến 1.0

            if (randomPoint < probabilities[0]) // 40%
            {
                randomthebai = new string[] { "doatmenhhaclong", "xhaclong", "doatmenhhaclong" }; // cuongno
                nameskill = "CuongNo";
            }
            else if (randomPoint < probabilities[0] + probabilities[1]) // 30%
            {
                randomthebai = new string[] { "doatmenhhaclong", "xhaclong", "xhaclong" }; // haphuyet
                nameskill = "HapHuyet";
            }
            else // 30%
            {
                randomthebai = new string[] { "doatmenhhaclong", "doatmenhhaclong", "doatmenhhaclong" }; // doatmenh
                nameskill = "DoatMenh";
            }

           // nameskill = "HapHuyet";
            Transform Obj = GiaoDienPVP.ins.OSkill.transform.Find("DienKienTuThan");
            Obj.gameObject.SetActive(true);
            StartCoroutine(delay());
            IEnumerator delay()
            {
                for (int i = 0; i < 3; i++)
                {
                    Image img = Obj.transform.GetChild(i).GetComponent<Image>();
                    Animator anim = Obj.transform.GetChild(i).GetComponent<Animator>();

                    Obj.transform.GetChild(i).gameObject.SetActive(true);
                    anim.enabled = true;
                    yield return new WaitForSeconds(0.2f);
                   
                    anim.enabled = false;
                    img.sprite = Inventory.LoadSprite(randomthebai[i]);
                    img.SetNativeSize();

                }
                for (int i = 1; i < vienchinh.TeamXanh.transform.childCount; i++)
                {
                    DragonPVEController dra = vienchinh.TeamXanh.transform.GetChild(i).GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
                    if (dra.nameobj == "RongHacLong")
                    {
                        HacLongAttack haclong = dra.GetComponent<HacLongAttack>();
                        // haclong.CuongNo();
                        haclong.FuncInvokeOnline(nameskill,false);
                    }
                }

                yield return new WaitForSeconds(3f);
                Obj.gameObject.SetActive(false);
                for (int i = 0; i < 3; i++)
                {
                    Obj.transform.GetChild(i).gameObject.SetActive(false);

                    RectTransform rectTransform = Obj.transform.GetChild(i).GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(200, 200); // Thay đổi kích thước

                }
            }
        }
        else
        {
            for (int i = 1; i < vienchinh.TeamDo.transform.childCount; i++)
            {
                DragonPVEController dragonPVEController = vienchinh.TeamDo.transform.GetChild(i).GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
                if (dragonPVEController != null)
                {
                    // ChiSo cs = vienchinh.TeamDo.transform.GetChild(i).GetComponent<ChiSo>();

                    dragonPVEController.MatMau(dame, null);

                }
            }
        }    
    }

    public void SetBienCuuOnline(Transform dra,float time)
    {
        if (dra.transform.Find("BienCuu") == null)
        {
            dra.transform.Find("SkillDra").GetComponent<DragonPVEController>().BienCuuABS(time);
        }
        else
        {
            GameObject hieuung = dra.transform.Find("BienCuu").gameObject;
            hieuung.GetComponent<BienCuu>().time = time;
            hieuung.SetActive(true);
        }
    }
    public string nameskillthuthach;
    public string nameskillthuthachdich;
    byte sorongxanhchet = 0;
    byte sorongdochet = 0;
    byte tatcarongchet = 0;
    public void KichHoatSkillThuThach(string nameskill, string TeamKichHoat)
    {
        if (nameskill == "LoiKeo")
        {
            if (TeamKichHoat == "TeamXanh")
            {
                sorongxanhchet += 1;
                if (sorongxanhchet >= 2)
                {
                    sorongxanhchet = 0;
                    debug.LogError("Team Xanh kích hoạt skill lôi kéo");
                    GameObject animationskill = Inventory.LoadObjectResource("GameData/Skill/LoiKeo");
                    GameObject animskill = Instantiate(animationskill, transform.position, Quaternion.identity);
                   
                    int rongrandom = UnityEngine.Random.Range(1, net.vienchinh.TeamDo.transform.childCount - 1);
                    animskill.transform.position = net.vienchinh.TeamDo.transform.GetChild(rongrandom).gameObject.transform.position;
                    animskill.SetActive(true);
                    Destroy(animskill, 1);
                    Destroy(TeamDo.transform.GetChild(rongrandom).gameObject);
                }
            }
            else
            {
                sorongdochet += 1;
                if (sorongdochet >= 2)
                {
                    sorongdochet = 0;
                    debug.LogError("Team Đỏ kích hoạt skill lôi kéo");
                    GameObject animationskill = Inventory.LoadObjectResource("GameData/Skill/LoiKeo");
                    GameObject animskill = Instantiate(animationskill, transform.position, Quaternion.identity);
                    int rongrandom = UnityEngine.Random.Range(1, net.vienchinh.TeamXanh.transform.childCount - 1);
                    animskill.transform.position = net.vienchinh.TeamXanh.transform.GetChild(rongrandom).gameObject.transform.position;
                    animskill.SetActive(true);
                    Destroy(animskill, 1);
                    Destroy(TeamXanh.transform.GetChild(rongrandom).gameObject);
                }
            }
        }
        if (nameskill == "HungPhan")
        {
            if (TeamKichHoat == "TeamXanh")
            {
                tatcarongchet += 1;
                if (tatcarongchet >= 2)
                {
                    tatcarongchet = 0;
                    debug.LogError("Team Xanh kích hoạt skill hưng phấn");
                    for (int i = 1; i < TeamXanh.transform.childCount; i++)
                    {
                        DragonPVEController cs = TeamXanh.transform.GetChild(i).GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
                        if (cs != null)
                        {
                            //Animator anim = TeamXanh.transform.GetChild(i).GetComponent<Animator>();
                            dataLamCham data = new dataLamCham(3, "", 0, (cs.speed + 0.5f).ToString() + "+1.7+1.7");
                            cs.LamChamABS(data);
                            //cs.speed += 0.5f;
                        }
                        else break;
                    }
                }
            }
            else
            {
                tatcarongchet += 1;
                if (tatcarongchet >= 2)
                {
                    tatcarongchet = 0;
                    debug.LogError("Team Đỏ kích hoạt skill hưng phấn");
                    for (int i = 1; i < TeamDo.transform.childCount; i++)
                    {
                        DragonPVEController cs = TeamDo.transform.GetChild(i).GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
                        if (cs != null)
                        {
                            //Animator anim = TeamXanh.transform.GetChild(i).GetComponent<Animator>();
                            dataLamCham data = new dataLamCham(3, "", 0, (cs.speed + 0.5f).ToString() + "+1.7+1.7");
                            cs.LamChamABS(data);
                            //cs.speed += 0.5f;
                        }
                        else break;
                    }

                   
                }
            }
        }
      
    }
    public void SetHienPlayer(dataHienPlayer data)
    {
        //  debug.Log("data " + data.ToString()) ;

        if (data.nameplayer1 == "") return;
        GiaoDienPVP.ins.hienPlayer.SetActive(true);

        GameObject player1 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(0).gameObject;

        player1.transform.GetChild(2).GetComponent<Text>().text = data.nameplayer1;
        player1.transform.GetChild(0).GetComponent<Image>().sprite = data.avtplayer1;
        if (data.khungavt1.GetComponent<Animator>())
        {
            RuntimeAnimatorController runtime = data.khungavt1.GetComponent<Animator>().runtimeAnimatorController;
            if (runtime != null)
            {
                Animator anim = player1.transform.GetChild(1).GetComponent<Animator>();
                anim.runtimeAnimatorController = runtime;
                GamIns.SetNativeSizeAnimator(anim);
            }
            else SetAvtPlayer1();
        }
        else SetAvtPlayer1();

        void SetAvtPlayer1()
        {
            // player1.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = null;
            Image img = player1.transform.GetChild(1).GetComponent<Image>();
            img.sprite = data.khungavt1.GetComponent<Image>().sprite;
            img.SetNativeSize();
        }
        GameObject player2 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(1).gameObject;

        player2.transform.GetChild(2).GetComponent<Text>().text = data.nameplayer2;
        player2.transform.GetChild(0).GetComponent<Image>().sprite = data.avtplayer2;
        if (data.khungavt2.GetComponent<Animator>())
        {
            RuntimeAnimatorController runtime = data.khungavt2.GetComponent<Animator>().runtimeAnimatorController;
            if (runtime != null)
            {
                Animator anim = player2.transform.GetChild(1).GetComponent<Animator>();
                anim.runtimeAnimatorController = runtime;
                GamIns.SetNativeSizeAnimator(anim);
            }
            else SetAvtPlayer2();
        }
        else SetAvtPlayer2();
        void SetAvtPlayer2()
        {
            //     player2.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = null;
            // Destroy(player2.transform.GetChild(1).GetComponent<Animator>());
            Image img = player2.transform.GetChild(1).GetComponent<Image>();
            img.sprite = data.khungavt2.GetComponent<Image>().sprite;
            img.SetNativeSize();
        }
    }
    public void SetHienPlayer(string id2,string name2)
    {
        //  debug.Log("data " + data.ToString()) ;

        GiaoDienPVP.ins.hienPlayer.SetActive(true);

        GameObject player1 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(0).gameObject;

        player1.transform.GetChild(2).GetComponent<Text>().text = crgame.FB_userName.text;
        player1.transform.GetChild(0).GetComponent<Image>().sprite = crgame.khungAvatar.transform.parent.GetComponent<Image>().sprite;
        if (crgame.khungAvatar.GetComponent<Animator>())
        {
            RuntimeAnimatorController runtime = crgame.khungAvatar.GetComponent<Animator>().runtimeAnimatorController;
            if (runtime != null)
            {
                Animator anim = player1.transform.GetChild(1).GetComponent<Animator>();
                anim.runtimeAnimatorController = runtime;
                GamIns.SetNativeSizeAnimator(anim);
            }
            else SetAvtPlayer1();
        }

        else SetAvtPlayer1();

        void SetAvtPlayer1()
        {
            // player1.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = null;
            Image img = player1.transform.GetChild(1).GetComponent<Image>();
            img.sprite = crgame.khungAvatar.GetComponent<Image>().sprite;
            img.SetNativeSize();
        }


        GameObject player2 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(1).gameObject;
        player2.transform.GetChild(2).GetComponent<Text>().text = name2;
    }
    public string GetTeamByName(string name)
    {
        GameObject player1 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(0).gameObject;
        if (player1.transform.GetChild(2).GetComponent<Text>().text == name) return "TeamXanh";
        else return "TeamDo";
    }
    public string GetNameDoiThuByTarget(Transform target)
    {
        if(target.transform.parent.name == "TeamXanh")
        {
            GameObject player1 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(0).gameObject;
            return player1.transform.GetChild(2).GetComponent<Text>().text;
        }
        else
        {
            GameObject player2 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(1).gameObject;
            return player2.transform.GetChild(2).GetComponent<Text>().text;
        }
    }    
    public void OffhienPlayer()
    {
        GiaoDienPVP.ins.hienPlayer.SetActive(false);
        GameObject player1 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(0).gameObject;
        player1.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = null;
        GameObject player2 = GiaoDienPVP.ins.hienPlayer.transform.GetChild(1).gameObject;
        player2.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = null;
    }
    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
    Vector3 Clampcamera(Vector3 targetPosition)
    {
        float CamHeight = cam.orthographicSize;
        float CamWidth = cam.orthographicSize * cam.aspect;
        float MinX = MapMinX + CamWidth;
        float MaxX = MapMaxX - CamWidth;
        float MinY = mapMiny + CamHeight;
        float MaxY = MapMaxY - CamHeight;
        float NewX = Mathf.Clamp(targetPosition.x, MinX, MaxX);
        float NewY = Mathf.Clamp(targetPosition.y, MinY, MaxY);
        return new Vector3(NewX, NewY, -10);
    }

    public void GetDoiHinh(string namemap,CheDoDau chedodau)
    {
        NetworkManager.ins.vienchinh.nameMapvao = namemap;

        NetworkManager.ins.socket.Emit("DoiHinhDanh", JSONObject.CreateStringObject(NetworkManager.ins.vienchinh.nameMapvao + "/" + chedodau.ToString()));

        NetworkManager.ins.vienchinh.enabled = true;

        NetworkManager.ins.vienchinh.chedodau = chedodau;
    }
    public class HandleSocket
    {
        private static Dictionary<string, Action<SocketIOEvent>> eventhandle = new Dictionary<string, Action<SocketIOEvent>>
        {
            {"doihinh",TaoDoiHinh},
            {"xanhtrieuhoi",XanhTrieuHoi},
            {"dotrieuhoi",DoTrieuHoi},
        };
        //public HandleSocket(SocketIOEvent e)
        //{
        //    eventhandle[e.data[0].str](e);
        //}
        public static void ParseData(SocketIOEvent e)
        {
           // debug.LogError("Parse dataaa, event: " + e.data.keys[0]);
            eventhandle[e.data.keys[0]](e);
        }
        private static void TaoDoiHinh(SocketIOEvent e)
        {
            //debug.Log("che do dau " + e.data["chedodau"].str);
          //  CheDoDau chedodau = (CheDoDau)Enum.Parse(typeof(CheDoDau), e.data["chedodau"].str);
            
            //if(chedodau == CheDoDau.LanSu)
            //{
            //    if (!AllMenu.ins.menu.ContainsKey("GiaoDienLanSu")) return;
            //}
            GiaoDienPVP gdpvp = AllMenu.ins.GetCreateMenu("GiaoDienPVP").GetComponent<GiaoDienPVP>();
            for (int i = 0; i < e.data["doihinh"].Count; i++)
            {
                string id = e.data["doihinh"][i]["id"].str;
                string nameObject = e.data["doihinh"][i]["nameobject"].str;
                gdpvp.AddItemRongDanh(nameObject, id, int.Parse(e.data["doihinh"][i]["sao"].ToString()), int.Parse(e.data["doihinh"][i]["tienhoa"].ToString()), i);
            }
            GiaoDienPVP.ins.LoadSkill(e.data["skill"]);
            gdpvp.maxtime = float.Parse(GamIns.CatDauNgoacKep(e.data["time"].ToString())) * 60;
            GiaoDienPVP.ins.TxtTime.GetComponent<timePvp>().enabled = true;
            if (GamIns.CatDauNgoacKep(e.data["icontoclenh"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconTocLenhXanh");
            if (GamIns.CatDauNgoacKep(e.data["iconhoathanlong"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconHoaThanLongXanh");
            if (GamIns.CatDauNgoacKep(e.data["iconcovip"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconCoVipXanh");
            if (GamIns.CatDauNgoacKep(e.data["iconNuiThanBi"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconNuiThanBiXanh");
            vienchinh.StartCoroutine(vienchinh.delayGame());
        }
        private static void XanhTrieuHoi(SocketIOEvent e)
        {
            PVEManager.TrieuHoiDra(e.data["xanhtrieuhoi"], vienchinh.Teamthis.ToString());
            if (e.data["truhuyentinh"])
            {
                Inventory.ins.AddItem("HuyenTinh", -int.Parse(e.data["truhuyentinh"].ToString()));
                GiaoDienPVP.ins.txtHuyenTinh.text = Inventory.ins.ListItemThuong["itemHuyenTinh"].transform.GetChild(0).GetComponent<Text>().text;
            }
        }
        private static void DoTrieuHoi(SocketIOEvent e)
        {
            PVEManager.TrieuHoiDra(e.data["dotrieuhoi"], vienchinh.Teamthis == Team.TeamXanh? "TeamDo":"TeamXanh");
        }
    }    
}
public class dataHienPlayer : MonoBehaviour
{
    public string nameplayer1;
    public GameObject khungavt1;
    public Sprite avtplayer1;
    public string nameplayer2;
    public GameObject khungavt2;
    public Sprite avtplayer2;
    public int btnchon = 1;
    public dataHienPlayer(string name1, GameObject khung1, Sprite avt1, string name2, GameObject khung2, Sprite avt2, int btchon)
    {
        nameplayer1 = name1;
        khungavt1 = khung1;
        avtplayer1 = avt1;

        nameplayer2 = name2;
        khungavt2 = khung2;
        avtplayer2 = avt2;

        btnchon = btchon;
    }    
    public dataHienPlayer(string name1 = "")
    {
        nameplayer1 = name1;
    }
}
