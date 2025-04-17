using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RongLuaMatXanhAttack : DragonPVEController
{
    public string LoaiRong = "RongLuaMatXanh";
    private Action actionSkillMoveOk;
    private byte danh = 1;
    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
        debug.Log("AbsStart");
        if (LoaiRong == "RongLuaMatXanh") actionSkillMoveOk = SkillMoveOkRongLuaMatXanh;
        else actionSkillMoveOk = SkillMoveOkRongLua;
     //   Transform parent = transform.parent;
     //   parent.transform.position = new Vector3(transform.position.x, transform.position.y + 3);


    }
    protected override void Updatee()
    {

    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void SetHp(float fillhp)
    {
        SetHpDefault(fillhp);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs)
    {
        MatMauDefault(maumat, cs);
    }
    public override void DayLuiABS()
    {
        DayLuiDefault();
    }
    public override void LamChamABS(dataLamCham data)
    {
        LamChamDefault(data);
    }
    public override void SkillMoveOk()
    {
        //List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(3, 2)));
        actionSkillMoveOk();
       

    }
    private void SkillMoveOkRongLuaMatXanh()
    {
        float damee = dame;
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController chisodich = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            if (nameobj == "RongLuaMatXanhSapphire")
            {
                if (CrGame.ins.NgayDem == ENgayDem.Dem)
                {
                 //   debug.Log("Sapphire Ban đêm + 50% sức đánh");
                    damee += damee / 2; //Cộng 50% sức đánh
                }
            }
            if (danh <= 3) danh += 1;
            else
            {
                danh = 0;
                debug.Log("damee x5");
                damee *= 5;
            }
            if (Random.Range(1, 100) <= _ChiMang)
            {
                damee *= 5;
                PVEManager.InstantiateHieuUngChu("chimang", transform);
            }

            chisodich.MatMau(damee, this);
        }
        else
        {
            KillTru();
        }
        //gameObject.SetActive(false);
    }
    private void SkillMoveOkRongLua()
    {
        float damee = dame;
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController chisodich = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            if (Random.Range(1, 100) <= _ChiMang)
            {
                damee *= 5;
                PVEManager.InstantiateHieuUngChu("chimang", transform);
            }

            chisodich.MatMau(damee, this);
        }
        else
        {
            KillTru();
        }
    }    
    public override void ABSAnimatorRun()
    {
        animplay = "Flying";
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
            ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
            skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
        if (stateAnimAttack == maxStateAttack) danh += 1;
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
