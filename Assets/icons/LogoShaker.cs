using MoreMountains.Feedbacks;
using UnityEngine;

public class LogoShaker : MonoBehaviour
{
    public MMF_Player feedback;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        feedback.PlayFeedbacks();
    }

}
