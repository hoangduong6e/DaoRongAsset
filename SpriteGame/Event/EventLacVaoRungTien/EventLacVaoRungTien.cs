using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventLacVaoRungTien : EventManager
{
    public GameObject O1, O2, O22, O4;

    public RectTransform rectTransform, childRectTransform, DayTrung, Trung ; // Tham chiếu đến RectTransform
    public float sizeChangeSpeed = 10f; // Tốc độ thay đổi kích thước
  //  public float maxHeight = 300f;        // Chiều cao tối đa của RectTransform
    public float minHeight = 100f;        // Chiều cao ban đầu (tối thiểu) của RectTransform
    private bool isExpanding = false;     // Biến để theo dõi trạng thái tăng hay giảm
    public Transform objbanhlan;

    public float leftLimit = -10f;  // Giới hạn bên trái
    public float rightLimit = 10f;  // Giới hạn bên phải
    private bool movingRight = true;  // Biến xác định hướng di chuyển
    JSONNode dataAiHienTai,dataNhan;
    public Sprite spriteHopQua;
    public Transform allvitri;
    public Image imgqua;
    private byte soQuaDaCau;
    public Text txtngay, txtTime;
    byte days = 0;

    public int hours = 0;
    public int minutes = 0;
    public int seconds = 0;

    //public float[] xCacO = new float[] 
    //{
    //    -2.32f,
    //    -1.05f,
    //    0.22f,
    //    1.49f,
    //    2.76f 
    //};
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void ABSAwake()
    {

    }
    protected override void DiemDanhOk(JSONNode json)
    {
        SetCanhCamYeuCau(json["data"]["CanhCam"].AsInt, json["data"]["CanhCamYeuCau"].AsInt);
    }
    public void SetCanhCamYeuCau(int canhcamco, int canhcamyeucau)
    {
        Text txt = transform.Find("imgqua").transform.Find("CanhCam").transform.GetChild(0).GetComponent<Text>();
        txt.text = (canhcamco >= canhcamyeucau) ? "<color=lime>" + canhcamco + "/" + canhcamyeucau + "</color>" : "<color=red>" + canhcamco + "/" + canhcamyeucau + "</color>";
    }
    public void ParseData(JSONNode json)
    {
        CrGame.ins.GetComponent<ZoomCamera>().enabled = false; Camera.main.orthographicSize = 5;
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        btnHopQua.transform.SetParent(transform);
        debug.Log(json.ToString());
        dataAiHienTai = json["dataAiHienTai"];
        dataNhan = json["data"]["dataAi"];

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, minHeight);
        SetDayTrung(json["data"]["soQuaDaCau"].AsInt * 4.7f);
        soQuaDaCau = json["data"]["soQuaDaCau"].AsByte;
        LoadDayTrung();
        gameObject.SetActive(true);
       // debug.Log("CanhCam " + json["data"]["allItem"]["CanhCam"].AsString + " CanhCamYeuCau: " + json["CanhCamYeuCau"].AsString);
        SetCanhCamYeuCau(json["data"]["allItem"]["CanhCam"].AsInt, json["CanhCamYeuCau"].AsInt);
        if (json["data"]["nhanrong"].AsBool) animTrungNguSac.gameObject.SetActive(false);
        KhoiTaoOQua();
        days = json["data"]["ngaydangnhap"].AsByte;
        days -= 1;
        hours = json["hours"].AsInt;
        minutes = json["minutes"].AsInt;
        seconds = json["seconds"].AsInt;
        UpdateNgay();
        StartCoroutine(StartClock());
    }

    IEnumerator StartClock()
    {
        IncrementTime();
        while (true)
        {
            yield return new WaitForSeconds(1f); // Đợi 1 giây trước khi tiếp tục.
            IncrementTime();
        }
    }
    void IncrementTime()
    {
        seconds++;

        // Kiểm tra nếu giây vượt quá 59.
        if (seconds >= 60)
        {
            seconds = 0;
            minutes++;
        }

        // Kiểm tra nếu phút vượt quá 59.
        if (minutes >= 60)
        {
            minutes = 0;
            hours++;
        }

        // Kiểm tra nếu giờ vượt quá 23.
        if (hours >= 24)
        {
            hours = 0;
            days++;
           UpdateNgay();
        }

        // In ra thời gian hiện tại trên console.
        string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        txtTime.text = timeFormatted;
    }
    void UpdateNgay()
    {
        txtngay.text = "<color=yellow>" + days + " ngày</color>";
    }    

    bool dangkhoitaoqua = false;
    private void KhoiTaoOQua(bool delayy = false)
    {
        if (dangkhoitaoqua) return;
        dangkhoitaoqua = true;
        debug.Log("Khởi tạo quàaaaaaaaaaaaaaaa");
        Transform imgcay = transform.Find("imgcay");
        GameObject allO = imgcay.transform.Find("allO").gameObject;
        RectTransform child1 = allO.transform.GetChild(5).GetComponent<RectTransform>(); ;//allO.transform.GetChild(0).GetComponent<RectTransform>();
        float PosX = child1.anchoredPosition.x;
        float PosY = child1.anchoredPosition.y;
        float spacingY = 79;

        float spacingX = 79;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            duoccau = false;
            bool delay = true;
           // debug.Log("ô 0: " + dataAiHienTai[0][0]["o"].AsString);
            for (int i = 0; i < dataNhan.Count; i++)
            {
                for (int j = 0; j < dataNhan[i].Count; j++)
                {
                    if (dataNhan[i][j].AsString != "0")
                    {
                        delay = false;
                        break;
                    }
                }
            }
            for (int i = 0; i < dataAiHienTai.Count; i++)
            {
                float y = PosY - (i * spacingY);
                for (var j = 0; j < dataAiHienTai[i].Count; j++)
                {
                    //   debug.Log(dataAiHienTai[i][j]["o"].AsString);
                    GameObject g = null;
                    bool ok = true;
                    string asstr = dataAiHienTai[i][j]["o"].AsString;
                    if (asstr == "1") g = O1;
                    else if (asstr == "2") g = O2;
                    else if (asstr == "22") g = O22;
                    else if (asstr == "4") g = O4;
                    else g = O1;
                    if (i > 0)
                    {
                        if (dataAiHienTai[i - 1][j]["o"].AsString == "2")
                        {
                            ok = false;
                        }
                        if (dataAiHienTai[i - 1][j]["o"].AsString == "4")
                        {
                            ok = false;
                        }
                    }
                    if (j > 0)
                    {
                        if (dataAiHienTai[i][j - 1]["o"].AsString == "22")
                        {
                            ok = false;
                        }
                        if (dataAiHienTai[i][j - 1]["o"].AsString == "4")
                        {
                            ok = false;
                        }
                        if (i > 0)
                        {
                            if (dataAiHienTai[i - 1][j - 1]["o"].AsString == "4")
                            {
                                ok = false;
                            }
                        }
                    }
                    //if(ok)
                    // {
                    GameObject instan = Instantiate(g, transform.position, Quaternion.identity);
                    instan.transform.SetParent(allO.transform.GetChild(j), false);

                    if (ok)
                    {
                        instan.transform.position = allO.transform.position;
                        instan.name = i.ToString();
                        float x = PosX + (j * spacingX);
                        instan.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                        instan.gameObject.SetActive(true);
                        Transform hopqua = instan.transform.GetChild(0).transform.GetChild(0);
                        if (dataAiHienTai[i][j]["qua"].ISNull)
                        {
                            hopqua.gameObject.SetActive(false);
                        }
                        else
                        {
                            // Lấy RectTransform của menu
                            RectTransform rectTransform = hopqua.GetComponent<RectTransform>();
                            // Thiết lập vị trí ban đầu của RectTransform


                            if (!CheckDuocCau(i, j)) rectTransform.GetComponent<Animator>().enabled = false;
                            if (delay || delayy)
                            {
                                rectTransform.transform.SetParent(allO.transform);
                                float ybandau = rectTransform.anchoredPosition.y;
                                Vector2 startPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 580);
                                rectTransform.anchoredPosition = startPosition;

                                // Di chuyển RectTransform từ vị trí ban đầu về vị trí Y = 0
                                Vector2 endPosition = new Vector2(rectTransform.anchoredPosition.x, ybandau);
                                rectTransform.LeanMoveLocal(endPosition, 0.4f); // Thời gian di chuyển là 0.5 giây
                                StartCoroutine(delayy());
                                IEnumerator delayy()
                                {
                                    yield return new WaitForSeconds(0.4f);
                                    Transform child0 = instan.transform.GetChild(0);
                                    rectTransform.transform.SetParent(child0);
                                }
                                //StartDelay2(() => {
                                //    Transform child0 = instan.transform.GetChild(0);
                                //     rectTransform.transform.SetParent(child0);
                                //}, 0.4f);
                            }

                            if (dataNhan[i.ToString()][j.ToString()].AsString != "0")
                            {
                                hopqua.gameObject.SetActive(false);
                            }
                            rectTransform.name = j.ToString();
                        }
                        if (delay || delayy) yield return new WaitForSeconds(0.1f);
                    }

                    // }    
                }
            }
            duoccau = true;
            dangkhoitaoqua = false;
        }
    }
    private bool CheckDuocCau(int i, int j)
    {
        if (i == 0) return true;// nếu ở trên cùng thì luôn luôn được câu
        string strO = dataAiHienTai[i][j]["o"].AsString;
        //  debug.Log("strO " + strO);
        bool ok1 = false;
        for (int ii = i - 1; ii >= 0; ii--) // duyệt từ ô hiện tại đến ô trên cùng xem có ô nào chưa câu hay không
        {
            string astring1 = dataNhan[ii.ToString()][j.ToString()].AsString;
            if (astring1 == "1" || astring1 == "")// nếu như ô cùng vị trí ở bên trên đã câu rồi hoặc là ô bên trên trống, ngay từ lúc đầu k có quà
            {
                ok1 = true; // cho tạm là true
            }
            else return false;

            if (strO == "22" || strO == "4")
            {
                string astring2 = dataNhan[ii.ToString()][(j + 1).ToString()].AsString;
                if (astring2 == "1" || astring2 == "")// nếu như ô cùng vị trí + thêm 1 ô đã câu rồi
                {
                    ok1 = true;
                }

                else return false;
            }    
            if (dataAiHienTai[ii][j + 1]["o"].AsString == "22")
            {
                string astring2 = dataNhan[ii.ToString()][(j + 1).ToString()].AsString;
                if (astring2 == "1" || astring2 == "")// nếu như ô cùng vị trí + thêm 1 ô vì ô hiện tại là ô 2 ngang, ở bên trên đã câu rồi
                {
                    ok1 = true;
                }
                
                else return false;
            }

            if (j > 0)
            {
                if (dataAiHienTai[ii][j - 1]["o"].AsString == "22")
                {
                    string astring3 = dataNhan[ii.ToString()][(j - 1).ToString()].AsString;
                    if (astring3 == "1" || astring3 == "")// nếu như ô cùng vị trí - thêm 1 ô vì ô hiện tại
                    {
                        ok1 = true; // cho tạm là true
                    }
                    else return false;
                }

            }
        }

        return ok1;
        //if(strO == "1" || strO == "2")
        //{

            //}    
            //else if (strO == "22" || strO == "4") // nếu như ô hiện tại là ô 2 ngang
            //{
            //    bool ok22 = false;
            //    for (int ii = i - 1; ii >= 0; ii--) // duyệt từ ô hiện tại đến ô trên cùng xem có ô nào chưa câu hay không
            //    {
            //        string astring1 = dataNhan[ii.ToString()][j.ToString()].AsString;
            //        if (astring1 == "1" || astring1 == "")// nếu như ô cùng vị trí ở bên trên đã câu rồi hoặc là ô bên trên trống, ngay từ lúc đầu k có quà
            //        {
            //            ok22 = true; // cho tạm là true
            //        }
            //        else return false;
            //        string astring2 = dataNhan[ii.ToString()][(j + 1).ToString()].AsString;
            //        if (astring2 == "1" || astring2 == "")// nếu như ô cùng vị trí + thêm 1 ô vì ô hiện tại là ô 2 ngang, ở bên trên đã câu rồi
            //        {
            //            ok22 = true; // cho tạm là true
            //        }
            //        else return false;
            //    //    debug.Log("i " + i + " j " + j + " astring1: " + astring1 + ", astring2: " + astring2);
            //    }

            //    return ok22;
            //}
    }    
    public GameObject khungqua;
    public void XemQua(bool b)
    {
        khungqua.SetActive(b);
        if (!b) return;
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int i = 0;
        int j = 0;
        if (!btnchon.GetComponent<Animator>())
        {
           i =  int.Parse(btnchon.transform.parent.transform.parent.transform.parent.name);
           j = int.Parse(btnchon.transform.parent.name);
        }
        else
        {
           i = int.Parse(btnchon.transform.parent.transform.parent.name);
           j = int.Parse(btnchon.name);
        }
        Image imghopqua = khungqua.transform.GetChild(1).GetComponent<Image>();
        Text txtsoluong = khungqua.transform.GetChild(2).GetComponent<Text>();
        if(dataAiHienTai[i][j]["qua"]["name"].AsString != "")
        {
         
            if (dataAiHienTai[i][j]["qua"]["item"].AsString == "item")
            {
                string namequa = dataAiHienTai[i][j]["qua"]["name"].AsString;
                if (namequa == "Exp%")
                {
                    namequa = "Exp";
                    txtsoluong.text =  dataAiHienTai[i][j]["qua"]["soluong"].AsString + "%";
                }    
                else
                {
                    txtsoluong.text = "x" + dataAiHienTai[i][j]["qua"]["soluong"].AsString;
                }
                imghopqua.sprite = Inventory.LoadSprite(namequa);
               
            }
            else if (dataAiHienTai[i][j]["qua"]["item"].AsString == "itemevent")
            {
                imghopqua.sprite = GetSprite(dataAiHienTai[i][j]["qua"]["name"].AsString);
                txtsoluong.text = "x" + dataAiHienTai[i][j]["qua"]["soluong"].AsString;
            }
            else if (dataAiHienTai[i][j]["qua"]["item"].AsString == "itemrong")
            {
                imghopqua.sprite = Inventory.LoadSpriteRong(dataAiHienTai[i][j]["qua"]["name"].AsString + "1");
                txtsoluong.text = dataAiHienTai[i][j]["qua"]["sao"].AsString + " sao";
            }
        
        }
        else
        {
            imghopqua.sprite = spriteHopQua;
            txtsoluong.text = "Quà ngẫu nhiên";
        }
        imghopqua.SetNativeSize();
        GamIns.ResizeItem(imghopqua);
        //  Transform imgcay = transform.Find("imgcay");
        //  GameObject allO = imgcay.transform.Find("allO").gameObject;
        khungqua.transform.position = btnchon.transform.position;
        debug.Log(dataAiHienTai[i][j].ToString());

    }


    private bool dangkeodai = false, duoccau = false;
    public Transform objNhen;

    public Animator animTrungNguSac;

    private void SetDayTrung(float newHeight)
    {
        // Tính sự thay đổi chiều cao trước khi set chiều cao mới
        float heightChange = newHeight - DayTrung.sizeDelta.y;
        // Điều chỉnh posY để giữ nguyên vị trí giữa
        DayTrung.anchoredPosition = new Vector2(DayTrung.anchoredPosition.x, DayTrung.anchoredPosition.y - heightChange / 2);

        // Set chiều cao mới
        DayTrung.sizeDelta = new Vector2(DayTrung.sizeDelta.x, newHeight);

        float parentHeight = DayTrung.sizeDelta.y;
        float childHeight = childRectTransform.sizeDelta.y;

        // Đặt đối tượng con ở cuối cha
        Trung.anchoredPosition = new Vector2(
            Trung.anchoredPosition.x,
            (-parentHeight / 2 + childHeight / 2)
        );
    }
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    dodaidaytrung += 4.7f;
        //    SetDayTrung(dodaidaytrung);
        //}
      //  SetDayTrung(dodaidaytrung * 99);
        // Kiểm tra nếu biến isExpanding là true thì tăng chiều cao cho đến khi đạt maxHeight
        if (duoccau)
        {
            if (!isExpanding && rectTransform.anchoredPosition.y > 30f || !dangkeodai)// nếu đang không cần gạt và dây tơ đã về vị trí cũ
            {
                // Nếu đang di chuyển sang phải
                if (movingRight)
                {
                    objbanhlan.transform.Translate(Vector3.right * 1.7f * Time.deltaTime);

                    // Kiểm tra nếu đạt giới hạn bên phải
                    if (objbanhlan.transform.position.x >= rightLimit)
                    {
                        movingRight = false; // Đổi hướng di chuyển sang trái
                    }
                }
                else // Nếu đang di chuyển sang trái
                {
                    objbanhlan.transform.Translate(Vector3.left * 1.7f * Time.deltaTime);

                    // Kiểm tra nếu đạt giới hạn bên trái
                    if (objbanhlan.transform.position.x <= leftLimit)
                    {
                        movingRight = true; // Đổi hướng di chuyển sang phải
                    }
                }

            }
        }
  
        // Kiểm tra nếu phím mũi tên xuống được bấm (tăng chiều cao)


        // Kiểm tra nếu biến isExpanding là true và chiều cao chưa đạt đến vị trí của targetGameObject
        if (isExpanding)
        {
            Vector3 targetPosition = targetOCau.transform.position;
            float currentHeight = objNhen.position.y;
            float targetHeight = targetPosition.y + 0.5f;

            // Kiểm tra nếu chưa đạt đến chiều cao của gameobject đích
            if (currentHeight > targetHeight)
            {
                dangkeodai = true;
                // Tăng chiều cao
                float heightChange = sizeChangeSpeed * Time.deltaTime;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + heightChange);

                // Điều chỉnh posY để giữ nguyên vị trí giữa
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - heightChange / 2);
            }
        }

        // Khi không còn mở rộng, giảm chiều cao về minHeight
        if (!isExpanding && rectTransform.sizeDelta.y > minHeight)
        {
         //   targetOCau = null;
            // Giảm chiều cao
            float heightChange = sizeChangeSpeed * 1.2f * Time.deltaTime;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - heightChange);

            // Điều chỉnh posY để giữ nguyên vị trí giữa
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + heightChange / 2);
          //  OGat = -1;
            if(rectTransform.anchoredPosition.y > 30f) dangkeodai = false;
            //if(!duoccau)
            //{
            // //  duoccau = true;
            //    Cau();
            //}
         
                    
        }
        if (targetOCau != null)
        {
            if (objNhen.transform.position.y <= targetOCau.transform.position.y + 0.5f)
            {
                StopExpanding();
                Cau();
            }

        }
        SetViTriNhen();
     


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StopExpanding();
        }
    }

    void SetViTriNhen()
    {
        float parentHeight = rectTransform.sizeDelta.y;
        float childHeight = childRectTransform.sizeDelta.y;

        // Đặt đối tượng con ở cuối cha
        childRectTransform.anchoredPosition = new Vector2(
            childRectTransform.anchoredPosition.x,
            -parentHeight / 2 + childHeight / 2
        );
    }
    // Gọi hàm này để ngừng mở rộng và bắt đầu giảm chiều cao về ban đầu
    public void StopExpanding()
    {
        isExpanding = false;
    }
    public Animator objCanGat;

    //int OGat = -1;
    // Transform objcau; 
    // float[] allvitriX = new float[] { -2.36f,-1.09f,0.17f,1.43f,2.73f};   
    private Transform targetOCau;
    public Transform duoicung;
    public void CanGat()
    {
        if (!duoccau) return;
        if (!isExpanding && rectTransform.anchoredPosition.y > 30)
        {
            duoccau = false;

            float xbanhlan = Math.Abs(objbanhlan.transform.position.x);
            int o = FindClosestChild().GetSiblingIndex();// lấy ô theo hàng ngang
            debug.Log("Ô " + o);
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "Cau";
            datasend["data"]["o"] = o.ToString();
            NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    debug.Log(json.ToString());

                    Transform imgcay = transform.Find("imgcay");
                    GameObject allO = imgcay.transform.Find("allO").gameObject;
                    datacau = json["datacau"];
                    if (datacau["o"].AsString != "5" && datacau["o"].AsString != "5")
                    {
                        targetOCau = allO.transform.Find(datacau["o"].AsString).transform.Find(datacau["hang"].AsString);
                    }
                    else targetOCau = duoicung;
                    debug.Log("targetOCau: " + targetOCau.name);
                    isExpanding = true;  // Bắt đầu tăng chiều cao
                   // duoccau = true;
                    objCanGat.Play("objCanGat");
                    if (datacau["quaai"].AsBool)
                    {
                        dataAiHienTai = new JSONNode();
                        dataNhan = new JSONNode();
                        dataAiHienTai = json["dataAiHienTai"];
                        dataNhan = json["dataAi"];
                    }    
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                    duoccau = true;
                }
            }
        }
    }
    JSONNode datacau;
    void Cau()
    {
        if (datacau["duoccau"].AsBool)
        {
            debug.Log("Cau");
            Animator anim = targetOCau.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
            if (anim.enabled == false) anim.enabled = true;
            anim.Play("hopquamo");
            StartDelay(() => {
                if (anim != null)
                {
                    anim.gameObject.SetActive(false);
                }

            }, 1f);
            if (!datacau["quaai"].AsBool) dataNhan[datacau["hang"].AsString][datacau["o"].AsString] = "1";

            Transform imgcay = transform.Find("imgcay");
            GameObject allO = imgcay.transform.Find("allO").gameObject;

            GameObject qua = Instantiate(imgqua.gameObject, transform.position, Quaternion.identity);
            qua.transform.SetParent(transform, false);
            Image imghopqua = qua.GetComponent<Image>();

            if (datacau["qua"]["item"].AsString == "item")
            {
                string namequa = datacau["qua"]["name"].AsString;
                if (namequa == "Exp%") namequa = "Exp";
                imghopqua.sprite = Inventory.LoadSprite(namequa);
            }
            else if (datacau["qua"]["item"].AsString == "itemevent")
            {
                imghopqua.sprite = GetSprite(datacau["qua"]["name"].AsString);
            }
            else if (datacau["qua"]["item"].AsString == "itemrong")
            {
                imghopqua.sprite = Inventory.LoadSpriteRong(datacau["qua"]["name"].AsString + "1");
            }
            imghopqua.SetNativeSize();
            imghopqua.transform.position = anim.transform.position;
            QuaBay quabay = imghopqua.AddComponent<QuaBay>();
            quabay.enabled = false;
            quabay.gameObject.SetActive(true);
            SetDayTrung(datacau["soQuaDaCau"].AsInt * 4.7f);
            soQuaDaCau = datacau["soQuaDaCau"].AsByte;
            if(soQuaDaCau >= 99 && animTrungNguSac.gameObject.activeInHierarchy)
            {
                Image[] images = new Image[] {
                    animTrungNguSac.transform.parent.transform.GetChild(1).GetComponent<Image>(),
                    animTrungNguSac.transform.parent.transform.parent.GetComponent<Image>(),
                    animTrungNguSac.transform.parent.transform.parent.transform.parent.GetComponent<Image>()
                };

                foreach (Image img in images)
                {
                    FadeOut(img);
                }

                void FadeOut(Image img)
                {
                    LeanTween.value(gameObject, img.color.a, 0f, 0.5f).setOnUpdate((float alpha) =>
                    {
                        Color newColor = img.color;
                        newColor.a = alpha;
                        img.color = newColor;
                    }).setOnComplete(() => {
                        debug.Log($"Fade out complete for {img.gameObject.name}");
                    });
                }
                animTrungNguSac.Play("TrungIdle");
            }
            else
            {
                //   LoadDayTrung();
                animTrungNguSac.Play("TrungDrop");
              
            }
            Animator animDayLac = DayTrung.transform.GetChild(0).GetComponent<Animator>();
            animDayLac.Play("TrungLac");

 
            //DayTrung.transform.LeanScale(new Vector3(1.6f, 1.03f, 0), 0.3f);
            //StartDelay(() =>
            //{
            //    DayTrung.transform.LeanScale(new Vector3(1.6f, 1f, 0), 0.3f);
            //}, 0.3f);
            StartCoroutine(delayyy());
            IEnumerator delayyy()
            {
                yield return new WaitForSeconds(0.6f);
                quabay.vitribay = btnHopQua;
                quabay.enabled = true;

                if (datacau["quaai"].AsBool)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < allO.transform.GetChild(i).transform.childCount; j++)
                        {
                            Destroy(allO.transform.GetChild(i).transform.GetChild(j).gameObject);
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                    KhoiTaoOQua(true);
                }
                else 
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < allO.transform.GetChild(i).transform.childCount; j++)
                        {
                            if (allO.transform.GetChild(i).transform.GetChild(j).gameObject.activeSelf)
                            {
                                Animator animmm = allO.transform.GetChild(i).transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
                                if (animmm.gameObject.activeSelf)
                                {
                                    //  debug.Log("i: " + allO.transform.GetChild(i).transform.GetChild(j).name + " j: " + animmm.name);
                                    if (CheckDuocCau(int.Parse(allO.transform.GetChild(i).transform.GetChild(j).name), int.Parse(animmm.name)))
                                    {
                                        animmm.enabled = true;
                                    }
                                }
                            }
                        }
                    }
                    duoccau = true;
                }
                //   HopQua.ins.Them1Qua();
            }
            //StartDelay(() => {
             
            //}, 0.6f);
            SetCanhCamYeuCau(datacau["CanhCam"].AsInt, datacau["CanhCamYeuCau"].AsInt);
     
        }
        else duoccau = true;
    }
    Transform FindClosestChild()
    {
        Transform closestChild = null;
        float minDistance = Mathf.Infinity;
        Vector3 targetPosition = objbanhlan.transform.position;

        // Duyệt qua các child của allvitri
        foreach (Transform child in allvitri)
        {
            float distance = Vector3.Distance(child.position, targetPosition);

            // Kiểm tra nếu khoảng cách này nhỏ hơn khoảng cách đã lưu
            if (distance < minDistance)
            {
                minDistance = distance;
                closestChild = child;
            }
        }

        return closestChild;
    }

    private GameObject menumuaitem;
    string nameitemmua;
    Text txtupdate;
    public void OpenMenuMuaXeng()
    {
        AudioManager.SoundClick();
          GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        txtupdate = btnchon.transform.GetChild(0).GetComponent<Text>();
          nameitemmua = btnchon.name;
      //  nameitemmua = "CanhCam";

        GameObject menu = Instantiate(Inventory.LoadObjectResource("GameData/" + nameEvent + "/MenuMuaItem"), transform.position, Quaternion.identity);
        menumuaitem = menu;
        menu.transform.SetParent(transform, false);
        //    menu.transform.position = CrGame.ins.trencung.transform.position;
        menu.SetActive(true);

        //    GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaItem", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        g.transform.Find("txttanggia").GetComponent<Text>().text ="Giá tăng khi mua nhiều trong ngày, sẽ được reset khi qua ngày mới.";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = "Mua Item";
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
            GameObject menu = menumuaitem;
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
        if (s.Length > 4) s = "1000";
        if (int.Parse(s) > 1000) s = "1000";
        debug.Log("onEndEdit " + s);
        GameObject menu = menumuaitem;
        Transform g = menu.transform.GetChild(0);
        InputField input = g.transform.Find("InputField").GetComponent<InputField>();
        soluongMuaQueThu = short.Parse(s);
        XemGiaMuaQueThu();
        input.text = soluongMuaQueThu.ToString();
    }
    public void XemGiaMuaQueThu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] =nameEvent;
        datasend["method"] = "XemGiaMua";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
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
        datasend["class"] =  nameEvent;
        datasend["method"] = "MuaXeng";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;

        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
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
                    //txtupdate.text = json["soitem"].AsString;
                    //if (txtupdate.transform.parent.transform.parent.name == "Item")
                    //{
                    //    string s = json["soitem"].AsString;
                    //    txtupdate.text = s;
                    //}
                    //SetsucXac(json["sucxac"].AsString);

                    int HoaTuyet = 0;
                    if (Inventory.ins.ListItemThuong.ContainsKey("itemHoaTuyet")) HoaTuyet = int.Parse(Inventory.ins.ListItemThuong["itemHoaTuyet"].transform.GetChild(0).GetComponent<Text>().text);
                    string YeuCau = txtupdate.text.Split("/")[1].Split("<")[0];
                    SetCanhCamYeuCau(int.Parse(json["soitem"].AsString), int.Parse(YeuCau));
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
    short id;
    public void XemTrung(bool b)
    {
        if (!b)
        {
            CrGame.ins.OffThongBaoNhanh(id);
            return;
        }
        string s = "";
        if (soQuaDaCau < 99) s = "Bạn cần gắp thêm <color=yellow>" + (99 - soQuaDaCau) + " món quà</color> nữa để có thể lấy được trứng";
        else
        {
            s = "Click để nhận Rồng!";
        }
        id = (short)s.Length;
        CrGame.ins.OnThongBaoNhanh(s,2,false);
    }    
    private void LoadDayTrung()
    {
        if(soQuaDaCau>=99)
        {
            animTrungNguSac.transform.parent.transform.GetChild(1).GetComponent<Image>().enabled = false;
            animTrungNguSac.transform.parent.transform.parent.GetComponent<Image>().enabled = false;
            animTrungNguSac.transform.parent.transform.parent.transform.parent.GetComponent<Image>().enabled = false;
            animTrungNguSac.Play("TrungIdle");
        }
    }
    public void NhanTrung()
    {
        if (soQuaDaCau >= 99)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = nameEvent;
            datasend["method"] = "NhanTrung";
            NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    Image imgtoidan = transform.Find("PanelToiDan").GetComponent<Image>();
                    imgtoidan.gameObject.SetActive(true);
                    animTrungNguSac.transform.SetParent(imgtoidan.transform);
                  // animTrungNguSac.GetComponent<ButtonEvents>().enabled = false;
                    animTrungNguSac.GetComponent<Image>().raycastTarget = false;
                    animTrungNguSac.transform.LeanMove(Vector3.zero, 1.5f);

                    // Đặt màu bắt đầu với r = 0, g = 0, b = 0, a = 0
                    Color startColor = new Color(0f, 0f, 0f, 0f);
                    imgtoidan.color = startColor;

                    // Sử dụng LeanTween để làm sáng dần (fade in) từ alpha 0 đến 150
                    LeanTween.value(gameObject, imgtoidan.color.a, 150f / 255f, 1.5f).setOnUpdate((float alpha) =>
                    {
                        // Cập nhật giá trị alpha dần về 150/255
                        Color newColor = imgtoidan.color;
                        newColor.a = alpha;
                        imgtoidan.color = newColor;
                        animTrungNguSac.Play("TrungNo");
                   
                    }).setOnComplete(() => {
                        //   debug.Log($"Fade in complete for {imgtoidan.gameObject.name}");
                        StartDelay(() => {
                            imgtoidan.transform.GetChild(0).gameObject.SetActive(true);
                            imgtoidan.GetComponent<Button>().enabled = true;
                        }, 0.7f);
                    });
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                    duoccau = true;
                }
            }
        }
    }
    public void OpenGiaoDienSucXac()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetDataGiaoDienMoTrung";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
               // GameObject menu = AllMenu.ins.transform.Find("GiaoDienMoTrung").gameObject;
                GameObject menu = GetCreateMenu("GiaoDienMoTrung", AllMenu.ins.transform,false,transform.GetSiblingIndex()+1);
                menu.GetComponent<GiaoDienMoTrungRungTien>().ParseData(json);
                
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void OpenMenuNhiemvu()
    {
        AudioManager.SoundClick();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "GetNhiemVu";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject MenuNhiemVu = GetCreateMenu("MenuNhiemVu", CrGame.ins.trencung, true);
                GameObject AllNhiemVu = MenuNhiemVu.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject obj = AllNhiemVu.transform.GetChild(0).gameObject;
                for (int i = 0; i < json["allNhiemvu"].Count; i++)
                {
                    //debug.Log(json["allNhiemvu"][i].ToString());
                    GameObject instan = Instantiate(obj, transform.position, Quaternion.identity);
                    instan.transform.SetParent(AllNhiemVu.transform, false);
                    Text txtnv = instan.transform.GetChild(0).GetComponent<Text>();
                    txtnv.text = json["allNhiemvu"][i]["namenhiemvu"].Value;

                    Text txttiendo = instan.transform.GetChild(1).GetComponent<Text>();
                    Text txtphanthuong = instan.transform.GetChild(2).GetComponent<Text>();
                    if (int.Parse(json["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["allNhiemvu"][i]["maxnhiemvu"].Value))
                    {
                        txttiendo.text = "<color=#00ff00ff>" + GamIns.FormatCash(json["allNhiemvu"][i]["dalam"].AsInt) + "/" + GamIns.FormatCash(json["allNhiemvu"][i]["maxnhiemvu"].AsInt) + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + GamIns.FormatCash(json["allNhiemvu"][i]["dalam"].AsInt) + "/" + GamIns.FormatCash(json["allNhiemvu"][i]["maxnhiemvu"].AsInt) + "</color>";
                    }
                    txtphanthuong.text = json["allNhiemvu"][i]["qua"]["soluong"].AsString;
                    Image img = instan.transform.GetChild(3).GetComponent<Image>();
                    img.sprite = GetSprite(json["allNhiemvu"][i]["qua"]["name"].AsString);

                    img.SetNativeSize();
                    instan.SetActive(true);
                }
                MenuNhiemVu.transform.GetChild(0).transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate { DestroyMenu("MenuNhiemVu"); });

            }
        }
    }


    public void VeNha()
    {
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        AudioManager.PlaySound("soundClick");

        Transform dangodao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");

        Vector3 vec = dangodao.transform.position;
        vec.z = -10;
        CrGame.ins.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
    //    gameObject.SetActive(false);
       AllMenu.ins.DestroyMenu("MenuEventLacVaoRungTien");
        //     Destroy(gameObject);
    }

}