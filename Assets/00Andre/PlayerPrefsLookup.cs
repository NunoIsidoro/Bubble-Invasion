using Project.Runtime.Scripts.Core;
using TMPro;
using UnityEngine;

public class PlayerPrefsLookup : MonoBehaviour
{
    public string BestTimeName;
    public TextMeshProUGUI BestTimeTarget;
    
    public string EnemiesKilledName;
    public TextMeshProUGUI EnemiesKilledTarget;
    
    
    void Update()
    {
        BestTimeTarget.text = $"{BestTimeName}\n{PlayerPrefsManager.BestTime}";
        EnemiesKilledTarget.text = $"{EnemiesKilledName}\n{PlayerPrefsManager.EnemiesKilled}";
    }
}
