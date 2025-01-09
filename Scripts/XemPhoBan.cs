using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XemPhoBan : MonoBehaviour
{
    private Camera cam;
    Vector3 dragOrigin;
    public SpriteRenderer imgMap;
    public float zoomstep,Maxcamsize,MinCamsize;
    float MapMinX, MapMaxX, mapMiny, MapMaxY;
    public GameObject Hieuungden;public GameObject GdMapVienChinh;InfoLevelPhoBan infolevel;
    // Start is called before the first frame update
    void Awake()
    {
        //transform.position = CrGame.ins.friend.DaoFriend.transform.position;
    }
    private void OnEnable()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        infolevel = AllMenu.ins.GetCreateMenu("GiaoDienMapVienChinh").GetComponent<InfoLevelPhoBan>();

        GdMapVienChinh = infolevel.gameObject;
        Hieuungden = GdMapVienChinh.transform.GetChild(1).gameObject;

        MapMinX = imgMap.transform.position.x - imgMap.bounds.size.x / 2f;
        MapMaxX = imgMap.transform.position.x + imgMap.bounds.size.x / 2f;

        mapMiny = imgMap.transform.position.y - imgMap.bounds.size.y / 2f;
        MapMaxY = imgMap.transform.position.y + imgMap.bounds.size.y / 2f;

        Hieuungden.SetActive(true);
        if (Inventory.ins.ListItemThuong.ContainsKey("itemManhVoThuNguyen")) infolevel.txtsomanhvo.text = Inventory.ins.ListItemThuong["itemManhVoThuNguyen"].transform.GetChild(0).GetComponent<Text>().text;
        else infolevel.txtsomanhvo.text = "0";
        if (Inventory.ins.ListItemThuong.ContainsKey("itemHuyenTinh")) infolevel.txtsohuyentinh.text = Inventory.ins.ListItemThuong["itemHuyenTinh"].transform.GetChild(0).GetComponent<Text>().text;
        else infolevel.txtsohuyentinh.text = "0";
        GdMapVienChinh.SetActive(true);
        cam.GetComponent<ZoomCamera>().enabled = false; Camera.main.orthographicSize = 5;
        AudioManager.SetSoundBg("nhacnen1");
        // NhacPhoBan.Play();
    }
    private void OnDisable()
    {
       Hieuungden.SetActive(false);
       GdMapVienChinh.SetActive(false);
      // NhacPhoBan.Stop();
       cam.GetComponent<ZoomCamera>().enabled = true;
    }
    public void CloseMenu()
    {
        //gameObject.SetActive(false);
        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        Dao.SetActive(true);
        Vector3 vec = Dao.transform.position;
        vec.z = -10;
        cam.transform.position = vec;
        CrGame.ins.giaodien.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        AllMenu.ins.DestroyMenu("menuPhoban");
        AllMenu.ins.DestroyMenu("GiaoDienMapVienChinh");
    }
    public void VaoMap()
    {
        if(int.Parse(CrGame.ins.giaodien.transform.Find("btnQuaOnline").transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text) >= 99)
        {
            CrGame.ins.OnThongBaoNhanh("Hộp quà đã đầy, hãy nhận quà trước!");
            return;
        }    
        infolevel.CloseMenu();
      //  NetworkManager.ins.vienchinh.chedodau = CheDoDau.VienChinh;
        gameObject.SetActive(false);
        //    NetworkManager.ins.vienchinh.enabled = true;
        VienChinh.vienchinh.GetDoiHinh(NetworkManager.ins.vienchinh.nameMapvao, CheDoDau.VienChinh);
        CrGame.ins.DonDepDao();
  

     //   NetworkManager.ins.socket.Emit("DoiHinhDanh", JSONObject.CreateStringObject(NetworkManager.ins.vienchinh.nameMapvao));
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 difrence = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position = Clampcamera(cam.transform.position + difrence);
        }
    }
    public void ZoomIn()
    {
        float newsize = cam.orthographicSize - zoomstep;
        cam.orthographicSize = Mathf.Clamp(newsize,MinCamsize,Maxcamsize);
        cam.transform.position = Clampcamera(cam.transform.position);
    }
    public void ZoomOut()
    {
        float newsize = cam.orthographicSize + zoomstep;
        cam.orthographicSize = Mathf.Clamp(newsize, MinCamsize, Maxcamsize);
        cam.transform.position = Clampcamera(cam.transform.position);
    }
    Vector3 Clampcamera(Vector3 targetPosition)
    {
        float CamHeight = cam.orthographicSize;
        float CamWidth = cam.orthographicSize * cam.aspect;
        float MinX = MapMinX + CamWidth;
        float MaxX = MapMaxX - CamWidth;
        float MinY = mapMiny + CamHeight;
        float MaxY = MapMaxY - CamHeight;
        float NewX = Mathf.Clamp(targetPosition.x,MinX,MaxX);
        float NewY = Mathf.Clamp(targetPosition.y, MinY, MaxY);
        return new Vector3(NewX,NewY,targetPosition.z);
    }
}
