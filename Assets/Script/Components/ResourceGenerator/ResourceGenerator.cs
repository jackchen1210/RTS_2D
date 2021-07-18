using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour, IResourceGenerator
{
    public bool IsHQ => isHq;
    public int NodesCount => nodes.Count;
    public ResourceTypeEnum ResourceTypeEnum { get; private set; }
    [SerializeField] private bool isHq;
    private int updateValue;
    private float resourceDetectRadius;
    private int maxResourceCollect;
    private List<Collider2D> nodes = new List<Collider2D>();

    private void Start()
    {
        if (isHq)
        {
            var type = ScriptableObject.CreateInstance<BuildingType>();
            type.ResourceTypeEnum = ResourceTypeEnum.Wood;
            type.ResourceUpdateValue = 1;
            Init(type);
            GetComponentInChildren<HarvestOverlap>().Init(null, NodesCount);

        }
    }

    public void Init(BuildingType currentBuildingType)
    {
        this.ResourceTypeEnum = currentBuildingType.ResourceTypeEnum;
        this.updateValue = currentBuildingType.ResourceUpdateValue;
        this.resourceDetectRadius = currentBuildingType.ResourceDetectRadius;
        maxResourceCollect = currentBuildingType.MaxResourceCollect;

        var sourceNodes = Physics2D.OverlapCircleAll(transform.position, resourceDetectRadius);
        foreach (var sourceNode in sourceNodes)
        {
            if (sourceNode.TryGetComponent<ResourceNode>(out var resource))
            {
                if (resource.ResourceTypeEnum == ResourceTypeEnum)
                {
                    nodes.Add(sourceNode);
                }
            }
        }

        TimeManager.GetInstance().OnOneSecTick += UpdateResource;
    }

    private void UpdateResource()
    {
        var value = NodesCount < maxResourceCollect ? updateValue * NodesCount : updateValue * maxResourceCollect;
        ResourceManager.GetInstance().AddResource(ResourceTypeEnum, value);
    }

    private void OnDestroy()
    {
        TimeManager.GetInstance().OnOneSecTick -= UpdateResource;
    }
}
