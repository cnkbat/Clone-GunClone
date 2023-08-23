using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGate : MonoBehaviour, IDamagable,IInteractable
{
    [SerializeField] int maxBulletCounter;
    int bulletCounter;
    bool isGateActive;

    void Start()
    {
        
    }

    public void Interact()
    {
        Debug.Log("interacted");
        Debug.Log(isGateActive);
        if(isGateActive)
        {
            GameManager.instance.bulletSizeUp = true;
            Debug.Log("gate active");
        }
        
    }

    public void TakeDamage()
    {
        Debug.Log("damage taken");
        bulletCounter++;
        bulletCounter = Mathf.Clamp(bulletCounter,0,maxBulletCounter);
        if(bulletCounter == maxBulletCounter)  
        {
            Debug.Log("limit reached");
            isGateActive = true;
        }
    }
}
