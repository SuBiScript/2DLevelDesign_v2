using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBottomLadderState : GroundedState
{
    float inputY;

    public OnBottomLadderState(PlayerController pc) : base(pc)
    {

    }
    public override void Update(PlayerController pc)
    {
        base.Update(pc);
        inputY = Input.GetAxis("Vertical");
    }
    public override void FixedUpdate(PlayerController pc)
    {
        base.FixedUpdate(pc);
    }
    public override void CheckTransition(PlayerController pc)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(pc.groundPoint.position, pc.playerModel.groundRadius, pc.groundLayer.value + pc.ladderLayer.value);
        if (col.Length == 0)
        {
            pc.ChangeState(new OnAirState(pc));
            return;
        }
        else
        {
            bool foundSelf = false;
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].gameObject.tag == "BottomLadder")
                {
                    foundSelf = true;
                    var emission = pc.smokeRunEffect.emission; //particle system
                    emission.rateOverTime = 0f;
                }
            }
            if (!foundSelf)
            {
                pc.ChangeState(new GroundedState(pc));
                return;
            }
        }
        if (inputY >= pc.gameSettingsModel.minInputToChangeState)
        {
            pc.ChangeState(new OnLadderState(pc)); 
        }
    }
    public override void OnStateEnter(PlayerController pc)
    {

    }
    public override void OnStateExit(PlayerController pc)
    {
        pc.playerAnim.SetBool("playerRun", false);
    }
}