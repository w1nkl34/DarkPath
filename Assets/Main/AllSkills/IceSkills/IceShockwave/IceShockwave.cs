using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShockwave : Skill
{
    public override void Attack()
    {
        base.Attack();
        StartCoroutine(AttackDelay());
    }

    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject inst = Instantiate(base.prefab,
        new Vector3(base.myCharacterController.transform.position.x, base.myCharacterController.transform.position.y ,
        base.myCharacterController.transform.position.z), Quaternion.Euler(0, 0, 0));
        inst.GetComponent<AreaDamage>().damage = base.damage + base.myCharacterController.damage;
        StartCoroutine(AttackDelay());
    }

    public override void LevelUp()
    {
        base.LevelUp();
        switch (base.level)
        {
            case 2:
                base.damage += 5;
                break;
            case 3:
                base.damage += 5;
                break;
            case 4:
                base.damage += 5;
                break;
            case 5:
                base.damage += 5;
                break;
            case 6:
                base.damage += 5;
                break;
            case 7:
                base.damage += 5;
                break;
        }
    }
}
