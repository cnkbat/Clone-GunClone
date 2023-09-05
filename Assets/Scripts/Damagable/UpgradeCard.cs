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
    bool firstLevel,secondLevel, thirdLevel;

    public int givingValue; 

    public bool  inityearCard, gunAmountCardPlus,fireRateCard,fireRangeCard, doubleShotCard , gunAmountCardMulti;
    [SerializeField] int maxfillingValue;
    int currentValue;

    [Header ("Visual")]
    [SerializeField] Vector3 hitEffectScale;
    [SerializeField] float hitEffectDur;
    [SerializeField] Vector3 originalScale;
    [SerializeField] TMP_Text addOnValueText , textOnTop;

    [Header("Sliding")]
    [SerializeField] float slidingMoveDur;
    [SerializeField] Vector3 rotationValue;

    [Header("UpgradeSystem")]
    public GameObject upgradeCardChooserCollider;

    [SerializeField] GameObject playerCollider;

    [Header("HealthBar")]
    [SerializeField] Image firstStar;
    [SerializeField] Image secondStar, thirdStar;
    void Start()
    {
        firstLevel = true;
        originalScale = transform.localScale;
        upgradeCardChooserCollider = transform.Find("WeaponChooserCollider").gameObject;

        LevelChoosing();
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

    public void TakeDamage(float dmg)
    {
        currentValue = Mathf.RoundToInt(currentValue + dmg);
       // GateHitEffect();
        if(currentValue >= firstLevelMaxValue && !secondLevel && !thirdLevel)
        {
            currentValue = 0;
            firstLevel = false;
            secondLevel = true;
            thirdLevel = false;
            if(firstStar) firstStar.fillAmount = 1;
        }
        else if(currentValue >= secondLevelMaxValue && !firstLevel && !thirdLevel )
        {
            currentValue = 0;
            firstLevel = false;
            secondLevel = false;
            thirdLevel = true;
            if(secondStar) secondStar.fillAmount = 1;
        }
        else if(currentValue >= thirdLevelMaxValue && !firstLevel && !secondLevel)
        {
            if(thirdStar) thirdStar.fillAmount = 1;
            Interact();
        }

        if(firstLevel && firstStar)
        {
            firstStar.fillAmount = (float)currentValue / (float)firstLevelMaxValue;
        }
        else if(secondLevel && secondStar)
        {
            secondStar.fillAmount = (float)currentValue / (float)secondLevelMaxValue;
        }
        else if(thirdLevel && thirdStar)
        {
            thirdStar.fillAmount = (float)currentValue / (float)thirdLevelMaxValue;
        }
        LevelChoosing();
        GateHitEffect();

    }

    public void Interact()
    {   
        GameManager.instance.collectedCards.Add(gameObject);
        gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");
        playerCollider.SetActive(false);
        
        transform.DOMove(new Vector3(GameManager.instance.leftPlatform.transform.position.x,transform.position.y,
            transform.position.z),slidingMoveDur).
                OnPlay(() => {transform.DORotate(rotationValue,slidingMoveDur);})
                    .OnComplete(MoveTowardsCollectionPoint);
    }

    public void UpgradeActionEnd()
    {
        GameManager.instance.collectedCards.Remove(gameObject);
        if(GameManager.instance.collectedCards.Count <= 0)
        {
            GameManager.instance.upgradePhase = false;
            GameManager.instance.EnableCam(GameManager.instance.endingCam);
            Player.instance.SetWeaponsInitYearTextState(false);
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
        
        if(!addOnValueText) return;
        addOnValueText.text = "+" + givingValue.ToString();
        if(gunAmountCardMulti)
        {
            addOnValueText.text = "X" + (givingValue +1).ToString();
        }
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
