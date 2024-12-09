using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillRongMaThach : SkillDraController
{
    protected override void ABSAwake()
    {
     
    }
    private void Start()
    {
        if (controller.team == Team.TeamXanh)
        {
          //  Transform child0 = transform.GetChild(0);
            Vector3 vec = transform.localScale;
            vec.x = -vec.x;

            transform.localScale = vec;
            //debug.Log("vec rong ma thach " + vec);
        }
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        transform.position = controller.transform.position;
        if (controller.Target != null)
        {
          //  transform.position = new Vector3(controller.Target.transform.position.x - Random.Range(-0.3f, 0.3f), controller.Target.transform.position.y, controller.Target.transform.position.z);
            if (skillmoveok != null) skillmoveok();
        }
        else gameObject.SetActive(false);
    }
    public override void ABSUpdateAnimationSkill()
    {
        gameObject.SetActive(false);
    }
}
