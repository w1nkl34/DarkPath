using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : Skill
{

    public GameObject whirl;
    public override void Attack()
    {
        base.Attack();
        whirl = Instantiate(base.prefab,
        new Vector3(base.myCharacterController.transform.position.x, base.myCharacterController.transform.position.y +1,
        base.myCharacterController.transform.position.z), base.myCharacterController.transform.rotation * Quaternion.Euler(0, 0, 0));
        whirl.GetComponent<WhirlwindController>().damage = base.damage;
    }
    public override void LevelUp()
    {
        base.LevelUp();
        whirl.GetComponent<WhirlwindController>().damage = base.damage + 5 * base.level;
        //switch (base.level)
        //{
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //    case 4:
        //        break;
        //    case 5:
        //        break;
        //    case 6:
        //        break;
        //    case 7:
        //        break;
        //}
    }


}
