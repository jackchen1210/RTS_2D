using UnityEngine;
using DG.Tweening;
using TMPro;

class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform healthTrans;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;
    private int maxHealth;
    private Tween healthTween;
    private Tween spriteTween;

    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        gameObject.SetActive(false);
    }

    public void UpdateHealth(int value)
    {
        gameObject.SetActive(true);
        healthTween?.Kill();
        spriteTween?.Kill();
        spriteRenderer.color = Color.red;
        healthTween = healthTrans.DOScaleX((float)value / maxHealth, 0.5f);
        spriteTween = spriteRenderer.DOColor(Color.green, 0.5f);
        textMeshPro.text = $"{((float)value / maxHealth)*100}%";
    }

    private void OnDestroy()
    {
        healthTween?.Kill();
        spriteTween?.Kill();
    }
}