using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GiaoDienPVP : MonoBehaviour
{
    public GameObject ItemRongDanh,
        ContentItemRong,
        btnSetting,
        menuSetting,
        OSkill,
        panelBatdau, 
        menuWin;
    public static GiaoDienPVP ins;
    private bool thahetrong = false;
    private byte trang = 1, maxtrang;
    public byte SoDoihinh;
    public Button btnTrieuHoiSuPhu;
    public float time, maxtime;
    public Text txtBatDau;
    public Sprite thua, thang;
    public Image spriteWin;
    public Transform HienRongObj;
    public Text TxtTime, txtHuyenTinh, thongtin;
    public GameObject btnTangToc,hienPlayer;
    private bool thanhanh = false;
    public MinimapController minimap;
    [SerializeField]
    GameObject paneltoi;

    public Transform[] objtatkhitoi; // Mảng chứa các đối tượng
    private bool[] initialStates;    // Mảng lưu trạng thái ban đầu

    public bool SetPanelToi { 
        set 
        { 
            if(value)
            {
                if(!paneltoi.activeSelf)
                {
                     for (int i = 0; i < objtatkhitoi.Length; i++)
                {
                    if (objtatkhitoi[i] != null)
                    {
                        initialStates[i] = objtatkhitoi[i].gameObject.activeSelf;
                    }
                }
                foreach (Transform obj in objtatkhitoi)
                {
                    if (obj != null)
                    {
                        obj.gameObject.SetActive(false);
                    }
                }
                }
               
            }
            else
            {
                for (int i = 0; i < objtatkhitoi.Length; i++)
                {
                    if (objtatkhitoi[i] != null)
                    {
                        objtatkhitoi[i].gameObject.SetActive(initialStates[i]);
                    }
                }
            }
            paneltoi.SetActive(value);
        }
        get { return paneltoi.activeSelf; }
    }


    void Awake()
    {
        paneltoi.transform.SetParent(GameObject.FindGameObjectWithTag("canvasGame").transform);

        ins = this;
        initialStates = new bool[objtatkhitoi.Length];
        paneltoi.GetComponent<RectTransform>().anchoredPosition = new Vector2(5828, 270);
    }
    private void OnEnable()
    {
        SoDoihinh = 0;
        thahetrong = false;
        ResetAllHienRONG();
    }
    //private void Update()
    //{
    //    debug.Log("x HienRongObj " + HienRongObj.transform.position.x);
    //}
    public void AddItemRongDanh(string rong, string id, int sao, int tienhoa, int i)
    {
        GameObject Item = Instantiate(ItemRongDanh, transform.position, Quaternion.identity) as GameObject;
        Item.transform.SetParent(ContentItemRong.transform, false);
        Image imgrong = Item.transform.GetChild(1).GetComponent<Image>();
        Text txtSao = Item.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        txtSao.text = sao + "";
        ItemRongVienChinh itemvienchinh = Item.GetComponent<ItemRongVienChinh>();
        itemvienchinh.idRong = id;
        imgrong.sprite = Inventory.LoadSpriteRong(rong + tienhoa,(short)sao);
        // imgrong.sprite = SGResources.Load<Sprite>("Sprite/Rong/" + rong + tienhoa); 
        imgrong.SetNativeSize();
        Item.name = "rongdanh" + id;
        if (i < 9)
        {
            Item.SetActive(true);
        }
    }
    public void RongChet(string idrong)
    {
        StartCoroutine(delayy());
        // string chedodau = VienChinh.vienchinh.chedodau;
        CheDoDau chedodau = VienChinh.vienchinh.chedodau;
        for (int i = 0; i < ContentItemRong.transform.childCount; i++)
        {
            if (ContentItemRong.transform.GetChild(i).name == "rongdanh" + idrong)
            {
                NetworkManager.ins.socket.Emit("rongdie", JSONObject.CreateStringObject(idrong));
                ItemRongVienChinh itemvc = ContentItemRong.transform.GetChild(i).GetComponent<ItemRongVienChinh>();
                //  itemvc.hoisinh = true;
                if (ContentItemRong.transform.GetChild(i).gameObject.activeSelf == false)
                {
                    itemvc.DaulauChet.transform.GetChild(0).gameObject.SetActive(false);
                    if (chedodau == CheDoDau.VienChinh || chedodau == CheDoDau.Halloween || chedodau == CheDoDau.BossTG || chedodau == CheDoDau.SoloKhongTuoc ||chedodau == CheDoDau.LanSu || chedodau == CheDoDau.XucTu)
                    {
                        Text txthuyentinhhoisinh = itemvc.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                        itemvc.transform.GetChild(0).gameObject.SetActive(true);
                        itemvc.GetComponent<Button>().interactable = true;
                        float sao = float.Parse(itemvc.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text) * 2.5f;
                        txthuyentinhhoisinh.text = Math.Round(sao) + "";
                        // itemvc.hoisinh = false;
                    }
                    else itemvc.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(delaytat(itemvc.DaulauChet.transform.GetChild(0).gameObject));
                    if (chedodau == CheDoDau.VienChinh || chedodau == CheDoDau.Halloween || chedodau == CheDoDau.BossTG || chedodau == CheDoDau.SoloKhongTuoc || chedodau == CheDoDau.LanSu || chedodau == CheDoDau.XucTu)
                    {
                        Text txthuyentinhhoisinh = itemvc.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                        itemvc.transform.GetChild(0).gameObject.SetActive(true);
                        itemvc.GetComponent<Button>().interactable = true;
                        float sao = float.Parse(itemvc.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text) * 2.5f;
                        txthuyentinhhoisinh.text = Math.Round(sao) + "";
                        //   itemvc.hoisinh = false;
                    }
                    else itemvc.transform.GetChild(0).gameObject.SetActive(false);
                }
                itemvc.DaulauChet.SetActive(true);
                break;
            }
        }
        IEnumerator delaytat(GameObject g)
        {
            yield return new WaitForSeconds(1.5f);
            g.SetActive(false);

        }
        IEnumerator delayy()
        {
            yield return new WaitForSeconds(5f);
            if (VienChinh.vienchinh.dangdau)
            {
                if (VienChinh.vienchinh.TeamXanh.transform.childCount == 1 && VienChinh.vienchinh.TeamDo.transform.childCount == 1) VienChinh.vienchinh.Thang();
            }
        }
    }
    public void DestroyAllItemRong()
    {
        for (int i = 0; i < ContentItemRong.transform.childCount; i++)
        {
            Destroy(ContentItemRong.transform.GetChild(i).gameObject);
        }
    }
    public void OpenSetting(bool b)
    {
        menuSetting.SetActive(b);
        if (b)
        {
            StartCoroutine(delay());

        }
        else
        {
            Time.timeScale = 1;
        }
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            if (menuSetting.activeSelf) Time.timeScale = 0;
        }
    }
    public void LoadSkill(JSONObject json)
    {
       // debug.Log("jsonnnn :" + json);
        if (json.Count > 0)
        {
            OSkill.SetActive(true);
            for (int i = 0; i < json.Count; i++)
            {
                OSkill.transform.GetChild(1).transform.GetChild(i).gameObject.SetActive(true);
                OSkill.transform.GetChild(1).transform.GetChild(i).gameObject.name = GamIns.CatDauNgoacKep(json[i]["nameskill"].ToString());
                Image imgskill = OSkill.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>();
                imgskill.sprite = Inventory.LoadSprite(GamIns.CatDauNgoacKep(json[i]["nameskill"].ToString())); imgskill.SetNativeSize();
                OSkill.transform.GetChild(1).GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = imgskill.sprite;
               OSkill.transform.GetChild(1).transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = GamIns.CatDauNgoacKep(json[i]["level"].ToString());
            }
        }
        else
        {
            OSkill.SetActive(false);
        }
    }
    public void TangTocReplay()
    {
        ReplayData.speedReplay += 1;
        if (ReplayData.speedReplay > 3) ReplayData.speedReplay = 1;
        SettxtSpeedReplay();
        StopAllCoroutines();
    }
    public void SettxtSpeedReplay()
    {
        btnTangToc.transform.GetChild(0).GetComponent<Text>().text = "X" + ReplayData.speedReplay;
    }
    public void ResetAllHienRONG()
    {
        for (int i = 0; i < HienRongObj.transform.childCount; i++)
        {
            GameObject OBJi = HienRongObj.transform.GetChild(i).gameObject;
            Text txtsoluong = OBJi.transform.GetChild(2).GetComponent<Text>();
            OBJi.name = "obj";
            txtsoluong.text = "X0";
            OBJi.SetActive(false);
        }
    }
    public void AddHienRong(string namerong,int cong = 1)
    {
        if (cong == 1 && HienRongObj.transform.position.x >= 38)
        {
            ResetAllHienRONG();
            return;
        }
        for (int i = 0; i < HienRongObj.transform.childCount; i++)
        {
            string nameobj = HienRongObj.transform.GetChild(i).gameObject.name;
            if (nameobj == namerong)
            {
                add(i);
                break;
            }
            else if (nameobj == "obj" && cong == 1)
            {
                ChangeSprite(i);
                add(i);
                break;
            }
            else if(i == HienRongObj.transform.childCount - 1 && cong == 1)
            {
                HienRongObj.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "X0";
                ChangeSprite(i);
                add(i);
            }
        }
        void ChangeSprite(int i)
        {
            HienRongObj.transform.GetChild(i).gameObject.name = namerong;
            Image imgrong = HienRongObj.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
            imgrong.sprite = Inventory.LoadSpriteRong(namerong + "2");
            
        }
     
        void add(int i)
        {
            GameObject OBJi = HienRongObj.transform.GetChild(i).gameObject;
            Text txtsoluong = OBJi.transform.GetChild(2).GetComponent<Text>();
            int congxong = int.Parse(txtsoluong.text.Substring(1)) + cong;
            if (congxong > 0)
            {
                txtsoluong.text = "X" + congxong.ToString();
                OBJi.gameObject.SetActive(true);
            }
            else
            {
                txtsoluong.text = "X0";
                OBJi.name = "obj";
                OBJi.SetActive(false);
            }
        }
    }
    public void TrieuHoiSuPhu()
    {
        NetworkManager.ins.socket.Emit("trieuhoirong", JSONObject.CreateStringObject("suphu"));
        btnTrieuHoiSuPhu.interactable = false;
    }
    public void UseSkill()
    {
        GameObject objskill = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        NetworkManager.ins.socket.Emit("useskill", JSONObject.CreateStringObject(objskill.name + "+" + objskill.transform.GetSiblingIndex()));
        //timeskill[objskill.transform.GetSiblingIndex()] = 15;
        //HieuUngSkill("Skill" + objskill.name);
    }

    public void XemNoKhi()
    {
        GameObject trencung = GameObject.FindGameObjectWithTag("trencung");
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", trencung, true, trencung.transform.childCount - 1).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Mua thêm nộ khí với 2 kim cương?";
        tbc.btnChon.onClick.AddListener(MuaNoKhi);
    }
    void MuaNoKhi()
    {
        NetworkManager.ins.socket.Emit("MuaNoKhi");
    }
    public void Sang(int a)
    {
        if (a > 0)
        {
            maxtrang = (byte)(ContentItemRong.transform.childCount / 9 + 1);
            if (trang < maxtrang)
            {
                for (int i = trang * 9 - 9; i < trang * 9; i++)
                {
                    ContentItemRong.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = trang * 9; i < trang * 9 + 9; i++)
                {
                    if (i < ContentItemRong.transform.childCount)
                    {
                        ContentItemRong.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                trang++;
            }
        }
        else
        {
            if (trang > 1)
            {
                for (int i = trang * 9 + 9 - 1; i < trang * 9; i++)
                {
                    ContentItemRong.transform.GetChild(i).gameObject.SetActive(false);
                }
                trang--;
                for (int i = trang * 9 - 1; i >= trang * 9 - 9; i--)
                {
                    if (i < ContentItemRong.transform.childCount)
                    {
                        ContentItemRong.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    // Phương thức này được gọi liên tục để thực hiện các hành động trong quá trình chuyển đổi nhanh giữa các mục tiêu rồng.
    private void ThaNhanh()
    {
        StartCoroutine(DelaySettingButton()); // Thực hiện chức năng chậm trễ cho việc hiển thị nút cài đặt.
        CheckAndActivateRongItems(); // Kiểm tra và kích hoạt các phần tử rồng phù hợp.
        UpdateCurrentPage(); // Cập nhật trang hiện tại của danh sách phần tử rồng.

        if (thanhanh) // Nếu vẫn trong trạng thái thần hành, tiếp tục gọi phương thức này sau 0.1 giây.
        {
            Invoke("ThaNhanh", 0.1f);
        }
    }

    // Phương thức này tạo ra một chức năng chậm trễ để ẩn/hiện nút cài đặt.
    private IEnumerator DelaySettingButton()
    {
        if (btnSetting.activeSelf)
        {
            btnSetting.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            btnSetting.gameObject.SetActive(true);
        }
    }

    // Phương thức này kiểm tra và kích hoạt các phần tử rồng theo trình tự.
    private void CheckAndActivateRongItems()
    {
        bool ok = false;
        ItemRongVienChinh item = null;

        if (!thahetrong && SoDoihinh < ContentItemRong.transform.childCount)
        {
            item = ContentItemRong.transform.GetChild(SoDoihinh).GetComponent<ItemRongVienChinh>();
        }

        for (int i = 0; i < ContentItemRong.transform.childCount; i++)
        {
            Button button = ContentItemRong.transform.GetChild(i).GetComponent<Button>();
            if (button.interactable)
            {
                item = ContentItemRong.transform.GetChild(i).GetComponent<ItemRongVienChinh>();
                debug.Log(i + " id " + item.idRong);
                if (item.hoisinh || thahetrong)
                {
                    item.TrieuHoi();
                    ok = true;
                    if (i == ContentItemRong.transform.childCount - 1)
                    {
                        thahetrong = true;
                    }
                    break;
                }
            }
        }

        if (item != null && item.gameObject.activeSelf == false && ok)
        {
            int index = item.transform.GetSiblingIndex();
            if (index > trang * 9 - 1)
            {
                DeactivatePreviousItems();
                ActivateNextItems();
            }
            else
            {
                ActivateNextItemsFromIndex(index);
            }
        }
    }

    // Ẩn các phần tử rồng trước đó.
    private void DeactivatePreviousItems()
    {
        for (int i = SoDoihinh - 1; i >= SoDoihinh - 9; i--)
        {
            if (i >= 0)
            {
                ContentItemRong.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // Kích hoạt các phần tử rồng tiếp theo.
    private void ActivateNextItems()
    {
        for (int i = SoDoihinh; i < SoDoihinh + 9; i++)
        {
            if (i < ContentItemRong.transform.childCount)
            {
                ContentItemRong.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // Kích hoạt các phần tử rồng từ một index cụ thể.
    private void ActivateNextItemsFromIndex(int index)
    {
        for (int i = index; i < index + 9; i++)
        {
            if (i < ContentItemRong.transform.childCount)
            {
                ContentItemRong.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // Cập nhật trang hiện tại của danh sách phần tử rồng.
    private void UpdateCurrentPage()
    {
        for (int i = 0; i < ContentItemRong.transform.childCount; i++)
        {
            if (ContentItemRong.transform.GetChild(i).gameObject.activeSelf)
            {
                trang = (byte)((i + 9) / 9);
                break;
            }
        }
    }
    private void OnDestroy()
    {
        Destroy(paneltoi);
    }
    //private void ThaNhanh()
    //{
    //    // if (SoDoihinh < ContentItemRong.transform.childCount && thahetrong == false)
    //    // if (thahetrong == false)
    //    //{
    //    if (btnSetting.activeSelf)
    //    {
    //        btnSetting.gameObject.SetActive(false);
    //        StartCoroutine(delay());
    //        IEnumerator delay()
    //        {
    //            yield return new WaitForSeconds(2);
    //            btnSetting.gameObject.SetActive(true);
    //        }
    //    }

    //    bool ok = false;
    //    ItemRongVienChinh item = null;//ContentItemRong.transform.GetChild(SoDoihinh).GetComponent<ItemRongVienChinh>();
    //    if (thahetrong == false)
    //    {
    //        if(SoDoihinh < ContentItemRong.transform.childCount) item = ContentItemRong.transform.GetChild(SoDoihinh).GetComponent<ItemRongVienChinh>();

    //    }
    //    for (int i = 0; i < ContentItemRong.transform.childCount; i++)
    //    {
    //        if (ContentItemRong.transform.GetChild(i).GetComponent<Button>().interactable)
    //        {
    //            item = ContentItemRong.transform.GetChild(i).GetComponent<ItemRongVienChinh>();
    //            debug.Log(i + " id " + item.idRong);
    //            if (item.hoisinh)
    //            {
    //                item.TrieuHoi(); ok = true;
    //                if (i == ContentItemRong.transform.childCount - 1) thahetrong = true;
    //                break;
    //            }
    //            else if (thahetrong)
    //            {
    //                item.TrieuHoi(); ok = true;
    //                if (i == ContentItemRong.transform.childCount - 1) thahetrong = true;
    //                break;
    //            }
    //        }
    //    }
    //    if (item != null)
    //    {
    //        if (item.gameObject.activeSelf == false && ok)
    //        {
    //            int index = item.transform.GetSiblingIndex();
    //            if (index > trang * 9 - 1)
    //            {
    //                for (int i = SoDoihinh - 1; i >= SoDoihinh - 9; i--)
    //                {
    //                    ContentItemRong.transform.GetChild(i).gameObject.SetActive(false);
    //                }
    //                for (int i = SoDoihinh; i < SoDoihinh + 9; i++)
    //                {
    //                    if(i < ContentItemRong.transform.childCount)
    //                    {
    //                        ContentItemRong.transform.GetChild(i).gameObject.SetActive(true);
    //                    }    
    //                }
    //            }
    //            else
    //            {
    //                for (int i = index; i < index + 9; i++)
    //                {
    //                    ContentItemRong.transform.GetChild(i).gameObject.SetActive(true);
    //                }
    //            }
    //        }
    //        for (int i = 0; i < ContentItemRong.transform.childCount; i++)
    //        {
    //            if (ContentItemRong.transform.GetChild(i).gameObject.activeSelf)
    //            {
    //                trang = (byte)((i + 9) / 9);
    //                break;
    //            }
    //        }
    //    }
    //    if (thanhanh)
    //    {
    //        Invoke("ThaNhanh", 0.1f);
    //    }
    //}
    public void ThaNhanh(bool tha)
    {
        thanhanh = tha;
        if (!tha)
        {
            CancelInvoke();
            return;
        }
        ThaNhanh();
    }
    public void QuayVe()
    {
        ThongKeDame.ResetThongKe();
        VienChinh.vienchinh.QuayVe();
    }
    public void ThoatTran()
    {
        VienChinh.vienchinh.ClearQuai();
        VienChinh.vienchinh.QuayVe();
    }
    public void MuaHuyenTinh()
    {
        Shop shop = AllMenu.ins.GetCreateMenu("MenuShop", null, true).GetComponent<Shop>();
        shop.transform.GetChild(0).gameObject.SetActive(false);
        shop.LoadInfoitem("HuyenTinh");
    }
    private void OnDisable()
    {
        thanhanh = false;
    }
    public void ThongKe()
    {
        if (ThongKeDame.Data["TeamXanh"].ISNull)
        {
            CrGame.ins.OnThongBaoNhanh("Không có thống kê!");
            return;
        } 
            
        MenuThongKeTranDau menuthongke = AllMenu.ins.GetCreateMenu("MenuThongKeTranDau",CrGame.ins.trencung.gameObject,false,CrGame.ins.menulogin.transform.GetSiblingIndex()-1).GetComponent<MenuThongKeTranDau>() ;//CrGame.ins.trencung.transform.Find("MenuThongKeTranDau").GetComponent<MenuThongKeTranDau>();
        menuthongke.ParseData();
    }    
}
