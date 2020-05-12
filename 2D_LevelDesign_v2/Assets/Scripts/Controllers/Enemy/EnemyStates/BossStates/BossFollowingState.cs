using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowingState : BossState
{

    public BossFollowingState(BossController bc)
    {
        bc.bossAnim.SetBool("bossPatrol", true);
    }

    public override void Update(BossController bc)
    {

    }

    public override void FixedUpdate(BossController bc)
    {
        if (Vector2.Distance(bc.transform.position, bc.GetPlayerCollider().transform.position) > 1.5f)
        {
            if (bc.readyToChange)
            {
                bc.readyToChange = false;
                bc.timeToChange = false;
                bc.bossAnim.SetBool("bossPatrol", false);
                bc.ChangeState(new BossChargeState(bc));
            }
            else
                bc.rb2d.transform.position = Vector2.MoveTowards(bc.transform.position, bc.GetPlayerCollider().transform.position, bc.bossModel.maxSpeed * Time.fixedDeltaTime);
        }
        else
        {
            bc.bossAnim.SetBool("bossPatrol", false);
            bc.ChangeState(new BossShooterState(bc));
        }

        if (bc.gameOver)
        {
            bc.ChangeState(new BossIdleState(bc));
            bc.gameOver = !bc.gameOver;
        }
    }

    public override void CheckTransition(BossController bc)
    {
        base.BossTransitionState(bc);
    }
}
