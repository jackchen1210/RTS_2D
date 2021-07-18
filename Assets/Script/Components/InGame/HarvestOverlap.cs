using UnityEngine;
using TMPro;
using DG.Tweening;

public class HarvestOverlap : MonoBehaviour
{
    [SerializeField] private SpriteRenderer resourceRenderer;
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private Transform barTrans;

    private Tween tween;

    public void Init(Sprite resourceSp,int resourceInRange)
    {
        if (resourceSp != null)
        {
            gameObject.SetActive(true);
            resourceRenderer.sprite = resourceSp;
            textMesh.text = resourceInRange.ToString();
            tween = barTrans.DOScaleX(barTrans.localScale.x, 1f / resourceInRange).From(0).SetLoops(-1);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        tween?.Kill();
    }
}
