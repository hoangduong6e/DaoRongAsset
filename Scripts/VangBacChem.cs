using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VangBacChem : MonoBehaviour
{
    ChiSo chiso;
    short danh = 1;
    private void Awake()
    {
        chiso = transform.parent.GetComponent<ChiSo>();
    }
    private void OnEnable()
    {
        if (chiso.Muctieu != null)
        {
            if (chiso.danh)
            {
                transform.position = new Vector3(chiso.Muctieu.transform.position.x - Random.Range(-0.3f, 0.3f), chiso.Muctieu.transform.position.y, chiso.Muctieu.transform.position.z);
                if (chiso.Muctieu.name != "trudo" && chiso.Muctieu.name != "truxanh")
                {
                    ChiSo chisodich;
                    float dame = chiso.dame;
                    if (Random.Range(1, 100) <= chiso.chimang)
                    {
                        dame *= 5;
                        chiso.txtChiMang();
                    }
                    chisodich = chiso.Muctieu.GetComponent<ChiSo>();
                    chisodich.MatMau(dame, chiso);
                    if(danh < 3) danh += 1;
                    else
                    {
                        danh = 0;
                        chiso.BatTu(2);
                        debug.Log("Bất tử");
                    }    
                }
                else
                {
                    TruVienChinh tru = chiso.Muctieu.GetComponent<TruVienChinh>();
                    //   tru.MatMau(chiso.dame);
                    tru.MatMau(2000);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
