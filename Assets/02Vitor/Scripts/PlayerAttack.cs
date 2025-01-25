using System;
using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject meleeWeaponPrefab; // Prefab for the melee weapon hitbox or effect
    public Transform weaponSpawnPoint;  // Point from where the melee attack will originate
    public float attackRange = 1.5f;    // Range of the melee attack
    public float attackDuration = 0.2f; // How long the attack lasts
    public float attackCooldown = 1.0f; // Time between attacks
    public float knockbackForce = 10f;  // Force applied to bubbles when hit
    public LayerMask enemyLayers;       // Layers that are considered "enemies" (including bubbles)

    private float attackTimer;          // Timer to track the cooldown
    private bool isAttacking;           // Flag to indicate if an attack is ongoing
    private float attackVisualTimer;    // Timer to track how long the visual stays
    public float attackRadius = 1f; // Radius of the attack area

    private Animator animator;

    void Start()
    {
        attackTimer = 0f;
        isAttacking = false;

        // Get the Animator component from the player
        animator = GetComponent<Animator>();
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

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the Z-coordinate is zero for 2D gameplay

        // Calculate the direction and distance to the mouse position
        Vector3 attackDirection = (mousePosition - weaponSpawnPoint.position).normalized;
        float distanceToMouse = Vector3.Distance(weaponSpawnPoint.position, mousePosition);

        // Clamp the distance to the maximum attack range
        Vector3 clampedPosition = weaponSpawnPoint.position + attackDirection * Mathf.Min(distanceToMouse, attackRange);

        // Detect enemies within the customizable attack radius
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(clampedPosition, attackRadius, enemyLayers);
        Debug.Log($"Detected {hitObjects.Length} objects in range.");

        // Spawn the attack circle for visualization
        GameObject weapon = Instantiate(meleeWeaponPrefab, clampedPosition, Quaternion.identity);
        Destroy(weapon, attackDuration);

        // Apply effects to all detected objects
        foreach (Collider2D obj in hitObjects)
        {
            Debug.Log($"Hit object: {obj.name}");

            // Apply knockback force to the object if it has a Rigidbody2D
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate direction from the bubble to the mouse position
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0; // Ensure we're working in 2D

                // Calculate direction to mouse
                Vector2 forceDirection = (mouseWorldPos - obj.transform.position).normalized;

                // Optionally, you can add more upward force as needed
                forceDirection = new Vector2(forceDirection.x, Mathf.Abs(forceDirection.y)); // Ensure upward knockback

                rb.linearVelocity = Vector2.zero; // Reset velocity to ensure consistent knockback
                rb.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);

                Debug.Log($"Knockback applied to {obj.name}");
            }

            // If the object is a bubble, destroy it after a delay
            if (obj.CompareTag("Bubble")) // Ensure your bubbles have a "Bubble" tag
            {
                StartCoroutine(DestroyBubbleAfterDelay(obj.gameObject, 3f)); // Destroy after 3 seconds
            }
        }
    }


    // Coroutine to destroy the bubble after a delay
    IEnumerator DestroyBubbleAfterDelay(GameObject bubble, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay time
        Debug.Log($"Destroying {bubble.name} after {delay} seconds.");
        try
        {
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

        // Draw the maximum attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponSpawnPoint.position, attackRange);

        // Draw the attack radius if attacking
        if (isAttacking)
        {
            Gizmos.color = Color.yellow;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 attackDirection = (mousePosition - weaponSpawnPoint.position).normalized;
            float distanceToMouse = Vector3.Distance(weaponSpawnPoint.position, mousePosition);
            Vector3 clampedPosition = weaponSpawnPoint.position + attackDirection * Mathf.Min(distanceToMouse, attackRange);

            Gizmos.DrawWireSphere(clampedPosition, attackRadius);
        }
    }


}
