using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //public attributes
    [HideInInspector] public BossModel bossModel;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public Animator bossAnim;
    [HideInInspector] public bool timeToChange;
    [HideInInspector] public bool readyToChange;
    [HideInInspector] public bool crouchTime;
    [HideInInspector] public bool meleeAttack;
    [HideInInspector] public Collider2D colMeleeDamage;
    [HideInInspector] public bool gameOver;
    public Transform firePoint;
    public Transform meleePoint;
    public float meleeRange;

    //private attributes
    private Collider2D playerCol;
    private BossState currentBossState;
    private bool paused;

    private void Start()
    {
        GetComponents();
        bossModel = Instantiate(bossModel);
        ChangeState(new BossInitialState(this));
        GameManager.gameMaster.pauseGame.AddListener(Pause);
        GameManager.gameMaster.resumeGame.AddListener(Resume);
    }

    private void GetComponents()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {

        if (!timeToChange)
        {
            timeToChange = true;
            StartCoroutine(TimeToChange());
        }

        if (crouchTime)
            ForcingCharge();

        if (meleeAttack)
            StartCoroutine(TimeToMeleeAttack());


        Flip();
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

    public void ChangeState(BossState bs)
    {
        currentBossState = bs;
    }

    public Collider2D GetPlayerCollider()
    {
        return playerCol;
    }

    private void Flip()
    {
        if (GetPlayerCollider().transform.position.x > transform.position.x)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    IEnumerator TimeToChange()
    {
        yield return new WaitForSeconds(8.5f); //time to charge (bossmodel)
        readyToChange = true;
    }

    IEnumerator TimeToMeleeAttack()
    {
        var colMD = colMeleeDamage;
        meleeAttack = false;
        yield return new WaitForSeconds(0.2f); //time to melee attack
        if (GameManager.gameMaster.PlayerController.gameObject.activeSelf && !GameManager.gameMaster.PlayerController.healthManager.GetIsInvulnerable())
        {
            bool check = false;
            try
            {
                check = colMD.transform.parent.tag == "Player";
            }
            catch
            {
                //Nothing
            }
            if (check)
            {
                colMD.GetComponentInParent<PlayerController>().healthManager.HurtCharacter(bossModel.meleeDamage + 2);
                GameManager.gameMaster.PlayerController.ColliderHit();
            }
        }
    }

    public void CooldownAnimation()
    {
        bossAnim.SetBool("bossMelee", false);
        bossAnim.SetBool("bossCooldown", true);
    }

    public void FollowingState()
    {
        bossAnim.SetBool("bossCooldown", false);
        ChangeState(new BossFollowingState(this));
    }

    public void ForcingCharge()
    {
        crouchTime = false;
        bossAnim.SetBool("bossShoot", false);

        if (Vector2.Distance(transform.position, playerCol.transform.position) >= 0.8f)
            ChangeState(new BossChargeState(this));
        else
            ChangeState(new BossMeleeState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bossModel.visionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<GenericHealthManager>().HurtCharacter(bossModel.meleeDamage);
        }
    }

    private void OnDestroy()
    {
        if (AudioManager.instance != null && !paused)
        {
            AudioManager.instance.Play("BossExplosion");
            AudioManager.instance.Play("YouWin");
            AudioManager.instance.Stop("BossMusic");
        }
        if (GameManager.gameMaster != null)
        {
            GameManager.gameMaster.pauseGame.Invoke();
            GameManager.gameMaster.gameWin.Invoke();
        }
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
