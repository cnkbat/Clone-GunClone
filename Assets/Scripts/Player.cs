using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class Player : MonoBehaviour
{
    public static Player instance {get; private set;}
    //Component
    CapsuleCollider capsuleCollider;
    Rigidbody rBody;
    BoxCollider boxCollider;

    [Header("Movement")]
    [SerializeField] private float forwardMoveSpeed;
    [SerializeField] private float negativeLimitValue, positiveLimitValue,maxSwerveAmountPerFrame;
    private float _lastXPos;

    [Header("Movement Changers")]
    public bool knockbacked = false;
    [SerializeField] float knockbackValue = 10f ;
    [SerializeField] float knockbackDur = 0.4f;
    public float slowMovSpeed, fastMovSpeed, originalMoveSpeed;

    [Header("Saved Attributes")]
    public int initYear;
    public float income = 1;
    public float fireRate, fireRange;

    [Header("Weapon Selectors")]
    public List<GameObject> weaponSelectors;
    public List<int> weaponChoosingInitYearsLimit;

    [Header("Upgrade Index")]
        [Tooltip("Save & Load Value")]
    // we will save and load thorugh this header and set the values after

    public int fireRateValueIndex;
    public int fireRangeValueIndex, initYearValueIndex, incomeValueIndex;
    public int money;
    public int stars; 
    public int currentLevelIndex;
    public float playerDamage;

    // public GameObject startingWeapon;

    [HideInInspector]
    public int weaponIndex;

    [Header("UpgradePhase")]
    [SerializeField] GameObject WStransfromPrefab;
    [SerializeField] List<Transform> weaponSelectorsTransformPositive;
    [SerializeField] List<Transform> weaponSelectorsTransformNegative;
    [SerializeField] List<GameObject> WSPositiveList,WSNegativeList;
    public bool positiveTurn, negativeTurn;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 
    }

    void Start() 
    {
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();
        tag = "Player";
       // LoadPlayerData();
        SetUpgradedValues();

        originalMoveSpeed = forwardMoveSpeed;
        SetWSTransformsPos();
    }

    void Update() 
    {   
        if(!GameManager.instance.gameHasStarted) return;
        if(GameManager.instance.gameHasEnded) return;

        if(!GameManager.instance.gameHasEnded && !GameManager.instance.upgradePhase)
        {
            if(!knockbacked)
            {
                MoveCharacter();
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {   
        if(other.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        }
        else if(other.CompareTag("FinishLine"))
        {
            GameManager.instance.FinishLinePassed();
        }
        else if(other.CompareTag("UpgradeLine"))
        {
            GameManager.instance.UpgradePhase();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("MovementSlower"))
        {
            SetMovementSpeed(originalMoveSpeed);
        }
        else if(other.CompareTag("MovementFaster"))
        {
            SetMovementSpeed(originalMoveSpeed);
        }
    }
    private void MoveCharacter()
    {
        Vector3 moveDelta = Vector3.forward;
        if (Input.GetMouseButtonDown(0))
        {
            _lastXPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            float moveXDelta = Mathf.Clamp(Input.mousePosition.x - _lastXPos, -maxSwerveAmountPerFrame,
                maxSwerveAmountPerFrame);
            moveDelta += new Vector3(moveXDelta, 0, 0);
            _lastXPos = Input.mousePosition.x;
        }

        moveDelta *= Time.deltaTime * forwardMoveSpeed;

        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(
            Mathf.Clamp(currentPos.x + moveDelta.x, -negativeLimitValue, positiveLimitValue),
            currentPos.y,
            currentPos.z + moveDelta.z);
        transform.position = newPos;
    }

    // player knockBack
    public void KnockbackPlayer()
    {
        knockbacked = true;
        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            weaponSelectors[i].
                GetComponent<WeaponSelector>().IncrementInGameInitYear(GameManager.instance.playerKnockbackValue);
        }
    
        transform.DOMove
            (new Vector3(transform.position.x,transform.position.y, transform.position.z - knockbackValue),knockbackDur).
                OnComplete(ResetKnockback);
        
    }
    void ResetKnockback()
    {
        knockbacked = false;
    }    
    public void PlayerDeath()
    {

        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            weaponSelectors[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            weaponSelectors[i].GetComponent<Rigidbody>().useGravity = true;
            weaponSelectors[i].GetComponent<BoxCollider>().enabled = true;
        }

        // bu muhabbet böyle değil
        //currentLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        print(currentLevelIndex);
    }
    // SAVE LOAD
    public void SavePlayerData()
    {
        SaveSystem.SavePlayerData(this);
    }
    
    public void LoadPlayerData()
    {
        SaveSystem.LoadPlayerData();
        PlayerData data = SaveSystem.LoadPlayerData();

        if(data != null)
        {
            currentLevelIndex = data.level;
            fireRateValueIndex = data.fireRateValueIndex;
            initYearValueIndex = data.initYearValueIndex;
            incomeValueIndex = data.incomeValueIndex;
            money = data.money;
            stars = data.stars;
            fireRangeValueIndex = data.fireRangeValueIndex;
        }
    }

    public void SetWSTransformsPos()
    {
        for (int i = 0; i < GameManager.instance.weaponSelectorCount / 2; i++)
        {
            Vector3 nextSpawnPos = new Vector3(weaponSelectorsTransformPositive[i].position.x + GameManager.instance.weaponSelectorSize,
                weaponSelectorsTransformPositive[i].position.y,weaponSelectorsTransformPositive[i].position.z);
            GameObject newTransform = Instantiate(WStransfromPrefab,nextSpawnPos,Quaternion.identity);
            newTransform.transform.parent = transform;
            weaponSelectorsTransformPositive.Add(newTransform.transform);
        }
        for (int i = 0; i < GameManager.instance.weaponSelectorCount / 2; i++)
        {
            Vector3 nextSpawnPos = new Vector3(weaponSelectorsTransformNegative[i].position.x - GameManager.instance.weaponSelectorSize,
                weaponSelectorsTransformNegative[i].position.y,weaponSelectorsTransformNegative[i].position.z);
            GameObject newTransform = Instantiate(WStransfromPrefab,nextSpawnPos,Quaternion.identity);
            newTransform.transform.parent = transform;
            weaponSelectorsTransformNegative.Add(newTransform.transform);
        }
    }

    public void SpawnWeaponSelector(GameObject objectToSpawn)
    {
        if(positiveTurn)
        {
            GameObject spawnedWS = Instantiate(objectToSpawn,weaponSelectorsTransformPositive[WSPositiveList.Count].position,Quaternion.identity);
            spawnedWS.transform.parent = transform;
            WSPositiveList.Add(spawnedWS);
            positiveTurn = false;
            negativeTurn = true;
            
        }
        else if(negativeTurn)
        {
            GameObject spawnedWS = Instantiate(objectToSpawn,weaponSelectorsTransformNegative[WSNegativeList.Count].position,Quaternion.identity);
            spawnedWS.transform.parent = transform;
            WSNegativeList.Add(spawnedWS);
            positiveTurn = true;
            negativeTurn = false;
        }
        
    }
    
    public void SetWeaponsInitYearTextState(bool boolean)
    {
        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            for (int a = 0; a < weaponSelectors[i].GetComponent<WeaponSelector>().weapons.Count-1; a++)
            {
                weaponSelectors[i].GetComponent<WeaponSelector>().weapons[a].GetComponent<Weapon>().UpdateInitYearText(boolean);
            }
        }
    }
    
    // Getters And Setters
    public void SetMovementSpeed(float newMoveSpeed)
    {
        forwardMoveSpeed = newMoveSpeed;
    }

    public void SetUpgradedValues()
    {
        initYear = UpgradeManager.instance.initYearValues[initYearValueIndex];
        fireRate = UpgradeManager.instance.fireRateValues[fireRateValueIndex];
        fireRange = UpgradeManager.instance.fireRangeValues[fireRangeValueIndex];
        income = UpgradeManager.instance.incomeValues[incomeValueIndex];

        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            weaponSelectors[0].
                GetComponent<WeaponSelector>().SetStartingValues();   
        }
    }

    public void IncrementWeaponSelectorsInitYear(int value)
    {
        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            weaponSelectors[i].
                GetComponent<WeaponSelector>().IncrementInGameInitYear(value);   
        }
    }
    public void IncrementWeaponSelectorsFireRate(float value)
    {
        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            weaponSelectors[i].
                GetComponent<WeaponSelector>().IncrementInGameFireRate(value);   
        }
    }
    public void IncrementWeaponSelectorsFireRange(float value)
    {
        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            weaponSelectors[i].
                GetComponent<WeaponSelector>().IncrementInGameFireRange(value);   
        }
    }

    public void IncrementMoney(int value)
    {
        money += Mathf.RoundToInt(value * income);
      //  UIManager.instance.UpdateMoneyText();
    }
}
