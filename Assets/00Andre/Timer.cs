using TMPro;
using UnityEngine;

namespace Project.Runtime.Scripts.UI.Gameplay.Components
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private float _timeElapsed;
        private bool _isPaused;


        private void OnEnable()
        {
            Reset();
        }


        private void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
        }


        private void Update()
        {
            if (_isPaused) return;
            
            _timeElapsed += Time.deltaTime;

            var minutes = Mathf.FloorToInt(_timeElapsed / 60);
            var seconds = Mathf.FloorToInt(_timeElapsed % 60);

            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
        
        
        public string GetTime()
        {
            var minutes = Mathf.FloorToInt(_timeElapsed / 60);
            var seconds = Mathf.FloorToInt(_timeElapsed % 60);

            return $"{minutes:00}:{seconds:00}";
        }
        
        
        public void Pause() => _isPaused = true;
        public void Resume() => _isPaused = false;

        public void Reset()
        {
            _isPaused = false;
            _timeElapsed = 0;
        }
    }
}