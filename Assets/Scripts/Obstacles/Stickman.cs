using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class Stickman : MonoBehaviour , IInteractable, IDamagable
{
    [Header("Money")]
    [SerializeField] Transform moneySpawnTransform;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] int moneysValue;

    [Header("Health")]
    [SerializeField] float maxHealth;
    float currentHealth;


    [Header("Visual")]
    [SerializeField] TMP_Text healthText;
    [SerializeField] Vector3 hitEffectScale;
    [SerializeField] float hitEffectDur;
    Vector3 originalScale;

    private void Start() 
    {
        currentHealth = maxHealth;
        originalScale = healthText.transform.localScale;
        
        UpdateHealthText();
    }

    public void Interact()
    {
        Player.instance.KnockbackPlayer();
    }

    public void TakeDamage()
    {
        currentHealth -= Player.instance.playerDamage;
        UpdateHealthText();
        ObstacleHitEffect();
        
        if(currentHealth <= 0)
        {
            GameObject spawnedMoney = Instantiate(moneyPrefab,moneySpawnTransform.position,Quaternion.identity);
            spawnedMoney.GetComponent<Money>().value = moneysValue;
            Destroy(gameObject);
        }     
    }

    private void ObstacleHitEffect()
    {
        healthText.transform.DOScale(hitEffectScale,hitEffectDur).OnComplete(ObstacleHitEffectReset);
    }

    private void ObstacleHitEffectReset()
    {
        healthText.transform.DOScale(originalScale,hitEffectDur);
    }
    private void UpdateHealthText()
    {
        healthText.text = Mathf.RoundToInt(currentHealth).ToString();
    }
}
