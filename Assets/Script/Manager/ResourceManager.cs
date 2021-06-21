using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager 
{

    private static ResourceManager instance;
    private Dictionary<ResourceTypeEnum, int> reosurcesDic = new Dictionary<ResourceTypeEnum, int>();

    public static ResourceManager GetInstance()
    {
        if(instance == null)
        {
            instance = new ResourceManager();
        }
        return instance;
    }

    public void UpdateResource(ResourceTypeEnum resourceTypeEnum, int value)
    {
        if (!reosurcesDic.ContainsKey(resourceTypeEnum))
        {
            reosurcesDic.Add(resourceTypeEnum, value);
        }
        else
        {
            reosurcesDic[resourceTypeEnum] += value;
        }
    }

    public int GetResource(ResourceTypeEnum resourceTypeEnum)
    {
        if(reosurcesDic.TryGetValue(resourceTypeEnum,out var data))
        {
            return data;
        }
        return 0;
    }
}
