using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoldierState
{
    public abstract void Update(SoldierController sc);
    public abstract void FixedUpdate(SoldierController sc);
    public abstract void CheckTransition(SoldierController sc);

    //transition function between states for looking the player
    public void SoldierTransitionState(SoldierController sc)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D hit = Physics2D.Raycast(sc.transform.position, sc.GetPlayerCollider().transform.position - sc.transform.position, sc.soldierModel.visionRadius, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.transform.parent.tag == "Player")
                sc.soldierModel.shooting = true;
            else
                sc.soldierModel.shooting = false;
        }
        else
            sc.soldierModel.shooting = false;
    }
}
