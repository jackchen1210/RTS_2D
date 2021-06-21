using System;
using UniRx;
using UnityEngine;

public class TimeManager : IDisposable
{
    public event Action OnOneSecTick;

    private static TimeManager instance;
    private readonly IDisposable disposable;

    public static TimeManager GetInstance()
    {
        if (instance == null)
        {
            instance = new TimeManager();
        }
        return instance;
    }

    private TimeManager()
    {
        disposable = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(OnTick);
    }

    private void OnTick(long ticks)
    {
        OnOneSecTick?.Invoke();
    }

    public void Dispose()
    {
        OnOneSecTick = null;
        disposable?.Dispose();
        instance = null;
    }

}
