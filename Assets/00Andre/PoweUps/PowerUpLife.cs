using UnityEngine;

public class PowerUpLife : MonoBehaviour
{
    public float riseSpeed = 2f; // Velocidade de subida
    public float amplitude = 0.5f; // Amplitude da oscilação lateral
    public float frequency = 2f; // Frequência da oscilação lateral
    public float destroyHeight = 20f; // Altura em que o objeto será destruído

    private float startPosX; // Posição inicial no eixo X
    private float startPosY; // Posição inicial no eixo Y

    void Start()
    {
        // Salva a posição inicial
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }

    void Update()
    {
        // Movimento vertical (subida constante)
        float newY = transform.position.y + riseSpeed * Time.deltaTime;

        // Oscilação horizontal (movimento de onda no eixo X)
        float newX = startPosX + Mathf.Sin(Time.time * frequency) * amplitude;

        // Atualiza a posição do objeto
        transform.position = new Vector2(newX, newY);

        // Destrói o objeto quando atingir uma altura máxima
        if (transform.position.y >= destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
