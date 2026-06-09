using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Налаштування")]
    public float healAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats playerStats = other.GetComponent<CharacterStats>();

        if (playerStats != null)
        {
            playerStats.Heal(healAmount);

            Debug.Log("Підібрано аптечку! Відновлено " + healAmount + " ХП.");

            Destroy(gameObject);
        }
    }
}