using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RongLuaMatXanhGiapAttack : DragonPVEController
{

    private byte danh = 1;
    public Image fillGiap;

    private Action skillmoveok;
    protected override void ABSAwake()
    {
        if (!VienChinh.vienchinh.DanhOnline)
        {
            skillmoveok = sKILLmOVE;
        }
    }
    public override void AbsStart()
    {
        debug.Log("AbsStart");
        //   Transform parent = transform.parent;
        //   parent.transform.position = new Vector3(transform.position.x, transform.position.y + 3);


    }
    protected override void Updatee()
    {

    }
    public override void SetHpOnline(JSONObject data)
    {
        //  debug.LogError("dataaaaaa " + data);
        if (data["hpgiap"])
        {
            hpgiap = float.Parse(data["hpgiap"].str);
            fillGiap.fillAmount = hpgiap / maxhpgiap;
        }    
        else
        {
            hp = float.Parse(data["hp"].str);
            ImgHp.fillAmount = hp / Maxhp;
           
        }
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void SetHp(float fillhp)
    {
        if (fillGiap.fillAmount > 0)
        {
            fillGiap.fillAmount = fillhp;
            ImgHp.transform.parent.gameObject.SetActive(true);
        }
        else SetHpDefault(fillhp);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs)
    {
        if (hpgiap > 0)
        {
            //if (VienChinh.vienchinh.chedodau == CheDoDau.Online)
            //{
            //    JSONObject newjson = new JSONObject();
            //    newjson.AddField("id", transform.parent.name);
            //    // float fillset = (hpgiap - maumat) / maxhpgiap;
            //    double tru = System.Math.Round(hpgiap - maumat, 2);
            //    newjson.AddField("hpgiap", tru.ToString());
            //    newjson.AddField("hp","");
            //    hpgiap = (float)tru;
            //    DauTruongOnline.ins.AddUpdateData(newjson);

            //    return;
           // }

            hpgiap -= maumat;
            float fillamount = (float)hpgiap / (float)maxhpgiap;
            fillGiap.fillAmount = fillamount;

            ReplayData.addHp(idrong, fillGiap.fillAmount.ToString());
            ImgHp.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            MatMauDefault(maumat, cs);
        }
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
        skillmoveok?.Invoke();
    }

    private void sKILLmOVE()
    {
        //List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(3, 2)));
        float damee = dame;
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController chisodich = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            if (nameobj == "RongLuaMatXanhSapphire")
            {
                if (CrGame.ins.NgayDem == ENgayDem.Dem)
                {
                    debug.Log("Sapphire Ban đêm + 50% sức đánh");
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
        animplay = "Win";

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
