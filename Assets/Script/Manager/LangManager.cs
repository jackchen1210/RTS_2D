using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LangManager
{
    private static LangManager langManager;
    private Dictionary<LangType, LangSetting> langs;
    private LangType currentLangType;

    public static LangManager GetInstance()
    {
        if(langManager == null)
        {
            langManager = new LangManager();
        }
        return langManager;
    }

    private LangManager()
    {
    }


    public void OnLoadEnd(IList<LangSetting> langs)
    {
        this.langs = langs.ToDictionary(_ => _.LangType);
    }

    public void SetLang(LangType currentLangType)
    {
        this.currentLangType = currentLangType;
    }

    public string GetLangString(LangUsageType langUsageType,int index)
    {
        if(langs.TryGetValue(currentLangType,out var langSetting))
        {
           return langSetting.GetLangs(langUsageType,index);
        }
        else
        {
            Debug.LogError("查無該語言");
            return string.Empty;
        }
    }

}
