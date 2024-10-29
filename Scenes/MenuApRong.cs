using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class MenuApRong : MonoBehaviour
{
    public Transform allLongAp, btnNuoi, objDangNuoi,AllbtnNhanRong;
    private float[] maxtimeap = new float[] {0,0,0};
    public float[] timeap = new float[] {0,0,0};
    public Text[] txttime;
    public Text[] txtphantram;
    public Image[] fillphantram;
    public GameObject animApXong,objChon;
    public Sprite spriteNgoiSaoSang;
    // Start is called before the first frame update
    void Start()
    {
        LoadItemDangCo();
        LoadItemNangCapDangCo();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < timeap.Length; i++)
        {
            if (timeap[i] > 1)
            {
                timeap[i] -= Time.deltaTime;

                int sec = Mathf.FloorToInt(timeap[i]);
                int minutes = 0;
                int gio = 0;

                while (sec >= 60)
                {
                    sec -= 60;
                    minutes += 1;
                }
                while (minutes >= 60)
                {
                    minutes -= 60;
                    gio += 1;
                }
                if (gio > 0)
                {
                    txttime[i].text = "Đang nuôi..." +  gio.ToString("D2") + ":" + minutes.ToString("D2") + ":" + sec.ToString("D2");
                }
                else txttime[i].text = "Đang nuôi..." + minutes.ToString("D2") + ":" + sec.ToString("D2");

                float phantram = Mathf.FloorToInt((1 - timeap[i] / maxtimeap[i]) * 100);
                txtphantram[i].text = phantram + "%";
                fillphantram[i].fillAmount = 1 - (timeap[i] / maxtimeap[i]);
                if(timeap[i] < 1)
                {
                    AllbtnNhanRong.transform.GetChild(i).gameObject.SetActive(true);
                    string namerong = allLongAp.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).name.Replace("1(Clone)", "");
                    debug.Log("namerong " + namerong);
                    AllMenu.ins.LoadRongGiaoDien(namerong + "2", allLongAp.transform.GetChild(i).transform.GetChild(0), 1, true, "RongGiaoDien", new Vector3(30, 30));
                    txttime[i].text = "";
                    objDangNuoi.transform.GetChild(i).gameObject.SetActive(false);
                    GameObject anim = Instantiate(animApXong, transform.position, Quaternion.identity);
                    anim.transform.SetParent(allLongAp.transform.GetChild(i),false);
                    anim.transform.position = allLongAp.transform.GetChild(i).transform.position;
                    anim.transform.localScale = new Vector3(2,2);
                    anim.SetActive(true);
                    Destroy(anim, 3f);
                }    
            }
            //else
            //{
            //    timeap[i] = 0;
            //}
        }
      
    }
    string[] levelLaMa = new string[] { "0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII" };
    public void ParseData(JSONNode data)
    {
        debug.Log(data.ToString());

        for (int i = 1; i < 3; i++)
        {
            debug.Log(data["data"]["LongAp" + i]["lock"].AsString);
            if (!data["data"]["LongAp" + i]["lock"].AsBool)
            {
                Destroy(allLongAp.transform.GetChild(i).transform.GetChild(1).gameObject);
            }
           
        }
        for (int i = 0; i < 3; i++)
        {
            if (!data["data"]["LongAp" + i]["rong"].ISNull)
            {
                idronglongap[i] = data["data"]["LongAp" + i]["rong"]["id"].AsString;
                dangap[i] = true;
                if(data["data"]["LongAp" + i]["sec"].AsFloat <= 0)
                {
                    AllbtnNhanRong.transform.GetChild(i).gameObject.SetActive(true);
                    AllMenu.ins.LoadRongGiaoDien(data["data"]["LongAp" + i]["rong"]["nameobject"].AsString + "2", allLongAp.transform.GetChild(i).transform.GetChild(0), 1, true, "RongGiaoDien", new Vector3(30, 30));
                }    
                else
                {
                    objDangNuoi.transform.GetChild(i).gameObject.SetActive(true);
                    maxtimeap[i] = data["data"]["LongAp" + i]["timeap"].AsFloat;
                    timeap[i] = data["data"]["LongAp" + i]["sec"].AsFloat;
                    AllMenu.ins.LoadRongGiaoDien(data["data"]["LongAp" + i]["rong"]["nameobject"].AsString + "1", allLongAp.transform.GetChild(i).transform.GetChild(0), 1, true, "RongGiaoDien", new Vector3(42, 42));
                }
            }
            
        }
   
        for (int j = 0; j < 3; j++)
        {
            if (!data["data"]["LongAp" + j]["lock"].AsBool && !objDangNuoi.transform.GetChild(j).gameObject.activeSelf && !dangap[j])
            {
                btnNuoi.transform.GetChild(0).gameObject.SetActive(true);
                btnNuoi.transform.position = allLongAp.transform.GetChild(j).transform.position;
                break;
            }
        }
        LoadDra();


        GameObject objNangCap = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").gameObject;

        objNangCap.transform.Find("txtlevel").GetComponent<Text>().text = "Cấp " + data["data"]["levelLongAp"].AsString;
        objNangCap.transform.Find("txtExp").GetComponent<Text>().text = data["data"]["expLongAp"].AsString + "/" + data["data"]["maxExpLongAp"].AsString;
        Image imgfill = objNangCap.transform.Find("fill").GetComponent<Image>();
        imgfill.fillAmount = data["data"]["expLongAp"].AsFloat / data["data"]["maxExpLongAp"].AsFloat;


        GameObject objThuocTinh = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").transform.Find("objThuocTinh").gameObject;
        objThuocTinh.transform.Find("txtGiamTime").GetComponent<Text>().text = "Giảm thời gian yêu cầu: <color=yellow>"+ data["data"]["GiamThoiGian"].AsString +"%</color> <color=lime>+("+ data["data"]["GiamThoiGianDotPha"].AsString + "%)</color>";
        objThuocTinh.transform.Find("txtGiamKeo").GetComponent<Text>().text = "Giảm Kẹo yêu cầu:        <color=yellow>" + data["data"]["GiamKeo"].AsString +"%</color> <color=lime>+("+ data["data"]["GiamKeoDotPha"].AsString + "%)</color>";
       
        GameObject objDotPha = panelNangCap.transform.GetChild(0).transform.Find("objDotPha").gameObject;

        objDotPha.transform.Find("txtleveldotpha").GetComponent<Text>().text = "Cấp đột phá " + levelLaMa[data["data"]["levelDotPha"].AsInt];
        objDotPha.transform.GetChild(0).transform.Find("txtdotpha").GetComponent<Text>().text = data["allTxtDotPha"][data["data"]["levelDotPha"].AsInt].AsString;



        GameObject imgChiSoDotPha = panelNangCap.transform.GetChild(0).transform.Find("imgChiSoDotPha").gameObject;
        for (int i = 0; i < 8; i++)
        {
            imgChiSoDotPha.transform.GetChild(i).GetComponent<Text>().text = data["allTxtDotPha"][i].AsString;
        }
        gameObject.SetActive(true);
        LoadHienChiSoDotPha(data["data"]["levelDotPha"].AsInt);

        LoadItemYeuCauDotPha(data["YeuCauDotPha"]["BuiTienYeuCau"].AsInt, data["YeuCauDotPha"]["DaDotPhaYeuCau"].AsInt, data["YeuCauDotPha"]["LevelYeuCau"].AsString);
    }
    public void LoadItemYeuCauDotPha(int yeucauBuiTien,int yeuCauDaDotPha,string levelyeucau)
    {
        GameObject objDotPha = panelNangCap.transform.GetChild(0).transform.Find("objDotPha").gameObject;
        Transform objNangcap = objDotPha.transform.Find("objNangCap");
        GameObject itemall = objNangcap.transform.Find("itemall").gameObject;

        if (Inventory.ins.ListItemThuong.ContainsKey("itemBuiTien"))
        {
            int soluongitemco = int.Parse(Inventory.ins.ListItemThuong["itemBuiTien"].transform.GetChild(0).GetComponent<Text>().text);
            itemall.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = soluongitemco >= yeucauBuiTien ? "<color=lime>" + soluongitemco + "/" + yeucauBuiTien +"</color>" : "<color=red>" + soluongitemco + "/" + yeucauBuiTien + "</color>";
        }
        else itemall.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "<color=red>" + 0 + "/" + yeucauBuiTien + "</color>";
        if (Inventory.ins.ListItemThuong.ContainsKey("itemDaDotPha"))
        {
            int soluongitemco = int.Parse(Inventory.ins.ListItemThuong["itemDaDotPha"].transform.GetChild(0).GetComponent<Text>().text);
            itemall.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = soluongitemco >= yeuCauDaDotPha ? "<color=lime>" + soluongitemco + "/" + yeuCauDaDotPha + "</color>" : "<color=red>" + soluongitemco + "/" + yeuCauDaDotPha + "</color>";
        }
        else itemall.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "<color=red>" + 0 + "/" + yeuCauDaDotPha + "</color>";
        Text txtyeucaulevel = objNangcap.transform.Find("txtyeucaulevel").GetComponent<Text>();
        if (int.Parse(CrGame.ins.txtLevel.text) < int.Parse(levelyeucau))
        {
            txtyeucaulevel.text = "Đạt cấp độ " + levelyeucau;
        }
        else txtyeucaulevel.text = "";
    }    
    private void LoadHienChiSoDotPha(int level)
    {

        GameObject imgChiSoDotPha = panelNangCap.transform.GetChild(0).transform.Find("imgChiSoDotPha").gameObject;
        GameObject imgden = imgChiSoDotPha.transform.Find("imgden").gameObject;
        for (int i = 0; i < imgChiSoDotPha.transform.childCount - 2; i++)
        {
            imgChiSoDotPha.transform.Find(i.ToString()).transform.SetSiblingIndex(0);
        }
        for (int i = 0; i < level; i++)
        {
            imgChiSoDotPha.transform.Find(i.ToString()).transform.SetSiblingIndex(imgden.transform.GetSiblingIndex() + 1);
        }

        GameObject objsao = panelNangCap.transform.GetChild(0).transform.Find("objDotPha").transform.Find("objSao").gameObject;

        for (int i = 7; i >= 8 - level; i--)
        {
            objsao.transform.GetChild(i).GetComponent<Image>().sprite = spriteNgoiSaoSang;
        }
    }
    public void BtnGdDotPha()
    {
        GameObject imgChiSoDotPha = panelNangCap.transform.GetChild(0).transform.Find("imgChiSoDotPha").gameObject;
        imgChiSoDotPha.SetActive(true);
        GameObject objDotPha = panelNangCap.transform.GetChild(0).transform.Find("objDotPha").gameObject;
        objDotPha.SetActive(true);

        GameObject objNangCap = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").gameObject;
        objNangCap.SetActive(false);
    }
    public void BtnGdNangCap()
    {
        GameObject imgChiSoDotPha = panelNangCap.transform.GetChild(0).transform.Find("imgChiSoDotPha").gameObject;
        imgChiSoDotPha.SetActive(false);
        GameObject objDotPha = panelNangCap.transform.GetChild(0).transform.Find("objDotPha").gameObject;
        objDotPha.SetActive(false);

        GameObject objNangCap = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").gameObject;
        objNangCap.SetActive(true);
    }
    string itemChonNangCap = "";
    public void ChonONangCap()
    {
        if (dangnangcap) return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        itemChonNangCap = btnchon.name;
        objChon.SetActive(true);
        objChon.transform.transform.position = btnchon.transform.parent.transform.position;
    }
    bool dangnangcap = false;
    public void BtnNangCap()
    {
        if (dangnangcap) return;
        string itemchon = itemChonNangCap;
        if (itemchon == "")
        {
            CrGame.ins.OnThongBaoNhanh("Vui lòng chọn vật phẩm để nâng cấp!");
            return;
        }
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LongAp";
        datasend["method"] = "NangCap";
        datasend["data"]["ItemChon"] = itemchon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                dangnangcap = true;
                Inventory.ins.AddItem(itemchon, -json["trusoluong"].AsInt);
                LoadItemNangCapDangCo();

                GameObject objNangCap = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").gameObject;
                Text txtlevel = objNangCap.transform.Find("txtlevel").GetComponent<Text>();
                Text txtExp = objNangCap.transform.Find("txtExp").GetComponent<Text>();
                Image imgfill = objNangCap.transform.Find("fill").GetComponent<Image>();





                GameObject objThuocTinh = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").transform.Find("objThuocTinh").gameObject;
                Text txtgiamtime = objThuocTinh.transform.Find("txtGiamTime").GetComponent<Text>();
                Text txtGiamKeo = objThuocTinh.transform.Find("txtGiamKeo").GetComponent<Text>();

                Animator animlongap = panelNangCap.transform.GetChild(0).transform.Find("animLongAp").GetComponent<Animator>();
                Transform transformanim = objThuocTinh.transform.Find("transformanim");
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    int exp = json["expLongAp"].AsInt;
                    int maxexp = json["maxExpLongAp"].AsInt;
                    int count = json["leveltungcap"].Count;
                
                    float speed = count * 2;
                    for (int i = 0; i < count; i++)
                    {
                    //   int levelhientai = json["leveltungcap"][i].AsInt;
                        float phantramexphientai = 0;
                        if (i == 0) phantramexphientai = imgfill.fillAmount * 100;
                        // if (json["leveltungcap"].Count == 1) maxphantramexphientai = Mathf.FloorToInt(json["expLongAp"].AsFloat / json["maxExpLongAp"].AsFloat * 100);
                        //  else maxphantramexphientai = Mathf.FloorToInt(json["exptungcap"][i].AsInt / json["maxexptungcap"][i].AsInt * 100);
                        bool lencap = false;
                        for (float j = phantramexphientai; j < 100; j++)
                        {
                            phantramexphientai += 1;
                            float exptungcap = json["maxexptungcap"][i].AsFloat;
                            float exphientai = phantramexphientai / 100 * exptungcap;
                            txtExp.text = Mathf.FloorToInt(exphientai) + "/" + exptungcap;
                            imgfill.fillAmount = exphientai / exptungcap;

                            if (j == 99)
                            {
                                lencap = true;
                            }
                            if(exphientai >= exp && maxexp == exptungcap)
                            {
                                txtExp.text = exp + "/" + maxexp;
                                Settxt();
                                break;
                            }
                            yield return new WaitForSeconds(0.02f / speed);
                            if(count > 1) speed += 0.01f * speed;
                        }
                      
                        Settxt();
                       void Settxt()
                        {
                            if(lencap)
                            {
                                animlongap.Play("animLongAp");

                                GameObject anim = Instantiate(animApXong, transform.position, Quaternion.identity);
                                anim.transform.SetParent(panelNangCap.transform, false);
                                anim.transform.position = transformanim.transform.position;
                                anim.transform.localScale = new Vector3(2, 2);
                                anim.SetActive(true);

                                Destroy(anim, 3f);
                            }
                            txtlevel.text = "Cấp " + json["leveltungcap"][i].AsString;
                            txtgiamtime.text = "Giảm thời gian yêu cầu: <color=yellow>" + json["giamthoigiantungcap"][i].AsString + "%</color> <color=lime>+(" + json["GiamThoiGianDotPha"].AsString + "%)</color>";
                            txtGiamKeo.text = "Giảm Kẹo yêu cầu:        <color=yellow>" + json["giamkeotungcap"][i].AsString + "%</color> <color=lime>+(" + json["GiamKeoDotPha"].AsString + "%)</color>";
                       
                        
                        }
                   
                    }
                    txtExp.text = exp + "/" + maxexp;
                    dangnangcap = false;
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void BtnDotPha()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = "LongAp";
        datasend["method"] = "DotPha";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject objDotPha = panelNangCap.transform.GetChild(0).transform.Find("objDotPha").gameObject;
                GameObject imgChiSoDotPha = panelNangCap.transform.GetChild(0).transform.Find("imgChiSoDotPha").gameObject;
                objDotPha.transform.Find("txtleveldotpha").GetComponent<Text>().text = "Cấp đột phá " + levelLaMa[json["levelDotPha"].AsInt];
                if (json["levelDotPha"].AsInt < 8) objDotPha.transform.GetChild(0).transform.Find("txtdotpha").GetComponent<Text>().text = imgChiSoDotPha.transform.Find(json["levelDotPha"].AsString).GetComponent<Text>().text;
                else objDotPha.transform.GetChild(0).transform.Find("txtdotpha").GetComponent<Text>().text = "<color=red>Đã đạt tối đa!</color>";
;                LoadHienChiSoDotPha(json["levelDotPha"].AsInt);
                Animator animlongap = panelNangCap.transform.GetChild(0).transform.Find("animLongAp").GetComponent<Animator>();


                animlongap.Play("animLongAp");

                Inventory.ins.AddItem("BuiTien", -json["YeuCauDotPha"]["BuiTienYeuCau"].AsInt);
                Inventory.ins.AddItem("DaDotPha", -json["YeuCauDotPha"]["DaDotPhaYeuCau"].AsInt);

                LoadItemYeuCauDotPha(json["YeuCauDotPhanew"]["BuiTienYeuCau"].AsInt, json["YeuCauDotPhanew"]["DaDotPhaYeuCau"].AsInt, json["YeuCauDotPhanew"]["LevelYeuCau"].AsString);

                GameObject objThuocTinh = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").transform.Find("objThuocTinh").gameObject;
                Text txtgiamtime = objThuocTinh.transform.Find("txtGiamTime").GetComponent<Text>();
                Text txtGiamKeo = objThuocTinh.transform.Find("txtGiamKeo").GetComponent<Text>();

                txtgiamtime.text = "Giảm thời gian yêu cầu: <color=yellow>" + json["GiamThoiGian"].AsString + "%</color> <color=lime>+(" + json["GiamThoiGianDotPha"].AsString + "%)</color>";
                txtGiamKeo.text = "Giảm Kẹo yêu cầu:        <color=yellow>" + json["GiamKeo"].AsString + "%</color> <color=lime>+(" + json["GiamKeoDotPha"].AsString + "%)</color>";
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private GameObject PanelDieuKienMoKhoa;
    public void XemYeuCauMoKhoaLongAp()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        int longapchon = btnchon.transform.parent.transform.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LongAp";
        datasend["method"] = "XemYeuCauMoKhoaLongAp";
        datasend["data"]["LongAp"] = longapchon.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                if(PanelDieuKienMoKhoa == null) PanelDieuKienMoKhoa = transform.Find("PanelDieuKienMoKhoa").gameObject;
                PanelDieuKienMoKhoa.transform.SetParent(CrGame.ins.trencung,false);
              
                Transform obj = PanelDieuKienMoKhoa.transform.GetChild(0);


                for (int i = 0; i < obj.transform.childCount - 1; i++)
                {
                    Transform obji = obj.transform.GetChild(i);
                    Image fill = obji.transform.GetChild(1).GetComponent<Image>();
                    Text txtTienDo = obji.transform.GetChild(2).GetComponent<Text>();
                    Text txtname = obji.transform.GetChild(3).GetComponent<Text>();

                    fill.fillAmount = json["TienDo"][i]["tiendo"].AsFloat / json["TienDo"][i]["maxnhiemvu"].AsFloat;
                    txtTienDo.text = json["TienDo"][i]["tiendo"].AsString +"/"+ json["TienDo"][i]["maxnhiemvu"].AsString;
                    txtname.text = json["TienDo"][i]["namenhiemvu"].AsString;
                }

                PanelDieuKienMoKhoa.SetActive(true);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void ClosePanelDieuKienMoKhoa()
    {
        PanelDieuKienMoKhoa.transform.SetParent(transform, false);
        PanelDieuKienMoKhoa.SetActive(false);
    }    
    public void XemChiSoRong()
    {
        GameObject objitem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string idrong = objitem.transform.parent.name;
        CrGame.ins.ChiSoRong(idrong);
    }

    private void LoadDra()
    {
        GameObject contentRong = transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            Destroy(contentRong.transform.GetChild(i).gameObject);
        }
        GameObject slot = contentRong.transform.GetChild(0).gameObject;
        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;


                    string chuoiPhanDau = nameimg.Substring(0, nameimg.Length - 1); // Tách chuỗi từ đầu đến trước ký tự cuối cùng
                    string chuoiCuoiCung = nameimg.Substring(nameimg.Length - 1, 1); // Tách ký tự cuối cùng


                    if (chuoiCuoiCung == "1") SetRong();


                    void SetRong()
                        {
                            GameObject rong = Instantiate(slot, transform.position, Quaternion.identity);
                            rong.transform.SetParent(contentRong.transform, false);
                            // ite
                            rong.name = itemdra.name;
                            Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                            imgRong.name = chuoiPhanDau;
                            imgRong.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "1"); imgRong.SetNativeSize();
                            rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                            rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                            // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                            rong.SetActive(true);
                       
                        for (int i = 0; i < idronglongap.Length; i++)
                        {
                            if (idronglongap[i] == itemdra.name)
                            {
                                rong.transform.GetChild(0).GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                                break;
                            }
                        }
                    }
                    }
                }
            }

        //if (Inventory.ins.TuiRong.transform.childCount > 1)
        //{
        //    for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
        //    {
        //        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
        //        {
        //            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
        //            int sao = int.Parse(itemdra.txtSao.text);
        //        }
        //    }
        //}
    }
    private string[] idronglongap = new string[] {"","","" };
    private bool[] dangap = new bool[] {false,false,false };
    byte LongApChon = 0;
    Image imgrongvuachon;
    public void ChonRong()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string idrongchon = btnchon.transform.parent.name;
        //if (btnNuoi.transform.GetChild(1).gameObject.activeSelf)// nếu như nút nuôi rồng đang bật
        //{
        //    for (int i = 0; i < length; i++)
        //    {

        //    }
        //    goto toiday;
        //}

        for (int i = 0; i < idronglongap.Length; i++)
        {
            if (idronglongap[i] == idrongchon)
            {
                if (objDangNuoi.transform.GetChild(i).gameObject.activeSelf || AllbtnNhanRong.transform.GetChild(i).gameObject.activeSelf) return;
                if(allLongAp.transform.GetChild(i).transform.GetChild(0).transform.childCount > 0) Destroy(allLongAp.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).gameObject);
                idronglongap[i] = "";
                btnchon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                btnNuoi.transform.GetChild(0).gameObject.SetActive(true);
                btnNuoi.transform.GetChild(1).gameObject.SetActive(false);


                //for (int j = 0; j < objDangNuoi.transform.childCount; j++)S
                //{
                //    if (!objDangNuoi.transform.GetChild(i).gameObject.activeSelf)
                //    {
                //        objDangNuoi.transform.GetChild(i).gameObject.SetActive(true);
                //        btnNuoi.transform.GetChild(1).gameObject.SetActive(false);

                //        if (i < 2)
                //        {
                //            btnNuoi.transform.GetChild(0).gameObject.SetActive(true);
                //            btnNuoi.transform.position = allLongAp.transform.GetChild(i + 1).transform.position;
                //        }
                //        break;
                //    }
                //}

                return;
            }
        }
     //   if (btnNuoi.transform.GetChild(1).gameObject.activeSelf) return;

        //  toiday:
        for (int i = 0; i < idronglongap.Length; i++)
        {
            if (idronglongap[i] == "" || dangap[i] == false)
            {
                if (allLongAp.transform.GetChild(i).transform.Find("khoa") == null)
                {
                    JSONClass datasend = new JSONClass();
                    datasend["class"] = "LongAp";
                    datasend["method"] = "ChonRong";
                    datasend["data"]["nameobject"] = btnchon.name;
                    datasend["data"]["sao"] = btnchon.transform.parent.transform.GetChild(2).GetComponent<Text>().text;
                    NetworkManager.ins.SendServer(datasend.ToString(), Ok);
                    void Ok(JSONNode json)
                    {
                        
                        debug.Log(json.ToString());
                        if (json["status"].AsString == "0")
                        {

                            for (int j = 0; j < 3; j++)
                            {
                                if (allLongAp.transform.transform.GetChild(j).transform.GetChild(0).transform.childCount > 0 && !dangap[j])
                                {
                                    Destroy(allLongAp.transform.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).gameObject);
                                }
                            }

                            if (imgrongvuachon != null)
                            {
                                for (int i = 0; i < idronglongap.Length; i++)
                                {
                                    if (imgrongvuachon.transform.parent.name == idronglongap[i])
                                    {
                                        if (!dangap[i])
                                        {
                                            imgrongvuachon.color = new Color32(255, 255, 255, 255);
                                        }
                                        break;
                                    }
                                }
                        
                            }
                            Transform longap = allLongAp.transform.GetChild(i);
                            AllMenu.ins.LoadRongGiaoDien(btnchon.name + "1", longap.transform.GetChild(0), 1, true, "RongGiaoDien", new Vector3(42, 42));
                            longap.transform.GetChild(0).name = idrongchon;
                            idronglongap[i] = idrongchon;
                            Image img = btnchon.GetComponent<Image>();
                            imgrongvuachon = img;
                            img.color = new Color32(90, 90, 90, 255);
                            btnNuoi.transform.position = longap.transform.position;
                            btnNuoi.transform.GetChild(0).gameObject.SetActive(false);
                            btnNuoi.transform.GetChild(1).gameObject.SetActive(true);
                            LongApChon = (byte)i;

                            int KeoBonBon = int.Parse(transform.GetChild(0).gameObject.transform.Find("ThucAnKeoBonBon").transform.GetChild(1).GetComponent<Text>().text);
                            int KeoKobe = int.Parse(transform.GetChild(0).gameObject.transform.Find("ThucAnKeoSuaBoKobe").transform.GetChild(1).GetComponent<Text>().text);

                            btnNuoi.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = (KeoBonBon >= json["giaKeoBonBon"].AsInt) ? json["giaKeoBonBon"].AsString : "<color=red>"+json["giaKeoBonBon"].AsString+"</color>";
                            btnNuoi.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = (KeoKobe >= json["giaKeoSuaBoKoBe"].AsInt) ? json["giaKeoSuaBoKoBe"].AsString : "<color=red>" + json["giaKeoSuaBoKoBe"].AsString + "</color>";

                          
                        }
                        else
                        {
                            CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                        }

                    }

                    break;
                }
               
            }
        }
    }    
    public void CatRong()
    {
        GameObject longapchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (longapchon.transform.GetChild(0).transform.childCount > 0 && !objDangNuoi.transform.GetChild(longapchon.transform.GetSiblingIndex()).gameObject.activeSelf && !dangap[longapchon.transform.GetSiblingIndex()])
        {
            Destroy(longapchon.transform.GetChild(0).transform.GetChild(0).gameObject);
            imgrongvuachon.color = new Color32(255, 255, 255, 255);

            btnNuoi.transform.GetChild(0).gameObject.SetActive(true);
            btnNuoi.transform.GetChild(1).gameObject.SetActive(false);
       
            for (int i = 0; i < idronglongap.Length; i++)
            {
                if (idronglongap[i] == longapchon.transform.GetChild(0).name)
                {
                    idronglongap[i] = "";
                }
            }
            longapchon.transform.GetChild(0).name = "";

        }
       
    }    
    public void LoadItemDangCo()
    {
        if (Inventory.ins.ListItemThuong.ContainsKey("itemThucAnKeoBonBon"))
        {
            transform.GetChild(0).gameObject.transform.Find("ThucAnKeoBonBon").transform.GetChild(1).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemThucAnKeoBonBon"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else transform.GetChild(0).gameObject.transform.Find("ThucAnKeoBonBon").transform.GetChild(1).GetComponent<Text>().text = "0";
        if (Inventory.ins.ListItemThuong.ContainsKey("itemThucAnKeoSuaBoKobe"))
        {
            transform.GetChild(0).gameObject.transform.Find("ThucAnKeoSuaBoKobe").transform.GetChild(1).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemThucAnKeoSuaBoKobe"].transform.GetChild(0).GetComponent<Text>().text;
        }
        else transform.GetChild(0).gameObject.transform.Find("ThucAnKeoSuaBoKobe").transform.GetChild(1).GetComponent<Text>().text = "0";

    }
    public GameObject panelNangCap;
    public void LoadItemNangCapDangCo()
    {
        string[] allitem = new string[] { "ThuocTienThuong", "ThuocTienHiem", "ThuocTienCucHiem", "ThuocTienEvent" };
        GameObject itemall = panelNangCap.transform.GetChild(0).transform.Find("objNangCap").transform.Find("objNangCap").transform.Find("itemall").gameObject;

        for (int i = 0; i < allitem.Length; i++)
        {
            if (Inventory.ins.ListItemThuong.ContainsKey("item" + allitem[i]))
            {
                itemall.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = Inventory.ins.ListItemThuong["item" + allitem[i]].transform.GetChild(0).GetComponent<Text>().text;
            }
            else itemall.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = "0";
        }
   
    }
    public void OpenMenuNangCap()
    {
        panelNangCap.transform.SetParent(CrGame.ins.trencung.transform, false);
        panelNangCap.gameObject.SetActive(true);
        panelNangCap.transform.SetAsFirstSibling();
    }   
    public void CloseMenuNangCap()
    {
        panelNangCap.transform.SetParent(transform,false);
        panelNangCap.gameObject.SetActive(false);

    }
    public void BtnNuoi()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;

        string itemchon = btnchon.transform.parent.name;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LongAp";
        datasend["method"] = "NuoiRong";
        datasend["data"]["idrong"] = idronglongap[LongApChon];
        datasend["data"]["LongAp"] = LongApChon.ToString();
        datasend["data"]["ItemChon"] = itemchon;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                objDangNuoi.transform.GetChild(LongApChon).gameObject.SetActive(true);
                btnNuoi.transform.GetChild(1).gameObject.SetActive(false);

                for (int j = 0; j < allLongAp.transform.childCount; j++)
                {
                    if (allLongAp.transform.GetChild(j).transform.Find("khoa") == null && !objDangNuoi.transform.GetChild(j).gameObject.activeSelf && !AllbtnNhanRong.transform.GetChild(j).gameObject.activeSelf)
                    {
                        btnNuoi.transform.GetChild(0).gameObject.SetActive(true);
                        btnNuoi.transform.position = allLongAp.transform.GetChild(j).transform.position;
                        break;
                    }
                }
                timeap[LongApChon] = json["timeap"].AsFloat;
                maxtimeap[LongApChon] = json["timeap"].AsFloat;
                dangap[LongApChon] = true;
                Inventory.ins.AddItem(itemchon, -json["truitem"].AsInt);
                LoadItemDangCo();
                if (Inventory.ins.TuiRong.transform.childCount > 1)
                {
                    for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                        {
                            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            if(itemdra.name == idronglongap[LongApChon])
                            {
                                //Destroy(itemdra.gameObject);
                                itemdra.transform.Find("lock").gameObject.SetActive(true);
                                break;
                            }    
                        
                        }
                    }
                }
     
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void NhanRong()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int vitrichon = btnchon.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LongAp";
        datasend["method"] = "NhanRong";
        datasend["data"]["vitrichon"] = vitrichon.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject rong = allLongAp.transform.transform.GetChild(vitrichon).transform.GetChild(0).transform.GetChild(0).gameObject;
                SortingGroup group = rong.GetComponent<SortingGroup>();
                group.sortingLayerName = "GiaoDien2";
                QuaBay qua = rong.AddComponent<QuaBay>();
             //   qua.vitribay = GameObject.FindGameObjectWithTag("hopqua");
               // Destroy(allLongAp.transform.transform.GetChild(vitrichon).transform.GetChild(0).gameObject);
                AllbtnNhanRong.transform.GetChild(vitrichon).gameObject.SetActive(false);
                dangap[vitrichon] = false;

                GameObject contentRong = transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                for (int i = 1; i < contentRong.transform.childCount; i++)
                {
                    if (contentRong.transform.GetChild(i).gameObject.name == idronglongap[vitrichon])
                    {
                        Destroy(contentRong.transform.GetChild(i).gameObject);
                        break;
                    }   
                }


                idronglongap[vitrichon] = "";
                if (!btnNuoi.transform.GetChild(1).gameObject.activeSelf)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (allLongAp.transform.GetChild(j).Find("khoa") == null && !objDangNuoi.transform.GetChild(j).gameObject.activeSelf && !dangap[j])
                        {
                            btnNuoi.transform.GetChild(0).gameObject.SetActive(true);
                            btnNuoi.transform.position = allLongAp.transform.GetChild(j).transform.position;
                            break;
                        }
                    }
                }


                if (Inventory.ins.TuiRong.transform.childCount > 1)
                {
                    for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                        {
                            ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            if(itemdra.name == json["rong"]["id"].AsString)
                            {
                                itemdra.transform.Find("lock").gameObject.SetActive(false);
                                Image img = itemdra.transform.GetChild(0).GetComponent<Image>();
                                img.sprite = Inventory.LoadSpriteRong(itemdra.nameObjectDragon + "2");
                                itemdra.txtSao.text = (int.Parse(itemdra.txtSao.text) + 1) + "";
                                img.SetNativeSize();
                                break;
                            }    
                        }
                    }
                }

             
                //string id = json["rong"]["id"].AsString;
                //string nameitem = json["rong"]["nameitem"].AsString;
                //byte sao = (byte)json["rong"]["sao"].AsInt;
                //int level = 0; int exp = 0;
                //int maxexp = json["rong"]["maxexp"].AsInt;
                //byte tienhoa = (byte)json["rong"]["tienhoa"].AsInt;
                //float sothucan = json["rong"]["timedoi"].AsFloat;
                //string tenrong = json["rong"]["namerong"].AsString;
                //string nameobject = json["rong"]["nameobject"].AsString;
                //Inventory.ins.AddItemRong(id, nameitem, sao, level, exp, maxexp, tienhoa, sothucan, tenrong, nameobject, json["rong"]["hoangkim"].AsBool, json["rong"]["ngoc"], false);

                //else // xóa rồng không chọn
                //{

                //}

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuLongAp");
    }
 }    
