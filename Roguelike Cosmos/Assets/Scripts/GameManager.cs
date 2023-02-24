using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCombat))]
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] mobileButtons;
    DeviceType system;
    bool isMobileDevice;

    public PlayerCombat playerScript;
    private PlayerSkills playerSkills;

    private void Start()
    {
        system = SystemInfo.deviceType;
        if(system == DeviceType.Desktop)
        {
            ChangeStateMobileButtons(false);
        }
        isMobileDevice = false;
        playerSkills = playerScript.GetPlayerSkillScript();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMobileDevice = !isMobileDevice;
            ChangeStateMobileButtons(isMobileDevice);
            if (isMobileDevice)
                system = DeviceType.Handheld;
            else
                system = DeviceType.Desktop;
        }
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
        Debug.Log("Attack");
    }

    public void DashButton()
    {
        RbPlayerMovement playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<RbPlayerMovement>();
        playerMov.StartCoroutine("Dash");
    }

    public void UnlockDash()
    {
        if(!playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Dash))
        {
            Debug.Log("Erro ao debloquear Dash");
        }
        else
            Debug.Log("Desbloqueou Dash");

    }

    public void UnlockAgility()
    {
        if (!playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Agility))
        {
            Debug.Log("Erro ao debloquear Agilidade");
        }
        else
            Debug.Log("Desbloqueou Agilidade");

    }
}
