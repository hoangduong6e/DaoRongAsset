using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRongXuong : MonoBehaviour
{
    public ChiSo chiso;
    float speed = 15;
    public GameObject Nokhicham;
    // Start is called before the first frame update
    private void Awake()
    {
        if(chiso == null) chiso = gameObject.transform.parent.gameObject.GetComponent<ChiSo>();
    }
    private void OnEnable()
    {
        Nokhicham.SetActive(false);

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
                    if (Random.Range(1, 100) <= chiso.chimang)
                    {
                        float dame = chiso.dame;
                        dame *= 5;
                        ChiSo chisodich = chiso.Muctieu.GetComponent<ChiSo>();
                        chisodich.MatMau(dame, chiso);
                        chiso.txtChiMang();
                    }
                    else
                    {
                        float dame = chiso.dame;
                        ChiSo chisodich = chiso.Muctieu.GetComponent<ChiSo>();
                        chisodich.MatMau(dame, chiso);
                        //chiso.txtChiMang();
                    }
                }
                else if(chiso.Muctieu.name == "trudo")
                {
                    TruVienChinh truvienchinh = VienChinh.vienchinh.TeamDo.transform.GetChild(0).GetComponent<TruVienChinh>();
                    truvienchinh.MatMau(3000);
                }
                else if (chiso.Muctieu.name == "truxanh")
                {
                    TruVienChinh truvienchinh = VienChinh.vienchinh.TeamXanh.transform.GetChild(0).GetComponent<TruVienChinh>();
                    truvienchinh.MatMau(3000);
                }
                Nokhicham.transform.position = chiso.Target;
                Nokhicham.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
