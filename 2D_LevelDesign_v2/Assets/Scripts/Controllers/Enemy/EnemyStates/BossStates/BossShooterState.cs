using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooterState : BossState {

    public float timeShooting;

    public BossShooterState(BossController bc)
    {
        bc.bossAnim.SetBool("bossShoot", true);
        timeShooting = 3.5f;
        AudioManager.instance.Play("BossVoice");
    }

    public override void Update(BossController bc)
    {
        Shoot(bc);

        if (Vector2.Distance(bc.transform.position, bc.GetPlayerCollider().transform.position) > 3.5f)
        {
            bc.bossAnim.SetBool("bossShoot", false);
            bc.ChangeState(new BossFollowingState(bc));
        }

        if (timeShooting < 0)
            bc.crouchTime = true;
        else
            timeShooting -= Time.deltaTime;
    }

    public override void FixedUpdate(BossController bc)
    {
  
    } 

    private void Shoot(BossController bc)
    {
        GameObject projectile = GameManager.pool.GetPooledObject(GameManager.gameMaster.gameManagerModel.BossProjectile);
        if (bc.bossModel.shootCounter <= 0)
        {
            AudioManager.instance.Play("BossShoot");
            bc.bossModel.shootCounter = bc.bossModel.timeBetweenShots;
            projectile.transform.position = bc.firePoint.transform.position;
            projectile.transform.rotation = bc.firePoint.transform.rotation;
            projectile.SetActive(true);         
        }
        bc.bossModel.shootCounter -= Time.deltaTime;
    }

    public override void CheckTransition(BossController bc)
    {
        base.BossTransitionState(bc);
    }
}
