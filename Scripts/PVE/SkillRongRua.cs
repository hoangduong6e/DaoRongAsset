using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillRongRua : SkillDraController
{
    protected override void ABSAwake()
    {
        //skillmoveok += controller.SkillMoveOk;
    }
    // Start is called before the first frame update
    Vector3 target = Vector3.zero;
    private void OnEnable()
    {
      //  transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        if (controller.Target != null)
        {
            target = controller.Target.transform.position;
            // target.transform.Find("SkillDra").GetComponent<DragonPVEController>().idl
            target.x = controller.Target.transform.position.x - Random.Range(-0.3f, 0.3f);
            if (transform.position.y >= VienChinh.vienchinh.TruXanh.transform.position.y - 3f)
            {
                //RongBay
                target.y = VienChinh.vienchinh.TruXanh.transform.position.y -2f;
                transform.position = new Vector3(target.x, target.y, controller.Target.transform.position.z);
            }    
            else
            {
                transform.position = new Vector3(target.x, controller.Target.transform.position.y, controller.Target.transform.position.z);
            }
           
            if (skillmoveok != null) skillmoveok();
        }
        else
        {
            gameObject.SetActive(false);
            target = Vector3.zero;
        } 
            
    }
    private void LateUpdate()
    {
        if(target != Vector3.zero) transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
    }
    public override void ABSUpdateAnimationSkill()
    {
        gameObject.SetActive(false);
    }
}
