using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public static class DOTweenExtensions
{
    public static Task AsyncWaitForCompletion(this Tween tween)
    {
        if (tween is not { active: true } || tween.IsComplete())
            return Task.CompletedTask;

        var tcs = new TaskCompletionSource<object>();
        tween.OnComplete(() => tcs.TrySetResult(null));
        tween.OnKill(() => tcs.TrySetCanceled());

        return tcs.Task;
    }
}

public static class UIAnimation
{
    #region One Time Effects

    public static Tween DoAnimateScale(this ITransform transform, Vector3 from, Vector3 to, float duration,
        Ease ease = Ease.Linear)
    {
        return DOTween.To(() => from, x => transform.scale = x, to, duration).SetEase(ease);
    }
    
    // Overload for RectTransform
    public static Tween DoAnimateScale(this Transform rectTransform, Vector3 from, Vector3 to, float duration,
        Ease ease = Ease.Linear)
    {
        return rectTransform.DOScale(to, duration).From(from).SetEase(ease);
    }

    public static Tween DoAnimateRotation(this ITransform transform, float endRotation, float duration,
        Ease ease = Ease.Linear)
    {
        var initialRotation = transform.rotation.eulerAngles.z;
        var targetRotation = endRotation;
        var rotationTween = DOTween.To(() => initialRotation, x => transform.rotation = Quaternion.Euler(0, 0, x),
            targetRotation, duration).SetEase(ease);

        return rotationTween;
    }
    
    public static Tween DoAnimateRotation(this Transform rectTransform, float endRotation, float duration,
        Ease ease = Ease.Linear)
    {
        var initialRotation = rectTransform.rotation.eulerAngles.z;
        var targetRotation = endRotation;
        var rotationTween = DOTween.To(() => initialRotation, x => rectTransform.rotation = Quaternion.Euler(0, 0, x),
            targetRotation, duration).SetEase(ease);

        return rotationTween;
    }

    public static Tween DoAnimatePosition(this ITransform transform, Vector3 from, Vector3 to, float duration,
        Ease ease = Ease.Linear)
    {
        return DOTween.To(() => from, x => transform.scale = x, to, duration).SetEase(ease);
    }
    
    // Overload for RectTransform
    public static Tween DoAnimatePosition(this RectTransform rectTransform, Vector3 from, Vector3 to, float duration,
        Ease ease = Ease.Linear)
    {
        return rectTransform.DOMove(to, duration).From(from).SetEase(ease);
    }
    
    public static Tween DoAnimateLocalPosition(this Transform rectTransform, Vector3 from, Vector3 to, float duration,
        Ease ease = Ease.Linear)
    {
        return rectTransform.DOLocalMove(to, duration).From(from).SetEase(ease);
    }

    public static Tween DoAnimateFade(this VisualElement element, float from, float to, float duration,
        Ease ease = Ease.Linear)
    {
        return DOTween.To(() => from, x => element.style.opacity = x, to, duration).SetEase(ease);
    }
    
    //Overload Transform
    public static Tween DoAnimateFade(this Transform transform, float from, float to, float duration,
        Ease ease = Ease.Linear)
    {
        var image = transform.GetComponent<Image>();
        if (image == null) return null;
        
        return DOTween.To(() => from, x => image.color = new Color(image.color.r, image.color.g, image.color.b, x), to, duration).SetEase(ease);
    }
    
    public static Tween DoAnimateFade(this Image image, float from, float to, float duration,
        Ease ease = Ease.Linear)
    {
        return DOTween.To(() => from, x => image.color = new Color(image.color.r, image.color.g, image.color.b, x), to, duration).SetEase(ease);
    }

    public static Tween DoAnimateFade(this RawImage rawImage, float from, float to, float duration,
        Ease ease = Ease.Linear)
    {
        return DOTween.To(() => from, x => rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, x), to, duration).SetEase(ease);
    }
    
    public static Tween DoAnimateFade(this Image image, float from, float to, float duration, Action onComplete, Ease ease = Ease.Linear)
    {
        return DOTween.To(() => from, 
                x => image.color = new Color(image.color.r, image.color.g, image.color.b, x), 
                to, 
                duration)
            .SetEase(ease)
            .OnComplete(() => 
            {
                onComplete?.Invoke();
            });
    }
    
    public static Tween DoAnimateFadeGraphic(this Graphic graphic, float from, float to, float duration,
        Ease ease = Ease.Linear)
    {
        return DOTween.To(() => from, x => graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, x), to, duration).SetEase(ease);
    }

    public static Tween DoAnimatePopIn(this ITransform element, float duration, Ease ease = Ease.OutBack)
    {
        element.scale = Vector3.zero;
        var scaleTween = element.DoAnimateScale(Vector3.zero, Vector3.one, duration, ease);
        return scaleTween;
    }
    
    // Overload for RectTransform
    public static Tween DoAnimatePopIn(this Transform rectTransform, float duration, Ease ease = Ease.OutBack)
    {
        rectTransform.localScale = Vector3.zero;
        var scaleTween = rectTransform.DoAnimateScale(Vector3.zero, Vector3.one, duration, ease);
        return scaleTween;
    }

    public static Tween DoAnimatePopOut(this ITransform element, float duration, Ease ease = Ease.InBack)
    {
        var scaleTween = element.DoAnimateScale(Vector3.one, Vector3.zero, duration, ease);
        return scaleTween;
    }
    
    // Overload for RectTransform
    public static Tween DoAnimatePopOut(this Transform rectTransform, float duration, Ease ease = Ease.InBack)
    {
        var scaleTween = rectTransform.DoAnimateScale(Vector3.one, Vector3.zero, duration, ease);
        return scaleTween;
    }
    
    public static Tween DoAnimatePopOut(this Transform rectTransform, float duration, Action onComplete, Ease ease = Ease.InBack)
    {
        var scaleTween = rectTransform.DOScale(Vector3.zero, duration).SetEase(ease);
        if (onComplete != null)
        {
            scaleTween.OnComplete(() => onComplete());
        }
        return scaleTween;
    }
    
    public static Tween DoAnimateShake(this ITransform element, float duration = .25f, float strength = 20,
        int vibrato = 20, float randomness = 10)
    {
        var tween = DOTween.Shake(() => element.position, x => element.position = x, duration, strength, vibrato,
            randomness);
        return tween;
    }
    
    // Overload for Transform
    public static Tween DoAnimateShake(this Transform transform, float duration = .25f, float strength = 20,
        int vibrato = 20, float randomness = 10)
    {
        var tween = DOTween.Shake(() => transform.position, x => transform.position = x, duration, strength, vibrato,
            randomness);
        return tween;
    }
    
    public static Tween DoAnimateLocalShake(this Transform transform, float duration = .25f, float strength = 20,
        int vibrato = 20, float randomness = 10)
    {
        var tween = DOTween.Shake(() => transform.localPosition, x => transform.localPosition = x, duration, strength, vibrato,
            randomness);
        return tween;
    }
    
    // Do Local Move X
    public static Tween DoAnimateLocalMoveX(this Transform transform, float to, float duration, Ease ease = Ease.Linear)
    {
        return transform.DOLocalMoveX(to, duration).SetEase(ease);
    }
    
    
    // fade a sprite into another sprite
    public static Tween DoAnimateFadeSprite(this Image image, Sprite from, Sprite to, float duration, Ease ease = Ease.Linear)
    {
        return DOTween.To(() => 0, x => image.sprite = x < 0.5f ? from : to, 1, duration).SetEase(ease);
    }
    

    #endregion
    public static Tween BreatheLoopEffect(this VisualElement element, BreathingEffectParams breathingEffectParams, AnimationCurve customAnimationCurve = null)
    {
        // Stop any existing tweens on this element
        DOTween.Kill(element, true);

        var from = Color.white;
        var to = breathingEffectParams.EndColor;
        

        var colorTween = DOTween.To(() => from, x => element.style.unityBackgroundImageTintColor = x, to,
                breathingEffectParams.Duration)
            .SetEase(breathingEffectParams.Ease)
            .SetLoops(-1, LoopType.Yoyo)
            .SetTarget(element); // This allows DOTween.Kill(element) to work

        if (customAnimationCurve != null)
        {
            colorTween.SetEase(customAnimationCurve);
        }
        return colorTween;
    }
    
    // Overload for Transform
    public static Tween BreatheLoopEffect(this Transform transform, BreathingEffectParams breathingEffectParams, AnimationCurve customAnimationCurve = null)
    {
        // Stop any existing tweens on this element
        DOTween.Kill(transform, true);
        
        var scaleTween = transform.DOScale(Vector3.one * 1.2f, breathingEffectParams.Duration)
            .SetEase(breathingEffectParams.Ease)
            .SetLoops(-1, LoopType.Yoyo)
            .SetTarget(transform); // This allows DOTween.Kill(transform) to work

        if (customAnimationCurve != null)
        {
            scaleTween.SetEase(customAnimationCurve);
        }
        
        return scaleTween;
        
        //
        // var image = transform.GetComponent<Image>();
        // if (image == null) return null;
        //
        // // Stop any existing tweens on this element
        // DOTween.Kill(image, true);
        //
        // var from = Color.white;
        // var to = breathingEffectParams.EndColor;
        //
        //
        // var colorTween = DOTween.To(() => from, x => image.color = x, to,
        //         breathingEffectParams.Duration)
        //     .SetEase(breathingEffectParams.Ease)
        //     .SetLoops(-1, LoopType.Yoyo)
        //     .SetTarget(image); // This allows DOTween.Kill(image) to work
        //
        // if (customAnimationCurve != null)
        // {
        //     colorTween.SetEase(customAnimationCurve);
        // }
        // return colorTween;
    }

    public static void StopBreatheLoopEffect(this VisualElement element)
    {
        DOTween.Kill(element, true);
        element.style.unityBackgroundImageTintColor = Color.white;
    }
    
    // Overload for Transform
    public static void StopBreatheLoopEffect(this Transform transform)
    {
        DOTween.Kill(transform, true);
        transform.localScale = Vector3.one;
        
        // var image = transform.GetComponent<Image>();
        // if (image == null) return;
        //
        // DOTween.Kill(image, true);
        // image.color = Color.white;
    }

}

public class BreathingEffectParams
{
    public float Duration = .5f;
    public Color EndColor = Color.green;
    public Ease Ease = Ease.InSine;
}