using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [Header("Settings")]
    [SerializeField] private float speed = 6;
    [SerializeField] private int damage=20;
    private Enemy enemy;
    private Vector3 dir;
    private AsyncOperationHandle<GameObject> handle;

    public static void Create(Vector3 pos,Enemy enemy)
    {
        var handle = Addressables.InstantiateAsync("Arrow");
        handle.Completed += handle =>
        {
            var arrow = handle.Result.GetComponent<Arrow>();
            arrow.transform.position = pos;
            arrow.SetTarget(enemy);
            arrow.handle = handle;
        };
    }

    private Arrow SetTarget(Enemy enemy)
    {
        this.enemy = enemy; 
        if (enemy != null)
        {
            dir = (enemy.transform.position - transform.position + RandomSpread()).normalized;
        }
        return this;
    }

    private Vector3 RandomSpread()
    {
        return new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f));
    }


    void Update()
    {
        rigid.velocity = dir * speed;
        transform.right = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out var enemy))
        {
            if (enemy != null)
            {
                enemy.Damage(damage);
            }
            Destroy();
        }
    }

    private void Destroy()
    {
        if (handle.IsValid())
        {
            Addressables.ReleaseInstance(handle);
        }
    }
}
