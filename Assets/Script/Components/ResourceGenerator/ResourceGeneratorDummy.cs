using UnityEngine;

namespace Assets.Script.Components.ResourceGenerator
{
    class ResourceGeneratorDummy : MonoBehaviour, IResourceGenerator
    {
        public bool IsHQ => false;
        public ResourceTypeEnum ResourceTypeEnum { get; private set; }

        public int NodesCount => 0;

        public void Init(BuildingType currentBuildingType)
        {
            ResourceTypeEnum = currentBuildingType.ResourceTypeEnum;
        }
    }
}
