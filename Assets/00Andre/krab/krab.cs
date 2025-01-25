using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krab : MonoBehaviour
{
    public float speed = 5f;  // Velocidade do movimento
    public Vector2 direction = Vector2.right;  // Direção inicial (para a direita)
    public float initialOffset = 0.5f;  // Distância inicial da borda da tela
    public float minTimeToStop = 5f;  // Tempo mínimo para parar (5 segundos)
    public float maxTimeToStop = 20f;  // Tempo máximo para parar (20 segundos)
    public float downwardSpeed = 2f;  // Velocidade de movimento para baixo

    private Vector2 screenBounds;
    private bool movingDown = false;  // Controla se o círculo já deve se mover para baixo
    private bool stopMoving = false;

    public void StayStill(PlayerStats p)
    {
        stopMoving = true;
        StartCoroutine(EnableMovement(p));
    }
    
    // ienumerator disableMovement for 3 seconds and then set time as completed
    private IEnumerator EnableMovement(PlayerStats p)
    {
        yield return new WaitForSeconds(3f);
        stopMoving = false;
        movingDown = true;
        p.ResumeMovement();
    }
    
    
    void Start()
    {
        // Define os limites da tela (borda)
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Ajusta a posição inicial com o offset
        if (direction.x > 0)
        {
            transform.position = new Vector2(-screenBounds.x + initialOffset, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(screenBounds.x - initialOffset, transform.position.y);
        }

        // Gera um tempo aleatório entre minTimeToStop e maxTimeToStop
        float randomTimeToStop = Random.Range(minTimeToStop, maxTimeToStop);

        // Começa a contagem de tempo para parar o movimento
        Invoke("StopHorizontalMovement", randomTimeToStop);
    }

    void Update()
    {
        if (stopMoving)
        {
            return;
        }
        
        if (!movingDown)
        {
            // Move o círculo de acordo com a direção e a velocidade
            transform.Translate(direction * speed * Time.deltaTime);

            // Verifica se o círculo bateu na borda direita ou esquerda da tela
            if (transform.position.x >= screenBounds.x || transform.position.x <= -screenBounds.x)
            {
                // Inverte a direção horizontal quando atingir a borda
                direction.x = -direction.x;
            }
        }
        else
        {
            // Após o tempo de 10 segundos, move o círculo para baixo
            transform.Translate(Vector2.down * downwardSpeed * Time.deltaTime);

            // Se o círculo atingir a parte inferior da tela ou qualquer condição para desaparecer, destruímos o objeto
            if (transform.position.y <= -screenBounds.y)
            {
                Destroy(gameObject);  // Destrói o GameObject (faz o círculo desaparecer)
            }
        }
    }

    // Método chamado após o tempo aleatório para parar o movimento horizontal
    void StopHorizontalMovement()
    {
        direction = Vector2.zero;  // Para o movimento horizontal
        movingDown = true;  // Começa o movimento para baixo
    }
}
