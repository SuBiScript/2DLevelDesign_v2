using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryExtra
{
    public static InventoryExtra inventoryExtra;
    public static InventoryExtra Instance { get { return inventoryExtra; } }

    [HideInInspector] public bool weapon1stored;
    [HideInInspector] public bool weapon2stored;
    public bool[] isfull;
    public GameObject[] slots;
    public InventorySlots[] iSlots;
    public Weapons equippedWeapon;

    private GameManager master;
    private Weapons grenadeLauncher;
    private Weapons plasmaRifle;

    private void Awake()
    {
        inventoryExtra = this;
    }

    public InventoryExtra()
    {
        master = GameManager.gameMaster;
        iSlots = GameManager.gameMaster.inventoryCanvas.GetComponentsInChildren<InventorySlots>();
        slots = new GameObject[iSlots.Length];
        isfull = new bool[iSlots.Length];
        for (int i = 0; i < iSlots.Length; i++)
        {
            isfull[i] = false;
            slots[i] = iSlots[i].gameObject;
        }
        grenadeLauncher = new GrenadeLauncher(GameManager.gameMaster.gameManagerModel.GrenadeProjectile, GameManager.gameMaster.particleList.grenadeLauncherParticles, 6, 0.2f, 1f);
        plasmaRifle = new PlasmaRifle(GameManager.gameMaster.gameManagerModel.PlasmaProjectile,GameManager.gameMaster.particleList.plasmaParticles,0.2f);
        equippedWeapon = plasmaRifle;
        GameManager.gameMaster.MasterUpdate.AddListener(Update);
        weapon2stored = false;
        AddWeapon();
    }

    void Update()
    {
        grenadeLauncher.Update();
        plasmaRifle.Update();
        if (Input.GetKeyDown(KeyCode.E) && GameManager.gameMaster.player.GetComponent<GenericHealthManager>().GetCurrentHP() < GameManager.gameMaster.player.GetComponent<PlayerController>().playerModel.health)
        {
            for (int i = slots.Length - 1; i >= 2; i--)
            {
                if (isfull[i] == true)
                {
                    iSlots[i].DropItem();
                    isfull[i] = false;
                    //re-fill player hp
                    AudioManager.instance.Play("UseMedKit");
                    GameManager.gameMaster.player.GetComponent<GenericHealthManager>().RefillPlayerHP(GameManager.gameMaster.gameManagerModel.hpToGive);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (equippedWeapon != plasmaRifle)
            {
                equippedWeapon = plasmaRifle;
                GameManager.gameMaster.canvasManager.SetWeaponOnPortrait("Weapon1");
                AudioManager.instance.Play("ChangeWeapon1");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (equippedWeapon != grenadeLauncher && weapon2stored)
            {
                equippedWeapon = grenadeLauncher;
                GameManager.gameMaster.canvasManager.SetWeaponOnPortrait("Weapon2");
                AudioManager.instance.Play("ChangeWeapon2");
            }
        }
    }

    public void ShootEquippedWeapon(Vector2 position, Vector2 direction)
    {
        GameObject bullet;
        if (equippedWeapon.Shoot(out bullet))
        {
            GameObject var;
            var = GameManager.Instantiate(equippedWeapon.GetParticles(), GameManager.gameMaster.PlayerController.particlePoint.transform.position, GameManager.gameMaster.PlayerController.particlePoint.transform.rotation);
            var.transform.parent = GameManager.gameMaster.PlayerController.particlePoint.transform;
            GameManager.pool.ActivatePrefab(bullet, position, direction);
            GameManager.gameMaster.canvasManager.GetComponent<CanvasManager>().CanvasGrenadeAmmo(grenadeLauncher.GetCurrentAmmo());
        }
    }

    private void AddWeapon()
    {
        AddItemToInventory(0, master.startingWeapon); // storing weapon1 to slot 0
        weapon1stored = true;
    }

    public void AddItemToInventory(int position, GameObject item)
    {
        isfull[position] = true;
        GameManager.gameMaster.instantiatePrefab(item, slots[position].transform, false); // false = no world coords.
    }

    public Weapons GetGrenadeLauncher()
    {
        return grenadeLauncher;
    }
}

