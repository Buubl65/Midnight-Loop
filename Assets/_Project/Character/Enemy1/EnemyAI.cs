using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStats))] 
public class EnemyAI : MonoBehaviour
{
    [Header("Компоненти")]
    public NavMeshAgent navAgent;
    public Transform player;
    public Animator animator;

    [Header("Налаштування AI")]
    public float walkPointRange = 10f;
    public float timeBetweenAttacks = 2f;
    public float sightRange = 15f;
    public float attackRange = 2f;

    [Header("Швидкість руху")]
    public float walkSpeed = 3.5f; 
    public float chaseSpeed = 9f; 

    private EnemyStats stats;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool isTakingDamage; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("MainCharacter").transform;
        navAgent = GetComponent<NavMeshAgent>();
        stats = GetComponent<EnemyStats>(); 
    }

    private void Update()
    {
        if (stats.health <= 0 || isTakingDamage) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        bool playerInSightRange = distanceToPlayer <= sightRange;
        bool playerInAttackRange = distanceToPlayer <= attackRange;

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        navAgent.isStopped = false;
        navAgent.speed = walkSpeed;

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        animator.SetFloat("Speed", 0.5f);

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        navAgent.isStopped = false;
        navAgent.speed = chaseSpeed;
        navAgent.SetDestination(player.position);
        animator.SetFloat("Speed", 2f);
    }

    private void AttackPlayer()
    {
        navAgent.isStopped = true;
        navAgent.SetDestination(transform.position);

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
        animator.SetFloat("Speed", 0f);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            int randomAttack = Random.Range(0, 3);
            animator.SetInteger("AttackIndex", randomAttack);
            animator.SetTrigger("Attack");

            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            RaycastHit hit;
            Debug.Log("КОМАНДА НА УДАР ПІШЛА! Удар номер: " + randomAttack);
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                // Тут буде логіка завдання шкоди гравцю. 
                // Ти зможеш брати значення шкоди так: stats.damage
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TriggerHit()
    {
        animator.SetTrigger("Hit");
        StartCoroutine(TakeDamageCoroutine());
    }

    private IEnumerator TakeDamageCoroutine()
    {
        isTakingDamage = true;
        navAgent.isStopped = true;
        animator.SetFloat("Speed", 0f);

        yield return new WaitForSeconds(0.5f);

        isTakingDamage = false;
        if (stats.health > 0) navAgent.isStopped = false;
    }

    public void TriggerDeath()
    {
        navAgent.enabled = false;
        GetComponent<Collider>().enabled = false;

        animator.SetBool("Dead", true);
        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}