
using UnityEngine;

public static class Utils
{
    private static Camera mainCam;

    public static Vector3 GetMouseWorldPos()
    {
        if (mainCam == null) mainCam = Camera.main;

        return mainCam.ScreenToWorldPoint(Input.mousePosition).With(z: 0);
    }
}