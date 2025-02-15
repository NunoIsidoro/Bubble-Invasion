using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Project.Runtime.Scripts.Core;
using Project.Runtime.Scripts.UI.Core;
using Project.Runtime.Scripts.UI.Gameplay.Components;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerStats : MonoBehaviour
{
    public ScreenLoader screenLoader;
    public GameObject HeartContainer;
    public GameObject heartPrefab;
    public List<GameObject> hearts;
    public Timer timer;
    public PlayerAttack playerAttack;
    
    public bool isDefeatPopupActive = false;
    
    public MMF_Player player_meme_take_damage_feedback;
    public Image memeImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hearts = new List<GameObject>();
        foreach (Transform child in HeartContainer.transform)
        {
            hearts.Add(child.gameObject);
        }
        
        playerAttack = GetComponent<PlayerAttack>();
    }

    // take damage, remove a heart
    public void TakeDamage()
    {
        if (Random.value < 0.75f && hearts.Count > 3)
        {
            player_meme_take_damage_feedback.PlayFeedbacks();
        }
        
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
            memeImage.GetComponent<Image>().color = new Color(1, 1, 1, 0f);
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
    
    private IEnumerator WaitAndFreezeGame()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        if (isDefeatPopupActive) return;
        
        Debug.Log("Game Over");

        // Verifica se o tempo atual é melhor que o BestTime
        string currentTime = timer.GetTime();
        string bestTime = PlayerPrefsManager.BestTime;
        
        
        Debug.Log($"Current Time: {currentTime}");
        Debug.Log($"Best Time: {bestTime}");
        Debug.Log($"Is Better Time: {IsBetterTime(currentTime, bestTime)}");
        if (IsBetterTime(currentTime, bestTime))
        {
            Debug.Log($"New Best Time: {currentTime}!");
            PlayerPrefsManager.BestTime = currentTime;
        }
        
        StartCoroutine(WaitAndFreezeGame());
        screenLoader.OnButtonShowDefeatPopup();
        isDefeatPopupActive = true;
    }

    private bool IsBetterTime(string current, string best)
    {
        if (best == "00:00") return true; // Caso padrão: sem tempo registrado ainda

        // Converte ambos os tempos para segundos para comparação
        int currentSeconds = TimeToSeconds(current);
        int bestSeconds = TimeToSeconds(best);

        return currentSeconds > bestSeconds; // Um tempo menor é melhor
    }

    private int TimeToSeconds(string time)
    {
        var parts = time.Split(':');
        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);
        return minutes * 60 + seconds;
    }

    // OnTriggerEnter2D to detect collisions with bubbles
    private float previousPlayerSpeed = 0;
    private bool recentlyTriggered = false;
    private bool recentlyTriggeredKrab = false;
    private float cooldownTime = 0.1f; // Tempo em segundos para evitar múltiplos disparos

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recentlyTriggered) return;
        
        if (collision.IsTouching(playerAttack.weaponSpawnPoint.GetComponent<Collider2D>()))
        {
            // Se o player estiver atacando, não tome dano
            return;
        }

        if (collision.CompareTag("Bubble"))
        {
            recentlyTriggered = true; // Marca que o evento foi disparado
            TakeDamage();
            Destroy(collision.gameObject);
            Debug.Log("Hit by a bubble!");
            Invoke(nameof(ResetTrigger), cooldownTime); // Reseta o controle após o cooldown
            
            try
            {
                collision.gameObject.GetComponent<SimpleBubble>().DoOnHit();
            }
            catch (Exception)
            {
            }
            
            try
            {
                collision.gameObject.GetComponent<CircularMovement>().DoOnHit();
            }
            catch (Exception)
            {
            }
        }
        else if (collision.CompareTag("PowerUpLife"))
        {
            recentlyTriggered = true; // Marca que o evento foi disparado
            AddHeart();
            Destroy(collision.gameObject);
            Invoke(nameof(ResetTrigger), cooldownTime); // Reseta o controle após o cooldown
            
            try
            {
                collision.gameObject.GetComponent<SimpleBubble>().DoOnContact();
            }
            catch (Exception)
            {
            }
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