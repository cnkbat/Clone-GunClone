using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform tipOfWeapon;

    [Header("Firing")]
    [SerializeField] float firedRotationDelay = 0.2f, resetRotationDelay = 0.8f;
    public bool doubleShotActive;
    
    [SerializeField] Vector3 fireEndRotationValue = new Vector3(315,0,0);
    [SerializeField] Vector3 originalRotationValue;

    [Header("Attributes")]
    [SerializeField] float fireRange;
    [SerializeField] float fireRate;
    private float currentFireRate;
    public float damage;

    [Header("VFX")]
    [SerializeField] GameObject ownerSelector;

    [Header("UI")]
    [SerializeField] GameObject initYearImage;
    [SerializeField] TMP_Text initYearText;

    [Header("Materials")]
    public Material originalMaterial;
    private void Start() 
    {   
        tag = "Weapon";
        if(transform.parent.tag == "WeaponSelector")
        {
            ownerSelector = transform.parent.gameObject;
        }
        GetComponent<MeshRenderer>().material = originalMaterial;
    }

    private void ResetPos()
    {
        transform.DORotate(originalRotationValue,resetRotationDelay,RotateMode.Fast);
    }
    
    private void Update() 
    {
        if(!GameManager.instance.gameHasStarted) return;
        if(GameManager.instance.gameHasEnded) return;
        if(GameManager.instance.upgradePhase) return;
        if(transform.parent.GetComponent<WeaponSelector>().isCollectable) return;
        
        if(Player.instance.knockbacked)
        {
            UpdateFireRate();
            return;
        }

        currentFireRate -= Time.deltaTime;
        
            if(currentFireRate <= 0)
            {
                FireBullet();
                UpdateFireRate();
            }
    }

    public void FireBullet()
    {
        transform.DORotate(fireEndRotationValue,firedRotationDelay,RotateMode.Fast).
            OnComplete(ResetPos);
            
        if(!doubleShotActive)
        {
            GameObject firedBullet = Instantiate(bullet, tipOfWeapon.position ,Quaternion.identity);
        
            firedBullet.GetComponent<Bullet>().firedPoint = tipOfWeapon;
            firedBullet.GetComponent<Bullet>().SetRelatedWeapon(gameObject);
        }
        else if(doubleShotActive)
        {
            GameObject firstfiredBullet = Instantiate(bullet, tipOfWeapon.position ,Quaternion.identity);
            GameObject secondfiredBullet = Instantiate(bullet, tipOfWeapon.position ,Quaternion.identity);

            firstfiredBullet.GetComponent<Bullet>().firedPoint = tipOfWeapon;
            firstfiredBullet.GetComponent<Bullet>().SetRelatedWeapon(gameObject);
            firstfiredBullet.GetComponent<Bullet>().firstBullet = true;

            secondfiredBullet.GetComponent<Bullet>().firedPoint = tipOfWeapon;
            secondfiredBullet.GetComponent<Bullet>().SetRelatedWeapon(gameObject);
            secondfiredBullet.GetComponent<Bullet>().secondBullet = true;
        }
    }
    public float GetWeaponsFireRange()
    {
        return ownerSelector.GetComponent<WeaponSelector>().GetInGameFireRange() + fireRange;
    }

    public void UpdateFireRate()
    {
        currentFireRate = ownerSelector.GetComponent<WeaponSelector>().GetInGameFireRate() + fireRate;
    }


    public void UpdateInitYearText(bool boolean)
    {
        initYearImage.SetActive(boolean);
        initYearText.text = ownerSelector.GetComponent<WeaponSelector>().GetInGameInitYear().ToString();
        Debug.Log("update init year text");
    }
}
