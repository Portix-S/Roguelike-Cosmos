using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player;

public class PlayerSkills
{
    public event EventHandler<OnSkillUnlockedArgs> OnSkillUnlocked;

    private PlayerData _playerData;

    public class OnSkillUnlockedArgs : EventArgs
    {
        public SkillType skillType;
    }

    public enum SkillType
    {
        None, Dash, Strenght, Strenght1Left, Strenght1Right, Strenght2Left, Strenght2Right, Strenght3, Strenght4Left, Strenght4Right, Strenght5Left, Strenght5Right,
        Strenght6, Defense, Defense1Left, Defense1Right, Defense2Left, Defense2Right, Defense3, Defense4Left, 
        Defense4Right, Defense5Left, Defense5Right, Defense6, Agility, Agility1Left, Agility1Right, Agility2Left,
        Agility2Right, Agility3, Agility4Left, Agility4Right, Agility5Left, Agility5Right, Agility6, StrAgi2, AgiDef2, DefStr2, StrAgi4, AgiDef4, DefStr4, StrAgi6, AgiDef6, DefStr6,
    }

    TextMeshProUGUI pointsText;
    List<SkillType> unlockedSkillTypesList;
    LevelSystem levelSystem;
    public int requiredPoints;
    public PlayerSkills(LevelSystem levelSystem, TextMeshProUGUI pointsText, PlayerData playerData)
    {
        this.levelSystem = levelSystem;
        this.pointsText = pointsText;
        this._playerData = playerData;
        unlockedSkillTypesList = new List<SkillType>();
        requiredPoints = 1;
    }

    private void UnlockSkill(SkillType skillType, PlayerModifiers[] modifiers)
    {
        if (!IsSkillUnlocked(skillType))
        {
            increaseStats(skillType, modifiers);
            unlockedSkillTypesList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedArgs { skillType = skillType });
        }
    }

    private void increaseStats(SkillType skillType, PlayerModifiers[] modifiers)
    {
        /*
        if(skillType == SkillType.Dash) return;
        if(skillType == SkillType.Strenght6) return;
        if(skillType == SkillType.Defense6) return;
        if(skillType == SkillType.Agility6) return;
        if(skillType == SkillType.StrAgi6) return;
        if(skillType == SkillType.AgiDef6) return;
        if(skillType == SkillType.DefStr6) return;
        //*/
        foreach(PlayerModifiers playerModifiers in modifiers)
        {
            _playerData.modifier[(int)playerModifiers.stat].value += playerModifiers.value;
        }
        /*
        switch (skillType)
        {
            // First/Second Layer
            case SkillType.Strenght:
                _playerData.modifier[0].value += 10; //STR
                break;
            case SkillType.Strenght1Left:
                _playerData.modifier[0].value += 5; //STR
                _playerData.modifier[1].value += 5; //CON
                break;
            case SkillType.Strenght2Left:
                _playerData.modifier[0].value += 5; //STR
                _playerData.modifier[1].value += 5; //CON
                break;
            case SkillType.Strenght1Right:
                _playerData.modifier[3].value += 5; //INT
                _playerData.modifier[4].value += 5; //WIS
                break;
            case SkillType.Strenght2Right:
                _playerData.modifier[3].value += 5; //INT
                _playerData.modifier[2].value += 5; //AGI
                break; 


            case SkillType.Defense:
                _playerData.modifier[1].value += 10; //CON
                break;
            case SkillType.Defense1Left:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[4].value += 5; //WIS
                break;
            case SkillType.Defense2Left:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[2].value += 5; //AGI
                break;
            case SkillType.Defense1Right:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[0].value += 5; //STR
                break;
            case SkillType.Defense2Right:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[0].value += 5; //STR
                break;
            

            case SkillType.Agility:
                _playerData.modifier[2].value += 10; //AGI
                break;
            case SkillType.Agility1Left:
                _playerData.modifier[0].value += 5; //STR
                _playerData.modifier[2].value += 5; //AGI
                break;
            case SkillType.Agility2Left:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[2].value += 5; //AGI
                break;
            case SkillType.Agility1Right:
                _playerData.modifier[2].value += 5; //AGI
                _playerData.modifier[3].value += 5; //INT
                break;
            case SkillType.Agility2Right:
                _playerData.modifier[2].value += 5; //AGI
                _playerData.modifier[2].value += 5; //WIS
                break;

            // First Skill/Buff
            case SkillType.Strenght3:
                _playerData.modifier[0].value += 10; //STR
                _playerData.modifier[3].value += 10; //INT
                break;
            case SkillType.Defense3:
                _playerData.modifier[1].value += 10; //CON
                _playerData.modifier[4].value += 10; //WIS
                break;
            case SkillType.Agility3:
                _playerData.modifier[2].value += 10;
                break;
            case SkillType.StrAgi2:
                _playerData.modifier[0].value +=10; //STR
                _playerData.modifier[3].value +=10; //INT
                _playerData.modifier[2].value +=10; //AGI
                break;
            case SkillType.AgiDef2:
                _playerData.modifier[2].value +=10; //AGI
                _playerData.modifier[1].value +=10; //CON
                _playerData.modifier[4].value +=10; //WIS
                break;
            case SkillType.DefStr2:
                _playerData.modifier[1].value +=10; //CON
                _playerData.modifier[4].value +=10; //WIS
                _playerData.modifier[0].value +=10; //STR
                break;

            
            // Fourth/Fifth Layer and Second Buff
            case SkillType.Strenght4Left:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[0].value += 5; //STR
                break;
            case SkillType.Strenght5Left:
                _playerData.modifier[0].value += 15; //STR
                break;
            case SkillType.Strenght4Right:
                _playerData.modifier[2].value += 5; //AGI
                _playerData.modifier[3].value += 5; //INT
                break;
            case SkillType.Strenght5Right:
                _playerData.modifier[3].value += 15; //INT
                break;


            case SkillType.Defense4Left:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[2].value += 5; //AGI
                break;
            case SkillType.Defense5Left:
                _playerData.modifier[1].value += 10; //CON
                _playerData.modifier[4].value += 10; //WIS
                break;
            case SkillType.Defense4Right:
                _playerData.modifier[1].value += 5; //CON
                _playerData.modifier[0].value += 5; //STR
                break;
            case SkillType.Defense5Right:
                _playerData.modifier[1].value += 10; //CON
                _playerData.modifier[4].value += 10; //WIS
                break;


            case SkillType.Agility4Left:
                _playerData.modifier[1].value += 10; //CON
                _playerData.modifier[4].value += 10; //AGI
                break;
            case SkillType.Agility5Left:
                _playerData.modifier[1].value += 15; //AGI
                break;
            case SkillType.Agility4Right:
                _playerData.modifier[2].value += 10; //AGI
                _playerData.modifier[3].value += 10; //INT
                break;
            case SkillType.Agility5Right:
                _playerData.modifier[2].value += 15; // AGI
                break;
              
        
            case SkillType.StrAgi4:
                _playerData.modifier[0].value +=10; //STR
                _playerData.modifier[3].value +=10; //INT
                _playerData.modifier[2].value +=10; //AGI
                break;
            case SkillType.AgiDef4:
                _playerData.modifier[1].value +=20; //CON
                _playerData.modifier[4].value +=20; //WIS
                _playerData.modifier[2].value +=10; //AGI
                break;
            case SkillType.DefStr4:
                _playerData.modifier[1].value +=20; //CON
                _playerData.modifier[4].value +=20; //WIS
                _playerData.modifier[0].value +=10; //STR
                break;
        }
        //*/
    }

    public bool CanUnlock(SkillType skillType)
    {
        SkillType skillRequirement1;
        SkillType skillRequirement2;
        GetSkillRequirement(skillType, out skillRequirement1, out skillRequirement2, out requiredPoints);
        if (skillRequirement1 != SkillType.None && skillRequirement2 == SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement1))
                return true;
            else 
                return false;
        }
        else if (skillRequirement1 != SkillType.None && skillRequirement2 != SkillType.None)
        {
            if (CheckRequirement(skillRequirement1, skillRequirement2))
            {
                if (IsSkillUnlocked(skillRequirement1) || IsSkillUnlocked(skillRequirement2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (IsSkillUnlocked(skillRequirement1) && IsSkillUnlocked(skillRequirement2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return true;
        }

    }

    private bool HasRequiredPoint(int point)
    {
        return levelSystem.GetSkillTreePoints() >= point;
    }

     private bool CheckRequirement(PlayerSkills.SkillType skill1, PlayerSkills.SkillType skill2)
    {
        return ((skill1 == PlayerSkills.SkillType.Strenght2Left && skill2 == PlayerSkills.SkillType.Strenght2Right) ||
            (skill1 == PlayerSkills.SkillType.Strenght5Left && skill2 == PlayerSkills.SkillType.Strenght5Right) ||
            (skill1 == PlayerSkills.SkillType.Defense2Left && skill2 == PlayerSkills.SkillType.Defense2Right) ||
            (skill1 == PlayerSkills.SkillType.Defense5Left && skill2 == PlayerSkills.SkillType.Defense5Right) ||
            (skill1 == PlayerSkills.SkillType.Agility2Left && skill2 == PlayerSkills.SkillType.Agility2Right) ||
            (skill1 == PlayerSkills.SkillType.Agility5Left && skill2 == PlayerSkills.SkillType.Agility5Right));
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypesList.Contains(skillType);
    }

    public void GetSkillRequirement(SkillType skillType, out PlayerSkills.SkillType firstRequirement, out PlayerSkills.SkillType secondRequirement, out int requiredPoints)
    {
        firstRequirement = SkillType.None;
        secondRequirement = SkillType.None;
        requiredPoints = 1;
        switch (skillType)
        {
            // First/Second Layer
            case SkillType.Strenght: firstRequirement = SkillType.Dash; break;
            case SkillType.Strenght1Left: firstRequirement = SkillType.Strenght; break;
            case SkillType.Strenght2Left: firstRequirement = SkillType.Strenght1Left;break;
            case SkillType.Strenght1Right: firstRequirement = SkillType.Strenght; break;
            case SkillType.Strenght2Right: firstRequirement = SkillType.Strenght1Right; break;
            case SkillType.Defense: firstRequirement = SkillType.Dash; break;
            case SkillType.Defense1Left: firstRequirement = SkillType.Defense; break;
            case SkillType.Defense2Left: firstRequirement = SkillType.Defense1Left; break;
            case SkillType.Defense1Right: firstRequirement = SkillType.Defense; break;
            case SkillType.Defense2Right: firstRequirement = SkillType.Defense1Right; break;
            case SkillType.Agility: firstRequirement = SkillType.Dash; break;
            case SkillType.Agility1Left: firstRequirement = SkillType.Agility; break;
            case SkillType.Agility2Left: firstRequirement = SkillType.Agility1Left; break;
            case SkillType.Agility1Right: firstRequirement = SkillType.Agility; break;
            case SkillType.Agility2Right: firstRequirement = SkillType.Agility1Right; break;

            // First Skill/Buff
            case SkillType.Strenght3: firstRequirement = SkillType.Strenght2Left; secondRequirement = SkillType.Strenght2Right; requiredPoints = 2; break;
            case SkillType.Defense3: firstRequirement = SkillType.Defense2Left; secondRequirement = SkillType.Defense2Right; requiredPoints = 2; break;
            case SkillType.Agility3: firstRequirement = SkillType.Agility2Left; secondRequirement = SkillType.Agility2Right; requiredPoints = 2; break;
            case SkillType.StrAgi2: firstRequirement = SkillType.Agility2Right; secondRequirement = SkillType.Strenght2Right; requiredPoints = 2; break;
            case SkillType.AgiDef2: firstRequirement = SkillType.Agility2Left; secondRequirement = SkillType.Defense2Left; requiredPoints = 2; break;
            case SkillType.DefStr2: firstRequirement = SkillType.Strenght2Left; secondRequirement = SkillType.Defense2Right; requiredPoints = 2; break;

            // Fourth/Fifth Layer and Second Buff
            case SkillType.Strenght4Left: firstRequirement = SkillType.Strenght3; requiredPoints = 3; break;
            case SkillType.Strenght5Left: firstRequirement = SkillType.Strenght4Left; requiredPoints = 3; break;
            case SkillType.Defense4Left: firstRequirement = SkillType.Defense3; requiredPoints = 3; break;
            case SkillType.Defense5Left: firstRequirement = SkillType.Defense4Left; requiredPoints = 3; break;
            case SkillType.Agility4Left: firstRequirement = SkillType.Agility3; requiredPoints = 3; break;
            case SkillType.Agility5Left: firstRequirement = SkillType.Agility4Left; requiredPoints = 3; break;
            case SkillType.Strenght4Right: firstRequirement = SkillType.Strenght3; requiredPoints = 3; break;
            case SkillType.Strenght5Right: firstRequirement = SkillType.Strenght4Right; requiredPoints = 3; break;
            case SkillType.Defense4Right: firstRequirement = SkillType.Defense3; requiredPoints = 3; break;
            case SkillType.Defense5Right: firstRequirement = SkillType.Defense4Right; requiredPoints = 3; break;
            case SkillType.Agility4Right: firstRequirement = SkillType.Agility3; requiredPoints = 3; break;
            case SkillType.Agility5Right: firstRequirement = SkillType.Agility4Right; requiredPoints = 3; break;
            case SkillType.StrAgi4: firstRequirement = SkillType.Agility4Right; secondRequirement = SkillType.Strenght4Right; requiredPoints = 3; break;
            case SkillType.AgiDef4: firstRequirement = SkillType.Agility4Left; secondRequirement = SkillType.Defense4Left; requiredPoints = 3; break;
            case SkillType.DefStr4: firstRequirement = SkillType.Strenght4Left; secondRequirement = SkillType.Defense4Right; requiredPoints = 3; break;

            //  Second Skill
            case SkillType.Strenght6: firstRequirement = SkillType.Strenght5Left; secondRequirement = SkillType.Strenght5Right; requiredPoints = 4; break;
            case SkillType.Defense6: firstRequirement = SkillType.Defense5Left; secondRequirement = SkillType.Defense5Right; requiredPoints = 4; break;
            case SkillType.Agility6: firstRequirement = SkillType.Agility5Left; secondRequirement = SkillType.Agility5Right; requiredPoints = 4; break;

            // Ultimate
            case SkillType.StrAgi6: firstRequirement = SkillType.Agility6; secondRequirement = SkillType.Strenght6; requiredPoints = 5; break;
            case SkillType.AgiDef6: firstRequirement = SkillType.Agility6; secondRequirement = SkillType.Defense6; requiredPoints = 5; break;
            case SkillType.DefStr6: firstRequirement = SkillType.Strenght6; secondRequirement = SkillType.Defense6; requiredPoints = 5; break;

        }
        
    }

    public bool TryUnlockSkill(SkillType skillType, PlayerModifiers[] modifiers)
    {
        
        if (CanUnlock(skillType) && HasRequiredPoint(requiredPoints))
        {
            levelSystem.RemoveSkillTreePoints(requiredPoints);
            pointsText.text = "SkillPoints:\n" + levelSystem.GetSkillTreePoints();
            UnlockSkill(skillType, modifiers);
            return true;
        }
        else
            return false;
    }



}
