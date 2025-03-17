using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public static void PVP(SocketIOEvent e)
    {
        debug.Log("PVP: " + e.name + " " + e.data);
        if (e.data["anim"])
        {
            Transform tf = DragonsTF[e.data["anim"]["team"].str][e.data["anim"]["id"].str];

            Animator anim = tf.GetComponent<Animator>();
            anim.Play(e.data["anim"]["anim"].str);

        }
        else if (e.data["hp"])
        {
            Transform tf = DragonsTF[e.data["hp"]["team"].str][e.data["hp"]["id"].str];
            DragonPVEController dra = tf.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            dra.ImgHp.fillAmount = float.Parse(e.data["hp"]["fill"].ToString());
            dra.HienThanhHp();
        }
        else if (e.data["d"])//destroy
        {
            Transform tf = DragonsTF[e.data["d"]["team"].str][e.data["d"]["id"].str];
            Destroy(tf.gameObject);
            DragonsTF[e.data["d"]["team"].str].Remove(e.data["d"]["id"].str);
        }
        else if (e.data["c"])
        {
            debug.Log("netranhhhh");
            Transform tf = DragonsTF[e.data["c"]["team"].str][e.data["c"]["id"].str];
            PVEManager.InstantiateHieuUngChu(e.data["c"]["name"].str,tf.transform.Find("SkillDra")); 
        }
        else if (e.data["hptru"])
        {
            TruVienChinh tru = TruTeam[e.data["hptru"]["team"].str];
            tru.MauTru.sprite = tru.spriteMau[int.Parse(e.data["hptru"]["allmau"].ToString())];
            tru.MauTru.sprite = tru.spriteMau[int.Parse(e.data["hptru"]["allmau"].ToString())];

            float fillamount = float.Parse(e.data["hptru"]["fill"].ToString());
            tru.MauTru.fillAmount = fillamount;
        }
        else if (e.data["kq"])
        {
            foreach (KeyValuePair<string, Transform> i in DragonsTF[e.data["kq"]["teamwin"].str])
            {
                DragonPVEController dra = i.Value.transform.Find("SkillDra").GetComponent<DragonPVEController>();
                dra.enabled = false;
                dra.AnimWin();
            }
        }
        //else if(e.data["attack"])
        //{
        //     Transform tf = PVPManager.DragonsTF[e.data["attack"]["team"].str][e.data["attack"]["id"].str];
        //    debug.Log(tf.name + " attack");
        //    DragonPVEController dra = tf.transform.Find("SkillDra").GetComponent<DragonPVEController>();
        //    dra.AnimatorAttack();
        //}
    }
}
