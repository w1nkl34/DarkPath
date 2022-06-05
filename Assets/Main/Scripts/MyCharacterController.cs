using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCharacterController : MonoBehaviour
{
    public GameObject bullet;
    DynamicJoystick joystick;
    Rigidbody _rigidbody;
    GameManager gm;
    Animator animator;

    public GameObject healthBar;
    public Image healthFillBar;
    public Image delayHealthFill;
    public float lastFillAmount = 1;
    public Text healthText;
    GameObject closestEnemy = null;

    public float moveSpeed = 6f;
    public float currentHealth = 300;
    public float maxHealth = 300;
    public float attackSpeedIncreasePercent = 0;
    public int level = 1;
    public float damage = 10;
    public float currentExp = 0;

    public Skill mainSkill;
    public List<Skill> allSkills = new List<Skill>();

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        _rigidbody = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<DynamicJoystick>();
        animator = GetComponent<Animator>();
        healthText.text = currentHealth.ToString();
        AssignCharacter();
    }
    public void AssignCharacter()
    {
        animator.SetFloat("attackSpeedIncrease", 1 + (attackSpeedIncreasePercent / 100));
    }

    public void UpdateSkills()
    {
      foreach(Skill child in allSkills)
      {
        if(child.active == false)
         child.Attack();
      }
    }

    public void DamagePlayer(float damage)
    {
        currentHealth -= damage;
        healthFillBar.fillAmount = currentHealth / maxHealth;
        float degree = lastFillAmount - (currentHealth /maxHealth);
        lastFillAmount -= degree;
        StartCoroutine(DelayFill(degree));
        healthText.text = currentHealth.ToString();
    }

    public IEnumerator DelayFill(float degree)
    {
        yield return new WaitForSeconds(0.1f);
        delayHealthFill.fillAmount -= degree;
    }

    void Update()
    {
       closestEnemy = GetClosestEnemy();
       if(closestEnemy != null)
       {
           animator.SetBool("attacking",true);
       }
       else
       {
           animator.SetBool("attacking",false);
       }
    }

    void FixedUpdate()
    {
        Vector3 xV = new Vector3(joystick.Horizontal,1,joystick.Vertical);
        xV = transform.InverseTransformDirection(xV);
        animator.SetFloat("InputHorizontal",xV.x);
        animator.SetFloat("InputVertical",xV.z);
        animator.SetFloat("InputMagnitude", new Vector2(joystick.Horizontal,joystick.Vertical).magnitude);

        healthBar.transform.LookAt(Camera.main.transform.position,Vector3.up);
        _rigidbody.velocity = new Vector3(joystick.Horizontal,_rigidbody.velocity.y,joystick.Vertical ).normalized * moveSpeed;
      
        if(closestEnemy != null )
        {
            Vector3 targetDirection = closestEnemy.transform.position - transform.position;
            float singleStep = 12 * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            float singleStep = 12 * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, _rigidbody.velocity, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.ToString() == "experience")
        {
            LeanTween.move(other.gameObject,new Vector3(transform.position.x,transform.position.y+2,transform.position.z),0.15f).setOnComplete(() => {
                GainExperience();
                Destroy(other.gameObject);
            });
        }
    }

    public void GainExperience()
    {
      
        gm.UpdateExperience();
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
            if (dist < minDist && !t.dead )
            {
                tMin = t.gameObject;
                minDist = dist;
            }
        }
        return tMin;
    }
    public void AttackRelease()
    {
        mainSkill.Attack();
    }
}
