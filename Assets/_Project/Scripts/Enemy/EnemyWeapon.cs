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
        // Якщо ворог не атакує або вже завдав удару за цю анімацію — ігноруємо
        if (!enemy.IsAttacking || hasHit)
            return;

        // Перевіряємо, чи є у об'єкта, якого торкнувся меч, скрипт CharacterStats (це має бути ваш гравець)
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