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
    public bool canShoot = true;
    public float projectileSpeed;
    public float projectileCooldown;
    public bool isShooting;
    private Transform projectileHUD;

    [Header("AoE")]
    [SerializeField] private LayerMask whatIsEnemie;
    [SerializeField] private float areaSkillRange = 3.5f, areaSkillDamage = 2f;
    [SerializeField] private float areaSkillCooldown = 2f;
    private float cooldownCounter;
    [SerializeField] private ParticleSystem areaSkillEffect;
    public bool isAreaCasting = false;
    
    [Header("Mobile Input")]
    public Joystick joystickAttack;
    private Vector3 joystickAttackDirection;
    public Joystick joystickSkill;
    private Vector3 joystickSkillDirection;
    private bool isHUDActive = false;
    public Collider[] colliders;
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
        playerSkills = new PlayerSkills(levelSystem, pointsText, _playerData);
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
        playerRb = GetComponent<Rigidbody>();

        projectileHUD = transform.Find("Skills UI");//.transform.Find("Projectile Direction");
        if(projectileHUD.gameObject.activeSelf){
            projectileHUD.gameObject.SetActive(false);
        }
    }

    public LevelSystem GetLevelSystem()
    {
        return this.levelSystem;
    }

    private void Start()
    {
        system = SystemInfo.deviceType;
        if (system == DeviceType.Handheld)
        {
            attackMoveForce = 100f;
        }
    }

    private void LookAtMouse(Transform rotatedObject){
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        // Cria um raycast para achar o ponto do plano que o jogador está direcionando
        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(raio, out float enter)){
            Vector3 hit = raio.GetPoint(enter);
            Vector3 playerPos = plane.ClosestPointOnPlane(rotatedObject.position);
            Vector3 attackDirection = new Vector3(hit.x - playerPos.x, hit.y - playerPos.y, hit.z - playerPos.z);
            rotatedObject.rotation = Quaternion.LookRotation(attackDirection) * Quaternion.Euler(0f, -90f, 0f);
            if(rotatedObject != this.transform){
                rotatedObject.rotation *= Quaternion.Euler(90f, 0f, 0f);
            }
        }
    }

    void Update()
    {
        if (PauseMenu.isPaused) return;

        if (system == DeviceType.Desktop)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                LookAtMouse(this.transform);
                playerAnimator.SetTrigger("isPunching");
            }

            if(Input.GetKeyUp(KeyCode.Mouse1)){
                transform.rotation = projectileHUD.rotation * Quaternion.Euler(-90f, 0f, 0f);
                projectileHUD.rotation = transform.rotation * Quaternion.Euler( 90f, 0f, 0f);
                projectileHUD.gameObject.SetActive(false);

                RangedSkill();                
            }
            else if (Input.GetKey(KeyCode.Mouse1) && canShoot){
                // Should probably change the animation too
                // And make the player stay in place
                projectileHUD.gameObject.SetActive(true);
                LookAtMouse(projectileHUD);
            }

            if(Input.GetKeyDown(KeyCode.Q)){
                if(cooldownCounter <= 0){
                    isAreaCasting = true;
                    playerAnimator.SetTrigger("isAOE");
                    // AreaSkill();
                }
            }

            /*
                TEMPORARY CODE REMOVE LATER
            */
            if (Input.GetKey(KeyCode.X))
            {
                levelSystem.AddExperience(100);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                levelSystem.AddExperience(10);
            }
        }
        else if (system == DeviceType.Handheld){
            if(joystickAttack.Horizontal != 0 || joystickAttack.Vertical != 0){
                joystickAttackDirection = new Vector3(joystickAttack.Horizontal, 0f, joystickAttack.Vertical);
                transform.rotation = Quaternion.LookRotation(joystickAttackDirection) * Quaternion.Euler(0f, -90f, 0f);
                Debug.Log("ATACA O AST");
                playerAnimator.SetTrigger("isPunching");
            }

            if(joystickSkill.Horizontal != 0 || joystickSkill.Vertical != 0){
                if(!isHUDActive){
                    projectileHUD.gameObject.SetActive(true);
                    Debug.Log("ATACA O AST COM SKILL");

                    isHUDActive = true;
                }
                joystickSkillDirection = new Vector3(joystickSkill.Horizontal, 0f, joystickSkill.Vertical);
                projectileHUD.rotation = Quaternion.LookRotation(joystickSkillDirection) * Quaternion.Euler(90f, -90f, 0f);
            }
            else if(joystickSkill.Horizontal == 0 && joystickSkill.Vertical == 0 && isHUDActive){
                transform.rotation = projectileHUD.rotation * Quaternion.Euler(-90f, 0f, 0f);
                projectileHUD.rotation = transform.rotation * Quaternion.Euler( 90f, 0f, 0f);
                RangedSkill();
                projectileHUD.gameObject.SetActive(false);
                isHUDActive = false;
            }

        }
        currentPoints = levelSystem.GetSkillTreePoints();
        currentStatsPoints = levelSystem.GetStatPoints();

        if(cooldownCounter > 0)
            cooldownCounter -= Time.deltaTime;
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, areaSkillRange);
    // }

    public void MobilePunch()
    {
        playerAnimator.SetTrigger("isPunching");
    }

    public void RangedSkill()
    {
        if (canShoot && !isAttacking)
        {
            canShoot = false;
            isShooting = true;
            playerAnimator.SetTrigger("isShooting");
        }
    }

    public void MobileAreaSkill(){
        if(cooldownCounter <= 0){
            isAreaCasting = true;
            playerAnimator.SetTrigger("isAOE");
        }
    }

    public void AreaSkill(){
         colliders = Physics.OverlapSphere(transform.position, areaSkillRange, whatIsEnemie);
        areaSkillEffect.Play();
        foreach(Collider col in colliders){
            if(col.gameObject.tag == "Enemy")
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<EnemyController>().TakeDamage(_playerData.MagicDamage/3f);
            }
            else if(col.gameObject.tag == "Boss")
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<MageBoss>().TakeDamage(_playerData.MagicDamage/3f);
            }
            else if(col.gameObject.tag == "Lancer")
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<lancer>().TakeDamage(_playerData.MagicDamage/3f);
            }
            else if(col.gameObject.tag == "Tentacle")
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<TentacleController>().TakeDamage(_playerData.MagicDamage/3f);
            }
        }
        cooldownCounter = areaSkillCooldown;
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
        if((other.tag == "Enemy") && (isAttacking || isShooting)){
            UpdateColliders(false);
            EnemyController enemyScript = other.GetComponent<EnemyController>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if ((other.tag == "Boss") && (isAttacking || isShooting))
        {
            UpdateColliders(false);
            MageBoss enemyScript = other.GetComponent<MageBoss>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if ((other.tag == "Lancer") && (isAttacking || isShooting))
        {
            UpdateColliders(false);
            lancer enemyScript = other.GetComponent<lancer>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if ((other.tag == "Lasquinha") && (isAttacking || isShooting))
        {
            UpdateColliders(false);
            lasquinha enemyScript = other.GetComponent<lasquinha>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if ((other.tag == "Tentacle") && (isAttacking || isShooting))
        {
            UpdateColliders(false);
            TentacleController enemyScript = other.GetComponent<TentacleController>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }

    }


    // Ranged Skill Logic //
    public void Shoot(){
        var proj = Instantiate(pfProjectile, projectileSpawn.position, projectileSpawn.rotation);

        proj.GetComponent<Rigidbody>().velocity = projectileSpawn.forward * projectileSpeed;
        proj.GetComponent<Projectile>().SetDamage(_playerData.MagicDamage); //mudar dano depois
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

