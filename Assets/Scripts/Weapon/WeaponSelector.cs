using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class WeaponSelector : MonoBehaviour , IInteractable
{
    [Header("Weapon Selecting")] 
    public List<GameObject> weapons;
    public List<int> weaponChoosingInitYearsLimit;

    [Header("Weapon")]
    public GameObject currentWeapon;

    [Tooltip("Current Attributes")]
    private int inGameInitYear;
    private float inGameFireRate,inGameFireRange;

    public int weaponIndex;

    public bool isCollectable;    
    public bool isFirstWS;

    void Start()
    {   
        if(!isFirstWS && isCollectable) Player.instance.weaponSelectors.Add(gameObject);

        Player.instance.weaponChoosingInitYearsLimit = weaponChoosingInitYearsLimit;
        WeaponSelecting();

        if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        else Player.instance.SetWeaponsInitYearTextState(false);
        
        if(!GameManager.instance.gameHasStarted) Player.instance.SetWeaponsInitYearTextState(true);
        else Player.instance.SetWeaponsInitYearTextState(false);

        if(isCollectable) 
        {
            SetStartingValues();
            return;
        }

    }

    public void WeaponSelecting()
    {
        
        if(inGameInitYear <= weaponChoosingInitYearsLimit[0] && currentWeapon != weapons[0])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }
            currentWeapon = weapons[0];
            weaponIndex = 0;
            currentWeapon.SetActive(true);

            if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
            
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[0] && inGameInitYear <= weaponChoosingInitYearsLimit[1] && currentWeapon != weapons[1]) 
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[1];
            weaponIndex = 1;
            currentWeapon.SetActive(true);
            if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[1] && inGameInitYear <= weaponChoosingInitYearsLimit[2] && currentWeapon != weapons[2])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[2];
            weaponIndex = 2;
            currentWeapon.SetActive(true);
            if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[2] && inGameInitYear <= weaponChoosingInitYearsLimit[3] && currentWeapon != weapons[3])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 3;
            currentWeapon = weapons[3];
            currentWeapon.SetActive(true);
            if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        }

        if(inGameInitYear > weaponChoosingInitYearsLimit[3] && inGameInitYear <= weaponChoosingInitYearsLimit[4] && currentWeapon != weapons[4])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 4;
            currentWeapon = weapons[4];
            currentWeapon.SetActive(true);
            if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        }

        if(inGameInitYear > weaponChoosingInitYearsLimit[4] && inGameInitYear <= weaponChoosingInitYearsLimit[5] && currentWeapon != weapons[5])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 5;
            currentWeapon = weapons[5];
            currentWeapon.SetActive(true);
            if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[5] && inGameInitYear <= weaponChoosingInitYearsLimit[6] && currentWeapon != weapons[6])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[6];
            weaponIndex = 6;
            currentWeapon.SetActive(true);
            if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        }

        currentWeapon.transform.parent = transform;
    }

    public void SetStartingValues()
    {
        inGameFireRange = Player.instance.fireRange;
        inGameFireRate = Player.instance.fireRate;
        inGameInitYear = Player.instance.initYear;
    }

    public int GetInGameInitYear()
    {
        return inGameInitYear;
    }
    public float GetInGameFireRange()
    {
        return inGameFireRange;
    }
    public float GetInGameFireRate()
    {
        return inGameFireRate;
    }

    //spawning 
    public void SetInGameInitYear(int newInitYear)
    {
        inGameInitYear = newInitYear;
        WeaponSelecting();
    } 
    public void SetInGameFireRange(float newFireRange)
    {
        inGameFireRange = newFireRange;
    }
    public void SetInGameFireRate(float newFireRate)
    {
        inGameFireRate = newFireRate;
    }

    //SETTERS
    public void IncrementInGameFireRange(float value)
    {
        float effectiveValue = value / 500;
        inGameFireRange +=  effectiveValue;
    }
    public void IncrementInGameFireRate(float value)
    {
        float effectiveValue = value / 500;
        Debug.Log(effectiveValue);
        inGameFireRate -= effectiveValue;
    }
    public void IncrementInGameInitYear(int value)
    {
        if(value == -1) 
        {
            Debug.Log("display");
            UIManager.instance.DisplayInitYearReduce();
        }
        inGameInitYear += value;
        
        if(GameManager.instance.upgradePhase) Player.instance.SetWeaponsInitYearTextState(true);
        WeaponSelecting();
        
    }

    public void ActiveDoubleShot()
    {
        for (int i = 0; i < weapons.Count; i++)
        {   
            weapons[i].GetComponent<Weapon>().doubleShotActive = true;
        }
    }

    public void Interact()
    {
        if(isCollectable)
        {
            Player.instance.SpawnWeaponSelector(gameObject,inGameFireRange,inGameFireRate,inGameInitYear);
            Destroy(gameObject);
        }
    }
}
