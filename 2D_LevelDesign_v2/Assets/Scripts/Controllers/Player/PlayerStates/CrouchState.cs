using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : PlayerState
{
    float inputY;
    float jump;
    private bool mustJump;

    public CrouchState(PlayerController pc)
    {
        mustJump = false;
    }
    public override void Update(PlayerController pc)
    {
        pc.inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Crouch");
        jump = Input.GetAxis("Jump");
        pc.Flip();
        pc.Shoot();
    }
    public override void FixedUpdate(PlayerController pc) { }
    public override void CheckTransition(PlayerController pc)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(pc.groundPoint.position, pc.playerModel.groundRadius, pc.groundLayer.value + pc.ladderLayer.value);
        if (col.Length == 0 || mustJump)
        {
            pc.ChangeState(new OnAirState(pc));
            return;
        }
        if (inputY > -pc.gameSettingsModel.minInputToChangeState)
        {
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].gameObject.tag == "TopLadder")
                {
                    pc.ChangeState(new OnTopLadderState(pc));
                    return;
                }
                if (col[i].gameObject.tag == "BottomLadder")
                {
                    pc.ChangeState(new OnBottomLadderState(pc));
                    return;
                }
            }
            pc.ChangeState(new GroundedState(pc));
            return;
        }
        if (jump > pc.gameSettingsModel.minInputToChangeState)
        {
            pc.rb2d.AddForce(Vector2.up * pc.playerModel.jumpImpulse, ForceMode2D.Impulse);
            mustJump = true;
        }
    }
    public override void OnStateEnter(PlayerController pc)
    {
        //Animator
        pc.playerAnim.SetBool("playerProne", true);
        pc.rb2d.velocity = Vector3.zero;
        pc.crouchedCollider.enabled = true;
        pc.standingCollider.enabled = false;
        pc.weaponPosition = pc.crouchedShootingPosition;
        pc.particles1.SetActive(false);
        pc.particles2.SetActive(true);
        pc.particlePoint = pc.particles2;
    }
    public override void OnStateExit(PlayerController pc)
    {
        //Animator
        pc.playerAnim.SetBool("playerProne", false);
        pc.crouchedCollider.enabled = false;
        pc.standingCollider.enabled = true;
        pc.weaponPosition = pc.standingShootingPosition;
        pc.particles1.SetActive(true);
        pc.particles2.SetActive(false);
        pc.particlePoint = pc.particles1;
    }
}
