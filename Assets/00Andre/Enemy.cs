using System.Collections;
using Project.Runtime.Scripts.Core;
using UnityEngine;

public enum EnemyDifficulty
{
    Easy,
    Medium,
    Hard
}

public class Enemy : MonoBehaviour
{
    public GameObject BubbleParent;
    public Collider2D GameArea;

    private bool isBouncing;
    private Vector3 initialPosition;
    private float bounceSpeed = 1f;
    private float bounceHeight = 0.1f;

    public SimpleBubble simpleBubble;
    public float spawnBubbleDelay;
    private EnemyDifficulty difficulty;

    public void Initialize(Collider2D gameArea, GameObject bubbleParent, int currentWave)
    {
        Debug.Log("Inimigo inicializado!");
        GameArea = gameArea;
        BubbleParent = bubbleParent;
        
        difficulty = DetermineDifficulty(currentWave);

        // Ajustar o delay de spawn de bolhas com base na dificuldade
        spawnBubbleDelay = difficulty switch
        {
            EnemyDifficulty.Easy => Random.Range(2f, 4f),
            EnemyDifficulty.Medium => Random.Range(1.5f, 3f),
            EnemyDifficulty.Hard => Random.Range(1f, 2f),
            _ => 3f
        };
        
        // switch to change the color of the enemy based on the difficulty, white easy, orange medium, red hard
        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case EnemyDifficulty.Medium:
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
                break;
            case EnemyDifficulty.Hard:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }

        spawnBubbleDelay = Random.Range(1f, 3f);
        StartCoroutine(MoveToInitialPositionAndBounce());
        StartCoroutine(SpawnBubbles());
    }

    private EnemyDifficulty DetermineDifficulty(int wave)
    {
        // Chances baseadas na wave
        float easyChance = Mathf.Clamp01(1f - (wave - 1) * 0.1f); // Chance de Easy diminui com waves
        float mediumChance = wave >= 3 ? Mathf.Clamp01((wave - 3) * 0.1f) : 0f; // Medium começa na wave 3
        float hardChance = wave >= 5 ? Mathf.Clamp01((wave - 5) * 0.1f) : 0f;   // Hard começa na wave 5

        // Normalizar chances
        float totalChance = easyChance + mediumChance + hardChance;
        easyChance /= totalChance;
        mediumChance /= totalChance;
        hardChance /= totalChance;

        // Sorteio para definir dificuldade
        float randomValue = Random.value;

        if (randomValue <= easyChance)
            return EnemyDifficulty.Easy;
        else if (randomValue <= easyChance + mediumChance)
            return EnemyDifficulty.Medium;
        else
            return EnemyDifficulty.Hard;
    }
    
    private IEnumerator MoveToInitialPositionAndBounce()
    {
        float randomY = GetRandomPositionWithinCollider().y;
        Vector3 targetPosition = new Vector3(transform.position.x, randomY, transform.position.z);

        // Move para a posição inicial de forma manual
        float moveDuration = 1f;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // Começa o movimento de bounce
        initialPosition = transform.position;
        isBouncing = true;
    }

    private void Update()
    {
        // Movimento de bounce contínuo
        if (isBouncing)
        {
            float newY = initialPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnBubbleDelay);
            
            // Define o número de bolhas a spawnar com base na dificuldade
            int bubbleCount = difficulty switch
            {
                EnemyDifficulty.Easy => 1,
                EnemyDifficulty.Medium => 2,
                EnemyDifficulty.Hard => 4,
                _ => 1
            };

            // Spawn das bolhas
            for (int i = 0; i < bubbleCount; i++)
            {
                SpawnBubble();
            }
        }
    }

    private void SpawnBubble()
    {
        var prefab = Instantiate(simpleBubble, transform.position, Quaternion.identity, BubbleParent.transform);
        prefab.Initialize(spawnBubbleDelay);
    }

    public void EndWave()
    {
        // Para o movimento de bounce
        isBouncing = false;

        // Move o inimigo para fora da tela e destrói o objeto
        StartCoroutine(MoveOutAndDestroy());
    }

    private IEnumerator MoveOutAndDestroy()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);

        float moveDuration = 1f;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private Vector2 GetRandomPositionWithinCollider()
    {
        if (GameArea == null)
        {
            Debug.LogError("SpawnArea não foi atribuído!");
            return Vector2.zero;
        }

        Bounds bounds = GameArea.bounds;

        Vector2 randomPosition;

        // Garante que a posição gerada está dentro do Collider2D
        do
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            randomPosition = new Vector2(randomX, randomY);
        }
        while (!GameArea.OverlapPoint(randomPosition)); // Verifica se a posição está dentro do Collider2D

        return randomPosition;
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBubble"))
        {
            StartCoroutine(MoveOutAndDestroy());
            Destroy(collision.gameObject);
            PlayerPrefsManager.EnemiesKilled++;
            Debug.Log("Hit by a player bubble!");
        }
    }
}
