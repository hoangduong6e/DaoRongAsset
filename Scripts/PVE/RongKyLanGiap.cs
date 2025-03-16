using UnityEngine;
using UnityEngine.UI;

public class RongKyLanGiap : DragonPVEController
{
    public Image fillGiap;
    protected override void ABSAwake()
    {

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
        if (fillGiap.fillAmount > ImgHp.fillAmount)
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
            //if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !setonline)
            //{
            //    JSONObject newjson = new JSONObject();
            //    newjson.AddField("id", transform.parent.name);
            //    // float fillset = (hpgiap - maumat) / maxhpgiap;
            //    double tru = System.Math.Round(hpgiap - maumat, 2);
            //    newjson.AddField("hpgiap", tru.ToString());
            //    newjson.AddField("hp", "");
            //    hpgiap = (float)tru;
            //    DauTruongOnline.ins.AddUpdateData(newjson);

            //    return;
            //}

            hpgiap -= maumat;
            float fillamount = (float)hpgiap / (float)maxhpgiap;
            fillGiap.fillAmount = fillamount;

            ReplayData.addHp(transform.parent.name, fillGiap.fillAmount.ToString());
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
        GameObject teamdich = Target.transform.parent.gameObject;

        float damee = dame;
        bool chimanggg = false;

        if (teamdich.transform.childCount > 1)
        {
            for (int i = 1; i < teamdich.transform.childCount; i++)
            {
                DragonPVEController chisodich = teamdich.transform.GetChild(i).GetComponent<DraUpdateAnimator>().DragonPVEControllerr;

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
            }
        }
        else KillTru();

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
