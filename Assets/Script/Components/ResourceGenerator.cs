using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public ResourceTypeEnum ResourceTypeEnum { get; private set; }
    private int updateValue;
    private float resourceDetectRadius;
    private List<Collider2D> nodes = new List<Collider2D>();

    private void Start()
    {
        var sourceNodes = Physics2D.OverlapCircleAll(transform.position, resourceDetectRadius);
        foreach (var sourceNode in sourceNodes)
        {
            if(sourceNode.TryGetComponent<ResourceNode>(out var resource))
            {
                if(resource.ResourceTypeEnum == ResourceTypeEnum)
                {
                    nodes.Add(sourceNode);
                }
            }
        }

        TimeManager.GetInstance().OnOneSecTick += UpdateResource;
    }

    public void Init(BuildingType currentBuildingType)
    {
        this.ResourceTypeEnum = currentBuildingType.ResourceTypeEnum;
        this.updateValue = currentBuildingType.ResourceUpdateValue;
        this.resourceDetectRadius = currentBuildingType.ResourceDetectRadius;
    }

    private void UpdateResource()
    {
        ResourceManager.GetInstance().UpdateResource(ResourceTypeEnum, updateValue*nodes.Count);
    }

    private void OnDestroy()
    {
        TimeManager.GetInstance().OnOneSecTick -= UpdateResource;
    }
}
