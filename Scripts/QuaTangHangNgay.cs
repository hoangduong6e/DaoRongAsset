using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuaTangHangNgay : MonoBehaviour
{
    public GameObject MenuQuaHangNgay;NetworkManager net;
    public Text txtSoQua;
    public GameObject btnNhanQua;public GameObject quachon;
    public byte soqua;public bool load = false;CrGame crgame;
    // Start is called before the first frame update
    void Start()
    {
        net = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NetworkManager>();
        crgame = net.GetComponent<CrGame>();
    }

    public void MoQua(GameObject btn)
    {
        if(soqua > 0 && load == false)
        {
            load = true;
            quachon = btn.gameObject;
            net.socket.Emit("nhanquatanghangngay");
        }
    }
    public void NhanXong()
    {
        net.Nhiemvu.gameObject.SetActive(true);
        AllMenu.ins.DestroyMenu("menuQuaHangNgay");
    }
}
