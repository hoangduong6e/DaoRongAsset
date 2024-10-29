using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hienSkill : MonoBehaviour
{
    public GameObject Skill;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //GameObject skill = Instantiate(Skill, transform.position,Quaternion.identity) as GameObject;
        //skill.transform.SetParent(gameObject.transform.parent.gameObject.transform);
        Skill.SetActive(true);
    }
}
