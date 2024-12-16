
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RongMaThach3Attack : DragonPVEController
{

    private byte danh = 1;
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
    // [SerializeField]
    //LoaiMaThach loai;

    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {

    }

    protected override void Updatee()
    {

    }
    public override void DayLuiABS()
    {
        DayLuiDefault();
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
            animplay = "Flying";
        }
    }
    public override void AbsUpdateAnimIdle()
    {

    }
    public override void ABSAnimWin()
    {
        animplay = "Flying";
    }
    public override void AbsUpdateAnimAttack()
    {
        if (stateAnimAttack == 1)
        {
            for (int i = 0; i < skillObj.Length; i++)
            {
                if (!skillObj[i].gameObject.activeSelf)
                {
                    if (Target == null) return;
                    ReplayData.AddAttackTarget(transform.parent.name, i.ToString(), "dungdau");
                    skillObj[i].transform.position = transform.position;
                    skillObj[i].SetActive(true);
                    break;
                }
            }
        }

    }
    public override void SkillMoveOk()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(10, Target.transform.parent.transform, new Vector2(6, 6)));
        float damee = dame;
        if (CrGame.ins.NgayDem == "Dem")
        {
            damee *= 1.5f; //Cộng 50% sức đánh
        }
        bool chimanggg = false;

        if (danh <= 3) danh += 1;
        else
        {
            danh = 0;
            debug.Log("damee x5");
            damee *= 5;
        }

        dataLamCham data = new dataLamCham(3);

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
                chisodich.DayLuiABS();

                chisodich.LamChamABS(data);

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
