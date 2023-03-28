using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



// This script handles how the xp bar and current level will be shown on screen
public class LevelWindow : MonoBehaviour
{
    [SerializeField] Image xpBarImage;
    [SerializeField] TextMeshProUGUI levelText;
    private LevelSystem levelSystem;
    
    private void SetExperienceBarSize(float experienceNormalized)
    {
        xpBarImage.fillAmount = experienceNormalized;
    }

    private void SetLevelNumber(int levelNumber)
    {
        levelText.text = "Level " + (levelNumber);
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;

        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
        SetLevelNumber(levelSystem.GetLevelNumber());

        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        SetLevelNumber(levelSystem.GetLevelNumber());
    }

    private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    }
}
