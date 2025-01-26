using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleBubble : MonoBehaviour
{
    private Rigidbody2D rb;
    public MMF_Player bubbleContactFeedback;
    public MMF_Player bubbleHitFeedback;

    public void Initialize(float launchDelay)
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado! Certifique-se de que este objeto tem um componente Rigidbody2D.");
            return;
        }

        float randomSize = Mathf.Lerp(0.25f, 1.25f, launchDelay / 3f);
        transform.localScale = new Vector3(randomSize, randomSize, 1f);
        
        // float randomAngle = Random.Range(-45f, 45f);
        float launchForce = Mathf.Lerp(15f, 3f, launchDelay / 3f);  // Maior delay, menor força
        float randomAngle = Random.Range(0f, 360f);
        float radians = randomAngle * Mathf.Deg2Rad;
        Vector2 launchDirection = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
        rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
    }

    
    private void Update()
    {
        if (transform.position.y < -20f)
            Destroy(gameObject);
    }
    
    
    public void DoOnContact()
    {
        bubbleContactFeedback.PlayFeedbacks();
    }
    
    public void DoOnHit()
    {
        bubbleHitFeedback.PlayFeedbacks();
    }
}
