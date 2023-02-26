using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSystem
{

    public event EventHandler OnLevelChanged;
    public event EventHandler OnExperienceChanged;

    private int level;
    private int xp;
    private int maxLevel;
    private int skillTreePoints;
    private int statsPoints;
    public LevelSystem()
    {
        level = 0;
        xp = 0;
        maxLevel = 100;
        skillTreePoints = 0;
        statsPoints = 5;
    }

    public void AddExperience(int amount)
    {
        if (!IsMaxLevel())
        {
            xp += amount;
            while (!IsMaxLevel() && xp >= GetExperienceToNextLevel(level))
            {
                xp -= GetExperienceToNextLevel(level);
                level++;
                skillTreePoints++;
                statsPoints += 3;
                //if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
                OnLevelChanged?.Invoke(this, EventArgs.Empty);
            }
            OnExperienceChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetSkillTreePoints()
    {
        return skillTreePoints;
    }

    public int GetStatPoints()
    {
        return statsPoints;
    }

    public void RemoveSkillTreePoints(int amount)
    {
        if(skillTreePoints - amount >= 0)
            skillTreePoints -= amount;
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public float GetExperienceNormalized()
    {
        if (IsMaxLevel())
            return 1f;
        else
            return (float)xp / GetExperienceToNextLevel(level);
    }

    public int GetExperienceToNextLevel(int level)
    {
        if (!IsMaxLevel())
        {
            int xpToNextLevel = 100 + (level * 10);
            return xpToNextLevel;
        }
        else
        {
            Debug.LogError("Invalid Level: " + level);
            return 1;
        }
    }

    public bool IsMaxLevel()
    {
        return IsMaxLevel(level);
    }

    public bool IsMaxLevel(int level)
    {
        return level == maxLevel;
    }

}
