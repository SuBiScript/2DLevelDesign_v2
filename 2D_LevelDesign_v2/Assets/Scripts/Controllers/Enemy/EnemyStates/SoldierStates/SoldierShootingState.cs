using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierShootingState : SoldierState
{
    public SoldierShootingState(SoldierController sc)
    {
        sc.anim.SetBool("shooting", true);    
    }
    public override void Update(SoldierController sc)
    {
        if (sc.GetPlayerCollider().transform.position.x > sc.transform.position.x)
            sc.transform.eulerAngles = new Vector3(0, 0, 0);
        else
            sc.transform.eulerAngles = new Vector3(0, 180, 0);

        if (!sc.soldierModel.shooting)
            sc.ChangeState(new SoldierPatrolState(sc));

        GameObject projectile = GameManager.pool.GetPooledObject(GameManager.gameMaster.gameManagerModel.SoldierProjectile);

        if (projectile != null)
        {
            if (sc.soldierModel.shootCounter <= 0)
            {
                AudioManager.instance.Play("SoldierBlaster");
                sc.soldierModel.shootCounter = sc.soldierModel.timeBetweenShots;
                projectile.transform.position = sc.firePoint.transform.position;
                projectile.transform.rotation = sc.firePoint.transform.rotation;
                projectile.SetActive(true);
            }
            sc.soldierModel.shootCounter -= Time.deltaTime;
        }
    }
    public override void FixedUpdate(SoldierController sc)
    {

    }
    public override void CheckTransition(SoldierController sc)
    {
        base.SoldierTransitionState(sc);
    }
}
