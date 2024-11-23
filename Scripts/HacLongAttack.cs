using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public partial class HacLongAttack : DragonPVEController
{
    public float timeCDskill = 12;
    private byte hoisinh = 0;
    private bool CuongHoa = false;
    private float timeCuongNo;private float MaxtimeCuongNo = 5;

    public Transform TargetDoatMenh;
    protected override void ABSAwake()
    {
        
    }
    public override void AbsStart()
    {
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
                if (!oskill.transform.GetChild(i).gameObject.activeSelf)
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
                    break;
                }

            }
        }
        else
        {
            if(VienChinh.vienchinh.chedodau != CheDoDau.Online)
            {
                BiDong = true;
            }
        }

        StartBiDong();
       // if (VienChinh.vienchinh.timeskill[3] == 0)
      
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
    public override void ChoangABS(float giay = 0.2f)
    {
       // ChoangDefault(giay);
    }
    protected override void Updatee()
    {
        //if (Input.GetKeyUp(KeyCode.J))
        //{
        //    if(Random.Range(0,100) > 50) CuongNo();

        //}    
        if(CuongHoa)
        {
            timeCuongNo -= Time.deltaTime;
            if(timeCuongNo <= -1)
            {
                CuongHoa = false;
            }    
        }
        UpdateBiDong();
       // if (skillObj[6].activeSelf)
    }
    public override void SetHp(float fillhp, bool setonline = false)
    {
        SetHpDefault(fillhp, setonline);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        //MatMauDefault(maumat, cs);
        if (GetHpTru(maumat, cs, setonline) <= 0)
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
        if (Random.Range(1, 100) <= chimang)
        {
            damee *= 5;
            PVEManager.InstantiateHieuUngChu("chimang", transform);
        }

        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController dra = Target.transform.Find("SkillDra").GetComponent<DragonPVEController>();
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
        animplay = "Flying";
    }
    public override void ABSAnimatorAttack()
    {
        if (stateAnimAttack < maxStateAttack)
        {
            if (CuongHoa) animplay = "Attack2";
            else animplay = "Attack";
            if(BiDong)
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
        animplay = "Flying";
    }
    public override void LamChamABS(dataLamCham data)
    {
       // LamChamDefault(data);
        data.eff = "";
        data.chia = 2;
        LamChamDefault(data);
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

        if (DauTruongOnline.GetUpdateDra)
        {
            //  debug.Log("set lam chammmmm");
            JSONObject newjson = new JSONObject();
            newjson.AddField("id", idrong);
            newjson.AddField("func", "CuongNo");
            DauTruongOnline.ins.AddUpdateData(newjson);
            return;
        }

        Debug.Log("Cuồng nộ");
        setAnim = false;
   //     walking = false;
        animplay = "CuongNo";
        anim.Play("CuongNo");
        CuongHoa = true;
        stateAnimAttack = 0;
        timeCuongNo = MaxtimeCuongNo;
        UseSkill();
    }
    public void UpdateAnimCuongNo()
    {
        Debug.Log("UpdateAnimCuongNo");
        setAnim = true;
     //   walking = true;
        anim.Play(animplay);
    }
    public void HapHuyet()
    {
        Debug.Log("Hấp Huyết");
        StartCoroutine(VienChinh.vienchinh.CreateHieuUngSkillsBuff(team, "HapHuyetHacLong"));
        UseSkill();
    }

    public void DoatMenh()
    {
       Invoke("SDDoatMenh",1.2f);
    }


    private void SDDoatMenh()
    {
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
        if (!DauTruongOnline.GetUpdateDra) return;
        if(TargetDoatMenh != null)
        {
            DragonPVEController dra = TargetDoatMenh.transform.Find("SkillDra").GetComponent<DragonPVEController>();
    
         
            Debug.Log("Doat menh rong " + dra.nameobj);
            if(VienChinh.vienchinh.chedodau == CheDoDau.VienChinh ||
                VienChinh.vienchinh.chedodau == CheDoDau.BossTG ||
                VienChinh.vienchinh.chedodau == CheDoDau.XucTu ||
                VienChinh.vienchinh.chedodau == CheDoDau.XucTu)
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
        VienChinh.vienchinh.timeskill[2] = timeCDskill;
    }

    private void OnDestroy()
    {
        SDDoatMenh();
    }

    protected override void Die()
    {
        SDDoatMenh();
        Transform parent = transform.parent;
        parent.gameObject.SetActive(false);
        parent.SetParent(VienChinh.vienchinh.ObjSkill.transform);
        Destroy(parent.gameObject, 5);
       // base.Die();
    }
    public override void FuncInvokeOnline(string namefunc)
    {
        Invoke(namefunc,0);
    }
}
