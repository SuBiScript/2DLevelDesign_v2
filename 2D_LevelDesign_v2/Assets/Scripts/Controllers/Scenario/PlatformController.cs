using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    [HideInInspector] public PlatformModel platform;
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public SpriteRenderer spriteRend;

    private PlatformState currentState;

    public GameObject Positions;

    void Start()
    {
        platform = Instantiate(platform);
        rb2D = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        //MovePositions

        ChangeState(new PlatformMovement());

        currentState.Init(this);
    }

    void Update()
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

    public void ChangeState(PlatformState ps)
    {
        currentState = ps;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
            col.collider.transform.parent.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
            col.collider.transform.parent.SetParent(null);
    }
}
