using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaShootingState : RumbaState
{
    private float shootLoader;

    public RumbaShootingState(RumbaController rc)
    {
        shootLoader = 0.7f;
        rc.anim.SetBool("shooting", true);
    }

    public override void Update(RumbaController rc)
    {
        if (shootLoader <= 0)
        {
            AudioManager.instance.Play("RumbaShooting");
            GameObject projectile = GameManager.pool.GetPooledObject(GameManager.gameMaster.gameManagerModel.RumbaProjectile);
            if (projectile != null)
            {
                projectile.transform.position = rc.firePoint.transform.position;
                projectile.transform.rotation = rc.firePoint.transform.rotation;
                projectile.SetActive(true);
            }

            if (!rc.rumbaModel.following)
                rc.ChangeState(new RumbaPatrolState(rc));
            else
                rc.ChangeState(new RumbaFollowingState(rc));

            rc.anim.SetBool("shooting", false);
        }
        else
            shootLoader -= Time.deltaTime;
    }

    public override void FixedUpdate(RumbaController rc)
    {

    }

    public override void CheckTransition(RumbaController rc)
    {
        base.RumbaTransitionState(rc);
    }
}
