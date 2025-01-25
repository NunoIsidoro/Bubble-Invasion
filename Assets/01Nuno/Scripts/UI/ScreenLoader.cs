using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Project.Runtime.Scripts.UI.Core.Enums;
using UnityEngine;
using UnityEngine.UI;
using Screen = Project.Runtime.Scripts.UI.Core.Enums.Screen;

namespace Project.Runtime.Scripts.UI.Core
{
    public class ScreenLoader : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.5f;

        [SerializeField] private Transform _popupPanel;
        //[SerializeField] private RawImage _animatedBackground;

        [SerializedDictionary("Type", "Transform")]
        public SerializedDictionary<Screen, Transform> _screens;
        [SerializedDictionary("Type", "Transform")]
        public SerializedDictionary<Popup, Transform> _popups;

        private Transform _currentScreen;
        private readonly Stack<Transform> _popupActiveList = new();


        public void OnButtonShowGameplayScreen()
        {
            //HideAnimatedBackground();
            ShowScreen(Screen.Gameplay);
        }

        public void OnButtonShowMainMenuScreen()
        {
            //ShowAnimatedBackground();
            ShowScreen(Screen.MainMenu);
        }

        public void OnButtonShowSettingsPopup() => ShowPopup(Popup.Settings);
        public void OnButtonShowVictoryPopup() => ShowPopup(Popup.Victory);
        public void OnButtonShowDailyVictoryPopup() => ShowPopup(Popup.DailyVictory);
        public void OnButtonShowDefeatPopup() => ShowPopup(Popup.Defeat);
        public void OnButtonShowRevivePopup() => ShowPopup(Popup.Revive);
        public void OnButtonShowPausePopup() => ShowPopup(Popup.Pause);
        public void OnButtonShowConnectionFailedPopup() => ShowPopup(Popup.ConnectionFailed);

        public void OnButtonReturnFromPopup() => HideCurrentPopup();


        private void Start()
        {
            ShowScreen(Screen.MainMenu);
        }


        private void ShowScreen(Screen screen)
        {
            if (_screens == null) return;
            if (!_screens.TryGetValue(screen, out var screenTransform)) return;

            HideCurrentScreen(() =>
            {
                ActivateScreen(screenTransform);
            });
        }


        private void ShowPopup(Popup popup)
        {
            if (_popups == null) return;
            if (!_popups.TryGetValue(popup, out var popupTransform)) return;

            HideCurrentPopup(() =>
            {
                _popupActiveList.Push(popupTransform);
                ActivatePopup(popupTransform);
            });
        }


        private void HideCurrentScreen(Action onComplete = null)
        {
            if (!_currentScreen)
            {
                onComplete?.Invoke();
                return;
            }

            AnimatePopOut(_currentScreen, onComplete);
            _currentScreen = null;
        }


        private void HideCurrentPopup(Action onComplete = null)
        {
            if (_popupActiveList.Count == 0)
            {
                UpdatePopupPanelState();
                onComplete?.Invoke();
                return;
            }

            var isNewOneBeingAdded = onComplete != null;
            var currentPopup = isNewOneBeingAdded ? _popupActiveList.Peek() : _popupActiveList.Pop();
            var onCompleteToUse = isNewOneBeingAdded ? onComplete : () =>
            {

                try
                {
                    ActivatePopup(_popupActiveList.Peek());
                }
                catch (InvalidOperationException)
                {
                }
            };

            AnimatePopOut(currentPopup, () =>
            {
                UpdatePopupPanelState();
                onCompleteToUse();
            });
        }


        private void ActivateScreen(Transform screen)
        {
            screen.gameObject.SetActive(true);
            _currentScreen = screen;
            AnimatePopIn(screen);
        }


        private void ActivatePopup(Transform popup)
        {
            popup.gameObject.SetActive(true);
            AnimatePopIn(popup);
            UpdatePopupPanelState();
        }


        private void UpdatePopupPanelState()
        {
            _popupPanel.gameObject.SetActive(_popupActiveList.Count > 0);
        }


        private static void AnimatePopIn(Transform transform)
        {
            transform.DoAnimatePopIn(ANIMATION_DURATION);
        }


        private static void AnimatePopOut(Transform transform, Action onComplete = null)
        {
            transform.DoAnimatePopOut(ANIMATION_DURATION).OnComplete(() =>
            {
                transform.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }


        //private void ShowAnimatedBackground() => _animatedBackground.DoAnimateFade(0, 20 / 255f, ANIMATION_DURATION);
        //private void HideAnimatedBackground() => _animatedBackground.DoAnimateFade(20 / 255f, 0, ANIMATION_DURATION);
    }
}
