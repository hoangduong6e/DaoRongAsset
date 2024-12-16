
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class RongRuaAttack : DragonPVEController
{
    //public override void AbsMoveComponent(SocketIOEvent e)
    //{
    //    Transform tfMove = transform.parent.transform;
    //    tfMove.gameObject.AddComponent<RongXuongAttack>().ParseData(e);
    //    debug.Log("move component");
    //    //PVEManager.GetUpdateMove(tfMove, tfMove.transform.parent.name);
    //    Destroy(gameObject.GetComponent<RongXuongAttack>());
    //}
    //private void Start()
    //{
    //    Startt();
    //}
    // Update is called once per frame
    private byte lanmax = 5;// độ lan max số lượng rồng
    private float tamlan = 0;
    protected override void ABSAwake()
    {

    }
    public override void Choang(float giay = 0.4F, bool setOnline = false)
    {
        // base.Choang(giay);
    }
    public override void AbsStart()
    {
        if(saorong <= 15)
        {
            tamlan = 4;
            tamdanhxa = 12;
        }
        else if (saorong <= 20)
        {
            tamlan = 4.2f;
            tamdanhxa = 13;
        }
        else if (saorong <= 25)
        {
            tamlan = 4.5f;
            lanmax = 6;
            tamdanhxa = 14;
        }
        else if (saorong <= 29)
        {
            tamlan = 4.7f;
            lanmax = 6;
            tamdanhxa = 15;
        }
        else if (saorong > 29)
        {
            tamlan = 5;
            lanmax = 7;
            tamdanhxa = 16;
        }

       
        if (saorong > 15 && saorong <= 20)
        {
            maxStateIdle = 4;
        }
        else if (saorong > 20 && saorong <= 25)
        {
            maxStateIdle = 3;
        }
        else if (saorong > 25 && saorong <= 29)
        {
            maxStateIdle = 2;
        }
        else if (saorong >= 30)
        {
            maxStateIdle = 1;
        }
        debug.Log("sao rong " + saorong + " tam lan: " + tamlan + ", lanmax: " + lanmax + " tamdanhxa: " + tamdanhxa + ", maxStateIdle: " + maxStateIdle);
    }

    protected override void Updatee()
    {

    }
    public override void DayLuiABS()
    {
        //DayLuiDefault();
    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void SetHp(float fillhp, bool setonline = false)
    {
        SetHpDefault(fillhp, setonline);
    }
    private bool hoisinh = true;
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        MatMauDefault(maumat, cs, setonline);

    }

    public override void ABSAnimatorRun()
    {
        animplay = "Walking";
    }
    public override void LamChamABS(dataLamCham data)
    {
        LamChamDefault(data);
    }
    public override void ABSAnimatorAttack()
    {
        if (stateAnimAttack < maxStateAttack)
        {
            animplay = "Attack";
        }
        else
        {
            animplay = "Idlle";
        }
    }
    public override void AbsUpdateAnimIdle()
    {

    }
    public override void ABSAnimWin()
    {
        animplay = "Idlle";
    }
    public override void AbsUpdateAnimAttack()
    {
        if (stateAnimAttack == 1)
        {
            if (Target == null) return;
            for (int i = 0; i < skillObj.Length; i++)
            {
                if (!skillObj[i].gameObject.activeSelf)
                {
                    ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
                    skillObj[i].transform.position = transform.position;
                    skillObj[i].SetActive(true);
                    break;
                }
            }
         
        }
    }
    public override void SkillMoveOk()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(lanmax, Target.transform.parent.transform, new Vector2(5, 5)));
        float damee = dame;
        if(ronggan.Count == 1) damee *= 2;
        bool chimanggg = false;
        for (int i = 0; i < ronggan.Count; i++)
        {
            if (ronggan[i].name != "trudo" && ronggan[i].name != "truxanh")
            {
                DragonPVEController chisodich = ronggan[i].transform.Find("SkillDra").GetComponent<DragonPVEController>();

                if (!chimanggg)
                {
                    if (Random.Range(1, 100) <= _ChiMang)
                    {
                        chimanggg = true;
                        chisodich.MatMau(damee * 5, this);
                        PVEManager.InstantiateHieuUngChu("chimang", transform);
                    }
                }

                chisodich.MatMau(damee, this);
            }
            else
            {
                KillTru();
            }
        }
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
