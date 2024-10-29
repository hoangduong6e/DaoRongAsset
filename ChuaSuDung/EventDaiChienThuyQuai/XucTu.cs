using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class XucTu : MonoBehaviour
{
    DragonPVEController controller;
    void Start()
    {
        controller = transform.parent.transform.Find("SkillDra").GetComponent<DragonPVEController>();
    }
    public void UpdateAnimAttack()
    {
        controller.UpdateAnimAttack();
    }
    public void UpdateAnimIdle()
    {
        controller.UpdateAnimIdle();
    }
}
