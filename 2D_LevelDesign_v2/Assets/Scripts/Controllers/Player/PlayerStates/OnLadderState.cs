using System;
using UnityEngine;

public class OnLadderState : PlayerState
{
    float inputY;
    bool jump;
    bool firstFrame;
    float timeToClimb;

    public OnLadderState(PlayerController pc)
    {
        timeToClimb = Time.time;
    }

    public override void Update(PlayerController pc)
    {
        inputY = Input.GetAxis("Vertical");
        jump = Input.GetButton("Jump");
        if (!firstFrame)
        {
            if (inputY > pc.gameSettingsModel.minInputToChangeState)
            {
                pc.playerAnim.enabled = true;
            }
            else if (inputY < pc.gameSettingsModel.minInputToChangeState)
            {
                pc.playerAnim.enabled = false;
            }
            return;
        }
        if (firstFrame)
        {
            firstFrame = false;
        }
    }

    public override void FixedUpdate(PlayerController pc)
    {
        if (timeToClimb <= Time.time && inputY > pc.gameSettingsModel.minInputToChangeState)
        {
            pc.transform.position += new Vector3(0, pc.playerModel.climbLadderDistance, 0);
            timeToClimb = Time.time + pc.playerModel.upLadderClimbDelay;
            return;
        }
        if (inputY < -pc.gameSettingsModel.minInputToChangeState)
        {
            pc.transform.position += new Vector3(0, inputY * pc.playerModel.downLadderSpeedMultiplier * Time.fixedDeltaTime, 0);
        }
    }
    public override void CheckTransition(PlayerController pc)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(pc.groundPoint.position, pc.playerModel.groundRadius, pc.groundLayer.value + pc.ladderLayer.value);
        if (col.Length != 0)
        {
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].gameObject.tag == "BottomLadder" && inputY <= -pc.gameSettingsModel.minInputToChangeState)
                {
                    pc.ChangeState(new OnBottomLadderState(pc));
                    return;
                }
                else if (col[i].gameObject.tag == "TopLadder" && inputY >= pc.gameSettingsModel.minInputToChangeState)
                {
                    pc.ChangeState(new OnTopLadderState(pc));
                    return;
                }
            }
        }
    }
    public override void OnStateEnter(PlayerController pc)
    {
        firstFrame = true;
        pc.OnLadderEnter.Invoke();
        SetPlayerPositionOnLadder(pc); //This sets the player Position on the ladder.
        pc.playerAnim.SetBool("playerClimbLadder", true);
        pc.playerAnim.ResetTrigger("returnToIdle");
        pc.rb2d.isKinematic = true;
        pc.rb2d.velocity = Vector3.zero;
        pc.rb2d.gravityScale = 0;
        pc.playerAnim.enabled = true;
    }
    public override void OnStateExit(PlayerController pc)
    {
        pc.rb2d.isKinematic = false;
        pc.playerAnim.SetBool("playerClimbLadder", false);
        pc.rb2d.gravityScale = 1;
        pc.OnLadderExit.Invoke();
        pc.playerAnim.enabled = true;
    }

    private void SetPlayerPositionOnLadder(PlayerController pc)
    {
        //Gets the ladder position on the world and sets the player right into the ladder, so the animation can start.
        float xPos = GetLadderXPostion(pc);
        PlayerState lastState = pc.GetLastState();
        float offset = 0.0f;
        if (xPos != -1)
        {
            if (lastState is OnTopLadderState)
                offset = pc.playerModel.ladderOffset;
            pc.transform.position = new Vector2(xPos, pc.transform.position.y + offset);
        }
        else
        {
            throw new Exception("Did not found a ladder! OMG!");
        }
    }

    private float GetLadderXPostion(PlayerController pc)
    {
        //Get the ladder X position and return it if there is a ladder on the player. You never know :D
        int var = -1;
        Collider2D[] col = Physics2D.OverlapCircleAll(pc.groundPoint.position, pc.playerModel.groundRadius, pc.groundLayer.value + pc.ladderLayer.value);
        if (col.Length != 0)
        {
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].gameObject.tag == "BottomLadder" && pc.GetLastState() is OnBottomLadderState || col[i].gameObject.tag == "TopLadder" && pc.GetLastState() is OnTopLadderState)
                {
                    var = i;
                    break;
                }
            }
        }
        if (var >= 0)
        {
            return col[var].transform.position.x;
        }
        else return var;
    }
}
