using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiaoDienMiniGameTet2024 : MonoBehaviour
{
    // Start is called before the first frame update
    Transform g;
    public Sprite btnToi, btnSang;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ParseData(JSONNode json)
    {
        g = transform.GetChild(0);
        SetXuDoMayMan(json["XuDoMayMan"].AsString);
        SetTxtTichLuy(json["solanThangMiniGame"].AsString);
        LoadAllQua((byte)json["solanThangMiniGame"].AsInt);
        SetVong(json["Vong"].AsString);

    }
    private void SetXuDoMayMan(string s)
    {
        g.transform.Find("txtXuDoCo").GetComponent<Text>().text = s;
    }
    private void SetTxtTichLuy(string solantichluy)
    {
        g.transform.Find("txtTichLuy").GetComponent<Text>().text = "Tích lũy 8 lần thắng nhận quà lớn. Bạn đã tích lũy <color=lime>"+ solantichluy + "/8</color>";
    }
    private void SetVong(string vong)
    {
        g.transform.Find("txtVong").GetComponent<Text>().text =  "Vòng: <color=yellow>"+ vong + "</color>";
    }
    private void LoadAllQua(byte soluotthang)
    {
        GameObject allQua = g.transform.Find("AllQua").gameObject;
        for (int i = 0; i < soluotthang; i++)
        {
            allQua.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            allQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        for (int i = soluotthang; i < allQua.transform.childCount; i++)
        {
            allQua.transform.GetChild(i).GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            allQua.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
    }
    bool duocchonkeobuabao = true;
    public void ChonKeoBuaBao()
    {
        if (!duocchonkeobuabao) return;
        GameObject KeoBuaBao = g.transform.Find("ObjOanTuXi").transform.Find("KeoBuaBao").gameObject;
        transform.GetChild(0).transform.Find("txtKetQua").gameObject.SetActive(false);
        KeoBuaBao.SetActive(false);
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        AudioManager.PlaySound("soundClick");
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true,transform.GetSiblingIndex()+1).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();
        if (g.transform.Find("txtXuDoCo").GetComponent<Text>().text != "0")
        {
            tbc.txtThongBao.text = "Chọn " + btnchon.name + " sẽ tốn 1 Xu đỏ may mắn";
        }
        else tbc.txtThongBao.text = "Bạn không có Xu đỏ may mắn. Chọn " + btnchon.name + " sẽ tốn 200 Kim cương!";

        tbc.btnChon.onClick.AddListener(delegate {XacNhanChonKeoBuaBao(btnchon.transform.parent.gameObject);});
    }
    
    private void XacNhanChonKeoBuaBao(GameObject namechon)
    {
       
        AudioManager.PlaySound("soundClick");
        Transform tf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent;
        int btnnhan = tf.transform.parent.childCount - 1 - tf.transform.GetSiblingIndex();
      
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "ChonKeoBuaBao";
        datasend["data"]["namechon"] = namechon.name;
        duocchonkeobuabao = false;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "ok")
            {
                StartCoroutine(delay());
                IEnumerator delay()
                {
                  
                    GameObject parentnamechon = namechon.transform.parent.gameObject;
                    Image imgkeo = parentnamechon.transform.Find("Keo").GetComponent<Image>();
                    imgkeo.sprite = btnToi;imgkeo.SetNativeSize();
                    Image imgbua = parentnamechon.transform.Find("Bua").GetComponent<Image>();
                    imgbua.sprite = btnToi; imgbua.SetNativeSize();
                    Image imgbao = parentnamechon.transform.Find("Bao").GetComponent<Image>();
                    imgbao.sprite = btnToi; imgbao.SetNativeSize();
                    Image imgnamechon = namechon.GetComponent<Image>();
                    imgnamechon.sprite = btnSang;imgnamechon.SetNativeSize();

                    GameObject KeoBuaBao = g.transform.Find("ObjOanTuXi").transform.Find("KeoBuaBao").gameObject;
                    KeoBuaBao.SetActive(true);
                    Animator anim = KeoBuaBao.transform.GetChild(0).GetComponent<Animator>();
                    anim.Play("Random");
                    yield return new WaitForSeconds(2f);
                    Text txtketqua = transform.GetChild(0).transform.Find("txtKetQua").GetComponent<Text>();
                    if (json["ketqua"].AsString == "Hoa")
                    {
                        anim.Play(json["minhchon"].AsString + "Hoa");
                        txtketqua.text = "<color=yellow>Draw!!</color>";
                        // yield return new WaitForSeconds(0.4f);
                    }
                    else if (json["ketqua"].AsString == "Win")
                    {
                        anim.Play(json["maychon"].AsString);
                        txtketqua.text = "<color=lime>Win!!</color>";
                        
                       // yield return new WaitForSeconds(0.4f);
                    }
                    else if (json["ketqua"].AsString == "Thua")
                    {
                        anim.Play(json["maychon"].AsString);
                        txtketqua.text = "<color=red>Lose!!</color>";
                        // yield return new WaitForSeconds(0.4f);
                    }
                    txtketqua.gameObject.SetActive(true);
                    duocchonkeobuabao = true;
                    LoadAllQua((byte)json["solanThangMiniGame"].AsInt);
                    SetVong(json["Vong"].AsString);
                    SetTxtTichLuy(json["solanThangMiniGame"].AsString);
                    SetXuDoMayMan(json["XuDoMayMan"].AsString);
                }
    
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].AsString);
                duocchonkeobuabao = true;
            }
        }
    }
    public void OpenMenuNhiemvu()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "EventTrungThu2023";
        datasend["method"] = "GetNhiemVu";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject MenuNhiemVu = EventManager.ins.GetCreateMenu("MenuNhiemVu", transform);
                GameObject AllNhiemVu = MenuNhiemVu.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
                for (int i = 0; i < json["allNhiemvu"].Count; i++)
                {
                    Text txttiendo = AllNhiemVu.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
                    Text txtphanthuong = AllNhiemVu.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>();
                    Text txtnamenv = AllNhiemVu.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
                    if (int.Parse(json["allNhiemvu"][i]["dalam"].Value) >= int.Parse(json["allNhiemvu"][i]["maxnhiemvu"].Value))
                    {
                        txttiendo.text = "<color=#00ff00ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    else
                    {
                        txttiendo.text = "<color=#ff0000ff>" + json["allNhiemvu"][i]["dalam"].Value + "/" + json["allNhiemvu"][i]["maxnhiemvu"].Value + "</color>";
                    }
                    txtphanthuong.text = "x" + json["allNhiemvu"][i]["qua"]["soluong"].AsString;
                    txtnamenv.text = json["allNhiemvu"][i]["namenhiemvu"].AsString;
                }
            }
        }
    }
    public void CloseMenu()
    {
        EventManager.ins.DestroyMenu("GiaoDienMiniGame");
        //gameObject.SetActive(false);
    }    
}
