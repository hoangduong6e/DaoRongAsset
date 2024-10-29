using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : SkillDraController
{
    // Start is called before the first frame update

    public GameObject nokhicham;
  
    private float speed = 15;
    protected override void ABSAwake()
    {
        //skillmoveok += controller.SkillMoveOk;

    }
    private void Start()
    {

    }
    //public float speedrotation;

    //  public float rotationModifier;

    private void OnEnable()
    {
        if (controller.Target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (nokhicham != null) nokhicham.gameObject.SetActive(false);
        transform.position = controller.transform.position;
        target = controller.Target;
        PVEManager.RotationSkill(transform, target);
    }
    // Update is called once per frame


    void LateUpdate()
    {
        if(target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        Vector3 vectarget = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, target.transform.position.z), vectarget, speed * ReplayData.speedReplay * Time.deltaTime);
       
        //Vector3 relativePos = target.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //transform.rotation = rotation;
         
        if (transform.position == vectarget)
        {
            //movesuccessfully();
            // controller.SkillMoveOk();
            if(skillmoveok != null) skillmoveok();

            if (nokhicham != null)
            {
                nokhicham.transform.position = vectarget;
                nokhicham.gameObject.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
    public override void ABSUpdateAnimationSkill()
    {

    }
}
