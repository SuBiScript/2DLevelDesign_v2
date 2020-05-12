using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject Boss;
    public GameObject DoorTrigger;
    public CameraController Zoom;
    public Transform bossTransform;

    public Animator anim;
    public GameObject BossTittleAnimation;

    void Start()
    {
        anim.GetComponent<Collider2D>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        bool player = false;
        try
        {
            player = col.transform.parent.tag == "Player";
        }
        catch
        {
            //Nothing
        }
        if (player)
        {
            //FindObjectOfType<AudioManager>().Play("DoorSound");
            //StartCoroutine(BossAccessEnabled());
            anim.SetBool("ClosingDoor", true);
            anim.GetComponent<Collider2D>().enabled = true;
            Boss.SetActive(true);
            BossTittleAnimation.SetActive(true);
            GameManager.gameMaster.canvasManager.SetBossHPBar();
            GameManager.gameMaster.PlayerController.ChangeState(new RestrainedState(5f));
            Zoom.Zoom(bossTransform);
            Destroy(gameObject);
        }
    }
}
