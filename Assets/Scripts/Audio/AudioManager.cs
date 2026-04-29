using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDataBaseSO audioDB;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgSource;

    private Player player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySfx(string clipName, AudioSource sfxSource, float maxHearingRange)
    {
        var audioClipData = audioDB.GetAudioClipData(clipName);
        if (audioClipData == null)
        {
            Debug.Log("没有获取到音频");
            return;
        }

        var clip = audioClipData.GetRandomClip();
        if (clip == null)
            return;

        if (player == null)
            player = Player.Instance;

        float maxVolume = audioClipData.clipVolume;
        float distance = Vector2.Distance(player.transform.position, sfxSource.transform.position);
        float t = Mathf.Clamp01(1 - distance / maxHearingRange);

        sfxSource.volume = Mathf.Lerp(0, maxVolume, t * t);
        sfxSource.pitch = Random.Range(0.95f, 1.15f);
        sfxSource.PlayOneShot(clip);
    }

    public void PlayGlobalSfx(string clipName)
    {
        var audioClipData = audioDB.GetAudioClipData(clipName);
        if (audioClipData == null)
        {
            Debug.Log("没有获取到全局音频");
            return;
        }

        var clip = audioClipData.GetRandomClip();
        if (clip == null)
            return;

        float maxVolume = audioClipData.clipVolume;
        sfxSource.volume = maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
