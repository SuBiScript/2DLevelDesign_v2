using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StandingCollider : MonoBehaviour
{
    private PlayerController player;
    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.ColliderHit(collision);
    }
}
