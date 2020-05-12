using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeState : BossState
{

    public BossChargeState(BossController bc)
    {
        bc.bossAnim.SetBool("bossCharge", true);
        AudioManager.instance.Play("BossCharge");
    }

    public override void Update(BossController bc)
    {

    }

    public override void FixedUpdate(BossController bc)
    {


        if (Vector2.Distance(bc.transform.position, bc.GetPlayerCollider().transform.position) > 0.8f)
            bc.rb2d.transform.position = Vector2.MoveTowards(bc.transform.position, new Vector2(bc.GetPlayerCollider().transform.position.x, bc.transform.position.y), bc.bossModel.chargeSpeed * Time.fixedDeltaTime);
        else
        {
            bc.bossAnim.SetBool("bossCharge", false);
            bc.ChangeState(new BossMeleeState(bc));
        }
    }

    public override void CheckTransition(BossController bc)
    {
        base.BossTransitionState(bc);
    }
}

