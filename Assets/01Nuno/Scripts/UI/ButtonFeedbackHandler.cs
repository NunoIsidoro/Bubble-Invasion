using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFeedbackHandler : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private MMF_Player _clickFeedback;
    [SerializeField] private MMF_Player _pointerEnterFeedback;
    [SerializeField] private MMF_Player _idleFeedback;
    [SerializeField] private bool _autoPointerEnterFeedback;
    [SerializeField] private float _autoPointerEnterFeedbackDelay;


    private void Awake()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        if (button == null) return;
        button.onClick.AddListener(() =>
        {
            _clickFeedback?.PlayFeedbacks();
        });

        _idleFeedback?.PlayFeedbacks();
    }


    private void OnEnable()
    {
        if (_autoPointerEnterFeedback)
            StartCoroutine(AutoPointerEnterFeedbackCoroutine());
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerEnterFeedback?.PlayFeedbacks();
    }


    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator AutoPointerEnterFeedbackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_autoPointerEnterFeedbackDelay);
            _pointerEnterFeedback?.PlayFeedbacks();
        }
    }
}