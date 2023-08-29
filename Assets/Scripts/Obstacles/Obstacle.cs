using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Obstacle : MonoBehaviour , IDamagable, IInteractable
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    float currentHealth;

    [Header("Health Bar")]
    [SerializeField] Image fillImage;
    [SerializeField] GameObject healthBar;
    [Header("Animation")]
    [SerializeField] List<GameObject> parts;
    [SerializeField] List<float> healthPercentages;
    void Start()
    {
        currentHealth = maxHealth;
        if(transform.parent.GetComponent<UpgradeCard>())
        {
            transform.parent.gameObject.layer = LayerMask.NameToLayer("ObstacledCard");
            transform.parent.GetComponentInChildren<CollideWithPlayer>().gameObject.layer = LayerMask.NameToLayer("ObstacledCard");
        }

        fillImage.fillAmount = (float)currentHealth/ (float)maxHealth;
        healthBar.SetActive(false);
    }

    public void Interact()
    {
        Player.instance.KnockbackPlayer();
        if(transform.parent.GetComponent<UpgradeCard>())
        {
            transform.parent.gameObject.layer = LayerMask.NameToLayer("ObstacledCard");
            transform.parent.GetComponentInChildren<CollideWithPlayer>().gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");
        }
        transform.gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");
    }

    private void HitEffect()
    {
        CheckValues(healthPercentages[0],parts[0]);
        CheckValues(healthPercentages[1],parts[1]);
        CheckValues(healthPercentages[2],parts[2]);
        CheckValues(healthPercentages[3],parts[3]);
        CheckValues(healthPercentages[4],parts[4]);
    }

    public void TakeDamage()
    {
        currentHealth -= Player.instance.playerDamage;
        HitEffect();

        healthBar.SetActive(true);
        fillImage.fillAmount = (float)currentHealth/ (float)maxHealth;

        if(currentHealth <= 0)
        {
            if(transform.parent.GetComponent<UpgradeCard>())
            {
                transform.parent.gameObject.layer = LayerMask.NameToLayer("Default");
                transform.parent.GetComponentInChildren<CollideWithPlayer>().gameObject.layer = LayerMask.NameToLayer("OnlyPlayer");
            }

            Destroy(gameObject);
        }
    }

    private void CheckValues(float healthPercentage, GameObject obstaclePart)
    {
        if(currentHealth <= maxHealth * healthPercentage && obstaclePart.transform.parent == gameObject.transform)
        {
            
            obstaclePart.GetComponent<Rigidbody>().useGravity = true;
            obstaclePart.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            obstaclePart.GetComponent<Rigidbody>().velocity = new Vector3(0,GameManager.instance.obstaclePushValue/3,
                GameManager.instance.obstaclePushValue);

            float rand = Random.Range(0,180);
            Vector3 randVector = new Vector3(rand,rand,rand);
            obstaclePart.transform.DORotate(randVector,2);
            obstaclePart.transform.parent = null;

        }
    }
}
