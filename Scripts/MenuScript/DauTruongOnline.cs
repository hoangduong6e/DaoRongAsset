using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using SocketIO;

public class DauTruongOnline : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] imgAvatar;
    public GameObject MenuMoiFriend,OFriend,ContentMoi;
    public GameObject[] btnMoi;
    public bool coroom = false;
    private bool updateData = false;
    //public static bool GetUpdateDra
    //{
    //    get
    //    {
    //        if(DauTruongOnline.ins == null) return false;
    //        if(VienChinh.vienchinh.chedodau != CheDoDau.Online) return false;
    //        return DauTruongOnline.ins.updateData;
    //    }
    //}
    public static DauTruongOnline ins;
    public static string namedoithu = "";
    public static Dictionary<string, Transform> dicdra;
    private void Awake()
    {
        ins = this;
        NetworkManager.ins.socket.On("DauTruongOnline", DauOnline);
        dicdra = new Dictionary<string, Transform>();
       // debug.Log("new dic draaaa");
    }
    public void AddUpdateData(JSONObject json,bool set = false)
    {
        if (!updateData && !set) return;
        NetworkManager.ins.socket.Emit("UpdateDra", json);
    }
    private void OnApplicationFocus(bool focus)
    {
#if (!UNITY_EDITOR)
           NetworkManager.ins.socket.Emit("FocusGame", JSONObject.CreateStringObject(focus.ToString()));

        GameObject PanelTimPlayer = transform.Find("PanelTimPlayer").gameObject;
        PanelTimPlayer.SetActive(false);
#endif

    }
    void DauOnline(SocketIOEvent e)
    {
       // debug.Log(e.data);
        if (e.data["LoadTeam"])
        {
            Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
            Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
            GiaoDienPVP giaoDienPVP = AllMenu.ins.GetCreateMenu("GiaoDienPVP").GetComponent<GiaoDienPVP>();
            giaoDienPVP.SoDoihinh = 0;
            txtProgress.text = "0%";
            Progress.fillAmount = 0 / (float)100;
            giaoDienPVP.LoadSkill(e.data["LoadTeam"]["skill"]);
            giaoDienPVP.DestroyAllItemRong();
            for (int i = 0; i < e.data["LoadTeam"]["doihinh"].Count; i++)
            {
                string id = e.data["LoadTeam"]["doihinh"][i]["id"].str;
                string nameObject = e.data["LoadTeam"]["doihinh"][i]["nameobject"].str;
                giaoDienPVP.AddItemRongDanh(nameObject, id, int.Parse(e.data["LoadTeam"]["doihinh"][i]["sao"].ToString()), int.Parse(e.data["LoadTeam"]["doihinh"][i]["tienhoa"].ToString()), i);
                // inventory.LoadRongCoBan(nameObject[1]);
            }
            NetworkManager.ins.vienchinh.chedodau = CheDoDau.Online;
            NetworkManager.ins.vienchinh.enabled = true;
            NetworkManager.ins.vienchinh.StartCoroutine(NetworkManager.ins.vienchinh.delayGame());
            updateData = bool.Parse(e.data["LoadTeam"]["updateData"].str);
            VienChinh.vienchinh.SetHienPlayer(e.data["LoadTeam"]["id2"].str, e.data["LoadTeam"]["nameid2"].str);
            namedoithu = e.data["LoadTeam"]["nameid2"].str;
            gameObject.SetActive(false);
            return;
        }
        else if (e.data["UpdateDra"])
        {
           // dataSet.Add();
            if (e.data["UpdateDra"]["team"])
            {
                if (e.data["UpdateDra"]["hieuungskill"])
                {
                    string hieuungskill = e.data["UpdateDra"]["hieuungskill"].str;
                    float dame = float.Parse(e.data["UpdateDra"]["dame"].str);
                    float level = float.Parse(e.data["UpdateDra"]["level"].str);
                    VienChinh.vienchinh.HieuUngSkill(hieuungskill, dame, level, true, VienChinh.vienchinh.GetTeamByName(e.data["UpdateDra"]["team"].str));
                    return;
                }
                //if (e.data["UpdateDra"]["HienIconSkill"])
                //{
                //    float timehien = float.Parse(e.data["UpdateDra"]["timehien"].str);
                //    string team = e.data["UpdateDra"]["team"].str;
                //    string namee = e.data["UpdateDra"]["namee"].str;
                //    if (VienChinh.vienchinh.GetTeamByName(team) == "TeamXanh") team = "Do";
                //    else team = "Xanh";

                //    VienChinh.vienchinh.HienIconSkill(timehien,team,namee,true);
                //    return;
                //}
                if (VienChinh.vienchinh.GetTeamByName(e.data["UpdateDra"]["team"].str) == "TeamXanh")
                {
                    if (e.data["UpdateDra"]["truhptru"])
                    {
                        VienChinh.vienchinh.TruXanh.GetComponent<TruVienChinh>().MatMau(float.Parse(e.data["UpdateDra"]["truhptru"].str), true);
                    }
                }
                else
                {
                    if (e.data["UpdateDra"]["truhptru"])
                    {
                        VienChinh.vienchinh.TruDo.GetComponent<TruVienChinh>().MatMau(float.Parse(e.data["UpdateDra"]["truhptru"].str), true);
                    }
                }
                return;
            }
            if (e.data["UpdateDra"]["listdra"])
            {
                for (int i = 0; i < e.data["UpdateDra"]["listdra"].Count; i++)
                {
                    Transform draa = GetDra(e.data["UpdateDra"]["listdra"][i].str);
                    if (draa != null)
                    {
                        if (e.data["UpdateDra"]["biencuu"])
                        {
                            VienChinh.vienchinh.SetBienCuuOnline(draa, float.Parse(e.data["UpdateDra"]["time"].str));

                            if (e.data["UpdateDra"]["lamcham"])
                            {
                                float time = float.Parse(e.data["UpdateDra"]["time"].str);
                                //  string lamcham = e.data["UpdateDra"]["lamcham"].str;
                                string effect = e.data["UpdateDra"]["effect"].str;
                                float chia = float.Parse(e.data["UpdateDra"]["chia"].str);
                                string cong = e.data["UpdateDra"]["cong"].str;
                                dataLamCham data = new dataLamCham(time, effect, chia, cong, true);
                                draa.transform.Find("SkillDra").GetComponent<DragonPVEController>().LamChamABS(data);
                            }
                        }
                    }
                }
                return;
            }
            Transform dra = GetDra(e.data["UpdateDra"]["id"].str);
            if (dra == null)
            {
                debug.Log("dra nulll id: " + e.data["UpdateDra"]["id"].str);
                return;
            }
            DragonPVEController dragonPVEController = dra.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            if (e.data["UpdateDra"]["daylui"])
            {
                //debug.Log("day luiii " + e.data["UpdateDra"]["daylui"].ToString());
                dragonPVEController.SetDayLuiOnline(float.Parse(e.data["UpdateDra"]["daylui"].str));
            }
            else if (e.data["UpdateDra"]["hp"])
            {
                //  debug.Log("tru hpppp " + e.data["UpdateDra"]["truhp"].ToString());
                dragonPVEController.SetHpOnline(e.data["UpdateDra"]);
                //dragonPVEController.hp
             
            //    dragonPVEController.SetHp(float.Parse(e.data["UpdateDra"]["setfillhp"].str));
            }
            else if (e.data["UpdateDra"]["lamcham"])
            {
                float time = float.Parse(e.data["UpdateDra"]["time"].str);
              //  string lamcham = e.data["UpdateDra"]["lamcham"].str;
                string effect = e.data["UpdateDra"]["effect"].str;
                float chia = float.Parse(e.data["UpdateDra"]["chia"].str);
                string cong = e.data["UpdateDra"]["cong"].str;
                dataLamCham data = new dataLamCham(time, effect, chia, cong, true);
                dragonPVEController.LamChamABS(data);
            }
            else if (e.data["UpdateDra"]["hutmau"])
            {
                //  debug.Log("tru hpppp " + e.data["UpdateDra"]["truhp"].ToString());

                dragonPVEController.HutMauOnline(float.Parse(e.data["UpdateDra"]["hutmau"].str));
                //    dragonPVEController.SetHp(float.Parse(e.data["UpdateDra"]["setfillhp"].str));
            }
            else if (e.data["UpdateDra"]["hieuungchu"])
            {
                //  debug.Log("tru hpppp " + e.data["UpdateDra"]["truhp"].ToString());

                PVEManager.InstantiateHieuUngChu(e.data["UpdateDra"]["hieuungchu"].str,dragonPVEController.transform,2,true);
                //    dragonPVEController.SetHp(float.Parse(e.data["UpdateDra"]["setfillhp"].str));
            }
            else if (e.data["UpdateDra"]["rongdie"])
            {
                //  debug.Log("tru hpppp " + e.data["UpdateDra"]["truhp"].ToString());

                dragonPVEController.Died();
                //    dragonPVEController.SetHp(float.Parse(e.data["UpdateDra"]["setfillhp"].str));
            }
            else if (e.data["UpdateDra"]["func"])
            {
                debug.Log("UpdateDra func " + e.data["UpdateDra"]["func"].str);
                dragonPVEController.FuncInvokeOnline(e.data["UpdateDra"]["func"].str,true);
            }
        }
        else if (e.data["KetQua"])
        {
         //   if (AllMenu.ins.menu.ContainsKey("infoitem")) AllMenu.ins.menu["infoitem"].SetActive(false);
         
            VienChinh.vienchinh.OpenPanelWinLose(e.data["KetQua"]["KetQua"].str, e.data["KetQua"]["info"].str);
            //GiaoDienPVP.ins.menuWin.SetActive(true);

            //if (e.data["KetQua"].str == "Win") GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
            //else GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;

            //GiaoDienPVP.ins.thongtin.text = e.data["info"].str;
            //GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
            //GiaoDienPVP.ins.btnSetting.SetActive(true);
            //GiaoDienPVP.ins.spriteWin.SetNativeSize();
            //Invoke("QuayVe", 10f);
        }
    }

    private Transform GetDra(string id)
    {
        //for (int i = 1; i < VienChinh.vienchinh.TeamXanh.transform.childCount; i++)
        //{
        //    if(VienChinh.vienchinh.TeamXanh.transform.GetChild(i).gameObject.name == id)
        //    {
        //        return VienChinh.vienchinh.TeamXanh.transform.GetChild(i).transform;
        //    }
        //}
        //for (int i = 1; i < VienChinh.vienchinh.TeamDo.transform.childCount; i++)
        //{
        //    if (VienChinh.vienchinh.TeamDo.transform.GetChild(i).gameObject.name == id)
        //    {
        //        return VienChinh.vienchinh.TeamDo.transform.GetChild(i).transform;
        //    }
        //}
        if(dicdra.ContainsKey(id)) return dicdra[id];
        return null;
    }
    public void ThoatRoom()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongOnline";
        datasend["method"] = "ThoatRoomDauTruong";
        NetworkManager.ins.SendServer(datasend, ok);
        void ok(JSONNode json)
        {
            CrGame.ins.AllDao.transform.Find("BGDao"+CrGame.ins.DangODao).gameObject.SetActive(true);
            Vector3 vec = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).transform.position;
            vec.z = -10;
            CrGame.ins.gameObject.transform.position = vec;
            CrGame.ins.giaodien.SetActive(true);
            AudioManager.SetSoundBg("nhacnen0");

            if (json["status"].AsString == "0")
            {
                gameObject.SetActive(false);
                coroom = false;
                btnMoi[0].SetActive(true);
                btnMoi[1].SetActive(true);
                imgAvatar[0].gameObject.SetActive(false);
                imgAvatar[1].gameObject.SetActive(false);
                AllMenu.ins.DestroyMenu("MenuDauTruongOnIine");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
                //AllMenu.ins.DestroyMenu("MenuDauTruongOnIine");
            } 
                
        }

        //StartCoroutine(Thoat());

        //IEnumerator Thoat()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ThoatRoomDauTruong/taikhoan/" + LoginFacebook.ins.id);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(true);
        //    Vector3 vec = CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).transform.position;
        //    vec.z = -10;
        //    CrGame.ins.gameObject.transform.position = vec;
        //    CrGame.ins.giaodien.SetActive(true);
        //    AudioManager.SetSoundBg("nhacnen0");

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.OnThongBao(true, "Lỗi", true);
        //        AllMenu.ins.DestroyMenu("MenuDauTruongOnIine");
        //    }
        //    else
        //    {
        //        debug.Log(www.downloadHandler.text);
        //        gameObject.SetActive(false);
        //        coroom = false;
        //        btnMoi[0].SetActive(true);
        //        btnMoi[1].SetActive(true);
        //        imgAvatar[0].gameObject.SetActive(false);
        //        imgAvatar[1].gameObject.SetActive(false);
        //        AllMenu.ins.DestroyMenu("MenuDauTruongOnIine");
        //    }
        //}
      
    }
    public void LoadData(string kq = "")
    {
        if (CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.activeSelf || kq == "")
        {
            if (coroom == false || kq == "")
            {
                //  StartCoroutine(Taoroom());

                JSONClass datasend = new JSONClass();
                datasend["class"] = "DauTruongOnline";
                datasend["method"] = "CreateRoomDauTruong";
                datasend["data"]["kq"] = kq;
                NetworkManager.ins.SendServer(datasend, ok);
                void ok(JSONNode json)
                {
                    if (json["status"].AsString == "0")
                    {
                        AllMenu.ins.DestroyallMenu("MenuDauTruongOnIine");
                        btnMoi[0].SetActive(false);
                        btnMoi[1].SetActive(true);
                        imgAvatar[0].gameObject.SetActive(true);
                        imgAvatar[1].gameObject.SetActive(false);
                        //    imgAvatar[0].sprite = CrGame.ins.FB_useerDp.sprite;
                        //   imgAvatar[0].transform.GetChild(0).GetComponent<Image>().sprite = CrGame.ins.FB_useerDp.transform.GetChild(1).GetComponent<Image>().sprite;
                        imgAvatar[0].transform.GetChild(1).GetComponent<Text>().text = CrGame.ins.FB_userName.text;
                        Friend.ins.LoadAvtFriend(LoginFacebook.ins.id, imgAvatar[0], imgAvatar[0].transform.GetChild(0).GetComponent<Image>());

                        coroom = true;
                    }
                    else CrGame.ins.OnThongBaoNhanh("Không thể kết nối!");
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh("Không thể kết nối!");
                //btnMoi[1].SetActive(false);
                //imgAvatar[1].gameObject.SetActive(true);
                //imgAvatar[1].sprite = CrGame.ins.FB_useerDp.sprite;
                //imgAvatar[1].transform.GetChild(0).GetComponent<Image>().sprite = CrGame.ins.FB_useerDp.transform.GetChild(1).GetComponent<Image>().sprite;
                //imgAvatar[1].transform.GetChild(1).GetComponent<Text>().text = SCrGame.ins.FB_userName.text;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        GameObject PanelTimPlayer = transform.Find("PanelTimPlayer").gameObject;
        PanelTimPlayer.SetActive(false);
        LoadData();
    }
    public void MoiBan()
    {
        string id = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongOnline";
        datasend["method"] = "MoiBanDanh";
        datasend["data"]["taikhoanban"] = id;
        NetworkManager.ins.SendServer(datasend, ok);
        void ok(JSONNode json)
        {
            debug.Log(json.ToString());
            CrGame.ins.OnThongBao(true, json["message"].AsString, true);
        }
        //StartCoroutine(Moi());
        //IEnumerator Moi()
        //{ 
        //    CrGame.ins.OnThongBao(true, "Đang Mời...", false);
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "MoiBanDanh/taikhoan/" + LoginFacebook.ins.id +"/taikhoanban/" + id);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.OnThongBao(true,"Lỗi", true);
        //    }
        //    else
        //    {
        //        // Show results as text
        //        debug.Log(www.downloadHandler.text);
        //        CrGame.ins.OnThongBao(true, www.downloadHandler.text, true);
        //    }
        //}
    }    
    public void OpenMenuMoi()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongOnline";
        datasend["method"] = "GetFriendOnline";
        NetworkManager.ins.SendServer(datasend, ok);
        void ok(JSONNode json)
        {
      //      debug.Log(json.ToString()) ;
            if (json["status"].AsString == "0")
            {
                MenuMoiFriend.SetActive(true);
                for (int i = 0; i < ContentMoi.transform.childCount; i++)
                {
                    Destroy(ContentMoi.transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < json["friendonline"].Count; i++)
                {
                    if (json["friendonline"][i]["idfb"] != null)
                    {
                        GameObject ofr = Instantiate(OFriend, ContentMoi.transform.position, Quaternion.identity);
                        Image imgAvatar = ofr.transform.GetChild(1).GetComponent<Image>();
                        Image imgKhung = ofr.transform.GetChild(2).GetComponent<Image>();
                        Text txtname = ofr.transform.GetChild(3).GetComponent<Text>();
                        //  Friend.ins.GetAvatarFriend(json[i]["idfb"].Value, imgAvatar);
                        txtname.text = json["friendonline"][i]["name"].Value;
                        Friend.ins.LoadAvtFriend(json["friendonline"][i]["idfb"].Value, imgAvatar, imgKhung);
                      //  imgKhung.sprite = Inventory.LoadSprite("Avatar" + json["friendonline"][i]["toc"].Value);
                        ofr.transform.SetParent(ContentMoi.transform, false);
                        ofr.SetActive(true);
                        ofr.name = json["friendonline"][i]["idfb"].Value + "+" + json["friendonline"][i]["name"].Value;
                    }
                }

            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
        }
        //StartCoroutine(GetFriendOnline());
        //IEnumerator GetFriendOnline()
        //{
        //    CrGame.ins.OnThongBao(true,"Đang tải...",false);
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetFriendOnline/taikhoan/" + LoginFacebook.ins.id);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //    }
        //    else
        //    {
        //        // Show results as text
        //        debug.Log(www.downloadHandler.text);
        //        JSONNode json = JSON.Parse(www.downloadHandler.text);
        //        for (int i = 0; i < ContentMoi.transform.childCount; i++)
        //        {
        //            Destroy(ContentMoi.transform.GetChild(i).gameObject);
        //        }
        //        for (int i = 0; i < json.Count; i++)
        //        {
        //            if(json[i]["idfb"] != null)
        //            {
        //                GameObject ofr = Instantiate(OFriend, ContentMoi.transform.position, Quaternion.identity) as GameObject;
        //                Image imgAvatar = ofr.transform.GetChild(1).GetComponent<Image>();
        //                Image imgKhung = ofr.transform.GetChild(2).GetComponent<Image>();
        //                Text txtname = ofr.transform.GetChild(3).GetComponent<Text>();
        //              //  Friend.ins.GetAvatarFriend(json[i]["idfb"].Value, imgAvatar);
        //                txtname.text = json[i]["name"].Value;
        //                imgKhung.sprite = Inventory.LoadSprite("Avatar" + json[i]["toc"].Value);
        //                ofr.transform.SetParent(ContentMoi.transform,false);
        //                ofr.SetActive(true);
        //                ofr.name = json[i]["idfb"].Value + "+" + json[i]["name"].Value;
        //            }
        //        }
        //        CrGame.ins.OnThongBao(false);
        //    }
        //}
      
    }
    // Update is called once per frame
    public void btnDau()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongOnline";
        datasend["method"] = "VaoDanhOnline";
        NetworkManager.ins.SendServer(datasend, ok);
        void ok(JSONNode json)
        {
            if (json["status"].AsString == "ok")
            {
               
            }
            else if (json["status"].AsString == "3")
            {
                transform.Find("PanelTimPlayer").gameObject.SetActive(true);
            }    
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
        }
    }    
    public void HuyGhep()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            if (VienChinh.vienchinh.enabled) yield break;
            GameObject PanelTimPlayer = transform.Find("PanelTimPlayer").gameObject;
            Button btn = PanelTimPlayer.transform.Find("btnHuyGhep").GetComponent<Button>();
            btn.interactable = false;
            yield return new WaitForSeconds(1f);
            JSONClass datasend = new JSONClass();
            datasend["class"] = "DauTruongOnline";
            datasend["method"] = "HuyGhep";
            NetworkManager.ins.SendServer(datasend, ok);
            void ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                 
                }
                else CrGame.ins.OnThongBaoNhanh(json["message"].Value);

                PanelTimPlayer.SetActive(false);
                btn.interactable = true;
            }
           
        }
     
    }
    public static void OpenMenu()
    {
        AllMenu.ins.OpenMenu("MenuDauTruongOnIine");
    }
}
