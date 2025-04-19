using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PVPManager : MonoBehaviour
{
     public static Dictionary<string, Dictionary<string, Transform>> DragonsTF = new()
     {
        { "b", new Dictionary<string, Transform>() },//teamxanh
        { "r", new Dictionary<string, Transform>() }//teamdo
     };

    public static Dictionary<string, float> XTeam = new()
     {
        { "b", PVEManager.XTruTeamXanh },//teamxanh
        { "r",  PVEManager.XTruTeamDo }//teamdo
     };

    public static Dictionary<string, TruVienChinh> TruTeam = new()
     {
        { "b", VienChinh.vienchinh.TruXanh.GetComponent<TruVienChinh>() },//teamxanh
        { "r",   VienChinh.vienchinh.TruDo.GetComponent<TruVienChinh>() }//teamdo
     };

    public static void AddDragonTF(string team, string id, Transform tf)
     {
        if(VienChinh.vienchinh.Teamthis != Team.TeamXanh) team = team == "b" ? "r" : "b";
        DragonsTF[team].Add(id,tf);
     }

    public static void UpdateTick(SocketIOEvent e)
    {
        debug.Log("UpdateTick: " + e.name + " " + e.data);
        foreach (string team in e.data["p"].keys)
        {
            foreach (string id in e.data["p"][team].keys)
            {
                Transform tf = DragonsTF[team][id];
                float x = VienChinh.vienchinh.Teamthis == Team.TeamXanh ? XTeam[team] + float.Parse(e.data["p"][team][id].ToString()) : XTeam[team] - float.Parse(e.data["p"][team][id].ToString());
                tf.position = new Vector3(x, tf.transform.position.y, tf.transform.position.z);
            }
        }
    }
    public class HandleSocket
    {
        private static Dictionary<string, Action<JSONObject>> eventhandle = new Dictionary<string, Action<JSONObject>>
        {
            {"anim",Anim},
            {"hpg",HpGiap},
            {"hp",Hp},
            {"d",Destroyy},
            {"c",HieuUngChu},
            {"pos",Pos},
            {"lc",LamCham},
            {"lst",ListRong },
            {"hptru",HpTru },
            {"kq",KetQua },
            //{"xanhtrieuhoi",XanhTrieuHoi},
            //{"dotrieuhoi",DoTrieuHoi},
        };
        public static void ParseData(JSONObject e)
        {
            foreach (var key in e.keys)
            {
                if (eventhandle.ContainsKey(key))
                {
                    Debug.Log($"[Socket] Key: {key}, Data: {e[key]}");
                    eventhandle[key](e[key]);
                }
                else
                {
                    Debug.LogWarning($"[Socket] Không có event handle cho key: {key}");
                }
            }
        }

        public static void Anim(JSONObject data)
        {
            Transform tf = DragonsTF[data["team"].str][data["id"].str];

            Animator anim = tf.GetComponent<Animator>();
            anim.Play(data["anim"].str);
        }
        public static void Hp(JSONObject data)
        {
            Transform tf = DragonsTF[data["team"].str][data["id"].str];
            DragonPVEController dra = tf.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            dra.ImgHp.fillAmount = float.Parse(data["fill"].ToString());
            //dra.SetHp(float.Parse(data["fill"].ToString()));
            dra.HienThanhHp();
        }
        public static void HpGiap(JSONObject data)
        {
            Transform tf = DragonsTF[data["team"].str][data["id"].str];
            DragonPVEController dra = tf.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            dra.ImgHp.transform.GetChild(1).GetComponent<Image>().fillAmount = float.Parse(data["fill"].ToString());
            //dra.SetHp(float.Parse(data["fill"].ToString()));
            
            dra.HienThanhHp();
        }
        public static void Destroyy(JSONObject data)
        {
            Transform tf = DragonsTF[data["team"].str][data["id"].str];
            Destroy(tf.gameObject);
            DragonsTF[data["team"].str].Remove(data["id"].str);
        }
        public static void HieuUngChu(JSONObject data)
        {
            Transform tf = DragonsTF[data["team"].str][data["id"].str];
            PVEManager.InstantiateHieuUngChu(data["name"].str, tf.transform.Find("SkillDra"));
        }
        public static void Pos(JSONObject data)
        {
            string team = data["team"].str;
            string id = data["id"].str;
            Transform tf = DragonsTF[team][data["id"].str];
            float x = VienChinh.vienchinh.Teamthis == Team.TeamXanh ? XTeam[team] + float.Parse(data[team][id].ToString()) : XTeam[team] - float.Parse(data[team][id].ToString());
            tf.position = new Vector3(x, tf.transform.position.y, tf.transform.position.z);
        }
        public static void HpTru(JSONObject data)
        {
            TruVienChinh tru = TruTeam[data["team"].str];
            tru.MauTru.sprite = tru.spriteMau[int.Parse(data["allmau"].ToString())];

            float fillamount = float.Parse(data["fill"].ToString());
            tru.MauTru.fillAmount = fillamount;
        }
        public static void LamCham(JSONObject data)
        {
            Transform tf = DragonsTF[data["team"].str][data["id"].str];
            DragonPVEController dra = tf.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            dra.LamChamABS(new dataLamCham(float.Parse(data["time"].str), data["eff"].str, float.Parse(data["chia"].str), data["cong"].str, false, false, bool.Parse(data["setSpeedRun"].ToString()), bool.Parse(data["setSpeedAnim"].ToString())));
        }
        public static void ListRong(JSONObject data)
        {
            if (data["bc"])//bien cuu
            {
                for (int i = 0; i < data["all"].Count; i++)
                {
                    Transform tf = DragonsTF[data["team"].str][data["all"][i].str];
                    DragonPVEController dra = tf.transform.Find("SkillDra").GetComponent<DragonPVEController>();
                    dra.BienCuuABS(float.Parse(data["time"].str));
                }
            }
            else if (data["ph"])//pos và hp
            {
                //debug.Log("đầy lùi trừ máu");
                for (int i = 0; i < data["all"].Count; i++)
                {
                    string id = data["all"][i]["id"].str;
                    string team = data["team"].str;
                    float fill = float.Parse(data["all"][i]["fill"].ToString());
                  //  debug.Log("id là: " + id + " team là: " + team + " fill là:" + fill);
                    // debug.Log("fill là pos: " + float.Parse(data["all"][i]["pos"].ToString()));
                  //  debug.Log("name là: " + DragonsTF[team][id].name);
                    Transform tf = DragonsTF[team][id];
                    DragonPVEController dra = tf.transform.Find("SkillDra").GetComponent<DragonPVEController>();
                   // debug.Log("fill là: " + dra.ImgHp.fillAmount);
                    float x = VienChinh.vienchinh.Teamthis == Team.TeamXanh ? XTeam[team] + float.Parse(data["all"][i]["pos"].ToString()) : XTeam[team] - float.Parse(data["all"][i]["pos"].ToString());
                    tf.position = new Vector3(x, tf.transform.position.y, tf.transform.position.z);
            

                    dra.ImgHp.fillAmount = fill;
                  
                    dra.HienThanhHp();

                }
            }
        }
        public static void KetQua(JSONObject data)
        {
            foreach (KeyValuePair<string, Transform> i in DragonsTF[data["teamwin"].str])
            {
                DragonPVEController dra = i.Value.transform.Find("SkillDra").GetComponent<DragonPVEController>();
                dra.enabled = false;
                dra.AnimWin();
            }
            if (data["teamwin"].str == "b") Win();
            else Lose();
        }
    }
    public static void PVP(SocketIOEvent e)
    {
        debug.Log("PVP: " + e.name + " " + e.data);
        HandleSocket.ParseData(e.data);
    }
    public static void Win()
    {
        GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thang;
        GiaoDienPVP.ins.spriteWin.SetNativeSize();
        GiaoDienPVP.ins.thongtin.text = "Bạn đã đánh bại đối thủ!";
        CrGame.ins.SetPanelThangCap(true);
        CrGame.ins.txtThangCap.text = "";
        GiaoDienPVP.ins.menuWin.SetActive(true);
        VienChinh.vienchinh.OffThangCap();
        GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
    }
    public static void Lose()
    {
        GiaoDienPVP.ins.menuWin.SetActive(true);
        GiaoDienPVP.ins.spriteWin.sprite = GiaoDienPVP.ins.thua;
       // if (tb != "") GiaoDienPVP.ins.thongtin.text = tb;
        //else
        GiaoDienPVP.ins.thongtin.text = "Bạn đã bị đánh bại";
        GiaoDienPVP.ins.panelBatdau.transform.GetChild(2).GetComponent<Text>().text = "";
        GiaoDienPVP.ins.btnSetting.SetActive(true);
        GiaoDienPVP.ins.spriteWin.SetNativeSize();
    }



}
