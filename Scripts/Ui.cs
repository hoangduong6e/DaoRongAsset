using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    // chiso congtrinh
    LaiRong lai;ThuyenThucAn thuyen;
    public Image[] SpriteItem;
    public Text[] txtChiSo,txtGia, txtGiaNhaAptrung, txtTileThanhCong,txtlevel,txtGiaNangDao,txtGiaNangThucAn;
    public Button[] btnNangCap;
    public Button[] btnNangCapNhaApTrung;
    public Button[] btnNangCapThucAn;
    public GameObject nangcapDao;
    public GameObject tilethanhcong;

    private void Start()
    {
        thuyen = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThuyenThucAn>();
    }
    public void XnNangCap(string s)
    {
        CrGame.ins.XnMuaCongTrinh(s);
    }
    public void XnNangCapNhaLai(string s)
    {
        lai = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
        lai.NangCap(s);
    }
    public void XnNangCapDao(string s)
    {
        CrGame.ins.NangCapDao(s);
    }
    public void XnNangCapThucAn(string s)
    {
        thuyen.NangCapThucAn(s);
    }
    public void NangCapThanlong()
    {
        CrGame.ins.NangCapThanLong();
    }
    public void DestroyGD()
    {
        AllMenu.ins.DestroyMenu("MenuNangCapItem");
    }
}
