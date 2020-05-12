using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma : Projectiles
{
    public float speed;

    private void OnEnable()
    {
        AudioManager.instance.Play("PlayerBlaster");
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            try
            {
                collision.gameObject.GetComponent<GenericHealthManager>().HurtCharacter(damage);
            }
            catch
            {
                Debug.Log(collision.gameObject.ToString() + " does not have a GenericHealthManager!");
            }
        }
        Disable();
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnBecameInvisible()
    {
        Disable();
    }
}
