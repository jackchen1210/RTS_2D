using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppResourceMgrHandler : MonoBehaviour
{
    private void OnDestroy()
    {
        AppResourceMgr.GetInstance().Dispose();
    }
}
