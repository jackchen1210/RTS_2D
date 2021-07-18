using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float seconds=5;

    private async void Start()
    {
        var token = gameObject.GetCancellationTokenOnDestroy();
        await UniTask.Delay((int)(seconds*1000));
        if (!token.IsCancellationRequested)
        {
            Destroy(gameObject);
        }
    }
}
