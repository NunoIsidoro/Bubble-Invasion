using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Enemy enemy;
    
    public void Change()
    {
        EnemyPoseGetter.instance.GetRandomPose(enemy.difficulty);
    }
}
