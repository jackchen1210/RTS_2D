using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ResourceNearByOverlap resourceNearByOverlap;
    private IList<ResourceType> resourceTypes;
    private BuildingType currentBuildingType;
    private Vector3 mousePosTemp;

    private void Start()
    {
        Hide();
        BuildingManager.Instance.OnCurrentBuildingChanged += OnCurrentBuildingChanged;
    }

    private void Update()
    {
        var pos = Utils.GetMouseWorldPos();
        if (mousePosTemp != pos)
        {
            mousePosTemp = pos;
            transform.position = mousePosTemp;
            UpdateEfficientView();
        }
    }

    private void UpdateEfficientView()
    {
        if(currentBuildingType!=null && currentBuildingType.BuildingTypeEnum!= BuildingTypeEnum.Cursor)
        {
            var count = Utils.GetNearByCount<ResourceNode>(currentBuildingType.ResourceDetectRadius,source=>source.ResourceTypeEnum == currentBuildingType.ResourceTypeEnum);
            resourceNearByOverlap.SetEfficient(count, currentBuildingType.MaxResourceCollect);
        }
    }

    private void OnCurrentBuildingChanged(BuildingType type)
    {
        currentBuildingType = type;
        if (type.BuildingTypeEnum == BuildingTypeEnum.Cursor)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        if (resourceTypes == null)
        {
            resourceTypes = AppResourceMgr.GetInstance().GetResourceTypeAssets();
        }

        spriteRenderer.sprite = currentBuildingType.BuildingSp;
        resourceNearByOverlap.SetResourceSp(resourceTypes.FirstOrDefault(_=>_.ResourceTypeEnum == currentBuildingType.ResourceTypeEnum)?.ResourceSp);
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
