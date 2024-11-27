
using System;
using System.Collections.Generic;
using UnityEngine;
public class QuaiVienChinhNew : DragonPVEController
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

    private Action actionUpdateAnimAttack, actionMoveSkillok;
    public override void SetHpOnline(JSONObject data)
    {
     
    }
    public override void ChoangABS(float giay = 0.2f)
    {
        ChoangDefault(giay);
    }
    protected override void ABSAwake()
    {
    //    anim = transform.parent.GetComponent<Animator>();
         nameobj = "";
        transform.parent.GetComponent<DraUpdateAnimator>().DragonPVEControllerr = this;
        PVEManager.GetUpdateMove(transform, "TeamDo");

        actionMoveSkillok += SkillMoveOkk;
        for (int i = 0; i < skillObj.Length; i++)
        {
            skillObj[i].GetComponent<SkillDraController>().skillmoveok += actionMoveSkillok;
        }
        team = Team.TeamDo;
        actionUpdateAnimAttack += AbsUpdateAnimAttackk;
    }
    public override void AbsStart()
    {
  
  
    }

    protected override void Updatee()
    {

    }
    public override void DayLuiABS()
    {
       // DayLuiDefault();
    }
    public override void SetHp(float fillhp, bool setonline = false)
    {
        SetHpDefault(fillhp, setonline);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        MatMauDefault(maumat, cs, setonline);
    }


    public override void ABSAnimatorRun()
    {
        animplay = "Idlle";
    }
    public override void LamChamABS(dataLamCham data)
    {
        //LamChamDefault(data);
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
        //for (int i = 0; i < skillObj.Length; i++)
        //{
        //    skillObj[i].GetComponent<SkillDraController>().skillmoveok -= actionMoveSkillok;
        //}
        animplay = "Idlle";

    }
    public override void AbsUpdateAnimAttack()
    {
        //    actionUpdateAnimAttack();
        AbsUpdateAnimAttackk();
    }
    public override void SkillMoveOk()
    {
     //   actionMoveSkillok();
    }
    private void AbsUpdateAnimAttackk()
    {
        if (stateAnimAttack == 1)
        {
            if (Target == null) return;
           // ReplayData.AddAttackTarget(transform.parent.name, 0.ToString(), "dungdau");
            skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }

    }
    private void SkillMoveOkk()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(3, 2)));
        float damee = dame;
        if (UnityEngine.Random.Range(1, 100) <= _ChiMang)
        {
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }
        for (int i = 0; i < ronggan.Count; i++)
        {
            if (ronggan[i].name != "trudo" && ronggan[i].name != "truxanh")
            {
                DragonPVEController dra = ronggan[i].transform.Find("SkillDra").GetComponent<DragonPVEController>();

                dra.MatMau(damee, this);
              //  if (daylui) dra.DayLuiABS();
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
