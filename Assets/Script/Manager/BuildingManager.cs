using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.EventSystems;
using System;

public class BuildingManager : MonoBehaviour
{
    private const float MaxBuildingRange = 25f;

    public event Action<BuildingType> OnCurrentBuildingChanged;
    public static BuildingManager Instance { get; private set; }
    public Building HqBuilding => hqBuilding;
    [SerializeField] private AssetReferenceT<GameObject> harvesterTemplate;
    [SerializeField] private Building hqBuilding;
    private IList<BuildingType> buildingTypeDatas;
    private BuildingType currentBuildingType;
    private BoxCollider2D templateCollider;
    private string errorMsg;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        harvesterTemplate.LoadAssetAsync<GameObject>().Completed += OnTemplateLoaded;
    }
    public void OnBuildingAssetCompleted(IList<BuildingType> datas)
    {
        this.buildingTypeDatas = datas;
    }


    private void OnTemplateLoaded(AsyncOperationHandle<GameObject> obj)
    {
        templateCollider = obj.Result.GetComponent<BoxCollider2D>();
    }

    private async void Update()
    {
        if (buildingTypeDatas == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (CanBuild())
            {
                ToolTip.Instance.Hide();
                var spawnPos = Utils.GetMouseWorldPos();
                var type = currentBuildingType;
                var param = new UnityEngine.ResourceManagement.ResourceProviders.InstantiationParameters(spawnPos, Quaternion.identity, null);
                await BuildingConstruction.Create(param, type);
                await UniTask.Delay((int)(type.BuildingTime * 1000));
                harvesterTemplate.InstantiateAsync().Completed += thisHadle =>
                {
                    var go = thisHadle.Result;
                    var building = go.GetComponent<Building>();
                    building.Init(type,spawnPos, thisHadle);
                };
            }
            else
            {
                ToolTip.Instance.Show(errorMsg);
            }
        }


    }
    private bool CanBuild()
    {
        errorMsg = string.Empty;
        return currentBuildingType != null
            && currentBuildingType.BuildingTypeEnum != BuildingTypeEnum.Cursor
            && CheckValidOverlapArea()
            && CheckSameResourceBuilding()
            && CheckIsWithinMaxRange()
            && CheckCanAfford();
    }

    private bool CheckValidOverlapArea()
    {
        if (templateCollider != null)
        {
            var canBuild = Physics2D.OverlapBoxAll(Utils.GetMouseWorldPos() + (Vector3)templateCollider.offset, templateCollider.size, 0).Length == 0;
            if (!canBuild)
            {
                errorMsg = LangManager.GetInstance().GetLangString(LangUsageType.ToolTip, 0);
            }
            return canBuild;
        }
        return false;
    }
    private bool CheckSameResourceBuilding()
    {
        var count = Utils.GetNearByCount<IResourceGenerator>(
                currentBuildingType.SameBuildingRange, temp => temp.ResourceTypeEnum == currentBuildingType.ResourceTypeEnum && !temp.IsHQ);
        var canBuild = Utils.GetNearByCount<IResourceGenerator>(
                currentBuildingType.SameBuildingRange, temp => temp.ResourceTypeEnum == currentBuildingType.ResourceTypeEnum && !temp.IsHQ) == 0;
        if (!canBuild)
        {
            errorMsg = LangManager.GetInstance().GetLangString(LangUsageType.ToolTip, 1);
        }
        return canBuild;


    }

    private BuildingType GetCurrentBuildingType(BuildingTypeEnum currentBuildingType)
    {
        return buildingTypeDatas.First(_ => _.BuildingTypeEnum == currentBuildingType);
    }

    private bool CheckIsWithinMaxRange()
    {
        var canBuild = Utils.GetNearByCount<IResourceGenerator>(MaxBuildingRange) != 0;
        if (!canBuild)
        {
            errorMsg = LangManager.GetInstance().GetLangString(LangUsageType.ToolTip, 2);
        }
        return canBuild;
    }

    public void ChangeBuildingType(BuildingTypeEnum currentBuildingTypeEnum)
    {
        this.currentBuildingType = GetCurrentBuildingType(currentBuildingTypeEnum);
        OnCurrentBuildingChanged?.Invoke(currentBuildingType);
    }
    private bool CheckCanAfford()
    {
        var canBuild = ResourceManager.GetInstance().CheckCanAfford(currentBuildingType.BuildingCosts);
        if (!canBuild)
        {
            errorMsg = string.Format(LangManager.GetInstance().GetLangString(LangUsageType.ToolTip, 3), currentBuildingType.GetBuildingCostsString());
        }
        return canBuild;
    }


    private void OnDestroy()
    {
        OnCurrentBuildingChanged = null;
    }
}
