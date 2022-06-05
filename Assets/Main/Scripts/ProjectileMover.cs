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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            rb.velocity = transform.forward * speed;
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
                    var hitInstance = Instantiate(hit, collision.transform.position, Quaternion.identity);
                    Constants.hitParticleCount++;
                    Destroy(hitInstance, 2);

                }
                if(collision.gameObject.tag == "Enemy")
                {
                    currentPierceCount++;
                    collision.gameObject.GetComponent<EnemyControllerNoEcs>().DamagePlayer((int)( Random.Range(damage, damage * 1.2f)),push);
                }
            }
            if(pierceCount == 0 || pierceCount == currentPierceCount)
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
