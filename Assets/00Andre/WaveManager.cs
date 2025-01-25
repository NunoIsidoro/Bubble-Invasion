using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public BigBubbleSpawner bigBubbleSpawner;
    public PlayerStats PlayerStats;
    public float initialTimeBetweenWaves = 5f;
    public float timeIncrement = 5f;
    public float maxTimeBetweenWaves = 60f;

    private int currentWave = 1;
    private float timeBetweenWaves; // Tempo atual entre waves
    private int maxSpawnableBubbles = 1;
    
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
            
            if (currentWave >= 3)
                bigBubbleSpawner.StartSpawning(currentWave);

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
            if (currentWave >= 3 && currentWave % 2 == 0)
            {
                maxSpawnableBubbles++;
            }

            // Aumenta o tempo entre waves, mas não ultrapassa o máximo
            if (timeBetweenWaves < maxTimeBetweenWaves)
            {
                timeBetweenWaves += timeIncrement;
                if (timeBetweenWaves > maxTimeBetweenWaves)
                {
                    timeBetweenWaves = maxTimeBetweenWaves;
                }
            }

            currentWave++;
        }
    }
    
    public void EndGame()
    {
        StopAllCoroutines();
        waveText.text = "1";
        currentWave = 1;
        timeBetweenWaves = initialTimeBetweenWaves;
        
        PlayerStats.ResetHearts();
        enemySpawner.ResetSpawner();
        bigBubbleSpawner.ResetSpawner();
    }
}