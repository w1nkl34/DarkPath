using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    public int pierceCount;
    public int currentPierceCount = 0;
    public bool push = false;
    public float damage;
    public Transform target;
    Vector3 directionToTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 displacementFromTarget =
            new Vector3(target.transform.position.x , target.transform.position.y + 1f, target.transform.position.z) - transform.position;
        directionToTarget = displacementFromTarget.normalized;

        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        
        Destroy(gameObject,5);
	}
    void FixedUpdate ()
    {
        if (speed != 0)
        {
            rb.velocity = new Vector3(transform.forward.x,directionToTarget.y,transform.forward.z) * speed;
        }
	}

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag != "Player" && collision.gameObject.tag != "bullet" && collision.gameObject.tag != "experience" && collision.gameObject.tag != "pet")
        {
            if (hit != null)
            {

                if(Constants.hitParticleCount <= Constants.maxHitParticleCount)
                {
                    var hitInstance = Instantiate(hit, transform.position, Quaternion.identity);
                    Constants.hitParticleCount++;
                    Destroy(hitInstance, 2);

                }
                if(collision.gameObject.tag == "Enemy")
                {
                    currentPierceCount++;
                    collision.gameObject.GetComponent<EnemyControllerNoEcs>().DamagePlayer((int)( Random.Range(damage, damage * 1.2f)),push);
                }
            }


            if(pierceCount == 0 || pierceCount == currentPierceCount || collision.gameObject.tag != "Enemy")
            {
                foreach (var detachedPrefab in Detached)
                {
                    if (detachedPrefab != null)
                    {
                        detachedPrefab.transform.parent = null;
                    }
                }
                Destroy(gameObject);
            }
            
        }

    }
}
