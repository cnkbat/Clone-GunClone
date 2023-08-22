using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    [Header("Weapon Selecting")] 
    [SerializeField] List<GameObject> weapons;
    public List<int> weaponChoosingInitYearsLimit;

    [Header("Weapon")]
    [SerializeField] GameObject currentWeapon;

    [Tooltip("Current Attributes")]
    private int inGameInitYear;
    private float inGameFireRate,inGameFireRange;

    int weaponIndex;
    // Start is called before the first frame update
    void Start()
    {   
         Player.instance.weaponSelectors.Add(gameObject);
        Player.instance.weaponChoosingInitYearsLimit = weaponChoosingInitYearsLimit;
        WeaponSelecting();
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
    public float GetInGateFireRate()
    {
        return inGameFireRate;
    }

    //SETTERS
    public void IncrementInGameFireRange(float value)
    {
        inGameFireRange += value;
    }
    public void IncrementInGameFireRate(float value)
    {
        float effectiveValue = value / 100;
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
        

        WeaponSelecting();
        
    }

    public void ActiveDoubleShot()
    {
        for (int i = 0; i < weapons.Count; i++)
        {   
            weapons[i].GetComponent<Weapon>().doubleShotActive = true;
        }
    }
}
