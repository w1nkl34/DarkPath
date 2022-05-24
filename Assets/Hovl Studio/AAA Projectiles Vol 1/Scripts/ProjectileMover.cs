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

    public bool rioc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // if (flash != null)
        // {
        //     var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
        //     flashInstance.transform.forward = gameObject.transform.forward;
            
        //     var flashPs = flashInstance.GetComponent<ParticleSystem>();
        //     if (flashPs != null)
        //     {
        //         Destroy(flashInstance, flashPs.main.duration);
        //     }
        //     else
        //     {
        //         var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
        //         Destroy(flashInstance, flashPsParts.main.duration);
        //     }
        // }
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
        if(collision.gameObject.tag != "Player" && collision.gameObject.tag != "bullet" && collision.gameObject.tag != "experience" )
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
               
                    bool push = false;
                    // int pushVal = Random.Range(0,3);
                    // if(pushVal == 0 )
                    // push = true;
                    collision.gameObject.GetComponent<EnemyControllerNoEcs>().DamagePlayer(Random.Range(10,15),push);
                }
               
            }

            if(!rioc)
            {
                // rb.constraints = RigidbodyConstraints.FreezeAll;
                // speed = 0;
                // foreach (var detachedPrefab in Detached)
                // {
                //     if (detachedPrefab != null)
                //     {
                //         detachedPrefab.transform.parent = null;
                //     }
                // }
                // if(collision.gameObject.tag == "Enemy")
                // {
                //  if(!collision.gameObject.GetComponent<EnemyControllerNoEcs>().dead)
                // Destroy(gameObject);
                // }
                // else
                Destroy(gameObject);

            }
        }

    }
}
