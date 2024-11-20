using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacLongUpdateAnimator : DraUpdateAnimator
{
    HacLongAttack hacLongAttack;
    protected override void Start()
    {
        base.Start();
        if (DragonPVEControllerr != null) hacLongAttack = DragonPVEControllerr.GetComponent<HacLongAttack>();
    }
    public void UpdateAnimCuongNo()
    {
        if (DragonPVEControllerr != null)
        {
            HacLongAttack hacLongAttack = DragonPVEControllerr.GetComponent<HacLongAttack>();
            hacLongAttack.UpdateAnimCuongNo();
        }
    }
}
