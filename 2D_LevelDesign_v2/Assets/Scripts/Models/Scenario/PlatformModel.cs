using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Platform", menuName = "Models/Scenario/Platform", order = 0)]
public class PlatformModel : ScriptableObject
{
    public float maxSpeed = 5;
    public Vector3 position;
    
}