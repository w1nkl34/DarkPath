using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyControllerNoEcs : MonoBehaviour
{
 
    Rigidbody rb;
    Transform player;
    public GameObject healthBar;
    public Image healthFill;
    public bool init = false;
    public float maxHealth;
    public float currentHealth;
    public Animator anim;
    public float moveSpeed;

    public Renderer _renderer;

    public GameManager gm;

    public GameObject damagePrefab;
    bool dead = false;
    

    void Awake()
    {
        healthBar.GetComponent<Canvas>().worldCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void FixedUpdate()
    {
        if(init)
        {
            if(currentHealth <= 0 && dead == false)
            {
                KillPlayer();
                SpawnExperience();
            }
            if(!dead)
            {
                GetDistance();
                transform.position = Vector3.MoveTowards(transform.position,player.position,moveSpeed * Time.deltaTime);
                transform.LookAt(player.position,Vector3.up);
                healthBar.transform.LookAt(Camera.main.transform.position,Vector3.up);
            }
        }

        CanDamage();
    }

    void GetDistance()
    {
       float distance = Vector3.Distance(transform.position,player.position);
       if(distance > 10)
       {
           rb.velocity = Vector3.zero;
           rb.angularVelocity = Vector3.zero;
       }
        if(distance > 30)
        KillPlayer();
    }

    public void DamagePlayer(float damage,bool push)
    {
        if(Constants.damageTextCount < Constants.maxDamageTextCount)
        {
        GameObject textIn = Instantiate(damagePrefab,new Vector3(transform.position.x+Random.Range(-1f,1f),transform.position.y+2,transform.position.z),Quaternion.identity);
        textIn.GetComponent<DamageText>().text.text = damage.ToString();
        }
        Material mymat = _renderer.material;
        LeanTween.value(gameObject,Color.black, Color.grey, 0.05f).setOnUpdate((Color val) =>
        {
                mymat.SetColor("_EmissionColor", val);

         }).setOnComplete(() => {
              LeanTween.value(gameObject,Color.white, Color.black, 0.05f).setOnUpdate((Color val) =>
                {
                mymat.SetColor("_EmissionColor", val);
                });
         });

        LeanTween.value(gameObject,new Vector3(1,1,1), new Vector3(0.9f,0.9f,0.9f), 0.1f).setOnUpdate((Vector3 val) =>
        {
            transform.localScale = val;
         }).setEase(LeanTweenType.pingPong).setOnComplete(() => {
                    transform.localScale = new Vector3(1,1,1);
         });
        if(push)
        rb.AddForce(-transform.forward *2 *moveSpeed,ForceMode.Impulse);
        currentHealth -= damage;
        healthFill.fillAmount = currentHealth /maxHealth;
    }

    public void KillPlayer()
    {
        dead = true;
        healthBar.SetActive(false);
        GetComponent<Collider>().enabled = false;
        transform.localScale = new Vector3(0.9f,0.9f,0.9f);
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        anim.SetTrigger("Die");
        Destroy(gameObject,0.75f);
    
    }

    public void SpawnExperience()
    {
        var value = Random.Range(0,5);
        GameObject exp = null;
        if(value <= 2)
        exp =  Instantiate(gm.experienceParticle,transform.position,Quaternion.Euler(new Vector3(0,0,0)));
        if(exp != null)
        Destroy(exp,30);
    }

    public bool canDamage = false;

    public float damageSpeed = 1;

    public float damageTimer = 0;

    public void CanDamage()
    {
        damageTimer+= Time.deltaTime;
        if(damageTimer > damageSpeed)
        {
            canDamage = true;
        }
    }


    void OnCollisionStay(Collision other)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if(other.gameObject.tag == "Player" && canDamage)
        {
            canDamage = false;
            damageTimer = 0;
            other.gameObject.GetComponent<MyCharacterController>().DamagePlayer(Random.Range(10,20));
        }
    }
    
    private void RotateRgb(Transform _target)
    {
        Vector3 localTarget = transform.InverseTransformPoint(_target.position);
        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * 100);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void OnDestroy()
    {
      Constants.spawnPool--;
    }

}
