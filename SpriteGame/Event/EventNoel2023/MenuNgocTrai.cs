using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNgocTrai : MonoBehaviour
{
    // Start is called before the first frame update
    private MenuEventNoel2023 ev;
    public GameObject quabay,AllNgocTrai;
    public Sprite oxanh;
    public Button btnQuaAi;
    public Sprite sprite1, sprite2, sprite3;
    private string songoctrai;
    private bool boquaxacnhan = false;
    public void ParseData(JSONNode json)
    {
        debug.Log(json.ToString());
        ev = EventManager.ins.GetComponent<MenuEventNoel2023>();
        for (byte i = 0; i < json["GiaoDienNgocTrai"]["OQuaNgocTrai"].Count; i++)
        {
            LoadQuaNgocTrai(i, json["GiaoDienNgocTrai"]["OQuaNgocTrai"][i]);
        }
        Transform g = transform.GetChild(0);
        Text txtaihientai = g.transform.Find("AiHienTai").transform.GetChild(0).GetComponent<Text>();
        txtaihientai.text = "Ải hiên tại\r\n<size=80>"+ json["GiaoDienNgocTrai"]["aihientai"].AsString + "</size>";

        SetNgocTrai(json["NgocTraiVang"].AsString, json["NgocTrai"].AsString);
        LoadMocQua(json["GiaoDienNgocTrai"]["quatichluyNgocTrai"], json["allMocDiem"], json["GiaoDienNgocTrai"]["aihientai"].AsInt);
    }
    private void SetNgocTrai(string ngoctraivang,string ngoctrai)
    {
        Transform g = transform.GetChild(0);
        g.transform.Find("NgocTraiVang").transform.GetChild(1).GetComponent<Text>().text = ngoctraivang;
        g.transform.Find("txtngoctrai").GetComponent<Text>().text = "Bạn đang có " + ngoctrai + " ngọc trai";
        songoctrai = ngoctrai;
    }
    public void ChonNgocTrai()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AudioManager.PlaySound("soundClick");
        if (!boquaxacnhan)
        {
            if (songoctrai == "0")
            {
                EventManager.OpenThongBaoChon("Không đủ Ngọc Trai, xác nhận sẽ tốn 200 Kim Cương\n<size=45>(Chỉ nhắc lần đầu)</size>", delegate { BoQuaXacNhan(btnchon.transform.GetSiblingIndex()); });
                //ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan",CrGame.ins.trencung.gameObject, true, ev.giaodiennut1.transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
                //tbc.btnChon.onClick.RemoveAllListeners();
                //tbc.btnChon.onClick.AddListener(delegate { BoQuaXacNhan(btnchon.transform.GetSiblingIndex()); });
                return;
            }
        }
        MoNgocTrai(btnchon.transform.GetSiblingIndex());


    }
    private void MoNgocTrai(int index)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "ChonNgocTrai";
        datasend["data"]["vitri"] = index.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                GameObject oNgoctrai = AllNgocTrai.transform.GetChild(index).gameObject;
                Animator anim = oNgoctrai.GetComponent<Animator>();
                oNgoctrai.GetComponent<Button>().enabled = false;
                anim.Play("animngoctraimora");
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.4f);
                    LoadQuaNgocTrai((byte)json["vitri"].AsInt, json["QuaRandom"], true);
                    SetNgocTrai(json["NgocTraiVang"].AsString, json["NgocTrai"].AsString);
                }
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].Value);
        }
    }    
    private void BoQuaXacNhan(int index)
    {
        boquaxacnhan = true;
        //AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
        MoNgocTrai(index);
    }
    GameObject nhanngocTraiVang;
    public void LoadQuaNgocTrai(byte vitri, JSONNode qua,bool quabayy = false)
    {
       // debug.Log(qua.ToString());
        if (qua["namequa"].AsString == "chuamo")
        {
            return;
        }
        GameObject oNgoctrai = AllNgocTrai.transform.GetChild(vitri).gameObject;
        oNgoctrai.GetComponent<Animator>().enabled = false;
        GameObject quahienthi = Instantiate(quabay, transform.position, Quaternion.identity);
        quahienthi.transform.SetParent(oNgoctrai.transform,false);
        quahienthi.transform.position = oNgoctrai.transform.position;
        Image imgngoctrai = oNgoctrai.GetComponent<Image>();
        imgngoctrai.sprite = oxanh;
        imgngoctrai.SetNativeSize();
        Image imgqua = quahienthi.GetComponent<Image>();
        if (qua["loaiitem"].AsString == "Item")
        {
            imgqua.sprite = Inventory.LoadSprite(qua["namequa"].AsString);
        }
        else if (qua["loaiitem"].AsString == "ItemEvent")
        {
            //for (int i = 0; i < ev.allitemEvent.Length; i++)
            //{
            //    if (ev.allitemEvent[i].name == qua["namequa"].AsString)
            //    {
            //        imgqua.sprite = ev.allitemEvent[i];
            //    }
            //}
            imgqua.sprite = EventManager.ins.GetSprite(qua["namequa"].AsString);
            if (qua["namequa"].AsString == "NgocTraiVang")
            {
                if(quabayy)
                {
                    nhanngocTraiVang = ev.GetCreateMenu("PanelNhanNgocTraiVang",CrGame.ins.trencung.transform, true,transform.GetSiblingIndex()+1);
                }
                CrGame.ins.StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.8f);
                    btnQuaAi.interactable = true;
                }
          
            }
        }
        oNgoctrai.GetComponent<Button>().enabled = false;
        quahienthi.transform.GetChild(0).GetComponent<Text>().text = qua["soluong"].AsString;
        imgqua.SetNativeSize();

        if (quabayy && qua["loaiitem"].AsString == "Item")
        {
            GameObject quahienthi2 = Instantiate(quahienthi, transform.position, Quaternion.identity);
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(0.3f);
                quahienthi2.transform.SetParent(ev.giaodiennut1.transform.parent.transform,false);
                QuaBay quabay = quahienthi2.AddComponent<QuaBay>();
                quabay.vitribay = ev.btnHopQua;
                quahienthi2.transform.localScale = new Vector3(quahienthi2.transform.localScale.x*3, quahienthi2.transform.localScale.y * 3, quahienthi2.transform.localScale.z);
                quahienthi2.transform.position = quahienthi.transform.position;
                quahienthi2.gameObject.SetActive(true);
            }
        }

        Vector3 vec = quahienthi.transform.localScale;
        quahienthi.transform.localScale = new Vector3(0, 0, 0);
        quahienthi.SetActive(true);
   
        quahienthi.transform.LeanScale(vec,0.3f);

    }    
    public void QuaAi()
    {
      //  StopAllCoroutines();
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "QuaAi";
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString != "0")
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value);
                return;
            }

            for (int i = 0; i < AllNgocTrai.transform.childCount; i++)
            {
                GameObject oNgoctrai = AllNgocTrai.transform.GetChild(i).gameObject;
               
        
                Animator anim = oNgoctrai.GetComponent<Animator>();
                anim.enabled = true;
                anim.Play("animngoctrai");
                oNgoctrai.GetComponent<Button>().enabled = true;
                if(oNgoctrai.transform.childCount > 0)
                {
                    Destroy(oNgoctrai.transform.GetChild(0).gameObject);
                }
            }
            Transform g = transform.GetChild(0);
            Text txtaihientai = g.transform.Find("AiHienTai").transform.GetChild(0).GetComponent<Text>();
            txtaihientai.text = "Ải hiên tại\r\n<size=80>" + json["aihientai"].AsString + "</size>";

            SetNgocTrai(json["NgocTraiVang"].AsString, json["NgocTrai"].AsString);
            LoadMocQua(json["quatichluyNgocTrai"], json["allMocDiem"], json["aihientai"].AsInt);

            btnQuaAi.interactable = false;
        }
    }

    private void LoadMocQua(JSONNode allmocqua, JSONNode diemmocqua, int banhtrungthu)
    {
        GameObject gd1 = transform.GetChild(0).gameObject;
        GameObject ScrollViewallMocQua = gd1.transform.Find("ScrollViewallMocQua").gameObject;
        ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = banhtrungthu.ToString();
        GameObject content = ScrollViewallMocQua.transform.GetChild(1).transform.GetChild(0).gameObject;
        int tru = 0;
        // int trumocqua = diemmocqua[allmocqua.Count - 1].AsInt;
        for (int i = allmocqua.Count - 1; i >= 0; i--)
        {
            int mocqua = diemmocqua[allmocqua.Count - 1 - i].AsInt;
            //if(banhtrungthu >= mocqua)
            //{

            //}
            GameObject qua = content.transform.GetChild(i).gameObject;
            Image fill = qua.transform.GetChild(1).GetComponent<Image>();
            if (banhtrungthu >= mocqua)
            {
                fill.fillAmount = 1;
                tru = banhtrungthu - mocqua;
                //trumocqua -= mocqua;
            }
            else
            {
                if (i < allmocqua.Count - 1)
                {
                    debug.Log("tru " + tru);
                    int trumocqua = mocqua - diemmocqua[allmocqua.Count - 1 - i - 1].AsInt;
                    debug.Log("trumocqua " + trumocqua);
                    fill.fillAmount = (float)tru / (float)trumocqua;
                    tru = 0;
                }
                else
                {
                    fill.fillAmount = (float)banhtrungthu / diemmocqua[0].AsFloat;
                }


            }
            string trangthai = allmocqua[allmocqua.Count - 1 - i].AsString;
            if (trangthai == "chuaduocnhan")
            {
                Text txt = qua.transform.GetChild(5).GetComponent<Text>();
                txt.gameObject.SetActive(true);
                txt.text = mocqua.ToString();
                qua.transform.GetChild(3).GetComponent<Image>().sprite = sprite1;
          //      qua.transform.GetChild(2).GetComponent<Image>().sprite = chuaduocnhan;
            }
            else if (trangthai == "duocnhan")
            {
                qua.transform.GetChild(4).gameObject.SetActive(true);
                qua.transform.GetChild(5).gameObject.SetActive(false);
                qua.transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
               // qua.transform.GetChild(2).GetComponent<Image>().sprite = duocnhan;
                if (!qua.transform.GetChild(4).GetComponent<Button>())
                {
                    Button btn = qua.transform.GetChild(4).gameObject.AddComponent<Button>();
                    btn.onClick.AddListener(NhanQuaTichLuy);
                }
            }
            else if (trangthai == "danhan")
            {
                qua.transform.GetChild(4).gameObject.SetActive(false);
                Text txt = qua.transform.GetChild(5).GetComponent<Text>();
                txt.gameObject.SetActive(true);
                txt.text = "<color=red>Đã nhận</color>";
                qua.transform.GetChild(3).GetComponent<Image>().sprite = sprite2;
            //    qua.transform.GetChild(2).GetComponent<Image>().sprite = danhan;
            }
        }
    }
    public void NhanQuaTichLuy()
    {

    }

    public void ExitNgocTrai()
    {
        if (nhanngocTraiVang != null) Destroy(nhanngocTraiVang);
        ev.DestroyMenu("GiaoDienNgocTrai");
     //   GameObject menuNgocTrai = ev.giaodiennut1.transform.Find("GiaoDienNgocTrai").gameObject;
      //  menuNgocTrai.SetActive(false);
    }
}