using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [SerializeField] private GameObject counterSignPrefab;

    public void EnabledCounterSign(bool enbled) => counterSignPrefab.SetActive(enbled);
}
