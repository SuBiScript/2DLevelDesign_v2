using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileController : MonoBehaviour
{

    [Range(1, 5)]
    public float impulse;
    [Range(1, 5)]
    public int damageToGive;
    public Sprite bossProjectile;
    private Rigidbody2D rb2D;

    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * impulse;
    }


    void OnBecameInvisible()
    {
        Destroy();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //if (col.gameObject.GetComponent<PlayerController>().IsVulnerable())           
            col.gameObject.GetComponent<GenericHealthManager>().HurtCharacter(damageToGive);
            Destroy();
        }

        if (col.gameObject.tag == "Enemy")
        {
            Destroy();
        }

        if (col.gameObject.tag == "Ground")
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
