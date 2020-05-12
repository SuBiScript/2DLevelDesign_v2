using UnityEditor;
using UnityEngine;

public class OnAirState : PlayerState
{
    public OnAirState(PlayerController pc) { }
    public override void Update(PlayerController pc)
    {
        pc.Flip();
        pc.Shoot();
        pc.inputX = Input.GetAxis("Horizontal");
    }
    public override void FixedUpdate(PlayerController pc)
    {
        float horizontalVelocityOnAir = pc.inputX * pc.playerModel.horizontalForce * pc.playerModel.onAirFactor;
        pc.rb2d.AddForce(Vector2.right * horizontalVelocityOnAir, ForceMode2D.Force);

        float clampedSpeed = Mathf.Clamp(horizontalVelocityOnAir, -pc.playerModel.maxSpeed, pc.playerModel.maxSpeed);
        pc.rb2d.velocity = new Vector2(clampedSpeed, pc.rb2d.velocity.y);

    }
    public override void CheckTransition(PlayerController pc)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(pc.groundPoint.position, pc.playerModel.groundRadius, pc.groundLayer.value + pc.ladderLayer.value);
        if (col.Length != 0)
        {
            pc.ChangeState(new GroundedState(pc));
            return;
        }
    }
    public override void OnStateEnter(PlayerController pc)
    {
        pc.playerAnim.SetBool("playerJump", true);
        pc.rb2d.sharedMaterial = pc.playerModel.playerOnAirMaterial;
    }
    public override void OnStateExit(PlayerController pc)
    {
        pc.playerAnim.SetBool("playerJump", false);
        pc.rb2d.sharedMaterial = pc.playerModel.playerMaterial;
    }
}
