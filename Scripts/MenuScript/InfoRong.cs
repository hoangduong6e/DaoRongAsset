using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoRong : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panelinfo;
    public Text txtHiem, txtNameRong,txtDaTruongThanh;
    public Image imgRong;
    public GameObject Sao;
    public void CloseMenu()
    {
        //gameObject.SetActive(false);
        //for (int i = 0; i < panelinfo.transform.childCount; i++)
        //{
        //    panelinfo.transform.GetChild(i).gameObject.SetActive(false);
        //}
        //for (byte i = 0; i < Sao.transform.childCount; i++)
        //{
        //    if (Sao.transform.GetChild(i).gameObject.activeSelf == false) break;
        //    Sao.transform.GetChild(i).gameObject.SetActive(false);
        //}
        AllMenu.ins.DestroyMenu("MenuInfoRong");
    }
    public void LoadSao(byte sosao)
    {
        for (byte i = 0; i < sosao; i++)
        {
            Sao.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
