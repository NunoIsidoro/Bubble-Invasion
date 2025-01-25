using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jump;

    private float Move;

    public Rigidbody2D rb;
    public Animator animator;

    public bool isJumping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        
        rb.linearVelocity = new Vector2(speed * Move, rb.linearVelocity.y);


        // Update animator parameter to indicate running status
        animator.SetBool("IsRunning", Mathf.Abs(Move) > 0.1f);

        // Jumping
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            isJumping = true;
            // Set the IsJumping parameter (if you have one for jump animation)
            animator.SetBool("IsJumping", true);
        }

        FlipX(Move);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
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
