
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HacLongAttack : DragonPVEController
{

    private byte hoisinh = 0;
    private bool CuongHoa = false;
    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
        for (int i = 1; i < 6; i++)
        {
            skillObj[i].GetComponent<SkillDraController>().skillmoveok += SkillMoveOk;
        }
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
        if (Input.GetKeyUp(KeyCode.J))
        {
            if(Random.Range(0,100) > 50) CuongNo();

        }    
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
        setAnim = false;
        walking = false;
        animplay = "CuongNo";
        anim.Play("CuongNo");
        CuongHoa = true;
        stateAnimAttack = 0;
    }
    public void UpdateAnimCuongNo()
    {
        Debug.Log("UpdateAnimCuongNo");
        setAnim = true;
        walking = true;
        anim.Play(animplay);
    }

}
