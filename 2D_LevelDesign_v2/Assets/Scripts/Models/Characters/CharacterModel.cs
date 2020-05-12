using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : ScriptableObject
{
    [Range(1, 500)]
    public float health;
    [Range(0.5f, 10)]
    public float maxSpeed;
    [Range(0f, 5f)] [Tooltip("Amount of seconds of invulnerability when damaged")] public float invulnerabilityDuration;
    public int pointsToGive;  
}
