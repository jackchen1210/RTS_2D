using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{

    private static ResourceManager instance;
    private Dictionary<ResourceTypeEnum, int> reosurcesDic = new Dictionary<ResourceTypeEnum, int>();

    public static ResourceManager GetInstance()
    {
        if (instance == null)
        {
            instance = new ResourceManager();
        }
        return instance;
    }

    private ResourceManager()
    {
        CreateDefaultDic();
    }

    private void CreateDefaultDic()
    {
        foreach (var item in Enum.GetValues(typeof(ResourceTypeEnum)))
        {
            reosurcesDic.Add((ResourceTypeEnum)item, 0);
        }
    }

    public void AddResource(ResourceTypeEnum resourceTypeEnum, int value)
    {
        reosurcesDic[resourceTypeEnum] += value;
    }
    public void SpendResource(params BuildingCost[] buildingCosts)
    {
        for (int i = 0; i < buildingCosts.Length; i++)
        {
            var cost = buildingCosts[i];
            if (reosurcesDic[cost.ResourceType.ResourceTypeEnum] - cost.Amount >= 0)
            {
                reosurcesDic[cost.ResourceType.ResourceTypeEnum] -= cost.Amount;
            }
        }
    }

    public bool CheckCanAfford(params BuildingCost[] buildingCosts)
    {
        for (int i = 0; i < buildingCosts.Length; i++)
        {
            if (reosurcesDic[buildingCosts[i].ResourceType.ResourceTypeEnum] < buildingCosts[i].Amount)
            {
                return false;
            }
        }
        return true;
    }

    public int GetResource(ResourceTypeEnum resourceTypeEnum)
    {
        if (reosurcesDic.TryGetValue(resourceTypeEnum, out var data))
        {
            return data;
        }
        return 0;
    }

    public void Clear()
    {
        reosurcesDic.Clear();
        CreateDefaultDic();
    }
}
