using System.Collections;
using UnityEngine;
using System;

public class GrenadeLauncher : Weapons
{
    public int magazineCapacity;

    public int currentAmmo;
    float reloadTime;
    float nextTimeToReload;

    public GrenadeLauncher(GameObject projectile)
    {
        this.projectile = projectile;
        this.shootingCooldown = 1.0f;
        this.magazineCapacity = 6;
        this.currentAmmo = 6;
        this.reloadTime = 5.0f;
        this.ableToShoot = true;
    }

    public GrenadeLauncher(GameObject projectile, GameObject particles, int magazineCapacity, float shootingCooldown, float reloadTime)
    {
        this.projectile = projectile;
        this.shootingCooldown = shootingCooldown;
        this.magazineCapacity = magazineCapacity;
        this.currentAmmo = magazineCapacity;
        this.reloadTime = reloadTime;
        this.ableToShoot = true;
        this.particles = particles;
    }

    public override bool Shoot(out GameObject bullet)
    {
        bullet = this.projectile;
        if (ableToShoot && currentAmmo > 0)
        {
            currentAmmo--;
            ableToShoot = false;
            nextTimeToShoot = Time.time + shootingCooldown;
            nextTimeToReload = Time.time + reloadTime;
            return true;
        }
        return false;
    }

    public override void Update()
    {
        if (currentAmmo < magazineCapacity && nextTimeToReload <= Time.time)
        {
            GameManager.gameMaster.canvasManager.CanvasGrenadeAmmo(currentAmmo+1);
            currentAmmo++;
            nextTimeToReload = Time.time + reloadTime;
        }
        if (Time.time >= nextTimeToShoot)
        {
            ableToShoot = true;
        }
    }

    public override int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public override int GetMagazineCapacity()
    {
        return magazineCapacity;
    }
}
