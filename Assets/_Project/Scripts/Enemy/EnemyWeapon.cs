using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public EnemyAI enemy;
    private bool hasHit;

    private void Awake()
    {
        if (enemy == null)
        {
            enemy = GetComponentInParent<EnemyAI>();
        }
    }

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

            // Завдаємо шкоди
            stats.TakeDamage(enemyStats.damage);
            hasHit = true; // Забороняємо завдавати шкоду двічі за один помах

            Debug.Log("Ворог наніс дамаг!"); // Для перевірки в консолі
        }
    }
}