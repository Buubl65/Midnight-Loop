using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Компоненти")]
    public Animator animator;
    public Transform attackPoint; // Точка удару (пушка або кулаки)
    public LayerMask enemyLayers; // Шар, на якому знаходяться вороги (налаштуйте в Unity!)

    [Header("Налаштування Атаки")]
    public float attackDamage = 25f; // Шкода (зверніть увагу, тепер це float, як у вашому EnemyStats)
    public float attackRange = 0.8f; // Радіус удару

    private bool isLeftPunchNext = true;

    void Update()
    {
        // Клік лівою кнопкою миші
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // 1. Анімація удару по черзі
        if (isLeftPunchNext)
        {
            animator.SetTrigger("PunchLeft");
        }
        else
        {
            animator.SetTrigger("PunchRight");
        }
        isLeftPunchNext = !isLeftPunchNext;

        // 2. Викликаємо нанесення шкоди
        // Ідеально — викликати цю функцію через Animation Event в анімації удару!
        DealDamage();
    }

    // Функція, яка перевіряє попадання і наносить дамаг
    public void DealDamage()
    {
        // Перевіряємо, чи є точка удару
        if (attackPoint == null) return;

        // Створюємо невидиму сферу і збираємо всі коллайдери ворогів, які в неї потрапили
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Проходимось по кожному ворогу в зоні ураження
        foreach (Collider enemyCollider in hitEnemies)
        {
            // Шукаємо ВАШ скрипт EnemyStats на об'єкті, по якому попали
            EnemyStats enemyStats = enemyCollider.GetComponent<EnemyStats>();

            // Якщо скрипт знайдено (тобто це дійсно ворог)
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(attackDamage); // Наносимо дамаг!
                Debug.Log("Гравець попав по ворогу! Нанесено шкоди: " + attackDamage);
            }
        }
    }

    // Малює сферу удару в редакторі (щоб вам було зручно налаштувати attackRange)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}