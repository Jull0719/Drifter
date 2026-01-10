using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    public void CreateEffectVFX(GameObject vfxPrefab, Transform target)
    {
        Instantiate(vfxPrefab, target.position, Quaternion.identity);
    }
}
