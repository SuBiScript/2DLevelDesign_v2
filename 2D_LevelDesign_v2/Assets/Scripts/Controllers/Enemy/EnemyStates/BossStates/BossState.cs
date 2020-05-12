using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{
    public abstract void Update(BossController bc);
    public abstract void FixedUpdate(BossController bc);
    public abstract void CheckTransition(BossController bc);

    public void BossTransitionState(BossController bc)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D hit = Physics2D.Raycast(bc.transform.position, bc.GetPlayerCollider().transform.position - bc.transform.position, bc.bossModel.visionRadius, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.transform.parent.tag == "Player")
                bc.bossModel.following = true;
            else
            {
                if (!bc.gameOver)
                {
                    bc.ChangeState(new BossIdleState(bc));
                    bc.gameOver = !bc.gameOver;
                }
            }

        }
    }
}
