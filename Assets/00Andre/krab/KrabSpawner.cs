using UnityEngine;

public class KrabSpawner : MonoBehaviour
{
    public GameObject krabPrefab;           // Prefab do Krab
    public GameObject leftLimit;            // Limite esquerdo da tela
    public GameObject rightLimit;           // Limite direito da tela
    public GameObject krabParent;           // Objeto pai onde os Krabs serão instanciados
    
    
    // Método para iniciar o spawn com base no número máximo de Krabs a serem gerados
    public void StartSpawning(int maxSpawnableKrabs)
    {
        for (int i = 0; i < maxSpawnableKrabs; i++)
        {
            // 50% de chance de spawnar cada Krab
            if (Random.value <= 0.5f)
            {
                SpawnKrab();
            }
        }
    }

    // Método para spawnar um Krab
    private void SpawnKrab()
    {
        // Gera posições aleatórias para o Krab dentro dos limites
        float leftLimitX = leftLimit.transform.position.x;
        float rightLimitX = rightLimit.transform.position.x;
        
        float screenHeight = Camera.main.orthographicSize * 2;
        // Faz o Krab aparecer aleatoriamente na parte inferior da tela
        float randomY = -8.32f;

        // Decide se o Krab será spawnado à esquerda ou à direita
        float randomX = Random.value > 0.5f ? leftLimitX : rightLimitX;

        // Instancia o Krab na posição aleatória e o coloca no objeto pai
        Instantiate(krabPrefab, new Vector2(randomX, randomY), Quaternion.identity, krabParent.transform);
    }

    public void ResetSpawner()
    {
        // Resetar o spawner, se necessário (não faz nada no momento)
    }
}