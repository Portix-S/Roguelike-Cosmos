using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MageBoss : MonoBehaviour
{
    public float lookRadius = 200f;
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
    float rangedAttackCooldownTimer = 2f;
    public float meeleRadius = 200f;
    [SerializeField] int damage = 5;

    [Header("Stats/Experience")]
    [SerializeField] int xpAmount = 10;

    [SerializeField]
    private List<Transform> enemies = new List<Transform>();


    /// Para o inimigo não começar se movendo


    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        //enemyAnimator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();

    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        //*
        if (agent.velocity != Vector3.zero)
        {
            //enemyAnimator.SetBool("isWalking", true);
            //Debug.Log("Anim: Andando");
        }
        else
        {
            //enemyAnimator.SetBool("isWalking", false);
            //Debug.Log("Anim: Parado");
        }

        if (distance <= lookRadius)
        {
            if (distance > meeleRadius)
            {
                // Variar entre dois ataques meelee
                if (!isAttacking)
                {
                    float randomAttack = Random.Range(0f, 100f);

                    if (randomAttack < 50f)
                    {
                        Debug.Log("Anim: Ataque Ranged 1");
                        isAttacking = true;
                        //enemyAnimator.SetBool("isAttacking", true);
                        StartCoroutine(AttackCooldown(rangedAttackCooldownTimer));
                    }
                    else
                    {
                        Debug.Log("Anim: Ataque Ranged 2");
                        isAttacking = true;
                        // enemyAnimator.SetBool("isAttacking", true);
                        StartCoroutine(AttackCooldown(rangedAttackCooldownTimer));
                    }
                }
                else
                {
                    agent.velocity = Vector3.zero;
                }
            }
            else
            {
                if (distance <= agent.stoppingDistance && !isAttacking)
                {
                    Debug.Log("Ataque meelee");
                    isAttacking = true;
                    //enemyAnimator.SetBool("isAttacking", true);
                    StartCoroutine(AttackCooldown());
                }
                else
                {
                    agent.SetDestination(target.position);
                    FaceTarget();

                    nextLocation = false;
                }
            }
        }
        else
        {
            // Sem ver o player
            if (nextLocation)
            {
                agent.SetDestination(randomPoint);
                //Debug.Log(randomPoint);
                float distance2 = Vector3.Distance(randomPoint, transform.position);
                //Debug.Log(distance2);

                if (distance2 <= agent.stoppingDistance)
                {
                    nextLocation = false;
                }

            }
            else
            {
                randomPoint = RandomNavmeshLocation(randomRadius);
                nextLocation = true;
            }
        }
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

    IEnumerator AttackCooldown(float time = 2f)
    {

        yield return new WaitForSeconds(time);

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
            //Debug.Log(healthPoints);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, randomRadius);
        Gizmos.DrawSphere(randomPoint, 1);
    }
}
