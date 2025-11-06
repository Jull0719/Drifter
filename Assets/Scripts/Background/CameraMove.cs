using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.04f;

    private void FixedUpdate()
    {
        transform.position += Vector3.right * moveSpeed;
    }
}
