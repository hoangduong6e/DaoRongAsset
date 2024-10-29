using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiaoDienCheTaoLongDen : MonoBehaviour
{
    public void ParseData(JSONNode json)
    {
        debug.Log(json.ToString());
        Transform g = transform.GetChild(0);
        gameObject.SetActive(true);
        Transform trangtrilongden = g.transform.Find("trangtrilongden");
        trangtrilongden.transform.Find("objCheTao").transform.Find("btnCheTao").GetComponent<Button>().interactable = json["yeucau"]["trangtrilongden"]["ok"].AsBool;

        trangtrilongden.transform.Find("objCheTao").transform.Find("txtDaCheTao").GetComponent<Text>().text = json["solongdenchetao"].AsString;

        trangtrilongden.transform.Find("giay").transform.Find("txtsoluong").GetComponent<Text>().text = json["yeucau"]["trangtrilongden"]["giay"].AsString;
        trangtrilongden.transform.Find("daythep").transform.Find("txtsoluong").GetComponent<Text>().text = json["yeucau"]["trangtrilongden"]["daythep"].AsString;
        trangtrilongden.transform.Find("nen").transform.Find("txtsoluong").GetComponent<Text>().text = json["yeucau"]["trangtrilongden"]["nen"].AsString;

        Transform denchocaytrong = g.transform.Find("denchocaytrong");
        denchocaytrong.transform.Find("objCheTao").transform.Find("btnCheTao").GetComponent<Button>().interactable = json["yeucau"]["denchocaytrong"]["ok"].AsBool;

        denchocaytrong.transform.Find("giay").transform.Find("txtsoluong").GetComponent<Text>().text = json["yeucau"]["denchocaytrong"]["giay"].AsString;
        denchocaytrong.transform.Find("daythep").transform.Find("txtsoluong").GetComponent<Text>().text = json["yeucau"]["denchocaytrong"]["daythep"].AsString;
        denchocaytrong.transform.Find("nen").transform.Find("txtsoluong").GetComponent<Text>().text = json["yeucau"]["denchocaytrong"]["nen"].AsString;
        denchocaytrong.transform.Find("HoaTuyet").transform.Find("txtsoluong").GetComponent<Text>().text = json["yeucau"]["denchocaytrong"]["HoaTuyet"].AsString;
    }
    public void CheTaoLongDen(string s)
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "CheTaoLongDen";
        datasend["data"]["chetao"] = s;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
              //  Transform g = transform.GetChild(0);
                GameObject menudangchetao = transform.GetChild(1).gameObject;
                Image fill = menudangchetao.transform.Find("fill").GetComponent<Image>();
                Text txtchetao = menudangchetao.transform.Find("txtchetao").GetComponent<Text>();
                menudangchetao.SetActive(true);
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    int Randomfull = Random.Range(30, 100);
                    for (int i = 0; i < Randomfull; i++)
                    {
                        fill.fillAmount = (float)i / (float)100;
                        yield return new WaitForSeconds(Random.Range(0.0001f, 0.02f));
                    }
                    yield return new WaitForSeconds(0.02f);

                    menudangchetao.SetActive(false);
                    ParseData(json);
                    CrGame.ins.OnThongBaoNhanh(json["info"].AsString);
                    EventManager.ins.GetComponent<MenuEventVuonHoaThangTu>().SetLongDen(json["longden"].AsString);
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
            }
        }
    }
    short soluongMuaQueThu = 1;
    string nameitemmua = "";
    public void OpenMenuMuaQueThu()
    {
        nameitemmua = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        debug.Log("name itemmm " + UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name);
        GameObject menu = EventManager.ins.GetCreateMenu("MenuMuaItemCheTao", CrGame.ins.trencung.transform, true, transform.GetSiblingIndex() + 1);
        XemGiaMuaQueThu();
        Transform g = menu.transform.GetChild(0);
        g.transform.Find("txttanggia").GetComponent<Text>().text = "";
        Transform btn = g.transform.Find("btn");
        g.transform.GetChild(1).GetComponent<Text>().text = "Vật phẩm chế tạo";// tên giao diện
        Image imgitem = g.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        imgitem.sprite = EventManager.ins.GetSprite(nameitemmua);
        imgitem.SetNativeSize();
        Button btnsangtrai = btn.transform.GetChild(0).GetComponent<Button>();
        btnsangtrai.onClick.AddListener(delegate { CongThemSoLuongMua(-1); });
        Button btnsangPhai = btn.transform.GetChild(1).GetComponent<Button>();
        btnsangPhai.onClick.AddListener(delegate { CongThemSoLuongMua(1); });
        Button btnExit = g.transform.Find("btnExit").GetComponent<Button>();
        btnExit.onClick.AddListener(ExitMenuQueThu);
        Button btnXacNhan = g.transform.Find("btnXacNhan").GetComponent<Button>();
        btnXacNhan.onClick.AddListener(MuaQueThu);
        InputField input = g.transform.Find("InputField").GetComponent<InputField>();
        input.onEndEdit.AddListener(onEndEdit);
    }
    private void CongThemSoLuongMua(int i)
    {
        debug.Log("Tang so luong " + i);
        if (soluongMuaQueThu + i >= 1)
        {
            GameObject menu = EventManager.ins.menuevent["MenuMuaItemCheTao"];
            Transform g = menu.transform.GetChild(0);
            InputField input = g.transform.Find("InputField").GetComponent<InputField>();
            soluongMuaQueThu += (short)i;
            XemGiaMuaQueThu();
            input.text = soluongMuaQueThu.ToString();
        }
    }
    private void onEndEdit(string s)
    {
        if (s == "" || s == "0") s = "1";
        if (s.Length > 4) s = "500";
        if (int.Parse(s) >= 500) s = "500";
        debug.Log("onEndEdit " + s);
        GameObject menu = EventManager.ins.menuevent["MenuMuaItemCheTao"];
        Transform g = menu.transform.GetChild(0);
        InputField input = g.transform.Find("InputField").GetComponent<InputField>();
        soluongMuaQueThu = short.Parse(s);
        XemGiaMuaQueThu();
        input.text = soluongMuaQueThu.ToString();
    }

    public void XemGiaMuaQueThu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "XemGiaMuaItemCheTao";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaItemCheTao"))
                {
                    GameObject menu = EventManager.ins.menuevent["MenuMuaItemCheTao"];
                    Transform g = menu.transform.GetChild(0);
                    Text txtgia = g.transform.Find("txtGia").GetComponent<Text>();
                    txtgia.text = json["gia"].AsString;
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void MuaQueThu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "MuaItemCheTao";
        datasend["data"]["soluong"] = soluongMuaQueThu.ToString();
        datasend["data"]["nameitem"] = nameitemmua;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                if (EventManager.ins.menuevent.ContainsKey("MenuMuaItemCheTao"))
                {
                    ExitMenuQueThu();
                    ParseData(json);
                    //SetsucXac(json["sucxac"].AsString);
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    private void ExitMenuQueThu()
    {   
        EventManager.ins.DestroyMenu("MenuMuaItemCheTao"); soluongMuaQueThu = 1;
    }
    public void Exit()
    {
        EventManager.ins.DestroyMenu("MenuCheTaoLongDen");
    }    
}
