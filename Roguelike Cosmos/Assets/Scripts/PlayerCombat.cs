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

    [Header("Colliders")]
    [SerializeField] private BoxCollider leftHandCollider;
    [SerializeField] private BoxCollider rightHandCollider;

    [Header("")]
    public bool isAttacking;

    public void UpdateColliders(bool enable) {
        leftHandCollider.enabled = enable;
        rightHandCollider.enabled = enable;
    }

    private void Awake() {
        UpdateColliders(false);
        playerSkills = new PlayerSkills();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
    }

    public PlayerSkills GetPlayerSkillScript()
    {
        return playerSkills;
    }
    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedArgs e)
    {
        switch (e.skillType)
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


    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            UpdateColliders(true);
            //isAttacking = true;
            playerAnimator.SetTrigger("isPunching");
        }
    }


    private void Start()
    {
        system = SystemInfo.deviceType;
    }

    public void StartAttack()
    {
        isAttacking = true;
        UpdateColliders(true);
    }

    public void FinishAttack()
    {
        isAttacking = false;
        UpdateColliders(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateColliders(false);
            Debug.Log("Dealing " + _playerData.attackDamage + " damage to an enemy");
        }
    }

    private bool CanUseSkill()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Dash);
    }
} 

