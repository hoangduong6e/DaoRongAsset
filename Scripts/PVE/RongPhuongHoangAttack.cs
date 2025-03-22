
using System.Collections.Generic;
using UnityEngine;

public class RongPhuongHoangAttack : DragonPVEController
{

    private bool hoisinh = true;
    private bool daylui = true;
    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
        if (!VienChinh.vienchinh.DanhOnline) skillObj[1].GetComponent<SkillDraController>().skillmoveok += SkillMoveOk;
        //   Transform parent = transform.parent;
        //   parent.transform.position = new Vector3(transform.position.x, transform.position.y + 3);
    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
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
        //MatMauDefault(maumat, cs);
        if(GetHpTru(maumat,cs) <= 0)
        {
            if (hoisinh)
            { 
                hoisinh = false;
                hp = Maxhp;
                ImgHp.fillAmount = 1;
                PVEManager.InstantiateHieuUngChu("hoisinh", transform);
            }
            else
            {
                Died();
            }
        }
    }

    public override void SkillMoveOk()
    {
        if (Target == null) return;
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(5, 5)));
        float damee = dame;
        if (CrGame.ins.NgayDem == "Ngay")
        {
      //      debug.Log("Phượng hoàng ban ngày + 50% sức đánh");
            damee *= 1.5f; //Cộng 50% sức đánh
        }

        // bool daylui = PVEManager.GetTyLeDayLui(saorong);
        bool chimanggg = false;
        for (int i = 0;i < ronggan.Count;i++)
        {
            if (ronggan[i].name != "trudo" && ronggan[i].name != "truxanh")
            {
                if (ronggan[i] != null)
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
                }
                

            }
            else
            {
                KillTru();
            }
        }    

        //gameObject.SetActive(false);

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
    public override void LamChamABS(dataLamCham data)
    {
        LamChamDefault(data);
    }
    public override void DayLuiABS()
    {
        DayLuiDefault();
    }
    public override void AbsUpdateAnimAttack()
    {
        if (stateAnimAttack == 1 || stateAnimAttack == 3)
        {
            if (stateAnimAttack == 3) daylui = true;
            else daylui = false;
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
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
