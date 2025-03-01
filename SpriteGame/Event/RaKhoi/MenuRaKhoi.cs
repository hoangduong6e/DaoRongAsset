
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MenuRaKhoi : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite nhaSocChuSang, NhaSocChuToi, NhaBacSiSang, NhaBacSiToi;
    public Image nhaSocChu, NhaBacSi;
    public Text[] txt;

    private bool chonNhaSoc = true, chonNhaBacSi = true;

    private string[] SocChat = new string[] { "Có rất nhiều đồ hiếm trong quán BA CON SÓC!" };
    private string[] ThuongNhanChat = new string[] { "Bản đồ kho báu Râu Đen cho người anh hùng đây!" };
    private string[] BacSiChat = new string[] {"Vô đây ta nâng cấp rồng cho nè!", "Cảm thấy Rồng chưa đủ mạnh? Bấm vào đây!" };
    private List<string[]> list = new List<string[]>();
    private GameObject btnHopQua;
    Transform MapRaKhoi;
    Transform PanelAllDao;
    public GameObject EffectDao,DauX;
    private Vector2[] allPosEffectDao = new Vector2[] {new Vector2(50,84), new Vector2(48, 58.5f) , new Vector2(46, 51) , new Vector2(41, 55.3f) , new Vector2(23, 66) , new Vector2(41, 93) };

    private Vector2[] allPosDauX = new Vector2[] { new Vector2(97, -35), new Vector2(23.4f, -22.6f), new Vector2(47, -26), new Vector2(41, -30), new Vector2(23, -49), new Vector2(58, -31) };
    private Transform ViTriDao;
    private int[] vitriThuyen;
    private int[] vitriKhoBau;
    public Animator animThuyenTo;
    public float timeSec;
    public static string nameEvent = "EventTraoHongDoatLong";

    public static MenuRaKhoi ins;
    void Start()
    {
        ins = this;
        InvokeRepeating(nameof(AnimNhaSangToi),0.5f, 0.5f);
    }

    public void ParseData(JSONNode json)
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Camera.main.orthographicSize = 5;
        cam.GetComponent<ZoomCamera>().enabled = false;
        debug.Log(json.ToString());
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        GameObject g = transform.GetChild(0).gameObject;
        g.transform.Find("Item").transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = json["TuiLuongThuc"].AsString;
        gameObject.SetActive(true);

       // quan3ConSoc = transform.Find("Quan3ConSoc").transform.GetChild(0);
       
        MapRaKhoi = transform.Find("MapRaKhoi");
        PanelAllDao = MapRaKhoi.transform.Find("PanelAllDao");
        int hang = json["vitriThuyen"][0].AsInt;
        int O = json["vitriThuyen"][1].AsInt;
        vitriThuyen = new int[] { hang, O };
        vitriKhoBau = new int[] { json["vitriKhoBau"][0].AsInt, json["vitriKhoBau"][1].AsInt };


        btnAptrung.transform.SetParent(CrGame.ins.trencung);
        btnAptrung.transform.SetSiblingIndex(0);
        txtTimeApTrung.transform.SetParent(CrGame.ins.trencung);
        txtTimeApTrung.transform.SetSiblingIndex(0);
        ViTriDao = PanelAllDao.transform.GetChild(hang).transform.GetChild(O).transform;
        Thuyen.transform.position = ViTriDao.transform.position;
        btnHopQua.transform.SetParent(transform);
        //if (json["TrungRongRua"].AsInt > 0)
        //{
        //    if (json["TrangThaiTrung"].AsString == "chuaap")
        //    {
        //        btnAptrung.SetActive(true);
        //        Animator anim = transform.GetChild(0).transform.Find("LongApRongRua").GetComponent<Animator>();
        //        anim.Play("CoTrung_chua_ap");
        //    }
        //    else if (json["TrangThaiTrung"].AsString == "dangap")
        //    {
        //        if (json["sec"].AsFloat > 0)
        //        {
        //            timeSec = json["sec"].AsFloat;
        //            dem = true;
        //            txtTimeApTrung.gameObject.SetActive(true);
        //            Animator anim = transform.GetChild(0).transform.Find("LongApRongRua").GetComponent<Animator>();
        //            anim.Play("ApTrung");
        //        }
        //        else
        //        {
        //            timeSec = 0;
        //            txtTimeApTrung.text = "";
        //            Animator anim = transform.GetChild(0).transform.Find("LongApRongRua").GetComponent<Animator>();
        //            anim.Play("LongApRongRua");
        //        }
        //    }
        //    else
        //    {
        //        transform.GetChild(0).transform.Find("LongApRongRua").gameObject.SetActive(false);
        //    }    
        //}
       
        if (json["XucTu"]["xuathien"].AsBool)
        {
            //ThuyenDiChuyen(json["XucTu"]["vitriDangDen"][0].AsInt, json["XucTu"]["vitriDangDen"][1].AsInt, json["XucTu"]["xuathien"].AsBool);
            int hanggg = json["XucTu"]["vitriDangDen"][0].AsInt;
            int cottt = json["XucTu"]["vitriDangDen"][1].AsInt;
            LoadVitriThuyenTruocDo(hang,O);
            Transform tfhang = PanelAllDao.transform.GetChild(hanggg);
            Transform tfcot = tfhang.transform.GetChild(cottt);
            //Transform btn = null;
            //for (int i = 0; i < tfcot.transform.childCount; i++)
            //{
            //    if(tfcot.transform.GetChild(i).name.Substring(0,3) == "Dao")
            //    {
            //        btn = tfcot.transform.GetChild(i);
            //        break;
            //    }
            //}
            Vector3 vec = Vector3.zero;
            if (hanggg > vitriThuyen[0] || hanggg < vitriThuyen[0])
            {
                int cong = 3;
                if (cottt < vitriThuyen[1]) cong = -2;
                 vec = new Vector3(ViTriDao.transform.position.x + cong, tfcot.transform.position.y, tfcot.transform.position.z);
            }
            else
            {
                Transform eff = tfcot.transform.GetChild(0);
                float X = 0;
                // X = eff.transform.position.x - 1.5f;
                if (cottt < vitriThuyen[1]) X = eff.transform.position.x + 2.5f;
                else X = eff.transform.position.x - 1.5f;
                 vec = new Vector3(X, tfcot.transform.position.y, tfcot.transform.position.z);

            }
            Thuyen.transform.position = vec;
            HienXucTu(vec);
            LoadODao(cottt, hanggg);
        }
        else
        {
            if (json["vitriThuyenTruocDo"][0].AsInt != -1)
            {
                LoadVitriThuyenTruocDo(json["vitriThuyenTruocDo"][0].AsInt, json["vitriThuyenTruocDo"][1].AsInt);
                LoadChatThuyen();
            }
            LoadODao(O, hang);
        }

        giaodiennut1 = MapRaKhoi.transform.Find("giaodiennut1").gameObject;
        Transform allitem = giaodiennut1.transform.Find("Item");
        allitem.transform.Find("txtTuiLuongThuc").GetComponent<Text>().text = json["TuiLuongThuc"].AsString;
        allitem.transform.Find("txtDongXu").GetComponent<Text>().text = json["DongXuCo"].AsString;

      
        animThuyenTo.Play("Idle");
        ChuyenCanh.transform.SetParent(CrGame.ins.trencung);

        if (json["BanDoKhoBau"].AsInt == 0)
        {
            ThuongNhanChat[0] = "<size=35>Khi nào có Bản đồ Kho báu Râu Đen thì hãy đến nói chuyện với ta</size>";
        }
        list.Add(SocChat);
        list.Add(ThuongNhanChat);
        list.Add(BacSiChat);;

        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < list.Count; i++)
            {
                int random = Random.Range(0, list[i].Length);
                if (list[i][random] != "")
                {
                    txt[i].transform.parent.gameObject.SetActive(true);
                    txt[i].transform.parent.transform.LeanScale(new Vector3(0.7f, 0.7f, 1), 0.35f);

                    txt[i].text = list[i][random];
                }

                StartCoroutine(delaychat(i));
            }
        }
        AudioManager.SetSoundBg("nhacnen1");
    }
    
    private void LoadLuongThuc(int i)
    {
        Transform allitem = giaodiennut1.transform.Find("Item");
        Transform tf = allitem.transform.Find("txtTuiLuongThuc");
        Transform g = transform.GetChild(0);
        g.transform.Find("Item").transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = i.ToString();
        // tf.GetComponent<Text>().text = i.ToString();
        HieuUngTxt(tf, i.ToString());
    }    
    private void LoadDongXu(int i)
    {
        Transform allitem = giaodiennut1.transform.Find("Item");
        Transform tf = allitem.transform.Find("txtDongXu");
        // tf.GetComponent<Text>().text = i.ToString();

       // Text txtDongXuCo = quan3ConSoc.transform.Find("txtDongXuCo").GetComponent<Text>();

        //txtDongXuCo.text = i.ToString();

        HieuUngTxt(tf,i.ToString());
    }
    private void HieuUngTxt(Transform tf,string str)
    {
        Text txt = tf.GetComponent<Text>();
        if (int.Parse(txt.text) == int.Parse(str))
        {
            txt.text = str;
            return;
        }    
        GameObject hieuung = Instantiate(txtAnim, transform.position, Quaternion.identity);
        hieuung.transform.SetParent(giaodiennut1.transform,false);
        Vector3 vec = tf.transform.position;
        vec.y += 0.3f;
        hieuung.transform.position = vec;
    
        Text txteff = hieuung.transform.GetChild(0).GetComponent<Text>();
        if (int.Parse(str) > int.Parse(txt.text))
        {
            txteff.text = "+" + (int.Parse(str) - int.Parse(txt.text)) + "";  
        }
        else
        {
            txteff.text ="-" + (int.Parse(str) - int.Parse(txt.text)) + "";
        }
        txt.text = str;
        hieuung.gameObject.SetActive(true);
        Destroy(hieuung, 3f);
    }
    private void LoadODao(int O,int hang)
    {
        if (O <= 7)
        {
            AddBtnDao(hang, O + 1);
        }
        if (O >= 1)
        {
            AddBtnDao(hang, O - 1);
        }
        if (hang <= 0 && hang < 5)
        {
            AddBtnDao(hang + 1, O);
        }
        if (hang > 0 && hang < 4)
        {
            AddBtnDao(hang + 1, O);
        }
        if (hang > 0)
        {
            AddBtnDao(hang - 1, O);
        }
    }    
    private void AddBtnDao(int hang, int cot)
    {
        Transform tfhang = PanelAllDao.transform.GetChild(hang);
        Transform tfcot = tfhang.transform.GetChild(cot);
        Transform btndao = tfcot.transform.GetChild(0);
        btndao.AddComponent<Button>().onClick.AddListener(ChonDao);
        GameObject effdao = Instantiate(EffectDao, transform.position, Quaternion.identity);
        effdao.transform.SetParent(tfcot,false);
        effdao.transform.SetSiblingIndex(0);
        btndao.GetComponent<Image>().raycastTarget = true;
        string input = btndao.name;

        int numberPart = int.Parse(input.Substring(input.Length - 1)) - 1;

        effdao.GetComponent<RectTransform>().anchoredPosition = allPosEffectDao[numberPart];
        allEffDao.Add(effdao.transform);
        effdao.SetActive(true);
    }
    private void ClearAllEff()
    {
        for (int i = 0; i < allEffDao.Count; i++)
        {
            Transform btn = allEffDao[i].transform.parent.transform.GetChild(1).transform;
            Destroy(btn.GetComponent<Button>());
            btn.GetComponent<Image>().raycastTarget = false;
            Destroy(allEffDao[i].gameObject);
        }
        allEffDao.Clear();
    }    
    private bool thuyendi = false;
    private List<Transform> allEffDao = new List<Transform>();
    public GameObject XucTuXuatHien,ImgHopQua,txtAnim;
    private void ChonDao()
    {
        if (thuyendi || XucTuXuatHien.activeSelf) return;
        
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        Transform tfcot = btnchon.transform.parent;
        Transform tfhang = tfcot.transform.parent;

        int hang = tfhang.transform.GetSiblingIndex();
        int cot = tfcot.transform.GetSiblingIndex();

        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "ChonDao";
        datasend["data"]["hang"] = hang.ToString();
        datasend["data"]["cot"] = cot.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                ThuyenDiChuyen(cot, hang, json["xuctu"].AsBool,false, json["vitriKhoBau"]);
                LoadLuongThuc(json["TuiLuongThuc"].AsInt);
                LoadDongXu(json["DongXuCo"].AsInt);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
        }

    }
    private void ThuyenDiChuyen(int cot, int hang,bool xuctu = false,bool bochay = false,JSONNode vitrikhobau = null)
    {
        Transform tfhang = PanelAllDao.transform.GetChild(hang);
        Transform tfcot = tfhang.transform.GetChild(cot);
        GameObject objchat = Thuyen.transform.GetChild(1).gameObject;
        objchat.SetActive(false);
        objchat.transform.localScale = Vector3.zero;
        thuyendi = true;

       // int hang = tfhang.transform.GetSiblingIndex();
        //int cot = tfcot.transform.GetSiblingIndex();

        Vector3[] targetPositions = new Vector3[] { Vector3.zero };
        if (hang > vitriThuyen[0] || hang < vitriThuyen[0])
        {
            Transform eff = tfcot.transform.GetChild(0);
            int cong = 3;
            if (cot < vitriThuyen[1]) cong = -2;
            if (!xuctu)
            {
                targetPositions = new Vector3[3];
                targetPositions[2] = new Vector3(eff.transform.position.x, tfcot.transform.position.y, tfcot.transform.position.z);
            }
            else
            {
                targetPositions = new Vector3[2];
            }
            targetPositions[0] = new Vector3(ViTriDao.transform.position.x + cong, ViTriDao.transform.position.y, ViTriDao.transform.position.z);
            targetPositions[1] = new Vector3(ViTriDao.transform.position.x + cong, tfcot.transform.position.y, tfcot.transform.position.z);
            //  if (json["xuctu"].AsBool) HienXucTu(targetPositions[1]);
        }
        else
        {
            Transform eff = tfcot.transform.GetChild(0);
            if (!xuctu)
            {
                targetPositions[0] = new Vector3(eff.transform.position.x, tfcot.transform.position.y, tfcot.transform.position.z);

            }
            else
            {
                float X = 0;
                // X = eff.transform.position.x - 1.5f;
                if (cot < vitriThuyen[1]) X = eff.transform.position.x + 2.5f;
                else X = eff.transform.position.x - 1.5f;
                targetPositions[0] = new Vector3(X, tfcot.transform.position.y, tfcot.transform.position.z);

            }

        }
        if(bochay)// nếu bỏ chạy
        {
            if (targetPositions.Length > 1) 
            {
                // Tạo một mảng mới chỉ chứa phần tử cuối cùng
                Vector3[] newArray = { targetPositions[targetPositions.Length - 1] };

                // Gán mảng mới vào mảng gốc
                targetPositions = newArray;
            }
        }    
        StartCoroutine(MoveTowardsTarget());
        IEnumerator MoveTowardsTarget()
        {
            bool dichuyencam = true;
            if (!Thuyen.transform.parent.gameObject.activeSelf) dichuyencam = false;
            foreach (Vector3 targetPosition in targetPositions)
            {
                // Di chuyển đối tượng đến vị trí đích bằng MoveTowards

                if (targetPosition.x > Thuyen.transform.position.x) ScaleThuyen(-1);
                else ScaleThuyen(1);
                while (Vector3.Distance(Thuyen.transform.position, targetPosition) > 0.01f)
                {
                    Thuyen.transform.position = Vector3.MoveTowards(Thuyen.transform.position, targetPosition, 7f * Time.deltaTime);

                    if(dichuyencam)
                    {

                        // Kiểm tra và di chuyển camera nếu thuyền gần đến giới hạn của camera
                        Vector3 camTargetPosition = cam.transform.position;

                        if (Thuyen.transform.position.x > cam.transform.position.x + cam.orthographicSize * cam.aspect - 7 ||
                            Thuyen.transform.position.x < cam.transform.position.x - cam.orthographicSize * cam.aspect + 7 ||
                            Thuyen.transform.position.y > cam.transform.position.y + cam.orthographicSize - 4 ||
                            Thuyen.transform.position.y < cam.transform.position.y - cam.orthographicSize + 4)
                        {
                            camTargetPosition = new Vector3(Thuyen.transform.position.x, Thuyen.transform.position.y, cam.transform.position.z);

                            // Giới hạn vị trí camera
                            camTargetPosition.x = Mathf.Clamp(camTargetPosition.x, imgMap.bounds.min.x + cam.orthographicSize * cam.aspect, imgMap.bounds.max.x - cam.orthographicSize * cam.aspect);
                            camTargetPosition.y = Mathf.Clamp(camTargetPosition.y, imgMap.bounds.min.y + cam.orthographicSize, imgMap.bounds.max.y - cam.orthographicSize);
                        }

                        // Di chuyển camera theo thuyền sử dụng MoveTowards
                        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camTargetPosition, 7f * Time.deltaTime);

                    }
                   

                    yield return null; // Đợi đến khung hình tiếp theo
                }

                // Đảm bảo đối tượng đạt đúng vị trí đích
                Thuyen.transform.position = targetPosition;
            }
            if (xuctu)
            {
                HienXucTu(targetPositions[targetPositions.Length - 1]);
            }
            // Các thao tác khác sau khi di chuyển xong
            int[] vitriThuyenTruocDo = (int[])vitriThuyen.Clone();
            ViTriDao = tfcot.transform;
            vitriThuyen = new int[] { hang, cot };
            thuyendi = false;
    
            if(!bochay)
            {
                ClearAllEff();
                LoadODao(cot, hang);
            }
            //if(vitriThuyenTruocDo[1] == vitriThuyen[1] || vitriThuyenTruocDo[0] == vitriThuyen[0])
            //{
            //    ChatGanHon();
            //}    
            if(vitriThuyenTruocDo[1] < vitriThuyen[1] && vitriKhoBau[1] >= 7)
            {
                ChatGanHon();
            }
            else if (vitriThuyenTruocDo[1] > vitriThuyen[1] && vitriKhoBau[1] <= 1)
            {
                ChatGanHon();
            }
            else if (vitriThuyen[0] > vitriThuyenTruocDo[0] && vitriKhoBau[0] == 4)
            {
                ChatGanHon();
            }
            else if (vitriThuyen[0] < vitriThuyenTruocDo[0] && vitriKhoBau[0] == 0)
            {
                ChatGanHon();
            }
            else if (vitriThuyen[0] == vitriThuyenTruocDo[0] && vitriThuyen[1] == vitriThuyenTruocDo[1])
            {
                int hangDaux = DauX.transform.parent.transform.parent.GetSiblingIndex();
                int cotDaux = DauX.transform.parent.GetSiblingIndex();

                if (cotDaux < vitriThuyen[1] && vitriKhoBau[1] >= 7)
                {
                    ChatGanHon();
                }
                else if (cotDaux > vitriThuyen[1] && vitriKhoBau[1] <= 1)
                {
                    ChatGanHon();
                }
                else if (vitriThuyen[0] > hangDaux && vitriKhoBau[0] == 4)
                {
                    ChatGanHon();
                }
                else if (vitriThuyen[0] < hangDaux && vitriKhoBau[0] == 0)
                {
                    ChatGanHon();
                }
                else
                {
                    if (!CheckGanNhat()) OnChatThuyen(2);
                }
                //   debug.Log("Hang dau x " + hangDaux + " cot dau x " + cotDaux + ", vitriThuyen cot: " + vitriThuyen[0] + " vitriThuyen Hang : " + vitriThuyen[1]);

                //  ChatGanHon();
            }
            else
            {
                if (!CheckGanNhat()) OnChatThuyen(2);
            }
            bool CheckGanNhat()
            {
                if (xuctu)
                {
                    OnChatThuyen(5);
                    return true;
                }
                if (Mathf.Abs(vitriThuyen[1] - vitriKhoBau[1]) == 1 && vitriThuyen[0] == vitriKhoBau[0])
                {
                    OnChatThuyen(3);
                    //     debug.Log("tinh 0: " + Mathf.Abs(vitriThuyen[1] - vitriKhoBau[1]));
                    return true;
                }
                else if (Mathf.Abs(vitriThuyen[0] - vitriKhoBau[0]) == 1 && vitriThuyen[1] == vitriKhoBau[1])
                {
                    OnChatThuyen(3);
                    //   debug.Log("tinh 1: " + Mathf.Abs(vitriThuyen[0] - vitriKhoBau[0]));
                    return true;
                }
                else if (vitriThuyen[0] == vitriKhoBau[0] && vitriThuyen[1] == vitriKhoBau[1])
                {
                    OnChatThuyen(4);
                    ResetDao();
                    if(vitrikhobau != null) vitriKhoBau = new int[] { vitrikhobau[0].AsInt, vitrikhobau[1].AsInt };
                    return true;
                }
                return false;
            }
            void ChatGanHon()
            {
                if (xuctu)
                {
                    OnChatThuyen(5);
                    return;
                } 
                    
                if (!CheckGanNhat()) OnChatThuyen(1);
            }
            if(!bochay) LoadVitriThuyenTruocDo(vitriThuyenTruocDo[0], vitriThuyenTruocDo[1]);
            GameObject imgquabay = Instantiate(ImgHopQua,transform.position,Quaternion.identity);
            imgquabay.GetComponent<QuaBay>().vitribay = btnHopQua;
            imgquabay.transform.position = Thuyen.transform.GetChild(0).transform.position;
            imgquabay.SetActive(true);
        }
    }
    private void LoadChatThuyen()
    {
        int hangDaux = DauX.transform.parent.transform.parent.GetSiblingIndex();
        int cotDaux = DauX.transform.parent.GetSiblingIndex();


        if (Mathf.Abs(vitriThuyen[1] - vitriKhoBau[1]) == 1 && vitriThuyen[0] == vitriKhoBau[0])
        {
            OnChatThuyen(3);
            return;
        }
        else if (Mathf.Abs(vitriThuyen[0] - vitriKhoBau[0]) == 1 && vitriThuyen[1] == vitriKhoBau[1])
        {
            OnChatThuyen(3);
            return;
        }

        if (cotDaux < vitriThuyen[1] && vitriKhoBau[1] >= 7)
        {
            OnChatThuyen(1);
        }
        else if (cotDaux > vitriThuyen[1] && vitriKhoBau[1] <= 1)
        {
            OnChatThuyen(1);
        }
        else if (vitriThuyen[0] > hangDaux && vitriKhoBau[0] == 4)
        {
            OnChatThuyen(1);
        }
        else if (vitriThuyen[0] < hangDaux && vitriKhoBau[0] == 0)
        {
            OnChatThuyen(1);
        }
        else
        {
             OnChatThuyen(2);
        }
     //   debug.Log("Hang dau x " + hangDaux + " cot dau x " + cotDaux + ", vitriThuyen cot: " + vitriThuyen[0] + " vitriThuyen Hang : " + vitriThuyen[1]);

        //  ChatGanHon();
    }
    public void ChangeSprite(Sprite sprite)
    {
        GameObject btnChon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Image img = btnChon.GetComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
    }
    public Animator ChuyenCanh;
    private void ResetDao()
    {
        thuyendi = true;
        GameObject btnkhobau = giaodiennut1.transform.Find("RuongKhoBau").gameObject;
        btnkhobau.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(2f);
            GameObject imgquabay = Instantiate(ImgHopQua, transform.position, Quaternion.identity);
            imgquabay.GetComponent<QuaBay>().vitribay = btnHopQua;
            imgquabay.transform.position = btnkhobau.transform.position;
            imgquabay.SetActive(true);
            yield return new WaitForSeconds(2.6f);
            btnkhobau.SetActive(false);


            ChuyenCanh.transform.position = cam.transform.position;
            RectTransform rectTransform = ChuyenCanh.GetComponent<RectTransform>();
            Vector3 currentPos = rectTransform.localPosition;

            // Thay đổi giá trị z thành 0
            currentPos.z = 0;

            // Gán lại giá trị đã thay đổi vào localPosition
            rectTransform.localPosition = currentPos;
            ChuyenCanh.gameObject.SetActive(true);
            ChuyenCanh.Play("chuyencanh");

        
            yield return new WaitForSeconds(1.8f);
            DauX.SetActive(false);
            ClearAllEff();

            vitriThuyen = new int[] { 2, 4 };

            ViTriDao = PanelAllDao.transform.GetChild(2).transform.GetChild(4).transform;
            Thuyen.transform.position = ViTriDao.transform.position;
            LoadODao(4, 2);

            GameObject objchat = Thuyen.transform.GetChild(1).gameObject;
            objchat.SetActive(false);
            objchat.transform.localScale = Vector3.zero;

            ChuyenCanh.SetBool("ok", true);

            Vector3 camTargetPosition = cam.transform.position;
            camTargetPosition = new Vector3(Thuyen.transform.position.x, Thuyen.transform.position.y, cam.transform.position.z);

            // Giới hạn vị trí camera
            camTargetPosition.x = Mathf.Clamp(camTargetPosition.x, imgMap.bounds.min.x + cam.orthographicSize * cam.aspect, imgMap.bounds.max.x - cam.orthographicSize * cam.aspect);
            camTargetPosition.y = Mathf.Clamp(camTargetPosition.y, imgMap.bounds.min.y + cam.orthographicSize, imgMap.bounds.max.y - cam.orthographicSize);

            // Di chuyển camera theo thuyền sử dụng MoveTowards
            cam.transform.position = camTargetPosition;
            thuyendi = false;
            yield return new WaitForSeconds(1.3f);
            ChuyenCanh.gameObject.SetActive(false);
            ChuyenCanh.SetBool("ok", false);

        
        }
    }

    public GameObject btnAptrung;
    public void ApTrung()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "ApTrung";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                timeSec = 86400;
                txtTimeApTrung.gameObject.SetActive(true);
                btnAptrung.SetActive(false);
                Animator anim = transform.GetChild(0).transform.Find("LongApRongRua").GetComponent<Animator>();
                anim.Play("ApTrung");
                dem = true;
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
        }
    }
    public void NhanRong()
    {
        if (timeSec <= 0)
        {
            AudioManager.PlaySound("soundClick");
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "NhanRongRua";
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                debug.Log(json.ToString());
                if (json["status"].AsString == "0")
                {
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        GameObject LongAp = transform.GetChild(0).transform.Find("LongApRongRua").gameObject;
                        LongAp.transform.Find("btn").gameObject.SetActive(false);
                        //  GameObject trung = LongAp.transform.Find("Trung").gameObject;
                        // Vector3 vectrung = trung.transform.position;

                        //  GameObject rong = Instantiate(trung, trung.transform.position,Quaternion.identity);
                        //   rong.transform.SetParent(CrGame.ins.trencung);
                        //   rong.transform.position = vectrung;
                        //    rong.GetComponent<SpriteRenderer>().sortingLayerName = "GiaoDien2";

                        GameObject rong = transform.GetChild(0).transform.Find("imgRongRua").gameObject;
                        rong.transform.SetParent(CrGame.ins.trencung) ;

                     
                     //   rong.transform.position = LongAp.transform.position;
                        //QuaBay quabay = rong.AddComponent<QuaBay>();
                        //quabay.vitribay = btnHopQua;
                        rong.gameObject.SetActive(true);
                        rong.transform.LeanMove(btnHopQua.transform.position,0.8f);
                        Destroy(rong,0.8f);
                        yield return new WaitForSeconds(1f);
                        LongAp.SetActive(false);
                    }
                
                }
                else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
            }
        }   
        else
        {
            CrGame.ins.OnThongBaoNhanh("Trứng đang ấp, vui lòng chờ...");
        }
    }
    void DisplayTime()
    {
        //  timeToDisplay += 1;
        float sec = Mathf.FloorToInt(timeSec);
        float minutes = 0;
        int gio = 0;

        while (sec >= 60)
        {
            sec -= 60;
            minutes += 1;
        }
        while (minutes >= 60)
        {
            minutes -= 60;
            gio += 1;
        }
        //float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        txtTimeApTrung.text =
      (gio < 10 ? "0" : "") + gio + ":" +
      (minutes < 10 ? "0" : "") + minutes + ":" +
      (sec < 10 ? "0" : "") + sec;
        //   txttime.text = string.Format("{0:00}:{0:00}:{1:00}",gio, minutes, seconds);
    }
    public void BtnDanhXucTu()
    {
        EventManager.OpenThongBaoChon("Bạn có muốn chiến đấu với Bạch Tuộc Ma để nhận thêm quà và tiếp tục chặng đường hay không?", ChienDau,"Chiến đấu",BoChay,false,"<size=45>Bỏ Chạy</size>");
        void ChienDau()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "DanhXucTu";
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                debug.Log(json.ToString());
                if (json["status"].AsString == "0")
                {
                    CrGame.ins.giaodien.SetActive(false);
                    btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
                  //  Destroy(quan3ConSoc.transform.parent.gameObject);
                    Destroy(giaodiennut1.gameObject);
                    Destroy(MapRaKhoi.gameObject);
                    Destroy(ChuyenCanh.gameObject);
                    Destroy(btnAptrung.gameObject);
                    Destroy(txtTimeApTrung.gameObject);


                    VienChinh.vienchinh.GetDoiHinh("BossXucTu", CheDoDau.XucTu);

                    NetworkManager.ins.vienchinh.TruDo.SetActive(true);

                    NetworkManager.ins.vienchinh.TruXanh.SetActive(true);

                    TaoXucTu(json["NameBoss"].AsString, json["dame"].AsFloat);

                    float chia = json["hp"].AsFloat / 3;

                    float[] hplansu = new float[] { chia, chia, chia };

                    SetHpLanSu(hplansu);
                    // VienChinh.vienchinh.SetBGMap("BGLanSu");
                    VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBachTuoc");
                    btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
                    //Destroy(gameObject);
                    // AllMenu.ins.DestroyMenu(nameof(EventDaiChienThuyQuai));
                    AllMenu.ins.DestroyMenu(nameof(MenuRaKhoi));


                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
        void BoChay()
        {
            EventManager.OpenThongBaoChon("Bạn có muốn sử dụng thêm 1 Túi lương thực để đi đường vòng tránh quái vật hay không?", DongY, "Đồng ý", TuChoi, false, "<size=45>Từ chối</size>");
            
            void DongY()
            {
                JSONClass datasend = new JSONClass();
                datasend["class"] = nameEvent;
                datasend["method"] = "BoChay";
                NetworkManager.ins.SendServer(datasend, Ok);
                void Ok(JSONNode json)
                {
                    debug.Log(json.ToString());
                    if (json["status"].AsString == "0")
                    {
                        XucTuXuatHien.SetActive(false);

                        ThuyenDiChuyen(json["vitriThuyen"][1].AsInt, json["vitriThuyen"][0].AsInt, false, true, json["vitriKhoBau"]);
                        LoadLuongThuc(json["TuiLuongThuc"].AsInt);
                    }
                    else
                    {
                        CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                    }
                }
            }
            void TuChoi()
            {

            }
        }
    }
    public static Action KetQua(KetQuaTranDau kq, bool quayve = false)
    {
        void kqq()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "KetQuaKetLieu";
            datasend["data"]["kq"] = kq.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                //  debug.Log(json.ToString()) ;
                if (json["status"].AsString == "0")
                {

                    if (quayve)
                    {
                        CrGame.ins.OpenMenuRaKhoi();
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
                        GiaoDienPVP.ins.thongtin.text = "Bạn đã đánh bại Boss Xúc Tu";
                        CrGame.ins.OnThongBaoNhanh(json["infoqua"].AsString);
                    }
                    GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
                    GiaoDienPVP.ins.btnSetting.SetActive(true);
                    GiaoDienPVP.ins.spriteWin.SetNativeSize();
                   // CrGame.ins.OnThongBaoNhanh(json["infoqua"].AsString);
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
        return kqq;

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
        tru.actionwin = delegate {
            tru.actionwin = null;
            CrGame.ins.SetPanelThangCap(true);
            CrGame.ins.txtThangCap.text = "";
            VienChinh.vienchinh.OffThangCap();
            Transform xuctu = VienChinh.vienchinh.TeamDo.transform.GetChild(1).transform;
            VienChinh.vienchinh.StartCoroutine(MoveRight());

            IEnumerator MoveRight()
            {
                // Tốc độ di chuyển của đối tượng
                float moveSpeed = 2f;
                // Khoảng cách di chuyển
                float moveDistance = 5f;

                float startX = xuctu.transform.position.x;
                float targetX = startX + moveDistance;
                float elapsedTime = 0f;

                while (xuctu.transform.position.x < targetX)
                {
                    if (xuctu == null) break;
                    // Tính toán khoảng thời gian đã trôi qua
                    elapsedTime += Time.deltaTime;
                    // Tính toán vị trí mới của đối tượng
                    float newX = Mathf.Lerp(startX, targetX, elapsedTime * moveSpeed / moveDistance);
                    xuctu.transform.position = new Vector3(newX, xuctu.transform.position.y, xuctu.transform.position.z);

                    // Chờ một frame
                    yield return null;
                }

                // Đảm bảo đối tượng đã đến đúng vị trí đích
                xuctu.transform.position = new Vector3(targetX, xuctu.transform.position.y, xuctu.transform.position.z);

            }
        };
    }
    private void TaoXucTu(string nameBoss, float dame)
    {
        GameObject LanSu = Instantiate(GetObjectLanSu(nameBoss), transform.position, Quaternion.identity);
        LanSu.transform.SetParent(VienChinh.vienchinh.TeamDo.transform, false);
        LanSu.transform.position = VienChinh.vienchinh.TeamDo.transform.position;
        DragonPVEController dragonPVEController = LanSu.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
        dragonPVEController.dame = dame;
        //   dragonPVEController.giapso = 200;
        // dragonPVEController.xuyengiap = 99999;
        VienChinh.vienchinh.TruDo.GetComponent<SpriteRenderer>().enabled = false;
    }

    public GameObject GetObjectLanSu(string nameBoss)
    {
        return Inventory.LoadObjectResource("GameData/LanSu/" + nameBoss);

    }
    private void HienXucTu(Vector3 vec)
    {
        XucTuXuatHien.SetActive(true);
        vec.y -= 0.5f;
        XucTuXuatHien.transform.position = vec;
    }    
    private void LoadVitriThuyenTruocDo(int hang, int cot)
    {
        Transform tfhangg = PanelAllDao.transform.GetChild(hang);
        Transform tfcott = tfhangg.transform.GetChild(cot);
        Transform btndaoo = null;

        for (int i = 0; i < tfcott.transform.childCount; i++)
        {
            Transform tf = tfcott.transform.GetChild(i);
            if (tf.gameObject.name.Substring(0, 3) == "Dao")
            {
                btndaoo = tf;
                break;
            }
        }
      //  debug.Log("name input " + btndaoo.name);
        string input = btndaoo.name;

        int numberPart = int.Parse(input.Substring(input.Length - 1)) - 1;
        DauX.transform.SetParent(tfcott);
        DauX.GetComponent<RectTransform>().anchoredPosition = allPosDauX[numberPart];
        DauX.SetActive(true);
    }
    private void ScaleThuyen(int x)
    {
        Transform Thuyenn = Thuyen.transform.GetChild(0);
        Vector3 Scale = Thuyenn.transform.localScale;
        if (x == 1) Scale.x = Mathf.Abs(Scale.x);
        else
        {
            if(Scale.x >= 0) Scale.x = -Scale.x;
        } 
            

        Thuyenn.transform.localScale = Scale;
    }
    private void OnChatThuyen(int i)
    {
        GameObject objchat = Thuyen.transform.GetChild(1).gameObject;
        objchat.SetActive(true);
        Text txt = objchat.transform.GetChild(0).GetComponent<Text>();
        Animator anim = objchat.transform.GetChild(1).GetComponent<Animator>();
        if(i == 1)
        {
            txt.text = "Gần kho báu hơn rồi!!";
            anim.Play("Smile");
        }
        else if (i == 2)
        {
            txt.text = "Xa kho báu hơn rồi!!";
            anim.Play("Sad");
        }
        else if (i == 3)
        {
            txt.text = "<size=40>Rất gần\r\nkho báu rồi!\r\nYeah!!!</size>";
            anim.Play("Grin");
        }
        else if (i == 4)
        {
            txt.text = "<size=35>Tìm được kho báu rồi, Quay \r\nvề cảng thôi!</size>";
            anim.Play("Laugh");
        }
        else if (i == 5)
        {
            txt.text = "<size=35> Ối! thuyền chúng ta đang bị Bạch Tuộc Ma tấn công</size>";
            anim.Play("Sad");
        }
       
        objchat.transform.LeanScale(new Vector3(0.9f,0.9f),0.35f);
    }    
    public void OpenRaKhoi()
    {
        Button btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btnchon.enabled = false;

        imgMap = MapRaKhoi.transform.GetChild(0).GetComponent<SpriteRenderer>();
   


        animThuyenTo.Play("Sailing");
        StartCoroutine(delay());
        IEnumerator delay()
        {
         

            yield return new WaitForSeconds(0.4f);

            ChuyenCanh.transform.position = cam.transform.position;
            RectTransform rectTransformm = ChuyenCanh.GetComponent<RectTransform>();
            Vector3 currentPoss = rectTransformm.localPosition;

            // Thay đổi giá trị z thành 0
            currentPoss.z = 0;

            // Gán lại giá trị đã thay đổi vào localPosition
            rectTransformm.localPosition = currentPoss;
            ChuyenCanh.gameObject.SetActive(true);
            ChuyenCanh.Play("chuyencanh");
            yield return new WaitForSeconds(1.8f);
            Transform panel = MapRaKhoi;
            MapMinX = imgMap.transform.position.x - imgMap.bounds.size.x / 2f;
            MapMaxX = imgMap.transform.position.x + imgMap.bounds.size.x / 2f;
            mapMiny = imgMap.transform.position.y - imgMap.bounds.size.y / 2f;
            MapMaxY = imgMap.transform.position.y + imgMap.bounds.size.y / 2f;
          

            giaodiennut1.transform.SetParent(CrGame.ins.trencung.transform);
            giaodiennut1.transform.SetAsFirstSibling();
            giaodiennut1.transform.position = Vector3.zero;
            giaodiennut1.transform.localScale = Vector3.one;
            RectTransform rectTransform = giaodiennut1.transform.GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            // Đặt giá trị Pos Z bằng 0
            Vector3 currentPos = rectTransform.localPosition;
            currentPos.z = 0;
            rectTransform.localPosition = currentPos;

            panel.transform.SetParent(GameObject.FindGameObjectWithTag("vienchinh").transform);
       



            Camera.main.orthographicSize = 5;
            cam.GetComponent<ZoomCamera>().enabled = false;
            GameObject img = panel.gameObject;
            cam.transform.position = new Vector3(img.transform.position.x, img.transform.position.y, -10);
            //  CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).gameObject.SetActive(false);
            Transform dangodao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao);
            dangodao.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
            CrGame.ins.giaodien.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            btnHopQua.transform.SetParent(giaodiennut1.transform);

            Vector3 camTargetPosition = cam.transform.position;
            camTargetPosition = new Vector3(Thuyen.transform.position.x, Thuyen.transform.position.y, cam.transform.position.z);

            // Giới hạn vị trí camera
            camTargetPosition.x = Mathf.Clamp(camTargetPosition.x, imgMap.bounds.min.x + cam.orthographicSize * cam.aspect, imgMap.bounds.max.x - cam.orthographicSize * cam.aspect);
            camTargetPosition.y = Mathf.Clamp(camTargetPosition.y, imgMap.bounds.min.y + cam.orthographicSize, imgMap.bounds.max.y - cam.orthographicSize);

            // Di chuyển camera theo thuyền sử dụng MoveTowards
            cam.transform.position = camTargetPosition;
            btnchon.enabled = true;
            if (XucTuXuatHien.activeSelf) OnChatThuyen(5);
            btnAptrung.transform.SetParent(transform.GetChild(0));
            txtTimeApTrung.transform.SetParent(transform.GetChild(0));
            ChuyenCanh.SetBool("ok", true);
            yield return new WaitForSeconds(1.3f);

            ChuyenCanh.gameObject.SetActive(false);
            ChuyenCanh.SetBool("ok", false);
        }


    }
   // Transform quan3ConSoc;

    public GameObject Thuyen;
    private IEnumerator delaychat(int i)
    {
        yield return new WaitForSeconds(Random.Range(5,7));
        txt[i].transform.parent.gameObject.SetActive(false);
        txt[i].transform.parent.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(3, 5));
        int random = Random.Range(0, list[i].Length);
        if(list[i][random] != "")
        {
            txt[i].transform.parent.gameObject.SetActive(true);
            txt[i].transform.parent.transform.LeanScale(new Vector3(0.7f, 0.7f, 1), 0.35f);

            txt[i].text = list[i][random];
        }
    
        StartCoroutine(delaychat(i));
    }
    private void AnimNhaSangToi()
    {
        if(chonNhaSoc)
        {
            if (nhaSocChu.sprite == nhaSocChuSang)
            {
                nhaSocChu.sprite = NhaSocChuToi;
            }
            else nhaSocChu.sprite = nhaSocChuSang;
        }


        if (chonNhaBacSi)
        {
            if (NhaBacSi.sprite == NhaBacSiSang)
            {
                NhaBacSi.sprite = NhaBacSiToi;
            }
            else NhaBacSi.sprite = NhaBacSiSang;
        }
    }
    public void BtnNhaSocChu(bool b)
    {
        chonNhaSoc = b;
        nhaSocChu.sprite = nhaSocChuSang;

        if(!b)
        {
            nhaSocChu.transform.LeanScale(new Vector3(0.8f,0.8f,1),0.2f);
        }    
        else
        {
            nhaSocChu.transform.LeanScale(new Vector3(0.73f, 0.73f, 1), 0.2f);
        }
    }
    public void BtnNhaBacSi(bool b)
    {
        chonNhaBacSi = b;
        NhaBacSi.sprite = NhaBacSiSang;

        if (!b)
        {
            NhaBacSi.transform.LeanScale(new Vector3(0.8f, 0.8f, 1), 0.2f);
        }
        else
        {
            NhaBacSi.transform.LeanScale(new Vector3(0.73f, 0.73f, 1), 0.2f);
        }
    }
    Vector3 dragOrigin; Camera cam; SpriteRenderer imgMap;
    float MapMinX, MapMaxX, mapMiny, MapMaxY;
    bool drag;GameObject giaodiennut1;
    private bool dem = false;
    void Update()
    {
        if (drag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
                //debug.Log(dragOrigin);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 difrence = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position = Clampcamera(cam.transform.position + difrence);
            }
        }
        if(dem)
        {
            if (timeSec > 0)
            {
                timeSec -= Time.deltaTime;
                DisplayTime();
            }
            else
            {
                dem = false;
                timeSec = 0;
                txtTimeApTrung.text = "";
                Animator anim = transform.GetChild(0).transform.Find("LongApRongRua").GetComponent<Animator>();
                anim.Play("LongApRongRua");
            }
        }    
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
        return new Vector3(NewX, NewY, targetPosition.z);
    }
    public void Drag(bool b)
    {
        drag = b;
    }
    public Text txtTimeApTrung;
    public void VeNha()
    {
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        AudioManager.PlaySound("soundClick");
      //  Destroy(quan3ConSoc.transform.parent.gameObject);
        Destroy(giaodiennut1.gameObject);
        Destroy(MapRaKhoi.gameObject);
        Destroy(ChuyenCanh.gameObject);
        Destroy(btnAptrung.gameObject);
        Destroy(txtTimeApTrung.gameObject);

        Transform dangodao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");

        Vector3 vec = dangodao.transform.position;
        vec.z = -10;
        CrGame.ins.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        AllMenu.ins.DestroyMenu(nameof(MenuRaKhoi));
   //     Destroy(gameObject);
    }    
    public void VeNhaRaKhoi()
    {
        AudioManager.PlaySound("soundClick");
        giaodiennut1.transform.SetParent(MapRaKhoi);
        Vector3 vec = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).transform.position;
        vec.z = -10;
        cam.transform.position = vec;
        btnHopQua.transform.SetParent(transform);
 
        MapRaKhoi.gameObject.SetActive(false);
   //     MapRaKhoi.transform.SetParent(transform,false);
        transform.Find("Panel").gameObject.SetActive(true);
        animThuyenTo.Play("Idle");
        btnAptrung.transform.SetParent(CrGame.ins.trencung);
        btnAptrung.transform.SetSiblingIndex(0);
        txtTimeApTrung.transform.SetParent(CrGame.ins.trencung);
        txtTimeApTrung.transform.SetSiblingIndex(0);

    }
    string nameitemmua;
    Text txtupdate;
    string txtTv(string s)
    {
        switch (s)
        {
            case "TuiLuongThuc": return "Túi lương thực";
        }
        return "";
    }
    private GameObject menumuaitem;
    public void OpenMenuMuaXeng()
    {
        AudioManager.SoundClick();
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        txtupdate = btnchon.transform.parent.GetComponentsInChildren<Text>()[0];
        nameitemmua = btnchon.transform.parent.name;

        GameObject menu = Instantiate(Inventory.LoadObjectResource("GameData/"+nameEvent+"/MenuMuaItem"), transform.position, Quaternion.identity);
        menumuaitem = menu;
        menu.transform.SetParent(CrGame.ins.trencung.transform, false);
        menu.transform.position = CrGame.ins.trencung.transform.position;
        menu.SetActive(true);

    //    GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaItem", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        g.transform.Find("txttanggia").GetComponent<Text>().text = "Giá tăng khi mua nhiều trong ngày, sẽ được reset khi qua ngày mới.";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = txtTv(nameitemmua);// tên giao diện
        Image imgitem = g.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        imgitem.sprite = Resources.Load<Sprite>("GameData/"+nameEvent+"/" + nameitemmua);
        imgitem.SetNativeSize();
        Button btnsangtrai = btn.transform.GetChild(0).GetComponent<Button>();
        btnsangtrai.onClick.AddListener(delegate { CongThemSoLuongMua(-1); });
        Button btnsangPhai = btn.transform.GetChild(1).GetComponent<Button>();
        btnsangPhai.onClick.AddListener(delegate { CongThemSoLuongMua(1); });
        Button btnExit = g.transform.Find("btnExit").GetComponent<Button>();
        btnExit.onClick.AddListener(ExitMenuQueThu);
        Button btnXacNhan = g.transform.Find("btnXacNhan").GetComponent<Button>();
        btnXacNhan.onClick.AddListener(MuaQueThu);
        InputField input = g.transform.Find("InputField").GetComponent<InputField>();
        input.onEndEdit.AddListener(onEndEdit);
    }
    short soluongMuaQueThu = 1;
    private void CongThemSoLuongMua(int i)
    {
        AudioManager.SoundClick();
        debug.Log("Tang so luong " + i);
        if (soluongMuaQueThu + i >= 1)
        {
            GameObject menu =menumuaitem;
            Transform g = menu.transform.GetChild(0);
            InputField input = g.transform.Find("InputField").GetComponent<InputField>();
            soluongMuaQueThu += (short)i;
            XemGiaMuaQueThu();
            input.text = soluongMuaQueThu.ToString();
        }
    }
    private void onEndEdit(string s)
    {
        if (s == "" || s == "0") s = "1";
        if (s.Length > 4) s = "500";
        if (int.Parse(s) >= 500) s = "500";
        debug.Log("onEndEdit " + s);
        GameObject menu =menumuaitem;
        Transform g = menu.transform.GetChild(0);
        InputField input = g.transform.Find("InputField").GetComponent<InputField>();
        soluongMuaQueThu = short.Parse(s);
        XemGiaMuaQueThu();
        input.text = soluongMuaQueThu.ToString();
    }

    public void XemGiaMuaQueThu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemGiaMua";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = menumuaitem;
                if (menu != null)
                {
                   
                    Transform g = menu.transform.GetChild(0);
                    Text txtgia = g.transform.Find("txtGia").GetComponent<Text>();
                    txtgia.text = json["gia"].AsString;



                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void MuaQueThu()
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "MuaXeng";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;

        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                if (menumuaitem != null)
                {
                    ExitMenuQueThu();
                    //  SetXeng(json["soxeng"].AsString);
                    CrGame.ins.OnThongBaoNhanh("Mua thành công!");
                    txtupdate.text = json["soitem"].AsString;
                    if (txtupdate.transform.parent.transform.parent.name == "Item")
                    {
                        string s = json["soitem"].AsString;
                        txtupdate.text = s;
                    }
                    //SetsucXac(json["sucxac"].AsString);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void ExitMenuQueThu()
    {
        Destroy(menumuaitem); soluongMuaQueThu = 1;
    }
    public GameObject menumuakhobauRauDen;
    public void XemKhoBau()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemMuaKhoBau";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                //  EventManager.OpenThongBaoChon(json["txt"].AsString, Mua);
                menumuakhobauRauDen.transform.SetParent(CrGame.ins.trencung);
                menumuakhobauRauDen.gameObject.SetActive(true);
            }
            else if (json["status"].AsString == "2")
            {
                list[1][0] = "<size=35>Khi nào có Bản đồ Kho báu Râu Đen thì hãy đến nói chuyện với ta</size>";
                CrGame.ins.OnThongBaoNhanh("Bạn không có Bản đồ Kho báu!");
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
       
    }
    public void MuaKhoBau(string nameitem)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "MuaKhoBau";
        datasend["data"]["nameitem"] = nameitem;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
              
                if (json["BanDoKhoBau"].AsInt <= 0)
                {
                    list[1][0] = "<size=35>Khi nào có Bản đồ Kho báu Râu Đen thì hãy đến nói chuyện với ta</size>";
                }
                HuyMua();
              //  CrGame.ins.OnThongBaoNhanh("Mua ok");
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }
    public void HuyMua()
    {
        menumuakhobauRauDen.transform.SetParent(transform);
        menumuakhobauRauDen.gameObject.SetActive(false);
    }
    public void OpenMenuNangSao()
    {
        AllMenu.ins.GetCreateMenu("menuNangSaoRong",CrGame.ins.trencung.gameObject,true);
    }
    public void OpenQuan3ConSoc()
    {
        Quan3ConSoc.OpenMenuQuan3ConSoc();
    }
}
