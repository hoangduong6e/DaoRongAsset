using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RongDatAttack : MonoBehaviour
{
    Animator anim;
    public byte Tienhoa;
    ChiSo chiso; 
    GameObject TeamDich;
    // Start is called before the first frame update
    private void OnEnable()
    {
        chiso = GetComponent<ChiSo>();
        Vector3 Scale;
        Scale = transform.localScale;
        if (gameObject.transform.parent.name == "TeamXanh")
        { 
            TeamDich = VienChinh.vienchinh.TeamDo;
            Scale.x = -Scale.x;
            transform.localScale = Scale;
        }
        else
        {
            TeamDich = VienChinh.vienchinh.TeamXanh;
            chiso.ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
        }
        anim = gameObject.GetComponent<Animator>();
        anim.SetInteger("TienHoa", Tienhoa);
        
        chiso.tamdanhxa += Random.Range(-0.3f, 0.3f);
        chiso.speed += Random.Range(-0.2f, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        //if (TeamDich.transform.childCount > 1)
        //{
        //    chiso.Target = TeamDich.transform.GetChild(1).transform.position;
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
            else Danh();
        }
        else
        {
           
            chiso.Target = VienChinh.vienchinh.muctieuxanh.transform.position;
            chiso.Muctieu = VienChinh.vienchinh.muctieuxanh;
            if (transform.position.x < chiso.Target.x - chiso.tamdanhxa)
            {
                transform.position += Vector3.right * chiso.speed * Time.deltaTime;
                Chay();
            }
            else Danh();
        }
    }
    void Chay()
    {
        anim.SetInteger("tancong", 0);
        anim.SetBool("dichuyen", true);
        chiso.danh = false;
    }
    void Danh()
    {
        anim.SetInteger("tancong", 1);
        anim.SetBool("dichuyen", false);
        chiso.danh = true;
    }
}
