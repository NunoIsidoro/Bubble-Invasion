using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Collider2D SpawnArea;
    public Collider2D GameArea;
    public float spawnInterval = 0.5f;

    private int enemiesToSpawn;
    private int enemiesSpawned;
    private Coroutine spawnCoroutine;
    
    private List<Enemy> enemies = new List<Enemy>();


    public void StartSpawning(int numberOfEnemies)
    {
        enemiesToSpawn = numberOfEnemies;
        enemiesSpawned = 0;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    public bool AreAllEnemiesSpawned()
    {
        return enemiesSpawned >= enemiesToSpawn;
    }

    private IEnumerator SpawnEnemies()
    {
        // Call EndWave on all enemies from the previous wave
        foreach (var enemy in enemies)
        {
            enemy.EndWave();
        }
        enemies = new List<Enemy>();

        while (enemiesSpawned < enemiesToSpawn)
        {
            SpawnEnemy();
            enemiesSpawned++;
            Debug.Log($"Inimigo {enemiesSpawned}/{enemiesToSpawn} spawnado.");

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("Todos os inimigos desta wave foram spawnados.");
        spawnCoroutine = null; // Libera a referência da corrotina quando concluído
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomPositionWithinCollider();
        var obj = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        obj.SetActive(true);
        obj.GetComponent<Enemy>().Initialize(GameArea);
        enemies.Add(obj.GetComponent<Enemy>());
    }

    private Vector2 GetRandomPositionWithinCollider()
    {
        if (SpawnArea == null)
        {
            Debug.LogError("SpawnArea não foi atribuído!");
            return Vector2.zero;
        }

        Bounds bounds = SpawnArea.bounds;

        Vector2 randomPosition;

        // Garante que a posição gerada está dentro do Collider2D
        do
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            randomPosition = new Vector2(randomX, randomY);
        }
        while (!SpawnArea.OverlapPoint(randomPosition)); // Verifica se a posição está dentro do Collider2D

        return randomPosition;
    }
}
