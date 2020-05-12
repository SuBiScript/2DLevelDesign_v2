using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerModel", menuName = "Models/Testing/GameManagerModel", order = 1)]

public class GameManagerModel : ScriptableObject
{
    [Header("HP Medkit")]
    [Range(1, 10f)] public int hpToGive;
    [Header("Requirements")]
    public int framesToReactivateCollision;
    public GameObject SoldierProjectile;
    public GameObject RumbaProjectile;
    public GameObject GrenadeProjectile;
    public GameObject PlasmaProjectile;
    public GameObject BossProjectile;
}