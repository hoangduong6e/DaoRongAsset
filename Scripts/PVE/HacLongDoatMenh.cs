using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HacLongDoatMenh : SkillDraController
{
    HacLongAttack haclong;
    protected override void ABSAwake()
    {
        //skillmoveok += controller.SkillMoveOk;
        haclong = controller.GetComponent<HacLongAttack>();
    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        if (haclong.TargetDoatMenh != null)
        {
            transform.position = new Vector3(haclong.TargetDoatMenh.transform.position.x, VienChinh.vienchinh.TruXanh.transform.position.y - 1.8f, haclong.TargetDoatMenh.transform.position.z);
        }
        else gameObject.SetActive(false);
    }
    public override void ABSUpdateAnimationSkill()
    {
        gameObject.SetActive(false);
    }
    public void UpdateAnimVetCan()
    {
        haclong.DoatMenhOk();
    }
}
