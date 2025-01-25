using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Game;
    public GameObject GameInstances;
    public WaveManager WaveManager;
    
    public void StartGame()
    {
        Game.SetActive(true);
    }
    
    
    public void PauseGame()
    {
        // freeze game
        Time.timeScale = 0;
    }
    
    
    public void ResumeGame()
    {
        // unfreeze game
        Time.timeScale = 1;
    }
    
    
    public void LeaveGame()
    {
        //SceneManager.LoadScene(0);
        foreach (Transform child in GameInstances.transform)
        {
            try
            {
                Destroy(child.gameObject);
            }
            catch (Exception)
            {
            }
        }
        WaveManager.EndGame();
        Game.SetActive(false);
    }
    
    
    public void QuitApp()
    {
        Application.Quit();
    }
}
