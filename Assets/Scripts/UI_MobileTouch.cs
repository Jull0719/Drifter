using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MobileTouch : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_STANDALONE
        gameObject.SetActive(false);
#endif
    }
}
