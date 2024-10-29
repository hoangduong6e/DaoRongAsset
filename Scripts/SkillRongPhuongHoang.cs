using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRongPhuongHoang : MonoBehaviour
{
    ChiSo chiso;
    private void Awake()
    {
        chiso = transform.parent.parent.GetComponent<ChiSo>();
    }
    private void OnEnable()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            transform.GetChild(0).gameObject.SetActive(false);
            GameObject skilldown = transform.GetChild(1).gameObject;
            Vector3 vec = new Vector3(chiso.Muctieu.transform.position.x, chiso.Muctieu.transform.position.y + 1.5f, chiso.Muctieu.transform.position.z);
            skilldown.transform.position = vec;
            skilldown.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            skilldown.gameObject.SetActive(false);

            if (chiso.Muctieu.name != "trudo" && chiso.Muctieu.name != "truxanh")
            {
                ChiSo chisodich;
                float dame = chiso.dame;
                if (CrGame.ins.NgayDem == "Ngay")
                {
                    debug.Log("Phượng hoàng ban ngày + 50% sức đánh");
                    dame += dame / 2; //Cộng 50% sức đánh
                }
                if (Random.Range(1, 100) <= chiso.chimang)
                {
                    dame *= 5;
                    chiso.txtChiMang();
                }
                chisodich = chiso.Muctieu.GetComponent<ChiSo>();
                chisodich.MatMau(dame, chiso);
            }
            else
            {
                TruVienChinh tru = chiso.Muctieu.GetComponent<TruVienChinh>();
                //   tru.MatMau(chiso.dame);
                tru.MatMau(2000);
            }

            gameObject.SetActive(false);
        }
    }
}
