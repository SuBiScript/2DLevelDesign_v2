using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectiles
{
    [Header("Grenade Variables")]
    public bool destroyOnMaxBounce;
    public int currentBounces;
    public GameObject GrenadeSmoke;
    [Range(1, 10)] public int maxBounces = 3;
    [Range(0f, 10f)] public float maxEnabledDuration = 2.5f;
    [Range(0f, 50f)] public float throwForce = 2.0f;
    private float timeToDisable;
    private Rigidbody2D rb2d;

    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentBounces = 0;
        direction = Vector2.right;
    }


    public void OnEnable()
    {
        if (rb2d == null) rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0;
        currentBounces = 0;
        timeToDisable = Time.time + maxEnabledDuration;
        rb2d.AddForce((Vector2.up + direction) * throwForce, ForceMode2D.Impulse);
        AudioManager.instance.Play("GrenadeLauncher");
    }

    private void Update()
    {
        if (Time.time >= timeToDisable)
        {
            //Explode();
            Disable();
        }
    }
    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -3.0f, 2f));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            collision.gameObject.GetComponent<GenericHealthManager>().HurtCharacter(damage);
            //Explode();
            Disable();
            return;
        }
        currentBounces++;
        if (!destroyOnMaxBounce)
        {
            return;
        }
        if (currentBounces >= maxBounces)
        {
            //Explode();
            Disable();
            return;
        }
    }

    private void Explode()
    {
        Collider2D playerOverlap = Physics2D.OverlapCircle(transform.position, 0.66f, 1 << LayerMask.NameToLayer("Player"));
        Collider2D[] enemyOverlap = Physics2D.OverlapCircleAll(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Enemy"));
        Collider2D[] projectileOverlap = Physics2D.OverlapCircleAll(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Projectiles"));
        try
        {
            foreach (Collider2D hit in enemyOverlap)
            {
                Debug.DrawLine(hit.transform.position, transform.position);
                hit.GetComponent<GenericHealthManager>().HurtCharacter(damage);
                hit.GetComponent<Rigidbody2D>().AddForce((hit.transform.position - transform.position) * 15.0f, ForceMode2D.Impulse);
            }
        }
        catch
        {
            throw new UnityException("Something wrong in enemy overlap was wrong!");
        }

        try
        {
            foreach (Collider2D hit in projectileOverlap)
            {
                Debug.DrawLine(hit.transform.position, transform.position);
                hit.GetComponent<Grenade>().SetDetonationTimeRemaining(0.5f);
                hit.GetComponent<Rigidbody2D>().AddForce((hit.transform.position - transform.position) * 15.0f, ForceMode2D.Impulse);
            }
        }
        catch
        {
            throw new UnityException("Something wrong in projectile overlap!");
        }

        try
        {
            if (playerOverlap != null)
            {
                Debug.DrawLine(playerOverlap.transform.parent.position, transform.position);
                playerOverlap.transform.parent.GetComponent<GenericHealthManager>().HurtCharacter(5);
                playerOverlap.transform.parent.GetComponent<Rigidbody2D>().AddForce((playerOverlap.transform.position - transform.position) * 30.0f, ForceMode2D.Impulse);
            }
        }
        catch
        {
            throw new UnityException("Something wrong in player overlap!");
        }
    }

    public void SetDetonationTimeRemaining(float time)
    {
        timeToDisable = Time.time + time;
    }

    public override void Disable()
    {
        //Disable the item AND play the explosion at the position
        //Instantiate the explosion ma dood.
        //Debug.Log(this.ToString() + " exploded");
        AudioManager.instance.Play("GrenadeExplosion");
        rb2d.velocity = Vector2.zero;
        Instantiate(GrenadeSmoke, this.gameObject.transform.position, this.gameObject.transform.rotation);
        this.gameObject.SetActive(false);
    }
}
