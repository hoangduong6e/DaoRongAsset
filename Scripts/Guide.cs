using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SocketIO;

public class Guide : MonoBehaviour
{
    public Animator animMuiTen;
    public Text txtHuongDan;
    public GameObject AllbtnChon;
    public void CloseMenu()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        animMuiTen.Play("1");
        StartCoroutine(ShowText("Click để nhận món quà làm quen của Đảo Rồng nha"));
    }
    public void Nhanqua()
    {
        txtHuongDan.transform.parent.gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        animMuiTen.Play("2");
    }
    public void CloseMenu2()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        // transform.GetChild(3).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(0).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(0).transform.position = CrGame.ins.giaodien.transform.Find("ImageTuiDo").transform.GetChild(0).transform.position;
        AllbtnChon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(motuido);
        animMuiTen.Play("3");
    }
    public void motuido()
    {
        AllbtnChon.transform.GetChild(0).gameObject.SetActive(false);
        AllbtnChon.transform.GetChild(0).GetComponent<Button>().onClick.RemoveListener(motuido);
        Inventory.ins.menuTuiDo.SetActive(true);
        AllbtnChon.transform.GetChild(1).gameObject.SetActive(true);
        animMuiTen.Play("4");
    }
    public void MoTuiRong()
    {
        AllbtnChon.transform.GetChild(1).gameObject.SetActive(false);
        Inventory.ins.Sangtui("tuirong");

        animMuiTen.Play("5");
        StartCoroutine(ShowText("Hãy kéo chú những rồng này ra đảo"));

        StartCoroutine(test());
        IEnumerator test()
        {
            yield return new WaitForSeconds(2f);
            GameObject rongchon = Inventory.ins.TuiRong.transform.GetChild(0).transform.GetChild(0).gameObject;
            rongchon.isStatic = false;
            rongchon.transform.SetParent(transform);
            EventTrigger trigger = rongchon.transform.GetChild(0).GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { ThaRong1((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
    }
    public void ThaRong1(PointerEventData dSata)
    {
        Inventory.ins.HuyThaoTac.SetActive(false);
        if (Inventory.ins.TuiRong.transform.GetChild(1).transform.childCount > 0)
        {
            txtHuongDan.transform.parent.gameObject.SetActive(false);
            animMuiTen.Play("6");
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(2f);
                GameObject rongchon = Inventory.ins.TuiRong.transform.GetChild(1).transform.GetChild(0).gameObject;
                rongchon.transform.SetParent(transform);
                // rongchon.transform.position = Inventory.ins.TuiRong.transform.GetChild(1).transform.position;
                EventTrigger trigger = rongchon.transform.GetChild(0).GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Drag;
                entry.callback.AddListener((data) => { ThaRong2((PointerEventData)data); });
                trigger.triggers.Add(entry);
            }
        }
    }
    public void ThaRong2(PointerEventData data)
    {
        Inventory.ins.HuyThaoTac.SetActive(false);
        if (Inventory.ins.TuiRong.transform.GetChild(2).transform.childCount > 0)
        {
            txtHuongDan.transform.parent.gameObject.SetActive(false);
            animMuiTen.Play("7");
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(2f);
                GameObject rongchon = Inventory.ins.TuiRong.transform.GetChild(2).transform.GetChild(0).gameObject;
                rongchon.transform.SetParent(transform);
                //rongchon.transform.position = Inventory.ins.TuiRong.transform.GetChild(2).transform.position;
                EventTrigger trigger = rongchon.transform.GetChild(0).GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Drag;
                entry.callback.AddListener((data) => { ThaXongRong((PointerEventData)data); });
                trigger.triggers.Add(entry);
            }

        }
    }
    public void ThaXongRong(PointerEventData data)
    {
        Inventory.ins.HuyThaoTac.SetActive(false);
        animMuiTen.Play("8");
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DongTuiDo);
    }
    public void DongTuiDo()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(false);
        Inventory.ins.menuTuiDo.SetActive(false);
        AllbtnChon.transform.GetChild(2).GetComponent<Button>().onClick.RemoveListener(DongTuiDo);
        animMuiTen.Play("9");
    }
    public void CloseGuideThucAn()
    {
        transform.GetChild(3).gameObject.SetActive(false);
        animMuiTen.Play("10");
        GameObject thuyen = CrGame.ins.FindObject(CrGame.ins.AllDao.transform.GetChild(0).gameObject, "ThuyenThucAn");
        thuyen.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OpenMenuThucAn);
        thuyen.transform.SetParent(transform);
        thuyen.transform.SetSiblingIndex(0);
    }
    public void OpenMenuThucAn()
    {
        //CrGame.ins.FindObject(CrGame.ins.AllDao.transform.GetChild(0).gameObject, "menuThucAn").SetActive(true);
        GameObject thuyen = CrGame.ins.FindObject(gameObject, "ThuyenThucAn");
        thuyen.transform.GetChild(0).GetComponent<Button>().onClick.RemoveListener(OpenMenuThucAn);
        thuyen.transform.SetParent(CrGame.ins.AllDao.transform.GetChild(0).transform);
        AllbtnChon.transform.GetChild(3).gameObject.SetActive(true);
        animMuiTen.Play("11");
        //   CrGame.ins.tuithucAn.transform.GetChild(0);
    }
    public void UseThucAn()
    {
        ThucAn thucan = CrGame.ins.tuithucAn.transform.GetChild(0).transform.GetChild(0).GetComponent<ThucAn>();
        thucan.DauV.SetActive(true);
        AllbtnChon.transform.GetChild(3).gameObject.SetActive(false);
        AllbtnChon.transform.GetChild(4).gameObject.SetActive(true);
        animMuiTen.Play("12");
        // thucan.Use();
    }
    public void CloseThucAn()
    {
        ThucAn thucan = CrGame.ins.tuithucAn.transform.GetChild(0).transform.GetChild(0).GetComponent<ThucAn>();
        thucan.Use();
        AllbtnChon.transform.GetChild(4).gameObject.SetActive(false);
        CrGame.ins.FindObject(AllMenu.ins.gameObject, "menuThucAn").SetActive(false);
        animMuiTen.transform.GetChild(0).GetComponent<Image>().enabled = false;//gameObject.SetActive(false);
        StartCoroutine(ShowText("Đợi rồng ăn lên cấp"));
        StartCoroutine(delaylencap());
        IEnumerator delaylencap()
        {
            yield return new WaitForSeconds(5);
            animMuiTen.transform.GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(4).gameObject.SetActive(true);
            animMuiTen.Play("13");
        }
    }
    public void CloseHd3()
    {
        transform.GetChild(4).gameObject.SetActive(false);
        animMuiTen.Play("14");
        GameObject btnqua = CrGame.ins.FindObject(CrGame.ins.giaodien, "btnQuaOnline");
        btnqua.transform.SetParent(transform);
        btnqua.transform.SetSiblingIndex(0);
        // CrGame.ins.FindObject(gameObject, "btnQuaOnline").GetComponent<Button>().onClick.AddListener(OpenMenuNhanQua);
    }
    public void OpenMenuNhanQua()
    {
        CrGame.ins.FindObject(gameObject, "btnQuaOnline").transform.SetParent(CrGame.ins.giaodien.transform);
        animMuiTen.Play("15");
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.5f);
            AllbtnChon.transform.GetChild(5).gameObject.SetActive(true);
        }
    }
    public void NhanQua()
    {
        HopQua qua = CrGame.ins.FindObject(CrGame.ins.giaodien, "btnQuaOnline").GetComponent<HopQua>();
        qua.hienlientiep = false;
        qua.GetComponent<Button>().onClick.RemoveListener(OpenMenuNhanQua);
        qua.NhanQua();
        AllbtnChon.transform.GetChild(5).gameObject.SetActive(false);
        qua.CloseMenu();
        transform.GetChild(5).gameObject.SetActive(true);
        animMuiTen.Play("16");
    }
    public void CloseHDlai()
    {
        animMuiTen.Play("17");
        transform.GetChild(5).gameObject.SetActive(false);
        GameObject lai = CrGame.ins.FindObject(CrGame.ins.AllDao.transform.GetChild(0).gameObject, "Lai");
        lai.transform.SetParent(transform);
        lai.transform.SetSiblingIndex(0);
    }
    public void OpenMenuLai()
    {
        GameObject lai = CrGame.ins.FindObject(gameObject, "Lai");
        lai.transform.SetParent(CrGame.ins.AllDao.transform.GetChild(0).transform);
        animMuiTen.Play("18");
        LaiRong lairong = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
        GameObject rongchonlai = lairong.MenuSlot.transform.GetChild(0).transform.GetChild(0).gameObject;
        StartCoroutine(ShowText("Click chọn rồng để lai. Bố mẹ càng nhiều sao thì khả năng lai ra con nhiều sao càng lớn"));
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1);
            rongchonlai.transform.SetParent(transform);
            // rongchonlai.transform.SetSiblingIndex(0);
            rongchonlai.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ChonLai1);
        }
        rongchonlai.transform.position = lairong.MenuSlot.transform.GetChild(0).transform.position;
    }
    public void ChonLai1()
    {
        animMuiTen.Play("19");
        StartCoroutine(ShowText("Chúc phúc giúp tăng sao rồng vừa lai"));
        LaiRong lairong = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
        GameObject rongchonlai = lairong.MenuSlot.transform.GetChild(1).transform.GetChild(0).gameObject;
        rongchonlai.transform.SetParent(transform);
        //rongchonlai.transform.SetSiblingIndex(0);
        rongchonlai.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(NutLai);

        rongchonlai.transform.position = lairong.MenuSlot.transform.GetChild(1).transform.position;
    }
    public void NutLai()
    {
        LaiRong lairong = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
        animMuiTen.Play("20");
        AllbtnChon.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(lairong.Lairong);
        AllbtnChon.transform.GetChild(6).gameObject.SetActive(true);
    }
    public void Catrong()
    {
        LaiRong lairong = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
        lairong.CatRong();
        animMuiTen.Play("22");
        AllbtnChon.transform.GetChild(8).gameObject.SetActive(false);
        AllbtnChon.transform.GetChild(9).gameObject.SetActive(true);
        transform.GetChild(transform.childCount - 2).transform.SetParent(lairong.MenuSlot.transform.GetChild(0).transform);
        transform.GetChild(transform.childCount - 1).transform.SetParent(lairong.MenuSlot.transform.GetChild(1).transform);
    }
    public void CloseMenulai()
    {
        AllMenu.ins.menu["MenuLaiRong"].SetActive(false);
        transform.GetChild(6).gameObject.SetActive(true);
        animMuiTen.Play("23");
        AllbtnChon.transform.GetChild(9).gameObject.SetActive(false);
    }
    public void CloseHdCongtrinh()
    {
        transform.GetChild(6).gameObject.SetActive(false);
        GameObject ct = CrGame.ins.FindObject(CrGame.ins.AllDao.transform.GetChild(0).gameObject, "ObjectCongtrinh").transform.GetChild(1).gameObject;
        ct.transform.SetParent(transform);
        ct.transform.SetSiblingIndex(0);
        ct.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OpenCongtrinh);
        animMuiTen.Play("24");
    }
    public void OpenCongtrinh()
    {
        GameObject ct = transform.GetChild(0).gameObject;
        ct.transform.GetChild(0).GetComponent<Button>().onClick.RemoveListener(OpenCongtrinh);
        ct.transform.SetParent(CrGame.ins.FindObject(CrGame.ins.AllDao.transform.GetChild(0).gameObject, "ObjectCongtrinh").transform);
        ct.transform.SetSiblingIndex(1);
        animMuiTen.Play("25");
        MenuCongTrinh menu = AllMenu.ins.menu["menuShopCongTrinh"].GetComponent<MenuCongTrinh>();
        GameObject goccayvang = menu.contentShopct.transform.GetChild(0).transform.GetChild(0).gameObject;
        goccayvang.transform.SetParent(transform);
        goccayvang.transform.SetSiblingIndex(0);
        goccayvang.GetComponent<Button>().onClick.AddListener(OpenNangCapCongtrinh);
    }
    public void OpenNangCapCongtrinh()
    {
        MenuCongTrinh menu = AllMenu.ins.menu["menuShopCongTrinh"].GetComponent<MenuCongTrinh>();
        animMuiTen.Play("26");
        GameObject ct = transform.GetChild(0).gameObject;
        ct.transform.SetParent(menu.contentShopct.transform.GetChild(0).transform);
        ct.transform.SetSiblingIndex(0);
        transform.GetChild(6).gameObject.SetActive(false);
        AllbtnChon.transform.GetChild(10).GetComponent<Button>().onClick.AddListener(() => CrGame.ins.XnMuaCongTrinh("vang"));
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.8f);
            AllbtnChon.transform.GetChild(10).gameObject.SetActive(true);
        }
    }
    public void NangCapCtXong()
    {
        animMuiTen.Play("27");
        AllbtnChon.transform.GetChild(0).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Vaotuidotiep);
        AllbtnChon.transform.GetChild(10).gameObject.SetActive(false);
    }
    public void Vaotuidotiep()
    {
        AllbtnChon.transform.GetChild(0).gameObject.SetActive(false);
        Inventory.ins.menuTuiDo.SetActive(true);
        animMuiTen.Play("5");
        StartCoroutine(ShowText("Nếu trên đảo đã chật thì khi kéo rồng ra sẽ hiện lên bảng đổi rồng. Rồng vừa đổi sẽ mang thời gian tiêu hóa của rồng cất"));
        StartCoroutine(test());
        IEnumerator test()
        {
            yield return new WaitForSeconds(8);
            GameObject rongchon = Inventory.ins.TuiRong.transform.GetChild(0).transform.GetChild(0).gameObject;
            rongchon.isStatic = false;
            rongchon.transform.SetParent(transform);
            EventTrigger trigger = rongchon.transform.GetChild(0).GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => { KeoRongDoi((PointerEventData)data); });
            entry2.callback.AddListener((data) => { ThaRongDoi((PointerEventData)data); });
            trigger.triggers.Add(entry);
            trigger.triggers.Add(entry2);
        }
    }
    public void KeoRongDoi(PointerEventData data)
    {
        animMuiTen.Play("28");
    }
    public void ThaRongDoi(PointerEventData data)
    {
        debug.Log("Tha " + 0 + "-" + transform.GetChild(transform.childCount - 1).name);
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Bạn muốn hoán đổi 2 con rồng này ?";
        tbc.btnChon.onClick.RemoveAllListeners();
        tbc.btnChon.onClick.AddListener(() => Inventory.ins.DoiRong(0 + "-" + transform.GetChild(transform.childCount - 2).name));// vị trí rồng đổi trên đảo  + id item rồng
        tbc.btnChon.transform.SetParent(transform);
        tbc.btnChon.onClick.AddListener(delegate { DoiRongXong();Destroy(tbc.btnChon.gameObject); });
        //Destroy(transform.GetChild(transform.childCount - 2).gameObject);
        GameObject g = transform.GetChild(transform.childCount - 2).gameObject;
        g.transform.GetChild(1).GetComponent<Image>().enabled = false;
       // g.transform.SetParent(Inventory.ins.TuiRong.transform.GetChild(0).transform);
        g.transform.position = Inventory.ins.TuiRong.transform.GetChild(0).transform.position;
        animMuiTen.Play("29");
    }
    public void DoiRongXong()
    {
      // ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
       // tbc.btnChon.onClick.RemoveAllListeners();
     //   tbc.btnChon.transform.SetParent(tbc.transform);
        animMuiTen.Play("30");
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DongTuiDenangDao);
    }
    public void DongTuiDenangDao()
    {
        Inventory.ins.menuTuiDo.SetActive(false);
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(false);
        animMuiTen.Play("31");
        GameObject capdao = CrGame.ins.FindObject(CrGame.ins.AllDao.transform.GetChild(0).gameObject, "btnCapDao");
        capdao.GetComponent<Button>().onClick.AddListener(XemNangCapDao);
        capdao.transform.SetParent(transform);
    }
    public void XemNangCapDao()
    {
        animMuiTen.Play("32");
        GameObject capdao = CrGame.ins.FindObject(gameObject, "btnCapDao");
        capdao.transform.SetParent(CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).transform);
        capdao.GetComponent<Button>().onClick.RemoveListener(XemNangCapDao);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            CrGame.ins.UI.nangcapDao.transform.GetChild(1).transform.SetParent(transform);
        }
    }
    public void NangCapDaoXong()
    {
        CrGame.ins.FindObject(gameObject, "BtnnangcapbangKimCuongDao").transform.SetParent(CrGame.ins.UI.nangcapDao.transform);
        animMuiTen.Play("33");
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(false);
        AllbtnChon.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        AllbtnChon.transform.GetChild(0).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(MoTuiDoTiep);
    }
    public void MoTuiDoTiep()
    {
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(false);
        animMuiTen.Play("5");
        StopAllCoroutines();
        StartCoroutine(ShowText("Được thả thêm rồng sau khi nâng cấp đảo"));
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1);
            GameObject rongchon = Inventory.ins.TuiRong.transform.GetChild(0).transform.GetChild(0).gameObject;
            rongchon.isStatic = false;
            rongchon.transform.SetParent(transform);
            EventTrigger trigger = rongchon.transform.GetChild(0).GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { ThaXong((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
    }
    public void ThaXong(PointerEventData data)
    {
        animMuiTen.Play("34");
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(true);
        AllbtnChon.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        AllbtnChon.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Xong);
    }
    void Xong()
    {
        Inventory.ins.menuTuiDo.SetActive(false);
        AllbtnChon.transform.GetChild(2).gameObject.SetActive(false);
        //GameObject mn = Instantiate(SGResources.Load("GameData/Menu/MenuDatTen") as GameObject, transform.position, Quaternion.identity) as GameObject; ;
        //mn.transform.SetParent(AllMenu.ins.transform, false);
        //AllMenu.ins.menu.Add("MenuDatTen", mn);
        AllMenu.ins.menu["menuQuaHangNgay"].SetActive(true);
        CrGame.ins.GetComponent<ZoomCamera>().enabled = true;
        HopQua qua = CrGame.ins.FindObject(CrGame.ins.giaodien, "btnQuaOnline").GetComponent<HopQua>();
        qua.hienlientiep = true;
        Destroy(gameObject);
    }
    IEnumerator ShowText(string shuongdan)
    {
        txtHuongDan.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i <= shuongdan.Length; i++)
        {
            txtHuongDan.text = shuongdan.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
    void Start()
    {
        NetworkManager.ins.socket.On("Thongbao", qua);
        NetworkManager.ins.socket.On("itemlairong", itemlai);
        NetworkManager.ins.socket.On("laithanhcong",laixong);
        NetworkManager.ins.socket.On("upcongtrinhthanhcong", Nangctthanhcong);
    }
    void qua(SocketIOEvent e)
    {
        debug.Log("[SocketIO] Guide: " + e.name + " " + e.data);
        if (e.data["qualevel"])
        {
            OpenMenuNhanQua();
        }
    }
    void itemlai(SocketIOEvent e)
    {
        if (e.data["itemlai"])
        {
            OpenMenuLai();
        }
    }
    void laixong(SocketIOEvent e)
    {
        if (e.data["additem"])
        {
            animMuiTen.transform.GetChild(0).GetComponent<Image>().enabled = false;
            AllbtnChon.transform.GetChild(6).gameObject.SetActive(false);
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(15);
                {
                    animMuiTen.transform.GetChild(0).GetComponent<Image>().enabled = true;
                    AllbtnChon.transform.GetChild(7).gameObject.SetActive(true);
                    LaiRong lairong = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
                    AllbtnChon.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(lairong.Chucphuc);
                }
            }
        }
        if (e.data["chucphuc"])
        {
            animMuiTen.transform.GetChild(0).GetComponent<Image>().enabled = false;
            AllbtnChon.transform.GetChild(7).gameObject.SetActive(false);
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(7);
                {
                    animMuiTen.transform.GetChild(0).GetComponent<Image>().enabled = true;
                    AllbtnChon.transform.GetChild(8).gameObject.SetActive(true);
                    AllbtnChon.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(Catrong);
                    animMuiTen.Play("21");
                }
            }
        }
        if (e.data["nangcapdaothanhcong"])
        {
            NangCapDaoXong();
        }
    }
    void Nangctthanhcong(SocketIOEvent e)
    {
        if (e.data["congtrinh"])
        {
            NangCapCtXong();
        }
    }
}
