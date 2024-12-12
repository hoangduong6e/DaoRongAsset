using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class Quan3ConSoc : MonoBehaviour
{

    private string[] Quan3ConSocChat = new string[] { "Luôn có hàng mới mỗi ngày. Nhớ ghé mua nhé!","","","" };
    [SerializeField]
    private Text txt;
    private Transform quan3ConSoc;
    GameObject hopqua;

    [SerializeField] Sprite[] spriteChuaChon;
    [SerializeField] Sprite[] spriteDangChon;

    [SerializeField] Transform allListItem;

    [SerializeField] Transform allBtn;

    public JSONNode itemYeuCau;
    public JSONNode SoitemYeuCau;
    private byte tabhientai = 0;

    Text txtInfoItem;
    public byte OpenTab
    {
        set
        {
            for (int i = 0; i < allBtn.transform.childCount; i++)
            {
                allBtn.transform.GetChild(i).GetComponent<Image>().sprite = spriteChuaChon[i];
                allListItem.transform.GetChild(i).gameObject.SetActive(false);
            }
            allBtn.transform.GetChild(value).GetComponent<Image>().sprite = spriteDangChon[value];

           allListItem.transform.GetChild(value).gameObject.SetActive(true);
           tabhientai = value;

             Image imgIconItem = quan3ConSoc.transform.GetChild(0).GetComponent<Image>();
             imgIconItem.sprite = GetSprite(itemYeuCau[value]);
            imgIconItem.SetNativeSize();

            Text txtDongXuCo = quan3ConSoc.transform.Find("txtDongXuCo").GetComponent<Text>();

            txtDongXuCo.text = SoitemYeuCau[value].AsString;

            txtInfoItem.text = "";

            nameItemChon = "";
        }
    }
    private void Awake()
    {
        quan3ConSoc = transform.GetChild(0);
        hopqua = GameObject.FindGameObjectWithTag("hopqua");
        txtInfoItem = quan3ConSoc.transform.Find("txtInfoItem").GetComponent<Text>();
    }
    private void Start()
    {
        
        CrGame.ins.StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.5f);
             int random = Random.Range(0, Quan3ConSocChat.Length);
                if (Quan3ConSocChat[random] != "")
                {
                    txt.transform.parent.gameObject.SetActive(true);
                    txt.transform.parent.transform.LeanScale(new Vector3(0.7f, 0.7f, 1), 0.35f);

                    txt.text = Quan3ConSocChat[random];
                }

                CrGame.ins.StartCoroutine(delaychat());
        }
    }
    public static void OpenMenuQuan3ConSoc(byte tab = 0)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = MenuRaKhoi.nameEvent;
        datasend["method"] = "OpenMenu3ConSoc";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
        if (json["status"].AsString == "0")
        {
              Transform quan3ConSoc = AllMenu.ins.GetCreateMenu("Quan3ConSoc",CrGame.ins.trencung.gameObject,true,1).transform.GetChild(0);
       // quan3ConSoc.transform.parent.transform.SetParent(CrGame.ins.trencung);
      //  quan3ConSoc.transform.parent.transform.SetSiblingIndex(CrGame.ins.menulogin.transform.GetSiblingIndex() - 1);
          Quan3ConSoc menuquan3ConSoc = AllMenu.ins.menu["Quan3ConSoc"].GetComponent<Quan3ConSoc>();
                 menuquan3ConSoc.itemYeuCau = json["ItemYeuCau"];
                 menuquan3ConSoc.SoitemYeuCau = json["soItemYeuCau"];
          menuquan3ConSoc.OpenTab = tab;
          
              
      

           for(int a = 0; a < json["allItemQuan3ConSoc"].Count;a++)
           {
                      Transform content = menuquan3ConSoc.allListItem.transform.GetChild(a).transform.GetChild(0).transform.GetChild(0);

                 GameObject objItem = content.transform.GetChild(0).gameObject;
               for (int i = 0; i < json["allItemQuan3ConSoc"][a].Count; i++)
           {
            GameObject ins = Instantiate(objItem, Vector3.zero, Quaternion.identity);
            ins.transform.SetParent(content, false);
            Image imgitem = ins.transform.GetChild(0).GetComponent<Image>();
            Image imgitemyeucau = ins.transform.GetChild(2).GetComponent<Image>();
            imgitemyeucau.sprite = GetSprite(menuquan3ConSoc.itemYeuCau[a].AsString);
            if (json["allItemQuan3ConSoc"][a][i]["loaiitem"].AsString == "item")
            {
                imgitem.sprite = Inventory.LoadSprite(json["allItemQuan3ConSoc"][a][i]["nameitem"].AsString);
            }
            else if (json["allItemQuan3ConSoc"][a][i]["loaiitem"].AsString == "itemrong")
            {
                imgitem.sprite = Inventory.LoadSpriteRong(json["allItemQuan3ConSoc"][a][i]["nameitem"].AsString + "2");
                imgitem.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            else if (json["allItemQuan3ConSoc"][a][i]["loaiitem"].AsString == "itemevent")
            {
                imgitem.sprite = Resources.Load<Sprite>("GameData/" + MenuRaKhoi.nameEvent + "/" + json["allItemQuan3ConSoc"][a][i]["nameitem"].AsString);
            }
            imgitem.name = json["allItemQuan3ConSoc"][a][i]["nameitem"].AsString;
            imgitem.SetNativeSize();
            ins.transform.GetChild(3).GetComponent<Text>().text = json["allItemQuan3ConSoc"][a][i]["giaxuco"].AsString;
            ins.SetActive(true);
           
            GamIns.ResizeItem(imgitem,155);
        }
           }
       
        }
        else CrGame.ins.OnThongBaoNhanh(json["message"].Value);


        }
    }

    private IEnumerator delaychat()
    {
        yield return new WaitForSeconds(Random.Range(5,7));
         txt.transform.parent.gameObject.SetActive(false);
        txt.transform.parent.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(3, 5));
        int random = Random.Range(0, Quan3ConSocChat.Length);
        if(Quan3ConSocChat[random] != "")
        {
            txt.transform.parent.gameObject.SetActive(true);
            txt.transform.parent.transform.LeanScale(new Vector3(0.7f, 0.7f, 1), 0.35f);

            txt.text = Quan3ConSocChat[random];
        }
    
        CrGame.ins.StartCoroutine(delaychat());
    }
     public GameObject khung;
    private string nameItemChon;
    public void ChonItem()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        JSONClass datasend = new JSONClass();
        datasend["class"] = MenuRaKhoi.nameEvent;
        datasend["method"] = "XemInfoItem";
        datasend["data"]["nameitem"] = btnchon.name;
        datasend["data"]["tabchon"] = tabhientai.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                nameItemChon = btnchon.name;
                khung.SetActive(true);
                khung.transform.SetParent(btnchon.transform.parent);
                khung.transform.position = btnchon.transform.position;
               // Text txtInfoItem = quan3ConSoc.transform.Find("txtInfoItem").GetComponent<Text>();
                txtInfoItem.text = json["txt"].AsString;
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }

    }
    public void MuaItem()
    {
        if (nameItemChon == "") return;

        byte tabchon = tabhientai;
        JSONClass datasend = new JSONClass();
        datasend["class"] = MenuRaKhoi.nameEvent;
        datasend["method"] = "XemXacNhan";
        datasend["data"]["nameitem"] = nameItemChon;
        datasend["data"]["tabchon"] = tabchon.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                EventManager.OpenThongBaoChon(json["txt"].AsString,Mua);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
        void Mua()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = MenuRaKhoi.nameEvent;;
            datasend["method"] = "MuaItem";
            datasend["data"]["nameitem"] = nameItemChon;
            datasend["data"]["tabchon"] = tabchon.ToString();
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    //khung.SetActive(false);

                    Text txtInfoItem = quan3ConSoc.transform.Find("txtInfoItem").GetComponent<Text>();
                    txtInfoItem.text = json["txt"].AsString;

                    Text txtDongXuCo = quan3ConSoc.transform.Find("txtDongXuCo").GetComponent<Text>();

                    txtDongXuCo.text = json["soitem"].AsString;

                    Transform content = allListItem.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);

                    for (int i = 0; i < content.transform.childCount; i++)
                    {
                        Transform child = content.transform.GetChild(i).transform.GetChild(0);
                        if (child.name == nameItemChon)
                        {
                            GameObject qua = Instantiate(child.gameObject,transform.position,Quaternion.identity);
                            qua.transform.SetParent(CrGame.ins.trencung,false);
                            qua.transform.position = child.transform.position;
                            QuaBay quabay = qua.AddComponent<QuaBay>();
                            quabay.vitribay = hopqua;
                            break;
                        }
                    }

                    SoitemYeuCau[tabchon] = json["soitem"].AsString;
                    //if(nameItemChon == "TrungRongRua")
                    //{
                    //    btnAptrung.SetActive(true);
                    //    Animator anim = transform.GetChild(0).transform.Find("LongApRongRua").GetComponent<Animator>();
                    //    anim.Play("CoTrung_chua_ap");
                    //}
                }
                else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

    }
    public void OpenTabb()
    {
        OpenTab = (byte)UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
    }
     public static Sprite GetSprite(string name)
    {

        Sprite sprite = Resources.Load<Sprite>("GameData/" + MenuRaKhoi.nameEvent + "/" + name);

        // Nếu sprite không tồn tại, trả về sprite mặc định
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>("GameData/Sprite/Default");
        }

        return sprite;
    }
    public void ExitMenu()
    {
        AllMenu.ins.DestroyMenu("Quan3ConSoc");
    }
    
}
