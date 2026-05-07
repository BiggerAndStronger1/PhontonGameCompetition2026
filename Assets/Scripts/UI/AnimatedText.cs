using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
[RequireComponent(typeof(Anim2D))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class AnimatedText : MonoBehaviour, ICanvasManager
{
    [SerializeField] private RectTransform blocking;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float playTime = 2f;
    private Anim2D anim2D;
    private TextMeshProUGUI textMeshProUGUI;
    public void ForcedAwake()
    {
        anim2D = GetComponent<Anim2D>();
        anim2D.OnFadeStart = OnFadeStart;
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnFadeStart()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        var rectTransform = GetComponent<RectTransform>();

        TMPro.TextMeshProUGUI tmp = GetComponent<TMPro.TextMeshProUGUI>();
        if (tmp != null)
            tmp.ForceMeshUpdate();

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        var rectWidth = rectTransform.rect.width;

        var blockingRectTransform = blocking.GetComponent<RectTransform>();
        blockingRectTransform.anchoredPosition =
            new Vector2(rectWidth, blockingRectTransform.anchoredPosition.y);
    }


    public void ForcedStart()
    {
    }

    private void Update()
    {
        
    }

    public void Play()
    {
        var rectTransform = GetComponent<RectTransform>();

        TMPro.TextMeshProUGUI tmp = GetComponent<TMPro.TextMeshProUGUI>();
        if (tmp != null)
            tmp.ForceMeshUpdate();

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        var rectWidth = rectTransform.rect.width;

        var blockingRectTransform = blocking.GetComponent<RectTransform>();
        blockingRectTransform.anchoredPosition =
            new Vector2(rectWidth, blockingRectTransform.anchoredPosition.y);
        float startX = rectWidth;
        float endX = 0f;
        particle.Play();
        LeanTween.value(gameObject, startX, endX, playTime)
            .setIgnoreTimeScale(true)
            .setOnUpdate((float x) =>
            {
                blockingRectTransform.anchoredPosition =
                    new Vector2(x, blockingRectTransform.anchoredPosition.y);
            });
    }

    public void SetText(string text)
    {
        textMeshProUGUI.text = text;
    }

    public void ForcedOnApplicationQuit()
    {
        
    }
}
