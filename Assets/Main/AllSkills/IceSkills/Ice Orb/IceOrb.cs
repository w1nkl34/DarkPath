using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceOrb : Skill
{
    private List<GameObject> iceOrbs = new List<GameObject>();
    public override void Attack()
    {
        base.Attack();
       iceOrbs.Add(Instantiate(base.prefab,
       new Vector3(base.myCharacterController.transform.position.x + 3, base.myCharacterController.transform.position.y + 1,
       base.myCharacterController.transform.position.z), base.myCharacterController.transform.rotation * Quaternion.Euler(0, 0, 0)));
        iceOrbs[0].GetComponent<IceOrbController>().damage = damage;
        iceOrbs[0].GetComponent<IceOrbController>().orbitDegreesPerSec = 30 * base.level;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        foreach(GameObject orb in iceOrbs)
        {
            Destroy(orb);
        }
        iceOrbs = new List<GameObject>();
        for (int i = 0; i < base.level; i++)
        {
            int valX = 0;
            int valZ = 0;
            switch(i)
            {
                case 0:
                    valX = 3;
                    valZ = 0;
                    break;
                case 1:
                    valX = -3;
                    valZ = 0;
                    break;
                case 2:
                    valX = 0;
                    valZ = -3;
                    break;
                case 3:
                    valX = 0;
                    valZ = 3;
                    break;
                case 4:
                    valX = 2;
                    valZ = -2;
                    break;
                case 5:
                    valX = -2;
                    valZ = 2;
                    break;
                case 6:
                    valX = -2;
                    valZ = -2;
                    break;
            }
           iceOrbs.Add(Instantiate(base.prefab,
           new Vector3(base.myCharacterController.transform.position.x + valX, base.myCharacterController.transform.position.y + 1,
           base.myCharacterController.transform.position.z + valZ), base.myCharacterController.transform.rotation * Quaternion.Euler(0, 0, 0)));
            iceOrbs[i].GetComponent<IceOrbController>().damage = damage + 2 * base.level;
            iceOrbs[i].GetComponent<IceOrbController>().orbitDegreesPerSec = 30 * base.level;

        }

    }
}
