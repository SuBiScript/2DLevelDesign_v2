using System.Collections;
using UnityEngine;
using System;

public abstract class Weapons
{
    protected float shootingCooldown;
    protected bool ableToShoot;
    protected GameObject projectile;
    protected GameObject particles;
    protected GameObject sound;
    protected float nextTimeToShoot;

    public abstract bool Shoot(out GameObject bullet);
    public abstract void Update();
    public abstract int GetCurrentAmmo();
    public abstract int GetMagazineCapacity();
    public GameObject GetParticles()
    {
        return particles;
    }
    public GameObject GetSound()
    {
        return sound;
    }
}