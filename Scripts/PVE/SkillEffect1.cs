using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillEffect1 : SkillDraController
{
    protected override void ABSAwake()
    {
        //skillmoveok += controller.SkillMoveOk;
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (controller.Target != null)
        {
            transform.position = new Vector3(controller.Target.transform.position.x - Random.Range(-0.3f, 0.3f), controller.Target.transform.position.y, controller.Target.transform.position.z);
            if (skillmoveok != null) skillmoveok();
        }
        else gameObject.SetActive(false);
    }
    public override void ABSUpdateAnimationSkill()
    {
        gameObject.SetActive(false);
    }
}
