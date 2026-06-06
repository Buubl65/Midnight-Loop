using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public EnemyAI enemy;
    private bool hasHit;

    public void ResetHit()
    {
        hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemy.IsAttacking || hasHit)
            return;

        CharacterStats stats = other.GetComponent<CharacterStats>();

        if (stats != null)
        {
            EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

            stats.TakeDamage(enemyStats.damage);
            hasHit = true;
        }
    }
}