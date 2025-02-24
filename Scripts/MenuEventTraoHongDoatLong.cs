using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Image imgHoaHong = HoaHong.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            Animator animNguSac = HoaNguSac.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
           
            HoaHong.name = datHoaHong[i]["hoa"].AsString;
            HoaNguSac.name = dataHoaNguSac[i]["hoa"].AsString;

            SetHoaHongNo(i);
            //if(statushoaHong == 1)
            //{
            //    imgHoaHong.sprite = GetSprite("nu"+datHoaHong[i]["hoa"].AsString);
            //    imgHoaHong.SetNativeSize();
            //}
            //else if(statushoaHong == 2)
            //{
            //     imgHoaHong.sprite = GetSprite("hoa"+datHoaHong[i]["hoa"].AsString);
            //     imgHoaHong.SetNativeSize();
            //}
            if(dataHoaNguSac[i]["hoa"].AsString == "1")
            {
                animNguSac.runtimeAnimatorController = hoaNguSac1;
            }
            else animNguSac.runtimeAnimatorController = hoaNguSac2;
        }
        nextDecreaseTime = Time.time + 1f; // Đặt thời gian đầu tiên để trừ
        isTruGiay = true;
        gameObject.SetActive(true);
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
    public void BtnHoaNguSac()
    {

    }   
    public void BtnHoaHong()
    {

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

}
