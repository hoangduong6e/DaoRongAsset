using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameTrungThu : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator thanhgo;
    private GameObject ObjRong, allRong, allHp, BuiChamgo;
    private Transform tuong1, tuong2;
    private bool batdau = false,playing = false;
    private float time, maxtime = 1, timechoi = 95;
    private string[] allrongrandom = new string[] {
        "RongCay", "RongCayBang", "RongCayDat", "RongCayLua", "RongCaySam",
        "RongDat", "RongDatBang", "RongDatCay", "RongDatLua", "RongDatSam",
       "RongLua", "RongLuaBang", "RongLuaCay", "RongLuaDat" ,"RongLuaSam",
    };
    public bool SetBatDau { 
        set 
        {
            batdau = value;
            playing = value;
        } 
    }
    public Transform GetTuong2 { get { return tuong2; } }
    public static MiniGameTrungThu ins;
    public Sprite[] allThanhGo;
    private int diemThanhgo = 1, hp = 2;
    public Sprite spriteHp, spriteHpMat;
    private int Diem = 0;
    private string NgayDem = "Ngay";
    public Dictionary<string, int> allitemNhan;
    public Sprite[] allitemRoi;

    private Text txtDiem, txtKyluc, txtTime;

    private int hunglientiep = 0;
    public int SetHungLienTiep { get { return hunglientiep; } set { hunglientiep = value; } }
    // private Vector3 
    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        thanhgo = transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
        tuong1 = transform.parent.Find("vitriRongNhayDi");
        tuong2 = transform.parent.Find("vitrirongnhaytoi");
        ObjRong = transform.Find("RongChay").gameObject;
        allRong = transform.Find("allRong").gameObject;
        allHp = transform.Find("allHp").gameObject;
        for (int i = 0; i < allHp.transform.childCount; i++)
        {
            allHp.transform.GetChild(i).GetComponent<Image>().sprite = spriteHp;
        }
        txtDiem = transform.Find("txtDiem").GetComponent<Text>();
        txtKyluc = transform.Find("txtKyLuc").GetComponent<Text>();
        txtTime = transform.Find("txtTime").GetComponent<Text>();
        BuiChamgo = transform.Find("BuiChamThanhGo").gameObject;
        //maxtime = Random.Range(2,5);
    }
    public void SetLongDen()
    {
        transform.Find("txtLongDenKeoQuan").GetComponent<Text>().text = "1";
        allitemRoi[3] = allitemRoi[1];
    }
    public void Clear()
    {
        for(int i = 0; i < allRong.transform.childCount;i++)
        {
            Destroy(allRong.transform.GetChild(i).gameObject);
        }
    }    
    private void OnEnable()
    {
        nguyetlong = false;
        allitemNhan = new Dictionary<string, int>();
        EventTrungThu2023.ins.BtnHopQua.SetActive(false);
        hp = 3;
        for (int i = 2; i >= 0; i--)
        {
            Image imghp = allHp.transform.GetChild(i).GetComponent<Image>();
            imghp.sprite = spriteHp;
            imghp.SetNativeSize();
        }
        diemThanhgo = 1;
        SetThanhGo();
        SetTimeChoi(95);
        transform.Find("txtLongDenKeoQuan").GetComponent<Text>().text = "0";
    }
    private void OnDisable()
    {
        EventTrungThu2023.ins.BtnHopQua.SetActive(true);
        SetBatDau = false;
    }
    // Update is called once per frame
    public GameObject allRongg { get { return allRong; } }
    public static Sprite LoadSpriteResource(string name)
    {
        return Resources.Load<Sprite>("GameData/EventTrungThu2023/" + name);
    }
    public static GameObject LoadObjectResource(string name)
    {
        return Resources.Load<GameObject>("GameData/EventTrungThu2023/" + name);
    }
   
  
 
    public int SetDiemThanhGo {
        set {
            if (value > allThanhGo.Length - 1)
            {
                value = allThanhGo.Length - 1;
            }
            diemThanhgo = value;
            SetThanhGo();
        }
    }
    public int GetSetDiem {
        set {
            Diem = value;
            txtDiem.text = "Điểm " + "<color=lime>" + Diem + "</color>";
          
        }
        get { return Diem; }
    }

    public string GSNgayDem {
        get { return NgayDem; }
        set { NgayDem = value; }
    }
    public int Hp {
        set {
            hp = value;
        //    debug.Log("hp " + hp);
            //    allHp.transform.GetChild(hp).GetComponent<Image>().sprite ;
            if (hp <= 0)
            {
                for (int i = 2; i >= 0; i--)
                {
                    Image imghp = allHp.transform.GetChild(i).GetComponent<Image>();
                    imghp.sprite = spriteHpMat;
                    imghp.SetNativeSize();
                }
              //  debug.Log("Thua");
                StopAllCoroutines();
                EventTrungThu2023.ins.SendKQ();
            }
            else
            {
                for (int i = 2; i >= hp; i--)
                {
                    Image imghp = allHp.transform.GetChild(i).GetComponent<Image>();
                    imghp.sprite = spriteHpMat;
                    imghp.SetNativeSize();
                }
            }
            hunglientiep = 0;
        }
        get { return hp; }
    }
    public void OnBuiChamGo(Vector3 vec)
    {
        thanhgo.SetBool("nay", true);
        BuiChamgo.SetActive(true);
        BuiChamgo.transform.position = vec;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.10f);
            thanhgo.SetBool("nay", false);
            yield return new WaitForSeconds(0.05f);
            BuiChamgo.SetActive(false);
        }
    }
    public int GetDiemThanhGo { get { return diemThanhgo; } }
    public void SetThanhGo()
    {
        Image img = thanhgo.GetComponent<Image>();
        img.sprite = allThanhGo[diemThanhgo];
        img.SetNativeSize();

        BoxCollider2D box = thanhgo.GetComponent<BoxCollider2D>();
        RectTransform rectTransform = thanhgo.GetComponent<RectTransform>();
        box.size = new Vector2(rectTransform.rect.width, box.size.y);
    }
    public void TaoTxtEvent(string txt, Transform tf)
    {
        GameObject txtevent = Instantiate(LoadObjectResource("txtevent"),transform.position,Quaternion.identity);
        txtevent.transform.SetParent(allRong.transform,false);
        txtevent.transform.GetChild(0).GetComponent<Text>().text = txt;
        txtevent.transform.position = tf.position;
        txtevent.SetActive(true);
        Destroy(txtevent, 2f);
    }    
    void Update()
    {
        //if(Input.GetMouseButton(0))
        //{
        //    debug.Log("click");

        //}
        Vector3 vitriMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vitriMouse.y = thanhgo.transform.position.y;
        vitriMouse.z = thanhgo.transform.position.z;
        // vitriMouse.
        if (vitriMouse.x < tuong1.transform.position.x)
        {
            vitriMouse.x = tuong1.transform.position.x;
        }
        if (vitriMouse.x > tuong2.transform.position.x)
        {
            vitriMouse.x = tuong2.transform.position.x;
        }
        thanhgo.transform.position = vitriMouse;
        if(playing)
        {
            if (batdau)
            {
                time += Time.deltaTime;
                if (time >= maxtime)
                {
                    time = 0;
                    maxtime = UnityEngine.Random.Range(1, 4);
                    TaoRongRandom();
                }
            }
            ;
            SetTimeChoi(timechoi -= Time.deltaTime);
        }    
        //thanhgo.transform.Translate(vitriMouse);
    }
    public void AddItemRoi(string name,int soluong,Transform tf)
    {
        if(allitemNhan.ContainsKey(name))
        {
            allitemNhan[name] += soluong;
        }
        else
        {
            allitemNhan.Add(name, soluong);
        }
        TaoTxtEvent("x"+ soluong,tf);
    }
    public void SetTimeChoi(float time)
    {
        timechoi = time;
        if (timechoi >= 60)
        {
            txtTime.text = "01:" + (int)(timechoi - 60);
            if ((int)timechoi == 70)
            {
                nguyetlong = true;
            }
        }
        else
        {
            txtTime.text = "00:" + (int)timechoi;
            if ((int)timechoi == 45 || (int)timechoi == 20)
            {
                nguyetlong = true;
            }
        }
        if (timechoi <= 0)
        {
            EventTrungThu2023.ins.SendKQ();
        }
    }

    private bool nguyetlong = false;
    private void TaoRongRandom()
    {
        if(NgayDem == "Ngay")
        {
            GameObject rongrandom = Instantiate(ObjRong, transform.position, Quaternion.identity);
            rongrandom.transform.SetParent(allRong.transform, false);
            Vector3 newVec = tuong1.transform.GetChild(UnityEngine.Random.Range(0, 3)).transform.position;
            rongrandom.transform.position = new Vector3(newVec.x - 1, newVec.y, ObjRong.transform.position.z);
            Animator anim = rongrandom.GetComponent<Animator>();
            string namerongRandom = "";
            
            if (nguyetlong && NgayDem == "Ngay")
            {
                namerongRandom = "RongNguyetLong";
            }
            else
            {
                if (UnityEngine.Random.Range(0, 100) >= 60 && Diem > 150)
                {
                      StartCoroutine(delayy());
                }
                namerongRandom = allrongrandom[UnityEngine.Random.Range(0, allrongrandom.Length)];
            }
            if (UnityEngine.Random.Range(0, 100) >= 60)
            {
              StartCoroutine(randomBom());
            }

            if (UnityEngine.Random.Range(0, 100) >= 10)
            {
                StartCoroutine(randomItem());
            }
            anim.runtimeAnimatorController = Inventory.LoadAnimator(namerongRandom);
            RongNhayTrungThu rongnhay = rongrandom.GetComponent<RongNhayTrungThu>();
            rongnhay.SetNhayLen = UnityEngine.Random.Range(60, 100);
            rongnhay.SetNhaySang = UnityEngine.Random.Range(60, 100);
            //  anim.SetInteger("TienHoa", 1);
            rongrandom.SetActive(true);
            rongrandom.name = namerongRandom;
            anim.SetBool("dichuyen", true);
            if (namerongRandom == "RongNguyetLong")
            {
                //anim.SetInteger("TienHoa", 2);
                Vector3 scale = rongrandom.transform.localScale;
                scale.x *= 1.35f;
                scale.y *= 1.35f;
                rongrandom.transform.localScale = scale;

            }
            IEnumerator delayy()
            {
                GameObject rongrandom = Instantiate(ObjRong, transform.position, Quaternion.identity);
                rongrandom.transform.SetParent(allRong.transform, false);
                Vector3 newVec = tuong1.transform.GetChild(UnityEngine.Random.Range(0, 3)).transform.position;
                rongrandom.transform.position = new Vector3(newVec.x - 1, newVec.y, ObjRong.transform.position.z);
                Animator anim = rongrandom.GetComponent<Animator>();

                RongNhayTrungThu rongnhay = rongrandom.GetComponent<RongNhayTrungThu>();
                rongnhay.SetNhayLen = UnityEngine.Random.Range(60, 100);
                rongnhay.SetNhaySang = UnityEngine.Random.Range(60, 100);
                string namerongRandom = allrongrandom[UnityEngine.Random.Range(0, allrongrandom.Length)];
                anim.runtimeAnimatorController = Inventory.LoadAnimator(namerongRandom);
                //  anim.SetInteger("TienHoa", 1);
                rongrandom.SetActive(true);
                //   rongrandom.name = "rongluot";

                anim.SetBool("dichuyen", true);

                yield return new WaitForSeconds(0.2f);

            }
            IEnumerator randomBom()
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(1, 3));
                string[] allbom = new string[] { "BomDen", "BomDo", "BomDen", "BomDen", "BomDen", };

                string random = allbom[UnityEngine.Random.Range(0, allbom.Length)];

                GameObject bom = Instantiate(LoadObjectResource(random), transform.position, Quaternion.identity);
                bom.transform.SetParent(allRong.transform);
                Vector3 newVec = new Vector3();
                if(UnityEngine.Random.Range(0,100) >= 50)
                {
                    newVec = new Vector3(UnityEngine.Random.Range(3.5f, 8), allHp.transform.position.y, bom.transform.position.z);
                }
                else
                {
                    newVec = new Vector3(UnityEngine.Random.Range(-8 , -3.5f), allHp.transform.position.y, bom.transform.position.z);
                }
                //if (UnityEngine.Random.Range(0, 100) >= 50)
                //{
                //    newVec = tuong1.transform.GetChild(UnityEngine.Random.Range(0, 3)).transform.position;
                //}
                //else
                //{
                //    newVec = tuong2.transform.GetChild(UnityEngine.Random.Range(0, 3)).transform.position;
                //   // bom.GetComponent<Rigidbody2D>().AddForce(transform.right * UnityEngine.Random.Range(-70, -30));
                //}
                bom.name = random;
                bom.transform.position = new Vector3(newVec.x - 1, newVec.y, bom.transform.position.z);
                bom.SetActive(true);
                if(newVec.x >=3.5f)
                {
                    bom.GetComponent<Rigidbody2D>().AddForce(transform.right * UnityEngine.Random.Range(-170, -120));
                    //bom.GetComponent<Rigidbody2D>().AddForce(transform.up * UnityEngine.Random.Range(60, 100));
                }
                else if(newVec.x <= -3.5f)
                {
                    bom.GetComponent<Rigidbody2D>().AddForce(transform.right * UnityEngine.Random.Range(120, 170));
                }
                bom.GetComponent<Rigidbody2D>().AddForce(transform.up * UnityEngine.Random.Range(90, 130));

               // debug.Log("new vec x " + newVec.x);
            }
            IEnumerator randomItem()
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(1, 3));

                Sprite spriteitemroi = allitemRoi[UnityEngine.Random.Range(0, allitemRoi.Length)];
               // string random = spriteitemroi.name;

                GameObject bom = Instantiate(LoadObjectResource("itemRoi"), transform.position, Quaternion.identity);
                bom.transform.SetParent(allRong.transform);
                Vector3 newVec = new Vector3(UnityEngine.Random.Range(-3.5f, 3.5f), allHp.transform.position.y, bom.transform.position.z);
                bom.GetComponent<SpriteRenderer>().sprite= spriteitemroi;
                if(spriteitemroi.name == "LongDenKeoQuan")
                {

                    Inventory.ins.ScaleObject(bom,bom.transform.localScale.x/1.5f,bom.transform.localScale.y/1.5f);
                }
                debug.Log("item roi " + spriteitemroi.name);
                //if (UnityEngine.Random.Range(0, 100) >= 50)
                //{
                //    newVec = tuong1.transform.GetChild(UnityEngine.Random.Range(0, 3)).transform.position;
                //}
                //else
                //{
                //    newVec = tuong2.transform.GetChild(UnityEngine.Random.Range(0, 3)).transform.position;
                //   // bom.GetComponent<Rigidbody2D>().AddForce(transform.right * UnityEngine.Random.Range(-70, -30));
                //}
                bom.name = "itemRoi";
                bom.transform.position = new Vector3(newVec.x - 1, newVec.y, bom.transform.position.z);
                bom.SetActive(true);
                //if (newVec.x >= 3.5f)
                //{
                //    bom.GetComponent<Rigidbody2D>().AddForce(transform.right * UnityEngine.Random.Range(-170, -120));
                //    //bom.GetComponent<Rigidbody2D>().AddForce(transform.up * UnityEngine.Random.Range(60, 100));
                //}
                //else if (newVec.x <= -3.5f)
                //{
                //    bom.GetComponent<Rigidbody2D>().AddForce(transform.right * UnityEngine.Random.Range(120, 170));
                //}

                // debug.Log("new vec x " + newVec.x);
            }
        }
        else
        {
            batdau = false;
            int randomm = UnityEngine.Random.Range(0,100);
            StartCoroutine(delay());
            IEnumerator delay()
            {
                for (int i = 0; i < 20; i++)
                {
                    GameObject rongrandom = Instantiate(ObjRong, transform.position, Quaternion.identity);
                    rongrandom.transform.SetParent(allRong.transform, false);
                    Vector3 newVec = tuong1.transform.GetChild(1).transform.position;
                    rongrandom.transform.position = new Vector3(newVec.x - 1, newVec.y, ObjRong.transform.position.z);
                    Animator anim = rongrandom.GetComponent<Animator>();
                    string namerongRandom = allrongrandom[UnityEngine.Random.Range(0, allrongrandom.Length)];
                    anim.runtimeAnimatorController = Inventory.LoadAnimator(namerongRandom);
                    RongNhayTrungThu rongnhay = rongrandom.GetComponent<RongNhayTrungThu>();
                    //BoxCollider2D box = rongrandom.GetComponent<BoxCollider2D>();
                    //if(namerongRandom.Contains("RongDat"))
                    //{

                    //}
                    //else
                    //{

                    //}
               
                    if (randomm >= 50)
                    {
                        rongnhay.SetNhayLen = 20 + i * 7;
                        rongnhay.SetNhaySang = 20 + i * 7;
                    }
                    else
                    {
                        rongnhay.SetNhayLen = 160 - i * 7;
                        rongnhay.SetNhaySang = 160 - i * 7;
                    }
                    rongnhay.ViTriNhay = GetTuong2.transform.GetChild(1);
                    // anim.SetInteger("TienHoa", 1);
                    rongrandom.SetActive(true);
                    // rongrandom.name = "rongluot";
                   
                    anim.SetBool("dichuyen", true);

                    yield return new WaitForSeconds(0.22f);
                }

                batdau = true;
                // soluot = Diem + UnityEngine.Random.Range(10,25);
                nguyetlong = false;
                EventTrungThu2023.ins.transform.Find("GiaoDien1").GetComponent<Image>().sprite = LoadSpriteResource("BGNGAY");
                NgayDem = "Ngay";
            }
        }
    }
}
