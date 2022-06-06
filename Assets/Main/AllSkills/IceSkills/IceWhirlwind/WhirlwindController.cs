using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindController : MonoBehaviour
{
    public Transform target;
    public GameObject hit;
    public float damage;
    public bool push = false;

    public float attackTime = 0.5f;
    private float attackTimePlus;
    float currentAttackTime = 0;
    private bool canAttackNow = false;
    List<EnemyControllerNoEcs> damagedEnemies = new List<EnemyControllerNoEcs>();

    void Start()
    {
        target = FindObjectOfType<MyCharacterController>().gameObject.transform;
    }
    private void Update()
    {
        attackTimePlus = attackTime + 0.1f;
        transform.position = new Vector3(target.position.x,target.position.y+1,target.position.z);
        currentAttackTime += Time.deltaTime;
        if (currentAttackTime >= attackTime)
        {
            canAttackNow = true;
        }
        if(canAttackNow && currentAttackTime >= attackTimePlus)
        {
            canAttackNow = false;
            currentAttackTime = 0;
            damagedEnemies = new List<EnemyControllerNoEcs>();
        }

    }

    void OnTriggerStay(Collider collision)
    {
        if(canAttackNow)
        {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "bullet" && collision.gameObject.tag != "experience" && collision.gameObject.tag != "pet")
        {
            if (hit != null)
            {
                if (collision.gameObject.tag == "Enemy")
                {
                    if (!damagedEnemies.Contains(collision.gameObject.GetComponent<EnemyControllerNoEcs>()))
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
