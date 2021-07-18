using System;
using TMPro;
using UnityEngine;

public class ResourceNearByOverlap : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;

    public void SetResourceSp(Sprite sprite)
    {
        if(sprite == null)
        {
            Hide();
        }
        else
        {
            spriteRenderer.sprite = sprite;
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetEfficient(float nearByResource, float maxResource)
    {
        textMeshPro.text = $" {Mathf.Clamp(Mathf.RoundToInt((nearByResource / maxResource) * 100), 0, 100)} %";
    }
}
