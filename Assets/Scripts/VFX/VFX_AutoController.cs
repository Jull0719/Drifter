using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [Header("自动销毁")]
    [SerializeField] private bool isAutoDestroy;
    [SerializeField] private float destroyDelay = 1;
    [Space]
    [Header("偏移")]
    [SerializeField] private bool isAutoOffset;
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;
    [SerializeField] private float yMinOffset = -.3f;
    [SerializeField] private float yMaxOffset = .3f;
    [Space]
    [Header("旋转")]
    [SerializeField] private bool isAutoRotate;
    [SerializeField] private float rotation = 200;

    private void Start()
    {
        if (isAutoDestroy)
            AutoDestroy(destroyDelay);

        if (isAutoRotate)
            AutoRotate();

        if (isAutoOffset)
            AutoOffset();
    }

    // 自动销毁
    private void AutoDestroy(float duration)
    {
        Destroy(gameObject, duration);
    }

    // 旋转一定角度
    private void AutoRotate()
    {
        float rotationZ = Random.Range(-rotation, rotation);
        transform.Rotate(0, 0, rotationZ);
    }

    // 偏移一定位置
    private void AutoOffset()
    {
        float offsetX = Random.Range(xMinOffset, xMaxOffset);
        float offsetY = Random.Range(yMinOffset, yMaxOffset);
        transform.position += new Vector3(offsetX, offsetY, 0);
    }
}
