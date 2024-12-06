
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
    public void ExitYemBua()
    {
     //   if (!DuocYemBua) return;
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
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
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
                    Image imgQua = imgsao.transform.Find("qua").GetComponent<Image>();
                    Text txtsoluong = imgQua.transform.GetChild(0).GetComponent<Text>();
                    txtsoluong.text = (json["allQua"][i]["loaiitem"].AsString != "rong") ? json["allQua"][i]["soluong"].AsString: json["allQua"][i]["sao"].AsString + " sao";
                    imgQua.sprite = GetSpriteAll(json["allQua"][i]["name"].AsString, (LoaiItem)Enum.Parse(typeof(LoaiItem), json["allQua"][i]["loaiitem"].AsString, true));
                    imgQua.SetNativeSize();
                    GamIns.ResizeItem(imgQua,100);
                }
                g.transform.Find("panelBonus").transform.GetChild(0).GetComponent<Text>().text = json["BonusKhiChienDau"].AsString;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
    public void VaoMapDanh()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = EventManager.ins.nameEvent;
        datasend["method"] = "DanhBossHalloween";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                VienChinh.vienchinh.GetDoiHinh("BossXucTu", CheDoDau.XucTu);

                NetworkManager.ins.vienchinh.TruDo.SetActive(true);

                NetworkManager.ins.vienchinh.TruXanh.SetActive(true);

                //TaoXucTu(json["NameBoss"].AsString, json["dame"].AsFloat);

                float chia = json["hp"].AsFloat / 3;

                float[] hplansu = new float[] { chia, chia, chia };

                //SetHpLanSu(hplansu);
                // VienChinh.vienchinh.SetBGMap("BGLanSu");
                VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetSprite("BGBachTuoc");
                btnHopQua.transform.SetParent(NetworkManager.ins.loidai.GiaoDien.transform);
                //Destroy(objtrencung.gameObject);
                AllMenu.ins.DestroyMenu(nameof(EventDaiChienThuyQuai));
                //  gameObject.SetActive(false);
                //  VeNha();
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
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());

                NetworkManager.ins.socket.Emit("DoiHinhDanh", JSONObject.CreateStringObject(VienChinh.vienchinh.nameMapvao));
                VienChinh.vienchinh.chedodau = CheDoDau.Halloween;
                VienChinh.vienchinh.enabled = true;
                //   AllMenu.ins.DestroyMenu("MenuXacNhan");
                VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanDem");
                gameObject.SetActive(false);
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
