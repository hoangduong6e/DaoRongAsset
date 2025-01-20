using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System;
using System.Net;
using Unity.VisualScripting.Antlr3.Runtime;
//using SIDGIN.Patcher.SceneManagment;
//using SIDGIN.Patcher.Client;


//public class TestGenerics<T>
//{
//    private T value;
//    public TestGenerics(T v)
//    {
//        value = v;
//    }
//}
public class LoginFacebook : MonoBehaviour
{
    public string id,matkhau;public bool isLogin = false;
    public GameObject ThongBao;public Text txtThongbao,txtLoad;public Image maskload;
    public string nameNv;public string NameServer;
    public GameObject btnOk,MenuLogin,ThanhLoad,MenuChon,btnLoginThuong;bool duocdangnhap = true;
    public GameObject loginthuong,allloginthuong,panelloaddao;
    public string ServerChinh;
    public static LoginFacebook ins;
    public static string token = "";
    public static string thisServer = "";
#if UNITY_EDITOR && UNITY_EDITOR_OSX
// Chỉ Unity Editor trên macOS
public static string http = "http";
#elif UNITY_IOS || UNITY_ANDROID
    // Khi build trên iOS hoặc Android
    public static string http = "https";
#else
// Các trường hợp còn lại (bao gồm Unity Editor trên Windows)
public static string http = "https";
#endif


    public string keyAes
    {
        get
        {
            string substring = token.Substring(0, 10 - NetworkManager.ins.solanrequest.ToString().Length);

            return AesEncryption.Keyyy + substring + NetworkManager.ins.solanrequest;
        }
    }
    public string IVAes
    {
        get
        {
            string substring = token.Substring(0, 10 - NetworkManager.ins.solanrequest.ToString().Length);

            return AesEncryption.IVvvv + substring + NetworkManager.ins.solanrequest;
        }
    }
    // public static string ws { get {return (http == "https") } };
    private void Awake()
    {
        //  LoadAllServer();
#if UNITY_EDITOR_OSX
        if (ServerChinh.Contains("4444"))
        {
            ServerChinh = "localhost:4444";//
           // if(EditorScript.ngrok)
            //{
             //   http = "https";
             //   ServerChinh = "fa59-2402-800-61ee-2e81-4d34-6316-5906-54e2.ngrok-free.app";
           // }
            debug.Log("http la " + http);
        }
#elif UNITY_EDITOR_WIN
      if (ServerChinh.Contains("4444"))
        {
            ServerChinh = "183.80.188.231:4444";
        }
#endif

        DontDestroyOnLoad(this.gameObject);
        ins = this;
        NameServer = ServerChinh;
        Application.runInBackground = true;

        // testdownassetsbundle();

        //TestGenerics<GameObject> newgameobj = new TestGenerics<GameObject>(transform.gameObject);
 //       ScaleSpriteToScreen.ScaleSprite(GameObject.Find("BGLanSu_00000").GetComponent<SpriteRenderer>(),GameObject.Find("Canvasmenu").transform);
    }

    private void Start()
    {
        LoadAllServer();
        Application.targetFrameRate = 90;
    
    }
    void PanelLoadDao(bool active)
    {
       panelloaddao.SetActive(active);
    }
    void DangNhapThanhCong()
    {
        if (id == "testadmin")
        {
            btnLoginThuong.transform.parent.transform.GetChild(6).transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            btnLoginThuong.transform.parent.transform.GetChild(6).transform.GetChild(4).gameObject.SetActive(false);
        }
        debug.Log("Login ok");
    }
    public void XoaTk()
    {
        PanelLoadDao(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://" + ServerChinh + "/xoataikhoan/taikhoan/" + id + "/matkhau/" + matkhau);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Không thể kết nối đến máy chủ!"; btnOk.SetActive(true);
                PanelLoadDao(false);
                btnLoginThuong.SetActive(false);
                btnOk.GetComponent<Button>().onClick.AddListener(ThoatGame);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    DangXuatChonServer();
                    ThongBao.SetActive(true);
                    txtThongbao.text = "Tài khoản này đã bị xóa";
                    btnOk.SetActive(true);
                    GameObject.Find("menuxoatk").gameObject.SetActive(false);
                }
                panelloaddao.SetActive(false);
            }
        }

    }
    public void OpenMenuThongBao()
    {
        GameObject trencung = GameObject.FindGameObjectWithTag("trencung").gameObject;
        if(trencung.transform.Find("menuthongbao"))
        {
            trencung.transform.Find("menuthongbao").gameObject.SetActive(true);
        }    
        else
        {
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Menu/MenuThongBaoGame") as GameObject, transform.position, Quaternion.identity) as GameObject;
            mn.transform.SetParent(trencung.transform, false);
            mn.transform.SetSiblingIndex(0);
            mn.name = "menuthongbao";
            //  mn.transform.position = gameObject.transform.position;
            mn.SetActive(true);
        }
      
    }    
    public void QuenMatKhauMemobi()
    {
        // Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        // btndoi.interactable = false;
        //ThongBao.SetActive(true);
        // txtThongbao.text = "Đang đăng kí...";
        // btnOk.SetActive(false);
        GameObject objquenmk = allloginthuong.transform.GetChild(2).transform.GetChild(0).gameObject;
        if (objquenmk.transform.GetChild(0).GetComponent<InputField>().text.Length < 3)
        {
            //ThongBao.SetActive(true);
            //txtThongbao.text = "Email không hợp lệ!"; btnOk.SetActive(true);
            return;
        }
        PanelLoadDao(true);

        GameObject inputOTP = objquenmk.transform.GetChild(1).gameObject;
        if (inputOTP.activeSelf)
        {
            if (objquenmk.transform.GetChild(1).GetComponent<InputField>().text.Length > 1)
            {
                StartCoroutine(XacThuc());
            }
            else
            {
                ThongBao.SetActive(true); btnOk.SetActive(true);
                txtThongbao.text = "Vui lòng nhập mã OTP được gửi tới email của bạn, hãy kiểm tra trong thư rác";
                PanelLoadDao(false);
            }
        }
        else StartCoroutine(Load());

        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://" + ServerChinh + "/QuenMatKhauMemobi/email/" + objquenmk.transform.GetChild(0).GetComponent<InputField>().text);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Lỗi kết nối tới máy chủ..."; btnOk.SetActive(true);
                //  crgame.OnThongBaoNhanh("Lỗi");
                //btndoi.interactable = true;
                PanelLoadDao(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "ok")
                {
                    inputOTP.SetActive(true);
                    inputOTP.name = objquenmk.transform.GetChild(0).GetComponent<InputField>().text;
                    objquenmk.transform.GetChild(0).GetComponent<InputField>().text = "";
                    objquenmk.transform.GetChild(0).transform.Find("Placeholder").GetComponent<Text>().text = "Nhập mật khẩu mới";
                }
                else
                {
                    ThongBao.SetActive(true); btnOk.SetActive(true);
                    txtThongbao.text = www.downloadHandler.text;
                }
                PanelLoadDao(false);
            }
        }
        IEnumerator XacThuc()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://"+ ServerChinh + "/XacNhanQuenMatKhauMemobi/otp/" + objquenmk.transform.GetChild(1).GetComponent<InputField>().text + "/email/" + inputOTP.name + "/matkhaumoi/"+ objquenmk.transform.GetChild(0).GetComponent<InputField>().text);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Lỗi kết nối tới máy chủ..."; btnOk.SetActive(true);
                //  crgame.OnThongBaoNhanh("Lỗi");
                //btndoi.interactable = true;
                PanelLoadDao(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
              //  JSONNode json = JSON.Parse(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "ok")
                {
                    //        PlayerPrefs.SetString("logingametk", CrGame.EncryptString(objdangki.transform.GetChild(0).GetComponent<InputField>().text, "daorongAdmin"));
                    //        PlayerPrefs.SetString("logingamemk", CrGame.EncryptString(objdangki.transform.GetChild(1).GetComponent<InputField>().text, "daorongAdmin"));
                    ////        PlayerPrefs.SetString("server", CrGame.EncryptString(thutu, "daorongAdmin"));

                    //        id = objdangki.transform.GetChild(0).GetComponent<InputField>().text;
                    //      //  pass = objdangki.transform.GetChild(1).GetComponent<InputField>().text;
                    //        MenuLogin.transform.GetChild(8).gameObject.SetActive(false);//btndangnhap
                    //        MenuLogin.transform.GetChild(9).gameObject.SetActive(false);//btndangki
                    //        MenuChon.SetActive(true); //btnvaogame

                    //        objdangki.transform.GetChild(0).GetComponent<InputField>().text = "";
                    //        objdangki.transform.GetChild(1).GetComponent<InputField>().text = "";
                    //        objdangki.transform.GetChild(2).GetComponent<InputField>().text = "";
                    //        allloginthuong.transform.GetChild(0).gameObject.SetActive(false);
                    //        allloginthuong.transform.GetChild(1).gameObject.SetActive(false);
                    //        MenuChon.transform.GetChild(1).GetComponent<Text>().text = "Chào: Đảo chủ mới";
                    //     GameObject inputOTP = objdangki.transform.GetChild(3).gameObject;

                    allloginthuong.transform.GetChild(0).gameObject.SetActive(true);
                    allloginthuong.transform.GetChild(2).gameObject.SetActive(false);
                    objquenmk.transform.GetChild(0).transform.Find("Placeholder").GetComponent<Text>().text = "Email...";
                    objquenmk.transform.GetChild(0).GetComponent<InputField>().text = "";
                    objquenmk.transform.GetChild(1).GetComponent<InputField>().text = "";
                    inputOTP.SetActive(false);
                    ThongBao.SetActive(true); btnOk.SetActive(true);
                    txtThongbao.text = "Xác nhận mật khẩu mới thành công!";
                }
                else
                {
                    ThongBao.SetActive(true); btnOk.SetActive(true);
                    txtThongbao.text = www.downloadHandler.text;
                }
                PanelLoadDao(false);
            }
        }
    }

#if UNITY_IOS
    string hedieuhanh = "ios";
#elif UNITY_ANDROID
    string hedieuhanh = "android";
#elif UNITY_EDITOR
            string hedieuhanh = "editor";
#else
              string hedieuhanh = "unknown";
#endif
    public void DangKiThuong()
    {
        // Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        // btndoi.interactable = false;
        //ThongBao.SetActive(true);
        // txtThongbao.text = "Đang đăng kí...";
        // btnOk.SetActive(false);
        GameObject objdangki = allloginthuong.transform.GetChild(1).transform.GetChild(0).gameObject;
        if (objdangki.transform.GetChild(0).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Tài khoản không hợp lệ!",2);
            return;
        }
        if (objdangki.transform.GetChild(1).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Mật khẩu không hợp lệ!", 2);            
            return;
        }
        if (objdangki.transform.GetChild(2).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Email không hợp lệ!", 2);
            return;
        }
        PanelLoadDao(true);

        GameObject inputOTP = objdangki.transform.GetChild(3).gameObject;
        if(inputOTP.activeSelf)
        {
            if(objdangki.transform.GetChild(3).GetComponent<InputField>().text.Length > 1)
            {
                StartCoroutine(XacThuc());
            }    
            else
            {
                OnThongBaoNhanh("Vui lòng nhập mã OTP được gửi tới email của bạn, hãy kiểm tra trong thư rác", 3);
                PanelLoadDao(false);
            }
        }    
        else StartCoroutine(Load());

        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://" + ServerChinh + "/DangKiMemobiID/id/" + objdangki.transform.GetChild(0).GetComponent<InputField>().text + "/matkhau/" + objdangki.transform.GetChild(1).GetComponent<InputField>().text + "/email/" + objdangki.transform.GetChild(2).GetComponent<InputField>().text);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Lỗi kết nối tới máy chủ..."; btnOk.SetActive(true);
                //  crgame.OnThongBaoNhanh("Lỗi");
                //btndoi.interactable = true;
                PanelLoadDao(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "ok")
                {
                    objdangki.transform.GetChild(0).GetComponent<InputField>().enabled = false;
                    objdangki.transform.GetChild(1).GetComponent<InputField>().enabled = false;
                    objdangki.transform.GetChild(2).GetComponent<InputField>().enabled = false;
                   
                    inputOTP.SetActive(true);
                    Text btntxt = objdangki.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Text>();
                    btntxt.text = "Xác nhận";
                }
                else
                {
                    OnThongBaoNhanh(www.downloadHandler.text, 2);
                    //ThongBao.SetActive(true); btnOk.SetActive(true);
                    //txtThongbao.text = www.downloadHandler.text;
                }
                PanelLoadDao(false);
            }
        }
        IEnumerator XacThuc()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://" + ServerChinh + "/XacThucMemobiID/id/" + objdangki.transform.GetChild(2).GetComponent<InputField>().text + "/otp/" + objdangki.transform.GetChild(3).GetComponent<InputField>().text);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Lỗi kết nối tới máy chủ..."; btnOk.SetActive(true);
                //  crgame.OnThongBaoNhanh("Lỗi");
                //btndoi.interactable = true;
                PanelLoadDao(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                debug.Log(www.downloadHandler.text);
                if (json["status"].Value == "0")
                {
                    //        PlayerPrefs.SetString("logingametk", CrGame.EncryptString(objdangki.transform.GetChild(0).GetComponent<InputField>().text, "daorongAdmin"));
                    //        PlayerPrefs.SetString("logingamemk", CrGame.EncryptString(objdangki.transform.GetChild(1).GetComponent<InputField>().text, "daorongAdmin"));
                    ////        PlayerPrefs.SetString("server", CrGame.EncryptString(thutu, "daorongAdmin"));

                    //        id = objdangki.transform.GetChild(0).GetComponent<InputField>().text;
                    //      //  pass = objdangki.transform.GetChild(1).GetComponent<InputField>().text;
                    //        MenuLogin.transform.GetChild(8).gameObject.SetActive(false);//btndangnhap
                    //        MenuLogin.transform.GetChild(9).gameObject.SetActive(false);//btndangki
                    //        MenuChon.SetActive(true); //btnvaogame

                    //        objdangki.transform.GetChild(0).GetComponent<InputField>().text = "";
                    //        objdangki.transform.GetChild(1).GetComponent<InputField>().text = "";
                    //        objdangki.transform.GetChild(2).GetComponent<InputField>().text = "";
                    //        allloginthuong.transform.GetChild(0).gameObject.SetActive(false);
                    //        allloginthuong.transform.GetChild(1).gameObject.SetActive(false);
                    //        MenuChon.transform.GetChild(1).GetComponent<Text>().text = "Chào: Đảo chủ mới";
                    //     GameObject inputOTP = objdangki.transform.GetChild(3).gameObject;
                    allloginthuong.transform.GetChild(0).gameObject.SetActive(true);
                    allloginthuong.transform.GetChild(1).gameObject.SetActive(false);
                    GameObject objdangnhap = allloginthuong.transform.GetChild(0).transform.GetChild(0).gameObject;
                    objdangnhap.transform.GetChild(0).GetComponent<InputField>().text = "";
                    objdangnhap.transform.GetChild(1).GetComponent<InputField>().text = "";
                    inputOTP.SetActive(false);
                }
                else
                {
                  
                }
                ThongBao.SetActive(true); btnOk.SetActive(true);
                txtThongbao.text = json["message"].Value;
                PanelLoadDao(false);
            }
        }
    }
   public void OnThongBaoNhanh(string s, float time = 1f)
    {
        GameObject menu = GameObject.FindGameObjectWithTag("trencung").transform.Find("infoitem").gameObject;
        Transform find = menu.transform.Find(s);
        if (find != null)
        {
            if (find.gameObject.activeSelf) return;
        }
        for (int i = 0; i < menu.transform.childCount; i++)
        {
            if (!menu.transform.GetChild(i).gameObject.activeSelf)
            {
                menuinfoitem ifitem = menu.transform.GetChild(i).GetComponent<menuinfoitem>();

                menu.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = s;
                ifitem.gameObject.SetActive(true);
                ifitem.Disnable(time);
                // else Destroy(ifitem.gameObject, time);
                ifitem.name = s;
                break;
            }
            else if (i == menu.transform.childCount - 1)
            {
                GameObject instan = Instantiate(menu.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                instan.transform.GetChild(0).GetComponent<Text>().text = s;
                instan.transform.SetParent(menu.transform, false);
                instan.gameObject.SetActive(true);
                menuinfoitem ifitem = instan.GetComponent<menuinfoitem>();
                //instan.name = menu.transform.childCount.ToString();
                if (menu.transform.childCount <= 3) ifitem.Disnable(time);
                else
                {
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        yield return new WaitForSeconds(time);
                        if (menu.transform.childCount >= 3) Destroy(ifitem.gameObject);
                        else ifitem.gameObject.SetActive(false);

                    }
                }
                ifitem.name = s;
                break;
            }
        }
        if (menu.transform.childCount >= 6) Destroy(menu.transform.GetChild(1).gameObject);
        menu.transform.SetAsLastSibling();
    }
    public void DangNhap() // dangnhapmemobi
    {
        // Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        // btndoi.interactable = false;
        //ThongBao.SetActive(true);
        // txtThongbao.text = "Đang đăng kí...";
        // btnOk.SetActive(false);

        GameObject objdangnhap = allloginthuong.transform.GetChild(0).transform.GetChild(0).gameObject;
        if (objdangnhap.transform.GetChild(0).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Tài khoản không hợp lệ!", 2);
            return;
        }
        if (objdangnhap.transform.GetChild(1).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Mật khẩu không hợp lệ!", 2);
            return;
        }
        PanelLoadDao(true);

        StartCoroutine(Load());

        IEnumerator Load()
        {
            postLogin p = new postLogin(objdangnhap.transform.GetChild(0).GetComponent<InputField>().text,"", objdangnhap.transform.GetChild(1).GetComponent<InputField>().text, hedieuhanh, Application.version);
            string data = JsonUtility.ToJson(p);
            //var request = new UnityWebRequest(LoginFacebook.http + "://"+NameServer+"/dangnhapfbk1.7", "POST");
        //    debug.Log("test2 " + http + "://" + ServerChinh + "/DangNhapp");
            var request = new UnityWebRequest(http + "://" + ServerChinh + "/DangNhapp", "POST");
            //  debug.Log(crgame.ServerName + "dangnhapfbk");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.disposeUploadHandlerOnDispose = true;
            request.disposeDownloadHandlerOnDispose = true;
        
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                debug.Log(request.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Lỗi kết nối tới máy chủ..."; btnOk.SetActive(true);
                //   Invoke("duoclog",5);
            }
            else
            {
             //   debug.Log(request.downloadHandler.text);

                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(request.downloadHandler.text);
                JSONNode json = JSON.Parse(request.downloadHandler.text);
                if (json["status"].Value == "0")
                {
                    allloginthuong.transform.GetChild(0).gameObject.SetActive(false);
                    id = json["data"]["id"].Value;
                    matkhau = json["data"]["matkhau"].Value;
                //    btnloginfb.SetActive(false);
                    btnLoginThuong.SetActive(false);
                    MenuChon.SetActive(true);
                   // nameNv = json["tenhienthi"].Value;
                    MenuChon.transform.GetChild(1).GetComponent<Text>().text = "Chào: " + json["data"]["id"].Value;

                    PlayerPrefs.SetString("memobi", "");
                    PlayerPrefs.SetString("maychu", CrGame.EncryptString(MenuChon.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text, "daorongAdmin"));
                    PlayerPrefs.SetString("id", CrGame.EncryptString(id, "daorongAdmin"));
                    PlayerPrefs.SetString("matkhau", CrGame.EncryptString(matkhau, "daorongAdmin"));
                    PlayerPrefs.SetString("tenhienthi", CrGame.EncryptString(nameNv, "daorongAdmin"));
                    PlayerPrefs.SetString("log", "memobiid");
                    DangNhapThanhCong();
                }
                else
                {
                    OnThongBaoNhanh(json["message"].Value, 2);
                }
                PanelLoadDao(false);
            }
            request.Dispose();
        }
    }
    public void DoiMatKhau() // quenmatkhaumemobi
    {
        // Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        // btndoi.interactable = false;
        //ThongBao.SetActive(true);
        // txtThongbao.text = "Đang đăng kí...";
        // btnOk.SetActive(false);

        GameObject objquenmk = allloginthuong.transform.GetChild(3).transform.GetChild(0).gameObject;
        if (objquenmk.transform.GetChild(0).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Tài khoản không hợp lệ!", 2);
            return;
        }
        if (objquenmk.transform.GetChild(1).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Mật khẩu cũ không hợp lệ!", 2);
            return;
        }
        if (objquenmk.transform.GetChild(2).GetComponent<InputField>().text.Length < 3)
        {
            OnThongBaoNhanh("Mật khẩu mới không hợp lệ!", 2);
            return;
        }
        PanelLoadDao(true);

        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://" + ServerChinh + "/DoiMatKhauMemobi/taikhoan/"+ objquenmk.transform.GetChild(0).GetComponent<InputField>().text + "/matkhaucu/" + objquenmk.transform.GetChild(1).GetComponent<InputField>().text + "/matkhaumoi/"+ objquenmk.transform.GetChild(2).GetComponent<InputField>().text);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Lỗi kết nối tới máy chủ..."; btnOk.SetActive(true);
                //  crgame.OnThongBaoNhanh("Lỗi");
                //btndoi.interactable = true;
                PanelLoadDao(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "ok")
                {
                    objquenmk.transform.GetChild(0).GetComponent<InputField>().text = "";
                    objquenmk.transform.GetChild(1).GetComponent<InputField>().text = "";
                    objquenmk.transform.GetChild(2).GetComponent<InputField>().text = "";
                    allloginthuong.transform.GetChild(3).gameObject.SetActive(false);
                    allloginthuong.transform.GetChild(0).gameObject.SetActive(true);
                    OnThongBaoNhanh("Đổi mật khẩu thành công!", 2);
                }
                else
                {
                    ThongBao.SetActive(true); btnOk.SetActive(true);
                    txtThongbao.text = www.downloadHandler.text;
                }
                PanelLoadDao(false);
            }
        }

    }
    public void GuiLaiOTP()
    {
        GameObject objdangki = allloginthuong.transform.GetChild(1).transform.GetChild(0).gameObject;
        string id = objdangki.transform.GetChild(2).GetComponent<InputField>().text;
        string kieu = "DangKi";
        if (allloginthuong.transform.GetChild(2).gameObject.activeSelf)
        {
            GameObject objquenmk = allloginthuong.transform.GetChild(2).transform.GetChild(0).gameObject;
            GameObject inputOTP = objquenmk.transform.GetChild(1).gameObject;
            id = inputOTP.name;
            kieu = "QuenMK";
        }
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://" + ServerChinh + "/GuiLaiOTP/id/" + id + "/kieu/" + kieu);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Lỗi kết nối tới máy chủ..."; btnOk.SetActive(true);
                //  crgame.OnThongBaoNhanh("Lỗi");
                //btndoi.interactable = true;
                PanelLoadDao(false);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                ThongBao.SetActive(true); btnOk.SetActive(true);
                txtThongbao.text = www.downloadHandler.text;
                PanelLoadDao(false);
            }
        }
    }    
    public void ThoatGame()
    {
        debug.Log("Thoat game");
        Application.Quit();
    }    
    void LoadAllServer()
    {
        PanelLoadDao(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(LoginFacebook.http + "://" + ServerChinh + "/AllServeRGet");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                ThongBao.SetActive(true);
                txtThongbao.text = "Không thể kết nối đến máy chủ!"; btnOk.SetActive(true);
                PanelLoadDao(false);
              //  btnloginfb.SetActive(false);
                btnLoginThuong.SetActive(false);
                btnOk.GetComponent<Button>().onClick.AddListener(ThoatGame);
            }
            else
            {
                // Show results as text
                //   btndoi.interactable = false;
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                GameObject allmaychu = GameObject.Find("Canvas").transform.Find("menuchonmaychu").transform.GetChild(0).transform.GetChild(0).gameObject;

                int thutumaychu = 0;
                if (PlayerPrefs.HasKey("maychu"))
                {
                    thutumaychu = PlayerPrefs.GetInt("maychu");
                }
                for (int i = 0; i < json["allserver"].Count; i++)
                {
                    if(thutumaychu == i)
                    { 
                        NameServer = json["allserver"][i]["ipserver"].Value;
                        MenuChon.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Máy Chủ: " + json["allserver"][i]["nameserver"].AsString;
                        thisServer = json["allserver"][i]["nameserver"].AsString;
                    }    
                    GameObject maychu = allmaychu.transform.GetChild(i + 1).gameObject;
                    maychu.SetActive(true);
                    maychu.transform.GetChild(0).GetComponent<Text>().text = json["allserver"][i]["nameserver"].Value;
                    maychu.transform.GetChild(0).name = json["allserver"][i]["ipserver"].Value;
                //    debug.Log(json[i]["ipserver"].Value);
                //   debug.Log(json[i]);
                }
               // debug.Log(json["linkdown"].AsString);
                DownLoadAssetBundle.SetLinkDown = json["linkdown"].AsString;
                AssetBundleManager.infoasbundle = json["infoasbundle"];
                PanelLoadDao(false);
            //    btnloginfb.SetActive(true);
                btnLoginThuong.SetActive(true);
                //ThongBao.SetActive(false);
                OpenMenuThongBao();
                AutoLogin();
            }
        }
    }    
    void AutoLogin()
    {
        if (PlayerPrefs.HasKey("memobi"))
        {
            id = CrGame.DecryptString(PlayerPrefs.GetString("id"), "daorongAdmin");
            matkhau = CrGame.DecryptString(PlayerPrefs.GetString("matkhau"), "daorongAdmin");
            nameNv = CrGame.DecryptString(PlayerPrefs.GetString("tenhienthi"), "daorongAdmin");

            if (nameNv != "") MenuChon.transform.GetChild(1).GetComponent<Text>().text = "Chào: " + nameNv;
            else MenuChon.transform.GetChild(1).GetComponent<Text>().text = "Chào: Người chơi mới";

            if (PlayerPrefs.GetString("log") == "fb")
            {
                //FBLogin();
            }   
            else
            {
                GameObject objdangnhap = allloginthuong.transform.GetChild(0).transform.GetChild(0).gameObject;
                objdangnhap.transform.GetChild(0).GetComponent<InputField>().text = id;
                objdangnhap.transform.GetChild(1).GetComponent<InputField>().text = matkhau;
                DangNhap();
            }

            ThongBao.SetActive(false);
            btnOk.SetActive(false);

        }
        else
        {
            PlayerPrefs.DeleteAll();
            btnLoginThuong.SetActive(true);
           // btnloginfb.SetActive(true);// btnloginfb
            NameServer = ServerChinh;
        }
    }
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void LoginWithId(InputField input)
    {
        id = input.text;
        nameNv = "test";
        StartCoroutine(Loginfb());
    }    

    //void onHidenUnity(bool isGameShown)
    //{
    //    if (!isGameShown)
    //    {
    //        Time.timeScale = 0;
    //    }
    //    else
    //    {
    //        Time.timeScale = 1;
    //    }
    //}
    public void DangXuatFb()
    {
        NetworkManager.ins.socket.Close();
        CrGame.ins.OnThongBao(false,"Đang Đăng Xuất...",false);
     //   FB.LogOut();
        PlayerPrefs.DeleteKey("autologin");
        PlayerPrefs.DeleteKey("log"); 
        Destroy(gameObject);
        Application.LoadLevel("Login");
    }    
    public void DangXuatChonServer()
    {
     //   if(PlayerPrefs.GetString("log") == "fb") FB.LogOut();
        MenuChon.SetActive(false);
       // btnloginfb.SetActive(true);
        btnLoginThuong.SetActive(true);
        PlayerPrefs.DeleteAll();
    }
    public void ChonServer()
    {
        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string tensv = g.transform.GetChild(0).GetComponent<Text>().text;
        string maychu = "Máy Chủ: " + tensv;
        MenuChon.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = maychu;
        NameServer = g.transform.GetChild(0).name;
        g.transform.parent.transform.parent.transform.parent.gameObject.SetActive(false);
        PlayerPrefs.SetInt("maychu", g.transform.GetSiblingIndex() - 1);
        thisServer = tensv;
    }
   
   // Start is called before the first frame update
    public void loginn()
    {
      //  ThongBao.SetActive(true);
        //   if (duocdangnhap)
        //    {
        //txtThongbao.text = "Đợi chút ..."; btnOk.SetActive(false);
            PanelLoadDao(true);
            //MenuChon.transform.GetChild(0).GetComponent<Button>().interactable = false;
            StartCoroutine(Loginfb());
            duocdangnhap = false;
       // }
       // else
        //{
       //     txtThongbao.text = "Hãy đăng nhập lại sau vài giây"; btnOk.SetActive(true);
       // }
    }
    void duoclog()
    {
        duocdangnhap = true;
    }
 
    public IEnumerator Loginfb()
    {
        //NameServer = GameObject.Find("InputNameServer").GetComponent<InputField>().text;
        yield return id != "";

        string hedieuhanh = "khac";
#if UNITY_IOS
      hedieuhanh = "ios";
#elif UNITY_ANDROID
        hedieuhanh = "android";
#endif
          postLogin p = new postLogin(id,nameNv,matkhau, hedieuhanh, Application.version);
        string data = JsonUtility.ToJson(p);
        //var request = new UnityWebRequest(LoginFacebook.http + "://"+NameServer+"/dangnhapfbk1.7", "POST");
        var request = new UnityWebRequest(LoginFacebook.http + "://"+NameServer+"/dangnhapfbk3.9", "POST");
      //  debug.Log(crgame.ServerName + "dangnhapfbk");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        request.disposeUploadHandlerOnDispose = true;
        request.disposeDownloadHandlerOnDispose = true;

        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            debug.Log(request.error);
            OnThongBaoNhanh("Lỗi kết nối tới máy chủ...",2);
            PanelLoadDao(false);
            //   Invoke("duoclog",5);
        }
        else
        {
            debug.Log(request.downloadHandler.text);
            JSONNode json = JSON.Parse(request.downloadHandler.text);
            panelloaddao.transform.GetChild(1).GetComponent<Text>().text = "";
            if (json["status"].AsString == "0")
            {
                ThongBao.SetActive(false);
                // SGSceneManager.LoadScene("SampleScene");
                //   Application.LoadLevel("SampleScene");
            //    key = json["key"].Value;
                PlayerPrefs.SetString("memobi","");
              //  PlayerPrefs.SetString("maychu", CrGame.EncryptString(MenuChon.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text, "daorongAdmin"));
                PlayerPrefs.SetString("id", CrGame.EncryptString(id, "daorongAdmin"));
                PlayerPrefs.SetString("matkhau", CrGame.EncryptString(matkhau, "daorongAdmin"));
                PlayerPrefs.SetString("tenhienthi", CrGame.EncryptString(nameNv, "daorongAdmin"));
                PanelLoadDao(false);
                token = json["token"].AsString;
               
               // AesEncryption.Key += substring;
             //   AesEncryption.IV += substring;
                Debug.Log("token là " + token);
                StartCoroutine(BeginLoad());
               // SGSceneManager.LoadScene("SampleScene");
                MenuLogin.SetActive(false);
                ThanhLoad.SetActive(true);
             
            }
            else if (json["status"].AsString == "1")
            {
                MenuLogin menulogin = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MenuLogin>();
                panelloaddao.SetActive(true);
                menulogin.timecho = float.Parse(json["timecho"].Value) + 2;
              //  panelloaddao.transform.GetChild(1).GetComponent<Text>().text = "hahahihi";
            }
            else if(json["status"].AsString == "2")
            {
                // OnThongBaoNhanh(request.downloadHandler.text, 2);
                ThongBao.SetActive(true);
                 txtThongbao.text = json["message"].Value;
                btnOk.SetActive(true);
                // Invoke("duoclog", 5);
                PanelLoadDao(false);
            }
         
        }
        request.Dispose();
        // crgame.OnThongBao(true, request.downloadHandler.text, true)
    }
    public void Loginthuong()
    {
        ThongBao.SetActive(true);
        txtThongbao.text = "Đang đăng nhập...";
        btnOk.SetActive(false);
        loginthuong.transform.GetChild(2).GetComponent<Button>().interactable = false;
        StartCoroutine(LoginThuong());
        IEnumerator LoginThuong()
        {
            //NameServer = GameObject.Find("InputNameServer").GetComponent<InputField>().text;
            yield return id != "";
            postLogin p = new postLogin(loginthuong.transform.GetChild(0).GetComponent<InputField>().text, nameNv, loginthuong.transform.GetChild(1).GetComponent<InputField>().text,"android", Application.version);
            string data = JsonUtility.ToJson(p);
            var request = new UnityWebRequest(LoginFacebook.http + "://" + NameServer + "/dangnhap", "POST");
            //  debug.Log(crgame.ServerName + "dangnhapfbk");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.disposeUploadHandlerOnDispose = true;
            request.disposeDownloadHandlerOnDispose = true;
         
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                debug.Log(request.error);
                txtThongbao.text = "Không thể kết nối đến máy chủ";
                ThongBao.SetActive(true);
                btnOk.SetActive(true);
                loginthuong.transform.GetChild(2).GetComponent<Button>().interactable = true;
                Invoke("duoclog", 10);
            }
            else
            {
                debug.Log(request.downloadHandler.text);
                JSONNode json = JSON.Parse(request.downloadHandler.text);
                if (json["status"].Value == "thành công")
                {
                    id = loginthuong.transform.GetChild(0).GetComponent<InputField>().text;

                  //  btnloginfb.SetActive(false);
                    btnLoginThuong.SetActive(false);
                    MenuChon.SetActive(true);
                    nameNv = json["tenhienthi"].Value;
                    MenuChon.transform.GetChild(1).GetComponent<Text>().text = "Chào: " + nameNv;
                    PlayerPrefs.SetString("tenfb", nameNv);
                    ThongBao.SetActive(false);


                    allloginthuong.transform.GetChild(0).gameObject.SetActive(false);
                    PlayerPrefs.SetString("login", "thuong");
                }
                else
                {
                    txtThongbao.text = json["status"].Value;
                    btnOk.SetActive(true);
                    ThongBao.SetActive(true);
                    Invoke("duoclog", 20);
                }
                loginthuong.transform.GetChild(2).GetComponent<Button>().interactable = true;
            }
            // crgame.OnThongBao(true, request.downloadHandler.text, true)
            request.Dispose();
        }

    }
   
    private IEnumerator BeginLoad()
    {
         Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.High;
       // Application.backgroundLoadingPriority = ThreadPriority.Normal;
        //SGAsyncOperation operation = SGSceneManager.LoadSceneAsync("SampleScenetest");
        AsyncOperation operation = SceneManager.LoadSceneAsync("SampleScenetest");
        while (!operation.isDone)
        {
            UpdateProgressUI(operation.progress * 100f);
            yield return null;
        }
        UpdateProgressUI(operation.progress * 100f);
    }
    private void UpdateProgressUI(float progress)
    {
        progress /= 2;
        if (maskload == null) return;
        maskload.fillAmount = (float)progress / (float)100;
        txtLoad.text = "Đang tải dữ liệu " + Math.Round(progress,2) + "%";
    }
    //IEnumerator LoadYourAsyncScene()
    //{
    //    // The Application loads the Scene in the background as the current Scene runs.
    //    // This is particularly good for creating loading screens.
    //    // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
    //    // a sceneBuildIndex of 1 as shown in the In-App Settings.

    //    SGAsyncOperation asyncLoad = SGSceneManager.LoadSceneAsync("SampleScene");

    //    // Wait until the asynchronous scene fully loads
    //    while (!asyncLoad.isDone)
    //    {
    //        debug.Log((asyncLoad.progress * 100)+ "%");
    //        yield return null;
    //    }
    //}
    //IEnumerator SaveAnhDaiDien()
    //{
    //    // We should only read the screen buffer after rendering is complete
    //    yield return new WaitForEndOfFrame();

    //    // Create a texture the size of the screen, RGB24 format
    //    int width = Screen.width;
    //    int height = Screen.height;
    //    Texture2D tex = new Texture2D((int)FB_useerDp.sprite.rect.width, (int)FB_useerDp.sprite.rect.height);

    //    // Read screen contents into the texture
    //    // tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    //    Color[] pixels = FB_useerDp.sprite.texture.GetPixels(
    //          (int)FB_useerDp.sprite.textureRect.x,
    //          (int)FB_useerDp.sprite.textureRect.y,
    //          (int)FB_useerDp.sprite.textureRect.width,
    //          (int)FB_useerDp.sprite.textureRect.height
    //      );

    //    tex.SetPixels(pixels);
    //    tex.Apply();
    //    //  tex = FB_useerDp.GetComponent<RawImage>().texture;
    //    // _texture.wrapMode = TextureWrapMode.Clamp;
    //    // Encode texture into PNG
    //    byte[] bytes = tex.EncodeToPNG();
    //    debug.Log("ByteNe"+ bytes.ToString());

    //    Object.Destroy(tex);

    //    // For testing purposes, also write to a file in the project folder
    //    // File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);


    //    // Create a Web Form
    //    //WWWForm form = new WWWForm();
    //    //form.AddField("frameCount", Time.frameCount.ToString());
    //    //form.AddBinaryData("fileUpload", bytes);

    //    //// Upload to a cgi script
    //    //var w = UnityWebRequest.Post("http://localhost/cgi-bin/env.cgi?post", form);
    //    //yield return w.SendWebRequest();

    //    //if (w.result != UnityWebRequest.Result.Success)
    //    //{
    //    //    debug.Log(w.error);
    //    //}
    //    //else
    //    //{
    //    //    debug.Log("Finished Uploading Screenshot");
    //    //}
    //}
    //IEnumerator IEData()
    //{
    //    crgame.OnThongBao(true, "Đang lấy dữ liệu...");
    //    WWWForm form = new WWWForm();
    //    form.AddField("idFacebook", id);

    //    using (UnityWebRequest www = UnityWebRequest.Post("https://canhcutzuive.online/DaoRong/php/GetData.php", form))
    //    {
    //        yield return www.SendWebRequest();
    //        if (www.isNetworkError || www.isHttpError)
    //        {
    //            debug.Log(www.error);
    //            crgame.OnThongBao(true, "Lỗi", true);
    //        }
    //        else
    //        {
    //            debug.Log(www.downloadHandler.text);
    //            string[] data = www.downloadHandler.text.Split('/');
    //            if (data[0] == "Thành Công")
    //            {
    //                crgame.txtVang.text = data[5]; crgame.txtKimCuong.text = data[6];

    //                crgame.txtLevel.text = data[2];
    //                float fillamount = float.Parse(data[3]) / float.Parse(data[4]); crgame.imgThanhLevel.fillAmount = fillamount;
    //                crgame.Exp = int.Parse(data[3]);crgame.MaxExp = int.Parse(data[4]);
    //                string[] dataCongtrinh = data[7].Split('~');
    //                JsonCongTrinh JsonCT = JsonCongTrinh.CreateFromJSON(dataCongtrinh[0]); JsonCT.LoadArray();
    //                //debug.Log(data[8]);
    //                for (int i = 0; i < 4; i++)
    //                {
    //                    CongTrinh ct = GameObject.Find("CongTrinh (" + i + ")").GetComponent<CongTrinh>();
    //                    ct.nameCongtrinh = JsonCT.name[i]; ct.levelCongtrinh = JsonCT.capct[i]; ct.LoadImg();
    //                }
    //                JsonCongTrinh JsonCT2 = JsonCongTrinh.CreateFromJSON(dataCongtrinh[1]); JsonCT2.LoadArray();
    //                for (int i = 4; i < 8; i++)
    //                {
    //                    CongTrinh ct = GameObject.Find("CongTrinh (" + i + ")").GetComponent<CongTrinh>();
    //                    ct.nameCongtrinh = JsonCT2.name[i - 4]; ct.levelCongtrinh = JsonCT2.capct[i - 4]; ct.LoadImg();
    //                }
    //                if(data[8] != "")
    //                {
    //                    string[] datatemrong = data[8].Split('~');
    //                    for (int i = 0; i < datatemrong.Length; i++)
    //                    {
    //                        jsonitemRong itemrong = jsonitemRong.CreateFromJson(datatemrong[i]);
    //                        inventory.AddItemRong(itemrong.nameItem, itemrong.Sao,itemrong.levelrong,itemrong.exp,itemrong.maxexp,itemrong.tienhoa,itemrong.sothucAn,itemrong.tenrong);
    //                       // debug.Log("tui do rong" + datatemrong[i]);
    //                    }
    //                }
    //                if (data[9] != "")
    //                {
    //                    string[] datarongdao1 = data[9].Split('~');
    //                    for (int i = 0; i < datarongdao1.Length; i++)
    //                    {
    //                        if(datarongdao1[i] != "")
    //                        {
    //                            jsonitemRong itemrong = jsonitemRong.CreateFromJson(datarongdao1[i]);
    //                            foreach (Transform child in GameObject.Find("ChuaRong").transform)
    //                            {
    //                              //  debug.Log(itemrong.nameItem);
    //                                if (child.name == itemrong.nameItem)
    //                                {
    //                                    GameObject rong = Instantiate(child.gameObject, new Vector3(Random.Range(-3, 3), Random.Range(-3, 3)), Quaternion.identity) as GameObject;
    //                                    rong.transform.SetParent(crgame.GDien[0].transform);
    //                                    rong.name = itemrong.nameItem + crgame.soronghientai;
    //                                    crgame.soronghientai += 1;
    //                                    DragonController dra = rong.GetComponent<DragonController>();
    //                                    dra.sao = itemrong.Sao; dra.Exp = itemrong.exp; dra.maxExp = itemrong.maxexp;
    //                                    dra.sothucan = itemrong.sothucAn; dra.tienhoa = itemrong.tienhoa; dra.Levelrong = itemrong.levelrong;
    //                                    dra.tenrong = itemrong.tenrong;
    //                                    rong.SetActive(true);
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                if (data[10] != "")
    //                {
    //                    string[] datarongdao2 = data[10].Split('~');
    //                    for (int i = 0; i < datarongdao2.Length; i++)
    //                    {
    //                        if (datarongdao2[i] != "")
    //                        {
    //                            jsonitemRong itemrong = jsonitemRong.CreateFromJson(datarongdao2[i]);
    //                            foreach (Transform child in GameObject.Find("ChuaRong").transform)
    //                            {
    //                                //debug.Log(itemrong.nameItem);
    //                                if (child.name == itemrong.nameItem)
    //                                {
    //                                    GameObject rong = Instantiate(child.gameObject, new Vector3(crgame.GDien[1].transform.position.x + Random.Range(-3,3), crgame.GDien[1].transform.position.y + Random.Range(-3, 3)), Quaternion.identity) as GameObject;
    //                                    rong.transform.SetParent(crgame.GDien[1].transform);
    //                                    rong.name = itemrong.nameItem + crgame.soronghientai;
    //                                    crgame.soronghientai += 1;
    //                                    DragonController dra = rong.GetComponent<DragonController>();
    //                                    dra.sao = itemrong.Sao; dra.Exp = itemrong.exp; dra.maxExp = itemrong.maxexp;
    //                                    dra.sothucan = itemrong.sothucAn; dra.tienhoa = itemrong.tienhoa; dra.Levelrong = itemrong.levelrong;
    //                                    dra.tenrong = itemrong.tenrong;
    //                                    dra.sothucan = itemrong.sothucAn;
    //                                    rong.SetActive(true);
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                isLogin = true;
    //                net.PlayerOnline();
    //                crgame.OnThongBao(false);
    //                crgame.menulogin.SetActive(false);
    //            }
    //            else
    //            {
    //                crgame.OnThongBao(true, www.downloadHandler.text, true);
    //            }
    //        }
    //    }
    //}

}
[SerializeField]
public class SothucAn
{
    public string nameItem;
    public int exp, exprong, soluong;
    public string thoigiantieuhoa;
    public static SothucAn CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<SothucAn>(data);
    }
}
[SerializeField]
public class postLogin
{
    public string taikhoan,matkhau,name,hedieuhanh,version;
    public postLogin(string idf,string Name,string MatKhau,string HeDieuHanh,string Verison)
    {
        taikhoan = idf;
        name = Name;
        matkhau = MatKhau;
        hedieuhanh = HeDieuHanh;
        version = Verison;
    }
}
