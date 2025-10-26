using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform trans;
    [SerializeField] private float multiplier;
    private float imageFullWidth;
    private float imageHalfWidth;

    public void CalculateImageWidth()
    {
        imageFullWidth = trans.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distanceToMove)
    {
        trans.position += Vector3.right * distanceToMove * multiplier;
    }

    // 比较摄像机和图片的左右边界
    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageLeftEdge = trans.position.x - imageHalfWidth;
        float imageRightEdge = trans.position.x + imageHalfWidth;

        if (imageLeftEdge > cameraRightEdge)
            trans.position += Vector3.left * imageFullWidth;
        else if (imageRightEdge < cameraLeftEdge)
            trans.position += Vector3.right * imageFullWidth;
    }
}
