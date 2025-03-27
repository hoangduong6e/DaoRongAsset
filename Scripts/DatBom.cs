using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatBom : MonoBehaviour
{
    public float level;
    public float dame;
    Animator anim;
    public GameObject Team;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("speed",ReplayData.speedReplay);
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        float dame = 250 + 500 * level;
        float tamxa = 1f + level * 0.2f;
        yield return new WaitForSeconds(2.9f / ReplayData.speedReplay);

        int no = 0;
        for (int i = 1; i < Team.transform.childCount; i++)
        {
            if (Mathf.Abs(transform.position.x - Team.transform.GetChild(i).transform.position.x) <= tamxa
                && Mathf.Abs(transform.position.y - Team.transform.GetChild(i).transform.position.y) <= tamxa)
            {
                if (no < 3)
                {
                    Vector3 vec = Team.transform.GetChild(i).transform.position;
                    transform.GetChild(no).transform.position = new Vector3(vec.x + Random.Range(-1, 1), vec.y + Random.Range(-0.5f, 0.5f), transform.position.z);
                    transform.GetChild(no).gameObject.SetActive(true);
                }
                //       debug.LogError(Team.transform.GetChild(i).name + " bom nooo");
                if (!ReplayData.Replay && !VienChinh.vienchinh.DanhOnline)
                {
                    Team.transform.GetChild(i).transform.Find("SkillDra").GetComponent<DragonPVEController>().MatMau(dame, null);
                }

                no += 1;
            }
        }
    
            
      
        //for(int i = 0; i < rongno.Count; i++)
        //{
        //    if(i < 3)
        //    {
        //        transform.GetChild(i).transform.position = rongno[i].transform.position;
        //        transform.GetChild(i).gameObject.SetActive(true);
        //    }
        //    rongno[i].MatMau(dame, null);
        //}
        Destroy(gameObject, 2f);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (attack) return;
    //    if(collision.GetComponent<ChiSo>())
    //    {
    //        if(collision.transform.parent.name == "TeamDo")
    //        {
    //            debug.Log("add rong no");
    //            rongno.Add(collision.GetComponent<ChiSo>());
    //        }
    //    }    
    //}
}
