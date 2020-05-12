using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLineController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        try
        {
            if (col.gameObject.transform.parent.tag == "Player")
            {
                GameManager.gameMaster.PlayerController.healthManager.isInvulnerable = false;
                GameManager.gameMaster.PlayerController.healthManager.HurtCharacter((int)GameManager.gameMaster.PlayerController.playerModel.health);
            }
        }
        catch
        {
            Debug.Log("Something touched the end line");
        }
    }
}
