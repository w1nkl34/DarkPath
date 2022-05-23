using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public GameObject character;
    MyCharacterController myCharacterController;
    public GameObject[] enemies;
    public Vector3 characterPos;
    public GameObject experienceParticle;
    int spawnCounter = 5;

    public UIController uIController;

    

    void Awake()
    {
        Application.targetFrameRate = 30;
        myCharacterController = character.GetComponent<MyCharacterController>();
    }


    void Start()
    {
        StartCoroutine(StartSpawnCor());
        Spawn(true);
    }

    void Update()
    {
        spawnCounter = 5 + (3 * (myCharacterController.level-1));
        if(Vector3.Distance(characterPos,character.transform.position) > 40)
        Spawn(true);
    }

    public void UpdateExperience(int level,float experience)
    {
        uIController.UpdateExpBar(level,experience);
    }

    public IEnumerator StartSpawnCor()
    {
        yield return new WaitForSeconds(4f);
        Spawn(false);
        StartCoroutine(StartSpawnCor());
    }


    public void Spawn(bool spawn)
    {
      
        characterPos = character.transform.position;
        if(Constants.spawnPool <= spawnCounter  || spawn == true)
        {
             StartCoroutine(SpawnEnemies(0,spawnCounter/5,0));
             StartCoroutine(SpawnEnemies(0.2f,spawnCounter/5,spawnCounter/5));
             StartCoroutine(SpawnEnemies(0.4f,spawnCounter/5,spawnCounter/5 *2 ));
             StartCoroutine(SpawnEnemies(0.6f,spawnCounter/5,spawnCounter/5 *3));
             StartCoroutine(SpawnEnemies(0.8f,spawnCounter/5,spawnCounter/5 * 4));
        }
    }

    public IEnumerator SpawnEnemies(float waitTime,int spawnCount,int startIndex)
    {
        yield return new WaitForSeconds(waitTime);
        for(int i = startIndex;  i< startIndex + spawnCount; i++)
        {
             float3 position =  Random.insideUnitSphere * new float3( 30f , 1f ,30f );

            if(position.x > 25 || position.z > 25 || position.x < -25 || position.z <-25)
            {
            }
            else
            {
            if(position.x <= 15 && position.x > 0)
                position.x = Random.Range(15f,30f);
            if(position.z <=15  && position.z > 0)
                position.z = Random.Range(15f,30f);
            if(position.x >= -15 &&position.x <0 )
                position.x = Random.Range(-15f,-30f);
            if(position.z >= -15 &&position.z <0 )
                position.z = Random.Range(-15f,-30f);
            }
            var value = Random.Range(0,enemies.Length);
            GameObject instantiatedEnemy = Instantiate(enemies[value] ,
            new Vector3(position.x +  character.transform.position.x,1,position.z  + character.transform.position.z),Quaternion.identity);

            float hp = Random.Range(25f,40f);
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().maxHealth = hp;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().moveSpeed = Random.Range(3.5f,3.5f);
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().currentHealth = hp;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().gm = this;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().init = true;
            Constants.spawnPool++;
        }
    }
}