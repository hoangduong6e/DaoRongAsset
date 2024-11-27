using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RongNguSacAttack : DragonPVEController
{
    //private byte danh = 1;
    float maxtimetrieuhoinew = 0, maxtimesong;
    float timetrieuhoinew = -1;
    float[] timesong = new float[] {0,0};
    public Transform[] NamNguSac;
    byte solantrieuhoi = 0;
    string namenam = "NamNguSac1";
    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
        debug.Log("AbsStart");
    }
    private void TrieuHoiNamNguSac()
    {
        float hpnam = 0;
        float satthuongnam = 0;
        if (saorong <= 20)
        {
            namenam = "NamNguSac1";
            hpnam = Maxhp * 50 / 100;
            maxtimetrieuhoinew = 10;
        }
        else if (saorong <= 25)
        {
            namenam = "NamNguSac2";
            hpnam = Maxhp * 75 / 100;
            maxtimetrieuhoinew = 9;
        }
        else
        {
            namenam = "NamNguSac3";
            hpnam = Maxhp * 90 / 100;
            maxtimetrieuhoinew = 8;
        }
        if (saorong <= 15)
        {
            satthuongnam = dame;
        }
        else if (saorong <= 22) satthuongnam = dame * 2;
        else if (saorong <= 25) satthuongnam = dame * 3;
        else if (saorong <= 25) satthuongnam = dame * 3;
        else if (saorong <= 28) satthuongnam = dame * 4;
        else if (saorong == 29) satthuongnam = dame * 5;
        else if (saorong >= 30) satthuongnam = dame * 6;


        if (saorong <= 12) maxtimesong = 3;
        else if (saorong <= 15) maxtimesong = 4;
        else if (saorong <= 18) maxtimesong = 5;
        else if (saorong <= 22) maxtimesong = 6;
        else if (saorong <= 26) maxtimesong = 7;
        else if (saorong <= 29) maxtimesong = 8;
        else maxtimesong = 9;
        JSONObject data = new JSONObject();
        JSONObject chiso = new JSONObject();
        JSONObject chisoget = new JSONObject();

        // Thêm các giá trị vào chiso
        chiso.AddField("hp", hpnam);               // Chỉ số HP
        chiso.AddField("sucdanh", satthuongnam);           // Sức đánh
        chiso.AddField("hutmau", 0);             // Hút máu
        chiso.AddField("netranh", 0);            // Né tránh
        chiso.AddField("tilechimang", 0);         // Tỉ lệ chí mạng
        chiso.AddField("giapso", giapso);            // Giáp số
        chiso.AddField("giapphantram", giapphantram + 30);       // Giáp phần trăm
        chiso.AddField("xuyengiap", xuyengiap);          // Xuyên giáp
        giapphantram += 30;
        // Thêm các giá trị vào chisoget
        chisoget.AddField("tocchay", 2.5f);         // Tốc chạy
        chisoget.AddField("tamdanhxa", 5);       // Tầm đánh xa

        // Thêm các giá trị chính vào data
        data.AddField("nameobject", namenam);    // Tên đối tượng
        data.AddField("id", namenam +"-" + transform.parent.name+"-" + solantrieuhoi);    // Tên đối tượng
        data.AddField("sao", saorong);                     // Số sao của rồng
        data.AddField("chiso", chiso);               // Chỉ số của rồng
        data.AddField("chisoget", chisoget);         // Chỉ số bổ sung
        float congx = -1.5f;
        if(team == Team.TeamXanh)
        {
            congx = 1.5f;
        }    
        GameObject Rongtrieuhoi = PVEManager.GTrieuHoiDra(data, team.ToString(),new Vector3(transform.position.x + congx, transform.position.y+Random.Range(-0.5f,0.5f),transform.position.y));
        Rongtrieuhoi.transform.Find("SkillDra").GetComponent<NamNguSacAttack>().NguSacAttack = this;
        //if (NamNguSac[0] == null) // làm cho con mới luôn luôn ở đầu tie
        //{
        //    NamNguSac[0] = Rongtrieuhoi.transform;
        //}
        //else
        //{
        //    NamNguSac[1] = NamNguSac[0];
        //    NamNguSac[0] = Rongtrieuhoi.transform;
        //}
        for (int i = 0; i < NamNguSac.Length; i++)
        {
            if (NamNguSac[i] == null)
            {
                NamNguSac[i] = Rongtrieuhoi.transform;
                break;
            }
        }
        solantrieuhoi += 1;
        debug.Log("trieu hoiii");
      //  Rongtrieuhoi.transform.Find("SkillDra").transform.position = transform.position;
      //  Rongtrieuhoi.transform.position = transform.position;
    }

    public override void ChoangABS(float giay = 0.2f)
    {
        ChoangDefault(giay);
    }
    protected override void Updatee()
    {
        // transform.position = transform.parent.position;
        timetrieuhoinew += Time.deltaTime;
        for (int i = 0; i < NamNguSac.Length; i++)
        {
   

            if (NamNguSac[i] == null)
            {
                if (timetrieuhoinew > maxtimetrieuhoinew)
                {
                    TrieuHoiNamNguSac();
                    timetrieuhoinew = 0;
                }
            }
            else
            {
                timesong[i] += Time.deltaTime;
                if (timesong[i] > maxtimesong)
                {
                    NamNguSac[i].transform.Find("SkillDra").GetComponent<DragonPVEController>().MatMau(99999999, null);
                    GiaoDienPVP.ins.AddHienRong(namenam, -1);
                    timesong[i] = 0;
                }

            }
        }
      
    
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
            DragonPVEController chisodich = Target.transform.Find("SkillDra").GetComponent<DragonPVEController>();
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
        animplay = "Win";

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
        BienCuuDefault(time);
    }
}
