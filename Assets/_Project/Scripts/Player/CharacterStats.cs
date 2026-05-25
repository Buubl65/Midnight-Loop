using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float defense = 10f;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpSpeed = 7f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;

    public float staminaRegen = 15f;
    public float dashStaminaCost = 25f;
    public float jumpStaminaCost = 15f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        RegenerateStamina();
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

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
    }

    public bool HasEnoughStamina(float cost)
    {
        return currentStamina >= cost;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;

        if (currentStamina < 0)
            currentStamina = 0;
    }

    void Die()
    {
        Debug.Log("Игрок умер");

        GameManager.Instance.GameOver();
    }
}
