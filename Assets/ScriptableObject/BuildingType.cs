using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/BuildingType")]
public class BuildingType : ScriptableObject
{
    public BuildingTypeEnum BuildingTypeEnum;
    public Sprite BuildingSp;
    public ResourceTypeEnum ResourceTypeEnum;
    public int ResourceUpdateValue;
    public float ResourceDetectRadius=5f;
    public float SameBuildingRange = 10f;
    public GameObject AnimGO;
}
