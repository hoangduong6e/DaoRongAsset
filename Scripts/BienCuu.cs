using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BienCuu : MonoBehaviour
{
    // Start is called before the first frame update
    public float time = 0;
    float damerong;
    //public RuntimeAnimatorController animcuu;
    //RuntimeAnimatorController animrong;
    Animator animcuu;
    public DragonPVEController chiso;
    GameObject bongrong;
    float vecy;bool bayve = false,rongbay = false;
    float tamxa,speed,maxspeed;

    //float timedanh = 0, maxtimedanh = 2;
    void Start()
    {
        animcuu = transform.GetChild(1).GetComponent<Animator>();
        Transform parent = transform.parent;
       // chiso = parent.transform.Find("SkillDra").GetComponent<DragonPVEController>();
        GameObject allManhRong = parent.transform.GetChild(0).gameObject;
        if (transform.position.y >= VienChinh.vienchinh.TruXanh.transform.position.y + -1.5f)
        {
            rongbay = true;
            vecy = chiso.transform.position.y;
            bongrong = allManhRong.transform.Find("bong").gameObject;
            //   bongrong.name = "Bong" + chiso.gameObject.name;
            //  rigid = chiso.GetComponent<Rigidbody2D>();
            // rigid.bodyType = RigidbodyType2D.Dynamic;

            bongrong.transform.SetParent(VienChinh.vienchinh.transform);
            parent.transform.LeanMove(bongrong.transform.position, 0.3f);
            debug.Log("RongBay");
        }
       // debug.Log("chi so " + chiso.name);
       // if(!ReplayData.Replay)
      //  {
            tamxa = chiso.tamdanhxa;
            speed = chiso.speed;
            maxspeed = chiso.maxspeed;
            chiso.maxspeed = 0.3f;
            chiso.tamdanhxa = 2;

            damerong = chiso.dame;
            chiso.dame = 1;
       // }    
    
        //  chiso.speed = 0.3f;
        //  animrong = transform.parent.GetComponent<Animator>().runtimeAnimatorController;

     
  
        if(transform.parent.CompareTag("quai"))
        {
            transform.parent.GetComponent<SpriteRenderer>().enabled = false;
        }    
        else
        {
            allManhRong.gameObject.SetActive(false);
        }
      //  transform.parent.GetComponent<Animator>().enabled = false;
      //   transform.parent.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(delayDefault());
    }
    public IEnumerator delayDefault()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
   
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        bayve = true;
        transform.GetChild(0).gameObject.SetActive(false);
        //  transform.parent.GetComponent<SpriteRenderer>().enabled = true;
        // transform.parent.GetComponent<Animator>().enabled = true;
        chiso.dame = damerong;
        if (transform.parent.CompareTag("quai"))
        {
            transform.parent.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            transform.parent.transform.GetChild(0).gameObject.SetActive(true);
        }
        chiso.speed = maxspeed;
        chiso.maxspeed = maxspeed;
        Destroy(gameObject);
    
    }
    //private void OnDestroy()
    //{
    //    if(bongrong != null)
    //    {
    //        //Destroy(bongrong);

    //    }    
    //}
    // Update is called once per frame
    void LateUpdate()
    {
        //debug.Log("velocity " + rigid.velocity.x);
        //if(timedanh < maxtimedanh)
        //{
        //    timedanh += Time.deltaTime;
        //}    
        //else
        //{
        //    timedanh = 0;
        //    chiso.Muctieu.GetComponent<ChiSo>().MatMau(1, chiso);
        //}


        if(chiso.Target != null)
        {
            if (chiso.animplay == "Walking" || chiso.animplay == "Flying")
            {
                animcuu.SetBool("dichuyen", true);
            }
            else
            {
                animcuu.SetBool("dichuyen", false);
            }
        }
  
        if (bayve && rongbay)
        {
            transform.parent.transform.position = Vector3.MoveTowards(chiso.transform.position,new Vector3(chiso.transform.position.x,vecy), 10* Time.deltaTime);
            chiso.tamdanhxa = tamxa;
            chiso.speed = speed;
            chiso.maxspeed = maxspeed;
            if (Mathf.Abs(vecy - chiso.transform.position.y) <= 0.1f)
            {
                bongrong.transform.SetParent(chiso.transform);
                bongrong.transform.SetSiblingIndex(1);
             //   if (!ReplayData.Replay)
          //      {
                 
              //  }
                //Destroy(bongrong);
             //   bongrong = null;
                bongrong.transform.SetParent(transform.parent.transform.GetChild(0).transform);
                Destroy(gameObject);
            }
        }
        if(bongrong != null)
        {
            bongrong.transform.position = new Vector3(chiso.transform.position.x, bongrong.transform.position.y, bongrong.transform.position.z);
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.name == bongrong.name)
    //    {
    //        rigid.bodyType = RigidbodyType2D.Static;
    //       // chiso.tamdanhxa = 2;
    //      //  chiso.speed = 0.3f;
    //    }
    //}
}
