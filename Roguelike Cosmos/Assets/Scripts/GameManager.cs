using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] mobileButtons;
    //DeviceType system;
    bool isMobileDevice;

    public PlayerCombat playerScript;
    private PlayerSkills playerSkills;
    public GameObject skillTreeUI;
    private bool skillTreeActive;
    
    public Button[] skillButtonList; // Lista teste
    public List<Button> skillButtonList2; // Lista Completa
    [SerializeField] private List<PlayerSkills.SkillType> skillTypeList;

    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject enemyPrefab;

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedArgs e)
    {
        UpdateVisuals();
        //Debug.Log(e.skillType);
    }

    private void Start()
    {
        if(playerScript.system == DeviceType.Desktop)
        {
            ChangeStateMobileButtons(false);
        }
        isMobileDevice = false;
        playerSkills = playerScript.GetPlayerSkillScript();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        skillTypeList = Enum.GetValues(typeof(PlayerSkills.SkillType)).Cast<PlayerSkills.SkillType>().ToList();
        skillTypeList.Remove(PlayerSkills.SkillType.None);
        skillButtonList2 = skillTreeUI.GetComponentsInChildren<Button>().ToList();
        UpdateVisuals();
        skillTreeActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMobileDevice = !isMobileDevice;
            ChangeStateMobileButtons(isMobileDevice);
            if (isMobileDevice)
                playerScript.system = DeviceType.Handheld;
            else
                playerScript.system = DeviceType.Desktop;
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            OpenSkillTree();
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPos.position, spawnPos.rotation);
    }

    public void ChangeStateMobileButtons()
    {
        isMobileDevice = !isMobileDevice;
        ChangeStateMobileButtons(isMobileDevice);
    }

    public void OpenSkillTree()
    {
        skillTreeActive = !skillTreeActive;
        skillTreeUI.SetActive(skillTreeActive);
    }

    void ChangeStateMobileButtons(bool state)
    {
        for (int i = 0; i < mobileButtons.Length; i++)
        {
            mobileButtons[i].SetActive(state);
        }
    }

    public void AttackButton()
    {
        playerScript.MobilePunch();
    }

    public void FirstSkillButton()
    {
        playerScript.RangedSkill();
    }

    public void DashButton()
    {
        RbPlayerMovement playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<RbPlayerMovement>();
        playerMov.StartCoroutine("Dash");
    }

    private void CheckUnlock(PlayerSkills.SkillType skillType)
    {
        if (!playerSkills.TryUnlockSkill(skillType))
        {
            Debug.Log("Erro ao debloquear " + skillType + "!");
        }
        else
        {
            Debug.Log("Desbloqueou " + skillType + "!");
            //Debug.Log(skillType + " checkingUnloc");

        }
        //Button button = skillButtonList2[skillTypeList.IndexOf(skillType)]; Utiliza igual
        UpdateVisual(skillType, skillButtonList2[skillTypeList.IndexOf(skillType)]);

    }

    private void UpdateVisuals()
    {
        foreach(PlayerSkills.SkillType skillList in skillTypeList)
        {
            UpdateVisual(skillList, skillButtonList2[skillTypeList.IndexOf(skillList)]);
        }
    }


    public void UpdateVisual(PlayerSkills.SkillType skillType, Button button)
    {
        if (playerSkills.IsSkillUnlocked(skillType))
        {
            //image.material = null;
            //backgroundImage.material = null;
            //Debug.Log("Desbloq " + skillType);
            button.interactable = false;
        }
        else
        {
            if (playerSkills.CanUnlock(skillType))
            {
                //Debug.Log("Desbloquavel " + skillType);
                //image.material = skillUnlockableMaterial;
                //backgroundImage.color = UtilsClass.GetColorFromString("4B677D");
                //transform.GetComponent<Button_UI>().enabled = true;
                ColorBlock cb = button.colors;
                cb.normalColor = Color.blue;
                button.colors = cb;
            }
            else
            {
                //Debug.Log("Bloq " + skillType);
                //image.material = skillLockedMaterial;
                //backgroundImage.color = new Color(.3f, .3f, .3f);
                //transform.GetComponent<Button_UI>().enabled = false;
                ColorBlock cb = button.colors;
                cb.normalColor = Color.red;
                button.colors = cb;
            }
        }
    }

    public void UnlockDash()
    {
        CheckUnlock(PlayerSkills.SkillType.Dash);
    }
    public void UnlockStrenght()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght);
    }
    public void UnlockStrenght1Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght1Left);
    }
    public void UnlockStrenght2Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght2Left);
    }
    public void UnlockStrenght3()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght3);
    }
    public void UnlockStrenght4Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght4Left);
    }
    public void UnlockStrenght5Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght5Left);
    }
    public void UnlockStrenght6()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght6);
    }
    public void UnlockStrenght1Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght1Right);
    }
    public void UnlockStrenght2Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght2Right);
    }
    public void UnlockStrenght4Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght4Right);
    }
    public void UnlockStrenght5Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Strenght5Right);
    }
    public void UnlockDefense()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense);
    }
    public void UnlockDefense1Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense1Left);
    }
    public void UnlockDefense2Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense2Left);
    }
    public void UnlockDefense3()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense3);
    }
    public void UnlockDefense4Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense4Left);
    }
    public void UnlockDefense5Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense5Left);
    }
    public void UnlockDefense6()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense6);
    }
    public void UnlockDefense1Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense1Right);
    }
    public void UnlockDefense2Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense2Right);
    }
    public void UnlockDefense4Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense4Right);
    }
    public void UnlockDefense5Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Defense5Right);
    }
    public void UnlockAgility()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility);
    }
    public void UnlockAgility1Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility1Right);
    }
    public void UnlockAgility2Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility2Right);
    }
    public void UnlockAgility3()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility3);
    }
    public void UnlockAgility4Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility4Right);
    }
    public void UnlockAgility5Right()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility5Right);
    }
    public void UnlockAgility6()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility6);
    }
    public void UnlockAgility1Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility1Left);
    }
    public void UnlockAgility2Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility2Left);
    }
    public void UnlockAgility4Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility4Left);
    }
    public void UnlockAgility5Left()
    {
        CheckUnlock(PlayerSkills.SkillType.Agility5Left);
    }
    public void UnlockStrAgi2()
    {
        CheckUnlock(PlayerSkills.SkillType.StrAgi2);
    }
    public void UnlockStrAgi4()
    {
        CheckUnlock(PlayerSkills.SkillType.StrAgi4);
    }
    public void UnlockStrAgi6()
    {
        CheckUnlock(PlayerSkills.SkillType.StrAgi6);
    }
    public void UnlockAgiDef2()
    {
        CheckUnlock(PlayerSkills.SkillType.AgiDef2);
    }
    public void UnlockAgiDef4()
    {
        CheckUnlock(PlayerSkills.SkillType.AgiDef4);
    }
    public void UnlockAgiDef6()
    {
        CheckUnlock(PlayerSkills.SkillType.AgiDef6);
    }
    public void UnlockDefStr2()
    {
        CheckUnlock(PlayerSkills.SkillType.DefStr2);
    }
    public void UnlockDefStr4()
    {
        CheckUnlock(PlayerSkills.SkillType.DefStr4);
    }
    public void UnlockDefStr6()
    {
        CheckUnlock(PlayerSkills.SkillType.DefStr6);
    }


    // � mais custoso toda vez ler toda a lista do que ter 500 fun��es
    //*
    public void UnlockAbility()
    {
        string skillName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(skillName);
        foreach (PlayerSkills.SkillType type in skillTypeList)
        {
            if (type.ToString() == skillName)
            {
                CheckUnlock(type);
            }
        }
        UpdateVisuals();
    }
    //*/
}
