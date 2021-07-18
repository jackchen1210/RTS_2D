using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemolishUI : MonoBehaviour
{
    [SerializeField] private Button button;
    private Action onClicked;

    private void Start()
    {
        button.onClick.AddListener(()=> onClicked?.Invoke());
    }

    public void Init(Action onClicked)
    {
        this.onClicked = onClicked;
    }
}
