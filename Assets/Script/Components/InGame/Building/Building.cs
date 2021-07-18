using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(HealthSystem))]
public class Building : MonoBehaviour
{
    public BoolReactiveProperty IsDead  = new BoolReactiveProperty(false);

    [Header("Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damageHealth = 10;

    private DemolishUI demolishUI;
    private BuildingType type;
    private Vector3 spawnPos;
    private AsyncOperationHandle<GameObject> thisHandle;
    private HealthSystem healthSystem;
    private BoxCollider2D boxColli;
    private SpriteRenderer buildingSpRenderer;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.Init(maxHealth);
        healthSystem.IsDead.Subscribe(IsDeadCheck).AddTo(gameObject);
    }

    internal void Init(BuildingType type, Vector3 spawnPos, AsyncOperationHandle<GameObject> thisHandle)
    {
        this.type = type;
        this.spawnPos = spawnPos;
        this.thisHandle = thisHandle;
        ResourceManager.GetInstance().SpendResource(type.BuildingCosts);
        transform.position = spawnPos;

        AddSprite();
        AddResourceGenerator();
        if (type.BuildingTypeEnum == BuildingTypeEnum.Tower)
        {
            AddTower();
        }
        SetCollider();
        SetDemolishUI();
    }

    private void SetDemolishUI()
    {
        demolishUI = GetComponentInChildren<DemolishUI>();
        if (demolishUI != null)
        {
            demolishUI.transform.localPosition = Utils.GetCenterPosBySprite(buildingSpRenderer.sprite) + Offset(buildingSpRenderer.sprite);
            demolishUI.Init(OnDemolishBtnClicked);
            demolishUI.gameObject.SetActive(false);
        }
    }

    private Vector3 Offset(Sprite sprite)
    {
        return new Vector3(-sprite.rect.width, sprite.rect.height ,0) / (sprite.pixelsPerUnit * 2f);
    }

    private void SetCollider()
    {
        boxColli = GetComponent<BoxCollider2D>();
        if (boxColli != null && buildingSpRenderer !=null)
        {
            boxColli.size = buildingSpRenderer.sprite.rect.size / buildingSpRenderer.sprite.pixelsPerUnit*1.2f;
            boxColli.offset = Utils.GetCenterPosBySprite(buildingSpRenderer.sprite);
        }
    }

    private void OnDemolishBtnClicked()
    {
        Destroy(gameObject);
    }

    public void Damage()
    {
        healthSystem.Damage(damageHealth);
    }


    private void IsDeadCheck(bool isDead)
    {
        if (isDead)
        {
            Destroy(gameObject);
        }
        IsDead.Value = isDead;
    }
    private void AddSprite()
    {
        if (type.AnimGO != null)
        {
           var go = Instantiate(type.AnimGO, transform);
            buildingSpRenderer = go.GetComponentInChildren<SpriteRenderer>();
        }
        else
        {
            buildingSpRenderer = gameObject.AddComponent<SpriteRenderer>();
            buildingSpRenderer.sprite = type.BuildingSp;
        }
    }

    private void AddTower()
    {
        var tower = gameObject.AddComponent<Tower>();
        gameObject.AddComponent<SortingLayerHelper>();
    }

    private void AddResourceGenerator()
    {
        var resource = AppResourceMgr.GetInstance().GetResourceTypeAssets().FirstOrDefault(_ => _.ResourceTypeEnum == type.ResourceTypeEnum);
        var rg = gameObject.GetComponent<IResourceGenerator>();
        rg.Init(type);
        var overlap = gameObject.GetComponentInChildren<HarvestOverlap>();
        overlap.Init(resource?.ResourceSp, rg.NodesCount);
    }

    private void OnMouseEnter()
    {
        demolishUI?.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        demolishUI?.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (thisHandle.IsValid())
        {
            Addressables.Release(thisHandle);
        }
    }
}
