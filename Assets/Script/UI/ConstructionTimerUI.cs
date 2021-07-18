using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour
{
    [SerializeField] private Image progress;
    private BuildingConstruction buildingConstruction;

    public void Init(BuildingConstruction buildingConstruction)
    {
        this.buildingConstruction = buildingConstruction;
        buildingConstruction.OnTimeChanged += OnTimeChanged;
    }

    private void OnTimeChanged(float time)
    {
        var duraction = buildingConstruction.Duraction;
        progress.fillAmount = (duraction -time)/ duraction;
    }
}
