using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundedState : PlayerState
{
    protected float jump;
    protected bool crouch;
    protected bool ableToJump = false;

    public GroundedState(PlayerController pc)
    {
        pc.rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Update(PlayerController pc)
    {
        pc.Flip();
        pc.Shoot();
        crouch = Input.GetButton("Crouch");
        pc.inputX = Input.GetAxis("Horizontal");
        var emission = pc.smokeRunEffect.emission; // partycle system
        if (pc.inputX < -pc.gameSettingsModel.minInputToChangeState || pc.inputX > pc.gameSettingsModel.minInputToChangeState)
        {
            emission.rateOverTime = 8f;
            pc.playerAnim.SetBool("playerRun", true);
        }
        else
        {
            emission.rateOverTime = 0f;
            pc.playerAnim.SetBool("playerRun", false);
            pc.playerAnim.SetTrigger("returnToIdle");
        }

        jump = Input.GetAxis("Jump");
        if (!ableToJump)
            ableToJump = jump < pc.gameSettingsModel.minInputToChangeState;
            //FindObjectOfType<AudioManager>().Play("PlayerHurt");
    }
    public override void FixedUpdate(PlayerController pc)
    {
        pc.rb2d.AddForce(Vector2.right * pc.inputX * pc.playerModel.horizontalForce, ForceMode2D.Force);

        float clampedSpeed = Mathf.Clamp(pc.inputX * pc.playerModel.horizontalForce, -pc.playerModel.maxSpeed, pc.playerModel.maxSpeed);
        pc.rb2d.velocity = new Vector2(clampedSpeed, pc.rb2d.velocity.y);

        if (jump >= pc.gameSettingsModel.minInputToChangeState && ableToJump)
        {
            //Debug.Log("jumping");          
            pc.rb2d.AddForce(Vector2.up * pc.playerModel.jumpImpulse, ForceMode2D.Impulse);
            ableToJump = false;
            AudioManager.instance.Play("PlayerJump");
        }
    }
    public override void CheckTransition(PlayerController pc)
    {
        if (crouch)
        {
            pc.ChangeState(new CrouchState(pc));
            return;
        }
        Collider2D[] col = Physics2D.OverlapCircleAll(pc.groundPoint.position, pc.playerModel.groundRadius, pc.groundLayer.value + pc.ladderLayer.value);
        if (col.Length == 0)
        {
            pc.ChangeState(new OnAirState(pc));
            return;
        }
        else
        {
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].gameObject.tag == "BottomLadder")
                {
                    pc.ChangeState(new OnBottomLadderState(pc));
                    return;
                }
                else if (col[i].gameObject.tag == "TopLadder")
                {
                    pc.ChangeState(new OnTopLadderState(pc));
                    return;
                }
            }
        }
    }
    public override void OnStateEnter(PlayerController pc)
    {
        if (!(pc.GetLastState() is OnLadderState))
            pc.playerAnim.SetTrigger("returnToIdle");

        var emission = pc.smokeRunEffect.emission; //particle system
        emission.rateOverTime = 8f;
    }
    public override void OnStateExit(PlayerController pc)
    {
        var emission = pc.smokeRunEffect.emission;
        emission.rateOverTime = 0f;
        pc.playerAnim.SetBool("playerRun",false);
    }
}

