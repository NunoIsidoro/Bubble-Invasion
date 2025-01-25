using System;
using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject meleeWeaponPrefab; // Prefab for the melee weapon hitbox or effect
    public Transform weaponSpawnPoint;  // Point from where the melee attack will originate
    public float attackRange;    // Range of the melee attack
    public float attackDuration; // How long the attack lasts
    public float attackCooldown = 1.0f; // Time between attacks
    public float knockbackForce = 10f;  // Force applied to bubbles when hit
    public LayerMask enemyLayers;       // Layers that are considered "enemies" (including bubbles)

    private float attackTimer;          // Timer to track the cooldown
    public bool isAttacking;           // Flag to indicate if an attack is ongoing
    private float attackVisualTimer;    // Timer to track how long the visual stays
    public float attackRadius = 1f; // Radius of the attack area

    public Animator animator;

    public Collider2D attackCollider; // Collider for the attack hitbox

    void Start()
    {
        attackTimer = 0f;
        isAttacking = false;

        // Create or find the attack collider (e.g., a child collider in the meleeWeaponPrefab)
        attackCollider = meleeWeaponPrefab.GetComponent<Collider2D>();
        if (attackCollider != null)
        {
            // Initially disable the collider so it doesn't interfere when not attacking
            attackCollider.enabled = false;
        }
    }

    void Update()
    {
        // Update cooldown timer
        attackTimer -= Time.deltaTime;

        // Reduce visual timer
        if (isAttacking)
        {
            attackVisualTimer -= Time.deltaTime;
            if (attackVisualTimer <= 0f)
            {
                isAttacking = false; // Turn off attack visualization
                // Disable the collider once the attack is over
                if (attackCollider != null)
                {
                    attackCollider.enabled = false;
                }
            }
        }

        // Detect input for attack
        if (Input.GetButtonDown("Fire1") && attackTimer <= 0f) // Fire1 is typically left mouse button
        {
            PerformAttack();
            attackTimer = attackCooldown; // Reset the cooldown timer
        }
    }

    void PerformAttack()
    {
        Debug.Log("Attack fired! Checking for hits...");

        // Start visualizing attack area
        isAttacking = true;
        attackVisualTimer = attackDuration;

        // Trigger the attack animation
        animator.SetTrigger("Attack");

        // Enable the collider to start detecting hits
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }

        // You could also instantiate the meleeWeaponPrefab here to represent the weapon visually
        // Instantiate(meleeWeaponPrefab, weaponSpawnPoint.position, Quaternion.identity);
    }

    // This function will be called whenever a collider enters the attack area
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAttacking) return;
        
        // Check if the object is in the enemy layer
        if ((enemyLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log($"Hit object: {other.name}");

            // Apply knockback force to the object if it has a Rigidbody2D
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate direction from the object to the spawn point
                Vector2 forceDirection = (other.transform.position - weaponSpawnPoint.position).normalized;

                // Optionally, you can add more upward force as needed
                forceDirection = new Vector2(forceDirection.x, Mathf.Abs(forceDirection.y)); // Ensure upward knockback

                rb.linearVelocity = Vector2.zero; // Reset velocity to ensure consistent knockback
                rb.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);

                Debug.Log($"Knockback applied to {other.name}");
            }

            // If the object is a bubble, destroy it after a delay
            if (other.CompareTag("Bubble")) // Ensure your bubbles have a "Bubble" tag
            {
                // Change tag to PlayerBubble
                other.tag = "PlayerBubble";
                StartCoroutine(DestroyBubbleAfterDelay(other.gameObject, 3f)); // Destroy after 3 seconds
            }
        }
    }

    // Coroutine to destroy the bubble after a delay
    IEnumerator DestroyBubbleAfterDelay(GameObject bubble, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay time
        try
        {
            Debug.Log($"Destroying {bubble.name} after {delay} seconds.");
            Destroy(bubble); // Destroy the bubble
        }
        catch (Exception)
        {
        }
    }

    void OnDrawGizmos()
    {
        if (weaponSpawnPoint == null)
            return;

        // Draw the attack range as a red circle
        if (isAttacking) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(weaponSpawnPoint.position, attackRange); // Visualize attack range (somente aqui)
        }
    }
}
