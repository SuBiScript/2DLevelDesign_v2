using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaFollowingState : RumbaState
{
    private float shootLoader = 1.5f;

    public RumbaFollowingState(RumbaController rc)
    {

    }

    public override void Update(RumbaController rc)
    {
        if (!rc.rumbaModel.following)
            rc.ChangeState(new RumbaPatrolState(rc));

        if (shootLoader <= 0 && Vector2.Distance(rc.transform.position, rc.GetPlayerCollider().transform.position) > 1f)
            rc.ChangeState(new RumbaShootingState(rc));
        else
            shootLoader -= Time.deltaTime;

        if (rc.GetPlayerCollider().transform.position.x > rc.transform.position.x)
            rc.transform.eulerAngles = new Vector3(0, 0, 0);
        else
            rc.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public override void FixedUpdate(RumbaController rc)
    {
        if (Vector2.Distance(rc.transform.position, rc.GetPlayerCollider().transform.position) > 0.2f)
            rc.rb2d.transform.position = Vector2.MoveTowards(rc.transform.position, rc.GetPlayerCollider().transform.position, (rc.rumbaModel.maxSpeed - 0.1f) * Time.fixedDeltaTime);
    }

    public override void CheckTransition(RumbaController rc)
    {
        base.RumbaTransitionState(rc);
    }
}
