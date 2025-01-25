using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Project.Runtime.Scripts.UI.Core
{
    public class FPSHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _hudRefreshRate = 1f;

        private float _timer;
        private int _frameCounter;
        private float _fps;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }


        private async void Start()
        {
            await Task.Delay(1000);
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }


        private void Update()
        {
            _timer += Time.deltaTime;
            _frameCounter++;

            if (!(_timer >= _hudRefreshRate)) return;

            _fps = _frameCounter / _timer;

            _timer = 0;
            _frameCounter = 0;
        }
    }
}