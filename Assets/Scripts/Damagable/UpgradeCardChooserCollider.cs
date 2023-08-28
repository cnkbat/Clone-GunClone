using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCardChooserCollider : MonoBehaviour
{
    
    public List<GameObject> gunsInCollider;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("WeaponSelector"))
        {
            gunsInCollider.Add(other.gameObject);
        
            other.GetComponent<WeaponSelector>().currentWeapon.GetComponent<MeshRenderer>().material = GameManager.instance.highlitedMaterial;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("WeaponSelector"))
        {
            gunsInCollider.Remove(other.gameObject);
            other.GetComponent<WeaponSelector>().currentWeapon.GetComponent<MeshRenderer>().material =
                other.GetComponent<WeaponSelector>().currentWeapon.GetComponent<Weapon>().originalMaterial;
        }
    }

    public void UpgradeAction()
    {
        UpgradeCard parentUpgradeCard = transform.parent.GetComponent<UpgradeCard>();

        if(parentUpgradeCard.fireRangeCard)
        {
            for (int i = 0; i < gunsInCollider.Count; i++)
            {
                gunsInCollider[i].GetComponent<WeaponSelector>().IncrementInGameFireRange(parentUpgradeCard.givingValue);
            }
        }
        else if(parentUpgradeCard.fireRateCard)
        {
            for (int i = 0; i < gunsInCollider.Count; i++)
            {
                gunsInCollider[i].GetComponent<WeaponSelector>().IncrementInGameFireRate(parentUpgradeCard.givingValue);
            }
        }
        else if(parentUpgradeCard.inityearCard)
        {
            for (int i = 0; i < gunsInCollider.Count; i++)
            {
                gunsInCollider[i].GetComponent<WeaponSelector>().IncrementInGameInitYear(parentUpgradeCard.givingValue);
            }
        }
        else if(parentUpgradeCard.gunAmountCardPlus)
        {
            for (int i = 0; i < gunsInCollider.Count;)
            {
                for (int a = 0; a < parentUpgradeCard.givingValue; a++)
                {
                    float spawnedFireRange = gunsInCollider[0].GetComponent<WeaponSelector>().GetInGameFireRange();
                    float spawnedFireRate = gunsInCollider[0].GetComponent<WeaponSelector>().GetInGameFireRate();
                    int spawnedInitYear = gunsInCollider[0].GetComponent<WeaponSelector>().GetInGameInitYear();
                    Player.instance.SpawnWeaponSelector(gunsInCollider[0],spawnedFireRange,spawnedFireRate,spawnedInitYear);
                }
                break;
            }
        }
        else if(parentUpgradeCard.gunAmountCardMulti)
        {
            for (int i = 0; i < gunsInCollider.Count; i++)
            {
                for (int a = 0; a < parentUpgradeCard.givingValue; a++)
                {
                    float spawnedFireRange = gunsInCollider[i].GetComponent<WeaponSelector>().GetInGameFireRange();
                    float spawnedFireRate = gunsInCollider[i].GetComponent<WeaponSelector>().GetInGameFireRate();
                    int spawnedInitYear = gunsInCollider[i].GetComponent<WeaponSelector>().GetInGameInitYear();


                    Player.instance.SpawnWeaponSelector(gunsInCollider[i],spawnedFireRange,spawnedFireRate,spawnedInitYear);
                }
            }
        }
        else if(parentUpgradeCard.doubleShotCard)
        {
            for (int i = 0; i < gunsInCollider.Count; i++)
            {
                gunsInCollider[i].GetComponent<WeaponSelector>().ActiveDoubleShot();
            }
        }


        for (int i = 0; i < gunsInCollider.Count; i++)
        {
            gunsInCollider[i].GetComponent<WeaponSelector>().currentWeapon.GetComponent<MeshRenderer>().material = 
                gunsInCollider[i].GetComponent<WeaponSelector>().currentWeapon.GetComponent<Weapon>().originalMaterial; 
        }
        parentUpgradeCard.UpgradeActionEnd();
    }

}
