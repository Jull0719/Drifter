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
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, Settings.DefaultVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, Settings.DefaultVolume);
    }

    // 关闭/开启血条
    public void ToggleMiniHealthBar(bool enabled)
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        player?.health.EnabledMiniHealthBar(enabled);
    }

    // 保存游戏按钮
    public void SaveGameBTN()
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        SaveManager.instance.SaveGame();
    }

    public void ReturnInGameBTN()
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        UI.instance.SwitchToInGameUI();
    }

    // 删除存档
    public void DeleteSaveDataBTN()
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        SaveManager.instance.DeleteSaveData();
    }

    // 返回主菜单
    public void ReturnToMainMenuBTN()
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        GameManager.instance.ChangeScene("MainMenu", WaypointType.None);
    }
}
