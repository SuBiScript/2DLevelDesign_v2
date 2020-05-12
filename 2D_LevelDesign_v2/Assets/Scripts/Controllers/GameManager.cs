using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMaster;
    public static GameManager Instance { get { return gameMaster; } }

    //[HideInInspector]
    public GameManagerModel gameManagerModel;

    [HideInInspector] public GameObject player;
    [HideInInspector] public static ObjectPoolerController pool;
    [HideInInspector] public InventoryExtra inventory;
    [HideInInspector] public AudioManager audioManager;

    public ParticleList particleList;
    public GameObject inventoryCanvas;
    public CanvasManager canvasManager;
    public GameObject startingWeapon;
    public GameObject GameOverMenu, pauseMenu;
    public GameOverController youwinPanel;

    public UnityEvent MasterUpdate;
    public UnityEvent gameOver;
    public UnityEvent gameWin;
    public UnityEvent pauseGame;
    public UnityEvent resumeGame;

    public PlayerController PlayerController;  
    public new CameraController camera; // camera

    private Collider2D lastDisabledCollider;

    private void Awake()
    {
        if (gameMaster != null && gameMaster != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameMaster = this;
        }
        gameManagerModel = Instantiate(gameManagerModel);
        player = GameObject.FindGameObjectWithTag("Player");
        MasterUpdate = new UnityEvent();
        inventory = new InventoryExtra();
        pool = new ObjectPoolerController();
        particleList = Instantiate(particleList);
        audioManager = FindObjectOfType<AudioManager>();
        gameOver = new UnityEvent();
        gameWin = new UnityEvent();
        pauseGame = new UnityEvent();
        resumeGame = new UnityEvent();
    }

    void Start()
    {
        pool.StorePoolerObject(2, gameManagerModel.SoldierProjectile); // adding 8 soldier bullets in pooler
        pool.StorePoolerObject(2, gameManagerModel.RumbaProjectile); // adding 2 rumba bullets in pooler
        //pool.StorePoolerObject(0, gameManagerModel.GrenadeProjectile); // adding 2 player grenades in pooler*/
        pool.StorePoolerObject(3, gameManagerModel.BossProjectile); // adding 2 boss bullets in pooler*/

        PlayerController = player.GetComponent<PlayerController>();
        PlayerController.OnLadderEnter.AddListener(DisablePlatformColliders);
        PlayerController.OnLadderExit.AddListener(EnablePlatformColliders);

        gameOver.AddListener(GameOver);
        gameWin.AddListener(GameWin);
    }

    private void Update()
    {
        MasterUpdate.Invoke();
    }

    private void DisablePlatformColliders() //Desactiva el collider más cercano en una dirección en la Layer "Ground" para poder atravesarlo
    {
        Vector2 direction;
        if (PlayerController.GetLastState() is OnTopLadderState)
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.up;
        }
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, Mathf.Infinity, PlayerController.groundLayer);
        Debug.DrawLine(player.transform.position, hit.point);
        if (hit)
        {
            lastDisabledCollider = hit.collider;
            Physics2D.IgnoreCollision(player.GetComponentInChildren<Collider2D>(), lastDisabledCollider, true);
        }
        else
        {
            Debug.Log("NO PLATFORM ABOVE DETECTED!");
        }
    }

    private void EnablePlatformColliders()
    {
        if (lastDisabledCollider != null)
            Physics2D.IgnoreCollision(player.GetComponentInChildren<Collider2D>(), lastDisabledCollider, false);
        lastDisabledCollider = null;
    }

    public void instantiatePrefab(GameObject obj, Transform parent, bool boolean)
    {
        Instantiate(obj, parent, boolean);
    }

    private void GameOver()
    {
        if (GameOverMenu != null)
            GameOverMenu.SetActive(true);
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    private void GameWin()
    {
        if (GameOverMenu != null) 
        {
            GameOverMenu.SetActive(true);
            youwinPanel.SetYouWinPanel();
        }
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }
}
