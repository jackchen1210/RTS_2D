using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AppResourceMgr : IDisposable
{
    private AddressableResourceLoader<ResourceType> resourceLoader;
    private AddressableResourceLoader<BuildingType> buildingLoader;
    private AddressableResourceLoader<LangSetting> langLoader;

    private static AppResourceMgr instance;

    public static AppResourceMgr GetInstance()
    {

        if (instance == null)
        {
            instance = new AppResourceMgr();
        }

        return instance;
    }

    private AppResourceMgr()
    {
        resourceLoader = new AddressableResourceLoader<ResourceType>("Resource");
        buildingLoader = new AddressableResourceLoader<BuildingType>("Building");
        langLoader = new AddressableResourceLoader<LangSetting>("LangSetting");
    }

    public void LoadResourceTypeAssets(Action<IList<ResourceType>> onLoadCompleted)
    {
        resourceLoader.LoadResourceTypeAssets(onLoadCompleted);
    }

    internal IList<BuildingType> GetBuildingAssets()
    {
        return buildingLoader.GetResourceTypeAssets();
    }

    internal IList<LangSetting> GetLangAssets()
    {
        return langLoader.GetResourceTypeAssets();
    }

    public IList<ResourceType> GetResourceTypeAssets()
    {
        return resourceLoader.GetResourceTypeAssets();
    }

    public void LoadBuildingTypeAssets(Action<IList<BuildingType>> onLoadCompleted)
    {
        buildingLoader.LoadResourceTypeAssets(onLoadCompleted);
    }

    internal void LoadLangAssets(Action<IList<LangSetting>> onLoadEnd)
    {
        langLoader.LoadResourceTypeAssets(onLoadEnd);
    }


    public void Dispose()
    {
        resourceLoader?.Dispose();
        buildingLoader?.Dispose();
        langLoader?.Dispose();
        instance = null;
    }
}
