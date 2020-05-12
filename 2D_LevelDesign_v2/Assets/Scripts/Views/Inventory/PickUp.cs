using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUp : MonoBehaviour
{
    public GameObject item;

    void OnTriggerEnter2D(Collider2D col)
    {
        bool check = false;
        try
        {
            check = col.transform.parent.tag == "Player";
        }
        catch
        {
            //Nothing
        }
        if (check)
        {
            if (item.name == "Weapon2")
            {
                GameManager.gameMaster.inventory.AddItemToInventory(1, item);// storing weapon2 to slot 1
                GameManager.gameMaster.inventory.weapon2stored = true;
                AudioManager.instance.Play("Ohyeah");
                Destroy(gameObject);
            }
            else
            {
                for (int i = 2; i < GameManager.gameMaster.inventory.slots.Length; i++) // will check the inventory from slot 2 (consumable items) to last slot
                {
                    if (GameManager.gameMaster.inventory.isfull[i] == false)
                    {
                        GameManager.gameMaster.inventory.AddItemToInventory(i, item); // storing consumable items
                        AudioManager.instance.Play("PickItem");
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }
}
