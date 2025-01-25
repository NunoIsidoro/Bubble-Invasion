using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jump;

    private float Move;

    public Rigidbody2D rb;
    public Animator animator;

    public bool isJumping;
    public bool isFalling;
    
    public MMF_Player player_walk_feedback;

    // Update is called once per frame
    void Update()
    {
        // Movimentação horizontal
        Move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(speed * Move, rb.linearVelocity.y);

        // Atualiza o parâmetro do Animator para indicar se está correndo
        bool isRunning = Mathf.Abs(Move) > 0.1f;
        animator.SetBool("IsRunning", isRunning);

        // Toca ou para o feedback do som de passos
        if (isRunning && !isJumping && !player_walk_feedback.IsPlaying)
        {
            player_walk_feedback.PlayFeedbacks();
        }
        else if (!isRunning || isJumping)
        {
            player_walk_feedback.StopFeedbacks();
        }

        // Detecta e executa o salto
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            isJumping = true;
            isFalling = false;

            // Ativa a animação de salto
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);

            // Para o som de passos durante o salto
            player_walk_feedback.StopFeedbacks();
        }

        // Detecta se o jogador está caindo
        if (rb.linearVelocity.y < 0 && isJumping)
        {
            isFalling = true;
            animator.SetBool("IsFalling", true);
        }

        FlipX(Move);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isFalling = false;

            // Reseta os parâmetros de salto e queda no Animator
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;

            // Para o som de passos ao sair do chão
            player_walk_feedback.StopFeedbacks();
        }
    }

    private void FlipX(float x)
    {
        if (x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(x), 1);
        }
    }
}
