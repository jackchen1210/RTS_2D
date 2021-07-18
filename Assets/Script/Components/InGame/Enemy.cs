using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

[RequireComponent(typeof(HealthSystem))]
public class Enemy : MonoBehaviour
{
    public async static UniTask Create(Vector3 position)
    {
        var go = await Addressables.InstantiateAsync("Enemy");
        go.transform.position = position;
    }

    [SerializeField] private Rigidbody2D rigid;
    [Header("Setting")]
    [SerializeField] private int health=30;
    [SerializeField] private float speed = 6;

    private Building targetBuilding;
    private HealthSystem healthSystem;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.Init(health);
        healthSystem.IsDead.Subscribe(_=> { if (_) Destroy(); }).AddTo(gameObject);
        targetBuilding = BuildingManager.Instance.HqBuilding;
        InvokeRepeating(nameof(SetTarget),0,2);
    }

    private void FixedUpdate()
    {
        if (targetBuilding != null)
        {
            rigid.velocity = (targetBuilding.transform.position - transform.position).normalized * speed;
        }
        if (IsOutOfBorder())
        {
            Destroy();
        }
    }

    private bool IsOutOfBorder()
    {
        return Mathf.Abs(transform.position.x) > 150 || Mathf.Abs(transform.position.y) > 150;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<Building>(out var building))
        {
            building.Damage();
            Destroy();
        }
    }

    private void Destroy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void SetTarget()
    {
        foreach (var building in Utils.GetNearByObject<Building>(transform.position,10))
        {
            if (targetBuilding == null)
            {
                targetBuilding = BuildingManager.Instance.HqBuilding;
            }
            else
            {
                if (Vector2.Distance(targetBuilding.transform.position, transform.position) >
                    Vector2.Distance(building.transform.position, transform.position))
                {
                    targetBuilding = building;
                }
            }
        }
    }

    internal void Damage(int value)
    {
        healthSystem.Damage(value);
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(SetTarget));
        Addressables.ReleaseInstance(gameObject);
    }
}
