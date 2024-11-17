using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BXHDanhVong : MonoBehaviour
{
    int trang = 1, trangg = 1; float top, topcuoi;
    public Sprite Top1, Top2, Top3, Top; bool sangtrang = false;
    public Text txttime; public bool timerIsRunning = false;
    public float timedemnguoc = 0;

    public Sprite spriteChuaChon, spriteChon;
    public Text txtlammoimua,txtphatthuongdanhvong;
    private void OnEnable()
    {
        OpenMenuTop();
        InvokeRepeating(nameof(UpdateCountdownDanhVong), 0f, 1f); // Cập nhật mỗi giây
    }
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
               // txttime.transform.parent.transform.GetChild(2).GetComponent<Button>().interactable = true;
                timerIsRunning = false;
                // gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
                //    gameObject.SetActive(false);
            }
        }
        UpdateCountdownToc();
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
       //  int ngay = 0;
        int gio = 0;
       
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        while (minutes >= 60)
        {
            minutes -= 60;
            gio += 1;
        }
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        txttime.text = gio + ":" + minutes + ":" + seconds + "s";
        // txttimedemnguoc.text = string.Format("{0:00}:{0:00}:{1:00}",gio, minutes, seconds);
    }
    void OpenMenuTop()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "GetBXHDanhVong";
        datasend["data"]["top"] = top.ToString();
        datasend["data"]["trang"] = trang.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            sangtrang = false;
            if (json["alltop"].Count > 0)
            {
                GameObject contentop = transform.GetChild(1).transform.GetChild(4).transform.GetChild(1).gameObject;
                if (trangg % 2 == 1) topcuoi = float.Parse(json["alltop"][0]["DanhVong"].Value);
                for (int i = 0; i < 6; i++)
                {
                    contentop.transform.GetChild(i).gameObject.SetActive(false);
                    if (i < json["alltop"].Count)
                    {
                        //    debug.Log(json["alltop"][i]["Name"].Value);
                        Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                        Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idfb"].Value;
                        //   debug.Log("ok1");
                        Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                        Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                        //   debug.Log("ok2");
                        Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                        int sotop = i + 1 + trang - 1;
                        txtTop.text = sotop.ToString();
                        //  debug.Log("ok3");
                        //  net.friend.GetAvatarFriend(json["alltop"][i]["idfb"].Value, imgAvatar);
                        // imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                        Friend.ins.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar, imgKhungAvatar);
                        imgKhungAvatar.name = json["alltop"][i]["idfb"].AsString;
                        contentop.transform.GetChild(i).gameObject.SetActive(true);
                        //    debug.Log("ok4");
                        if (sotop > 3) HuyHieu.sprite = Top;
                        else if (sotop == 1) HuyHieu.sprite = Top1;
                        else if (sotop == 2) HuyHieu.sprite = Top2;
                        else if (sotop == 3) HuyHieu.sprite = Top3;
                        HuyHieu.SetNativeSize();
                        //   debug.Log("ok5");
                        txtName.text = json["alltop"][i]["Name"].Value;
                        Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["DanhVong"].Value;
                        //   debug.Log("ok5.1");
                        if (i == 5)
                        {
                            sangtrang = true;
                            top = float.Parse(json["alltop"][i]["DanhVong"].Value);
                        }
                        else if (i == json["alltop"].Count - 1) topcuoi = float.Parse(json["alltop"][i]["DanhVong"].Value);
                        //   debug.Log("ok5.2");
                    }
                    //  contentop.transform.GetChild(i).transform.SetParent(imgtop.transform, false);
                    // CrGame.ins.OnThongBao(false);
                    //AllTop.SetActive(true);S
                    // txtTrang.text = trang + "/100";
                }
                timedemnguoc = float.Parse(json["time"].Value);
                if (timedemnguoc > 0) timerIsRunning = true;
                transform.GetChild(1).gameObject.SetActive(true);
                //   trang += 6;
            }
            else
            {
                AllMenu.ins.DestroyMenu("MenuTopDanhVong");
                CrGame.ins.OnThongBaoNhanh("Chưa có xếp hạng");
            }
        }
    }
    public void XemThongTin()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        //net.friend.idObjectFriend = objitem.name;
        //net.friend.QuaNhaFriend(objitem.GetComponent<Image>().sprite);
        //net.menuLoiDai.SetActive(false);
        //gameObject.SetActive(false);
        MenuTTNguoichoi menutt = AllMenu.ins.GetCreateMenu("GiaoDienThongTin", gameObject.transform.parent.gameObject, false, transform.GetSiblingIndex() + 1).GetComponent<MenuTTNguoichoi>();
        menutt.transform.SetParent(gameObject.transform.parent, false);
        menutt.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        menutt.ID = btnchon.name;
        menutt.gameObject.SetActive(true);
    }
    public void exitMenuTop()
    {
        //top = 0;
        //trang = 1; trangg = 1;
        AllMenu.ins.DestroyMenu("MenuTopDanhVong");
    }
    public void sangtrangtop(int i)
    {
        debug.Log("top " + top + "top cuoi " + topcuoi);
        if (top < topcuoi & sangtrang)
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



    int tranglevel = 1;
    public Sprite Top1level, Top2level, Top3level, Top0level;
    private string tocchon = "Sam";

    public void SwichBxh()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        tocchon = btnchon.name;

        Transform allbtn = btnchon.transform.parent;
        for (int i = 0; i < allbtn.transform.childCount; i++)
        {
            allbtn.transform.GetChild(i).GetComponent<Image>().sprite = spriteChuaChon;
        }
        tranglevel = 1;
        btnchon.GetComponent<Image>().sprite = spriteChon;
        XemBXH();
    }
    public void sangtrangtoplevel(int i)
    {
        AudioManager.PlaySound("soundClick");
        if (tranglevel + i >= 1) tranglevel += i;
        else return;
        XemBXH();
    }
    public void XemBXH()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LanSu";
        datasend["method"] = "SwichBxhLevel";
        datasend["data"]["trang"] = tranglevel.ToString();
        datasend["data"]["toc"] = tocchon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
              //  GameObject menu = AllMenu.ins.GetCreateMenu("BxhLevel", CrGame.ins.trencung.gameObject);
                //GameObject menu = CrGame.ins.trencung.transform.Find("MenuTop").gameObject;
                //    menuevent["MenuTop"] = menu;
                // GameObject menu = GetCreateMenu("MenuTop",gameObject,false,3);
                GameObject menutop = transform.GetChild(1).transform.Find("MenuTopLevel").gameObject;
                transform.GetChild(0).gameObject.SetActive(true);
                if (json["alltop"].Count > 0)
                {
                    GameObject contentop = menutop.transform.GetChild(0).gameObject;
                    for (int i = 0; i < 10; i++)
                    {
                        contentop.transform.GetChild(i).gameObject.SetActive(false);
                        if (i < json["alltop"].Count)
                        {
                            // debug.Log(json["alltop"][i]["Name"].Value);
                            Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].AsString;
                            //   debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                            //   debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = json["alltop"][i]["top"].AsInt;
                            txtTop.text = sotop.ToString();
                            //   debug.Log("ok3");
                            NetworkManager.ins.friend.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar, imgKhungAvatar);
                            // imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                            contentop.transform.GetChild(i).gameObject.SetActive(true);
                            debug.Log("ok4");
                            if (sotop > 3) HuyHieu.sprite = Top0level;
                            else if (sotop == 1) HuyHieu.sprite = Top1level;
                            else if (sotop == 2) HuyHieu.sprite = Top2level;
                            else if (sotop == 3) HuyHieu.sprite = Top3level;
                            HuyHieu.SetNativeSize();
                            //  debug.Log("ok5");
                            txtName.text = json["alltop"][i]["Name"].Value;
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text = json["alltop"][i]["level"].AsString;
                            // debug.Log("ok5.1");

                            contentop.transform.GetChild(i).gameObject.SetActive(true);

                            //   debug.Log("ok5.2");
                        }

                        //  contentop.transform.GetChild(i).transform.SetParent(imgtop.transform, false);
                        // CrGame.ins.OnThongBao(false);
                        //AllTop.SetActive(true);S
                        // txtTrang.text = trang + "/100";
                    }
                }
                else CrGame.ins.OnThongBaoNhanh("Chưa có xếp hạng");

                string nametoc = tocchon;
                string toc = GiaoDienLanSu.GetStrToc(nametoc);
                Text txtTenBxh = menutop.transform.Find("txtTenBxh").GetComponent<Text>();
                txtTenBxh.text = "Bảng Xếp Hạng Level Tộc" + toc;
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
        }
    }
    public void OpenBxhBoss()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LanSu";
        datasend["method"] = "GetBXHBossTGTheoToc";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                Transform Bxh = transform.GetChild(1).transform.Find("MenuTopBoss");
                Bxh.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Tộc vô địch mùa trước:" + GiaoDienLanSu.GetStrToc(json["NameBossLanSu"].AsString);
                Transform bxhDameBoss = Bxh.transform.GetChild(2);
                Bxh.gameObject.SetActive(true);
                float damenhieunhat = 0;
                string[] skey = new string[5];
                float[] ivalue = new float[5];
                int ii = 0;
                foreach (KeyValuePair<string, JSONNode> key in json["TopDameBossTheoToc"].AsObject)
                {
                    float dame = key.Value.AsFloat;
                    if (dame >= damenhieunhat) damenhieunhat = dame;
                    skey[ii] = key.Key;
                    ivalue[ii] = key.Value.AsFloat;
                    ii++;
                    if (ii >= 5) break;
                }

                int n = ivalue.Length;

                // Sắp xếp chọn thủ công
                for (int i = 0; i < n - 1; i++)
                {
                    // Tìm phần tử lớn nhất trong mảng còn lại
                    int maxIndex = i;
                    for (int j = i + 1; j < n; j++)
                    {
                        if (ivalue[j] > ivalue[maxIndex])
                        {
                            maxIndex = j;
                        }
                    }

                    // Hoán đổi phần tử lớn nhất với phần tử đầu tiên của mảng chưa được sắp xếp
                    float temp = ivalue[maxIndex];
                    ivalue[maxIndex] = ivalue[i];
                    ivalue[i] = temp;

                    string temp2 = skey[maxIndex];
                    skey[maxIndex] = skey[i];
                    skey[i] = temp2;
                }
                //   debug.Log(string.Join(", ", skey) + " / " + string.Join(", ", ivalue));
                for (int i = 0; i < bxhDameBoss.transform.childCount; i++)
                {
                    Transform ObjToc = bxhDameBoss.transform.GetChild(i);
                    Image imgtoc = ObjToc.GetComponent<Image>();
                    imgtoc.sprite = GetIconToc(skey[i]);
                    imgtoc.SetNativeSize();
                    Text txt = ObjToc.transform.GetChild(1).GetComponent<Text>();
                    Image imgfill = ObjToc.transform.GetChild(0).GetComponent<Image>();
                    imgfill.color = GiaoDienLanSu.GetColorFillDameBoss(skey[i]);
                    GiaoDienLanSu.UpdateThanhTienDo(imgfill, txt, ivalue[i] / damenhieunhat);
                    txt.text = ivalue[i].ToString();
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private Sprite GetIconToc(string s)
    {
        for (int i = 0; i < iconToc.Length; i++)
        {
            if (iconToc[i].name == s) return iconToc[i];
        }
        return iconToc[0];
    }
    public Sprite[] iconToc;


    void UpdateCountdownToc()
    {
        DateTime now = DateTime.Now;
        DateTime targetDateTime;

        if (now.DayOfWeek == DayOfWeek.Monday || now.DayOfWeek == DayOfWeek.Friday || now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
        {
            // Nếu hôm nay là Thứ Hai, Thứ Sáu, Thứ Bảy hoặc Chủ Nhật, đếm ngược đến 00:00 Thứ Ba
            targetDateTime = GetNextTargetDayDateToc(now, DayOfWeek.Tuesday);
        }
        else
        {
            // Ngược lại, đếm ngược đến 00:00 Thứ Sáu
            targetDateTime = GetNextTargetDayDateToc(now, DayOfWeek.Friday);
        }

        TimeSpan timeDiff = targetDateTime - now;
        txtlammoimua.text = "Làm mới mùa sau: <color=yellow>" + FormatTimeSpanToc(timeDiff) + "</color>";
    }

    DateTime GetNextTargetDayDateToc(DateTime currentDate, DayOfWeek targetDay)
    {
        int daysToAdd = ((int)targetDay - (int)currentDate.DayOfWeek + 7) % 7;
        if (daysToAdd == 0) daysToAdd = 7; // Nếu targetDay là ngày hôm nay, thì chọn ngày targetDay tuần tới
        return currentDate.Date.AddDays(daysToAdd); // 00:00 của targetDay
    }

    string FormatTimeSpanToc(TimeSpan timeSpan)
    {
        return $"{timeSpan.Days}d :{timeSpan.Hours}h :{timeSpan.Minutes}m :{timeSpan.Seconds}s";
    }

    void UpdateCountdownDanhVong()
    {
        DateTime now = DateTime.Now;
        DateTime nextSaturday9PM = GetNextSaturday9PMDanhvong(now);

        TimeSpan timeRemaining = nextSaturday9PM - now;

        if (timeRemaining.TotalSeconds > 0)
        {
            txtphatthuongdanhvong.text = string.Format(
               "{0} ngày, {1} giờ, {2} phút, {3} giây",
               timeRemaining.Days,
               timeRemaining.Hours,
               timeRemaining.Minutes,
               timeRemaining.Seconds
           );
        }
        else
        {
            txtphatthuongdanhvong.text = "00:00:00:00";
        }
    }

    DateTime GetNextSaturday9PMDanhvong(DateTime from)
    {
        DateTime nextSaturday = from.AddDays((DayOfWeek.Saturday - from.DayOfWeek + 7) % 7);
        DateTime nextSaturday9PM = new DateTime(nextSaturday.Year, nextSaturday.Month, nextSaturday.Day, 21, 0, 0);

        if (nextSaturday9PM < from)
        {
            nextSaturday9PM = nextSaturday9PM.AddDays(7);
        }

        return nextSaturday9PM;
    }
}
