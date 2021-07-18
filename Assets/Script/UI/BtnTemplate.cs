using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnTemplate : MonoBehaviour
{
    public BuildingTypeEnum BuildingTypeEnum { get;private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Image select;
    [SerializeField] private EventTrigger eventTrigger;
    private BuildingType buildingType;

    public void Init(BuildingType buildingType,Action<BuildingTypeEnum> onBtnClicked)
    {
        this.buildingType = buildingType;
        CustomSetting();
        image.sprite = buildingType.BuildingSp;
        BuildingTypeEnum = buildingType.BuildingTypeEnum;
        button.onClick.AddListener(()=> onBtnClicked?.Invoke(buildingType.BuildingTypeEnum));
        SelectActive(false);
        var enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        var exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        enterEntry.callback.AddListener(OnPointerEnter);
        exitEntry.callback.AddListener(OnPointerExit);
        eventTrigger.triggers.Add(enterEntry);
        eventTrigger.triggers.Add(exitEntry);
    }

    private void OnPointerExit(BaseEventData arg0)
    {
        ToolTip.Instance.Hide();
    }

    private void OnPointerEnter(BaseEventData arg0)
    {
        ToolTip.Instance.Show($"{BuildingTypeEnum.ToString()}{GetToolTipRequest()}");
    }

    private string GetToolTipRequest()
    {
        return BuildingTypeEnum== BuildingTypeEnum.Cursor?"": $"\nRequest : {buildingType.GetBuildingCostsString()}";
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
