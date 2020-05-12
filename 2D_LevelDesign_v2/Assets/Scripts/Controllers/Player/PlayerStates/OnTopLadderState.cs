using System.Collections;
using UnityEngine;

public class OnTopLadderState : GroundedState
{
    float inputY;
    Vector2 ladderPosition;
    public int ladderFramesCooldown;
    private float timeToJump;
    private float timeToEnableLadder;

    private bool ableToUseLadder;

    public OnTopLadderState(PlayerController pc) : base(pc)
    {
        ableToJump = false;
        ableToUseLadder = false;
        Collider2D[] col = Physics2D.OverlapCircleAll(pc.groundPoint.position, pc.playerModel.groundRadius, pc.groundLayer.value + pc.ladderLayer.value);
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.tag == "TopLadder")
            {
                ladderPosition = col[i].gameObject.transform.position;
            }
        }
    }
    public override void Update(PlayerController pc)
    {
        pc.Flip();
        pc.Shoot();
        EnableLadder();
        pc.inputX = Input.GetAxis("Horizontal");
        if (pc.inputX < -pc.gameSettingsModel.minInputToChangeState || pc.inputX > pc.gameSettingsModel.minInputToChangeState)
            pc.playerAnim.SetBool("playerRun", true);
        else
        {
            pc.playerAnim.SetBool("playerRun", false);
            pc.playerAnim.SetTrigger("returnToIdle");
        }

        jump = Input.GetAxis("Jump");
        if (!ableToJump && Time.time > timeToJump)
            ableToJump = jump < pc.gameSettingsModel.minInputToChangeState;
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
                if (col[i].gameObject.tag == "TopLadder")
                {
                    foundSelf = true;
                }
            }
            if (!foundSelf)
            {
                pc.ChangeState(new GroundedState(pc));
                return;
            }
        }
        if (inputY <= -pc.gameSettingsModel.minInputToChangeState && ableToUseLadder)
        {
            pc.ChangeState(new OnLadderState(pc));
        }
    }
    public override void OnStateEnter(PlayerController pc)
    {
        if (pc.GetLastState() is OnLadderState)
        {
            ableToJump = false;
            timeToJump = Time.time + (pc.gameSettingsModel.useLadderCooldownInFrames * Time.deltaTime);
            ableToUseLadder = false;
            timeToEnableLadder = Time.time + (pc.gameSettingsModel.useLadderCooldownInFrames * Time.deltaTime);
            pc.playerAnim.SetTrigger("smallJump");
            SmallJump(pc);
        }
        else
        {
            ableToUseLadder = true;
            ableToJump = true;
        }
    }

    public override void OnStateExit(PlayerController pc)
    {
        pc.playerAnim.SetBool("playerRun", false);
        pc.playerAnim.ResetTrigger("smallJump");
    }

    private void SmallJump(PlayerController pc)
    {
        pc.rb2d.AddForce(Vector2.up * pc.playerModel.jumpImpulse * pc.playerModel.ladderJumpMultiplier, ForceMode2D.Impulse);
    }

    public void EnableLadder()
    {
        if(Time.time >= timeToEnableLadder)
            ableToUseLadder = true;
    }
}