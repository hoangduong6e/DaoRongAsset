
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RongMaTroiGiapAttack : DragonPVEController
{
    //public override void AbsMoveComponent(SocketIOEvent e)
    //{
    //    Transform tfMove = transform.parent.transform;
    //    tfMove.gameObject.AddComponent<RongXuongAttack>().ParseData(e);
    //    debug.Log("move component");
    //    //PVEManager.GetUpdateMove(tfMove, tfMove.transform.parent.name);
    //    Destroy(gameObject.GetComponent<RongXuongAttack>());
    //}
    //private void Start()
    //{
    //    Startt();
    //}
    // Update is called once per frame
    public Image fillGiap;
    public override void ChoangABS(float giay = 0.2f)
    {
        ChoangDefault(giay);
    }
    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
       
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
    public override void SetHp(float fillhp, bool setonline = false)
    {
        if(fillGiap.fillAmount > ImgHp.fillAmount)
        {
            fillGiap.fillAmount = fillhp;
            ImgHp.transform.parent.gameObject.SetActive(true);
        }
        else SetHpDefault(fillhp,setonline);
    }

    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        if (hpgiap > 0)
        {
            if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !setonline)
            {
                JSONObject newjson = new JSONObject();
                newjson.AddField("id", transform.parent.name);
                // float fillset = (hpgiap - maumat) / maxhpgiap;
                double tru = System.Math.Round(hpgiap - maumat, 2);
                newjson.AddField("hpgiap", tru.ToString());
                newjson.AddField("hp", "");
                hpgiap = (float)tru;
                DauTruongOnline.ins.AddUpdateData(newjson);

                return;
            }

            hpgiap -= maumat;
            float fillamount = (float)hpgiap / (float)maxhpgiap;
            fillGiap.fillAmount = fillamount;

            ReplayData.addHp(transform.parent.name, fillGiap.fillAmount.ToString());
            ImgHp.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            MatMauDefault(maumat, cs, setonline);
        }
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
        if (stateAnimAttack == 1)
        {
            if (Target != null) ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");

            skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
    }
    public override void SkillMoveOk()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(3, 2)));
        float damee = dame;

        bool chimanggg = false;
        for (int i = 0; i < ronggan.Count; i++)
        {
            if (ronggan[i].name != "trudo" && ronggan[i].name != "truxanh")
            {
                DragonPVEController chisodich = ronggan[i].transform.Find("SkillDra").GetComponent<DragonPVEController>();

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
                chisodich.DayLuiABS();

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
