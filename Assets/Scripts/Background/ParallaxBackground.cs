using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private ParallaxLayer[] layers;
    private Camera mainCamera;
    private float cameraHalfWidth;
    private float lastCameraPositionX;

    private void Awake()
    {
        mainCamera = Camera.main;
        lastCameraPositionX = mainCamera.transform.position.x;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect; // 摄像机视口宽度的一半

        CalculateBackgroundWidth();
    }

    private void Update()
    {
        // 摄像机前一帧和后一帧之间移动的距离
        float currentCameraPositionX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;

        // 计算摄像机左右边界
        float cameraLeftEdge = mainCamera.transform.position.x - cameraHalfWidth;
        float cameraRightEdge = mainCamera.transform.position.x + cameraHalfWidth;

        foreach (var layer in layers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void CalculateBackgroundWidth()
    {
        foreach (var layer in layers)
            layer.CalculateImageWidth();
    }
}
