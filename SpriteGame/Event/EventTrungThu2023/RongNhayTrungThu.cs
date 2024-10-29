using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RongNhayTrungThu : MonoBehaviour
{
    private float speed; Rigidbody2D rigid;
    private bool chay = true;
    private Transform vitrinhay;
    private float nhaylen,nhaysang;

    public float SetNhayLen { set { nhaylen = value; } }
    public float SetNhaySang { set { nhaysang = value; } }
    public Transform ViTriNhay
    {
        get { return vitrinhay; }
        set { vitrinhay = value; }
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();   
    }
    void Start()
    {
        speed = Random.Range(3f, 5f);
        if(vitrinhay == null)
        {
            vitrinhay = MiniGameTrungThu.ins.GetTuong2.transform.GetChild(Random.Range(0, 3));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(chay)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(vitrinhay.position.x, vitrinhay.position.y,transform.position.z),10 * Time.deltaTime);
            if(Mathf.Abs(transform.position.x - vitrinhay.position.x) < 1  && Mathf.Abs(transform.position.y - vitrinhay.position.y) < 1)
            {
                chay = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "tuong")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            speed = Random.Range(4f, 7f);
            rigid.AddForce(transform.up * nhaylen);
            rigid.AddForce(transform.right * nhaysang);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "imgThanhGo")
        {
            Vector3 newvec = transform.position;
            rigid.AddForce(transform.up * Random.Range(40,50));
            GetComponent<BoxCollider2D>().isTrigger = true;
            DestroyRongChay();
            Destroy(rigid);
            //rigid.bodyType = RigidbodyType2D.Kinematic;
            chay = false;
            MiniGameTrungThu.ins.SetDiemThanhGo = MiniGameTrungThu.ins.GetDiemThanhGo + 1;
            MiniGameTrungThu.ins.SetHungLienTiep += 1;
            if (MiniGameTrungThu.ins.GSNgayDem == "Ngay")
            {
                if (gameObject.name != "RongNguyetLong") MiniGameTrungThu.ins.GetSetDiem += 1;
                else if (gameObject.name == "RongNguyetLong")
                {
                    MiniGameTrungThu.ins.GetSetDiem += 5;
                    MiniGameTrungThu.ins.GSNgayDem = "Dem";
                    EventTrungThu2023.ins.transform.Find("GiaoDien1").GetComponent<Image>().sprite = MiniGameTrungThu.LoadSpriteResource("BGDEM");
                }
                if (MiniGameTrungThu.ins.SetHungLienTiep >= 3)
                {
                    MiniGameTrungThu.ins.TaoTxtEvent("Combo " + MiniGameTrungThu.ins.SetHungLienTiep, transform);
                }
            }
            else
            {
                MiniGameTrungThu.ins.GetSetDiem += 5;
                if (MiniGameTrungThu.ins.SetHungLienTiep >= 3 && MiniGameTrungThu.ins.SetHungLienTiep % 3 == 0)
                {
                    MiniGameTrungThu.ins.TaoTxtEvent("Combo " + MiniGameTrungThu.ins.SetHungLienTiep, transform);
                }
            }
          
      
            MiniGameTrungThu.ins.OnBuiChamGo(newvec);
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "roi")
        {
            Destroy(GetComponent<BoxCollider2D>());
            MiniGameTrungThu.ins.Hp = MiniGameTrungThu.ins.Hp - 1;
            DestroyRongChay();
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(0.5f);
                {
                    GameObject conma = Instantiate(MiniGameTrungThu.LoadObjectResource("ConMa"), transform.position, Quaternion.identity);
                    conma.transform.SetParent(MiniGameTrungThu.ins.allRongg.transform, false);
                    conma.transform.position = transform.position;
                    conma.SetActive(true);
                    // conma.transform.move
                }
            }
        }
    }
    void DestroyRongChay()
    {
        Destroy(gameObject, 5);
       
    }
}
