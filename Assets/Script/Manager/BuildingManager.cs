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

    [SerializeField] private AssetReferenceT<GameObject> harvesterTemplate;
    private Queue<AsyncOperationHandle<GameObject>> opHandleQueue = new Queue<AsyncOperationHandle<GameObject>>();
    private IList<BuildingType> buildingTypeDatas;
    private BuildingType currentBuildingType;
    private BoxCollider2D templateCollider;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        harvesterTemplate.LoadAssetAsync<GameObject>().Completed += OnTemplateLoaded;
        AppResourceMgr.GetInstance().LoadBuildingTypeAssets(OnCompleted);
    }

    private void OnTemplateLoaded(AsyncOperationHandle<GameObject> obj)
    {
       templateCollider = obj.Result.GetComponent<BoxCollider2D>();
    }

    private void OnCompleted(IList<BuildingType> datas)
    {
        this.buildingTypeDatas = datas;
    }

    private void Update()
    {
        if (buildingTypeDatas == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (CanBuild())
            {
                harvesterTemplate.InstantiateAsync().Completed += thisHadle =>
                {
                    var go = thisHadle.Result;
                    opHandleQueue.Enqueue(thisHadle);
                    go.transform.position = Utils.GetMouseWorldPos();
                    Instantiate(currentBuildingType.AnimGO,go.transform);
                    var rg = go.GetComponent<ResourceGenerator>();
                    rg.Init(currentBuildingType);
                };
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (opHandleQueue.Count > 0)
            {
                var handle = opHandleQueue.Dequeue();
                Addressables.ReleaseInstance(handle.Result);
            }
        }
    }

    private bool CanBuild()
    {
        return currentBuildingType != null 
            && currentBuildingType.BuildingTypeEnum != BuildingTypeEnum.Cursor
            && CheckValidOverlapArea()
            && CheckSameResoruceBuilding()
            && CheckIsWithinMaxRange();
    }
    private bool CheckValidOverlapArea()
    {
        if (templateCollider != null)
        {
           return Physics2D.OverlapBoxAll(Utils.GetMouseWorldPos() + (Vector3)templateCollider.offset, templateCollider.size,0).Length==0;
        }
        return false;
    }
    private bool CheckSameResoruceBuilding()
    {
        var total = Physics2D.OverlapCircleAll(Utils.GetMouseWorldPos(), currentBuildingType.SameBuildingRange)
            .Where(_=>_.gameObject.TryGetComponent<ResourceGenerator>(out var temp)&&temp.ResourceTypeEnum == currentBuildingType.ResourceTypeEnum)
            .Count();

        return total == 0;
    }

    private BuildingType GetCurrentBuildingType(BuildingTypeEnum currentBuildingType)
    {
        return buildingTypeDatas.First(_ => _.BuildingTypeEnum == currentBuildingType);
    }

    private bool CheckIsWithinMaxRange()
    {
        return Physics2D.OverlapCircleAll(Utils.GetMouseWorldPos(), MaxBuildingRange)
            .Where(_ => _.gameObject.TryGetComponent<ResourceGenerator>(out var temp))
            .Count()!=0;
    }

    public void ChangeBuildingType(BuildingTypeEnum currentBuildingTypeEnum)
    {
        this.currentBuildingType = GetCurrentBuildingType(currentBuildingTypeEnum);
        OnCurrentBuildingChanged?.Invoke(currentBuildingType);
    }

    private void OnDestroy()
    {
        OnCurrentBuildingChanged = null;
        foreach (var handle in opHandleQueue)
        {
            Addressables.ReleaseInstance(handle.Result);
        }
        opHandleQueue.Clear();
    }
}
