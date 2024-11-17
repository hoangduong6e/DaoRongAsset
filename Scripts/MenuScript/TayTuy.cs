using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TayTuy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ORong, thuoctinhgoc, thuoctinhmoi,
        contentRong, SlotRong;
     string idrong;
    public Button btnTayTuy, btnNhanThuocTinhMoi,btnHoabui;
    public Sprite spriteKhongkhoa, spriteKhoa;
    List<string> listcsKhoa = new List<string>();
    public Text txtGiaKc, txtGiaBuiRong;
    int sochiso = 0, saorong = 0;
    public GameObject menuTayTuy, menuHoaBui;
    // menuHoaBui
    public Text txtSoBuiRong,txtGiaBan;
    public GameObject ThuocTinhHoaBui,oRongHoaBui;
    public void XacNhanHoaBui()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung")).GetComponent<ThongBaoChon>();

        tbc.txtThongBao.text = "Bạn chắc chắn hóa con rồng này thành<color=#ffff00ff> " + txtGiaBan.text + " bụi rồng?</color> \n Rồng sẽ biến mất sau khi hóa bụi";

        tbc.btnChon.onClick.AddListener(() => { HoaBuiRong(); });
    }
    public void XemCs()
    {
        CrGame.ins.ChiSoRong(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name);
    }
    void HoaBuiRong()
    {
        btnHoabui.interactable = false;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TayTuy";
        datasend["method"] = "HoaBuiRong";
        datasend["data"]["id"] = idrong;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                for (int i = 0; i < contentRong.transform.childCount; i++)
                {
                    if (contentRong.transform.GetChild(i).name == idrong)
                    {
                        Destroy(contentRong.transform.GetChild(i).gameObject);
                        break;
                    }
                }
                for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                {
                    if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                    {
                        if (Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).name == idrong)
                        {
                            Destroy(Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).gameObject);
                            break;
                        }
                    }
                }
                oRongHoaBui.transform.GetChild(0).gameObject.SetActive(false);
                oRongHoaBui.transform.GetChild(1).gameObject.SetActive(true);
                CrGame.ins.OnThongBaoNhanh("Hóa bụi thành công", 1.5f);
                Inventory.ins.AddItem("BuiRong", int.Parse(txtGiaBan.text));
                LoadBuiRong();
                txtGiaBan.text = "0";
                if (AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                if (AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
            btnHoabui.interactable = true;
        }
    }
    public void OpenMenuHoaBui()
    {
        btnHoabui.interactable = false;
        CatRongTayTuy();
        LoadBuiRong();
        menuHoaBui.SetActive(true);
        menuTayTuy.SetActive(false);
        for (int i = 0; i < contentRong.transform.childCount; i++)
        {
            contentRong.transform.GetChild(i).gameObject.SetActive(false);
        }
        if(Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i <  Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if( Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra =  Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    for (int J = 1; J < contentRong.transform.childCount; J++)
                    {
                        if (itemdra.name == contentRong.transform.GetChild(J).name)
                        {
                            contentRong.transform.GetChild(J).gameObject.SetActive(true);
                            break;
                        }
                    }
                }    
            }
        }    
    }
    void LoadBuiRong()
    {
        if ( Inventory.ins.ListItemThuong.ContainsKey("itemBuiRong"))
        {
            txtSoBuiRong.text = "Số bụi rồng đang có: <color=#ffff00ff>" +  Inventory.ins.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text + "</color>";
        }
        else txtSoBuiRong.text = "Số bụi rồng đang có: <color=#ffff00ff>" + 0 + "</color>";
    }
    public void OpenMenuTayTuy()
    {
        CatRongTayTuy();
        menuHoaBui.SetActive(false);
        menuTayTuy.SetActive(true);
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            contentRong.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    void OnEnable()
    {
        menuTayTuy.SetActive(true);menuHoaBui.SetActive(false);
        LoadGia(0, 0);
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TayTuy";
        datasend["method"] = "XemRongTayTuy";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                for (int i = 0; i < json["rongdao"].Count; i++)
                {
                    string[] Dohiemcuarong = json["rongdao"][i]["nameitem"].Value.Split('-');
                    string namerong = json["rongdao"][i]["nameobject"].Value;
                    string id = json["rongdao"][i]["id"].Value;
                    string tienhoa = json["rongdao"][i]["tienhoa"].Value;
                    string sao = json["rongdao"][i]["sao"].Value;
                    AddItem(namerong + tienhoa, id, sao, Inventory.DoHiemCuaRong(Dohiemcuarong[1]));
                }
                //for (int i = 1; i < json["rongtui"].Count; i++)
                //{

                //    string[] Dohiemcuarong = json["rongtui"][i]["nameitem"].Value.Split('-');
                //    string namerong = json["rongtui"][i]["nameobject"].Value;
                //    string id = json["rongtui"][i]["id"].Value;
                //    string tienhoa = json["rongtui"][i]["tienhoa"].Value;
                //    string sao = json["rongtui"][i]["sao"].Value;
                //    AddItem(namerong + tienhoa, id, sao, Inventory.DoHiemCuaRong(Dohiemcuarong[1]));
                //}
                for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
                {
                    if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                    {
                        ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                        string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                        string tienhoa = nameimg.Substring(nameimg.Length - 1);
                        //   debug.Log("Rong trong tui: " + nameimg + " tienhoa " + tienhoa);
                        AddItem(nameimg, itemdra.name, itemdra.txtSao.text, itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text, itemdra.transform.Find("lock").gameObject.activeSelf);
                        //if (tienhoa == "2")
                        //{
                        //    AddItem(nameimg, itemdra.name, itemdra.txtSao.text, itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text);
                        //}
                    }
                }
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
        void AddItem(string namer,string id,string sao,string hiem,bool khoa = false)
        {
            GameObject rong = Instantiate(SlotRong, transform.position, Quaternion.identity);
            rong.transform.SetParent(contentRong.transform, false);
            // ite
            rong.name = id;
            Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
            imgRong.sprite = Inventory.LoadSpriteRong(namer); imgRong.SetNativeSize();
            rong.transform.GetChild(1).GetComponent<Text>().text = hiem;
            rong.transform.GetChild(2).GetComponent<Text>().text = sao;
            if(khoa)
            {
                rong.transform.Find("khoa").gameObject.SetActive(true);
            }
            // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
            rong.SetActive(true);
        }
    }
    public void ChonRong()
    {
        string id = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        idrong = id;
        if (menuTayTuy.activeSelf == true)
        {
            btnNhanThuocTinhMoi.interactable = false;
            for (int i = 0; i < thuoctinhgoc.transform.childCount; i++)
            {
                thuoctinhgoc.transform.GetChild(i).gameObject.SetActive(false);
                thuoctinhgoc.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = spriteKhongkhoa;

            }
            for (int i = 0; i < thuoctinhmoi.transform.childCount; i++)
            {
                thuoctinhmoi.transform.GetChild(i).gameObject.SetActive(false);
                thuoctinhmoi.transform.GetChild(i).transform.GetChild(5).gameObject.SetActive(false);
            }
            JSONClass datasend = new JSONClass();
            datasend["class"] = "TayTuy";
            datasend["method"] = "ChonRongTayTuy";
            datasend["data"]["id"] = id;
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    listcsKhoa = new List<string>();
                    saorong = int.Parse(json["rong"]["sao"].Value);
                    LoadGia(0, saorong * 2);


                    GameObject animRong = ORong.transform.GetChild(0).gameObject;
                    AllMenu.ins.LoadRongGiaoDien(json["rong"]["nameobject"].AsString + json["rong"]["tienhoa"].AsString, animRong.transform);
                    //animRong.runtimeAnimatorController = Inventory.LoadAnimator(json["rong"]["nameobject"].Value);
                    animRong.gameObject.SetActive(true);
                    //animRong.SetInteger("TienHoa", int.Parse(json["rong"]["tienhoa"].Value));
                    CrGame.ins.panelLoadDao.SetActive(false);
                    ORong.transform.GetChild(1).gameObject.SetActive(false);
                    sochiso = json["chisogoc"].Count;
                    for (int i = 0; i < json["chisogoc"].Count; i++)
                    {
                        //debug.Log(json["chisogoc"][i]["name"].Value);
                        for (int j = 0; j < thuoctinhgoc.transform.childCount; j++)
                        {
                            if (json["chisogoc"][i]["name"].Value == thuoctinhgoc.transform.GetChild(j).name)
                            {
                                thuoctinhgoc.transform.GetChild(j).gameObject.SetActive(true);
                                thuoctinhgoc.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = json["chisogoc"][i]["sao"].Value;
                                thuoctinhgoc.transform.GetChild(j).transform.GetChild(5).GetComponent<Text>().text = json["chisogoc"][i]["cong"].Value;
                                break;
                            }
                        }
                    }
                    if (json["chisomoi"].Value != "null")
                    {
                        for (int i = 0; i < json["chisomoi"].Count; i++)
                        {
                            //debug.Log(json["chisogoc"][i]["name"].Value);
                            for (int j = 0; j < thuoctinhgoc.transform.childCount; j++)
                            {
                                if (json["chisomoi"][i]["name"].Value == thuoctinhgoc.transform.GetChild(j).name)
                                {
                                    thuoctinhmoi.transform.GetChild(j).gameObject.SetActive(true);
                                    thuoctinhmoi.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = json["chisomoi"][i]["sao"].Value;
                                    thuoctinhmoi.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = json["chisomoi"][i]["cong"].Value;
                                    if (json["chisomoi"][i]["sao"].Value == "4") thuoctinhmoi.transform.GetChild(j).transform.GetChild(5).gameObject.SetActive(true);
                                    break;
                                }
                            }
                        }
                        btnNhanThuocTinhMoi.interactable = true;
                    }
                    else
                    {
                        for (int j = 0; j < thuoctinhmoi.transform.childCount; j++)
                        {
                            thuoctinhmoi.transform.GetChild(j).gameObject.SetActive(true);
                            thuoctinhmoi.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = "";
                            thuoctinhmoi.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = "";
                        }
                    }
                    btnTayTuy.interactable = true;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                }
            }
        }
        else
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "TayTuy";
            datasend["method"] = "XemHoaBuiRong";
            datasend["data"]["id"] = id;
            datasend["data"]["dao"] = "false";
            NetworkManager.ins.SendServer(datasend.ToString(), Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
                {
                    for (int j = 0; j < ThuocTinhHoaBui.transform.childCount; j++) ThuocTinhHoaBui.transform.GetChild(j).gameObject.SetActive(false);
                    GameObject animRong = oRongHoaBui.transform.GetChild(0).gameObject;
                    AllMenu.ins.LoadRongGiaoDien(json["rong"]["nameobject"].AsString + json["rong"]["tienhoa"].AsString, animRong.transform);
                    //    animRong.runtimeAnimatorController = Inventory.LoadAnimator(json["rong"]["nameobject"].Value);
                    animRong.gameObject.SetActive(true);
                    // animRong.SetInteger("TienHoa", int.Parse(json["rong"]["tienhoa"].Value));
                    oRongHoaBui.transform.GetChild(1).gameObject.SetActive(false);

                    for (int i = 0; i < json["chisogoc"].Count; i++)
                    {
                        //debug.Log(json["chisogoc"][i]["name"].Value);
                        for (int j = 0; j < ThuocTinhHoaBui.transform.childCount; j++)
                        {
                            if (json["chisogoc"][i]["name"].Value == ThuocTinhHoaBui.transform.GetChild(j).name)
                            {
                                ThuocTinhHoaBui.transform.GetChild(j).gameObject.SetActive(true);
                                ThuocTinhHoaBui.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = json["chisogoc"][i]["sao"].Value;
                                ThuocTinhHoaBui.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = json["chisogoc"][i]["cong"].Value;
                                break;
                            }
                        }
                    }
                    string hiem = json["rong"]["nameitem"].Value.Split('-')[1];
                    debug.Log(json["rong"]["sao"].Value);
                    if (hiem == "") txtGiaBan.text = json["rong"]["sao"].Value;
                    else if (hiem == "Hiem") txtGiaBan.text = (int.Parse(json["rong"]["sao"].Value) * 2) + "";
                    else txtGiaBan.text = (int.Parse(json["rong"]["sao"].Value) * 3) + "";
                    btnHoabui.interactable = true;
                }
                else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

            }
        }
    }
    public void Khoa()
    {
        Image imgchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        string chisokhoa = imgchon.transform.parent.name;
        if (imgchon.sprite.name == "khongkhoa")
        {
            if (listcsKhoa.Count < sochiso - 1)
            {
                LoadGia(int.Parse(txtGiaKc.text) + saorong * 2);
                imgchon.sprite = spriteKhoa;
                listcsKhoa.Add(chisokhoa);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh("Không thể khóa");
            }
        }
        else
        {
            imgchon.sprite = spriteKhongkhoa;
            LoadGia(int.Parse(txtGiaKc.text) - saorong * 2);
            listcsKhoa.Remove(chisokhoa);
        }
    }
    void LoadGia(int giakc,int giabuirong = 0)
    {
        if(giakc >= 0) txtGiaKc.text = giakc.ToString();
        if (giabuirong > 0)
        {
            string sobuirong = "0";
            if (Inventory.ins.ListItemThuong.ContainsKey("itemBuiRong")) sobuirong = Inventory.ins.ListItemThuong["itemBuiRong"].transform.GetChild(0).GetComponent<Text>().text;
            txtGiaBuiRong.text = sobuirong + "/" + giabuirong;
        }
    }
    public void Exit()
    {
        //ORong.transform.GetChild(2).gameObject.SetActive(false);//hieuung taytuy
        //for (int i = 1; i < contentRong.transform.childCount; i++)
        //{
        //    Destroy(contentRong.transform.GetChild(i).gameObject);
        //}
        //ORong.transform.GetChild(0).gameObject.SetActive(false);
        //ORong.transform.GetChild(1).gameObject.SetActive(true);
        //for (int i = 0; i < thuoctinhgoc.transform.childCount; i++)
        //{
        //    thuoctinhgoc.transform.GetChild(i).gameObject.SetActive(false);
        //    thuoctinhgoc.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = spriteKhongkhoa;
        //}
        //for (int i = 0; i < thuoctinhmoi.transform.childCount; i++)
        //{
        //    thuoctinhmoi.transform.GetChild(i).gameObject.SetActive(false);
        //    thuoctinhmoi.transform.GetChild(i).transform.GetChild(5).gameObject.SetActive(false);
        //}
        //btnTayTuy.interactable = false;
        //btnNhanThuocTinhMoi.interactable = false;
        //listcsKhoa = new List<string>();
        //gameObject.SetActive(false);
        AllMenu.ins.DestroyMenu("MenuTayTuy");
    }    
    public void CatRongTayTuy()
    {
        if(menuTayTuy.activeSelf)
        {
            LoadGia(0, 0);
            ORong.transform.GetChild(2).gameObject.SetActive(false);//hieuung taytuy
            btnTayTuy.interactable = false;
            btnNhanThuocTinhMoi.interactable = false;
            ORong.transform.GetChild(0).gameObject.SetActive(false);
            ORong.transform.GetChild(1).gameObject.SetActive(true);
            for (int i = 0; i < thuoctinhgoc.transform.childCount; i++)
            {
                thuoctinhgoc.transform.GetChild(i).gameObject.SetActive(false);
                thuoctinhgoc.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = spriteKhongkhoa;
            }
            for (int i = 0; i < thuoctinhmoi.transform.childCount; i++)
            {
                thuoctinhmoi.transform.GetChild(i).gameObject.SetActive(false);
                thuoctinhmoi.transform.GetChild(i).transform.GetChild(5).gameObject.SetActive(false);
            }
            listcsKhoa = new List<string>();
        }
        else
        {
            oRongHoaBui.transform.GetChild(0).gameObject.SetActive(false);
            oRongHoaBui.transform.GetChild(1).gameObject.SetActive(true);
            for (int i = 0; i < ThuocTinhHoaBui.transform.childCount; i++)
            {
                ThuocTinhHoaBui.transform.GetChild(i).gameObject.SetActive(false);
            }
            btnHoabui.interactable = false;
        }    
    }
    void TayTuyy()
    {
        btnTayTuy.interactable = false;
        btnNhanThuocTinhMoi.interactable = false;
        string khoa = "";
        for (int i = 0; i < listcsKhoa.Count; i++)
        {
            if (i == 0) khoa = listcsKhoa[0];
            else khoa += "-" + listcsKhoa[i];
        }
        if (khoa == "") khoa = "null";
        for (int i = 0; i < thuoctinhmoi.transform.childCount; i++)
        {
            thuoctinhmoi.transform.GetChild(i).transform.GetChild(5).gameObject.SetActive(false);
        }

        JSONClass datasend = new JSONClass();
        datasend["class"] = "TayTuy";
        datasend["method"] = "TayTuy";
        datasend["data"]["id"] = idrong;
        datasend["data"]["khoa"] = khoa;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    ORong.transform.GetChild(2).gameObject.SetActive(true);//hieuung taytuy
                    yield return new WaitForSeconds(2);
                    ORong.transform.GetChild(2).gameObject.SetActive(false);//hieuung taytuy
                    for (int i = 0; i < thuoctinhmoi.transform.childCount; i++)
                    {
                        thuoctinhmoi.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    for (int i = 0; i < json.Count; i++)
                    {
                        //debug.Log(json["chisogoc"][i]["name"].Value);
                        for (int j = 0; j < thuoctinhmoi.transform.childCount; j++)
                        {
                            if (json[i]["name"].Value == thuoctinhmoi.transform.GetChild(j).name)
                            {
                                thuoctinhmoi.transform.GetChild(j).gameObject.SetActive(true);
                                thuoctinhmoi.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = json[i]["sao"].Value;
                                thuoctinhmoi.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = json[i]["cong"].Value;
                                if (json[i]["sao"].Value == "4") thuoctinhmoi.transform.GetChild(j).transform.GetChild(5).gameObject.SetActive(true);
                                else thuoctinhmoi.transform.GetChild(j).transform.GetChild(5).gameObject.SetActive(false);
                                break;
                            }
                        }
                    }
                    btnTayTuy.interactable = true;
                    btnNhanThuocTinhMoi.interactable = true;
                    LoadGia(-1, saorong * 2);
                }
               
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
                btnTayTuy.interactable = true;
                btnNhanThuocTinhMoi.interactable = true;
                LoadGia(-1, saorong * 2);
            }
           
        }
    }
    public void BtnTayTuy()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung")).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.AddListener(TayTuyy);
        tbc.txtThongBao.text = "Bạn cần <color=#00ff00ff>" + txtGiaBuiRong.text.Split('/')[1] + " bụi rồng</color> và <color=#ff00ffff>" + txtGiaKc.text +" kim cương </color> để tẩy thuộc tính mới";
    }
    public void NhanThuocTinhMoi()
    {
        btnTayTuy.interactable = false;
        btnNhanThuocTinhMoi.interactable = false;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TayTuy";
        datasend["method"] = "NhanThuocTinhMoi";
        datasend["data"]["id"] = idrong;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                JSONNode json = jsonn["data"];
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    ORong.transform.GetChild(2).gameObject.SetActive(true);//hieuung taytuy
                    yield return new WaitForSeconds(2);
                    ORong.transform.GetChild(2).gameObject.SetActive(false);//hieuung taytuy

                    for (int j = 0; j < thuoctinhmoi.transform.childCount; j++)
                    {
                        thuoctinhmoi.transform.GetChild(j).gameObject.SetActive(true);
                        thuoctinhmoi.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = "";
                        thuoctinhmoi.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = "";
                        thuoctinhmoi.transform.GetChild(j).transform.GetChild(5).gameObject.SetActive(false);
                    }

                    for (int i = 0; i < json.Count; i++)
                    {
                        //debug.Log(json["chisogoc"][i]["name"].Value);
                        for (int j = 0; j < thuoctinhgoc.transform.childCount; j++)
                        {
                            if (json[i]["name"].Value == thuoctinhgoc.transform.GetChild(j).name)
                            {
                                thuoctinhgoc.transform.GetChild(j).gameObject.SetActive(true);
                                thuoctinhgoc.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = json[i]["sao"].Value;
                                thuoctinhgoc.transform.GetChild(j).transform.GetChild(5).GetComponent<Text>().text = json[i]["cong"].Value;
                                break;
                            }
                        }
                    }
                }
              
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }
            btnTayTuy.interactable = true;
            btnNhanThuocTinhMoi.interactable = false;
        }
    }
    public void BtnTangSao()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung")).GetComponent<ThongBaoChon>();
 
        tbc.txtThongBao.text = "Bạn cần " + (saorong * 5) + " kim cương để chúc phúc cho thuộc tính này";
 
        string namechiso = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        tbc.btnChon.onClick.AddListener(() => { TangSao(namechiso); });
    }
    void TangSao(string namechiso)
    {
        for (int j = 0; j < thuoctinhmoi.transform.childCount; j++)
        {
            thuoctinhmoi.transform.GetChild(j).transform.GetChild(5).gameObject.SetActive(false);
        }
        btnTayTuy.interactable = false;
        btnNhanThuocTinhMoi.interactable = false;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "TayTuy";
        datasend["method"] = "TangSaoTayTuy";
        datasend["data"]["id"] = idrong;
        datasend["data"]["namechiso"] = namechiso;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode jsonn)
        {
            if (jsonn["status"].AsString == "0")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    JSONNode json = jsonn["data"];
                    ORong.transform.GetChild(2).gameObject.SetActive(true);//hieuung taytuy
                    if (AllMenu.ins.menu.ContainsKey("MenuXacNhan")) AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                    yield return new WaitForSeconds(2);
                    ORong.transform.GetChild(2).gameObject.SetActive(false);//hieuung taytuy
                    for (int i = 0; i < thuoctinhmoi.transform.childCount; i++)
                    {
                        thuoctinhmoi.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    for (int i = 0; i < json.Count; i++)
                    {
                        //debug.Log(json["chisogoc"][i]["name"].Value);
                        for (int j = 0; j < thuoctinhmoi.transform.childCount; j++)
                        {
                            if (json[i]["name"].Value == thuoctinhmoi.transform.GetChild(j).name)
                            {
                                thuoctinhmoi.transform.GetChild(j).gameObject.SetActive(true);
                                thuoctinhmoi.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = json[i]["sao"].Value;
                                thuoctinhmoi.transform.GetChild(j).transform.GetChild(4).GetComponent<Text>().text = json[i]["cong"].Value;
                                if (json[i]["sao"].Value == "4") thuoctinhmoi.transform.GetChild(j).transform.GetChild(5).gameObject.SetActive(true);
                                else thuoctinhmoi.transform.GetChild(j).transform.GetChild(5).gameObject.SetActive(false);
                                break;
                            }
                        }
                    }
                }
              
            }    
            else
            {
                CrGame.ins.OnThongBaoNhanh(jsonn["message"].AsString);
            }

            btnTayTuy.interactable = true;
            btnNhanThuocTinhMoi.interactable = true;
        }
    }
}
