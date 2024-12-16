using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
public class RongKhongTuocAttack : DragonPVEController
{
    private byte landanh = 0;
    private string nameskillsd = "Attack_1";

    public override void AbsStart()
    {
      
    }
    protected override void Updatee()
    {

    }
    public override void SetHp(float fillhp, bool setonline = false)
    {
        SetHpDefault(fillhp, setonline);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        //if (Target.transform.parent.name == "TeamXanh" && VienChinh.vienchinh.chedodau == CheDoDau.SoloKhongTuoc)//nhoxoa
        //{
        //    VienChinh.vienchinh.dameKhongTuoc += maumat;
        //}
        MatMauDefault(maumat, cs, setonline);
    }


    //public override void ChoangABS(float giay = 0.2f)
    //{
    //    ChoangDefault(giay);
    //}
    protected override void ABSAwake()
    {

    }
    public override void DayLuiABS()
    {
        DayLuiDefault();
    }
    public override void LamChamABS(dataLamCham data)
    {
        LamChamDefault(data);
    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void SkillMoveOk()
    {
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            float damee = dame;
            DragonPVEController chisodich = Target.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            if (Random.Range(1, 100) <= _ChiMang)
            {
                damee *= 5;
                PVEManager.InstantiateHieuUngChu("chimang",transform);
            }
            chisodich.MatMau(damee, this);
        }
        else
        {
            KillTru();
        }
        // Nokhicham.transform.position = chiso.Target;
        //Nokhicham.SetActive(true);
        landanh += 1;
        if (landanh == 3)
        {
            debug.Log("Khổng tước bất tử");
            BatTu(2);
        }
        else if (landanh >= 6)
        {
            landanh = 0;
            stateAnimAttack = 0;
            nameskillsd = "Attack_2";
        }
     //   gameObject.SetActive(false);
    }
    public override void ABSAnimatorRun()
    {
        animplay = "Walking";
    }
    public override void ABSAnimatorAttack()
    {
        if (stateAnimAttack < maxStateAttack)
        {
            animplay = nameskillsd;
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
        if(nameskillsd == "Attack_1")
        {
            if (stateAnimAttack == 1)
            {
                ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
                // skillObj[0].transform.position = transform.position;
                skillObj[0].SetActive(true);
            }
        }   
        else
        {
            if (stateAnimAttack == 1)
            {
                if (VienChinh.vienchinh.checkIconSkill("iconKhongTuoc",transform.parent.transform.parent.name)) return;
                // ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
                // anim.Play("Attack_2");
                if (Target == null) return;
                float hutmaubandau = _HutHp;
                _HutHp = 0;
                BatTu(2);
                // anim.SetInteger("tancong", 2);

                landanh = 0;
                float tilelamcham = 20;
                float timelamcham = 3;
                GameObject teamdich = Target.transform.parent.gameObject;
               // float chialamcham = 2.5f;
                float timetan = 4;
              //  float speedtru = 1.5f;
                if (saorong > 15 && saorong <= 20)
                {
                    tilelamcham = 30;
                    timetan = 5;
                }
                else if (saorong >20 && saorong <= 25)
                {
                    tilelamcham = 50;
                    timetan =65;
                }
                else if (saorong > 25 && saorong <= 29)
                {
                    tilelamcham = 60;
                    timetan = 7;
                }
                else if(saorong >= 30)
                {
                    tilelamcham = 70;
                    timetan = 8;
                }    
                timelamcham = timetan;
                //debug.Log("Khổng tước " + saorong + " sao, tỉ lệ làm chậm: " + tilelamcham + " time làm chậm: " + timelamcham);
                for (int i = 1; i < teamdich.transform.childCount; i++)
                {
                    if (Random.Range(0, 100) < tilelamcham)
                    {
                        DragonPVEController chisoo = teamdich.transform.GetChild(i).transform.Find("SkillDra").GetComponent<DragonPVEController>();
                        chisoo.MatMau(saorong * 7, this);
                        dataLamCham data = new dataLamCham(timetan, "caylamcham");
                        chisoo.LamChamABS(data);
                    }
                }
                if (teamdich.name == "TeamDo")
                {
                    VienChinh.vienchinh.HienIconSkill(timelamcham, "Xanh", "iconKhongTuocXanh");
                }
                else
                {
                    VienChinh.vienchinh.HienIconSkill(timelamcham, "Do", "iconKhongTuocDo");
                }
                _HutHp = hutmaubandau;
               
                //skillObj[0].transform.position = transform.position;
                //skillObj[0].SetActive(true);
            }
            else nameskillsd = "Attack_1";
        }
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
