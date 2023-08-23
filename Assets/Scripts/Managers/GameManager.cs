using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cinemachine;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    //ending level related variables
    [Header("Phase variables")]
    public bool gameHasEnded = false;
    public bool gameHasStarted = false;
    public bool upgradePhase = false;
    //----------------

    [Header("Player Attributes")]
    public float playerDamage;
    public int playerKnockbackValue;

    [Header("Visual")]
    public GameObject mainCam,startingCam,endingCam,upgradeCam;

    [Header("Collecting Cards")]
    public List<GameObject> collectedCards;
    public Transform collectionPoint;
    public float cardCollectionMoveDur;
    public GameObject leftPlatform;

    [Header("UpgradeCars")]
    public float weaponSelectorSize;
    public int weaponSelectorCount;
    public Canvas canvas;
    public float selectedCardYAxis;

    [Header("UpgradingPhase")]
    public List<Transform> upgradeCardPlacements;
    public Transform playerUpgradingPos;
    public float upgradePosMoveDur;
    public Vector3 upgradeCardScale;

    [Header("Testing")]
    [SerializeField] GameObject testObject;
    
    [Header("Obstacle")]
    public float obstaclePushValue;

    [Header("Bullet")]
    public bool bulletSizeUp = false;
    
    ////
    ////   ***********<SUMMARY>*************
    //// Game manager script is alive every scene.
    /// this script manages all the scores and hat related stuff in the game
    /// and all the huds that player can and cant see.
    /// as you can see at the bottom this script also finishes the game.


    // assigning variables
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 
       
    }
    private void Start()
    {
        UpdatePlayerDamage();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.A))    
        {
            Player.instance.SpawnWeaponSelector(testObject);
        }
    }

    public void EndLevel()
    {
        gameHasEnded = true;
        Player.instance.PlayerDeath();
      //  UIManager.instance.FinishHud();
    }
    
    public void EnableCam(GameObject newCam)
    {
        newCam.SetActive(true);
    }

    public void FinishLinePassed()
    {
        EnableCam(endingCam);
    }

    public void UpgradePhase()
    {
        EnableCam(upgradeCam);
        upgradePhase = true;
        Player.instance.gameObject.transform.DOMove(playerUpgradingPos.position, upgradePosMoveDur);
        Player.instance.SetWeaponsInitYearTextState(true);
        
        for (int i = 0; i < collectedCards.Count; i++)
        {
            collectedCards[i].tag = "UpgradeCard";
            collectedCards[i].transform.DOMove(upgradeCardPlacements[i].position, upgradePosMoveDur);
            collectedCards[i].transform.DORotate(new Vector3(0,0,180),upgradePosMoveDur,RotateMode.Fast);
            collectedCards[i].transform.localScale = upgradeCardScale;
        }
    }
    
    // buttona basıldığında gerçekleşecek
    public void LoadNextScene()
    {
        Player.instance.SavePlayerData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void UpdatePlayerDamage()
    {
        playerDamage = Player.instance.playerDamage;
    }
}
