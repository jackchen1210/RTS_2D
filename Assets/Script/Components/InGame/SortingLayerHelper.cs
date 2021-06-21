using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortingLayerHelper : MonoBehaviour
{
    [SerializeField] private Renderer[] childs;
    [SerializeField] private bool runOnce;

    private SpriteRenderer spriteRenderer;
    private int[] tempChildOrders;

    private void Awake()
    {
              spriteRenderer = GetComponent<SpriteRenderer>();
        tempChildOrders = childs.Select(_ => _.sortingOrder).ToArray();
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(100 * -transform.position.y);
        if (childs != null)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                var item = childs[i];
                item.sortingOrder = spriteRenderer.sortingOrder+ tempChildOrders[i];
            }
        }

        if (runOnce)
        {
            Destroy(this);
        }
    }
}
    