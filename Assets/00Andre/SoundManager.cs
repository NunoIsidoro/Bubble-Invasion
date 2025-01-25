using MoreMountains.Feedbacks;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public MMF_Player backgroundMusic;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundMusic.PlayFeedbacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
