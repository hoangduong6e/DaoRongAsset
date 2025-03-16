
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BossHnew : DragonPVEController
{
    private string animAttackboss = "Attack";
    private bool den = true;
    private Action actionUpdateAnimAttack, actionMoveSkillok;
    float damechieucuoi = 6000;
    public override void SetHpOnline(JSONObject data)
    {

    }
    public override void Choang(float giay = 0.4F)
    {
        // base.Choang(giay);
    }
    protected override void ABSAwake()
    {
        //    anim = transform.parent.GetComponent<Animator>();
        nameobj = "";
        transform.parent.GetComponent<DraUpdateAnimator>().DragonPVEControllerr = this;
        PVEManager.GetUpdateMove(transform, Team.TeamDo);
        if(VienChinh.vienchinh.chedodau == CheDoDau.Halloween)
        {
            int aichon = MenuEventHalloween2024.inss.aiDangChon + 1;
            if (aichon < 9) damechieucuoi = 5000;
            else if (aichon == 9) damechieucuoi = 6000;
            else if (aichon == 12) damechieucuoi = 7000;
            else if (aichon == 15) damechieucuoi = 8000;
            else if (aichon == 18) damechieucuoi = 9000;
            else if (aichon == 19) damechieucuoi = 15000;
            else if (aichon == 20) damechieucuoi = 20000;
        }



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
    public override void SetHp(float fillhp)
    {
        SetHpDefault(fillhp);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs)
    {
        if(VienChinh.vienchinh.chedodau == CheDoDau.BossTG)
        {
            ThongKeDame.AddThongKe(new ThongKeDame.CData(cs.team.ToString(), cs.nameobj, cs.idrong, maumat, ThongKeDame.EType.dame));
            NetworkManager.ins.socket.Emit("DanhBossTG", JSONObject.CreateStringObject(maumat.ToString()));
        }    
        else
        {
            MatMauDefault(maumat, cs);
        }


      
        if (den)
        {
            if (hp <= Maxhp / 1.3f)
            {
                animAttackboss = "Idlle";
                Manden();
                den = false;
                anim.SetInteger("tancong", 2);
            }

        }
      
    }

    void Manden()
    {
        GiaoDienPVP.ins.SetPanelToi = true;
        //   vienchinh.Hieuungd.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1.5f);
            // vienchinh.Hieuungd.SetActive(false);
            GiaoDienPVP.ins.SetPanelToi = false;
            if (VienChinh.vienchinh.chedodau == CheDoDau.Halloween)
            {
                if (MenuEventHalloween2024.inss.isKichHoatGiamSucManh) damechieucuoi = 1000;
            }
                
            else damechieucuoi = 2000;
            animAttackboss = "Attack2";
            yield return new WaitForSeconds(10f);
            animAttackboss = "Attack";
            yield return new WaitForSeconds(25f);
            Manden();
        }
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
            animplay = animAttackboss;
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
        if(animAttackboss == "Attack")
        {
            if (stateAnimAttack == 1)
            {
                if (Target == null) return;
                // ReplayData.AddAttackTarget(transform.parent.name, 0.ToString(), "dungdau");
                skillObj[0].transform.position = transform.position;
                skillObj[0].SetActive(true);
            }
        }
        else
        {
            for (int i = 1; i < VienChinh.vienchinh.TeamXanh.transform.childCount; i++)
            {
                VienChinh.vienchinh.TeamXanh.transform.GetChild(i).transform.Find("SkillDra").GetComponent<DragonPVEController>().MatMau(2000,this);
            }
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
                DragonPVEController dra = ronggan[i].GetComponent<DraUpdateAnimator>().DragonPVEControllerr;

                dra.MatMau(damee, this);
                //  if (daylui) dra.DayLuiABS();
            }
            else
            {
                KillTru(10000);
            }
        }
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
    protected override void Die()
    {
        base.Die();
        GiaoDienPVP.ins.SetPanelToi = false;
    }
}
