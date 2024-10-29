using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RongLuaMatXanhGiap : MonoBehaviour
{
    Animator anim;
    public byte Tienhoa;
    public GameObject bongrong; Vector3 Scale;
    ChiSo chiso;
    GameObject TeamDich;
    // Start is called before the first frame update
    private void OnEnable()
    {
        chiso = GetComponent<ChiSo>();
        Scale = transform.localScale;
        if (gameObject.transform.parent.name == "TeamXanh")
        {
            TeamDich = VienChinh.vienchinh.TeamDo;
            Scale.x = -Scale.x;
        }
        else
        {
            TeamDich = VienChinh.vienchinh.TeamXanh;
            chiso.ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
        }
        anim = gameObject.GetComponent<Animator>();
        anim.Play("RongBay");
     //   chiso.arhp = new float[] { chiso.Maxhp };
        transform.localScale = Scale;
        chiso.tamdanhxa -= Random.Range(-0.5f, 2f);
        chiso.speed += Random.Range(-0.3f, 0.3f);
        transform.position = new Vector3(transform.position.x, transform.position.y + 3);
        bongrong.transform.position = new Vector3(transform.position.x, transform.position.y - 3);
    }
    // Update is called once per frame
    void Update()
    {
        //if (TeamDich.transform.childCount > 1)
        //{
        //    chiso.Target= TeamDich.transform.GetChild(1).transform.position;
        //    chiso.Muctieu = TeamDich.transform.GetChild(1).gameObject;
        //}
        //else
        //{
        //    chiso.Target = TeamDich.transform.GetChild(0).transform.position;
        //    chiso.Muctieu = TeamDich.transform.GetChild(0).gameObject;
        //}
        if (TeamDich.name == "TeamXanh")
        {
            chiso.Target = VienChinh.vienchinh.muctieudo.transform.position;
            chiso.Muctieu = VienChinh.vienchinh.muctieudo;
            if (transform.position.x > chiso.Target.x + chiso.tamdanhxa)
            {
                transform.position += Vector3.left * chiso.speed * Time.deltaTime;
                Chay();
            }
            else
            {
                Danh();
            }
        }
        else
        {
            chiso.Target = VienChinh.vienchinh.muctieuxanh.transform.position;
            chiso.Muctieu = VienChinh.vienchinh.muctieuxanh;


            if (transform.position.x < chiso.Target.x - chiso.tamdanhxa)
            {
                transform.position += Vector3.right * chiso.speed * Time.deltaTime;
                Chay();
                //  transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, Target.z), Target, speed * Time.deltaTime);
                // chay = true; danh = false;
            }
            else
            {
                //danh = true; chay = false;
                Danh();
            }
        }
        //if (chay)
        //{
        //    anim.SetInteger("tancong", 0);
        //}
        //if (danh)
        //{
        //    anim.SetInteger("tancong", 1);
        //}
        //if (transform.position.x > chiso.Target.x)
        //{
        //    if (Scale.x < 0)
        //    {
        //        Scale.x = Mathf.Abs(Scale.x);
        //        transform.localScale = Scale;
        //    }
        //}
        //else
        //{
        //    if (Scale.x > 0)
        //    {
        //        Scale.x = -Scale.x;
        //        transform.localScale = Scale;
        //    }
        //}
    }
    void Danh()
    {
        anim.SetInteger("tancong", 1);
        chiso.danh = true;
    }
    void Chay()
    {
        anim.SetInteger("tancong", 0);
        chiso.danh = false;
    }
}
