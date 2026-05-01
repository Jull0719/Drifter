using UnityEngine;

public class AudioRangeController : MonoBehaviour
{
    [SerializeField] private float maxHearingRange = 12;
    [SerializeField] private bool showGizmos;

    private AudioSource audioSource;
    private Player player;
    private float maxVolume;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        maxVolume = audioSource.volume;
        audioSource.volume = 0;

        player = Player.Instance;
    }

    private void Update()
    {
        if (player == null)
            return;

        UpdateVolume();
    }

    public void UpdateVolume()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        float t = Mathf.Clamp01(1 - distance / maxHearingRange);
        float targetVolume = Mathf.Lerp(0, maxVolume, t * t);
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * 3);
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
