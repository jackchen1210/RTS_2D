using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class AddressableResourceLoader<T> : IDisposable
{

    private AsyncOperationHandle<IList<T>> assetHandle;

    public string LoadingKey { get; }

    public AddressableResourceLoader(string loadingKey)
    {
        LoadingKey = loadingKey;
    }

    public void LoadResourceTypeAssets(Action<IList<T>> onLoadCompleted)
    {
        if (onLoadCompleted is null)
        {
            throw new ArgumentNullException(nameof(onLoadCompleted));
        }
        if (!assetHandle.IsValid()|| assetHandle.Result== null)
        {
            assetHandle = Addressables.LoadAssetsAsync<T>(LoadingKey, null);
            assetHandle.Completed += (_) => onLoadCompleted?.Invoke(_.Result);
        }
        else
        {
            onLoadCompleted?.Invoke(assetHandle.Result);
        }
    }
    public IList<T> GetResourceTypeAssets()
    {
        if (assetHandle.IsValid() && assetHandle.IsDone)
        {
            return assetHandle.Result;
        }
        else
        {
            return null;
        }
    }

    public void Dispose()
    {
        Addressables.Release(assetHandle);
    }
}