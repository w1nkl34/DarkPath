using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCharacterController : MonoBehaviour
{

    public GameObject bullet;
    public GameObject bullet2;
    public GameObject bullet3;

     DynamicJoystick joystick;
     Rigidbody _rigidbody;

     GameManager gm;

     Animator animator;
     public float moveSpeed = 2f;

     public float currentHealth = 300;
     public float maxHealth = 300;

     public GameObject healthBar;

     public Image healthFillBar;

     CharacterController characterController;

      public int level = 1;
      float currentExp = 0;

      float damageCounter = 0;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        _rigidbody = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<DynamicJoystick>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        // StartCoroutine(AttackCor());
    }

    public void DamagePlayer(float damage)
    {
        currentHealth -= damage;
        healthFillBar.fillAmount = currentHealth / maxHealth;
    }

    bool waitAttackFinish = false;
    void Update()
    {
        if(waitAttackFinish == false)
        damageCounter += Time.deltaTime;

        float decVal = 0.1f * (level-1);
        if(decVal <= 0.1)
        decVal = 0.1f;
        if(damageCounter > 0.5 - decVal && waitAttackFinish == false)
        {
            closestEnemy = GetClosestEnemy();
                if(closestEnemy != null)
                {

            waitAttackFinish = true;
            damageCounter = 0;
        
            animator.SetBool("attacking",true);
                }

        }
    }

    public void AttackFinish(string attackFinished)
    {
        waitAttackFinish = false;
        animator.SetBool("attacking",false);
        // animator.SetLayerWeight(1,0);
    }

    GameObject closestEnemy = null;

    public void AttackRelease(string attackRelease)
    {
        Attack();
    }
    void FixedUpdate()
    {
        Vector3 xV = new Vector3(joystick.Horizontal,1,joystick.Vertical);
        xV = transform.InverseTransformDirection(xV);
        animator.SetFloat("InputHorizontal",xV.x);
        animator.SetFloat("InputVertical",xV.z);
        animator.SetFloat("InputMagnitude", new Vector2(joystick.Horizontal,joystick.Vertical).magnitude);
        healthBar.transform.LookAt(Camera.main.transform.position,Vector3.up);
        _rigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed,_rigidbody.velocity.y,joystick.Vertical * moveSpeed);
      
         if(closestEnemy != null )
            {
                     Vector3 targetDirection = closestEnemy.transform.position - transform.position;
                    float singleStep = 12 * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);
            }

             if(waitAttackFinish == false || closestEnemy == null)
            {
                    float singleStep = 12 * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, _rigidbody.velocity, singleStep, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);
            
            }

    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "experience")
        {
            LeanTween.move(other.gameObject,new Vector3(transform.position.x,transform.position.y+2,transform.position.z),0.15f).setOnComplete(() => {
                GainExperience();
                Destroy(other.gameObject);
            });
        }
    }

    public void GainExperience()
    {
        currentExp += 10;
        if(currentExp >= level * 100)
        {
            currentExp = 0;
            level++;
        }
        gm.UpdateExperience(level,currentExp);
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


    public void Attack()
    {
        int x = Random.Range(0,2);
        for(int i = 0; i<level; i++)
        {
            int val = 0;
            switch(i)
            {
                case 0:
                val = 0;
                break;
                case 1:
                val = 10;
                break;
                case 2:
                val = -10;
                break;
                case 3:
                val = 20;
                break;
                case 4:
                val = -20;
                break;
                case 5:
                val = -5;
                break;
                case 6:
                val = -5;
                break;
            }
          Instantiate(bullet,new Vector3(transform.position.x ,transform.position.y+1,transform.position.z),transform.rotation * Quaternion.Euler(0,val,0));

        // Instantiate(bullet3,new Vector3(transform.position.x,transform.position.y+1,transform.position.z),
        // Quaternion.Euler(0,Random.Range(0f,360f),0));

        }
    }
}
