using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Tower : MonoBehaviour
{
    [SerializeField] private bool stopShooting;
    private TowerSetting towerSetting;
    private Enemy targetEnemy;

    private void Start()
    {
        Addressables.LoadAssetAsync<TowerSetting>("TowerSetting").Completed += handle =>
        {
            var setting = handle.Result;
            towerSetting = setting;
            InvokeRepeating(nameof(SetTarget), 0, setting.FindTargetTime);
            if (!stopShooting)
            {
                InvokeRepeating(nameof(ShootEnemy), 0, setting.ShootEnemyTime);
            }
            CreateDetectRange();
        };
    }

    private void CreateDetectRange()
    {
        var go = new GameObject();
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one * towerSetting.DetectRange*2;
        var detectRange = go.AddComponent<SpriteRenderer>();
        detectRange.sprite = towerSetting.DetectCircle;
        detectRange.color = towerSetting.DetectColor;
        detectRange.sortingLayerName = "towerDetectCircle";
        go.AddComponent<SortingLayerHelper>();
    }

    private void ShootEnemy()
    {
        if (targetEnemy != null)
        {
            var pos = transform.position + new Vector3(0,0.5f,0);
            Arrow.Create(pos, targetEnemy);
        }
    }

    private void SetTarget()
    {
        var enemys = Utils.GetNearByObject<Enemy>(transform.position, towerSetting.DetectRange);
        if(enemys.Count() == 0)
        {
            targetEnemy = null;
            return;
        }

        foreach (var enemy in enemys)
        {

            if (targetEnemy == null)
            {
                targetEnemy = enemy;
            }
            else
            {
                if (Vector2.Distance(targetEnemy.transform.position, transform.position) >
                    Vector2.Distance(enemy.transform.position, transform.position))
                {
                    targetEnemy = enemy;
                }
            }
        }
    }


    private void OnDestroy()
    {
        CancelInvoke(nameof(SetTarget));
        CancelInvoke(nameof(ShootEnemy));
        if (towerSetting != null)
        {
            Addressables.Release(towerSetting);
        }
    }
}
