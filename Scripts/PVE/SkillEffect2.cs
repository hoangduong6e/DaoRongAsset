using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillEffect2 : SkillDraController
{
    // Start is called before the first frame update
  
    public override void ABSUpdateAnimationSkill()
    {
        
    }
    protected override void ABSAwake()
    {

    }
    private void OnEnable()
    {
        if (controller.Target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = controller.transform.position;
        target = controller.Target;
        StartCoroutine(delay());
        IEnumerator delay()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f / ReplayData.speedReplay);
            transform.GetChild(0).gameObject.SetActive(false);
            GameObject skilldown = transform.GetChild(1).gameObject;
            if (target == null)
            {
                gameObject.SetActive(false);
                yield break;
            }
                
            Vector3 vec = new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, target.transform.position.z);
            skilldown.transform.position = vec;
            skilldown.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f / ReplayData.speedReplay);
            skilldown.gameObject.SetActive(false);

            if(skillmoveok != null) skillmoveok();


            gameObject.SetActive(false);
        }
    }
}
