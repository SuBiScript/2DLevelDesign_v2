using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GenericHealthManager : MonoBehaviour
{
    [Header("Settings")]
    public bool destroyParent;
    public bool setActiveFalse;
    public bool ableToBecomeInvulnerable;

    [Header("Requirements")]
    public CharacterModel charModel;
    public GameObject deathAnim;
    public CanvasManager score;
    public Image HPSlider;
    public bool isInvulnerable = false;

    private float currentHealth;

    void Start()
    {
        charModel = Instantiate(charModel);
        currentHealth = charModel.health;
    }

    public void HurtCharacter(int damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
        }
        if (isInvulnerable && ableToBecomeInvulnerable)
        {
            return;
        }

        if (HPSlider != null)
        {
            HPSlider.gameObject.SetActive(true);
            HPSlider.fillAmount = currentHealth / charModel.health;
        }

        if (currentHealth <= 0)
        {
            if (deathAnim != null)
                Instantiate(deathAnim, this.gameObject.transform.position, this.gameObject.transform.rotation);

            if (score != null)
                score.CanvasScore(charModel.pointsToGive);

            if (destroyParent)
            {
                Destroy(transform.parent.gameObject);
            }
            else if (setActiveFalse)
            {               
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void MakeInvulnerable()
    {
        StartCoroutine(StartInvulnerableNextFrame());
        StartCoroutine(InvulnerabilityFlash());
        Invoke("MakeVulnerable", charModel.invulnerabilityDuration);
    }

    IEnumerator StartInvulnerableNextFrame()
    {
        yield return null;
        isInvulnerable = true;
    }

    IEnumerator InvulnerabilityFlash()
    {
        yield return new WaitForSeconds(Time.deltaTime * GetComponent<PlayerController>().gameSettingsModel.invulnerabilityFlashDuration);
        if (!isInvulnerable)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            StartCoroutine(InvulnerabilityFlash());
        }
    }

    private void MakeVulnerable()
    {
        isInvulnerable = false;
    }

    public void RefillPlayerHP(int healthPoints)
    {
        currentHealth += Mathf.Min(healthPoints, charModel.health - currentHealth); ;
        HPSlider.fillAmount = currentHealth / charModel.health;
    }

    public float GetCurrentHP()
    {
        return currentHealth;
    }

    public bool GetIsInvulnerable()
    {
        return isInvulnerable;
    }
}
