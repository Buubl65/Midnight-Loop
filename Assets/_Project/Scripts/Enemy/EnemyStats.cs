using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Характеристики")]
    public float health = 100f;
    public int damage = 10;

    [Header("Ефекти")]
    public ParticleSystem hitEffect;

    private EnemyAI enemyAI;

    private void Start()
    {
        if (DifficultyManager.Instance != null)
        {
            int loop = DifficultyManager.Instance.currentLoop;

            if (loop > 1)
            {
                health += health * (DifficultyManager.Instance.healthMultiplier * (loop - 1));
                damage += DifficultyManager.Instance.extraDamage * (loop - 1);
            }
        }
    }

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float damageAmount)
    {
        if (health <= 0) return;

        health -= damageAmount;

        if (hitEffect != null) hitEffect.Play();

        if (health <= 0)
        {
            enemyAI.TriggerDeath();
        }
        else
        {
            enemyAI.TriggerHit();
        }
    }
}