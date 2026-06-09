using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    [Header("Налаштування суми")]
    public int minMoney = 5;  
    public int maxMoney = 20; 

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats playerStats = other.GetComponent<CharacterStats>();

        if (playerStats != null)
        {
            int randomAmount = Random.Range(minMoney, maxMoney + 1);

            // Додаємо гроші гравцю
            playerStats.AddMoney(randomAmount);

            Debug.Log("Ви підібрали гроші! Отримано: " + randomAmount + " монет.");

            Destroy(gameObject);
        }
    }
}