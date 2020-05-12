using UnityEngine;

[CreateAssetMenu(fileName = "SettingsModel", menuName = "Models/Settings/SettingsModel", order = 0)]

public class GameSettingsModel : ScriptableObject
{
    [Header("Input Configuration")]
    [Range(0, 1.0f)] public float minInputToChangeState;

    [Header("Ladder Configuration")]
    public int useLadderCooldownInFrames;

    [Header("Invulnerability config")]
    public float invulnerabilityFlashDuration;

    [Header("Camera shake attributes")]
    [Range(0, 0.15f)]
    public float camShakeAmount;
    [Range(0, 0.30f)]
    public float camShakeLenght;
}
