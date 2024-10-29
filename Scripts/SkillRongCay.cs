using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRongCay : MonoBehaviour
{
    ChiSo chiso;
    // Start is called before the first frame update
    void Awake()
    {
        chiso = transform.parent.GetComponent<ChiSo>();
    }
    private void OnDisable()
    {
        transform.position = chiso.gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(chiso.danh)
        {
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, chiso.Target.z), chiso.Target, 7 * Time.deltaTime);
            if (transform.position == chiso.Target)
            {
                if (chiso.Muctieu.name != "trudo" && chiso.Muctieu.name != "truxanh")
                {
                    ChiSo chisoMuctieu = chiso.Muctieu.GetComponent<ChiSo>();
                    float dame = chiso.dame;
                    if (Random.Range(1, 100) <= chiso.chimang)
                    {
                        dame *= 5;
                        chiso.txtChiMang();
                    }
                    chisoMuctieu.MatMau(dame,chiso);
                    if (Random.Range(0, 3) == 0)
                    {
                        GameObject teamdich = chiso.Muctieu.transform.parent.gameObject;
                        ChiSo dongbang = teamdich.transform.GetChild(Random.Range(1, teamdich.transform.childCount - 1)).GetComponent<ChiSo>();
                        dongbang.DongBang("caylamcham");
                    }
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
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
