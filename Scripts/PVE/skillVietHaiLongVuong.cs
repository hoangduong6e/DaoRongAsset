using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillVietHaiLongVuong : SkillDraController
{
    protected override void ABSAwake()
    {
        //skillmoveok += controller.SkillMoveOk;
        
    }
    private void Start()
    {
        //if (controller.team == Team.TeamDo)
        //{
        //  //  Transform child0 = transform.GetChild(0);
        //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //   // child0.transform.localScale = new Vector3(-child0.transform.localScale.x, child0.transform.localScale.y, child0.transform.localScale.z);
        //}

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    //public float speedrotation;

    //  public float rotationModifier;

    private void OnEnable()
    {
        if (skillmoveok != null) skillmoveok();
        target = controller.Target;
        PVEManager.RotationSkill(transform, target);

    }
    // Update is called once per frame

    public override void ABSUpdateAnimationSkill()
    {
        gameObject.SetActive(false);
    }
}
