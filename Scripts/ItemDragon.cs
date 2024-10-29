using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragon : MonoBehaviour
{
    public GameObject item;bool drag; Vector2 mousePosition;
    public string nameObjectDragon;
    Transform transformitem;
    public Text txtSao;
    public GameObject imageOdo;
    // Start is called before the first frame update
    private void Start()
    {
        transformitem = gameObject.transform.parent;
    }
    public void OnPointerDown()
    {
        this.enabled = true;
    }
    public void OnPointerUp()
    {
        //StopCoroutine(delay());
       // Friend.ins.crgame.allmenu.CloseMenu("MenuChiSoRong");
        this.enabled = false;
    }
    public void XemCs()
    {
        CrGame.ins.ChiSoRong(gameObject.name);
    }
    private void OnEnable()
    {
        if (Friend.ins.QuaNha)
        {
            GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua",null,false);
            Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            imgItemTang.sprite = transform.GetChild(0).GetComponent<Image>().sprite;
            Friend.ins.MaxSoluong = 1;
            Friend.ins.XemTangQua("rong*" + nameObjectDragon,gameObject.name);
        }
        else NetworkManager.ins.socket.Emit("ChonRongTha");
    }
    void Update()
    {
        if (drag)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - item.transform.position;
            item.transform.Translate(mousePosition);
        }
    }
    public void Drag(bool d)
    {
        if(Friend.ins.QuaNha == false)
        {
            drag = d;
            Inventory.ins.HuyThaoTac.SetActive(true);
            if (drag == false)
            {
                Inventory.ins.HuyThaoTac.SetActive(false);
                if (Mathf.Abs(item.transform.position.x - Inventory.ins.HuyThaoTac.transform.position.x) <= 0.5f &&
                    Mathf.Abs(item.transform.position.y - Inventory.ins.HuyThaoTac.transform.position.y) <= 0.5f)
                {
                    item.transform.position = transformitem.position;
                    item.transform.SetParent(gameObject.transform);
                }
                else
                {
                    GameObject DoiRong = Inventory.ins.menuTuiDo.transform.GetChild(2).gameObject;
                    if (DoiRong.activeSelf == false)
                    {
                        //Inventory.ins.targetTharong = item.transform.position;
                        //InfoItemRong itemJSON = new InfoItemRong(gameObject.name);
                        //string data = JsonUtility.ToJson(itemJSON);
                        DragonIslandManager.DropDragon(gameObject.name, item.transform.position);
                        item.transform.position = transformitem.position;
                        //net.socket.Emit("tharong", new JSONObject(data));
                        item.transform.SetParent(gameObject.transform);

                    }
                    else
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (Mathf.Abs(item.transform.position.x - DoiRong.transform.GetChild(i).transform.position.x) <= 0.5f &&
                                Mathf.Abs(item.transform.position.y - DoiRong.transform.GetChild(i).transform.position.y) <= 0.5f)
                            {
                                ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
                                tbc.txtThongBao.text = "Bạn muốn hoán đổi 2 con rồng này ?";
                             //   tbc.deleteclick();
                                tbc.btnChon.onClick.AddListener(() => Inventory.ins.DoiRong(i + "-" + gameObject.name));// vị trí rồng đổi trên đảo  + id item rồng
                                tbc.gameObject.SetActive(true);
                                break;
                            }
                        }
                        item.transform.position = transformitem.position;
                        item.transform.SetParent(gameObject.transform);
                    }
                }
            }
            else
            {
                item.transform.SetParent(AllMenu.ins.transform);
            }
        }
    }
}
//[SerializeField]
//public class InfoItemRong
//{
//    public string id;
//    public InfoItemRong(string iditem)
//    {
//        id = iditem;
//    }
//}