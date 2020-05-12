using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RumbaState
{
    public abstract void Update(RumbaController rc);
    public abstract void FixedUpdate(RumbaController rc);
    public abstract void CheckTransition(RumbaController rc);

    //transition function between states for looking the player
    public void RumbaTransitionState(RumbaController rc)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D hit = Physics2D.Raycast(rc.transform.position, rc.GetPlayerCollider().transform.position - rc.transform.position, rc.rumbaModel.visionRadius, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.transform.parent.tag == "Player")
                rc.rumbaModel.following = true;
            else
                rc.rumbaModel.following = false;
        }
        else
            rc.rumbaModel.following = false;
    }
}
