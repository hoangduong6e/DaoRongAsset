using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class DauTruongDanhVong : MonoBehaviour
{
    // Start is called before the first frame update
     public Sprite[] spriteSkill;
    public Sprite hopquachuanhan, hopquadanhan, ochuanhan, odanhan;
    private void OnEnable()
    {
        //     btnHopQua = GameObject.FindGameObjectWithTag("hopqua");
        if (CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.activeSelf || NetworkManager.ins.loidai.load)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "DauTruongThuThach";
            datasend["method"] = "GetDataDauTruongThuThach";
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].Value == "ok")
                {
                    CrGame.ins.DonDepDao();
                    AudioManager.SetSoundBg("nhacnen1");
                    if (json["data"]["data"]["skillchon"].Value == "")
                    {
                        GameObject giaodien1 = transform.GetChild(0).gameObject;
                        GameObject objRongDuocChon = giaodien1.transform.GetChild(1).transform.GetChild(1).gameObject;
                        for (var i = 0; i < objRongDuocChon.transform.childCount; i++)
                        {
                            objRongDuocChon.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["RongCoTheSuDung"][i]["sao"].Value + " sao";
                            Image imgrong = objRongDuocChon.transform.GetChild(i).GetComponent<Image>();
                            imgrong.sprite = Inventory.LoadSpriteRong(json["RongCoTheSuDung"][i]["nameobject"].Value + "2");
                            imgrong.SetNativeSize();
                            objRongDuocChon.transform.GetChild(i).name = json["RongCoTheSuDung"][i]["id"].Value;
                        }
                        GameObject objRongChon = giaodien1.transform.GetChild(0).transform.GetChild(1).gameObject;
                        for (int i = 0; i < json["DoiHinhChuanBi"].Count; i++)
                        {
                            if (json["DoiHinhChuanBi"][i]["id"].Value != "")
                            {
                                objRongChon.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["DoiHinhChuanBi"][i]["sao"].Value + " sao";
                                objRongChon.transform.GetChild(i).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["DoiHinhChuanBi"][i]["nameobject"].Value + "2");
                                objRongChon.transform.GetChild(i).name = json["DoiHinhChuanBi"][i]["id"].Value;
                                objRongChon.transform.GetChild(i).gameObject.SetActive(true);
                            }
                        }

                        giaodien1.SetActive(true);
                    }

                    else
                    {
                        LoadGiaoDien2(json["data"]["data"]);
                    }
                    transform.GetChild(4).transform.GetChild(5).GetComponent<Text>().text = json["tyle"].Value;
                    NetworkManager.ins.loidai.load = true;
                }
                else if (json["status"].Value == "thongbao")
                {
                    AudioManager.SetSoundBg("nhacnen1");
                    ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                    tbc.btnChon.onClick.RemoveAllListeners();
                    tbc.txtThongBao.text = json["thongbao"].Value;
                    tbc.btnChon.onClick.AddListener(VeNha);
                    tbc.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(VeNha);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                    VeNha();
                }
            }
        }
        else gameObject.SetActive(false);
    }
    void LoadGiaoDien2(JSONNode json)
    {
        GameObject giaodien2 = transform.GetChild(1).gameObject;
        GameObject allrongchon = giaodien2.transform.GetChild(1).gameObject;
        for (int i = 0; i < allrongchon.transform.childCount - 1; i++)
        {
            if(i < json["DoiHinh"].Count)
            {
                allrongchon.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = json["DoiHinh"][i]["sao"].Value + " sao";
                Image imgrong = allrongchon.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                imgrong.sprite = Inventory.LoadSpriteRong(json["DoiHinh"][i]["nameobject"].Value + "2");
                imgrong.SetNativeSize();
                allrongchon.transform.GetChild(i).transform.GetChild(1).name = json["DoiHinh"][i]["id"].Value;
                allrongchon.transform.GetChild(i).gameObject.SetActive(true);
            }
            else allrongchon.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < spriteSkill.Length; i++)
        {
          //  debug.LogError(json["skillchon"].Value);
           // debug.Log(spriteSkill[i].name);
            if(spriteSkill[i].name == json["skillchon"].Value)
            {
                Image imgskill = allrongchon.transform.GetChild(9).transform.GetChild(1).GetComponent<Image>();
                imgskill.sprite = spriteSkill[i];
                imgskill.SetNativeSize();
                break;
            }
        }
        GameObject alliconthua = giaodien2.transform.GetChild(4).gameObject;
        for (int i = 0; i < int.Parse(json["thualientiep"].Value); i++)
        {
            alliconthua.transform.GetChild(i).gameObject.SetActive(true);
        }

        GameObject allhopqua = giaodien2.transform.GetChild(2).gameObject;
        giaodien2.transform.GetChild(3).GetComponent<Text>().text = json["thanglientiep"].Value;
        for (int i = 0; i < int.Parse(json["thanglientiep"].Value) + 1; i++)
        {
            if(i == int.Parse(json["thanglientiep"].Value))
            {
                allhopqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = odanhan;
                allhopqua.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = hopquadanhan;
                break;
            }    
            allhopqua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = odanhan;
            allhopqua.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = hopquadanhan;
            allhopqua.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
        }
        GameObject info = transform.GetChild(4).gameObject;
        info.transform.GetChild(1).GetComponent<Text>().text = "Bạn đang ở lần chơi thứ <color=yellow>"+ json["luotdanh"].Value + "</color>";
        info.transform.GetChild(2).GetComponent<Text>().text = "Bạn đang đánh đối thủ thứ <color=yellow>" + (int.Parse(json["thanglientiep"].Value) + 1) + "</color>";
        info.transform.GetChild(4).GetComponent<Text>().text = "Bạn đã thua <color=red>" + json["thualientiep"].Value + "</color> trận";
       // GameObject.FindGameObjectWithTag("hopqua").transform.SetParent(transform);
       // debug.LogError("xong doan nay");
        info.SetActive(true);
        giaodien2.SetActive(true);
    }
    public void ChonRongDanh()
    {
        GameObject btnRong = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongThuThach";
        datasend["method"] = "ChonRongThuThach";
        datasend["data"]["idrong"] = btnRong.name;
        datasend["data"]["vitri"] = btnRong.transform.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                GameObject giaodien1 = transform.GetChild(0).gameObject;
                GameObject objRongDuocChon = giaodien1.transform.GetChild(0).transform.GetChild(1).gameObject;
                for (int i = 0; i < json["DoiHinhChuanBi"].Count; i++)
                {
                    if (json["DoiHinhChuanBi"][i]["id"].Value != "")
                    {
                        objRongDuocChon.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["DoiHinhChuanBi"][i]["sao"].Value + " sao";
                        Image imgchon = objRongDuocChon.transform.GetChild(i).GetComponent<Image>();
                        imgchon.sprite = Inventory.LoadSpriteRong(json["DoiHinhChuanBi"][i]["nameobject"].Value + "2");
                        imgchon.SetNativeSize();
                        objRongDuocChon.transform.GetChild(i).name = json["DoiHinhChuanBi"][i]["id"].Value;
                        objRongDuocChon.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                giaodien1.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = "Tổng giá thuê rồng: " + json["tonggiathue"].Value;
                giaodien1.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>().text = "Giá chơi lượt mới: " + json["giachoimoi"].Value;
                //GameObject giaodien1 = transform.GetChild(0).gameObject;
                //GameObject objRongDuocChon = giaodien1.transform.GetChild(1).transform.GetChild(1).gameObject;
                //for (var i = 0; i < objRongDuocChon.transform.childCount; i++)
                //{
                //    objRongDuocChon.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["RongCoTheSuDung"][i]["sao"].Value + " sao";
                //    objRongDuocChon.transform.GetChild(i).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["RongCoTheSuDung"][i]["nameobject"].Value + "2");
                //    objRongDuocChon.transform.GetChild(i).name = json["RongCoTheSuDung"][i]["id"].Value;
                //}
                btnRong.GetComponent<Button>().interactable = false;
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
        }

    }
    public void ChonSkill()
    {
        Image btnSkill = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongThuThach";
        datasend["method"] = "ChonSkillThuThach";
        datasend["data"]["nameskill"] = btnSkill.name;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                btnSkill.color = new Color32(255, 255, 255, 255);
                GameObject allskill = transform.GetChild(0).transform.GetChild(2).gameObject;
                debug.Log("allskill " + allskill.name);
                for (int i = 0; i < allskill.transform.childCount - 1; i++)
                {
                    if (i != btnSkill.transform.GetSiblingIndex())
                    {
                        allskill.transform.GetChild(i).GetComponent<Image>().color = new Color32(130, 130, 130, 255);
                    }
                }
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
        }

    }


    public void XacNhan()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongThuThach";
        datasend["method"] = "XemXacNhanDoiHinh";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                tbc.btnChon.onClick.RemoveAllListeners();
                tbc.txtThongBao.text = json["yeucau"].Value;
                tbc.btnChon.onClick.AddListener(XacNhann);
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
        }
      
    }
    void XacNhann()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongThuThach";
        datasend["method"] = "XacNhanDoiHinhThuthach";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                transform.GetChild(0).gameObject.SetActive(false);
                // transform.GetChild(1).gameObject.SetActive(true);
                if (AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                LoadGiaoDien2(json["data"]);
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
        }
    }
    short id;
    public void XemSkill()
    {
        GameObject nameskill = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string s = "";
        switch (nameskill.name)
        {
            case "LoiKeo":
                s = "<color=yellow>Lôi kéo</color>\nMỗi khi phe ta có 2 Rồng tử trận sẽ có 50% tỉ lệ lôi kéo\ntheo 1 Rồng bất kì bên địch";
                break;
            case "SamLuc":
                s = "<color=yellow>Sấm Lực</color>\nTăng tỉ lệ chí mạng cho toàn bộ Rồng phe ta";
                break;
            case "HoaLuc":
                s = "<color=yellow>Hỏa Lực</color>\nTăng sát thương cho toàn bộ Rồng phe ta";
                break;
            case "ThoLuc":
                s = "<color=yellow>Thổ Lực</color>\nTăng Máu cho toàn bộ Rồng phe ta";
                break;
            case "HungPhan":
                s = "<color=yellow>Hưng Phấn</color>\nMỗi khi có 2 rồng bất kì tử trận\ntoàn bộ Rồng phe ta sẽ được tăng tốc đánh trong 3s";
                break;
            case "TangTamDanh":
                s = "<color=yellow>Tăng tầm đánh</color>\nTăng tầm đánh cho toàn bộ Rồng tầm gần phe ta";
                break;
            default:
                break;
        }
        // AllMenu.ins.OpenCreateMenu("infoitem", GameObject.FindGameObjectWithTag("trencung"));
        //  AllMenu.ins.menu["infoitem"].transform.GetChild(0).GetComponent<Text>().text = s;
        // menuinfoitem ifitem = AllMenu.ins.menu["infoitem"].GetComponent<menuinfoitem>();
        // ifitem.Disnable(10f);
        CrGame.ins.OnThongBaoNhanh(s);
        id = (short)s.Length;
    }
    public void Offxemskill()
    {
        CrGame.ins.OffThongBaoNhanh(id);
       // AllMenu.ins.menu["infoitem"].SetActive(false);
    }
    public void TraRong()
    {
        GameObject btnRong = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongThuThach";
        datasend["method"] = "TraRongThuThach";
        datasend["data"]["idrong"] = btnRong.name;
        datasend["data"]["vitri"] = btnRong.transform.GetSiblingIndex().ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                GameObject giaodien1 = transform.GetChild(0).gameObject;
                GameObject objRongDuocChon = giaodien1.transform.GetChild(0).transform.GetChild(1).gameObject;
                for (int i = 0; i < json["DoiHinhChuanBi"].Count; i++)
                {
                    if (json["DoiHinhChuanBi"][i]["id"].Value != "")
                    {
                        objRongDuocChon.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["DoiHinhChuanBi"][i]["sao"].Value + " sao";
                        objRongDuocChon.transform.GetChild(i).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["DoiHinhChuanBi"][i]["nameobject"].Value + "2");
                        objRongDuocChon.transform.GetChild(i).name = json["DoiHinhChuanBi"][i]["id"].Value;
                        objRongDuocChon.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        objRongDuocChon.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                giaodien1.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>().text = "Tổng giá thuê rồng: " + json["tonggiathue"].Value;
                // giaodien1.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>().text = "Giá chơi lượt mới: " + json["tonggiathue"].Value;
                //GameObject giaodien1 = transform.GetChild(0).gameObject;
                //GameObject objRongDuocChon = giaodien1.transform.GetChild(1).transform.GetChild(1).gameObject;
                //for (var i = 0; i < objRongDuocChon.transform.childCount; i++)
                //{
                //    objRongDuocChon.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["RongCoTheSuDung"][i]["sao"].Value + " sao";
                //    objRongDuocChon.transform.GetChild(i).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json["RongCoTheSuDung"][i]["nameobject"].Value + "2");
                //    objRongDuocChon.transform.GetChild(i).name = json["RongCoTheSuDung"][i]["id"].Value;
                //}
                GameObject objRongChon = giaodien1.transform.GetChild(1).transform.GetChild(1).gameObject;
                for (int i = 0; i < objRongChon.transform.childCount; i++)
                {
                    if (objRongChon.transform.GetChild(i).name == btnRong.name)
                    {
                        objRongChon.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        break;
                    }
                }
                // btnRong.interactable = false;
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
        }
    }

    public void BatDau()
    {
        GameObject timplayer = transform.GetChild(2).gameObject;
        timplayer.SetActive(true);
        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongThuThach";
        datasend["method"] = "BatDauDauTruongThuThach";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    // CrGame.ins.panelLoadDao.SetActive(false);
                    //     timplayer.SetActive(true);
                    Text txttendoithu = timplayer.transform.GetChild(2).GetComponent<Text>();
                    //  btnHopQua.transform.SetParent(CrGame.ins.net.loidai.GiaoDien.transform);
                    for (int i = 0; i < 5; i++)
                    {
                        txttendoithu.text = "";
                        if (i < 5 - json["DoiHinhDoiThu"].Count)
                        {
                            for (int j = 0; j < Random.Range(4, 10); j++)
                            {
                                yield return new WaitForSeconds(Random.Range(0.06f, 0.2f));
                                txttendoithu.text += ".";
                            }
                        }
                        else
                        {
                            int y = 5 - i - 1;
                            foreach (char c in json["DoiHinhDoiThu"][y]["tenhienthi"].Value)
                            {
                                txttendoithu.text += c;
                                yield return new WaitForSeconds(Random.Range(0.06f, 0.2f));
                            }
                            //if(y == 3)
                            //{
                            //    net.vienchinh.HienIconSkill(200, "Do", "icon" + json["DoiHinhDoiThu"][y]["data"]["skillchon"].Value + "Do") ;
                            //}    
                        }
                    }
                    yield return new WaitForSeconds(0.2f);
                    timplayer.SetActive(false);
                    transform.GetChild(3).gameObject.SetActive(true);
                    yield return new WaitForSeconds(1.1f);
                    NetworkManager.ins.socket.Emit("DanhDauTruongThuThach", JSONObject.CreateStringObject(""));
                    // gameObject.SetActive(false);
                    //  CrGame.ins.menulogin.SetActive(true);
                    VienChinh.vienchinh.enabled = true;
                    AllMenu.ins.DestroyMenu("MenuDauTruongThuThach");
                }
           
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
                VeNha();
            }
        }
    }

    public void VeNha()
    {

        CrGame.ins.giaodien.SetActive(true);
        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        Dao.SetActive(true);
        Vector3 newvec = Dao.transform.position; newvec.z = -10;
        CrGame.ins.transform.position = newvec;
        AudioManager.SetSoundBg("nhacnen0");
        //  btnHopQua.transform.SetParent(CrGame.ins.net.loidai.GiaoDien.transform);
        NetworkManager.ins.loidai.load = false;
        AllMenu.ins.DestroyMenu("MenuDauTruongThuThach");
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
