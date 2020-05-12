using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Requirements")]
    public Image BossHPPortrait;
    public Image BossHPBar;
    public Image playerHP;
    public Image playerEnergy;
    public Image Weapon1;
    public Image Weapon2;
    public Text score;

    private int removingAmmo;
    private int gameScore = 0;

    void Start()
    {
        removingAmmo = GameManager.gameMaster.inventory.GetGrenadeLauncher().GetMagazineCapacity();
        score.text = "" + gameScore;
        BossHPPortrait.enabled = false;
        BossHPBar.enabled = false;
        Weapon2.enabled = false;
    }

    public void CanvasGrenadeAmmo(float currentAmmo)
    {
        playerEnergy.fillAmount = currentAmmo / removingAmmo;
    }

    public void CanvasScore(int addingScore)
    {
        gameScore += addingScore;
        score.text = "" + gameScore;
    }

    public void SetBossHPBar()
    {
        BossHPPortrait.enabled = true;
        BossHPBar.enabled = true;
    }

    public void SetWeaponOnPortrait(string weapon)
    {
        if (weapon == "Weapon1")
        {
            Weapon2.enabled = false;
            Weapon1.enabled = true;
        }
        else
        {
            Weapon1.enabled = false;
            Weapon2.enabled = true;
        }
    }
}
