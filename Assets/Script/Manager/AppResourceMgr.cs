using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AppResourceMgr : IDisposable
{
    private AsyncOperationHandle<IList<ResourceType>> assetHandle;
    private AsyncOperationHandle<IList<BuildingType>> buildingHandle;

    private static AppResourceMgr instance;

    public static AppResourceMgr GetInstance()
    {

        if (instance == null)
        {
            instance = new AppResourceMgr();
        }

        return instance;
    }

    public void LoadResourceTypeAssets(Action<IList<ResourceType>> onLoadCompleted)
    {
        if (onLoadCompleted is null)
        {
            throw new ArgumentNullException(nameof(onLoadCompleted));
        }
        if (!assetHandle.IsValid())
        {
            assetHandle = Addressables.LoadAssetsAsync<ResourceType>("Resource", null);
            assetHandle.Completed += (_) => onLoadCompleted?.Invoke(_.Result);
        }
        else
        {
            onLoadCompleted?.Invoke(assetHandle.Result);
        }
    }
    public void LoadBuildingTypeAssets(Action<IList<BuildingType>> onLoadCompleted)
    {
        if (!buildingHandle.IsValid() )
        {
            buildingHandle = Addressables.LoadAssetsAsync<BuildingType>("Building", null);
            buildingHandle.Completed += (_) => onLoadCompleted?.Invoke(_.Result);
        }
        else
        {
            if (!buildingHandle.IsDone)
            {
                buildingHandle.Completed += (_) => onLoadCompleted?.Invoke(_.Result);
            }
            else
            {
                onLoadCompleted?.Invoke(buildingHandle.Result);
            }
        }
    }


    public void Dispose()
    {
        Addressables.Release(assetHandle);
        Addressables.Release(buildingHandle);
        instance = null;
    }
}
