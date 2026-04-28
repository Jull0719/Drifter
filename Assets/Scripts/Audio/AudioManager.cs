using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDataBaseSO audioDB;
    private AudioSource sfxSource;
    private AudioSource bgSource;

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

    public void PlaySfx(string clipName, AudioSource sfxSource)
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

        sfxSource.clip = clip;
        sfxSource.volume = audioClipData.clipVolume;
        sfxSource.pitch = Random.Range(0.95f, 1.15f);
        sfxSource.PlayOneShot(clip);
    }
}
