using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySfx(attackHit, audioSource);
    }

    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySfx(attackMiss, audioSource);
    }
}
