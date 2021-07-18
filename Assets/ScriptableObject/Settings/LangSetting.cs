using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LangSetting")]
public class LangSetting : ScriptableObject
{
    public LangType LangType;

    [SerializeField] string[] generalLangs;
    [SerializeField] string[] toolTipLangs;

    public string GetLangs(LangUsageType langUsageType,int index)
    {
        switch (langUsageType)
        {
            case LangUsageType.General:
                return GetLangs(generalLangs, index);
            case LangUsageType.ToolTip:
                return GetLangs(toolTipLangs, index);
        }
        return string.Empty;
    }

    private string GetLangs(string[] target,int index)
    {
        if (target != null && target.Length > index)
        {
            return target[index];
        }
        return string.Empty;
    }
}
