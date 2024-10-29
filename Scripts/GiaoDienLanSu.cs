using SimpleJSON;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GiaoDienLanSu : MonoBehaviour
{
    // Start is called before the first frame update
    public Text statusText;
    public Text TextXacDinhTocChienThang;
    public Sprite[] iconToc;
    public Sprite spriteChuaChon, spriteChon;
    void Start()
    {
      
     
    }

    // Update is called once per frame
    void Update()
    {
        //Transform Bxh = transform.Find("BxhDanhGiaVongToc");
        //Bxh.gameObject.SetActive(true);
        //Transform bxhDameBoss = Bxh.transform.GetChild(0);

        //UpdateThanhTienDo(bxhDameBoss.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>(), bxhDameBoss.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>(), bxhDameBoss.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().fillAmount);
    }
    public void ParseData(JSONNode json)
    {
        if (Friend.ins.QuaNha) Friend.ins.GoHome();
        debug.Log(json);
        transform.Find("txtSoLanDanhLanSu").GetComponent<Text>().text = "Bạn đã <color=magenta>đánh thắng Trùm Lân Sư "+ json["soLanDanhLanSu"].AsString + " lần</color>\nBạn có thể đánh thắng Trùm Lân Sư <color=magenta>tối đa 3 lần</color>";
        transform.Find("txtdanhthang").GetComponent<Text>().text = "\nBạn đã đánh thắng Trùm Lân Sư <color=magenta>" + json["soLanDanhLanSu"].AsString + "/3</color>";
        ScaleSpriteToScreen.ScaleSprite(transform.GetChild(0).GetComponent<SpriteRenderer>(), AllMenu.ins.GetComponent<Canvas>());
        Image imgtocwin = transform.Find("imttocwin").GetComponent<Image>();
        imgtocwin.sprite = GetIconToc(json["Tocwin"].AsString);
        imgtocwin.SetNativeSize();
        InvokeRepeating(nameof(UpdateCountdownCongKhongGian), 0, 1);
        InvokeRepeating(nameof(UpdateCountdownToc), 0, 1);
        gameObject.SetActive(true);

        for (int i = 0; i < NetworkManager.ins.loidai.objGiaoDienOff.Length; i++)
        {
            NetworkManager.ins.loidai.objGiaoDienOff[i].SetActive(false);
        }

        CrGame.ins.giaodien.SetActive(true);
        //CrGame.ins.giaodien.transform.GetChild(5).gameObject.SetActive(false);
        CrGame.ins.txtDanhVong.gameObject.SetActive(true);
        CrGame.ins.giaodien.transform.SetParent(transform);
        string[] strbuff = new string[] {"lucky","dame","hp"};
        for (int i = 0; i < strbuff.Length; i++)
        {
            if (json["buff" + strbuff[i]].AsInt > 0) transform.Find(strbuff[i]).transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        AudioManager.SetSoundBg("nhacnen1");
    }
    public void VaoMapLanSu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LanSu";
        datasend["method"] = "DanhLanSu";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                VienChinh.vienchinh.GetDoiHinh("BossLanSu", CheDoDau.LanSu);

                NetworkManager.ins.vienchinh.TruDo.SetActive(true);
                NetworkManager.ins.vienchinh.TruXanh.SetActive(true);

                TaoLanSu("LanSu" + json["NameBossLanSu"].AsString, json["dame"].AsFloat);
                float chia = json["hp"].AsFloat / 3;
                float[] hplansu = new float[] { chia, chia, chia };
                SetHpLanSu(hplansu);
                VienChinh.vienchinh.SetBGMap("BGLanSu");
                gameObject.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void Muabuff()
    {
        GameObject btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string namebuff = btn.transform.parent.name;

        EventManager.OpenThongBaoChon("Bạn muốn mua <color=yellow>buff " + namebuff.ToUpper() + "</color> với giá <color=lime>" + btn.transform.parent.transform.GetChild(1).GetComponent<Text>().text + " Kim cương</color>", delegate { Send(); }); ;
        void Send()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "LanSu";
            datasend["method"] = "MuaBuff";
            datasend["data"]["namebuff"] = namebuff;
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode json)
            {
                debug.Log(json.ToString());
                if (json["status"].AsString == "0")
                {
                    btn.GetComponent<Button>().interactable = false;
                }
                else
                {

                }
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
      
    }
    public static Action KetQua(KetQuaTranDau kq,bool quayve = false)
    {
        void kqq()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "LanSu";
            datasend["method"] = "KetQua";
            datasend["data"]["kq"] = kq.ToString();
            NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    if(quayve)
                    {
                        CrGame.ins.OpenGiaoDienLanSu();
                        return;
                    }
                    GiaoDienPVP.ins.menuWin.SetActive(true);
                    if (kq == KetQuaTranDau.Thua)
                    {
                        GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;
                        GiaoDienPVP.ins.thongtin.text = "Bạn đã bị đánh bại";
                    }
                    else
                    {
                        GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
                        GiaoDienPVP.ins.thongtin.text = "Bạn đã đánh bại Trùm Lân Sư";
                    }
                    GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
                    GiaoDienPVP.ins.btnSetting.SetActive(true);
                    GiaoDienPVP.ins.spriteWin.SetNativeSize();
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
        return kqq;
       
    }
    private void TaoLanSu(string nameBoss, float dame)
    {
        GameObject LanSu = Instantiate(GetObjectLanSu(nameBoss), transform.position, Quaternion.identity);
        LanSu.transform.SetParent(VienChinh.vienchinh.TeamDo.transform, false);
        LanSu.transform.position = VienChinh.vienchinh.TeamDo.transform.position;
        DragonPVEController dragonPVEController = LanSu.transform.Find("SkillDra").GetComponent<DragonPVEController>();
        dragonPVEController.dame = dame;
        dragonPVEController.xuyengiap = 99999;
        VienChinh.vienchinh.TruDo.GetComponent<SpriteRenderer>().enabled = false;
        TruVienChinh truVienChinh = VienChinh.vienchinh.TruDo.GetComponent<TruVienChinh>();

        truVienChinh.actionwin = delegate {
            truVienChinh.actionwin = null;
            CrGame.ins.SetPanelThangCap(true);
            CrGame.ins.txtThangCap.text = "";
            VienChinh.vienchinh.OffThangCap();
            VienChinh.vienchinh.TeamDo.transform.GetChild(1).gameObject.SetActive(false);
            GameObject eff = Instantiate(Inventory.ins.GetEffect("LanSuNo"), transform.position, Quaternion.identity);
            //  eff.transform.SetParent(transform);
            eff.transform.position = new Vector3(LanSu.transform.position.x,LanSu.transform.position.y + 3);
            eff.gameObject.SetActive(true);
            Destroy(eff,5f);
        };
    }
    public void SetHpLanSu(float[] hp)
    {
        TruVienChinh tru = VienChinh.vienchinh.TruDo.GetComponent<TruVienChinh>();
        tru.allmau = 2;
        for (int i = 0; i < tru.Hp.Length; i++)
        {
            tru.MaxHp[i] = hp[i];
            tru.Hp[i] = tru.MaxHp[i];
        }
        tru.LoadImgHp();
    }
    public GameObject GetObjectLanSu(string nameBoss)
    {
        return Inventory.LoadObjectResource("GameData/LanSu/" + nameBoss);
    }
    public void TrangBi()
    {
        GameObject tc = GameObject.FindGameObjectWithTag("trencung");
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", tc, true, transform.GetSiblingIndex() + 1);
    }
    public void ChonBxhDanhVong()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LanSu";
        datasend["method"] = "SwichBxhDanhVong";
        datasend["data"]["Toc"] = btnchon.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                Transform allbtn = btnchon.transform.parent;
                for (int i = 0; i < allbtn.transform.childCount; i++)
                {
                    allbtn.transform.GetChild(i).GetComponent<Image>().sprite = spriteChuaChon;
                }
                btnchon.GetComponent<Image>().sprite = spriteChon;

                Transform Bxh = transform.Find("BxhDanhGiaVongToc");
                Bxh.gameObject.SetActive(true);
                Transform scrollview = Bxh.transform.Find("Scroll View");
                scrollview.transform.Find("txtbxh").GetComponent<Text>().text = "Bảng Top 10 Cống Hiến Tộc" + GetStrToc(btnchon.name);
                Transform contentTopCongHien = scrollview.transform.GetChild(0).transform.GetChild(0);
                for (int i = 0; i < json["arrayTop"].Count; i++)
                {
                    GameObject obj = contentTopCongHien.transform.GetChild(i).gameObject;
                    obj.SetActive(true);
                    Friend.ins.LoadAvtFriend(json["arrayTop"][i]["idfb"].AsString, obj.transform.GetChild(0).GetComponent<Image>(), obj.transform.GetChild(1).GetComponent<Image>());
                    obj.transform.GetChild(2).GetComponent<Text>().text = json["arrayTop"][i]["Name"].AsString;
                    obj.transform.GetChild(3).GetComponent<Text>().text = json["arrayTop"][i]["DanhVong"].AsString;
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
     
    }    
    public void OpenBxhDanhGiaVongToc()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LanSu";
        datasend["method"] = "GetBXH";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                Transform Bxh = transform.Find("BxhDanhGiaVongToc");
                Bxh.gameObject.SetActive(true);
                Transform bxhDameBoss = Bxh.transform.GetChild(1);
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
                for (int i = 0; i < bxhDameBoss.transform.childCount - 1; i++)
                {
                    Transform ObjToc = bxhDameBoss.transform.GetChild(i);
                    Image imgtoc = ObjToc.GetComponent<Image>();
                    imgtoc.sprite = GetIconToc(skey[i]);
                    imgtoc.SetNativeSize();
                    Text txt = ObjToc.transform.GetChild(1).GetComponent<Text>();
                    Image imgfill = ObjToc.transform.GetChild(0).GetComponent<Image>();
                    imgfill.color = GetColorFillDameBoss(skey[i]);
                    UpdateThanhTienDo(imgfill, txt, ivalue[i] / damenhieunhat);
                    txt.text = ivalue[i].ToString();
                }
                Transform contentTopCongHien = Bxh.transform.Find("Scroll View").transform.GetChild(0).transform.GetChild(0);
                for (int i = 0; i < json["arrayTop"].Count; i++)
                {
                    GameObject obj = contentTopCongHien.transform.GetChild(i).gameObject;
                    obj.SetActive(true);
                    Friend.ins.LoadAvtFriend(json["arrayTop"][i]["idfb"].AsString, obj.transform.GetChild(0).GetComponent<Image>(), obj.transform.GetChild(1).GetComponent<Image>());
                    obj.transform.GetChild(2).GetComponent<Text>().text = json["arrayTop"][i]["Name"].AsString;
                    obj.transform.GetChild(3).GetComponent<Text>().text = json["arrayTop"][i]["DanhVong"].AsString;
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public Sprite GetIconToc(string s)
    {
        for (int i = 0; i < iconToc.Length; i++)
        {
            if (iconToc[i].name == s) return iconToc[i];
        }
        return iconToc[0];
    }    
    public static Color32 GetColorFillDameBoss(string toc)
    {
        switch(toc)
        {
            case "Sam": return new Color32(197, 0, 197, 255);
            case "Cay": return new Color32(20, 200, 0, 255);
            case "Bang": return new Color32(0, 114, 255, 255);
            case "Lua": return new Color32(255, 0, 7, 255);
            case "Dat": return new Color32(255, 126, 0, 255);
        }
        return new Color32(197, 0, 197, 255);
    }
    public static string GetStrToc(string toc)
    {
        switch (toc)
        {
            case "Sam": return " Sấm";
            case "Cay": return " Cây";
            case "Bang": return " Băng";
            case "Lua": return " Lửa";
            case "Dat": return " Đất";
        }
        return " Sấm ";
    }
   public static void UpdateThanhTienDo(Image mainImage,Text followText,float fillAmount)
    {
        // Lấy kích thước và vị trí của image chính
        if (fillAmount <= 0) fillAmount = 0.001f;
        RectTransform mainRectTransform = mainImage.GetComponent<RectTransform>();
        RectTransform followTextRectTransform = followText.GetComponent<RectTransform>();

        // Tính toán vị trí mới dựa trên fillAmount của image chính
        mainImage.fillAmount = fillAmount;
        float cong = 210 / fillAmount;

        Vector2 sizeDelta = new Vector2(mainRectTransform.sizeDelta.x + cong, mainRectTransform.sizeDelta.y);

        // Xác định hướng fill của hình ảnh
        Vector2 newPos;
        if (mainImage.fillMethod == Image.FillMethod.Horizontal)
        {
            // Nếu fill theo chiều ngang, cập nhật vị trí x
            newPos = new Vector2(sizeDelta.x * fillAmount, followTextRectTransform.anchoredPosition.y);
        }
        else if (mainImage.fillMethod == Image.FillMethod.Vertical)
        {
            // Nếu fill theo chiều dọc, cập nhật vị trí y
            newPos = new Vector2(followTextRectTransform.anchoredPosition.x, sizeDelta.y * fillAmount);
        }
        else
        {
            // Nếu không phải là fill ngang hoặc dọc, đặt theo mặc định
            newPos = new Vector2(sizeDelta.x * fillAmount, followTextRectTransform.anchoredPosition.y);
        }

        // Đặt vị trí mới cho followText
        followTextRectTransform.anchoredPosition = new Vector3(newPos.x,newPos.y);
    }

    public void VeNha()
    {
    
        AudioManager.SetSoundBg("nhacnen0");

        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        Dao.SetActive(true);
        Vector3 newvec = Dao.transform.position; newvec.z = -10;
        CrGame.ins.transform.position = newvec;
        CrGame.ins.giaodien.SetActive(true);
        CrGame.ins.giaodien.transform.SetParent(GameObject.FindGameObjectWithTag("CanvasS").transform);
        for (int i = 0; i < NetworkManager.ins.loidai.objGiaoDienOff.Length; i++)
        {
            NetworkManager.ins.loidai.objGiaoDienOff[i].SetActive(true);
        }
        CrGame.ins.txtDanhVong.gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("GiaoDienLanSu");
        //   gameObject.SetActive(false);
    }    
    void UpdateCountdownCongKhongGian()
    {
        DateTime now = DateTime.Now;
        DateTime nextMidnight = now.Date.AddDays(1); // 00:00 ngày hôm sau
        TimeSpan timeDiff;

        if (IsTuesdayOrFridayCongKhongGian(now))
        {
            timeDiff = nextMidnight - now;
            statusText.text = "Cổng không gian đang mở\nsẽ đóng lại sau: <color=yellow>" + FormatTimeSpanCongKhongGian(timeDiff) + "</color>";
        }
        else
        {
            DayOfWeek targetDay = GetNextTargetDayCongKhongGian(now.DayOfWeek);
            DateTime nextTargetDay = GetNextTargetDayDateCongKhongGian(now, targetDay);
            timeDiff = nextTargetDay - now;
            statusText.text = "Cổng không gian đang đóng\nsẽ được mở sau: <color=yellow>" + FormatTimeSpanCongKhongGian(timeDiff) + "</color>";
        }
    }

    bool IsTuesdayOrFridayCongKhongGian(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Tuesday || date.DayOfWeek == DayOfWeek.Friday;
    }

    DayOfWeek GetNextTargetDayCongKhongGian(DayOfWeek currentDay)
    {
        if (currentDay >= DayOfWeek.Saturday || currentDay <= DayOfWeek.Monday)
        {
            return DayOfWeek.Tuesday;
        }
        else
        {
            return DayOfWeek.Friday;
        }
    }

    DateTime GetNextTargetDayDateCongKhongGian(DateTime currentDate, DayOfWeek targetDay)
    {
        int daysToAdd = ((int)targetDay - (int)currentDate.DayOfWeek + 7) % 7;
        if (daysToAdd == 0) daysToAdd = 7; // Nếu targetDay là ngày hôm nay, thì chọn ngày targetDay tuần tới
        return currentDate.Date.AddDays(daysToAdd); // 00:00 của targetDay
    }

    string FormatTimeSpanCongKhongGian(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours >= 24)
        {
            int days = timeSpan.Days;
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;
            return $"{days}d:{hours:D2}h:{minutes:D2}:{seconds:D2}s";
        }
        else
        {
            int hours = (int)timeSpan.TotalHours;
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;
            return $"{hours:D2}h:{minutes:D2}m:{seconds:D2}s";
        }
    }

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
        TextXacDinhTocChienThang.text = "Tộc chiến thắng tiếp theo\nsẽ được xác định sau:\n <color=yellow>" + FormatTimeSpanToc(timeDiff) + "</color>";
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
}

