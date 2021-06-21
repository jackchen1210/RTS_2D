using System;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Hide();
        BuildingManager.Instance.OnCurrentBuildingChanged += OnCurrentBuildingChanged;
    }

    private void Update()
    {
        transform.localPosition = Utils.GetMouseWorldPos();
    }

    private void OnCurrentBuildingChanged(BuildingType type)
    {
        if(type.BuildingTypeEnum == BuildingTypeEnum.Cursor)
        {
            Hide();
        }
        else
        {
            Show(type.BuildingSp);
        }
    }

    private void Show(Sprite buildingSp)
    {
        spriteRenderer.sprite = buildingSp;
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
