using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RumbaModel", menuName = "Models/Enemies/Rumba", order = 0)]

public class RumbaModel : EnemyModel
{
    [Range(0.1f, 2)]
    public float startWaitTime;
    [Range(0.5f, 4)]
    public float maxPatrolDistance;
    [HideInInspector] public float[] coordPositions = new float[4]; //  0 = maxX; 1= minX; 2= maxY; 3 = minY
    [HideInInspector] public bool terrainDetected;
    [HideInInspector] public bool following; 
}
