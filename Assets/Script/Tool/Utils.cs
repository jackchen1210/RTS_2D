using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    private static Camera mainCam;

    public static Vector3 GetMouseWorldPos()
    {
        if (mainCam == null) mainCam = Camera.main;

        return mainCam.ScreenToWorldPoint(Input.mousePosition).With(z: 0);
    }

    public static int GetNearByCount<T>(float range, Predicate<T> predicate = null)
    {
        var objs = Physics2D.OverlapCircleAll(GetMouseWorldPos(), range);
        var coms = objs.Select(_=>_.TryGetComponent<T>(out var temp)).ToArray();
        return objs
        .Where(_ => _.gameObject.TryGetComponent<T>(out var temp) && (predicate?.Invoke(temp) ?? true))
        .Count();
    }
    public static IEnumerable<T> GetNearByObject<T>(Vector3 point, float range, Predicate<T> predicate = null) where T : MonoBehaviour
    {
        return Physics2D.OverlapCircleAll(point, range)
        .Where(_ => _.gameObject.TryGetComponent<T>(out var temp) && (predicate?.Invoke(temp) ?? true))
        .Select(_=>_.transform.GetComponent<T>());
    }

    public static float GetAngleFromVector(Vector3 vector3)
    {
        float radians = Mathf.Atan2(vector3.y,vector3.x);
        float degree = Mathf.Rad2Deg*radians;
        return degree;
    }

    internal static Vector3 GetCenterPosBySprite(Sprite sprite)
    {
        var normalize = 1 / sprite.pixelsPerUnit;
        return (sprite.rect.center - sprite.pivot) * normalize;
    }

    internal static Vector3 GetCameraPos()
    {
        if (mainCam == null) mainCam = Camera.main;
        return mainCam.transform.position;
    }

    internal static Vector3 GetRNGDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
}