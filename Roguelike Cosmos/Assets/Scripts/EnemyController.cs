using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{

    public float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;
    public bool nextLocation = false;
    public Vector3 randomPoint = new Vector3(0, 0, 0);
    [SerializeField] float healthPoints = 100f;
    [SerializeField] Animator enemyAnimator;

    [Header("Attack Config")]
    bool isAttacking;
    float attackCooldownTimer = 2f;
    [SerializeField] int damage = 5;

    [Header("Stats/Experience")]
    [SerializeField] int xpAmount = 10;

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        //*
        if (agent.velocity != Vector3.zero)
        {
            enemyAnimator.SetBool("isWalking", true);
        }
        else 
        {
            enemyAnimator.SetBool("isWalking", false);
        }

        if (distance <= agent.stoppingDistance && !isAttacking)
        {
            isAttacking = true;
            enemyAnimator.SetBool("isAttacking", true);
            StartCoroutine(AttackCooldown());
        }

        //*/

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }
        else
        {

            if (nextLocation)
            {
                agent.SetDestination(randomPoint);
                float distance2 = Vector3.Distance(randomPoint, transform.position);
                //Debug.Log(distance2);

                if (distance2 <= agent.stoppingDistance)
                {
                    nextLocation = false;
                    //Attack;
                }

            }
            else
            {
                randomPoint = new Vector3(Random.Range(-20f, 3f), 0, Random.Range(-10f, 3f));
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

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldownTimer);
        isAttacking = false;
    }
    public void TakeDamage(float amount)
    {
        enemyAnimator.SetBool("isTakingDamage", true);
        CameraShake.Instance.ShakeCamera(2f, 0.2f);
        if(healthPoints - amount > 0f)
        {
            healthPoints -= amount;
            Debug.Log(healthPoints);
        }
        else
        {
            healthPoints = 0f;
            target.GetComponent<PlayerCombat>().GetLevelSystem().AddExperience(xpAmount);
            Destroy(gameObject);
        }
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
    }
}
