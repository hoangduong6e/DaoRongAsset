using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThuyenThucAn : MonoBehaviour
{
    //public Text txtNameDoAn,CongKinhNghiem,txtRotVang,txtMoiGiay,
    //    txtTongThuhoach,txtExpChoRong,txtTieuHoaTrong,txtSuDungHieuQuaDenCap,
    //    txtCapNangBangKimCuong,txtCapNangBangVang;// txt mỗi giây sẽ rớt vàng
    public GameObject InfoThucAn;
    GameObject imgThuyen;Animator anim;
    public bool ThaThucAn = true;
    public sbyte SoThucAnDaTha;
    public GameObject thucAnobj;
    public GameObject ThuongChoRongAnPrefab,ThuyenObject;
    public Image ImageThucAn;
    public string NameThucAnChon;public GameObject ThucAnTha,contentTau;
  //  public Text txtThongTin;
    //public Text capdoanhientai,capdoanchuanangcap,capnangbangVang,capNangBangKc,GiaVang,GiaKc;public Button btnMuabangvang, btnMuabangkc;
    // Start is called before the first frame update
    void Start()
    {
        imgThuyen = GameObject.Find("ThuyenThucAn");
       // anim = imgThuyen.GetComponent<Animator>();
        // StartCoroutine(bay());        CrGame.ins = GameObject.Find("Main Camera").GetComponent<CrGame.ins>();
        anim = imgThuyen.GetComponent<Animator>();
        // capdoanhientai.text = "<color=#00ff00ff>+6 kinh nghiệm </color>\nRớt <color=#ffff00ff>0 tiền vàng </color>mỗi <color=#ffff00ff>0 giây</color>\ntổng thu hoạch: <color=#ffff00ff>0 tiền vàng </color>\nkinh nghiệm cho rồng con:<color=#ff00ffff>0</color>\ntiêu hóa trong: <color=#ffff00ff>0 phút 0 giây </color> \nsử dụng hiệu quả đến cấp độ:<color=#ffff00ff>0</color>";
    }
    public void XemThucAn(GameObject gameOb)
    {
        //InfoThucAn.GetComponent<Transform>().transform.position = new Vector3(gameOb.transform.position.x - 3, InfoThucAn.transform.position.y);
        //InfoThucAn.SetActive(true);
    }
    public void NangCapThucAn(string s)
    {
        NetworkManager.ins.socket.Emit("XemNangCapThucAn", JSONObject.CreateStringObject("nangcap-" + NameThucAnChon + "-" + s));
        AllMenu.ins.menu["MenuNangCapItem"].SetActive(false);
    }    
   public IEnumerator bay(Sprite spriteThucAn,int soluong)
    {
        ImageThucAn.sprite = spriteThucAn;
        ImageThucAn.enabled = true;
        ImageThucAn.transform.GetChild(0).gameObject.SetActive(false);
        ThaThucAn = false;
        scale(-1f);
        anim.Play("ThuyenBay");
        float[] timerot = new float[] {2.16f,0.84f,0.47f,1.02f,0.93f,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        Transform vitridao = GameObject.Find("BGDao" + CrGame.ins.DangODao).transform;
        for (int i = 0;i < soluong; i++)
        {
            yield return new WaitForSeconds(timerot[i]);
            GameObject thucan = Instantiate(thucAnobj, new Vector3(vitridao.transform.position.x + Random.Range(-1.6f, 1.3f), vitridao.transform.position.y + Random.Range(-0.5f, 1.5f)), Quaternion.identity) as GameObject;
            thucan.transform.SetParent(DragonIslandManager.DungThucAn.transform);
            //thucan.transform.SetSiblingIndex(2);
            thucan.name = "thucan";
            SoThucAnDaTha += 1;
            scale(1f);
            thucan.SetActive(true);
            AudioManager.PlaySound("thathucan");
        }
        yield return new WaitForSeconds(2f);
        ThaThucAn = true;
        ThuyenObject.transform.SetParent(CrGame.ins.AllDao.transform.GetChild(CrGame.ins.DangODao).transform);
    }
    void craeteHieuUng(float x,float y)
    {
        GameObject hieuung = Instantiate(Inventory.ins.GetObj("HieuUng1"), new Vector3(x, y + 1f), Quaternion.identity) as GameObject;
        Vector3 Scale; Scale = hieuung.transform.localScale;
        Scale.x = 1; Scale.y = 1.1f; hieuung.transform.localScale = Scale;
        hieuung.SetActive(true);
        Destroy(hieuung, 1.5f);
    }
    void scale(float x)
    {
        Vector3 Scale;
        Scale = imgThuyen.transform.localScale;
        Scale.x = x;
        imgThuyen.transform.localScale = Scale;
    }
  
    public void ChonThuyen()
    {
        GameObject chon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        NetworkManager.ins.socket.Emit("ChonThuyenMacDinh", JSONObject.CreateStringObject(chon.transform.parent.name));
        for (int j = 0; j < contentTau.transform.childCount; j++)
        {
            if (contentTau.transform.GetChild(j).transform.GetChild(0).transform.GetChild(1).gameObject.activeSelf)
            {
                contentTau.transform.GetChild(j).transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        Image imgthuyen = imgThuyen.transform.GetChild(0).GetComponent<Image>();
        imgthuyen.sprite = Inventory.LoadSprite(chon.transform.parent.name);
        imgthuyen.SetNativeSize();
        imgThuyen.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(chon.transform.parent.name);
        chon.transform.parent.transform.GetChild(1).gameObject.SetActive(true);
    }
}
