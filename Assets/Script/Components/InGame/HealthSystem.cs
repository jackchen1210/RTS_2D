using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }
    public ReadOnlyReactiveProperty<int> Health { get; private set; }
    private BoolReactiveProperty isDead { get; } = new BoolReactiveProperty();
    private IntReactiveProperty health { get; } = new IntReactiveProperty();
    private void Awake()
    {
        Health = new ReadOnlyReactiveProperty<int>(health);
        IsDead = new ReadOnlyReactiveProperty<bool>(isDead);
        health.SkipLatestValueOnSubscribe().Select(_ => _ <= 0).Subscribe(_ => isDead.Value = _).AddTo(gameObject);
    }

    internal void Init(int initHealth)
    {
        health.Value = initHealth;
        healthBar.Init(initHealth);
        health.SkipLatestValueOnSubscribe().Subscribe(OnHealthChanged).AddTo(gameObject);
    }

    public void Damage(int value)
    {
        if(health.Value- value < 0)
        {
            health.Value = 0;
        }
        else
        {
            health.Value -= value;
        }
    }

    private void OnHealthChanged(int value)
    {
        healthBar.UpdateHealth(value);
    }
}
