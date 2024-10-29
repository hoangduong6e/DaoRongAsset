using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuaiVienChinh : MonoBehaviour
{
    Animator anim;bool chay = true;bool danh = false;
    public Image ImgHp;
    ChiSo chiso;
    // Start is called before the first frame update
    private void Awake()
    {
        chiso = GetComponent<ChiSo>();
        anim = gameObject.GetComponent<Animator>();
    }
    private void Start()
    {
        chiso.tamdanhxa -= Random.Range(-0.2f, 0.2f);
        chiso.speed += Random.Range(-0.2f, 0.2f);
        if(transform.position.y > -1)
        {
            transform.position = new Vector3(transform.position.x, -1, transform.position.z);
        }
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
            anim.SetBool("danh", false);
        }
        if (danh)
        {
            anim.SetBool("danh", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
     //   debug.Log(collision.name);
        if (collision.name == "SKillRongBang")
        {
            ChiSo cs = collision.transform.parent.GetComponent<ChiSo>();
            chiso.MatMau(cs.dame,chiso);
        }
    }
}
