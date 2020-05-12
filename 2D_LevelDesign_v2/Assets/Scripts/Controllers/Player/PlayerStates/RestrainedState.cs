using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrainedState : PlayerState
{
    float timeInThisState;

    public RestrainedState(float timeInThisState)
    {
        this.timeInThisState = Time.time + timeInThisState;

    }
    public override void Update(PlayerController pc)
    {

    }
    public override void FixedUpdate(PlayerController pc)
    {
        pc.rb2d.velocity = Vector2.zero;
    }
    public override void CheckTransition(PlayerController pc)
    {
        if (timeInThisState <= Time.time)
        {
            pc.ChangeState(new GroundedState(pc));
        }
    }

    public override void OnStateEnter(PlayerController pc)
    {
        pc.playerAnim.SetTrigger("returnToIdle");
    }
    public override void OnStateExit(PlayerController pc)
    {
        pc.playerAnim.ResetTrigger("returnToIdle");
    }
}
