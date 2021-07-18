using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public IntReactiveProperty WaveCount { get; private set; } = new IntReactiveProperty();
    public FloatReactiveProperty LeftTimer { get; private set; } = new FloatReactiveProperty();
    public Vector3 WaveSpawnPos { get; private set; }

    [SerializeField] private Transform nextWaveEnemySpawnPos;
    [Header("Settings")]
    [SerializeField] private float enemySpawnRange = 100;
    [SerializeField] private float waveTimer = 5;
    [SerializeField] private int initEnemyPerWave=5;

    private void Start()
    {
        LeftTimer.Value = waveTimer;
        NextEnemyPos();
    }

    private void NextEnemyPos()
    {
        WaveSpawnPos = Utils.GetRNGDir() * enemySpawnRange;
        nextWaveEnemySpawnPos.transform.position = WaveSpawnPos;
    }

    private void Update()
    {
        if(BuildingManager.Instance.HqBuilding == null)
        {
            return;
        }

        if (LeftTimer.Value <= 0)
        {
            _ = SpawnEnemy();
            LeftTimer.Value = waveTimer;
        }
        LeftTimer.Value -= Time.deltaTime;
    }

    private async UniTask SpawnEnemy()
    {
        for (int i = 0; i < initEnemyPerWave+ WaveCount.Value*3; i++)
        {
           await  Enemy.Create(WaveSpawnPos+ Utils.GetRNGDir()*Random.Range(0,10));
        }

        NextEnemyPos();
        WaveCount.Value++;
    }

    private void OnDestroy()
    {
        WaveCount?.Dispose();
        LeftTimer?.Dispose();
    }

}
