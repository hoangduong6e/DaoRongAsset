using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillRongKhongTuoc : MonoBehaviour
{
    ChiSo chiso;
    float speed = 15;Animator anim;
    byte landanh = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        chiso = gameObject.transform.parent.gameObject.GetComponent<ChiSo>();
        anim = chiso.GetComponent<Animator>();
    }
    private void OnEnable()
    {
       // Nokhicham.SetActive(false);
    }
    private void OnDisable()
    {
        transform.position = chiso.gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, chiso.Target.z), chiso.Target, speed * Time.deltaTime);
        //  transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, chiso.Target.z);
        if (chiso.danh)
        {
            if (transform.position == chiso.Target)
            {
                if (chiso.Muctieu.name != "trudo" && chiso.Muctieu.name != "truxanh")
                {
                    float dame = chiso.dame;
                    ChiSo chisodich = chiso.Muctieu.GetComponent<ChiSo>();
                    if (Random.Range(1, 100) <= chiso.chimang)
                    {
                        dame *= 5;
                        chiso.txtChiMang();
                    }
                    chisodich.MatMau(dame, chiso);
                }
                else if (chiso.Muctieu.name == "trudo")
                {
                    TruVienChinh truvienchinh = VienChinh.vienchinh.TeamDo.transform.GetChild(0).GetComponent<TruVienChinh>();
                    truvienchinh.MatMau(3000);
                }
                else if (chiso.Muctieu.name == "truxanh")
                {
                    TruVienChinh truvienchinh = VienChinh.vienchinh.TeamXanh.transform.GetChild(0).GetComponent<TruVienChinh>();
                    truvienchinh.MatMau(3000);
                }
                // Nokhicham.transform.position = chiso.Target;
                //Nokhicham.SetActive(true);
                landanh += 1;
                if (landanh == 3)
                {
                    debug.Log("Khổng tước bất tử");
                    chiso.BatTu(2);
                }
                else if(landanh >= 6)
                {
                    float hutmaubandau = chiso.huthp;
                    chiso.huthp = 0;
                    chiso.BatTu(2);
                    anim.SetInteger("tancong", 2);
                    landanh = 0;
                    float tilelamcham = chiso.saorong * 3.5f;
                    float timelamcham = 5;
                    GameObject teamdich = chiso.Muctieu.transform.parent.gameObject;
                    float chialamcham = 2.5f;
                    float timetan = 5;
                    float speedtru = 1.5f;
                    if (chiso.saorong > 15)
                    {
                        chialamcham = 3f;
                        timetan = 6;
                        speedtru = 2;
                    }
                    if (chiso.saorong > 20)
                    {
                        timetan = 7;
                        speedtru = 2.5f;
                    }
                    for (int i = 1; i < teamdich.transform.childCount; i++)
                    {
                        if (Random.Range(0, 100) < tilelamcham)
                        {
                            ChiSo chisoo = teamdich.transform.GetChild(i).GetComponent<ChiSo>();
                            chisoo.MatMau(chiso.saorong * 7,chiso);
                            chisoo.DongBang("caylamcham",chialamcham, timetan, speedtru);
                        }
                    }
                    if (teamdich.name == "TeamDo")
                    {
                        VienChinh.vienchinh.HienIconSkill(timelamcham, "Xanh", "iconKhongTuocXanh");
                    }
                    else
                    {
                        VienChinh.vienchinh.HienIconSkill(timelamcham, "Do", "iconKhongTuocDo");
                    }
                    chiso.huthp = hutmaubandau;
                }

                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
