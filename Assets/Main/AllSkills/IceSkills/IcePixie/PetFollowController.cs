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
    public float damage;
    Animator anim;
    bool attackingNow = false;
    public float baseAttackSpeed = 3f;
    public float attackSpeed = 3f;
    float attackTimer = 0f;

    void Awake()
    {
        myCharacterController = FindObjectOfType<MyCharacterController>();
        character = myCharacterController.transform;
        target = new Vector3(character.position.x,3,character.position.z);
        anim = GetComponent<Animator>();
    } 

    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackSpeed && attackingNow == false)
        {
            attackingNow = true;
            anim.SetBool("attack", true);
        }
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

    public void AttackRelease()
    {
        if(closestEnemy != null)
         Attack();
    }

    public void AttackFinish()
    {
        anim.SetBool("attack", false);
        attackTimer = 0f;
        attackingNow = false;
    }


    public void Attack()
    {
     
            if(closestEnemy != null)
            {
                GameObject inst = Instantiate(bullet,new Vector3(closestEnemy.transform.position.x,2, closestEnemy.transform.position.z),
                Quaternion.Euler(0, 0,0));
                inst.GetComponent<AreaDamage>().damage = damage;
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


}
