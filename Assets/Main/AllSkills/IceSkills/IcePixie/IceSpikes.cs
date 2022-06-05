using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikes : Skill
{

    private GameObject pet;
    public float attackSpeed;
    public override void Attack()
    {
        base.Attack();
        pet = Instantiate(base.prefab,
       new Vector3(base.myCharacterController.transform.position.x, base.myCharacterController.transform.position.y + 1,
       base.myCharacterController.transform.position.z), base.myCharacterController.transform.rotation * Quaternion.Euler(0, 0, 0));
        pet.GetComponent<PetFollowController>().damage = base.damage;
        pet.GetComponent<PetFollowController>().attackSpeed = 2f;
        pet.GetComponent<PetFollowController>().baseAttackSpeed = 2f;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        switch (base.level)
        {
            case 2:
                pet.GetComponent<PetFollowController>().attackSpeed = pet.GetComponent<PetFollowController>().baseAttackSpeed * 0.9f;
                pet.GetComponent<PetFollowController>().damage += 5;
                break;
            case 3:
                pet.GetComponent<PetFollowController>().attackSpeed = pet.GetComponent<PetFollowController>().baseAttackSpeed * 0.7f;
                pet.GetComponent<PetFollowController>().damage += 5;
                break;
            case 4:
                pet.GetComponent<PetFollowController>().damage += 5;
                break;
            case 5:
                pet.GetComponent<PetFollowController>().attackSpeed = pet.GetComponent<PetFollowController>().baseAttackSpeed * 0.5f;
                pet.GetComponent<PetFollowController>().damage += 5;
                break;
            case 6:
                pet.GetComponent<PetFollowController>().damage += 5;
                break;
            case 7:
                pet.GetComponent<PetFollowController>().attackSpeed = pet.GetComponent<PetFollowController>().baseAttackSpeed * 0.2f;
                pet.GetComponent<PetFollowController>().damage += 5;
                break;
        }
    }
}
