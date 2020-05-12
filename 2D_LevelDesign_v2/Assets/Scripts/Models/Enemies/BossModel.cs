using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossModel", menuName = "Models/Enemies/Boss", order = 3)]

public class BossModel : EnemyModel
{
    [Range(0.1f, 8)]
    public float timeBetweenShots;
    [Range(0.1f, 10f)]
    public float timeBetweenMelee;
    [Range(0.1f, 10f)]
    public float skillDuration;
    [Range(0.1f, 10f)]
    public float introTime;
    [Range(0.1f, 10f)]
    public float chargeSpeed;
    [Range(0.1f, 10f)]
    public float ShootingTime;
    [HideInInspector] public float shootCounter;
    [HideInInspector] public bool following = false;
}
