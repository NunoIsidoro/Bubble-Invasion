using Project.Runtime.Scripts.UI.Gameplay.Components;
using TMPro;
using UnityEngine;

public class TimerLookup : MonoBehaviour
{
    public string TimerName;
    public TextMeshProUGUI Target;
    public Timer timer;

    // Update is called once per frame
    void Update()
    {
        Target.text = $"{TimerName}\n{timer.GetTime()}";
    }
}
