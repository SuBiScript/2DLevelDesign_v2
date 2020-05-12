using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPatrolState : SoldierState
{

    //private attributes
    private const float distance = 0.1f;

    public SoldierPatrolState(SoldierController sc)
    {
        sc.anim.SetBool("shooting", false);
    }

    public override void Update(SoldierController sc)
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(sc.groundPoint.position, Vector2.down, distance); // origin, direction, lenght

        if (groundInfo.collider == false)
            Flip(sc);

        if (sc.soldierModel.shooting)
            sc.ChangeState(new SoldierShootingState(sc));
    }
    public override void FixedUpdate(SoldierController sc)
    {
        sc.rb2d.transform.Translate(Vector2.right * sc.soldierModel.maxSpeed * Time.fixedDeltaTime);
    }
    public override void CheckTransition(SoldierController sc)
    {
        base.SoldierTransitionState(sc);
    }

    private void Flip(SoldierController sc)
    {
        if (sc.soldierModel.movingRight == true)
        {
            sc.transform.eulerAngles = new Vector3(0, 180, 0);
            sc.soldierModel.movingRight = false;
        }
        else
        {
            sc.transform.eulerAngles = new Vector3(0, 0, 0);
            sc.soldierModel.movingRight = true;
        }
    }
}
