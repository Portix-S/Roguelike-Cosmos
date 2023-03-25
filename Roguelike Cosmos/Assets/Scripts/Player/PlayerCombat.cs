using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public DeviceType system;
    private PlayerSkills playerSkills;
    Rigidbody playerRb;

    [Header("Level System")]
    [SerializeField] LevelWindow levelWindow;
    private LevelSystem levelSystem;
    [SerializeField] int currentPoints;
    [SerializeField] int currentStatsPoints;
    [SerializeField] TextMeshProUGUI pointsText;

    [Header("Animation")]
    public Animator playerAnimator;

    [Header("Basic Combat")]
    [SerializeField] private BoxCollider leftHandCollider;
    [SerializeField] private BoxCollider rightHandCollider;
    public bool isAttacking;
    [SerializeField] float attackMoveForce = 1f;

    [Header("Projectile")]
    public GameObject pfProjectile;
    public Transform  projectileSpawn;
    [SerializeField] private bool canShoot = true;
    public float projectileSpeed;
    public float projectileCooldown;
    public bool isShooting;

    // Basic Attack Logic //
    public void UpdateColliders(bool enable){
        leftHandCollider.enabled = enable;
        rightHandCollider.enabled = enable;
    }

    private void Awake() {
        system = SystemInfo.deviceType;
        UpdateColliders(false);
        levelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(levelSystem);
        pointsText.text = "SkillPoints:\n" + levelSystem.GetSkillTreePoints();
        playerSkills = new PlayerSkills(levelSystem, pointsText);
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
        playerRb = GetComponent<Rigidbody>();
    }

    public LevelSystem GetLevelSystem()
    {
        return this.levelSystem;
    }

    private void Start()
    {
        system = SystemInfo.deviceType;
    }

    void Update()
    {
        if (system == DeviceType.Desktop)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerAnimator.SetTrigger("isPunching");
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && canShoot && !isAttacking)
            {
                canShoot = false;
                playerAnimator.SetTrigger("isShooting");
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && !canShoot)
            {
                Debug.Log("On cooldown");
            }

            
            if (Input.GetKey(KeyCode.X))
            {
                levelSystem.AddExperience(100);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                levelSystem.AddExperience(10);
            }
        }
        currentPoints = levelSystem.GetSkillTreePoints();
        currentStatsPoints = levelSystem.GetStatPoints();
    }

    public void MobilePunch()
    {
        playerAnimator.SetTrigger("isPunching");
    }

    public void RangedSkillMobile()
    {
        playerAnimator.SetTrigger("isShooting");
    }

    public void MoveForward()
    {
        playerRb.velocity = Vector3.zero;
        //playerRb.AddForce(transform.right * attackMoveForce * 100f * Time.deltaTime, ForceMode.Force);
        playerRb.AddForce(transform.right * attackMoveForce * 100f * Time.deltaTime, ForceMode.Acceleration);

    }


    public void AddXP()
    {
        levelSystem.AddExperience(90);
    }

    public void StartAttack(){
        isAttacking = true;
        UpdateColliders(true);
    }

    public void FinishAttack(){
        UpdateColliders(false);
    }

    // Temporary damage dealer
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy"){
            UpdateColliders(false);
            EnemyController enemyScript = other.GetComponent<EnemyController>();
            enemyScript.TakeDamage(_playerData.attackDamage);
        }
    }


    // Ranged Skill Logic //
    public void Shoot(){
        var proj = Instantiate(pfProjectile, projectileSpawn.position, projectileSpawn.rotation);

        proj.GetComponent<Rigidbody>().velocity = projectileSpawn.forward * projectileSpeed;
        proj.GetComponent<Projectile>().SetDamage(_playerData.attackDamage); //mudar dano depois
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
    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedArgs e)
    {
        switch (e.skillType)
        {
            case PlayerSkills.SkillType.Agility:
                //Debug.Log("+Agility");
                break;
            case PlayerSkills.SkillType.Defense:
                //Debug.Log("+Def");
                break;
            case PlayerSkills.SkillType.Strenght:
                //Debug.Log("+Str");
                break;
        }
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        pointsText.text = "SkillPoints:\n" + levelSystem.GetSkillTreePoints();
    }
} 

