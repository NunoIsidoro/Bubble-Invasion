using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject powerUpParent; // Objeto pai para organizar os PowerUps na hierarquia
    public GameObject powerUpPrefab; // Prefab do PowerUp
    public BoxCollider2D spawnArea; // Collider que define a área de spawn
    public float minSpawnInterval = 15f; // Intervalo mínimo entre os spawns
    public float maxSpawnInterval = 35f; // Intervalo máximo entre os spawns

    private float nextSpawnTime;

    void Start()
    {
        // Define o primeiro tempo de spawn com aleatoriedade
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        if (playerStats.GetHeartsCount() >= 5)
            return;
        
        // Verifica se é hora de spawnar um novo PowerUp
        if (Time.time >= nextSpawnTime)
        {
            SpawnPowerUp();
            // Atualiza o tempo do próximo spawn com aleatoriedade
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnPowerUp()
    {
        // Gera uma posição aleatória dentro dos limites do BoxCollider2D
        Bounds bounds = spawnArea.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Instancia o PowerUp na posição gerada
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity, powerUpParent.transform);
    }
}
