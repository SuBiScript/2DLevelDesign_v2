using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    [HideInInspector] public BossModel bossModel;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public Animator bossAnim;

    private BossState currentBossState;

	private void Start ()
    {
        GetComponents();
        bossModel = Instantiate(bossModel);
        ChangeState(new BossInitialState());
	}
	
    private void GetComponents()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();
    }

	private void Update ()
    {
        currentBossState.Update(this);
	}

    private void FixedUpdate()
    {
        currentBossState.FixedUpdate(this);
    }

    private void LateUpdate()
    {
        currentBossState.CheckTransition(this);
    }

    private void ChangeState(BossState bs)
    {
        currentBossState = bs;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bossModel.visionRadius);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player hit");
            //col.gameObject.GetComponent<PlayerHealthController>().HurtPlayer(damageToGive);
        }
    }
}
