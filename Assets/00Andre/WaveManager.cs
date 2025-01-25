using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public float initialTimeBetweenWaves = 5f;
    public float timeIncrement = 5f;
    public float maxTimeBetweenWaves = 60f;

    private int currentWave = 1;
    private float timeBetweenWaves; // Tempo atual entre waves

    private void Start()
    {
        timeBetweenWaves = initialTimeBetweenWaves;
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (true)
        {
            Debug.Log($"Iniciando Wave {currentWave}.");
            enemySpawner.StartSpawning(currentWave);

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

            currentWave++;
        }
    }
}