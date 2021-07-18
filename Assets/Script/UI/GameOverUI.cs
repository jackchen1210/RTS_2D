using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Action OnRetryBtnClicked { get; set; }
    public Action OnMainBtnClicked { get; set; }

    [SerializeField] private Button retryBtn;
    [SerializeField] private Button mainBtn;

    private void Start()
    {
        retryBtn.onClick.AddListener(()=> OnRetryBtnClicked?.Invoke());
        mainBtn.onClick.AddListener(()=> OnMainBtnClicked?.Invoke());
    }

}
