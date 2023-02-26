using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler<OnSkillUnlockedArgs> OnSkillUnlocked;
    public class OnSkillUnlockedArgs : EventArgs
    {
        public SkillType skillType;
    }

    public enum SkillType
    {
        None,
        Dash,
        Strenght,
        Defense,
        Agility,
    }

    List<SkillType> unlockedSkillTypesList;
     
    public PlayerSkills()
    {
        unlockedSkillTypesList = new List<SkillType>();
    }

    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            unlockedSkillTypesList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedArgs { skillType = skillType });
        }
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypesList.Contains(skillType);
    }

    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch(skillType)
        {
            case SkillType.Strenght: return SkillType.Dash;
            case SkillType.Agility: return SkillType.Dash;
            case SkillType.Defense: return SkillType.Dash;
        }
        return SkillType.None;
    }

    public bool TryUnlockSkill(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                UnlockSkill(skillType);
                return true;
            }
            else
                return false;
        }
        else
        {
            UnlockSkill(skillType);
            return true;
        }
    }

    
}
