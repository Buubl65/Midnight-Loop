using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Характеристики")]
    public float health = 100f;
    public int damage = 10;

    [Header("Ефекти")]
    public ParticleSystem hitEffect;

    [Header("Лут (Нагорода)")]
    public GameObject moneyPrefab;  
    public GameObject healthPrefab;  
    [Range(0, 100)]
    public int dropChance = 80;     

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
        int randomChance = Random.Range(0, 101);

        if (randomChance <= dropChance)
        {
            int itemType = Random.Range(0, 2);
            GameObject itemToDrop = null;

            if (itemType == 0)
            {
                itemToDrop = moneyPrefab;
            }
            else if (itemType == 1)
            {
                itemToDrop = healthPrefab;
            }

            if (itemToDrop != null)
            {
                Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 0f);

                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Префаб луту не призначено в Інспекторі!");
            }
        }
    }
}