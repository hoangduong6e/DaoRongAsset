using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LanSu : MonoBehaviour
{
    ParticleSystem skill;
    DragonPVEController controller;
    void Start()
    {
        skill = GetComponentInChildren<ParticleSystem>();
        controller = transform.parent.transform.Find("SkillDra").GetComponent<DragonPVEController>();
    }
    public void UseSkill()
    {
        skill.Play();
    }
    public void Emit()
    {
        skill.Emit(3);
    }
    public void UpdateAnimAttack()
    {
        controller.UpdateAnimAttack();
    }    
}
