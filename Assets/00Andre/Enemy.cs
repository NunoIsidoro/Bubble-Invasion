using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject BubbleParent;
    public Collider2D GameArea;
    private Tween bounceTween;
    
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
        var randomY = GetRandomPositionWithinCollider().y;
        transform.DOMoveY(randomY, 1f).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(1f);

        var currentPosition = transform.position;

        bounceTween = DOTween.Sequence()
            .Append(transform.DOMoveY(currentPosition.y + 0.1f, 1f).SetEase(Ease.InOutSine))
            .Append(transform.DOMoveY(currentPosition.y, 1f).SetEase(Ease.InOutSine))
            .SetLoops(-1);
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
        // Para a animação de bounce
        if (bounceTween != null && bounceTween.IsActive())
        {
            bounceTween.Kill();
        }

        // Move o inimigo para fora da tela e destrói o objeto
        transform.DOMoveY(transform.position.y + 10f, 1f).SetEase(Ease.InOutSine);
        Destroy(gameObject, 2f);
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
