using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDataBaseSO audioDB;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private bool bgmShouldPlay;
    
    private string currentMusicName;
    private AudioClip lastMusicName;
    private Coroutine currentCo;
    
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

    private void Update()
    {
        // 自动循环播放音乐
        if (!bgmSource.isPlaying && bgmShouldPlay)
        {
            if (!String.IsNullOrEmpty(currentMusicName))
                SwitchToNextMusic(currentMusicName);
        }

        // 自动停止音乐
        //if (bgmSource.isPlaying && !bgmShouldPlay)
        //    StopBGM();
    }

    // 停止播放背景音乐
    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(ChangeVolumeCo(bgmSource, 0, 1));

        if (currentCo != null)
            StopCoroutine(currentCo);
    }

    // 播放背景音乐
    public void StartBGM(string bgmName)
    {
        bgmShouldPlay = true;

        if (currentMusicName == bgmName)
            return;

        SwitchToNextMusic(bgmName);
    }

    // 切换音乐
    public void SwitchToNextMusic(string nextMusic)
    {
        bgmShouldPlay = true;
        currentMusicName = nextMusic;

        if (currentCo != null)
            StopCoroutine(currentCo);

        currentCo = StartCoroutine(SwithMusicCo(nextMusic));
    }

    // 结束当前音乐，切换到下一首
    private IEnumerator SwithMusicCo(string musicName)
    {
        var data = audioDB.GetAudioClipData(musicName);
        if (data == null)
        {
            Debug.Log("没有名为" + musicName + "的音频");
            yield break;
        }

        var clip = data.GetRandomClip();
        // 如果和上一首音乐重复，则换一首
        if (data.clipList.Count > 1)
        {
            while (clip == lastMusicName)
            {
                clip = data.GetRandomClip();
            }
        }

        // 音量渐弱
        if (bgmSource.isPlaying)
            yield return ChangeVolumeCo(bgmSource, 0, 1f);

        lastMusicName = clip;
        bgmSource.clip = clip;
        bgmSource.volume = 0;
        bgmSource.Play();

        // 音量渐强
        StartCoroutine(ChangeVolumeCo(bgmSource, data.clipVolume, 1f));
    }

    // 音量渐弱渐强效果
    private IEnumerator ChangeVolumeCo(AudioSource audioSource, float targetVolume, float duration)
    {
        float time = 0;
        float startVolume = audioSource.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            //Debug.Log(audioSource.volume);
            yield return null;
        }
        audioSource.volume = targetVolume;
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
        sfxSource.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
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
