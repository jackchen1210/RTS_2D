using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ToolTip : MonoBehaviour
{
    public static ToolTip Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private RectTransform bgImageRect;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("Settings")]
    [SerializeField] private float timer= 3f;
    private RectTransform rectTransform;
    private Tween timerTween;

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void Update()
    {
        var pos = Input.mousePosition;

        if (pos.x + bgImageRect.rect.width > canvasRect.rect.width)
        {
            pos.x = canvasRect.rect.width - bgImageRect.rect.width;
        }
        else if (pos.x < 0)
        {
            pos.x = 0;
        }
        if (pos.y + bgImageRect.rect.height > canvasRect.rect.height)
        {
            pos.y = canvasRect.rect.height - bgImageRect.rect.height;
        }
        else if (pos.y < 0)
        {
            pos.y = 0;
        }

        rectTransform.anchoredPosition = pos;
    }

    public void Show(string text)
    {
        canvasGroup.alpha = 1;
        textMeshProUGUI.SetText(text);
        textMeshProUGUI.ForceMeshUpdate();
        bgImageRect.sizeDelta = textMeshProUGUI.GetRenderedValues(false) + new Vector2(8, 0);
        StartHideTimer();
    }

    private void StartHideTimer()
    {
        if(timerTween!=null)
        {
            timerTween.Kill(true);
        }
       timerTween = DOVirtual.DelayedCall(timer,Hide);
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
