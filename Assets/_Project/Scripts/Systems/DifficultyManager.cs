using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Налаштування проходження")]
    public int currentLoop = 1;

    [Header("Множники характеристик")]
    public float healthMultiplier = 0.2f;
    public int extraDamage = 5;
    public float speedMultiplier = 0.05f;

    [Header("Налаштування кількості ворогів на чанк")]
    public int baseEnemyCountPerChunk = 1; 
    public int extraEnemiesPerLoop = 1;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseDifficulty()
    {
        currentLoop++;
        Debug.Log("Почався цикл №: " + currentLoop);
    }

    public int GetEnemyCountForChunk()
    {
        return baseEnemyCountPerChunk + (extraEnemiesPerLoop * (currentLoop - 1));
    }
}