using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTrungRong : MonoBehaviour
{
    // Start is called before the first frame update
    VongQuayRong VongquayRong;public string nametrung;
    // Update is called once per frame
    public void XemTrungRong()
    {
        if (Friend.ins.QuaNha == false)
        {
           
            AllMenu.ins.OpenMenu("MenuQuayRong");
            VongquayRong = AllMenu.ins.menu["MenuQuayRong"].GetComponent<VongQuayRong>();
            VongquayRong.XemTrungRong(nametrung);
        }
        else
        {

            GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua", null, false);
            Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            imgItemTang.sprite = GetComponent<Image>().sprite;
            Friend.ins.MaxSoluong = int.Parse(transform.GetChild(0).GetComponent<Text>().text);
            Friend.ins.XemTangQua("item*" + nametrung);
        }
    }
}
