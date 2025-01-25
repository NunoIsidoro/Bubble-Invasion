using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public float speed; // Velocidade horizontal
    public float amplitude; // Amplitude da onda (distância para cima e para baixo)
    public float frequency; // Frequência da onda (quantas vezes ela oscila por unidade de tempo)
    public float horizontalOffset = 0f; // Distância extra fora da tela
    public float chanceIncrease; // Aumento da chance de destruição a cada passagem (25%)
    public float threshold = 0.1f; // Tolerância para considerar que o objeto atingiu um limite (pode ser ajustado)
    public float rotationSpeed = -100f; // Velocidade de rotação da bola (inverte a rotação com sinal negativo)

    private float startPosX; // Posição inicial horizontal
    private float startPosY; // Posição inicial horizontal
    private bool isRightSide; // Variável para determinar se começa à direita
    private float leftLimit; // Posição do limite esquerdo
    private float rightLimit; // Posição do limite direito
    private int hitCount = 0; // Contador de quantas vezes o objeto atingiu um limite
    private bool hasBeenDestroyed = false; // Flag para impedir destruição repetida

    void Start()
    {
        amplitude = Random.Range(2f, 5f); // Amplitude aleatória
        frequency = Random.Range(2f, 5f); // Frequência aleatória

        // Obtém as posições dos limites esquerdo e direito
        leftLimit = GameObject.Find("left").transform.position.x;
        rightLimit = GameObject.Find("right").transform.position.x;

        // Inicializa a posição inicial aleatoriamente na ponta esquerda ou direita com o offset
        isRightSide = Random.Range(-1f, 1f) > 0; // Se for maior que 0, começa à direita

        // Definindo o valor inicial da posição horizontal com o offset
        startPosX = isRightSide ? rightLimit + horizontalOffset : leftLimit - horizontalOffset;

        // Obtém os limites superior e inferior da tela
        startPosY = transform.position.y;
        transform.position = new Vector2(startPosX, startPosY);
    }

    void Update()
    {
        // Calcula a direção correta para o movimento horizontal
        float direction = isRightSide ? -1f : 1f;  // Se começar à direita, vai para a direita (1); se começar à esquerda, vai para a esquerda (-1)

        // Atualiza a posição horizontal com base no movimento desejado
        float newX = Mathf.PingPong(Time.time * speed, rightLimit - leftLimit) + leftLimit;

        // Calcula a nova posição vertical com base na função seno
        float newY = startPosY + Mathf.Sin(Time.time * frequency) * amplitude;

        // Atualiza a posição do círculo
        transform.position = new Vector2(newX, newY);

        // Rotaciona o objeto continuamente no sentido oposto
        transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime); // Rotação contínua ao contrário

        // Verifica se o objeto atingiu o limite esquerdo ou direito, considerando a tolerância (threshold)
        if (!hasBeenDestroyed)
        {
            if ((newX <= leftLimit + threshold || newX >= rightLimit - threshold)) // Verificação com threshold
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
