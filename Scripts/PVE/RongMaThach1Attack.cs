
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class RongMaThach1Attack : DragonPVEController
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

    protected override void ABSAwake()
    {

    }
    public override void ChoangABS(float giay = 0.2f)
    {
        ChoangDefault(giay);
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
            ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
            skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
    }
    public override void SkillMoveOk()
    {
        float damee = dame;
        if (Random.Range(1, 100) <= _ChiMang)
        {
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }

        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController dra = Target.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            dra.MatMau(damee, this);
            //  dra.LamChamABS(5, "caylamcham");
        }
        else
        {
            KillTru();
        }
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
