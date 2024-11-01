
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class RongMatXeAttack : DragonPVEController
{
    private string animplayrun = "Run", animplayIdle = "Idlle";
    public Transform xuongMatXe;
    private bool setSpeedDefault = false;
    protected override void ABSAwake()
    {
        xuongMatXe.transform.position = new Vector3(xuongMatXe.transform.position.x,xuongMatXe.transform.position.y + Random.Range(0f,3f));
    }
    public override void ChoangABS(float giay = 0.2f)
    {
        ChoangDefault(giay);
    }
    public override void AbsStart()
    {
        //  actionUpdateAnimAttack += AbsUpdateAnimAttackRongCay;
        //  actionMoveSkillok += SkillMoveOkRongDat;
        //else if (loaiRong == RongLoai1.RongSam)
        //{
        //    actionUpdateAnimAttack += AbsUpdateAnimAttackRongCay;
        //    actionMoveSkillok += SkillMoveOkRongDat;
        //}
        
    }
    protected override void Updatee()
    {
        if(xuongMatXe != null)
        {
            Vector3 vec = Vector3.zero;
            if (team == Team.TeamXanh)
            {
                vec = new Vector3(VienChinh.vienchinh.TruDo.transform.position.x + 25, xuongMatXe.transform.position.y, xuongMatXe.transform.position.z);
            }
            else
            {
                vec = new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x - 25, xuongMatXe.transform.position.y, xuongMatXe.transform.position.z);
            }
            xuongMatXe.transform.LeanMove(vec, 3f).setOnComplete(() => {
                Destroy(xuongMatXe.gameObject);
            });
        }    
    }
    public override void DayLuiABS()
    {
       // DayLuiDefault();
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
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        MatMauDefault(maumat, cs, setonline);

    }
    public override void ABSAnimatorRun()
    {
        animplay = animplayrun;
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
            animplay = animplayIdle;
        }
        animplayrun = "Walking";
        if(!setSpeedDefault)
        {
            setSpeedDefault = true;
            maxspeed = 2.5f;
            speed = maxspeed;
        }
        
    }
    public override void AbsUpdateAnimIdle()
    {
        if(stateAnimIdle == 0)// nếu mới reset
        {
            animplayIdle = (Random.Range(0, 100) >= 50) ? "Idlle" : "Idlle2";
        }
    }
    public override void ABSAnimWin()
    {
        animplay = "Win";
    }
    public override void AbsUpdateAnimAttack()
    {
        if (stateAnimAttack == 1)
        {
            if (Target == null) return;
            ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
            SkillMoveOk();
           // skillObj[0].transform.position = transform.position;
           // skillObj[0].SetActive(true);
        }
    }
    public override void SkillMoveOk()
    {
        float damee = dame;
        if (Random.Range(1, 100) <= chimang)
        {
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }

        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController dra = Target.transform.Find("SkillDra").GetComponent<DragonPVEController>();
          //  Debug.Log("nameobj " + dra.nameobj);
            if(dra.nameobj.Contains("RongXuong") || dra.nameobj.Contains("RongMaTroi"))
            {
                damee *= 2;
                debug.Log("x2 dame đánh ma trơi, xương");
            }
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
 