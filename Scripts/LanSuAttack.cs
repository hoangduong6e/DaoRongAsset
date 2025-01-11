
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LanSuAttack : DragonPVEController
{
    private TruVienChinh hptru;
  //  private Action actionUpdate = null;
    public override void SetHpOnline(JSONObject data)
    {

    }

    protected override void ABSAwake()
    {
        anim = transform.parent.transform.GetChild(0).GetComponent<Animator>();
        hptru = VienChinh.vienchinh.TruDo.GetComponent<TruVienChinh>();
        actionUpdate = delegate {
            if (transform.parent.transform.position.x > Target.position.x + tamdanhxa)
            {
                AnimatorRun();
            }
            else
            {
                AnimatorAttack();
                actionUpdate = null;
            }
        };
        thongke = false;
        transform.parent.GetComponent<DraUpdateAnimator>().DragonPVEControllerr = this;
    }
    public override void Choang(float giay = 0.4F, bool setOnline = false)
    {
        // base.Choang(giay);
    }
    public override void AbsStart()
    {

    }
    protected override void Updatee()
    {
        //VienChinh.vienchinh.SetMucTieuTeamDo();
        Target = VienChinh.vienchinh.muctieudo.transform;
        if (actionUpdate != null) actionUpdate();
       
      //  debug.Log("parent: " + transform.parent.transform.position.x + ", tamxa: " + (Target.position.x - tamdanhxa));

    }
    public override void DayLuiABS()
    {
        // DayLuiDefault();
    }
    public override void SetHp(float fillhp, bool setonline = false)
    {
        //SetHpDefault(fillhp, setonline);
    }
    //  float tongdame = 0;
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        //tongdame += maumat; 
        //  UnityEngine.debug.Log(cs.nameobj + " đánh, dame: " + maumat + " tổng dame: " + tongdame);
        ThongKeDame.AddThongKe(new ThongKeDame.CData(cs.team.ToString(), cs.nameobj, cs.idrong, maumat, ThongKeDame.EType.dame));
        hptru.MatMau(maumat);

        if (actionUpdate != null)
        {
            AnimatorAttack();
            actionUpdate = null;
        }
        //NetworkManager.ins.socket.Emit("DanhBossTG", JSONObject.CreateStringObject(maumat.ToString()));
        //   MatMauDefault(maumat, cs);
    }


    public override void ABSAnimatorRun()
    {
        animplay = "Idlle";
    }
    public override void LamChamABS(dataLamCham data)
    {
        // LamChamDefault(data);
    }
    public override void ABSAnimatorAttack()
    {
        animplay = "Attack";
        //if (stateAnimAttack < maxStateAttack)
        //{
        //    animplay = "Attack";
        //}
        //else
        //{
        //    animplay = "Idlle";
        //}
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
            SkillMoveOkk();
        }
        else stateAnimAttack = 0;
    }
    private void SkillMoveOkk()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(Target.transform.parent.transform, new Vector2(2.2f, 2.2f)));
        debug.LogWarning("Skill move lân sư, count ronggan: " + ronggan.Count);
        float damee = dame;
        bool chimanggg = false;
        for (int i = 0; i < ronggan.Count; i++)
        {
            if (ronggan[i].name != "trudo" && ronggan[i].name != "truxanh")
            {
                DragonPVEController chisodich = ronggan[i].GetComponent<DraUpdateAnimator>().DragonPVEControllerr;

                if (!chimanggg)
                {
                    if (Random.Range(1, 100) <= _ChiMang)
                    {
                        chimanggg = true;
                        chisodich.MatMau(damee * 5, this);
                        PVEManager.InstantiateHieuUngChu("chimang", transform);
                    }
                }
                chisodich.DayLuiABS();
                dataLamCham data = new dataLamCham(1f, "caylamcham");
                chisodich.LamChamABS(data);
                chisodich.Choang(Random.Range(0.2f,1));
                chisodich.MatMau(damee, this);
            }
            else
            {
               // KillTru();
            }
        }
    }
    public override void BienCuuABS(float time)
    {
       // BienCuuDefault(time);
    }
}
