using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class HacLongAttack : DragonPVEController
{
    public float timeCDskill = 12;
    private byte hoisinh = 0;
    private bool CuongHoa = false,isHapHuyet=false;
    private float timeCuongNo; private float MaxtimeCuongNo = 5;
    private float timeHapHuyet; private float MaxtimeHapHuyet = 5;
    private float chiMangCuongNo = 5, HutHpChoToanDoi = 25;
    public Transform TargetDoatMenh;

    byte indexskill = 0;
    Dictionary<string, Action<object[]>> methodMap;
    protected override void ABSAwake()
    {
   
    }
    public override void AbsStart()
    {
        LoadTimeCDSkill();
        methodMap = new Dictionary<string, Action<object[]>>
         {
             { "CuongNo", args => CuongNo() },
             { "HapHuyet", args => HapHuyet((bool)args[0]) },
             { "DoatMenh", args => DoatMenh((bool)args[0]) },
             { "SDDoatMenh", args => SDDoatMenh((bool)args[0]) }
         };
        for (int i = 1; i < 6; i++)
        {
            skillObj[i].GetComponent<SkillDraController>().skillmoveok += SkillMoveOk;
        }
        if (team == Team.TeamXanh)
        {
            GiaoDienPVP.ins.OSkill.SetActive(true);
            Transform oskill = GiaoDienPVP.ins.OSkill.transform.GetChild(1);


            for (int i = 0; i < oskill.transform.childCount; i++)
            {
                if (!oskill.transform.GetChild(i).gameObject.activeSelf || oskill.transform.GetChild(i).gameObject.name == "DienKienTuThan")
                {
                    oskill.transform.GetChild(i).gameObject.SetActive(true);
                    oskill.transform.GetChild(i).gameObject.name = "DienKienTuThan";
                    Image imgskill = oskill.transform.GetChild(i).GetComponent<Image>();
                    imgskill.sprite = Inventory.LoadSprite("DienKienTuThan");
                    imgskill.SetNativeSize();
                    oskill.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = imgskill.sprite;
                    oskill.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = GamIns.CatDauNgoacKep("");

                    oskill.GetChild(i).GetComponent<Button>().interactable = true;
                    oskill.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                    VienChinh.vienchinh.timeskill[i] = 0;
                    indexskill = (byte)i;
                    break;
                }

            }

        }
        else
        {
            if (VienChinh.vienchinh.chedodau != CheDoDau.Online)
            {
                BiDong = true;
            }
        }
        LoadAllChiSo();
        StartBiDong();
    }
    private void LoadTimeCDSkill()
    {
        if (saorong <= 15) timeCDskill = 43;
        else if (saorong == 16) timeCDskill = 42;
        else if (saorong == 17) timeCDskill = 41;
        else if (saorong == 18) timeCDskill = 40;
        else if (saorong == 19) timeCDskill = 36;
        else if (saorong == 20) timeCDskill = 35;
        else if (saorong == 21) timeCDskill = 34;
        else if (saorong == 22) timeCDskill = 33;
        else if (saorong == 23) timeCDskill = 32;
        else if (saorong == 24) timeCDskill = 28;
        else if (saorong == 25) timeCDskill = 27;
        else if (saorong == 26) timeCDskill = 26;
        else if (saorong == 27) timeCDskill = 25;
        else if (saorong == 28) timeCDskill = 24;
        else if (saorong == 29) timeCDskill = 23;
        else if (saorong == 30) timeCDskill = 20;
    }
    private void LoadAllChiSo()
    {
        ChiSoCuongNo();
        ChiSoHapHuyet();
    }
    private void ChiSoCuongNo()
    {
        if(saorong >= 21 && saorong <= 25)
        {
            MaxtimeCuongNo = 6;
        }
        else if(saorong >= 26 && saorong <= 29)
        {
            MaxtimeCuongNo = 7;
        }
        else if(saorong >= 30)
        {
            MaxtimeCuongNo = 8;
        }

        if (saorong == 17)
        {
            chiMangCuongNo = 6;
        }
        else if(saorong == 18)
        {
            chiMangCuongNo = 7;
        }
        else if(saorong == 19)
        {
            chiMangCuongNo = 8;
        }
         else if(saorong == 20)
        {
            chiMangCuongNo = 9;
        }
         else if(saorong == 21)
        {
            chiMangCuongNo = 11;
        }
         else if(saorong == 22)
        {
            chiMangCuongNo = 12;
        }
         else if(saorong == 23)
        {
            chiMangCuongNo = 13;
        }
         else if(saorong == 24)
        {
            chiMangCuongNo = 14;
        }
          else if(saorong == 25)
        {
            chiMangCuongNo = 15;
        }
        else if(saorong == 26)
        {
            chiMangCuongNo = 17;
        }
        else if(saorong == 27)
        {
            chiMangCuongNo = 18;
        }
        else if(saorong == 28)
        {
            chiMangCuongNo = 19;
        }
         else if(saorong == 29)
        {
            chiMangCuongNo = 20;
        }
         else if(saorong >= 30)
        {
            chiMangCuongNo = 22;
        }
    }
    private void ChiSoHapHuyet()
    {
        MaxtimeHapHuyet = MaxtimeCuongNo;
        if(saorong == 16)
        {
            HutHpChoToanDoi = 30;
        }
         if(saorong == 17)
        {
            HutHpChoToanDoi = 32;
        }
        else if(saorong == 18)
        {
            HutHpChoToanDoi = 34;
        }
        else if(saorong == 19)
        {
            HutHpChoToanDoi = 36;
        }
         else if(saorong == 20)
        {
            HutHpChoToanDoi = 38;
        }
         else if(saorong == 21)
        {
            HutHpChoToanDoi = 41;
        }
         else if(saorong == 22)
        {
            HutHpChoToanDoi = 43;
        }
         else if(saorong == 23)
        {
            HutHpChoToanDoi = 45;
        }
         else if(saorong == 24)
        {
            HutHpChoToanDoi = 47;
        }
          else if(saorong == 25)
        {
            HutHpChoToanDoi = 49;
        }
        else if(saorong == 26)
        {
            HutHpChoToanDoi = 53;
        }
        else if(saorong == 27)
        {
            HutHpChoToanDoi = 55;
        }
        else if(saorong == 28)
        {
            HutHpChoToanDoi = 57;
        }
         else if(saorong == 29)
        {
            HutHpChoToanDoi = 59;
        }
         else if(saorong >= 30)
        {
            HutHpChoToanDoi = 65;
        }
        
    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void Choang(float giay = 0.4F)
    {
        // base.Choang(giay);
    }
    protected override void Updatee()
    {
        //if (Input.GetKeyUp(KeyCode.J))
        //{
        //    if(Random.Range(0,100) > 50) CuongNo();

        //}    
        if (CuongHoa)
        {
            timeCuongNo -= Time.deltaTime;
            if (timeCuongNo <= -1)
            {
                CuongHoa = false;
                _ChiMang -= chiMangCuongNo;
            }
        }
        if (isHapHuyet)
        {
            timeHapHuyet -= Time.deltaTime;
            if (timeHapHuyet <= -1)
            {
                isHapHuyet = false;
                VienChinh.vienchinh.SetBuffHutHpall(-HutHpChoToanDoi, team);
                _HutHp = maxhuthp;
                //Transform TeamTf = null;
                //if (team == Team.TeamDo) TeamTf = VienChinh.vienchinh.TeamDo.transform;
                //else TeamTf = VienChinh.vienchinh.TeamXanh.transform;

                //for (int i = 1; i < TeamTf.transform.childCount; i++)
                //{
                //    Transform skilldra = TeamTf.transform.GetChild(i).transform.Find("SkillDra");
                //    if (skilldra != null)
                //    {
                //        DragonPVEController dra = skilldra.GetComponent<DragonPVEController>();
                //        dra._HutHp = maxhuthp;
                //    }
                //}

            }
        }
        UpdateBiDong();
        // if (skillObj[6].activeSelf)
    }
    public override void SetHp(float fillhp)
    {
        SetHpDefault(fillhp);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs)
    {
        //MatMauDefault(maumat, cs);
        if (GetHpTru(maumat, cs) <= 0)
        {
            if (hoisinh < 2)
            {
                hoisinh += 1;
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
            //dataLamCham data = new dataLamCham(5, "caylamcham");

            //  dra.LamChamABS(data);
        }
        else
        {
            KillTru();
        }
    }
    public override void ABSAnimatorRun()
    {
         if(!setAnim) return;
        animplay = "Flying";
    }
    public override void ABSAnimatorAttack()
    {
         if(!setAnim) return;
        if (stateAnimAttack < maxStateAttack)
        {
            if (CuongHoa) animplay = "Attack2";
            else animplay = "Attack";
            if (BiDong)
            {
                KichHoatCuongNoStart();
            }
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
    public override void LamChamABS(dataLamCham data)
    {
        // LamChamDefault(data);
        if(data.tangtoc != "0")
        {
            data.eff = "";
           // data.chia = 1;
            //data.time = 0;
         //   data.setSpeedanim = true;
         //   data.setSpeedrun = true;
            LamChamDefault(data);
        }    
 
    }
    public override void DayLuiABS()
    {
        DayLuiDefault();
    }
    public override void AbsUpdateAnimAttack()
    {
        if (CuongHoa)
        {
            //if (stateAnimAttack % 2 ==0)
            //{
            //    StartCoroutine(VienChinh.vienchinh.Shake(0.12f,0.06f));
            //}    

            for (int i = 2; i < 6; i++)
            {
                if (!skillObj[i].gameObject.activeSelf)
                {
                    if (Target == null) return;
                    ReplayData.AddAttackTarget(transform.parent.name, i.ToString(), "dungdau");
                    skillObj[i].SetActive(true);
                    break;
                }
            }

        }
        else
        {

            if (stateAnimAttack == 1 || stateAnimAttack == 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (!skillObj[i].gameObject.activeSelf)
                    {
                        if (Target == null) return;
                        ReplayData.AddAttackTarget(transform.parent.name, i.ToString(), "dungdau");
                        skillObj[i].SetActive(true);
                        break;
                    }
                }
            }
        }

    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
    public void CuongNo()
    {
        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online)
        //{
        //    //  debug.Log("set lam chammmmm");
        //    JSONObject newjson = new JSONObject();
        //    newjson.AddField("id", idrong);
        //    newjson.AddField("func", "CuongNo");
        //    DauTruongOnline.ins.AddUpdateData(newjson,true);
        //    return;
        //}
        battu = true;
       // Debug.Log("Cuồng nộ");
        setAnim = false;
   //     walking = false;
        animplay = "CuongNo";
        anim.Play("CuongNo");
        
        CuongHoa = true;
        stateAnimAttack = 0;
        timeCuongNo = MaxtimeCuongNo;
        UseSkill();
        Shake();
        _ChiMang += chiMangCuongNo;
        GiaoDienPVP.ins.SetPanelToi = true;
    }
    public static void Shake()
    {
        VienChinh.vienchinh.StartCoroutine(Shakee());
        IEnumerator Shakee()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.1f);
                VienChinh.vienchinh.StartCoroutine(VienChinh.vienchinh.Shake());
            }
              GiaoDienPVP.ins.SetPanelToi = false;
        }
    }

    public void UpdateAnimCuongNo()
    {
      //  Debug.Log("UpdateAnimCuongNo");
        setAnim = true;
        //   walking = true;
        battu = false;
        animplay = "Flying";
        anim.Play(animplay);
    }
    public void HapHuyet(bool setonline)
    {
        if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !setonline)
        {
            //  debug.Log("set lam chammmmm");
            JSONObject newjson = new JSONObject();
            newjson.AddField("id", idrong);
            newjson.AddField("func", "HapHuyet");
            DauTruongOnline.ins.AddUpdateData(newjson, true);
            return;
        }
        // Debug.Log("Hấp Huyết");

        EventManager.StartDelay2(()=>{

        StartCoroutine(VienChinh.vienchinh.CreateHieuUngSkillsBuff(team, "HapHuyetHacLong"));
        _HutHp = 100;
            //Transform TeamTf = null;
            //if(team == Team.TeamDo) TeamTf = VienChinh.vienchinh.TeamDo.transform;
            //else TeamTf = VienChinh.vienchinh.TeamXanh.transform;

            //for (int i = 1; i < TeamTf.transform.childCount; i++)
            //{
            //   Transform skilldra = TeamTf.transform.GetChild(i).transform.Find("SkillDra");
            //   if(skilldra != null)
            //   {
            //      DragonPVEController dra = skilldra.GetComponent<DragonPVEController>();
            //      dra._HutHp += HutHpChoToanDoi;
            //   }
            //}

        VienChinh.vienchinh.SetBuffHutHpall(HutHpChoToanDoi,team);
        timeHapHuyet = MaxtimeHapHuyet;
        isHapHuyet = true;
        if (team == Team.TeamXanh)
        {
            VienChinh.vienchinh.HienIconSkill(timeHapHuyet, "Xanh", "iconHapHuyetXanh");
        }
        else
        {
            //  ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
            VienChinh.vienchinh.HienIconSkill(timeHapHuyet, "Do", "iconHapHuyetDo");
        }

        UseSkill();
            },1f);
       
    }
    public void DoatMenh(bool setonline = false)
    {
        EventManager.StartDelay2(() => { SDDoatMenh(setonline); }, 1.2f);
    }

    private void SDDoatMenh(bool setonline = false)
    {
        if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !setonline)
        {
            //  debug.Log("set lam chammmmm");
            JSONObject newjson = new JSONObject();
            newjson.AddField("id", idrong);
            newjson.AddField("func", "SDDoatMenh");
            DauTruongOnline.ins.AddUpdateData(newjson, true);
            return;
        }
        if (VienChinh.vienchinh.chedodau == CheDoDau.Replay) return;
        Debug.Log("Đoạt mệnh");
        Transform strongestDragonTransform = PVEManager.GetRongManhNhat(team);
        if (strongestDragonTransform != null)
        {
            ReplayData.AddAttackTarget(transform.parent.name, "6", "dungdau");
            TargetDoatMenh = strongestDragonTransform;
            skillObj[6].SetActive(true);
        }
        else
        {
            Debug.Log("Không tìm thấy con rồng nào đáp ứng điều kiện.");
        }
        UseSkill();
    }
    protected override void ClearSkill()
    {
        for (int i = 0; i < skillObj.Length - 1; i++)// trừ skill đoạt mệnh ra
        {
            if (!skillObj[i].activeSelf)
            {
                Destroy(skillObj[i].gameObject);
            }
        }
    }
    public void DoatMenhOk()
    {
        if (VienChinh.vienchinh.chedodau == CheDoDau.Replay) return;
       debug.Log("DoatMenhOk, TargetDoatMenh: " + TargetDoatMenh);
        if (TargetDoatMenh != null)
        {
            DragonPVEController dra = TargetDoatMenh.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;

            Debug.Log("Doat menh rong " + dra.nameobj);
            if (VienChinh.vienchinh.chedodau == CheDoDau.VienChinh ||
                VienChinh.vienchinh.chedodau == CheDoDau.BossTG ||
                VienChinh.vienchinh.chedodau == CheDoDau.XucTu ||
                VienChinh.vienchinh.chedodau == CheDoDau.Halloween ||
                VienChinh.vienchinh.chedodau == CheDoDau.LanSu
                )
            {
                dra.MatMau(9999, this);
            }
            else
            {
                dra.ImgHp.fillAmount = -1;
                ReplayData.addHp(dra.transform.parent.name, "-1");
                dra.Died();
            }

        }
    }
    private void UseSkill()
    {
        if(team == Team.TeamDo) return;
      //  if(VienChinh.vienchinh.chedodau == CheDoDau.Online)
        VienChinh.vienchinh.timeskill[indexskill] = timeCDskill;
    }

    //private void OnDisable()
    //{
    //    if (VienChinh.vienchinh.chedodau != CheDoDau.Online) return;
    //    Debug.Log("Doat Menh Onlineeee");
    //    SDDoatMenh();
    //}

    protected override void Die()
    {

        SDDoatMenh();
        Transform parent = transform.parent;
        parent.gameObject.SetActive(false);
        parent.SetParent(VienChinh.vienchinh.ObjSkill.transform);
     //   VienChinh.vienchinh.SetMucTieuTeamDo();
     //   VienChinh.vienchinh.SetMucTieuTeamXanh();
        HapHuyetBiDong(-1);

        Transform oskill = GiaoDienPVP.ins.OSkill.transform.GetChild(1);

        if(team == Team.TeamXanh)
        {
            for (int i = 0; i < oskill.transform.childCount; i++)
            {
                if (oskill.transform.GetChild(i).gameObject.name == "DienKienTuThan")
                {
                    oskill.transform.GetChild(i).gameObject.SetActive(false);

                    break;
                }

            }

        }

        Destroy(parent.gameObject, 5);
     

        // base.Die();
    }
    public override void FuncInvokeOnline(string namefunc, params object[] parameters)
    {
        if (methodMap.ContainsKey(namefunc))
        {
            debug.Log("FuncInvokeOnline " + namefunc);
            methodMap[namefunc].Invoke(parameters);
        }
        else
        {
            debug.LogError($"Method '{namefunc}' not found.");
        }
    }
    public override void PlayAnimReplay(string s)
    {
        base.PlayAnimReplay(s);
        if(s == "CuongNo")
        {
            GiaoDienPVP.ins.SetPanelToi = true;
            Shake();
        }
    } 
}
