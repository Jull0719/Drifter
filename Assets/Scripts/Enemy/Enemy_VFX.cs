using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("反击效果")]
    [SerializeField] protected GameObject stunnedIcon;
    [SerializeField] protected GameObject attackAlert;

    public void EnabledStunnedVfx(bool enbled) => stunnedIcon.SetActive(enbled);
    public void EnabledAttackAlert(bool enbled) => attackAlert.SetActive(enbled);
}
