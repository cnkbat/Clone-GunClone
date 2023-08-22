using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour , IDamagable, IInteractable
{
    [SerializeField] float health;

    void Start()
    {
        //  if(transform.parent.GetComponent<UpgradeCard>())
        {
            transform.parent.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void Interact()
    {
        Player.instance.KnockbackPlayer();
        //  if(transform.parent.GetComponent<UpgradeCard>())
        {
            transform.parent.GetComponent<BoxCollider>().enabled = false;
        }
        
        GetComponent<BoxCollider>().enabled = false;
    }

    private void HitEffect()
    {
        // effecti yazılacak
    }

    public void TakeDamage()
    {
        health -= Player.instance.playerDamage;
        if(health <= 0)
        {
          //  if(transform.parent.GetComponent<UpgradeCard>())
            {
                transform.parent.GetComponent<BoxCollider>().enabled = true;
            }

            Destroy(gameObject);
        }
    }

}
