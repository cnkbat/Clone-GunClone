using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {get; private set;}

    [Header("Starting Hud")]
    [SerializeField] GameObject startingHud;
    bool canHideStartingUI;

    [SerializeField] Button startButton, fireRateButton, inityearButton;
    [SerializeField] Image fingerImage;
    [SerializeField] float leftRightMovement;
    [SerializeField] float animTime;


    [Header("Game Hud")]
    public GameObject gameHud;
    [SerializeField] TMP_Text initYearNumber;
    [SerializeField] TMP_Text currentLevelText;

    [Header("GameHud Attributes")]
    [SerializeField] TMP_Text playerMoneyText, reducerText;
    [SerializeField] float reducerMoveValue,reducerMoveDur;
    [SerializeField] Vector2 reducerTextResetPos;

    [Header("End Hud")]
    public GameObject endHud;
    [SerializeField] GameObject initYearImage;

    [Header("Upgrades")]
    [SerializeField] TMP_Text fireRateLevelText;
    [SerializeField] TMP_Text fireRangeLevelText, incomeLevelText, initYearLevelText;
    [SerializeField] TMP_Text fireRateCostText, fireRangeCostText, incomeCostText, initYearCostText;

    [Header("Slider")]
    [SerializeField] Slider weaponSlider;
    [SerializeField] List<GameObject> blackandWhiteImages;
    [SerializeField] List<GameObject> coloredImages;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start() 
    {
        MoveFinger();
        UpdateStartingHudTexts();
      //  UpdateWeaponBar();
        /*reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
        reducerText.gameObject.SetActive(false); */
    }
    private void Update() 
    {
        if(canHideStartingUI)
        {
            HideStartingUI();
        }   
    }

    public void UpdateWeaponBar()
    {
        for (int i = 0; i < blackandWhiteImages.Count; i++)
        {
            blackandWhiteImages[i].SetActive(false);
        }
        for (int i = 0; i < coloredImages.Count; i++)
        {
            coloredImages[i].SetActive(false);
        }

        // diğeriyle tam aynı değil değişecek
        coloredImages[Player.instance.weaponIndex].SetActive(true);
        blackandWhiteImages[Player.instance.weaponIndex].SetActive(true);
        if(Player.instance.weaponIndex == 0)
        {
            weaponSlider.minValue = 1750;
        } 
        else weaponSlider.minValue = Player.instance.weaponChoosingInitYearsLimit[Player.instance.weaponIndex -1];
        weaponSlider.maxValue = Player.instance.weaponChoosingInitYearsLimit[Player.instance.weaponIndex];
        weaponSlider.value = Player.instance.initYear;
    }

    // STARTING HUD
    public void OnPlayButtonPressed()
    {
        startButton.interactable = false;
        GameManager.instance.gameHasStarted = true;
        
        GameManager.instance.EnableCam(GameManager.instance.mainCam);
        canHideStartingUI = true;
        Player.instance.SetWeaponsInitYearTextState(false);
    }
    private void HideStartingUI()
    {
        startButton.transform.SetParent(startingHud.transform,false);
        fireRateButton.transform.SetParent(startingHud.transform,false);
        inityearButton.transform.SetParent(startingHud.transform,false);

        startingHud.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
        if(startingHud.GetComponent<CanvasGroup>().alpha <= 0)
        {
            canHideStartingUI = false;
            startingHud.SetActive(false);
        }
    }
    private void MoveFinger()
    {
        fingerImage.rectTransform.DOAnchorPos
            (new Vector3(fingerImage.rectTransform.anchoredPosition.x + leftRightMovement,
                fingerImage.rectTransform.anchoredPosition.y),animTime)
                    .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                    
    }
    public void UpdateMoneyText()
    {
        playerMoneyText.text = Player.instance.money.ToString();
    }
    public void UpdateStartingHudTexts()
    {
        currentLevelText.text = "LEVEL " + (Player.instance.currentLevelIndex + 1).ToString();
      //  UpdateInitYearText();
        UpdateMoneyText();

        fireRateLevelText.text = "LEVEL " + (Player.instance.fireRateValueIndex + 1).ToString();
        initYearLevelText.text = "LEVEL " + (Player.instance.initYearValueIndex + 1).ToString();

        fireRateCostText.text = (UpgradeManager.instance.costs[Player.instance.fireRateValueIndex].ToString());
        initYearCostText.text = (UpgradeManager.instance.costs[Player.instance.initYearValueIndex].ToString());
    }
    public void UpdateEndingHudTexts()
    {
        fireRangeLevelText.text = "LEVEL " + (Player.instance.fireRangeValueIndex + 1 ).ToString();
        incomeLevelText.text = "LEVEL " + (Player.instance.incomeValueIndex + 1 ).ToString();

        fireRangeCostText.text = (UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex].ToString());
        incomeCostText.text = (UpgradeManager.instance.costs[Player.instance.incomeValueIndex].ToString());

    }
    // Upgrades
    public void OnFireRateUpdatePressed()
    {
        
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.fireRateValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.fireRateValueIndex];
            Player.instance.fireRateValueIndex +=1;
            fireRateLevelText.text = "LEVEL " + (Player.instance.fireRateValueIndex + 1).ToString();
            fireRateCostText.text = (UpgradeManager.instance.costs[Player.instance.fireRateValueIndex].ToString());
            Player.instance.SetUpgradedValues();
            UpdateMoneyText();
            Player.instance.SavePlayerData();
        }
    }
    public void OnFireRangeUpdatePressed()
    {
        print("firerange");
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex];
            Player.instance.fireRangeValueIndex +=1;
            fireRangeLevelText.text = "LEVEL " + (Player.instance.fireRangeValueIndex + 1).ToString();
            fireRangeCostText.text = (UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex].ToString());
            Player.instance.SetUpgradedValues();
            UpdateMoneyText();
            Player.instance.SavePlayerData();

        }
    }
    public void OnInitYearUpdatePressed()
    {
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.initYearValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.initYearValueIndex];
            Player.instance.initYearValueIndex +=1;
            initYearLevelText.text = "LEVEL " + (Player.instance.initYearValueIndex + 1).ToString();
            initYearCostText.text = (UpgradeManager.instance.costs[Player.instance.initYearValueIndex].ToString());
            UpdateMoneyText();
            Player.instance.SetUpgradedValues();
           // UpdateInitYearText();
            Player.instance.SavePlayerData();
            
        }
    }
    public void OnIncomeUpdatePressed()
    {
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.incomeValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.incomeValueIndex];
            Player.instance.incomeValueIndex +=1;
            incomeLevelText.text = "LEVEL " + (Player.instance.incomeValueIndex + 1).ToString();
            incomeCostText.text = (UpgradeManager.instance.costs[Player.instance.incomeValueIndex].ToString());
            UpdateMoneyText();
            Player.instance.SetUpgradedValues();
            Player.instance.SavePlayerData();
        }
    }
    public void OnContinueButtonPressed()
    {
        GameManager.instance.LoadNextScene();
    }
    public void FinishHud()
    {
        endHud.SetActive(true);
        UpdateEndingHudTexts();
    }

    public void DisplayInitYearReduce()
    {

        // spawn olark yapalım
        reducerText.gameObject.SetActive(true);
        reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
        
        reducerText.rectTransform.DOAnchorPos(new Vector2(reducerText.rectTransform.anchoredPosition.x,
            reducerText.rectTransform.anchoredPosition.y - reducerMoveValue),reducerMoveDur).
                OnPlay(() => {reducerText.DOFade(0,reducerMoveDur);}).
                    OnComplete(() => 
                    {
                        reducerText.DOFade(255,reducerMoveDur);
                        reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
                        reducerText.gameObject.SetActive(false);
                    });
    }
}