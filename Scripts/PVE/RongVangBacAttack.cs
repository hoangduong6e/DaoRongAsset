
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class RongVangBacAttack : DragonPVEController
{
    protected byte danh = 0;
    public override void AbsStart()
    {
      //  debug.Log("AbsStart");
    }
    protected override void Updatee()
    {

    }
    public override void SetHp(float fillhp)
    {
        SetHpDefault(fillhp);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs)
    {
        MatMauDefault(maumat,cs);
    }
    protected override void ABSAwake()
    {

    }

    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void LamChamABS(dataLamCham data)
    {
        LamChamDefault(data);
    }
    public override void SkillMoveOk()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(1f, 1f)));
        float damee = dame;
        if (Random.Range(1, 100) <= _ChiMang)
        {
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }

        bool daylui = PVEManager.GetTyLeDayLui(saorong);

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

                chisodich.MatMau(damee, this);
                if (daylui) chisodich.DayLuiABS();
                chisodich.Choang();
            }
            else
            {
                KillTru();
            }
        }

        if (danh < 3) danh += 1;
        else
        {
            danh = 0;
            BatTu(1);
            debug.Log("Rồng Bất tử");
        }
    }
    public override void DayLuiABS()
    {
        DayLuiDefault();
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
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
