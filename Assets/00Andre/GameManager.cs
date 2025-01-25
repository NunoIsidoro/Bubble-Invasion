using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerStats PlayerStats;
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
        StartCoroutine(WaitAndFreezeGame());
    }
    
    
    public void ResumeGame()
    {
        // unfreeze game
        Time.timeScale = 1;
    }
    
    
    private IEnumerator WaitAndFreezeGame()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }
    
    
    public void LeaveGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        /*
        WaveManager.EndGame();
        Game.SetActive(false);
        */
    }


    public void TryAgain()
    {
        Time.timeScale = 1;
        // remove enemies
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
        
        //reset wave
        WaveManager.EndGame();
        
        // reset hearts
        PlayerStats.transform.position = new Vector3(0, 15.96f, 0);
        PlayerStats.ResetHearts();
        PlayerStats.isDefeatPopupActive = false;
    }
    
    
    public void QuitApp()
    {
        Application.Quit();
    }
}
