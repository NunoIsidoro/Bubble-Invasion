using System.Collections;
using UnityEngine;

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

    public void Initialize(Collider2D gameArea, GameObject bubbleParent)
    {
        Debug.Log("Inimigo inicializado!");
        GameArea = gameArea;
        BubbleParent = bubbleParent;

        spawnBubbleDelay = Random.Range(1f, 3f);
        StartCoroutine(MoveToInitialPositionAndBounce());
        StartCoroutine(SpawnBubbles());
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
            SpawnBubble();
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
}
