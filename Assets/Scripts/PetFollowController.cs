using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PetFollowController : MonoBehaviour
{
    MyCharacterController myCharacterController;
    Transform character;

    GameObject closestEnemy = null;

    Vector3 target;

    bool moveToCharacter = false;

    bool setDestination = false;

    Vector3 destination;

    bool getRandom = false;

    public GameObject bullet;


    void Awake()
    {
        myCharacterController = FindObjectOfType<MyCharacterController>();
        character = myCharacterController.transform;
        target = new Vector3(character.position.x,3,character.position.z);
    } 

    void Start()
    {
       StartCoroutine(AttackCor());
    }
    void FixedUpdate()
    {
        closestEnemy = GetClosestEnemy();

        Vector3 targetDirection = target - transform.position;
       float singleStep = 4 * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if(Vector3.Distance(transform.position,character.position) > 5)
        {
            moveToCharacter = true;
            getRandom = false;
            target = new Vector3(character.position.x,3,character.position.z);
        }
        else
        {
            if(closestEnemy != null && !moveToCharacter)
            {
                target = new Vector3(closestEnemy.transform.position.x,3,closestEnemy.transform.position.z);
            }
            if(!getRandom)
            {
                getRandom = true;
                float3 position = UnityEngine.Random.insideUnitSphere * new float3( 4.5f , 1f , 4.5f );
                destination = new Vector3(position.x +  character.transform.position.x,3,position.z  + character.transform.position.z);
            }
            transform.position = Vector3.MoveTowards(transform.position,destination,2 * Time.deltaTime);
            if(Vector3.Distance(transform.position,destination) <= 0.2)
            {
                getRandom = false;
            }
        }
        if(moveToCharacter)
        {
            if(!setDestination)
            {
                setDestination = true;
                destination = new Vector3(character.position.x,3,character.position.z) + newDirection * 2;
            }
            transform.position = Vector3.MoveTowards(transform.position,destination,7 * Time.deltaTime);
            if(Vector3.Distance(transform.position,destination) <= 0.2)
            {
                moveToCharacter = false;
                setDestination = false;
            }

        }

    }

    GameObject GetClosestEnemy()
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        EnemyControllerNoEcs[] enemies = GameObject.FindObjectsOfType<EnemyControllerNoEcs>();
        foreach (EnemyControllerNoEcs t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist && !t.dead && dist <= 20)
            {
                tMin = t.gameObject;
                minDist = dist;
            }
        }
        return tMin;
    }

    public IEnumerator AttackCor()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f,4f));
        Attack();
        StartCoroutine(AttackCor());
    }

    public void Attack()
    {
        for(int i = 0; i<=3; i++)
        {

        Instantiate(bullet,new Vector3(transform.position.x,2,transform.position.z),
        Quaternion.Euler(0, (90 * i) + UnityEngine.Random.Range(0,45),0));
        }

    }

}
