using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

//JSONClass datasend = new JSONClass();
//         datasend["class"] = "EventTraoHongDoatLong";
//         datasend["method"] = "ThuHoach";
//         datasend["data"]["idrong"] = gameObject.name;
//         //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
//         NetworkManager.ins.SendServer(datasend, Ok);
//         void Ok(JSONNode json)
//         {
//             if (json["status"].AsString == "0")
//             {

//             }
//             else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

//         }
public class MenuEventTraoHongDoatLong : EventManager
{
    private float nextDecreaseTime;
    private bool isTruGiay = false;

    public RectTransform imageRectTransform; // Tham chiếu đến RectTransform của ảnh
    public Transform allHoaHong, allHoaNguSac;
    public GameObject panelInfoHoa;
    public RuntimeAnimatorController hoaNguSac1, hoaNguSac2;
    private float[] TimeHoaHongConLai = new float[15];
    private float[] MaxTimeHoaHong = new float[15];

    private byte[] statusHoaHong = new byte[15];
    public Text txtTimeHoaHong;

    private byte indexHoaHongChon = 0;
    public Transform KhungHoa;
    public GameObject txtAnim, menuXacNhanSoc, menuNhanQuaSoc, allSoc, MenuDoiHoa;
    public Transform[] BongBong;
    public bool socnongdan = false;
    private int socThuThap, gioiHanSocNongDan;
    public bool socNongDan
    {
        get { return socnongdan; }
        set
        {
            socnongdan = value;
            if (socnongdan)
            {
                CheckSocAutoThuHoach();
            }
            else
            {
                for (int i = 0; i < allSoc.transform.childCount; i++)
                {
                    allSoc.transform.GetChild(i).GetComponent<Animator>().Play("Sit");
                }
            }
        }
    }
    private int socNongDanThuThap
    {
        get { return socThuThap; }
        set
        {
            socThuThap = value;
            bool active = socThuThap > 0;
            for (int i = 0; i < allSoc.transform.childCount; i++)
            {
                allSoc.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(active);
            }

        }
    }
    private void CheckSocAutoThuHoach()
    {
        for (int i = 0; i < statusHoaHong.Length; i++)
        {
            if (statusHoaHong[i] == 2)
            {
                SocThuHoach();
                break;
            }
        }
    }
    public void OpenMenuDoiManh2()
    {
        //return;
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "MenuDoiManh";
        //  if (menuevent.ContainsKey("GiaoDien2")) datasend["method"] = "MenuDoiManh2";
        NetworkManager.ins.SendServer(datasend, Ok);

        void Ok(JSONNode json)
        {
            if (json["status"].Value == "0")
            {
                GameObject menuDoiManh = EventManager.ins.GetCreateMenu("MenuDoiManh", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
                GameObject ContentManh = menuDoiManh.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
                GameObject ObjectManh = ContentManh.transform.GetChild(0).gameObject;

                for (int i = 0; i < json["ManhDoi"].Count; i++)
                {
                    GameObject manh = Instantiate(ObjectManh, ContentManh.transform.position, Quaternion.identity);
                    manh.transform.SetParent(ContentManh.transform, false);
                    manh.GetComponent<Button>().onClick.AddListener(XemManhDoi);
                    Image imgmanh = manh.transform.GetChild(0).GetComponent<Image>();
                    if (json["ManhDoi"][i]["itemgi"].Value == "Item" || json["ManhDoi"][i]["itemgi"].Value == "ThuyenThucAn")
                    {
                        imgmanh.sprite = Inventory.LoadSprite(json["ManhDoi"][i]["nameitem"].Value);
                    }
                    else if (json["ManhDoi"][i]["itemgi"].Value == "ItemEvent")
                    {
                        imgmanh.sprite = EventManager.ins.GetSprite(json["ManhDoi"][i]["nameitem"].AsString);
                        imgmanh.SetNativeSize();
                    }
                    else if (json["ManhDoi"][i]["itemgi"].Value == "Avatar")
                    {
                        Friend.ins.LoadImage("avt", json["ManhDoi"][i]["nameitem"].AsString, imgmanh);
                        // imgmanh.sprite = EventManager.ins.GetSprite(json["ManhDoi"][i]["nameitem"].AsString);
                        // imgmanh.SetNativeSize();
                    }
                    else
                    {
                        imgmanh.sprite = Inventory.LoadSpriteRong(json["ManhDoi"][i]["nameitem"].Value + "2");
                    }
                    imgmanh.SetNativeSize();
                    manh.name = json["ManhDoi"][i]["namekey"];
                    manh.SetActive(true);
                    GamIns.ResizeItem(imgmanh, 200);
                }

                menuDoiManh.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DoiManh);
                menuDoiManh.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ExitDoiManh);
                menuDoiManh.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private int YeuCauNguSac = 0;
    public void ParseData(JSONNode json)
    {
        JSONNode data = json["data"];
        JSONNode datHoaHong = data["allHoaHong"];
        JSONNode dataHoaNguSac = data["allHoaNguSac"];
        socNongDan = data["socNongDan"].AsBool;
        socNongDanThuThap = data["socThuThap"].AsInt;
        gioiHanSocNongDan = json["GioiHanSocThuThap"].AsInt;
        int SoHoaNguSacFalse = 0;
        for (int i = 0; i < 15; i++)
        {
            TimeHoaHongConLai[i] = json["TimeHoaHongConLai"][i].AsFloat;
            MaxTimeHoaHong[i] = json["MaxTimeHoaHong"][i].AsFloat;
            Transform HoaHong = allHoaHong.transform.GetChild(i);

            Transform HoaNguSac = allHoaNguSac.transform.GetChild(i);
            bool active = data["allHoaNguSac"][i]["active"].AsBool;
            HoaNguSac.gameObject.SetActive(active);
            //   Image imgHoaHong = HoaHong.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            Animator animNguSac = HoaNguSac.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
            if (!active) SoHoaNguSacFalse += 1;
            HoaHong.name = datHoaHong[i]["hoa"].AsString;
            HoaNguSac.name = dataHoaNguSac[i]["hoa"].AsString;

            SetHoaHongNo(i);

            if (dataHoaNguSac[i]["hoa"].AsString == "1")
            {
                animNguSac.runtimeAnimatorController = hoaNguSac1;
            }
            else animNguSac.runtimeAnimatorController = hoaNguSac2;
        }

        if (SoHoaNguSacFalse >= dataHoaNguSac.Count)
        {
            if (!data["ResetHoaNguSac"].AsBool)
            {
                transform.Find("KhungHoa").transform.Find("btnLamMoi").gameObject.SetActive(true);
            }
        }
        nextDecreaseTime = Time.time + 1f; // Đặt thời gian đầu tiên để trừ
        isTruGiay = true;
        KhungHoa.transform.Find("txtTimeEvent").GetComponent<Text>().text = json["txtTimeEvent"].AsString;
        YeuCauNguSac = json["YeuCauNguSac"].AsInt;
        SetTxtHoaHong(json["TongHoaHong"].AsString);
        SetTxtHoaNguSac(data["HoaNguSac"].AsString);
        SetLuotHaiNguSacFree(data["luotHaiNguSacFree"].AsString);
        btnHopQua = CrGame.ins.giaodien.transform.Find("btnQuaOnline").gameObject;
        btnHopQua.transform.SetParent(CrGame.ins.trencung.transform);
        SetQuaAi(json["QuaAi"], data["HoaNguSac"].AsInt, data["phantramGiaiPhongAn"].AsString, data["aiHienTai"].AsString);

        Transform vitriRong = transform.Find("vitriRong");
        GameObject rong = AllMenu.ins.GetRongGiaoDien("RongNuTamXuan1", vitriRong, 1);
        Vector3 scale = rong.transform.localScale;
        rong.transform.localScale = new Vector3(scale.x * 1.2f, scale.y * 1.2f, scale.z);
        panelInfoHoa.transform.SetParent(CrGame.ins.trencung.transform);
        gameObject.SetActive(true);
    }
    private void SetQuaAi(JSONNode dataAi, int HoaNguSacHienTai, string phantramphongan, string aihientai)
    {
        Text txtHoaGiai = transform.Find("txtHoaGiai").GetComponent<Text>();
        txtHoaGiai.text = "Đã hóa giải được <color=lime>" + phantramphongan + "%</color> phong ấn.\n Rồng Nụ Tầm Xuân\n\n\n\n<color=yellow>Ải hiện tại: </color>"+aihientai;
        for (int i = 0; i < 2; i++)
        {
            Image imgItem = BongBong[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            Text txtSoLuong = BongBong[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            Text txtyeucau = BongBong[i].transform.Find("txtyeucau").GetComponent<Text>();
            LoaiItem loai = (LoaiItem)Enum.Parse(typeof(LoaiItem), dataAi[i]["loai"].AsString, true);
            imgItem.sprite = GetSpriteAll(dataAi[i]["name"].AsString, loai);
            imgItem.SetNativeSize();
            imgItem.Resize(150);
            if (loai == LoaiItem.rong)
            {
                txtSoLuong.text = dataAi[i]["sao"].AsString + " sao";
            }
            else txtSoLuong.text = dataAi[i]["soluong"].AsString;
            imgItem.name = dataAi[i]["name"].AsString;
            //Transform txt = KhungHoa.transform.Find("txtHoaNguSac");

            txtyeucau.text = HoaNguSacHienTai >= YeuCauNguSac ? "<color=lime>" + HoaNguSacHienTai + "/" + YeuCauNguSac + "</color>" : "<color=red>" + HoaNguSacHienTai + "/" + YeuCauNguSac + "</color>";
        }
    }
    private bool xacNhanChonQua = false;

    public void ChonQuaAi(int qua)
    {
        GameObject btnChon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Send();
        void Send()
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "EventTraoHongDoatLong";
            datasend["method"] = "ChonItem";
            datasend["data"]["qua"] = qua.ToString();
            datasend["data"]["xacnhan"] = xacNhanChonQua.ToString();

            //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    xacNhanChonQua = false;
                    LoaiItem loai = (LoaiItem)Enum.Parse(typeof(LoaiItem), json["QuaChon"]["loai"].AsString, true);
                    string hienthi = loai == LoaiItem.rong ? json["QuaChon"]["sao"].AsString + " sao" : "x" + GamIns.FormatCash(json["QuaChon"]["soluong"].AsInt);
                    for (int i = 0; i < 2; i++)
                    {
                        BongBong[i].transform.LeanScale(Vector3.zero, 0.4f);
                    }
                    YeuCauNguSac = json["YeuCauNguSac"].AsInt;
                    OpenMenuNhanDuocItem(json["QuaChon"]["name"].AsString, hienthi, loai, () => {
                        SetQuaAi(json["QuaAi"], json["HoaNguSac"].AsInt, json["phantramGiaiPhongAn"].AsString,json["aiHienTai"].AsString);
                        for (int i = 0; i < 2; i++)
                        {
                            BongBong[i].transform.LeanScale(Vector3.one, 0.4f);
                        }
                        if (json["nhanRong"].AsBool)
                        {
                            OpenMenuNhanDuocItem(json["RongNhan"]["name"].AsString, json["RongNhan"]["sao"].AsString + " sao", LoaiItem.rong);
                        }
                    });
                    SetTxtHoaNguSac(json["HoaNguSac"].AsString);

                }
                else if (json["status"].AsString == "2")
                {
                    EventManager.OpenThongBaoChon(json["message"].AsString, () => { xacNhanChonQua = true; Send(); });
                }
                else if (json["status"].AsString == "3")
                {
                    EventManager.OpenThongBaoChon(json["message"].AsString, () => { });
                }
                else
                {
                    xacNhanChonQua = false;
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }

            }
        }

    }

    private void SetLuotHaiNguSacFree(string luot)
    {
        transform.Find("txtFree").GetComponent<Text>().text = "Bạn có <color=lime>" + luot + " lượt</color> hái hoa ngũ sắc <color=lime>miễn phí</color>";
    }
    private void InsTxtAnim(Transform tf, string soluong = "+1")
    {
        GameObject txtAnimClone = Instantiate(txtAnim, transform.position, Quaternion.identity);
        txtAnimClone.transform.SetParent(CrGame.ins.trencung, false);
        txtAnimClone.transform.position = new Vector3(tf.transform.position.x + 1, tf.transform.position.y, tf.transform.position.z);
        txtAnimClone.transform.GetChild(0).GetComponent<Text>().text = soluong;
        txtAnimClone.SetActive(true);
        Destroy(txtAnimClone, 3f);
    }

    private void SetTxtHoaHong(string sl, bool txtanim = false, string slcong = "+1")
    {
        Transform txt = KhungHoa.transform.Find("txtHoaHongThuong");
        txt.GetComponent<Text>().text = "<color=orange>Hoa hồng thường</color>: <color=yellow>" + sl + "</color> bông";
        if (txtanim) InsTxtAnim(KhungHoa.transform.Find("HoaHongThuong"), slcong);
    }
    private void SetTxtHoaNguSac(string sl, bool txtanim = false, string slcong = "+1")
    {
        Transform txt = KhungHoa.transform.Find("txtHoaNguSac");
        txt.GetComponent<Text>().text = "<color=orange>Hoa Ngũ Sắc</color>: <color=yellow>" + sl + "</color> bông";
        if (txtanim) InsTxtAnim(KhungHoa.transform.Find("HoaNguSac"), slcong);


        for (int i = 0; i < 2; i++)
        {
            Text txtyeucau = BongBong[i].transform.Find("txtyeucau").GetComponent<Text>();
            int HoaNguSacHienTai = int.Parse(sl);
            txtyeucau.text = HoaNguSacHienTai >= YeuCauNguSac ? "<color=lime>" + HoaNguSacHienTai + "/" + YeuCauNguSac + "</color>" : "<color=red>" + HoaNguSacHienTai + "/" + YeuCauNguSac + "</color>";
        }
    }
    bool SendSocThuHoach = false;
    private void SetHoaHongNo(int i)
    {
        if (TimeHoaHongConLai[i] <= 0 && statusHoaHong[i] != 2)
        {
            statusHoaHong[i] = 2;//đã nở

            SetSpriteHoaHong(i, "hoa");
            SocThuHoach();

            //if(socNongDan && socNongDanThuThap < gioiHanSocNongDan)
            //{
            //      Transform HoaHong = allHoaHong.transform.GetChild(i);
            //      ThuHoachHoaHong(i,HoaHong.transform.GetChild(0).transform.GetChild(0).gameObject,HoaHong);
            // }
        }
        else if (TimeHoaHongConLai[i] <= MaxTimeHoaHong[i] / 2 && statusHoaHong[i] != 1)
        {
            statusHoaHong[i] = 1;// nở thành nụ
            SetSpriteHoaHong(i, "nu");
        }
        else
        {
            statusHoaHong[i] = 0;// chưa nở
        }
    }

    private void SetSpriteHoaHong(int i, string StrSprite)
    {
        Transform HoaHong = allHoaHong.transform.GetChild(i);
        string hoa = HoaHong.name;
        Image imgHoaHong = HoaHong.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        imgHoaHong.sprite = GetSprite(StrSprite + hoa);
        imgHoaHong.SetNativeSize();
    }
    private void Update()
    {
        if (isTruGiay)
        {
            if (Time.time >= nextDecreaseTime)
            {
                for (int i = 0; i < TimeHoaHongConLai.Length; i++)
                {
                    if (TimeHoaHongConLai[i] > 0)
                    {
                        TimeHoaHongConLai[i] -= 1f;
                        SetHoaHongNo(i);

                    }
                }
                nextDecreaseTime += 1f; // Đặt mốc thời gian tiếp theo
                                        //Debug.Log("Time: " + time);
            }

            txtTimeHoaHong.text = TimeHoaHongConLai[indexHoaHongChon] + " giây";
        }
        //if(Input.GetKeyUp(KeyCode.Z))
        //{
        //    SocThuHoach();
        //}
    }
    protected override void ABSAwake()
    {

    }
    void Start()
    {


        //  AdjustImageSize();
    }
    void AdjustImageSize()
    {
        float screenHeight = Screen.height;
        float worldHeight = GetWorldHeight(imageRectTransform);

        Debug.Log("📏 Screen Height: " + screenHeight);
        Debug.Log("🖼️ World Height (Image): " + worldHeight);
        Debug.Log("📏 Image Local Scale: " + imageRectTransform.localScale);

        if (worldHeight < screenHeight)
        {
            float scaleFactor = screenHeight / worldHeight;
            imageRectTransform.localScale *= scaleFactor;
            Debug.Log("✅ Adjusted Image Scale: " + imageRectTransform.localScale);
        }
    }

    float GetWorldHeight(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return corners[1].y - corners[0].y;
    }
    protected override void DiemDanhOk(JSONNode json)
    {
        SetLuotHaiNguSacFree(json["data"]["luotHaiNguSacFree"].AsString);
    }

    private JSONClass DataSendThuHoach(int index, string loaihoa)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "ThuHoach";
        datasend["data"]["index"] = index.ToString();
        datasend["data"]["loai"] = loaihoa;
        return datasend;
    }
    private void quaBay(Transform parent, Transform g, string loaihoa = "HoaHongThuong")
    {
        GameObject hoaClone = Instantiate(parent.gameObject, transform.position, Quaternion.identity);
        hoaClone.transform.SetParent(CrGame.ins.trencung, false);
        hoaClone.transform.position = g.transform.position;
        Transform tfHoaHong = transform.Find("KhungHoa").transform.Find(loaihoa);
        QuaBay quabay = hoaClone.AddComponent<QuaBay>();
        quabay.vitribay = tfHoaHong.gameObject;

    }
    bool xacnhanHaiNguSac = false;
    public void BtnHoaNguSac()
    {
        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Transform parent = g.transform.parent.transform.parent;
        int index = parent.transform.GetSiblingIndex();

        indexHoaHongChon = (byte)index;
        Send();
        // string namesprite = g.GetComponent<Image>().sprite.name;  
        void Send()
        {
            JSONClass datasend = DataSendThuHoach(index, "HoaNguSac");
            datasend["data"]["xacnhan"] = xacnhanHaiNguSac.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    xacnhanHaiNguSac = false;
                    quaBay(parent, g.transform, "HoaNguSac");
                    SetTxtHoaNguSac(json["HoaNguSac"].AsString, true);
                    parent.gameObject.SetActive(false);

                    int soHoaNguSacFalse = 0;
                    for (int i = 0; i < allHoaNguSac.transform.childCount; i++)
                    {
                        if (!allHoaNguSac.transform.GetChild(i).gameObject.activeSelf)
                        {
                            soHoaNguSacFalse += 1;
                        }
                    }
                    SetLuotHaiNguSacFree(json["luotHaiNguSacFree"].AsString);

                    if (soHoaNguSacFalse >= allHoaNguSac.transform.childCount)
                    {
                        if (!json["ResetHoaNguSac"].AsBool)
                        {
                            transform.Find("KhungHoa").transform.Find("btnLamMoi").gameObject.SetActive(true);
                        }
                    }
                    // transform.Find("KhungHoa").transform.Find("btnLamMoi").gameObject.SetActive(true);
                }
                else if (json["status"].AsString == "2")
                {
                    EventManager.OpenThongBaoChon(json["message"].AsString, () => { xacnhanHaiNguSac = true; Send(); });
                }
                else
                {
                    xacnhanHaiNguSac = false;
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }


            }
        }

    }
    public void BtnHoaHong()
    {
        if (socNongDan && socNongDanThuThap < gioiHanSocNongDan) return;

        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Transform parent = g.transform.parent.transform.parent;
        int index = parent.transform.GetSiblingIndex();
        if (statusHoaHong[index] != 2)
        {
            CrGame.ins.OnThongBaoNhanh("Chưa được thu hoạch!");
            return;
        }
        ThuHoachHoaHong(index, g, parent);
    }
    private void ThuHoachHoaHong(int index, GameObject g, Transform parent)
    {
        indexHoaHongChon = (byte)index;
        // string namesprite = g.GetComponent<Image>().sprite.name;  
        NetworkManager.ins.SendServer(DataSendThuHoach(index, "HoaHong"), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                quaBay(parent, g.transform);
                SetTxtHoaHong(json["HoaHong"].AsString, true);
                TimeHoaHongConLai[index] = json["TimeYeuCau"].AsFloat;
                MaxTimeHoaHong[index] = TimeHoaHongConLai[index];
                parent.name = json["hoaNew"]["hoa"].AsString;
                Image img = g.GetComponent<Image>();
                img.sprite = GetSprite("mamhoa");
                img.SetNativeSize();
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

        }
    }
    private void SocThuHoach()
    {
        debug.Log("soc thu hoachh, SendSocThuHoach: " + SendSocThuHoach + ", socNongDanThuThap: " + socNongDanThuThap + ", gioiHanSocNongDan: " + gioiHanSocNongDan);
        if (!socNongDan) return;
        if (SendSocThuHoach || socNongDanThuThap >= gioiHanSocNongDan) return;
        SendSocThuHoach = true;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "SocThuHoach";
        // string namesprite = g.GetComponent<Image>().sprite.name;  
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                JSONNode allHoaThuHoach = json["allHoaThuHoach"];
                JSONNode allTimeYeuCau = json["allTimeYeuCau"];
                JSONNode allHoaNew = json["allHoaNew"];
                //  quaBay(parent,g.transform);
                // SetTxtHoaHong(json["HoaHong"].AsString);
                // TimeHoaHongConLai[index] = json["TimeYeuCau"].AsFloat;
                // MaxTimeHoaHong[index] = TimeHoaHongConLai[index];
                // parent.name = json["hoaNew"]["hoa"]
                // .AsString;
                List<Transform> allsocThuThap = new();
                int soSocDiThuHoach = allHoaThuHoach.Count;
                if (soSocDiThuHoach >= allSoc.transform.childCount)
                {
                    soSocDiThuHoach = allSoc.transform.childCount;
                    for (int i = 0; i < soSocDiThuHoach; i++)
                    {
                        FLowersToggle soc = allSoc.transform.GetChild(i).GetComponent<FLowersToggle>();
                        soc.ThuHoach();
                        allsocThuThap.Add(soc.transform);
                    }
                }
                else
                {
                    int soSocRandomOk = 0;
                    for (int i = 0; i < allSoc.transform.childCount; i++)
                    {
                        if (UnityEngine.Random.Range(0, 100) > 50)
                        {
                            FLowersToggle soc = allSoc.transform.GetChild(i).GetComponent<FLowersToggle>();
                            if (!soc.thuhoach && soc.walk)
                            {
                                soSocRandomOk++;
                                soc.ThuHoach();
                                allsocThuThap.Add(soc.transform);
                                if (soSocRandomOk >= soSocDiThuHoach) break;
                            }
                        }
                    }
                    if (soSocRandomOk < soSocDiThuHoach)
                    {
                        for (int i = 0; i < allSoc.transform.childCount; i++)
                        {
                            FLowersToggle soc = allSoc.transform.GetChild(i).GetComponent<FLowersToggle>();
                            soc.ThuHoach();
                            allsocThuThap.Add(soc.transform);
                        }
                    }
                }
                StartCoroutine(WaitForSocThuHoach());
                IEnumerator WaitForSocThuHoach()
                {
                    yield return new WaitForSeconds(1f);
                    for (int i = 0; i < allHoaThuHoach.Count; i++)
                    {
                        int index = allHoaThuHoach[i].AsInt;
                        float startTime = Time.time; // Lưu thời điểm bắt đầu

                        yield return new WaitUntil(() => statusHoaHong[index] == 2 || (Time.time - startTime > 3f)); // Chờ tối đa 3 giây

                        if (statusHoaHong[index] == 2)
                        {
                            //Debug.Log("Điều kiện đã thỏa mãn!");
                            Transform parent = allHoaHong.transform.GetChild(allHoaThuHoach[i].AsInt);
                            GameObject g = parent.transform.GetChild(0).transform.GetChild(0).gameObject;


                            GameObject hoaClone = Instantiate(parent.gameObject, transform.position, Quaternion.identity);
                            hoaClone.transform.SetParent(CrGame.ins.trencung, false);
                            hoaClone.transform.position = g.transform.position;
                            Transform vitribay = allsocThuThap[UnityEngine.Random.Range(0, allsocThuThap.Count)];
                            hoaClone.LeanMove(vitribay.transform.position, 0.5f).setOnComplete(() => {
                                Destroy(hoaClone.gameObject);
                            });


                            TimeHoaHongConLai[index] = allTimeYeuCau[i].AsFloat;
                            MaxTimeHoaHong[index] = allTimeYeuCau[i].AsFloat;
                            parent.name = allHoaNew[i]["hoa"].AsString;
                            Image img = g.GetComponent<Image>();
                            img.sprite = GetSprite("mamhoa");
                            img.SetNativeSize();
                            //ThuHoachHoaHong(i,HoaHong.transform.GetChild(0).transform.GetChild(0).gameObject,HoaHong);
                        }
                        else
                        {
                            Debug.Log("Hết thời gian chờ!");
                        }
                    }
                }



                socNongDan = json["socNongDan"].AsBool;
                socNongDanThuThap = json["socThuThap"].AsInt;
                gioiHanSocNongDan = json["GioiHanSocThuThap"].AsInt;
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            SendSocThuHoach = false;
        }
    }


    private void SetTransformPanelInfoHoa(Transform tf)
    {
        Vector3 vec = tf.transform.position;
        if (vec.y > 2)// nếu như ở bên trên quá thì cho hiện ở bên dưới
        {
            vec = new Vector3(vec.x, vec.y - 2, vec.z);
        }
        else if (vec.y < 2)
        {
            vec = new Vector3(vec.x, vec.y + 2, vec.z);
        }
        else
        {
            vec = new Vector3(vec.x, vec.y + 2, vec.z);
        }
        panelInfoHoa.transform.position = vec;
    }
    public void XemInfoHoaHong(bool b)
    {
        if (b)
        {
            GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            Transform parent = g.transform.parent.transform.parent;
            int index = parent.transform.GetSiblingIndex();
            indexHoaHongChon = (byte)index;
            string namesprite = g.GetComponent<Image>().sprite.name;

            string namehoa = "<color=lime>" + GetNameHoa(parent.name) + "</color>";
            string info = "";
            string time = "Sẽ nở thành hoa sau";
            if (namesprite.Contains("mamhoa"))
            {
                info = "Hạt giống bé nhỏ";
                txtTimeHoaHong.gameObject.SetActive(true);
            }
            else if (namesprite.Contains("nu"))
            {
                info = "Nụ hoa tươi đẹp";
                txtTimeHoaHong.gameObject.SetActive(true);

            }
            else if (namesprite.Contains("hoa"))
            {
                time = "";
                info = "Hoa đã nở, click để hái nào!";
                txtTimeHoaHong.gameObject.SetActive(false);
            }

            panelInfoHoa.transform.GetChild(0).GetComponent<Text>().text = namehoa + "\n" + info + "\n" + time;
            txtTimeHoaHong.text = TimeHoaHongConLai[index] + " giây";
            SetTransformPanelInfoHoa(parent);
        }
        panelInfoHoa.SetActive(b);
    }
    public void XemInfoHoaNguSac(bool b)
    {
        if (b)
        {
            GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            Transform parent = g.transform.parent.transform.parent;
            int index = parent.transform.GetSiblingIndex();
            indexHoaHongChon = (byte)index;
            //  string namesprite = g.GetComponent<Image>().sprite.name;

            string namehoa = "<color=lime>Hoa Ngũ Sắc</color>";
            string time = "";
            string info = "Hoa đã nở, click để hái nào!";
            txtTimeHoaHong.gameObject.SetActive(false);

            panelInfoHoa.transform.GetChild(0).GetComponent<Text>().text = namehoa + "\n" + info + "\n" + time;
            txtTimeHoaHong.text = TimeHoaHongConLai[index] + " giây";
            SetTransformPanelInfoHoa(parent);

        }
        panelInfoHoa.SetActive(b);
    }
    private string GetNameHoa(string id)
    {
        switch (id)
        {
            case "1": return "Hoa Hồng Cam";
            case "2": return "Hoa Hồng Tím";
            case "3": return "Hoa Hồng Vàng";
            case "4": return "Hoa Hồng Xanh Lá";
            case "5": return "Hoa Hồng Xanh Dương";
            case "6": return "Hoa Hồng Đỏ";
            case "7": return "Hoa Hồng Trắng";
            default: return "";
        }
    }
    public void OpenMenuDoiHoa()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "GetMenuDoiHoa";
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                MenuDoiHoa.transform.SetParent(CrGame.ins.trencung.transform);
                MenuDoiHoa.transform.SetAsFirstSibling();
                Transform PanelHoa = MenuDoiHoa.transform.Find("Panel");

                Text txtTongHoa = MenuDoiHoa.transform.GetChild(3).GetComponent<Text>();
                txtTongHoa.text = "Để đổi <color=lime>1 Hoa Ngũ Sắc</color> bạn cần <color=yellow>100 Hoa Hồng Thường</color>. Hiện bạn đang có <color=yellow>" + json["TongHoa"].AsString + " Hoa Hồng Thường</color>";
                for (int i = 0; i < json["allSoluongHoa"].Count; i++)
                {
                    PanelHoa.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = json["allSoluongHoa"][i].AsString;
                }
                MenuDoiHoa.transform.GetChild(0).transform.Find("txtSoLuong").GetComponent<Text>().text = json["tongHoaDoi"].AsString;
                MenuDoiHoa.SetActive(true);
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

        }
    }
    bool xacNhanDoiNguSac = false;
    public void DoiHoaNguSac()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "DoiHoaNguSac";
        datasend["data"]["xacNhanDoiNguSac"] = xacNhanDoiNguSac.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
              //  GameObject menuDoiHoa = transform.Find("MenuDoiHoa").gameObject;
                GameObject imgHoaNguSac = MenuDoiHoa.transform.GetChild(0).transform.Find("imgHoa").gameObject;
                GameObject imgClone = Instantiate(imgHoaNguSac, transform.position, Quaternion.identity);
                imgClone.transform.SetParent(CrGame.ins.trencung, false);
                imgClone.transform.position = imgHoaNguSac.transform.position;
                QuaBay quabay = imgClone.AddComponent<QuaBay>();
                Transform tfHoaHong = transform.Find("KhungHoa").transform.Find("HoaNguSac");
                quabay.vitribay = tfHoaHong.gameObject;
                MenuDoiHoa.SetActive(false);
                SetTxtHoaNguSac(json["HoaNguSac"].AsString, true, "+" + json["tongHoaDoi"].AsString);
                xacNhanDoiNguSac = false;
            }
            else if (json["status"].AsString == "2")
            {
                EventManager.OpenThongBaoChon(json["message"].AsString, () => { xacNhanDoiNguSac = true; DoiHoaNguSac(); });
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

        }
    }
    public void OpenMenuThueSoc()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "GetGiaThueSoc";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                menuXacNhanSoc.transform.SetParent(CrGame.ins.trencung.transform, false);

                Text txt = menuXacNhanSoc.transform.Find("txt").GetComponent<Text>();
                Text txtVang = menuXacNhanSoc.transform.Find("txtVang").GetComponent<Text>();
                Text txtKimCuong = menuXacNhanSoc.transform.Find("txtKimCuong").GetComponent<Text>();
                txt.text = json["txt"].AsString;
                txtVang.text = json["giaVang"].AsString;
                txtKimCuong.text = json["giaKimCuong"].AsString;
                menuXacNhanSoc.SetActive(true);
            }
            else if (json["status"].AsString == "2")
            {
                EventManager.OpenThongBaoChon(json["message"].AsString, () => { ThueSoc("KimCuong"); });
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }
    public void ThueSoc(string s)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "ThueSoc";
        datasend["data"]["loai"] = s;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                CrGame.ins.OnThongBaoNhanh(json["strThongBaoOk"].AsString, 3f);
                menuXacNhanSoc.SetActive(false);
                gioiHanSocNongDan = json["gioihanThuThap"].AsInt;
                socNongDan = true;
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }
    public void XemNhanHoaSoc()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "XemNhanHoaSoc";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                menuNhanQuaSoc.transform.SetParent(CrGame.ins.trencung, false);
                Text txtHoaHong = menuNhanQuaSoc.transform.Find("txtHoaHong").GetComponent<Text>();
                menuNhanQuaSoc.SetActive(true);
                txtHoaHong.text = json["strThuThap"].AsString;
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }
    public void XacNhanNhanQuaSoc()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "XacNhanNhanQuaSoc";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {

                GameObject HoaHong = menuNhanQuaSoc.transform.Find("HoaHong").gameObject;
                GameObject imgClone = Instantiate(HoaHong, transform.position, Quaternion.identity);
                imgClone.transform.SetParent(CrGame.ins.trencung, false);
                imgClone.transform.position = HoaHong.transform.position;
                QuaBay quabay = imgClone.AddComponent<QuaBay>();
                Transform tfHoaHong = transform.Find("KhungHoa").transform.Find("HoaHongThuong");
                quabay.vitribay = tfHoaHong.gameObject;
                socNongDanThuThap = 0;
                SetTxtHoaHong(json["HoaHong"].AsString, true, "+" + json["socThuThap"].AsString);
                CheckSocAutoThuHoach();
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            menuNhanQuaSoc.SetActive(false);
        }
    }
    bool XacNhanLamMoi = false;
    public void btnResetHoaNguSac()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTraoHongDoatLong";
        datasend["method"] = "LamMoiHoaNguSac";
        datasend["data"]["XacNhanLamMoi"] = XacNhanLamMoi.ToString();
        //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                transform.Find("KhungHoa").transform.Find("btnLamMoi").gameObject.SetActive(false);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    JSONNode allHoaNguSacReset = json["allHoaNguSacReset"];
                    for (int i = 0; i < 15; i++)
                    {
                        //
                        Transform HoaNguSac = allHoaNguSac.transform.GetChild(i);
                        Vector3 vecbandau = HoaNguSac.transform.position;
                        HoaNguSac.transform.position = new Vector3(vecbandau.x, vecbandau.y + 2);
                        HoaNguSac.gameObject.SetActive(true);

                        Animator animNguSac = HoaNguSac.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();


                        HoaNguSac.name = allHoaNguSacReset[i]["hoa"].AsString;



                        if (allHoaNguSacReset[i]["hoa"].AsString == "1")
                        {
                            animNguSac.runtimeAnimatorController = hoaNguSac1;
                        }
                        else animNguSac.runtimeAnimatorController = hoaNguSac2;
                        HoaNguSac.transform.LeanMove(vecbandau, 0.3f);

                        yield return new WaitForSeconds(0.11f);
                    }
                }


                debug.Log("Reset Hoa Ngũ Sắc");
            }
            else if (json["status"].AsString == "2")
            {
                EventManager.OpenThongBaoChon(json["message"].AsString, () => { XacNhanLamMoi = true; btnResetHoaNguSac(); });
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            XacNhanLamMoi = false;
        }
    }
    public void VeNha()
    {
        Destroy(menuXacNhanSoc.gameObject);
        Destroy(menuNhanQuaSoc.gameObject);
        Destroy(MenuDoiHoa.gameObject);
        Destroy(panelInfoHoa.gameObject);

        AudioManager.PlaySound("soundClick");
        btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
        CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject.SetActive(true);
        AudioManager.SetSoundBg("nhacnen0");
        // gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuEventTraoHongDoatLong");

    }
}
