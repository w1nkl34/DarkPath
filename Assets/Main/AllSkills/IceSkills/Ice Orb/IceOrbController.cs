using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceOrbController : MonoBehaviour
{
    public Transform target;
    public Vector3 relativeDistance = Vector3.zero;
    public GameObject hit;
    public float damage;
    public bool push = false;

    void Start()
    {
        target = FindObjectOfType<MyCharacterController>().gameObject.transform;
        relativeDistance = transform.position - target.position;
    }

    public float orbitDistance = 10.0f;
    public float orbitDegreesPerSec = 180.0f;
    void Orbit()
    {
        if (target != null)
        {
            transform.position = target.position + relativeDistance;
            transform.RotateAround(target.position, Vector3.up, orbitDegreesPerSec * Time.deltaTime);
            relativeDistance = transform.position - target.position;
        }
    }

   
    void LateUpdate()
    {

        Orbit();

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "bullet" && collision.gameObject.tag != "experience" && collision.gameObject.tag != "pet")
        {
            if (hit != null)
            {

                if (Constants.hitParticleCount <= Constants.maxHitParticleCount)
                {
                    var hitInstance = Instantiate(hit, collision.transform.position, Quaternion.identity);
                    Constants.hitParticleCount++;
                    Destroy(hitInstance, 2);

                }
                if (collision.gameObject.tag == "Enemy")
                {
                    collision.gameObject.GetComponent<EnemyControllerNoEcs>().DamagePlayer((int)(Random.Range(damage, damage * 1.2f)), push);
                }
            }
          
        }

    }
}
