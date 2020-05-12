using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryExtra
{
    [HideInInspector] public bool weapon1stored;
    public bool[] isfull;
    public GameObject[] slots;
    public InventorySlots[] iSlots;
    public GameObject weapon1;

    public Weapons equippedWeapon;
    private Weapons grenadeLauncher;
    private Weapons plasmaRifle;

    public InventoryExtra()
    {
        AddWeapon();
        iSlots = GameManager.gameMaster.inventoryCanvas.GetComponentsInChildren<InventorySlots>();
        slots = new GameObject[iSlots.Length];
        isfull = new bool[iSlots.Length];
        for (int i = 0; i < iSlots.Length; i++)
        {
            isfull[i] = false;
            slots[i] = iSlots[i].gameObject;
        }
        grenadeLauncher = new GrenadeLauncher(GameManager.gameMaster.gameManagerModel.GrenadeProjectile, 12, 0.1f, 2.0f);
        equippedWeapon = grenadeLauncher;
        GameManager.gameMaster.MasterUpdate.AddListener(Update);
    }

    void Update()
    {
        equippedWeapon.Update();
        if (Input.GetKeyDown(KeyCode.E) && GameManager.gameMaster.player.GetComponent<GenericHealthManager>().GetCurrentHP() < GameManager.gameMaster.player.GetComponent<PlayerController>().playerModel.health)
        {
            for (int i = slots.Length - 1; i >= 2; i--)
            {
                if (isfull[i] == true)
                {
                    iSlots[i].DropItem();
                    isfull[i] = false;
                    //re-fill player hp
                    GameManager.gameMaster.player.GetComponent<GenericHealthManager>().RefillPlayerHP(4);
                    break;
                }
            }
        }
    }

    public void ShootEquippedWeapon(Vector2 position, Vector2 direction) // no one direction anymore lol
    {
        GameObject bullet;
        if (equippedWeapon.Shoot(out bullet))
        {
            GameManager.pool.ActivatePrefab(bullet, position, direction);
        }
    }

    private void AddWeapon()
    {
        AddItemToInventory(0, weapon1); // storing weapon1 to slot 0
        weapon1stored = true;
    }

    public void AddItemToInventory(int position, GameObject item)
    {
        //Debug.Log(GameManager.gameMaster.inventory.isfull[position]);
        //GameManager.gameMaster.inventory.isfull[position] = true;
        //GameManager.gameMaster.instantiatePrefab(item, slots[position].transform, false); // false = no world coords.
    }

    public Weapons GetGrenadeLauncher()
    {
        return grenadeLauncher;
    }
}

