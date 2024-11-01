using Random = UnityEngine.Random;
public class RongTuanLongAttack : DragonPVEController
{
    public override void AbsStart()
    {
        //  debug.Log("AbsStart");
        string team = transform.parent.transform.parent.name;
        if (team == "TeamXanh")
        {
            VienChinh.vienchinh.HienIconSkill(300, "Xanh", "iconKhienTuanLongXanh");
            VienChinh.vienchinh.SetBuffGiapall(saorong, team);
        }
        else
        {
          //  ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
            VienChinh.vienchinh.HienIconSkill(300, "Do", "iconKhienTuanLongDo");
            VienChinh.vienchinh.SetBuffGiapall(saorong, team);
        }
    }
    protected override void Updatee()
    {

    }
    public override void SetHp(float fillhp,bool setonline = false)
    {
        SetHpDefault(fillhp, setonline);
    }
    public override void SetHpOnline(JSONObject data)
    {
        hp = float.Parse(data["hp"].str);
        ImgHp.fillAmount = hp / Maxhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        delaytatthanhmau();
    }
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        MatMauDefault(maumat, cs, setonline);
    }
    public override void ChoangABS(float giay = 0.2f)
    {
        ChoangDefault(giay);
    }
    public override void LamChamABS(dataLamCham data)
    {
        data.eff = "";
        data.chia = 2;
        LamChamDefault(data);
    }
    protected override void ABSAwake()
    {

    }
    public override void DayLuiABS()
    {
        
    }
    public override void SkillMoveOk()
    {
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
        animplay = "Idlle";
    }
    public override void AbsUpdateAnimAttack()
    {
        if (stateAnimAttack == 1)
        {
            ReplayData.AddAttackTarget(transform.parent.name, "0", "dungdau");
            // skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        string team = transform.parent.transform.parent.name;
        if (team == "TeamDo")
        {
            VienChinh.vienchinh.TaticonSkill("iconKhienTuanLongDo", "Do");
            VienChinh.vienchinh.SetBuffGiapall(-saorong, team);
        }
        else
        {
            VienChinh.vienchinh.TaticonSkill("iconKhienTuanLongXanh", "Xanh");
            VienChinh.vienchinh.SetBuffGiapall(-saorong, team);
        }
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
