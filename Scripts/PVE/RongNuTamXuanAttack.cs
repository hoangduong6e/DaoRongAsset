
using UnityEngine;

public class RongNuTamXuanAttack : DragonPVEController
{
    private byte danh = 1;
    private float nhandame = 12;
    protected override void ABSAwake()
    {

    }
    public override void AbsStart()
    {
        Transform tf = transform.parent.transform.GetChild(0).transform.Find("bong").transform.GetChild(0).transform;
        Vector3 vecbandau = tf.transform.position;
        tf.transform.position = new Vector3(vecbandau.x, vecbandau.y - 1, vecbandau.z);
        if(saorong >= 16 && saorong <= 20) nhandame = 14;
        else if(saorong >= 21 && saorong <= 25) nhandame = 16;
        else if(saorong >= 26 && saorong <= 29) nhandame = 18;
        else if(saorong >= 30) nhandame = 20;
    }
    protected override void Updatee()
    {

    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void SetHp(float fillhp)
    {
        SetHpDefault(fillhp);
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs)
    {
        MatMauDefault(maumat, cs);
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
            DragonPVEController chisodich = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;

            if (danh < 3) danh += 1;
            else
            {
                danh = 1;
                debug.Log("damee x"+nhandame);
                damee *= nhandame;
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

      //  animplay = "Attack";
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
       // if (stateAnimAttack == maxStateAttack) stateAnimAttack = 0;
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
    private void OnDestroy()
    {
      
    }
}
