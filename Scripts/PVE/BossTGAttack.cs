
using System;

public class BossTGAttack : DragonPVEController
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
    public override void SetHpOnline(JSONObject data)
    {
       
    }
    private Action actionUpdateAnimAttack, actionMoveSkillok;

    protected override void ABSAwake()
    {
        //    anim = transform.parent.GetComponent<Animator>();
        transform.parent.GetComponent<DraUpdateAnimator>().DragonPVEControllerr = this;
        PVEManager.GetUpdateMove(transform, "TeamDo");

        actionMoveSkillok += SkillMoveOkk;
        for (int i = 0; i < skillObj.Length; i++)
        {
            skillObj[i].GetComponent<SkillDraController>().skillmoveok += actionMoveSkillok;
        }
        actionUpdateAnimAttack += AbsUpdateAnimAttackk;
        thongke = false;
    }
    public override void AbsStart()
    {


    }

    protected override void Updatee()
    {

    }
    public override void DayLuiABS()
    {
       // DayLuiDefault();
    }
    public override void SetHp(float fillhp, bool setonline = false)
    {
        SetHpDefault(fillhp, setonline);
    }
  //  float tongdame = 0;
    public override void AbsMatMau(float maumat, DragonPVEController cs, bool setonline = false)
    {
        //tongdame += maumat; 
        //  UnityEngine.debug.Log(cs.nameobj + " đánh, dame: " + maumat + " tổng dame: " + tongdame);
        ThongKeDame.AddThongKe(new ThongKeDame.CData(cs.team.ToString(), cs.nameobj, cs.idrong, maumat, ThongKeDame.EType.dame));
        NetworkManager.ins.socket.Emit("DanhBossTG", JSONObject.CreateStringObject(maumat.ToString()));
     //   MatMauDefault(maumat, cs);
    }


    public override void ABSAnimatorRun()
    {
        animplay = "Idlle";
    }
    public override void LamChamABS(dataLamCham data)
    {
       // LamChamDefault(data);
    }
    public override void ABSAnimatorAttack()
    {
        animplay = "Attack";
        //if (stateAnimAttack < maxStateAttack)
        //{
        //    animplay = "Attack";
        //}
        //else
        //{
        //    animplay = "Idlle";
        //}
    }
    public override void AbsUpdateAnimIdle()
    {

    }
    public override void ABSAnimWin()
    {
        //for (int i = 0; i < skillObj.Length; i++)
        //{
        //    skillObj[i].GetComponent<SkillDraController>().skillmoveok -= actionMoveSkillok;
        //}
        animplay = "Idlle";

    }
    public override void AbsUpdateAnimAttack()
    {
        //    actionUpdateAnimAttack();
        AbsUpdateAnimAttackk();
    }
    public override void SkillMoveOk()
    {
        //   actionMoveSkillok();
    }
    private void AbsUpdateAnimAttackk()
    {
        if (stateAnimAttack < 3)
        {
            if (Target == null) return;
            // ReplayData.AddAttackTarget(transform.parent.name, 0.ToString(), "dungdau");
            skillObj[0].transform.position = transform.position;
            skillObj[0].SetActive(true);
        }
        else stateAnimAttack = 0;

    }
    private void SkillMoveOkk()
    {
       // debug.Log("Target.name " + Target.name);
        if (Target.name != "trudo" && Target.name != "truxanh")
        {
            DragonPVEController dra = Target.GetComponent<DraUpdateAnimator>().DragonPVEControllerr;
            dra.MatMau(99999, this);
        }
        else
        {
            KillTru();
            debug.Log("kill tru");
        }
        //List<Transform> ronggan = new List<Transform>(PVEManager.GetDraDungTruoc(3, Target.transform.parent.transform, new Vector2(3, 2)));
        //float damee = dame;
        //if (UnityEngine.Random.Range(1, 100) <= chimang)
        //{
        //    damee *= 5;
        //    PVEManager.InstantiateHieuUngChu("chimang", transform);
        //}
        //for (int i = 0; i < ronggan.Count; i++)
        //{
        //    if (ronggan[i].name != "trudo" && ronggan[i].name != "truxanh")
        //    {
        //        DragonPVEController dra = ronggan[i].GetComponent<DraUpdateAnimator>().DragonPVEControllerr;

        //        dra.MatMau(damee, this);
        //        //  if (daylui) dra.DayLuiABS();
        //    }
        //    else
        //    {
        //        KillTru();
        //    }
        //}
    }
    public override void BienCuuABS(float time)
    {
        BienCuuDefault(time);
    }
}
