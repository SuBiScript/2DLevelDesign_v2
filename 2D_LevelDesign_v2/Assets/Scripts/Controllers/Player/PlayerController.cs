using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public enum StartingState
{
    None, Grounded, OnAir, OnBottomLadder, OnTopLadder, OnLadder, Crouched
}

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    public PlayerModel playerModel;
    public GameSettingsModel gameSettingsModel;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public Animator playerAnim;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public GenericHealthManager healthManager;

    [Header("Required Colliders")]
    public Collider2D crouchedCollider;
    public Collider2D standingCollider;
    [Header("Positions")]
    [Tooltip("Where the collider for ground detection will be placed")]
    public Transform groundPoint;
    public ParticleSystem smokeRunEffect; //particle system
    public GameObject particlePoint;
    public GameObject particles1;
    public GameObject particles2;
    public Transform crouchedShootingPosition;
    public Transform standingShootingPosition;
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask ladderLayer;
    [Header("State Configuration")]
    public StartingState initialState;

    //Events
    [Header("Events")]
    [HideInInspector] public UnityEvent OnLadderEnter;
    [HideInInspector] public UnityEvent OnLadderExit;

    [Header("Protected and Hidden")]
    [HideInInspector] public Transform weaponPosition;
    [HideInInspector] public Vector2 aimingDirection;
    [HideInInspector] public float inputX;

    private PlayerState lastState;
    private PlayerState currentState;
    private bool pause;
    private CameraController cam;

    private void Start()
    {
        playerModel = Instantiate(playerModel);
        gameSettingsModel = Instantiate(gameSettingsModel);
        GetComponents();
        SetVariables();
        restoreKnockback();
        switch (initialState)
        {
            case StartingState.None:
                break;
            case StartingState.Grounded:
                ChangeState(new GroundedState(this));
                break;
            case StartingState.OnAir:
                ChangeState(new OnAirState(this));
                break;
            case StartingState.OnBottomLadder:
                ChangeState(new OnBottomLadderState(this));
                break;
            case StartingState.OnTopLadder:
                ChangeState(new OnTopLadderState(this));
                break;
            case StartingState.OnLadder:
                ChangeState(new OnLadderState(this));
                break;
            case StartingState.Crouched:
                ChangeState(new CrouchState(this));
                break;
            default:
                break;
        }
    }

    private void GetComponents()
    {
        try
        {
            rb2d = GetComponent<Rigidbody2D>();
            playerAnim = GetComponent<Animator>();
            healthManager = GetComponent<GenericHealthManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            cam = FindObjectOfType<CameraController>();
        }
        catch
        {
            throw new UnityException("Error in GetComponents, are you missing required components?");
        }
    }

    private void SetVariables()
    {
        weaponPosition = standingShootingPosition;
        aimingDirection = Vector2.right;
        particlePoint = particles1;
        pause = false;
        GameManager.gameMaster.pauseGame.AddListener(Pause);
        GameManager.gameMaster.resumeGame.AddListener(Resume);
    }

    private void Update()
    {
        if (!pause)
        {
            currentState.Update(this);
        }
    }
    private void FixedUpdate()
    {
        if (!pause)
        {
            currentState.FixedUpdate(this);
        }
    }
    private void LateUpdate()
    {
        if (!pause)
        {
            currentState.CheckTransition(this);
        }
    }

    public void ChangeState(PlayerState ps)
    {
        if (currentState != null)
        {
            currentState.OnStateExit(this);
            lastState = currentState;
        }
        currentState = ps;
        currentState.OnStateEnter(this);
    }

    public void Flip()
    {
        if (inputX > gameSettingsModel.minInputToChangeState * 0.5f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            aimingDirection = Vector2.right;
            return;
        }
        if (inputX < -gameSettingsModel.minInputToChangeState * 0.5f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            aimingDirection = Vector2.left;
            return;
        }
    }

    public PlayerState GetLastState()
    {
        return lastState;
    }

    public void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameManager.gameMaster.inventory.ShootEquippedWeapon(weaponPosition.position, aimingDirection);
        }
    }

    public void ColliderHit(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !healthManager.GetIsInvulnerable() && this.gameObject.activeSelf)
        {
            AudioManager.instance.Play("PlayerHurt");
            spriteRenderer.enabled = false;
            Vector2 vector = transform.position - collision.gameObject.transform.position;
            BounceOff(vector);
            Physics2D.IgnoreLayerCollision(9, 11, true);
            Invoke("restoreKnockback", playerModel.invulnerabilityDuration);
            cam.Shake(gameSettingsModel.camShakeAmount, gameSettingsModel.camShakeLenght); //camera shake
            healthManager.MakeInvulnerable();
        }
    }

    public void ColliderHit()
    {
        if (gameObject.activeSelf)
        {
            AudioManager.instance.Play("PlayerHurt");
            spriteRenderer.enabled = false;
            Vector2 vector = Quaternion.Euler(transform.rotation.eulerAngles) * Vector2.right;
            BounceOff(vector);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
            Invoke("restoreKnockback", playerModel.invulnerabilityDuration);
            cam.Shake(gameSettingsModel.camShakeAmount, gameSettingsModel.camShakeLenght); //camera shake
            healthManager.MakeInvulnerable();
        }
    }

    private void restoreKnockback()
    {
        Physics2D.IgnoreLayerCollision(9, 11, false);
    }

    private void BounceOff(Vector2 forceDir)
    {
        forceDir.y = Mathf.Abs(forceDir.x);
        forceDir.Normalize();
        rb2d.AddForce(forceDir * playerModel.knockbackForce, ForceMode2D.Impulse);
    }

    private void OnDisable()
    {
        if (AudioManager.instance != null && !pause)
        {
            AudioManager.instance.Stop("BossMusic");
            AudioManager.instance.Stop("LevelMusic");
            AudioManager.instance.Stop("MenuMusic");
            AudioManager.instance.Play("PlayerDead");
            AudioManager.instance.Play("GameOver");
        }
        if (GameManager.gameMaster != null && !pause)
        {
            GameManager.gameMaster.pauseGame.Invoke();
            GameManager.gameMaster.gameOver.Invoke();
        }
    }

    private void Pause()
    {
        pause = true;
        inputX = 0;
    }

    private void Resume()
    {
        pause = false;
    }
}