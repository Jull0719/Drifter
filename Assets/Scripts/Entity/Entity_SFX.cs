using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    [SerializeField] private float maxHearingRange = 15;
    [SerializeField] private string walk = "Player_Walk";
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;

    [SerializeField] private bool showGizmos;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayWalk()
    {
        AudioManager.instance.PlaySfx(walk, audioSource, maxHearingRange);
    }

    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySfx(attackHit, audioSource, maxHearingRange);
    }

    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySfx(attackMiss, audioSource, maxHearingRange);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, maxHearingRange);
        }
    }
}
