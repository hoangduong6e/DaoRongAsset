using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

   //JSONClass datasend = new JSONClass();
   //         datasend["class"] = "EventTraoHongDoatLong";
   //         datasend["method"] = "ThuHoach";
   //         datasend["data"]["idrong"] = gameObject.name;
   //         //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
   //         NetworkManager.ins.SendServer(datasend, Ok);
   //         void Ok(JSONNode json)
   //         {
   //             if (json["status"].AsString == "0")
   //             {

   //             }
   //             else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

   //         }
public class MenuEventTraoHongDoatLong : EventManager
{
    private float nextDecreaseTime;
    private bool isTruGiay = false;

    public RectTransform imageRectTransform; // Tham chiếu đến RectTransform của ảnh
    public Transform allHoaHong, allHoaNguSac;
    public GameObject panelInfoHoa;
    public RuntimeAnimatorController hoaNguSac1, hoaNguSac2;
    private float[] TimeHoaHongConLai = new float[15];
    private float[] MaxTimeHoaHong = new float[15];

    private byte[] statusHoaHong = new byte[15];
    public Text txtTimeHoaHong;

    private  byte indexHoaHongChon = 0;
    public Transform KhungHoa;
    public GameObject txtAnim;
    public void ParseData(JSONNode json)
    {
        JSONNode data = json["data"];
        JSONNode datHoaHong = data["allHoaHong"];
        JSONNode dataHoaNguSac = data["allHoaNguSac"];
        for(int i = 0; i < 15;i++)
        {
            TimeHoaHongConLai[i] = json["TimeHoaHongConLai"][i].AsFloat;
            MaxTimeHoaHong[i] = json["MaxTimeHoaHong"][i].AsFloat;
            Transform HoaHong = allHoaHong.transform.GetChild(i);
           
            Transform HoaNguSac = allHoaNguSac.transform.GetChild(i);
             HoaNguSac.gameObject.SetActive(data["allHoaNguSac"][i]["active"].AsBool);
            Image imgHoaHong = HoaHong.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            Animator animNguSac = HoaNguSac.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
           
            HoaHong.name = datHoaHong[i]["hoa"].AsString;
            HoaNguSac.name = dataHoaNguSac[i]["hoa"].AsString;

            SetHoaHongNo(i);
           
            if(dataHoaNguSac[i]["hoa"].AsString == "1")
            {
                animNguSac.runtimeAnimatorController = hoaNguSac1;
            }
            else animNguSac.runtimeAnimatorController = hoaNguSac2;
        }
        nextDecreaseTime = Time.time + 1f; // Đặt thời gian đầu tiên để trừ
        isTruGiay = true;
        KhungHoa.transform.Find("txtTimeEvent").GetComponent<Text>().text = json["txtTimeEvent"].AsString;
        SetTxtHoaHong(data["HoaHong"].AsString);
        SetTxtHoaNguSac(data["HoaNguSac"].AsString);
        SetLuotHaiNguSacFree(data["luotHaiNguSacFree"].AsString);
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        btnHopQua.transform.SetParent(CrGame.ins.trencung.transform);
        gameObject.SetActive(true);
    }
    private void SetLuotHaiNguSacFree(string luot)
    {
        transform.Find("txtFree").GetComponent<Text>().text = "Bạn có <color=lime>"+luot+" lượt</color> hái hoa ngũ sắc <color=lime>miễn phí</color>";
    }
    private void InsTxtAnim(Transform tf)
    {
        GameObject txtAnimClone = Instantiate(txtAnim,transform.position,Quaternion.identity);
        txtAnimClone.transform.SetParent(transform,false);
        txtAnimClone.transform.position = new Vector3(tf.transform.position.x + 1,tf.transform.position.y,tf.transform.position.z);
        txtAnimClone.SetActive(true);
        Destroy(txtAnimClone,3f);
    }

    private void SetTxtHoaHong(string sl, bool txtanim = false)
    {
        Transform txt = KhungHoa.transform.Find("txtHoaHongThuong");
          txt.GetComponent<Text>().text = "<color=orange>Hoa hồng thường</color>: <color=yellow>"+sl+"</color> bông";
        if(txtanim) InsTxtAnim(KhungHoa.transform.Find("HoaHongThuong"));
    }
    private void SetTxtHoaNguSac(string sl, bool txtanim = false)
    {
         Transform txt = KhungHoa.transform.Find("txtHoaNguSac");
          txt.GetComponent<Text>().text = "<color=orange>Hoa Ngũ Sắc</color>: <color=yellow>"+sl+"</color> bông";
        if(txtanim) InsTxtAnim(KhungHoa.transform.Find("HoaNguSac"));
    }
    private void SetHoaHongNo(int i)
    {
            if(TimeHoaHongConLai[i] <= 0 && statusHoaHong[i] != 2)
            {
                statusHoaHong[i] = 2;//đã nở
               
                SetSpriteHoaHong(i,"hoa");
            }
            else if(TimeHoaHongConLai[i] <= MaxTimeHoaHong[i] / 2 && statusHoaHong[i] != 1)
            {
                statusHoaHong[i] = 1;// nở thành nụ
                SetSpriteHoaHong(i,"nu");
            }
            else
            {
                statusHoaHong[i] = 0;// chưa nở
            }
    }

    private void SetSpriteHoaHong(int i,string StrSprite)
    {
        Transform HoaHong = allHoaHong.transform.GetChild(i);
        string hoa = HoaHong.name;
        Image imgHoaHong = HoaHong.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        imgHoaHong.sprite = GetSprite(StrSprite+hoa);
        imgHoaHong.SetNativeSize();
    }
    private void Update()
    {
        if(isTruGiay)
        {
            if (Time.time >= nextDecreaseTime)
            {
                for(int i = 0; i < TimeHoaHongConLai.Length;i++)
                {
                     if(TimeHoaHongConLai[i] > 0)
                     {
                        TimeHoaHongConLai[i] -= 1f;
                        SetHoaHongNo(i);
                     }
                }
                nextDecreaseTime += 1f; // Đặt mốc thời gian tiếp theo
                //Debug.Log("Time: " + time);
             }
          
             txtTimeHoaHong.text = TimeHoaHongConLai[indexHoaHongChon] + " giây";
        }
    }
    protected override void ABSAwake()
    {
       
    }
    void Start()
    {


      //  AdjustImageSize();
    }
   void AdjustImageSize()
    {
       float screenHeight = Screen.height;
    float worldHeight = GetWorldHeight(imageRectTransform);

    Debug.Log("📏 Screen Height: " + screenHeight);
    Debug.Log("🖼️ World Height (Image): " + worldHeight);
    Debug.Log("📏 Image Local Scale: " + imageRectTransform.localScale);

    if (worldHeight < screenHeight)
    {
        float scaleFactor = screenHeight / worldHeight;
        imageRectTransform.localScale *= scaleFactor;
        Debug.Log("✅ Adjusted Image Scale: " + imageRectTransform.localScale);
    }
    }

    float GetWorldHeight(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return corners[1].y - corners[0].y;
    }
    protected override void DiemDanhOk(JSONNode json)
    {

    }

    private JSONClass DataSendThuHoach(int index,string loaihoa)
    {
          JSONClass datasend = new JSONClass();
            datasend["class"] = "EventTraoHongDoatLong";
            datasend["method"] = "ThuHoach";
            datasend["data"]["index"] = index.ToString();
            datasend["data"]["loai"] = loaihoa;
        return datasend;
    }
    private void quaBay(Transform parent,string loaihoa = "HoaHongThuong")
    {
                    GameObject hoaClone = Instantiate(parent.gameObject,transform.position,Quaternion.identity);
                    hoaClone.transform.SetParent(transform,false);
                    hoaClone.transform.position = parent.transform.position;
                    Transform tfHoaHong = transform.Find("KhungHoa").transform.Find(loaihoa);
                    QuaBay quabay = hoaClone.AddComponent<QuaBay>();
                    quabay.vitribay = tfHoaHong.gameObject;

    }
    bool xacnhanHaiNguSac = false;
    public void BtnHoaNguSac()
    {
         GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
          Transform parent = g.transform.parent.transform.parent;
          int index = parent.transform.GetSiblingIndex();

        
          indexHoaHongChon = (byte)index;
        Send();
         // string namesprite = g.GetComponent<Image>().sprite.name;  
        void Send()
        {
              JSONClass datasend = DataSendThuHoach(index,"HoaNguSac");
            datasend["data"]["xacnhan"] = xacnhanHaiNguSac.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    xacnhanHaiNguSac = false;
                    quaBay(parent,"HoaNguSac");
                    SetTxtHoaNguSac(json["HoaNguSac"].AsString,true);
                    parent.gameObject.SetActive(false);
                    SetLuotHaiNguSacFree(json["luotHaiNguSacFree"].AsString);

                    if(json["reset"].AsBool)
                    {
                        //   Vector3 vecbandau = instan.transform.position;
                        //instan.transform.position = new Vector3(vecbandau.x, vecbandau.y + 2);
                        //instan.transform.LeanMove(vecbandau, 0.3f);
                        StartCoroutine(delay());
                        IEnumerator delay()
                        {
                              JSONNode allHoaNguSacReset = json["allHoaNguSacReset"];
                        for(int i = 0; i < 15;i++)
                        {
                             //
                              Transform HoaNguSac = allHoaNguSac.transform.GetChild(i);
                              Vector3 vecbandau = HoaNguSac.transform.position;
                               HoaNguSac.transform.position = new Vector3(vecbandau.x, vecbandau.y + 2);
                              HoaNguSac.gameObject.SetActive(true);
                             
                              Animator animNguSac = HoaNguSac.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
           
         
                              HoaNguSac.name = allHoaNguSacReset[i]["hoa"].AsString;

     
           
                              if(allHoaNguSacReset[i]["hoa"].AsString == "1")
                              {
                                 animNguSac.runtimeAnimatorController = hoaNguSac1;
                              }
                              else animNguSac.runtimeAnimatorController = hoaNguSac2;
                                HoaNguSac.transform.LeanMove(vecbandau, 0.3f);

                            yield return new WaitForSeconds(0.11f);
                        }
                        }
                      
                        
                        debug.Log("Reset Hoa Ngũ Sắc");
                    }
                }
                else if(json["status"].AsString == "2")
                {
                   EventManager.OpenThongBaoChon(json["message"].AsString,()=>{xacnhanHaiNguSac = true; Send();});
                }
                else
                {
                     xacnhanHaiNguSac = false;
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }


            }
        }
          
    }   
    public void BtnHoaHong()
    {
          GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
          Transform parent = g.transform.parent.transform.parent;
          int index = parent.transform.GetSiblingIndex();
          if(statusHoaHong[index] != 2)
          {
            CrGame.ins.OnThongBaoNhanh("Chưa được thu hoạch!");
            return;
          }
          indexHoaHongChon = (byte)index;
         // string namesprite = g.GetComponent<Image>().sprite.name;  
            NetworkManager.ins.SendServer(DataSendThuHoach(index,"HoaHong"), Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                     quaBay(parent);
                     SetTxtHoaHong(json["HoaHong"].AsString,true);
                     TimeHoaHongConLai[index] = json["TimeYeuCau"].AsFloat;
                     MaxTimeHoaHong[index] = TimeHoaHongConLai[index];
                     parent.name = json["hoaNew"]["hoa"].AsString;
                     Image img = g.GetComponent<Image>();
                     img.sprite = GetSprite("mamhoa");
                     img.SetNativeSize();
                }
                else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

            }

    }
    private void SetTransformPanelInfoHoa(Transform tf)
    {
          Vector3 vec = tf.transform.position;
            if(vec.y > 2)// nếu như ở bên trên quá thì cho hiện ở bên dưới
            {
                vec = new Vector3(vec.x,vec.y-2,vec.z);
            }
            else if(vec.y < 2)
            {
                 vec = new Vector3(vec.x,vec.y+2,vec.z);
            }
            else
            {
                 vec = new Vector3(vec.x,vec.y+2,vec.z);
            }
            panelInfoHoa.transform.position = vec;
    }
    public void XemInfoHoaHong(bool b)
    {
        if(b)
        {
            GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            Transform parent = g.transform.parent.transform.parent;
            int index = parent.transform.GetSiblingIndex();
            indexHoaHongChon = (byte)index;
            string namesprite = g.GetComponent<Image>().sprite.name;
           
            string namehoa = "<color=lime>"+GetNameHoa(parent.name)+"</color>";
            string info = "";
            string time = "Sẽ nở thành hoa sau";
            if(namesprite.Contains("mamhoa"))
            {
                info = "Hạt giống bé nhỏ";
                txtTimeHoaHong.gameObject.SetActive(true);
            }
            else if(namesprite.Contains("nu"))
            {
                info = "Nụ hoa tươi đẹp";
                txtTimeHoaHong.gameObject.SetActive(true);

            }
            else if(namesprite.Contains("hoa"))
            {
                time = "";
                info = "Hoa đã nở, click để hái nào!";
                txtTimeHoaHong.gameObject.SetActive(false);
            }
            
            panelInfoHoa.transform.GetChild(0).GetComponent<Text>().text = namehoa+"\n"+info+"\n" + time;
            txtTimeHoaHong.text = TimeHoaHongConLai[index] + " giây";
            SetTransformPanelInfoHoa(parent);
        }
        panelInfoHoa.SetActive(b);
    }
    public void XemInfoHoaNguSac(bool b)
    {
        if(b)
        {
            GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            Transform parent = g.transform.parent.transform.parent;
            int index = parent.transform.GetSiblingIndex();
            indexHoaHongChon = (byte)index;
          //  string namesprite = g.GetComponent<Image>().sprite.name;
           
            string namehoa = "<color=lime>Hoa Ngũ Sắc</color>";
            string time = "";
             string info = "Hoa đã nở, click để hái nào!";
             txtTimeHoaHong.gameObject.SetActive(false);
            
            panelInfoHoa.transform.GetChild(0).GetComponent<Text>().text = namehoa+"\n"+info+"\n" + time;
            txtTimeHoaHong.text = TimeHoaHongConLai[index] + " giây";
            SetTransformPanelInfoHoa(parent);

        }
        panelInfoHoa.SetActive(b);
    }
    private string GetNameHoa(string id)
    {
        switch(id)
        {
            case "1": return "Hoa Hồng Cam";
            case "2": return "Hoa Hồng Tím";
            case "3": return "Hoa Hồng Vàng";
            case "4": return "Hoa Hồng Xanh Lá";
            case "5": return "Hoa Hồng Xanh Dương";
            case "6": return "Hoa Hồng Đỏ";
            case "7": return "Hoa Hồng Trắng";
            default: return "";
        }
    }
    public void OpenMenuDoiHoa()
    {

    }
    public void OpenMenuThueSoc()
    {

    }
}
