using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruVienChinh : MonoBehaviour
{
    public float[] Hp = new float[] { 10000, 10000, 10000 }, MaxHp = new float[] { 10000, 10000, 10000 };
    public Image MauTru;
    public Sprite[] spriteMau;
    public byte allmau = 2;Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //  //  debug.Log(collision.name);
    //    if(transform.parent.gameObject.transform.childCount == 1)
    //    {
    //        if (vienchinh.CheckTrigger(collision.name))
    //        {
    //          //  ChiSo cs = collision.transform.parent.GetComponent<ChiSo>();
    //            // MatMau(cs.dame);
    //            MatMau(2000);
    //        }
    //        if (collision.name == "QuaCauLua")
    //        {
    //            collision.gameObject.SetActive(false);
    //           // ChiSo ronglua = collision.transform.parent.gameObject.GetComponent<ChiSo>();
    //            // MatMau(ronglua.dame);
    //            MatMau(2000);
    //        }
    //        if (collision.name == "NgonGio")
    //        {
    //         //   ChiSo rongcay = collision.transform.parent.gameObject.GetComponent<ChiSo>();
    //            // MatMau(rongcay.dame);
    //            MatMau(2000);
    //        }
    //    }
    //}
    public void ReserHp()
    {
        allmau = 2;
        MauTru.transform.parent.gameObject.SetActive(true);
        //for (int i = 0; i < Hp.Length; i++)
        //{
        //    Hp[i] = MaxHp[i];
        //}
        Hp = new float[] { 10000, 10000, 10000 }; MaxHp = new float[] { 10000, 10000, 10000 };
        LoadImgHp();
        VienChinh.vienchinh.TruDo.GetComponent<SpriteRenderer>().enabled = true;
        anim.Play("trudungyen");
    }
    public Action actionwin;
    public void MatMau(float maumat,bool setOnline = false)
    {
        if(VienChinh.vienchinh.chedodau == CheDoDau.Online && !setOnline)
        {
            JSONObject newjson = new JSONObject();
            newjson.AddField("truhptru", maumat.ToString());
            newjson.AddField("team", VienChinh.vienchinh.GetNameDoiThuByTarget(transform));
            DauTruongOnline.ins.AddUpdateData(newjson);
            return;
        }
        Hp[allmau] -= maumat;
        LoadImgHp();
        if (Hp[allmau] <= 0)
        {
            if (allmau>0)
            {
                MauTru.sprite = spriteMau[allmau - 1];
                allmau -= 1;
            }
            else
            {
                if (actionwin != null)
                {
                    debug.Log("action wwinnn");
                    actionwin();
                    actionwin = null;
                }
                anim.Play("die");
                StopAllCoroutines();
                AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(6).gameObject.SetActive(false);
                AllMenu.ins.menu["GiaoDienPVP"].transform.GetChild(7).gameObject.SetActive(false);

                VienChinh.vienchinh.SetAnimWinAllDra();
          
              //  ReplayData.Record = false;
              //  vienchinh.ClearQuai();
                MauTru.transform.parent.gameObject.SetActive(false);
                GiaoDienPVP.ins.TxtTime.gameObject.SetActive(false);
             //   GiaoDienPVP.ins.TxtTime.GetComponent<timePvp>().enabled = true;
                if (gameObject.name == "trudo")
                {
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        yield return new WaitForSeconds(3.5f);
                        VienChinh.vienchinh.Thang();
                    }
                }
                else
                {
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        yield return new WaitForSeconds(3.5f);
                        VienChinh.vienchinh.Thua();
                    }
                }
            }
        }
    }
    public void LoadImgHp()
    {
        float fillamount = (float)Hp[allmau] / (float)MaxHp[allmau];
        MauTru.fillAmount = fillamount;
    }
}
