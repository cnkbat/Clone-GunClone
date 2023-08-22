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
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("WeaponSelector"))
        {
            gunsInCollider.Remove(other.gameObject);
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
        else if(parentUpgradeCard.gunAmountCard)
        {
            for (int i = 0; i < gunsInCollider.Count; i++)
            {
                Player.instance.SpawnWeaponSelector(gunsInCollider[i]);
            }
        }
        else if(parentUpgradeCard.doubleShotCard)
        {
            for (int i = 0; i < gunsInCollider.Count; i++)
            {
                gunsInCollider[i].GetComponent<WeaponSelector>().ActiveDoubleShot();
            }
        }

        parentUpgradeCard.UpgradeActionEnd();
    }

}
