using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossH : MonoBehaviour
{
    GameObject TeamXanh, TruXanh;
    Animator anim; bool chay = true; bool danh = false,den = true;
    string Team; public Image ImgHp; VienChinh vienchinh;
    ChiSo chiso;int tancong = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        chiso = GetComponent<ChiSo>();
        TeamXanh = GameObject.Find("TeamXanh");
        TruXanh = GameObject.Find("truxanh");
        vienchinh = GameObject.FindGameObjectWithTag("vienchinh").GetComponent<VienChinh>();
  
        anim = gameObject.GetComponent<Animator>();

      //  GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
    private void Start()
    {
    
        Team = transform.parent.name;
        chiso.tamdanhxa -= Random.Range(-0.2f, 0.2f);
        chiso.speed += Random.Range(-0.2f, 0.2f);
        //if (transform.position.y > -6)
        //{
        //    transform.position = new Vector3(transform.position.x, -6f, transform.position.z);
        //}
        transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        chiso.daylui = false;
    }
    // Update is called once per frame
    void Update()
    {
        //if (TeamXanh.transform.childCount > 1)
        //{
        //    chiso.Muctieu = TeamXanh.transform.GetChild(1).gameObject;
        //    chiso.Target = TeamXanh.transform.GetChild(1).transform.position;
        //}
        //else
        //{
        //    chiso.Muctieu = TeamXanh.transform.GetChild(0).gameObject;
        //    chiso.Target = TeamXanh.transform.GetChild(0).transform.position;
        //}
        chiso.Target = VienChinh.vienchinh.muctieudo.transform.position;
        chiso.Muctieu = VienChinh.vienchinh.muctieudo;
        if (chiso.Target.y > -1)
        {
            chiso.Target = new Vector3(chiso.Target.x, -1.5f, chiso.Target.z);
        }
        if (transform.position.x > chiso.Target.x + chiso.tamdanhxa)
        {
            chay = true; danh = false; chiso.danh = false;
            //transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, chiso.Target.z), chiso.Target, chiso.speed * Time.deltaTime);
            transform.position += Vector3.left * chiso.speed * Time.deltaTime;
        }
        //if (transform.position.x > chiso.Target.x + chiso.tamdanhxa)
        //{
        //    transform.position += Vector3.left * chiso.speed * Time.deltaTime;
        //    //  transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, Target.z), Target, speed * Time.deltaTime);
        //    chay = true; danh = false;
        //}
        else
        {
            danh = true;
            chay = false;
            chiso.danh = true;
        }

        if (chay)
        {
            anim.SetInteger("tancong", 0);
        }
        if (danh)
        {
            anim.SetInteger("tancong", tancong);
            if (den)
            {
                if (chiso.hp <= chiso.Maxhp / 1.3f)
                {
                    tancong = 0;
                    Manden();
                    den = false;
                    anim.SetInteger("tancong", 2);
                }
                //if (solandanh >= 10)
                //{
                //    Manden();
                //    den = false;
                //    anim.SetInteger("tancong", 2);
                //    if (solandanh >= 20)
                //    {
                //        solandanh = 0;
                //        den = true;
                //    }
                //}    
            }
        }
    }
    void Manden()
    {
        vienchinh.Hieuungd.SetActive(true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(3f);
            vienchinh.Hieuungd.SetActive(false);
            tancong = 2;
            yield return new WaitForSeconds(10f);
            tancong = 1;
            yield return new WaitForSeconds(25f);
            Manden();
        }
    }
    private void OnDestroy()
    {
        vienchinh.Hieuungd.SetActive(false);
    }
    public void DanhAll()
    {
        if (TeamXanh.transform.childCount > 1)
        {
            for (int i = 1; i < TeamXanh.transform.childCount; i++)
            {
                ChiSo cs = TeamXanh.transform.GetChild(i).GetComponent<ChiSo>();
                cs.MatMau(3000, cs);
            }
        }    
        else
        {
            TeamXanh.transform.GetChild(0).GetComponent<TruVienChinh>().MatMau(5000);
        }
    
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //   debug.Log(collision.name);
        if (collision.name == "SKillRongBang")
        {
            ChiSo cs = collision.transform.parent.GetComponent<ChiSo>();
            chiso.MatMau(cs.dame, chiso);
        }
    }
}
