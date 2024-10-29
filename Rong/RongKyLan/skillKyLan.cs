using System;
using UnityEngine;

public class skillKyLan : SkillDraController
{
    private bool ok = true;
    private Team nameTeam = Team.Null;
    protected override void ABSAwake()
    {
        
    }
    private void OnEnable()
    {
        transform.position = transform.parent.transform.position;
        ok = true;
        if(nameTeam == Team.Null)
        {
            nameTeam = (Team)Enum.Parse(typeof(Team), transform.parent.transform.parent.transform.parent.gameObject.name);
            if (nameTeam == Team.TeamXanh)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }    
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(nameTeam == Team.TeamXanh)
        {
            transform.position += Vector3.right * 15 * ReplayData.speedReplay * Time.deltaTime;
            if (skillmoveok != null && ok) skillmoveok();
            if (transform.position.x >= VienChinh.vienchinh.TruDo.transform.position.x + 3)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
         
            transform.position += Vector3.left * 15 * ReplayData.speedReplay * Time.deltaTime;
            if (skillmoveok != null && ok) skillmoveok();

            if (transform.position.x <= VienChinh.vienchinh.TruXanh.transform.position.x - 3)
            {
                gameObject.SetActive(false);
            }
        }
        ok = false;
    }

    public override void ABSUpdateAnimationSkill()
    {

    }
}
