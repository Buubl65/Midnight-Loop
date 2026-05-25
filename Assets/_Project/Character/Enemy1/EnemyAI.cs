using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Transform player;
    public float health;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float sightRange;
    public float attackRange;
    public int damage;
    public Animator animator;
    public ParticleSystem hitEffect;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool takeDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("MainCharacter").transform;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (health <= 0 || takeDamage) return;

        // Просто вимірюємо дистанцію до гравця замість використання LayerMask
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

        // Створюємо випадкову точку навколо ворога
        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Перевіряємо, чи є поруч із цією точкою поверхня NavMesh (без використання шарів землі)
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
        navAgent.SetDestination(player.position);
        animator.SetFloat("Speed", 1f);
    }

    private void AttackPlayer()
    {
        navAgent.isStopped = true;
        navAgent.SetDestination(transform.position);
        transform.LookAt(player.position);
        animator.SetFloat("Speed", 0f);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            int randomAttack = Random.Range(0, 3);
            animator.SetInteger("AttackIndex", randomAttack);
            animator.SetTrigger("Attack");

            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                // Тут буде логіка завдання шкоди гравцю
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0) return;

        health -= damage;

        if (hitEffect != null) hitEffect.Play();
        animator.SetTrigger("Hit");

        StartCoroutine(TakeDamageCoroutine());

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        takeDamage = true;
        navAgent.isStopped = true;
        animator.SetFloat("Speed", 0f);

        yield return new WaitForSeconds(0.5f);

        takeDamage = false;
        if (health > 0) navAgent.isStopped = false;
    }

    private void Die()
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