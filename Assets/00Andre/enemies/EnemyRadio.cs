using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyRadio : MonoBehaviour
{
    public MMF_Player musicFeedback;
    public MMF_Player idleFeedback;

    public float speed = 2f; // Velocidade do movimento
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private bool isMoving;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(120f, 0f, 0f); // Define a posição alvo
    }
    
    void OnEnable()
    {
        idleFeedback.PlayFeedbacks(); // Inicia o feedback de idle
    }

    void Update()
    {
        // Verifica se o objeto ainda não chegou ao destino
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            if (!isMoving)
            {
                isMoving = true;
                musicFeedback.PlayFeedbacks(); // Inicia o feedback de música
            }

            // Move o objeto em direção ao alvo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if (isMoving)
        {
            // Para o movimento e alterna para o feedback de idle quando atingir o alvo
            isMoving = false;
            musicFeedback.StopFeedbacks(); // Para o feedback de música
            EnemyRadioPrefabSpawner.instance.DeactivateEnemyRadio();
        }
    }
}
