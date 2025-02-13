
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class NapinApp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject RestoreButton;NetworkManager net;
    inappload inapp;
    void Awake()
    {
        inapp = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<inappload>();
        UpdateUI();
    }
    void UpdateUI()
    {
        GameObject Onap = transform.GetChild(0).gameObject;
        for (int i = 0; i < Onap.transform.childCount; i++)
        {
            Onap.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = inapp.sotien[i];
            Onap.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = inapp.sokimcuong[i];
        }
    }
    public void MuaKimCuong(string id)
    {
        inapp.MuaKimCuong(id);
       // crgame.OnThongBaoNhanh("Đang khởi tạo giao dịch...",3);
       // m_StoreController.InitiatePurchase(id);
    }
    public void ExitMenu()
    {
        AllMenu.ins.CloseMenu("MenuNapinapp");
    }    
}