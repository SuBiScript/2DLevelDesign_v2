using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaPatrolState : RumbaState
{
    //pubilc attribues
    private float waitTime;

    public RumbaPatrolState(RumbaController rc)
    {
        PatrolCoords(rc);
        waitTime = rc.rumbaModel.startWaitTime;
    }

    public override void Update(RumbaController rc)
    {
        if (Vector2.Distance(rc.transform.position, rc.movingSpot.position) < 0.2f || rc.rumbaModel.terrainDetected == true)
        {
            if (waitTime <= 0)
            {
                rc.rumbaModel.terrainDetected = false;
                rc.movingSpot.position = new Vector2(Random.Range(rc.rumbaModel.coordPositions[0], rc.rumbaModel.coordPositions[1]), Random.Range(rc.rumbaModel.coordPositions[2], rc.rumbaModel.coordPositions[3]));
                waitTime = rc.rumbaModel.startWaitTime;
            }
            else
                waitTime -= Time.deltaTime;
        }

        if (rc.rumbaModel.following)
            rc.ChangeState(new RumbaFollowingState(rc));
    }
    public override void FixedUpdate(RumbaController rc)
    {
        if (!rc.rumbaModel.terrainDetected)
            rc.rb2d.transform.position = Vector2.MoveTowards(rc.transform.position, rc.movingSpot.position, rc.rumbaModel.maxSpeed * Time.fixedDeltaTime);
    }
    public override void CheckTransition(RumbaController rc)
    {
        base.RumbaTransitionState(rc);
    }

    void PatrolCoords(RumbaController rc)
    {
        for (int i = 0; i < rc.rumbaModel.coordPositions.Length; i++)
        {
            switch (i)
            {
                case 0:
                    rc.rumbaModel.coordPositions[i] = rc.transform.position.x + rc.rumbaModel.maxPatrolDistance; //maxX
                    break;
                case 1:
                    rc.rumbaModel.coordPositions[i] = rc.transform.position.x - rc.rumbaModel.maxPatrolDistance; // minX
                    break;
                case 2:
                    rc.rumbaModel.coordPositions[i] = rc.transform.position.y + rc.rumbaModel.maxPatrolDistance; // maxY
                    break;
                default:
                    rc.rumbaModel.coordPositions[i] = rc.transform.position.y - rc.rumbaModel.maxPatrolDistance; //minY
                    break;
            }
        }
        rc.movingSpot.position = new Vector2(Random.Range(rc.rumbaModel.coordPositions[0], rc.rumbaModel.coordPositions[1]), Random.Range(rc.rumbaModel.coordPositions[2], rc.rumbaModel.coordPositions[3]));
    }
}
