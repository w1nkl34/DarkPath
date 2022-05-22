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

      int level = 1;
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

        if(damageCounter > 0.3 && waitAttackFinish == false)
        {
            closestEnemy = GetClosestEnemy();
            waitAttackFinish = true;
            damageCounter = 0;
            animator.SetBool("attacking",true);
            animator.SetLayerWeight(1,1);
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
            LeanTween.move(other.gameObject,transform.position,0.15f).setOnComplete(() => {
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }


    public IEnumerator AttackCor()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(AttackCor());
    }

    public void Attack()
    {
        int x = Random.Range(0,2);
        for(int i = 0; i<10; i++)
        {
        // if(x == 0)
        // Instantiate(bullet2,new Vector3(transform.position.x,transform.position.y+1,transform.position.z),
        // Quaternion.Euler(0,Random.Range(0f,360f),0));
         if(x == 0) Instantiate(bullet2,new Vector3(transform.position.x + Random.Range(-2f,2f),transform.position.y+1,transform.position.z),transform.rotation);
        if(x == 1) Instantiate(bullet,new Vector3(transform.position.x + Random.Range(-2f,2f),transform.position.y+1,transform.position.z),transform.rotation);
        // if(x == 2)
        // Instantiate(bullet3,new Vector3(transform.position.x + Random.Range(-2f,2f),transform.position.y+1,transform.position.z),transform.rotation);
        Instantiate(bullet3,new Vector3(transform.position.x,transform.position.y+1,transform.position.z),
        Quaternion.Euler(0,Random.Range(0f,360f),0));

        }
    }
}
