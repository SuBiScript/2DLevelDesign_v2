using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeState : BossState
{

    public BossMeleeState(BossController bc)
    {
        AudioManager.instance.Play("BossMelee");
        AudioManager.instance.Play("Combo");
        bc.bossAnim.SetBool("bossMelee", true);
        bc.colMeleeDamage = Physics2D.OverlapCircle(bc.meleePoint.position, bc.meleeRange, 1 << LayerMask.NameToLayer("Player"));

        if (GameManager.gameMaster.PlayerController.gameObject.activeSelf)
            bc.meleeAttack = true;          
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
