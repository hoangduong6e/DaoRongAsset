using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCongTrinh : MonoBehaviour
{
    Vector2 mousePosition;bool drag;Inventory inventory;Text txtsoluong;
    GameObject parnett;CrGame crgame;NetworkManager net;
    public string nameitem;Friend friend;
    // Start is called before the first frame update
    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>();
        crgame = inventory.GetComponent<CrGame>();
        net = inventory.GetComponent<NetworkManager>();
        friend = inventory.GetComponent<Friend>();
        txtsoluong = gameObject.transform.GetChild(0).GetComponent<Text>();
    }
    private void OnEnable()
    {
        parnett = transform.parent.gameObject;
        if (friend.QuaNha)
        {

            GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua", null, false);
            Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            imgItemTang.sprite = GetComponent<Image>().sprite;
            friend.MaxSoluong = int.Parse(transform.GetChild(0).GetComponent<Text>().text);
            friend.XemTangQua("item*" + nameitem);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(drag)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            mousePosition.x -= 1.5f;
            transform.Translate(mousePosition);
        }
    }
    public void Drag(bool d)
    {
        if(friend.QuaNha == false)
        {
            drag = d;
            if (drag)
            {
                transform.SetParent(GameObject.Find("Canvasmenu").transform);
                inventory.menuTuiDo.transform.GetChild(1).gameObject.SetActive(false);
                inventory.HuyThaoTac.SetActive(true);
                txtsoluong.enabled = false;
            }
            else
            {
                if (Mathf.Abs(transform.position.x - inventory.HuyThaoTac.transform.position.x) <= 0.5f &&
                      Mathf.Abs(transform.position.y - inventory.HuyThaoTac.transform.position.y) <= 0.5f)
                {
                    Huy();
                }
                else
                {
                    //  GameObject congtrinh = GameObject.Find("ObjectCongtrinh" + crgame.DangODao);
                    GameObject objectct = crgame.FindObject(crgame.AllDao.transform.GetChild(crgame.DangODao).gameObject, "ObjectCongtrinh");
                    for (int i = 0; i < 4; i++)
                    {
                        if (Mathf.Abs(transform.position.x - objectct.transform.GetChild(i).transform.position.x) <= 0.5f &&
                        Mathf.Abs(transform.position.y - objectct.transform.GetChild(i).transform.position.y) <= 0.5f)
                        {
                            CongTrinh ct = objectct.transform.GetChild(i).GetComponent<CongTrinh>();
                            if (ct.nameCongtrinh == "DatTrong")
                            {
                                net.socket.Emit("TrongCongTrinh", JSONObject.CreateStringObject(nameitem + "+" + i));
                                debug.Log("trong cong trinh thanh cong " + objectct.transform.GetChild(i).name);
                                Huy();
                                break;
                            }
                            else Huy();
                        }
                        else
                        {
                            if (i == 3) Huy();
                        }
                    }
                }
            }
        }
    }
    public void Huy()
    {
        inventory.menuTuiDo.transform.GetChild(1).gameObject.SetActive(true);
        inventory.HuyThaoTac.SetActive(false);
        txtsoluong.enabled = true;
        transform.position = parnett.transform.position;
        transform.SetParent(parnett.transform);
    }
}
