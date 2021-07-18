using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using Assets.Script.Components.ResourceGenerator;
using System;

public class BuildingConstruction : MonoBehaviour
{
    public event Action<float> OnTimeChanged;
    public float Duraction { get; private set; }
    [SerializeField] private SpriteRenderer buildingSpr;
    private float tempTime;

    public static async UniTask<BuildingConstruction> Create(InstantiationParameters instantiationParameters,BuildingType type)
    {
        var go = await Addressables.InstantiateAsync("BuildingConstruction", instantiationParameters);
        go.GetComponent<IResourceGenerator>().Init(type);
        var building = go.GetComponent<BuildingConstruction>();
        var ui = go.GetComponentInChildren<ConstructionTimerUI>();
        ui.Init(building);
        building.Duraction = type.BuildingTime;
        building.buildingSpr.sprite = type.BuildingSp;
        ui.transform.localPosition = Utils.GetCenterPosBySprite(building.buildingSpr.sprite);  
        return building;
    }

    private void Start()
    {
        tempTime = Duraction;
        OnTimeChanged += ChangeMaterial;
    }

    private void ChangeMaterial(float time)
    {
        var mat = buildingSpr.material;
        mat.SetFloat("_Progress",(Duraction- time)/Duraction);
    }

    private void Update()
    {
        if (tempTime >= 0) {
            tempTime -= Time.deltaTime;
            OnTimeChanged?.Invoke(tempTime);
            if (tempTime <= 0)
            {
                OnTime();
            }
        }
    }

    private void OnTime()
    {
        if (!gameObject.GetCancellationTokenOnDestroy().IsCancellationRequested)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnTimeChanged = null;
        Addressables.ReleaseInstance(gameObject);
    }
}
