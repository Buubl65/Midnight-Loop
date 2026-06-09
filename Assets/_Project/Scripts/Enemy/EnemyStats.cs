using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Характеристики")]
    public float health = 100f;
    public int damage = 10;

    [Header("Ефекти")]
    public ParticleSystem hitEffect;

    [Header("Налаштування Луту")]
    public GameObject moneyPrefab;  
    public GameObject healthPrefab;  
    [Range(0, 100)]
    public int dropChance = 70;     

    private EnemyAI enemyAI;
    private bool isDead = false;    


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
        if (health <= 0 || isDead) return;

        health -= damageAmount;

        if (hitEffect != null) hitEffect.Play();

        if (health <= 0)
        {
            isDead = true;

            DropLoot();

            enemyAI.TriggerDeath();
        }
        else
        {
            enemyAI.TriggerHit();
        }
    }

    private void DropLoot()
    {
        if (Random.Range(0, 101) <= dropChance)
        {
            int randomChoice = Random.Range(0, 2);
            GameObject selectedPrefab = (randomChoice == 0) ? moneyPrefab : healthPrefab;

            if (selectedPrefab != null)
            {
                Vector3 spawnPos = transform.position + new Vector3(0f, 0.5f, 0f);

                Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}