using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform firedPoint;
    Weapon relatedWeaponComponent;

    [SerializeField] float moveSpeed;
    private Vector3 firedPointCurrent;
    private float fireDist;
    [SerializeField] GameObject relatedWeapon; 

    [SerializeField] GameObject hitEffect;
    public bool firstBullet,secondBullet = false;

    [Header("Bigger Bullets")]
    [SerializeField] Vector3 biggerScale;
    [SerializeField] float biggerScaler = 1.25f;
    private void Start() 
    {
        firedPointCurrent = firedPoint.position;
        
        fireDist =  relatedWeapon.GetComponent<Weapon>().GetWeaponsFireRange();

        biggerScale = transform.localScale * biggerScaler;

        if(GameManager.instance.bulletSizeUp)
        {
            Debug.Log("bigger scale on");
            transform.localScale = biggerScale;
        }
    }

    void Update()
    {
        if(!relatedWeapon.GetComponent<Weapon>().doubleShotActive)
        {
            if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);
            }   
            else
            {
                Destroy(gameObject); //setactive false da olabilir
            }
        }
        else if(relatedWeapon.GetComponent<Weapon>().doubleShotActive)
        {
            if(firstBullet)
            {
                if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist))
                {
                    transform.position = new Vector3(transform.position.x + moveSpeed/6 * Time.deltaTime , transform.position.y, 
                        transform.position.z + moveSpeed * Time.deltaTime);
                }   
                else
                {
                    Destroy(gameObject); //setactive false da olabilir
                }
            }
            else if(secondBullet)
            {
                if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist))
                {
                    transform.position = new Vector3(transform.position.x - moveSpeed/6 * Time.deltaTime , transform.position.y, 
                        transform.position.z + moveSpeed * Time.deltaTime);
                }   
                else
                {
                    Destroy(gameObject); //setactive false da olabilir
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage();
            PlayHitFX();
            Destroy(gameObject);
        }
    }

    private void PlayHitFX()
    {
        if (hitEffect != null)
        {
            GameObject hitfx = Instantiate(hitEffect, transform.position, Quaternion.identity);
            hitfx.GetComponent<ParticleSystem>().Play();
        }
    }

    public void SetRelatedWeapon(GameObject newWeapon)
    {
        relatedWeapon = newWeapon;
    }
}
