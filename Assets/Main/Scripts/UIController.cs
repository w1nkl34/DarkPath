using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image expBar;
    public Text levelText;
    public GameObject levelUpBar;
    public GameObject levelUpTemp;
    public GameManager gm;
    public MyCharacterController myCharacterController;

    public void UpdateExpBar(int level,float currentExp)
    {
        levelText.text = level.ToString();
        expBar.fillAmount = currentExp / (level * 100f);
    }

    public void OpenLevelUpBar()
    {
        levelUpBar.SetActive(true);
    }
    public void CloseLevelUpBar()
    {
        levelUpBar.SetActive(false);
    }

    List<Skill> updatedSkills = new List<Skill>();
    public void GenerateLevelUp(List<Skill> skills,MyCharacterController myCharacterController)
    {
        ResetLevelUps();
        updatedSkills = new List<Skill>();
        this.myCharacterController = myCharacterController;
        foreach (Skill skill in skills)
        {
            bool found = false;
            foreach(Skill chSkills in myCharacterController.allSkills)
            {
                if(chSkills.skillName == skill.skillName)
                {
                    found = true;
                    updatedSkills.Add(chSkills);
                    break;
                }
            }
            if(found == false)
            {
                updatedSkills.Add(skill);
            }
        }

        for(int i = 0; i<updatedSkills.Count; i++)
        {
            string skillName = updatedSkills[i].skillName;
            GameObject levelUpBar = Instantiate(levelUpTemp, levelUpTemp.transform.parent);
            if(updatedSkills[i].active == false)
            {
                levelUpBar.GetComponent<LevelUpSkillController>().skillInfoText.text = updatedSkills[i].skillName;
                levelUpBar.GetComponent<LevelUpSkillController>().skillLevelText.text = "NEW";
                levelUpBar.GetComponent<LevelUpSkillController>().skillIcon.sprite = updatedSkills[i].skillIcon;
            }
            else
            {
                levelUpBar.GetComponent<LevelUpSkillController>().skillInfoText.text = updatedSkills[i].skillName + " "  + updatedSkills[i].levelMessages[updatedSkills[i].level-1];
                levelUpBar.GetComponent<LevelUpSkillController>().skillLevelText.text = updatedSkills[i].level.ToString() + " > " + (updatedSkills[i].level+1).ToString();
                levelUpBar.GetComponent<LevelUpSkillController>().skillIcon.sprite = updatedSkills[i].skillIcon;
            }
            levelUpBar.SetActive(true);
            levelUpBar.GetComponent<Button>().onClick.AddListener( () => { 
                LevelUpSkill(skillName)
                ; });
        }
    }

    public void LevelUpSkill(string skillName)
    {
        foreach(Skill skill in updatedSkills)
        {
            if(skill.skillName == skillName)
            {
                if(skill.active == true)
                {
                    skill.LevelUp();
                }
                else
                {
                    foreach(GameObject sk in gm.allSkills)
                    {
                        if(sk.GetComponent<Skill>().skillName == skillName)
                        {
                            GameObject skx = Instantiate(sk);
                            myCharacterController.allSkills.Add(skx.GetComponent<Skill>());
                            skx.GetComponent<Skill>().myCharacterController = this.myCharacterController;
                            myCharacterController.UpdateSkills();
                            break;
                        }
                    }
                 
                }
                break;
            }
        }
        Time.timeScale = 1;
        ResetLevelUps();
        CloseLevelUpBar();
    }

    public void ResetLevelUps()
    {
       int index = 0;
       foreach(Transform child in levelUpTemp.transform.parent)
        {
            if (index != 0)
                Destroy(child.gameObject);
            index++;
        }
    }
}
