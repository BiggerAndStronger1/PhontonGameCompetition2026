using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class NamePage : MonoBehaviour
{
    private List<AnimatedText> textList;
    [FormerlySerializedAs("pages")] [SerializeField] private GameObject nextPage;
    private CanvasGroup[] canvasGroups;
    [SerializeField] private GameObject background;
    [SerializeField] private bool titlePage;
    void OnEnable()
    {
        textList = GetComponentsInChildren<AnimatedText>().ToList();
        canvasGroups = GetComponentsInChildren<CanvasGroup>();
        canvasGroups.ToList().ForEach((group => group.alpha = 0));
        if (!titlePage)
        {
            StartCoroutine(Delayed());
        }
        else LeanTween.value(0, 1, 2).setOnUpdate(f => canvasGroups.ToList().ForEach(group => group.alpha = f)).setOnComplete(OnFadeComplete);
    }

    private IEnumerator Delayed()
    {
        yield return new WaitForSeconds(1f);
        LeanTween.value(0, 1, 2).setOnUpdate(f => canvasGroups.ToList().ForEach(group => group.alpha = f)).setOnComplete(OnFadeComplete);
    }

    private void OnFadeComplete()
    {
        
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        textList.ForEach(text => text.Play());
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void Start()
    {
        CanvasManager.actionsUI.Disable();
    }

    private void OnDestroy()
    {
        if (nextPage) nextPage.SetActive(true);
        else
        {
            CanvasManager.actionsUI.Enable();
            Destroy(background);
        }
    }

    void Update()
    {
        
    }
}
