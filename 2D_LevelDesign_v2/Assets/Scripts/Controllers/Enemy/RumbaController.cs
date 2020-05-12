using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaController : MonoBehaviour
{
    //public attributes
    [HideInInspector] public RumbaModel rumbaModel;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public Animator anim;
    public Transform movingSpot;
    public Transform firePoint;


    //private attributes
    private RumbaState currentState;
    private Collider2D playerCol;
    private bool paused;

    private void Start()
    {
        GetComponents();
        rumbaModel = Instantiate(rumbaModel);
        ChangeState(new RumbaPatrolState(this));
        GameManager.gameMaster.pauseGame.AddListener(Pause);
        GameManager.gameMaster.resumeGame.AddListener(Resume);
    }

    private void GetComponents()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Collider2D>();
        if (playerCol == null)
            throw new UnityException("There is no player in the scene!");
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

    public void ChangeState(RumbaState rs)
    {
        currentState = rs;
    }

    public Collider2D GetPlayerCollider()
    {
        return playerCol;
    }

    // Sphere Radio Vision 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rumbaModel.visionRadius);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<GenericHealthManager>().HurtCharacter(rumbaModel.meleeDamage);
        }

        if (col.gameObject.tag == "Ground")
            rumbaModel.terrainDetected = true;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
            rumbaModel.terrainDetected = true;
    }

    private void OnDestroy()
    {
        if (AudioManager.instance != null && !paused)
            AudioManager.instance.Play("RumbaExplosion");
    }

    private void Pause()
    {
        paused = true;
    }
    private void Resume()
    {
        paused = false;
    }
}
