using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmallingObstacles : MonoBehaviour,IDamagable,IInteractable
{
    [Header("Hexagon")]
    [SerializeField] GameObject hexagon;
    [SerializeField] float smallingValue;

    [Header("Health")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] TMP_Text healthText;

    [Header("Money")]
    [SerializeField] Transform moneySpawnTransform;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] int moneysValue;

    [Header("Gun")]
    [SerializeField] GameObject weaponSelector;

    [Header("State")]
    [SerializeField] bool moneyHexagon;
    [SerializeField] bool gunHexagon;

    private void Start() 
    {
        maxHealth = Random.Range(10,30);
        currentHealth = maxHealth;
        hexagon.transform.localScale = new Vector3(hexagon.transform.localScale.x, hexagon.transform.localScale.y, 
            hexagon.transform.localScale.z + 2 * maxHealth);
        UpdateHealthText();
        hexagon.GetComponent<MeshRenderer>().material = GameManager.instance.hexagonMaterials[Random.Range(0,GameManager.instance.hexagonMaterials.Count)];
        if(gunHexagon)
        {
            weaponSelector.GetComponent<Rigidbody>().useGravity = true;
            weaponSelector.gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");
        }
    }

    public void TakeDamage()
    {
        currentHealth -= 1;
        hexagon.transform.localScale = new Vector3(hexagon.transform.localScale.x, hexagon.transform.localScale.y, 
            hexagon.transform.localScale.z - smallingValue);
        UpdateHealthText();
        if(currentHealth <= 0)
        {
            if(moneyHexagon)
            {
                GameObject spawnedMoney = Instantiate(moneyPrefab,moneySpawnTransform.position,Quaternion.identity);
                spawnedMoney.GetComponent<Money>().value = moneysValue;
                spawnedMoney.GetComponent<Rigidbody>().useGravity = true;
                Destroy(gameObject);
            }
            else if(gunHexagon)
            {
                weaponSelector.transform.parent = null;
                weaponSelector.gameObject.layer = LayerMask.NameToLayer("GunStand");
                Destroy(gameObject);
            }
        }
    }

    private void UpdateHealthText()
    {
        healthText.text = Mathf.RoundToInt(currentHealth).ToString();
    }

    public void Interact()
    {
        Player.instance.KnockbackPlayer();
    }
}
