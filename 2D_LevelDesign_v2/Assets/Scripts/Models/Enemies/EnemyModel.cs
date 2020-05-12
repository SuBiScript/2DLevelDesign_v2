using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "EnemyModel", menuName = "Models/Enemies/Enemy", order = 2)]

public class EnemyModel : CharacterModel
{
    [Range(1, 10)]
    public int meleeDamage;
    [Range(1, 5)]
    public float visionRadius;
    [Range(0.1f, 2)]
    public float projectileDamage;
    [Range(0.1f, 4)]
    public float ProjectileSpeed;
}
