using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    public ResourceTypeEnum ResourceTypeEnum { get; private set; }
    [SerializeField] private Image image;
    [SerializeField] private Text amountText;


    internal void Init(ResourceType item)
    {
        image.sprite = item.ResourceSp;
        ResourceTypeEnum = item.ResourceTypeEnum;
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
