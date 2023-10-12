using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletGate : MonoBehaviour, IDamagable,IInteractable
{
    [SerializeField] int maxBulletCounter;
    int bulletCounter;
    bool isGateActive;
    [SerializeField] Image fillImage;
    
    public void Interact()
    {
        if(isGateActive)
        {
            GameManager.instance.bulletSizeUp = true;
            Destroy(gameObject);
        }
        fillImage.fillAmount =  (float)bulletCounter/ (float)maxBulletCounter;
    }

    public void TakeDamage(float damage)
    {
        bulletCounter++;
        bulletCounter = Mathf.Clamp(bulletCounter,0,maxBulletCounter);
        fillImage.fillAmount =  (float)bulletCounter/ (float)maxBulletCounter;

        if(bulletCounter == maxBulletCounter)  
        {
            Debug.Log("limit reached");
            isGateActive = true;
        }
    }
}
