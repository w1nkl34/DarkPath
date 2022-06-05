using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public SkillType skillType;
    public bool active;
    public GameObject prefab;
    public int level;
    public int maxLevel;
    public float damage;
    public bool mainSkill;
    public string skillName;
    public MyCharacterController myCharacterController;
    public List<string> levelMessages = new List<string>();
    public Sprite skillIcon;
    public virtual void Attack(){
        active = true;
    }
    public virtual void LevelUp() {
        level++;
    }

}
