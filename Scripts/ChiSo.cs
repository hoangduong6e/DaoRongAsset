using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChiSo : MonoBehaviour
{
    public float dame = 500,
        hp = 2000,
        Maxhp = 2000,
        speed = 2,
        tamdanhxa,
        chimang = 0,
        netranh = 0,
        huthp = 0,
        giapso = 0,
        giapphantram = 0,
        xuyengiap = 0,
        hpgiap = 0,
        maxhpgiap = 0;
    public int saorong;
    public Image ImgHp;GameObject ThanhMau;
    public Vector3 Target;
    public GameObject Muctieu;Animator anim;public bool danh = false,battu = false,daylui = true;
    public float[] arhp = new float[] { };byte solanhoisinh = 0;
    public float maxspeed;
    float speedDanh = 1;
    float speedchay = 1;
    Rigidbody2D rigid;
    private void Awake()
    {
        ThanhMau = ImgHp.transform.parent.gameObject;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        maxspeed = speed;
        speedDanh = anim.GetFloat("speedDanh");
        speedchay = anim.GetFloat("speedchay");
    }
    // Start is called before the first frame update
    public void txtChiMang()
    {
        //GameObject txtChiMang = Instantiate(VienChinh.vienchinh.txtChiMang,transform.position,Quaternion.identity);
        //Vector3 vec = new Vector3(transform.position.x,transform.position.y + 0.5f,100);
        //txtChiMang.transform.position = vec;
        //txtChiMang.gameObject.SetActive(true);
        //Destroy(txtChiMang, 2f);
    }
    public void SpeedAnim(float timeskill,float timedanh,float timechay)
    {
    //    float chaybandau = speed;
      //  speed += 0.5f;
        debug.Log(timeskill);
        anim.SetFloat("speedDanh", timedanh);
        anim.SetFloat("speedchay", timechay);
        if(timeskill > 0) StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(timeskill);
            anim.SetFloat("speedDanh", speedDanh);
            anim.SetFloat("speedchay", speedchay);
            speed = maxspeed;
            debug.Log("giam skill");
        }
    }    
    public void BatTu(float s)
    {
        battu = true;
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(s);
            battu = false;
        }
    }
    public void newDame(float damee,float timee,float giapp = 0)
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
    //public void newHp(float hpp, float timee)
    //{
    //    float hoisinh = solanhoisinh;
    //    float hplucdau = hp;
    //    hp = hpp;
    //    StartCoroutine(delay());
    //    IEnumerator delay()
    //    {
    //        yield return new WaitForSeconds(timee);
    //        hp = hplucdau;
    //    }
    //}
    public void HutMau()
    {
        float conghp = dame * huthp / 100;
        if (conghp <= 0) return;
            hp += conghp;
        if (hp > Maxhp) hp = Maxhp;
        float fillamount = (float)hp / (float)Maxhp;
        ImgHp.fillAmount = fillamount;

        GameObject hutmau = Instantiate(Inventory.ins.GetEffect("hutmau"), transform.position, Quaternion.identity);
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, 100);
        hutmau.transform.position = vec;
        hutmau.gameObject.SetActive(true);
        Destroy(hutmau, 0.45f);

        ThanhMau.SetActive(true);
        CancelInvoke();
        Invoke("tatthanhmau", 5);
    }
    public void MatMau(float maumat,ChiSo cs)
    {
        if(battu == false)
        {
            if (Random.Range(1, 100) <= netranh)
            {
                //GameObject txtnetranh = Instantiate(VienChinh.vienchinh.txtneTranh, transform.position, Quaternion.identity);
                //Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.5f, 100);
                //txtnetranh.transform.position = vec;
                //txtnetranh.gameObject.SetActive(true);
                //Destroy(txtnetranh, 2f);
                return;
            }
            if(cs != null)
            {
                if (cs.huthp > 0)
                {
                    //   debug.LogError(this.);
                    cs.HutMau();
                }
                if (cs.xuyengiap < giapso) maumat -= giapso - cs.xuyengiap;
            }
            float giappt = giapphantram;
            if (gameObject.transform.parent.name == "TeamXanh")
            {
                giappt += VienChinh.vienchinh.buffgiapallxanh;
            }
            else giappt += VienChinh.vienchinh.buffgiapalldo;
            if (giappt > 0) maumat -= maumat / 100 * giappt;
            if (maumat < 0) maumat = 0;
          //  debug.Log("chedodau "+ VienChinh.vienchinh.chedodau);
            //if (VienChinh.vienchinh.chedodau == "SoloKhongTuoc")
            //{
            //    if (gameObject.transform.parent.name == "TeamDo")
            //    {
            //        VienChinh.vienchinh.dameKhongTuoc += maumat;
            //    }    
           
            //}
            if (VienChinh.vienchinh.chedodau == CheDoDau.BossTG)
            {
                if(gameObject.tag == "quai")
                {
                    NetworkManager.ins.socket.Emit("DanhBossTG", JSONObject.CreateStringObject(maumat.ToString()));
                } 
                else
                {
                    if(hpgiap > 0)
                    {
                        hpgiap -= maumat;
                        float fillamount = (float)hpgiap / (float)maxhpgiap;
                        ImgHp.transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount = fillamount;
                    }
                    else
                    {
                        hp -= maumat;
                        float fillamount = (float)hp / (float)Maxhp;
                        ImgHp.fillAmount = fillamount;
                    }
                }
            }    
            else
            {
                if (hpgiap > 0)
                {
                    hpgiap -= maumat;
                    float fillamount = (float)hpgiap / (float)maxhpgiap;
                    ImgHp.transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount = fillamount;
                }
                else
                {
                    hp -= maumat;
                    float fillamount = (float)hp / (float)Maxhp;
                    ImgHp.fillAmount = fillamount;
                }
            }
         //   debug.LogError("MauMat " + gameObject.transform.parent.name + ": " + maumat);
            if (hp <= 0)
            {
                if (arhp.Length > solanhoisinh)
                {
                    //Maxhp = arhp[solanhoisinh];
                    //hp = arhp[solanhoisinh];
                    //solanhoisinh += 1;
                    //GameObject txtnetranh = Instantiate(VienChinh.vienchinh.txthoisinh, transform.position, Quaternion.identity);
                    //Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.5f, 100);
                    //txtnetranh.transform.position = vec;
                    //txtnetranh.gameObject.SetActive(true);
                    //Destroy(txtnetranh, 2f);
                }
                else
                {
                    string[] id = gameObject.name.Split('-');
                    if (id.Length > 1 && Muctieu.transform.parent.name == "TeamDo")
                    {
                        GiaoDienPVP.ins.RongChet(id[1]);
                    }
                    if (VienChinh.vienchinh.chedodau == CheDoDau.solo)
                    {
                        if (gameObject.transform.parent.name == "TeamDo")
                        {
                            VienChinh.vienchinh.soloWin("win");
                        }
                        else VienChinh.vienchinh.soloWin("thua");
                    }
                    if(VienChinh.vienchinh.chedodau == CheDoDau.ThuThach)
                    {
                        if (gameObject.transform.parent.name == "TeamDo")
                        {
                            VienChinh.vienchinh.KichHoatSkillThuThach(VienChinh.vienchinh.nameskillthuthachdich,"TeamDo");
                        }
                        else VienChinh.vienchinh.KichHoatSkillThuThach(VienChinh.vienchinh.nameskillthuthach, "TeamXanh");
                    }
                    Destroy(gameObject);
                }
               
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
            ThanhMau.SetActive(true);
            CancelInvoke();
            Invoke("tatthanhmau", 5);
        }    
    }
    void tatthanhmau()
    {
        ThanhMau.SetActive(false);
    }
    [HideInInspector] public bool dongbang = false;
    public void DongBang(string nameobject,float chia = 2,float timetan = 5,float speedtru = 1)//caylamcham
    {
        GameObject Bang = null;
        if (dongbang == false)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == nameobject)
                {
                    Bang = child.gameObject;
                    Bang.SetActive(true); 
                    speed -= speedtru;
                    if (speed < 0) speed = 0;
                    dongbang = true;
                    anim.SetFloat("speedDanh", speedDanh / chia);
                    anim.SetFloat("speedchay", speedchay / chia);
                    StartCoroutine(BangTan());
                    break;
                }
            }
        }
        IEnumerator BangTan()
        {
            yield return new WaitForSeconds(timetan);
            Bang.SetActive(false);
            speed = maxspeed;
            anim.SetFloat("speedDanh", speedDanh);
            anim.SetFloat("speedchay", speedchay);
            dongbang = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if(collision.name == "quaichem")
        {
            if (collision.transform.parent.transform.parent.name != transform.parent.name)
            {
                ChiSo cs = collision.transform.parent.GetComponent<ChiSo>();
                MatMau(cs.dame / 3, cs);
            }
        }
        if (collision.name == "daylui")
        {
            ChiSo cs = collision.transform.parent.GetComponent<ChiSo>();
            cs.daylui = false;
            cs.duocdaylui();
            DayLui(collision.transform.parent.transform.parent.name);
        }
        if (collision.name == "VangBacChem")
        {
            if (collision.transform.parent.transform.parent.name != transform.parent.name)
            {
                ChiSo cs = collision.transform.parent.GetComponent<ChiSo>();
                MatMau(cs.dame/1.3f, cs);
            }
        }
        if (collision.name == "chammatmau")
        {
            if (collision.transform.parent.transform.parent.name != transform.parent.name)
            {
                if(collision.transform.parent.GetComponent<ChiSo>())
                {
                    ChiSo cs = collision.transform.parent.GetComponent<ChiSo>();
                    cs.daylui = false;
                    cs.duocdaylui();
                    MatMau(cs.dame / 2, cs);
                    DayLui(collision.transform.parent.transform.parent.name);
                    //collision.gameObject.SetActive(false);
                }    
            }
        }
        if (collision.name == "skillRongPhuongHoangdown")
        {
            debug.Log("skillRongPhuongHoangdown " + collision.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.name);
            if (collision.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.name != transform.parent.name)
            {
                if (collision.transform.parent.transform.parent.transform.parent.transform.transform.parent.GetComponent<ChiSo>())
                {
                    ChiSo cs = collision.transform.parent.transform.parent.transform.parent.transform.transform.parent.GetComponent<ChiSo>();
                   // debug.LogError("dame " + cs.dame);
                    MatMau(cs.dame / 2, cs);
                }
            }
        }
    }
    public void DayLui(string nameTeam,float nhan = 1.8f)
    {
        if (nameTeam != transform.parent.name)
        {
            debug.Log("Đẩy lùi " + daylui) ;
            //   rigid.AddForce(transform.forward);
            if (gameObject.transform.parent.name == "TeamDo")
            {
                if (daylui)
                {
                    if (transform.position.x < VienChinh.vienchinh.TruDo.transform.position.x)
                    {
                        daylui = false;
                        for (int i = 0; i < Random.Range(45,60); i++)
                        {
                            transform.position += Vector3.right * nhan * Time.deltaTime;
                        }
                        speed /= 5;
                        duocdaylui();
                    }
                }
            }
            else
            {
                if (daylui)
                {
                    if (transform.position.x > VienChinh.vienchinh.TruXanh.transform.position.x)
                    {
                        daylui = false;
                     
                        for (int i = 0; i < Random.Range(45, 60); i++)
                        {
                            transform.position += Vector3.left * nhan * Time.deltaTime;
                        }
                        speed /= 5;
                        duocdaylui();
                    }
                }
            }
        }
    }    
    public void duocdaylui()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(Random.Range(1f,2));
            speed = maxspeed;
            yield return new WaitForSeconds(Random.Range(0.5f,3));
            debug.Log("delaydaylui");
            daylui = true;
        }
    }
}