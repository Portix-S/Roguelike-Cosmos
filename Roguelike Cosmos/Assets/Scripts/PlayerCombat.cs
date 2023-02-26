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


    // Basic Attack Logic //
    public void UpdateColliders(bool enable){
        leftHandCollider.enabled = enable;
        rightHandCollider.enabled = enable;
    }

    private void Awake() {
        UpdateColliders(false);
        playerSkills = new PlayerSkills();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
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

    public void FinishAttack(){
        UpdateColliders(false);
    }

    // Temporary damage dealer
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemie"){
            UpdateColliders(false);
            Debug.Log("Dealing damage to " + other.gameObject.name);
        }
    }
     // Ranged Skill Logic //
    public void Shoot(){
        var proj = Instantiate(pfProjectile, projectileSpawn.position, projectileSpawn.rotation);

        proj.GetComponent<Rigidbody>().velocity = projectileSpawn.forward * projectileSpeed;

        canShoot = false;
        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown(){
        yield return new WaitForSeconds(projectileCooldown);
        canShoot = true;
    }

    private bool CanUseSkill()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Dash);
    }

    public PlayerSkills GetPlayerSkillScript()
    {
        return playerSkills;
    }
    
    // Unlock Skill Logic //
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
    

    private void Start()
    {
        system = SystemInfo.deviceType;
    }

    private bool CanUseSkill()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Dash);
    }
}
