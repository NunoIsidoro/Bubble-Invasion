using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jump;

    private float Move;

    public Rigidbody2D rb;

    public bool isJumping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");

        // Disable horizontal movement while jumping
        if (!isJumping)
        {
            rb.linearVelocity = new Vector2(speed * Move, rb.linearVelocity.y);
        }

        // Jump logic
        if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jump));
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
