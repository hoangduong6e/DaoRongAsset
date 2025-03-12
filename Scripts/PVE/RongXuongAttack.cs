
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public enum RongLoai1
{
    XuongMaTroi,
    NguyetLong,
    Rong2Dau,
    RongCay,
    RongBang,
    RongDat,
    RongSam,
    KyLan
}
public class RongXuongAttack : DragonPVEController
{
    public RongLoai1 loaiRong = RongLoai1.XuongMaTroi;
    private Action actionUpdateAnimAttack;
    private Action actionMoveSkillok;
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

    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
        if(loaiRong == RongLoai1.XuongMaTroi)
        {
            actionUpdateAnimAttack += AbsUpdateAnimAttackXuongMaTroi;
            actionMoveSkillok += SkillMoveOkXuongMaTroi;
        }
        else if (loaiRong == RongLoai1.Rong2Dau)
        {
            actionUpdateAnimAttack += AbsUpdateAnimAttackNguyetLong;
            actionMoveSkillok += SkillMoveOkHaiDau;
            hp /= 2;
            Maxhp = hp;
        }
        else if(loaiRong == RongLoai1.RongCay)
        {
            actionUpdateAnimAttack += AbsUpdateAnimAttackRongCay;
            actionMoveSkillok += SkillMoveOkRongCay;
        }    
        else if(loaiRong == RongLoai1.RongBang)
        {
            actionUpdateAnimAttack += AbsUpdateAnimAttackRongCay;
            actionMoveSkillok += SkillMoveOkRongBang;
        }
        else if (loaiRong == RongLoai1.RongDat || loaiRong == RongLoai1.RongSam)
        {
            actionUpdateAnimAttack += AbsUpdateAnimAttackRongCay;
            actionMoveSkillok += SkillMoveOkRongDat;
        }
        else if (loaiRong == RongLoai1.NguyetLong)
        {
            actionUpdateAnimAttack += AbsUpdateAnimAttackNguyetLong;
            actionMoveSkillok += SkillMoveOkNguyetLong;
        }
        else if (loaiRong == RongLoai1.KyLan)
        {
            actionUpdateAnimAttack += AbsUpdateAnimAttackKyLan;
            actionMoveSkillok += SkillMoveOkKyLan;
        }
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
    private bool hoisinh = true;
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        if(loaiRong == RongLoai1.Rong2Dau)
        {
            if (GetHpTru(maumat, cs, setonline) <= 0)
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
        else MatMauDefault(maumat, cs, setonline);

    }

    public override void ABSAnimatorRun()
    {
        if (loaiRong == RongLoai1.XuongMaTroi || loaiRong == RongLoai1.NguyetLong || loaiRong == RongLoai1.RongCay || loaiRong == RongLoai1.RongDat || loaiRong == RongLoai1.KyLan)
        {
            animplay = "Walking";
        }    
        else if(loaiRong == RongLoai1.Rong2Dau || loaiRong == RongLoai1.RongBang || loaiRong == RongLoai1.RongSam)
        {
            animplay = "Flying";
        }
    }
    public override void LamChamABS(dataLamCham data)
    {
        LamChamDefault(data);
    }
    public override void ABSAnimatorAttack()
    {
        if(stateAnimAttack < maxStateAttack)
        {
            animplay = "Attack";
        }
        else
        {
            if (loaiRong == RongLoai1.XuongMaTroi || loaiRong == RongLoai1.NguyetLong || loaiRong == RongLoai1.RongCay || loaiRong == RongLoai1.RongDat || loaiRong == RongLoai1.KyLan)
            {
                animplay = "Idlle";
            }
            else if (loaiRong == RongLoai1.Rong2Dau || loaiRong == RongLoai1.RongBang || loaiRong == RongLoai1.RongSam)
            {
                animplay = "Flying";
            }
        }
    }
    public override void AbsUpdateAnimIdle()
    {
       
    }
    public override void ABSAnimWin()
    {
        if (loaiRong == RongLoai1.XuongMaTroi || loaiRong == RongLoai1.NguyetLong || loaiRong == RongLoai1.RongBang || loaiRong == RongLoai1.RongDat || loaiRong == RongLoai1.KyLan)
        {
            animplay = "Idlle";
        }
        else if (loaiRong == RongLoai1.Rong2Dau)
        {
            animplay = "Win";
        }
        else if(loaiRong == RongLoai1.RongSam)
        {
            animplay = "Flying";
        }    
    }
    public override void AbsUpdateAnimAttack()
    {
        actionUpdateAnimAttack();
    }
    public override void SkillMoveOk()
    {
        actionMoveSkillok();
    }
    private void AbsUpdateAnimAttackXuongMaTroi()
    {
        if (stateAnimAttack == 1)
        {
            if (Target != null) ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");

            skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
    }
    private void SkillMoveOkXuongMaTroi()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(5, Target.transform.parent.transform, new Vector2(6, 6)));
        float damee = dame;

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
                chisodich.DayLuiABS();

            }
            else
            {
                KillTru();
            }
        }
    }
    private void AbsUpdateAnimAttackNguyetLong()
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
    private void AbsUpdateAnimAttackKyLan()
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
    private void SkillMoveOkKyLan()
    {
        GameObject teamdich = Target.transform.parent.gameObject;

        float damee = dame;
        bool chimanggg = false;

        if(teamdich.transform.childCount > 1)
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

    private void SkillMoveOkRongCay()
    {
        float damee = dame;
        if (Random.Range(1, 100) <= _ChiMang)
        {   
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }
        
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController dra = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            dra.MatMau(damee, this);
            dataLamCham data = new dataLamCham(5, "caylamcham");

            dra.LamChamABS(data);
        }
        else
        {
            KillTru();
        }
    }
    private void SkillMoveOkRongBang()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(6, Target.transform.parent.transform, new Vector2(3, 3)));
        float damee = dame;
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
                if (Random.Range(0, 100) > 50)
                {
                    dataLamCham data = new dataLamCham(5, "banglamcham");
                    chisodich.LamChamABS(data);
                }

            }
            else
            {
                KillTru();
            }
        }
      
    }
    private void SkillMoveOkRongDat()
    {
        //float damee = dame;
        //if (Random.Range(1, 100) <= _ChiMang)
        //{
        //    damee *= 5;
        //    PVEManager.InstantiateHieuUngChu("chimang", transform);
        //}

        //if (Target.name != "trudo" && Target.name != "truxanh")
        //{
        //    DragonPVEController dra = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
        //    dra.MatMau(damee, this);
        //  //  dra.LamChamABS(5, "caylamcham");
        //}
        //else
        //{
        //    KillTru();
        //}
    }
    private void AbsUpdateAnimAttackRongCay()
    {
        if (stateAnimAttack == 1)
        {
            if (Target == null) return;
            ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
            skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
    }
    private void SkillMoveOkHaiDau()
    {
        float damee = dame;
        if (Random.Range(1, 100) <= _ChiMang)
        {
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }

        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController dra = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            dra.MatMau(damee, this);
        }
        else
        {
            KillTru();
        }
    }
    private void SkillMoveOkNguyetLong()
    {
        List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(4, 4)));
        float damee = dame;
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
