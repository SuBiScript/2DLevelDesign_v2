using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour
{
    public void DropItem()
    {
        foreach (Transform child in transform) // number of times equal to number of children insinde slot image
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
