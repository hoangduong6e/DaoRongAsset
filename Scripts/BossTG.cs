using SimpleJSON;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BossTG : MonoBehaviour
{
    public GameObject RongUsers, menuChat; public Sprite trangthaivang, trangthaido;
    public Image imgHpBoss;public float timedemnguoc = 0;public Text txttimedemnguoc; public bool timerIsRunning = false;
    int trang = 1, trangg = 1; float top,topcuoi;
    public Sprite Top1, Top2, Top3, Top;public bool gettop = true;bool sangtrang = false;
  //  private GameObject nguoichoi;
    // Start is called before the first frame update
    private void Start()
    {
        NetworkManager.ins.socket.On("usersOut", UsersOut);
        NetworkManager.ins.socket.On("usersOn", UsersOn);
        NetworkManager.ins.socket.On("usersChat", UsersChat);
        NetworkManager.ins.socket.On("UpdateHpBoss", UpdateHpBoss);
        NetworkManager.ins.socket.On("BossDie", BossDie);
    //    GameObject nguoichoi = transform.GetChild(0).gameObject;

    }
    void UsersOut(SocketIOEvent e)
    {
      //  debug.Log(e.data);
        DeleteUsers(NetworkManager.ins.CatDauNgoacKep(e.data["id"].ToString()));
    }
    void UsersOn(SocketIOEvent e)
    {
       // debug.Log(e.data);
        if(NetworkManager.ins.vienchinh.dangdau == false) CrGame.ins.CreateUserTopBoss(2, NetworkManager.ins.CatDauNgoacKep(e.data["tenhienthi"].ToString()), NetworkManager.ins.CatDauNgoacKep(e.data["id"].ToString()), NetworkManager.ins.CatDauNgoacKep(e.data["toc"].ToString()), NetworkManager.ins.CatDauNgoacKep(e.data["chientuong"]["name"].ToString()), float.Parse(NetworkManager.ins.CatDauNgoacKep(e.data["x"].ToString())),float.Parse(NetworkManager.ins.CatDauNgoacKep(e.data["y"].ToString())));
        // DeleteUsers(NetworkManager.ins.CatDauNgoacKep(e.data["id"].ToString()));
    }
    void UsersChat(SocketIOEvent e)
    {
//debug.Log(e.data);
        if (NetworkManager.ins.vienchinh.dangdau == false) AddChat(NetworkManager.ins.CatDauNgoacKep(e.data["message"].ToString()));
    }
    void UpdateHpBoss(SocketIOEvent e)
    {
      //  debug.Log(e.data);
        if(NetworkManager.ins.vienchinh.chedodau == CheDoDau.BossTG)
        {
            float hp = float.Parse(e.data.GetField("hp").str);
            float fillamount = (float)hp / (float)float.Parse(NetworkManager.ins.CatDauNgoacKep(e.data.GetField("maxhp").str));
            if (NetworkManager.ins.vienchinh.dangdau == false)
            {
                //debug.Log("updatehp ngoai");
                imgHpBoss.fillAmount = fillamount;
            }
            else
            {

                DragonPVEController cs = NetworkManager.ins.vienchinh.TeamDo.transform.GetChild(1).transform.Find("SkillDra").GetComponent<DragonPVEController>();
                cs.ImgHp.fillAmount = fillamount;
                cs.hp = hp;
              //  cs.Maxhp
            }
        }    
    
    }
    void BossDie(SocketIOEvent e)
    {
        //debug.Log(e.data);
        if (NetworkManager.ins.vienchinh.dangdau)
        {
           if(VienChinh.vienchinh.chedodau == CheDoDau.BossTG) NetworkManager.ins.vienchinh.Thua();
        }
        else
        {
            transform.GetChild(2).transform.GetChild(2).GetComponent<Text>().text = NetworkManager.ins.CatDauNgoacKep(e.data["nguoitieudietboss"].ToString());
        } 
    }
    public void Updatehp(float hp,float maxhp)
    {
        float fillamount = (float)hp / (float)maxhp;
        imgHpBoss.fillAmount = fillamount;
    }    
    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timedemnguoc > 0)
            {
                timedemnguoc -= Time.deltaTime;
                DisplayTime(timedemnguoc);
            }
            else
            {
                debug.Log("Time has run out!");
                timedemnguoc = 0;
          //      txttimedemnguoc.transform.parent.transform.GetChild(2).GetComponent<Button>().interactable = true;
                timerIsRunning = false;
                // gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
                //    gameObject.SetActive(false);
            }
        }
    }
    public void GetTop()
    {
        if(gettop)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "BossTheGioi";
            datasend["method"] = "GetTopBossTG";
            datasend["data"]["top"] = (trang - 1).ToString();
            datasend["data"]["trang"] = "0";
            NetworkManager.ins.SendServer(datasend.ToString(), ok);
            void ok(JSONNode json)
            {
                GameObject objecttop = transform.GetChild(7).gameObject;
                GameObject imgtop = transform.GetChild(8).gameObject;
                for (int i = 0; i < json["alltop"].Count; i++)
                {
                    GameObject top = Instantiate(objecttop, transform.position, Quaternion.identity);

                    Image imgAvatar = top.transform.GetChild(0).GetComponent<Image>();
                    Image imgKhungAvatar = top.transform.GetChild(1).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].Value;
                    //  Image HuyHieu = top.transform.GetChild(3).GetComponent<Image>();
                    Text txtName = top.transform.GetChild(2).GetComponent<Text>();
                    //  Text txtTop = top.transform.GetChild(5).GetComponent<Text>();
                    //    txtTop.text = json[i]["Top"].Value;
                    Friend.ins.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar, imgKhungAvatar);
                    // NetworkManager.ins.friend.GetAvatarFriend(json["alltop"][i]["idfb"].Value, imgAvatar);
                    //    imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                    //int sotop = int.Parse(json[i]["Top"].Value);
                    //if (sotop > 3) HuyHieu.sprite = Top;
                    //else if (sotop == 1) HuyHieu.sprite = Top1;
                    //else if (sotop == 2) HuyHieu.sprite = Top2;
                    //else if (sotop == 3) HuyHieu.sprite = Top3;
                    //HuyHieu.SetNativeSize();
                    txtName.text = json["alltop"][i]["Name"].Value;
                    top.transform.SetParent(imgtop.transform, false);
                    // CrGame.ins.OnThongBao(false);
                    //AllTop.SetActive(true);
                    // txtTrang.text = trang + "/100";
                    top.SetActive(true);
                    if (i == 9) break;
                }
                gettop = false;
            }
        //    StartCoroutine(Get());
        //    IEnumerator Get()
        //    {
        //        //  CrGame.ins.OnThongBao(true, "Đang tải...", false);
        //        UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetTopBossTG/taikhoan/" + LoginFacebook.ins.id + "/top/" + (trang - 1) + "/trang/0");
        //        www.downloadHandler = new DownloadHandlerBuffer();
        //        yield return www.SendWebRequest();

        //        if (www.result != UnityWebRequest.Result.Success)
        //        {
        //            debug.Log(www.error);
        //        }
        //        else
        //        {
        //            // Show results as text 
        //            debug.Log(www.downloadHandler.text);
        //            JSONNode json = JSON.Parse(www.downloadHandler.text);
        //            GameObject objecttop = transform.GetChild(7).gameObject;
        //            GameObject imgtop = transform.GetChild(8).gameObject;
        //            for (int i = 0; i < json["alltop"].Count; i++)
        //            {
        //                GameObject top = Instantiate(objecttop, transform.position, Quaternion.identity);

        //                Image imgAvatar = top.transform.GetChild(0).GetComponent<Image>();
        //                Image imgKhungAvatar = top.transform.GetChild(1).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].Value;
        //                //  Image HuyHieu = top.transform.GetChild(3).GetComponent<Image>();
        //                Text txtName = top.transform.GetChild(2).GetComponent<Text>();
        //                //  Text txtTop = top.transform.GetChild(5).GetComponent<Text>();
        //                //    txtTop.text = json[i]["Top"].Value;
        //                Friend.ins.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar, imgKhungAvatar);
        //                // NetworkManager.ins.friend.GetAvatarFriend(json["alltop"][i]["idfb"].Value, imgAvatar);
        //                //    imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
        //                //int sotop = int.Parse(json[i]["Top"].Value);
        //                //if (sotop > 3) HuyHieu.sprite = Top;
        //                //else if (sotop == 1) HuyHieu.sprite = Top1;
        //                //else if (sotop == 2) HuyHieu.sprite = Top2;
        //                //else if (sotop == 3) HuyHieu.sprite = Top3;
        //                //HuyHieu.SetNativeSize();
        //                txtName.text = json["alltop"][i]["Name"].Value;
        //                top.transform.SetParent(imgtop.transform, false);
        //                // CrGame.ins.OnThongBao(false);
        //                //AllTop.SetActive(true);
        //                // txtTrang.text = trang + "/100";
        //                top.SetActive(true);
        //                if (i == 9) break;
        //            }
        //            gettop = false;
        //        }
        //    }
        }    
    }
    private void OnDestroy()
    {
       // NetworkManager.ins.socket.Off("UpdateHpBoss", UpdateHpBoss);
        CrGame.ins.giaodien.transform.GetChild(5).gameObject.SetActive(true);
        for (int i = 0; i < NetworkManager.ins.loidai.objGiaoDienOff.Length; i++)
        {
            NetworkManager.ins.loidai.objGiaoDienOff[i].SetActive(true);
        }
        CrGame.ins.txtDanhVong.gameObject.SetActive(false);
        CrGame.ins.GetComponent<ZoomCamera>().enabled = true;
        //NetworkManager.ins.loidai.GiaoDien.transform.SetParent(NetworkManager.ins.loidai.CanvasS.transform);
        //NetworkManager.ins.loidai.GiaoDien.transform.SetSiblingIndex(2);
    }
    public void OpenMenuTop()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "BossTheGioi";
        datasend["method"] = "GetTopBossTG";
        datasend["data"]["top"] = top.ToString();
        datasend["data"]["trang"] = trangg.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), ok);
        void ok(JSONNode json)
        {
          //  JSONNode json = JSON.Parse(www.downloadHandler.text);
            //GameObject objecttop = transform.GetChild(5).transform.GetChild(6).gameObject;
            if (json["alltop"].Count > 0)
            {
                GameObject menutop = transform.GetChild(10).gameObject;

                GameObject contentop = menutop.transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).gameObject;
                if (trangg % 2 == 1)
                {
                    topcuoi = float.Parse(json["alltop"][0]["tongsatthuong"].Value);
                }
                for (int i = 1; i <= 6; i++)
                {
                    contentop.transform.GetChild(i).gameObject.SetActive(false);
                    if (i <= json["alltop"].Count)
                    {
                        debug.Log(json["alltop"][i - 1]["Name"].Value);
                        Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                        Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i - 1]["idobject"].Value;
                        debug.Log("ok1");
                        Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                        Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                        debug.Log("ok2");
                        Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                        int sotop = i + trang - 1;
                        txtTop.text = sotop.ToString();
                        debug.Log("ok3");
                        NetworkManager.ins.friend.LoadAvtFriend(json["alltop"][i - 1]["idfb"].Value, imgAvatar, imgKhungAvatar);
                        //NetworkManager.ins.friend.GetAvatarFriend(json["alltop"][i - 1]["idfb"].Value, imgAvatar);
                        //imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i - 1]["Toc"].Value);
                        contentop.transform.GetChild(i).gameObject.SetActive(true);
                        debug.Log("ok4");
                        if (sotop > 3) HuyHieu.sprite = Top;
                        else if (sotop == 1) HuyHieu.sprite = Top1;
                        else if (sotop == 2) HuyHieu.sprite = Top2;
                        else if (sotop == 3) HuyHieu.sprite = Top3;
                        HuyHieu.SetNativeSize();
                        debug.Log("ok5");
                        txtName.text = json["alltop"][i - 1]["Name"].Value;
                        Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i - 1]["tongsatthuong"].Value;
                        debug.Log("ok5.1");
                        if (i == 6)
                        {
                            top = float.Parse(json["alltop"][i - 1]["tongsatthuong"].Value);
                            sangtrang = true;
                        }
                        else if (i == json["alltop"].Count) topcuoi = float.Parse(json["alltop"][i - 1]["tongsatthuong"].Value);
                        debug.Log("ok5.2");
                    }


                    //  contentop.transform.GetChild(i).transform.SetParent(imgtop.transform, false);
                    // CrGame.ins.OnThongBao(false);
                    //AllTop.SetActive(true);S
                    // txtTrang.text = trang + "/100";
                }
                debug.Log("ok6");
                debug.Log("Dung si tien phong" + json["dungsitienphong"]["idfb"].Value);
                if (json["dungsitienphong"]["idfb"].Value != "")
                {
                    Image imgAvatar = contentop.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
                    Image imgKhungAvatar = contentop.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["dungsitienphong"]["idobject"].Value;
                    debug.Log("ok7");
                    Text txtName = contentop.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
                    Text txttime = contentop.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>(); txttime.text = json["dungsitienphong"]["tongsatthuong"].Value;
                    debug.Log("ok8");
                    // NetworkManager.ins.friend.GetAvatarFriend(json["dungsitienphong"]["idfb"].Value, imgAvatar);
                    //imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["dungsitienphong"]["Toc"].Value);
                    NetworkManager.ins.friend.LoadAvtFriend(json["dungsitienphong"]["idfb"].Value, imgAvatar, imgKhungAvatar);
                    debug.Log("ok9");
                    txtName.text = json["dungsitienphong"]["Name"].Value;
                    contentop.transform.GetChild(0).gameObject.SetActive(true);
                    debug.Log("ok10");
                }
                menutop.SetActive(true);
                //   trang += 6;
            }
            else CrGame.ins.OnThongBaoNhanh("Chưa có xếp hạng");
        }

        //CrGame.ins.panelLoadDao.SetActive(true);
        //StartCoroutine(delay());
        //IEnumerator delay()
        //{
        //    //  CrGame.ins.OnThongBao(true, "Đang tải...", false);
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetTopBossTG/taikhoan/" + LoginFacebook.ins.id + "/top/" + top + "/trang/"+ trangg);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //        CrGame.ins.OnThongBaoNhanh("Lỗi");
        //    }
        //    else
        //    {
        //        // Show results as text 
        //        sangtrang = false;
        //        debug.Log(www.downloadHandler.text);
        //        JSONNode json = JSON.Parse(www.downloadHandler.text);
        //        //GameObject objecttop = transform.GetChild(5).transform.GetChild(6).gameObject;
        //        if (json["alltop"].Count > 0)
        //        {
        //            GameObject menutop = transform.GetChild(10).gameObject;

        //            GameObject contentop = menutop.transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).gameObject;
        //            if (trangg % 2 == 1)
        //            {
        //                topcuoi = float.Parse(json["alltop"][0]["tongsatthuong"].Value);
        //            }
        //            for (int i = 1; i <= 6; i++)
        //            {
        //                contentop.transform.GetChild(i).gameObject.SetActive(false);
        //                if (i <= json["alltop"].Count)
        //                {
        //                    debug.Log(json["alltop"][i - 1]["Name"].Value);
        //                    Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
        //                    Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i - 1]["idobject"].Value;
        //                    debug.Log("ok1");
        //                    Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
        //                    Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
        //                    debug.Log("ok2");
        //                    Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
        //                    int sotop = i + trang - 1;
        //                    txtTop.text = sotop.ToString();
        //                    debug.Log("ok3");
        //                    NetworkManager.ins.friend.LoadAvtFriend(json["alltop"][i - 1]["idfb"].Value,imgAvatar,imgKhungAvatar);
        //                    //NetworkManager.ins.friend.GetAvatarFriend(json["alltop"][i - 1]["idfb"].Value, imgAvatar);
        //                    //imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i - 1]["Toc"].Value);
        //                    contentop.transform.GetChild(i).gameObject.SetActive(true);
        //                    debug.Log("ok4");
        //                    if (sotop > 3) HuyHieu.sprite = Top;
        //                    else if (sotop == 1) HuyHieu.sprite = Top1;
        //                    else if (sotop == 2) HuyHieu.sprite = Top2;
        //                    else if (sotop == 3) HuyHieu.sprite = Top3;
        //                    HuyHieu.SetNativeSize();
        //                    debug.Log("ok5");
        //                    txtName.text = json["alltop"][i - 1]["Name"].Value;
        //                    Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i - 1]["tongsatthuong"].Value;
        //                    debug.Log("ok5.1");
        //                    if (i == 6)
        //                    {
        //                        top = float.Parse(json["alltop"][i - 1]["tongsatthuong"].Value);
        //                        sangtrang = true;
        //                    }
        //                    else if (i == json["alltop"].Count) topcuoi = float.Parse(json["alltop"][i - 1]["tongsatthuong"].Value);
        //                    debug.Log("ok5.2");
        //                }


        //                //  contentop.transform.GetChild(i).transform.SetParent(imgtop.transform, false);
        //                // CrGame.ins.OnThongBao(false);
        //                //AllTop.SetActive(true);S
        //                // txtTrang.text = trang + "/100";
        //            }
        //            debug.Log("ok6");
        //            debug.Log("Dung si tien phong" + json["dungsitienphong"]["idfb"].Value);
        //            if (json["dungsitienphong"]["idfb"].Value != "")
        //            {
        //                Image imgAvatar = contentop.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        //                Image imgKhungAvatar = contentop.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["dungsitienphong"]["idobject"].Value;
        //                debug.Log("ok7");
        //                Text txtName = contentop.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
        //                Text txttime = contentop.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>(); txttime.text = json["dungsitienphong"]["tongsatthuong"].Value;
        //                debug.Log("ok8");
        //                // NetworkManager.ins.friend.GetAvatarFriend(json["dungsitienphong"]["idfb"].Value, imgAvatar);
        //                //imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["dungsitienphong"]["Toc"].Value);
        //                NetworkManager.ins.friend.LoadAvtFriend(json["dungsitienphong"]["idfb"].Value, imgAvatar, imgKhungAvatar);
        //                debug.Log("ok9");
        //                txtName.text = json["dungsitienphong"]["Name"].Value;
        //                contentop.transform.GetChild(0).gameObject.SetActive(true);
        //                debug.Log("ok10");
        //            }
        //            menutop.SetActive(true);
        //            //   trang += 6;
        //        }
        //        else CrGame.ins.OnThongBaoNhanh("Chưa có xếp hạng");
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //    }
        //}
    }
    public void exitMenuTop()
    {
        top = 0;
        trang = 1; trangg = 1;
        transform.GetChild(10).gameObject.SetActive(false);
    }
    public void sangtrangtop(int i)
    {
        if (top < topcuoi && sangtrang)
        {
            if (i > 0) trangg += 1;
            else return;
            //   else trangg -= 1;
            if (trang + i >= 0)
            {
                if (i < 0)
                {
                    trang -= 12;
                    top = topcuoi;
                }
                else trang += 6;
                OpenMenuTop();
            }
            //   debug.Log(i);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        int gio = 0;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        while (minutes >= 60)
        {
            minutes -= 60;
            gio += 1;
        }
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        txttimedemnguoc.text = gio + ":" + minutes + ":" + seconds;
        // txttimedemnguoc.text = string.Format("{0:00}:{0:00}:{1:00}",gio, minutes, seconds);
    }
    public void DanhBoss()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "BossTheGioi";
        datasend["method"] = "DanhBossTG";
        NetworkManager.ins.SendServer(datasend.ToString(), ok);

        void ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
               // debug.LogError("bossok");

                VienChinh.vienchinh.GetDoiHinh("BossTG", CheDoDau.BossTG);
                NetworkManager.ins.vienchinh.TruDo.SetActive(true);
                NetworkManager.ins.vienchinh.TruXanh.SetActive(true);
                //   CrGame.ins.allmenu.DestroyMenu("MenuXacNhan");
                //   NetworkManager.ins.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGTinhVan");
                //gameObject.SetActive(false);
                //  Destroy(gameObject);

                if (json["buffdame"].Value != "0")
                {
                    NetworkManager.ins.vienchinh.HienIconSkill(300, "Xanh", "iconBuffDame");
                }
                if (json["buffxuyengiap"].Value != "0")
                {
                    NetworkManager.ins.vienchinh.HienIconSkill(300, "Xanh", "iconBuffXuyenGiap");
                }
                AllMenu.ins.DestroyMenu("MenuBossTG");
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
    }
    public void TrangBi()
    {
        GameObject tc = GameObject.FindGameObjectWithTag("trencung");
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", tc, true, tc.transform.GetSiblingIndex() - 1);
    }
    public void Thoat()
    {
        NetworkManager.ins.socket.Off("usersOut", UsersOut);
        NetworkManager.ins.socket.Off("usersOn", UsersOn);
        NetworkManager.ins.socket.Off("usersChat", UsersChat);
        NetworkManager.ins.socket.Off("UpdateHpBoss", UpdateHpBoss);
        NetworkManager.ins.socket.Off("BossDie", BossDie);
        gameObject.SetActive(false);
        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        Dao.gameObject.SetActive(true);
        Vector3 vec = Dao.transform.position;
        vec.z = -10;
        CrGame.ins.gameObject.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        NetworkManager.ins.vienchinh.TruDo.SetActive(true);
        NetworkManager.ins.vienchinh.TruXanh.SetActive(true);
      //  NetworkManager.ins.socket.Off("usersOut", UsersOut);
        NetworkManager.ins.socket.Emit("outbosstg");
        AllMenu.ins.DestroyMenu("MenuBossTG");
     
    }    
    public void DeleteUsers(string id)
    {
        GameObject allusers = transform.GetChild(0).gameObject;
        for (int i = 1; i < allusers.transform.childCount; i++)
        {
            if(allusers.transform.GetChild(i).gameObject.name == id)
            {
                Destroy(allusers.transform.GetChild(i).gameObject);
               // break;
            }
        }
    }
    public void VaoKhu()
    {
        Button btnChon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        JSONClass datasend = new JSONClass();
        datasend["class"] = "BossTheGioi";
        datasend["method"] = "VaoKhuVuc";
        datasend["data"]["khu"] = (btnChon.transform.GetSiblingIndex() - 1).ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), ok);
        void ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                CrGame.ins.loadbosstg(json);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
            //  GameObject menukhu = transform.GetChild(17).gameObject;
            //  menukhu.SetActive(false);
            ThoatXemKhu();
        }
        //CrGame.ins.panelLoadDao.SetActive(true);
        //StartCoroutine(delay());
        //IEnumerator delay()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "VaoKhuVuc/taikhoan/" + LoginFacebook.ins.id + "/khu/"+ (btnChon.transform.GetSiblingIndex() - 1));
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.OnThongBaoNhanh("Lỗi");
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //    }
        //    else
        //    {
        //        // Show results as text
        //        //   btndoi.interactable = false;
             
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //    }
        //}
    }    
    public void XemKhu()
    {
        GameObject menukhu = transform.GetChild(17).gameObject;
        if (menukhu.activeSelf == false)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "BossTheGioi";
            datasend["method"] = "XemKhuVuc";
            NetworkManager.ins.SendServer(datasend.ToString(), ok);

            void ok(JSONNode json)
            {
                if (json["status"].Value == "ok")
                {

                    GameObject contentKhu = menukhu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject objectChonKhu = contentKhu.transform.GetChild(0).gameObject;

                    for (int i = 0; i < json["allkhu"].Count; i++)
                    {
                        GameObject objkhu = Instantiate(objectChonKhu, transform.position, Quaternion.identity);
                        objkhu.transform.SetParent(contentKhu.transform, false);

                        Text txtnamekhu = objkhu.transform.GetChild(0).GetComponent<Text>();
                        Image imgTrangThai = objkhu.transform.GetChild(1).GetComponent<Image>();

                        txtnamekhu.text = json["allkhu"][i]["namekhu"].Value;
                        if (json["allkhu"][i]["trangthaikhu"].Value == "vang") imgTrangThai.sprite = trangthaivang;
                        else if (json["allkhu"][i]["trangthaikhu"].Value == "do") imgTrangThai.sprite = trangthaido;
                        objkhu.SetActive(true);

                        if (json["allkhu"][i]["dangoday"].Value != "")
                        {
                            objkhu.GetComponent<Button>().interactable = false;
                        }
                    }
                    menukhu.SetActive(true);
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
           // CrGame.ins.panelLoadDao.SetActive(true);
            //StartCoroutine(Load());
            //IEnumerator Load()
            //{
            //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemKhuVuc/taikhoan/" + LoginFacebook.ins.id);
            //    www.downloadHandler = new DownloadHandlerBuffer();
            //    yield return www.SendWebRequest();

            //    if (www.result != UnityWebRequest.Result.Success)
            //    {
            //        debug.Log(www.error);
            //        CrGame.ins.panelLoadDao.SetActive(false);
            //        CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
            //    }
            //    else
            //    {
            //        JSONNode json = JSON.Parse(www.downloadHandler.text);
                   
            //        debug.Log(www.downloadHandler.text);
            //        CrGame.ins.panelLoadDao.SetActive(false);
            //    }
            //}
        }    
     
    }
    public void ThoatXemKhu()
    {
        GameObject menukhu = transform.GetChild(17).gameObject;
        GameObject contentKhu = menukhu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;

        for (int i = 1; i < contentKhu.transform.childCount; i++)
        {
            Destroy(contentKhu.transform.GetChild(i).gameObject);
        }
        menukhu.SetActive(false);
    }    
    public void AddChat(string txt)
    {
        GameObject contentChat = menuChat.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject objChat = contentChat.transform.GetChild(0).gameObject;

        GameObject chat = Instantiate(objChat,transform.position,Quaternion.identity);

        chat.transform.SetParent(contentChat.transform,false);
        chat.GetComponent<TextMeshProUGUI>().text = txt;
        chat.SetActive(true);
    }    

    public void Chat()
    {
        if (menuChat.activeSelf)
        {
            TMP_InputField txt = menuChat.transform.GetChild(3).GetComponent<TMP_InputField>();
            txt.text.TrimStart();
            txt.text.TrimEnd();
            if (txt.text != "")
            {
                //  CrGame.ins.panelLoadDao.SetActive(true);
                Button btnchat = transform.GetChild(14).GetComponent<Button>();
                btnchat.interactable = false;

                JSONClass datasend = new JSONClass();
                datasend["class"] = "BossTheGioi";
                datasend["method"] = "ChatbossTG";
                datasend["data"]["noidung"] = txt.text;
                NetworkManager.ins.SendServer(datasend.ToString(), ok);
                void ok(JSONNode json)
                {
                    if (json["status"].Value == "ok")
                    {
                        //debug.Log(www.downloadHandler.text);
                        //AddChat(json["message"].Value);
                        txt.text = "";
                    }
                    // txt.text = "";
                    btnchat.interactable = true;
                }
                //StartCoroutine(Load());
                //IEnumerator Load()
                //{
                //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ChatbossTG/taikhoan/" + LoginFacebook.ins.id + "/noidung/" + txt.text);
                //    www.downloadHandler = new DownloadHandlerBuffer();
                //    yield return www.SendWebRequest();

                //    if (www.result != UnityWebRequest.Result.Success)
                //    {
                //        debug.Log(www.error);
                //        //   CrGame.ins.panelLoadDao.SetActive(false);
                //        CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                //        btnchat.interactable = true;
                //    }
                //    else
                //    {
                //        JSONNode json = JSON.Parse(www.downloadHandler.text);
                //        if (json["status"].Value == "ok")
                //        {
                //            //debug.Log(www.downloadHandler.text);
                //            //AddChat(json["message"].Value);
                //            txt.text = "";
                //        }
                //       // txt.text = "";
                //        btnchat.interactable = true;
                //    }
                //}
            }
            else if(txt.text == "") menuChat.SetActive(false);
        }
        else menuChat.SetActive(true);
    }
    public void Buffchiso(string namechiso)
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = "BossTheGioi";
        datasend["method"] = "XemBuffchisoboss";
        datasend["data"]["chiso"] = namechiso;
        NetworkManager.ins.SendServer(datasend.ToString(), ok);
        void ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                tbc.txtThongBao.text = json["info"].Value;
                tbc.btnChon.onClick.AddListener(() => MuaBuff(namechiso));
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
            }
        }
        //CrGame.ins.panelLoadDao.SetActive(true);
        //StartCoroutine(delay());
        //IEnumerator delay()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemBuffchisoboss/taikhoan/" + LoginFacebook.ins.id + "/chiso/" + namechiso);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.OnThongBaoNhanh("Lỗi");
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //    }
        //    else
        //    {
        //        // Show results as text
        //        //   btndoi.interactable = false;
        //        debug.Log(www.downloadHandler.text);
        //        JSONNode json = JSON.Parse(www.downloadHandler.text);
        //        if (json["status"].Value == "ok")
        //        {
        //            ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        //            tbc.btnChon.onClick.RemoveAllListeners();
        //            tbc.txtThongBao.text = json["info"].Value;
        //            tbc.btnChon.onClick.AddListener(() => MuaBuff(namechiso));
        //        }
        //        else
        //        {
        //            CrGame.ins.OnThongBaoNhanh(json["status"].Value);
        //        }
        //        //  GameObject menukhu = transform.GetChild(17).gameObject;
        //        //  menukhu.SetActive(false);
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //    }
        //}
    }    
    void MuaBuff(string name)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "BossTheGioi";
        datasend["method"] = "MuaBuffchisoboss";
        datasend["data"]["chiso"] = name;
        NetworkManager.ins.SendServer(datasend.ToString(), ok);
        void ok(JSONNode json)
        {
            CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            //  GameObject menukhu = transform.GetChild(17).gameObject;
            //  menukhu.SetActive(false);
            AllMenu.ins.DestroyMenu("MenuXacNhan");
            //AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            CrGame.ins.panelLoadDao.SetActive(false);
        }
        //CrGame.ins.panelLoadDao.SetActive(true);
        //StartCoroutine(delay());
        //IEnumerator delay()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "MuaBuffchisoboss/taikhoan/" + LoginFacebook.ins.id + "/chiso/" + name);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.OnThongBaoNhanh("Lỗi");
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //    }
        //    else
        //    {
        //        // Show results as text
        //        //   btndoi.interactable = false;
        //        debug.Log(www.downloadHandler.text);
        //        JSONNode json = JSON.Parse(www.downloadHandler.text);
        //        CrGame.ins.OnThongBaoNhanh(json["status"].Value,2);
        //        //  GameObject menukhu = transform.GetChild(17).gameObject;
        //        //  menukhu.SetActive(false);
        //        AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //    }
        //}
    }    
}
