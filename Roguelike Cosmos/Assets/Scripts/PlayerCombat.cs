using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    DeviceType system;
    private PlayerSkills playerSkills;


    [Header("Animation")]
    public Animator playerAnimator;

    private void Awake()
    {
        playerSkills = new PlayerSkills();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
    }

    public PlayerSkills GetPlayerSkillScript()
    {
        return playerSkills;
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedArgs e)
    {
        switch(e.skillType)
        {
            case PlayerSkills.SkillType.Agility:
                Debug.Log("+Agility");
                break;
            case PlayerSkills.SkillType.Defense:
                Debug.Log("+Def");
                break;
            case PlayerSkills.SkillType.Strenght:
                Debug.Log("+Str");
                break;
        }
    }

    private void Start()
    {
        system = SystemInfo.deviceType;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0) && system == DeviceType.Desktop)
        {
            //playerAnimator.SetBool("isPunching", true);
            Debug.Log(_playerData.attackDamage);
        }
    }

    private bool CanUseSkill()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Dash);
    }
}
