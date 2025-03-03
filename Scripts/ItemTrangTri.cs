using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;

public class ItemTrangTri : MonoBehaviour
{
    bool drag = false; Vector2 mousePosition;GameObject parnett; 
    Inventory inventory;Text txtsoluong; CrGame crgame; NetworkManager net; Friend friend;
    public string NameItemTrangTri;
    // Start is called before the first frame update
    void Awake()
    {
        friend = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Friend>();
        inventory = friend.GetComponent<Inventory>();
        crgame = friend.GetComponent<CrGame>();
        net = friend.GetComponent<NetworkManager>();     
        txtsoluong = gameObject.transform.GetChild(0).GetComponent<Text>();
    }
    private void OnEnable()
    {
        parnett = transform.parent.gameObject;
        if (friend.QuaNha)
        {
            //GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua");

            GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua", null, false);
            Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            imgItemTang.sprite = GetComponent<Image>().sprite;
            friend.MaxSoluong = int.Parse(transform.GetChild(0).GetComponent<Text>().text);
            friend.XemTangQua("item*" + NameItemTrangTri);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            mousePosition.x -= 1.5f;
            transform.Translate(mousePosition);
        }
    }
    public void Drag(bool d)
    {
        if(friend.QuaNha==false)
        {
            drag = d;
            if (drag)
            {
                transform.SetParent(GameObject.Find("Canvasmenu").transform);
                inventory.menuTuiDo.transform.GetChild(1).gameObject.SetActive(false);
                inventory.HuyThaoTac.SetActive(true);
                txtsoluong.enabled = false;
            }
            if (drag == false)
            {
                if (Mathf.Abs(transform.position.x - inventory.HuyThaoTac.transform.position.x) <= 0.5f &&
                       Mathf.Abs(transform.position.y - inventory.HuyThaoTac.transform.position.y) <= 0.5f)
                {
                    inventory.menuTuiDo.transform.GetChild(1).gameObject.SetActive(true);
                    inventory.HuyThaoTac.SetActive(false);
                    txtsoluong.enabled = true;
                    transform.position = parnett.transform.position;
                    transform.SetParent(parnett.transform);
                }
                else
                {
                    if (int.Parse(txtsoluong.text) <= 0)
                    {
                        inventory.menuTuiDo.transform.GetChild(1).gameObject.SetActive(true);
                        inventory.HuyThaoTac.SetActive(false);
                        txtsoluong.enabled = true;
                        transform.position = parnett.transform.position;
                        transform.SetParent(parnett.transform);
                        inventory.AddItem(NameItemTrangTri, int.Parse(txtsoluong.text));
                        return;
                    } 
                        
                        string post = NameItemTrangTri + "+" + transform.position.x + "+" + transform.position.y;
                    net.socket.Emit("DropTrangTri", JSONObject.CreateStringObject(post));
                    // txtsoluong.text = (int.Parse(txtsoluong.text) - 1) + "";
                    txtsoluong.enabled = true;
                    transform.position = parnett.transform.position;
                    transform.SetParent(parnett.transform);
                    inventory.menuTuiDo.transform.GetChild(1).gameObject.SetActive(true);
                    inventory.HuyThaoTac.SetActive(false);
                    if (int.Parse(txtsoluong.text) == 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
