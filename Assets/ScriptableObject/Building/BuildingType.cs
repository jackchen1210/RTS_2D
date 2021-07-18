using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingType : ScriptableObject
{
    public BuildingTypeEnum BuildingTypeEnum;
    public Sprite BuildingSp;
    public ResourceTypeEnum ResourceTypeEnum;
    public int ResourceUpdateValue;
    public float ResourceDetectRadius = 5f;
    public float SameBuildingRange = 10f;
    public GameObject AnimGO;
    public int MaxResourceCollect = 4;
    public BuildingCost[] BuildingCosts;
    public float BuildingTime=5;

    public string GetBuildingCostsString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < BuildingCosts.Length; i++)
        {
            stringBuilder.Append($"\n<color=#{ColorUtility.ToHtmlStringRGBA(BuildingCosts[i].ResourceType.Color)}>{GetResourceTypeName(i)} : {BuildingCosts[i].Amount}</color>");
        }

        return stringBuilder.ToString();
    }

    private string GetResourceTypeName(int i)
    {
        switch (BuildingCosts[i].ResourceType.ResourceTypeEnum)
        {
            case ResourceTypeEnum.Wood:
                return LangManager.GetInstance().GetLangString(LangUsageType.General, 0);
            case ResourceTypeEnum.Stone:
                return LangManager.GetInstance().GetLangString(LangUsageType.General, 1);
            case ResourceTypeEnum.Gold:
                return LangManager.GetInstance().GetLangString(LangUsageType.General, 2);
        }
        return string.Empty;
    }
}
