using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string NameItem;Friend friend;
    private void Start()
    {
        friend = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Friend>();
    }
    bool xem = false;
    public void xemItem()
    {
        if (xem == false) xem = true;// hien thong tin
        else xem = false;
        if(friend.QuaNha == false)
        {

        }
        else
        {

            GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua", null, false);
            Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            imgItemTang.sprite = GetComponent<Image>().sprite;
            friend.MaxSoluong = int.Parse(transform.GetChild(0).GetComponent<Text>().text);
            friend.XemTangQua("item*"+NameItem);
        }
    }
}
