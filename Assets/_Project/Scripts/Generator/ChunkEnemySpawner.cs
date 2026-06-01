using UnityEngine;
using System.Collections.Generic;

public class ChunkEnemySpawner : MonoBehaviour
{
    [Header("Кого спавнити")]
    public GameObject enemyPrefab;

    [Header("Можливі точки спавну")]
    public List<Transform> spawnPoints;

    [Header("Базова кількість (якщо немає Менеджера)")]
    public int fallbackEnemyCount = 1;

    public void SpawnEnemies()
    {
        if (spawnPoints.Count == 0 || enemyPrefab == null) return;

        int enemiesToSpawn = fallbackEnemyCount;

        if (DifficultyManager.Instance != null)
        {
            enemiesToSpawn = DifficultyManager.Instance.GetEnemyCountForChunk();
        }

        enemiesToSpawn = Mathf.Min(enemiesToSpawn, spawnPoints.Count);

        ShuffleSpawnPoints();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
        }
    }

    private void ShuffleSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Transform temp = spawnPoints[i];
            int randomIndex = Random.Range(i, spawnPoints.Count);
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }
    }
}