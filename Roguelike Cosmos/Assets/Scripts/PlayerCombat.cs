using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    DeviceType system;
    private PlayerSkills playerSkills;

    [Header("Level System")]
    [SerializeField] LevelWindow levelWindow;
    private LevelSystem levelSystem;
    [SerializeField] int currentPoints;
    [SerializeField] int currentStatsPoints;
    [SerializeField] TextMeshProUGUI pointsText;

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
        levelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(levelSystem);
        pointsText.text = "SkillPoints:\n" + levelSystem.GetSkillTreePoints();
        playerSkills = new PlayerSkills(levelSystem, pointsText);
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            UpdateColliders(true);
            //isAttacking = true;
            playerAnimator.SetTrigger("isPunching");
        }
        currentPoints = levelSystem.GetSkillTreePoints();
        currentStatsPoints = levelSystem.GetStatPoints();
        if(Input.GetKey(KeyCode.X))
        {
            levelSystem.AddExperience(100);
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            levelSystem.AddExperience(10);
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

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        pointsText.text = "SkillPoints:\n" + levelSystem.GetSkillTreePoints();
    }
} 

