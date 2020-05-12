using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModel", menuName = "Models/Testing/PlayerModel", order = 0)]

public class PlayerModel : CharacterModel
{
    public float damage; //For now it's useless for the character.
    [Header("Mid-air settings")]
    [Tooltip("Speed multiplier while mid-air")] [Range(0, 1.0f)] public float onAirFactor;
    [Header("Force settings")]
    [Range(1, 25)] public float jumpImpulse;
    [Range(1, 20)] public float horizontalForce;
    [Range(1, 20)] public float knockbackForce;
    public float throwForceFactor;
    //[Range(1, 20)] public float verticalForce;
    [Header("Collider settings")]
    [Range(0.005f, 0.1f)] public float groundRadius;
    [Header("Ladder settings")]
    [Tooltip("Multiplier for the end jump on the ladder")][Range(0f, 1.0f)] public float ladderJumpMultiplier;
    [Range(1.0f, 3.0f)] public float downLadderSpeedMultiplier;
    [Range(0.0f, 1.0f)] public float upLadderClimbDelay;
    [Range(0.0f, 3.5f)] public float climbLadderDistance;
    [Range(-1.0f, 1.0f)] public float ladderOffset;
    [Header("Requirements")]
    public PhysicsMaterial2D playerMaterial;
    public PhysicsMaterial2D playerOnAirMaterial;
    public GameObject PlayerRunSmoke; //Particle System
}

