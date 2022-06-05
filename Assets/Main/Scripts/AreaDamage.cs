using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    public GameObject hit;
    public float damage;
    bool push = false;
    bool damageOnce = false;
    public float attackTime;
    float currentAttackTime = 0;
    public bool delayedAttack;
    public float delayTimer;
    bool delayCallOnce = false;
    List<EnemyControllerNoEcs> damagedEnemies = new List<EnemyControllerNoEcs>();
    private void Awake()
    {
        if (delayedAttack == true)
            damageOnce = true;
         Destroy(gameObject, 3);
    }

    private void Update()
    {
        currentAttackTime += Time.deltaTime;
        if(currentAttackTime >= attackTime)
            damageOnce = true;
        if(delayedAttack == true)
        {
            if(currentAttackTime >= delayTimer && !delayCallOnce)
            {
                delayCallOnce = true;
                damageOnce = false;
            }
        }

    }
    void OnTriggerStay(Collider collision)
    {
        if(!damageOnce)
        {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "bullet" && collision.gameObject.tag != "experience" && collision.gameObject.tag != "pet")
        {
            if (hit != null)
            {

               
                if (collision.gameObject.tag == "Enemy")
                {
                     if(!damagedEnemies.Contains(collision.gameObject.GetComponent<EnemyControllerNoEcs>()))
                        {
                            if (Constants.hitParticleCount <= Constants.maxHitParticleCount)
                            {
                                var hitInstance = Instantiate(hit, collision.transform.position, Quaternion.identity);
                                Constants.hitParticleCount++;
                                Destroy(hitInstance, 2);
                            }
                            collision.gameObject.GetComponent<EnemyControllerNoEcs>().DamagePlayer((int)(Random.Range(damage, damage * 1.2f)), push);
                            damagedEnemies.Add(collision.gameObject.GetComponent<EnemyControllerNoEcs>());
                        }
                    }
                }
        }
        }


    }
}
