using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using DG.Tweening;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public DeviceType system;
    private PlayerSkills playerSkills;
    private Rigidbody playerRb;

    /*------------------------------  LEVEL SYSTEM  ------------------------------------*/
    [Header("Level System")]
    [SerializeField] LevelWindow levelWindow;
    private LevelSystem levelSystem;
    [SerializeField] int currentPoints;      // Necessário?
    [SerializeField] int currentStatsPoints; // Necessário?
    [SerializeField] TextMeshProUGUI pointsText;

    /*------------------------------  ANIMATION  --------------------------------------*/
    [Header("Animation")]
    public Animator playerAnimator;

    /*------------------------------  BASIC COMBAT  ------------------------------------*/
    [Header("Basic Combat")]
    [SerializeField] private Collider[] combatColliders;
    [SerializeField] float attackMoveForce = 1f;
    [HideInInspector] public bool isAttacking;

    /*------------------------------  RANGED SKILL  ------------------------------------*/
    [Header("Ranged Skill")]
    public GameObject pfProjectile;
    public Transform  projectileSpawnPoint;
    
    public float rangedSkillSpeed;
    public float rangedSkillCoolDown;
    [HideInInspector] public bool canShoot = true;

    [HideInInspector] public bool isShooting;

    private Transform projectileHUD;
    private Transform directionHUD;

    /*------------------------------  AREA SKILL  ------------------------------------*/
    [Header("Area Skill")]
    [SerializeField] private ParticleSystem areaSkillEffect;
    [SerializeField] private LayerMask whatIsEnemie;

    [SerializeField] private float areaSkillRange = 3.5f;
    [SerializeField] private float areaSkillDamage = 2f;
    [SerializeField] private float areaSkillCooldown = 2f;

    [HideInInspector] public bool isAreaCasting = false;
    private float cooldownCounter;
    //[HideInInspector] public Collider[] enemieColliders; // Isso precisa ser global?
    
    /*------------------------------  MOBILE BUTTONS  ------------------------------------*/
    [Header("Mobile Input")]
    public Joystick joystickAttack;
    private Vector3 joystickAttackDirection;

    public Joystick joystickSkill;
    private Vector3 joystickSkillDirection;

    private bool isHUDActive = false;

    private void Awake() {
        system = SystemInfo.deviceType;

        // @Portix, documenta isso aqui
        levelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(levelSystem);
        pointsText.text = "SkillPoints:\n" + levelSystem.GetSkillTreePoints();
        playerSkills = new PlayerSkills(levelSystem, pointsText, _playerData);
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

        playerRb = GetComponent<Rigidbody>();

        UpdateColliders(false);

        projectileHUD = transform.Find("Skills UI");
        if(projectileHUD.gameObject.activeSelf){
            projectileHUD.gameObject.SetActive(false);
        }
        directionHUD = projectileHUD.Find("Projectile Direction");
    }

    private void Start()
    {
        system = SystemInfo.deviceType; // Isso já não ta sendo feito no Awake?
        if (system == DeviceType.Handheld)
        {
            attackMoveForce = 100f;
        }
    }

    public LevelSystem GetLevelSystem()
    {
        return this.levelSystem;
    }

    /*
        Função para ativar e desativar os colliders usados para registrar o ataque básico
    */
    public void UpdateColliders(bool enable){
        foreach(Collider col in combatColliders){
            col.enabled = enable;
        }
    }

    Vector3 hit;
    /*
        LookAtMouse()
        Ajusta a posição do jogador/HUD para fazer o objeto olhar na direção do mouse
    */
    private void LookAtMouse(Transform rotatedObject){
        // Usa o NavMesh para achar a altura do plano que corresponde ao chão do jogador
        NavMesh.SamplePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), out NavMeshHit NMHit, 100f, NavMesh.AllAreas);
        Plane plane = new Plane(Vector3.up, NMHit.position);

        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);

        hit = rotatedObject.position;
        if(plane.Raycast(raio, out float enter)){
            // Usando Raycast, acha o ponto corresponde à posição do mouse no plano isométrico
            hit = raio.GetPoint(enter);

            // Se essa função for chamada para o rotacionar o jogador, apenas LookAt() basta
            if(rotatedObject == this.transform){
                rotatedObject.LookAt(hit);
                return;
            }

            // Como o HUD é "2D", é preciso ajustar cada eixo manualmente calculando o ângulo da rotação
            Vector3 diff = hit - rotatedObject.position;
            float rot = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
            rotatedObject.rotation = Quaternion.Euler(-90f, 0f, rot - 90f); 
        }
    }

    void Update()
    {
        if (PauseMenu.isPaused) return;

        /*--- CONTROLES DESKTOP ---*/
        if (system == DeviceType.Desktop)
        {
            /*-- ATAQUE BÁSICO  --*/
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                LookAtMouse(this.transform);
                playerAnimator.SetTrigger("isPunching");
            }


            /*-- SKILL --*/
            /*
                O jogador pode segurar o botão do mouse para mirar a skill;
                Enquanto mira, ele pode mover-se livremente, e uma HUD mostra a direção mirada.

                Quando o jogador solta o botão ele para de se mover, se vira para a direção mirada e atira uma bola de fogo uau  
            */
            if(Input.GetKeyUp(KeyCode.Mouse1)){
                transform.forward = directionHUD.right;
                directionHUD.right = transform.forward;
                projectileHUD.gameObject.SetActive(false);

                TriggerRangedSkill(); // Ver Implementeção (Selecione + F12)         
            }
            else if (Input.GetKey(KeyCode.Mouse1) && canShoot){
                if(!projectileHUD.gameObject.activeSelf){
                    projectileHUD.gameObject.SetActive(true);
                }

                LookAtMouse(directionHUD);
            }


            /*-- AREA SKILL --*/
            if(Input.GetKeyDown(KeyCode.Q)){
                TriggerAreaSkill(); // Ver Implementação (Selecione + F12)
            }

            /* Usar as funções do menu de contexto (Implementação no fim do código) */
            // if (Input.GetKey(KeyCode.X) levelSystem.AddExperience(100);
            // if (Input.GetKeyDown(KeyCode.Z)) levelSystem.AddExperience(10);
        }


        /*--- CONTROLES MOBILE---*/
        else if (system == DeviceType.Handheld)
        {
            /*-- ATAQUE BÁSICO  --*/
            if(Mathf.Abs(joystickAttack.Horizontal) > Mathf.Epsilon || Mathf.Abs(joystickAttack.Vertical) > Mathf.Epsilon){
                joystickAttackDirection = new Vector3(joystickAttack.Horizontal, 0f, joystickAttack.Vertical);
                transform.rotation = Quaternion.LookRotation(joystickAttackDirection) * Quaternion.Euler(0f, -90f, 0f);
                playerAnimator.SetTrigger("isPunching");
            }

            /*-- SKILL --*/
            if(Mathf.Abs(joystickSkill.Horizontal) > Mathf.Epsilon || Mathf.Abs(joystickSkill.Vertical) > Mathf.Epsilon){
                if(!projectileHUD.gameObject.activeSelf){
                    projectileHUD.gameObject.SetActive(true);
                }

                joystickSkillDirection = new Vector3(joystickSkill.Horizontal, 0f, joystickSkill.Vertical);

                // Semelhante à função LookAtMouse()
                float rot = Mathf.Atan2(joystickSkillDirection.x, joystickSkillDirection.z) * Mathf.Rad2Deg;
                directionHUD.rotation = Quaternion.Euler(-90f, 0f, rot - 90f);
            }
            else if(Mathf.Abs(joystickSkill.Horizontal) < Mathf.Epsilon  && Mathf.Abs(joystickSkill.Vertical) < Mathf.Epsilon   && projectileHUD.gameObject.activeSelf){
                transform.forward = directionHUD.right;
                directionHUD.right = transform.forward;
                projectileHUD.gameObject.SetActive(false);

                TriggerRangedSkill();
            }

            /*-- AREA SKILL --*/
            /* -Implementado como botão */
        }

        currentPoints = levelSystem.GetSkillTreePoints();
        currentStatsPoints = levelSystem.GetStatPoints();

        // Update do timer do cooldown
        if(cooldownCounter > 0)
            cooldownCounter -= Time.deltaTime;
    }

    /*-------------------- LÓGICA SKILL RANGED --------------------*/
    public void TriggerRangedSkill()
    {
        if (canShoot && !isAttacking){
            playerAnimator.SetTrigger("isShooting");
            canShoot = false;
            isShooting = true;
        }
    }

    // Chamada por evento na animação
    public void Shoot(){
        var proj = Instantiate(pfProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        proj.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * rangedSkillSpeed;
        proj.GetComponent<Projectile>().SetDamage(_playerData.MagicDamage);
        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown(){
        yield return new WaitForSeconds(rangedSkillCoolDown);
        canShoot = true;
    }

    /*-------------------- LÓGICA SKILL AREA --------------------*/
    public void TriggerAreaSkill()
    {
        if(cooldownCounter <= 0){
            playerAnimator.SetTrigger("isAOE");
            isAreaCasting = true;
        }
    }

    // Chamada por evento na animação
    public void AreaSkill(){
        // Coleta todos os colliders na área da skill
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaSkillRange, whatIsEnemie);

        // Ativa efeito de partícula
        areaSkillEffect.Play();

        // Testa se o collider é de um inimigo e aplica o dano
        foreach(Collider col in colliders){
            if(col.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<EnemyController>().TakeDamage(_playerData.MagicDamage/3f);
            }
            else if(col.gameObject.CompareTag("Boss"))
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<MageBoss>().TakeDamage(_playerData.MagicDamage/3f);
            }
            else if(col.gameObject.CompareTag("Lancer"))
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<lancer>().TakeDamage(_playerData.MagicDamage/3f);
            }
            else if(col.gameObject.CompareTag("Tentacle"))
            {
                Debug.Log("Dano em área boom");
                col.GetComponent<TentacleController>().TakeDamage(_playerData.MagicDamage/3f);
            }
        }

        // Atualiza o cooldown da skill
        cooldownCounter = areaSkillCooldown;
    }

    /*-------------------- LÓGICA ATAQUE BÁSICO --------------------*/
    // Chamada no Behaviour do animator
    public void StartAttack(){
        isAttacking = true;
        UpdateColliders(true);
        Invoke("MoveForward", 0.5f);
    }

    // Chamada no Behaviour do animator
    public void FinishAttack(){
        UpdateColliders(false);
    }

    public void MoveForward(){
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(transform.forward * attackMoveForce * 100f * Time.deltaTime, ForceMode.Acceleration);
    }

    /* Aplica dano do ataque básico */
    public void Attack(Collider other) {
        Debug.Log("Trying to attack " + other.name);
        if((other.CompareTag("Enemy")) && (isAttacking || isShooting)){
            EnemyController enemyScript = other.GetComponent<EnemyController>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if ((other.CompareTag("Boss")) && (isAttacking || isShooting))
        {
            MageBoss enemyScript = other.GetComponent<MageBoss>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if ((other.CompareTag("Lancer")) && (isAttacking || isShooting))
        {
            lancer enemyScript = other.GetComponent<lancer>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if ((other.CompareTag("Lasquinha")) && (isAttacking || isShooting))
        {
            lasquinha enemyScript = other.GetComponent<lasquinha>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
        else if((other.CompareTag("Tentacle")) && (isAttacking || isShooting))
        {
            TentacleController enemyScript = other.GetComponent<TentacleController>();
            enemyScript.TakeDamage(_playerData.AttackDamage);
        }
    }

    /*-------------------- LÓGICA SKILL SET --------------------*/
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

    /*------------------------------------------ TESTE ------------------------------------------*/
    [ContextMenu("Add 10 Exp")]
    void Plus10exp(){
        levelSystem.AddExperience(10);
    }

    [ContextMenu("Add 100 Exp")]
    void Plus100exp(){
        levelSystem.AddExperience(100);
    }
} 