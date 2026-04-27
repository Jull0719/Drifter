using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle healthBarToggle;

    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBarToggle.onValueChanged.AddListener(ToggleMiniHealthBar);
    }

    // 关闭/开启血条
    public void ToggleMiniHealthBar(bool enabled) => player.health.EnabledMiniHealthBar(enabled);

    // 返回主菜单
    public void ReturnToMainMenuBTN() => GameManager.instance.ChangeScene("MainMenu", WaypointType.None);
}
