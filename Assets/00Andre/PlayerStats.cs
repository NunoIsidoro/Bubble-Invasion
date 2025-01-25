using System;
using System.Collections.Generic;
using Project.Runtime.Scripts.UI.Core;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public ScreenLoader screenLoader;
    public GameObject HeartContainer;
    public GameObject heartPrefab;
    public List<GameObject> hearts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hearts = new List<GameObject>();
        foreach (Transform child in HeartContainer.transform)
        {
            hearts.Add(child.gameObject);
        }
    }

    // take damage, remove a heart
    public void TakeDamage()
    {
        if (hearts.Count > 1)
        {
            Destroy(hearts[hearts.Count - 1]);
            hearts.RemoveAt(hearts.Count - 1);
        }
        else
        {
            try
            {
                Destroy(hearts[hearts.Count - 1]);
                hearts.RemoveAt(hearts.Count - 1);
            }
            catch (Exception)
            {
            }
            Debug.Log("Game Over");
            GameOver();
        }
    }

    public void AddHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, HeartContainer.transform);
        hearts.Add(newHeart);
    }

    public void ResetHearts()
    {
        foreach (Transform child in HeartContainer.transform)
        {
            Destroy(child.gameObject);
        }
        hearts = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            AddHeart();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        screenLoader.OnButtonShowDefeatPopup();
    }

    // OnTriggerEnter2D to detect collisions with bubbles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a "Bubble" tag or a component indicating it's a bubble
        if (collision.CompareTag("Bubble"))
        {
            // Optional: Destroy the bubble after the collision
            Destroy(collision.gameObject);
            
            Debug.Log("Hit by a bubble!");
            TakeDamage();
        }
    }
}