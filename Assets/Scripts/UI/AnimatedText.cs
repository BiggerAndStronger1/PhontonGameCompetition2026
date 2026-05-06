using System;
using UnityEngine;
using UnityEngine.Assertions;
[RequireComponent(typeof(Anim2D))]
public class AnimatedText : MonoBehaviour, ICanvasManager
{
    [SerializeField] private RectTransform blocking;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float playTime = 2f;
    private bool started;
    private Anim2D anim2D;
    public void ForcedAwake()
    {
        blocking.GetComponent<CanvasGroup>().alpha = 1f;
        anim2D = GetComponent<Anim2D>();
        anim2D.OnFadeComplete = Play;
    }

    public void ForcedStart()
    {
        
    }

    private void Update()
    {
        if (!particle.isPlaying && started) Destroy(gameObject);
    }

    public void Play()
    {
        particle.Play();
        started = true;
        var rectWidth = GetComponent<RectTransform>().rect.width;
        var blockingRectTransform = blocking.GetComponent<RectTransform>();
        blockingRectTransform.anchoredPosition =
            new Vector2(rectWidth, blockingRectTransform.anchoredPosition.y);
        float startX = rectWidth;
        float endX = 0f;

        LeanTween.value(gameObject, startX, endX, playTime)
            .setIgnoreTimeScale(true)
            .setOnUpdate((float x) =>
            {
                blockingRectTransform.anchoredPosition =
                    new Vector2(x, blockingRectTransform.anchoredPosition.y);
            });
    }

    

    public void ForcedOnApplicationQuit()
    {
        
    }
}
