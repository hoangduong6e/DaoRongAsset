
using UnityEngine;

public class VietHaiLongVuongAttack : DragonPVEController
{
    protected override void ABSAwake()
    {
       
    }
    public override void AbsStart()
    {
        VienChinh.vienchinh.SetTyLeHoiSinh(saorong,team);

        if (saorong >= 1 && saorong <= 11)
        {
            VienChinh.vienchinh.maxhs = 1;
        }
        else if (saorong >= 12 && saorong <= 20)
        {
            VienChinh.vienchinh.maxhs = 2;
        }
        else if (saorong == 21)
        {
            VienChinh.vienchinh.maxhs = 3;
        }
        else
        {
            VienChinh.vienchinh.maxhs = 0; // Giá trị mặc định nếu không nằm trong các khoảng
        }

        hs = VienChinh.vienchinh.maxhs;// ngăn không cho hồi sinh
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
    public override void SetHp(float fillhp, bool setonline = false)
    {
        SetHpDefault(fillhp, setonline);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        MatMauDefault(maumat, cs, setonline);
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
        animplay = "Walking";
    }
    public override void ABSAnimatorAttack()
    {
        //if (stateAnimAttack < maxStateAttack)
        //{
        //    animplay = "Attack";
        //}
        //else
        //{
        //    animplay = "Idlle";
        //}

        animplay = "Attack";
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
        if (stateAnimAttack == maxStateAttack) stateAnimAttack = 0;
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
    private void OnDestroy()
    {
        VienChinh.vienchinh.SetTyLeHoiSinh(saorong, team);
        VienChinh.vienchinh.maxhs = 0;
    }
}
