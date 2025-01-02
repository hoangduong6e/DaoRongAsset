
using SocketIO;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

using SimpleJSON;
using System.Collections.Generic;
using System.IO;
using System;

using Random = UnityEngine.Random;

[SerializeField]
public class postSend
{
    public string data, taikhoan;
    public postSend(string Data,string TaiKhoan)
    {
        taikhoan = TaiKhoan;
        data = Data;
    }
}
public class NetworkManager : MonoBehaviour
{
    public SocketIOComponent socket;
    public GameObject go;public Inventory inventory;LaiRong lairong;public VienChinh vienchinh;HopQua hopqua;ThuyenThucAn thuyen;
    [HideInInspector] public Friend friend;public GameObject Thongbao, TextThongbao;[HideInInspector] public LoiDai loidai;//QuaTangHangNgay quatanghangngay;
    public GameObject menuLoiDai;[HideInInspector] public int LevelNhaAp = 0;
    public NhiemVu Nhiemvu;//public DauTruongOnline dautruong;
    public delegate void CallbackServerResult(JSONNode json);
    public static NetworkManager ins;
    private void Awake()
    {
        ins = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ins = this;
         friend = GetComponent<Friend>();
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("duocthuhoach", DcThuHoach);
        socket.On("quacongtrinh", quaCongtrinh);
        CrGame.ins.menulogin.SetActive(true);
        socket.On("open", ConnectSuccess);// ket noi server thanh cong
        socket.On("error", ConnectError);// ket noi server that bai
        socket.On("Tharongthanhcong", ThaRongOk);// ket noi server that bai
        socket.On("rongdoi", RongDoi);
        socket.On("LoginSuccess", LoginSuccess);
        socket.On("upcongtrinhthanhcong", Nangctthanhcong);
        inventory = GetComponent<Inventory>();
        socket.On("itemlairong", GetitemLai);
        socket.On("laithanhcong", OnlaiThanhcong);
        socket.On("Thongbao", OnThongBao);
        socket.On("Info", Info);
        socket.On("Friend", Friend);
        socket.On("VienChinh", Vienchinh);
        socket.On("LoiDai", LoiDai);
        socket.On("updateMoney", UpdateMoney);
        socket.On("Event", Event);
        hopqua = GameObject.Find("btnQuaOnline").GetComponent<HopQua>();
      //  quatanghangngay = GetComponent<QuaTangHangNgay>();
        thuyen = GetComponent<ThuyenThucAn>();
        loidai = menuLoiDai.GetComponent<LoiDai>();
     //   vienchinh = GameObject.FindGameObjectWithTag("vienchinh").GetComponent<VienChinh>();
    }
    public static bool isSend = true;

    public int solanrequest = 0;
    public void SendServer(JSONClass dataa, CallbackServerResult call, bool setisSend = false)
    {
        if (!isSend && !setisSend) return;

        //StartCoroutine(Load());

        //IEnumerator Load()
        //{
        //   // debug.Log("LoginFacebook.ins.keyAes: " + LoginFacebook.ins.keyAes + ", LoginFacebook.ins.IVAes: " + LoginFacebook.ins.IVAes);
        //    postSend p = new postSend(AesEncryption.Encrypt(dataa), LoginFacebook.ins.id); // Dữ liệu cần gửi
        //    string data = JsonUtility.ToJson(p); // Chuyển đổi đối tượng thành chuỗi JSON

        //    // Tạo request POST
        //    var request = new UnityWebRequest(CrGame.ins.ServerName + "RequestSend23", "POST");
        //    debug.Log("sendddd: " + dataa.ToString());

        //    // Chuyển đổi chuỗi JSON thành mảng byte và thiết lập nội dung yêu cầu
        //    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        //    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        //    request.downloadHandler = new DownloadHandlerBuffer();

        //    // Thiết lập headers
        //    request.SetRequestHeader("Content-Type", "application/json");
        //    // Nếu cần gửi token trong header
        //    request.SetRequestHeader("Authorization", "Bearer " + LoginFacebook.token);  // Ví dụ về Authorization header nếu có

        //    // Gửi yêu cầu
        //    yield return request.SendWebRequest();

        //    // Kiểm tra lỗi (cập nhật với result trong các phiên bản mới của Unity)
        //    if (request.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log("Request failed: " + request.error); // Hiển thị lỗi
        //        JSONClass jsonerror = new JSONClass();
        //        jsonerror["status"] = "1";
        //        jsonerror["message"] = "Không thể kết nối tới máy chủ";
        //        call(jsonerror); // Trả về kết quả lỗi
        //    }
        //    else
        //    {
        //        // Nếu thành công, parse và trả về kết quả
        //     //   debug.Log(request.downloadHandler.text);
        //        JSONNode json = JSON.Parse(AesEncryption.Decrypt(request.downloadHandler.text));
        //        call(json); // Trả về dữ liệu đã nhận từ server

        //        solanrequest = json["solanrequest"].AsInt;

        //        socket.Emit("testttttt", JSONObject.CreateStringObject(AesEncryption.Encrypt(dataa)), (response) =>
        //        {
        //            // Xử lý phản hồi từ server
        //            solanrequest = json["solanrequest"].AsInt;
        //            Debug.Log("Server response: " + response.ToString());
        //            call(json); // Trả về dữ liệu đã nhận từ server
        //            isSend = true;
        //        });
        //    }

        //    // Dọn dẹp
        //    request.Dispose();
        //    isSend = true;

        //    CrGame.ins.panelLoadDao.SetActive(false);
        //}


       // JSONObject.

       
        //socket.Emit("SendRequest", JSONObject.CreateStringObject(AesEncryption.Encrypt(dataa)), (response) =>
        //{
        //    Debug.Log("Server response: " + response.ToString());
        //    Debug.Log("Server responseeee: " + response[0].str);
        //    JSONNode json = JSON.Parse(AesEncryption.Decrypt(response[0].str));
        //    solanrequest = json["solanrequest"].AsInt;

        //    call(json); // Trả về dữ liệu đã nhận từ server
        //    isSend = true;
        //});

      //  JSONNode json = JSON.Parse(dataa);
      //  JSONClass jsonclass = json.AsObject;
       // Debug.Log("json gui la: " + jsonclass.ToString());
        socket.EmitWithJSONClass("SendRequest", dataa, (response) =>
        {
             debug.Log("Server response: " + response.ToString());
            //Debug.Log("Server responseeee: " + response[0].str);
            // JSONNode json = JSON.Parse(AesEncryption.Decrypt(response[0].str));
            // solanrequest = json["solanrequest"].AsInt;
            // JSONNode json = JSON.Parse(response[0].ToString());
            call(response[0]); // Trả về dữ liệu đã nhận từ server
            isSend = true;
        });


        //StartCoroutine(Send());
        //IEnumerator Send()
        //{
        //    if (!setisSend) StartCoroutine(StartSend());
        //    //crgame.OnThongBao(true, "Đang Mời...", false);
        //    //   UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "SendRequest/data/" + data + "/key/" + LoginFacebook.ins.key+"/taikhoan/"+LoginFacebook.ins.id);

        //    data = AesEncryption.Encrypt(data);
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "RequestSend23/data/" + data + "/taikhoan/" + LoginFacebook.ins.id);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    www.SetRequestHeader("Authorization", "Bearer " + LoginFacebook.token);
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        //   CrGame.ins.OnThongBaoNhanh("Không thể kết nối tới máy chủ!");
        //        JSONClass jsonerror = new JSONClass();
        //        jsonerror["status"] = "1";
        //        jsonerror["message"] = "Không thể kết nối tới máy chủ";
        //        call(jsonerror);
        //    }
        //    else
        //    {
        //        // Show results as text
        //        // debug.Log(www.downloadHandler.text);

        //        JSONNode json = JSON.Parse(www.downloadHandler.text);

        //        //StopCoroutine(StartSend());

        //        call(json);
        //    }
        //    CrGame.ins.panelLoadDao.SetActive(false);
        //    isSend = true;
        //}
    }
    IEnumerator StartSend()
    {
        isSend = false;
        yield return new WaitForSeconds(3);
        if (!isSend)
        {
            CrGame.ins.panelLoadDao.SetActive(true);
        }
        //yield return new WaitForSeconds(20);
        //if (!isSend)
        //{
        //    StopAllCoroutines();
        //    CrGame.ins.OnThongBaoNhanh("Kết nối mạng không ổn định!");
        //    CrGame.ins.panelLoadDao.SetActive(false);
        //}
    }

    void Event(SocketIOEvent e)
    {
        debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        if (e.data["itemdao"])
        {
            Transform dao = null;
            debug.Log("daominhhh" + e.data["daominh"].str);
            if (e.data["daominh"].str == "daominh")
            {
                dao = CrGame.ins.AllDao.transform.Find("BGDao" + e.data["dao"].ToString());
            }
            else dao = friend.DaoFriend.transform.Find("DaoFriend" + e.data["dao"].ToString());
            debug.Log("dao name " + dao);
            if (dao != null)
            {
                Transform ObjItemEvent = dao.transform.Find("ObjItemEvent");
                if (ObjItemEvent != null)
                {
                    Transform item = ObjItemEvent.transform.Find(e.data["itemdao"].str);
                    if (item != null)
                    {
                        item.GetComponent<Button>().enabled = false;
                        item.transform.LeanScale(Vector3.zero, 0.2f);
                        Destroy(item.gameObject, 0.2f);
                    }

                }
            }
            if (e.data["message"].str != "") CrGame.ins.OnThongBaoNhanh(e.data["message"].str, 3);

        }
        if (e.data["emitevent"])
        {
            if(MenuEventTrungThu2024.inss != null)
            {
                MenuEventTrungThu2024.inss.EmitEvent(e.data);
            }
            if (e.data["emitevent"]["message"])
            {
                CrGame.ins.OnThongBaoNhanh(e.data["emitevent"]["message"].str);
            }
        }    
        //if (e.data["TangThiep"])
        //{
        //    debug.Log("tangthiep");
        //    GameObject trencung = GameObject.FindGameObjectWithTag("trencung");

        //    GameObject phao = Inventory.LoadObjectResource("GameData/EventTrungThu2023/Anim" + CatDauNgoacKep(e.data["TangThiep"].ToString()));
        //    GameObject phaoo = Instantiate(phao, transform.position, Quaternion.identity);
        //    phaoo.transform.position = new Vector3(phaoo.transform.position.x, transform.position.y, 10);
        //    phaoo.transform.SetParent(trencung.transform);
        //    phaoo.transform.SetSiblingIndex(0);
        //    //    phaoo.transform.GetChild(0).transform.position = new Vector3(phaoo.transform.GetChild(0).transform.position.x , phaoo.transform.GetChild(0).transform.position.y,10);
        //    phaoo.SetActive(true);
        //    //if (i % 2 == 0)
        //    //{
        //    //    AudioSource audio = phaoo.GetComponent<AudioSource>();
        //    //    audio.Play();
        //    //}
        //    //  phaoo.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //    Destroy(phaoo, 2f);
        //}
        // event sinhnhat2023
        //if (e.data["TangThiep"])
        //{
        //    debug.Log("tangthiep");
        //    GameObject trencung = GameObject.FindGameObjectWithTag("trencung");
        //    StartCoroutine(delay());
        //    IEnumerator delay()
        //    {
        //        int soluongphao = int.Parse(CatDauNgoacKep(e.data["soluong"].ToString()));
        //        for (int i = 0; i < soluongphao; i++)
        //        {
        //            GameObject phao = Inventory.LoadObjectResource("GameData/EventSinhNhat2023/" + CatDauNgoacKep(e.data["TangThiep"].ToString()));
        //            GameObject phaoo = Instantiate(phao, transform.position, Quaternion.identity);
        //            phaoo.transform.position = new Vector3(phaoo.transform.position.x + Random.Range(-5, 5), transform.position.y - Random.Range(-2,2), 10);
        //            phaoo.transform.SetParent(trencung.transform);
        //            //    phaoo.transform.GetChild(0).transform.position = new Vector3(phaoo.transform.GetChild(0).transform.position.x , phaoo.transform.GetChild(0).transform.position.y,10);
        //            phaoo.SetActive(true);
        //            if (i % 2 == 0)
        //            {
        //                AudioSource audio = phaoo.GetComponent<AudioSource>();
        //                audio.Play();
        //            }
        //            //  phaoo.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //            Destroy(phaoo, 1.5f);
        //            yield return new WaitForSeconds(0.2f);
        //        }
        //    }
        //}
        ////

        //if (e.data["diemtangthiep"])
        //{
        //    if (AllMenu.ins.menu.ContainsKey("MenuEventTet2023"))
        //    {
        //        GameObject menutuoicay = AllMenu.ins.menu["MenuEventTet2023"].transform.GetChild(0).gameObject;
        //        Text txtdiemphao = menutuoicay.transform.GetChild(7).transform.GetChild(0).GetComponent<Text>();
        //        txtdiemphao.text = CatDauNgoacKep(e.data["diemphaohoa"].ToString());
        //    }
        //}

        // event tet 2024
        //if (e.data["DotPhao"])
        //{
        //    debug.Log("dotphao");
        //    StartCoroutine(delay());
        //    IEnumerator delay()
        //    {
        //        int soluongphao = int.Parse(CatDauNgoacKep(e.data["soluongphao"].ToString()));
        //        for (int i = 0; i < soluongphao; i++)
        //        {
        //            GameObject phao = Inventory.LoadObjectResource("GameData/EventTet2024/" + CatDauNgoacKep(e.data["DotPhao"].ToString()));
        //            GameObject phaoo = Instantiate(phao, transform.position, Quaternion.identity);
        //            phaoo.transform.position = new Vector3(phaoo.transform.position.x + Random.Range(-5, 5), transform.position.y - 4, 10);
        //            //    phaoo.transform.GetChild(0).transform.position = new Vector3(phaoo.transform.GetChild(0).transform.position.x , phaoo.transform.GetChild(0).transform.position.y,10);
        //            phaoo.SetActive(true);
        //            if (i % 2 == 0)
        //            {
        //                AudioSource audio = phaoo.GetComponent<AudioSource>();
        //                audio.Play();
        //            }
        //            //  phaoo.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //            Destroy(phaoo, 1.5f);
        //            yield return new WaitForSeconds(0.2f);
        //        }
        //    }
        //}
        //if (e.data["diemphaohoa"])
        //{
        //    if (AllMenu.ins.menu.ContainsKey("MenuEventTet2023"))
        //    {
        //        GameObject menutuoicay = AllMenu.ins.menu["MenuEventTet2023"].transform.GetChild(0).gameObject;
        //        Text txtdiemphao = menutuoicay.transform.GetChild(7).transform.GetChild(0).GetComponent<Text>();
        //        txtdiemphao.text = CatDauNgoacKep(e.data["diemphaohoa"].ToString());
        //    }
        //}
        ////////////////////////
    }
    void LoiDai(SocketIOEvent e)
    {
        if(e.data["usertop"])
        {
            if (!loidai.gameObject.activeSelf) return;
            StartCoroutine(delay());
            IEnumerator delay()
            {
                int limit = 0;
                if (e.data["usertop"]["usertop"].Count < 11)
                {
                    limit = 11 - e.data["usertop"]["usertop"].Count;
                }
                loidai.txtSoluotFree.text = CatDauNgoacKep(e.data["usertop"]["soluotdauloidai"].ToString());
                menuLoiDai.SetActive(true);
                CrGame.ins.menulogin.SetActive(false);
                AudioManager.SetSoundBg("nhacnen1");
             //   debug.Log(e.data["usertop"]["usertop"]);
                for (int i = 0; i < e.data["usertop"]["usertop"].Count; i++)
                {
                    if(menuLoiDai.activeSelf)
                    {
                        if (e.data["usertop"]["usertop"][i]["tenhienthi"])
                        {
                            string tenhienthi = CatDauNgoacKep(e.data["usertop"]["usertop"][i]["tenhienthi"].ToString());
                            string idfb = CatDauNgoacKep(e.data["usertop"]["usertop"][i]["idfb"].ToString());
                            string top = e.data["usertop"]["usertop"][i]["top"].ToString();
                            if(i == 0)
                            {
                                loidai.CreateUserTop(0, tenhienthi, top, idfb, CatDauNgoacKep(e.data["usertop"]["usertop"][i]["chientuong"].ToString()),byte.Parse(e.data["usertop"]["usertop"][i]["sao"].ToString()));
                            }
                            else
                            {
                                loidai.CreateUserTop(limit, tenhienthi, top, idfb, CatDauNgoacKep(e.data["usertop"]["usertop"][i]["chientuong"].ToString()), byte.Parse(e.data["usertop"]["usertop"][i]["sao"].ToString()));
                            }
                            limit += 1;
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        if(e.data["danhloidai"])
        {
            ReplayData.Record = bool.Parse(GamIns.CatDauNgoacKep(e.data["Record"].ToString()));
            ReplayData.ResetReplayData();
            CrGame.ins.giaodien.SetActive(false);
            vienchinh.chedodau = CheDoDau.LoiDai;
            Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
            Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
            GiaoDienPVP giaoDien = AllMenu.ins.GetCreateMenu("GiaoDienPVP").GetComponent<GiaoDienPVP>();
            giaoDien.btnSetting.SetActive(false);
         //   giaoDien.time = 0;
           // debug.Log("ok 1");
            for (int i = 0; i < e.data["danhloidai"]["doihinh"].Count; i++)
            {
                string[] id = e.data["danhloidai"]["doihinh"][i]["id"].ToString().Split('"');
                string[] nameObject = e.data["danhloidai"]["doihinh"][i]["nameobject"].ToString().Split('"');
                giaoDien.AddItemRongDanh(nameObject[1], id[1], int.Parse(e.data["danhloidai"]["doihinh"][i]["sao"].ToString()), int.Parse(e.data["danhloidai"]["doihinh"][i]["tienhoa"].ToString()), i);
              //  debug.Log("AddItemRongDanh " + i);
            }
         //   debug.Log("ok 1");
            GiaoDienPVP.ins.LoadSkill(e.data["danhloidai"]["skill"]);
           /// debug.Log("ok 2");
            if (CatDauNgoacKep(e.data["danhloidai"]["icontoclenh"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconTocLenhXanh");
            if (CatDauNgoacKep(e.data["danhloidai"]["iconcovip"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconCoVipXanh");
            if (CatDauNgoacKep(e.data["danhloidai"]["iconhoathanlong"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconHoaThanLongXanh");
            if (CatDauNgoacKep(e.data["danhloidai"]["iconNuiThanBi"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconNuiThanBiXanh");

            if (CatDauNgoacKep(e.data["danhloidai"]["icontoclenhfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconTocLenhDo");
            if (CatDauNgoacKep(e.data["danhloidai"]["iconcovipfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconCoVipDo");
            if (CatDauNgoacKep(e.data["danhloidai"]["iconhoathanlongfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconHoaThanLongDo");
            if (CatDauNgoacKep(e.data["danhloidai"]["iconNuiThanBifriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconNuiThanBiDo");
            //  RuntimeAnimatorController[] AnimFriend = new RuntimeAnimatorController[e.data["danhloidai"]["doihinhfriend"].Count];
            // debug.Log("ok 3");
            StartCoroutine(delayfriend());
            IEnumerator delayfriend()
            {
                int count = e.data["danhloidai"]["doihinhfriend"].Count;
                CrGame.ins.menulogin.SetActive(false);
                GiaoDienPVP.ins.transform.Find("btnTrieuHoiNhanh").gameObject.SetActive(true);
               // AudioManager.SetSoundBg("");
                vienchinh.StartCoroutine(vienchinh.delayGame("nhacloidai"));
                yield return new WaitForSeconds(3.5f);
                for (int i = 0; i < count;i++)
                {
                   PVEManager.TrieuHoiDra(e.data["danhloidai"]["doihinhfriend"][i], "TeamDo");
                 
                    yield return new WaitForSeconds(0.1f);
                   
                }
            }
        }
        if (e.data["danhthuthach"])
        {
            //    Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
            //   Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
       
            GiaoDienPVP.ins.SoDoihinh = 0;
            GiaoDienPVP.ins.maxtime = 60;
            GiaoDienPVP.ins.TxtTime.GetComponent<timePvp>().enabled = true;
          //  txtProgress.text = "0%";
          //  Progress.fillAmount = 0 / (float)100;
            for (int i = 0; i < e.data["danhthuthach"]["doihinh"].Count; i++)
            {
                string[] id = e.data["danhthuthach"]["doihinh"][i]["id"].ToString().Split('"');
                string[] nameObject = e.data["danhthuthach"]["doihinh"][i]["nameobject"].ToString().Split('"');
                GiaoDienPVP.ins.AddItemRongDanh(nameObject[1], id[1], int.Parse(e.data["danhthuthach"]["doihinh"][i]["sao"].ToString()), int.Parse(e.data["danhthuthach"]["doihinh"][i]["tienhoa"].ToString()), i);
               // inventory.LoadRongCoBan(nameObject[1]);
            }
       //     debug.Log(e.data["danhthuthach"]);
            TruVienChinh trux = vienchinh.TruXanh.GetComponent<TruVienChinh>();
            trux.allmau = 1;
            TruVienChinh trud = vienchinh.TruDo.GetComponent<TruVienChinh>(); trud.allmau = 1;
            for (int i = 0; i < trux.Hp.Length; i++)
            {
                trux.Hp[i] = 1;
                trud.Hp[i] = 1;
            }
            GiaoDienPVP.ins.LoadSkill(e.data["danhthuthach"]["skill"]);
            vienchinh.HienIconSkill(200, "Do", "icon" + CatDauNgoacKep(e.data["skilldich"].ToString()) + "Do");
            vienchinh.HienIconSkill(200, "Xanh", "icon" + CatDauNgoacKep(e.data["skillchon"].ToString()) + "Xanh");
            vienchinh.nameskillthuthach = CatDauNgoacKep(e.data["skillchon"].ToString());
            vienchinh.nameskillthuthachdich = CatDauNgoacKep(e.data["skilldich"].ToString());

            StartCoroutine(delayfriend());
            IEnumerator delayfriend()
            {
                int count = e.data["danhthuthach"]["doihinhfriend"].Count;
                CrGame.ins.menulogin.SetActive(false);
                vienchinh.chedodau = CheDoDau.ThuThach;
                vienchinh.enabled = true;
                ReplayData.Record = false;
                vienchinh.StartCoroutine(vienchinh.delayGame());
                yield return new WaitForSeconds(3.5f);
                for (int i = 0; i < count; i++)
                {
                    PVEManager.TrieuHoiDra(e.data["danhthuthach"]["doihinhfriend"][i], "TeamDo");

                  
                    yield return new WaitForSeconds(0.1f);

                }
            }
        }
        //if (e.data["SoloKhongTuoc"])
        //{
        //   // debug.Log(e.data);
        //    if (CatDauNgoacKep(e.data["status"].ToString()) == "ok")
        //    {
        //        CrGame.ins.menulogin.SetActive(true);
        //        MenuEventVuonHoaThangTu vuonhoa = AllMenu.ins.menu["MenuEventVuonHoaThangTu"].GetComponent<MenuEventVuonHoaThangTu>();
        //        vuonhoa.VeNha();
        //        vienchinh.dameKhongTuoc = 0;
        //        GiaoDienPVP gdpvp = AllMenu.ins.GetCreateMenu("GiaoDienPVP").GetComponent<GiaoDienPVP>();
        //        // Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
        //        //    Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
        //        gdpvp.maxtime = 60;
        //        gdpvp.SoDoihinh = 0;
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //       // CrGame.ins.allmenu.menu["MenuEventVuonHoaThangTu"].GetComponent<MenuEventVuonHoaThangTu>().VeNha();
        //        for (int i = 0; i < e.data["SoloKhongTuoc"]["doihinh"].Count; i++)
        //        {
        //            string[] id = e.data["SoloKhongTuoc"]["doihinh"][i]["id"].ToString().Split('"');
        //            string[] nameObject = e.data["SoloKhongTuoc"]["doihinh"][i]["nameobject"].ToString().Split('"');
        //            gdpvp.AddItemRongDanh(nameObject[1], id[1], int.Parse(e.data["SoloKhongTuoc"]["doihinh"][i]["sao"].ToString()), int.Parse(e.data["SoloKhongTuoc"]["doihinh"][i]["tienhoa"].ToString()), i);
        //        }
        //        gdpvp.LoadSkill(e.data["SoloKhongTuoc"]["skill"]);
        //        if (CatDauNgoacKep(e.data["SoloKhongTuoc"]["icontoclenh"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconTocLenhXanh");
        //        if (CatDauNgoacKep(e.data["SoloKhongTuoc"]["iconcovip"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconCoVipXanh");
        //        if (CatDauNgoacKep(e.data["SoloKhongTuoc"]["iconhoathanlong"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconHoaThanLongXanh");

        //        if (CatDauNgoacKep(e.data["SoloKhongTuoc"]["icontoclenhfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconTocLenhDo");
        //        if (CatDauNgoacKep(e.data["SoloKhongTuoc"]["iconhoathanlongfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconHoaThanLongDo");
        //     //   RuntimeAnimatorController[] AnimFriend = new RuntimeAnimatorController[e.data["SoloKhongTuoc"]["doihinhfriend"].Count];
        //        StartCoroutine(delayfriend());
        //        IEnumerator delayfriend()
        //        {
        //            int count = e.data["SoloKhongTuoc"]["doihinhfriend"].Count;
        //            CrGame.ins.menulogin.SetActive(false);
        //            vienchinh.chedodau = CheDoDau.SoloKhongTuoc;
        //            vienchinh.enabled = true;

        //            vienchinh.StartCoroutine(vienchinh.delayGame());
        //            yield return new WaitForSeconds(3.5f);
        //            PVEManager.TrieuHoiDra(e.data["SoloKhongTuoc"]["doihinhfriend"][0], "TeamDo");
        //            debug.Log(e.data["SoloKhongTuoc"]["doihinhfriend"][0]);
        //            yield return new WaitUntil(()=>vienchinh.TeamDo.transform.childCount > 1);
        //            yield return new WaitForSeconds(0.5f);
        //            debug.Log("Tăng scale khổng tước lên 1.5");
        //            //DragonPVEController khongtuoc = vienchinh.TeamDo.transform.GetChild(1).transform.Find("SkillDra").GetComponent<DragonPVEController>();
        //            //Vector3 vec = khongtuoc.transform.parent.transform.localScale;
        //            //vec = new Vector3(vec.x * 1.3f, vec.y * 1.3f);
        //            //khongtuoc.transform.parent.localScale = vec;
        //        }
        //    }
        //    else
        //    {
        //        CrGame.ins.panelLoadDao.SetActive(false);
        //        CrGame.ins.OnThongBaoNhanh(CatDauNgoacKep(e.data["status"].ToString()),2);
        //    }
        //}
        //if (e.data["DoTrieuHoi"])
        //{
        //    string[] nameitem = e.data["DoTrieuHoi"]["nameitem"].ToString().Split('"');
        //    string[] namerong = e.data["DoTrieuHoi"]["namerong"].ToString().Split('"');
        //    string[] nameObject = e.data["DoTrieuHoi"]["nameobject"].ToString().Split('"');
        //    string[] id = e.data["DoTrieuHoi"]["id"].ToString().Split('"');
        //    Vector3 randomvec = new Vector3(vienchinh.TruDo.transform.position.x + Random.Range(0, 5), vienchinh.TruXanh.transform.position.y + Random.Range(-1, -3f), vienchinh.TruDo.transform.position.z);
        //    GameObject hieuung = Instantiate(vienchinh.HieuUngTrieuHoi, new Vector3(randomvec.x, 6, randomvec.z), Quaternion.identity) as GameObject;
        //    StartCoroutine(hieuungtrieuhoi());
        //    IEnumerator hieuungtrieuhoi()
        //    {
        //        yield return new WaitForSeconds(0.4f);
        //        GameObject rongtrieuhoi = Instantiate(inventory.ObjectRong(nameObject[1]), randomvec, Quaternion.identity) as GameObject;
        //        Animator anim = rongtrieuhoi.GetComponent<Animator>();
        //        if (anim.runtimeAnimatorController == null)
        //        {
        //            rongtrieuhoi.GetComponent<Animator>().runtimeAnimatorController = inventory.LoadAnimatorRongCoBan(nameObject[1]);//SGResources.Load<RuntimeAnimatorController>( nameObject[1]);
        //        }
        //        rongtrieuhoi.name = "name-" + CatDauNgoacKep(e.data["DoTrieuHoi"]["id"].ToString());
        //        DragonController dra = rongtrieuhoi.GetComponent<DragonController>();
        //        dra.tienhoa = byte.Parse(e.data["DoTrieuHoi"]["tienhoa"].ToString());
        //        GameObject TeamDo = vienchinh.TeamDo;
        //        rongtrieuhoi.transform.SetParent(TeamDo.transform, true);
        //        rongtrieuhoi.transform.SetSiblingIndex(TeamDo.transform.childCount - 1);
        //        Destroy(dra);
        //        ChiSo chiso = rongtrieuhoi.GetComponent<ChiSo>();
        //        for (int i = 0; i < 1; i++)
        //        {
        //            if (rongtrieuhoi.GetComponent<RongLuaAttack>())
        //            {
        //                RongLuaAttack dratancong = rongtrieuhoi.GetComponent<RongLuaAttack>();
        //                dratancong.enabled = true; break;
        //            }
        //            if (rongtrieuhoi.GetComponent<RongDatAttack>())
        //            {
        //                RongDatAttack dratancong = rongtrieuhoi.GetComponent<RongDatAttack>();
        //                dratancong.enabled = true; break;
        //            }
        //        }
        //        //Destroy(rongtrieuhoi.GetComponent<Rigidbody2D>());
        //        chiso.vienchinh = vienchinh;
        //        chiso.hp = float.Parse(e.data["DoTrieuHoi"]["chiso"]["hp"].ToString());
        //        chiso.Maxhp = float.Parse(e.data["DoTrieuHoi"]["chiso"]["hp"].ToString());
        //        dra.speed = float.Parse(e.data["DoTrieuHoi"]["chiso"]["tocchay"].ToString());
        //        dra.tienhoa = byte.Parse(e.data["DoTrieuHoi"]["tienhoa"].ToString());
        //        chiso.dame = float.Parse(e.data["DoTrieuHoi"]["chiso"]["sucdanh"].ToString());

            //        Destroy(hieuung);
            //     //   debug.Log(int.Parse(e.data["DoTrieuHoi"]["tienhoa"].ToString()));
            //       // anim.SetInteger("TienHoa", int.Parse(e.data["DoTrieuHoi"]["tienhoa"].ToString()));
            //        rongtrieuhoi.SetActive(true);
            //        anim.SetInteger("TienHoa", int.Parse(e.data["DoTrieuHoi"]["tienhoa"].ToString()));
            //    }
            //}
    } 
    void Vienchinh(SocketIOEvent e)
    {
        VienChinh.HandleSocket.ParseData(e);
    }

    void Friend(SocketIOEvent e)
    {
        debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        if (e.data["solochientuong"])
        {
            GiaoDienPVP.ins.btnSetting.SetActive(true);
            CrGame.ins.panelLoadDao.SetActive(false);
            vienchinh.chedodau = CheDoDau.solo;
            string[] id = e.data["solochientuong"]["rongminh"]["id"].ToString().Split('"');
            string[] nameObject = e.data["solochientuong"]["rongminh"]["nameobject"].ToString().Split('"');
            string[] nameObjectfriend = e.data["solochientuong"]["rongfriend"]["nameobject"].ToString().Split('"');
            GiaoDienPVP.ins.AddItemRongDanh(nameObject[1], id[1], int.Parse(e.data["solochientuong"]["rongminh"]["sao"].ToString()), int.Parse(e.data["solochientuong"]["rongminh"]["tienhoa"].ToString()),0);
            GiaoDienPVP.ins.LoadSkill(e.data["skill"]);

            vienchinh.StartCoroutine(vienchinh.delayGame());
            if (CatDauNgoacKep(e.data["icontoclenh"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconTocLenhXanh");
            if (CatDauNgoacKep(e.data["iconhoathanlong"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconHoaThanLongXanh");
            if (CatDauNgoacKep(e.data["iconcovip"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconCoVipXanh");
            if (CatDauNgoacKep(e.data["iconNuiThanBi"].ToString()) == "true") vienchinh.HienIconSkill(999, "Xanh", "iconNuiThanBiXanh");


            if (CatDauNgoacKep(e.data["icontoclenhfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconTocLenhDo");
            if (CatDauNgoacKep(e.data["iconhoathanlongfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconHoaThanLongDo");
            if (CatDauNgoacKep(e.data["iconcovipfriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconCoVipDo");
            if (CatDauNgoacKep(e.data["iconNuiThanBifriend"].ToString()) == "true") vienchinh.HienIconSkill(999, "Do", "iconNuiThanBiDo");

            //StartCoroutine(hieuungtrieuhoi(SGResources.Load("GameData/Rong/" + nameObject[1]) as GameObject));
            // StartCoroutine(trieuhoifriend(SGResources.Load("GameData/Rong/" + nameObjectfriend[1]) as GameObject));
            StartCoroutine(hieuungtrieuhoi());
         //   StartCoroutine(trieuhoifriend(inventory.ObjectRong(nameObjectfriend[1])));
            IEnumerator hieuungtrieuhoi()
            {
                yield return new WaitForSeconds(4f);
                PVEManager.TrieuHoiDra(e.data["solochientuong"]["rongminh"], "TeamXanh");
                PVEManager.TrieuHoiDra(e.data["solochientuong"]["rongfriend"], "TeamDo");
                GiaoDienPVP.ins.ContentItemRong.transform.GetChild(0).GetComponent<Button>().interactable = false;
            }
        }
        if(e.data["Add"])
        {
            if (CatDauNgoacKep(e.data["Add"].ToString()) != "")
            {
                CrGame.ins.OnThongBao(true, "Kết bạn thành công.", true);
                if (AllMenu.ins.menu.ContainsKey("menuFriend"))
                {
                    ListFriend listfriend = AllMenu.ins.menu["menuFriend"].GetComponent<ListFriend>();
                   
                    GameObject Ofriend = Instantiate(listfriend.Ofriend, listfriend.ContentFriend.transform.position, Quaternion.identity) as GameObject;
                    Ofriend.transform.SetParent(listfriend.ContentFriend.transform, false);
                    Image Avatar = Ofriend.transform.GetChild(0).GetComponent<Image>();
                    Avatar.name = e.data["Add"]["idfb"].str;
                    //btnFriend btnfr = Ofriend.GetComponent<btnFriend>();
                    //btnfr.idObjectFriend = CatDauNgoacKep(e.data["Add"]["_id"].ToString());
                    //  btnfr.idfb = CatDauNgoacKep(e.data["Add"]["idfb"].ToString());
                    //Ofriend.name = CatDauNgoacKep(e.data["Add"]["_id"].ToString());
                    Ofriend.name = CatDauNgoacKep(e.data["Add"]["name"].ToString());
                    Text txtname = Ofriend.transform.GetChild(2).GetComponent<Text>();
                    txtname.text = CatDauNgoacKep(e.data["Add"]["name"].ToString());
                    if (txtname.text.Length > 8)
                    {
                        string newname = txtname.text.Substring(0, 8) + "...";
                        txtname.text = newname;
                    }
                    Image Khung = Ofriend.transform.GetChild(1).GetComponent<Image>();
                    friend.LoadAvtFriend(CatDauNgoacKep(e.data["Add"]["objectId"].ToString()), Avatar, Khung);
                    //FB.API("https" + "://graph.facebook.com/" + CatDauNgoacKep(e.data["Add"]["taikhoan"].ToString()) + "/picture?type=large", HttpMethod.GET, delegate (IGraphResult result)
                    //{
                    //    Avatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
                    //});
                    //  friend.GetAvatarFriend(CatDauNgoacKep(e.data["Add"]["taikhoan"].ToString()),Avatar);

                    //Khung.sprite = Inventory.LoadSprite("Avatar" + CatDauNgoacKep(e.data["Add"]["coban"]["toc"].ToString()));
                    Ofriend.SetActive(true);
                }    
              
            }
            else
            {
                CrGame.ins.OnThongBao(true, "Không tìm thấy bạn bè", true);
            }
            return;
        }
        if(e.data["Delete"])
        {
            if(GameObject.Find(CatDauNgoacKep(e.data["Delete"].ToString())))
            {
                Destroy(GameObject.Find(CatDauNgoacKep(e.data["Delete"].ToString())));
            }
            return;
        }
        if(e.data["Tangqua"])
        {
            string[] cat = CatDauNgoacKep(e.data["Tangqua"]["nameitem"].ToString()).Split('*');
            if (cat[0] == "item")
            {
                inventory.AddItem(cat[1],-int.Parse(e.data["Tangqua"]["soluong"].ToString()));
            }
            else
            {
                Destroy(GameObject.Find(CatDauNgoacKep(e.data["Tangqua"]["idrong"].ToString())));
            }
            AllMenu.ins.DestroyMenu("MenuTangQua");
         //   friend.menuTangQua.SetActive(false);
            return;
        }
        if(e.data["XemQua"])
        {
            if(e.data["XemQua"]["nameitemthuong"])
            {
                // hopqua.imgQuaFriend.sprite = GameObject.Find("Sprite"+CatDauNgoacKep(e.data["XemQua"]["nameitemthuong"].ToString())).GetComponent<SpriteRenderer>().sprite;
                hopqua.imgQuaFriend.sprite = Inventory.LoadSprite(CatDauNgoacKep(e.data["XemQua"]["nameitemthuong"].ToString()));//GameObject.Find(CatDauNgoacKep(e.data["XemQua"]["nameitemthuong"].ToString())).GetComponent<Image>().sprite;
                hopqua.TxtSoLuongQua.text = "x" + e.data["XemQua"]["soluong"].ToString();
            }
            if(e.data["XemQua"]["rong"])
            {
                string nameobject = CatDauNgoacKep(e.data["XemQua"]["rong"]["nameobject"].ToString());
                string tienhoa = e.data["XemQua"]["rong"]["tienhoa"].ToString();
                //if (inventory.Cachesprite.ContainsKey(nameobject + tienhoa))
                //{
                //    hopqua.imgQuaFriend.sprite = inventory.Cachesprite[nameobject + tienhoa];
                //}
                //else
                //{
                //    hopqua.imgQuaFriend.sprite = SGResources.Load<Sprite>("GameData/Sprite/Rong/" + nameobject + tienhoa);
                //    inventory.Cachesprite.Add(nameobject + tienhoa, hopqua.imgQuaFriend.sprite);
                //}
                hopqua.imgQuaFriend.sprite = Inventory.LoadSpriteRong(nameobject + tienhoa);
                //SpriteRong spriterong = GameObject.Find("Sprite" + CatDauNgoacKep(e.data["XemQua"]["rong"]["nameobject"].ToString())).GetComponent<SpriteRong>();
                //if(spriterong.spriteTienHoa[int.Parse(e.data["XemQua"]["rong"]["tienhoa"].ToString()) - 1] != null)
                //{
                //    hopqua.imgQuaFriend.sprite = spriterong.spriteTienHoa[int.Parse(e.data["XemQua"]["rong"]["tienhoa"].ToString()) - 1];
                //}
                //else hopqua.imgQuaFriend.sprite = spriterong.spriteTienHoa[1];
                hopqua.TxtSoLuongQua.text = e.data["XemQua"]["rong"]["sao"].ToString() + " sao";
            }
            hopqua.imgQuaFriend.SetNativeSize();
            hopqua.txtNguoiTang.text = "Người tặng: " + CatDauNgoacKep(e.data["XemQua"]["nguoitang"].ToString());
            hopqua.txtTenQua.text = CatDauNgoacKep(e.data["XemQua"]["tenqua"].ToString());
            hopqua.menuQua.SetActive(true);
            return;
        }
        if (e.data["nhanqua"])
        {
            CrGame.ins.OnThongBao(false);
            if (e.data["nhanqua"]["nameitemthuong"])
            {
                inventory.AddItem(CatDauNgoacKep(e.data["nhanqua"]["nameitemthuong"].ToString()), int.Parse(e.data["nhanqua"]["soluong"].ToString()));
            }
            if (e.data["nhanqua"]["rong"])
            {
                string id = CatDauNgoacKep(e.data["nhanqua"]["rong"]["id"].ToString());
                string nameitem = CatDauNgoacKep(e.data["nhanqua"]["rong"]["nameitem"].ToString());
                byte sao = byte.Parse(CatDauNgoacKep(e.data["nhanqua"]["rong"]["sao"].ToString()));
                int level = 0; int exp = 0;
                int maxexp = int.Parse(CatDauNgoacKep(e.data["nhanqua"]["rong"]["maxexp"].ToString()));
                byte tienhoa = byte.Parse(CatDauNgoacKep(e.data["nhanqua"]["rong"]["tienhoa"].ToString()));
                float sothucan = float.Parse(CatDauNgoacKep(e.data["nhanqua"]["rong"]["timedoi"].ToString()));
                string tenrong = CatDauNgoacKep(e.data["nhanqua"]["rong"]["namerong"].ToString());
                string nameobject = CatDauNgoacKep(e.data["nhanqua"]["rong"]["nameobject"].ToString());
                inventory.AddItemRong(id, nameitem, sao, level, exp, maxexp, tienhoa, sothucan, tenrong, nameobject, e.data["nhanqua"]["rong"]["hoangkim"], e.data["nhanqua"]["rong"]["ngoc"],false);
            }
        
            hopqua.SoquaDanhan += 1;
            hopqua.imgQua[friend.quaxem].sprite = hopqua.ImgquaKhongDuocNhan;
            hopqua.imgQua[friend.quaxem].GetComponent<Button>().enabled = false;
            hopqua.imgQua[friend.quaxem].transform.GetChild(0).gameObject.SetActive(true);
            hopqua.imgQua[friend.quaxem].transform.GetChild(0).GetComponent<Image>().sprite = hopqua.thedanhan;
            hopqua.boolqua[friend.quaxem] = "danhan";
            hopqua.SoquaFriend -= 1;
            if (hopqua.SoquaFriend > 0)
            {
                hopqua.coquafriend.SetActive(true);
                Text txtsoqua = hopqua.coquafriend.transform.GetChild(0).GetComponent<Text>();
                txtsoqua.text = hopqua.SoquaFriend.ToString();
            }
            else hopqua.coquafriend.SetActive(false);
            hopqua.menuQua.SetActive(false);
            return;
        }
        if (e.data["friendtangqua"])
        {
            hopqua.SoquaFriend += 1;
            hopqua.coquafriend.SetActive(true);
            Text txtsoqua = hopqua.coquafriend.transform.GetChild(0).GetComponent<Text>();
            txtsoqua.text = hopqua.SoquaFriend.ToString();
            for (int i = 0; i < hopqua.boolqua.Length; i++)
            {
                if(hopqua.boolqua[i] == "")
                {
                    hopqua.imgQua[i].sprite = hopqua.imgquaDuocNhan;
                    hopqua.imgQua[i].GetComponent<Button>().enabled = true;
                    hopqua.boolqua[i] = "coqua";
                    break;
                }
            }
        }
        if(e.data["moidanhdautruong"])
        {
            //debug.Log("co nguoi moi");
            if(thuyen.ThuyenObject.activeInHierarchy)
            {
                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                tbc.txtThongBao.text = CatDauNgoacKep(e.data["moidanhdautruong"]["thongbao"].ToString());
                string id = CatDauNgoacKep(e.data["moidanhdautruong"]["id"].ToString());
                tbc.btnChon.onClick.AddListener(() => VaoRoomDauTruong(id));
                tbc.gameObject.SetActive(true);
            }    
        }

        if(e.data["RoomDauTruong"])
        {
            if(AllMenu.ins.menu.ContainsKey("MenuDauTruongOnIine"))
            {
                DauTruongOnline dautruong = AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>(); 
                for (int i = 0; i < 2; i++)
                {
                    dautruong.btnMoi[i].SetActive(false);
                   // debug.Log("co nguoi vao room1");
                    dautruong.imgAvatar[i].gameObject.SetActive(true); 
                   // debug.Log("co nguoi vao room2");
                    // friend.GetAvatarFriend(CatDauNgoacKep(e.data["RoomDauTruong"][i]["idfb"].ToString()), dautruong.imgAvatar[i]); debug.Log("co nguoi vao room3");
                    friend.LoadAvtFriend(GamIns.CatDauNgoacKep(e.data["RoomDauTruong"][i]["idfb"].ToString()), dautruong.imgAvatar[i], dautruong.imgAvatar[i].transform.GetChild(0).GetComponent<Image>());
            //        dautruong.imgAvatar[i].transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite("Avatar" + CatDauNgoacKep(e.data["RoomDauTruong"][i]["toc"].ToString())); 
                   // debug.Log("co nguoi vao room4");
                    dautruong.imgAvatar[i].transform.GetChild(1).GetComponent<Text>().text = CatDauNgoacKep(e.data["RoomDauTruong"][i]["tenhienthi"].ToString());
                }
            }
          
        }
        else if (e.data["ThoatRoom"])
        {
            if (AllMenu.ins.menu.ContainsKey("MenuDauTruongOnIine"))
            {
                DauTruongOnline dautruong = AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>();
                int i = int.Parse(CatDauNgoacKep(e.data["ThoatRoom"].ToString()));
                dautruong.btnMoi[i].SetActive(true);
                dautruong.imgAvatar[i].gameObject.SetActive(false);
            }    
  
        }
    }
    public void VaoRoomDauTruong(string id)
    {
        // net.socket.Emit("VaoRoomGiaoHuu",JSONObject.CreateStringObject(idphong));

        JSONClass datasend = new JSONClass();
        datasend["class"] = "DauTruongOnline";
        datasend["method"] = "VaoRoomDauTruong";
        datasend["data"]["id"] = id;
        SendServer(datasend, ok);
        void ok(JSONNode json)
        {
            AllMenu.ins.DestroyallMenu("MenuDauTruongOnIine");
            if (json["status"].AsString == "0")
            {
                AllMenu.ins.OpenCreateMenu("MenuDauTruongOnIine");
                for (int i = 0; i < AllMenu.ins.transform.childCount; i++)
                {
                    AllMenu.ins.transform.GetChild(i).gameObject.SetActive(false);
                }
                GameObject trencung = GameObject.FindGameObjectWithTag("trencung");
                for (int i = 1; i < trencung.transform.childCount; i++)
                {
                    trencung.transform.GetChild(i).gameObject.SetActive(false);
                }
                if(AllMenu.ins.menu.ContainsKey("menuPhoban"))
                {
                    XemPhoBan xemphoban = AllMenu.ins.menu["menuPhoban"].GetComponent<XemPhoBan>();
                    xemphoban.gameObject.SetActive(false);
                }    
      
                CrGame.ins.giaodien.SetActive(false);
                DauTruongOnline dautruong = AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>();
                dautruong = AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>();
                CrGame.ins.OnThongBao(false);
                dautruong.gameObject.SetActive(true);
                if(AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);

                dautruong.coroom = true;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                if (AllMenu.ins.menu.ContainsKey("MenuDauTruongOnIine"))
                {
                    AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>().coroom = false;
                }
            } 
                


        }
        //StartCoroutine(Taoroom());
        //IEnumerator Taoroom()
        //{
        //    CrGame.ins.OnThongBao(true, "Đang Vào...", false);
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "VaoRoomDauTruong/taikhoan/" + LoginFacebook.ins.id + "/id/" + id);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //        CrGame.ins.OnThongBao(true, "Lỗi", true);
        //        if (AllMenu.ins.menu.ContainsKey("MenuDauTruongOnIine"))
        //        {
        //            AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>().coroom = false;
        //        }
        //    }
        //    else
        //    {
        //        // Show results as text
        //        debug.Log(www.downloadHandler.text);
        //        AllMenu.ins.OpenCreateMenu("MenuDauTruongOnIine");
        //        for (int i = 0; i < AllMenu.ins.transform.childCount; i++)
        //        {
        //            AllMenu.ins.transform.GetChild(i).gameObject.SetActive(false);
        //        }
        //        GameObject trencung = GameObject.FindGameObjectWithTag("trencung");
        //        for (int i = 1; i < trencung.transform.childCount; i++)
        //        {
        //            trencung.transform.GetChild(i).gameObject.SetActive(false);
        //        }
        //        XemPhoBan xemphoban = AllMenu.ins.menu["menuPhoban"].GetComponent<XemPhoBan>();
        //        xemphoban.gameObject.SetActive(false);
        //        CrGame.ins.giaodien.SetActive(false);
        //        DauTruongOnline dautruong = AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>();
        //        dautruong = AllMenu.ins.menu["MenuDauTruongOnIine"].GetComponent<DauTruongOnline>();
        //        CrGame.ins.OnThongBao(false);
        //        dautruong.gameObject.SetActive(true);
        //        AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        //        dautruong.coroom = true;
        //    }
        //}
    }
    private IEnumerator delayHienToMoney(GameObject g)
    {
        GameObject vitribay = g;
        bool activeseft = vitribay.activeSelf;
        vitribay.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        GameObject add = Instantiate(vitribay.gameObject, vitribay.transform.position, Quaternion.identity);
        add.name = "delayhiento";
        Destroy(add,2.2f);
       // add.name = vitribay.name;
        add.SetActive(true);
        add.transform.SetParent(CrGame.ins.trencung.transform, false);

        add.transform.position = vitribay.transform.position;

        add.transform.SetAsLastSibling();
        add.transform.LeanScale(new Vector3(add.transform.localScale.x * 1.2f, add.transform.localScale.y * 1.2f, add.transform.position.z), 0.3f);
        yield return new WaitForSeconds(0.3f);
        add.transform.LeanScale(new Vector3(add.transform.localScale.x / 1.2f, add.transform.localScale.y / 1.2f, add.transform.position.z), 0.3f);

      
        yield return new WaitForSeconds(1.9f);
       
        if (activeseft) vitribay.SetActive(true);
    }
    void UpdateMoney(SocketIOEvent e)
    {
        debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        if (e.data["updatevang"])
        {
            CrGame.ins.txtVang.text = CatDauNgoacKep(e.data["updatevang"].ToString());
            LoginFacebook.ins.StartCoroutine(delayHienToMoney(CrGame.ins.txtVang.transform.parent.gameObject));
        }
        else if (e.data["updatekimcuong"])
        {
            CrGame.ins.txtKimCuong.text = CatDauNgoacKep(e.data["updatekimcuong"].ToString());
            LoginFacebook.ins.StartCoroutine(delayHienToMoney(CrGame.ins.txtKimCuong.transform.parent.gameObject));
        }
        else if (e.data["updatedanhvong"])
        {
            CrGame.ins.txtDanhVong.text = CatDauNgoacKep(e.data["updatedanhvong"].ToString());
            LoginFacebook.ins.StartCoroutine(delayHienToMoney(CrGame.ins.txtDanhVong.gameObject));
        }
        //else if(e.data["token"])
        //{
        //    string token = CatDauNgoacKep(e.data["token"].ToString());
        //    debug.Log("token: " + token);
        //    socket.Emit("authenticate",JSONObject.CreateStringObject(LoginFacebook.token));
        //}
        if (e.data["updateNoKhi"])
        {
            GiaoDienPVP.ins.OSkill.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().fillAmount = float.Parse(CatDauNgoacKep(e.data["updateNoKhi"].ToString())) / 150;
            GiaoDienPVP.ins.OSkill.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = CatDauNgoacKep(e.data["updateNoKhi"].ToString());
            if(AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        }
        else if (e.data["ThanLong"])
        {
            GameObject daoo = CrGame.ins.AllDao.transform.Find("BGDao2").gameObject;
            if (e.data["ThanLong"]["TuyetThanLong"])
            {
               
                GameObject BeThanLong = daoo.transform.Find("BeTuyetThanLong").gameObject;
                Animator anim = BeThanLong.transform.GetChild(0).GetComponent<Animator>();
                if(CatDauNgoacKep(e.data["ThanLong"]["TuyetThanLong"].ToString()) == "doi")
                {
                    anim.SetBool("Doi", true);
                    BeThanLong.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    anim.SetBool("Doi", false);
                    BeThanLong.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            if (e.data["ThanLong"]["HoaThanLong"])
            {
                GameObject BeThanLong = daoo.transform.Find("BeHoaThanLong").gameObject;
                Animator anim = BeThanLong.transform.GetChild(0).GetComponent<Animator>();
                if (CatDauNgoacKep(e.data["ThanLong"]["HoaThanLong"].ToString()) == "doi")
                {
                    anim.SetBool("Doi", true);
                    BeThanLong.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    anim.SetBool("Doi", false);
                    BeThanLong.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else if (e.data["reconnect"])
        {
            CrGame.ins.OnThongBao(false);
        }
        else if (e.data["ngatketnoi"])
        {
            debug.Log("Ngat Ket Noi");
            socket.Close();
            StartCoroutine(delayy());
            IEnumerator delayy()
            {
                yield return new WaitForSeconds(10);
                Application.LoadLevel(0);
            }
        }
        else if (e.data["cothu"])
        {
            debug.Log("cothu");
            GameObject.FindGameObjectWithTag("btnhomthu").transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (e.data["updatenhiemvu"])
        {
            string nhiemvuj = CatDauNgoacKep(e.data["updatenhiemvu"]["nhiemvu"].ToString());
            string sonhiemvu = CatDauNgoacKep(e.data["updatenhiemvu"]["sonhiemvu"].ToString());
            string namenhiemvu = CatDauNgoacKep(e.data["updatenhiemvu"]["namenv"].ToString());
            Nhiemvu.UpdateNhiemVu(namenhiemvu,sonhiemvu,nhiemvuj);
        }
        else if (e.data["useskill"])
        {
            //e.data["useskill"]["nameskill"] = JSONObject.CreateStringObject("");
             int index = int.Parse(CatDauNgoacKep(e.data["useskill"]["index"].ToString()));
            // Transform oskill = GiaoDienPVP.ins.OSkill.transform.GetChild(1);
            //  oskill.GetChild(index).transform.GetChild(1).gameObject.SetActive(true);

            string nameskill = e.data["useskill"]["nameskill"].str;
            GiaoDienPVP.ins.OSkill.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().fillAmount = float.Parse(CatDauNgoacKep(e.data["useskill"]["sonokhi"].ToString())) / 150;

            GiaoDienPVP.ins.OSkill.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = CatDauNgoacKep(e.data["useskill"]["sonokhi"].ToString());

           
            vienchinh.timeskill[index] = float.Parse(CatDauNgoacKep(e.data["useskill"]["time"].ToString()));

            vienchinh.HieuUngSkill("Skill" + CatDauNgoacKep(e.data["useskill"]["nameskill"].ToString()), float.Parse(CatDauNgoacKep(e.data["useskill"]["satthuong"].ToString())), float.Parse(CatDauNgoacKep(e.data["useskill"]["level"].ToString())));
            
            if (nameskill == "SamNo")
            {
                vienchinh.HienIconSkill(float.Parse(CatDauNgoacKep(e.data["useskill"]["timeskill"].ToString())),"Xanh", "iconSamNoXanh");
                if (vienchinh.TeamXanh.transform.childCount > 1)
                {
                    for (int i = 1; i < vienchinh.TeamXanh.transform.childCount; i++)
                    {
                        DragonPVEController dragonPVEController = vienchinh.TeamXanh.transform.GetChild(i).transform.Find("SkillDra").GetComponent<DragonPVEController>();
                        if (dragonPVEController != null)
                        {
                            dataLamCham data = new dataLamCham(float.Parse(CatDauNgoacKep(e.data["useskill"]["timeskill"].ToString())), "", 0, "0.7+0.7+1.3",false,true);
                            dragonPVEController.LamChamABS(data);// [speed][speedrun][speedAttack]
                        }
                        else break;
                    }
                }
             //   debug.Log("samno ok1");
                //if (vienchinh.TeamDo.transform.childCount > 1)
                //{
                //    for (int i = 1; i < vienchinh.TeamDo.transform.childCount; i++)
                //    {
                //        DragonPVEController dragonPVEController = vienchinh.TeamDo.transform.GetChild(i).transform.Find("SkillDra").GetComponent<DragonPVEController>();
                //        if (dragonPVEController != null)
                //        {
                //            dataLamCham data = new dataLamCham(float.Parse(CatDauNgoacKep(e.data["useskill"]["timeskill"].ToString())), "", 2, "0",false,true);
                //            dragonPVEController.LamChamABS(data);// [speed][speedrun][speedAttack]
                //        }
                //        else break;
                //    }
                //}
             //   debug.Log("samno ok2");
            }
            else if (nameskill == "DatBom")
            {

            }
            else if(nameskill == "BienCuu")
            {
                if (vienchinh.chedodau == CheDoDau.BossTG) return;
                float level = float.Parse(CatDauNgoacKep(e.data["useskill"]["level"].ToString()));


                int socuu = 2;
                float time = 8;
                for (int i = 1; i < level; i++)
                {
                    if (i < 15)
                    {
                        socuu += 2;
                        time += 0.1f;
                    }
                    else
                    {
                        socuu += 1;
                        time += 0.3f;
                    }
                }
                int socuubien = 0;
                bool send = false;
                if (vienchinh.chedodau == CheDoDau.Online)
                {
                    send = true;
                }

                JSONObject newjson = new JSONObject();
                if (send)
                {
                    newjson.AddField("listdra", new JSONObject());
                    newjson.AddField("biencuu", "");
                    newjson.AddField("time", time.ToString());
                }

                for (int i = 1; i < socuu + 1; i++)
                {
                    //  int random = UnityEngine.Random.Range(1,TeamDo.transform.childCount-1);
                    if (vienchinh.TeamDo.transform.childCount < i)
                    {
                        //  debug.Log("so cuu bien " + socuubien);
                        if (send) DauTruongOnline.ins.AddUpdateData(newjson, true);
                        return;
                    }

                    if (send)
                    {
                        newjson["listdra"].Add(vienchinh.TeamDo.transform.GetChild(i).name);
                    }
                    else
                    {
                        vienchinh.SetBienCuuOnline(vienchinh.TeamDo.transform.GetChild(i).transform, time);
                        socuubien += 1;
                        //if (TeamDo.transform.GetChild(i).transform.Find("BienCuu") == null)
                        //{
                        //    TeamDo.transform.GetChild(i).transform.Find("SkillDra").GetComponent<DragonPVEController>().BienCuuABS(time);
                        //    socuubien += 1;
                        //}
                        //else
                        //{
                        //    GameObject hieuung = TeamDo.transform.GetChild(i).transform.Find("BienCuu").gameObject;
                        //    hieuung.GetComponent<BienCuu>().time = time;
                        //    hieuung.SetActive(true);
                        //    socuubien += 1;
                        //}
                    }

                }
                if (send) DauTruongOnline.ins.AddUpdateData(newjson, true);

            }   
            else if(nameskill == "CuongLoan")
            {
                //float phantramdame = 20 + float.Parse(CatDauNgoacKep(e.data["useskill"]["level"].ToString())) * 2;
                //float time = 3 + float.Parse(CatDauNgoacKep(e.data["useskill"]["level"].ToString())) * 0.2f;
                //vienchinh.HienIconSkill(time, "Xanh", "iconCuongLoanXanh");
                //for (int i = 1; i < vienchinh.TeamXanh.transform.childCount; i++)
                //{
                //    DragonPVEController cs = vienchinh.TeamXanh.transform.GetChild(i).transform.Find("SkillDra").GetComponent<D ragonPVEController>();
                // //   cs.DayLui("TeamDo",0.5f);
                //    cs.newDame(cs.dame + cs.dame * phantramdame / 100, time,-200);
                //    cs.MatMau(cs.hp * 30 / 100,null);
                //}
            }
            else
            {
               
            }
         //   vienchinh.nokhi -= int.Parse(CatDauNgoacKep(e.data["useskill"]["nokhi"].ToString()));
            return;
        }
        else if (e.data["DuocXemQuangCao"])
        {
//#if UNITY_EDITOR || UNITY_ANDROID
            GetComponent<Admobb>().RequestRewardedVideo();
        //    CrGame.ins.BtnXemQuangCao.SetActive(true);
//#endif
        }
        else if (e.data["TimeQuaOnline"])
        {
            TimeQuaonl timequa = CrGame.ins.txtTimeQuaonl.gameObject.GetComponent<TimeQuaonl>();
            timequa.timeRemaining = float.Parse(e.data["TimeQuaOnline"].ToString());
            CrGame.ins.txtTimeQuaonl.gameObject.SetActive(true);
            timequa.gameObject.SetActive(true);
            timequa.timerIsRunning = true;
            Image img = timequa.transform.parent.GetChild(0).GetComponent<Image>();
            img.sprite = Inventory.LoadSprite(CatDauNgoacKep(e.data["NameQuaOnl"].ToString()));img.SetNativeSize();
            timequa.transform.parent.GetChild(2).gameObject.SetActive(false);
        }
        else if (e.data["mokhoaNhiemvu"])
        {
            debug.Log("Mo Khoa nhiem vu thanh cong");
            if(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"])
            {
                Nhiemvu.QuaNhanHangNgay.SetActive(true);
                Nhiemvu.ContentNVHangNgay.transform.GetChild(Nhiemvu.ContentNVHangNgay.transform.childCount - 1).gameObject.SetActive(false);
                for (int j = 0; j < 4; j++)
                {
                    string nv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["allNhiemvu"][j]["namenhiemvu"].ToString());
                    string sonv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["allNhiemvu"][j]["dalam"].ToString());
                    string maxnv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["allNhiemvu"][j]["maxnhiemvu"].ToString());
                    string keynv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["allNhiemvu"].keys[j]);
                    Nhiemvu.AddinfoNhiemVuHangNgay(j, nv, sonv + "/" + maxnv, keynv);
                    Nhiemvu.ContentNVHangNgay.transform.GetChild(j).transform.GetChild(3).gameObject.SetActive(false);
                }
                for (int i = 0; i < e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["QuaNhanDuoc"].Count; i++)
                {
                    Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
                    string namequa = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["QuaNhanDuoc"][i]["namequa"].ToString());
                    string soluong = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["QuaNhanDuoc"][i]["soluong"].ToString());
                    if (namequa != "Exp")
                    {
                        Image imgqua = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                        imgqua.sprite = Inventory.LoadSprite(namequa);
                        imgqua.SetNativeSize();
                    }
                    Text txtSoLuong = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    txtSoLuong.text = soluong;
                }
                for (int i = 0; i < e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["CoTheNhanDuoc"].Count; i++)
                {
                    Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i + 2).gameObject.SetActive(true);
                    string namequa = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["CoTheNhanDuoc"][i]["namequa"].ToString());
                    string soluong = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvuhangngay"]["CoTheNhanDuoc"][i]["soluong"].ToString());
                    if (namequa != "Exp")
                    {
                        Image imgqua = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(0).GetComponent<Image>();
                        imgqua.sprite = Inventory.LoadSprite(namequa);
                        imgqua.SetNativeSize();
                    }
                    Text txtSoLuong = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(1).GetComponent<Text>();
                    txtSoLuong.text = soluong;
                }
            }
            else if (e.data["mokhoaNhiemvu"]["nhiemvurong"])
            {
                Nhiemvu.QuaNhanRong.SetActive(true);
                Nhiemvu.ContentNvRong.transform.GetChild(Nhiemvu.ContentNvRong.transform.childCount - 1).gameObject.SetActive(false);
                for (int j = 0; j < 4; j++)
                {
                    string nv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["allNhiemvu"][j]["namenhiemvu"].ToString());
                    string sonv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["allNhiemvu"][j]["dalam"].ToString());
                    string maxnv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["allNhiemvu"][j]["maxnhiemvu"].ToString());
                    string keynv = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["allNhiemvu"].keys[j]);
                    Nhiemvu.AddinfoNhiemVuRong(j, nv, sonv + "/" + maxnv, keynv);
                    Nhiemvu.ContentNvRong.transform.GetChild(j).transform.GetChild(3).gameObject.SetActive(false);
                }
                for (int i = 0; i < e.data["mokhoaNhiemvu"]["nhiemvurong"]["QuaNhanDuoc"].Count; i++)
                {
                    Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
                    string namequa = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["QuaNhanDuoc"][i]["namequa"].ToString());
                    string soluong = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["QuaNhanDuoc"][i]["soluong"].ToString());
                    if (namequa != "Exp")
                    {
                        Image imgqua = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                        imgqua.sprite = Inventory.LoadSprite(namequa);
                        imgqua.SetNativeSize();
                    }
                    Text txtSoLuong = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    txtSoLuong.text = soluong;
                }
                for (int i = 0; i < e.data["mokhoaNhiemvu"]["nhiemvurong"]["CoTheNhanDuoc"].Count; i++)
                {
                    Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i + 2).gameObject.SetActive(true);
                    string namequa = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["CoTheNhanDuoc"][i]["namequa"].ToString());
                    string soluong = CatDauNgoacKep(e.data["mokhoaNhiemvu"]["nhiemvurong"]["CoTheNhanDuoc"][i]["soluong"].ToString());
                    if (namequa != "Exp")
                    {
                        Image imgqua = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(0).GetComponent<Image>();
                        imgqua.sprite = Inventory.LoadSprite(namequa);
                        imgqua.SetNativeSize();
                    }
                    Text txtSoLuong = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(1).GetComponent<Text>();
                    txtSoLuong.text = soluong;
                }
            }
        }
        else if (e.data["xoaitem"])
        {
            string nameitem = CatDauNgoacKep(e.data["xoaitem"]["nameitem"].ToString());
            int soluong = int.Parse(CatDauNgoacKep(e.data["xoaitem"]["soluong"].ToString()));
            inventory.AddItem(nameitem,-soluong);
        }
        else if (e.data["additem"])
        {
            string nameitem = CatDauNgoacKep(e.data["additem"]["nameitem"].ToString());
            int soluong = int.Parse(CatDauNgoacKep(e.data["additem"]["soluong"].ToString()));
            inventory.AddItem(nameitem, soluong);
        }
        else if (e.data["UpdateThanhTuu"])
        {
            string str = e.data["UpdateThanhTuu"]["thongbao"].str;
            MenuThanhTuu.thanhtuuEmitHoanThanh.Add(str);
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(Random.Range(0.02f,0.1f));

                yield return new WaitUntil(checkk);
                if (!AllMenu.ins.menu.ContainsKey("MenuThanhTuu"))
                {
                    EventManager.OpenThongBaoChon(e.data["UpdateThanhTuu"]["thongbao"].str.Replace("\\n", "\n"), () => {
                        MenuThanhTuu menu = AllMenu.ins.GetCreateMenu("MenuThanhTuu", CrGame.ins.trencung.gameObject, false,CrGame.ins.panelLoadDao.transform.GetSiblingIndex()).GetComponent<MenuThanhTuu>();
                        menu.nameThanhTuuGet = e.data["UpdateThanhTuu"]["namethanhtuu"].str;
                        menu.gameObject.SetActive(true);
                        MenuThanhTuu.thanhtuuEmitHoanThanh = new List<string>();
                    }, "Nhận quà", btnHuy, true);
                }
            }
            bool checkk()
            {
               // debug.Log("checkkkkkkkkkkkkkkkk" + " length " + MenuThanhTuu.thanhtuuEmitHoanThanh.Count  +" str "+ MenuThanhTuu.thanhtuuEmitHoanThanh[0]);
                if(MenuThanhTuu.thanhtuuEmitHoanThanh.Count > 0)
                {
                    if (MenuThanhTuu.thanhtuuEmitHoanThanh[0] == str) return true;
                }    
                return false;
            }
            void btnHuy()
            {
                MenuThanhTuu.thanhtuuEmitHoanThanh.Remove(str);
                StopCoroutine(delay());
            }
        
        }
    }
    void Info(SocketIOEvent e)
    {
        debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
   
        if(e.data["infoct"])
        {
            AllMenu.ins.menu["infoct"].transform.GetChild(0).GetComponent<Text>().text = CatDauNgoacKep(e.data["infoct"].ToString()).Replace("\\n", "\n");
            return;
        }
        if(e.data["xemnangcapthucan"])
        {
            Ui UI;
            UI = AllMenu.ins.GetCreateMenu("MenuNangCapItem", null, true).GetComponent<Ui>();
            UI.transform.SetSiblingIndex(5);
            UI.SpriteItem[0].sprite = Inventory.LoadSprite(CatDauNgoacKep(e.data["xemnangcapthucan"]["name"].ToString())); UI.SpriteItem[0].SetNativeSize();
            UI.SpriteItem[1].sprite = UI.SpriteItem[0].sprite; UI.SpriteItem[1].SetNativeSize();
            UI.txtlevel[0].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["namethucan1"].ToString());
            UI.txtlevel[1].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["namethucan2"].ToString());
            UI.txtChiSo[0].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["chiso1"].ToString()).Replace("\\n", "\n");
            UI.txtChiSo[1].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["chiso2"].ToString()).Replace("\\n", "\n");

            UI.tilethanhcong.SetActive(true);
            UI.tilethanhcong.GetComponent<Text>().text = "Cấp độ nâng cấp bằng vàng:";
            UI.tilethanhcong.transform.GetChild(0).GetComponent<Text>().text = "Cấp độ nâng cấp kim cương:";
            UI.txtTileThanhCong[0].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["capdonangbangvang"].ToString());
            UI.txtTileThanhCong[1].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["capdonangcapbangkimcuong"].ToString());
            UI.btnNangCap[0].gameObject.SetActive(false);
            UI.btnNangCap[1].gameObject.SetActive(false);
            UI.btnNangCapNhaApTrung[0].gameObject.SetActive(false);
            UI.btnNangCapNhaApTrung[1].gameObject.SetActive(false);
            UI.nangcapDao.SetActive(false);
            UI.btnNangCapThucAn[0].gameObject.SetActive(true);
            UI.btnNangCapThucAn[1].gameObject.SetActive(true);
            UI.txtGiaNangThucAn[0].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["gianangcapvang"].ToString());
            UI.txtGiaNangThucAn[1].text = CatDauNgoacKep(e.data["xemnangcapthucan"]["giannangcapbangkimcuong"].ToString());//json["giannangcapbangkimcuong"].Value;

            UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
            GameObject objThanLong = UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;
            objThanLong.SetActive(false);

            if (int.Parse(CrGame.ins.txtLevel.text) < int.Parse(CatDauNgoacKep(e.data["xemnangcapthucan"]["capdonangbangvang"].ToString()))) UI.btnNangCapThucAn[0].interactable = false;
            else UI.btnNangCapThucAn[0].interactable = true;
            if (int.Parse(CrGame.ins.txtLevel.text) < int.Parse(CatDauNgoacKep(e.data["xemnangcapthucan"]["capdonangcapbangkimcuong"].ToString()))) UI.btnNangCapThucAn[1].interactable = false;
            else UI.btnNangCapThucAn[1].interactable = true;
            return;
        }
        if(e.data["quadao"])
        {
            if(CatDauNgoacKep(e.data["quadao"].ToString()) == "thanhcong")
            {
                if (AllMenu.ins.transform.Find("BtnSangDao"))
                {
                    AllMenu.ins.transform.Find("BtnSangDao").transform.SetParent(CrGame.ins.giaodien.transform);
                }
            

                //load data dao
                 
                Dao dao = CrGame.ins.AllDao.transform.Find("BGDao"+CrGame.ins.DangODao).GetComponent<Dao>();

                DragonIslandManager.DungThucAn = dao.transform.Find("ThucAn").gameObject;
                int i = dao.transform.GetSiblingIndex();
                if (dao.load == false && e.data["datadao"])
                {
                    dao.load = true;


                    CrGame.ins.leveldao[i] = byte.Parse(CatDauNgoacKep(e.data["datadao"]["leveldao"].ToString()));
                   // debug.LogError(e.data["datadao"]["congtrinh"]);
                    Image imgCapDao = CrGame.ins.FindObject(dao.gameObject, "btnCapDao").GetComponent<Image>();
                    imgCapDao.sprite = Inventory.LoadSprite("Dao" + CrGame.ins.leveldao[i]);
                    imgCapDao.SetNativeSize();
                    GameObject congtrinhdao = CrGame.ins.FindObject(dao.gameObject, "ObjectCongtrinh");

                    for (int j = 0; j < e.data["datadao"]["congtrinh"].Count; j++)
                    {
                        CongTrinh ct = congtrinhdao.transform.GetChild(j).GetComponent<CongTrinh>();
                        string[] namect = e.data["datadao"]["congtrinh"][j]["name"].ToString().Split('"');
                        byte capct = byte.Parse(e.data["datadao"]["congtrinh"][j]["cap"].ToString());
                        ct.nameCongtrinh = namect[1]; ct.levelCongtrinh = capct; //ct.LoadImg();
                        ct.LoadImg();
                    }
                    CrGame.ins.OnThongBao(true, "Đang lấy dữ liệu Rồng...", false);
                    for (int j = 0; j < e.data["datadao"]["rong"].Count; j++)
                    {
                         debug.Log("rong " + j);
                        DragonIslandManager.ParseDragonIsland(JSON.Parse(e.data["datadao"]["rong"][j].ToString()), CrGame.ins.DangODao);
                    }


                    if (e.data["datadao"]["timeNuiThanBi"])
                    {
                        CrGame.ins.timeNuiThanBi = float.Parse(e.data["datadao"]["timeNuiThanBi"].ToString());
                        NuiLua.Instance.MauNgoc = (_mauNgoc)Enum.Parse(typeof(_mauNgoc), GamIns.CatDauNgoacKep(e.data["datadao"]["namengoc"].ToString()));
                        // debug.Log("Time nui than bi dao " + i + ": " + CrGame.ins.timeNuiThanBi);
                    }
                    if (e.data["datadao"]["itemEvent"])  DragonIslandManager.InsAllItemDao(JSON.Parse(e.data["datadao"]["itemEvent"].ToString()), CrGame.ins.DangODao);
                    //if (e.data["datadao"]["itemEvent"])
                    //{
                    //    for (int j = 0; j < e.data["datadao"]["itemEvent"].Count; j++)
                    //    {
                    //        DragonIslandManager.InsItemDao(JSON.Parse(e.data["datadao"]["itemEvent"][j].ToString()), CrGame.ins.DangODao);
                    //    }
                    //}
                    CrGame.ins.OnThongBao(true, "Đang lấy dữ liệu Trang trí", false);
                    GameObject objecttrangtridao = CrGame.ins.FindObject(dao.gameObject, "ObjectTrangTri");
                    if (e.data["datadao"]["trangtri"].Count > 0)
                    {
                        for (int j = 0; j < e.data["datadao"]["trangtri"].Count; j++)
                        {
                            // string nametrangtri = CatDauNgoacKep(e.data["datadao"]["trangtri"][j]["name"].ToString());
                            GameObject trangtri = Instantiate(Inventory.LoadObjectResource("GameData/Item/" + CatDauNgoacKep(e.data["datadao"]["trangtri"][j]["name"].ToString())) as GameObject, transform.position, Quaternion.identity) as GameObject;
                            Destroy(trangtri.GetComponent<ItemTrangTri>());
                            Destroy(trangtri.GetComponent<EventTrigger>());
                            Destroy(trangtri.GetComponent<Button>());
                            trangtri.transform.SetParent(objecttrangtridao.transform);
                            float x = 0, y = 0;
                            float.TryParse(GamIns.CatDauNgoacKep(e.data["datadao"]["trangtri"][j]["x"].ToString(),true),out x);
                            float.TryParse(GamIns.CatDauNgoacKep(e.data["datadao"]["trangtri"][j]["y"].ToString(),true),out y);
                           // float.TryParse(float.Parse(CatDauNgoacKep(e.data["datadao"]["trangtri"][j]["x"].ToString()), out y);
                            trangtri.transform.position = new Vector3(x, y, 0);
                            trangtri.transform.GetChild(1).gameObject.SetActive(true);
                            trangtri.GetComponent<Image>().enabled = false;
                            trangtri.transform.GetChild(1).transform.position = new Vector3(trangtri.transform.GetChild(1).transform.position.x, trangtri.transform.GetChild(1).transform.position.y, 0);
                            trangtri.transform.GetChild(0).gameObject.SetActive(false);
                            if (CatDauNgoacKep(e.data["datadao"]["trangtri"][j]["name"].ToString()) == "TocLenh")
                            {
                                trangtri.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator("TocLenh" + CatDauNgoacKep(e.data["coban"]["toc"].ToString()));
                            }
                          //  debug.LogError("trang tri " + j + " " + e.data["datadao"]["trangtri"][j]);
                        }
                    }
                    //debug.LogError("load den day");
                    for (int j = 0; j < e.data["datadao"]["vangroi"].Count; j++)
                    {
                        Vector3 tf = new Vector3(float.Parse(e.data["datadao"]["vangroi"][j]["x"].ToString()), float.Parse(e.data["datadao"]["vangroi"][j]["y"].ToString()));
                        GameObject vang = Instantiate(Inventory.ins.GetObj("vangroi"), tf, Quaternion.identity) as GameObject;
                        vang.transform.SetParent(CrGame.ins.FindObject(dao.gameObject, "itemroi").transform);
                        vang.SetActive(true);
                    }
                
                }

                if (e.data["daobay"])
                {
                    if(GamIns.CatDauNgoacKep(e.data["daobay"].ToString()) == "true")
                    {
                        Transform daobay = dao.transform.Find("DaoBay");
                        if (daobay != null) daobay.gameObject.SetActive(true);
                    }
                        
            

                }

                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.MenuMoDao.SetActive(false);
                CrGame.ins.OnThongBao(false);
            }
            else
            {
                CrGame.ins.MenuMoDao.transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).gameObject.SetActive(false);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.MenuMoDao.SetActive(true);
                Text txtnamedao = CrGame.ins.MenuMoDao.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                Text txtyeucau = CrGame.ins.MenuMoDao.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
                txtyeucau.text = CatDauNgoacKep(e.data["quadao"]["levelyeucau"].ToString());
                txtnamedao.text = CatDauNgoacKep(e.data["quadao"]["namedao"].ToString());
                CrGame.ins.giaodien.transform.Find("BtnSangDao").transform.SetParent(AllMenu.ins.transform);

                if(e.data["quadao"]["giamodaokc"])
                {
                    GameObject objectmodao = CrGame.ins.MenuMoDao.transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).gameObject;

                    objectmodao.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = CatDauNgoacKep(e.data["quadao"]["giamodaohuanchuong"].ToString());
                    objectmodao.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = CatDauNgoacKep(e.data["quadao"]["giamodaovang"].ToString());
                    objectmodao.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = CatDauNgoacKep(e.data["quadao"]["giamodaokc"].ToString());

                    objectmodao.SetActive(true);
                }
            }
            return;
        }
        //if (e.data["phongchientuong"])
        //{
        //    Destroy(CrGame.ins.chientuong);
        //    DragonController dra = CrGame.ins.TfrongInfo.gameObject.GetComponent<DragonController>();
        //    GameObject NewObj = new GameObject();
        //    inventory.ScaleObject(NewObj, 0.0035f, 0.0035f);
        //    NewObj.transform.position = dra.txtnamerong.transform.position;
        //    Image SpriteChienTuong = NewObj.AddComponent<Image>();
        //    SpriteChienTuong.sprite = vienchinh.ChienTuongVang; SpriteChienTuong.SetNativeSize();
        //    NewObj.transform.SetParent(dra.transform.Find("Canvas").transform);
        //    NewObj.name = "iconchientuong";
        //    CrGame.ins.chientuong = NewObj;
        //    return;
        //}
        if (e.data["nhanquatanghangngay"])
        {
            QuaTangHangNgay quatanghangngay = AllMenu.ins.GetCreateMenu("menuQuaHangNgay").GetComponent<QuaTangHangNgay>();
            AudioManager.PlaySound("sound2");
            GameObject qua = Instantiate(Inventory.LoadObjectResource("GameData/Item/" + CatDauNgoacKep(e.data["nhanquatanghangngay"]["namequa"].ToString())), quatanghangngay.quachon.transform.position, Quaternion.identity) as GameObject;
            quatanghangngay.quachon.GetComponent<Image>().enabled = false;
            if (qua.GetComponent<Button>()) qua.GetComponent<Button>().enabled = false;
            Text txtSoluong = qua.transform.GetChild(0).GetComponent<Text>();
            txtSoluong.text = CatDauNgoacKep(e.data["nhanquatanghangngay"]["soluong"].ToString());
            qua.transform.SetParent(quatanghangngay.quachon.transform);
            quatanghangngay.soqua -= 1;quatanghangngay.load = false;
            inventory.ScaleObject(qua,0.5f,0.5f);
            if(CatDauNgoacKep(e.data["nhanquatanghangngay"]["namequa"].ToString()) != "Vang")
            {
                inventory.AddItem(CatDauNgoacKep(e.data["nhanquatanghangngay"]["namequa"].ToString()), int.Parse(e.data["nhanquatanghangngay"]["soluong"].ToString()));
            }
            if (quatanghangngay.soqua <= 0)
            {
                quatanghangngay.btnNhanQua.SetActive(true);
            }
            return;
        }
        if (e.data["chisodao"])
        {
          //  AllMenu.ins.OpenMenu("MenulInfoDao");
            InfoDao infodao = AllMenu.ins.menu["MenulInfoDao"].GetComponent<InfoDao>();
            infodao.txtNameDao.text = CatDauNgoacKep(e.data["chisodao"]["namedao"].ToString());
            infodao.txtRong.text = CatDauNgoacKep(e.data["chisodao"]["sorong"].ToString());
            infodao.txtTrangtri.text = CatDauNgoacKep(e.data["chisodao"]["sotrangtri"].ToString());
            infodao.TxtCongtrinh.text = CatDauNgoacKep(e.data["chisodao"]["socongtrinh"].ToString());
            return;
        }
        if (e.data["muavatphamthanhcong"])
        {
            string[] add = CatDauNgoacKep(e.data["muavatphamthanhcong"].ToString()).Split('+');
            Shop shop = AllMenu.ins.menu["MenuShop"].GetComponent<Shop>();
            GameObject imgItem = shop.MenuMua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            QuaBay quabay = null;
            if (add[2] != "avt")
            {
                GameObject g = Instantiate(imgItem, transform.position, Quaternion.identity);
                g.transform.SetParent(CrGame.ins.trencung.transform);
                g.transform.localScale = Vector3.one / 2f;
                g.transform.position = imgItem.transform.position;
                quabay = g.AddComponent<QuaBay>();
            }
      
            if (add[0] != "funcItem")
            {
                inventory.AddItem(add[0], int.Parse(add[1]));
            }
            else
            {

                quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
            }
         
            //GameObject.Find("MenuMua").SetActive(false);
     
          //  shop.transform.GetChild(0).gameObject.SetActive(false);
            shop.MenuMua.SetActive(false);
           // AllMenu.ins.menu["MenuShop"].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            //    CrGame.ins.OnThongBao(true,"Mua Thành Công",true);
        //    CrGame.ins.OnThongBaoNhanh("Mua Thành Công",1);
            if(vienchinh.dangdau)
            {
                AllMenu.ins.menu["MenuShop"].SetActive(false);
                GiaoDienPVP.ins.txtHuyenTinh.text = inventory.ListItemThuong["itemHuyenTinh"].transform.GetChild(0).GetComponent<Text>().text;
                shop.transform.GetChild(0).gameObject.SetActive(true);
            }
            return;
        }
        if (e.data["congtrinh"])
        {
            string[] cat = CatDauNgoacKep(e.data["congtrinh"].ToString()).Split('+');
            if(GameObject.Find("item" + cat[0]))
            {
                ItemCongTrinh itemct = GameObject.Find("item" + cat[0]).GetComponent<ItemCongTrinh>();
                itemct.Huy();
            }
            inventory.AddItem(cat[0], -1);
            inventory.menuTuiDo.SetActive(false);
            GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
            CongTrinh ct = CrGame.ins.FindObject(Dao, "ObjectCongtrinh").transform.GetChild(int.Parse(cat[1])).GetComponent<CongTrinh>();
            StartCoroutine(CreateHieuUngCongtrinh(ct,cat[0]));
            return;
        }
        if(e.data["rongquayra"])
        {
            VongQuayRong vongquayrong = AllMenu.ins.menu["MenuQuayRong"].GetComponent<VongQuayRong>();
            vongquayrong.btnExit.interactable = false;
            vongquayrong.btnQuay.interactable = false;
            vongquayrong.soquay = 0;

            inventory.RemoveItem(vongquayrong.nameTrung,1);
            for (int i = 0; i < e.data["rongquayra"].Count; i++)
            {
                //debug.Log(CatDauNgoacKep(e.data["rongquayra"][i].ToString()));
                if (CatDauNgoacKep(e.data["rongquayra"][i].ToString()) != "null")
                {
                   // debug.Log("Oitem" + i + CatDauNgoacKep(e.data["rongquayra"][i]["nameobject"].ToString()));
                    QuayRong quayrong = vongquayrong.ORong[i].transform.GetChild(0).GetComponent<QuayRong>();
                    quayrong.RongQuayDuoc = GameObject.Find("Oitem" + i + CatDauNgoacKep(e.data["rongquayra"][i]["nameobject"].ToString()));
                  //  debug.Log(quayrong.RongQuayDuoc.name);
                    quayrong.imgRongQuay = quayrong.RongQuayDuoc.transform.GetChild(0).GetComponent<Image>();
                    string id = CatDauNgoacKep(e.data["rongquayra"][i]["id"].ToString());
                    string nameitem = CatDauNgoacKep(e.data["rongquayra"][i]["nameitem"].ToString());
                    byte sao = byte.Parse(CatDauNgoacKep(e.data["rongquayra"][i]["sao"].ToString()));
                    int level = 0;int exp = 0;
                    int maxexp = int.Parse(CatDauNgoacKep(e.data["rongquayra"][i]["maxexp"].ToString()));
                    byte tienhoa = byte.Parse(CatDauNgoacKep(e.data["rongquayra"][i]["tienhoa"].ToString()));
                    int sothucan = int.Parse(CatDauNgoacKep(e.data["rongquayra"][i]["timedoi"].ToString()));
                    string tenrong = CatDauNgoacKep(e.data["rongquayra"][i]["namerong"].ToString());
                    string nameobject = CatDauNgoacKep(e.data["rongquayra"][i]["nameobject"].ToString());
                    inventory.AddItemRong(id,nameitem,sao,level,exp,maxexp,tienhoa,sothucan,tenrong,nameobject, e.data["rongquayra"][i]["hoangkim"], e.data["rongquayra"][i]["ngoc"], false);
                    // debug.Log(quayrong.RongQuayDuoc.name);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (vongquayrong.ORong[i].activeSelf)
                {
                    vongquayrong.soquay++;
                    QuayRong quayrong = vongquayrong.ORong[i].transform.GetChild(0).GetComponent<QuayRong>();
                    quayrong.enabled = true;
                }
            }
        }
        return;
    }
    IEnumerator OnThongBaoChay(string s)
    {
        if(TextThongbao.activeSelf == false)
        {
            Text txttb = TextThongbao.transform.GetChild(0).GetComponent<Text>();
            txttb.text = s;
            Thongbao.SetActive(true);
            TextThongbao.SetActive(true);
            yield return new WaitForSeconds(11);
            Thongbao.SetActive(false);
            TextThongbao.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(10);
            Text txttb = TextThongbao.transform.GetChild(0).GetComponent<Text>();
            txttb.text = s;
            Thongbao.SetActive(true);
            TextThongbao.SetActive(true);
            yield return new WaitForSeconds(11);
            Thongbao.SetActive(false);
            TextThongbao.SetActive(false);
        }
    }
    List<string> listThongBao = new List<string>();
    void OnThongBao(SocketIOEvent e)
    {
        debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        if (e.data["tb"])
        {
            string[] thongbao = e.data["tb"].ToString().Split('"');
            CrGame.ins.OnThongBao(bool.Parse(e.data["a"].ToString()), thongbao[1], bool.Parse(e.data["btok"].ToString()));
            //   if (e.data["additem"])
        }
        if (e.data["tbnhanh"])
        {
            string[] thongbao = e.data["tbnhanh"]["tb"].ToString().Split('"');
            //byte soTbOn = 0;
            listThongBao.Add(thongbao[1]);
            GameObject menu = null;
            if (AllMenu.ins.menu.ContainsKey("infoitem")) menu = AllMenu.ins.menu["infoitem"];
            else menu = AllMenu.ins.GetCreateMenu("infoitem", CrGame.ins.trencung.gameObject);
            //for (int i = 0; i < menu.transform.childCount; i++)
            //{
            //    if (menu.transform.GetChild(i).gameObject.activeSelf) soTbOn++;
            //}
            Inventory.ins.StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitUntil(()=> listThongBao[0] == thongbao[1]);
                
                // CrGame.ins.OnThongBao(bool.Parse(e.data["a"].ToString()), thongbao[1], bool.Parse(e.data["btok"].ToString()));
                CrGame.ins.OnThongBaoNhanh(thongbao[1], float.Parse(e.data["tbnhanh"]["time"].ToString()));
                yield return new WaitForSeconds(float.Parse(e.data["tbnhanh"]["time"].ToString()) - float.Parse(e.data["tbnhanh"]["time"].ToString()) / 1.4f);
                listThongBao.Remove(thongbao[1]);
            }
            //bool gettrue()
            //{
            //    return menu.activeSelf == false;
            //}
            //   if (e.data["additem"])
        }
        if (e.data["thongbaochay"])
        {
            StartCoroutine(OnThongBaoChay(CatDauNgoacKep(e.data["thongbaochay"].ToString())));
        }
        //if (e.data["catrongthanhcong"])
        //{
        //    string[] nameitem = e.data["catrongthanhcong"]["nameitem"].ToString().Split('"');
        //    string[] namerong = e.data["catrongthanhcong"]["namerong"].ToString().Split('"');
        //    string[] nameObject = e.data["catrongthanhcong"]["nameobject"].ToString().Split('"');
        //    string[] id = e.data["catrongthanhcong"]["id"].ToString().Split('"');
        //    AllMenu.ins.menu["MenuVongtronXemRong"].SetActive(false);
        //    Destroy(GameObject.Find(id[1]));
        //    inventory.AddItemRong(id[1], nameitem[1], byte.Parse(e.data["catrongthanhcong"]["sao"].ToString()),
        //        int.Parse(e.data["catrongthanhcong"]["level"].ToString()), int.Parse(e.data["catrongthanhcong"]["exp"].ToString()), int.Parse(e.data["catrongthanhcong"]["maxexp"].ToString()),
        //        byte.Parse(e.data["catrongthanhcong"]["tienhoa"].ToString()), 0, namerong[1], nameObject[1]);
        //    return;
        //}
        //if (e.data["doitenthanhcong"])
        //{
        //    string[] tenrongmoi = e.data["doitenthanhcong"].ToString().Split('"');
        //   // inventory.MenuDoiTenRong.SetActive(false);
        //    AllMenu.ins.DestroyMenu("PanelDoiTenRong");
        //    DragonController dra = CrGame.ins.TfrongInfo.gameObject.GetComponent<DragonController>();
        //    dra.tenrong = tenrongmoi[1];
        //    dra.txtnamerong.text = dra.tenrong + " (" + dra.sao + " sao)";
        //    dra.txtnamerong.gameObject.SetActive(true);
        //    return;
        //}
        if (e.data["qualevel"])
        {
            for (int i = 1; i < hopqua.Slot.transform.childCount; i++)
            {
                Destroy(hopqua.Slot.transform.GetChild(i).gameObject);
            }
            hopqua.menuNhanQua.SetActive(true);
            string[] infoqua = e.data["qualevel"].ToString().Split('"');
            hopqua.txtQua.text = infoqua[1];
            if (e.data["qua"].Count > 3)
            {
                hopqua.Thongbaoconnua.SetActive(true);
            }
            for (int i = 0; i < e.data["qua"].Count; i++)
            {
                if (e.data["qua"][i]["name"])//ITEM thuong
                {
                    string[] name = e.data["qua"][i]["name"].ToString().Split('"');
                    string[] nametv = e.data["qua"][i]["nametv"].ToString().Split('"');
                    hopqua.AddSlotQua(name[1], CatDauNgoacKep(e.data["qua"][i]["soluong"].ToString()), nametv[1]);
                }
                if (e.data["qua"][i]["namerong"])//ITEM Rong
                {
                    string[] name = e.data["qua"][i]["nameobject"].ToString().Split('"');
                    string[] nametv = e.data["qua"][i]["namerong"].ToString().Split('"');
                    hopqua.AddSlotQua(name[1], e.data["qua"][i]["sao"].ToString() + " Sao", nametv[1], true, int.Parse(e.data["qua"][i]["tienhoa"].ToString()));
                }
            
            }
            hopqua.btnNhanQua.interactable = true;
            return;
        }
        if (e.data["lencap"])
        {
            //  CrGame.ins.Exp = float.Parse(e.data["lencap"]["exp"].ToString());
            CrGame.ins.MaxExp = float.Parse(e.data["lencap"]["maxexp"].ToString());
            CrGame.ins.txtLevel.text = e.data["lencap"]["level"].ToString();
            CrGame.ins.txtThangCap.text = "Thăng Cấp " + CrGame.ins.txtLevel.text;
            StartCoroutine(CrGame.ins.offlencap(CrGame.ins.txtThangCap.text));
            if (CrGame.ins.PanelThangCap.activeSelf)
            {
               // CrGame.ins.panelThangCap.transform.GetChild(1).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                StopCoroutine(CrGame.ins.offlencap(CrGame.ins.txtThangCap.text));
            }
            CrGame.ins.SetPanelThangCap(true);
             
            float fillamount = float.Parse(e.data["lencap"]["exp"].ToString()) / (float)CrGame.ins.MaxExp;
            CrGame.ins.imgThanhLevel.fillAmount = fillamount;
            
        }
        if (e.data["updateExp"])
        {
            //  CrGame.ins.Exp = float.Parse(e.data["lencap"]["exp"].ToString());
            CrGame.ins.MaxExp = float.Parse(e.data["updateExp"]["maxexp"].ToString());
            CrGame.ins.txtLevel.text = e.data["updateExp"]["level"].ToString();
            float fillamount = (float)float.Parse(e.data["updateExp"]["exp"].ToString()) / (float)CrGame.ins.MaxExp;
            CrGame.ins.imgThanhLevel.fillAmount = fillamount;
            return;
        }
        if (e.data["nhanqua"])
        {
            hopqua.ThemQua(byte.Parse(e.data["quaconlai"].ToString()));
            for (int i = 0; i < e.data["nhanqua"]["qua"].Count; i++)
            {
                 string nametrongqua = "";
                if (e.data["nhanqua"]["qua"][i]["namerong"])//ITEM Rong
                {
                    //  debug.Log("addrong");
                    nametrongqua = CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["nameobject"].ToString());
                    inventory.AddItemRong(CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["id"].ToString()), CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["nameitem"].ToString()), byte.Parse(e.data["nhanqua"]["qua"][i]["sao"].ToString()),
                int.Parse(e.data["nhanqua"]["qua"][i]["level"].ToString()), int.Parse(e.data["nhanqua"]["qua"][i]["exp"].ToString()), int.Parse(e.data["nhanqua"]["qua"][i]["maxexp"].ToString()),
                byte.Parse(e.data["nhanqua"]["qua"][i]["tienhoa"].ToString()), 0,
                CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["namerong"].ToString()),
                CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["nameobject"].ToString()), e.data["nhanqua"]["qua"][i]["hoangkim"], e.data["nhanqua"]["qua"][i]["ngoc"],false);
                }
                else if (e.data["nhanqua"]["qua"][i]["name"] && e.data["nhanqua"]["qua"][i]["namethuyenthucan"] == false)//ITEM thuong
                {
                    string namequa = CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["name"].ToString());
                   
                    nametrongqua = namequa;
                    if(e.data["nhanqua"]["qua"][i]["name"].str != "Exp") inventory.AddItem(namequa, int.Parse(CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["soluong"].ToString())));

                }
                else if (e.data["nhanqua"]["qua"][i]["namethuyenthucan"])//ITEM ThuyenThucAn
                {
                    //  debug.Log(e.data["nhanqua"]["qua"][i]["namethuyenthucan"]);
                    string namethuyen = CatDauNgoacKep(e.data["nhanqua"]["qua"][i]["namethuyenthucan"].ToString());
                    nametrongqua = namethuyen;
                    inventory.AddThuyen(namethuyen);
                }
                //if (nametrongqua != "Exp") QuaBay(nametrongqua);
                QuaBay(nametrongqua);
                //else 
            }
            if (hopqua.Slot.transform.childCount == 1)
            {
                if (hopqua.coqua > 0 && hopqua.hienlientiep)
                {
                    socket.Emit("xemqua");
                }
            }
            return;
        }
        if (e.data["addqua"])
        {
            hopqua.ThemQua(byte.Parse(e.data["addqua"].ToString()));
            return;
        }
        //if (e.data["dropthucan"])
        //{
        //    // debug.Log(GameObject.Find("Sprite" + CatDauNgoacKep(e.data["dropthucan"]["namethucan"].ToString())).GetComponent<SpriteRenderer>().sprite);
        //    //  thuyen.StartCoroutine(thuyen.bay(GameObject.Find("Sprite" + CatDauNgoacKep(e.data["dropthucan"]["namethucan"].ToString())).GetComponent<SpriteRenderer>().sprite, int.Parse(e.data["dropthucan"]["soluong"].ToString())));
        //    string namethucan = CatDauNgoacKep(e.data["dropthucan"]["namethucan"].ToString());
        //    inventory.AddItem(namethucan, -int.Parse(e.data["dropthucan"]["soluong"].ToString()));
        //    if (namethucan == "BinhTieuHoa30p" || namethucan == "BinhTieuHoa1h" || namethucan == "BinhTieuHoa3h")
        //    {
        //        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        //        GameObject RongDao = Dao.transform.Find("RongDao").gameObject;
        //        debug.Log("obj rong dao " +RongDao.name );
        //        if(RongDao.transform.childCount > 2)
        //        {
        //            for (int i = 3; i < RongDao.transform.childCount; i++)
        //            {
        //                RongDao.transform.GetChild(i).GetComponent<DragonController>().doi = true;
        //            }
        //        }
        //        if(inventory.ListItemThuong.ContainsKey("item"+ namethucan) == false)
        //        {
        //            GameObject objtuibinhtieuhoa = CrGame.ins.tuithucAn.transform.parent.transform.parent.transform.parent.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).gameObject;
        //            objtuibinhtieuhoa.transform.Find(namethucan).gameObject.SetActive(false);
        //        }    
              
        //    }
            
        //    thuyen.StartCoroutine(thuyen.bay(Inventory.LoadSprite(namethucan), int.Parse(e.data["dropthucan"]["soluong"].ToString())));
        
        //    return;
        //}
        if (e.data["doiten"])
        {
            // CrGame.ins.menuDatten.SetActive(false);
            Destroy(AllMenu.ins.menu["MenuDatTen"]);
            AllMenu.ins.menu.Remove("MenuDatTen");
            CrGame.ins.FB_userName.text = CatDauNgoacKep(e.data["doiten"].ToString());
            GameObject canvstren = GameObject.Find("CanvasTrenCung");
            AllMenu.ins.GetCreateMenu("MenuHuongDan", canvstren, true, canvstren.transform.childCount - 1);
        }
    }

    void QuaBay(string nametrongqua)
    {
        for (int j = 1; j < hopqua.Slot.transform.childCount; j++)
        {
            if (hopqua.Slot.transform.GetChild(j).gameObject.name == "Qua" + nametrongqua)
            {
                QuaBay quabay = hopqua.Slot.transform.GetChild(j).GetComponent<QuaBay>();
                Text txtNameItem = hopqua.Slot.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                Text txtSoluongItem = hopqua.Slot.transform.GetChild(j).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
                txtNameItem.text = ""; txtSoluongItem.text = "";
                //inventory.ScaleObject(quabay.gameObject,0.85f,0.85f);
                hopqua.Slot.transform.GetChild(j).transform.SetParent(CrGame.ins.trencung.transform);
                quabay.enabled = true;
                break;
            }
           // else Destroy(hopqua.Slot.transform.GetChild(j).gameObject);
        }
    }
    public void PlayerOnline()
    {

         socket.Emit("Login", JSONObject.CreateStringObject(LoginFacebook.ins.id));
      
        
    }
    void OnlaiThanhcong(SocketIOEvent e)
    {
         debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        if(e.data["banrong"])
        {
            string Rongban = CatDauNgoacKep(e.data["banrong"].ToString());
            for (int i = 0; i < inventory.TuiRong.transform.childCount; i++)
            {
                if (inventory.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    if(inventory.TuiRong.transform.GetChild(i).transform.GetChild(0).name == Rongban)
                    {
                        Destroy(inventory.TuiRong.transform.GetChild(i).transform.GetChild(0).gameObject);
                        AllMenu.ins.menu["MenuBanRong"].SetActive(false);
                        if(AllMenu.ins.menu.ContainsKey("MenuLaiRong"))
                        {
                            if(AllMenu.ins.menu["MenuLaiRong"].activeSelf)
                            {
                                lairong.Catrongxuong(0);
                                lairong.Catrongxuong(1);
                            }
                        }
                        break;
                    }
                }
            }
            return;
        }
        if (e.data["additem"])
        {
            lairong.Lai();
            string[] nameItem = e.data["additem"]["nameitem"].ToString().Split('"');
            string[] nameObject = e.data["additem"]["nameobject"].ToString().Split('"');
            string[] nameRong = e.data["additem"]["namerong"].ToString().Split('"');
            string[] id = e.data["additem"]["id"].ToString().Split('"');
            GameObject objlaira = AllMenu.ins.menu["MenuLaiRong"].transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).gameObject;
            //Animator anim = lairong.spritelaira.GetComponent<Animator>();
            AllMenu.ins.LoadRongGiaoDien(nameObject[1] + "1", objlaira.transform.GetChild(0));
           // anim.runtimeAnimatorController = Inventory.LoadAnimator( nameObject[1]);
            lairong.idrongvualai = id[1];
            //anim.SetInteger("TienHoa", 1);
            inventory.AddItemRong(id[1], nameItem[1], byte.Parse(e.data["additem"]["sao"].ToString()), 
                int.Parse(e.data["additem"]["level"].ToString()), int.Parse(e.data["additem"]["exp"].ToString()), 
                int.Parse(e.data["additem"]["maxexp"].ToString()),
                byte.Parse(e.data["additem"]["tienhoa"].ToString()),
                float.Parse(e.data["additem"]["timedoi"].ToString()),
                e.data["additem"]["namerong"].ToString(), nameObject[1], e.data["additem"]["hoangkim"], e.data["additem"]["ngoc"], false);
            lairong.Saoronglai = byte.Parse(e.data["additem"]["sao"].ToString());
            lairong.txtTenRong.text = "<color=#ff00ffff>" + nameRong[1] + "</color><color=#ffff00ff>(" + e.data["additem"]["sao"].ToString() + "sao)</color>";
            string[] Dohiem = nameItem[1].Split('-');
            Text TxtHiem = objlaira.transform.GetChild(0).GetComponent<Text>();
            TxtHiem.text = lairong.DoHiemCuaRong(Dohiem[1]);
            string[] Nameitemtru = e.data["yeucaulai"]["nameitemcan"].ToString().Split('"');
            inventory.AddItem(Nameitemtru[1], -int.Parse(e.data["yeucaulai"]["gia"].ToString()));
            int sohuyentinh = inventory.GetItem("HuyenTinh");
            if(sohuyentinh >= int.Parse(e.data["giachucphuc"].ToString()))
            {
                lairong.btnChucPhuc.SetActive(true);
            }

            else lairong.btnChucPhuc.SetActive(false);
            lairong.txtSoitemCo.text = e.data["giachucphuc"].ToString() + "/" + sohuyentinh;

            for (int i = 0; i < e.data["chiso"].Count; i++)
            {
                if (e.data["chiso"][i]["phantramvang"])
                {
                    string cong = CatDauNgoacKep(e.data["chiso"][i]["phantramvang"]["cong"].ToString());
                    lairong.HienChiSo("phantramvang", "+" + cong + "%");
                }
                else if (e.data["chiso"][i]["giamthoigian"])
                {
                    string cong = CatDauNgoacKep(e.data["chiso"][i]["giamthoigian"]["cong"].ToString());
                    lairong.HienChiSo("giamthoigian","-"+ cong + "s");
                }
                else if (e.data["chiso"][i]["rotvatpham"])
                {
                    string cong = CatDauNgoacKep(e.data["chiso"][i]["rotvatpham"]["cong"].ToString());
                    lairong.HienChiSo("rotvatpham", "+" + cong + "%");
                }
                else if (e.data["chiso"][i]["satthuong"])
                {
                    string cong = CatDauNgoacKep(e.data["chiso"][i]["satthuong"]["cong"].ToString());
                    lairong.HienChiSo("satthuong", "+" +cong);
                }
                else if (e.data["chiso"][i]["phantramexp"])
                {
                    string cong = CatDauNgoacKep(e.data["chiso"][i]["phantramexp"]["cong"].ToString());
                    lairong.HienChiSo("phantramexp", "+" + cong + "%");
                }
            }

            return;
        }
        if(e.data["chucphuc"])
        {
            lairong.imgitemGiaLai.gameObject.SetActive(false);
            lairong.banRongCatRong.SetActive(false);
            lairong.btnExit.SetActive(false);
            lairong.nutcatlai[0].interactable = false; lairong.nutcatlai[1].interactable = false;
            inventory.AddItem("HuyenTinh",-int.Parse(e.data["chucphuc"]["truhuyentinh"].ToString()));
            lairong.HieuUngChucPhuc.SetActive(true);
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(5.6f);
                lairong.nutcatlai[0].interactable = true; lairong.nutcatlai[1].interactable = true;
                lairong.SaoRong.transform.GetChild(lairong.Saoronglai).gameObject.SetActive(true);
                lairong.HieuUngChucPhuc.SetActive(false);
                lairong.txtTenRong.text = "<color=#ff00ffff>" + CatDauNgoacKep(e.data["chucphuc"]["rong"].ToString()) + "</color><color=#ffff00ff>(" + (lairong.Saoronglai + 1) + "sao)</color>";
                lairong.banRongCatRong.SetActive(true);
                lairong.btnChucPhuc.SetActive(false);
                lairong.btnExit.SetActive(true);
            }
            for (int i = 0; i < e.data["chucphuc"]["chiso"].Count; i++)
            {
                if (e.data["chucphuc"]["chiso"][i]["phantramvang"])
                {
                    string cong = CatDauNgoacKep(e.data["chucphuc"]["chiso"][i]["phantramvang"]["cong"].ToString());
                    lairong.HienChiSo("phantramvang", "+" + cong + "%");
                }
                else if (e.data["chucphuc"]["chiso"][i]["giamthoigian"])
                {
                    string cong = CatDauNgoacKep(e.data["chucphuc"]["chiso"][i]["giamthoigian"]["cong"].ToString());
                    lairong.HienChiSo("giamthoigian", "-" + cong + "s");
                }
                else if (e.data["chucphuc"]["chiso"][i]["rotvatpham"])
                {
                    string cong = CatDauNgoacKep(e.data["chucphuc"]["chiso"][i]["rotvatpham"]["cong"].ToString());
                    lairong.HienChiSo("rotvatpham", "+" + cong + "%");
                }
                else if (e.data["chucphuc"]["chiso"][i]["satthuong"])
                {
                    string cong = CatDauNgoacKep(e.data["chucphuc"]["chiso"][i]["satthuong"]["cong"].ToString());
                    lairong.HienChiSo("satthuong", "+" + cong);
                }
                else if (e.data["chucphuc"]["chiso"][i]["phantramexp"])
                {
                    string cong = CatDauNgoacKep(e.data["chucphuc"]["chiso"][i]["phantramexp"]["cong"].ToString());
                    lairong.HienChiSo("phantramexp", "+" + cong + "%");
                }
            }

            for (int i = 0; i < inventory.TuiRong.transform.childCount; i++)
            {
                if(inventory.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    if (inventory.TuiRong.transform.GetChild(i).transform.GetChild(0).name == lairong.idrongvualai)
                    {
                        ItemDragon idra = inventory.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                        idra.txtSao.text = (lairong.Saoronglai + 1) + "";
                        break;
                    }    
                }
            }
            lairong.banRongCatRong.transform.GetChild(0).GetComponent<Button>().interactable = true;
            lairong.banRongCatRong.transform.GetChild(1).GetComponent<Button>().interactable = true;
            lairong.banRongCatRong.transform.GetChild(2).GetComponent<Button>().interactable = true;
        }
        if (e.data["dotrangtri"])
        {
            //GameObject.Find(CatDauNgoacKep(e.data["dotrangtri"][0].ToString())
            GameObject trangtri = Instantiate(Inventory.LoadObjectResource("GameData/Item/" + CatDauNgoacKep(e.data["dotrangtri"][0].ToString())) as GameObject, new Vector3(float.Parse(CatDauNgoacKep(e.data["dotrangtri"][1].ToString())), float.Parse(CatDauNgoacKep(e.data["dotrangtri"][2].ToString())),0), Quaternion.identity) as GameObject;
            Destroy(trangtri.GetComponent<ItemTrangTri>());
            Destroy(trangtri.GetComponent<EventTrigger>());
            Destroy(trangtri.GetComponent<Button>());
            trangtri.GetComponent<Image>().enabled = false;
            GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
            trangtri.transform.SetParent(CrGame.ins.FindObject(Dao, "ObjectTrangTri").transform);
            inventory.ScaleObject(trangtri, 0.52f, 0.5100001f);
            trangtri.transform.GetChild(1).gameObject.SetActive(true);
            trangtri.transform.GetChild(0).gameObject.SetActive(false);
           // debug.LogError(CrGame.ins.khungAvatar.GetComponent<Image>().sprite.name.Substring(6));
            if(CatDauNgoacKep(e.data["dotrangtri"][0].ToString()) == "TocLenh")
            {
                trangtri.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator("TocLenh" + CrGame.ins.khungAvatar.GetComponent<Image>().sprite.name.Substring(6));
            }
            inventory.AddItem(CatDauNgoacKep(e.data["dotrangtri"][0].ToString()), -1);
            return;
        }
        if (e.data["cattrangtri"])
        {
            string[] vitri = CatDauNgoacKep(e.data["cattrangtri"].ToString()).Split('+');
            GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
            Destroy(CrGame.ins.FindObject(Dao, "ObjectTrangTri").transform.GetChild(int.Parse(CatDauNgoacKep(vitri[1]))).gameObject);
            inventory.AddItem(vitri[2], 1);
            return;
        }
        if (e.data["addslottui"])
        {
            inventory.addMaxSlot(CatDauNgoacKep(e.data["addslottui"].ToString()));
            return;
        }
        if (e.data["nangcapdaothanhcong"])
        {
            GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
            Image btnnangcapdao = CrGame.ins.FindObject(Dao, "btnCapDao").GetComponent<Image>();
            StartCoroutine(CreateHieuUng());
            IEnumerator CreateHieuUng()
            {
                //CrGame.ins.MenuNangCapItem.SetActive(false);
              //  AllMenu.ins.menu["MenuNangCapItem"].SetActive(false);
                AllMenu.ins.DestroyMenu("MenuNangCapItem");
                GameObject hieuung = Instantiate(Inventory.ins.GetObj("HieuUng1"), new Vector3(btnnangcapdao.transform.position.x, btnnangcapdao.transform.position.y + 0.5f), Quaternion.identity) as GameObject;
                Vector3 Scale; Scale = hieuung.transform.localScale;
                Scale.x = 1; Scale.y = 1.1f; hieuung.transform.localScale = Scale;
                hieuung.SetActive(true);
                CrGame.ins.OnThongBao(false); CrGame.ins.leveldao[CrGame.ins.DangODao] += 1;//vienchinh.MaxDoiHinh += 1;
                yield return new WaitForSeconds(1.5f);
                btnnangcapdao.sprite = Inventory.LoadSprite("Dao" + CrGame.ins.leveldao[CrGame.ins.DangODao]); btnnangcapdao.SetNativeSize();
                Destroy(hieuung);
            }
            return;
        }
    }
    void GetitemLai(SocketIOEvent e)
    {
      //  debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        // debug.Log(e.data["ItemrongLai"][0]["nameitem"]);
        if(e.data["itemlai"])
        {
            lairong = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
            lairong.txtChiSo.text = CatDauNgoacKep(e.data["itemlai"]["ItemrongLai"][0].ToString());
            for (int i = 1; i < e.data["itemlai"]["ItemrongLai"].Count; i++)
            {
                string[] id = e.data["itemlai"]["ItemrongLai"][i]["id"].ToString().Split('"');
                string[] nameObject = e.data["itemlai"]["ItemrongLai"][i]["nameobject"].ToString().Split('"');
                string[] nameItem = e.data["itemlai"]["ItemrongLai"][i]["nameitem"].ToString().Split('"');
                string[] herong = e.data["itemlai"]["ItemrongLai"][i]["he"].ToString().Split('"');
                string[] Gen;
                if (e.data["itemlai"]["ItemrongLai"][i]["gen"].ToString() != "")
                {
                    Gen = e.data["itemlai"]["ItemrongLai"][i]["gen"].ToString().Split('"');
                }
                else
                {
                    Gen = new string[2] { "", "" };
                }
                string[] hiem = nameItem[1].Split('-');
                lairong.LoadItem(nameItem[1], int.Parse(e.data["itemlai"]["ItemrongLai"][i]["sao"].ToString()), herong[1], Gen[1], id[1], nameObject[1], int.Parse(e.data["itemlai"]["ItemrongLai"][i]["tienhoa"].ToString()), hiem[1]);
                // string[] nameItem = e.data["ItemrongLai"][i]["nameitem"].ToString().Split('"');
                // nameRong[i] = nameItem[1];
                //  saoRong[j] = int.Parse(e.data["ItemrongLai"][i]["sao"].ToString());
            }
        }
        if (e.data["quai"])
        {
           // GameObject prefab = vienchinh.transform.GetChild(1).gameObject;
            StartCoroutine (delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(2);
                for (int i = 0; i < e.data["quai"].Count; i++)
                {
                    for (int n = 0; n < int.Parse(e.data["quai"][i]["soluong"].ToString()); n++)
                    {
                        string namequai = CatDauNgoacKep(e.data["quai"][i]["name"].ToString());
                        //debug.Log("quai ok 0 ");
                        GameObject instance = Instantiate(Resources.Load("GameData/QuaiVienChinh/" + namequai) as GameObject, new Vector3(vienchinh.TruDo.transform.position.x + 400, vienchinh.TruDo.transform.position.y + Random.Range(-200, -50)), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(vienchinh.TeamDo.transform, false);
                        DragonPVEController chiso = instance.transform.Find("SkillDra").GetComponent<DragonPVEController>();
                        int level = int.Parse(CrGame.ins.txtLevel.text);
                        if (namequai != "TinhTinhLuaDo" && namequai != "BossHalloween")
                        {
                            chiso.dame = float.Parse(e.data["quai"][i]["dame"].ToString()) * level;
                            chiso.hp = float.Parse(e.data["quai"][i]["hp"].ToString()) * level;
                            chiso.Maxhp = chiso.hp;
                        }
                        else
                        {
                            chiso.dame = float.Parse(e.data["quai"][i]["dame"].ToString());
                            chiso.hp = float.Parse(e.data["quai"][i]["hp"].ToString());
                            chiso.Maxhp = chiso.hp;
                            instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y + 2, instance.transform.position.z);
                        }
                     //   debug.Log("quai ok 1");
                        chiso.speed = float.Parse(GamIns.CatDauNgoacKep(e.data["quai"][i]["speed"].ToString()));

                        chiso._HutHp = float.Parse(e.data["quai"][i]["hutmau"].ToString());
                        chiso.netranh = float.Parse(e.data["quai"][i]["netranh"].ToString());
                        chiso._ChiMang = float.Parse(e.data["quai"][i]["tilechimang"].ToString());
                     //   debug.Log("quai ok 2");
                        instance.GetComponent<DraUpdateAnimator>().CreateChamDo();
                      //  debug.Log("quai ok 3");
                        instance.SetActive(true);
                 
                      //  debug.Log("quai ok");
                        yield return new WaitForSeconds(1);
                    }    
                  

                    //    for (int j = 0; j < prefab.transform.childCount; j++)
                    //    {
                    //        if (prefab.transform.GetChild(j).name == namequai)
                    //        {
                    //            for (int n = 0; n < int.Parse(e.data["quai"][i]["soluong"].ToString()); n++)
                    //            {
                    //                GameObject instance = Instantiate(prefab.transform.GetChild(j).gameObject, new Vector3(vienchinh.TruDo.transform.position.x + 600, vienchinh.TruDo.transform.position.y + Random.Range(-200, -50)), Quaternion.identity) as GameObject;
                    //                instance.transform.SetParent(GameObject.Find("TeamDo").transform, false);
                    //                ChiSo chiso = instance.GetComponent<ChiSo>();
                    //                int level = int.Parse(CrGame.ins.txtLevel.text);
                    //                if(namequai != "TinhTinhLuaDo" && namequai != "BossHalloween")
                    //                {
                    //                    chiso.dame = float.Parse(e.data["quai"][i]["dame"].ToString()) * level;
                    //                    chiso.hp = float.Parse(e.data["quai"][i]["hp"].ToString()) * level;
                    //                    chiso.Maxhp = float.Parse(e.data["quai"][i]["hp"].ToString()) * level;
                    //                }    
                    //                else
                    //                {
                    //                    chiso.dame = float.Parse(e.data["quai"][i]["dame"].ToString());
                    //                    chiso.hp = float.Parse(e.data["quai"][i]["hp"].ToString());
                    //                    chiso.Maxhp = float.Parse(e.data["quai"][i]["hp"].ToString());
                    //                    instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y + 100, instance.transform.position.x);
                    //                }
                    //                chiso.speed = float.Parse(e.data["quai"][i]["speed"].ToString());

                    //                chiso.huthp = float.Parse(e.data["quai"][i]["hutmau"].ToString());
                    //                chiso.netranh = float.Parse(e.data["quai"][i]["netranh"].ToString());
                    //                chiso.chimang = float.Parse(e.data["quai"][i]["tilechimang"].ToString());

                    //                instance.SetActive(true);
                    //                yield return new WaitForSeconds(1);
                    //            }
                    //            break;
                    //        }
                    //    }
                }
            }
        }
        debug.Log("[SocketIO] rong chinh chien: " + e.name + " " + e.data);
        if (e.data["rongchinhchien"])
        {
            MenuDoiHinh doihinh = AllMenu.ins.menu["MenuDoiHinh"].GetComponent<MenuDoiHinh>();

            for (int i = 0; i < doihinh.ContentDoiHinh.transform.childCount; i++)
            {
                if (doihinh.ContentDoiHinh.transform.GetChild(i).childCount > 0)
                {
                    Destroy(doihinh.ContentDoiHinh.transform.GetChild(i).transform.GetChild(0).gameObject);
                }
            }
            for (int i = 0; i < doihinh.ContentItem.transform.childCount; i++)
            {
                Destroy(doihinh.ContentItem.transform.GetChild(i).gameObject);
            }
            // doihinh.Xoahet();
            // doihinh.MaxDoiHinh = int.Parse(e.data["rongchinhchien"]["maxdoihinh"].ToString());
            //  debug.Log("maxdoihinh " + int.Parse(e.data["rongchinhchien"]["maxdoihinh"].ToString()));
            if(doihinh.ContentItem.transform.childCount == 0)
            {
                doihinh.TaoSlotTrong(int.Parse(e.data["rongchinhchien"]["maxdoihinh"].ToString()));
            }
            else
            {
                for (int i = 0; i < int.Parse(e.data["rongchinhchien"]["maxdoihinh"].ToString()); i++)
                {
                    GameObject otrong = Instantiate(doihinh.oxanhtrong, doihinh.ContentItem.transform.position, Quaternion.identity) as GameObject;
                    otrong.transform.SetParent(doihinh.ContentItem.transform.transform, false);
                    otrong.SetActive(true);
                }
            }
            for (int i = 0; i < e.data["rongchinhchien"]["doihinh"].Count; i++)
            {
                if(e.data["rongchinhchien"]["doihinh"][i]["nameobject"])
                {
                    byte sao = byte.Parse(e.data["rongchinhchien"]["doihinh"][i]["sao"].ToString());
                    Sprite sprite = Inventory.LoadSpriteRong(CatDauNgoacKep(e.data["rongchinhchien"]["doihinh"][i]["nameobject"].ToString()) + e.data["rongchinhchien"]["doihinh"][i]["tienhoa"].ToString(), sao);
                    string id = CatDauNgoacKep(e.data["rongchinhchien"]["doihinh"][i]["id"].ToString());
                    doihinh.LoadDoihinh(id, sprite, e.data["rongchinhchien"]["doihinh"][i]["sao"].ToString(), i);
                }    
            }
            for (int i = 0; i < e.data["rongchinhchien"]["allrong"].Count; i++)
            {
                if (e.data["rongchinhchien"]["allrong"][i]["nameobject"])
                {
                    byte sao = byte.Parse(e.data["rongchinhchien"]["allrong"][i]["sao"].ToString());
                    // doihinh.soitemrong = e.data["rongchinhchien"]["allrong"].Count;
                    Sprite sprite = Inventory.LoadSpriteRong(CatDauNgoacKep(e.data["rongchinhchien"]["allrong"][i]["nameobject"].ToString()) + e.data["rongchinhchien"]["allrong"][i]["tienhoa"].ToString(), sao);
                    string id = CatDauNgoacKep(e.data["rongchinhchien"]["allrong"][i]["id"].ToString());
                    doihinh.LoadRong(id, sprite, e.data["rongchinhchien"]["allrong"][i]["sao"].ToString());
                }
            }
        }
        if (e.data["editdoihinh"])
        {
            MenuDoiHinh doihinh = AllMenu.ins.menu["MenuDoiHinh"].GetComponent<MenuDoiHinh>();
            if (e.data["editdoihinh"]["them"])
            {
                doihinh.AddDoiHinh();
            }
            if (e.data["editdoihinh"]["xoa"])
            {
                doihinh.Xoa1Rong();
            }
            if (e.data["editdoihinh"]["save"]) 
            {
                int j = 0;
                for (int i = 0; i < doihinh.ContentDoiHinh.transform.childCount;i++)
                {
                    if (doihinh.ContentDoiHinh.transform.GetChild(j).childCount == 0)
                    {
                       doihinh.ContentDoiHinh.transform.GetChild(j).transform.SetSiblingIndex(doihinh.ContentDoiHinh.transform.childCount - 1);
                    }
                    else
                    {
                        j++;
                    }
                }
            }
            if (e.data["editdoihinh"]["xoahet"])
            {
                 //  doihinh.Xoahet();
                // doihinh.TaoSlotTrong(int.Parse(e.data["editdoihinh"]["maxdoihinh"].ToString()));
                for (int i = 0; i < doihinh.ContentDoiHinh.transform.childCount; i++)
                {
                    if (doihinh.ContentDoiHinh.transform.GetChild(i).childCount > 0)
                    {
                        Destroy(doihinh.ContentDoiHinh.transform.GetChild(i).transform.GetChild(0).gameObject);
                    }
                }
                for (int i = 0; i < doihinh.ContentItem.transform.childCount; i++)
                {
                    Destroy(doihinh.ContentItem.transform.GetChild(i).gameObject);
                }
                //for (int i = 0; i < int.Parse(e.data["editdoihinh"]["maxdoihinh"].ToString()); i++)
                //{
                //    GameObject otrong = Instantiate(doihinh.oxanhtrong, doihinh.ContentItem.transform.position, Quaternion.identity) as GameObject;
                //    otrong.transform.SetParent(doihinh.ContentItem.transform.transform, false);
                //    otrong.SetActive(true);
                //}
                for (int i = 0; i < e.data["editdoihinh"]["xoahet"].Count; i++)
                {
                    //doihinh.soitemrong = e.data["editdoihinh"]["xoahet"].Count;
                    Sprite sprite = Inventory.LoadSpriteRong(CatDauNgoacKep(e.data["editdoihinh"]["xoahet"][i]["nameobject"].ToString()) + e.data["editdoihinh"]["xoahet"][i]["tienhoa"].ToString());
                    string id = CatDauNgoacKep(e.data["editdoihinh"]["xoahet"][i]["id"].ToString());
                    doihinh.LoadRong(id, sprite, e.data["editdoihinh"]["xoahet"][i]["sao"].ToString());
                }
            }
        }
    }

    void LoginSuccess(SocketIOEvent e)/////////////////////////////////////////////////////////
    {
       debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        Time.timeScale = 1;
        if (e.data["trangthai"])
        {
            string[] status = e.data["trangthai"].ToString().Split('"');
            CrGame.ins.OnThongBao(true,status[1]);
            if (status[1] == "Tài khoản đang đăng nhập ở nơi khác") socket.Close();
            StartCoroutine(delayy());
            IEnumerator delayy()
            {
                yield return new WaitForSeconds(10);
                debug.Log("Quit game");
                Application.Quit();
            }
        }
        else
        {
            if (LoginFacebook.ins.isLogin == false)
            {
             //   int sodao = CrGame.ins.AllDao.transform.childCount;
                CrGame.ins.leveldao = new byte[CrGame.ins.sodao];
                LoginFacebook.ins.isLogin = true;
                CrGame.ins.menulogin.SetActive(true);
                  Image maskload = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
                  Text txtload = CrGame.ins.menulogin.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

                //  for (int i = 0; i < e.data["dao"].Count; i++)
#if UNITY_EDITOR
                if (EditorScript.editorGetData)
                {
                    debug.Log("Load tu editor");
                    string assetBundleDirectory;

#if UNITY_EDITOR_WIN
                    assetBundleDirectory = @"C:\Dao Rong Mobile Asset GitHub\AssetsBundles\Android";
#elif UNITY_EDITOR_OSX
    assetBundleDirectory = "/Users/nguyenngochoangduong/DRUnityIOS/AssetsBundles/IOS";
#else
    assetBundleDirectory = "Unknown OS";///
#endif

                    debug.Log("Asset Bundle Directory: " + assetBundleDirectory);

                    // Combine the directory path with the bundle name
                    string bundlePath = Path.Combine(assetBundleDirectory, "alldragon");

                    // Load the AssetBundle
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);

                    if (assetBundle == null)
                    {
                        debug.LogError("Failed to load AssetBundle!");
                        return;
                    }
                    DownLoadAssetBundle.bundleDragon = assetBundle;
                    StartCoroutine(delayParse());
                }
                else DownLoadData();

#elif UNITY_ANDROID || UNITY_IOS
                DownLoadData();

#endif
                void DownLoadData()
                {
                    float process = 50;
                    StartCoroutine(DownLoadAllRong());
                    IEnumerator DownLoadAllRong()
                    {
                        //  string nameasset = "manh" + gameObject.name.ToLower();
                        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(DownLoadAssetBundle.linkdown + "alldragon");
                        // DownloadHandler handle = www.downloadHandler;
                        www.SendWebRequest();
                        while (!www.isDone)
                        {
                            process = 50 + (www.downloadProgress * 100f) / 2;
                            debug.Log("Downloading... " + process + "%");
                            txtload.text = "Đang tải dữ liệu: " + System.Math.Round(process, 2) + "%";
                            maskload.fillAmount = (float)process / 100;
                            yield return new WaitForSeconds(.01f);
                        }
                        if (www.result != UnityWebRequest.Result.Success)
                        {
                            debug.Log(www.error);
                        }
                        else
                        {
                            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                            DownLoadAssetBundle.bundleDragon = bundle;
                            // bundle.Unload()
                            int process2 = Mathf.FloorToInt(process);
                            for (int i = process2; i < 99; i++)
                            {
                                process += 1;
                                txtload.text = "Đang tải dữ liệu: " + System.Math.Round(process, 2) + "%";
                                maskload.fillAmount = (float)process / 100;

                                yield return new WaitForSeconds(0.01f);

                            }
                            StartCoroutine(delayParse());
                        }
                    }
                }
                //  StartCoroutine(DownLoadAssetBundle.ins.DownLoadAllRong());

                IEnumerator delayParse()
                {
                    yield return new WaitUntil(()=>DownLoadAssetBundle.bundleDragon != null);
                    for (int i = 0; i < 1; i++)
                    {
                        GameObject dao = CrGame.ins.AllDao.transform.Find("BGDao" + i).gameObject;

                        CrGame.ins.leveldao[i] = byte.Parse(CatDauNgoacKep(e.data["dao"][i]["leveldao"].ToString()));
                        Image imgCapDao = CrGame.ins.FindObject(dao, "btnCapDao").GetComponent<Image>();
                        imgCapDao.sprite = Inventory.LoadSprite("Dao" + CrGame.ins.leveldao[i]);
                        imgCapDao.SetNativeSize();
                        GameObject congtrinhdao = CrGame.ins.FindObject(dao, "ObjectCongtrinh");
                        for (int j = 0; j < e.data["dao"][i]["congtrinh"].Count; j++)
                        {
                            CongTrinh ct = congtrinhdao.transform.GetChild(j).GetComponent<CongTrinh>();
                            string[] namect = e.data["dao"][i]["congtrinh"][j]["name"].ToString().Split('"');
                            byte capct = byte.Parse(e.data["dao"][i]["congtrinh"][j]["cap"].ToString());
                            ct.nameCongtrinh = namect[1]; ct.levelCongtrinh = capct; //ct.LoadImg();
                            if (i == 0) ct.LoadImg();

                         
                        }

                        if (e.data["dao"][i]["timeNuiThanBi"])
                        { 
                            debug.Log("namengoc: " + GamIns.CatDauNgoacKep(e.data["dao"][i]["namengoc"].ToString()));
                            CrGame.ins.timeNuiThanBi = float.Parse(e.data["dao"][i]["timeNuiThanBi"].ToString());
                                         NuiLua.Instance.MauNgoc = (_mauNgoc)Enum.Parse(typeof(_mauNgoc), GamIns.CatDauNgoacKep(e.data["dao"][i]["namengoc"].ToString()));
                           // debug.Log("Time nui than bi dao " + i + ": " + CrGame.ins.timeNuiThanBi);
                        }


                        CrGame.ins.OnThongBao(true, "Đang lấy dữ liệu Rồng...", false);
                        //
                        DragonIslandManager.DungThucAn = dao.transform.Find("ThucAn").gameObject;
                        for (int j = 0; j < e.data["dao"][i]["rong"].Count; j++)
                        {
                            // JSONNode newjson = 
                            //   debug.LogError("new json " + test);
                          //  debug.Log(JSON.Parse(e.data["dao"][i]["rong"][j].ToString().ToString()));
                            DragonIslandManager.ParseDragonIsland(JSON.Parse(e.data["dao"][i]["rong"][j].ToString()), 0);

                        }
                        if(e.data["dao"][i]["itemEvent"]) DragonIslandManager.InsAllItemDao(JSON.Parse(e.data["dao"][i]["itemEvent"].ToString()), 0);

                        //if (e.data["dao"][i]["itemEvent"])
                        //{
                        //    for (int j = 0; j < e.data["dao"][i]["itemEvent"].Count; j++)
                        //    {
                        //        DragonIslandManager.InsItemDao(JSON.Parse(e.data["dao"][i]["itemEvent"][j].ToString()),0);
                        //    }
                        //}
                        CrGame.ins.OnThongBao(true, "Đang lấy dữ liệu Trang trí", false);
                        GameObject objecttrangtridao = CrGame.ins.FindObject(dao, "ObjectTrangTri");
                        if (e.data["dao"][i]["trangtri"].Count > 0)
                        {
                            // debug.Log("trang tri  ok 1");
                            for (int j = 0; j < e.data["dao"][i]["trangtri"].Count; j++)
                            {
                                // string nametrangtri = CatDauNgoacKep(e.data["dao"][i]["trangtri"][j]["name"].ToString());
                                GameObject trangtri = Instantiate(Inventory.LoadObjectResource("GameData/Item/" + CatDauNgoacKep(e.data["dao"][i]["trangtri"][j]["name"].ToString())) as GameObject, transform.position, Quaternion.identity) as GameObject;
                                // debug.Log("trang tri dao " + i + " " + j);
                                Destroy(trangtri.GetComponent<ItemTrangTri>());
                                //   debug.Log("trang tri dao " + i + " " + j + " 1");
                                Destroy(trangtri.GetComponent<EventTrigger>());
                                //   debug.Log("trang tri dao " + i + " " + j + " 2");

                                Destroy(trangtri.GetComponent<Button>());
                                //   debug.Log("trang tri dao " + i + " " + j + " 3");

                                trangtri.transform.SetParent(objecttrangtridao.transform);
                                //  debug.Log("trang tri dao " + i + " " + j + " 4");
                                float x = 0, y = 0;
                                float.TryParse(GamIns.CatDauNgoacKep(e.data["dao"][i]["trangtri"][j]["x"].ToString(),true),out x);
                                float.TryParse(GamIns.CatDauNgoacKep(e.data["dao"][i]["trangtri"][j]["y"].ToString(),true),out y);
                                trangtri.transform.position = new Vector3(x, y, 0);
                                trangtri.transform.GetChild(1).gameObject.SetActive(true);
                                // debug.Log("trang tri dao " + i + " " + j + " 5");
                                trangtri.GetComponent<Image>().enabled = false;
                                trangtri.transform.GetChild(1).transform.position = new Vector3(trangtri.transform.GetChild(1).transform.position.x, trangtri.transform.GetChild(1).transform.position.y, 0);
                                trangtri.transform.GetChild(0).gameObject.SetActive(false);
                                if (CatDauNgoacKep(e.data["dao"][i]["trangtri"][j]["name"].ToString()) == "TocLenh")
                                {
                                    trangtri.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator("TocLenh" + CatDauNgoacKep(e.data["coban"]["toc"].ToString()));
                                }
                            }
                        }
                        for (int j = 0; j < e.data["dao"][i]["vangroi"].Count; j++)
                        {
                            Vector3 tf = new Vector3(float.Parse(e.data["dao"][i]["vangroi"][j]["x"].ToString()), float.Parse(e.data["dao"][i]["vangroi"][j]["y"].ToString()));
                            GameObject vang = Instantiate(Inventory.ins.GetObj("vangroi"), tf, Quaternion.identity) as GameObject;
                            vang.transform.SetParent(CrGame.ins.FindObject(dao, "itemroi").transform);
                            vang.SetActive(true);
                        }
                    }//GetData Công Trình
                    inventory.LoadSlotRong(int.Parse(e.data["item"]["itemrong"][0]["maxslot"].ToString()));
                    //    debug.LogError(e.data["item"]["itemthuong"][0]);
                    inventory.LoadSlotTuiThuong(int.Parse(e.data["item"]["itemthuong"][0]["maxslot"].ToString()) - 4);
                    debug.Log("test haha");
                    CrGame.ins.OnThongBao(true, "Đang tải item...", false);
                    for (int i = 1; i < e.data["item"]["itemrong"].Count; i++)
                    {
                        debug.Log("itemrong" + i);
                        string[] nameitem = e.data["item"]["itemrong"][i]["nameitem"].ToString().Split('"');
                        string[] namerong = e.data["item"]["itemrong"][i]["namerong"].ToString().Split('"');
                        string[] nameObject = e.data["item"]["itemrong"][i]["nameobject"].ToString().Split('"');
                        string[] id = e.data["item"]["itemrong"][i]["id"].ToString().Split('"');
                        bool lockk = false;
                        if (e.data["item"]["itemrong"][i]["lock"])
                        {
                            lockk = bool.Parse(e.data["item"]["itemrong"][i]["lock"].ToString());
                        }    
                        inventory.AddItemRongByindex(i - 1, id[1], nameitem[1], byte.Parse(e.data["item"]["itemrong"][i]["sao"].ToString()),
                            int.Parse(e.data["item"]["itemrong"][i]["level"].ToString()), int.Parse(e.data["item"]["itemrong"][i]["exp"].ToString()), int.Parse(e.data["item"]["itemrong"][i]["maxexp"].ToString()),
                            byte.Parse(e.data["item"]["itemrong"][i]["tienhoa"].ToString()), 0, namerong[1], nameObject[1], e.data["item"]["itemrong"][i]["hoangkim"], e.data["item"]["itemrong"][i]["ngoc"],lockk);
                    }//GetData Item Rồng
                    for (int i = 0; i < e.data["item"]["itemthucan"].Count; i++)
                    {
                        debug.Log("item thuc an " + i);
                        inventory.addMaxSlot("tuithuong");
                        string[] nameThucAn = e.data["item"]["itemthucan"][i]["name"].ToString().Split('"');

                        if (int.Parse(e.data["item"]["itemthucan"][i]["soluong"].ToString()) > 0)
                        {
                        
                            inventory.AddItem(nameThucAn[1], int.Parse(e.data["item"]["itemthucan"][i]["soluong"].ToString()));
                            for (int j = 0; j < CrGame.ins.tuithucAn.transform.childCount; j++)
                            {
                                if (nameThucAn[1] == CrGame.ins.tuithucAn.transform.GetChild(j).name)
                                {
                                    CrGame.ins.tuithucAn.transform.GetChild(j).gameObject.SetActive(true);
                                    ThucAn thucAn = CrGame.ins.tuithucAn.transform.GetChild(j).transform.GetChild(0).gameObject.GetComponent<ThucAn>();
                                    thucAn.levelThucAn = int.Parse(e.data["item"]["itemthucan"][i]["level"].ToString());
                                }
                            }
                        }
                    }
                    //GetData Thức ăn
                    Image imgthuyen = thuyen.ThuyenObject.transform.GetChild(0).GetComponent<Image>();
                    imgthuyen.sprite = Inventory.LoadSprite(CatDauNgoacKep(e.data["coban"]["tauthucan"]["taumacdinh"].ToString()));
                    imgthuyen.SetNativeSize();
                    thuyen.ThuyenObject.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(CatDauNgoacKep(e.data["coban"]["tauthucan"]["taumacdinh"].ToString()));
                    for (int i = 0; i < e.data["coban"]["tauthucan"]["alltau"].Count; i++)
                    {
                        for (int j = 0; j < thuyen.contentTau.transform.childCount; j++)
                        {
                            if (CatDauNgoacKep(e.data["coban"]["tauthucan"]["alltau"][i].ToString()) == thuyen.contentTau.transform.GetChild(j).name)
                            {
                                thuyen.contentTau.transform.GetChild(j).gameObject.SetActive(true);
                                debug.Log(e.data["coban"]["tauthucan"]["alltau"][i].ToString());
                                if (thuyen.contentTau.transform.GetChild(j).name == CatDauNgoacKep(e.data["coban"]["tauthucan"]["taumacdinh"].ToString())) thuyen.contentTau.transform.GetChild(j).transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                            }
                        }
                    }
                    CrGame.ins.OnThongBao(true, "Đang tải itemm...", false);
                    for (int i = 1; i < e.data["item"]["itemthuong"].Count; i++)
                    {
                       // debug.Log(e.data["item"]["itemthuong"][i]["soluong"].ToString());
                        string[] nameitem = e.data["item"]["itemthuong"][i]["name"].ToString().Split('"');
                        double number = double.Parse(e.data["item"]["itemthuong"][i]["soluong"].ToString());
                        int result = (int)number;
                        debug.Log(nameitem[1]);
                        inventory.AddItem(nameitem[1], result);
                        if (nameitem[1] == "BinhTieuHoa30p")
                        {
                            GameObject objtuibinhtieuhoa = CrGame.ins.tuithucAn.transform.parent.transform.parent.transform.parent.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).gameObject;
                            objtuibinhtieuhoa.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        if (nameitem[1] == "BinhTieuHoa1h")
                        {
                            GameObject objtuibinhtieuhoa = CrGame.ins.tuithucAn.transform.parent.transform.parent.transform.parent.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).gameObject;
                            objtuibinhtieuhoa.transform.GetChild(1).gameObject.SetActive(true);
                        }
                        if (nameitem[1] == "BinhTieuHoa3h")
                        {
                            GameObject objtuibinhtieuhoa = CrGame.ins.tuithucAn.transform.parent.transform.parent.transform.parent.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).gameObject;
                            objtuibinhtieuhoa.transform.GetChild(2).gameObject.SetActive(true);
                        }
                        if (nameitem[1] == "GiapKimLoai-" || nameitem[1] == "GiapKimLoai-CuongHoa" || nameitem[1] == "GiapDo-" || nameitem[1] == "GiapDo-CuongHoa" || nameitem[1] == "GiapNgocXanh-" || nameitem[1] == "GiapNgocXanh-CuongHoa")
                        {
                            int sl = int.Parse(e.data["item"]["itemthuong"][i]["soluong"].ToString());
                            for (int j = 0; j < sl; j++)
                            {
                                inventory.AddItemGiap(nameitem[1]);
                            }

                        }
                    }

                    for (int i = 0; i < e.data["item"]["itemngoc"].Count; i++)
                    {
                        inventory.AddNgoc(CatDauNgoacKep(e.data["item"]["itemngoc"][i]["namengoc"].ToString()), int.Parse(e.data["item"]["itemngoc"][i]["soluong"].ToString()));
                    }
                    CrGame.ins.OnThongBao(true, "Đang lấy danh sách bạn bè...", false);

                    //for (int i = 1; i < e.data["friend"]["listFriend"].Count; i++)
                    //{
                    //    GameObject Ofriend = Instantiate(friend.Ofriend, friend.ContentFriend.transform.position, Quaternion.identity) as GameObject;
                    //    Ofriend.transform.SetParent(friend.ContentFriend.transform, false);
                    //    Image Avatar = Ofriend.transform.GetChild(0).GetComponent<Image>();
                    //    btnFriend btnfr = Ofriend.GetComponent<btnFriend>();
                    //    btnfr.idfb = CatDauNgoacKep(e.data["friend"]["listFriend"][i]["idfb"].ToString());
                    //    btnfr.idObjectFriend = CatDauNgoacKep(e.data["friend"]["listFriend"][i]["objectId"].ToString());
                    //    Ofriend.name = btnfr.idObjectFriend;
                    //    Text txtname = Ofriend.transform.GetChild(2).GetComponent<Text>();
                    //    txtname.text = CatDauNgoacKep(e.data["friend"]["listFriend"][i]["name"].ToString());
                    //    if (txtname.text.Length > 8)
                    //    {
                    //        string newname = txtname.text.Substring(0, 8) + "...";
                    //        txtname.text = newname;
                    //    }
                    //    friend.GetAvatarFriend(CatDauNgoacKep(e.data["friend"]["listFriend"][i]["idfb"].ToString()),Avatar);
                    //    Image Khung = Ofriend.transform.GetChild(1).GetComponent<Image>();
                    //    Khung.sprite = Inventory.LoadSprite("Avatar" + CatDauNgoacKep(e.data["friend"]["listFriend"][i]["toc"].ToString()));
                    //    Ofriend.SetActive(true);
                    //}
                    // nhiemvu//////////////////////
                    Nhiemvu.ContentNVHangNgay.transform.GetChild(4).GetComponent<Text>().text = "Nhiệm vụ hằng ngày " + CatDauNgoacKep(CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["SoNhiemVu"].ToString()));
                    if (CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["allNhiemvu"].ToString()) != "Khoa")
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            string nv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["allNhiemvu"][j]["namenhiemvu"].ToString());
                            string sonv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["allNhiemvu"][j]["dalam"].ToString());
                            string maxnv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["allNhiemvu"][j]["maxnhiemvu"].ToString());
                            string keynv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["allNhiemvu"].keys[j]);
                            Nhiemvu.AddinfoNhiemVuHangNgay(j, nv, sonv + "/" + maxnv, keynv);
                        }
                        for (int i = 0; i < e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["QuaNhanDuoc"].Count; i++)
                        {
                            Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
                            string namequa = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["QuaNhanDuoc"][i]["namequa"].ToString());
                            string soluong = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["QuaNhanDuoc"][i]["soluong"].ToString());
                            if (namequa != "Exp")
                            {
                                Image imgqua = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                                imgqua.sprite = Inventory.LoadSprite(namequa);
                                imgqua.SetNativeSize();
                            }
                            Text txtSoLuong = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                            txtSoLuong.text = soluong;
                        }
                        for (int i = 0; i < e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["CoTheNhanDuoc"].Count; i++)
                        {
                            Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i + 2).gameObject.SetActive(true);
                            string namequa = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["CoTheNhanDuoc"][i]["namequa"].ToString());
                            string soluong = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["CoTheNhanDuoc"][i]["soluong"].ToString());
                            if (namequa != "Exp")
                            {
                                Image imgqua = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(0).GetComponent<Image>();
                                imgqua.sprite = Inventory.LoadSprite(namequa);
                                imgqua.SetNativeSize();
                            }
                            Text txtSoLuong = Nhiemvu.QuaNhanHangNgay.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(1).GetComponent<Text>();
                            txtSoLuong.text = soluong;
                        }
                    }
                    else
                    {
                        GameObject khoa = Nhiemvu.ContentNVHangNgay.transform.GetChild(Nhiemvu.ContentNVHangNgay.transform.childCount - 1).gameObject;
                        khoa.SetActive(true);
                        string gia = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuhangngay"]["giaMokhoaNhiemVu"].ToString());
                        if (gia != "Khoa")
                        {
                            khoa.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = gia;
                        }
                        else
                        {
                            khoa.transform.GetChild(1).gameObject.SetActive(false);
                            khoa.transform.GetChild(2).GetComponent<Text>().text = "Đã hoàn thành hết nhiệm vụ trong ngày";
                        }
                    }
                    Nhiemvu.ContentNvRong.transform.GetChild(4).GetComponent<Text>().text = "Nhiệm vụ rồng " + CatDauNgoacKep(CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["SoNhiemVu"].ToString()));
                    if (CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["allNhiemvu"].ToString()) != "Khoa")
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            string nv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["allNhiemvu"][j]["namenhiemvu"].ToString());
                            string sonv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["allNhiemvu"][j]["dalam"].ToString());
                            string maxnv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["allNhiemvu"][j]["maxnhiemvu"].ToString());
                            string keynv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["allNhiemvu"].keys[j]);
                            Nhiemvu.AddinfoNhiemVuRong(j, nv, sonv + "/" + maxnv, keynv);
                        }
                        for (int i = 0; i < e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["QuaNhanDuoc"].Count; i++)
                        {
                            Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
                            string namequa = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["QuaNhanDuoc"][i]["namequa"].ToString());
                            string soluong = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["QuaNhanDuoc"][i]["soluong"].ToString());
                            if (namequa != "Exp")
                            {
                                Image imgqua = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                                imgqua.sprite = Inventory.LoadSprite(namequa);
                                imgqua.SetNativeSize();
                            }
                            Text txtSoLuong = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                            txtSoLuong.text = soluong;
                        }
                        for (int i = 0; i < e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["CoTheNhanDuoc"].Count; i++)
                        {
                            Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i + 2).gameObject.SetActive(true);
                            string namequa = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["CoTheNhanDuoc"][i]["namequa"].ToString());
                            string soluong = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["CoTheNhanDuoc"][i]["soluong"].ToString());
                            if (namequa != "Exp")
                            {
                                Image imgqua = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(0).GetComponent<Image>();
                                imgqua.sprite = Inventory.LoadSprite(namequa);
                                imgqua.SetNativeSize();
                            }
                            Text txtSoLuong = Nhiemvu.QuaNhanRong.transform.GetChild(0).transform.GetChild(i + 2).transform.GetChild(1).GetComponent<Text>();
                            txtSoLuong.text = soluong;
                        }
                    }
                    else
                    {
                        //Nhiemvu.ContentNvRong.transform.GetChild(Nhiemvu.ContentNvRong.transform.childCount - 1).gameObject.SetActive(true);
                        GameObject khoa = Nhiemvu.ContentNvRong.transform.GetChild(Nhiemvu.ContentNvRong.transform.childCount - 1).gameObject;
                        khoa.SetActive(true);
                        string gia = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvurong"]["giaMokhoaNhiemVu"].ToString());
                        if (gia != "Khoa")
                        {
                            khoa.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = gia;
                        }
                        else
                        {
                            khoa.transform.GetChild(1).gameObject.SetActive(false);
                            khoa.transform.GetChild(2).GetComponent<Text>().text = "Đã hoàn thành hết nhiệm vụ trong ngày";
                        }
                    }
                    for (int i = 0; i < e.data["coban"]["nhiemvuhangngay"]["nhiemvuexp"]["allNhiemvu"].Count; i++)
                    {
                        string nv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuexp"]["allNhiemvu"][i]["namenhiemvu"].ToString());
                        string sonv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuexp"]["allNhiemvu"][i]["dalam"].ToString());
                        string maxnv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuexp"]["allNhiemvu"][i]["maxnhiemvu"].ToString());
                        string keynv = CatDauNgoacKep(e.data["coban"]["nhiemvuhangngay"]["nhiemvuexp"]["allNhiemvu"].keys[i]);
                        Nhiemvu.AddinfoNhiemVuExp(nv, sonv + "/" + maxnv, keynv);
                    }
                    ///////////////////////
                    debug.Log("fix1");
                    hopqua.ThemQua(int.Parse(e.data["coban"]["hopqua"].Count.ToString()));
                    debug.Log("fix2");
                    CrGame.ins.txtVang.text = CatDauNgoacKep(e.data["vangformat"].ToString());
                    debug.Log("fix3");
                    CrGame.ins.txtKimCuong.text = CatDauNgoacKep(e.data["coban"]["kimcuong"].ToString()); debug.Log("fix4");
                    CrGame.ins.txtDanhVong.text = CatDauNgoacKep(e.data["coban"]["DanhVong"].ToString());
                    CrGame.ins.txtLevel.text = e.data["coban"]["level"].ToString(); debug.Log("fix5");
                    float Maxexp = float.Parse(e.data["coban"]["maxexp"].ToString()); debug.Log("fix6");
                    float fillamount = float.Parse(e.data["coban"]["exp"].ToString()) / Maxexp; CrGame.ins.imgThanhLevel.fillAmount = fillamount; debug.Log("fix7");
                    //CrGame.ins.Exp = float.Parse(e.data["coban"]["exp"].ToString());
                    LevelNhaAp = int.Parse(e.data["coban"]["levelnhaaptrung"].ToString()); debug.Log("fix8");
                    // lairong.LoadImage();
                    // SpriteRong spritect = GameObject.Find("SpriteNhaApTrung").GetComponent<SpriteRong>();
                    Image imgNHaApTrung = GameObject.Find("btnLaiRong").GetComponent<Image>(); debug.Log("fix9");
                    StartCoroutine(LoadSpriteNhApTrung(imgNHaApTrung, LevelNhaAp / 5));
                    //imgNHaApTrung.sprite = spritect.spriteTienHoa[LevelNhaAp / 5];
                    CrGame.ins.MaxExp = Maxexp;

                    //     CrGame.ins.khungAvatar.sprite = Inventory.LoadSprite("Avatar"+CatDauNgoacKep(e.data["coban"]["toc"].ToString()));//Load("Sprite/Avatar" + CatDauNgoacKep(e.data["coban"]["toc"].ToString())) as Sprite;
                    socket.Emit("Gettime");
                    //for (int i = vienchinh.LevelVienChinh.transform.childCount - 1; i > e.data["coban"]["levelphoban"].Count + 1; i--)
                    //{
                    //    Button btn = vienchinh.LevelVienChinh.transform.GetChild(i).GetComponent<Button>();
                    //    btn.interactable = false;
                    //}
                    //qua friend
                    hopqua.SoquaDanhan = byte.Parse(e.data["friend"]["quafriend"]["soquadanhan"].ToString());
                    hopqua.SoquaFriend = (byte)e.data["friend"]["quafriend"]["qua"].Count;
                    hopqua.LoadQuaFriend(); bool moichoi = true;
                    string namehienthi = CatDauNgoacKep(e.data["tenhienthi"].ToString());
                    CrGame.ins.FB_userName.text = namehienthi;
                    LoginFacebook.ins.nameNv = namehienthi;
                    if (namehienthi == "")
                    {
                        GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Menu/MenuDatTen") as GameObject, transform.position, Quaternion.identity) as GameObject; ;
                        mn.transform.SetParent(AllMenu.ins.transform, false);
                        AllMenu.ins.menu.Add("MenuDatTen", mn);
                        moichoi = false;
                    }
                    else
                    {
                        GetComponent<ZoomCamera>().enabled = true;
                    }
                    CrGame.ins.OnThongBao(false);
                    //      Destroy(CrGame.ins.menulogin);
                    CrGame.ins.menulogin.SetActive(false);
                    if (e.data["coban"]["quatanghangngay"].Count > 0)
                    {
                        AllMenu.ins.OpenCreateMenu("menuQuaHangNgay", null, moichoi);
                        QuaTangHangNgay quatanghangngay = AllMenu.ins.menu["menuQuaHangNgay"].GetComponent<QuaTangHangNgay>();
                        quatanghangngay.txtSoQua.text = "Hãy chọn " + e.data["coban"]["quatanghangngay"].Count + " món quà";
                        quatanghangngay.soqua = (byte)e.data["coban"]["quatanghangngay"].Count;
                    }
                    friend.LoadImage("khungavt", CatDauNgoacKep(e.data["coban"]["khungavtsudung"].ToString()), CrGame.ins.khungAvatar);
                    friend.LoadImage("avt", CatDauNgoacKep(e.data["coban"]["avtsudung"].ToString()), CrGame.ins.FB_useerDp);
                    //    friend.LoadImage("avt", CatDauNgoacKep(e.data["coban"]["avtsudung"].ToString()),CrGame.ins.FB_useerDp);
                    AudioManager.SetSoundBg("nhacnen0");

                    //for (int i = 0; i < 2; i++)
                    //{
                    //    int index = i; // Đảm bảo biến i không bị thay đổi trong lambda
                    //    System.Threading.Tasks.Task.Run(() =>
                    //    {
                    //        socket.Emit("test", JSONObject.CreateStringObject("test lan thu " + index));
                    //    });
                    //}
                    //Destroy(GameObject.Find("SoundBg"));
                }
            }
            else
            {
                if (vienchinh.dangdau)
                {
                    vienchinh.Thua("Trận đấu bị gián đoạn do mạng không ổn định");
                }
                socket.Emit("QuaDao", JSONObject.CreateStringObject(CrGame.ins.DangODao.ToString()));
                socket.Emit("Gettime");
            }
        }
    }
    IEnumerator LoadSpriteNhApTrung(Image img, int cap)
    {
        WWW www = new WWW(CrGame.ins.ServerName + "image/name/NhaApTrung/cap/" + cap);
        yield return www;
        img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        img.SetNativeSize();
    }
    void Nangctthanhcong(SocketIOEvent e)
    {
        //debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        if(e.data["nhaaptrung"])
        {
            CrGame.ins.OnThongBao(true, CatDauNgoacKep(e.data["nhaaptrung"]["trangthai"].ToString()), true);
            if (CatDauNgoacKep(e.data["nhaaptrung"]["trangthai"].ToString()) == "thành công")
            {
                StartCoroutine(CreateHieuUngNhaAp());
            }
        }
        if(e.data["congtrinh"])
        {
            string[] status = e.data["congtrinh"]["trangthai"].ToString().Split('"');
            if (status[1] == "thành công")
            {
                // CrGame.ins.txtVang.text = CatDauNgoacKep(e.data["vang"].ToString());
                CongTrinh ct = CrGame.ins.VungCongTrinh.GetComponent<CongTrinh>();
                StartCoroutine(CreateHieuUngCongtrinh(ct, CrGame.ins.nameCongtrinh));
                CrGame.ins.OnThongBao(false);
                AllMenu.ins.DestroyMenu("MenuNangCapItem");
            }
            else
            {
                CrGame.ins.OnThongBao(true, status[1], true);
            }
        }
        //CrGame.ins.OnThongBao(false);
    }
    public string CatDauNgoacKep(string s)
    {
        string[] cat = s.Split('"');
        if(cat.Length > 1)
        {
            return cat[1];
        }
        else
        {
            return cat[0];
        }
    }

    IEnumerator CreateHieuUngNhaAp()
    {
        GameObject NHaAPTrung = GameObject.Find("btnLaiRong");
        GameObject hieuung = Instantiate(Inventory.ins.GetObj("HieuUng1"), new Vector3(NHaAPTrung.transform.position.x, NHaAPTrung.transform.position.y), Quaternion.identity) as GameObject;
        Vector3 Scale; Scale = hieuung.transform.localScale;
        Scale.x = 1; Scale.y = 1.1f; hieuung.transform.localScale = Scale;
        LevelNhaAp += 1;
        hieuung.SetActive(true);
        AllMenu.ins.menu["MenuLaiRong"].SetActive(false);
        //lairong.menuLai.SetActive(false);
        CrGame.ins.OnThongBao(false);
        AllMenu.ins.DestroyMenu("MenuNangCapItem");
        yield return new WaitForSeconds(1.5f);
       // SpriteRong spritect = GameObject.Find("SpriteNhaApTrung").GetComponent<SpriteRong>();
        Image imgNHaApTrung = NHaAPTrung.GetComponent<Image>();
       StartCoroutine( LoadSpriteNhApTrung(imgNHaApTrung, LevelNhaAp / 5));
        Destroy(hieuung);
    }
    IEnumerator CreateHieuUngCongtrinh(CongTrinh ct,string namect)
    {
        //CongTrinh ct = CrGame.ins.VungCongTrinh.GetComponent<CongTrinh>();
        GameObject hieuung = Instantiate(Inventory.ins.GetObj("HieuUng1"), new Vector3(ct.transform.position.x, ct.transform.position.y + 1f), Quaternion.identity) as GameObject;
        Vector3 Scale; Scale = hieuung.transform.localScale;
        Scale.x = 1; Scale.y = 1.1f; hieuung.transform.localScale = Scale;

        hieuung.SetActive(true);
        ct.levelCongtrinh += 1; CrGame.ins.OnThongBao(false);
        ct.nameCongtrinh = namect;
        AllMenu.ins.DestroyMenu("menuMuaCongtrinh");
        AllMenu.ins.DestroyMenu("menuShopCongTrinh");
        yield return new WaitForSeconds(1.5f);
        ct.LoadImg();
        Destroy(hieuung);
    }
    void ConnectError(SocketIOEvent e)
    {
        // debug.Log("[SocketIO] Open received: " + e.name);
        debug.Log("Mất kết nối tới máy chủ");
        debug.Log(emit);
        if (emit)
        {
            emit = false;
            socket.Close();
            CrGame.ins.OnThongBao(true, "Đang kết nối...");
           // Time.timeScale = 0;
            StartCoroutine(matketnoi());
        
            //Invoke("Time0", 10f);
        }    
        
    }
    IEnumerator matketnoi()
    {
        yield return new WaitForSeconds(5);
        Application.LoadLevel(0);
    }
    void Time0()
    {
         Time.timeScale = 0;
        //Application.LoadLevel(0);
    }
    bool login = false,emit = false;
    void ConnectSuccess(SocketIOEvent e)
    {
        debug.Log("[SocketIO] Open received: " + e.name);
        //CrGame.ins.OnThongBao(false);
        if(emit == false)
        {
            socket.Emit("Login", JSONObject.CreateStringObject(LoginFacebook.ins.id));
            login = true;emit = true;
        }
    }
    void RongDoi(SocketIOEvent e)
    {
        debug.Log("[SocketIO] RongDoi: " + e.name);
        if (e.data["status"])
        {
            DragonIslandManager.RongDoi(JSON.Parse(e.data.ToString()));
        }
        if(e.data["vangroi"])
        {
            debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
            int dao = int.Parse(CatDauNgoacKep(e.data["vangroi"]["dao"].ToString()));
            string id = CatDauNgoacKep(e.data["vangroi"]["idrong"].ToString());
            GameObject Rong = CrGame.ins.FindRongDao(dao, id);
            Vector3 tf = new Vector3();
            if(Rong.activeInHierarchy == false)
            {
                tf = new Vector3(int.Parse(e.data["vangroi"]["x"].ToString()), int.Parse(e.data["vangroi"]["y"].ToString()));
            }
            else
            {
                tf = Rong.transform.position;
            }
            GameObject daoo = CrGame.ins.AllDao.transform.Find("BGDao" + dao).gameObject;
            GameObject vang = Instantiate(Inventory.ins.GetObj("vangroi"), tf, Quaternion.identity) as GameObject;
            vang.transform.SetParent(CrGame.ins.FindObject(daoo, "itemroi").transform);
            vang.SetActive(true);
        }
    }
    void ThaRongOk(SocketIOEvent e)
    {
        //if (e.data["tharongok"])
        //{
        //    string[] id = e.data["tharongok"]["id"].ToString().Split('"');
        //    ItemDragon itemdra = GameObject.Find(id[1]).GetComponent<ItemDragon>();
        //    Destroy(itemdra.gameObject);
        //    //debug.Log("[SocketIO] Open received: " + e.name + " " + e.data["tharongok"]);
        //    string[] namerong = e.data["tharongok"]["namerong"].ToString().Split('"');
        //    string[] nameitemrong = e.data["tharongok"]["nameitem"].ToString().Split('"');
        //    string[] nameGameobject = e.data["tharongok"]["nameobject"].ToString().Split('"');
        //    GameObject rong = null;//Instantiate(SGResources.Load("GameData/Menu/Rong/" + nameGameobject[1]) as GameObject, inventory.targetTharong, Quaternion.identity) as GameObject;
        //    rong = Instantiate(inventory.ObjectRong(nameGameobject[1]), inventory.targetTharong, Quaternion.identity) as GameObject;
        //    Animator anim = rong.GetComponent<Animator>();
        //    if (anim.runtimeAnimatorController == null)
        //    {
        //        rong.GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(nameGameobject[1]);//SGResources.Load<RuntimeAnimatorController>( nameGameobject[1]);//SGResources.Load( s) as RuntimeAnimatorController;
        //    }
        //    rong.name = id[1];
        //    Destroy(rong.GetComponent<ChiSo>());
        //    GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        //    rong.transform.SetParent(CrGame.ins.FindObject(Dao, "RongDao").transform);
        //    DragonController dracontroller = rong.GetComponent<DragonController>();
        //    dracontroller.tenrong = namerong[1];
        //    dracontroller.sao = byte.Parse(e.data["tharongok"]["sao"].ToString());
        //    dracontroller.Exp = int.Parse(e.data["tharongok"]["exp"].ToString());
        //    dracontroller.maxExp = int.Parse(e.data["tharongok"]["maxexp"].ToString());
        //    dracontroller.tienhoa = byte.Parse(e.data["tharongok"]["tienhoa"].ToString());
        //    dracontroller.Levelrong = byte.Parse(e.data["tharongok"]["level"].ToString());
        //    if (bool.Parse(e.data["tharongok"]["chiso"]["hienthiten"].ToString()))
        //    {
        //        dracontroller.txtnamerong.text = dracontroller.tenrong + " (" + dracontroller.sao + " sao)";
        //        dracontroller.txtnamerong.gameObject.SetActive(true);
        //    }
        //    Destroy(rong.GetComponent<ChiSo>());
        //    rong.SetActive(true);
        //}
        if (e.data["openmenudoirong"])
        {
            GameObject menuDoiRong = inventory.menuTuiDo.transform.GetChild(2).gameObject;
            if (menuDoiRong.activeSelf == false)
            {
                for (var i = 0; i < 9; i++)
                {
                    if (menuDoiRong.transform.GetChild(i).transform.childCount > 0) Destroy(menuDoiRong.transform.GetChild(i).transform.GetChild(0).gameObject);
                    else break;
                }

                for (int i = 0; i < e.data["openmenudoirong"].Count; i++)
                {
                    string[] nameitem = e.data["openmenudoirong"][i]["nameitem"].ToString().Split('"');
                    string[] namerong = e.data["openmenudoirong"][i]["namerong"].ToString().Split('"');
                    string[] nameObject = e.data["openmenudoirong"][i]["nameobject"].ToString().Split('"');
                    string[] id = e.data["openmenudoirong"][i]["id"].ToString().Split('"');

                    GameObject itemRong = Instantiate(inventory.ItemRong, menuDoiRong.transform.GetChild(i).transform.position, Quaternion.identity) as GameObject;
                    itemRong.transform.SetParent(menuDoiRong.transform.GetChild(i).transform);
                    itemRong.name = "rongdoi-" + id[1];
                    Image imgRong = itemRong.transform.GetChild(0).GetComponent<Image>();
                    string[] Dohiemcuarong = nameitem[1].Split('-');
                    imgRong.sprite = Inventory.LoadSpriteRong(nameObject[1] + e.data["openmenudoirong"][i]["tienhoa"].ToString(),byte.Parse(e.data["openmenudoirong"][i]["sao"].ToString()));
                    if (Dohiemcuarong[1] != "")
                    {
                        Text txtHiem = imgRong.transform.GetChild(0).GetComponent<Text>();
                        txtHiem.text = Inventory.DoHiemCuaRong(Dohiemcuarong[1]);
                    }
                    imgRong.SetNativeSize();
                    ItemDragon idra = itemRong.GetComponent<ItemDragon>();
                    idra.txtSao.text = e.data["openmenudoirong"][i]["sao"].ToString();
                    inventory.ScaleObject(itemRong, 1, 1);
                    itemRong.SetActive(true);
                    Destroy(idra);
                }
                menuDoiRong.SetActive(true);
            }
        }
        if (e.data["DoiRong"])
        {
            debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
            GameObject rongdoi = inventory.menuTuiDo.transform.GetChild(2).gameObject;
            for (var i = 0; i < 9; i++)
            {
                if (rongdoi.transform.GetChild(i).transform.childCount > 0) Destroy(rongdoi.transform.GetChild(i).transform.GetChild(0).gameObject);
                else break;
            }
            Transform tfRongCat = CrGame.ins.FindRongDao(CrGame.ins.DangODao, CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["id"].ToString())).transform;
            Vector3 vecrongcat = tfRongCat.transform.position;
            Destroy(tfRongCat.gameObject);
            // Destroy(inventory.TuiRong.transform.Find(CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["id"].ToString())));


            string idrongcat = CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["id"].ToString());
            string nameitem = CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["nameitem"].ToString());
            byte sao = byte.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["sao"].ToString()));
            int level = int.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["level"].ToString()));
            int exp = int.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["exp"].ToString()));
            int maxexp = int.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["maxexp"].ToString()));
            byte tienhoa = byte.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["tienhoa"].ToString()));
            string tenrong = CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["namerong"].ToString());
            string nameobj = CatDauNgoacKep(e.data["DoiRong"]["RongCat"]["nameobject"].ToString());

            GameObject itemrongxoa = GameObject.Find(CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["id"].ToString()));
            inventory.AddItemRong(idrongcat, nameitem, sao, level, exp, maxexp, tienhoa, 0, tenrong, nameobj, e.data["DoiRong"]["RongCat"]["hoangkim"], e.data["DoiRong"]["RongCat"]["ngoc"],false);
            //inventory.AddItemRongByindex(itemrongxoa.transform.parent.GetSiblingIndex(), idrongcat, nameitem, sao, level, exp, maxexp, tienhoa, 0, tenrong, nameobj);
            Destroy(itemrongxoa);

            //string nameobjtha = CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["nameobject"].ToString());
            //string idrongtha = CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["id"].ToString());
            //string nameitemtha = CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["nameitem"].ToString());
            //byte saotha = byte.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["sao"].ToString()));
            //byte leveltha = byte.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["level"].ToString()));
            //int exptha = int.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["exp"].ToString()));
            //int maxexptha = int.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["maxexp"].ToString()));
            //byte tienhoatha = byte.Parse(CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["tienhoa"].ToString()));
            //string tenrongtha = CatDauNgoacKep(e.data["DoiRong"]["RongTha"]["namerong"].ToString());

          //  GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;

            //GameObject rong = Instantiate(inventory.ObjectRong(nameobjtha), new Vector3(Dao.transform.position.x - Random.Range(-3, 3), Dao.transform.position.y + Random.Range(-2.5f, 2.5f), 0), Quaternion.identity) as GameObject;
            //Animator anim = rong.GetComponent<Animator>();
            //if (anim.runtimeAnimatorController == null)
            //{
            //    rong.GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(nameobjtha);//SGResources.Load<RuntimeAnimatorController>( nameobject[1]);
            //}
            //GameObject ChuaRongDao = CrGame.ins.FindObject(Dao, "RongDao");
            //rong.transform.SetParent(ChuaRongDao.transform);
            //rong.name = idrongtha;

            //DragonController dra = rong.GetComponent<DragonController>();
            //dra.sao = saotha; dra.Exp = exptha; dra.maxExp = maxexptha;
            //// dra.sothucan = itemrong.sothucAn;
            //dra.tienhoa = tienhoatha; dra.Levelrong = leveltha;
            //dra.tenrong = tenrongtha;
            //if (bool.Parse(e.data["DoiRong"]["RongTha"]["chiso"]["hienthiten"].ToString()))
            //{
            //    dra.txtnamerong.text = dra.tenrong + " (" + dra.sao + " sao)";
            //    dra.txtnamerong.gameObject.SetActive(true);
            //}
            //if (bool.Parse(e.data["DoiRong"]["RongTha"]["chiso"]["chientuong"].ToString()))
            //{
            //    //DragonIslandManager.SetChienTuong();
            //    GameObject NewObj = new GameObject();
            //    inventory.ScaleObject(NewObj, 0.0035f, 0.0035f);
            //    NewObj.transform.position = dra.txtnamerong.transform.position;
            //    Image SpriteChienTuong = NewObj.AddComponent<Image>();
            //    SpriteChienTuong.sprite = vienchinh.ChienTuongVang; SpriteChienTuong.SetNativeSize();
            //    NewObj.transform.SetParent(rong.transform.Find("Canvas").transform);
            //    NewObj.name = "iconchientuong";
            //    DragonIslandManager.ChienTuong = NewObj;
            //}
            //rong.gameObject.SetActive(true);
            DragonIslandManager.ParseDragonIsland(JSON.Parse(e.data["DoiRong"]["RongTha"].ToString()), CrGame.ins.DangODao, vecrongcat);
            if(AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            rongdoi.SetActive(false);
        }
    }
    void DcThuHoach(SocketIOEvent e)
    {
        debug.Log("[SocketIO] dc thu hoach: " + e.name + " " + e.data);
        int dao = int.Parse(e.data["dao"].ToString());
        //  GameObject congtrinhdao = GameObject.Find("ObjectCongtrinh" + e.data["dao"].ToString());
        GameObject daoo = CrGame.ins.AllDao.transform.Find("BGDao"+ dao).gameObject;
   //     debug.Log(daoo.name);
        CongTrinh ct = CrGame.ins.FindObject(daoo, "ObjectCongtrinh").transform.GetChild(int.Parse(e.data["idcongtrinh"].ToString())).GetComponent<CongTrinh>();//congtrinhdao.transform.GetChild(int.Parse(e.data["idcongtrinh"].ToString())).GetComponent<CongTrinh>();
                                                                                                                                                            // ct.DuocThuHoach(int.Parse(e.data["dao"].ToString()));
      //  debug.LogError("xem thu hoach " + ct.Xemthuhoach);
        if (ct.Xemthuhoach == false)
        {
            ct.dcthuhoach = Instantiate(Inventory.ins.GetEffect("DuocThuHoach"), new Vector3(ct.transform.position.x, ct.transform.position.y - 0.3f), Quaternion.identity) as GameObject;
            ct.dcthuhoach.transform.SetParent(daoo.transform);
            Vector3 Scale;
            Scale = ct.dcthuhoach.transform.localScale;
            Scale.x = 0.1f; Scale.y = 0.1f;
            ct.dcthuhoach.transform.localScale = Scale;
            ct.thuhoach = true;
        }
    }
    public void quaCongtrinh(SocketIOEvent e)
    {
       debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        // inventory.AddItem("ThachAnh", int.Parse(e.data["ThachAnh"].ToString()));
        if(e.data["quacongtrinh"])
        {
            AudioManager.PlaySound("sound1");
            CongTrinh congtrinh = CrGame.ins.VungCongTrinh.GetComponent<CongTrinh>();
            Destroy(congtrinh.dcthuhoach);
            congtrinh.thuhoach = false;
            inventory.AddItem(CatDauNgoacKep(e.data["quacongtrinh"]["name"].ToString()), int.Parse(e.data["quacongtrinh"]["soluong"].ToString()));
        }
        //if (e.data["idrong"])
        //{
        //    string[] idrong = e.data["idrong"].ToString().Split('"');
        //    DragonController dracontroller = CrGame.ins.FindRongDao(CrGame.ins.DangODao,idrong[1]).GetComponent<DragonController>();//GameObject.Find(idrong[1]).GetComponent<DragonController>();
        //    int exprongcong = int.Parse(e.data["exprongcong"].ToString());
        //    int exprong = int.Parse(e.data["exprong"].ToString());
        //    int levelrong = int.Parse(e.data["levelrong"].ToString());
        //    dracontroller.ThuongAn(int.Parse(e.data["exp"].ToString()), exprongcong);

        //    dracontroller.LoadExpRong(exprong, int.Parse(e.data["maxexprong"].ToString()),levelrong, byte.Parse(e.data["tienhoa"].ToString()));
        //    dracontroller.sao = byte.Parse(e.data["sao"].ToString());
        //}
    }
}
[SerializeField]
public class jsonitemRong
{
    public string nameItem,tenrong;
    public byte Sao;
    public byte levelrong;
    public int exp,maxexp;
    public byte tienhoa;
    public float sothucAn;
    public string timetieuhoa;
    public static jsonitemRong CreateFromJson(string data)
    {
        return JsonUtility.FromJson<jsonitemRong>(data);
    }
}
