using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MageBoss : MonoBehaviour
{
    public float lookSpeed = 5;
    public float lookRadius = 200f;
    public float randomRadius = 20f;
    Transform target;
    NavMeshAgent agent;
    public bool nextLocation = false;
    public Vector3 randomPoint = new Vector3(0, 0, 0);
    [SerializeField] float healthPoints = 100f;
    Animator animator;
    [SerializeField] private Collider collider;

    [Header("Attack Config")]
    bool isAttacking;
    float attackCooldownTimer = 2f;
    float rangedAttackCooldownTimer = 2f;
    public float atkRadius = 200f;
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
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();

    }

    void Update()
    {
        
        float distance = Vector3.Distance(target.position, transform.position);
        //*
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Debug.Log("Anim: Andando");
        }
        else
        {
            animator.SetBool("isMoving", false);
            Debug.Log("Anim: Parado");
            FaceTarget(1);
        }

        if (distance <= lookRadius && !isAttacking)
        {
            if (distance < atkRadius) //parar para atacar
            {
                
                agent.velocity = Vector3.zero;
                animator.SetBool("isMoving", false);
                // Variar entre dois ataques meelee
                if (!isAttacking)
                {
                    
                    float randomAttack = Random.Range(0f, 100f);

                    if (randomAttack < 40f)
                    {
                        Debug.Log("Anim: Ataque Ranged 1");
                        isAttacking = true;
                        animator.SetTrigger("atk1");
                        
                        StartCoroutine(AttackCooldown(3));
                    }
                    else if(randomAttack >= 40 && randomAttack < 80)
                    {
                        Debug.Log("Anim: Ataque Ranged 2");
                        isAttacking = true;
                        animator.SetTrigger("atk2");
                        StartCoroutine(AttackCooldown(4));
                    }
                    else
                    {
                        Debug.Log("Anim: Ataque Ranged 3");
                        isAttacking = true;
                        animator.SetTrigger("atk3");
                        StartCoroutine(AttackCooldown(1));
                    }
                }
            }
            else
            {
                FaceTarget(lookSpeed);
                agent.SetDestination(target.position);
                
                nextLocation = false;
            }
        }

    }

    public void StopAttacking()
    {
        animator.SetBool("isAttacking", false);
    }

    public void StopTakingDamage()
    {
        animator.SetTrigger("takeDamage");
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
        animator.SetTrigger("takeDamage");
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

    void FaceTarget(float ls)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * ls);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, randomRadius);
        Gizmos.DrawSphere(randomPoint, 1);
    }
}
