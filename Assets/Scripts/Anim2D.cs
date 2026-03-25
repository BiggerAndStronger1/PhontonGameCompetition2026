using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

/// <summary>
/// Speed profiles for movement interpolation using LeanTween easing types.
/// </summary>
public enum SpeedMode
{
    /// <summary>
    /// Moves at a constant speed with no acceleration or deceleration.
    /// </summary>
    Constant,

    /// <summary>
    /// Starts fast and slows down toward the end (ease-out).
    /// </summary>
    Decelerate,

    /// <summary>
    /// Starts slow and speeds up toward the end (ease-in).
    /// </summary>
    Accelerate,

    /// <summary>
    /// Starts slow, speeds up in the middle, and slows down again at the end (ease-in-out).
    /// </summary>
    EaseInOut,

    /// <summary>
    /// Adds a bounce effect at the end of the motion.
    /// </summary>
    Bounce,

    /// <summary>
    /// Overshoots the target and then settles back with an elastic spring-like motion.
    /// </summary>
    Spring,

    /// <summary>
    /// Slightly overshoots the target before snapping back (ease-out back).
    /// </summary>
    Elastic,

    /// <summary>
    /// Accelerates and decelerates using a cubic curve (ease-in-out cubic).
    /// </summary>
    Cubic,

    /// <summary>
    /// Accelerates and decelerates using a quartic curve (ease-in-out quart).
    /// </summary>
    Quartic,

    /// <summary>
    /// Accelerates and decelerates using a quintic curve (ease-in-out quint).
    /// </summary>
    Quintic,

    /// <summary>
    /// Accelerates and decelerates using an exponential curve (ease-in-out expo).
    /// </summary>
    Expo,

    /// <summary>
    /// Accelerates and decelerates using a circular curve (ease-in-out circ).
    /// </summary>
    Circular
}





/// <summary>
/// Anim2D provides 2D animation utilities for movement, scaling, rotation, and fading using LeanTween.
/// </summary>
public class Anim2D : MonoBehaviour
{
    [Serializable]
    public class AnimEventSettings
    {
        [Tooltip("move and scale both requires a vector, others require a float")]
        public AnimationType animationType;
        public float duration = 1f;
        public SpeedMode speedMode = SpeedMode.Constant;

        public float targetFloat;
        public Vector3 targetV3;
    }

    public enum AnimationType
    {
        Move,
        Scale,
        Rotate,
        Fade
    }

    // Delegates for move events
    public Action OnMoveStart, OnMoveComplete;
    public Action<float> OnMoveUpdate;

    // Delegates for scale events
    public Action OnScaleStart, OnScaleComplete;
    public Action<float> OnScaleUpdate;

    // Delegates for rotate events
    public Action OnRotateStart, OnRotateComplete;
    public Action<float> OnRotateUpdate;

    // Delegates for fade events
    public Action OnFadeStart, OnFadeComplete;
    public Action<float> OnFadeUpdate;

    [Tooltip("If set, this GameObject will be disabled after the disable animation completes. If null, the current GameObject will be disabled.")]
    public GameObject disableTarget;

    // Tween IDs for tracking active tweens
    private int moveTweenId = -1;
    private int scaleTweenId = -1;
    private int rotateTweenId = -1;
    private int fadeTweenId = -1;

    // Initial values for resetting after cancel
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private float initialAlpha;

    // Store initial alphas for Image and SpriteRenderer fades
    private float[] initialImageAlphas = new float[0];
    private float[] initialSpriteAlphas = new float[0];

    [SerializeField] private bool animateOnEnable = true;
    [SerializeField] private bool animateOnDisable = true;

    [Header("OnEnable Animation Settings")]
    [SerializeField] private AnimEventSettings[] onEnableAnimations;

    [Header("OnDisable Animation Settings")]
    [SerializeField] private AnimEventSettings[] onDisableAnimations;

    [Tooltip("If true, fade applies to all active descendants; otherwise only to this GameObject.")]
    [SerializeField] private bool fadeAll = true;

    private Coroutine disableCoroutine;

    private void Awake()
    {
        if (disableTarget == null)
        {
            disableTarget = gameObject;
        }
        print("w");
    }

    /// <summary>
    /// Called when the object is destroyed. Cancels all active tweens.
    /// </summary>
    private void OnDestroy()
    {
        CancelAll();

    }

    private void Update()
    {

    }


    private void OnEnable()
    {
        PlayAnimations(onEnableAnimations, animateOnEnable, false);


    }

    private void OnDisable()
    {
    }

    public void AnimatedDisable()
    {
        disableCoroutine ??= StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        PlayAnimations(onDisableAnimations, animateOnDisable, true);

        // Wait until all tweens on this GameObject are finished
        yield return new WaitWhile(() =>
            LeanTween.isTweening(uniqueId: moveTweenId) || LeanTween.isTweening(uniqueId: scaleTweenId) || LeanTween.isTweening(uniqueId: rotateTweenId) || LeanTween.isTweening(uniqueId: fadeTweenId)
        );
        disableCoroutine = null;
        disableTarget.SetActive(false);
    }


    private void PlayAnimations(AnimEventSettings[] settingsArray, bool isEnable, bool isDisableAnim)
    {
        if (disableCoroutine != null && !isDisableAnim)
            return;

        if (!isEnable || settingsArray.Length == 0)
            return;
        foreach (var anim in settingsArray)
        {
            switch (anim.animationType)
            {
                case AnimationType.Move:
                    MoveTo(anim.targetV3, anim.duration, anim.speedMode);
                    break;

                case AnimationType.Scale:
                    ScaleTo(anim.targetV3, anim.duration, anim.speedMode);
                    break;

                case AnimationType.Rotate:
                    RotateTo(anim.targetFloat, anim.duration, anim.speedMode);
                    break;

                case AnimationType.Fade:
                    FadeTo(anim.targetFloat, anim.duration, anim.speedMode);
                    break;
            }
        }
    }


    /// <summary>
    /// Cancels all active tweens. Optionally resets to initial values.
    /// </summary>
    /// <param name="resetToInitial">If true, resets to initial values before tween started.</param>
    private void CancelAll(bool resetToInitial = false)
    {
        CancelMove(resetToInitial);
        CancelScale(resetToInitial);
        CancelRotate(resetToInitial);
        CancelFade(resetToInitial);
    }

    // Move

    /// <summary>
    /// Moves the GameObject to the target position over the given duration using the specified speed mode.
    /// </summary>
    /// <param name="targetPosition">Target position to move to.</param>
    /// <param name="duration">Duration of the move.</param>
    /// <param name="speedMode">Easing type for the move.</param>
    public void MoveTo(Vector3 targetPosition, float duration, SpeedMode speedMode = SpeedMode.Constant)
    {
        CancelMove();
        initialPosition = transform.position;

        // Ensure Z remains unchanged for world position
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, initialPosition.z);

        OnMoveStart?.Invoke();

        if (transform is RectTransform rectTransform)
        {
            // Use anchoredPosition for UI
            Vector2 startPos = rectTransform.anchoredPosition;
            Vector2 targetPos2D = new Vector2(targetPosition.x, targetPosition.y);

            var tween = LeanTween.value(gameObject, startPos, targetPos2D, duration)
                .setEase(GetEaseType(speedMode))
                .setOnUpdate((Vector2 val) => rectTransform.anchoredPosition = val)
                .setOnUpdate((float t) => OnMoveUpdate?.Invoke(t))
                .setOnComplete(() => OnMoveComplete?.Invoke());

            moveTweenId = tween.uniqueId;

        }
        else
        {
            var tween = LeanTween.move(gameObject, targetPosition, duration)
                .setEase(GetEaseType(speedMode))
                .setOnUpdate((float t) => OnMoveUpdate?.Invoke(t))
                .setOnComplete(() => OnMoveComplete?.Invoke());

            moveTweenId = tween.uniqueId;
        }
    }

    public void CancelMove(bool resetToInitial = false)
    {
        if (LeanTween.isTweening(moveTweenId))
        {
            LeanTween.cancel(moveTweenId);
            if (resetToInitial)
            {
                if (transform is RectTransform rectTransform)
                    rectTransform.anchoredPosition = initialPosition;
                else
                    transform.position = initialPosition;
            }
        }
    }

    public void ScaleTo(Vector3 targetScale, float duration, SpeedMode speedMode = SpeedMode.Constant)
    {
        CancelScale();
        initialScale = transform.localScale;
        OnScaleStart?.Invoke();

        // Works for both RectTransform and normal transforms
        var tween = LeanTween.scale(gameObject, targetScale, duration)
            .setEase(GetEaseType(speedMode))
            .setOnUpdate((float t) => OnScaleUpdate?.Invoke(t))
            .setOnComplete(() => OnScaleComplete?.Invoke());

        scaleTweenId = tween.uniqueId;
    }

    public void CancelScale(bool resetToInitial = false)
    {
        if (LeanTween.isTweening(scaleTweenId))
        {
            LeanTween.cancel(scaleTweenId);
            if (resetToInitial)
                transform.localScale = initialScale;
        }
    }

    public void RotateTo(float targetAngle, float duration, SpeedMode speedMode = SpeedMode.Constant)
    {
        CancelRotate();
        initialRotation = transform.localRotation;
        OnRotateStart?.Invoke();
        if (transform is RectTransform)
        {
            // RectTransform rotation still uses eulerAngles
            var tween = LeanTween.rotateAround(gameObject, Vector3.forward, targetAngle, duration)
                .setEase(GetEaseType(speedMode))
                .setOnUpdate((float t) => OnRotateUpdate?.Invoke(t))
                .setOnComplete(() => OnRotateComplete?.Invoke());

            rotateTweenId = tween.uniqueId;
        }
        else
        {
            var tween = LeanTween.rotateAround(gameObject, Vector3.forward, targetAngle, duration)
                .setEase(GetEaseType(speedMode))
                .setOnUpdate((float t) => OnRotateUpdate?.Invoke(t))
                .setOnComplete(() => OnRotateComplete?.Invoke());

            rotateTweenId = tween.uniqueId;
        }
    }

    public void CancelRotate(bool resetToInitial = false)
    {
        if (LeanTween.isTweening(rotateTweenId))
        {
            LeanTween.cancel(rotateTweenId);
            if (resetToInitial)
                transform.rotation = initialRotation;
        }
    }


    // Fade

    /// <summary>
    /// Fades the GameObject's alpha to the target value over the given duration using the specified speed mode.
    /// Uses Image components for UI and SpriteRenderer for sprites.
    /// Sepports Decelerate, Accelerate and Constant speed modes.
    /// </summary>
    /// <param name="targetAlpha">Target alpha value.</param>
    /// <param name="duration">Duration of the fade.</param>
    /// <param name="speedMode">Easing type for the fade.</param>
    public void FadeTo(float targetAlpha, float duration, SpeedMode speedMode = SpeedMode.Constant)
    {
        CancelFade();
        OnFadeStart?.Invoke();

        Image[] images;
        SpriteRenderer[] spriteRenderers;

        if (fadeAll)
        {
            images = GetComponentsInChildren<Image>(includeInactive: true);
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);

        }
        else
        {
            images = new Image[] { GetComponent<Image>() };
            spriteRenderers = new SpriteRenderer[] { GetComponent<SpriteRenderer>() };
        }

        // Filter out nulls (in case component is missing)
        images = System.Array.FindAll(images, img => img != null);
        spriteRenderers = System.Array.FindAll(spriteRenderers, sr => sr != null);

        if (images.Length == 0 && spriteRenderers.Length == 0)
        {
            Debug.LogWarning("No active Image or SpriteRenderer found for fading.");
            return;
        }

        initialImageAlphas = new float[images.Length];
        for (int i = 0; i < images.Length; i++)
            initialImageAlphas[i] = images[i].color.a;

        initialSpriteAlphas = new float[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
            initialSpriteAlphas[i] = spriteRenderers[i].color.a;

        // keep single initialAlpha for legacy CancelFade behavior when only one component present
        float initGroupAlpha = 0;
        if (initialImageAlphas.Length > 0)
            { 
            if (TryGetComponent<CanvasGroup>(out CanvasGroup group)) initGroupAlpha = group.alpha;
            initialAlpha = initialImageAlphas[0]; 
        }

        else if (initialSpriteAlphas.Length > 0)
            initialAlpha = initialSpriteAlphas[0];

        fadeTweenId = LeanTween.value(gameObject, 0, 1, duration)
            .setOnUpdate((float t) =>
            {
                float adjustedT = ApplySpeedMode(t, speedMode);

                if (TryGetComponent<CanvasGroup>(out CanvasGroup group))
                {
                    float a = Mathf.Lerp(initGroupAlpha, targetAlpha, adjustedT);
                    group.alpha = a;
                }
                else
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        Color c = images[i].color;
                        float a = Mathf.Lerp(initialImageAlphas[i], targetAlpha, adjustedT);
                        images[i].color = new Color(c.r, c.g, c.b, a);
                    }
                }
                

                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    Color c = spriteRenderers[i].color;
                    float a = Mathf.Lerp(initialSpriteAlphas[i], targetAlpha, adjustedT);
                    spriteRenderers[i].color = new Color(c.r, c.g, c.b, a);
                }

                OnFadeUpdate?.Invoke(adjustedT);
            })
            .setOnComplete(() =>
            {
                OnFadeComplete?.Invoke();
            })
            .uniqueId;


    }


    /// <summary>
    /// Cancels the current fade tween. Optionally resets to the initial alpha.
    /// </summary>
    /// <param name="resetToInitial">If true, resets to the initial alpha.</param>
    public void CancelFade(bool resetToInitial = false)
    {
        if (LeanTween.isTweening(fadeTweenId))
        {
            LeanTween.cancel(fadeTweenId);
        }

        if (!resetToInitial)
            return;

        Image[] images;
        SpriteRenderer[] spriteRenderers;

        if (fadeAll)
        {
            images = GetComponentsInChildren<Image>(includeInactive: true);
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        }
        else
        {
            images = new Image[] { GetComponent<Image>() };
            spriteRenderers = new SpriteRenderer[] { GetComponent<SpriteRenderer>() };
        }

        images = System.Array.FindAll(images, img => img != null);
        spriteRenderers = System.Array.FindAll(spriteRenderers, sr => sr != null);

        // Restore images
        for (int i = 0; i < images.Length; i++)
        {
            float alpha = initialAlpha;
            if (i < initialImageAlphas.Length)
                alpha = initialImageAlphas[i];

            Color c = images[i].color;
            images[i].color = new Color(c.r, c.g, c.b, alpha);
        }

        // Restore sprite renderers
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            float alpha = initialAlpha;
            if (i < initialSpriteAlphas.Length)
                alpha = initialSpriteAlphas[i];

            Color c = spriteRenderers[i].color;
            spriteRenderers[i].color = new Color(c.r, c.g, c.b, alpha);
        }
    }

    /// <summary>
    /// Applies the selected speed mode to the interpolation value.
    /// Used for manual value tweens like fading.
    /// </summary>
    /// <param name="t">Normalized time (0 to 1).</param>
    /// <param name="mode">Speed mode to apply.</param>
    /// <returns>Adjusted interpolation value.</returns>
    private float ApplySpeedMode(float t, SpeedMode mode)
    {
        switch (mode)
        {
            case SpeedMode.Decelerate: return Mathf.Sin(t * Mathf.PI * 0.5f);
            case SpeedMode.Accelerate: return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            default: return t;
        }
    }

    /// <summary>
    /// Maps SpeedMode to LeanTweenType.
    /// </summary>
    /// <param name="mode">Speed mode to map.</param>
    /// <returns>Corresponding LeanTweenType.</returns>
    private LeanTweenType GetEaseType(SpeedMode mode)
    {
        switch (mode)
        {
            case SpeedMode.Decelerate: return LeanTweenType.easeOutQuad;
            case SpeedMode.Accelerate: return LeanTweenType.easeInQuad;
            case SpeedMode.EaseInOut: return LeanTweenType.easeInOutQuad;
            case SpeedMode.Bounce: return LeanTweenType.easeOutBounce;
            case SpeedMode.Spring: return LeanTweenType.easeOutElastic;
            case SpeedMode.Elastic: return LeanTweenType.easeOutBack;
            case SpeedMode.Cubic: return LeanTweenType.easeInOutCubic;
            case SpeedMode.Quartic: return LeanTweenType.easeInOutQuart;
            case SpeedMode.Quintic: return LeanTweenType.easeInOutQuint;
            case SpeedMode.Expo: return LeanTweenType.easeInOutExpo;
            case SpeedMode.Circular: return LeanTweenType.easeInOutCirc;
            case SpeedMode.Constant:
            default: return LeanTweenType.linear;
        }
    }

    public void QuitApp()
    {
        Application.Quit(0);
    }
}
