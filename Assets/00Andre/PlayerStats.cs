using System;
using System.Collections;
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
    
    public int GetHeartsCount()
    {
        return hearts.Count;
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
    private float previousPlayerSpeed = 0;
    private bool recentlyTriggered = false;
    private bool recentlyTriggeredKrab = false;
    private float cooldownTime = 0.1f; // Tempo em segundos para evitar múltiplos disparos

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recentlyTriggered) return; // Ignora se já foi disparado recentemente

        if (collision.CompareTag("Bubble"))
        {
            recentlyTriggered = true; // Marca que o evento foi disparado
            TakeDamage();
            Destroy(collision.gameObject);
            Debug.Log("Hit by a bubble!");
            Invoke(nameof(ResetTrigger), cooldownTime); // Reseta o controle após o cooldown
        }
        else if (collision.CompareTag("PowerUpLife"))
        {
            recentlyTriggered = true; // Marca que o evento foi disparado
            AddHeart();
            Destroy(collision.gameObject);
            Invoke(nameof(ResetTrigger), cooldownTime); // Reseta o controle após o cooldown
        }
        else if (collision.CompareTag("krab"))
        {
            if (recentlyTriggeredKrab) return; // Ignora se já foi disparado recentemente
            recentlyTriggeredKrab = true;
            
            // disable movement
            previousPlayerSpeed = GetComponent<PlayerMovement>().speed;
            GetComponent<PlayerMovement>().speed = 0;
            collision.GetComponent<krab>().StayStill(this);
        }
    }

    public void ResumeMovement()
    {
        recentlyTriggeredKrab = false;
        GetComponent<PlayerMovement>().speed = previousPlayerSpeed;
    }
    

    private void ResetTrigger()
    {
        recentlyTriggered = false; // Libera novamente para novas colisões
    }
}