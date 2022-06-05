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
    int spawnCounter = 12;
    public UIController uIController;
    public List<GameObject> mainSkills;
    public List<GameObject> allSkills;
    void Awake()
    {
        Application.targetFrameRate = 30;
        myCharacterController = character.GetComponent<MyCharacterController>();
    }

    void Start()
    {
        InitGame();
        StartCoroutine(StartSpawnCor());
        Spawn(true);
    }

    void InitGame()
    {
        GameObject mainSkillInst = Instantiate(mainSkills[0]);
        mainSkillInst.GetComponent<Skill>().myCharacterController = myCharacterController;
        myCharacterController.mainSkill = mainSkillInst.GetComponent<Skill>();
    }

    void Update()
    {
        spawnCounter = 12 + (3 * (myCharacterController.level-1));
        if(Vector3.Distance(characterPos,character.transform.position) > 40)
        Spawn(true);
    }

    public void UpdateExperience()
    {
        myCharacterController.currentExp += 50;
        if (myCharacterController.currentExp >= myCharacterController.level * 100)
        {
            myCharacterController.currentExp = 0;
            myCharacterController.level++;
            HandleLevelUp();
        }
        uIController.UpdateExpBar(myCharacterController.level, myCharacterController.currentExp);
    }

    public void HandleLevelUp()
    {
        Time.timeScale = 0;
        List<Skill> skills = GetSkills();
        if (skills == null || skills.Count == 0)
        {
            Time.timeScale = 1;
            return;
        }
        uIController.OpenLevelUpBar();
        uIController.GenerateLevelUp(skills,myCharacterController);
    }

    public List<Skill> GetSkills()
    {
        int maxCount;
        
        List<GameObject> affordableSkills = new List<GameObject>();
        foreach(GameObject skill in allSkills)
        {
            affordableSkills.Add(skill);
        }
        List<Skill> returnSkills = new List<Skill>();

        if(myCharacterController.mainSkill.level != myCharacterController.mainSkill.maxLevel)
        {
            affordableSkills.Add(myCharacterController.mainSkill.gameObject);
        }
        
        if(myCharacterController.allSkills.Count != 0)
        {
        foreach(Skill skill in myCharacterController.allSkills)
        {
            if(skill.level == skill.maxLevel)
            {
                foreach(GameObject allSk in affordableSkills)
                {
                    if(allSk.GetComponent<Skill>().skillName == skill.skillName)
                    {
                        affordableSkills.Remove(allSk);
                        break;
                    }
                }
            }
        }
        }

        if (affordableSkills.Count == 2)
            maxCount = 2;
        else if (affordableSkills.Count == 1)
            maxCount = 1;
        else if (affordableSkills.Count == 0)
            maxCount = 0;
        else
            maxCount = 3;
        if(maxCount == 0)
        {
            return returnSkills;
        }

         List<int> uniqueNumbers = new List<int>();
          List<int> finishedList = new List<int>();

        for (int i = 0; i < affordableSkills.Count; i++)
        {
            uniqueNumbers.Add(i);
        }
        for (int i = 0; i < affordableSkills.Count; i++)
        {
            int ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count )];
            finishedList.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }

        for (int j = 0; j < maxCount; j++)
        {
            returnSkills.Add(affordableSkills[finishedList[j]].GetComponent<Skill>());
        }

        return returnSkills;
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
            // var value = Random.Range(0,enemies.Length);
            var value = 2;
            GameObject instantiatedEnemy = Instantiate(enemies[value] ,
            new Vector3(position.x +  character.transform.position.x,1,position.z  + character.transform.position.z),Quaternion.identity);

            float hp = Random.Range(10f,10f) + 2f * myCharacterController.level;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().maxHealth = hp;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().moveSpeed = 2.5f;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().currentHealth = hp;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().gm = this;
            instantiatedEnemy.GetComponent<EnemyControllerNoEcs>().init = true;
            Constants.spawnPool++;
        }
    }
}