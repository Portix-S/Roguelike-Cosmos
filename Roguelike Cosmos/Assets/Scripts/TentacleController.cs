using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TentacleController : MonoBehaviour
{

    public float lookRadius = 10f;
    public float randomRadius = 20f;
    Transform target;
    NavMeshAgent agent;
    public bool nextLocation = false;
    public Vector3 randomPoint = new Vector3(0, 0, 0);
    [SerializeField] float healthPoints = 100f;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] private Collider collider;

    [Header("Attack Config")]
    bool isAttacking;
    float attackCooldownTimer = 2f;
    [SerializeField] int damage = 5;

    [Header("Stats/Experience")]
    [SerializeField] int xpAmount = 10;
    private bool isActive = false;
    private int attack;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            if(!isActive){
                isActive = true;
                foreach (Transform child in this.transform){
                    child.gameObject.SetActive(true);
                }
            }
            else{
                isAttacking = true;
                other.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
                enemyAnimator.SetInteger("isAttacking", attack);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player")
        {
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Player"){
            if(isActive){
                isAttacking = true;
                attack = Random.Range(0, 3);
                enemyAnimator.SetInteger("isAttacking", attack);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    void Start()
    {
        foreach (Transform child in this.transform){
            child.gameObject.SetActive(false);
        }
        target = PlayerManager.instance.player.transform;
        //agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        FaceTarget();
    }

    public void StopAttacking()
    {
        enemyAnimator.SetBool("isAttacking", false);
    }

    public void StopTakingDamage()
    {
        enemyAnimator.SetBool("isTakingDamage", false);
    }


    public int GetDamage()
    {
        return damage;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldownTimer);
        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        enemyAnimator.SetBool("isTakingDamage", true);
        CameraShake.Instance.ShakeCamera(2f, 0.2f);
        float height = collider.bounds.extents.y / 2f;
        Vector3 popupPos = transform.position + transform.up * height;
        if (healthPoints - amount > 0f)
        {
            healthPoints -= amount;
            Tools.Graphics.CreateDamagePopup(amount, popupPos);
        }
        else
        {
            Tools.Graphics.CreateDamagePopup(healthPoints, popupPos);
            healthPoints = 0f;
            target.GetComponent<PlayerCombat>().GetLevelSystem().AddExperience(xpAmount);
            Destroy(gameObject);
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, lookRadius);
    //     Gizmos.DrawWireSphere(transform.position, randomRadius);
    //     Gizmos.DrawSphere(randomPoint, 1);
    // }
}
