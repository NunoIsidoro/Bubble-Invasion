using UnityEngine;

public class EnemyPoseGetter : MonoBehaviour
{
    public static EnemyPoseGetter instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // array of poses for different difficulties
    public Sprite[] easyPoses;
    public Sprite[] mediumPoses;
    public Sprite[] hardPoses;
    
    
    public Sprite GetRandomPose(EnemyDifficulty difficulty)
    {
        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                return easyPoses[Random.Range(0, easyPoses.Length)];
            case EnemyDifficulty.Medium:
                return mediumPoses[Random.Range(0, mediumPoses.Length)];
            case EnemyDifficulty.Hard:
                return hardPoses[Random.Range(0, hardPoses.Length)];
            default:
                return easyPoses[Random.Range(0, easyPoses.Length)];
        }
    }


    public Sprite GetPose(EnemyDifficulty difficulty, int id)
    {
        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                return easyPoses[id];
            case EnemyDifficulty.Medium:
                return mediumPoses[id];
            case EnemyDifficulty.Hard:
                return hardPoses[id];
            default:
                return easyPoses[id];
        }
    }
    
    
    public Sprite GetRandomEasyPose()
    {
        return easyPoses[Random.Range(0, easyPoses.Length)];
    }
}
