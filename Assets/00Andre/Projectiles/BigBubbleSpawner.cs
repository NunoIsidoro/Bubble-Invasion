using UnityEngine;

public class BigBubbleSpawner : MonoBehaviour
{
    public GameObject bigBubbleParent;
    public CircularMovement bigBubblePrefab;  // Prefab da bolha
    public GameObject leftLimit;              // Limite esquerdo da tela
    public GameObject rightLimit;             // Limite direito da tela

    // Método chamado para spawnar bolhas com base na capacidade máxima da wave
    public void StartSpawning(int maxSpawnableBubbles)
    {
        for (int i = 0; i < maxSpawnableBubbles; i++)
        {
            // 50% de chance de spawnar cada bolha
            if (Random.value <= 0.5f)
            {
                SpawnBigBubble();
            }
        }
    }

    // Método para spawnar uma bolha
    private void SpawnBigBubble()
    {
        float leftLimitX = leftLimit.transform.position.x;
        float rightLimitX = rightLimit.transform.position.x;

        float screenHeight = Camera.main.orthographicSize * 2;
        // make randomY spawn in the bottom half of the screen
        float randomY = Random.Range((-screenHeight / 2) + 3f, 0f);
        float randomX = Random.value > 0.5f ? leftLimitX : rightLimitX;

        Instantiate(bigBubblePrefab, new Vector2(randomX, randomY), Quaternion.identity, bigBubbleParent.transform);
    }
    
    public void ResetSpawner()
    {
        // nada
    }
}