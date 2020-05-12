using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    //public attributes
    [HideInInspector] public SoldierModel soldierModel;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public Animator anim;
    public Transform groundPoint;
    public Transform firePoint;

    //private attributes
    private SoldierState currentState;
    private Collider2D playerCol;

    private void Start()
    {
        GetComponents();
        soldierModel = Instantiate(soldierModel);
        ChangeState(new SoldierPatrolState(this));
    }

    private void GetComponents()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        currentState.Update(this);
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }
    private void LateUpdate()
    {
        currentState.CheckTransition(this);
    }

    public void ChangeState(SoldierState es)
    {
        currentState = es;
    }

    public Collider2D GetPlayerCollider()
    {
        return playerCol;
    }

    // Sphere Radio Vision 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, soldierModel.visionRadius);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<GenericHealthManager>().HurtCharacter(soldierModel.meleeDamage);
        }
    }
    
    void OnDestroy()
    {
        //if (gameObject.activeSelf)
        AudioManager.instance.Play("SoldierDead");
    }
}
