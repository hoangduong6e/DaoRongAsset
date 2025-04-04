﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class ThanTuanLongAttack : DragonPVEController
{
    public override void AbsStart()
    {
        //  debug.Log("AbsStart");
        // string team = transform.parent.transform.parent.name;
        if (team == Team.TeamXanh)
        {
            VienChinh.vienchinh.HienIconSkill(300, "Xanh", "iconKhienTuanLongXanh");
            VienChinh.vienchinh.SetBuffGiapall(35, team);
        }
        else
        {
            //  ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
            VienChinh.vienchinh.HienIconSkill(300, "Do", "iconKhienTuanLongDo");
            VienChinh.vienchinh.SetBuffGiapall(35, team);
        }

        Transform khien = transform.Find("khientuanlong");
        Vector3 vec1 = khien.localScale;
        vec1 = new Vector3(vec1.x * 1.15f, vec1.y * 1.15f, vec1.z);

        khien.transform.localScale = vec1;
    }
    protected override void Updatee()
    {

    }
    public override void SetHp(float fillhp)
    {
        SetHpDefault(fillhp);
    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs)
    {
        MatMauDefault(maumat, cs);
    }
    public override void LamChamABS(dataLamCham data)
    {
        data.eff = "";
        data.chia = 1;
        LamChamDefault(data);
    }
    protected override void ABSAwake()
    {

    }
    public override void DayLuiABS()
    {

    }
    private byte luotdanh = 0;

    public override void SkillMoveOk()
    {
        float damee = dame;
        if (Random.Range(1, 100) <= _ChiMang)
        {
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }
        luotdanh++;
        if(luotdanh >= 3)
        {
            damee *= 2;
            luotdanh = 0;
        }    
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController dra = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            dataLamCham data = new dataLamCham(5f, "",3,"0",false,false,false,true);//làm chậm 90%
            dra.LamChamABS(data);
            dra.MatMau(damee, this);
        }
        else
        {
            KillTru();
        }

    }
    public override void ABSAnimatorRun()
    {
        animplay = "Walking";
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
            ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
            // skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (team == Team.TeamDo)
        {
            VienChinh.vienchinh.TaticonSkill("iconKhienTuanLongDo", "Do");
            VienChinh.vienchinh.SetBuffGiapall(-saorong, team);
        }
        else
        {
            VienChinh.vienchinh.TaticonSkill("iconKhienTuanLongXanh", "Xanh");
            VienChinh.vienchinh.SetBuffGiapall(-saorong, team);
        }
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
