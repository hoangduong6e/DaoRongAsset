﻿using EZ_Pooling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class dataLamCham
{
    public float time;
    public string eff = "";
    public float chia = 2;
    public string tangtoc = "0";
    public bool setOnline = false;
    public bool set = false;
    public bool setSpeedrun = true;
    public bool setSpeedanim = true;
    public dataLamCham(float Time, string Eff = "", float Chia = 2, string TangToc = "0", bool SetOnline = false, bool Set = false,bool SpeedRun = true, bool SpeedDanh = true)
    {
        time = Time;
        eff = Eff;
        chia = Chia;
        tangtoc = TangToc;
        setOnline = SetOnline;
        set = Set;

        setSpeedrun = SpeedRun;
        setSpeedanim = SpeedDanh;
    }
    
}
public abstract class DragonPVEController : MonoBehaviour
{
    // Start is called before the first frame update
    public Action actionUpdate { get; set; }
    public float dame = 500,
        hp = 2000,
        Maxhp = 2000,
        speed = 2,
        tamdanhxa,
        netranh = 0,
        maxhuthp = 0,
        giapso = 0,
        giapphantram = 0,
        xuyengiap = 0,
        hpgiap = 0,
        maxhpgiap = 0;
    // protected bool battu;



    [SerializeField]
    private float huthp = 0, chimang = 0;
    //public string animIdlle;
    public float _HutHp
    {
        get
        {
            if (team == Team.TeamXanh) return huthp + VienChinh.vienchinh.buffhuthpallxanh;
            else return huthp + VienChinh.vienchinh.buffhuthpalldo;
        }
        set
        {
            huthp = value;
            if (huthp > 100)
            {
                huthp = 100;
            }
        }
    }
    public float _ChiMang
    {
        get
        {
            return chimang;
        }
        set
        {
            chimang = value;
            if (chimang > 100)
            {
                chimang = 100;
            }
        }
    }
    public string nameobj { get; protected set; }
    protected byte saorong;
    //public Transform dra;
    public Transform Target;
    public Animator anim;
    [SerializeField]
    protected byte stateAnimIdle = 0;
    [SerializeField]
    protected byte maxStateIdle = 1;

    protected byte stateAnimAttack = 0;

    public byte maxStateAttack = 1;

    public float maxspeed { get; set; }

    protected bool battu = false; private bool daylui = true;
    public string animplay { get; protected set; }
    //protected byte stateAnimIdle = 0, stateAnimAttack = 0;

    //   public abstract void AbsMoveComponent(SocketIOEvent e);
    public GameObject[] skillObj;
    public Image ImgHp;
    public string idrong;
    public Team team;
    public bool thongke = true;
    protected bool setAnim = true;
    public bool DanganimAttack = false;
    //private void Awake()
    //{
    //    ImgHp = ThanhMau.transform.GetChild(0).GetComponent<Image>();
    //}
    private void Awake()
    {
        Animator animm = transform.parent.GetComponent<Animator>();
        if (animm != null) anim = animm;
        idrong = transform.parent.name;
        ABSAwake();
    
    }
    private void OnDestroy()
    {
         ClearSkill();
    }
    protected virtual void ClearSkill()
    {
        for (int i = 0; i < skillObj.Length; i++)
        {
            if (!skillObj[i].activeSelf)
            {
                Destroy(skillObj[i].gameObject);
            }
        }
    }
    //private void OnEnable()
    //{


    //   // tf.transform.LeanScale(scale, 3f);
    //}

    public void SpawmComplete()
    {
        //for (int i = 0; i < skillObj.Length; i++)
        //{
        //    skillObj[i].transform.SetParent(VienChinh.vienchinh.transform);
        //}
        //    debug.Log("SpawmComplete " + gameObject.name);
        Startt();

        //StartCoroutine(delay());
        //IEnumerator delay()
        //{
        //    Transform tf = transform.parent.gameObject.transform;
        //    Vector3 scale = tf.transform.localScale;
        //    tf.transform.localScale = new Vector3(scale.x * 1.5f, scale.y * 1.5f, scale.z);
        //    yield return new WaitForSeconds(1.3f);
        //    tf.transform.localScale = scale;
        //}
    }

    protected abstract void ABSAwake();
    public void Startt()
    {
        Transform parent = transform.parent;
        // if(team == Team.TeamXanh) PVEManager.GetUpdateMove(transform, parent.transform.parent.name);
       // parent.transform.position = parent.transform.parent.transform.GetChild(0).transform.position;
          PVEManager.GetUpdateMove(transform, team);
        if (!VienChinh.vienchinh.DanhOnline) skillObj[0].GetComponent<SkillDraController>().skillmoveok += SkillMoveOk;
        
        //     ImgHp = ThanhMau.transform.GetChild(0).GetComponent<Image>();
        //  tamdanhxa += Random.Range(0, 2);
        //  speed += Random.Range(0, 1);
        AbsStart();
    }
    public abstract void AbsStart();
    protected abstract void Updatee();
    public abstract void ABSAnimatorRun();
    public void AnimatorRun()
    {
        ABSAnimatorRun();
        stateAnimAttack = 0;
        stateAnimIdle = 0;
        if(setAnim)
        {
            anim.Play(animplay);
        }
        //  ReplayData.AddStateAnimator(gameObject.name, animplay);
    }
    public Action ActionAttack;
    public void AnimatorAttack()
    {
        ABSAnimatorAttack();
        if (setAnim) anim.Play(animplay);
    }
    public abstract void ABSAnimatorAttack();

    //int testidle = 0;
    //int testattack = 0;

    public void UpdateAnimIdle()
    {
        //   debug.Log("UpdateAnimIdle " + gameObject.name);

        //if (team == Team.TeamXanh)
        //{
        //    testidle++;
        //    debug.Log(idrong + " idlle: " + testidle);
        //}

        stateAnimIdle += 1;
        if (stateAnimIdle >= maxStateIdle)
        {
            stateAnimIdle = 0;
            stateAnimAttack = 0;
        }
        AbsUpdateAnimIdle();
        if (setAnim) anim.Play(animplay);
    }
    public abstract void AbsUpdateAnimIdle();
    public void UpdateAnimAttack()
    {
        //if(team == Team.TeamXanh)
        //{
        //    testattack++;
        //    debug.Log(idrong + " attack: " + testattack);
        //}    
    

        //  debug.Log("UpdateAnimAttack " + gameObject.name);
        stateAnimAttack += 1;
        if (stateAnimAttack >= maxStateAttack)
        {
            DanganimAttack = false;
        }
        else DanganimAttack = true;
        AbsUpdateAnimAttack();

    }
    public void AnimWin()
    {
        ABSAnimWin();
        if(anim !=  null) anim.Play(animplay);

        actionUpdate = null;
        this.enabled = false;
    }    
    public abstract void ABSAnimWin();
    public abstract void AbsUpdateAnimAttack();
    void FixedUpdate()
    {
        Updatee();
        //debug.Log(team + ": " + transform.position.x);
        if (actionUpdate == null) return;
        actionUpdate();
        //ReplayData.AddPositionDragon(transform.parent.name,transform.position);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void MatMau(float maumat,DragonPVEController cs)
    {
        //if (setonline)
        //{
        //    AbsMatMau(maumat, cs, true);
        //    return;
        //}
        if (battu) return;
        if (Random.Range(1, 100) <= netranh)
        {
            //ThongKeDame.AddDame(cs.transform.parent.transform.parent.name, cs.nameobj, cs.transform.parent.name, maumat);
            if(thongke)ThongKeDame.AddThongKe(new ThongKeDame.CData(team.ToString(), nameobj,idrong, maumat, ThongKeDame.EType.chongchiu));
            if(thongke)ThongKeDame.AddThongKe(new ThongKeDame.CData(team.ToString(), nameobj,idrong, 1, ThongKeDame.EType.netranh));
            PVEManager.InstantiateHieuUngChu("netranh", transform);
            return;
        }
        AbsMatMau(maumat, cs);
    }
    //public void MatMauAction1(float maumat, DragonPVEController cs, bool setonline = false)
    //{
    //    if (setonline)
    //    {
    //        AbsMatMau(maumat, cs, true);
    //        return;
    //    }
    //    if (battu) return;
    //    if (Random.Range(1, 100) <= netranh)
    //    {
    //        //ThongKeDame.AddDame(cs.transform.parent.transform.parent.name, cs.nameobj, cs.transform.parent.name, maumat);
    //        if(thongke)ThongKeDame.AddThongKe(new ThongKeDame.CData(transform.parent.transform.parent.name, nameobj,idrong, maumat, ThongKeDame.EType.chongchiu));
    //        if(thongke)ThongKeDame.AddThongKe(new ThongKeDame.CData(transform.parent.transform.parent.name, nameobj,idrong, 1, ThongKeDame.EType.netranh));
    //        PVEManager.InstantiateHieuUngChu("netranh", transform);
    //        return;
    //    }
    //    AbsMatMau(maumat, cs, false);
    //}
    public float GetHpTru(float maumat, DragonPVEController cs)
    {
        //string color = (team == Team.TeamXanh) ? "<color=lime>" + team.ToString() + "</color>" : "<color=red>" + team.ToString() + "</color>";
     
        //if (setonline)
        //{
        //    TruMau();
        //    return hp;
        //}
        if(thongke)ThongKeDame.AddThongKe(new ThongKeDame.CData(team.ToString(), nameobj,idrong, maumat, ThongKeDame.EType.chongchiu));
        if (cs != null)
        {
            if (cs._HutHp > 0)
            {
                cs.HutMau();
            }
            if (cs.xuyengiap < giapso) maumat -= giapso - cs.xuyengiap;
        }
        //string team = gameObject.transform.parent.transform.parent.name;
        float giappt = giapphantram;

     //   debug.Log("giáp phần trăm ban đầu của " + nameobj + " " + color + " là " + giappt);
 
        if (team == Team.TeamXanh)
        {
            giappt += VienChinh.vienchinh.buffgiapallxanh;
        }
        else giappt += VienChinh.vienchinh.buffgiapalldo;
        if (giappt > 85) giappt = 85;

     //   debug.Log("giáp phần trăm sau khi cộng của " + nameobj + " " + color + " là " + giappt);
        if (giappt > 0) maumat -= maumat / 100 * giappt;
        // if (maumat < 0) maumat = 0;
        // debug.Log("máu mất " + color + " " + maumat);
        //if (!setonline)
        //{
        //    if (VienChinh.vienchinh.chedodau == CheDoDau.Online)
        //    {
        //        JSONObject newjson = new JSONObject();
        //        newjson.AddField("id",idrong);
        //        //  float fillset = (hp - maumat) / Maxhp;
        //        //   newjson.AddField("sethp", Math.Round(fillset, 2).ToString());
        //        double tru = Math.Round(hp - maumat, 2);
        //        newjson.AddField("hp", tru.ToString());
        //        hp = (float)tru;
        //        DauTruongOnline.ins.AddUpdateData(newjson);

        //        return hp;
        //    }
        //}
        // VienChinh.vienchinh.SetMucTieuTeamDo();//setmuctieuuu
        //VienChinh.vienchinh.SetMucTieuTeamXanh();

        //  debug.Log("chedodau "+ VienChinh.vienchinh.chedodau);
        //if (VienChinh.vienchinh.chedodau == "SoloKhongTuoc")
        //{
        //    if (gameObject.transform.parent.name == "TeamDo")
        //    {
        //        VienChinh.vienchinh.dameKhongTuoc += maumat;
        //    }    

        //}
        //TruMau();
        //void TruMau()
        //{

        //}

        if (cs != null)
        {
            if (cs.thongke)
            {
                ThongKeDame.AddThongKe(new ThongKeDame.CData(cs.team.ToString(), cs.nameobj, cs.idrong, maumat, ThongKeDame.EType.dame));
            }

        }
        hp -= maumat;
        float fillamount = (float)hp / (float)Maxhp;

        ImgHp.fillAmount = fillamount;
        ReplayData.addHp(transform.parent.name, ImgHp.fillAmount.ToString());
        //ImgHp.transform.parent.gameObject.SetActive(true);
        //CancelInvoke();
        //Invoke("tatthanhmau", 5);
        HienThanhHp();

        return hp;
    }
    public abstract void AbsMatMau(float maumat, DragonPVEController cs);
    public abstract void SetHp(float fillhp);
    public abstract void SetHpOnline(JSONObject data);
    protected float hs = 0;
    
    protected void MatMauDefault(float maumat, DragonPVEController cs)
    {
        //   debug.LogError("MauMat " + gameObject.transform.parent.name + ": " + maumat);
        if (GetHpTru(maumat, cs) <=0)
        {
            float tylehoisinh = VienChinh.vienchinh.tilehoisinh[team];
            if(tylehoisinh > 0)
            {
                if (hs < VienChinh.vienchinh.maxHS[team] && Random.Range(0, 100) < tylehoisinh && VienChinh.vienchinh.solanHSAll[team] < VienChinh.vienchinh.maxHSAll[team])
                {
                    battu = true;
                    PVEManager.InstantiateHieuUngChu("hoisinh", transform);
                    hp = Maxhp;
                    ImgHp.fillAmount = 1;
                    hs += 1;
                    VienChinh.vienchinh.solanHSAll[team] += 1;
                    EventManager.StartDelay2(() => { battu = false; },0.2f);
                }
                else Died();
            }
            else Died();
        }
    }
    protected void SetHpDefault(float fillhp)
    {
        // float fillamount = (float)hpp / (float)maxhp;
        ImgHp.fillAmount = fillhp;
        ImgHp.transform.parent.gameObject.SetActive(true);
        //if(!setonline)
        //{
        //    if (fillhp <= 0)
        //    {
        //        GameObject dieAnim = Instantiate(Inventory.ins.GetEffect("DieAnim"), transform.position, Quaternion.identity);
        //        dieAnim.transform.position = transform.position;
        //        Destroy(dieAnim, 0.3f);
        //        Destroy(transform.parent.gameObject);
        //    }
        //}
        if (fillhp <= 0)
        {
            GameObject dieAnim = Instantiate(Inventory.ins.GetEffect("DieAnim"), transform.position, Quaternion.identity);
            dieAnim.transform.position = transform.position;
            Destroy(dieAnim, 0.3f);
            Destroy(transform.parent.gameObject);
        }
        delaytatthanhmau();
    }
    protected void delaytatthanhmau()
    {
        CancelInvoke();
        Invoke("tatthanhmau", 5 / ReplayData.speedReplay);
    }    
    public void HutMauOnline(float conghp)
    {

        if (conghp <= 0) return;
        hp += conghp;
        if (hp > Maxhp) hp = Maxhp;
        float fillamount = (float)hp / (float)Maxhp;
        ImgHp.fillAmount = fillamount;

        Transform hutmau = EZ_PoolManager.Spawn(Inventory.ins.GetEffect("hutmau").transform, transform.position, Quaternion.identity);
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, 100);
        hutmau.transform.position = vec;
        hutmau.gameObject.SetActive(true);
        EZ_PoolManager.Despawn(hutmau, 0.45f);

        HienThanhHp();
    }
    public void HutMau()
    {
        
        float conghp = dame * _HutHp / 100;
        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online)
        //{
        //    JSONObject newjson = new JSONObject();
        //    newjson.AddField("id",idrong);
        //    newjson.AddField("hutmau", conghp.ToString());
        //    DauTruongOnline.ins.AddUpdateData(newjson);

        //    return;
        //}

       
        if (conghp <= 0) return;
        hp += conghp;
        if (thongke) ThongKeDame.AddThongKe(new ThongKeDame.CData(team.ToString(), nameobj,idrong, conghp, ThongKeDame.EType.hoiphuc));
        if (hp > Maxhp) hp = Maxhp;
        float fillamount = (float)hp / (float)Maxhp;
        ImgHp.fillAmount = fillamount;

        Transform hutmau = EZ_PoolManager.Spawn(Inventory.ins.GetEffect("hutmau").transform, transform.position, Quaternion.identity);
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, 100);
        hutmau.transform.position = vec;
        hutmau.gameObject.SetActive(true);
        EZ_PoolManager.Despawn(hutmau, 0.45f);

        //ImgHp.transform.parent.gameObject.SetActive(true);
        //CancelInvoke();
        //Invoke("tatthanhmau", 5);
        HienThanhHp();
    }
    private Coroutine hideHpBarCoroutine;
    public void HienThanhHp()
    {
        ImgHp.transform.parent.gameObject.SetActive(true);
        if (Setting.cauhinh == CauHinh.CauHinhThap) return;
        // Reset thời gian hiển thị thanh HP
        if (hideHpBarCoroutine != null)
        {
            StopCoroutine(hideHpBarCoroutine);
        }
        hideHpBarCoroutine = StartCoroutine(TatThanhMauSau(5f));
    }

    private IEnumerator TatThanhMauSau(float delay)
    {
        yield return new WaitForSeconds(delay);
        ImgHp.transform.parent.gameObject.SetActive(false);
    }
    public abstract void SkillMoveOk();
    //public class DataParseDra
    //{
    //    public string nameobj;
    //    public float hp, speed, dame, huthp, netranh, chimang, giapso;
    //}    
    public void ParseData(JSONObject data)
    {
        //    debug.Log("Parse data " + data);
        // GiaoDienPVP.ins.SoDoihinh += 1;
        //    string nameitem = data["nameitem"].ToString().Split('"')[1];
        //     string namerong = data["namerong"].ToString().Split('"')[1];
        //     string id = data["id"].ToString().Split('"')[1];

        nameobj = data["nameobject"].str;

        hp = float.Parse(data["chiso"]["hp"].ToString());
        Maxhp = float.Parse(data["chiso"]["hp"].ToString());
        //    debug.Log("rong12");
        speed = float.Parse(GamIns.CatDauNgoacKep(data["chisoget"]["tocchay"].ToString()));
        maxspeed = speed;
        //tienhoa = bytParse(data["tienhoa"].ToString());
        //   // debug.Log("rong13");
        //   // debug.Log(data["chiso"]["sucdanh"].ToString());c
        dame = float.Parse(data["chiso"]["sucdanh"].ToString());
        //    debug.Log("rong13,1");
        huthp = float.Parse(data["chiso"]["hutmau"].ToString());
        maxhuthp = huthp;
        //    debug.Log("rong14");
        netranh = float.Parse(data["chiso"]["netranh"].ToString());
        chimang = float.Parse(data["chiso"]["tilechimang"].ToString());
        giapso = float.Parse(data["chiso"]["giapso"].ToString());

       // debug.Log("giáp phần trăm ban đầu parse của " + nameobj + " Team " + team.ToString() + " là " + data["chiso"]["giapphantram"].ToString());
        giapphantram = float.Parse(data["chiso"]["giapphantram"].ToString());
       

        xuyengiap = float.Parse(data["chiso"]["xuyengiap"].ToString());
        saorong = byte.Parse(data["sao"].ToString());
        //   speed = float.Parse(data["chisoget"]["tocchay"].ToString());
        tamdanhxa = float.Parse(GamIns.CatDauNgoacKep(data["chisoget"]["tamdanhxa"].ToString()));
      //  CrGame.ins.OnThongBaoNhanh("toc chay: " + data["chisoget"]["tocchay"].ToString() + " tam danh xa " + data["chisoget"]["tamdanhxa"].ToString() + "\n" +
      //      "Sau khi parse: " + speed + "  " + tamdanhxa,10);
        //    debug.Log("rong15");
        //    Destroy(hieuung);
        //    //debug.Log(int.Parse(e.data["tienhoa"].ToString()));
        //    //  anim.SetInteger("TienHoa", int.Parse(e.data["tienhoa"].ToString()));
      //  gameObject.name = id;
        if (data["Giap"])
            {
                if (GamIns.CatDauNgoacKep(data["Giap"].ToString()) != "")
                {
               //     debug.Log("Rong mac giap hp giap: " + data["chiso"]["hpgiap"].ToString() + " hpzin " + data["chiso"]["hpzin"].ToString());
                    hp = float.Parse(data["chiso"]["hpzin"].ToString());
                    hpgiap = float.Parse(data["chiso"]["hpgiap"].ToString());
                    maxhpgiap = hpgiap;
                }
            }
        //  anim.SetInteger("TienHoa", int.Parse(e.data["tienhoa"].ToString()));
    }

    public void SetDayLuiOnline(float random)
    {
        Transform dra = transform.parent;

        if (dra.transform.parent.name == "TeamDo")
        {
            if (dra.transform.position.x < VienChinh.vienchinh.TruDo.transform.position.x)
            {
                dra.transform.position = new Vector3(dra.transform.position.x + random, dra.transform.position.y);
            }
        }
        else
        {
            if (dra.transform.position.x > VienChinh.vienchinh.TruXanh.transform.position.x)
            {
                dra.transform.position = new Vector3(dra.transform.position.x - random, dra.transform.position.y);
            }
        }
    }    
    protected void DayLuiDefault(float random = 0)
    {
        //  debug.Log("Đẩy lùi " + daylui);
        //   rigid.AddForce(transform.forward);
        if (!daylui) return;
        daylui = false;

        if (random == 0)
        {
            random = Random.Range(1, 4);
        }
        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online)
        //{
        //    JSONObject newjson = new JSONObject();
        //    newjson.AddField("id",idrong);
        //    newjson.AddField("daylui", random.ToString());
        //    // debug.Log("add daylui " + newjson.ToString());
        //    DauTruongOnline.ins.AddUpdateData(newjson);
        //    duocdaylui();

        //    return;
        //}

        
        Transform dra = transform.parent;

        if (team == Team.TeamDo)
        {
            if (dra.transform.position.x < VienChinh.vienchinh.TruDo.transform.position.x)
            {
                dra.transform.position = new Vector3(dra.transform.position.x + random,dra.transform.position.y);
            }
        }
        else
        {
            if (dra.transform.position.x > VienChinh.vienchinh.TruXanh.transform.position.x)
            {
                dra.transform.position = new Vector3(dra.transform.position.x - random, dra.transform.position.y);
            }
        }
        duocdaylui();
    }
    public void duocdaylui()
    {
        StopCoroutine(delaydaylui());
        StartCoroutine(delaydaylui());

    }
    IEnumerator delaydaylui()
    {
        yield return new WaitForSeconds(Random.Range(0.3f, 1.5f));
        //debug.Log("delaydaylui");
        daylui = true;
    }
    public abstract void DayLuiABS();
    protected void LamChamDefault(dataLamCham data) // cong = [speed][speedrun][speedAttack]
    {
        // return;
        //debug.Log("lam chaammm effect " + effect + " chia " +  chia );

        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !data.setOnline)
        //{
        //  //  debug.Log("set lam chammmmm");
        //    JSONObject newjson = new JSONObject();
        //    newjson.AddField("id",idrong);
        //    newjson.AddField("lamcham", "");
        //    newjson.AddField("time", data.time.ToString());
        //    newjson.AddField("effect", data.eff);
        //    newjson.AddField("chia", data.chia.ToString());
        //    newjson.AddField("cong", data.tangtoc);
        //    DauTruongOnline.ins.AddUpdateData(newjson,data.set);
        //    return;
        //}
        debug.Log("Làm chậm eff: " + data.eff);
        LamChamOnline(data.time,data.eff,data.chia,data.tangtoc,data.setSpeedrun,data.setSpeedanim);
    }
    public void LamChamOnline(float time, string effect = "", float chia = 2, string cong = "0",bool setSpeedRun = true, bool setSpeedAnim = true)
    {
      //  if (!setOnline) return;
        ReplayData.AddLamCham(transform.parent.name, time.ToString(), effect, chia.ToString(), cong);
        GameObject e = null;
        if (effect != "" && Setting.cauhinh == CauHinh.CauHinhCao)
        {
            Transform efff = transform.Find(effect);
            if (efff == null)
            {
                GameObject eff = Instantiate(Inventory.ins.GetEffect(effect), transform.position, Quaternion.identity);
                eff.transform.SetParent(transform);
                eff.transform.position = transform.position;
                eff.name = effect;
                e = eff;
            }
            else e = efff.gameObject;
            e.gameObject.SetActive(true);
       
        }
        //  debug.Log("tangtoc " + cong);
        if (cong == "0")
        {
            if(effect == "caylamcham")
            {
                setSpeedRun = false;
            }
            else if (effect == "banglamcham")
            {
                setSpeedAnim = false;
            }
            
            if (maxspeed * ReplayData.speedReplay / chia >= 0.3f)
            {
                if(setSpeedRun) speed = maxspeed * ReplayData.speedReplay / chia;
                if(setSpeedAnim)
                {
                    anim.SetFloat("speedRun", ReplayData.speedReplay / chia);
                    anim.SetFloat("speedAttack", ReplayData.speedReplay / chia);
                }    
            }
            else
            {
                if (setSpeedRun) speed = 0.3f * ReplayData.speedReplay;
                if (setSpeedAnim)
                {
                    anim.SetFloat("speedRun", 0.3f * ReplayData.speedReplay);
                    anim.SetFloat("speedAttack", 0.3f * ReplayData.speedReplay);
                }
              
            }
        }
        else
        {
            //debug.Log("Tăng tốccccccccccccccccccccccccccccccccccccccccccccccccccccc");
            string[] split = cong.Split('+');
            speed = maxspeed * ReplayData.speedReplay + float.Parse(split[0]);
            // speed = maxspeed * ReplayData.speedReplay + cong;
            anim.SetFloat("speedRun", ReplayData.speedReplay + float.Parse(split[1]));
            anim.SetFloat("speedAttack", ReplayData.speedReplay + float.Parse(split[2]));
        }
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(time / ReplayData.speedReplay);
            if (e != null) e.SetActive(false);
            speed = maxspeed * ReplayData.speedReplay;
            anim.SetFloat("speedRun", ReplayData.speedReplay);
            anim.SetFloat("speedAttack", ReplayData.speedReplay);
        }
    }
    public abstract void LamChamABS(dataLamCham data);

    public virtual void Choang(float giay = 0.4f)
    {
        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !setOnline)
        //{
        //    //  debug.Log("set lam chammmmm");
        //    //JSONObject newjson = new JSONObject();
        //    //newjson.AddField("id", parnent.name);
        //    //newjson.AddField("rongdie", "");
        //    //DauTruongOnline.ins.AddUpdateData(newjson);
        //    return;
        //}
        return;
        PVEManager.InstantiateHieuUngChu("choang", transform,giay);
        
        speed = 0;
        //  setAnim = false;
        //  animplay = animIdlle;
        anim.SetFloat("speedRun", 0);
        anim.SetFloat("speedAttack", 0);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(giay);
            speed = maxspeed;
            anim.SetFloat("speedRun", 1);
            anim.SetFloat("speedAttack", 1);
            // setAnim = true;
        }
    }    
    //public abstract void ChoangABS(float giay = 0.4f);
    //protected void ChoangDefault(float giay)
    //{
      
    //}
    //public void duocdaylui()
    //{
    //    StartCoroutine(delay());
    //    IEnumerator delay()
    //    {
    //        yield return new WaitForSeconds(Random.Range(1f, 2));
    //        speed = maxspeed;
    //        yield return new WaitForSeconds(Random.Range(0.5f, 3));
    //      //  debug.Log("delaydaylui");
    //        daylui = true;
    //    }
    //}
    protected virtual void RongChet()
    {
           if (team == Team.TeamXanh)
        {
            GiaoDienPVP.ins.RongChet(idrong);
           
            //   if(VienChinh.vienchinh.muctieuxanh.name == id) VienChinh.vienchinh.SetMucTieuTeamDo();//

        }
    }
    public void Died()
    {
        Transform parnent = transform.parent;
        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !setonline)
        //{
        //    //  debug.Log("set lam chammmmm");
        //    JSONObject newjson = new JSONObject();
        //    newjson.AddField("id", parnent.name);
        //    newjson.AddField("rongdie", "");
        //    DauTruongOnline.ins.AddUpdateData(newjson);
        //    return;
        //}
      //  string team = transform.parent.transform.parent.name;
     RongChet();
        //   else if (VienChinh.vienchinh.muctieudo.name == id) VienChinh.vienchinh.SetMucTieuTeamXanh();//
        //if (Target.transform.parent.name == "TeamDo")
        //{
        //    GiaoDienPVP.ins.RongChet(id);
        //}
        if (VienChinh.vienchinh.chedodau == CheDoDau.solo)
        {
            if (team == Team.TeamDo && VienChinh.vienchinh.TeamDo.transform.childCount == 2)
            {
                VienChinh.vienchinh.soloWin("win");
            }
            else if (team == Team.TeamXanh && VienChinh.vienchinh.TeamXanh.transform.childCount == 2) VienChinh.vienchinh.soloWin("thua");
        }
        if (VienChinh.vienchinh.chedodau == CheDoDau.ThuThach)
        {
            if (team == Team.TeamDo)
            {
                VienChinh.vienchinh.KichHoatSkillThuThach(VienChinh.vienchinh.nameskillthuthachdich, "TeamDo");
            }
            else VienChinh.vienchinh.KichHoatSkillThuThach(VienChinh.vienchinh.nameskillthuthach, "TeamXanh");
        }

        if(Setting.cauhinh == CauHinh.CauHinhCao)
        {
            GameObject dieAnim = Instantiate(Inventory.ins.GetEffect("DieAnim"), transform.position, Quaternion.identity);
            dieAnim.transform.position = transform.position;
            Destroy(dieAnim, 0.4f);
     
        }
        Die();
      
        // }

        //if (GetComponent<QuaiVienChinh.vienchinh>())
        //{
        //    anim.SetBool("chet", true);
        //    Destroy(gameObject, 1);
        //}
        //else Destroy(gameObject);
        //else if(VienChinh.vienchinh.chedodau == "VienChinh.vienchinh")
        //{

        //}    
    }


    protected virtual void Die()
    {
        Destroy(transform.parent.gameObject);
    }    
    public void BatTu(float sec)
    {
        battu = true;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(sec);
            battu = false;
        }
    }
    public void KillTru(float hp = 3000)
    {
       // debug.Log("killl tru");
        TruVienChinh truvienchinh = null;
        if (team == Team.TeamXanh)
        {
            truvienchinh = VienChinh.vienchinh.TeamDo.transform.GetChild(0).GetComponent<TruVienChinh>();
        }
        else
        {
            truvienchinh = VienChinh.vienchinh.TeamXanh.transform.GetChild(0).GetComponent<TruVienChinh>();

        }
        //   debug.Log("killl tru2");
        truvienchinh.MatMau(hp);
        ReplayData.AddKillTru(idrong);
    }
    public abstract void BienCuuABS(float time);

    public void BienCuuDefault(float time)
    {
        ReplayData.AddBienCuu(transform.parent.name, time.ToString());
        Transform parent = transform.parent;
     
        GameObject objskill = Resources.Load("GameData/Skill/SkillBienCuu") as GameObject;
        GameObject hieuung = Instantiate(objskill, transform.position, Quaternion.identity);
        BienCuu biencuu = hieuung.GetComponent<BienCuu>();
        //SetIEDelayBienCuu(ieDelayBienCuu);
        biencuu.chiso = this;
        hieuung.transform.SetParent(parent.transform);
        //    vienchinh.TruXanh.transform.position.y + Random.Range(-1, -3f)
        //      Vector3 newvec = new Vector3(TeamDo.transform.GetChild(random).transform.position.x,);
        hieuung.transform.position = parent.transform.position;
        if (team == Team.TeamXanh)
        {
            Vector3 scale = hieuung.transform.GetChild(1).transform.localScale;
            hieuung.transform.GetChild(1).transform.localScale = new Vector3(MathF.Abs(scale.x), scale.y, scale.z);
        }
        hieuung.name = "BienCuu";
        biencuu.time = time;
        hieuung.SetActive(true);
    }
    //public void SpeedAnim(float timeskill, float timedanh, float timechay)
    //{
    //    //    float chaybandau = speed;
    //    //  speed += 0.5f;
    //    debug.Log("time skill " + timeskill);
    //    anim.SetFloat("speedAttack", timedanh);
    //    anim.SetFloat("speedRun", timechay);
    //    if (timeskill > 0) StartCoroutine(delay());
    //    IEnumerator delay()
    //    {
    //        yield return new WaitForSeconds(timeskill);
    //        anim.SetFloat("speedAttack", 1);
    //        anim.SetFloat("speedRun", 1);
    //        speed = maxspeed;
    //        debug.Log("giam skill");
    //    }
    //}
    public void newDame(float damee, float timee, float giapp = 0)
    {
        float damelucdau = dame;
        dame = damee;
        giapso += giapp;
        debug.Log("dame ban dau " + damelucdau + " dame buff " + damee);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(timee);
            dame = damelucdau;
        }
    }
    public virtual void FuncInvokeOnline(string namefunc, params object[] parameters) { }
    public virtual void PlayAnimReplay(string s)
    {
        anim.Play(s);
    }
}
