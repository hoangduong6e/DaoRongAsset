using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuTTNguoichoi : MonoBehaviour
{
    // Start is called before the first frame update
    public string ID = "";
    public Scrollbar barkhungavt;
    public Scrollbar baravt;
    bool loadthem = true;
    int loadtrang = 1;public GameObject txtsudung, giaoDienKhoanhKhac;
    public Image imgavt;string avtchon,tenhienthi;
    public Sprite imgWin, imgLose;
    public GameObject objtrandau, objkinang;
    private void OnEnable()
    {
        //load
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "GetTTNguoiChoi";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    GameObject GiaoDienThongTin = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject ObjAvatar = GiaoDienThongTin.transform.GetChild(1).gameObject;
                    GameObject ObjInfo = GiaoDienThongTin.transform.GetChild(2).gameObject;
                    GameObject ObjGhiChu = GiaoDienThongTin.transform.GetChild(3).gameObject;

                    InputField inputghichu = ObjGhiChu.transform.GetChild(1).GetComponent<InputField>();
                    Image imgkhung = imgavt.transform.GetChild(0).GetComponent<Image>();
                    if (ID != LoginFacebook.ins.id)
                    {
                        ObjAvatar.transform.GetChild(0).gameObject.SetActive(true);
                        ObjAvatar.transform.GetChild(1).gameObject.SetActive(false);
                        Friend.ins.LoadAvtFriend(ID, imgavt, imgavt.transform.GetChild(0).GetComponent<Image>());
                        inputghichu.enabled = false;
                        // imgavt.sprite = CrGame.ins.friend.imgAvatarFriend.sprite;
                        // imgavt.transform.GetChild(0).GetComponent<Image>().sprite
                    }
                    else
                    {
                        imgavt.sprite = CrGame.ins.FB_useerDp.sprite;

                        if (CrGame.ins.khungAvatar.GetComponent<Animator>())
                        {
                            imgavt.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = CrGame.ins.khungAvatar.GetComponent<Animator>().runtimeAnimatorController;
                            //  yield return new WaitForSeconds(0.6f);

                        }
                        else
                        {
                            imgkhung.sprite = CrGame.ins.FB_useerDp.transform.GetChild(1).GetComponent<Image>().sprite;
                        }

                        //Friend.ins.LoadImage("khungavt", NetworkManager.ins.CatDauNgoacKep(jsonavt[i].Value), imgavt);
                    }

                    Text txtten = ObjInfo.transform.GetChild(0).GetComponent<Text>();
                    Text txtguild = ObjInfo.transform.GetChild(1).GetComponent<Text>();
                    Text txtdanhhieu = ObjInfo.transform.GetChild(2).GetComponent<Text>();
                    Text txttoc = ObjInfo.transform.GetChild(3).GetComponent<Text>();
                    Text txtcapdo = ObjInfo.transform.GetChild(4).GetComponent<Text>();
                    Text txtdao = ObjInfo.transform.GetChild(5).GetComponent<Text>();
                    Text txtrong = ObjInfo.transform.GetChild(6).GetComponent<Text>();

                    txtten.text = "Tên: <color=yellow>" + json["info"]["tenhienthi"].Value + " </color>";
                    txtguild.text = "Guild: " + json["info"]["guild"].Value;
                    txtdanhhieu.text = "Danh hiệu: " + json["info"]["danhhieu"].Value;
                    txttoc.text = "Tộc: " + json["info"]["toc"].Value;
                    txtcapdo.text = "Cấp độ: " + json["info"]["capdo"].Value;
                    txtdao.text = "Đảo: " + json["info"]["dao"].Value;
                    txtrong.text = "Số lượng rồng: " + json["info"]["soluongrong"].Value;
                    tenhienthi = json["info"]["tenhienthi"].Value;

                    inputghichu.text = json["info"]["ghichu"].Value;
                    CrGame.ins.panelLoadDao.SetActive(false);
                    yield return new WaitForSeconds(0.3f);
                    imgkhung.SetNativeSize();
                }
             
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
        //CrGame.ins.panelLoadDao.SetActive(true);
        //StartCoroutine(Load());
        //IEnumerator Load()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetTTNguoiChoi/id/" + ID);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //        CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
        //        gameObject.SetActive(false);
        //    //    CrGame.ins.allmenu.DestroyMenu("MenuEventTet2023");
        //    }
        //    else
        //    {
        //        // Show results as text
        //        debug.Log(www.downloadHandler.text);

        //        JSONNode json = JSON.Parse(www.downloadHandler.text);
               
            
        //        //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
        //    }
        //}
    }
    public void SaveGhiChu()
    {
        GameObject GiaoDienThongTin = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject ObjGhiChu = GiaoDienThongTin.transform.GetChild(3).gameObject;
        InputField inputghichu = ObjGhiChu.transform.GetChild(1).GetComponent<InputField>();
        if(inputghichu.text.Length > 70)
        {
            CrGame.ins.OnThongBaoNhanh("Độ dài ghi chú dưới 70 kí tự");
            return;
        }
        debug.Log("save ghi chu");
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "SaveGhiChu/id/" + LoginFacebook.ins.id + "/ghichu/" + inputghichu.text);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                //    CrGame.ins.allmenu.DestroyMenu("MenuEventTet2023");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }    
    public void KetBan()
    {
        CrGame.ins.OnThongBao(true, "Đang gửi yêu cầu...", false);
        NetworkManager.ins.socket.Emit("AddFriend", JSONObject.CreateStringObject(tenhienthi));
    }
    public void BaiSu()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        if(ID != LoginFacebook.ins.id) StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ChonTruyenNhan/taikhoanchon/" + tenhienthi + "/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi (0)", 1f);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json != null)
                {
                    CrGame.ins.OnThongBao(true, "Đã bái sư " + tenhienthi, true);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(www.downloadHandler.text, 1f);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("GiaoDienThongTin");
    }
    public void ChonAvt()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject GiaoDien = transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject GiaoDienDoiAvt = GiaoDien.transform.GetChild(1).gameObject;
        Text txtinfo = GiaoDienDoiAvt.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        avtchon = btnchon.transform.parent.name;
        Image imgkhung = imgavt.transform.GetChild(0).GetComponent<Image>();
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "LoadChonAvt/id/" + ID + "/avtchon/" + avtchon + "/loai/" + GiaoDienDoiAvt.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                //CrGame.ins.allmenu.DestroyMenu("MenuEventTet2023");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);

                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    if (GiaoDienDoiAvt.name == "avt")
                    {
                        imgavt.sprite = btnchon.GetComponent<Image>().sprite;
                    }
                    else
                    {
                      


                        if (btnchon.GetComponent<Animator>())
                        {
                            imgkhung.GetComponent<Animator>().runtimeAnimatorController = btnchon.GetComponent<Animator>().runtimeAnimatorController;
                            //  yield return new WaitForSeconds(0.6f);
                        }
                        else
                        {
                            imgkhung.GetComponent<Animator>().runtimeAnimatorController = null;
                            imgkhung.sprite = btnchon.GetComponent<Image>().sprite;
                            imgkhung.SetNativeSize();
                        }
                    }
       
                    txtsudung.transform.SetParent(btnchon.transform);
                    txtsudung.transform.position = btnchon.transform.position;
                    txtsudung.gameObject.SetActive(true);
                    txtinfo.text = json["info"].Value;
                    CrGame.ins.panelLoadDao.SetActive(false);
                    yield return new WaitForSeconds(0.3f);
                    imgkhung.SetNativeSize();
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }

           
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }

       
    }    
    public void CuonKhungAvt()
    {
       // debug.Log(barkhungavt.size);
        if (barkhungavt.value < 0 && loadthem) 
        {
            loadthem = false;
            debug.Log("Load theem diii");
        }
    }
    public void LoadAvt(string loai)
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "LoadAvt/id/" + ID+"/loai/"+loai);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                //CrGame.ins.allmenu.DestroyMenu("MenuEventTet2023");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                giaoDienKhoanhKhac.SetActive(false);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject GiaoDien = transform.GetChild(0).transform.GetChild(0).gameObject;
                    GiaoDien.transform.GetChild(0).gameObject.SetActive(false);
                    GameObject GiaoDienDoiAvt = GiaoDien.transform.GetChild(1).gameObject;
                    GiaoDienDoiAvt.SetActive(true);
                    GameObject Content = GiaoDienDoiAvt.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject ObjAvt = Content.transform.GetChild(0).gameObject;
                    GiaoDienDoiAvt.name = loai;
                    if (loai == "avt")
                    {
                        JSONNode jsonavt = json["data"]["allavt"];
                        for (int i = 0; i < jsonavt.Count; i++)
                        {
                            GameObject AvtObj = Instantiate(ObjAvt, transform.position, Quaternion.identity);
                            AvtObj.transform.SetParent(Content.transform, false);
                            Image imgavt = AvtObj.transform.GetChild(0).GetComponent<Image>();
                            AvtObj.transform.GetChild(1).gameObject.SetActive(true);
                            if (NetworkManager.ins.CatDauNgoacKep(jsonavt[i].Value) == json["data"]["avtsudung"].Value)
                            {
                                txtsudung.transform.SetParent(AvtObj.transform);
                                txtsudung.gameObject.SetActive(true);
                                txtsudung.transform.position = AvtObj.transform.position;
                            }
                            Friend.ins.LoadImage(loai, NetworkManager.ins.CatDauNgoacKep(jsonavt[i].Value), imgavt);
                            AvtObj.name = jsonavt[i].Value;
                            AvtObj.SetActive(true);
                        }
                     
                    }    
                    else if(loai == "khungavt")
                    {
                        JSONNode jsonavt = json["data"]["allkhungavt"];
                        for (int i = 0; i < jsonavt.Count; i++)
                        {
                            GameObject AvtObj = Instantiate(ObjAvt, transform.position, Quaternion.identity);
                            AvtObj.transform.SetParent(Content.transform, false);
                            Image imgavt = AvtObj.transform.GetChild(0).GetComponent<Image>();
                            if (NetworkManager.ins.CatDauNgoacKep(jsonavt[i].Value) == json["data"]["khungavtsudung"].Value)
                            {
                                //AvtObj.transform.GetChild(2).gameObject.SetActive(true);
                                txtsudung.transform.SetParent(AvtObj.transform);
                                txtsudung.gameObject.SetActive(true);
                                txtsudung.transform.position = AvtObj.transform.position;
                            }
                            Friend.ins.LoadImage(loai, NetworkManager.ins.CatDauNgoacKep(jsonavt[i].Value), imgavt);
                            AvtObj.name = jsonavt[i].Value;
                            AvtObj.SetActive(true);
                        }
                    }    
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }    
    //public void LoadImage(string namefolder,string namefile, Image img,GameObject obj)
    //{
    //    StartCoroutine(DownloadImage());
    //    IEnumerator DownloadImage()
    //    {
    //        UnityWebRequest request = UnityWebRequestTexture.GetTexture("http://" + LoginFacebook.ins.ServerChinh + "/LoadImage/namefolder/" + namefolder + "/name/" + namefile);
    //        yield return request.SendWebRequest();
    //        if (request.isNetworkError || request.isHttpError)
    //            debug.Log(request.error);
    //        else
    //        {
    //            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
    //            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
    //            img.sprite = sprite;
    //            if (namefolder == "khungavt") img.SetNativeSize();
    //            //obj.SetActive(true);
    //        }
    //    }
    //}
    public void Huy()
    {
        imgavt.sprite = CrGame.ins.FB_useerDp.sprite;
        GameObject GiaoDien = transform.GetChild(0).transform.GetChild(0).gameObject;
        GiaoDien.transform.GetChild(0).gameObject.SetActive(true);
        txtsudung.transform.SetParent(GiaoDien.transform.GetChild(0).transform);
        txtsudung.gameObject.SetActive(false);
        GameObject GiaoDienDoiAvt = GiaoDien.transform.GetChild(1).gameObject;
       
        GiaoDienDoiAvt.SetActive(false);
        GameObject Content = GiaoDienDoiAvt.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 1; i < Content.transform.childCount; i++)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }
    }    
    public void XacNhan()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject GiaoDien = transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject GiaoDienDoiAvt = GiaoDien.transform.GetChild(1).gameObject;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ChonAvt/id/" + ID + "/avtchon/" + avtchon + "/loai/" + GiaoDienDoiAvt.name);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                //CrGame.ins.allmenu.DestroyMenu("MenuEventTet2023");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);

                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    if (GiaoDienDoiAvt.name == "avt")
                    {
                        CrGame.ins.FB_useerDp.sprite = imgavt.sprite;
                    }
                    else
                    {
                        //  CrGame.ins.FB_useerDp.transform.GetChild(1).GetComponent<Image>().sprite = imgavt.transform.GetChild(0).GetComponent<Image>().sprite;
                        // CrGame.ins.FB_useerDp.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                        Friend.ins.LoadImage("khungavt", json["name"].AsString,CrGame.ins.khungAvatar);
                    }
                    Huy();

                    CrGame.ins.OnThongBaoNhanh("Đã đổi!");
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }    
    public void GetAllKhoanhKhac()
    {
        AudioManager.SoundClick();
        GameObject content = giaoDienKhoanhKhac.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;

        GameObject GiaoDienThongTin = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        //   GameObject ObjAvatar = GiaoDienThongTin.transform.GetChild(1).gameObject;
        GameObject ObjInfo = GiaoDienThongTin.transform.GetChild(2).gameObject;
        GameObject ObjGhiChu = GiaoDienThongTin.transform.GetChild(3).gameObject;

        if (giaoDienKhoanhKhac.activeSelf) return;
        if(content.transform.childCount > 0)
        {
            ObjInfo.gameObject.SetActive(false);
            ObjGhiChu.gameObject.SetActive(false);
            giaoDienKhoanhKhac.gameObject.SetActive(true);
            return;
        }
        JSONClass datasend = new JSONClass();
        datasend["class"] = "ReplayData";
        datasend["method"] = "GetKhoanhKhacTranDau";
        datasend["data"]["idxem"] = ID;
        NetworkManager.ins.SendServer(datasend.ToString(), ok);
        void ok(JSONNode json)
        {
      
        //    debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
             
                ObjInfo.gameObject.SetActive(false);
                ObjGhiChu.gameObject.SetActive(false);

                JSONNode alltrandau = json["data"]["alltrandau"];
           
                GameObject Obj = objtrandau;

                for (int i = alltrandau.Count - 1; i >= 0; i--)
                {
                    JSONNode trandau = alltrandau[i];
                    GameObject ins = Instantiate(Obj, transform.position, Quaternion.identity);
                    ins.transform.SetParent(content.transform,false);
                    ins.name = trandau["idtrandau"].AsString;
                    ins.transform.GetChild(0).name = trandau["serverData"].AsString;
                 //   ins.transform.GetChild(1).name = trandau["player"].AsString;
                    GameObject player1 = ins.transform.Find("Player1").gameObject;
                    player1.transform.GetChild(2).GetComponent<Text>().text = trandau["player1"]["name"].AsString;
                    player1.transform.GetChild(3).GetComponent<Image>().sprite = GetSpriteThangThua(trandau["player1"]["thangthua"].AsString);
                    player1.transform.GetChild(0).GetComponent<Image>().sprite = imgavt.sprite;
                    if(imgavt.transform.GetChild(0).GetComponent<Animator>())
                    {
                        if (imgavt.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController != null)
                        {
                            Animator anim = player1.transform.GetChild(1).GetComponent<Animator>();
                            anim.runtimeAnimatorController = imgavt.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController;
                            GamIns.SetNativeSizeAnimator(anim);
                        }
                        else player1.transform.GetChild(1).GetComponent<Image>().sprite = imgavt.transform.GetChild(0).GetComponent<Image>().sprite;
                    }    
                 
                    else player1.transform.GetChild(1).GetComponent<Image>().sprite = imgavt.transform.GetChild(0).GetComponent<Image>().sprite;


                    GameObject player2 = ins.transform.Find("Player2").gameObject;

                    player2.transform.GetChild(2).GetComponent<Text>().text = trandau["player2"]["name"].AsString;
                    player2.transform.GetChild(3).GetComponent<Image>().sprite = GetSpriteThangThua(trandau["player2"]["thangthua"].AsString);
                    Friend.ins.LoadAvtFriend(trandau["player2"]["id"].AsString, player2.transform.GetChild(0).GetComponent<Image>(), player2.transform.GetChild(1).GetComponent<Image>());


                    player1.name = "Player" + trandau["player1"]["player"].AsString;
                    player2.name = "Player" + trandau["player2"]["player"].AsString;

                    ins.transform.Find("txttime").GetComponent<Text>().text = trandau["timeTranDau"].AsString;

                    ins.transform.Find("txtLike").GetComponent<Text>().text = trandau["like"].AsString;
                    ins.SetActive(true);

                    GameObject alltrangbi1 = player1.transform.Find("allTrangBiDoiHinh").gameObject;

                    for (int j = 0; j < trandau["player1"]["skillsudung"].Count; j++)
                    {
                        if(trandau["player1"]["skillsudung"][j].AsString != "" && trandau["player1"]["skillsudung"][j].AsString != "Khoa")
                        {
                            GameObject kinang = Instantiate(objkinang, transform.position, Quaternion.identity);
                            kinang.transform.SetParent(alltrangbi1.transform, false);
                            kinang.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(trandau["player1"]["skillsudung"][j].AsString);
                            kinang.SetActive(true);
                        }
                    
                    }

                    GameObject alltrangbi2 = player2.transform.Find("allTrangBiDoiHinh").gameObject;

                    for (int j = 0; j < trandau["player2"]["skillsudung"].Count; j++)
                    {
                        if (trandau["player2"]["skillsudung"][j].AsString != "" && trandau["player2"]["skillsudung"][j].AsString != "Khoa")
                        {
                            GameObject kinang = Instantiate(objkinang, transform.position, Quaternion.identity);
                            kinang.transform.SetParent(alltrangbi2.transform, false);
                            kinang.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(trandau["player2"]["skillsudung"][j].AsString);
                            kinang.SetActive(true);
                        }
                
                    }
                }

                giaoDienKhoanhKhac.SetActive(true);
            }
            else if (json["status"].AsString == "2")
            {
                if(ID == LoginFacebook.ins.id)
                {
                    CrGame.ins.OnThongBaoNhanh("Bạn chưa chia sẻ khoảnh khắc nào!");
                }
                else CrGame.ins.OnThongBaoNhanh(ObjInfo.transform.GetChild(0).GetComponent<Text>().text.Split(":")[1] + "chưa chia sẻ khoảnh khắc nào!");

            }    
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }

        }
    

    }
    public void XemLaiTranDau()
    {

    }
    public void LikeTranDau()
    {

    }
    private Sprite GetSpriteThangThua(string thangthua)
    {
        if (thangthua == "Thang")
        {
            return imgWin;
        }
        return imgLose;
    }

    public void StartReplay()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        string idtrandau = btnchon.name;
        string linkdata = btnchon.transform.GetChild(0).name;

        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true, transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();


        GameObject player1 = btnchon.transform.Find("Player1").gameObject;
        string nameplayer1 = player1.transform.GetChild(2).GetComponent<Text>().text;
        Image avtplayer1 = player1.transform.GetChild(0).GetComponent<Image>();
        GameObject khungavt1 = player1.transform.GetChild(1).gameObject;

        GameObject player2 = btnchon.transform.Find("Player2").gameObject;
        string nameplayer2 = player2.transform.GetChild(2).GetComponent<Text>().text;
        Image avtplayer2 = player2.transform.GetChild(0).GetComponent<Image>();
        GameObject khungavt2 = player2.transform.GetChild(1).gameObject;


        tbc.txtThongBao.text = "Bạn muốn xem lại trận đấu <color=lime>" + nameplayer1 + "</color> <color=red>VS</color> <color=lime>" + nameplayer2 + "</color>?";


        dataHienPlayer data = new dataHienPlayer(nameplayer1, khungavt1, avtplayer1.sprite, nameplayer2, khungavt2, avtplayer2.sprite, btnchon.transform.GetSiblingIndex());
        //  debug.Log("data " + data.nameplayer2);
        tbc.btnChon.onClick.AddListener(delegate { ReplayOk(idtrandau, linkdata, data); });

     //   ReplayOk(idtrandau,linkdata,);
    }

    private void ReplayOk(string idtrandau, string linkdata, dataHienPlayer data)
    {
        if (Friend.ins.QuaNha)
        {
            GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;//GameObject.Find("BGDao" + crgame.DangODao);
            Dao.SetActive(true);
            Friend.ins.DataDao = null;

            transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, transform.position.z);
            CrGame.ins.ngaydem.transform.position = Dao.transform.position;
            CrGame.ins.ngaydem.transform.SetParent(Dao.transform);
            CrGame.ins.ngaydem.transform.SetSiblingIndex(4);
            CrGame.ins.Bg.transform.position = Dao.transform.position;
        }
        NetworkManager.ins.loidai.Clear();
        NetworkManager.ins.loidai.gameObject.SetActive(false);
        VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanDem");
        ReplayData.quayve = QuayVe;

        ReplayData.StartReplay(idtrandau, linkdata, data);
        gameObject.SetActive(false);
       // AllMenu.ins.DestroyMenu("GiaoDienThongTin");
    }    
    public static void QuayVe()
    {
        AllMenu.ins.DestroyallMenu();
        GiaoDienPVP.ins.gameObject.SetActive(false);
        // VienChinh.vienchinh.gameObject.SetActive(false);
        VienChinh.vienchinh.enabled = false;

        CrGame.ins.giaodien.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");

        if (Friend.ins.QuaNha)
        {
            Friend.ins.GoHome();
            return;
        }
     
        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        Dao.SetActive(true);
        Vector3 newvec = Dao.transform.position; newvec.z = -10;
        CrGame.ins.transform.position = newvec;


    
        CrGame.ins.txtDanhVong.gameObject.SetActive(false);

        if(NetworkManager.ins.loidai.CanvasS != null)
        {
            NetworkManager.ins.loidai.GiaoDien.transform.SetParent(NetworkManager.ins.loidai.CanvasS.transform);
            NetworkManager.ins.loidai.GiaoDien.transform.SetSiblingIndex(2);
            for (int i = 0; i < NetworkManager.ins.loidai.objGiaoDienOff.Length; i++)
            {
                NetworkManager.ins.loidai.objGiaoDienOff[i].SetActive(true);
            }
        }    
  
        //    gameObject.SetActive(false);
    }
}
