using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaRifle : Weapons
{
    public PlasmaRifle(GameObject projectile)
    {
        this.projectile = projectile;
        this.ableToShoot = true;
        this.shootingCooldown = 0.25f;
    }

    public PlasmaRifle(GameObject projectile, GameObject particles,float shootingCooldown)
    {
        this.projectile = projectile;
        this.ableToShoot = true;
        this.shootingCooldown = shootingCooldown;
        this.particles = particles;
    }

    public override bool Shoot(out GameObject bullet)
    {
        bullet = this.projectile;
        if (ableToShoot)
        {
            ableToShoot = false;
            nextTimeToShoot = Time.time + shootingCooldown;
            return true;
        }
        return false;
    }

    public override void Update()
    {
        if (Time.time >= nextTimeToShoot)
        {
            ableToShoot = true;
        }
    }
    public override int GetCurrentAmmo()
    {
        throw new System.NotImplementedException();
    }
    public override int GetMagazineCapacity()
    {
        throw new System.NotImplementedException();
    }
}
