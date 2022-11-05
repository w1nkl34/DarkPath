using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShot: Skill
{

    public int spawnCount = 1;
    public bool push = false;
    public int pierceCount = 1;
    public override void Attack()
    {
        base.Attack();
        for (int i = 0; i < spawnCount; i++)
        {
            int val = 0;
            if (i != 0)
                val = Random.Range(-30, 30);
            StartCoroutine(AttackDelay(val, i));
        }
    }

    public IEnumerator AttackDelay(int val,int i)
    {
        yield return new WaitForSeconds(0.05f * (float)i);
        GameObject inst = Instantiate(base.prefab,
        new Vector3(base.myCharacterController.transform.position.x, base.myCharacterController.transform.position.y + 1f,
        base.myCharacterController.transform.position.z)
        //+ myCharacterController.transform.forward * 2
        , base.myCharacterController.transform.rotation * Quaternion.Euler(0, val, 0));
        inst.GetComponent<ProjectileMover>().damage = base.damage + myCharacterController.damage;
        inst.GetComponent<ProjectileMover>().push = push;
        inst.GetComponent<ProjectileMover>().pierceCount = pierceCount;
        inst.GetComponent<ProjectileMover>().target = myCharacterController.closestEnemy.transform;

    }

    public override void LevelUp()
    {
        base.LevelUp();
        switch (base.level)
        {
            case 2:
                base.damage+= 5;
                push = true;
                break;
            case 3:
                pierceCount++;
                spawnCount++;
                break;
            case 4:
                base.damage += 5;
                spawnCount++;
                break;
            case 5:
                base.damage += 5;
                spawnCount++;
                break;
            case 6:
                base.damage += 5;
                break;
            case 7:
                pierceCount++;
                break;
        }
    }

}
