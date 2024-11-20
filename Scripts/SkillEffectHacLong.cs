using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectHacLong : SkillDraController
{
    // Start is called before the first frame update
    public float speed = 20;
    protected override void ABSAwake()
    {
        //skillmoveok += controller.SkillMoveOk;
      

    }
    private void Start()
    {
        Transform child0 = transform.GetChild(0);
        child0.transform.position = new Vector3(child0.transform.position.x, controller.transform.position.y + 0.6f, 0);
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
        transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, 0);
        target = controller.Target;
        PVEManager.RotationSkill(transform, target);
    }
    // Update is called once per frame


    void LateUpdate()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        float cong = (controller.team == Team.TeamXanh) ? 0.5f : -0.5f;
        Vector3 vectarget = new Vector3(target.transform.position.x + cong, target.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, target.transform.position.z), vectarget, speed * ReplayData.speedReplay * Time.deltaTime);

        //Vector3 relativePos = target.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //transform.rotation = rotation;

        if (transform.position == vectarget)
        {
            //movesuccessfully();
            // controller.SkillMoveOk();
            if (skillmoveok != null) skillmoveok();
            gameObject.SetActive(false);
        }
    }
    public override void ABSUpdateAnimationSkill()
    {

    }
}
