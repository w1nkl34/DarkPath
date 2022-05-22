using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image expBar;
    public Text levelText;

    public void UpdateExpBar(int level,float currentExp)
    {
        levelText.text = level.ToString();
        expBar.fillAmount = currentExp / (level * 100f);
    }
}
