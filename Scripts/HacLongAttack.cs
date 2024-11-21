using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HacLongAttack : DragonPVEController
{
    public float timeCDskill = 12;
    private byte hoisinh = 0;
    private bool CuongHoa = false;
    private float timeCuongNo;private float MaxtimeCuongNo = 5;

    public bool BiDong = false;

    private void AutoKichHoatSkill()
    {
        string nameskill = "";
        float[] probabilities = { 0.4f, 0.3f, 0.3f }; // Xác suất: 40%, 40%, 20%
        float randomPoint = Random.value; // Random từ 0.0 đến 1.0

        if (randomPoint < probabilities[0]) // 40%
        {
            nameskill = "CuongNo";
        }
        else if (randomPoint < probabilities[0] + probabilities[1]) // 30%
        {
            nameskill = "HapHuyet";
        }
        else // 30%
        {
            nameskill = "DoatMenh";
        }
    }
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
 
        if(team == Team.TeamXanh)
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
        Debug.Log("Cuồng nộ");
        setAnim = false;
        walking = false;
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
        walking = true;
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
    public void DoatMenhOk()
    {
        if(TargetDoatMenh != null)
        {
            DragonPVEController dra = TargetDoatMenh.transform.Find("SkillDra").GetComponent<DragonPVEController>();
            dra.ImgHp.fillAmount = -1;
            ReplayData.addHp(dra.transform.parent.name, "-1");
            Debug.Log("Doat menh rong " + dra.nameobj);
            dra.Died();
        }
    }
    private void UseSkill()
    {
        VienChinh.vienchinh.timeskill[2] = timeCDskill;
    }
}
