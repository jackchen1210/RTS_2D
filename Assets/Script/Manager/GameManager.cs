using System;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingGO;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private BuildingManager buildingManager;
    private AppResourceMgr appResourceMgr;

    private void Awake()
    {
        SceneManagerHelper.OnSceneLoaded -= OnSceneLoaded;
        SceneManagerHelper.OnSceneLoaded += OnSceneLoaded;
        appResourceMgr = AppResourceMgr.GetInstance();
        //loadingGO.SetActive(true);
        Init();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TimeManager.GetInstance().Dispose();
    }

    private void Init()
    {
        Observable.WhenAll(
            UnirxTool.ToObserverable<IList<LangSetting>>(appResourceMgr.LoadLangAssets),
            UnirxTool.ToObserverable<IList<ResourceType>>(appResourceMgr.LoadResourceTypeAssets),
            UnirxTool.ToObserverable<IList<BuildingType>>(appResourceMgr.LoadBuildingTypeAssets))
            .Subscribe(OnEnd).AddTo(gameObject);
    }

    private void OnEnd(Unit obj)
    {
        uIManager.OnBuildingCompleted(appResourceMgr.GetBuildingAssets());
        uIManager.OnResourceCompleted(appResourceMgr.GetResourceTypeAssets());
        LangManager.GetInstance().OnLoadEnd(appResourceMgr.GetLangAssets());
        buildingManager.OnBuildingAssetCompleted(appResourceMgr.GetBuildingAssets());
    }

}