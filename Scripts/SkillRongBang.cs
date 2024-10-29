using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRongBang : MonoBehaviour
{
    ChiSo chiso;
    private void Awake()
    {
        chiso = transform.parent.GetComponent<ChiSo>();
    }
    private void OnEnable()
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
            chisoMuctieu.MatMau(dame, chiso);
            GameObject teamdich = chiso.Muctieu.transform.parent.gameObject;
            ChiSo dongbang = teamdich.transform.GetChild(Random.Range(1, teamdich.transform.childCount - 1)).GetComponent<ChiSo>();
            dongbang.DongBang("Bang");
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
        transform.position = chiso.Muctieu.transform.position;
        //if (chiso.Muctieu.name != "trudo" && chiso.Muctieu.name != "truxanh")
        //{
        //    //ChiSo chisodich;
        //    //chisodich = chiso.Muctieu.GetComponent<ChiSo>();
        //    //chisodich.MatMau(chiso.dame);
        //}
        //else
        //{
        //    //TruVienChinh tru = chiso.Muctieu.GetComponent<TruVienChinh>();
        //    //tru.MatMau(chiso.dame);
        //}
    }
}
