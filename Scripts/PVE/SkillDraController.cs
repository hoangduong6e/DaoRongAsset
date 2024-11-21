using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillDraController : MonoBehaviour
{
    // Start is called before the first frame update
    protected DragonPVEController controller;
    public Transform target;
    public Action skillmoveok { get; set; }
    private void Awake()
    {
        controller = transform.parent.GetComponent<DragonPVEController>();
        //if(controller.transform.parent.transform.parent.name == "TeamXanh")
        //{

        //}
        ABSAwake();
    }
    protected abstract void ABSAwake();
    public void UpdateAnimationSkill()
    {
    //    debug.Log("UpdateAnimationSkill");
        ABSUpdateAnimationSkill();
    }
    public abstract void ABSUpdateAnimationSkill();



}
