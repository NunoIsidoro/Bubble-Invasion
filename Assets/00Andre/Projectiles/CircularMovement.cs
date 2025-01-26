using MoreMountains.Feedbacks;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public float speed; // Velocidade horizontal
    public float amplitude; // Amplitude da onda (distância para cima e para baixo)
    public float frequency; // Frequência da onda (quantas vezes ela oscila por unidade de tempo)
    public float horizontalOffset = 0f; // Distância extra fora da tela
    public float rotationSpeed = -100f; // Velocidade de rotação da bola (inverte a rotação com sinal negativo)
    public float threshold = 0.1f; // Tolerância para considerar que o objeto atingiu um limite

    private float startPosY; // Posição inicial vertical
    private float leftLimit; // Posição do limite esquerdo
    private float rightLimit; // Posição do limite direito
    private int hitCount = 0; // Contador de quantas vezes o objeto atingiu um limite
    public float chanceIncrease; // Aumento da chance de destruição a cada passagem (25%)
    private bool hasBeenDestroyed = false; // Flag para impedir destruição repetida

    private float creationTime; // Armazena o tempo desde a criação do objeto
    private int movementDirection; // 1 para iniciar à direita, -1 para iniciar à esquerda

    
    public MMF_Player bubbleHitFeedback;
    
    
    public void DoOnHit()
    {
        bubbleHitFeedback.PlayFeedbacks();
    }
    
    
    void Start()
    {
        amplitude = Random.Range(2f, 5f); // Amplitude aleatória
        frequency = Random.Range(2f, 5f); // Frequência aleatória

        // Obtém as posições dos limites esquerdo e direito
        leftLimit = GameObject.Find("left").transform.position.x;
        rightLimit = GameObject.Find("right").transform.position.x;

        // Define a direção inicial aleatória (50% de chance)
        movementDirection = Random.value > 0.5f ? 1 : -1;

        // Inicializa a posição inicial com base na direção escolhida
        float startPosX = movementDirection == 1 ? leftLimit : rightLimit;
        startPosY = transform.position.y;
        transform.position = new Vector2(startPosX, startPosY);

        // Armazena o tempo de criação do objeto
        creationTime = Time.time;
    }

    void Update()
    {
        // Calcula o deslocamento horizontal com o tempo desde a criação, ajustado pela direção
        float newX = Mathf.PingPong((Time.time - creationTime) * speed, rightLimit - leftLimit) + leftLimit;

        // Se o objeto começa à direita, invertemos o cálculo de `newX`
        if (movementDirection == -1)
        {
            newX = rightLimit - Mathf.PingPong((Time.time - creationTime) * speed, rightLimit - leftLimit);
        }

        // Calcula a nova posição vertical com base na função seno, usando o tempo desde a criação
        float newY = startPosY + Mathf.Sin((Time.time - creationTime) * frequency) * amplitude;

        // Atualiza a posição do círculo
        transform.position = new Vector2(newX, newY);

        // Rotaciona o objeto continuamente
        transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

        // Verifica se o objeto atingiu o limite esquerdo ou direito, considerando a tolerância (threshold)
        if (!hasBeenDestroyed)
        {
            // Verifica se o objeto atingiu o limite esquerdo ou direito, considerando a tolerância
            if (Mathf.Abs(newX - leftLimit) <= threshold || Mathf.Abs(newX - rightLimit) <= threshold)
            {
                HandleLimitHit();
            }
        }
    }

    // Lida com o evento de o objeto atingir um limite
    private void HandleLimitHit()
    {
        // Aumenta o contador de passes pelo limite
        hitCount++;

        // Calcula a chance de destruição com base no contador
        float destructionChance = hitCount * chanceIncrease;
        Debug.Log($"Chance de destruição: {destructionChance:P}");

        // Se a chance for maior ou igual a 100%, destrói o objeto
        if (Random.value <= destructionChance)
        {
            Destroy(gameObject); // Destrói o objeto
            hasBeenDestroyed = true; // Marca que o objeto foi destruído
        }
    }
}
