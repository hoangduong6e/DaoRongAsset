
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class RongHuyetNguyetLongAttack : DragonPVEController
{
    protected override void ABSAwake()
    {
      
    }
    public override void ChoangABS(float giay = 0.2f)
    {
       // ChoangDefault(giay);
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
        if (stateAnimAttack < 3)
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
         List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(4, Target.transform.parent.transform, new Vector2(4, 4)));
        float damee = dame;
        bool chimanggg = false;
        for (int i = 0; i < ronggan.Count; i++)
        {
            if (ronggan[i].name != "trudo" && ronggan[i].name != "truxanh")
            {
                DragonPVEController chisodich = ronggan[i].transform.Find("SkillDra").GetComponent<DragonPVEController>();

                if (!chimanggg)
                {
                    if (Random.Range(1, 100) <= chimang)
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
 