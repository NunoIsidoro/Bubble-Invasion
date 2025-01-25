using UnityEngine;
using System.Collections;
using Project.Runtime.Scripts.UI.Gameplay.Components;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public BigBubbleSpawner bigBubbleSpawner;
    public KrabSpawner KrabSpawner;
    public PlayerStats PlayerStats;
    public Timer timer;
    public float initialTimeBetweenWaves = 0f;
    public float timeIncrement = 5f;
    public float maxTimeBetweenWaves = 60f;

    public int currentWave = 1;
    private float timeBetweenWaves; // Tempo atual entre waves
    private int maxSpawnableBubbles = 1;
    private int maxSpawnableKrabs = 1;
    
    [Header("UI")]
    public TextMeshProUGUI waveText;

    private void OnEnable()
    {
        timeBetweenWaves = initialTimeBetweenWaves;
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (true)
        {
            waveText.text = $"{currentWave}";
            Debug.Log($"Iniciando Wave {currentWave}.");
            enemySpawner.StartSpawning(currentWave + 1);
            
            if (currentWave >= 2)
                bigBubbleSpawner.StartSpawning(maxSpawnableBubbles);
            
            if (currentWave >= 3)
                KrabSpawner.StartSpawning(maxSpawnableKrabs);

            while (!enemySpawner.AreAllEnemiesSpawned())
            {
                yield return null;
            }

            Debug.Log($"Wave {currentWave} concluída!");

            if (timeBetweenWaves < maxTimeBetweenWaves)
            {
                timeBetweenWaves += timeIncrement;
                if (timeBetweenWaves > maxTimeBetweenWaves)
                {
                    timeBetweenWaves = maxTimeBetweenWaves;
                }
            }

            Debug.Log($"Próxima wave começará em {timeBetweenWaves} segundos...");
            yield return new WaitForSeconds(timeBetweenWaves);
            
            // Incrementa a capacidade de spawnar bolhas a cada 2 waves
            if (currentWave >= 2 && currentWave % 2 == 0)
            {
                maxSpawnableBubbles++;
            }
            // Incrementa a capacidade de spawnar Krabs a cada 3 waves
            if (currentWave >= 3 && currentWave % 3 == 0)
            {
                maxSpawnableKrabs++; // Aumenta o número máximo de Krabs spawnados por wave
            }

            // Aumenta o tempo entre waves, mas não ultrapassa o máximo
            if (timeBetweenWaves <= maxTimeBetweenWaves)
            {
                timeBetweenWaves += timeIncrement;
                if (timeBetweenWaves > maxTimeBetweenWaves)
                {
                    timeBetweenWaves = maxTimeBetweenWaves;
                }
            }

            currentWave++;
            
            if (currentWave % 3 == 0)
            {
                EnemyRadioPrefabSpawner.instance.SpawnEnemyRadio();
            }
        }
    }
    
    public void EndGame()
    {
        StopAllCoroutines();
        waveText.text = "1";
        currentWave = 1;
        timer.Reset();
        timeBetweenWaves = initialTimeBetweenWaves;
        
        maxSpawnableKrabs = 1;
        maxSpawnableBubbles = 1;
        
        PlayerStats.ResetHearts();
        enemySpawner.ResetSpawner();
        bigBubbleSpawner.ResetSpawner();
    }
    
    
    public void RestartGame()
    {
        StopAllCoroutines();
        waveText.text = "1";
        currentWave = 1;
        timer.Reset();
        timeBetweenWaves = initialTimeBetweenWaves;
        
        maxSpawnableKrabs = 1;
        maxSpawnableBubbles = 1;
        
        PlayerStats.ResetHearts();
        enemySpawner.ResetSpawner();
        bigBubbleSpawner.ResetSpawner();
        
        StartCoroutine(WaveRoutine());
    }
}