using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour , IDamagable, IInteractable
{

    [Header("Level")]

    [SerializeField] int firstLevelGivingValue;
    [SerializeField] int secondLevelGivingValue, thirdLevelGivingValue;
    [SerializeField] int firstLevelMaxValue, secondLevelMaxValue, thirdLevelMaxValue;
    [SerializeField] Slider firstSlider, secondSlider, thirdSlider;
    bool firstLevel,secondLevel, thirdLevel;

    public int givingValue; 

    public bool  inityearCard, gunAmountCard,fireRateCard,fireRangeCard, doubleShotCard;
    [SerializeField] int maxfillingValue;
    int currentValue;

    [Header ("Visual")]
    [SerializeField] Vector3 hitEffectScale;
    [SerializeField] float hitEffectDur;
    [SerializeField] Vector3 originalScale;
    [SerializeField] TMP_Text addOnValueText , textOnTop;

    [Header("Sliding")]
    [SerializeField] float slidingMoveDur;
    [SerializeField] Transform slidingPoint;
    [SerializeField] Vector3 rotationValue;

    [Header("UpgradeSystem")]
    public GameObject upgradeCardChooserCollider;

    [SerializeField] GameObject playerCollider;

    void Start()
    {
        firstLevel = true;
        originalScale = transform.localScale;
        LevelChoosing();
        slidingPoint.position = new Vector3(GameManager.instance.leftPlatform.transform.position.x,
            transform.position.y,transform.position.z);
    }
    public void LevelChoosing()
    {
        if(firstLevel)
        {
            maxfillingValue = firstLevelMaxValue;
            givingValue = firstLevelGivingValue;
        }
        else if(secondLevel)
        {
            maxfillingValue = secondLevelMaxValue;
            givingValue = secondLevelGivingValue;
        }
        else if(thirdLevel)
        {
            maxfillingValue = thirdLevelMaxValue;
            givingValue = thirdLevelGivingValue;
        }
        UpdateGateText();
    }

    public void TakeDamage()
    {
        currentValue +=1;
       // GateHitEffect();
        if(currentValue >= firstLevelMaxValue)
        {
            currentValue = 0;
            firstLevel = false;
            secondLevel = true;
            thirdLevel = false;
        }
        else if(currentValue >= secondLevelMaxValue)
        {
            currentValue = 0;
            firstLevel = false;
            secondLevel = false;
            thirdLevel = true;
        }

        LevelChoosing();
        GateHitEffect();

    }

    public void Interact()
    {   
        GameManager.instance.collectedCards.Add(gameObject);
        gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");
        playerCollider.SetActive(false);
        
        transform.DOMove(slidingPoint.position,slidingMoveDur).
            OnPlay(() => {transform.DORotate(rotationValue,slidingMoveDur);})
                .OnComplete(MoveTowardsCollectionPoint);
    }

    public void UpgradeActionEnd()
    {
        GameManager.instance.collectedCards.Remove(gameObject);
        if(GameManager.instance.collectedCards.Count == 0)
        {
            GameManager.instance.upgradePhase = false;
        }

        Destroy(gameObject);
        
    }
    private void MoveTowardsCollectionPoint()
    {
        transform.DOMove(GameManager.instance.collectionPoint.position,GameManager.instance.cardCollectionMoveDur);
    }

    private void UpdateGateText()
    {
        textOnTop.text = currentValue.ToString() + "/" + maxfillingValue;
        addOnValueText.text = "+" + givingValue.ToString();
    }
    private void GateHitEffect()
    {
        transform.DOScale(hitEffectScale,hitEffectDur).OnComplete(GateHitEffectReset);
    }
    private void GateHitEffectReset()
    {
        transform.DOScale(originalScale,hitEffectDur);
    }
}
