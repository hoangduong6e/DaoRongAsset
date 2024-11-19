using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NamNguSacAttack : DragonPVEController
{
    //private byte danh = 1;
    public RongNguSacAttack NguSacAttack;
    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
        debug.Log("AbsStart");
        idrong = transform.parent.name.Split('-')[1];
        nameobj = "RongNguSac";
        //thongke = false;
        //for (int i = 0; i < skillObj.Length; i++)
        //{
        //    if (team == Team.TeamDo)
        //    {
        //        Vector3 scale = transform.localScale;
        //        scale.x = -scale.x;
        //        transform.localScale = scale;
        //    }
        //}

    }
    public override void ChoangABS(float giay = 0.2f)
    {
      //  ChoangDefault(giay);
    }
    protected override void Updatee()
    {
      //  transform.position = transform.parent.position;
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
       // DayLuiDefault();
    }
    public override void LamChamABS(dataLamCham data)
    {
      //LamChamDefault(data);
    }
    public override void SkillMoveOk()
    {
        float damee = dame;
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController chisodich = Target.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            if (Random.Range(1, 100) <= chimang)
            {
                damee *= 5;
                PVEManager.InstantiateHieuUngChu("chimang", transform);
            }

            chisodich.MatMau(damee, this);
        }
        else
        {
            KillTru(10000);
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
        animplay = "Walking";

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
        //   if (stateAnimAttack == maxStateAttack)
    }
    public override void BienCuuABS(float time)
    {
       // BienCuuDefault(time);
    }
    private void OnDestroy()
    {
        if(NguSacAttack != null) NguSacAttack.giapphantram -= 30;
    }
}
