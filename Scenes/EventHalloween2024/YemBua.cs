
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public partial class MenuEventHalloween2024 : EventManager
{
    [Header("----Giao diện yểm bùa-----")]
    [SerializeField] private Transform[] allTxtAnim;

    private bool DuocYemBua = true;
    [Header("----Boss Halloween-----")]

    [SerializeField] private Transform PanelXemDanhBoss;

    public bool isKichHoatGiamSucManh;
    public void ExitYemBua()
    {
        if (!DuocYemBua) return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        btnchon.gameObject.SetActive(false);
    }
    public void YemBua()
    {
        if (!DuocYemBua) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] =nameEvent;
        datasend["method"] = "YemBua";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {

        
                DuocYemBua = false;
                debug.Log(json.ToString());

                Transform PanelYemBua = transform.Find("PanelYemBua");
                Animator anim = PanelYemBua.transform.GetChild(0).transform.Find("animBua").GetComponent<Animator>();
                anim.Play("anim");
                GameObject alleff = PanelYemBua.transform.GetChild(0).transform.Find("alleff").gameObject;
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.4f);
                    alleff.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.6f);
                    alleff.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.3f);
                    for (int i = 0; i < 3; i++)
                    {
                        allTxtAnim[i].gameObject.SetActive(true);
                        allTxtAnim[i].transform.GetChild(0).GetComponent<Text>().text = "+" + json["RandomCongBua"][i].AsString;
                    }

                    foreach (KeyValuePair<string, JSONNode> key in json["allitemUpdate"].AsObject)
                    {
                        SetItem(key.Key, key.Value.AsInt);
                    }

                    yield return new WaitForSeconds(0.3f);

                    DuocYemBua = true;

                    yield return new WaitForSeconds(1.3f);

                    for (int i = 0; i < 3; i++)
                    {
                        allTxtAnim[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
      
    }

    public void XemAiBoss()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "XemAiBoss";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                PanelXemDanhBoss.gameObject.SetActive(true);
                Transform g = PanelXemDanhBoss.transform.GetChild(0);
                Transform all = g.transform.Find("all");
                for (int i = 0; i < 3; i++)
                {
                    Image imgsao = all.transform.GetChild(i).GetComponent<Image>();
                    all.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = json["allDieuKienSao"][i].AsString;
                    Image imgQua = imgsao.transform.Find("qua").GetComponent<Image>();
                    Text txtsoluong = imgQua.transform.GetChild(0).GetComponent<Text>();
                    txtsoluong.text = (json["allQua"][i]["loaiitem"].AsString != "rong") ? json["allQua"][i]["soluong"].AsString: json["allQua"][i]["sao"].AsString + " sao";
                    imgQua.sprite = GetSpriteAll(json["allQua"][i]["name"].AsString, (LoaiItem)Enum.Parse(typeof(LoaiItem), json["allQua"][i]["loaiitem"].AsString, true));
                    imgQua.SetNativeSize();
                    GamIns.ResizeItem(imgQua,100);
                    Button btnNhan = imgsao.transform.GetChild(3).GetComponent<Button>();


                    btnNhan.interactable = (json["allQuaAi"][i].AsString == "2")?true:false;
                    Text txt = btnNhan.transform.GetChild(0).GetComponent<Text>();
                    if (json["allQuaAi"][i].AsString == "1")
                    {
                        btnNhan.interactable = false;
                        txt.text = "Nhận";
                    }
                    else if (json["allQuaAi"][i].AsString == "2")
                    {
                        btnNhan.interactable = true;
                        txt.text = "Nhận";
                    }
                    else if (json["allQuaAi"][i].AsString == "3")
                    {
                        btnNhan.interactable = false;
                        txt.text = "<color=cyan>Đã nhận</color>";
                    }
                    if (json["allQuaAi"][i].AsInt >= 2)
                    {
                        imgsao.sprite = MenuEventHalloween2024.inss.GetSprite("ngoisao2");
                    }
                    else
                    {
                        imgsao.sprite = MenuEventHalloween2024.inss.GetSprite("ngoisao1");
                    }
                }
                g.transform.Find("panelBonus").transform.GetChild(0).GetComponent<Text>().text = json["BonusKhiChienDau"].AsString;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }

    public void ThamChien()
    {
        VienChinh.vienchinh.nameMapvao = "BossHalloween";
        LoadMap();
    }

    void LoadMap()
    {

        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "DanhBossHalloween";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                PanelXemDanhBoss.gameObject.SetActive(false);
                debug.Log(json.ToString());
                VienChinh.vienchinh.chedodau = CheDoDau.Halloween;
                NetworkManager.ins.socket.Emit("DoiHinhDanh", JSONObject.CreateStringObject(VienChinh.vienchinh.nameMapvao + "/" + VienChinh.vienchinh.chedodau.ToString()));
              
                VienChinh.vienchinh.enabled = true;
                //   AllMenu.ins.DestroyMenu("MenuXacNhan");
                VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanDem");
                gameObject.SetActive(false);

                isKichHoatGiamSucManh = json["isKichHoatGiamSucManh"].AsBool;

                PanelXemDanhBoss.gameObject.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void NhanQuaAi()
    {
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        int quachon = btnchon.transform.parent.transform.GetSiblingIndex();
        JSONClass datasend = new JSONClass();
        datasend["class"] = nameEvent;
        datasend["method"] = "NhanQuaAiBoss";
        datasend["data"]["quachon"] = quachon.ToString();
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                Image imgQuaNhan = btnchon.transform.parent.transform.Find("qua").GetComponent<Image>();
                Button btnnhan = btnchon.transform.parent.transform.Find("btnnhan").GetComponent<Button>();
                btnnhan.interactable = false;
                btnnhan.transform.GetChild(0).GetComponent<Text>().text = "<color=cyan>Đã nhận</color>";
                GameObject quabay = Instantiate(imgQuaNhan.gameObject,transform.position,Quaternion.identity);
                quabay.transform.GetChild(0).gameObject.SetActive(false);
                quabay.transform.SetParent(transform,false);
                quabay.transform.position = imgQuaNhan.transform.position;
                QuaBay bay = quabay.AddComponent<QuaBay>();
                bay.vitribay = btnHopQua;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

    }
    public void SuaDoiHinh()
    {
        AllMenu.ins.GetCreateMenu("MenuDoiHinh", CrGame.ins.trencung.gameObject, true);
    }

}
