public interface IResourceGenerator
{
    int NodesCount { get; }
    bool IsHQ { get; }
    ResourceTypeEnum ResourceTypeEnum { get; }

    void Init(BuildingType currentBuildingType);
}