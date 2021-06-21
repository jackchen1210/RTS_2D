using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ResourceUI[] resourceUIs;
    [SerializeField] private BtnTemplate[] btnTemplates;
    private BuildingTypeEnum currentBuildingType;

    private void Start()
    {
        AppResourceMgr.GetInstance().LoadResourceTypeAssets(OnResourceCompleted);
        AppResourceMgr.GetInstance().LoadBuildingTypeAssets(OnBuildingCompleted);
    }

    private void OnResourceCompleted(IList<ResourceType> datas)
    {
        var orderDatas = datas.OrderBy(_ => _.ResourceTypeEnum).ToArray();
        for (int i = 0; i < resourceUIs.Length; i++)
        {
            ResourceType item = orderDatas[i];
            resourceUIs[i].Init(item);
        }
        TimeManager.GetInstance().OnOneSecTick += UpdateUI;
    }

    private void OnBuildingCompleted(IList<BuildingType> datas)
    {
        var orderDatas = datas.OrderBy(_=>(int)_.BuildingTypeEnum).ToArray();
        for (int i = 0; i < btnTemplates.Length; i++)
        {
            BuildingType item = orderDatas[i];
            btnTemplates[i].Init(item,OnBtnClicked);
        }
        UpdateBtnSelect();
    }

    private void OnBtnClicked(BuildingTypeEnum type)
    {
        currentBuildingType = type;
        BuildingManager.Instance?.ChangeBuildingType(type);
        UpdateBtnSelect();
    }

    private void UpdateBtnSelect()
    {
        foreach (var item in btnTemplates)
        {
            if(item.BuildingTypeEnum == currentBuildingType)
            {
                item.SelectActive(true);
            }
            else
            {
                item.SelectActive(false);
            }
        }
    }

    private void UpdateUI()
    {
        var resource = ResourceManager.GetInstance();

        foreach (var resourceUI in resourceUIs)
        {
            resourceUI.UpdateAmount(resource.GetResource(resourceUI.ResourceTypeEnum));
        }
    }
}
