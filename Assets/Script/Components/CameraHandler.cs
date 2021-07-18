using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera myCamera;
    [SerializeField] private GameObject target;
    [SerializeField] private float targetMovingSpeed=5;

    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start()
    {
        orthographicSize = myCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = myCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        target.transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * targetMovingSpeed * Time.deltaTime;
        SetCamera();
    }

    private void SetCamera()
    {
        targetOrthographicSize -= Input.mouseScrollDelta.y;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, 10,30);
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, 5*Time.deltaTime);
        myCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
