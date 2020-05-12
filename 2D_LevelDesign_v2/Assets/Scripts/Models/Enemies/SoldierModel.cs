using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierModel", menuName = "Models/Enemies/Soldier", order = 1)]

public class SoldierModel : EnemyModel
{
    [Range(0.1f, 4)]
    public float timeBetweenShots;
    [HideInInspector] public float shootCounter;
    [HideInInspector] public bool movingRight = true;
    [HideInInspector] public bool shooting = false;
}
