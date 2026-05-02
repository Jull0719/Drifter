using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private string bgmParameter;
    [SerializeField] private Slider bgmSlider;

    [Header("SFX")]
    [SerializeField] private string sfxParameter;
    [SerializeField] private Slider sfxSlider;
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [SerializeField] private Toggle healthBarToggle;

    private Player player;

    private void OnEnable()
    {
        LoadVolume();
        healthBarToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(Settings.ShowMiniHealthBarParameter) == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);    
        PlayerPrefs.SetInt(Settings.ShowMiniHealthBarParameter, healthBarToggle.isOn ? 1 : 0);
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBarToggle.onValueChanged.AddListener(ToggleMiniHealthBar);
    }

    // 调节背景音乐音量
    public void SetBGMVolume(float value)
    {
        float newVal = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newVal);
    }

    // 调节音效音量
    public void SetSFXVolume(float value)
    {
        float newVal = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newVal);
    }

    // 加载音量
    public void LoadVolume()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, Settings.defaultVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, Settings.defaultVolume);
    }

    // 关闭/开启血条
    public void ToggleMiniHealthBar(bool enabled) => player?.health.EnabledMiniHealthBar(enabled);

    // 返回主菜单
    public void ReturnToMainMenuBTN() => GameManager.instance.ChangeScene("MainMenu", WaypointType.None);
}
