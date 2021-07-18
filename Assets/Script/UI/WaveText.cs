using System;
using TMPro;
using UniRx;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;
    [SerializeField] private TextMeshProUGUI nextWaveTimeText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Transform waveIndicator;

    private void Start()
    {
        enemyWaveManager.WaveCount.Subscribe(SetWaveCount).AddTo(gameObject);
        enemyWaveManager.LeftTimer.Subscribe(SetWaveTime).AddTo(gameObject);
    }

    private void Update()
    {
        var enemyPos = enemyWaveManager.WaveSpawnPos;
        var dir = (enemyPos - Utils.GetCameraPos()).normalized;
        waveIndicator.localPosition = dir * 300;
        waveIndicator.eulerAngles = new Vector3(0,0,Utils.GetAngleFromVector(dir));
        waveIndicator.gameObject.SetActive(Vector2.Distance(enemyPos, Utils.GetCameraPos())> Camera.main.orthographicSize *1.5f);
    }


    private void SetWaveCount(int count)
    {
        waveText.text = $"Wave {count}";
    }

    private void SetWaveTime(float leftTime)
    {
        nextWaveTimeText.text = $"Next Wave in {leftTime.ToString("F1")}s";
    }
}
