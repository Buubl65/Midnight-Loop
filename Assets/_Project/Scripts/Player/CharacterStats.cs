using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    public float defense = 10f;
    public float moveSpeed = 5f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Получение урона
    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defense;

        if (finalDamage < 0)
            finalDamage = 0;

        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Лечение
    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    void Die()
    {
        Debug.Log("Игрок умер");
    }
}
