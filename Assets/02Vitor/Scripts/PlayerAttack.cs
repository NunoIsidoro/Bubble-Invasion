using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject meleeWeaponPrefab; // Prefab for the melee weapon hitbox or effect
    public Transform weaponSpawnPoint;  // Point from where the melee attack will originate
    public float attackRange = 1.5f;    // Range of the melee attack
    public float attackDuration = 0.2f; // How long the attack lasts
    public float attackCooldown = 1.0f; // Time between attacks
    public LayerMask enemyLayers;       // Layers that are considered "enemies"

    private Camera mainCamera;
    private Vector3 lastHitPosition;    // Store the last hit position
    private bool showHitGizmo;         // Whether to show the hit gizmo
    private float hitGizmoTimer;       // Timer for how long to show the gizmo
    private float attackTimer;         // Timer to track the cooldown

    void Start()
    {
        mainCamera = Camera.main;
        attackTimer = 0f; // Initialize the cooldown timer
    }

    void Update()
    {
        // Update the cooldown timer
        attackTimer -= Time.deltaTime;

        // Detect input for attack
        if (Input.GetButtonDown("Fire1") && attackTimer <= 0f) // Fire1 is typically left mouse button
        {
            PerformAttack();
            attackTimer = attackCooldown; // Reset the cooldown timer
        }

        // Countdown the gizmo timer
        if (showHitGizmo)
        {
            hitGizmoTimer -= Time.deltaTime;
            if (hitGizmoTimer <= 0f)
            {
                showHitGizmo = false;
            }
        }
    }

    void PerformAttack()
    {
        // Get mouse position in the world
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure it's in 2D space

        // Calculate attack direction
        Vector3 attackDirection = (mousePosition - transform.position).normalized;

        // Rotate the weapon spawn point to face the attack direction
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        weaponSpawnPoint.rotation = Quaternion.Euler(0, 0, angle);

        // Spawn a melee weapon hitbox or effect
        GameObject weapon = Instantiate(meleeWeaponPrefab, weaponSpawnPoint.position, weaponSpawnPoint.rotation);
        Destroy(weapon, attackDuration);

        // Detect enemies in the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(weaponSpawnPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.name);
            // Add logic here to damage the enemy
        }

        // Set the hit gizmo to show
        lastHitPosition = weaponSpawnPoint.position + attackDirection * attackRange;
        showHitGizmo = true;
        hitGizmoTimer = attackDuration;
    }

    void OnDrawGizmos()
    {
        if (weaponSpawnPoint == null)
            return;

        // Draw the attack range at the weapon spawn point
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponSpawnPoint.position, attackRange);

        // Draw the last hit position when the gizmo is active
        if (showHitGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastHitPosition, 0.2f); // Adjust the size of the sphere if needed
        }
    }
}
