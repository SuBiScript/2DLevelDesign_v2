using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    
    public BossIdleState(BossController bc)
    {
        bc.bossAnim.SetBool("bossShoot", false);
    }

    public override void Update(BossController bc)
    {
        
    }

    public override void FixedUpdate(BossController bc)
    {

    }

    public override void CheckTransition(BossController bc)
    {
        base.BossTransitionState(bc);
    }
}
