using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTemplate : MonoBehaviour
{
    public BuildingTypeEnum BuildingTypeEnum { get;private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Image select;
    private BuildingType buildingType;

    public void Init(BuildingType buildingType,Action<BuildingTypeEnum> onBtnClicked)
    {
        this.buildingType = buildingType;
        CustomSetting();
        image.sprite = buildingType.BuildingSp;
        BuildingTypeEnum = buildingType.BuildingTypeEnum;
        button.onClick.AddListener(()=> onBtnClicked?.Invoke(buildingType.BuildingTypeEnum));
        SelectActive(false);
    }

    private void CustomSetting()
    {
        if(buildingType.BuildingTypeEnum == BuildingTypeEnum.Cursor)
        {
            image.GetComponent<RectTransform>().localScale = new Vector2(0.5f,0.5f);
        }
    }

    public void SelectActive(bool isSelect)
    {
        select.enabled = isSelect;
    }
}
